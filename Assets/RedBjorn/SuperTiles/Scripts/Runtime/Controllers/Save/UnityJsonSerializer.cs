using System.Text;
using UnityEngine;

namespace RedBjorn.SuperTiles.Saves
{
    public class UnityJsonSerializer<T> : ISerializer where T : class
    {
        ISerializerCallback Callback;

        public UnityJsonSerializer(ISerializerCallback callback)
        {
            Callback = callback;
        }

        public void Serialize(object data, string name)
        {
            var result = true;
            var dataString = JsonUtility.ToJson(data, true);
            var serialized = Encoding.UTF8.GetBytes(dataString);
            if (Callback != null)
            {
                Callback.OnSerializeCompleted(result, name, serialized);
            }
        }

        public void Deserialize(byte[] data, string name)
        {
            var result = true;
            var dataString = Encoding.UTF8.GetString(data);
            var deserialized = JsonUtility.FromJson<T>(dataString);
            if (Callback != null)
            {
                Callback.OnDeserializeCompleted(result, name, deserialized);
            }
        }
    }
}
