using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;

public class AchievementsState : IGameState
{
    private Game1 _game;
    private SpriteFont _font;
    private Texture2D _background;
    private bool mReleased = true;

    public AchievementsState(Game1 game)
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

        if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
        {
            // Powrót do menu
            if (mouseState.X > MenuPosition.X && mouseState.X < MenuPosition.X + MenuSize.X
                && mouseState.Y > MenuPosition.Y && mouseState.Y < MenuPosition.Y + MenuSize.Y)
            {
                _game.ChangeState(new MenuState(_game, _game.Player1, _game.Player2));
            }


            mReleased = false;
        }

        if (mouseState.LeftButton == ButtonState.Released)
        {
            mReleased = true;
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

        Vector2 MenuSize = _font.MeasureString("Menu");
        Vector2 MenuPosition = new Vector2(700, 450);
        spriteBatch.DrawString(_font, "Menu", MenuPosition, Color.Yellow);
        spriteBatch.DrawString(_font, "Achievemnts", new Vector2(50, 20), Color.Black);

        Vector2 startPosition = new Vector2(50, 80);
        foreach (var achievement in _game.Player1.Achievements)
        {
            string achievementText = $"{achievement.AchievementName}: {achievement.IsUnlocked}";
            spriteBatch.DrawString(_font, achievementText, startPosition, Color.Black);
            startPosition.Y += 30;
        }



        spriteBatch.End();
    }

}
