using BPT.FMS.Domain.Entities;
using BPT.FMS.Domain.Repositories;
using BPT.FMS.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Infrastructure.Repositories
{
    public class UserRepository : Repository<User,Guid>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository( ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsAsync(Guid id,string email)
        {
            return await _context.Users
                                 .AnyAsync(u => u.Email == email && u.Id != id);
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            return await _context.Users
                                 .AnyAsync(u => u.Email == email && u.Password == password && u.IsActive);
        }

    }
}
