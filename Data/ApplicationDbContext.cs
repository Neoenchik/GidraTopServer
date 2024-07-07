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

    public DbSet<User> Users { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<ProductInfo> ProductInfo { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //1 корзина пренадлежит 1 пользователю
        modelBuilder.Entity<User>()
            .HasOne(u => u.Basket)
            .WithOne(b => b.User)
            .HasForeignKey<Basket>(b => b.UserId);

        //1 корзина может иметь много продуктов-корзина
        modelBuilder.Entity<Basket>()
            .HasMany(b => b.Basket_Products)
            .WithOne(bp => bp.Basket)
            .HasForeignKey(bp => bp.BasketId);

        //1 продукт из корзины является 1 продуктом
        modelBuilder.Entity<BasketProduct>()
            .HasOne(bp => bp.Product)
            .WithOne()
            .HasForeignKey<BasketProduct>(bp => bp.ProductId);

        //1 категории может пренадлежать много продуктов
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        //1 бренду может пренадлежать мнодество продуктов
        modelBuilder.Entity<Brand>()
            .HasMany(b => b.Products)
            .WithOne(p => p.Brand)
            .HasForeignKey(p => p.BrandId);

        // Настройка связи между Brand и Country
        modelBuilder.Entity<Brand>()
            .HasOne(b => b.Country)
            .WithMany(c => c.Brands)
            .HasForeignKey(b => b.CountryId)
            .OnDelete(DeleteBehavior.Cascade); // Удаление связанных брендов при удалении страны

        //один продукт может иметь множество оценок
        modelBuilder.Entity<Product>()
        .HasMany(p => p.Ratings)
        .WithOne(r => r.Product)
        .HasForeignKey(r => r.ProductId);

        //1 пользователь может иметь несколько оценок
        modelBuilder.Entity<User>()
            .HasMany(u => u.Ratings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<ProductInfo>()
            .HasOne(pi => pi.Product)
            .WithOne(p => p.ProductInfo)
            .HasForeignKey<ProductInfo>(pi => pi.ProductId);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Brands)
            .WithMany(b => b.Categories)
            .UsingEntity(j => j.ToTable("CategoryBrand"));
    }
}
