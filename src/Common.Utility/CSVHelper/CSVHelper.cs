using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNet.Utilities.CSVHelper
{
    public class CSVHelper
    {
       
        /// <summary>
        /// CSV转换成DataTable（OleDb数据库访问方式）
        /// </summary>
        /// <param name="csvPath">csv文件路径</param>
        /// <returns></returns>
        public static DataTable CSVToDataTableByOledb(string csvPath)
        {
            DataTable csvdt = new DataTable("csv");
            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException("csv文件路径不存在!");
            }

            FileInfo fileInfo = new FileInfo(csvPath);
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo.DirectoryName + ";Extended Properties='Text;'"))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + fileInfo.Name + "]", conn);
                adapter.Fill(csvdt);
            }
            return csvdt;
        }

        /// <summary>
        /// CSV转换成DataTable（文件流方式）
        /// </summary>
        /// <param name="csvPath">csv文件路径</param>
        /// <returns></returns>
        public static DataTable CSVToDataTableByStreamReader(string csvPath)
        {
            DataTable csvdt = new DataTable("csv");

            int intColCount = 0;
            bool blnFlag = true;
            DataColumn column;
            DataRow row;
            string strline = null;
            string[] aryline;
            Encoding encoding=null;

            using (StreamReader reader = new StreamReader(csvPath, encoding))
            {
                while (!string.IsNullOrEmpty((strline = reader.ReadLine())))
                {
                    aryline = strline.Split(new char[] { ',' });

                    if (blnFlag)
                    {
                        blnFlag = false;
                        intColCount = aryline.Length;
                        for (int i = 0; i < aryline.Length; i++)
                        {
                            column = new DataColumn(aryline[i]);
                            csvdt.Columns.Add(column);
                        }
                        continue;
                    }

                    row = csvdt.NewRow();
                    for (int i = 0; i < intColCount; i++)
                    {
                        row[i] = aryline[i];
                    }
                    csvdt.Rows.Add(row);
                }
            }

            return csvdt;
        }

        /// <summary>
        /// DataTable 生成 CSV
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="csvPath">csv文件路径</param>
        public static void DataTableToCSV(DataTable dt, string csvPath)
        {
            if (null == dt)
                return;

            StringBuilder csvText = new StringBuilder();
            StringBuilder csvrowText = new StringBuilder();
            foreach (DataColumn dc in dt.Columns)
            {
                csvrowText.Append(",");
                csvrowText.Append(dc.ColumnName);
            }
            csvText.AppendLine(csvrowText.ToString().Substring(1));

            foreach (DataRow dr in dt.Rows)
            {
                csvrowText = new StringBuilder();
                foreach (DataColumn dc in dt.Columns)
                {
                    csvrowText.Append(",");
                    csvrowText.Append(dr[dc.ColumnName].ToString().Replace(',', ' '));
                }
                csvText.AppendLine(csvrowText.ToString().Substring(1));
            }

            File.WriteAllText(csvPath, csvText.ToString(), Encoding.Default);
        }


        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableheader">表头</param>
        /// <param name="columname">字段标题,逗号分隔</param>
        public static bool dt2csv(DataTable dt, string strFilePath, string tableheader, string columname)
        {
            try
            {
                string strBufferLine = "";
                StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);
                strmWriterObj.WriteLine(tableheader);
                strmWriterObj.WriteLine(columname);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strBufferLine = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dt.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        public static DataTable csv2dt(string filePath, int n, DataTable dt)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8, false);
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    string[] split = str.Split(',');

                    System.Data.DataRow dr = dt.NewRow();
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

    }
}
