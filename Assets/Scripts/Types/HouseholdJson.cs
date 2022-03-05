using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JsonTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Night
    {
        public bool has_dialog { get; set; }
        public List<string> dialog { get; set; }
    }

    public class Day
    {
        public bool has_dialog { get; set; }
    }

    public class HouseholdJson
    {
        public Night night { get; set; }
        public Day day { get; set; }
    }


}
