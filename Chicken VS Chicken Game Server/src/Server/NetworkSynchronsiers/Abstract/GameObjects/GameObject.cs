using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameServer.NetworkSynchronisers
{
    abstract class GameObject : NetworkSynchroniser
    {
        public static List<GameObject> allObjects = new List<GameObject>();

        public GameObject(SynchroniserType _synchroniseType) : base(_synchroniseType)
        {
            allObjects.Add(this);
        }
        public abstract void Update();

        public virtual void Destroy()
        {
            // TODO: tell client object was destroyed
            // TODO: dispose object so it wont leak memory
        }

        public static void UpdateAll()
        {
            // TODO: This calles a collection modified exception sometimes because its being changed on a different thread.
            foreach (GameObject _go in allObjects)
            {
                _go.Update();
            }
        }
    }
}
