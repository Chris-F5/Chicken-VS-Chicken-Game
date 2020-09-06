using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.NetworkObjects
{
    public class Player : NetworkObject
    {
        public Player(PlayerController _controller, Vector2 _position) 
            : base(
                  new PlayerMovement(
                      new DynamicPhysicsBehaviour(
                          new RectCollider(
                              new PositionComponent(
                                  null,
                                  _position
                                  )
                              )
                          ),
                      _controller
                      ),
                  (short) NetworkObjectTemplateIds.player
                  )
        { }

        protected internal override void Update()
        {
            Logging.Logger.LogDebug($"Player position: ({GetComponent<PositionComponent>().Position.x}, {GetComponent<PositionComponent>().Position.y})");
            base.Update();
        }
    }
}
