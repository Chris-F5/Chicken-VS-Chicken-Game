using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedClassLibrary.GameLogic;

namespace GameClient
{
    class SpriteManager
    {
        private Dictionary<short, Texture2D> sprites = new Dictionary<short, Texture2D>();
        private readonly ContentManager contentManager;

        public SpriteManager(ContentManager _contentManager)
        {
            contentManager = _contentManager;
        }

        public void LoadContent()
        {
            sprites.Clear();
            sprites.Add((short)SpriteIds.Chicken, contentManager.Load<Texture2D>("Chicken"));
        }

        public Texture2D GetSprite(short _id)
        {
            return sprites[_id];
        }
    }
}
