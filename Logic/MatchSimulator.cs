using TournamentSimulator.Models;

namespace TournamentSimulator.Logic
{
    public class MatchSimulator : IMatchSimulator
    {
        private readonly Random _random = new();
        private const double AverageMatchGoals = 2.72;
        private const double HomeTeamBonus = 0.1;

        public MatchResult Simulate(Team home, Team away)
        {
            var homeExpected = CalculateExpectedGoals(home.Strength, away.Strength, isHome: true);
            var awayExpected = CalculateExpectedGoals(away.Strength, home.Strength, isHome: false);

            var homeGoals = AddRandomness(homeExpected);
            var awayGoals = AddRandomness(awayExpected);

            return new MatchResult(homeGoals, awayGoals);
        }

        private double CalculateExpectedGoals(int attack, int defense, bool isHome)
        {
            var baseValue = (double)attack / (attack + defense);

            var expected = baseValue * AverageMatchGoals;

            if (isHome)
                expected += HomeTeamBonus;

            return expected;
        }

        private int AddRandomness(double expectedGoals)
        {
            // Generate goals using Poisson distribution

            var threshold = Math.Exp(-expectedGoals);
            var goals = 0;
            var accumulatedRandom = 0.8;

            const int maxIterations = 30;
            int iterations = 0;

            while (true)
            {
                accumulatedRandom *= _random.NextDouble();

                if (accumulatedRandom <= threshold)
                    break;

                goals++;

                if (++iterations > maxIterations)
                    break;
            }

            return goals;
        }
    }
}
