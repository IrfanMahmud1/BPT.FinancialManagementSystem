using BPT.FMS.Domain;
using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPT.FMS.Domain.Dtos;

namespace BPT.FMS.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IChartOfAccountRepository ChartOfAccountRepository { get; private set; }
        public IVoucherRepository VoucherRepository { get; private set; }
        public IJournalRepository JournalRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        private readonly ApplicationDbContext _context;
        public ApplicationUnitOfWork(ApplicationDbContext applicationDbContext,
            IChartOfAccountRepository chartOfAccountRepository,
            IVoucherRepository voucherRepository,
            IJournalRepository journalRepository,
            IUserRepository userRepository) : base(applicationDbContext)
        {
            ChartOfAccountRepository = chartOfAccountRepository;
            VoucherRepository = voucherRepository;
            JournalRepository = journalRepository;
            UserRepository = userRepository;
            _context = applicationDbContext;
        }

        public async Task<List<VoucherEntryDto>> GetVoucherEntriesByVoucherIdAsync(Guid voucherId)
        {
            return await _context.VoucherEntries
                                 .AsNoTracking()
                                 .AsSplitQuery()
                                 .Where(ve => ve.VoucherId == voucherId)
                                 .Select(ve => new VoucherEntryDto
                                 {
                                     Id = ve.Id,
                                     ChartOfAccountId = ve.ChartOfAccountId,
                                     Debit = ve.Debit,
                                     Credit = ve.Credit,
                                     VoucherId = ve.VoucherId,
                                     Voucher = ve.Voucher == null ? null : new VoucherDto
                                     {
                                         Id = ve.Voucher.Id,
                                         Type = ve.Voucher.Type,
                                         Date = ve.Voucher.Date,
                                         ReferenceNo = ve.Voucher.ReferenceNo
                                     },
                                        ChartOfAccount = ve.ChartOfAccount == null ? null : new ChartOfAccountDto
                                        {
                                            Id = ve.ChartOfAccount.Id,
                                            AccountName = ve.ChartOfAccount.AccountName,
                                        }
                                 })
                                 .ToListAsync();
        }
        public async Task<List<JournalEntryDto>> GetJournalEntriesByJournalIdAsync(Guid journalId)
        {
            return await _context.JournalEntries
                                 .AsNoTracking()
                                 .AsSplitQuery()
                                 .Where(je => je.JournalId == journalId)
                                 .Select(je => new JournalEntryDto
                                 {
                                     Id = je.Id,
                                     ChartOfAccountId = je.ChartOfAccountId,
                                     Debit = je.Debit,
                                     Credit = je.Credit,
                                     JournalId = je.JournalId,
                                     Journal = je.Journal == null ? null : new JournalDto
                                     {
                                         Id = je.Journal.Id,
                                         Type = je.Journal.Type,
                                         Date = je.Journal.Date,
                                         ReferenceNo = je.Journal.ReferenceNo
                                     },
                                     ChartOfAccount = je.ChartOfAccount == null ? null : new ChartOfAccountDto
                                     {
                                         Id = je.ChartOfAccount.Id,
                                         AccountName = je.ChartOfAccount.AccountName,
                                     }
                                 })
                                 .ToListAsync();
        }
        public async Task<(List<JournalEntry> data, int total, int totalDisplay)> GetPagedJournalEntriesAsync(
           Guid journalId, int pageIndex = 1, int pageSize = 10, string order = "Debit asc", string? search = "")
        {
            int total = await _context.JournalEntries.CountAsync();

            try
            {
                IQueryable<JournalEntry> query = _context.JournalEntries
                                                    .AsNoTracking()
                                                    .AsSplitQuery()
                                                    .Include(j => j.Journal)
                                                    .Include(j => j.ChartOfAccount);

                query = query.Where(j => j.JournalId == journalId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.Credit.ToString().Contains(search) ||
                        c.Debit.ToString().Contains(search) ||
                        c.ChartOfAccount.AccountName.Contains(search) ||
                        c.Journal.Type.Contains(search)
                    );
                }


                int totalDisplay = await query.CountAsync();

                query = query.OrderBy(order ?? "Debit asc");
                var result = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result, total, totalDisplay);
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while retrieving journal entries ");
                throw;
            }
        }
        public async Task<(List<VoucherEntry> data, int total, int totalDisplay)> GetPagedVoucherEntriesAsync(
           Guid voucherId, int pageIndex = 1, int pageSize = 10, string order = "Debit asc", string? search = "")
        {
            int total = await _context.Vouchers.CountAsync();

            try
            {
                IQueryable<VoucherEntry> query = _context.VoucherEntries
                                                    .AsNoTracking()
                                                    .AsSplitQuery()
                                                    .Include(v => v.Voucher)
                                                    .Include(v => v.ChartOfAccount);

                query = query.Where(v => v.VoucherId == voucherId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.Credit.ToString().Contains(search) ||
                        c.Debit.ToString().Contains(search) ||
                        c.ChartOfAccount.AccountName.Contains(search) ||
                        c.Voucher.Type.Contains(search)
                    );
                }


                int totalDisplay = await query.CountAsync();

                query = query.OrderBy(order ?? "VoucherType asc"); // requires System.Linq.Dynamic.Core
                var result = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result, total, totalDisplay);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving voucher entries ");
                throw;
            }
        }
    }
}
