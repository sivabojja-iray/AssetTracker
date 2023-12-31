﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    [DataContract]
    public class DataPoint
    {
        //DataContract for Serializing Data - required to serve in JSON format
        public DataPoint(string name, double y)
        {
            this.Name = name;
            this.Y = y;
        }
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "name")]
        public string Name = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}