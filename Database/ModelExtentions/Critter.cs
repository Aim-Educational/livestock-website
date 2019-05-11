using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Bit flags that are used to determine the reproduction capabilities of a critter.
    /// </summary>
    [Flags]
    public enum ReproduceFlags
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The flag that is set if a *user* determines that the critter can reproduce.
        /// 
        /// 0 = User says they can't.
        /// 1 = User says they can.
        /// </summary>
        YesReproduceUser = 1 << 0,

        /// <summary>
        /// The flag that is set if the critter has been castrated.
        /// 
        /// If this flag is true, no other flags can be used, since bioglogically they simply can't reproduce.
        /// </summary>
        NoReproduceCastrate = 1 << 1,

        /// <summary>
        /// The flag that is set if the critter is too old to reproduce.
        /// </summary>
        NoReproduceTooOld = 1 << 2
    }
}
