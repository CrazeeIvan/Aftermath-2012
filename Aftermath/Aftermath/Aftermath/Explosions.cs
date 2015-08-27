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
    class Explosions : DrawableGameComponent
    {
        #region Variables
        private readonly SpriteBatch _spriteBatch;
        private readonly Rectangle _titleSafeArea;
        private readonly ContentManager _content;
        private Texture2D _sprite;
        private Vector2 _position;
        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox;}
        }
        Point _frameSize = new Point(64, 63);
        Point _currentFrame = new Point(0, 0);
        Point _sheetSize = new Point(6, 1);
        int _timeSinceLastFrame = 0;
        int _millisecondsPerFrame = 75;
        private Boolean _alive = true;
        public Boolean Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }
        public float Rotation { get; set; }
        #endregion
        public Explosions(Game game, SpriteBatch spriteBatch, Rectangle titleSafeArea, Vector2 missilePosition)
        : base(game)
		{
            _spriteBatch = spriteBatch;
            _titleSafeArea = titleSafeArea;
            _content = game.Content;
            _position.X = missilePosition.X -25;
            _position.Y = missilePosition.Y - 50; 
        }
        protected override void LoadContent() 
        {
            base.LoadContent();
            _sprite = _content.Load<Texture2D>(@"Textures/Spritesheet/explosion");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _boundingBox = new Rectangle((int)_position.X, (int)_position.Y, 64, 63);
            #region Explosion Animation
            _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (_timeSinceLastFrame > _millisecondsPerFrame)
            {
                _timeSinceLastFrame -= _millisecondsPerFrame;
                ++_currentFrame.X;
                if (_currentFrame.X >= _sheetSize.X)
                {
                    _currentFrame.X = 0;
                    ++_currentFrame.Y;
                    if (_currentFrame.Y >= _sheetSize.Y)
                    {
                        _currentFrame.Y = 0;
                    }
                    _alive = false;
                }
            }
            #endregion
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            if (_alive)
            {
                _spriteBatch.Draw(_sprite, _position,
                            new Rectangle(_currentFrame.X * _frameSize.X,
                            _currentFrame.Y * _frameSize.Y,
                            _frameSize.X,
                            _frameSize.Y),
                            Color.White, 0, Vector2.Zero,
                            1, SpriteEffects.None, 1);
            }
            _spriteBatch.End();
        }
    }
}
