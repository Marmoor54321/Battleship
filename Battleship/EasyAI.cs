using System;


public class EasyAI : AIStrategy
{
    private Random _random = new Random();

    public void PlaceFleet(Board board)
    {
        board.PlaceFleetRandom();
    }

    public (int x, int y) MakeMove(Board opponentBoard)
    {
        int x, y;
        do
        {
            x = _random.Next(10); // Random x coordinate
            y = _random.Next(10); // Random y coordinate
        }
        while (opponentBoard.GetCell(x, y).IsHit || opponentBoard.GetCell(x, y).IsMiss);

        return (x, y);
    }
}