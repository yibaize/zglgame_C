namespace org.zgl
{
    public interface ISocket
    {
        /// <summary>
        /// 同步请求
        /// </summary>
        object sync(IoMessage ioMessage);
        /// <summary>
        /// 异步请求
        /// </summary>
        /// <returns></returns>
        void async(IoMessage ioMessage);
    }
}
