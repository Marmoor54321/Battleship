using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Board board1;
        Board board2;
        Texture2D cellTexture;
        Texture2D hitTexture;
        Texture2D missTexture;
        Texture2D skyTexture;
        Texture2D shipPartTexture;

        private bool placingShips = true;
        private int[] shipSizes = { 5, 4, 3, 3, 2 };
        private int currentShipIndex = 0;
        private bool horizontal = true;

        const int sec_board_start_x = 352;
        const int sec_board_end_x = 672;

        bool mReleased = true;
        bool turn2 = false;

        private SpriteFont font;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            

            base.Initialize();
            board1 = new Board();
            board2 = new Board();
       

            //board1.PlaceFleetRandom();

            
            //board2.PlaceFleetRandom();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cellTexture = Content.Load<Texture2D>("cell");
            hitTexture = Content.Load<Texture2D>("hit");
            missTexture = Content.Load<Texture2D>("miss");
            shipPartTexture = Content.Load<Texture2D>("shippart");
            skyTexture = Content.Load<Texture2D>("sky");
            font = Content.Load<SpriteFont>("Fonts/font1");
        }

        protected override void Update(GameTime gameTime)
        {


            
            MouseState mouseState = Mouse.GetState();

            if (placingShips)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && mReleased)
                {
                    int x = mouseState.X / 32;
                    int y = mouseState.Y / 32;

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
                if (mouseState.RightButton == ButtonState.Pressed && mReleased)
                {
                    horizontal = !horizontal;
                    mReleased = false;
                }
            }
            else
            {
                
                base.Update(gameTime);
            }

            if (mouseState.LeftButton == ButtonState.Released && mouseState.RightButton == ButtonState.Released)
            {
                mReleased = true;
            }

        
            if(mouseState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                if (mouseState.X < 320 && mouseState.Y < 320 && !turn2)
                {
                    int x = mouseState.X / 32;
                    int y = mouseState.Y / 32;
                    if(board1.Shoot(x, y)==true)
                    {
                        turn2 = false;
                    }
                    
                    else
                    turn2 = true;
                }
                else if(mouseState.X > 352 && mouseState.X < sec_board_end_x && mouseState.Y < 320 && turn2)
                {
                    int x = (mouseState.X-sec_board_start_x) / 32;
                    int y = mouseState.Y / 32;
                    if(board2.Shoot(x, y)==true)
                    {
                        turn2 = true;
                    }
                    else
                    turn2 = false;

                }
                mReleased = false;
            }
           
                
            

            if(mouseState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }


           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(skyTexture, new Vector2(0, 0), Color.White);
            if (placingShips)
            {
                _spriteBatch.DrawString(font, "Place your ships!", new Vector2(10, 340), Color.White);
            }
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Cell cell = board1.GetCell(x, y);
                    Vector2 position = new Vector2(x * 32, y * 32);
                    if (cell.IsHit)
                    {
                        _spriteBatch.Draw(hitTexture, position, Color.White);
                    }
                    else if (cell.IsMiss)
                    {
                        _spriteBatch.Draw(missTexture, position, Color.White);
                    }
                    else if(cell.HasShip)
                    {
                        _spriteBatch.Draw(shipPartTexture, position, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(cellTexture, position, Color.White);
                    }
                }
            }

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Cell cell = board2.GetCell(x, y);
                    Vector2 position = new Vector2(x * 32 + sec_board_start_x, y * 32);
                    if (cell.IsHit)
                    {
                        _spriteBatch.Draw(hitTexture, position, Color.White);
                    }
                    else if (cell.IsMiss)
                    {
                        _spriteBatch.Draw(missTexture, position, Color.White);
                    }
                    else if (cell.HasShip)
                    {
                        _spriteBatch.Draw(shipPartTexture, position, Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(cellTexture, position, Color.White);
                    }
                }
            }

            Vector2 positionText = new Vector2(10, 350);
            foreach (var ship in board1.GetShips())
            {
                string status = ship.IsSunk() ? "Sunk" : "Not Sunk";
                _spriteBatch.DrawString(font, $"Ship: {status}", positionText, Color.White);
                positionText.Y += 20;
            }

            positionText = new Vector2(370, 350);
            foreach (var ship in board2.GetShips())
            {
                string status = ship.IsSunk() ? "Sunk" : "Not Sunk";
                _spriteBatch.DrawString(font, $"Ship: {status}", positionText, Color.White);
                positionText.Y += 20;
            }

            _spriteBatch.End();

            base.Draw(gameTime);

        }

        
    }
}
