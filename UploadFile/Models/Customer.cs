using System.ComponentModel.DataAnnotations;

namespace UploadFile.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Job { get; set; }
    public float Amount { get; set; }
    public DateTime HireDate { get; set; }
}

