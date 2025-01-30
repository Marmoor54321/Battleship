using System;
using System.Collections.Generic;

public class GameHistoryMemento
{
    public List<GameHistory> GameHistories { get; private set; }

    public GameHistoryMemento(List<GameHistory> histories)
    {
        GameHistories = new List<GameHistory>(histories);
    }
}
