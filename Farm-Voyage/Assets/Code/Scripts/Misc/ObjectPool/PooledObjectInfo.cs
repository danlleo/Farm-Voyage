using System.Collections.Generic;
using UnityEngine;

namespace Misc.ObjectPool
{
    public class PooledObjectInfo
    {
        public string LookupString;
        public List<GameObject> InactiveObjects = new();
    }
}