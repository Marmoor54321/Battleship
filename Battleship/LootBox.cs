using System;
using Battleship;
using System.Collections.Generic;
using System.Linq;


public class LootBox
{
    public string LootBoxName { get; private set; }
    public int Price { get; private set; }
    private List<Skin> _lootItems;
    private Random _random;


    public LootBox(string lootBoxName, int price)
    {
        LootBoxName = lootBoxName;
        Price = price;
        _lootItems = new List<Skin>();
        _random = new Random();

    }

    public void AddItem(Skin item)
    {
        _lootItems.Add(item);
    }

    public Skin Open()
    {
        // Oblicz sumę rzadkości
        int totalRarity = _lootItems.Sum(item => item.Rarity);

        // Wylosuj liczbę z zakresu od 0 do totalRarity
        int roll = _random.Next(0, totalRarity);

        // Przechodzimy przez przedmioty i sprawdzamy, który "wypadł"
        int current = 0;
        foreach (var item in _lootItems)
        {
            current += item.Rarity;
            if (roll < current)
            {
                return item;
            }
        }

        // W razie błędu (teoretycznie nie powinno się zdarzyć)
        return null;
    }
}

