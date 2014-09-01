using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDT.Core.MSData
{
    public interface IDomainEvent
    {
        DateTime Timestamp { get; }
    }
}
