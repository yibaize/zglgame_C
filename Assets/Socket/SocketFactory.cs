namespace org.zgl
{
    public class SocketFactory
    {
        private TcpSocketImpl tcp;
        private HttpSocketImpl http;
        private static SocketFactory instance;

        public static SocketFactory getInstance()
        {
            if (instance == null)
                instance = new SocketFactory();
            return instance;
        }

        public void tcpAsyncRequest(IoMessage ioMessage)
        {
            tcp.async(ioMessage);
        }
        public object tcpSyncRequest(IoMessage ioMessage)
        {
            return tcp.sync(ioMessage);
        }
        public void httpAsyncRequest(IoMessage ioMessage)
        {
            http.async(ioMessage);
        }
        public object httpSyncRequest(IoMessage ioMessage)
        {
            return http.sync(ioMessage);
        }
    }
}
