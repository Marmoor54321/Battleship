using Microsoft.Xna.Framework.Graphics;
using System;

public class Skin
{
    public string SkinName { get; private set; }
    public string SkinPath { get; private set; }
    public bool IsUnlocked { get; set; }
    public bool IsEquipped { get; set; }
    public int Rarity { get; set; }
    public Skin(string skinName, string skinPath, bool isUnlocked, bool isEquipped, int rarity)
    {
        SkinName = skinName;
        SkinPath = skinPath;
        IsUnlocked = isUnlocked;
        IsEquipped = isEquipped;
        Rarity = rarity;
    }
}
