using System.IO;

/// <summary>
/// Gif使用的可变长度的LZW压缩算法解码器
/// </summary>
public class LZWDecoder
{
    #region 变量
    /// <summary>
    /// GIF规定编码最大为12bit，最大值即为4096
    /// </summary>
    protected static readonly int MaxStackSize = 4096;
    protected Stream stream;
    #endregion

    #region 构造函数
    /// <summary>
    /// 构造函数
    /// </summary>
    public LZWDecoder(Stream stream)
    {
        this.stream = stream;
    }
    #endregion

    #region 私有方法
    /// <summary>
    /// 读取数据段
    /// </summary>
    byte[] ReadData()
    {
        int blockSize = Read();
        return ReadByte(blockSize);
    }

    /// <summary>
    /// 读取指定长度的字节字节
    /// </summary>
    byte[] ReadByte(int len)
    {
        byte[] buffer = new byte[len];
        stream.Read(buffer, 0, len);
        return buffer;
    }

    /// <summary>
    /// 读取一个字节
    /// </summary>
    int Read()
    {
        return stream.ReadByte();
    }
    #endregion

    #region 调用方法
    /// <summary>
    /// LZW压缩算法解码器
    /// </summary>
    /// <param name="width">长度</param>
    /// <param name="height">高度</param>
    /// <param name="stream">包含编码流的数据流</param>
    /// <returns>原始数据流</returns>
    public byte[] DecodeImageData(int width, int height, Stream stream)
    {
        int NullCode = -1;
        int pixelCount = width * height;//获取原图像的像素数，公式为 像素数 = 图像长度*图像高度
        byte[] pixels = new byte[pixelCount];
        int dataSize = Read();          //图像编码流的第一个字节(byte)存放的是数据位大小，在gif通常为1,4,8
        int codeSize = dataSize + 1;    //编码位大小，根据lzw算法的要求，编码位的大小 = 数据位大小+1 ，针对gif，有如下对应关系 1->3 4->5 ->9,而最大的codeSize为12
        int clearFlag = 1 << dataSize;  //在lzw算法有两个特殊标记，clearFlag为其中的清除标记，此后的编码将重头再来，这样做可以防止编码位无限增大
        int endFlag = clearFlag + 1;    //lzw算法两个特殊标记之一，endFlag为结束标记，表示一次编码的结束  endFlag=clearFlag+1
        int available = endFlag + 1;    //初始的可用编码大小，因为0-(clear-1)为元数据，所以均可用，不必研究，此处从能形成压缩的编码开始算起

        int code = NullCode;     //用于存储当前的编码值
        int old_code = NullCode; //用于存储上一次的编码值
        int code_mask = (1 << codeSize) - 1;//表示编码的最大值，如果codeSize=5,则code_mask=31
        int bits = 0;//在编码流中数据的保存形式为byte，而实际编码过程中是找实际编码位来存储的，比如当codeSize=5的时候，那么实际上5bit的数据就应该可以表示一个编码，这样取出来的1个字节就富余了3个bit，这3个bit用于和第二个字节的后两个bit进行组合，再次形成编码值，如此类推

        int[] prefix = new int[MaxStackSize];          //用于保存前缀的集合
        int[] suffix = new int[MaxStackSize];          //用于保存后缀
        int[] pixelStatck = new int[MaxStackSize + 1]; //用于临时保存数据流

        int top = 0;
        int count = 0; //在下面的循环中，每次会获取一定量的编码的字节数组，而处理这些数组的时候需要1个个字节来处理，count就是表示还要处理的字节数目
        int bi = 0;    //count表示还剩多少字节需要处理，而bi则表示本次已经处理的个数
        int i = 0;     //i代表当前处理得到像素数

        int data = 0;          //表示当前处理的数据的值
        int first = 0;         //一个字符串重的第一个字节
        int inCode = NullCode; //在lzw中，如果认识了一个编码所代表的数据entry，则将编码作为下一次的prefix，此处inCode代表传递给下一次作为前缀的编码值

        //先生成元数据的前缀集合和后缀集合，元数据的前缀均为0，而后缀与元数据相等，同时编码也与元数据相等
        for (code = 0; code < clearFlag; code++)
        {
            //前缀初始为0
            prefix[code] = 0;
            //后缀=元数据=编码
            suffix[code] = (byte)code;
        }

        byte[] buffer = null;
        while (i < pixelCount)
        {
            //最大像素数已经确定为pixelCount = width * width
            if (top == 0)
            {
                if (bits < codeSize)
                {
                    //如果当前的要处理的bit数小于编码位大小，则需要加载数据
                    if (count == 0)
                    {
                        //如果count为0，表示要从编码流中读一个数据段来进行分析
                        buffer = ReadData();
                        count = buffer.Length;
                        if (count == 0)
                        {
                            //再次想读取数据段，却没有读到数据，此时就表明已经处理完了
                            break;
                        }
                        //重新读取一个数据段后，应该将已经处理的个数置0
                        bi = 0;
                    }
                    //获取本次要处理的数据的值
                    data += buffer[bi] << bits;//此处为何要移位呢，比如第一次处理了1个字节为176，第一次只要处理5bit就够了，剩下3bit留给下个字节进行组合。也就是第二个字节的后两位+第一个字节的前三位组成第二次输出值
                    bits += 8; //本次又处理了一个字节，所以需要+8
                    bi++;      //将处理下一个字节
                    count--;   //已经处理过的字节数+1
                    continue;
                }

                //如果已经有足够的bit数可供处理，下面就是处理过程
                code = data & code_mask; //获取data数据的编码位大小bit的数据
                data >>= codeSize;       //将编码数据截取后，原来的数据就剩下几个bit了，此时将这些bit右移，为下次作准备
                bits -= codeSize;        //同时需要将当前数据的bit数减去编码位长，因为已经得到了处理。

                //下面根据获取的code值来进行处理
                if (code > available || code == endFlag)
                {
                    //当编码值大于最大编码值或者为结束标记的时候，停止处理
                    break;
                }
                if (code == clearFlag)
                {
                    //如果当前是清除标记，则重新初始化变量，好重新再来
                    codeSize = dataSize + 1;
                    //重新初始化最大编码值
                    code_mask = (1 << codeSize) - 1;
                    //初始化下一步应该处理得编码值
                    available = clearFlag + 2;
                    //将保存到old_code中的值清除，以便重头再来
                    old_code = NullCode;
                    continue;
                }

                //下面是code属于能压缩的编码范围内的的处理过程
                if (old_code == NullCode)
                {
                    //如果当前编码值为空,表示是第一次获取编码
                    pixelStatck[top++] = suffix[code];//获取到1个数据流的数据
                    //本次编码处理完成，将编码值保存到old_code中
                    old_code = code;
                    //第一个字符为当前编码
                    first = code;
                    continue;
                }
                inCode = code;
                if (code == available)
                {
                    //如果当前编码和本次应该生成的编码相同
                    pixelStatck[top++] = (byte)first;//那么下一个数据字节就等于当前处理字符串的第一个字节
                    code = old_code; //回溯到上一个编码
                }
                while (code > clearFlag)
                {
                    //如果当前编码大于清除标记，表示编码值是能压缩数据的
                    pixelStatck[top++] = suffix[code];
                    code = prefix[code];//回溯到上一个编码
                }
                first = suffix[code];
                if (available > MaxStackSize)
                {
                    //当编码最大值大于gif所允许的编码（4096）最大值的时候停止处理
                    break;
                }

                //获取下一个数据
                pixelStatck[top++] = suffix[code];
                //设置当前应该编码位置的前缀
                prefix[available] = old_code;
                //设置当前应该编码位置的后缀
                suffix[available] = first;
                //下次应该得到的编码值
                available++;
                if (available == code_mask + 1 && available < MaxStackSize)
                {
                    //增加编码位数
                    codeSize++;
                    //重设最大编码值
                    code_mask = (1 << codeSize) - 1;
                }
                //还原old_code
                old_code = inCode;
            }
            //回溯到上一个处理位置
            top--;
            //获取元数据
            pixels[i++] = (byte)pixelStatck[top];
        }
        return pixels;
    }
    #endregion
}

//LZW数据压缩算法的原理分析：http://www.cnblogs.com/jillzhang/archive/2006/11/06/551298.html