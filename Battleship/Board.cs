using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Board
{
    private const int Size = 10;
    private Cell[,] cells;
    private List<Ship> ships;

    public Board()
    {
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

    // Umieszczanie statku na planszy
    public void PlaceShip(Ship ship, int startX, int startY, bool horizontal)
    {
        ships.Add(ship);
        int index = 0;
        foreach (var part in GetParts(ship))
        {
            if (horizontal)
            {
                cells[startX + index, startY].ShipComponent = part;
            }
            else
            {
                cells[startX, startY + index].ShipComponent = part;
            }
            index++;
        }
    }

    

    public List<Ship> GetShips()
    {
        return ships;
    }

    private static List<ShipComponent> GetParts(Ship ship)
    {
        // Zakładamy, że Ship przechowuje swoje części w polu "parts".
        return ship.GetParts();
    }



    // Strzał na planszy
    public bool Shoot(int x, int y)
    {
        if (x < 0 || x >= cells.GetLength(0) || y < 0 || y >= cells.GetLength(1))
        {
            return false;
        }

        var cell = cells[x, y];
        if (cell.ShipComponent != null && !cell.IsHit)
        {
            cell.Shoot(); // Trafienie
            return true;
        }

        cell.IsMiss = true; // Pudło
        return false;
    }

    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }
}