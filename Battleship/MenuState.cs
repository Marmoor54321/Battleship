using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class MenuState : IGameState
{
    private Game1 _game;
    private SpriteFont _font;
    private Texture2D _background;
    private Player _player1;
    private Player _player2;
    private bool mReleased = true;

    public MenuState(Game1 game, Player player1, Player player2)
    {
        _game = game;
        _player1 = player1;
        _player2 = player2;
    }

    public void LoadContent()
    {
        _font = _game.Content.Load<SpriteFont>("Fonts/font1");
        _background = _game.Content.Load<Texture2D>("sky");
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        Vector2 titleSize = _font.MeasureString("BATTLESHIP MENU");
        Vector2 playerVsPlayerSize = _font.MeasureString("Player vs Player");
        Vector2 playerVsAISize = _font.MeasureString("Player vs AI");

        Vector2 playerVsPlayerPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - playerVsPlayerSize.X) / 2, 150);
        Vector2 playerVsAIPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - playerVsAISize.X) / 2, 250);

        if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
        {
            if (mouseState.X > playerVsPlayerPosition.X && mouseState.X < playerVsPlayerPosition.X + playerVsPlayerSize.X
                && mouseState.Y > playerVsPlayerPosition.Y && mouseState.Y < playerVsPlayerPosition.Y + playerVsPlayerSize.Y)
            {
                _game.ChangeState(new PlayerVsPlayerState(_game));
            }
            else if (mouseState.X > playerVsAIPosition.X && mouseState.X < playerVsAIPosition.X + playerVsAISize.X
                && mouseState.Y > playerVsAIPosition.Y && mouseState.Y < playerVsAIPosition.Y + playerVsAISize.Y)
            {
                _game.ChangeState(new PlayerVsAISubMenuState(_game));
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

        // Obliczanie pozycji na środku ekranu
        Vector2 titleSize = _font.MeasureString("BATTLESHIP MENU");
        Vector2 playerVsPlayerSize = _font.MeasureString("Player vs Player");
        Vector2 playerVsAISize = _font.MeasureString("Player vs AI");

        // Pozycja tytułu na środku
        Vector2 titlePosition = new Vector2((_game.GraphicsDevice.Viewport.Width - titleSize.X) / 2, 50);
        spriteBatch.DrawString(_font, "BATTLESHIP MENU", titlePosition, Color.White);

        // Pozycja opcji w pionie, z równą odległością
        Vector2 playerVsPlayerPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - playerVsPlayerSize.X) / 2, 150);
        spriteBatch.DrawString(_font, "Player vs Player", playerVsPlayerPosition, Color.Yellow);

        Vector2 playerVsAIPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - playerVsAISize.X) / 2, 250);
        spriteBatch.DrawString(_font, "Player vs AI", playerVsAIPosition, Color.Yellow);

        // Statystyki graczy
        Vector2 statsPosition = new Vector2(50, 350);
        spriteBatch.DrawString(_font, $"{_player1.Name}: Wins: {_player1.Wins}, Hits: {_player1.Hit}", statsPosition, Color.White);

        statsPosition.Y += 30; // Odstęp między statystykami graczy
        spriteBatch.DrawString(_font, $"{_player2.Name}: Wins: {_player2.Wins}, Hits: {_player2.Hit}", statsPosition, Color.White);

        spriteBatch.End();
    }
}