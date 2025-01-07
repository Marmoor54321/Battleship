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
        for (int i = 0; i < size; i++)
        {
            int x = horizontal ? startX + i : startX;
            int y = horizontal ? startY : startY + i;
            if (x >= Size || y >= Size || cells[x, y].ShipComponent != null)
            {
                return false;
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
        return false; // Wyjście, jeśli strzał jest poza planszą.
    }

    var cell = cells[x, y];
    if (cell.IsHit || cell.IsMiss)
    {
        return true; // Pole już trafione lub oznaczone jako chybione.
    }

    if (cell.ShipComponent != null)
    {
        cell.Shoot();
        return true; // Trafiono statek.
    }

    cell.IsMiss = true; // Strzał chybiony.
    return false;
}

    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }
}