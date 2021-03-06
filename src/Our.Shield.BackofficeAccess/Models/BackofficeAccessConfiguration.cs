﻿using Newtonsoft.Json;
using Our.Shield.Core;
using Our.Shield.Core.Attributes;
using Our.Shield.Core.Models;

namespace Our.Shield.BackofficeAccess.Models
{
    /// <summary>
    /// The Backofffice Access Configuration
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BackofficeAccessConfiguration : Configuration
    {
        /// <summary>
        /// The desired backoffice access url
        /// </summary>
        [JsonProperty("backendAccessUrl")]
        [SingleEnvironment]
        public string BackendAccessUrl { get; set; }

        /// <summary>
        /// Client access
        /// </summary>
        [JsonProperty("ipAccessRules")]
        public IpAccessControl IpAccessRules { get; set; }

        /// <summary>
        /// The Url Type Selector and the url
        /// </summary>
        [JsonProperty("unauthorized")]
        public TransferUrl Unauthorized { get; set; }
    }
}
