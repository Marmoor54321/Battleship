using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cell
{
    public bool HasShip {  get; set; }
    public bool IsHit {  get; set; }
    public bool IsMiss {  get; set; }

    public Cell()
    {
        HasShip = false;
        IsHit = false;
        IsMiss = false;
    }
}
