using BPT.FMS.Domain.Entities;
using BPT.FMS.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPT.FMS.Domain.Repositories;

namespace BPT.FMS.Infrastructure.Repositories
{
    public class VoucherRepository : Repository<Voucher, Guid>, IVoucherRepository
    {
        private readonly string? _connectionString;
        private readonly ApplicationDbContext _context;
        public VoucherRepository(IConfiguration configuration,ApplicationDbContext context) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }
        public async Task<(List<Voucher> data, int total, int totalDisplay)> GetPagedVouchersAsync(
            int pageIndex = 1, int pageSize = 10, string order = "Type asc", string? search = "")
        {
            int total = await _context.Vouchers.CountAsync();

            try
            {
                IQueryable<Voucher> query = _context.Vouchers
                                                    .AsNoTracking();

                query = query.Where(v => v.Entries.Count > 0);

                if (DateTime.TryParse(search, out var searchDate))
                {
                    query = query.Where(c => c.Date.Date == searchDate.Date);
                }
                else if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.Type.Contains(search) ||
                        c.ReferenceNo.Contains(search)
                    );
                }


                int totalDisplay = await query.CountAsync();

                query = query.OrderBy(order ?? "Type asc"); // requires System.Linq.Dynamic.Core
                var result = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result, total, totalDisplay);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving vouchers ");
                throw;
            }
        }
        public async Task CreateVoucherWithEntriesAsync(Voucher voucher)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_CreateVoucher", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VoucherId", voucher.Id);
            cmd.Parameters.AddWithValue("@Date", voucher.Date);
            cmd.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo);
            cmd.Parameters.AddWithValue("@Type", voucher.Type);

            var entries = new DataTable();
            entries.Columns.Add("EntryId", typeof(Guid));
            entries.Columns.Add("VoucherId", typeof(Guid));
            entries.Columns.Add("ChartOfAccountId", typeof(Guid));
            entries.Columns.Add("Debit", typeof(decimal));
            entries.Columns.Add("Credit", typeof(decimal));

            foreach (var e in voucher.Entries)
            {
                entries.Rows.Add(e.Id, voucher.Id, e.ChartOfAccountId, e.Debit, e.Credit);
            }

            var entriesParam = cmd.Parameters.AddWithValue("@Entries", entries);
            entriesParam.SqlDbType = SqlDbType.Structured;
            entriesParam.TypeName = "dbo.VoucherEntryTableType";

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
