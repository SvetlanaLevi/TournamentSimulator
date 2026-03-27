using TournamentSimulator.Logic;
using TournamentSimulator.Models;

namespace TournamentSimulator.Tests
{
    public class MatchSimulatorTests
    {
        private readonly MatchSimulator _simulator = new();

        [Fact]
        public void Simulate_ShouldReturnResult()
        {
            var home = new Team("Home", 80);
            var away = new Team("Away", 70);

            var result = _simulator.Simulate(home, away);

            Assert.NotNull(result);
        }

        [Fact]
        public void Simulate_GoalsShouldBeNonNegative()
        {
            var home = new Team("Home", 50);
            var away = new Team("Away", 50);

            var result = _simulator.Simulate(home, away);

            Assert.True(result.HomeTeamScore >= 0);
            Assert.True(result.AwayTeamScore >= 0);
        }

        [Fact]
        public void StrongerTeam_ShouldScoreMoreOnAverage()
        {
            var strong = new Team("Strong", 90);
            var weak = new Team("Weak", 10);

            int strongGoals = 0;
            int weakGoals = 0;

            for (int i = 0; i < 1000; i++)
            {
                var result = _simulator.Simulate(strong, weak);
                strongGoals += result.HomeTeamScore;
                weakGoals += result.AwayTeamScore;
            }

            Assert.True(strongGoals > weakGoals);
        }

        [Fact]
        public void EqualTeams_ShouldHaveSimilarGoalsOnAverage()
        {
            var teamA = new Team("A", 50);
            var teamB = new Team("B", 50);

            int totalA = 0;
            int totalB = 0;

            for (int i = 0; i < 2000; i++)
            {
                var result = _simulator.Simulate(teamA, teamB);
                totalA += result.HomeTeamScore;
                totalB += result.AwayTeamScore;
            }

            var diff = Math.Abs(totalA - totalB);

            Assert.True(diff < totalA * 0.2);
        }

        [Fact]
        public void HomeTeam_ShouldHaveAdvantage()
        {
            var teamA = new Team("A", 60);
            var teamB = new Team("B", 60);

            int homeGoals = 0;
            int awayGoals = 0;

            for (int i = 0; i < 2000; i++)
            {
                var result = _simulator.Simulate(teamA, teamB);
                homeGoals += result.HomeTeamScore;
                awayGoals += result.AwayTeamScore;
            }

            Assert.True(homeGoals > awayGoals);
        }

        [Fact]
        public void VeryWeakAttack_ShouldProduceLowGoals()
        {
            var weak = new Team("Weak", 0);
            var strong = new Team("Strong", 100);

            int totalGoals = 0;

            for (int i = 0; i < 1000; i++)
            {
                var result = _simulator.Simulate(weak, strong);
                totalGoals += result.HomeTeamScore;
            }

            Assert.True(totalGoals < 300);
        }
    }
}