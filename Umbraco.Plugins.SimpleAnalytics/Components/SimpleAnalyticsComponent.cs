using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Plugins.SimpleAnalytics.Dashboards;
using Umbraco.Plugins.SimpleAnalytics.Models;
using Umbraco.Tools.ConfigurationActions.Extensions;

namespace Umbraco.Plugins.SimpleAnalytics.Components
{

    public class SimpleAnalyticsComponent : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var logger = LoggerResolver.Current.Logger;
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = new DatabaseSchemaHelper(dbContext.Database, logger, dbContext.SqlSyntax);

            //Analytics
            if (!db.TableExist(AnalyticsVisit.TABLENAME))
            {
                db.CreateTable<AnalyticsVisit>(false);
            }

            var simpleAnalyticsDahboard = new AnalyticsDashboard();
            simpleAnalyticsDahboard.InstallDashboard();
        }
    }
}
