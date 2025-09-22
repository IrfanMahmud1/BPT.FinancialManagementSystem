using BPT.FMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Domain.Repositories
{
    public interface IVoucherRepository : IRepository<Voucher, Guid>
    {
        public Task<(List<Voucher> data, int total, int totalDisplay)> GetPagedVouchersAsync(
    int pageIndex = 1, int pageSize = 10, string order = " asc", string? search = "");
        public Task CreateVoucherWithEntriesAsync(Voucher voucher);
    }
}
