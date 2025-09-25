using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure.Repositories
{
    public class JournalRepository : Repository<Journal, Guid>, IJournalRepository
    {
        private readonly string? _connectionString;
        private readonly ApplicationDbContext _context;

        public JournalRepository(IConfiguration configuration, ApplicationDbContext context) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        public async Task<(List<Journal> data, int total, int totalDisplay)> GetPagedJournalsAsync(
            int pageIndex = 1, int pageSize = 10, string order = "Type asc", string? search = "")
        {
            int total = await _context.Journals.CountAsync();

            try
            {
                IQueryable<Journal> query = _context.Journals
                                                    .AsNoTracking();

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

                query = query.OrderBy(order ?? "Type asc");
                var result = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result, total, totalDisplay);
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while retrieving journals ");
                throw;
            }
        }

        public async Task CreateJournalWithEntriesAsync(Journal journal)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_CreateJournal", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@JournalId", journal.Id);
            cmd.Parameters.AddWithValue("@Date", journal.Date);
            cmd.Parameters.AddWithValue("@ReferenceNo", journal.ReferenceNo);
            cmd.Parameters.AddWithValue("@Type", journal.Type);

            var entries = new DataTable();
            entries.Columns.Add("EntryId", typeof(Guid));
            entries.Columns.Add("JournalId", typeof(Guid));
            entries.Columns.Add("ChartOfAccountId", typeof(Guid));
            entries.Columns.Add("Debit", typeof(decimal));
            entries.Columns.Add("Credit", typeof(decimal));

            foreach (var e in journal.Entries)
            {
                entries.Rows.Add(e.Id, journal.Id, e.ChartOfAccountId, e.Debit, e.Credit);
            }

            var entriesParam = cmd.Parameters.AddWithValue("@Entries", entries);
            entriesParam.SqlDbType = SqlDbType.Structured;
            entriesParam.TypeName = "dbo.JournalEntryTableType";

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

