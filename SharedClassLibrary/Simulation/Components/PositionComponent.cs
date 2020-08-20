namespace SharedClassLibrary.Simulation.Components
{
    public class PositionComponent : RollbackComponent
    {
        public Vector2 Position 
        { 
            get 
            {
                return (state as PositionComponentState).Position;
            } 
            set 
            {
                (state as PositionComponentState).Position = value;
            } 
        }

        internal PositionComponent(Component _nextComponent, Vector2 _position) 
            : base(_nextComponent, new PositionComponentState( _position ) ) 
        { }
    }

    internal class PositionComponentState : ComponentRollbackState
    {
        public PositionComponentState(Vector2 _position)
        {
            position = _position;
        }

        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return (activeState as PositionComponentState).position;
            }
            internal set
            {
                ChangeMade();
                (activeState as PositionComponentState).position = value;
            }
        }

        protected override ComponentRollbackState Clone()
        {
            return new PositionComponentState(position);
        }
    }
}