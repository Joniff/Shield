﻿namespace Shield.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    /// <summary>
    /// Api Controller for the Umbraco Access area of the custom section
    /// </summary>
    /// <example>
    /// Endpoint: /umbraco/backoffice/Shield/ShieldApi/{Action}
    /// </example>
    [PluginController(Constants.App.Alias)]
    public class ShieldApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Api Endpoint for Posting the Umbraco Access Configuration.
        /// </summary>
        /// <param name="id">
        /// The Id of the configuration
        /// </param>
        /// <param name="model">
        /// The new configuration settings.
        /// </param>
        /// <example>
        /// Endpoint: /umbraco/backoffice/Shield/ShieldApi/PostConfiguration
        /// </example>
        /// <returns>
        /// Whether was successfully updated.
        /// </returns>
        [HttpPost]
        public bool PostConfiguration(string id, string model)
        {
            var op = Models.Operation<Models.Configuration>.Create(id);

            var curConfig = op.ReadConfiguration();

            Models.Configuration newConfig = Newtonsoft.Json.JsonConvert.DeserializeObject(model, curConfig.GetType()) as Models.Configuration;

            if (!op.Execute(newConfig))
            {
                // oh well, leave for polling to try and update
            }

            var curUmbracoUser = UmbracoContext.Security.CurrentUser;

            op.WriteJournal(new Models.Journal
            {
                Datestamp = DateTime.UtcNow,
                Message = $"{curUmbracoUser.Name} has updated configuration."
            });

            return op.WriteConfiguration(newConfig);
        }


        /// <summary>
        /// Api Endpoint for Getting the Umbraco Access Configuration.
        /// </summary>
        /// <param name="id">
        /// Id Of the configuration to return
        /// </param>
        /// <example>
        /// Endpoint: /umbraco/backoffice/Shield/ShieldApi/GetConfiguration?id={Id}
        /// </example>
        /// <returns>
        /// The configuration for the Umbraco Access area.
        /// </returns>
        [HttpGet]
        public JsonNetResult GetConfiguration(string id)
        {
            var op = Models.Operation<Models.Configuration>.Create(id);

            if(op == null)
            {
                return new JsonNetResult
                {
                    Data = null
                };
            }

            var config = op.ReadConfiguration();

            return new JsonNetResult
            {
                Data = config
            };
        }

        /// <summary>
        /// Api Endpoint for getting the Umbraco Access Journals
        /// </summary>
        /// <param name="id">
        /// Id of the configuration to return journals for
        /// </param>
        /// <param name="page">
        /// The page of results to return
        /// </param>
        /// <param name="itemsPerPage">
        /// Number of items per page
        /// </param>
        /// <returns>
        /// Collection of journals for the desired configuration
        /// </returns>
        [HttpGet]
        public JsonNetResult GetJournals(string id, int page, int itemsPerPage)
        {
            var op = Models.Operation<Models.Configuration>.Create(id);

            if(op == null)
            {
                return new JsonNetResult
                {
                    Data = null
                };
            }

            var journals = op.ReadJournals(page, itemsPerPage).ToArray();

            return new JsonNetResult
            {
                Data = journals
            };
        }
    }
}
