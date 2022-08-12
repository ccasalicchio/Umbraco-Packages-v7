using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevistaUFO.Models
{
    public class RelatedLink
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public bool NewWindow { get; set; }
        public string Target { get { return NewWindow ? "_blank" : null; } }

        public RelatedLink(string text, string url, bool newWindow)
        {
            Text = text;
            Url = url;
            NewWindow = newWindow;
        }
    }
}