using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public interface ITournamentService
    {
        public SimulationResponse Simulate();
    }
}