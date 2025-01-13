using System;

public class EasyAI : AIStrategy
{
    // Pole statyczne przechowujące jedyną instancję klasy
    private static EasyAI _instance;

    // Obiekt do synchronizacji dla środowiska wielowątkowego
    private static readonly object _lock = new object();

    // Pole random dla generowania losowych ruchów
    private Random _random;

    // Prywatny konstruktor
    private EasyAI()
    {
        _random = new Random();
    }

    // Publiczna właściwość dostępu do instancji Singletona
    public static EasyAI Instance
    {
        get
        {
            // Synchronizacja dostępu w środowisku wielowątkowym
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

    // Implementacja metody PlaceFleet z interfejsu AIStrategy
    public void PlaceFleet(Board board)
    {
        board.PlaceFleetRandom();
    }

    // Implementacja metody MakeMove z interfejsu AIStrategy
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
