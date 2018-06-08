
using System;
using System.Net.Sockets;
using UnityEngine;
namespace org.zgl
{
    public class TcpSocketImpl : AbstractSocket
    {

        private Socket socket;
        private ByteArray ioBuffer = new ByteArray();
        private byte[] readBuffer = new byte[1024];
        private bool isInit = false;

        private bool isRead;
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                //结束异步读取数据并获取数据长度
                int readCount = socket.EndReceive(ar);
                byte[] bytes = new byte[readCount];
                //将接收缓冲池的内容复制到临时消息存储数组
                Buffer.BlockCopy(readBuffer, 0, bytes, 0, readCount);
                ioBuffer.WriteBytes(bytes);
                if (!isRead)
                {
                    isRead = true;
                    onData();
                }
            }
            catch (Exception)
            {
                Debug.Log("房间 远程服务器主动断开连接");
                socket.Close();
                return;
            }
            socket.BeginReceive(readBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuffer);
        }
        /// <summary>
        /// // 数据包的基本长度：包头+1级协议+2级协议+结果码+数据长度；
        /// 每个协议都是一个int类型的基本数据占4个字节
        /// </summary>
        private int BASE_LENGTH = 4 + 2 + 2;
        private void onData()
        {

            //消息长度小于数据基础长度说明包没完整
            if (ioBuffer.Length < BASE_LENGTH)
            {
                isRead = false;
                return;
            }

            //读取定义的消息长度
            while (true)
            {
                int datazie = ioBuffer.ReadInt();
                if (datazie == -777888)
                {
                    break;
                }
            }
            ByteArray ioData = new ByteArray();
            short dataSrcResponse = ioBuffer.ReadShort();//数据类型 1，全是基础数据类型 2，PB类型
            short length = ioBuffer.ReadShort();
            if (ioBuffer.Length < length - BASE_LENGTH)
            {
                //还原指针
                ioBuffer.Postion = 0;
                isRead = false;
                //数据不全 还原指针等待下一数据包（分包）
                return;
            }
            ioData.WriteBytes(ioBuffer.Buffer, BASE_LENGTH, length);//
            ioBuffer.Postion += length;
            byte[] buf = new byte[length];
            buf = ioData.ReadBytes();

            switch (dataSrcResponse) {
                case (short)DataSrcEnum.BaseType:
                    baseType(buf);
                    break;
                case (short)DataSrcEnum.PBType:
                    pbType(buf);
                    break;
            }

            ByteArray bytes = new ByteArray();
            bytes.WriteBytes(ioBuffer.Buffer, ioBuffer.Postion, ioBuffer.Length - ioBuffer.Postion);
            ioBuffer = bytes;
            onData();
        }
        
        public ISocket init()
        {
            if (!isInit)
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect("", 0);
                    Debug.Log("连接到 房间 服务器");
                    socket.BeginReceive(readBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuffer);
                }
                catch (Exception)
                {
                    Debug.Log("服务器连接失败");
                    isInit = false;

                }
                isInit = true;
            }
            return this;
        }
        public void close()
        {
            if (socket != null)
            {
                socket.Close();
            }
            isInit = false;
            // t.Stop();
            // t.Close();
        }

        public override void async(IoMessage ioMessage)
        {
            byte[] buf = ProtostuffUtils.ProtobufSerialize(ioMessage);
            ByteArray arr = new ByteArray();
            arr.WriteInt(-777888);//包头
            arr.WriteShort(dataSrc);
            arr.WriteShort((short)buf.Length);
            arr.WriteBytes(buf);
            try
            {
                socket.Send(arr.Buffer);
            }
            catch (Exception)
            {
                this.close();
                Debug.Log("网络错误");

            }
        }

        public override object sync(IoMessage ioMessage)
        {
            throw new NotImplementedException();
        }
    }
}
