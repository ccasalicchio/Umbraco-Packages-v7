using System;
using System.ComponentModel.DataAnnotations;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;


namespace Umbraco.Plugins.SimpleAnalytics.Models
{
    [TableName(TABLENAME)]
    [PrimaryKey("Id", autoIncrement = true)]
    public class AnalyticsVisit
    {
        public const string TABLENAME = "AnalyticsVisits";
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string IPAddress { get; set; }

        public int NodeId { get; set; }

        [SpecialDbType(SpecialDbTypes.NTEXT), NullSetting(NullSetting = NullSettings.Null)]
        public string BrowserInfo { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Resolution { get; set; }

        public DateTime VisitedStarted { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? VisitFinished { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string ExitUrl { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public bool RecurringVisit { get; set; }
    }
}
