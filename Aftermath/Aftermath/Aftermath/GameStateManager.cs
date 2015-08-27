using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Aftermath
{
    class GameStateManager
    {
        #region Variables
        private readonly ContentManager _content;
        private Texture2D _asteroidTexture, _missilesTexture, _shieldGenTexture;
        private Rectangle _shieldGenBB, _asteroidBB, _missilesBB;

        #region GameStates
        private const int _mainMenu = 0;
        public int MainMenu
        {
            get { return _mainMenu; }
        }
        private const int _gameActive = 1;
        public int GameActive
        {
            get { return _gameActive; }
        }
        private const int _endOfLevelScreen = 2;
        public int EndOfLevelScreen
        {
            get { return _endOfLevelScreen; }
        }
        private const int _upgradeMenu = 3;
        public int UpgradeMenu
        {
            get { return _upgradeMenu; }
        }
        private const int _gameOver = 4;
        public int GameOver
        {
            get { return _gameOver; }
        }
        private const int _highScore = 5;
        public int HighScore
        {
            get { return _highScore; }
        }
        private const int _gamePause = 6;
        public int GamePause
        {
            get { return _gamePause; }
        }
        #endregion
        private int _gameStateID = 0;
        public int GameStateID
        {
            get { return _gameStateID; }
            set { _gameStateID = value; }
        }
        #endregion
        public GameStateManager(Game game)
		{ 
            _content = game.Content;
        }
        public void LoadContent()
        {
            _shieldGenTexture = _content.Load<Texture2D>(@"Textures/Sprites/shieldgen");
            _shieldGenBB = new Rectangle(100, 200, (int)(_shieldGenTexture.Width / 5f), (int)(_shieldGenTexture.Height / 5f));
            _asteroidTexture = _content.Load<Texture2D>(@"Textures/Sprites/asteroid");
            _asteroidBB = new Rectangle(100,250, (int)(_asteroidTexture.Width / 1.5f), (int)(_asteroidTexture.Height / 1.5f));
            _missilesTexture = _content.Load<Texture2D>(@"Textures/Sprites/missile");
            _missilesBB = new Rectangle(100, 300, (int)(_missilesTexture.Width / 2f), (int)(_missilesTexture.Height / 2f));
        }
        public void drawGeneratorForEndOfLevel(SpriteFont gameFont, GraphicsDevice graphics, GameStateManager gameState, SpriteBatch spriteBatch, int numberOfGeneratorsAlive)
        {
                spriteBatch.DrawString(gameFont, numberOfGeneratorsAlive.ToString(), new Vector2(75, 200), Color.White);
                spriteBatch.Draw(_shieldGenTexture, _shieldGenBB, Color.White);
                numberOfGeneratorsAlive--;
               
                        
        }
        public void drawAsteroidsForEndOfLevel(SpriteFont gameFont, GraphicsDevice graphics, GameStateManager gameState, SpriteBatch spriteBatch, int asteroidsKilled)
        {
                spriteBatch.DrawString(gameFont, asteroidsKilled.ToString(), new Vector2(75, 250), Color.White);
                spriteBatch.Draw(_asteroidTexture, _asteroidBB, Color.White);
                asteroidsKilled--;
                asteroidsKilled = asteroidsKilled / 5;
               
        }
        public void drawMissilesForEndOfLevel(SpriteFont gameFont, GraphicsDevice graphics, GameStateManager gameState, SpriteBatch spriteBatch, int missilesRemaining)
        {
            spriteBatch.DrawString(gameFont, missilesRemaining.ToString(), new Vector2(75, 300), Color.White);
            spriteBatch.Draw(_missilesTexture, _missilesBB, Color.White);
            missilesRemaining--;
            missilesRemaining = missilesRemaining / 10;
          
        }
    }
}
