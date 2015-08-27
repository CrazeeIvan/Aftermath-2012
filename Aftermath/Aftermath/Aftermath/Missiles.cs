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
    class Missiles : DrawableGameComponent
    {
        #region Variables
        private readonly SpriteBatch _spriteBatch;
        private readonly Rectangle _titleSafeArea;
        private readonly ContentManager _content;
        private Texture2D _sprite;
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _direction = new Vector2(0, 0.5f);
        private Vector2 _mousePosition;
        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
        }
        private int _speed = 2;
        public float Rotation { get; set; }
        private Boolean explosion = false;
        public Boolean Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }
        private Boolean _alive = true;
        public Boolean Alive
        {
            get {return _alive;}
            set {_alive = value;}
        }
        Vector2 _hqPosition;
        #endregion
        public Missiles(Game game, SpriteBatch spriteBatch, Rectangle titleSafeArea, Vector2 mousePosition, Vector2 hqPosition)
			: base(game)
		{
            _spriteBatch = spriteBatch;
            _titleSafeArea = titleSafeArea;
            _content = game.Content;
            _mousePosition = mousePosition;
            _hqPosition = hqPosition;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _sprite = _content.Load<Texture2D>(@"Textures/Sprites/missile");
            
            _position.X = (_hqPosition.X/2)+10;
            _position.Y = _hqPosition.Y+25;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _boundingBox = new Rectangle((int)_position.X, (int)_position.Y, (int)(_sprite.Width / 2f), (int)(_sprite.Height / 2f));
            missileNavigation();
        }

        public void missileNavigation()
        {
            var vectorToTarget = _mousePosition - _position;
            if (vectorToTarget.Length() < 1)
            {
                explosion = true;
                _alive = false;
                return;
            }
            vectorToTarget.Normalize();
            _direction = (vectorToTarget + _direction) /2;
            _position += _direction * _speed;
            Rotation = (float)Math.Atan2(_direction.Y, _direction.X);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            if (_alive)
            {
                _spriteBatch.Draw(_sprite, BoundingBox, null, Color.White, Rotation, Vector2.Zero, SpriteEffects.None, 1);
            }
            _spriteBatch.End();
        }


    }
}
