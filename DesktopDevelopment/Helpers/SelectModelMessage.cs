using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDevelopment.Helpers
{
    public class SelectModelMessage<T>
    {
        public object Recipient { get; set; }
        public T Message { get; set; }
        public SelectModelMessage(object recipient, T message)
        {
            Recipient = recipient;
            Message = message;
        }
    }
}
