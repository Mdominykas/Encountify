using Encountify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public interface IScoreboardCreation
    {
        Task<List<ScoreboardEntry>> CreateScoreboard(bool reversed = true);
    }
}
