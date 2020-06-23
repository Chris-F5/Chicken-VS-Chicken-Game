using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameObjects
{
    sealed class Wall : KenimaticColliderObject
    {
        public Wall(Rect _rect) : base((byte)ObjectTypes.wall, _rect)
        {
            SendNewObjectPacket();
        }
    }
}
