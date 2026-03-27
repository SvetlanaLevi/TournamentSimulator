using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public class FixedMatchScheduler : IMatchScheduler
    {
        // Fixed round-robin schedule for 4 teams as per requirements
        public IReadOnlyList<Match> CreateSchedule(IReadOnlyList<Team> teams)
        {
            if (teams.Count != 4)
                throw new ArgumentException("The tournament is supported only for 4 teams");

            var team1 = teams[0];
            var team2 = teams[1];
            var team3 = teams[2];
            var team4 = teams[3];

            return
            [
                new Match(team1, team4, 1),
                new Match(team3, team2, 1),
                new Match(team2, team1, 2),
                new Match(team4, team3, 2),
                new Match(team4, team2, 3),
                new Match(team3, team1, 3)
            ];
        }
    }
}
