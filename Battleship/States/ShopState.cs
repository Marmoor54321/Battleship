using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;

public class ShopState : IGameState
{
    private Game1 _game;
    private SpriteFont _font;
    private Texture2D _background;
    private bool mReleased = true;
    private string _message = ""; // Wiadomość dla gracza (np. wynik otwierania skrzynki)

    public ShopState(Game1 game)
    {
        _game = game;
    }

    public void LoadContent()
    {
        _font = _game.Content.Load<SpriteFont>("Fonts/font1");
        _background = _game.Content.Load<Texture2D>("sky");
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        Vector2 MenuSize = _font.MeasureString("Menu");
        Vector2 MenuPosition = new Vector2(700, 450);

        Vector2 LootBoxSize = _font.MeasureString("Name: " + _game.LootBox1.LootBoxName + "  Price: " + _game.LootBox1.Price);
        Vector2 LootBoxPosition = new Vector2(50, 50);

        if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
        {
            // Powrót do menu
            if (mouseState.X > MenuPosition.X && mouseState.X < MenuPosition.X + MenuSize.X
                && mouseState.Y > MenuPosition.Y && mouseState.Y < MenuPosition.Y + MenuSize.Y)
            {
                _game.ChangeState(new MenuState(_game, _game.Player1, _game.Player2));
            }

            else
            {

                Vector2 startPosition = new Vector2(50, 80);
                for (int i = 0; i < _game.LootBoxes.Count; i++)
                {
                    string boxText = $"Name: {_game.LootBoxes[i].LootBoxName} Price: {_game.LootBoxes[i].Price}";
                    Vector2 textSize = _font.MeasureString(boxText);

                    if (mouseState.X > startPosition.X && mouseState.X < startPosition.X + textSize.X &&
                        mouseState.Y > startPosition.Y && mouseState.Y < startPosition.Y + textSize.Y)
                    {

                        OpenLootBox(_game.LootBoxes[i]);
                        break;
                    }

                    startPosition.Y += 30;
                }

               
            }
            mReleased = false;
        }
        
        if (mouseState.LeftButton == ButtonState.Released)
        {
            mReleased = true;
        }
    }

    private void OpenLootBox(LootBox box)
    {
        if (_game.Player1.Coins >= box.Price) // Sprawdzamy, czy gracz ma wystarczająco monet
        {
            _game.Player1.AddCoins(-box.Price); // Odejmujemy koszt skrzynki
            Skin unlockedSkin = box.Open(); // Otwieramy skrzynkę

            if (unlockedSkin != null)
            {
                unlockedSkin.IsUnlocked = true; // Odblokowujemy wylosowanego skina
                if (!_game.Player1.Skins.Any(s => s.SkinName == unlockedSkin.SkinName))
                {
                    _game.Player1.Skins.Add(unlockedSkin); // Dodajemy skina do kolekcji gracza, jeśli jeszcze go nie ma
                }

                _message = $"You unlocked: {unlockedSkin.SkinName}"; // Wyświetlamy komunikat o odblokowaniu skina
            }
            else
            {
                _message = "Error: No skin was unlocked!";
            }
        }
        else
        {
            _message = "Not enough coins to open this loot box!"; // Brak wystarczających monet
        }
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

        Vector2 MenuSize = _font.MeasureString("Menu");
        Vector2 MenuPosition = new Vector2(700, 450);
        Vector2 CoinSize = _font.MeasureString("Coins: ");
        spriteBatch.DrawString(_font, "Menu", MenuPosition, Color.Yellow);

        Vector2 CoinPosition = new Vector2(10, 10);
        spriteBatch.DrawString(_font, "Coins:" + _game.Player1.Coins, CoinPosition, Color.Yellow);

        spriteBatch.DrawString(_font, "LootBoxes:", new Vector2(80, 20), Color.Black);

        Vector2 startPosition = new Vector2(80, 80);
        foreach (var LootBox in _game.LootBoxes)
        {
            string boxText = $"Name: {LootBox.LootBoxName} Price: {LootBox.Price}";
            spriteBatch.DrawString(_font, boxText, startPosition, Color.Black);
            startPosition.Y += 30;
        }

        // Wyświetlamy wiadomość dla gracza
        if (!string.IsNullOrEmpty(_message))
        {
            spriteBatch.DrawString(_font, _message, new Vector2 (400, 100), Color.Red);
        }

        spriteBatch.End();
    }
}
