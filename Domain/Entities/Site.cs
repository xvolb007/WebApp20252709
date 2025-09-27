using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Site
    {
        public long Id { get; set; } // Primary key
        public DateTimeOffset CreatedAt { get; set; }
    }

}
