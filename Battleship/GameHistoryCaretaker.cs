using System;
using System.Collections.Generic;

public class GameHistoryCaretaker
{
    private Stack<GameHistoryMemento> _mementos = new Stack<GameHistoryMemento>();

    public void Save(GameHistoryMemento memento)
    {
        _mementos.Push(memento);
    }

    public GameHistoryMemento Restore()
    {
        return _mementos.Count > 0 ? _mementos.Pop() : null;
    }
}