using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
public class AIVsAISubMenuState : IGameState
{
    private Game1 _game;
    private SpriteFont _font;
    private Texture2D _background;
    private bool mReleased = true;

    public AIVsAISubMenuState(Game1 game)
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

        Vector2 easySize = _font.MeasureString("Easy");
        Vector2 hardSize = _font.MeasureString("Hard");

        Vector2 easyPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - easySize.X) / 2, 150);
        Vector2 hardPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - hardSize.X) / 2, 250);

        if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
        {
            if (mouseState.X > easyPosition.X && mouseState.X < easyPosition.X + easySize.X
                && mouseState.Y > easyPosition.Y && mouseState.Y < easyPosition.Y + easySize.Y)
            {
                _game.ChangeState(new EasyVsEasyState(_game));
            }
            else if (mouseState.X > hardPosition.X && mouseState.X < hardPosition.X + hardSize.X
                && mouseState.Y > hardPosition.Y && mouseState.Y < hardPosition.Y + hardSize.Y)
            {
                
                // tu bedzie hard mode 
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

        Vector2 easySize = _font.MeasureString("Easy");
        Vector2 hardSize = _font.MeasureString("Hard");

        Vector2 easyPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - easySize.X) / 2, 150);
        spriteBatch.DrawString(_font, "Easy", easyPosition, Color.Yellow);

        Vector2 hardPosition = new Vector2((_game.GraphicsDevice.Viewport.Width - hardSize.X) / 2, 250);
        spriteBatch.DrawString(_font, "Hard", hardPosition, Color.Yellow);

        spriteBatch.End();
    }
}
