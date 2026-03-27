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

            var comparer = new StandingComparer(matches);

            var orderedStandings = standings.Values
                .OrderBy(s => s, comparer)
                .ToList();

            SetPositions(comparer, orderedStandings);

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

        private static void SetPositions(StandingComparer comparer, List<Standing> orderedStandings)
        {
            for (int i = 0; i < orderedStandings.Count; i++)
            {
                if (i > 0 && comparer.Compare(orderedStandings[i], orderedStandings[i - 1]) == 0)
                {
                    orderedStandings[i].Position = orderedStandings[i - 1].Position;
                }
                else
                {
                    orderedStandings[i].Position = i + 1;
                }
            }
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
