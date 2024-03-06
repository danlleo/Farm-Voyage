namespace DataPersistence
{
    public interface IBind<TData> where TData : ISaveable
    {
        public SerializableGuid ID { get; set; }
        public void Bind(TData data);
    }
}