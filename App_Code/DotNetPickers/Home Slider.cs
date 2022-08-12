using nuPickers.Shared.DotNetDataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web;
using Umbraco.Web.Models;

public class Home_Slider : IDotNetDataSource
{
    //[DotNetDataSource(Title = "Home Slider", Description = "Shows the Highlighted Nodes to be displayed in the Home Slider")]
    //public string ExampleProperty { get; set; }
    UmbracoHelper helper = new UmbracoHelper(UmbracoContext.Current);

    IEnumerable<KeyValuePair<string, string>> IDotNetDataSource.GetEditorDataItems(int contextId)
    {
        // return a collection of key / labels for picker
        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
        DynamicPublishedContent node = helper.Content(contextId);
        var children = node.Descendants().ToList().Where(x => x.GetPropertyValue<int>("Thing_highlight_") == 1 && x.GetPropertyValue<int>("umbracoNaviHide") == 0 && x.GetPropertyValue<DateTime>("Thing_highlight_until_") > DateTime.Now);

        foreach (var child in children) list.Add(new KeyValuePair<string, string>(child.Id.ToString(), child.Name));
        return list;
    }
}