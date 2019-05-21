﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Bit flags for a critter.
    /// </summary>
    [Flags]
    public enum CritterFlags
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None = 0,

        /// <summary>
        /// [Reproduction]
        /// The flag that is set if a *user* determines that the critter can reproduce.
        /// 
        /// 0 = User says they can't.
        /// 1 = User says they can.
        /// </summary>
        ReproduceYesUser = 1 << 0,

        /// <summary>
        /// [Reproduction]
        /// The flag that is set if the critter can no longer reproduce due to a life event (e.g. castration).
        /// 
        /// If this flag is true, no other [Reproduction] flags can be used, since (in most cases) bioglogically they simply can't reproduce.
        /// </summary>
        ReproduceNoLifeEvent = 1 << 1,

        /// <summary>
        /// [Reproduction]
        /// The flag that is set if the critter is too old to reproduce.
        /// </summary>
        ReproduceNoTooOld = 1 << 2
    }

    public partial class Critter
    {
        public void UpdateFlag(CritterFlags flag, bool setOrUnset)
        {
            if(setOrUnset)
                this.Flags |= (int)flag;
            else
                this.Flags &= ~(int)flag;
        }

        public bool HasFlag(CritterFlags flag) => (this.Flags & (int)flag) > 0;

        public bool CanReproduce =>
            (this.Flags & (int)CritterFlags.ReproduceNoLifeEvent) == 0
         && (this.Flags & (int)CritterFlags.ReproduceNoTooOld) == 0
         && (this.Flags & (int)CritterFlags.ReproduceYesUser) > 0;

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
