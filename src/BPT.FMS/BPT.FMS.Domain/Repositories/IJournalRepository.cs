using BPT.FMS.Domain.Entities;

namespace BPT.FMS.Domain.Repositories
{
    public interface IJournalRepository : IRepository<Journal, Guid>
    {
        Task<(List<Journal> data, int total, int totalDisplay)> GetPagedJournalsAsync(
            int pageIndex = 1, int pageSize = 10, string order = "Type asc", string? search = "");

        Task CreateJournalWithEntriesAsync(Journal journal);
    }
}

