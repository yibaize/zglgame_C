
namespace org.zgl
{
    public class ErrorImpl : IError
    {
        public void err(short errorCode, string[] msg)
        {
            string st = "";//从静态表中获取
            string str = string.Format(st, msg);
            throw new System.Exception(errorCode+"");
      
}
    }
}
