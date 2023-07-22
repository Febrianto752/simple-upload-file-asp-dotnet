using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using UploadFile.Models;

namespace UploadFile.Controllers;

public class ExcelController : Controller
{
    public IActionResult Index()
    {
        var products = ImportExcel<Product>(@"C:\Users\Febrianto\Desktop\latihan-upload-file\UploadFile\UploadFile\product-test.xlsx", "Products");
        foreach (var product in products)
        {
            Console.WriteLine($"product.Name : {product.Name}");
            Console.WriteLine($"product.Price : {product.Price}");
            Console.WriteLine($"product.Units : {product.Units}");
            Console.WriteLine();
        }
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
                    Console.WriteLine($"val : {val}");
                    Console.WriteLine($"type : {type}");
                    Console.WriteLine($"prop : {prop}");
                    Console.WriteLine();
                    prop.SetValue(obj, Convert.ChangeType(val.ToString(), type));

                }

                list.Add(obj);
            }
        }




        return list;
    }
}

