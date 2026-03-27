using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public interface ITeamProvider
    {
        IReadOnlyList<Team> GetTeams();
    }
}
