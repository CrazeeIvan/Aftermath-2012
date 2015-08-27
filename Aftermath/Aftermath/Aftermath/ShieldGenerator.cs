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
    class ShieldGenerator
    {
        #region Variables
        private readonly SpriteBatch _spriteBatch;
        private readonly Rectangle _titleSafeArea;
        private readonly ContentManager _content;

        Texture2D _sprite;
        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
        }

        Boolean _alive;
        public Boolean Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }

        #endregion



        public ShieldGenerator(Game game, SpriteBatch spriteBatch, Rectangle titleSafeArea, Texture2D shieldGenSprite, Vector2 position )
		{
            _spriteBatch = spriteBatch;
            _titleSafeArea = titleSafeArea; 
            _content = game.Content;
            _sprite = shieldGenSprite;
            _boundingBox = new Rectangle((int)position.X, (int)position.Y, (int)(_sprite.Width / 4f), (int)(_sprite.Height / 4f));
            _alive = true;
        }

        

        public void Update(GameTime gameTime)
        {
            
        }
        public void generatorCollisionDetection(List<Asteroids> _asteroids)
        {
            foreach (var a in _asteroids)
            {
                if (_alive)
                {
                    if (a.BoundingBox.Intersects(_boundingBox))
                    {
                        a.Alive = false;
                        _alive = false;
                    }
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_alive)
           spriteBatch.Draw(_sprite, BoundingBox, Color.White);
        }
    }
}
