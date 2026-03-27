using Moq;
using TournamentSimulator.Logic;
using TournamentSimulator.Models;
using Match = TournamentSimulator.Models.Match;

namespace TournamentSimulator.Tests
{
    public class TournamentServiceTests
    {
        private readonly Mock<ITeamProvider> _teamProviderMock = new();
        private readonly Mock<IMatchScheduler> _schedulerMock = new();
        private readonly Mock<IMatchSimulator> _simulatorMock = new();

        private TournamentService CreateService()
        {
            return new TournamentService(
                _teamProviderMock.Object,
                _schedulerMock.Object,
                _simulatorMock.Object
            );
        }

        [Fact]
        public void Simulate_ShouldReturnResponse()
        {
            var teams = new List<Team>
            {
                new("A", 50),
                new("B", 50)
            };

            var matches = new List<Match>
            {
                new Match(teams[0], teams[1], 1)
            };

            _teamProviderMock.Setup(x => x.GetTeams()).Returns(teams);
            _schedulerMock.Setup(x => x.CreateSchedule(teams)).Returns(matches);
            _simulatorMock
                .Setup(x => x.Simulate(It.IsAny<Team>(), It.IsAny<Team>()))
                .Returns(new MatchResult(1, 0));

            var service = CreateService();

            var result = service.Simulate();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Standings);
            Assert.NotEmpty(result.Rounds);
        }

        [Fact]
        public void Simulate_ShouldApplyMatchResultsToStandings()
        {
            var teamA = new Team("A", 50);
            var teamB = new Team("B", 50);

            var teams = new List<Team> { teamA, teamB };

            var match = new Match(teamA, teamB, 1);

            _teamProviderMock.Setup(x => x.GetTeams()).Returns(teams);
            _schedulerMock.Setup(x => x.CreateSchedule(teams)).Returns(new List<Match> { match });

            _simulatorMock
                .Setup(x => x.Simulate(teamA, teamB))
                .Returns(new MatchResult(2, 1));

            var service = CreateService();

            var result = service.Simulate();

            var standingA = result.Standings.First(s => s.Team.Name == "A");
            var standingB = result.Standings.First(s => s.Team.Name == "B");

            Assert.True(standingA.Points > standingB.Points);
        }

        [Fact]
        public void Simulate_ShouldReturnTop2QualifiedTeams()
        {
            var teams = new List<Team>
            {
                new("A", 50),
                new("B", 50),
                new("C", 50)
            };

            var matches = new List<Match>
            {
                new(teams[0], teams[1], 1),
                new(teams[1], teams[2], 2),
                new(teams[0], teams[2], 3)
            };

            _teamProviderMock.Setup(x => x.GetTeams()).Returns(teams);
            _schedulerMock.Setup(x => x.CreateSchedule(teams)).Returns(matches);

            _simulatorMock
                .Setup(x => x.Simulate(It.IsAny<Team>(), It.IsAny<Team>()))
                .Returns<Team, Team>((home, away) =>
                    home.Name == "A"
                        ? new MatchResult(2, 0)
                        : new MatchResult(0, 1)
                );

            var service = CreateService();

            var result = service.Simulate();

            Assert.Equal(2, result.QualifiedTeams.Count);
        }

        [Fact]
        public void Simulate_ShouldAssignSamePosition_WhenTie()
        {
            var teamA = new Team("A", 50);
            var teamB = new Team("B", 50);

            var teams = new List<Team> { teamA, teamB };

            var match = new Match(teamA, teamB, 1);

            _teamProviderMock.Setup(x => x.GetTeams()).Returns(teams);
            _schedulerMock.Setup(x => x.CreateSchedule(teams)).Returns(new List<Match> { match });

            // ничья
            _simulatorMock
                .Setup(x => x.Simulate(teamA, teamB))
                .Returns(new MatchResult(1, 1));

            var service = CreateService();

            var result = service.Simulate();

            var standings = result.Standings;

            Assert.Equal(standings[0].Position, standings[1].Position);
        }

        [Fact]
        public void Simulate_ShouldGroupMatchesByRounds()
        {
            var teamA = new Team("A", 50);
            var teamB = new Team("B", 50);

            var teams = new List<Team> { teamA, teamB };

            var matches = new List<Match>
            {
                new(teamA, teamB, 1),
                new(teamB, teamA, 2)
            };

            _teamProviderMock.Setup(x => x.GetTeams()).Returns(teams);
            _schedulerMock.Setup(x => x.CreateSchedule(teams)).Returns(matches);

            _simulatorMock
                .Setup(x => x.Simulate(It.IsAny<Team>(), It.IsAny<Team>()))
                .Returns(new MatchResult(1, 0));

            var service = CreateService();

            var result = service.Simulate();

            Assert.Equal(2, result.Rounds.Count);
        }

        [Fact]
        public void Simulate_ShouldCallSimulatorForEachMatch()
        {
            var teams = new List<Team>
            {
                new("A", 50),
                new("B", 50)
            };

            var matches = new List<Match>
            {
                new(teams[0], teams[1], 1),
                new(teams[1], teams[0], 2)
            };

            _teamProviderMock.Setup(x => x.GetTeams()).Returns(teams);
            _schedulerMock.Setup(x => x.CreateSchedule(teams)).Returns(matches);

            _simulatorMock
                .Setup(x => x.Simulate(It.IsAny<Team>(), It.IsAny<Team>()))
                .Returns(new MatchResult(1, 0));

            var service = CreateService();

            service.Simulate();

            _simulatorMock.Verify(
                x => x.Simulate(It.IsAny<Team>(), It.IsAny<Team>()),
                Times.Exactly(matches.Count)
            );
        }
    }
}