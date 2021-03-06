﻿using Newtonsoft.Json;
using Our.Shield.Core;
using Our.Shield.Core.Models;

namespace Our.Shield.FrontendAccess.Models
{
    public class FrontendAccessConfiguration : Configuration
    {
        /// <summary>
        /// Whether or not the Frontend can be accessed when the request is coming from an authenticated umbraco user
        /// </summary>
        [JsonProperty("umbracoUserEnable")]
        public bool UmbracoUserEnable { get; set; }

        /// <summary>
        /// Client access
        /// </summary>
        [JsonProperty("ipAccessRules")]
        public IpAccessControl IpAccessRules { get; set; }

        /// <summary>
        /// Where to send unauthorized users too
        /// </summary>
        [JsonProperty("unauthorized")]
        public TransferUrl Unauthorized { get; set; }
    }
}
