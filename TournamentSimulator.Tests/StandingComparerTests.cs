using TournamentSimulator.Logic.Comparers;
using TournamentSimulator.Models;

namespace TournamentSimulator.Tests
{
    public class StandingComparerTests
    {
        private Team CreateTeam(string name) => new(name, 50);

        private Standing CreateStanding(Team team, int goalsFor, int goalsAgainst)
        {
            var s = new Standing(team);
            s.ApplyMatch(goalsFor, goalsAgainst);
            return s;
        }

        private Match CreateMatch(Team home, Team away, int homeGoals, int awayGoals)
        {
            var match = new Match(home, away, 1);
            match.SetResult(new MatchResult(homeGoals, awayGoals));
            return match;
        }

        [Fact]
        public void Compare_ShouldThrow_WhenAIsNull()
        {
            // Arrange
            var comparer = new StandingComparer(new List<Match>());
            var standing = new Standing(CreateTeam("A"));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(null, standing));
        }

        [Fact]
        public void Compare_ShouldThrow_WhenBIsNull()
        {
            // Arrange
            var comparer = new StandingComparer(new List<Match>());
            var standing = new Standing(CreateTeam("A"));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => comparer.Compare(standing, null));
        }

        [Fact]
        public void Compare_ShouldSortByPoints_First()
        {
            // Arrange
            var a = new Standing(CreateTeam("A"));
            var b = new Standing(CreateTeam("B"));

            a.ApplyMatch(2, 0); // win → 3 points
            b.ApplyMatch(1, 1); // draw → 1 point

            var comparer = new StandingComparer(new List<Match>());

            // Act
            var result = comparer.Compare(a, b);

            // Assert
            Assert.True(result < 0); // A should be ranked higher
        }

        [Fact]
        public void Compare_ShouldSortByGoalDifference_WhenPointsEqual()
        {
            // Arrange
            var a = CreateStanding(CreateTeam("A"), 3, 0);
            var b = CreateStanding(CreateTeam("B"), 1, 0);

            var comparer = new StandingComparer(new List<Match>());

            // Act
            var result = comparer.Compare(a, b);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_ShouldSortByGoalsFor_WhenGoalDifferenceEqual()
        {
            // Arrange
            var a = CreateStanding(CreateTeam("A"), 3, 1); // GF 3, GD 2
            var b = CreateStanding(CreateTeam("B"), 2, 0); // GF 2, GD 2

            var comparer = new StandingComparer(new List<Match>());

            // Act
            var result = comparer.Compare(a, b);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void Compare_ShouldSortByGoalsAgainst_WhenAllAboveEqual()
        {
            // Arrange
            var a = CreateStanding(CreateTeam("A"), 2, 1);
            var b = CreateStanding(CreateTeam("B"), 2, 2);

            var comparer = new StandingComparer(new List<Match>());

            // Act
            var result = comparer.Compare(a, b);

            // Assert
            Assert.True(result < 0); // fewer goals conceded → higher rank
        }

        [Fact]
        public void Compare_ShouldUseHeadToHead_WhenAllStatsEqual()
        {
            // Arrange
            var teamA = CreateTeam("A");
            var teamB = CreateTeam("B");

            var a = CreateStanding(teamA, 1, 1);
            var b = CreateStanding(teamB, 1, 1);

            var match = CreateMatch(teamA, teamB, 2, 1); // A wins

            var comparer = new StandingComparer(new List<Match> { match });

            // Act
            var result = comparer.Compare(a, b);

            // Assert
            Assert.True(result < 0); // A should be ranked higher
        }

        [Fact]
        public void Compare_HeadToHead_Draw_ShouldReturnZero()
        {
            // Arrange
            var teamA = CreateTeam("A");
            var teamB = CreateTeam("B");

            var a = CreateStanding(teamA, 1, 1);
            var b = CreateStanding(teamB, 1, 1);

            var match = CreateMatch(teamA, teamB, 1, 1); // draw

            var comparer = new StandingComparer(new List<Match> { match });

            // Act
            var result = comparer.Compare(a, b);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_ShouldThrow_WhenMatchHasNoResult()
        {
            // Arrange
            var teamA = CreateTeam("A");
            var teamB = CreateTeam("B");

            var a = CreateStanding(teamA, 1, 1);
            var b = CreateStanding(teamB, 1, 1);

            var match = new Match(teamA, teamB, 1); // no result set

            var comparer = new StandingComparer(new List<Match> { match });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => comparer.Compare(a, b));
        }
    }
}