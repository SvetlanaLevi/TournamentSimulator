using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public class StaticTeamProvider : ITeamProvider
    {
        public IReadOnlyList<Team> GetTeams()
        {
            return
            [
                new Team(Name: "A", Strength:100),
                new Team(Name: "B", Strength:60),
                new Team(Name: "C", Strength:30),
                new Team(Name: "D", Strength:90)
            ];
        }
    }
}
