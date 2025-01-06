using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Ship : ShipComponent
{
    private List<ShipComponent> parts;


    public Ship()
    {
        parts = new List<ShipComponent>();
    }

    // Dodanie części do statku
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
            Console.WriteLine("Ship is already sunk!");
            return;
        }

        // Uderzenie w pierwszą nienaruszoną część statku
        foreach (var part in parts)
        {
            if (!part.IsSunk())
            {
                part.TakeHit();
                Console.WriteLine("A part of the ship has been hit!");
                break;
            }
        }
    }

    public override bool IsSunk()
    {
        // Statek jest zatopiony, jeśli wszystkie jego części są zatopione
        return parts.All(part => part.IsSunk());
    }
  

}