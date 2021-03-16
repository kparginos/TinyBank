using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBank.Model
{
    public class AuditInfo
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public AuditInfo()
        {
            Created = DateTime.Now;
            Updated = Created;
        }

    }
}
