using System;

public class GameHistory
{
    public DateTime GameDate { get; private set; } // Data gry
    public string OpponentName { get; private set; } // Imię przeciwnika
    public bool IsWin { get; private set; } // Czy gracz wygrał grę
    public int ShipsSunk { get; private set; } // Liczba statków zestrzelonych w grze

    public GameHistory(string opponentName, bool isWin, int shipsSunk)
    {
        GameDate = DateTime.Now;
        OpponentName = opponentName;
        IsWin = isWin;
        ShipsSunk = shipsSunk;
    }

    public override string ToString()
    {
        return $"Data: {GameDate}, Przeciwnik: {OpponentName}, Wynik: {(IsWin ? "Wygrana" : "Przegrana")}, Zestrzelone statki: {ShipsSunk}";
    }
}
