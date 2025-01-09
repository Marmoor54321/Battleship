using System;

public class GameHistory
{
    public DateTime GameDate { get; private set; }
    public string OpponentName { get; private set; }
    public bool IsWin { get; private set; }
    public int Hits { get; private set; }

    public GameHistory(string opponentName, bool isWin, int hits)
    {
        GameDate = DateTime.Now;
        OpponentName = opponentName;
        IsWin = isWin;
        Hits = hits;
    }
}