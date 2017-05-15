using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Data;

namespace YoTennis.Models
{
    public interface IMatchesRepository
    {
        IEnumerable<MatchModelView> GetMatchListByUser(string userId);
    }
}
