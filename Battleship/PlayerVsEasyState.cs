using Battleship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

public class PlayerVsEasyState : IGameState
{
    private Game1 _game;
    private Board board1;
    private Board board2;
    private EasyAI ai;

    private Texture2D cellTexture, hitTexture, missTexture, skyTexture, shipPartTexture;
    private SpriteFont font;

    private bool placingShips = true;
    private int[] shipSizes = { 5, 4, 3, 3, 2 };
    private int currentShipIndex = 0;
    private bool horizontal = true;

    private bool mReleased = true;
    private bool mRightReleased = true;
    private bool turn2 = false;

    const int sec_board_start_x = 352;
    const int sec_board_end_x = 672;

    private double _aiMoveDelay = 1.0; // Opóźnienie AI w sekundach
    private double _timeSinceLastAIMove = 0.0;
    private bool _aiMovePending = true; // Czy AI ma wykonać ruch


    public PlayerVsEasyState(Game1 game)
    {
        _game = game;
        ai = new EasyAI();
    }

    public void LoadContent()
    {
        board1 = new Board(_game.Player1);
        board2 = new Board(_game.PlayerEASY);

        cellTexture = _game.Content.Load<Texture2D>("cell");
        hitTexture = _game.Content.Load<Texture2D>("hit");
        missTexture = _game.Content.Load<Texture2D>("miss");
        shipPartTexture = _game.Content.Load<Texture2D>("shippart");
        skyTexture = _game.Content.Load<Texture2D>("sky");
        font = _game.Content.Load<SpriteFont>("Fonts/font1");

        ai.PlaceFleet(board2); // AI losowo rozmieszcza statki na swojej planszy
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        if (placingShips)
        {
            int x = mouseState.X / 32;
            int y = mouseState.Y / 32;

            if (mouseState.LeftButton == ButtonState.Pressed && mReleased && mouseState.X < 320)
            {
                int size = shipSizes[currentShipIndex];
                if (board1.CanPlaceShip(size, x, y, horizontal))
                {
                    Ship ship = new Ship();
                    for (int i = 0; i < size; i++)
                    {
                        ship.AddPart(new ShipPart());
                    }
                    board1.PlaceShip(ship, x, y, horizontal);
                    currentShipIndex++;
                    if (currentShipIndex >= shipSizes.Length)
                    {
                        placingShips = false;
                    }
                }
                mReleased = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed && mRightReleased)
            {
                horizontal = !horizontal;
                mRightReleased = false;
            }
        }
        else
        {
            if (board1.AreAllShipsSunk())
            {
                Console.WriteLine("Gracz 2 wygrywa!");
                _game.PlayerEASY.AddWin();
                _game.ChangeState(new MenuState(_game, _game.Player1, _game.PlayerEASY));
                return;
            }

            if (board2.AreAllShipsSunk())
            {
                Console.WriteLine("Gracz 1 wygrywa!");
                _game.Player1.AddWin();
                _game.ChangeState(new MenuState(_game, _game.Player1, _game.PlayerEASY));
                return;
            }

            if (!turn2) // Ruch gracza
            {

                if (mouseState.LeftButton == ButtonState.Pressed && mReleased && mouseState.X > sec_board_start_x && mouseState.X < sec_board_end_x && mouseState.Y < 320)
                {
                    int x = (mouseState.X - sec_board_start_x) / 32;
                    int y = mouseState.Y / 32;
                    if (board2.Shoot(x, y))
                    {
                        _game.Player1.AddHit();
                    }
                    else
                    {
                        turn2 = true;
                    }
                    mReleased = false;
                }
            }
            else // Ruch AI
            {
                _timeSinceLastAIMove += gameTime.ElapsedGameTime.TotalSeconds;

                if (_aiMovePending && _timeSinceLastAIMove >= _aiMoveDelay)
                {
                    var (x, y) = ai.MakeMove(board1);
                    if (board1.Shoot(x, y))
                    {
                        _game.PlayerEASY.AddHit();
                        // AI pozostaje w swojej turze, jeśli trafia
                    }
                    else
                    {
                        turn2 = false; // Jeśli chybi, zmienia turę na gracza
                    }

                    _timeSinceLastAIMove = 0.0; // Resetuje licznik czasu dla AI
                    _aiMovePending = false; // Ustawia ruch AI jako zakończony
                }

                // Jeśli ruch AI zakończony, ustaw flagę na nowy ruch w przyszłości
                if (!_aiMovePending && turn2)
                {
                    _aiMovePending = true; // AI będzie gotowe do kolejnego ruchu w przyszłej turze
                }

            }
        }

        if (mouseState.LeftButton == ButtonState.Released)
        {
            mReleased = true;
        }

        if (mouseState.RightButton == ButtonState.Released)
        {
            mRightReleased = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        spriteBatch.Draw(skyTexture, new Vector2(0, 0), Color.White);

        if (placingShips)
        {
            spriteBatch.DrawString(font, "Place your ships on board 1!", new Vector2(10, 330), Color.White);
            spriteBatch.DrawString(font, horizontal ? "Horizontal" : "Vertical", new Vector2(300, 330), Color.Red);
        }

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
