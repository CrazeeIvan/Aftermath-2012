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
    class DrawLine
    {
        private readonly ContentManager _content;
        private GraphicsDeviceManager _graphics;
        Texture2D SimpleTexture;
        Int32[] pixel = {0xFFFFFF};
        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get { return _boundingBox; }
        }
        
        public DrawLine(Game game, ContentManager content, GraphicsDeviceManager graphics)
		{
            _content = game.Content;
            _graphics = graphics;
            SimpleTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);
        }
        public void clearDeadAsteroids(List<Asteroids> _asteroids)
        {
            foreach (var a in _asteroids)
            {
                    if (a.BoundingBox.Intersects(_boundingBox))
                    {
                        a.Alive = false;
                    }
                
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 startPoint, int length, int thickness)
        {
            _boundingBox = new Rectangle((int)startPoint.X, (int)startPoint.Y, length, thickness);
            spriteBatch.Draw(SimpleTexture, _boundingBox, Color.White);
        }

    }
}
