using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaToGarmin.Client.Strava.Models
{
    public class FrontendAthletesCurrentResponse
    {
        public CurrentAthlete currentAthlete { get; set; }
        public object pageContext { get; set; }
    }

    public class CurrentAthlete
    {
        public int id { get; set; }
        public string external_identity_hash { get; set; }
        public bool super_user { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public int athlete_type { get; set; }
        public bool is_subscriber { get; set; }
        public bool is_trial_eligible { get; set; }
        public string profile_medium { get; set; }
        public string measurement_units { get; set; }
        public object features { get; set; }
        public object experiments { get; set; }
        public object preferences { get; set; }
        public bool in_preview { get; set; }
        public object num_days_remaining_in_preview { get; set; }
        public bool dob_required { get; set; }
    }

}
