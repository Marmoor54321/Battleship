using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class GameHistoryState : IGameState
{
    private Game1 _game;
    private SpriteFont _font;
    private Texture2D _background;
    private bool mReleased = true;

    public GameHistoryState(Game1 game)
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
            else
            {
                
                Vector2 startPosition = new Vector2(50, 80); 
                for (int i = 0; i < _game.GameHistories.Count; i++)
                {
                    string historyText = $"{_game.GameHistories[i].GameDate}: {_game.GameHistories[i].Player1Name} (Hits: {_game.GameHistories[i].Player1Hits}) vs {_game.GameHistories[i].Player2Name} (Hits: {_game.GameHistories[i].Player2Hits}), Winner: {(_game.GameHistories[i].Player1Won ? _game.GameHistories[i].Player1Name : _game.GameHistories[i].Player2Name)}";
                    Vector2 textSize = _font.MeasureString(historyText);

                    if (mouseState.X > startPosition.X && mouseState.X < startPosition.X + textSize.X &&
                        mouseState.Y > startPosition.Y && mouseState.Y < startPosition.Y + textSize.Y)  
                    {
                        
                        _game.RemoveGameFromHistory(i);
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
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Z))
        {
            var memento = _game.Caretaker.Restore();
            if (memento != null)
            {
                _game.GameHistories = memento.GameHistories;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

        Vector2 MenuSize = _font.MeasureString("Menu");
        Vector2 MenuPosition = new Vector2(700, 450);
        spriteBatch.DrawString(_font, "Menu", MenuPosition, Color.Yellow);

        spriteBatch.DrawString(_font, "Game History", new Vector2(50, 20), Color.Black);
        spriteBatch.DrawString(_font, "Click record to delete. Press Z to undo.", new Vector2(50, 40), Color.Black);


        
        Vector2 startPosition = new Vector2(50, 80);
        foreach (var gameHistory in _game.GameHistories)
        {
            string historyText = $"{gameHistory.GameDate}: {gameHistory.Player1Name} (Hits: {gameHistory.Player1Hits}) vs {gameHistory.Player2Name} (Hits: {gameHistory.Player2Hits}), Winner: {(gameHistory.Player1Won ? gameHistory.Player1Name : gameHistory.Player2Name)}";
            spriteBatch.DrawString(_font, historyText, startPosition, Color.Black);
            startPosition.Y += 30;
        }

        spriteBatch.End();
    }

}
