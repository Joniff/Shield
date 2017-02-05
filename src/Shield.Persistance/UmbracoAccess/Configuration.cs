﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Shield.Persistance.UmbracoAccess
{
    /// <summary>
    /// The Umbraco Access Configuration serializable object for storing within the database.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Configuration : Bal.IJsonValues
    {
        /// <summary>
        /// Gets or sets the Backend Access URL.
        /// </summary>
        [JsonProperty]
        public string BackendAccessUrl { get; set; }

        /// <summary>
        /// Gets or sets the Status Code.
        /// </summary>
        [JsonProperty]
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or set the Ip Addresses.
        /// </summary>
        [JsonProperty]
        public IEnumerable<string> IpAddresses { get; set; }

        /// <summary>
        /// Gets or sets the Unauthorised URL by XPath.
        /// </summary>
        [JsonProperty]
        public string UnauthorisedUrlXPath { get; set; }

        /// <summary>
        /// Gets or sets the Unauthorised URL Type.
        /// </summary>
        [JsonProperty]
        public int UnauthorisedUrlType { get; set; }

        /// <summary>
        /// Gets or sets the Unauthorised URL by Content Picker.
        /// </summary>
        [JsonProperty]
        public string UnauthorisedUrlContentPicker { get; set; }

        /// <summary>
        /// Gets or sets the Unauthorised URL.
        /// </summary>
        [JsonProperty]
        public string UnauthorisedUrl { get; set; }
    }
}
