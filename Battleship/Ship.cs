using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship;

public class Ship : ShipComponent
{
    private List<ShipComponent> parts;

    public Ship()
    {
        parts = new List<ShipComponent>();
    }

    public void AddPart(ShipComponent part)
    {
        parts.Add(part);
    }

    public List<ShipComponent> GetParts()
    {
        return parts;
    }

    public override void TakeHit()
    {
        if (IsSunk())
        {
            return;
        }

        foreach (var part in parts)
        {
            if (!part.IsSunk())
            {
                part.TakeHit();
                break;
            }
        }
    }

    public override bool IsSunk()
    {
        return parts.All(part => part.IsSunk());
    }
}

