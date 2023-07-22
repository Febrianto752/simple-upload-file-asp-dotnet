using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using UploadFile.Models;

namespace UploadFile.Controllers;

public class ExcelController : Controller
{
    public IActionResult Index()
    {
        var products = ImportExcel<Product>(@"C:\Users\Febrianto\Desktop\latihan-upload-file\UploadFile\UploadFile\product-test.xlsx", "Products");
        Console.WriteLine(products);
        return View();
    }



    public List<T> ImportExcel<T>(string excelFilePath, string sheetName)
    {
        var list = new List<T>();

        Type typeOfObject = typeof(T);

        using (IXLWorkbook workbook = new XLWorkbook(excelFilePath))
        {
            var worksheet = workbook.Worksheets.Where(w => { Console.WriteLine(w.Name); return w.Name == sheetName; }).FirstOrDefault();
            var properties = typeOfObject.GetProperties();
            Console.WriteLine($"properties : {properties}");
            // header column texts
            var columns = worksheet.FirstRow().Cells().Select((v, i) => new { Value = v.Value, Index = i + 1 });

            // skip first row which is used for column header texts
            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
            {
                T obj = (T)Activator.CreateInstance(typeOfObject);
                Console.WriteLine($"obj : {obj}");
                foreach (var prop in properties)
                {
                    int colIndex = columns.SingleOrDefault(c => c.Value.ToString().ToLower() == prop.Name.ToString().ToLower()).Index;
                    var val = row.Cell(colIndex).Value;
                    var type = prop.PropertyType;
                    Console.WriteLine($"val : {val.GetType()}");
                    Console.WriteLine($"type : {type}");
                    Console.WriteLine($"prop : {prop.GetType()}");
                    Console.WriteLine();
                    prop.SetValue(obj, val);

                }

                list.Add(obj);
            }
        }


        //using (var workbook = new XLWorkbook(excelFilePath))
        //{
        //    var worksheet = workbook.Worksheet(1); // Ambil worksheet pertama

        //    var rows = worksheet.RowsUsed();

        //    foreach (var row in rows.Skip(1)) // Lewati baris pertama karena itu adalah judul kolom
        //    {
        //        Product product = new Product();
        //        product.Name = row.Cell(1).Value.ToString();
        //        product.Price = Convert.ToDecimal(row.Cell(2).Value);
        //        product.Units = Convert.ToInt32(row.Cell(3).Value);

        //        list.Add(product);
        //    }
        //}




        return list;
    }
}

