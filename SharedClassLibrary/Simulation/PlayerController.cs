using SharedClassLibrary.Simulation.ObjectTemplates;

namespace SharedClassLibrary.Simulation
{
    public class PlayerController
    {
        public bool rightKey = false;
        public bool leftKey = false;
        public bool upKey = false;

        private NetworkObject player;
        public PlayerController()
        {
            player = new Player(this, new Vector2(0, 10)).CreateObjectInstance();
        }
    }
}
