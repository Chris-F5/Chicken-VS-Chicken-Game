using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedClassLibrary.ECS;
using SharedClassLibrary.GameLogic.Components;

namespace GameClient
{
    class SpriteRendererSystem : GameSystem
    {
        private ComponentManager<TransformComponent> transformManager;
        private ComponentManager<SpriteComponent> spriteComponentManager;
        private SpriteBatch spriteBatch;
        private SpriteManager spriteManager;
        private Camera camera;

        public SpriteRendererSystem(World _world, SpriteBatch _spriteBatch, SpriteManager _spriteManager, Camera _camera) 
            : base(_world, typeof(TransformComponent), typeof(SpriteComponent))
        {
            spriteManager = _spriteManager;
            spriteBatch = _spriteBatch;
            camera = _camera;

            componentMask.GetComponentManager(out transformManager);
            componentMask.GetComponentManager(out spriteComponentManager);
        }

        public override void Update()
        {
            spriteBatch.Begin();
            foreach (Entity entity in activeEntities)
            {
                ref TransformComponent transform = ref transformManager.GetComponent(entity);
                ref SpriteComponent spriteComponent = ref spriteComponentManager.GetComponent(entity);

                Texture2D sprite = spriteManager.GetSprite(spriteComponent.spriteId);
                DrawSprite(sprite, transform.position);
            }
            spriteBatch.End();
        }
        private void DrawSprite(Texture2D _sprite, SharedClassLibrary.Utilities.Vector2 _worldPosition)
        {
            Vector2 screenPosition = camera.WorldSpaceToScreenSpace(_worldPosition);
            Rectangle drawRect = new Rectangle((int)Math.Round(screenPosition.X), (int)Math.Round(screenPosition.Y), _sprite.Width, _sprite.Height);
            spriteBatch.Draw(_sprite, drawRect, Color.White);
            //Console.WriteLine($"{drawRect.X}, { drawRect.Y}");
        }
    }
}
