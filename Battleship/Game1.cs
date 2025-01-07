using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Battleship
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public class MenuState : IGameState
    {
        private Game1 _game;
        private SpriteFont _font;
        private Texture2D _background;
        private bool mReleased = true;

        public MenuState(Game1 game)
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
                    Console.WriteLine("Player vs AI mode is not implemented yet!");
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

            spriteBatch.End();
        }
    }

    public class PlayerVsPlayerState : IGameState
    {
        private Game1 _game;
        private Board board1;
        private Board board2;
        private Texture2D cellTexture, hitTexture, missTexture, skyTexture, shipPartTexture;
        private SpriteFont font;

        private bool placingShips = true;
        private int[] shipSizes = { 5, 4, 3, 3, 2 };
        private int currentShipIndex = 0;
        private bool horizontal = true;
        private bool placingForBoard2 = false;

        private bool mReleased = true;
        private bool mRightReleased = true;
        private bool turn2 = false;

        const int sec_board_start_x = 352;
        const int sec_board_end_x = 672;

        public PlayerVsPlayerState(Game1 game)
        {
            _game = game;
        }

        public void LoadContent()
        {
            board1 = new Board();
            board2 = new Board();

            cellTexture = _game.Content.Load<Texture2D>("cell");
            hitTexture = _game.Content.Load<Texture2D>("hit");
            missTexture = _game.Content.Load<Texture2D>("miss");
            shipPartTexture = _game.Content.Load<Texture2D>("shippart");
            skyTexture = _game.Content.Load<Texture2D>("sky");
            font = _game.Content.Load<SpriteFont>("Fonts/font1");
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();


            if (placingShips)
            {
                Board currentBoard = placingForBoard2 ? board2 : board1;
                int x, y;

                if (placingForBoard2 && mouseState.X > sec_board_start_x && mouseState.X < sec_board_end_x)
                {
                    x = (mouseState.X - sec_board_start_x) / 32;
                    y = mouseState.Y / 32;
                }
                else if (!placingForBoard2 && mouseState.X < 320)
                {
                    x = mouseState.X / 32;
                    y = mouseState.Y / 32;
                }
                else
                {
                    return;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
                {
                    int size = shipSizes[currentShipIndex];
                    if (currentBoard.CanPlaceShip(size, x, y, horizontal))
                    {
                        Ship ship = new Ship();
                        for (int i = 0; i < size; i++)
                        {
                            ship.AddPart(new ShipPart());
                        }
                        currentBoard.PlaceShip(ship, x, y, horizontal);
                        currentShipIndex++;
                        if (currentShipIndex >= shipSizes.Length)
                        {
                            if (placingForBoard2)
                            {
                                placingShips = false;
                            }
                            else
                            {
                                placingForBoard2 = true;
                                currentShipIndex = 0;
                            }
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
                    _game.ChangeState(new MenuState(_game)); // Powrót do menu
                    return;
                }

                if (board2.AreAllShipsSunk())
                {
                    Console.WriteLine("Gracz 1 wygrywa!");
                    _game.ChangeState(new MenuState(_game)); // Powrót do menu
                    return;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
                {
                    if (mouseState.X < 320 && mouseState.Y < 320 && !turn2)
                    {
                        int x = mouseState.X / 32;
                        int y = mouseState.Y / 32;
                        if (board1.Shoot(x, y))
                        {
                            turn2 = false;
                        }
                        else
                        {
                            turn2 = true;
                        }
                    }
                    else if (mouseState.X > sec_board_start_x && mouseState.X < sec_board_end_x && mouseState.Y < 320 && turn2)
                    {
                        int x = (mouseState.X - sec_board_start_x) / 32;
                        int y = mouseState.Y / 32;
                        if (board2.Shoot(x, y))
                        {
                            turn2 = true;
                        }
                        else
                        {
                            turn2 = false;
                        }
                    }
                    mReleased = false;
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
                spriteBatch.DrawString(font, placingForBoard2 ? "Place your ships on board 2!" : "Place your ships on board 1!", new Vector2(10, 330), Color.White);
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

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IGameState _currentState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ChangeState(new MenuState(this));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            (_currentState as MenuState)?.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
            if (newState is MenuState menuState)
            {
                menuState.LoadContent();
            }
            else if (newState is PlayerVsPlayerState pvpState)
            {
                pvpState.LoadContent();
            }
        }
    }
}
