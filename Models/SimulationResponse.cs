namespace TournamentSimulator.Models
{
    class SimulationResponse
    {
        public List<MatchDto> Matches { get; set; }
        public List<StandingDto> Standings { get; set; }
        public List<string> QualifiedTeams { get; set; }
    }
}
