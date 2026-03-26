using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public interface IMatchSimulator
    {
        MatchResult Simulate(Team home, Team away);
    }
}