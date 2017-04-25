using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;

namespace YoTennis.Models
{
    public class DatabaseRepository : IMatchesRepository
    {
        private readonly MyDbContext _context;

        public DatabaseRepository(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MyMatchModel> GetMatchListByUser(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            return _context.Matches.Where(x => x.UserId == userId)
                .Select(x => new MyMatchModel { Name = x.Id.ToString() });
        }
    }
}
