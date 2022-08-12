using System.Collections.Generic;
using System.Linq;
using umbraco.MacroEngines;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
namespace RevistaUFO
{
    /// <summary>
    /// Handles Blog functions
    /// </summary>
    public class RevistaUFOBlog
    {
        private UmbracoHelper helper;
    	private IPublishedContent root;

        public RevistaUFOBlog()
        {
           helper = new UmbracoHelper(UmbracoContext.Current);
           root = helper.TypedContentAtRoot().First();
        }

        public IPublishedContent BlogRoot(IPublishedContent currentNode){
            var id = currentNode.DocumentTypeAlias  == "BlogPost" ? currentNode.Parent.Id : currentNode.Id;
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            IPublishedContent content = umbracoHelper.TypedContent(id);
            return content;
        }
        /*
        dynamic list = root.Descendants("List_Blogs").FirstOrDefault();
    	dynamic posts = new DynamicNode(list.Id).Descendants("Blog").Items;
        */
         public IEnumerable<ITag> AllTags(string author){
            var tagService = ApplicationContext.Current.Services;
            return tagService.TagService.GetAllContentTags();
    	}

        public IEnumerable<TagWithCount> GetTagsInGroup(string group)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;

            const string sql = @"SELECT T.id, T.[group], tag as Text, COUNT(N.id) as NodeCount 
            FROM dbo.cmsTagRelationship TR 
            INNER JOIN dbo.cmsTags T ON T.id = TR.tagId
            INNER JOIN dbo.umbracoNode N ON TR.nodeId = N.id
            WHERE [group] = @0
            AND N.trashed = 0
            GROUP BY T.id, T.[group], tag
            ORDER BY NodeCount desc, tag asc";

            return db.Fetch<TagWithCount>(sql, group);
        }
        public IEnumerable<TagWithCount> GetTagsForBlog(IPublishedContent node)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            int id = node.Id;
           
           const string sql = @"SELECT T.[group], tag as Text, COUNT(N.id) as NodeCount 
                                FROM dbo.cmsTagRelationship TR 
                                INNER JOIN dbo.cmsTags T ON T.id = TR.tagId
                                INNER JOIN dbo.umbracoNode N ON TR.nodeId = N.id
                                WHERE N.parentId = @0
                                AND
                                N.trashed = 0
                                GROUP BY T.[group], tag
                                ORDER BY NodeCount desc, tag asc";

            return db.Fetch<TagWithCount>(sql,id);
        }
       
        public IDictionary<int,string> GetCategoriesForBlog(IPublishedContent node){
            
            dynamic posts = node.DescendantsOrSelf("BlogPost");
            Dictionary<int, string> categories = new Dictionary<int, string>();
            foreach(dynamic post in posts){
                dynamic dyn = post;
                if(post.Thing_category_.ToString().Contains("nuPickers.Picker"))
                    dyn = node;
                
                if(!node.GetPropertyValue<string>("Thing_category_").Contains("nuPickers.Picker")){
                    string[] cats = dyn.Thing_category_.ToString().Split(',');
				     foreach(string cat in cats){
                         if(!categories.ContainsKey(int.Parse(cat))){
					         string name = new DynamicNode(cat).Name;
					         categories.Add(int.Parse(cat),name);
                         }
				     }
                }
             }
			return categories;
        }
    }
    public class TagWithCount
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int NodeCount { get; set; }
        public string Group { get; set; }
    }
}