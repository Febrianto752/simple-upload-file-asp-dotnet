using Microsoft.EntityFrameworkCore;

namespace UploadFile.Models;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options) { }

    public virtual DbSet<Customer> Customers { get; set; }
}

