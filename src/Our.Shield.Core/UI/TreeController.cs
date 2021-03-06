﻿using System.Linq;
using System.Net.Http.Formatting;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using System;
using System.Collections.Generic;
using Umbraco.Web.Models.ContentEditing;

namespace Our.Shield.Core.UI
{
    /// <summary>
    /// The Umbraco Access Tree Controller for the custom section
    /// </summary>
    [PluginController(Constants.App.Name)]
    [Umbraco.Web.Trees.Tree(Constants.App.Alias, Constants.App.Alias, Constants.App.Name)]
    public class TreeController : Umbraco.Web.Trees.TreeController, ISearchableTree
    {
        /// <summary>
        /// Gets the menu for a node by it's Id
        /// </summary>
        /// <param name="id">The Id of the node</param>
        /// <param name="queryStrings">The query string parameters</param>
        /// <returns>Menu Item Collection containing the Menu Item(s)</returns>
        protected override MenuItemCollection GetMenuForNode(string idText, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            int id = int.Parse(idText);

            if (id == global::Umbraco.Core.Constants.System.Root)
            {
                menu.Items.Add<ActionNew>("Create Environment");
                menu.Items.Add<ActionRefresh>("Reload Environments");
                menu.Items.Add<ActionSort>("Sort Environments");

                return menu;
            }

            if (id.Equals(Constants.Tree.DefaultEnvironmentId))
            {
                menu.Items.Add<ActionRefresh>("Reload Apps");

                return menu;
            }

            var environments = Operation.JobService.Instance.Environments;
            
            foreach (var environment in environments)
            {
                if (environment.Key.Id.Equals(id))
                {
                    menu.Items.Add<ActionDelete>("Delete Environment");
                    menu.Items.Add<ActionRefresh>("Reload Apps");

                    return menu;
                }
            }

            return menu;
        }

        /// <summary>
        /// Gets the Tree Node Collection.
        /// </summary>
        /// <param name="id">The Id</param>
        /// <param name="queryStrings">The query string parameters</param>
        /// <returns>Tree Node Collection containing the Tree Node(s)</returns>
        protected override TreeNodeCollection GetTreeNodes(string idText, FormDataCollection queryStrings)
        {
            int id = int.Parse(idText);
            var treeNodeCollection = new TreeNodeCollection();
            var environments = Operation.JobService.Instance.Environments.OrderBy(x => x.Key.SortOrder);
            
            if (id == global::Umbraco.Core.Constants.System.Root)
            {
                if (environments != null && environments.Any())
                {
                    foreach (var environment in environments)
                    {
                        var environmentId = environment.Key.Id.ToString();
                        var node = CreateTreeNode(
                            environmentId,
                            global::Umbraco.Core.Constants.System.Root.ToString(),
                            queryStrings,
                            environment.Key.Name,
                            ((Models.Environment)environment.Key).Icon,
                            environment.Value.Any());

                        if (!environment.Key.Enable)
                        {
                            node.SetNotPublishedStyle();
                        }

                        treeNodeCollection.Add(node);
                    }
                }
                return treeNodeCollection;
            }

            foreach (var environment in environments)
            {
                if (environment.Key.Id == id)
                {
                    foreach (var job in environment.Value.OrderBy(x => x.App.Name))
                    {
                        var jobId = job.Id.ToString();
                        var node = CreateTreeNode(
                            jobId,
                            environment.Key.Id.ToString(),
                            queryStrings,
                            job.App.Name,
                            job.App.Icon,
                            false);

                        if (!job.ReadConfiguration().Enable || !environment.Key.Enable)
                        {
                            node.SetNotPublishedStyle();
                        }
                        treeNodeCollection.Add(node);
                    }
                    return treeNodeCollection;
                }
            }
            return treeNodeCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public IEnumerable<SearchResultItem> Search(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
