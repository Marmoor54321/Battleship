using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Position
{
    private int x { get; set; }
    private int y { get; set; }

    public Position()
    {
        x = 1;
        y = 1;
    }

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int getx()
    {
        return x;
    }

    public int gety()
    {
        return y;
    }

    public string toString()
    {
        return $"({x},{y})";
    }
}