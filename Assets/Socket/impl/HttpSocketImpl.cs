
using UnityEngine;
namespace org.zgl
{
    public class HttpSocketImpl : AbstractSocket
    {
        public override void async(IoMessage ioMessage)
        {
            throw new System.NotImplementedException();
        }
        public override object sync(IoMessage ioMessage)
        {
            byte[] buf = ProtostuffUtils.ProtobufSerialize(ioMessage);
            ByteArray arr = new ByteArray();
            arr.WriteInt(-777888);//包头
            arr.WriteShort(dataSrc);
            arr.WriteShort((short)buf.Length);
            arr.WriteBytes(buf);
            using (WWW www = new WWW("", arr.Buffer))
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.Log(www.error);
                }
                else
                {
                    ByteArray byteArray = new ByteArray();

                    byteArray.WriteBytes(www.bytes);
                    int head = byteArray.ReadInt();
                    if (head == -777888)
                    {
                        short dataSrcRespone = byteArray.ReadShort();//数据类型 1，全是基础数据类型 2，PB类型
                        short length = byteArray.ReadShort();
                        byte[] result = byteArray.ReadBytes();
                        switch (dataSrcRespone)
                        {
                            case (short)DataSrcEnum.BaseType:
                                baseType(buf);
                                break;
                            case (short)DataSrcEnum.PBType:
                                pbType(buf);
                                break;
                        }
                    }

                }
            }
            return null;
        }
    }
}
