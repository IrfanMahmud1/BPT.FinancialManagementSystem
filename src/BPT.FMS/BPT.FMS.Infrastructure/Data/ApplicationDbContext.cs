using BPT.FMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPT.FMS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherEntry> VoucherEntries { get; set; }

        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });
            modelBuilder.Entity<ChartOfAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountName).IsRequired();
                entity.Property(e => e.AccountType).IsRequired();
            });
            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.ReferenceNo).IsRequired();
                entity.Property(e => e.Type).IsRequired();
            });
            modelBuilder.Entity<VoucherEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ChartOfAccountId).IsRequired();
                entity.Property(e => e.Debit).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Credit).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Voucher)
                                .WithMany(v => v.Entries)
                                .HasForeignKey(e => e.VoucherId)
                                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<User>()
               .HasData(
               new User
               {
                   Id = new Guid("8C647159-9A27-43C9-AA21-115E9DDDEE9E"),
                   Email = "admin@gmail.com",
                   Password = "admin12345",
                   UserName = "admin",
                   AccessLevel = "Read-Write",
                   CreatedBy = new Guid("8C647159-9A27-43C9-AA21-115E9DDDEE9E"),
                   CreatedDate = new DateTime(2025, 8, 17, 19, 31, 26, DateTimeKind.Utc),
                   IsActive = true
               });
            base.OnModelCreating(modelBuilder);
        }
    }
}
