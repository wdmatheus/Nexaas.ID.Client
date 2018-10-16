using System;

namespace Nexaas.ID.Client
{
    public class NexaasIDException : Exception
    {
        public NexaasIDException() : base()
        {
            
        }
        
        public NexaasIDException(string msg) : base(msg)
        {
            
        }

        public NexaasIDException(string msg, Exception e) : base(msg, e)
        {
            
        }
    }
}