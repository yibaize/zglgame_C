
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

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
            var request = new UnityWebRequest("http://127.0.0.1:8888/game/handle", "POST");
            byte[] buf = ProtostuffUtils.ProtobufSerialize(ioMessage);
            ByteArray arr = new ByteArray();
            arr.WriteInt(-777888);//包头
            arr.WriteShort(1);
            arr.WriteShort((short)buf.Length);
            arr.WriteBytes(buf);
            request.uploadHandler = new UploadHandlerRaw(arr.Buffer);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/octet-stream");
            request.SendWebRequest();
            Debug.Log("发送完成");
            if (request.responseCode == 200)
            {
                ByteArray byteArray = new ByteArray();
                byteArray.WriteBytes(request.downloadHandler.data);
                int head = byteArray.ReadInt();
                if (head == -777888)
                {
                    short dataSrcRespone = byteArray.ReadShort();//数据类型 1，全是基础数据类型 2，PB类型
                    short resultCode = byteArray.ReadShort();//成功失败码
                    short length = byteArray.ReadShort();
                    byte[] result = byteArray.ReadBytes();
                    if (resultCode == 404) {
                        IoMessageBaseTypeImpl errorMessage = ProtostuffUtils.ProtobufDeserialize<IoMessageBaseTypeImpl>(result);
                        string errMsg = (string)errorMessage.getData();
                        IError error = new ErrorImpl();
                        
                        string[] str = errMsg.Split('$');
                        string[] msg = new string[] { "" };
                        if (str.Length <= 0) {
                            error.err(202, msg);
                            throw new NotImplementedException("远程调用异常");
                        }else if (str.Length > 1)
                        {
                            msg = new string[str.Length - 1];
                            for (int i = 0; i < str.Length; i++)
                            {
                                msg[i] = str[i + 1];
                            }
                        }
                        error.err(short.Parse(str[0]), msg);
                        throw new NotImplementedException("远程调用异常");

                    }
                    switch (dataSrcRespone)
                    {
                        case (short)DataSrcEnum.BaseType:
                            Debug.Log("马上返回");
                            return ProtostuffUtils.ProtobufDeserialize<IoMessageBaseTypeImpl>(result);
                        case (short)DataSrcEnum.PBType:
                            return ProtostuffUtils.ProtobufDeserialize<IoMessagePBTypeImpl>(result);
                    }
                }
            }
            return null;
        }
        public IEnumerator PostUrl(string url, string postData)
        {
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.Send();
                if (www.isError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    // Show results as text    
                    if (www.responseCode == 200)
                    {
                        Debug.Log(www.downloadHandler.text);
                    }
                }
            }
        }
    }
}
