using System.Collections.Generic;

namespace Umbraco.Plugins.SimpleAnalytics.Models
{
    public class PagedResults<T> where T : class
    {
        public PagedResults()
        {
            Results = new List<T>();
        }

        public IList<T> Results { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Found { get; set; }
        public string Query { get; set; }
    }
}
