﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetTools;
using Newtonsoft.Json;

namespace Our.Shield.Core.Models
{
    public class IpAccessControl
    {
        public enum AccessTypes
        {
            [Description("Allow all client access except for specific ip addresses")]
            AllowAll = 0,

            [Description("Allow no client access except for these specific ip addresses")]
            AllowNone = 1
        }

        /// <summary>
        /// IP Address Model
        /// </summary>
        public class Entry
        {
            /// <summary>
            /// Range or Ip Address with optional Cidr
            /// </summary>
            [JsonProperty("value")]
            public string Value { get; set; }

            internal IPAddressRange Range { get; set; }

            /// <summary>
            /// Optional description
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }
        }

        /// <summary>
        /// What type of access is allowed
        /// </summary>
        [JsonProperty("accessType")]
        public AccessTypes AccessType { get; set; }

        /// <summary>
        /// List of exceptions to the access type
        /// </summary>
        [JsonProperty("exceptions")]
        public IEnumerable<Entry> Exceptions { get; set; }


    }
}
