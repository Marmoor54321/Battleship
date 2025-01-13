using System;
using Battleship;


public class Achievement
{
    public string AchievementName { get; private set; }
    public bool IsUnlocked { get; set; }

    public Achievement(string achievementName, bool isUnlocked)
    {
        AchievementName = achievementName;
        IsUnlocked = isUnlocked;  
    }
}


public class AchievementWins1 : IPlayerObserver
{
    public void Update(Player player, string operation, int amount)
    {
        if (operation == "Win" && amount == 1)
        {
            foreach (var achievement in player.Achievements)
            {
                if (achievement.AchievementName == "1 win")
                {
                    achievement.IsUnlocked = true;

                }
            }
            foreach (var skin in player.Skins)
            {
                if (skin.SkinName == "PinkShip")
                {
                    skin.IsUnlocked = true;

                }
            }
        }

    }
}

public class AchievementHits10 : IPlayerObserver
{
    public void Update(Player player, string operation, int amount)
    {
        if (operation == "Hit" && amount == 10)
        {
            foreach (var achievement in player.Achievements)
            {
                if (achievement.AchievementName == "10 hits")
                {
                    achievement.IsUnlocked = true;
                }
            }
        }

    }
}
