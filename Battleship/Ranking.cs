using System;

public class Ranking
{
    public string PlayerName { get; private set; }
    public int PlayerWins { get;  set; }

    public Ranking(string playerName, int playerWins)
    {
        PlayerName = playerName;
        PlayerWins = playerWins;
        
    }
}
