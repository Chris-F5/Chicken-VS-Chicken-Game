using SharedClassLibrary.Utilities;

namespace SharedClassLibrary.GameLogic.Components
{
    public struct VelocityComponent
    {
        public VelocityComponent(float _xVelocity, float _yVelocity)
        {
            velocity.x = _xVelocity;
            velocity.y = _yVelocity;
        }
        public Vector2 velocity;
    }
}
