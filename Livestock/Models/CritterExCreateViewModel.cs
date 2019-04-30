using Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class CritterExCreateViewModel
    {
        public Critter Critter { get; set; }
        public IFormFile File { get; set; }
    }
}
