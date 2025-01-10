using System;

public class GameHistory
{
    public DateTime GameDate { get; private set; }
    public string Player1Name { get; private set; }
    public string Player2Name { get; private set; }
    public int Player1Hits { get; private set; }
    public int Player2Hits { get; private set; }
    public bool Player1Won { get; private set; }

    public GameHistory(string player1Name, string player2Name, int player1Hits, int player2Hits, bool player1Won)
    {
        GameDate = DateTime.Now;
        Player1Name = player1Name;
        Player2Name = player2Name;
        Player1Hits = player1Hits;
        Player2Hits = player2Hits;
        Player1Won = player1Won;
    }
}