using System;
using System.Collections.Generic;

public class Player
{
    public string Name { get; private set; } // Imię gracza
    public int Wins { get; private set; } // Liczba wygranych meczów
    public int Hit { get; private set; } // Liczba zestrzelonych statków
    public List<GameHistory> GameHistories { get; private set; } // Historia gier
    public AIStrategy AIStrategy { get; private set; }

    public Player(string name, AIStrategy aiStrategy = null)
    {
        Name = name;
        AIStrategy = aiStrategy;
        Wins = 0;
        Hit = 0;
        GameHistories = new List<GameHistory>();
    }

    // Metoda zwiększająca liczbę wygranych meczów
    public void AddWin()
    {
        Wins++;
    }

    // Metoda zwiększająca liczbę zestrzelonych statków
    public void AddHit()
    {
        Hit++;
    }

    // Dodanie historii gry
    public void AddGameHistory(GameHistory history)
    {
        GameHistories.Add(history);
    }

    // Metoda resetująca statystyki gracza
    public void ResetStats()
    {
        Wins = 0;
        Hit = 0;
    }

    public void SetAIStrategy(AIStrategy strategy)
    {
        AIStrategy = strategy;
    }

}