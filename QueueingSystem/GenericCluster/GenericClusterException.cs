namespace QueuingSystem.GenericCluster
{
    public class GenericClusterException: QueuingSystemException
    {
        public GenericClusterException(int code, string message) : base(code, message)
        {
        }
    }
}