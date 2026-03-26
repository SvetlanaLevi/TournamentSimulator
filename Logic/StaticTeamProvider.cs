using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public class StaticTeamProvider : ITeamProvider
    {
        public IReadOnlyList<Team> GetTeams()
        {
            return
            [
                new Team(Name: "A", Strength:10),
                new Team(Name: "B", Strength:6),
                new Team(Name: "C", Strength:3),
                new Team(Name: "D", Strength:9)
            ];
        }
    }
}
