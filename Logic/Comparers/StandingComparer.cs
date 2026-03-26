using TournamentSimulator.Models;

namespace TournamentSimulator.Logic.Comparers
{
    public class StandingComparer : IComparer<Standing>
    {
        private readonly IReadOnlyList<Models.Match> _matches;

        public StandingComparer(IReadOnlyList<Match> matches)
        {
            _matches = matches;
        }

        public int Compare(Standing? a, Standing? b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            int result = b.Points.CompareTo(a.Points);
            if (result != 0) return result;

            result = b.GoalDifference.CompareTo(a.GoalDifference);
            if (result != 0) return result;

            result = b.GoalsFor.CompareTo(a.GoalsFor);
            if (result != 0) return result;

            result = a.GoalsAgainst.CompareTo(b.GoalsAgainst);
            if (result != 0) return result;

            return CompareHeadToHead(a, b);
        }

        private int CompareHeadToHead(Standing a, Standing b)
        {
            var match = _matches.FirstOrDefault(m =>
                (m.HomeTeam == a.Team && m.AwayTeam == b.Team) ||
                (m.HomeTeam == b.Team && m.AwayTeam == a.Team));

            if (match == null) throw new ArgumentNullException(nameof(Match));
            if (match.Result == null) throw new ArgumentNullException(nameof(MatchResult));

            if (match.Result.HomeTeamScore > match.Result.AwayTeamScore)
                return match.HomeTeam == a.Team ? -1 : 1;

            if (match.Result.HomeTeamScore < match.Result.AwayTeamScore)
                return match.HomeTeam == b.Team ? -1 : 1;

            return 0; //draw
        }
    }
}
