using TournamentSimulator.Models;
using Match = TournamentSimulator.Models.Match;

namespace TournamentSimulator.Logic.Comparers
{
    public class StandingComparer : IComparer<Standing>
    {
        private readonly IReadOnlyList<Match> _matches;

        public StandingComparer(IReadOnlyList<Match> matches)
        {
            _matches = matches ?? throw new ArgumentNullException(nameof(matches));
        }

        public int Compare(Standing? x, Standing? y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            int cmp;

            cmp = y.Points.CompareTo(x.Points);
            if (cmp != 0) return cmp;

            cmp = y.GoalDifference.CompareTo(x.GoalDifference);
            if (cmp != 0) return cmp;

            cmp = y.GoalsFor.CompareTo(x.GoalsFor);
            if (cmp != 0) return cmp;

            cmp = x.GoalsAgainst.CompareTo(y.GoalsAgainst);
            if (cmp != 0) return cmp;

            return CompareHeadToHead(x, y);
        }

        private int CompareHeadToHead(Standing x, Standing y)
        {
            var matches = _matches
                .Where(m => IsHeadToHead(m, x, y))
                .ToList();

            if (matches.Any(m => m.Result == null))
                throw new InvalidOperationException("All head-to-head matches must have results");

            int aGoals = 0;
            int bGoals = 0;

            foreach (var match in matches)
            {
                var result = match.Result!;

                if (match.HomeTeam.Name == x.Team.Name)
                {
                    aGoals += result.HomeTeamScore;
                    bGoals += result.AwayTeamScore;
                }
                else
                {
                    aGoals += result.AwayTeamScore;
                    bGoals += result.HomeTeamScore;
                }
            }

            if (aGoals > bGoals) return -1;
            if (aGoals < bGoals) return 1;

            return 0;
        }

        private static bool IsHeadToHead(Match match, Standing x, Standing y) =>
            (match.HomeTeam.Name == x.Team.Name && match.AwayTeam.Name == y.Team.Name)
            || (match.HomeTeam.Name == y.Team.Name && match.AwayTeam.Name == x.Team.Name);
    }
}
