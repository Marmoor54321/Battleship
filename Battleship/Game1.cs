using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IGameState _currentState;

        public Player Player1 { get;  set; }
        public Player Player2 { get;  set; }
        public Player PlayerEASY { get; private set; }
        public Player PlayerEASY2 {  get; private set; }

        public List<GameHistory> GameHistories { get; set; } = new List<GameHistory>();

        public List<Ranking> Rankings { get; set; } = new List<Ranking>();

        public GameHistoryCaretaker Caretaker { get; private set; }



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameHistories = new List<GameHistory>();
            Rankings = new List<Ranking>();
            Caretaker = new GameHistoryCaretaker();
        }
        public void AddGameToHistory(string player1Name, string player2Name, bool player1Won, int hitsp1, int hitsp2)
        {
            var history = new GameHistory(player1Name, player2Name, hitsp1, hitsp2, player1Won);
            GameHistories.Add(history);
            Caretaker.Save(new GameHistoryMemento(GameHistories));
        }

        public void AddPlayerToRanking(string playerName, int playerWins)
        {

            var ranking = new Ranking(playerName, playerWins);

            if(!Rankings.Any(r => r.PlayerName == playerName))
            {
                Rankings.Add(ranking);
            }
            else
            {
                foreach(var r in Rankings)
                {
                    if(r.PlayerName == playerName)
                    {
                        r.PlayerWins = playerWins;
                    }
                }

            }
        }
        public void RemoveGameFromHistory(int index)
        {
            if (index >= 0 && index < GameHistories.Count)
            {
                // Zapisz stan przed modyfikacją
                Caretaker.Save(new GameHistoryMemento(GameHistories));

                // Usuń grę z historii
                GameHistories.RemoveAt(index);
            }
        }

        public void RestoreHistory()
    {
        var memento = Caretaker.Restore();
        if (memento != null)
        {
            GameHistories = memento.GameHistories;
        }
    }


        protected override void Initialize()
        {
            base.Initialize();
            //Player1 = new Player("Player 1");
            //Player2 = new Player("Player 2");
            //base.Initialize();
            ChangeState(new PlayerNameInputState(this));
            PlayerEASY = new Player("BOB", new EasyAI());
            PlayerEASY2 = new Player("ROB", new EasyAI());

            //ChangeState(new MenuState(this, Player1, Player2));
          
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            (_currentState as PlayerNameInputState)?.LoadContent();

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
            else if (newState is PlayerNameInputState nameInputState)
            {
                nameInputState.LoadContent();
            }

            else if (newState is PlayerVsAISubMenuState subMenuState)
            {
                subMenuState.LoadContent();
            }
            else if (newState is PlayerVsEasyState easyState)
            {
                easyState.LoadContent();
            }
            else if (newState is PlayerVsPlayerState pvpState)
            {
                pvpState.LoadContent();
            }
            else if (newState is AIVsAISubMenuState AIState)
            {
                AIState.LoadContent();
            }
            else if (newState is EasyVsEasyState AIEasyState)
            {
                AIEasyState.LoadContent();
            }
            else if (newState is GameHistoryState HistoryState)
            {
                HistoryState.LoadContent();
            }
            else if (newState is RankingState RankingState)
            {
                RankingState.LoadContent();
            }



        }

    }
}
