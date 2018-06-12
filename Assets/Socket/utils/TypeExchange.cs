
using System;
namespace org.zgl
{
    public class TypeExchange
    {
        public static object exchange(Type t, string param)
        {
            if (t == typeof(int))
            {
                return int.Parse(param);
            }
            else if (t == typeof(long))
            {
                return long.Parse(param);
            }
            else if (t == typeof(float))
            {
                return float.Parse(param);
            }
            else if (t == typeof(double))
            {
                return double.Parse(param);
            }
            else if (t == typeof(short))
            {
                return short.Parse(param);
            }
            else if (t == typeof(char))
            {
                return char.Parse(param);
            }
            else if (t == typeof(bool))
            {
                return bool.Parse(param);
            }
            else if (t == typeof(byte))
            {
                return byte.Parse(param);
            }
            else if (t == typeof(string)) {
                return param;
            }
            throw new SystemException("数据参数转换异常");
        }
    }
}
