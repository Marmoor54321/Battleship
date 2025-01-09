using System;

public class GameHistory
{
    public DateTime GameDate { get; private set; } // Data gry
    public string OpponentName { get; private set; } // Imię przeciwnika
    public bool IsWin { get; private set; } // Czy gracz wygrał grę
    public int Hits { get; private set; } // Liczba statków zestrzelonych w grze

    public GameHistory(string opponentName, bool isWin, int hits)
    {
        GameDate = DateTime.Now;
        OpponentName = opponentName;
        IsWin = isWin;
        Hits = hits;
    }
}
