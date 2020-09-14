using SharedClassLibrary.Utilities;

namespace SharedClassLibrary.GameLogic.Components
{
    public struct TransformComponent
    {
        public Vector2 position;

        public TransformComponent(float _x, float _y)
        {
            position.x = _x;
            position.y = _y;
        }
    }
}
