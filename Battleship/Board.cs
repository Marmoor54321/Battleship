using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship;


public class Board
{
    private const int Size = 10;
    private Cell[,] cells;
    private List<Ship> ships;
    public Player Owner { get; private set; }

    public Board(Player owner)
    {
        Owner = owner;
        cells = new Cell[Size, Size];
        ships = new List<Ship>();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                cells[x, y] = new Cell();
            }
        }
    }

    public void PlaceShip(Ship ship, int startX, int startY, bool horizontal)
    {
        ships.Add(ship);
        int index = 0;
        foreach (var part in GetParts(ship))
        {
            if (horizontal)
            {
                cells[startX + index, startY].ShipComponent = part;
                cells[startX + index, startY].HasShip = true;
            }
            else
            {
                cells[startX, startY + index].ShipComponent = part;
                cells[startX, startY + index].HasShip = true;
            }
            index++;
        }
    }
    public bool AreAllShipsSunk()
    {
        return ships.All(ship => ship.IsSunk());
    }
    public void PlaceFleetRandom()
    {
        int[] shipSizes = { 5, 4, 3, 3, 2 };
        Random rand = new Random();

        foreach (int size in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                int startX = rand.Next(Size);
                int startY = rand.Next(Size);
                bool horizontal = rand.Next(2) == 0;
                if (CanPlaceShip(size, startX, startY, horizontal))
                {
                    Ship ship = new Ship();
                    for (int i = 0; i < size; i++)
                    {
                        ship.AddPart(new ShipPart());
                    }
                    PlaceShip(ship, startX, startY, horizontal);
                    placed = true;
                }
            }
        }
    }

    public bool CanPlaceShip(int size, int startX, int startY, bool horizontal)
    {
        // Sprawdzanie, czy statek mieści się na planszy
        if (horizontal)
        {
            if (startX + size > Size) return false;
        }
        else
        {
            if (startY + size > Size) return false;
        }

        // Sprawdzanie, czy wszystkie komórki statku i ich sąsiedzi są wolne
        for (int i = 0; i < size; i++)
        {
            int x = horizontal ? startX + i : startX;
            int y = horizontal ? startY : startY + i;

            if (x < 0 || x >= Size || y < 0 || y >= Size || cells[x, y].HasShip)
            {
                return false; 
            }

            // Sprawdzanie sąsiadujących komórek (również po rogach)
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int neighborX = x + dx;
                    int neighborY = y + dy;

                    if (neighborX >= 0 && neighborX < Size && neighborY >= 0 && neighborY < Size)
                    {
                        if (cells[neighborX, neighborY].HasShip)
                        {
                            return false; 
                        }
                    }
                }
            }
        }

        return true; 
    }

    public List<Ship> GetShips()
    {
        return ships;
    }

    private static List<ShipComponent> GetParts(Ship ship)
    {
        return ship.GetParts();
    }

public bool Shoot(int x, int y)
{
    if (x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1))
    {
        return false; 
    }

    var cell = cells[x, y];
    if (cell.IsHit || cell.IsMiss)
    {
        return true; 
    }

    if (cell.ShipComponent != null)
    {
        cell.Shoot();
        return true; 
    }

    cell.IsMiss = true; 
    return false;
}

    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }
}