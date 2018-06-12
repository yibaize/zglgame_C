using ProtoBuf;
namespace org.zgl
{
   
    [ProtoContract]
    public class IoMessageBaseTypeImpl : IoMessage
    {
        /**接口名*/
        [ProtoMember(1)]
        public string interfaceName;
        /**方法名*/
        [ProtoMember(2)]
        public string methodName;
        /**参数*/
        [ProtoMember(3)]
        public string args;

        public object getData()
        {
            return args;

        }

        public string getInterfaceName()
        {
            return interfaceName;
        }

        public string getMethodName()
        {
            return methodName;
        }
    }
}
