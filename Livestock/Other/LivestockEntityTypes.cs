using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Other
{
    /// <summary>
    /// This is for use with the AimLogin user data mappings.
    /// 
    /// Livestock entities use type ids 1000-1999
    /// </summary>
    public class LivestockEntityTypes
    {
        public static int Role => 1000;
        public static int AlUserInfo => 1001;
    }

    /// <summary>
    /// This is for use with the Livestock AdmuGroup data mappings.
    /// </summary>
    public class AdmuGroupEntityTypes
    {
        public static int User => 1;
        public static int Critter => 2;
    }
}
