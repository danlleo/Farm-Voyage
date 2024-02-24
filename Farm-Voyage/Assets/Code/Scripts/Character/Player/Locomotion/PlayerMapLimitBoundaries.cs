using UnityEngine;

namespace Character.Player.Locomotion
{
    public class PlayerMapLimitBoundaries
    {
        private readonly Transform _playerTransform;

        private readonly float _minX;
        private readonly float _maxX;
        private readonly float _minZ;
        private readonly float _maxZ;

        public PlayerMapLimitBoundaries(Transform playerTransform, float minX, float maxX, float minZ, float maxZ)
        {
            _playerTransform = playerTransform;
            _minX = minX;
            _maxX = maxX;
            _minZ = minZ;
            _maxZ = maxZ;
        }

        public void KeepWithinBoundaries()
        {
            Vector3 position = _playerTransform.position;
            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            position.z = Mathf.Clamp(position.z, _minZ, _maxZ);
            _playerTransform.position = position;
        }
    }
}