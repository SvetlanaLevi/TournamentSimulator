using TournamentSimulator.Logic.Comparers;
using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public class TournamentService(ITeamProvider teamProvider, IMatchScheduler scheduler, IMatchSimulator simulator) : ITournamentService
    {
        public SimulationResponse Simulate()
        {
            var teams = teamProvider.GetTeams();

            var standings = teams.ToDictionary(
                t => t.Name,
                t => new Standing(t)
            );

            var matches = scheduler.CreateSchedule(teams);

            foreach (var match in matches)
            {
                var result = simulator.Simulate(match.HomeTeam, match.AwayTeam);

                ApplyMatchResult(standings, match, result);
            }

            var orderedStandings = standings.Values
                .OrderBy(s => s, new StandingComparer(matches))
                .Select((s, index) =>
                {
                    s.Position = index + 1;
                    return s;
                })
                .ToList();

            var qualified = orderedStandings
                .Take(2)
                .Select(s => s.Team)
                .ToList();

            return new SimulationResponse
            {
                Rounds = matches
                .GroupBy(m => m.Round)
                    .Select(g => new Round(g.Key, g.ToList()))
                    .ToList(),
                Standings = orderedStandings,
                QualifiedTeams = qualified.Select(x => x.Name).ToList()
            };
        }

        private static void ApplyMatchResult(Dictionary<string, Standing> standings, Match match, MatchResult result)
        {
            match.SetResult(result);

            standings[match.HomeTeam.Name]
                .ApplyMatch(result.HomeTeamScore, result.AwayTeamScore);

            standings[match.AwayTeam.Name]
                .ApplyMatch(result.AwayTeamScore, result.HomeTeamScore);
        }
    }
}
