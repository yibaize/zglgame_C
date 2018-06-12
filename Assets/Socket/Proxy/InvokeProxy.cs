using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using UnityEngine;

namespace org.zgl
{
    public class InvokeProxy<T> : RealProxy
    {
        private Type type = null;
        public InvokeProxy() : this(typeof(T))
        {
            type = typeof(T);
        }

        protected InvokeProxy(Type classToProxy) : base(classToProxy)
        {
        }

        //接收本地调用请求，然后转发远程访问

        public override IMessage Invoke(IMessage msg)
        {
            try
            {
                IMethodCallMessage callMessage = (IMethodCallMessage)msg;
                MethodInfo methodInfo = (MethodInfo)callMessage.MethodBase;
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                Type returnType = methodInfo.ReturnType;
                IoMessage ioMessage = getIoMessage(type.ToString(), methodInfo.Name, callMessage.Args);
                //远程调用
                object result = request(ioMessage);
                Debug.Log("返回了:"+ result);
                //同步获取返回值
                if (!returnType.ToString().Equals("System.Void"))
                {
                    IoMessage message = (IoMessage)result;
                    //如果不是基础数据类型
                    if (!returnType.IsPrimitive && !returnType.ToString().Equals("System.String"))
                    {
                        //反射ProtostuffUtils工具类
                        Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                        object protoObj = assembly.CreateInstance("org.zgl.ProtostuffUtils"); // 创建类的实例，返回为 object 类
                                                                                      //获取工具类类型
                        Type protoType = protoObj.GetType();
                        //获取带有泛型反序列化的工具类方法
                        MethodInfo protoMethodInfo = protoType.GetMethod("ProtobufDeserialize").MakeGenericMethod(returnType);
                        //反射调用泛型参数序列化返回序列化后的数据
                        object protoResult = protoMethodInfo.Invoke(protoObj, new object[] { message.getData() });
                        //返回最终返回值
                        return returnmessage(callMessage, protoResult);
                    }
                    else if (returnType.GetType().IsPrimitive || returnType.ToString().Equals("System.String"))
                    {
                        //如果是基础数据类型
                        object o = TypeExchange.exchange(returnType, message.getData().ToString());
                        return returnmessage(callMessage, o);
                    }
                }
                else
                {
                    //如果返回类型是void
                    return returnmessage();
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException("远程调用异常", e);
            }
            return null;
        }
        private ReturnMessage returnmessage(IMethodCallMessage callMessage, object callBackResult)
        {
            return new ReturnMessage(callBackResult, new object[0], 0, null, callMessage);
        }
        private ReturnMessage returnmessage()
        {
            return new ReturnMessage(null, null, 0, null, null);
        }
        private IoMessage getIoMessage(string interfaceName, string methodName, params object[] arg)
        {
            IoMessageBaseTypeImpl ioMessage = new IoMessageBaseTypeImpl();
            ioMessage.interfaceName = interfaceName;
            ioMessage.methodName = methodName;
            string args = "";
            for (int i = 0; i < arg.Length; i++)
            {
                args += arg[i];
                if (i != arg.Length - 1)
                {
                    args += "$";
                }
            }
            ioMessage.args = args;
            return ioMessage;
        }
        private object request(IoMessage ioMessage)
        {
            if (typeof(IHttpAsyncService).IsAssignableFrom(type))
            {
                SocketFactory.getInstance().httpAsyncRequest(ioMessage);
                return null;
            }
            else if (typeof(IHttpSyncService).IsAssignableFrom(type))
            {
                return SocketFactory.getInstance().httpSyncRequest(ioMessage);
            }
            else if (typeof(ITcpAsyncService).IsAssignableFrom(type))
            {
                SocketFactory.getInstance().tcpAsyncRequest(ioMessage);
                return null;
            }
            else if (typeof(ITcpSyncService).IsAssignableFrom(type))
            {
                return SocketFactory.getInstance().tcpSyncRequest(ioMessage);
            }
            return null;
        }
    }
}