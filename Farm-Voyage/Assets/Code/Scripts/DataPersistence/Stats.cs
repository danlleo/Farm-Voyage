using UnityEngine;

namespace DataPersistence
{
    [System.Serializable]
    public struct SerializableVector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3 GetPosition()
        {
            return new Vector3(X, Y, Z);
        }
    }
    
    [System.Serializable]
    public class Stats
    {
        public int LifeAmount;
        public int Level;
    }
}