namespace RedBjorn.SuperTiles.Saves
{
    public interface ISerializer
    {
        void Serialize(object data, string name);
        void Deserialize(byte[] data, string name);
    }

    public interface ISerializerCallback
    {
        void OnSerializeCompleted(bool success, string name, byte[] serialized);
        void OnDeserializeCompleted(bool success, string name, object deserialized);
    }
}
