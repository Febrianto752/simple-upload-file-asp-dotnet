using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace UploadFile.Repositories;

public class CustomerRepo : ICustomerRepo
{
    private IConfiguration _configuration;
    private IWebHostEnvironment _environment;

    public CustomerRepo(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public DataTable CustomerDataTable(string path)
    {
        var excelConnectionStr = _configuration.GetConnectionString("excelConnection");
        var dataTable = new DataTable();

        excelConnectionStr = string.Format(excelConnectionStr, path);

        using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionStr))
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                using (OleDbDataAdapter adapterExcel = new OleDbDataAdapter())
                {
                    excelConnection.Open();
                    cmd.Connection = excelConnection;
                    DataTable excelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    var sheetName = excelSchema.Rows[0]["Table_Name"].ToString();
                    excelConnection.Close();

                    excelConnection.Open();
                    cmd.CommandText = "Select * from [" + sheetName + "]";
                    adapterExcel.SelectCommand = cmd;
                    adapterExcel.Fill(dataTable);
                    excelConnection.Close();
                }
            }
        }

        return dataTable;

    }

    public string DocumentUpload(IFormFile formFile)
    {
        string uploadPath = _environment.WebRootPath;
        string destPath = Path.Combine(uploadPath, "uploaded_doc");

        if (!Directory.Exists(destPath))
        {
            Directory.CreateDirectory(destPath);
        }

        string sourceFile = Path.GetFileName(formFile.FileName);
        string path = Path.Combine(destPath, sourceFile);

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            formFile.CopyTo(fileStream);
        }

        return path;
    }

    public void ImportCustomer(DataTable customer)
    {
        var sqlConnectionStr = _configuration.GetConnectionString("sqlConnection");

        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection))
            {
                sqlBulkCopy.DestinationTableName = "Customers";
                sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                sqlBulkCopy.ColumnMappings.Add("Job", "Job");
                sqlBulkCopy.ColumnMappings.Add("Amount", "Amount");
                sqlBulkCopy.ColumnMappings.Add("HireDate", "HireDate");

                sqlConnection.Open();
                sqlBulkCopy.WriteToServer(customer);
                sqlConnection.Close();
            }
        }


    }
}

