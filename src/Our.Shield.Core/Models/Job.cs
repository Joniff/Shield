﻿using Our.Shield.Core.Operation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Our.Shield.Core.Models
{
    /// <summary>
    /// Class that conatins each of our executions
    /// </summary>
    internal class Job : IJob
    {
        /// <summary>
        /// The Job Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Job Environment
        /// </summary>
        public IEnvironment Environment { get; set; }

        /// <summary>
        /// The Job App Id
        /// </summary>
        public IApp App { get; set; }

        internal Type ConfigType;

        internal DateTime? LastRan;
        internal Task<bool> Task;
        internal CancellationTokenSource CancelToken;

        internal IJob DeepCopy()
        {
            var app = App<IConfiguration>.Create(this.App.Id);
            app.Migrations = this.App.Migrations;

            return new Job
            {
                Id = this.Id,
                Environment = this.Environment,
                App = app,
                ConfigType = this.ConfigType,
                LastRan = this.LastRan,
                Task = this.Task,
                CancelToken = this.CancelToken
            };
        }

        /// <summary>
        /// Writes the job App's configuration to the database
        /// </summary>
        /// <param name="config">The configuration to write</param>
        /// <returns>True, if successfully written the config to the database; Otherwise, False</returns>
        public bool WriteConfiguration(IConfiguration config) =>
            JobService.Instance.WriteConfiguration(this, config);

        /// <summary>
        /// Writes a journal to the database
        /// </summary>
        /// <param name="journal">The journal to write</param>
        /// <returns>True, if successfully written the journal to the database; Otherwise, False</returns>
        public bool WriteJournal(IJournal journal) =>
            JobService.Instance.WriteJournal(this, journal);

        /// <summary>
        /// Reads the configuration from the database
        /// </summary>
        /// <returns>The configuration for the App</returns>
        public IConfiguration ReadConfiguration() =>
            JobService.Instance.ReadConfiguration(this);

        /// <summary>
        /// Reads a list of Journals from the database
        /// </summary>
        /// <typeparam name="T">The Type of Journal to return</typeparam>
        /// <param name="page">The page of results to return</param>
        /// <param name="itemsPerPage">The number of items to return per page</param>
        /// <returns>Collection of Journals of the desired type</returns>
        public IEnumerable<T> ListJournals<T>(int page, int itemsPerPage, out int totalPages) where T : IJournal =>
            JobService.Instance.ListJournals<T>(this, page, itemsPerPage, out totalPages);

        /// <summary>
        /// Adds a Web Request to WebRequestsHandler collection
        /// </summary>
        /// <param name="regex">The Regex use to match for requests</param>
        /// <param name="beginRequestPriority">The priority of the begin request watch</param>
        /// <param name="beginRequest">The function to call when the Regex matches a request</param>
        /// <param name="endRequestPriority">The priority of the end request watch</param>
        /// <param name="endRequest">The function to call when the regex matches a request</param>
        /// <returns></returns>
        public int WatchWebRequests(PipeLineStages stage, Regex regex, 
            int priority, Func<int, HttpApplication, WatchResponse> request) =>
            WebRequestHandler.Watch(this, stage, regex, priority, request);

        /// <summary>
        /// Removes a Web Requests from the WebRequestHandler collection
        /// </summary>
        /// <param name="regex">The regex of the corresponding Web Request to remove</param>
        /// <returns></returns>
        public int UnwatchWebRequests(PipeLineStages stage, Regex regex) =>
            WebRequestHandler.Unwatch(this, stage, regex);

        /// <summary>
        /// Removes all Web Requests from the WebRequestHandler collection created by this job 
        /// </summary>
        /// <returns></returns>
        public int UnwatchWebRequests(PipeLineStages stage) =>
            WebRequestHandler.Unwatch(this, stage);

        public int UnwatchWebRequests() =>
            WebRequestHandler.Unwatch(this.Environment.Id, this.App.Id);

        /// <summary>
        /// Removes all Web Requests from the WebRequestsHandler collection for the given App
        /// </summary>
        /// <param name="app">The App of the corresponding Web Requests to remove</param>
        /// <returns></returns>
        public int UnwatchWebRequests(IApp app) =>
            WebRequestHandler.Unwatch(app.Id);

        public int ExceptionWebRequest(Regex regex) => WebRequestHandler.Exception(this, regex);
        public int ExceptionWebRequest(UmbracoUrl url) => WebRequestHandler.Exception(this, url);

        public int UnexceptionWebRequest(Regex regex) => WebRequestHandler.Unexception(this, regex);
        public int UnexceptionWebRequest(UmbracoUrl url) => WebRequestHandler.Unexception(this, url);

        public int UnexceptionWebRequest() => WebRequestHandler.Unexception(this);

    }
}
