using GidraTopServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace GidraTopServer.Data;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ProductCard> ProductCards { get; set; }

}
