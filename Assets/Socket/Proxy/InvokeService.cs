namespace org.zgl
{
    public class InvokeService
    {
        public static T Proxy<T>()
        {
            var proxy = new InvokeProxy<T>();
            return (T)proxy.GetTransparentProxy();
        }
    }
}
