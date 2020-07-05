using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.NetworkSynchronisers
{
    sealed class Wall : KenimaticColliderObject
    {
        public Wall(Rect _rect) : base(SynchroniserType.wall, _rect) {}
    }
}
