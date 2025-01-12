using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace Battleship
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
    public interface IPlayerObserver
    {
        void Update(Player player, string operation, int amount);
    }



    public class Game1 : Game
    {
        private const string HistoryFilePath = "gameHistory.json"; // Ścieżka pliku z historią
        private const string RankingFilePath = "rankings.json"; // Ścieżka pliku z rankingiem

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IGameState _currentState;

        public Player Player1 { get;  set; }
        public Player Player2 { get;  set; }
        public Player PlayerEASY { get; private set; }
        public Player PlayerEASY2 {  get; private set; }

        public Achievement AchievementWins1 {  get; set; }
        public Achievement AchievementHits10 { get; set; }

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

            LoadGameHistory();
            LoadRankings();    // Wczytaj ranking graczy

        }

        public void AddGameToHistory(string player1Name, string player2Name, bool player1Won, int hitsp1, int hitsp2)
        {
            var history = new GameHistory(player1Name, player2Name, hitsp1, hitsp2, player1Won);
            GameHistories.Add(history);
            Caretaker.Save(new GameHistoryMemento(GameHistories));
            SaveGameHistory(); // Automatyczny zapis
        }

        public void AddPlayerToRanking(string playerName, int playerWins)
        {
            var ranking = Rankings.FirstOrDefault(r => r.PlayerName == playerName);
            if (ranking == null)
            {
                ranking = new Ranking(playerName, playerWins);
                Rankings.Add(ranking);
            }
            else
            {
                ranking.PlayerWins += playerWins; // Dodaj wygrane do istniejących
            }

            SaveRankings(); // Zapisz ranking po aktualizacji
        }
        private void SaveRankings()
        {
            try
            {
                var json = JsonSerializer.Serialize(Rankings);
                File.WriteAllText(RankingFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving rankings: {ex.Message}");
            }
        }

        private void LoadRankings()
        {
            try
            {
                if (File.Exists(RankingFilePath))
                {
                    var json = File.ReadAllText(RankingFilePath);
                    Rankings = JsonSerializer.Deserialize<List<Ranking>>(json) ?? new List<Ranking>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading rankings: {ex.Message}");
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
                SaveGameHistory(); // Automatyczny zapis

            }
        }

        public void RestoreHistory()
        {
            var memento = Caretaker.Restore();
            if (memento != null)
            {
                GameHistories = memento.GameHistories;
                SaveGameHistory(); // Automatyczny zapis po przywróceniu

            }
        }
        private void SaveGameHistory()
        {
            try
            {
                var json = JsonSerializer.Serialize(GameHistories);
                File.WriteAllText(HistoryFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game history: {ex.Message}");
            }
        }

        // Wczytanie historii z pliku
        private void LoadGameHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    var json = File.ReadAllText(HistoryFilePath);
                    GameHistories = JsonSerializer.Deserialize<List<GameHistory>>(json) ?? new List<GameHistory>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game history: {ex.Message}");
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            //Player1 = new Player("Player 1");
            //Player2 = new Player("Player 2");
            ChangeState(new PlayerNameInputState(this));
            AchievementWins1 = new Achievement("1 win", false);
            AchievementHits10 = new Achievement("10 hits", false);
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

            if (Player1 != null && !Player1.observers.Any(r => r is AchievementWins1))
            {
                Player1.AddObserver(new AchievementWins1());
            }
            if (Player1 != null && !Player1.observers.Any(r => r is AchievementHits10))
            {
                Player1.AddObserver(new AchievementHits10());
            }

            if (Player1 != null && !Player1.Achievements.Any(r => r.AchievementName == AchievementWins1.AchievementName))
            {
                Player1.Achievements.Add(AchievementWins1);
            }
            if (Player1 != null && !Player1.Achievements.Any(r => r.AchievementName == AchievementHits10.AchievementName))
            {
                Player1.Achievements.Add(AchievementHits10);
            }

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
            else if (newState is AchievementsState AchievemntsState)
            {
                AchievemntsState.LoadContent();
            }



        }

    }
}
