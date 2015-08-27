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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly List<Asteroids> _asteroids = new List<Asteroids>();
        private readonly List<Missiles> _missiles = new List<Missiles>();
        private readonly List<Explosions> _explosions = new List<Explosions>();

        private Texture2D _hq, _newGame, _exitGame, _logo, _shieldGen, _hqShields, _upgrades, _begin, _highscore, _nextWave, _resume, _generator, _shields, _missile, _bomb;
        private Rectangle _hqBB, _mouseBB, _newGameBB, _exitGameBB, _logoBB, _hqShieldsBB, _upgradesBB, _beginBB, _highscoreBB, _nextWaveBB, _resumeBB, _generatorBB, _shieldsBB, _missileBB, _bombBB;

        public SpriteFont _gamefont;
        private ShieldGenerator _shieldGeneratorA, _shieldGeneratorB, _shieldGeneratorC, _shieldGeneratorD;
        private Hq _headquarters;
        private HqShields _headquartersShields;
        public MouseState ms, msPrevious;
        public KeyboardState ks;
        private int spawnAsteroid = 0;
        private Boolean _debugMode = false;
        private DrawLine _drawLine;
        private int _missileStockPile;
        private int _wave = 1;
        private int _score = 0;
        private int _missileMax = 20;
        private int _shieldGensAlive = 0;
        private int _asteroidsRemainingToSpawn;
        private int _asteroidsKilled;
        private int _bombsAvailable = 0;
        private int _bonusShieldGen;
        private int _bonusAsteroidsKilled;
        private int _bonusMissilesStockPile; 
        private GameStateManager _gameState;

        #endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
    
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }
        protected override void Initialize()
        {
            base.Initialize();
            
        }
        protected override void LoadContent()
        {
            

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _mouseBB = new Rectangle(ms.X, ms.Y, 1, 1);
            _gamefont = Content.Load<SpriteFont>(@"gamefont");

            _hq = Content.Load<Texture2D>(@"Textures\Sprites\hq");
            _hqBB = new Rectangle((GraphicsDevice.PresentationParameters.BackBufferWidth/2 - (_hq.Width/2)) , 350, (int)(_hq.Width / 2f), (int)(_hq.Height / 2f));

            _hqShields = Content.Load<Texture2D>(@"Textures\Sprites\hqShields");
            _hqShieldsBB = new Rectangle(_hqBB.X -20, _hqBB.Y, (int)(_hqShields.Width), (int)(_hqShields.Height));

            _shieldGen = Content.Load<Texture2D>(@"Textures\Sprites\shieldgen");

            _logo = Content.Load<Texture2D>(@"Textures\Sprites\aftermathLogo");
            _logoBB = new Rectangle(275, -10, (int)(_logo.Width / 2f), (int)(_logo.Height / 2f));

            _newGame = Content.Load<Texture2D>(@"Textures\Sprites\NewGame");
            _newGameBB = new Rectangle(300, 150, (int)(_newGame.Width / 3f), (int)(_newGame.Height / 3f));
            _highscore = Content.Load<Texture2D>(@"Textures\Sprites\HighScore");
            _highscoreBB = new Rectangle(300, 205, (int)(_highscore.Width / 3f), (int)(_highscore.Height / 3f));
            _exitGame = Content.Load<Texture2D>(@"Textures\Sprites\Exit");
            _exitGameBB = new Rectangle(300, 260, (int)(_exitGame.Width / 3f), (int)(_exitGame.Height / 3f));

            _upgrades = Content.Load<Texture2D>(@"Textures\Sprites\Upgrades");
            _upgradesBB = new Rectangle(300, 150, (int)(_upgrades.Width / 3f), (int)(_upgrades.Height / 3f));
            _nextWave = Content.Load<Texture2D>(@"Textures\Sprites\NextWave");
            _nextWaveBB = new Rectangle(600, 530, (int)(_nextWave.Width / 3f), (int)(_nextWave.Height / 3f));
            _generator = Content.Load<Texture2D>(@"Textures\Sprites\Generator");
            _generatorBB = new Rectangle(600, 240, (int)(_generator.Width / 3f), (int)(_generator.Height / 3f));
            _shields = Content.Load<Texture2D>(@"Textures\Sprites\Shields");
            _shieldsBB = new Rectangle(600, 150, (int)(_shields.Width / 3f), (int)(_shields.Height / 3f));
            _missile = Content.Load<Texture2D>(@"Textures\Sprites\Missiles");
            _missileBB = new Rectangle(600, 315, (int)(_missile.Width / 3f), (int)(_missile.Height / 3f));
            _bomb = Content.Load<Texture2D>(@"Textures\Sprites\Bomb");
            _bombBB = new Rectangle(600, 420, (int)(_bomb.Width / 3f), (int)(_bomb.Height / 3f));
            

            _begin = Content.Load<Texture2D>(@"Textures\Sprites\Begin");
            _beginBB = new Rectangle(300, 205, (int)(_beginBB.Width / 3f), (int)(_beginBB.Height / 3f));
            
            
            _resume = Content.Load<Texture2D>(@"Textures\Sprites\Resume");
            _resumeBB = new Rectangle(300, 205, (int)(_exitGame.Width / 3f), (int)(_exitGame.Height / 3f));

            _shieldGeneratorA = new ShieldGenerator(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, _shieldGen, new Vector2(_hqBB.X - 100, _hqBB.Y + 100));
            _shieldGeneratorB = new ShieldGenerator(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, _shieldGen, new Vector2(_hqBB.X - 200, _hqBB.Y + 100));
            _shieldGeneratorC = new ShieldGenerator(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, _shieldGen, new Vector2(_hqBB.X + (_hq.Width / 2) + 50, _hqBB.Y + 100));
            _shieldGeneratorD = new ShieldGenerator(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, _shieldGen, new Vector2(_hqBB.X + (_hq.Width / 2) + 150, _hqBB.Y + 100));
            _headquarters = new Hq(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, _hq, _hqBB);
            _headquartersShields = new HqShields(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, _hqShields, _hqShieldsBB);

            _drawLine = new DrawLine(this, Content, graphics);

            _gameState = new GameStateManager(this);
            _gameState.LoadContent();
        }
        protected override void UnloadContent()
        {
          
        }

        protected override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            ks = Keyboard.GetState();

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _gameState.GameStateID = _gameState.MainMenu;
                resetGame();
            }


            if (_gameState.GameStateID == _gameState.GameActive)
            {
                updateShieldGenerators(gameTime);
                _headquartersShields.Update(gameTime);
                _shieldGensAlive = 0;
                _shieldGensAlive = totalShieldGeneratorsAlive(_shieldGeneratorA, _shieldGensAlive);
                _shieldGensAlive = totalShieldGeneratorsAlive(_shieldGeneratorB, _shieldGensAlive);
                _shieldGensAlive = totalShieldGeneratorsAlive(_shieldGeneratorC, _shieldGensAlive);
                _shieldGensAlive = totalShieldGeneratorsAlive(_shieldGeneratorD, _shieldGensAlive);
                gameOver();

                #region Cheats
                var _cheatMaxMissiles = new KeyboardState(Keys.LeftControl, Keys.Q);
                var _cheatMaxShields = new KeyboardState(Keys.LeftControl, Keys.W);
                var _cheatMoreAsteroids = new KeyboardState(Keys.LeftControl, Keys.E);
                var _cheatdebugMode = new KeyboardState(Keys.LeftControl, Keys.R);
                var _cheatEndAsteroids = new KeyboardState(Keys.LeftControl, Keys.A);
                var _cheatScore = new KeyboardState(Keys.LeftControl, Keys.S);
                var _cheatBombs = new KeyboardState(Keys.LeftControl, Keys.D);
                var _changeGameModeUp = new KeyboardState(Keys.LeftControl, Keys.Up);
                var _changeGameModeDown = new KeyboardState(Keys.LeftControl, Keys.Down);
                
                    if (ks == _cheatMaxMissiles)
                    {
                        _missileStockPile = 999;
                    }
                    if (ks == _cheatMaxShields)
                    {
                        _headquartersShields.Health = 999;
                    }
                    if (ks == _cheatMoreAsteroids)
                    {
                        createAsteroids();
                    }
                    if (ks == _cheatEndAsteroids)
                    {
                        _asteroidsRemainingToSpawn = 0;
                        foreach (Asteroids a in _asteroids)
                        {
                            a.Alive = false;
                        }
                    }
                    if (ks == _changeGameModeUp)
                    {
                        _gameState.GameStateID++;
                    }
                    if (ks == _changeGameModeDown)
                    {
                        _gameState.GameStateID--;
                    }
                    if (ks == _cheatScore)
                    {
                        _score = 9999;
                    }
                    if (ks == _cheatBombs)
                    {
                        _bombsAvailable= 999;
                    }
                    

                    if ((ks == _cheatdebugMode) && (_debugMode == false))
                        _debugMode = true;
                    else if ((ks == _cheatdebugMode)&& (_debugMode == true))
                        _debugMode = false;
                #endregion
                    
                #region Missile Firing Logic
                if ((ms.LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_missileStockPile >=1))
                {
                    var MousePosition = new Vector2();
                    MousePosition.X = ms.X;
                    MousePosition.Y = ms.Y;
                    Vector2 hqPosition;
                    hqPosition.X = _hqBB.X + (_hq.Width );
                    hqPosition.Y = _hqBB.Y;
                    createMissiles(MousePosition, hqPosition);
                    _missileStockPile--;
                }
                if ((ms.RightButton == ButtonState.Pressed) && (msPrevious.RightButton == ButtonState.Released) && (_bombsAvailable >= 1))
                {
                    for (int i = _asteroids.Count() - 1; i >= 0; i--)
                    {
                        _score += 10;
                        _asteroidsKilled++;
                        
                    }
                    foreach (Asteroids a in _asteroids)
                    {
                        a.Alive = false;
                    }
                    _bombsAvailable--;
                }
                
                msPrevious = ms;
                #endregion
                if ((spawnAsteroid <0) && (_asteroidsRemainingToSpawn >0 ))
                {
                    createAsteroids();
                    _asteroidsRemainingToSpawn--;
                    spawnAsteroid = 50;
                }
                spawnAsteroid--;
                #region Collision Detection
                _shieldGeneratorA.generatorCollisionDetection(_asteroids);
                _shieldGeneratorB.generatorCollisionDetection(_asteroids);
                _shieldGeneratorC.generatorCollisionDetection(_asteroids);
                _shieldGeneratorD.generatorCollisionDetection(_asteroids);
                _headquartersShields.shieldCollisionDetection(_asteroids);
                foreach (var a in _asteroids)
                {
                    if (a.BoundingBox.Intersects(_hqBB))
                    {
                        if ((_headquarters.Alive) && (_headquartersShields.Alive != true))
                        {
                            a.Alive = false;
                            _headquarters.Alive = false;
                        }
                    }

                    foreach (var e in _explosions)
                    {

                        if (e.BoundingBox.Intersects(a.BoundingBox))
                        {
                            a.Alive = false;
                            _score += 10;
                            _asteroidsKilled++;
                        }
                    }
                }
                #endregion
                #region Missile Explosions
                foreach (var m in _missiles)
                {
                    if (m.Explosion == true)
                    {
                        var tempExplosion = new Explosions(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, m.Position);
                        _explosions.Add(tempExplosion);
                        Components.Add(tempExplosion);
                    }
                }
                #endregion
                garbageCollection();
                
            }
            checkIfWaveComplete();
            base.Update(gameTime);
        }
        #region GarbageCollection
        private void garbageCollection()
        {
            clearDeadExplosions();
            clearDeadMissiles();
            clearDeadAsteroids();
            _drawLine.clearDeadAsteroids(_asteroids);
        }

        private void clearDeadMissiles()
        {
            for (int i = _missiles.Count() - 1; i >= 0; i--)
            {
                if (_missiles[i].Alive == false)
                {
                    _missiles.RemoveAt(i);
                }
            }
        }
        private void clearDeadAsteroids()
        {
            for (int i = _asteroids.Count() - 1; i >= 0; i--)
            {
                if (_asteroids[i].Alive == false)
                {
                    _asteroids.RemoveAt(i);
                }
            }
        }
        private void clearDeadExplosions()
        {
            for (int i = _explosions.Count() - 1; i >= 0; i--)
            {
                if (_explosions[i].Alive == false)
                {
                    _explosions.RemoveAt(i);
                }
            }
        }
        #endregion
        #region CreateObjects
        private void createMissiles(Vector2 MousePosition, Vector2 hqPosition)
        {
            var tempMissile = new Missiles(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea, MousePosition, hqPosition);
            _missiles.Add(tempMissile);
            Components.Add(tempMissile);
        }
        private void createAsteroids()
        {
            var tempAsteroid = new Asteroids(this, spriteBatch, graphics.GraphicsDevice.Viewport.TitleSafeArea);
            _asteroids.Add(tempAsteroid);
            Components.Add(tempAsteroid);
        }
        #endregion
        
        private void checkIfWaveComplete()
        {
            if ((_asteroidsRemainingToSpawn == 0) && (_asteroids.Count() == 0) && (_gameState.GameStateID == _gameState.GameActive))
            {
                _gameState.GameStateID = _gameState.EndOfLevelScreen;
            }
        }
        private void gameOver()
        {
            if (_headquarters.Alive == false)
            {
                _gameState.GameStateID = _gameState.GameOver;
                resetGame();
            }
        }
        private void nextLevel()
        {
            _wave++;
            _asteroidsRemainingToSpawn = 5 * _wave;
            _missileStockPile = _missileMax;
            _score += _bonusShieldGen;
            _score += _bonusAsteroidsKilled;
            _score += _bonusMissilesStockPile; 
        }
        private void resetGame()
        {
            _bombsAvailable = 0;
            _missileMax = 20;
            _missileStockPile = _missileMax;
            _score = 0;
            _headquarters.Alive = true;
            _shieldGeneratorA.Alive = true;
            _shieldGeneratorB.Alive = true;
            _shieldGeneratorC.Alive = true;
            _shieldGeneratorD.Alive = true;
            _headquartersShields.Alive = true;
            _headquartersShields.Health = 100;
            _asteroidsRemainingToSpawn = 5*_wave;
            _wave = 1;
            _asteroidsKilled = 0;

            for (int i = _asteroids.Count - 1; i >= 0; i--)
            {
                _asteroids[i].Alive = false;

            }
            for (int i = _explosions.Count - 1; i >= 0; i--)
            {
                _explosions[i].Alive = false;
            }
            for (int i = _missiles.Count - 1; i >= 0; i--)
            {
                _missiles[i].Alive = false;
            }
            garbageCollection();
        }
        private int totalShieldGeneratorsAlive(ShieldGenerator _shieldGenerator, int _totalShieldGen)
        {
            if (_shieldGenerator.Alive)
            {
                _totalShieldGen++;

                return _totalShieldGen;
            }
            else
                return _totalShieldGen;
        }

        private void updateShieldGenerators(GameTime gameTime)
        {
            _shieldGeneratorA.Update(gameTime);
            _shieldGeneratorB.Update(gameTime);
            _shieldGeneratorC.Update(gameTime);
            _shieldGeneratorD.Update(gameTime);
        }
        private void drawShieldGenerators()
        {
            _shieldGeneratorA.Draw(spriteBatch);
            _shieldGeneratorB.Draw(spriteBatch);
            _shieldGeneratorC.Draw(spriteBatch);
            _shieldGeneratorD.Draw(spriteBatch);
        }
        private void repairGenerators()
        {
            _shieldGeneratorA.Alive = true;
            _shieldGeneratorB.Alive = true;
            _shieldGeneratorC.Alive = true;
            _shieldGeneratorD.Alive = true;
        }
        private void drawUI()
        {
            spriteBatch.DrawString(_gamefont, "Score:", new Vector2(10, 525), Color.White);
            spriteBatch.DrawString(_gamefont, _score.ToString(), new Vector2(10, 550), Color.White);

            spriteBatch.DrawString(_gamefont, "Missiles:", new Vector2(85, 525), Color.White);
            spriteBatch.DrawString(_gamefont, _missileStockPile.ToString(), new Vector2(85, 550), Color.White);

            spriteBatch.DrawString(_gamefont, "Shields:", new Vector2(200, 525), Color.White);
            spriteBatch.DrawString(_gamefont, _headquartersShields.Health.ToString(), new Vector2(200, 550), Color.White);

            spriteBatch.DrawString(_gamefont, "Asteroids:", new Vector2(300, 525), Color.White);
            spriteBatch.DrawString(_gamefont, _asteroidsRemainingToSpawn.ToString(), new Vector2(300, 550), Color.White);

            spriteBatch.DrawString(_gamefont, "Wave:", new Vector2(415, 525), Color.White);
            spriteBatch.DrawString(_gamefont, _wave.ToString(), new Vector2(415, 550), Color.White);
            spriteBatch.DrawString(_gamefont, "Bombs:", new Vector2(480, 525), Color.White);
            spriteBatch.DrawString(_gamefont, _bombsAvailable.ToString(), new Vector2(480, 550), Color.White);
            _drawLine.Draw(spriteBatch, new Vector2(0, 520), 800, 5);
        }

        private void drawGame()
        {
                drawShieldGenerators();
                _headquarters.Draw(spriteBatch);
                _headquartersShields.Draw(spriteBatch);
                GraphicsDevice.Clear(Color.Black);
                drawUI();
        }
        private void drawEndScreenMenu()
        {
            ms = Mouse.GetState();
            _mouseBB = new Rectangle(ms.X, ms.Y, 1, 1);
            _bonusShieldGen = ((_shieldGensAlive *50) * _shieldGensAlive);
            _bonusAsteroidsKilled = ((_asteroidsKilled *10) * _asteroidsKilled);
            _bonusMissilesStockPile = ((_missileStockPile * 100) * _missileStockPile);
            
            GraphicsDevice.Clear(Color.Black);
            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_upgradesBB.Intersects(_mouseBB)))
            {
                _gameState.GameStateID = _gameState.UpgradeMenu;
            }
            else if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_nextWaveBB.Intersects(_mouseBB)))
            {
                nextLevel();
                _gameState.GameStateID = _gameState.GameActive;
            }
            else
            {
                spriteBatch.Draw(_logo, _logoBB, Color.White);
                spriteBatch.Draw(_upgrades, _upgradesBB, Color.White);
                spriteBatch.Draw(_nextWave, _nextWaveBB, Color.White);
                drawUI();
                spriteBatch.DrawString(_gamefont, "Bonus:", new Vector2(75, 175), Color.White);
                _gameState.drawGeneratorForEndOfLevel(_gamefont, GraphicsDevice, _gameState, spriteBatch, _shieldGensAlive);
                spriteBatch.DrawString(_gamefont, _bonusShieldGen.ToString(), new Vector2(200, 215), Color.White);
                
                
                _gameState.drawAsteroidsForEndOfLevel(_gamefont, GraphicsDevice, _gameState, spriteBatch, _asteroidsKilled);
                spriteBatch.DrawString(_gamefont, _bonusAsteroidsKilled.ToString(), new Vector2(200, 250), Color.White);
                
                
                _gameState.drawMissilesForEndOfLevel(_gamefont, GraphicsDevice, _gameState, spriteBatch, _missileStockPile);
                spriteBatch.DrawString(_gamefont, _bonusMissilesStockPile.ToString(), new Vector2(200, 300), Color.White);
                
            }
            ms = msPrevious;

        }
        private void drawUpgradesMenu()
        {
            ms = Mouse.GetState();
            _mouseBB = new Rectangle(ms.X, ms.Y, 1, 1);
            GraphicsDevice.Clear(Color.Black);
            var textPosition = new Vector2(10, 145);

            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_generatorBB.Intersects(_mouseBB)))
            {
                if (_score >= 1000)
                {
                    repairGenerators();
                    spriteBatch.DrawString(_gamefont, "Generator Repaired!", new Vector2(300, 300), Color.White);
                    _score -= 100;
                }
                else
                {
                    spriteBatch.DrawString(_gamefont, "Insufficient Score!", new Vector2(300, 300), Color.White);
                }
            }
            else if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_shieldsBB.Intersects(_mouseBB)))
            {
                if (_score >= 100)
                {
                    _headquartersShields.Health = 100;
                    spriteBatch.DrawString(_gamefont, "Shields Repaired!", new Vector2(300, 300), Color.White);
                    _score -= 1000;
                }
                else
                {
                    spriteBatch.DrawString(_gamefont, "Insufficient Score!", new Vector2(300, 300), Color.White);
                }
            }
            else if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_missileBB.Intersects(_mouseBB)))
            {
                if (_score >= 750)
                {
                    _missileMax += 10;
                    spriteBatch.DrawString(_gamefont, "Missile Capacity Increased!", new Vector2(300, 300), Color.White);
                    _score -= 750;
                }
                else
                {
                    spriteBatch.DrawString(_gamefont, "Insufficient Score!", new Vector2(300, 300), Color.White);
                }
            }
            else if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_bombBB.Intersects(_mouseBB)))
            {
                if (_score >= 1500)
                {
                    spriteBatch.DrawString(_gamefont, "Bomb purchased!", new Vector2(300, 300), Color.White);
                    _score -= 1500;
                    _bombsAvailable++;
                }
                else
                {
                    spriteBatch.DrawString(_gamefont, "Insufficient Score!", new Vector2(300, 300), Color.White);
                }
            }
            else if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_nextWaveBB.Intersects(_mouseBB)))
            {
                nextLevel();
                _gameState.GameStateID = _gameState.GameActive;
            }
            else
            {
                spriteBatch.Draw(_logo, _logoBB, Color.White);
                //spriteBatch.DrawString(_gamefont, new Vector2(_mouseBB.X, _mouseBB.Y).ToString(), new Vector2(_mouseBB.X, _mouseBB.Y), Color.White);
                _drawLine.Draw(spriteBatch, new Vector2(0, textPosition.Y-5), 800, 2);
                spriteBatch.Draw(_generator, _generatorBB, Color.White);
                spriteBatch.DrawString(_gamefont, "Repair Shield Health.\nRestores your Headquarter shields to full.\nCosts 100 score.", textPosition, Color.White); 
                textPosition.Y += 80;
                _drawLine.Draw(spriteBatch, new Vector2(0, textPosition.Y - 5), 800, 2);
                spriteBatch.DrawString(_gamefont, "Repair Generators.\nRestores all damaged shield generators.\nCosts 750 score.", textPosition, Color.White);
                spriteBatch.Draw(_shields, _shieldsBB, Color.White);
                textPosition.Y += 80;
                _drawLine.Draw(spriteBatch, new Vector2(0, textPosition.Y - 5), 2, 800);
                spriteBatch.Draw(_missile, _missileBB, Color.White);
                spriteBatch.DrawString(_gamefont, "Improve missile stockpiles.\nIncreases your maximum amount of missiles.\nCosts 1,000 score.", textPosition, Color.White);
                textPosition.Y += 80;
                _drawLine.Draw(spriteBatch, new Vector2(0, textPosition.Y - 5), 800, 2);
                spriteBatch.Draw(_bomb, _bombBB, Color.White);
                spriteBatch.DrawString(_gamefont, "Purchase a 'Supernova 1337 Bomb'.\nThis enourmous explosive is large enough to \ndestroy any asteroids threatening your Headquarters\nand it's next-of-kin!\nCosts 1,500 score.", textPosition, Color.White);
                spriteBatch.Draw(_nextWave, _nextWaveBB, Color.White);
                drawUI();           
            }
            ms = msPrevious;
        }
        private void drawMainMenu()
        {
            ms = Mouse.GetState();
            _mouseBB = new Rectangle(ms.X, ms.Y, 1, 1);
            garbageCollection();
            GraphicsDevice.Clear(Color.Black);
            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_newGameBB.Intersects(_mouseBB)))
            {
                _gameState.GameStateID = _gameState.GameActive;
                resetGame();
            }
            else if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (msPrevious.LeftButton == ButtonState.Released) && (_exitGameBB.Intersects(_mouseBB)))
            {
                this.Exit();
            }
            else
            {
                spriteBatch.Draw(_logo, _logoBB, Color.White);
                spriteBatch.Draw(_newGame, _newGameBB, Color.White);
                spriteBatch.Draw(_highscore, _highscoreBB, Color.White);
                spriteBatch.Draw(_exitGame, _exitGameBB, Color.White);
                spriteBatch.DrawString(_gamefont, "af-ter-math\nnoun\n1.something that results or follows from an event, \nespecially one of a disastrous or unfortunate nature; \nconsequence: \nthe aftermath of the asteroids.", new Vector2(150, 300), Color.White);
            }
            msPrevious = ms;
        }
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (_gameState.GameStateID == _gameState.MainMenu)
            {
                drawMainMenu();
            }
            if (_gameState.GameStateID == _gameState.EndOfLevelScreen)
            {
                drawEndScreenMenu();
            }
            else if (_gameState.GameStateID == _gameState.UpgradeMenu)
            {
                drawUpgradesMenu();
            }
            else if (_gameState.GameStateID == _gameState.GameActive)
            {
                drawGame();
            }
            
            //spriteBatch.DrawString(_gamefont, "GameState", new Vector2(80, 100), Color.White);
            //spriteBatch.DrawString(_gamefont, _gameState.GameStateID.ToString(), new Vector2(100, 125), Color.White);
            //spriteBatch.DrawString(_gamefont, _shieldGensAlive.ToString(), new Vector2(425, 550), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }
}


