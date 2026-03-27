Welcome to the Football Tournament Simulator!

The application simulates a football tournament with 4 teams, where each team plays against every other team once. The results of the matches are generated randomly, and the final standings are displayed at the end of the tournament.

The current implementation is quite simple and uses fixed teams and a predefined schedule for simplicity.

To support dynamic configuration (custom team count or tournament format),
it would be sufficient to:
- Implement new ITeamProvider and IMatchScheduler
- Extend the API endpoint to accept new parameters
 
No changes to the core simulation logic (MatchSimulator, Standing, Comparers) would be required.

The team strength is represented by a random value between 0 and 100, and the expected number of goals is calculated based on the strength difference between the two teams. Randomness of goals number added using a Poisson-like distribution. 

In a real world application I would add IDs to the Team and Match classes, and use a database or cache to store the data.

In case when 2 teams played absolutely equal they would share the same position. I think this is a fair decision for a sports tournament, but it creates a problem when selecting qualified teams. For now I dont have a solution for this problem, but I would consider adding more tiebreaker rules or initiate a rematch for these teams.

PS: Please do not look at the code of the index.html file, it was AI generated for the convenience of testing and demonstration :)

