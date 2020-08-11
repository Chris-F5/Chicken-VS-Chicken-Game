using System.Collections.Generic;

namespace SharedClassLibrary.Simulation.Components
{
    public class PositionComponent : Component
    {
        private IPositionComponentHandler handler;
        private Vector2 position;

        public Vector2 Position 
        { 
            get { return position; } 
            set 
            {
                position = value;
                HandlePosition();
            } 
        }

        public PositionComponent(NetworkObject _object, Vector2 _position) : base(_object) 
        {
            position = _position;
        }

        public override void CallStartupHandlers()
        {
            base.CallStartupHandlers();
            HandlePosition();
        }

        public override void Update()
        {
            HandlePosition();
        }

        public void HandlePosition()
        {
            handler.SetPosition(position.x, position.y);
        }
    }

    public interface IPositionComponentHandler
    {
        void SetPosition(float _xPos, float _yPos);
    }
}
