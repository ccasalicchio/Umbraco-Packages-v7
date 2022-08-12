using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Persistence;
using Umbraco.Web.WebApi;

namespace RevistaUFO.Controllers
{
    /// <summary>
    /// Summary description for SurveyApiController
    /// </summary>
    public class AnalyticsApiController : UmbracoApiController
    {
        private static Database db = null;

        public AnalyticsApiController()
        {
            db = ApplicationContext.DatabaseContext.Database;
        }

        private IEnumerable<Analytics> GetVisitsByNodeId(int nodeId)
        {
            return db.Query<Analytics>("where [nodeId]=" + nodeId);
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
        public IHttpActionResult LogVisit(int nodeId, string ip, string browser = "", string resolution = "", double lengthOfVisit = 0.0d, string exitUrl = "")
        {
            var recurring = AlreadyVisited(nodeId, ip);

            Analytics visit = new Analytics
            {
                NodeId = nodeId,
                IPAddress = ip,
                Browser = browser,
                Resolution = resolution,
                VisitLength = lengthOfVisit,
                VisitedOn = DateTime.Now,
                ExitUrl = exitUrl,
                RecurringVisit = recurring
            };
            db.Insert(visit);

            return Ok(visit);
        }

        private bool AlreadyVisited(int nodeId, string ip)
        {
            return db.ExecuteScalar<int>(string.Format("SELECT COUNT([IPAddress]) FROM [Analytics] WHERE [nodeid] = {0} AND [IPAddress] = \'{1}\'", nodeId, ip)) > 0;
        }
    }
}