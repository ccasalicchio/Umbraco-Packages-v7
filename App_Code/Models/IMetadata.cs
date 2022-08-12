using System;
/// <summary>
/// Summary description for IMetadata
/// </summary>
public interface IMetadata
{
    string meta_title { get; set; }
    string meta_description { get; set; }
    string meta_keywords { get; set; }
    string meta_author { get; set; }
    string Thing_category_ { get; set; }
    string pageImage { get; set; }
    string meta_robots { get; set; }
    string seo_Priority { get; set; }
    string seo_frequency { get; set; }
    string openGraph_images { get; set; }
    string openGraph_type { get; set; }
    string seo_share_title { get; set; }
    string seo_share_text { get; set; }
    string seo_mentions { get; set; }
    string pageUrl { get; set; }
    string umbracoNaviHide { get; set; }
    string umbracoUrlAlias { get; set; }
    string published { get; set; }
}