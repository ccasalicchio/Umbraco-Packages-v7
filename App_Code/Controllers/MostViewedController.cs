using RevistaUFO.Controllers;
using RevistaUFO.Helpers;
using RevistaUFO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace RevistaUFO.Controllers
{
    public class MostViewedController : RenderMvcController
    {
        // GET: MostViewed
        public override ActionResult Index(RenderModel model)
        {
            string nId = Request.QueryString["nId"];
            int take = string.IsNullOrEmpty(Request.QueryString["take"]) ? 5 : int.Parse(Request.QueryString["take"]);

            var mostViewedModel = new MostViewedRenderModel(model.Content, nId, take);
            return base.Index(mostViewedModel);
        }
    }
}

namespace RevistaUFO.Helpers
{
    public class MostViewedComparer : IComparer<MostViewedModel>
    {
        public int Compare(MostViewedModel x, MostViewedModel y)
        {
            if (x.Views > y.Views)
                return -1;
            if (x.Views < y.Views)
                return 1;

            return 0;
        }
    }
}

namespace RevistaUFO.Models
{
    public class MostViewedRenderModel : RenderModel
    {
        public List<MostViewedModel> TopViewed { get; set; }
        new public IPublishedContent Content { get; set; }

        /// <summary>
        /// Constructor to set the IPublishedContent and the CurrentCulture is set by the UmbracoContext
        /// </summary>
        /// <param name="content"></param>
        /// <param name="nId"></param>
        /// <param name="take"></param>
        public MostViewedRenderModel(IPublishedContent content, string nId, int take) : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        {
            UmbracoHelper helper = new UmbracoHelper(UmbracoContext.Current);
            if (TopViewed == null) TopViewed = new List<MostViewedModel>();
            if (string.IsNullOrEmpty(nId)) nId = helper.TypedContentAtRoot().FirstOrDefault().Id.ToString();

            var collectionNode = helper.TypedContent(nId);
            var children = collectionNode.Children;
            var analytics = new AnalyticsApiController();
            var list = new List<MostViewedModel>();
            foreach (var child in children)
            {
                list.Add(new MostViewedModel
                {
                    Views = analytics[child.Id],
                    NodeId = child.Id
                });
            }
            //sort by number of views
            list.Sort(new MostViewedComparer());
            TopViewed.AddRange(list.Take(take));
            this.Content = content;
        }
    }

    public class MostViewedModel
    {
        public int Views { get; set; }
        public int NodeId { get; set; }
    }
}