using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ExcelRW 
{
    public static List<string> readFile = new List<string>();
    public static List<string> compareFile = new List<string>();
    public static Dictionary<string, string> readDic = new Dictionary<string, string>();
    //找并集

    public static  (List<string>,Dictionary<string,string>) EppReadExcel(string path)
    {
        var pjList = new List<string>();
        var pjDic = new Dictionary<string, string>();
        using (var package = new ExcelPackage(new FileInfo(path)))
        {
            if (package == null)
            {
                Debug.LogError("not a correct path!");
                return (pjList,pjDic);
            }
            var worksheet = package.Workbook.Worksheets[0];
            int row = worksheet.Dimension.Rows;
            int col = worksheet.Dimension.Columns;
            object value = null;
            for (int i = 2; i < row; i++)
            {
                value = worksheet.Cells[i, 2].Value;
                if (value != null && !pjList.Contains(value.ToString()))
                {
                    pjList.Add(value.ToString());
                    if (worksheet.Cells[i,5].Value!=null)
                    {
                        pjDic.Add(value.ToString(), worksheet.Cells[i, 5].Value.ToString());
                    }
                }
            }
            Debug.Log("finish");
            return (pjList, pjDic);
        }
    }
    public static void WriteOrCreateExcel(string path,List<string> mergedList)
    {
        using (var package = new ExcelPackage())
        {
            if (package == null)
            {
                Debug.LogError("not a correct path!");
                return;
            }
            ExcelPackage.License.SetNonCommercialPersonal("MyTest");
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            int count = mergedList.Count;
            for (int i = 1; i <= count; i++)
            {
                worksheet.Cells[i, 1].Value = mergedList[i-1];
                worksheet.Cells[i, 2].Value = readDic[mergedList[i - 1]];
            }
            //worksheet.Cells.AutoFitColumns();
            FileInfo file = new FileInfo(path);
            package.SaveAs(file);
            Debug.Log("finish");
        }
    }
    public static void DrawExcel(string readPath,string loadPath, List<string> drawList,int judgeCow)
    {
        using (var package = new ExcelPackage(new FileInfo(readPath)))
        {
            if (package == null)
            {
                Debug.LogError("not a correct path!");
                return;
            }
            ExcelPackage.License.SetNonCommercialPersonal("MyTest");
            var worksheet = package.Workbook.Worksheets[0];
            int count = readFile.Count;
            var list = new List<int>();
            for (int i = 2; i <= count; i++)
            {
                if (!drawList.Contains(worksheet.Cells[i, judgeCow].Value.ToString()))
                {
                    SetRowColor(worksheet,i,System.Drawing.Color.Yellow);
                    list.Add(i);
                }
            }
            package.SaveAs(loadPath);
            Debug.Log("finish");
        }
    }

    private static void SetRowColor(ExcelWorksheet worksheet, int rowIndex, System.Drawing.Color color)
    {
        for (int colIndex = 1; colIndex <= worksheet.Dimension.End.Column; colIndex++) // 对所有列进行操作
        {
            var cell = worksheet.Cells[rowIndex, colIndex];
            cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
        }
    }
    public static List<string> GetIntersectionList(List<string> list1,List<string> list2)
    {
        var hashSet = new HashSet<string>(list1);
        List<string> intersectionList = hashSet.Intersect(list2).ToList();
        return intersectionList;
    }
}
