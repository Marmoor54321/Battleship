﻿using System;
using System.Collections.Generic;
using Battleship;

public class Player
{
    public string Name { get; private set; } // Imię gracza
    public int Wins { get; private set; } // Liczba wygranych meczów
    public int Hit { get; private set; } // Liczba zestrzelonych statków
    public List<GameHistory> GameHistories { get; private set; }
    public List<Achievement> Achievements { get; private set; }
    public List<Skin> Skins { get; private set; }
    public AIStrategy AIStrategy { get; private set; }

    public List<IPlayerObserver> observers = new();

    public Player(string name, AIStrategy aiStrategy = null)
    {
        Name = name;
        AIStrategy = aiStrategy;
        Wins = 0;
        Hit = 0;
        GameHistories = new List<GameHistory>();
        Achievements = new List<Achievement>();
        Skins = new List<Skin>();
    }
    public void AddObserver(IPlayerObserver observer)
    {
        observers.Add(observer);
    }


    public void RemoveObserver(IPlayerObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyObservers(string operation, int amount)
    {
        foreach (var observer in observers)
        {
            observer.Update(this, operation, amount);
        }
    }
    // Metoda zwiększająca liczbę wygranych meczów
    public void AddWin()
    {
        Wins++;
        NotifyObservers("Win", Wins);
    }

    // Metoda zwiększająca liczbę zestrzelonych statków
    public void AddHit()
    {
        Hit++;
        NotifyObservers("Hit", Hit);
    }

    // Metoda resetująca statystyki gracza
    public void ResetHits()
    {
        Hit = 0;
    }

    public void SetAIStrategy(AIStrategy strategy)
    {
        AIStrategy = strategy;
    }

    

    public GameHistoryMemento SaveHistoryToMemento()
    {
        return new GameHistoryMemento(GameHistories);
    }

    public void RestoreHistoryFromMemento(GameHistoryMemento memento)
    {
        if (memento != null)
        {
            GameHistories = memento.GameHistories;
        }
    }

    public string ReturnEquippedSkinPath ()
    {
        foreach (var skin in Skins)
        {
            if(skin.IsEquipped == true)
            {
                return skin.SkinPath;
            }
        }
        return "ShipPart.png";
    }

    
}