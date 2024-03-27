using UnityEngine;

namespace Character.Player
{
    [System.Serializable]
    public struct PlayerTransformPoints
    {
        [field:SerializeField] public Transform WorkbenchStayPoint { get; private set; }
        [field:SerializeField] public Transform EmmaStoreStayPoint { get; private set; }
        [field:SerializeField] public Transform HomeLeavePoint { get; private set; }
        [field:SerializeField] public Transform HomeStayPoint { get; private set; }
        [field:SerializeField] public Transform WellStayPoint { get; private set; }
    }
}