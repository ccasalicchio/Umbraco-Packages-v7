using System.IO;
using System.Net;
namespace RevistaUFO.Helpers
{
    /// <summary>
    /// Summary description for API
    /// </summary>
    public class API
    {
        public API()
        {

        }
        public string postStringData(string destination, string query, ContentType type)
        {
            string contentType = string.Empty;

            switch (type)
            {
                case ContentType.UTF_8:
                    contentType = "text/xml; encoding='utf-8'";
                    break;
                case ContentType.ISO_8859_1:
                    contentType = "application/xml; charset=ISO-8859-1";
                    break;
                default:
                    contentType = "text/xml; encoding='utf-8'";
                    break;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destination + query);
            request.ContentType = contentType;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            else return null;
        }


        /// <summary>
        /// Sends XML via WebRequest
        /// </summary>
        /// <param name="destinationUrl">the Url to send the request</param>
        /// <param name="requestXml">The xml to be sent</param>
        /// <param name="type">Type of Content and Encoding</param>
        /// <returns>String with WebResponse</returns>
        public string postXMLData(string destinationUrl, string requestXml, ContentType type)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);

            string contentType = string.Empty;

            switch (type)
            {
                case ContentType.UTF_8:
                    contentType = "text/xml; encoding='utf-8'";
                    break;
                case ContentType.ISO_8859_1:
                    contentType = "application/xml; charset=ISO-8859-1";
                    break;
                default:
                    contentType = "text/xml; encoding='utf-8'";
                    break;
            }

            request.ContentType = contentType;
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                return responseStr;
            }
            return null;
        }
        public enum ContentType
        {
            UTF_8,
            ISO_8859_1
        }
    }
}