using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class LifeEventEditCommon
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class LifeEventEditDateTime
    {
        public LifeEventEditCommon Common { get; set; }
        public DateTime DateTime { get; set; }
    }
}
