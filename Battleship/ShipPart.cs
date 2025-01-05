using System;
using System.Collections.Generic;
using System.Linq;
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
        Console.WriteLine("A part of the ship has been hit!");
    }

    public override bool IsSunk()
    {
        return isHit;
    }
}