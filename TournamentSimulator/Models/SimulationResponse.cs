namespace TournamentSimulator.Models
{
    public class SimulationResponse
    {
        public required IReadOnlyList<Round> Rounds { get; set; }
        public required IReadOnlyList<Standing> Standings { get; set; }
        public required IReadOnlyList<string> QualifiedTeams { get; set; }
    }
}
