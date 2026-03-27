namespace TournamentSimulator.Models
{
    public record Round(int Number, IReadOnlyList<Match> Matches)
    {
    }
}
