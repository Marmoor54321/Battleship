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
        private const string HistoryFilePath = "gameHistory.json"; 
        private const string RankingFilePath = "rankings.json"; 

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

        public Skin SkinDefault { get; set; }
        public Skin Skin1 { get; set; }
        public Skin SkinGRG { get; set; }
        
        public LootBox LootBox1 { get; set; }

        public List<LootBox> LootBoxes { get; set; } = new List<LootBox>();



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameHistories = new List<GameHistory>();
            Rankings = new List<Ranking>();
            Caretaker = new GameHistoryCaretaker();

            LoadGameHistory();
            LoadRankings(); 

        }

        public void AddGameToHistory(string player1Name, string player2Name, bool player1Won, int hitsp1, int hitsp2)
        {
            var history = new GameHistory(player1Name, player2Name, hitsp1, hitsp2, player1Won);
            GameHistories.Add(history);
            Caretaker.Save(new GameHistoryMemento(GameHistories));
            SaveGameHistory();
        }

        public void AddPlayerToRanking(string playerName, int playerWins)
        {
            var ranking = new Ranking(playerName, playerWins);
            if (!Rankings.Any(r => r.PlayerName == playerName))
            {
                Rankings.Add(ranking);
            }
            else
            {
                foreach (var r in Rankings)
                {
                    if (r.PlayerName == playerName)
                    {
                        r.PlayerWins += 1;
                    }
                }
            }
            SaveRankings();
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

                Caretaker.Save(new GameHistoryMemento(GameHistories));

                GameHistories.RemoveAt(index);
                SaveGameHistory(); 

            }
        }

        public void RestoreHistory()
        {
            var memento = Caretaker.Restore();
            if (memento != null)
            {
                GameHistories = memento.GameHistories;
                SaveGameHistory(); 

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
            ChangeState(new PlayerNameInputState(this));
            AchievementWins1 = new Achievement("1 win", false);
            AchievementHits10 = new Achievement("10 hits", false);

            PlayerEASY = new Player("BOB", EasyAI.Instance);
            PlayerEASY2 = new Player("ROB", EasyAI.Instance);

            SkinDefault = new Skin("DefaultShip", "ShipPart", true, true,0);
            Skin1 = new Skin("PinkShip", "ShipPartPink", false, false,50);
            SkinGRG = new Skin("GRGCell", "george", false, false,5);

            LootBox1 = new LootBox("Basic LootBox", 100);
            LootBox1.AddItem(Skin1);
            LootBox1.AddItem(SkinGRG);

            LootBoxes.Add(LootBox1);

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


            if (Player1 != null && !Player1.Skins.Any(r => r.SkinName == SkinDefault.SkinName))
            {
                Player1.Skins.Add(SkinDefault);
            }
            if (Player1 != null && !Player1.Skins.Any(r => r.SkinName == Skin1.SkinName))
            {
                Player1.Skins.Add(Skin1);
            }
            if (Player1 != null && !Player1.Skins.Any(r => r.SkinName == SkinGRG.SkinName))
            {
                Player1.Skins.Add(SkinGRG);
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
            else if (newState is CustomizationState CustomizationState)
            {
                CustomizationState.LoadContent();
            }
            else if (newState is ShopState ShopState)
            {
                ShopState.LoadContent();
            }



        }

    }
}
