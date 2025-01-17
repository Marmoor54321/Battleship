using System;

public class EasyAI : AIStrategy
{
    private static EasyAI _instance;
    private static readonly object _lock = new object();

    private Random _random;

    
    private EasyAI()
    {
        _random = new Random();
    }

    
    public static EasyAI Instance
    {
        get
        {
            
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new EasyAI();
                }
                return _instance;
            }
        }
    }

    
    public void PlaceFleet(Board board)
    {
        board.PlaceFleetRandom();
    }

    
    public (int x, int y) MakeMove(Board opponentBoard)
    {
        int x, y;
        do
        {
            x = _random.Next(10); 
            y = _random.Next(10); 
        }
        while (opponentBoard.GetCell(x, y).IsHit || opponentBoard.GetCell(x, y).IsMiss);

        return (x, y);
    }
}
