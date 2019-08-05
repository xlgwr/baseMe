using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace System
{
    /// <summary>
    /// C# 调用NPOI，修改EXCEL中的数据后并保存后，不会对公式进行更新操作。打开Excel表需要更新一下公式才生效
    /// 强制更新公式：C# 调用sheet.ForceFormulaRecalculation = true;  保存文件。打开Excel时将更新公式。但只在打开excel的瞬间进行更新。如果使用NPOI调用这个excel，查看表格内容，会发现没有发生变化。
    ///如果需要实时更新公式的结果，需要调用如下代码：hssfWorkBook.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();
    /// </summary>
    public static class ExcelExt
    {
        public static string ForceFormulaRecalculation(string filePath)
        {
            //excelPath-模板路径，为传递过来的参数
            IWorkbook wb = null;
            var saveNew = filePath;
            try
            {
                var getExts = Path.GetExtension(filePath);
                var getNames = Path.GetFileNameWithoutExtension(filePath);
                saveNew = Path.Combine(Path.GetDirectoryName(filePath), string.Format("{0}{1}{2}", getNames, "1", getExts));
                using (var fs = new FileStream(saveNew, FileMode.Create, FileAccess.Write))
                {
                    switch (getExts)
                    {
                        case ".xls":
                            wb = new HSSFWorkbook(new FileStream(filePath, FileMode.Open));
                            break;

                        case ".xlsx":
                            wb = new XSSFWorkbook(filePath);
                            break;

                        default:
                            wb = new XSSFWorkbook(filePath);
                            break;
                    }
                    var sheet = wb.GetSheetAt(0);
                    sheet.ForceFormulaRecalculation = true;
                    wb.Write(fs);
                }
                wb.Close();
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception)
                { 

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (wb != null)
                {
                    wb.Close();
                }
               
            }
            return saveNew;
        }
    }
}
