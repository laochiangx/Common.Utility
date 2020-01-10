using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Utility
{
    public class RunCmd
    {
        private Process proc = null;
        public RunCmd()
        {
            proc = new Process();
        }

        public void Exe(string cmd)
        {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

           // proc.OutputDataReceived += new DataReceivedEventHandler(sortProcess_OutputDataReceived);
            proc.Start();
            StreamWriter cmdWriter = proc.StandardInput;
            proc.BeginOutputReadLine();
            if (!String.IsNullOrEmpty(cmd))
            {
                cmdWriter.WriteLine(cmd);
            }
            cmdWriter.Close();          
            proc.Close();
        }

      
        private void sortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                //this.BeginInvoke(new Action(() => { this.listBox1.Items.Add(e.Data); }));
            }
        }
    }
}
