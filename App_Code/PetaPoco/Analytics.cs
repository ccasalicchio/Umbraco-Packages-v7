using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RevistaUFO.PetaPoco
{
    [TableName("Analytics")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Analytics
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }
        public string IPAddress { get; set; }

        public int NodeId { get; set; }
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Browser { get; set; }
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Resolution { get; set; }

        public DateTime VisitedOn { get; set; }
        [NullSetting(NullSetting = NullSettings.Null)]
        public double VisitLength { get; set; }
        [NullSetting(NullSetting = NullSettings.Null)]
        public string ExitUrl { get; set; }
        [NullSetting(NullSetting = NullSettings.Null)]
        public bool RecurringVisit { get; set; }
    }
}