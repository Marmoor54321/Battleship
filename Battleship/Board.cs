using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Board
{
    private const int Size = 10;
    private Cell[,] cells;
    public Board()
    {
        cells = new Cell[Size, Size];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                cells[x, y] = new Cell();
            }
        }
    }


    public void PlaceShip(int x, int y, int length, bool horizontal)
    {
        if (horizontal)
        {
            for (int i = 0; i < length; i++)
            {
                cells[x + i, y].HasShip = true;
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                cells[x, y + i].HasShip = true;
            }
        }
    }

    public bool Shoot(int x, int y)
    {
        if (cells[x, y].HasShip)
        {
            cells[x, y].IsHit = true;
            return true;
        }
        cells[x, y].IsMiss = true;
        return false;
    }


    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }
}
