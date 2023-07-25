using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using UploadFile.Models;

namespace UploadFile.Controllers;

public class ExcelController : Controller
{
    public IActionResult Index()
    {
        //var products = ImportExcel(@"C:\Users\Febrianto\Desktop\latihan-upload-file\UploadFile\UploadFile\product-test.xlsx", "Products");
        var products = ImportExcel(Path.Combine(Directory.GetCurrentDirectory(), @"product-test.xlsx"));

        foreach (var product in products)
        {
            Console.WriteLine($"product.Name : {product.Name}");
            Console.WriteLine($"product.Price : {product.Price}");
            Console.WriteLine($"product.Units : {product.Units}");
            Console.WriteLine();
        }

        return View();
    }

    public List<Product> ImportExcel(string excelFilePath, string sheetName = "products")
    {
        var list = new List<Product>();

        using (var workbook = new XLWorkbook(excelFilePath))
        {
            var worksheet = workbook.Worksheet(1); // Ambil worksheet pertama

            var rows = worksheet.RowsUsed();

            foreach (var row in rows.Skip(1)) // Lewati baris pertama karena itu adalah judul kolom
            {
                Product product = new Product();
                product.Name = row.Cell(1).Value.ToString();
                product.Price = Convert.ToDecimal(row.Cell(2).Value.ToString());
                product.Units = Convert.ToInt32(row.Cell(3).Value.ToString());

                list.Add(product);
            }
        }

        return list;
    }
}

