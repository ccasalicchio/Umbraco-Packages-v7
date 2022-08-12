using Umbraco.Web;
namespace RevistaUFO
{
    //Default Values Class
    public class ValueOrDefault
    {
    	private UmbracoHelper umbracoHelper = new UmbracoHelper(Umbraco.Web.UmbracoContext.Current);

        public string Summary(string text){
            if(text.Length>Defaults.SYSTEM_MAX_CHAR)
            return string.Format("{0} {1}",text.Substring(0,Defaults.SYSTEM_MAX_CHAR),this.Elipsys);
            else return text;
        }

    	public string MonthYearDateFormat{get{
    		var monthYearFormat = umbracoHelper.Field("sitewide_monthYearDateFormat", recursive: true).ToString();
    		if(monthYearFormat == string.Empty)return Defaults.SYSTEM_MONTH_YEAR_DATE_FORMAT;
    		else return monthYearFormat;
    		}}

		public string ShortDateFormat{get{
			var shortDateFormat = umbracoHelper.Field("sitewide_shortDateFormat", recursive: true).ToString();
			if(shortDateFormat == string.Empty)return Defaults.SYSTEM_SHORT_DATE_FORMAT;
			else return shortDateFormat;
			}}

		public string FullDateFormat{get{
			var fullDateFormat = umbracoHelper.Field("sitewide_fullDateFormat", recursive: true).ToString();
			if(fullDateFormat == string.Empty)return Defaults.SYSTEM_FULL_DATE_FORMAT;
			else return fullDateFormat;
			}}

		public string FullDateTimeFormat{get{
			var fullDateTimeFormat = umbracoHelper.Field("sitewide_fullDateTimeFormat", recursive: true).ToString();
			if(fullDateTimeFormat == string.Empty)return Defaults.SYSTEM_FULL_DATE_TIME_FORMAT;
			else return fullDateTimeFormat;
			}}

		public int MaxChar{get{
			var maxChar = int.Parse(umbracoHelper.Field("sitewide_format_char_limit", recursive: true).ToString());
			if(maxChar == 0)return Defaults.SYSTEM_MAX_CHAR;
			else return maxChar;
			}}

		public bool HasElipsys{get{return bool.Parse(umbracoHelper.Field("sitewide_display_elipsys", recursive: true).ToString());}}

		public string Elipsys {get{return HasElipsys?"...":"";}}
	}
    }