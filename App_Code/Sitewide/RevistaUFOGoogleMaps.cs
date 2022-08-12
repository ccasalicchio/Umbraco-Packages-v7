using Newtonsoft.Json;
using System.Text;

namespace RevistaUFO.GoogleMaps
{
    /*RESPONSE
   {
   "results" : [
      {
         "address_components" : [
            {
               "long_name" : "277", //[0]
               "short_name" : "277",
               "types" : [ "street_number" ]
            },
            {
               "long_name" : "Bedford Avenue", //[1]
               "short_name" : "Bedford Ave",
               "types" : [ "route" ]
            },
            {
               "long_name" : "Williamsburg", //[2]
               "short_name" : "Williamsburg",
               "types" : [ "neighborhood", "political" ]
            },
            {
               "long_name" : "Brooklyn", //[3]
               "short_name" : "Brooklyn",
               "types" : [ "sublocality", "political" ]
            },
            {
               "long_name" : "Kings", //[4]
               "short_name" : "Kings",
               "types" : [ "administrative_area_level_2", "political" ]
            },
            {
               "long_name" : "New York", //[5]
               "short_name" : "NY",
               "types" : [ "administrative_area_level_1", "political" ]
            },
            {
               "long_name" : "United States", //[6]
               "short_name" : "US",
               "types" : [ "country", "political" ]
            },
            {
               "long_name" : "11211", //[7]
               "short_name" : "11211",
               "types" : [ "postal_code" ]
            }
         ],
         "formatted_address" : "277 Bedford Avenue, Brooklyn, NY 11211, USA",
         "geometry" : {
            "location" : {
               "lat" : 40.714232,
               "lng" : -73.9612889
            },
            "location_type" : "ROOFTOP",
            "viewport" : {
               "northeast" : {
                  "lat" : 40.7155809802915,
                  "lng" : -73.9599399197085
               },
               "southwest" : {
                  "lat" : 40.7128830197085,
                  "lng" : -73.96263788029151
               }
            }
         },
         "place_id" : "ChIJd8BlQ2BZwokRAFUEcm_qrcA",
         "types" : [ "street_address" ]
      },

  ... Additional results[] ...*/
    /// <summary>
    /// Class that handles google maps data
    /// </summary>
    public class GoogleGeoCodeResponse
	{

		public string status { get; set; }
		public results[] results { get; set; }

	}

	public class results
	{
		public string formatted_address { get; set; }
		public geometry geometry { get; set; }
		public string[] types { get; set; }
		public address_component[] address_components { get; set; }
	}

	public class geometry
	{
		public string location_type { get; set; }
		public location location { get; set; }
	}

	public class location
	{
		public string lat { get; set; }
		public string lng { get; set; }
	}

	public class address_component
	{
		public string long_name { get; set; }
		public string short_name { get; set; }
		public string[] types { get; set; }
	}

    public class GoogleMaps{
        public GoogleGeoCodeResponse GetMapFromAddress(string address){
            var google_address = string.Format("http://maps.google.com/maps/api/geocode/json?address={0}&language=pt-BR&sensor=false",address);
            string result = string.Empty;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                result = wc.DownloadString(google_address);
            }
            return JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);
        }
        public GoogleGeoCodeResponse GetMap(string latitude, string longitude){
            var address = string.Format("http://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&language=pt-BR&sensor=false",latitude,longitude);
            string result = string.Empty;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                try{
                   result = wc.DownloadString(address); 
                }
                catch{
                    result = "";
                }
            }
            return JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);
        }
        public GoogleGeoCodeResponse GetMap(string latlong){
            var loc = latlong.Split(',');
            return GetMap(loc[0],loc[1]);
        }
    }
}