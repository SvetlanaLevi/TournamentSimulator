using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public interface IMatchScheduler
    {
        IReadOnlyList<Match> CreateSchedule(IReadOnlyList<Team> teams);
    }
}
