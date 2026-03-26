namespace TournamentSimulator.Models
{
    public class Match(Team HomeTeam, Team AwayTeam, int Round)
    {
        public MatchResult? Result { get; private set; }
        public Team HomeTeam { get; } = HomeTeam;
        public Team AwayTeam { get; } = AwayTeam;
        public int Round { get; } = Round;

        public void SetResult(MatchResult result)
        {
            Result = result;
        }
    }
}
