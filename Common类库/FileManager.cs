using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

[Serializable]
public class FileItem
{
    public FileItem()
    { }

    #region 私有字段
    private string _Name;
    private string _FullName;
    private DateTime _CreationDate;
    private bool _IsFolder;
    private long _Size;
    private DateTime _LastAccessDate;
    private DateTime _LastWriteDate;
    private int _FileCount;
    private int _SubFolderCount;
    #endregion

    #region 公有属性
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    /// <summary>
    /// 文件或目录的完整目录
    /// </summary>
    public string FullName
    {
        get { return _FullName; }
        set { _FullName = value; }
    }

    /// <summary>
    ///  创建时间
    /// </summary>
    public DateTime CreationDate
    {
        get { return _CreationDate; }
        set { _CreationDate = value; }
    }

    public bool IsFolder
    {
        get { return _IsFolder; }
        set { _IsFolder = value; }
    }

    /// <summary>
    /// 文件大小
    /// </summary>
    public long Size
    {
        get { return _Size; }
        set { _Size = value; }
    }

    /// <summary>
    /// 上次访问时间
    /// </summary>
    public DateTime LastAccessDate
    {
        get { return _LastAccessDate; }
        set { _LastAccessDate = value; }
    }

    /// <summary>
    /// 上次读写时间
    /// </summary>
    public DateTime LastWriteDate
    {
        get { return _LastWriteDate; }
        set { _LastWriteDate = value; }
    }

    /// <summary>
    /// 文件个数
    /// </summary>
    public int FileCount
    {
        get { return _FileCount; }
        set { _FileCount = value; }
    }

    /// <summary>
    /// 目录个数
    /// </summary>
    public int SubFolderCount
    {
        get { return _SubFolderCount; }
        set { _SubFolderCount = value; }
    }
    #endregion
}

public class FileManager
{
    #region 构造函数
    private static string strRootFolder;
    static FileManager()
    {
        strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath + "File\\";
        strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
    }
    #endregion

    #region 目录
    /// <summary>
    /// 读根目录
    /// </summary>
    public static string GetRootPath()
    {
        return strRootFolder;
    }

    /// <summary>
    /// 写根目录
    /// </summary>
    public static void SetRootPath(string path)
    {
        strRootFolder = path;
    }

    /// <summary>
    /// 读取目录列表
    /// </summary>
    public static List<FileItem> GetDirectoryItems()
    {
        return GetDirectoryItems(strRootFolder);
    }

    /// <summary>
    /// 读取目录列表
    /// </summary>
    public static List<FileItem> GetDirectoryItems(string path)
    {
        List<FileItem> list = new List<FileItem>();
        string[] folders = Directory.GetDirectories(path);
        foreach (string s in folders)
        {
            FileItem item = new FileItem();
            DirectoryInfo di = new DirectoryInfo(s);
            item.Name = di.Name;
            item.FullName = di.FullName;
            item.CreationDate = di.CreationTime;
            item.IsFolder = false;
            list.Add(item);
        }
        return list;
    }
    #endregion

    #region 文件
    /// <summary>
    /// 读取文件列表
    /// </summary>
    public static List<FileItem> GetFileItems()
    {
        return GetFileItems(strRootFolder);
    }

    /// <summary>
    /// 读取文件列表
    /// </summary>
    public static List<FileItem> GetFileItems(string path)
    {
        List<FileItem> list = new List<FileItem>();
        string[] files = Directory.GetFiles(path);
        foreach (string s in files)
        {
            FileItem item = new FileItem();
            FileInfo fi = new FileInfo(s);
            item.Name = fi.Name;
            item.FullName = fi.FullName;
            item.CreationDate = fi.CreationTime;
            item.IsFolder = true;
            item.Size = fi.Length;
            list.Add(item);
        }
        return list;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    public static bool CreateFile(string filename, string path)
    {
        try
        {
            FileStream fs = File.Create(path + "\\" + filename);
            fs.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    public static bool CreateFile(string filename, string path, byte[] contents)
    {
        try
        {
            FileStream fs = File.Create(path + "\\" + filename);
            fs.Write(contents, 0, contents.Length);
            fs.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    public static string OpenText(string parentName)
    {
        StreamReader sr = File.OpenText(parentName);
        StringBuilder output = new StringBuilder();
        string rl;
        while ((rl = sr.ReadLine()) != null)
        {
            output.Append(rl);
        }
        sr.Close();
        return output.ToString();
    }

    /// <summary>
    /// 读取文件信息
    /// </summary>
    public static FileItem GetItemInfo(string path)
    {
        FileItem item = new FileItem();
        if (Directory.Exists(path))
        {
            DirectoryInfo di = new DirectoryInfo(path);
            item.Name = di.Name;
            item.FullName = di.FullName;
            item.CreationDate = di.CreationTime;
            item.IsFolder = true;
            item.LastAccessDate = di.LastAccessTime;
            item.LastWriteDate = di.LastWriteTime;
            item.FileCount = di.GetFiles().Length;
            item.SubFolderCount = di.GetDirectories().Length;
        }
        else
        {
            FileInfo fi = new FileInfo(path);
            item.Name = fi.Name;
            item.FullName = fi.FullName;
            item.CreationDate = fi.CreationTime;
            item.LastAccessDate = fi.LastAccessTime;
            item.LastWriteDate = fi.LastWriteTime;
            item.IsFolder = false;
            item.Size = fi.Length;
        }
        return item;
    }

    /// <summary>
    /// 写入一个新文件，在文件中写入内容，然后关闭文件。如果目标文件已存在，则改写该文件。 
    /// </summary>
    public static bool WriteAllText(string parentName, string contents)
    {
        try
        {
            File.WriteAllText(parentName, contents, Encoding.Unicode);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    public static bool DeleteFile(string path)
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 移动文件
    /// </summary>
    public static bool MoveFile(string oldPath, string newPath)
    {
        try
        {
            File.Move(oldPath, newPath);
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region 文件夹
    /// <summary>
    /// 创建文件夹
    /// </summary>
    public static void CreateFolder(string name, string parentName)
    {
        DirectoryInfo di = new DirectoryInfo(parentName);
        di.CreateSubdirectory(name);
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    public static bool DeleteFolder(string path)
    {
        try
        {
            Directory.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 移动文件夹
    /// </summary>
    public static bool MoveFolder(string oldPath, string newPath)
    {
        try
        {
            Directory.Move(oldPath, newPath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    public static bool CopyFolder(string source, string destination)
    {
        try
        {
            String[] files;
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar) destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            files = Directory.GetFileSystemEntries(source);
            foreach (string element in files)
            {
                if (Directory.Exists(element))
                    CopyFolder(element, destination + Path.GetFileName(element));
                else
                    File.Copy(element, destination + Path.GetFileName(element), true);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region 检测文件
    /// <summary>
    /// 判断是否为安全文件名
    /// </summary>
    /// <param name="str">文件名</param>
    public static bool IsSafeName(string strExtension)
    {
        strExtension = strExtension.ToLower();
        if (strExtension.LastIndexOf(".") >= 0)
        { 
            strExtension = strExtension.Substring(strExtension.LastIndexOf(".")); 
        }
        else 
        { 
            strExtension = ".txt"; 
        }
        string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png", ".rar", ".zip" };
        for (int i = 0; i < arrExtension.Length; i++)
        {
            if (strExtension.Equals(arrExtension[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  判断是否为不安全文件名
    /// </summary>
    /// <param name="str">文件名、文件夹名</param>
    public static bool IsUnsafeName(string strExtension)
    {
        strExtension = strExtension.ToLower();
        if (strExtension.LastIndexOf(".") >= 0)  
        { 
            strExtension = strExtension.Substring(strExtension.LastIndexOf(".")); 
        }
        else 
        { 
            strExtension = ".txt";
        } 
        string[] arrExtension = { ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx", ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", ".mdf", ".resx", ".xsd" };
        for (int i = 0; i < arrExtension.Length; i++)
        {
            if (strExtension.Equals(arrExtension[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///  判断是否为可编辑文件
    /// </summary>
    /// <param name="str">文件名、文件夹名</param>
    public static bool IsCanEdit(string strExtension)
    {
        strExtension = strExtension.ToLower();

        if (strExtension.LastIndexOf(".") >= 0) 
        {
            strExtension = strExtension.Substring(strExtension.LastIndexOf(".")); 
        }
        else 
        {
            strExtension = ".txt";
        } 
        string[] arrExtension = { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
        for (int i = 0; i < arrExtension.Length; i++)
        {
            if (strExtension.Equals(arrExtension[i]))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}