using System;

public interface AIStrategy
{

    void PlaceFleet(Board board);
    (int x, int y) MakeMove(Board opponentBoard);
}