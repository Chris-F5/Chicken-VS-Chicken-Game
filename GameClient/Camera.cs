using System;
using System.Collections.Generic;
using System.Text;
using SharedClassLibrary.Utilities;

namespace GameClient
{
    class Camera
    {
        public Camera(int _screenWidth, int _screenHeight)
        {
            screenHeight = _screenHeight;
            screenWidth = _screenWidth;
        }

        // TODO: make position private set after testing.
        public Vector2 position { get; set; }
        private int screenWidth;
        private int screenHeight;

        public Microsoft.Xna.Framework.Vector2 WorldSpaceToScreenSpace(Vector2 _spritePosition)
        {
            _spritePosition -= position - new Vector2(screenWidth / 2, screenHeight / 2);
            _spritePosition.y = _spritePosition.y * -1 + screenHeight;
            return new Microsoft.Xna.Framework.Vector2(_spritePosition.x, _spritePosition.y);
        }
    }
}
