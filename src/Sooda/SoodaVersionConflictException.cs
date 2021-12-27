using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sooda
{
    public class SoodaVersionConflictException : SoodaException
    {
        public SoodaVersionConflictException() { }
        public SoodaVersionConflictException(string message) : base(message) { }

        public SoodaVersionConflictException(SoodaObject obj) : this(obj.GetType().Name + "#" + obj.GetPrimaryKeyValue())
        {

        }

        public SoodaVersionConflictException(string message, Exception inner) : base(message, inner) { }
        protected SoodaVersionConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
