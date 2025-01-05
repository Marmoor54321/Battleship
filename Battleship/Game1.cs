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

        const int sec_board_start_x = 352;
        const int sec_board_end_x = 672;

        bool mReleased = true;
        bool turn2 = false;
        

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
            Ship ship1 = new Ship();
            ShipPart part1 = new ShipPart();
            ShipPart part2 = new ShipPart();
            ShipPart part3 = new ShipPart();
            ship1.AddPart(part1);
            ship1.AddPart(part2);
            ship1.AddPart(part3);
            board1.PlaceShip(ship1, 0,0, true);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cellTexture = Content.Load<Texture2D>("cell");
            hitTexture = Content.Load<Texture2D>("hit");
            missTexture = Content.Load<Texture2D>("miss");
            skyTexture = Content.Load<Texture2D>("sky");

        }

        protected override void Update(GameTime gameTime)
        {
            
            MouseState mouseState = Mouse.GetState();
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
                    else
                    {
                        _spriteBatch.Draw(cellTexture, position, Color.White);
                    }
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        
    }
    }
}
