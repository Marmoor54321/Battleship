using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

public class EasyVsEasyState : IGameState
{
    private Game1 _game;
    private Board board1;
    private Board board2;
    private EasyAI ai;

    private Texture2D cellTexture, hitTexture, missTexture, skyTexture, shipPartTexture;
    private SpriteFont font;


    private bool turn2 = false;

    const int sec_board_start_x = 352;

    private double _aiMoveDelay = 1.0; // Opóźnienie AI w sekundach
    private double _timeSinceLastAIMove = 0.0;
    private bool _aiMovePending = true; // Czy AI ma wykonać ruch

    private double _aiMoveDelay2 = 1.0; // Opóźnienie AI w sekundach
    private double _timeSinceLastAIMove2 = 0.0;
    private bool _aiMovePending2 = true; // Czy AI ma wykonać ruch


    public EasyVsEasyState(Game1 game)
    {
        _game = game;
        ai = new EasyAI();
    }

    public void LoadContent()
    {
        board1 = new Board(_game.PlayerEASY);
        board2 = new Board(_game.PlayerEASY2);

        cellTexture = _game.Content.Load<Texture2D>("cell");
        hitTexture = _game.Content.Load<Texture2D>("hit");
        missTexture = _game.Content.Load<Texture2D>("miss");
        shipPartTexture = _game.Content.Load<Texture2D>("shippart");
        skyTexture = _game.Content.Load<Texture2D>("sky");
        font = _game.Content.Load<SpriteFont>("Fonts/font1");

        ai.PlaceFleet(board1);
        ai.PlaceFleet(board2); 
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        
            if (board1.AreAllShipsSunk())
            {
                Console.WriteLine("Gracz 2 wygrywa!");
                _game.PlayerEASY2.AddWin();
            _game.AddGameToHistory(_game.PlayerEASY.Name, _game.PlayerEASY2.Name, false, _game.PlayerEASY.Hit, _game.PlayerEASY2.Hit);
            _game.PlayerEASY.ResetHits();
            _game.PlayerEASY2.ResetHits();
            _game.ChangeState(new MenuState(_game, _game.PlayerEASY, _game.PlayerEASY2));
                return;
            }

            if (board2.AreAllShipsSunk())
            {
                Console.WriteLine("Gracz 1 wygrywa!");
                _game.PlayerEASY.AddWin();
            _game.AddGameToHistory(_game.PlayerEASY.Name, _game.PlayerEASY2.Name, true, _game.PlayerEASY.Hit, _game.PlayerEASY2.Hit);
            _game.PlayerEASY.ResetHits();
            _game.PlayerEASY2.ResetHits();
            _game.ChangeState(new MenuState(_game, _game.PlayerEASY, _game.PlayerEASY2));
                return;
            }

            if (!turn2) // Ruch gracza
            {

                _timeSinceLastAIMove += gameTime.ElapsedGameTime.TotalSeconds;

                if (_aiMovePending && _timeSinceLastAIMove >= _aiMoveDelay)
                {
                    var (x, y) = ai.MakeMove(board2);
                    if (board2.Shoot(x, y))
                    {
                        _game.PlayerEASY.AddHit();
                        // AI pozostaje w swojej turze, jeśli trafia
                    }
                    else
                    {
                        turn2 = true; // Jeśli chybi, zmienia turę na gracza
                    }

                    _timeSinceLastAIMove = 0.0; // Resetuje licznik czasu dla AI
                    _aiMovePending = false; // Ustawia ruch AI jako zakończony
                }

                // Jeśli ruch AI zakończony, ustaw flagę na nowy ruch w przyszłości
                if (!_aiMovePending && !turn2)
                {
                    _aiMovePending = true; // AI będzie gotowe do kolejnego ruchu w przyszłej turze
                }
            }
            else // Ruch AI
            {
                _timeSinceLastAIMove2 += gameTime.ElapsedGameTime.TotalSeconds;

                if (_aiMovePending2 && _timeSinceLastAIMove2 >= _aiMoveDelay2)
                {
                    var (x, y) = ai.MakeMove(board1);
                    if (board1.Shoot(x, y))
                    {
                        _game.PlayerEASY2.AddHit();
                        // AI pozostaje w swojej turze, jeśli trafia
                    }
                    else
                    {
                        turn2 = false; // Jeśli chybi, zmienia turę na gracza
                    }

                    _timeSinceLastAIMove2 = 0.0; // Resetuje licznik czasu dla AI
                    _aiMovePending2 = false; // Ustawia ruch AI jako zakończony
                }

                // Jeśli ruch AI zakończony, ustaw flagę na nowy ruch w przyszłości
                if (!_aiMovePending2 && turn2)
                {
                    _aiMovePending2 = true; // AI będzie gotowe do kolejnego ruchu w przyszłej turze
                }

            }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.Draw(skyTexture, new Vector2(0, 0), Color.White);


        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Cell cell = board1.GetCell(x, y);
                Vector2 position = new Vector2(x * 32, y * 32);
                spriteBatch.Draw(cell.IsHit ? hitTexture : cell.IsMiss ? missTexture : cell.HasShip ? shipPartTexture : cellTexture, position, Color.White);
            }
        }

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Cell cell = board2.GetCell(x, y);
                Vector2 position = new Vector2(x * 32 + sec_board_start_x, y * 32);
                spriteBatch.Draw(cell.IsHit ? hitTexture : cell.IsMiss ? missTexture : cell.HasShip ? shipPartTexture : cellTexture, position, Color.White);
            }
        }
        Vector2 positionText = new Vector2(10, 350);
        foreach (var ship in board1.GetShips())
        {
            int shipLength = ship.GetParts().Count;
            string status = ship.IsSunk() ? "Sunk" : "Not Sunk";
            spriteBatch.DrawString(font, $"Ship (Length: {shipLength}): {status}", positionText, Color.White);
            positionText.Y += 20;
        }

        positionText = new Vector2(370, 350);
        foreach (var ship in board2.GetShips())
        {
            int shipLength = ship.GetParts().Count;
            string status = ship.IsSunk() ? "Sunk" : "Not Sunk";
            spriteBatch.DrawString(font, $"Ship (Length: {shipLength}): {status}", positionText, Color.White);
            positionText.Y += 20;
        }
        spriteBatch.End();
    }
}
