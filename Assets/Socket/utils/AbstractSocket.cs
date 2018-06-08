
namespace org.zgl
{
    public enum DataSrcEnum {
        BaseType = 1,
        PBType = 2
    }
    public abstract class AbstractSocket : ISocket
    {
        protected readonly short dataSrc = 1;
        public abstract void async(IoMessage ioMessage);
        public abstract object sync(IoMessage ioMessage);
        /// <summary>
        /// 都是基础数据类型使用IoMessageBaseTypeImpl反序列
        /// </summary>
        /// <param name="buf"></param>
        protected void baseType(byte[] buf)
        {
            IoMessageBaseTypeImpl ioMessage = ProtostuffUtils.ProtobufDeserialize<IoMessageBaseTypeImpl>(buf);
            //添加队列
            IoMessageHandler.getInstance().push(ioMessage);
        }
        /// <summary>
        /// PB类型使用IoMessagePBTypeImpl反序列
        /// </summary>
        /// <param name="buf"></param>
        protected void pbType(byte[] buf)
        {
            IoMessagePBTypeImpl ioMessage = ProtostuffUtils.ProtobufDeserialize<IoMessagePBTypeImpl>(buf);
            //添加队列
            IoMessageHandler.getInstance().push(ioMessage);
        }
    }
}
