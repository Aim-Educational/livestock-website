using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Website.Models
{
    public class GroupIndexInfo
    {
        public AdmuGroup Group { get; set; }
        public int GroupMemberCount;
    }

    public class GroupIndexViewModel
    {
        public IEnumerable<GroupIndexInfo> Groups { get; set; }
        public List<int> TEMP { get; set; }
    }
}
