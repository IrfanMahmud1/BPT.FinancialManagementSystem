using BPT.FMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BPT.FMS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherEntry> VoucherEntries { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                entity.HasMany(e => e.Entries).WithOne().HasForeignKey(e => e.VoucherId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<VoucherEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ChartOfAccountId).IsRequired();
                entity.Property(e => e.Debit).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Credit).HasColumnType("decimal(18,2)");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
