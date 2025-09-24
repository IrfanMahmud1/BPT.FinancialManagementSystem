using BPT.FMS.Domain.Dtos;
using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        public Task<List<VoucherEntryDto>> GetVoucherEntriesByVoucherIdAsync(Guid voucherId);
        public IChartOfAccountRepository ChartOfAccountRepository { get; }
        public IVoucherRepository VoucherRepository { get; }
        public IUserRepository UserRepository { get; }
        public Task<(List<VoucherEntry> data, int total, int totalDisplay)> GetPagedVoucherEntriesAsync(
           Guid voucherId, int pageIndex = 1, int pageSize = 10, string order = "Type asc", string? search = "");
    }
}
