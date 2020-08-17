using System;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.ObjectTemplates
{
    public sealed class Wall : NetworkObject
    {
        public Wall(Vector2 _position, Vector2 _size) 
            : base(
                  new KenimaticCollider(
                      new RectCollider(
                          new PositionComponent(
                              null,
                              _position
                              ), 
                          _size
                          )
                      ),
                  (short)NetworkObjectTemplateIds.wall
                  )
        { }
    }
}
