
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkingCrm.Model
    
{
    public class BirthdayTrackerModel
    {
        public string Name { get; set; }
        public DateTime DateofBirthday { get; set; }

        public int DayOfYear { get; set; }

        public string UserId { get; set; }

    }
}
