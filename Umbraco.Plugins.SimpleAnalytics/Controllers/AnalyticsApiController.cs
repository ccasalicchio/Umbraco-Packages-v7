using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Umbraco.Core.Persistence;
using Umbraco.Plugins.SimpleAnalytics.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Umbraco.Plugins.SimpleAnalytics.Controllers.Controllers
{
    [PluginController("Analytics")]
    public class AnalyticsApiController : UmbracoApiController
    {
        private static Database db = null;

        public AnalyticsApiController()
        {
            db = ApplicationContext.DatabaseContext.Database;
        }

        private IEnumerable<AnalyticsVisit> GetVisitsByNodeId(int nodeId)
        {
            return db.Query<AnalyticsVisit>("where [nodeId]=" + nodeId);
        }

        public int this[int nodeId]
        {
            get { return GetVisitsByNodeId(nodeId).Count(); }
        }

        [HttpGet]
        public IHttpActionResult GetVisits(int nodeId)
        {
            return Ok(GetVisitsByNodeId(nodeId));
        }

        [HttpGet]
        public IHttpActionResult GetVisitCount(int nodeId)
        {
            return Ok(GetVisitsByNodeId(nodeId).Count());
        }

        [HttpGet]
        public IHttpActionResult LogVisit(string jsonData)
        {
            var thisVisit = JsonConvert.DeserializeObject<AnalyticsVisitModel>(jsonData);
            var visit = GetCurrentVisit(thisVisit.NodeId, thisVisit.IPAddress);
            var recurring = AlreadyVisited(thisVisit.NodeId, thisVisit.IPAddress);

            if (visit == null || visit.Id == 0)
            {
                visit = new AnalyticsVisit
                {
                    NodeId = thisVisit.NodeId,
                    IPAddress = thisVisit.IPAddress,
                    BrowserInfo = JsonConvert.SerializeObject(thisVisit.Browser),
                    Resolution = thisVisit.Resolution,
                    VisitedStarted = DateTime.Now,
                    ExitUrl = thisVisit.ExitUrl,
                    RecurringVisit = recurring
                };
                db.Insert(visit);
            }
            else
            {
                if (!string.IsNullOrEmpty(thisVisit.ExitUrl))
                {
                    visit.ExitUrl = thisVisit.ExitUrl;
                    visit.VisitFinished = DateTime.Now;
                    db.Update(visit);
                }
            }

            return Ok(visit);
        }

        [HttpGet]
        public AnalyticsVisit GetCurrentVisit(int nodeId, string ip)
        {
            string sql = string.Format("SELECT * FROM [" + AnalyticsVisit.TABLENAME + "] WHERE [nodeid] = {0} AND [IPAddress] = \'{1}\' AND [VisitFinished] IS NULL", nodeId, ip);
            return db.Query<AnalyticsVisit>(sql).FirstOrDefault();
        }

        private bool AlreadyVisited(int nodeId, string ip)
        {
            return db.ExecuteScalar<int>(string.Format("SELECT COUNT([IPAddress]) FROM " + AnalyticsVisit.TABLENAME + " WHERE [nodeid] = {0} AND [IPAddress] = \'{1}\'", nodeId, ip)) > 0;
        }
    }
}