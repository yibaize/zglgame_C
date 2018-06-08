
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace org.zgl
{
    public class IoMessageHandler
    {
        private static IoMessageHandler instance;
        public static IoMessageHandler getInstance()
        {
            if (instance == null)
                instance = new IoMessageHandler();
            return instance;
        }
        private Queue<IoMessage> messageQueue = new Queue<IoMessage>();
        //消息入队
        public void push(IoMessage ioMessage)
        {
            messageQueue.Enqueue(ioMessage);
        }
        //消息出队
        private IoMessage pop()
        {
            return messageQueue.Dequeue();
        }
        private void handler()
        {
            //这里还要判断是基本数据参数还是PB参数
            IoMessage ioMessage = pop();
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            object obj = assembly.CreateInstance(ioMessage.getInterfaceName()); // 创建类的实例，返回为 object 类
            Type t = obj.GetType();
            if (ioMessage.GetType().IsAssignableFrom(typeof(IoMessageBaseTypeImpl)))
            {
                baseIoMessageHandler(obj, t, ioMessage);
            }
            else if (ioMessage.GetType().IsAssignableFrom(typeof(IoMessageBaseTypeImpl)))
            {
                PBIoMessageHandler(obj, t, ioMessage);
            }
        }
        /**都是基础类型的参数*/
        private void baseIoMessageHandler(object obj, Type t, IoMessage ioMessage)
        {
            try
            {
                MethodInfo method = t.GetMethod(ioMessage.getMethodName());
                ParameterInfo[] parameterInfo = method.GetParameters();
                object[] args = null;
                if (parameterInfo.Length > 0)
                {
                    string[] paramxx = ((string)ioMessage.getData()).Split('$');//先分割$字符串
                    for (int i = 0; i < parameterInfo.Length; i++)
                    {
                        args[i] = TypeExchange.exchange(parameterInfo[i].GetType(), paramxx[i]);
                    }
                }
                method.Invoke(obj, args);
            }
            catch (Exception e)
            {
                Debug.LogError("反射异常");
            }
        }
        /**
         * PB参数
         * */
        private void PBIoMessageHandler(object obj, Type t, IoMessage ioMessage)
        {
            try
            {
                //反射获取具体对象具体方法
                MethodInfo method = t.GetMethod(ioMessage.getMethodName());
                //获取方法所有参数数组
                ParameterInfo[] parameterInfo = method.GetParameters();
                //反射ProtostuffUtils工具类
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                object protoObj = assembly.CreateInstance("ProtostuffUtils"); // 创建类的实例，返回为 object 类
                                                                              //获取工具类类型
                Type protoType = protoObj.GetType();
                //获取带有泛型反序列化的工具类方法
                MethodInfo protoMethodInfo = protoType.GetMethod("ProtobufDeserialize").MakeGenericMethod(parameterInfo[0].GetType());
                //反射调用泛型参数序列化返回序列化后的数据
                object protoResult = protoMethodInfo.Invoke(protoObj, new object[] { ioMessage.getData() });
                //反射调用具体方法
                method.Invoke(obj, new object[] { protoResult });
            }
            catch (Exception e)
            {
                Debug.LogError("反射异常");
            }
        }
    }
}
