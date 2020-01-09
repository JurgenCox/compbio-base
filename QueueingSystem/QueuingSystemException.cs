using System;

namespace QueuingSystem
{
    public class QueuingSystemException: Exception
    {
        public readonly int code;
        public readonly string message;

        public QueuingSystemException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public override string ToString(){
            return $"code: {code}, message: \"{message}\"";
        }
    }
}