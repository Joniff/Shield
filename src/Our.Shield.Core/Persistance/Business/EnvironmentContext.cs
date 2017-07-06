﻿namespace Our.Shield.Core.Persistance.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// The Environment Context.
    /// </summary>
    internal class EnvironmentContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Data.Dto.Environment> List()
        {
            try
            {
                var environments = Database.FetchAll<Data.Dto.Environment>();
                var domains = MapUmbracoDomains(Database.FetchAll<Data.Dto.Domain>());

                foreach (var environment in environments)
                {
                    environment.Domains = domains.Where(x => x.EnvironmentId == environment.Id);
                }

                return environments;
            }
            catch(Exception ex)
            {
                LogHelper.Error(typeof(EnvironmentContext), $"Error listing environments", ex);
                return Enumerable.Empty<Data.Dto.Environment>();
            }
        }

        /// <summary>
        /// Reads a environment from the database.
        /// </summary>
        /// <param name="id">
        /// The id of the configuration.
        /// </param>
        /// <param name="type">
        /// The type of configuration to return;
        /// </param>
        /// <returns>
        /// The Configuration as the desired type.
        /// </returns>
        public Data.Dto.Environment Read(int id)
        {
            try
            {
                var environment = Database.SingleOrDefault<Data.Dto.Environment>((object)id);
                if (environment != null)
                {
                    environment.Domains = MapUmbracoDomains(Database.FetchAll<Data.Dto.Domain>())
                        .Where(x => x.EnvironmentId == environment.Id);
                }
                return environment;
            }
            catch(Exception ex)
            {
                LogHelper.Error(typeof(EnvironmentContext), $"Error reading environment with id: {id}", ex);
                return null;
            }
        }

        /// <summary>
        /// Writes a Configuration to the database.
        /// </summary>
        /// <param name="id">
        /// The id of Configuration to write.
        /// </param>
        /// <param name="config">
        /// The configuration to write to the database
        /// </param>
        /// <returns>
        /// If successfull, returns true, otherwise false.
        /// </returns>
        public bool Write(Data.Dto.Environment environment)
        {
            try
            {
                if (environment.Id == 0 && Database.Exists<Data.Dto.Environment>(environment.Id))
                {
                    Database.Update(environment);
                }
                else
                {
                    environment.Id = (int) Database.Insert(environment);
                }

                return true;
            }
            catch(Exception ex)
            {
                LogHelper.Error(typeof(EnvironmentContext), $"Error writing environment with id: {environment.Id}", ex);
            }
            return false;
        }
    }
}