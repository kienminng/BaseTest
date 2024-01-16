using Microsoft.EntityFrameworkCore;
using BaseTest.Models.Entities;

namespace BaseTest.Repository.Context
{
    public class AppDbContext : DbContext,IAppDbContext
    {
        public AppDbContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<ExchangeHistory> ExchangeHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserCard> UserCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<ProductReviews> ProductReviews { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public async Task<int> CommitChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
