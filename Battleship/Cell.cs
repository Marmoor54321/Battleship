using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cell
{
    public ShipComponent ShipComponent { get; set; }
    public bool IsHit { get; set; }
    public bool IsMiss { get; set; }
    public bool HasShip { get; set; }

    public Cell()
    {
        ShipComponent = null;
        IsHit = false;
        IsMiss = false;
        HasShip = false;
    }
    public void Shoot()
    {
        if (ShipComponent != null && !IsHit)
        {
            ShipComponent.TakeHit();
            IsHit = true;
        }
        else if (!IsMiss)
        {
            IsMiss = true;
        }
    }
}
