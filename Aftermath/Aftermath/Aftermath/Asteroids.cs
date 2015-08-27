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
    class Asteroids : DrawableGameComponent
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
            get { return _boundingBox; }
        }
        private bool _alive = true;
        public bool Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }
        private Vector2 _direction = new Vector2(0, 1);
        private float _speed;
        private Vector2 _westSouthWest= new Vector2(-1, 0.5f);
        private Vector2 _southWest = new Vector2(-0.5f, 0.5f);
        private Vector2 _southSouthWest = new Vector2(-0.5f, 1);
        private Vector2 _south = new Vector2(0, 0.5f);
        private Vector2 _southSouthEast = new Vector2(0.5f, 1);
        private Vector2 _southEast = new Vector2(0.5f, 0.5f);
        private Vector2 _eastSouthEast = new Vector2(1, 0.5f);
        Random randDirection = new Random();
        public SpriteFont _gamefont;
        
        #endregion
        public Asteroids(Game game, SpriteBatch spriteBatch, Rectangle titleSafeArea)
			: base(game)
		{
            _spriteBatch = spriteBatch;
            _titleSafeArea = titleSafeArea;
            _content = game.Content;
            _speed = GameRandom.Random.Next(1, 3);
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _sprite = _content.Load<Texture2D>(@"Textures/Sprites/asteroid");
            _position = new Vector2((float)GameRandom.Random.NextDouble() * GraphicsDevice.Viewport.Width - 30, 0);
            _direction = returnDirection();
            _gamefont = _content.Load<SpriteFont>(@"gamefont");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _boundingBox = new Rectangle((int)_position.X, (int)_position.Y, (int)(_sprite.Width / 1.5f), (int)(_sprite.Height / 1.5f));
            if(_boundingBox.X < _titleSafeArea.X)
            {
                 _direction = Vector2.Reflect(_direction,new Vector2(1,0));
            }
            if (_boundingBox.X > _titleSafeArea.Width-_boundingBox.Width)
            {
                _direction = Vector2.Reflect(_direction, new Vector2(-1, 0));
            }
            _position += _direction * _speed;

        }
        public Vector2 returnDirection()
        {
            var tempRand = GameRandom.Random.Next(0, 7);
            if (tempRand == 0)
                return _westSouthWest;
            else if (tempRand == 1)
                return _southWest;
            else if (tempRand == 2)
                return _southSouthWest;
            else if (tempRand == 3)
                return _southSouthEast;
            else if (tempRand == 4)
                return _southEast;
            else if (tempRand == 5)
                return _eastSouthEast;
            else
                return _south;
        }

        

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            if (_alive == true)
            {
                _spriteBatch.Draw(_sprite, BoundingBox, Color.White);               
            }
            _spriteBatch.End();
        }

    }
}
