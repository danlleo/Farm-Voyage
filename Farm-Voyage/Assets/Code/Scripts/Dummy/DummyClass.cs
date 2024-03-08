using DataPersistence;
using UnityEngine;

public class DummyClass : MonoBehaviour, IBind<DummyClassData>
{
    [field:SerializeField] public SerializableGuid ID { get; set; } = SerializableGuid.NewGuid();
    [SerializeField] private DummyClassData _dummyClassData;
    
    public void Bind(DummyClassData data)
    {
        _dummyClassData = data;
        _dummyClassData.ID = ID;
        transform.position = data.Position;
        transform.rotation = data.Rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _dummyClassData.Position = transform.position * Random.Range(1, 5);
            _dummyClassData.Rotation = transform.rotation;
        }        
    }
}

[System.Serializable]
public class DummyClassData : ISaveable
{
    [field:SerializeField] public SerializableGuid ID { get; set; }
    public Vector3 Position;
    public Quaternion Rotation;
}
