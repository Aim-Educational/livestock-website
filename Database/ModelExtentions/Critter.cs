using System;
using System.Collections.Generic;
using System.Linq;
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

    public partial class Critter
    {
        public void UpdateReproduceFlag(ReproduceFlags flag, bool setOrUnset)
        {
            if(setOrUnset)
                this.ReproduceFlags |= (int)flag;
            else
                this.ReproduceFlags &= ~(int)flag;
        }

        public bool CanReproduce =>
            (this.ReproduceFlags & (int)Models.ReproduceFlags.NoReproduceCastrate) == 0
         && (this.ReproduceFlags & (int)Models.ReproduceFlags.NoReproduceTooOld) == 0
         && (this.ReproduceFlags & (int)Models.ReproduceFlags.YesReproduceUser) > 0;

        public bool CanSafelyDelete(out string reasonIfFalse)
        {
            if (this.InverseDadCritter == null)
                throw new InvalidOperationException($"Please make sure to .Include() the 'InverseDadCritter' property.");

            if (this.InverseMumCritter == null)
                throw new InvalidOperationException($"Please make sure to .Include() the 'InverseMumCritter' property.");

            if(this.CritterLifeEvent == null)
                throw new InvalidOperationException($"Please make sure to .Include() the 'CritterLifeEvent' property.");

            if(this.InverseDadCritter.Count > 0 || this.InverseMumCritter.Count > 0)
            {
                reasonIfFalse = $"This Critter is a parent of another critter. TODO: Display which critters.";
                return false;
            }

            // TODO: Life event checks to see if they can't be deleted.
            reasonIfFalse = null;
            return true;
        }
    }
}
