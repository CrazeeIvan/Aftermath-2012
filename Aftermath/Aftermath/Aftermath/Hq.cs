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
    


    class Hq
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

        public Hq(Game game, SpriteBatch spriteBatch, Rectangle titleSafeArea, Texture2D hqSprite, Rectangle positionBB)
		{
            _spriteBatch = spriteBatch;
            _titleSafeArea = titleSafeArea;
            _content = game.Content;
            _sprite = hqSprite;
            _boundingBox = positionBB;
            _alive = true;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            spriteBatch.Draw(_sprite, BoundingBox, Color.White);
        }
    }
}
