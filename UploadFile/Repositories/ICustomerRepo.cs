using System.Data;

namespace UploadFile.Repositories;

public interface ICustomerRepo
{
    string DocumentUpload(IFormFile formFile);
    DataTable CustomerDataTable(string path);
    void ImportCustomer(DataTable customer);
}

