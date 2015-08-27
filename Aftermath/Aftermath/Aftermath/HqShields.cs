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
    class HqShields
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Rectangle _titleSafeArea;
        private readonly ContentManager _content;
        Texture2D _sprite;

        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
        }

        private Boolean _alive = true;
        public Boolean Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }
        private int _health;
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        private Color _healthStatus;

        public HqShields(Game game, SpriteBatch spriteBatch, Rectangle titleSafeArea, Texture2D shieldsSprite, Rectangle positionBB)
		{
            _spriteBatch = spriteBatch;
            _titleSafeArea = titleSafeArea;
            _content = game.Content;
            _sprite = shieldsSprite;
            _boundingBox = positionBB;
            _alive = true;
            _health = 100;
            _healthStatus = Color.Blue;
        }
        public void Update(GameTime gameTime)
        {
            if (_health <= 0)
                _alive = false;
            else if ((_health <= 19) && (_health >= 1))
                _healthStatus = Color.BlueViolet ;
            else if ((_health <= 39) && (_health >= 20))
                _healthStatus = Color.Blue;
            else if ((_health <= 59) && (_health >= 40))
                _healthStatus = Color.Blue;
            else if ((_health <= 79) && (_health >= 60))
                _healthStatus = Color.Blue;
            else if ((_health <= 99) && (_health >= 80))
                _healthStatus = Color.Blue;
        }

        public void shieldCollisionDetection(List<Asteroids> _asteroids)
        {
            foreach (var a in _asteroids)
            {
                if (_alive)
                {
                    var tempBB = _boundingBox;
                    tempBB.Width -= 20;
                    tempBB.Height -= 50;
                    if (a.BoundingBox.Intersects(tempBB))
                    {
                        a.Alive = false;
                        _health-=10;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
                spriteBatch.Draw(_sprite, BoundingBox, _healthStatus);
        }
    }
}
