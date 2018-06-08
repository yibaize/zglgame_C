using ProtoBuf;
using System.IO;
namespace org.zgl
{
    public class ProtostuffUtils
    {
        //序列化操作
        public static byte[] ProtobufSerialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                byte[] data = ms.ToArray();//length=27  709
                return data;
            }
        }
        //反序列化操作
        public static T ProtobufDeserialize<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }
    }
}