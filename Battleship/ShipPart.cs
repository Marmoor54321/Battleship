using System;
using System.Collections.Generic;
using System.Linq;
using Battleship;

public class ShipPart : ShipComponent
{
    private bool isHit;

    public ShipPart()
    {
        isHit = false;
    }

    public override void TakeHit()
    {
        isHit = true;
    }

    public override bool IsSunk()
    {
        return isHit;
    }
}