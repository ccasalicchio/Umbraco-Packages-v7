using Umbraco.Web;


namespace RevistaUFO
{
    //Defaults Class
    public static class Defaults
    {
    	private static UmbracoHelper umbracoHelper = new UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
        //System
        public static string SYSTEM_FULL_DATE_TIME_FORMAT = umbracoHelper.Field("#[Default]sitewide_fullDateTimeFormat").ToString(); 
        public static string SYSTEM_FULL_DATE_FORMAT = umbracoHelper.Field("#[Default]sitewide_fullDateFormat").ToString(); 
        public static string SYSTEM_SHORT_DATE_FORMAT = umbracoHelper.Field("#[Default]sitewide_shortDateFormat").ToString();
        public static string SYSTEM_MONTH_YEAR_DATE_FORMAT = umbracoHelper.Field("#[Default]sitewide_monthYearDateFormat").ToString();
        public static int SYSTEM_MAX_CHAR = int.Parse(umbracoHelper.Field("#[Default]sitewide_format_char_limit").ToString());
        
    }
}
