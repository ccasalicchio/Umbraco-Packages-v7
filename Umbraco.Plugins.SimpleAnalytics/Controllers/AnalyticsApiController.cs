using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using Umbraco.Core.Persistence;
using Umbraco.Plugins.SimpleAnalytics.Extensions;
using Umbraco.Plugins.SimpleAnalytics.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Umbraco.Plugins.SimpleAnalytics.Controllers.Controllers
{
    [PluginController("Analytics")]
    public class AnalyticsApiController : UmbracoApiController
    {
        private static Database db = null;
        private const string IP_DATABASE = "IP2LOCATION-LITE-DB11.BIN";
        private const string PLUGIN_PATH = "~/App_Plugins/VisitCounterDashboard";
        private const string PUBLIC_IP_ENDPOINT = "https://api.ipify.org?format=text";

        private IEnumerable<AnalyticsVisit> GetVisitsByNodeId(int nodeId)
        {
            db = ApplicationContext.DatabaseContext.Database;

            return db.Query<AnalyticsVisit>("where [nodeId]=" + nodeId).ToList();
        }

        public int this[int nodeId]
        {
            get { return GetVisitsByNodeId(nodeId).Count(); }
        }

        [HttpGet]
        public IHttpActionResult GetVisits(int nodeId)
        {
            db = ApplicationContext.DatabaseContext.Database;

            return Ok(GetVisitsByNodeId(nodeId));
        }

        [HttpGet]
        public IEnumerable<VisitStats> GetResultsBy(string filter)
        {
            var serverPath = Umbraco.UmbracoContext.HttpContext.Server.MapPath(PLUGIN_PATH);
            var path = Path.Combine(serverPath, IP_DATABASE);
            string sqlBaseCount = string.Format("SELECT IPAddress, \'{1}\' AS FILTER, COUNT(*) AS COUNT FROM {0} GROUP BY [IPAddress]", AnalyticsVisit.TABLENAME, filter);
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.Query<VisitStats>(sqlBaseCount).ToList();
            var list = new List<VisitStats>();
            foreach (var result in results)
            {
                var visitStats = new VisitStats
                {
                    Mapping = Ip2LocationExtensions.GetIpMapping(result.IPAddress, path),
                    Count = result.Count,
                    Filter = result.Filter,
                    IPAddress = result.IPAddress
                };
                list.Add(visitStats);
            }
            return list.Where(x => x.Filter == filter);
        }

        [HttpGet]
        public int GetRecurringVisits()
        {
            string sqlBaseCount = string.Format("SELECT COUNT(*) AS COUNT FROM {0} WHERE RecurringVisit = 'TRUE'", AnalyticsVisit.TABLENAME);
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.ExecuteScalar<int>(sqlBaseCount);
            return results;
        }

        [HttpGet]
        public KeyValuePair<DateTime, int> GetResultsByDate(DateTime date)
        {
            string sqlBaseCount = string.Format("SELECT COUNT(*) FROM {0} WHERE VisitedStarted BETWEEN '{1} 00:00:00' and '{1} 23:59:59'", AnalyticsVisit.TABLENAME, date.Date.ToString("yyyy-MM-dd"));
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.ExecuteScalar<int>(sqlBaseCount);
            return new KeyValuePair<DateTime, int>(date, results);
        }

        [HttpGet]
        public KeyValuePair<DateTime, int>[] GetResultsXDays(int days)
        {
            var startDay = DateTime.Now.AddDays(-(days)).Date;
            var currDay = startDay;
            KeyValuePair<DateTime, int>[] results = new KeyValuePair<DateTime, int>[days];
            for (int i = 0; i < days; i++)
            {
                results[i] = GetResultsByDate(currDay);
                currDay = currDay.AddDays(1);
            }
            return results;
        }

        [HttpGet]
        public int GetRealTimeVisits()
        {
            string sqlBaseCount = string.Format("SELECT COUNT(*) AS COUNT FROM {0} WHERE RecurringVisit = 'FALSE'", AnalyticsVisit.TABLENAME);
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.ExecuteScalar<int>(sqlBaseCount);
            return results;
        }

        [HttpGet]
        public int GetTotalVisits()
        {
            string sqlBaseCount = string.Format("SELECT COUNT(*) FROM {0}", AnalyticsVisit.TABLENAME);
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.ExecuteScalar<int>(sqlBaseCount);
            return results;
        }

        [HttpGet]
        public List<VisitFilter> GetVisitsByEntryUrl()
        {
            string sqlEntryUrl = string.Format("SELECT [NodeId] AS Filter, COUNT(*) AS COUNT FROM {0} GROUP BY [NodeId]", AnalyticsVisit.TABLENAME);
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.Query<VisitFilter>(sqlEntryUrl).ToList();
            var list = new List<VisitFilter>();
            foreach (var result in results)
            {
                var entryNode = Umbraco.Content(result.Filter);
                var visitStats = new VisitFilter
                {
                    Count = result.Count,
                    Filter = entryNode.Url()
                };
                list.Add(visitStats);
            }
            return list;
        }

        [HttpGet]
        public List<VisitFilter> GetVisitsByExitUrl()
        {
            string sqlExitUrl = string.Format("SELECT [ExitUrl] AS Filter, COUNT(*) AS COUNT FROM {0} WHERE [ExitUrl] <> '' GROUP BY [ExitUrl]", AnalyticsVisit.TABLENAME);
            db = ApplicationContext.DatabaseContext.Database;

            var results = db.Query<VisitFilter>(sqlExitUrl).ToList();
            var list = new List<VisitFilter>();
            foreach (var result in results)
            {
                var visitStats = new VisitFilter
                {
                    Count = result.Count,
                    Filter = result.Filter
                };
                list.Add(visitStats);
            }
            return list;
        }

        [HttpGet]
        public PagedResults<AnalyticsVisitModel> GetPagedResults(int page = 1, int pageSize = 10, string ipAddress = "")
        {
            db = ApplicationContext.DatabaseContext.Database;
            string sqlBase = string.Format("SELECT * FROM {0}", AnalyticsVisit.TABLENAME);
            string sqlBaseCount = string.Format("SELECT COUNT(*) FROM {0}", AnalyticsVisit.TABLENAME);
            PagedResults<AnalyticsVisitModel> pagedResults = new PagedResults<AnalyticsVisitModel>();
            var serverPath = Umbraco.UmbracoContext.HttpContext.Server.MapPath(PLUGIN_PATH);
            var path = Path.Combine(serverPath, IP_DATABASE);

            if (!string.IsNullOrEmpty(ipAddress))
            {
                var sqlWhere = " WHERE 1 = 1";
                var sqlIp = !string.IsNullOrEmpty(ipAddress) ? " AND [IPAddress] = '" + ipAddress + "'" : "";
                sqlBase += sqlWhere + sqlIp;
                sqlBaseCount += sqlWhere + sqlIp;
            }
            int count = db.ExecuteScalar<int>(sqlBaseCount);
            string sql = sqlBase + string.Format(" ORDER BY Id OFFSET({0}) ROWS FETCH NEXT({1}) ROWS ONLY", (page - 1) * pageSize, pageSize);
            var results = db.Query<AnalyticsVisit>(sql).ToList();
            foreach (var result in results)
            {
                var node = Umbraco.Content(result.NodeId);
                var info = new AnalyticsVisitModel
                {
                    Browser = JsonConvert.DeserializeObject<BrowserInfo>(result.BrowserInfo),
                    BrowserInfo = result.BrowserInfo,
                    ExitUrl = result.ExitUrl,
                    Id = result.Id,
                    IPAddress = result.IPAddress,
                    IPMapping = Ip2LocationExtensions.GetIpMapping(result.IPAddress, path),
                    NodeId = result.NodeId,
                    NodeName = node.Name,
                    EntryUrl = node.Url,
                    RecurringVisit = result.RecurringVisit,
                    Resolution = result.Resolution,
                    VisitedStarted = result.VisitedStarted,
                    VisitFinished = result.VisitFinished,
                    TotalVisits = count,
                    VisitLength = result.VisitedStarted != null && result.VisitFinished != null ? (result.VisitFinished - result.VisitedStarted).Value.ToString(@"hh\:mm\:ss") : "",
                    UserAgent = BrowserExtensions.GetBrowserInfo(result.BrowserInfo),
                };

                info.Browser.LanguageName = !string.IsNullOrEmpty(info.Browser.Language) ? new CultureInfo(info.Browser.Language).DisplayName : "";
                info.Browser.LanguageFlag = !string.IsNullOrEmpty(info.Browser.Language) ? info.Browser.Language.Substring(3) + ".png" : "";
                info.Browser.OS = BrowserExtensions.ParseOS(info.UserAgent.Platform);

                pagedResults.Results.Add(info);
                pagedResults.Found = count;
                pagedResults.PageNumber = page;
                pagedResults.PageSize = pageSize;
                pagedResults.Query = ipAddress;
            }

            return pagedResults;
        }

        [HttpGet]
        public IHttpActionResult GetVisitCount(int nodeId)
        {
            db = ApplicationContext.DatabaseContext.Database;

            return Ok(GetVisitsByNodeId(nodeId).Count());
        }

        [HttpGet]
        public async Task<IHttpActionResult> LogVisit(string jsonData)
        {
            db = ApplicationContext.DatabaseContext.Database;

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
            await Task.FromResult(0);
            return Ok(visit);
        }

        [HttpGet]
        public AnalyticsVisit GetCurrentVisit(int nodeId, string ip)
        {
            string sql = string.Format("SELECT * FROM [" + AnalyticsVisit.TABLENAME + "] WHERE [nodeid] = {0} AND [IPAddress] = \'{1}\' AND [VisitFinished] IS NULL", nodeId, ip);
            db = ApplicationContext.DatabaseContext.Database;

            return db.Query<AnalyticsVisit>(sql).FirstOrDefault();
        }

        private bool AlreadyVisited(int nodeId, string ip)
        {
            db = ApplicationContext.DatabaseContext.Database;

            return db.ExecuteScalar<int>(string.Format("SELECT COUNT([IPAddress]) FROM " + AnalyticsVisit.TABLENAME + " WHERE [nodeid] = {0} AND [IPAddress] = \'{1}\'", nodeId, ip)) > 0;
        }
    }
}