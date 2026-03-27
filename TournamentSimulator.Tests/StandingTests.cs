using TournamentSimulator.Models;

namespace TournamentSimulator.Tests
{
    public class StandingTests
    {
        private readonly Team _team = new("Test", 50);

        [Fact]
        public void ApplyMatch_ShouldIncreasePlayed()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(1, 0);

            Assert.Equal(1, standing.Played);
        }

        [Fact]
        public void ApplyMatch_Win_ShouldIncreaseWinsAndPoints()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(2, 1);

            Assert.Equal(1, standing.Wins);
            Assert.Equal(0, standing.Draws);
            Assert.Equal(0, standing.Losses);
            Assert.Equal(3, standing.Points);
        }

        [Fact]
        public void ApplyMatch_Draw_ShouldIncreaseDrawsAndPoints()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(1, 1);

            Assert.Equal(0, standing.Wins);
            Assert.Equal(1, standing.Draws);
            Assert.Equal(0, standing.Losses);
            Assert.Equal(1, standing.Points);
        }

        [Fact]
        public void ApplyMatch_Loss_ShouldIncreaseLossesOnly()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(0, 2);

            Assert.Equal(0, standing.Wins);
            Assert.Equal(0, standing.Draws);
            Assert.Equal(1, standing.Losses);
            Assert.Equal(0, standing.Points);
        }

        [Fact]
        public void ApplyMatch_ShouldUpdateGoalsCorrectly()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(3, 1);

            Assert.Equal(3, standing.GoalsFor);
            Assert.Equal(1, standing.GoalsAgainst);
            Assert.Equal(2, standing.GoalDifference);
        }

        [Fact]
        public void ApplyMatch_MultipleMatches_ShouldAccumulateStats()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(2, 0); // win
            standing.ApplyMatch(1, 1); // draw
            standing.ApplyMatch(0, 3); // loss

            Assert.Equal(3, standing.Played);

            Assert.Equal(1, standing.Wins);
            Assert.Equal(1, standing.Draws);
            Assert.Equal(1, standing.Losses);

            Assert.Equal(4, standing.Points);

            Assert.Equal(3, standing.GoalsFor);
            Assert.Equal(4, standing.GoalsAgainst);
            Assert.Equal(-1, standing.GoalDifference);
        }

        [Fact]
        public void GoalDifference_ShouldBeCalculatedCorrectly()
        {
            var standing = new Standing(_team);

            standing.ApplyMatch(5, 3);

            Assert.Equal(2, standing.GoalDifference);
        }

        [Fact]
        public void NewStanding_ShouldHaveZeroStats()
        {
            var standing = new Standing(_team);

            Assert.Equal(0, standing.Played);
            Assert.Equal(0, standing.Wins);
            Assert.Equal(0, standing.Draws);
            Assert.Equal(0, standing.Losses);
            Assert.Equal(0, standing.Points);
            Assert.Equal(0, standing.GoalsFor);
            Assert.Equal(0, standing.GoalsAgainst);
            Assert.Equal(0, standing.GoalDifference);
        }
    }
}