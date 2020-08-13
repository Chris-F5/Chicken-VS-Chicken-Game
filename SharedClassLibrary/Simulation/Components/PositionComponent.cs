using System;

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

        public PositionComponent(Component _nextComponent, IPositionComponentHandler _handler, Vector2 _position) : base(_nextComponent) 
        {
            if (_handler == null)
                throw new ArgumentNullException("_handler");

            handler = _handler;
            position = _position;
        }

        internal override void CallStartupHandlers()
        {
            base.CallStartupHandlers();
            HandlePosition();
        }

        internal override void Update()
        {
            HandlePosition();

            base.Update();
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
