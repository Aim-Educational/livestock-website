using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Website.Models
{
    public class CritterLifeEventJavascriptInfo
    {
        public string Name;
        public string DataType; // The javascript will call the appropriate callback based off of this.
                                // Note: DataTypes must be converted to lowercase ("DateTime" -> "datetime").
    }

    public class CritterLifeEventTableInfo
    {
        public string Type;
        public string DataType; // Same as JavascriptInfo.DataType
        public string Description;
        public DateTime DateTime;
        public int Id;
    }

    public class CritterExEditViewModel
    {
        public Critter Critter { get; set; }

        // Only needed for the GET web page, POST version of the page doesn't need this.
        public IList<CritterLifeEventJavascriptInfo> Javascript;
        public IList<CritterLifeEventTableInfo> LifeEventTableInfo;

        // Common fields for each POST
        public string EventTypeName { get; set; }
        public string EventDescription { get; set; }

        // The rest of these are specific to the seperate POST functions for the various event data types.
        public CritterLifeEventDatetime DateTime { get; set; }
    }
}
