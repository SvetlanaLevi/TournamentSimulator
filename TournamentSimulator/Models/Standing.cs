namespace TournamentSimulator.Models
{
    public class Standing(Team team)
    {
        public int Position { get; set; }
        public Team Team { get; } = team;

        public int Played { get; private set; }  
        public int Wins { get; private set; }
        public int Draws { get; private set; }
        public int Losses { get; private set; }

        public int GoalsFor { get; private set; }
        public int GoalsAgainst { get; private set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int Points { get; private set; }

        public void ApplyMatch(int goalsFor, int goalsAgainst)
        {
            Played++;
            GoalsFor += goalsFor;
            GoalsAgainst += goalsAgainst;

            if (goalsFor > goalsAgainst)
            {
                Wins++;
                Points += 3;
            }
            else if (goalsFor == goalsAgainst)
            {
                Draws++;
                Points += 1;
            }
            else
            {
                Losses++;
            }
        }
    }
}
