using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using HD.Helper.Common;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace HD.Helper.Test
{
    [TestFixture]
    public class FileHelperTest
    {
        [Test]
        public void CopyFileFromClipbordTest()
        {
            Thread th = new Thread(new ThreadStart(FileCopy));
            th.ApartmentState = ApartmentState.STA;//坑啊
            th.Start();
        }

        private void FileCopy()
        {
            string desPath = Path.Combine(Environment.CurrentDirectory, "test");
            
            if (Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList();

                if (files.Count >0 )
                {
                    FileHelper.CopyFileFromClipbord(desPath);
                    foreach (var file in files)
                    {
                        string newFilePath = Path.Combine(desPath, Path.GetFileName(file));
                        Assert.IsTrue(File.Exists(newFilePath));
                    }
                }
            }
        }

    }
}
