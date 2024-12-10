using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Board board;
        Texture2D cellTexture;
        Texture2D hitTexture;
        Texture2D missTexture;

        bool mReleased = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            

            base.Initialize();
            board = new Board();
            board.PlaceShip(2, 2, 4, true);
            board.PlaceShip(5, 5, 3, false);
            board.PlaceShip(0, 0, 2, true);
            board.PlaceShip(7, 8, 1, false);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cellTexture = Content.Load<Texture2D>("cell");
            hitTexture = Content.Load<Texture2D>("hit");
            missTexture = Content.Load<Texture2D>("miss");
        }

        protected override void Update(GameTime gameTime)
        {
            
            MouseState mouseState = Mouse.GetState();
            if(mouseState.LeftButton == ButtonState.Pressed && mouseState.X < 320 && mouseState.Y < 320 && mReleased == true)
            {
                int x = mouseState.X / 32;
                int y = mouseState.Y / 32;
                board.Shoot(x, y);
                mReleased = false;
            }
            else
            {
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
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Cell cell = board.GetCell(x, y);
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
            _spriteBatch.End();

            base.Draw(gameTime);
        
    }
    }
}
