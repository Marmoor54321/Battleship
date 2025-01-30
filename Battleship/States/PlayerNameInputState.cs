using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class PlayerNameInputState : IGameState
{
    private Game1 _game;
    private SpriteFont _font;
    private string _player1Name = "";
    private string _player2Name = "";
    private bool _isPlayer1 = true;
    private KeyboardState _previousKeyboardState;


    public PlayerNameInputState(Game1 game)
    {
        _game = game;
        _previousKeyboardState = Keyboard.GetState(); 
    }

    public void LoadContent()
    {
        _font = _game.Content.Load<SpriteFont>("Fonts/font1");
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();

        foreach (var key in keyboardState.GetPressedKeys())
        {
            
            if (_previousKeyboardState.IsKeyUp(key))
            {
                if (key == Keys.Back)
                {
                    if (_isPlayer1 && _player1Name.Length > 0)
                        _player1Name = _player1Name.Substring(0, _player1Name.Length - 1);
                    else if (!_isPlayer1 && _player2Name.Length > 0)
                        _player2Name = _player2Name.Substring(0, _player2Name.Length - 1);
                }
                else if (key == Keys.Enter)
                {
                    if (_isPlayer1)
                        _isPlayer1 = false; 
                    else
                    {
                        
                        _game.Player1 = new Player(_player1Name);
                        _game.Player2 = new Player(_player2Name);
                        _game.ChangeState(new MenuState(_game, _game.Player1, _game.Player2));
                    }
                }
                else
                {
                    string keyString = key.ToString();
                    if (keyString.Length == 1 && char.IsLetterOrDigit(keyString[0]))
                    {
                        if (_isPlayer1)
                            _player1Name += keyString;
                        else
                            _player2Name += keyString;
                    }
                }
            }
        }

       
        _previousKeyboardState = keyboardState;
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        string prompt = _isPlayer1 ? "Enter Player 1 Name:" : "Enter Player 2 Name:";
        string input = _isPlayer1 ? _player1Name : _player2Name;

        Vector2 promptPosition = new Vector2(50, 100);
        Vector2 inputPosition = new Vector2(50, 150);

        spriteBatch.DrawString(_font, prompt, promptPosition, Color.White);
        spriteBatch.DrawString(_font, input, inputPosition, Color.Yellow);

        spriteBatch.End();
    }

}
