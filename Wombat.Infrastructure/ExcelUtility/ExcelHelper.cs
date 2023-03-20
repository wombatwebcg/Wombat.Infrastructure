using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Wombat.Infrastructure
{
    public static partial class ExcelHelper
    {

        public static string ExportExcel(string path, string fileName,DataSet sourceDs, string replaceSign = "_")
        {
            if (!Directory.Exists(Environment.CurrentDirectory + path))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + path);
            }
            fileName = fileName.Replace("*", replaceSign);
            fileName = fileName.Replace("<", replaceSign);
            fileName = fileName.Replace(">", replaceSign);
            fileName = fileName.Replace(":", replaceSign);
            fileName = fileName.Replace("?", replaceSign);
            fileName = fileName.Replace("/", replaceSign);
            fileName = fileName.Replace(@"\", replaceSign);
            string newPath = Environment.CurrentDirectory + path + $"\\{fileName}.xls";
            var fs = File.OpenWrite(newPath);//以write方式打开文件，wb工作表写回
            //创建EXCEL
            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个Sheet
            //ISheet sheet = wk.CreateSheet(fileName);
            wk.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
            wk.Write(fs);
            fs.Close();
          return  ToExcel(sourceDs, newPath);
        }






    }
}
