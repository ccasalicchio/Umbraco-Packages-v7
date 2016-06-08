using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace SocialMediaChannels.Helpers
{
    public static class MediaHelper
    {
        #region Properties
        private readonly static int LABEL_ID = -92;
        private readonly static int UPLOAD_ID = -90;
        private readonly static int TEXT_ID = -88;
        private readonly static int DATE_ID = -41;
        private readonly static int IMAGE_ID = 1031;
        private readonly static int FOLDER_ID = 1032;
        private readonly static string APP_PLUGIN = "~/App_Plugins/";
        #endregion


        #region Social Media Channel Themes

        private readonly static string SOCIAL_MEDIA_THEME_NAME = "Social Media Theme";
        private readonly static string SOCIAL_MEDIA_THEME_FOLDER_NAME = "Social Media Channels Themes";
        private readonly static string SOCIAL_MEDIA_THEME_ALIAS = "socialMediaThemeType";
        private readonly static string SOCIAL_MEDIA_CHANNELS_FOLDER = Path.Combine(APP_PLUGIN, "Social.Media.Channels/themes/");
        public static void AddSocialMediaChannelMediaType()
        {
            MediaType mediaType = new MediaType(-1);
            mediaType.AllowedAsRoot = true;
            mediaType.Name = SOCIAL_MEDIA_THEME_NAME;
            mediaType.Description = "Container for the Social Media Channel Theme Images";
            mediaType.IsContainer = true;
            mediaType.Icon = "icon-picture";
            mediaType.Alias = SOCIAL_MEDIA_THEME_ALIAS;

            //Allowed child nodes
            var children = new List<ContentTypeSort>
                {
                    new ContentTypeSort(FOLDER_ID, 0),
                    new ContentTypeSort(IMAGE_ID, 1)
                };

            mediaType.AllowedContentTypes = children;


            //Add properties
            DataTypeService dataTypeService = (DataTypeService)ApplicationContext.Current.Services.DataTypeService;

            var name = new PropertyType(dataTypeService.GetDataTypeDefinitionById(TEXT_ID), "themeName");
            name.Name = "Theme Name";
            name.Description = "Name for the theme";
            name.SortOrder = 0;

            var url = new PropertyType(dataTypeService.GetDataTypeDefinitionById(TEXT_ID), "themeUrl");
            url.Name = "Theme Url";
            url.Description = "Url for the original theme";
            url.SortOrder = 1;

            var createdBy = new PropertyType(dataTypeService.GetDataTypeDefinitionById(TEXT_ID), "createdBy");
            createdBy.Name = "Created By";
            createdBy.Description = "Theme Author";
            createdBy.SortOrder = 2;

            var createdDate = new PropertyType(dataTypeService.GetDataTypeDefinitionById(DATE_ID), "createdDate");
            createdDate.Name = "Created Date";
            createdDate.Description = "Date the Theme was created";
            createdDate.SortOrder = 3;

            var upload = new PropertyType(dataTypeService.GetDataTypeDefinitionById(UPLOAD_ID), "umbracoFile");
            upload.Name = "Upload Image";
            upload.Description = "Theme Thumbnail";
            upload.SortOrder = 4;

            var width = new PropertyType(dataTypeService.GetDataTypeDefinitionById(LABEL_ID), "umbracoWidth");
            width.Name = "Image Width";
            width.SortOrder = 5;

            var height = new PropertyType(dataTypeService.GetDataTypeDefinitionById(LABEL_ID), "umbracoHeight");
            height.Name = "Image Height";
            height.SortOrder = 5;

            var size = new PropertyType(dataTypeService.GetDataTypeDefinitionById(LABEL_ID), "umbracoBytes");
            size.Name = "Image Size";
            size.SortOrder = 5;

            var extension = new PropertyType(dataTypeService.GetDataTypeDefinitionById(LABEL_ID), "umbracoExtension");
            extension.Name = "Image Extension";
            extension.SortOrder = 5;

            mediaType.AddPropertyType(name, "Theme");
            mediaType.AddPropertyType(url, "Theme");
            mediaType.AddPropertyType(createdBy, "Theme");
            mediaType.AddPropertyType(createdDate, "Theme");
            mediaType.AddPropertyType(upload, "Theme");
            mediaType.AddPropertyType(width, "Theme");
            mediaType.AddPropertyType(height, "Theme");
            mediaType.AddPropertyType(size, "Theme");
            mediaType.AddPropertyType(extension, "Theme");

            //Save new media type
            ContentTypeService contentTypeService = (ContentTypeService)ApplicationContext.Current.Services.ContentTypeService;
            contentTypeService.Save(mediaType);
        }

        public static void RemoveSocialMediaChannelMediaType()
        {
            ContentTypeService contentTypeService = (ContentTypeService)ApplicationContext.Current.Services.ContentTypeService;
            contentTypeService.Delete(contentTypeService.GetMediaType(SOCIAL_MEDIA_THEME_ALIAS));
        }

        public static void AddSocialMediaChannelThemes()
        {
            string themesPath = HttpContext.Current.Server.MapPath(SOCIAL_MEDIA_CHANNELS_FOLDER);
            DirectoryInfo themesDir = new DirectoryInfo(themesPath);

            MediaService mediaService = (MediaService)ApplicationContext.Current.Services.MediaService;

            //Create Root Themes Folder
            var rootFolder = mediaService.CreateMedia(SOCIAL_MEDIA_THEME_FOLDER_NAME, -1, "Folder");

            mediaService.Save(rootFolder);

            var rootFolderId = mediaService.GetMediaOfMediaType(1031).Where(x => x.Name == SOCIAL_MEDIA_THEME_FOLDER_NAME).FirstOrDefault().Id;

            //Get the media type id
            ContentTypeService cts = (ContentTypeService)ApplicationContext.Current.Services.ContentTypeService;
            int mediaTypeId = cts.GetMediaType(SOCIAL_MEDIA_THEME_ALIAS).Id;

            foreach (DirectoryInfo dir in themesDir.EnumerateDirectories())
            {
                FileInfo infoFile = dir.GetFiles("info.txt")[0];
                string themeDir = dir.Name;
                string thumbnailPath = Path.Combine(themesDir.FullName, themeDir + ".jpg");
                string themeNick, themeName, themeUrl, themeCreatedBy, themeCreatedDate;
                using (StreamReader reader = new StreamReader(infoFile.FullName))
                {
                    themeNick = reader.ReadLine();
                    themeName = reader.ReadLine();
                    themeCreatedBy = reader.ReadLine();
                    themeUrl = reader.ReadLine();
                    themeCreatedDate = reader.ReadLine();
                }

                var theme = mediaService.CreateMedia(themeNick, rootFolderId, SOCIAL_MEDIA_THEME_ALIAS);
                var thumbnail = new FileStream(thumbnailPath, FileMode.Open);
                theme.SetValue("umbracoFile", Path.GetFileName(thumbnailPath), thumbnail);
                theme.SetValue("themeName", themeName);
                theme.SetValue("themeUrl", themeUrl);
                theme.SetValue("createdBy", themeCreatedBy);
                theme.SetValue("createdDate", themeCreatedDate);

                mediaService.Save(theme);
                int currThemeId = mediaService.GetMediaOfMediaType(mediaTypeId).Where(x => x.GetValue("themeName").ToString() == themeName).FirstOrDefault().Id;

                foreach (FileInfo image in dir.EnumerateFiles())
                {
                    if (!image.Extension.Equals(".txt") && !image.Extension.Equals(".xml"))
                    {
                        var media = mediaService.CreateMedia(image.Name, currThemeId, "Image");
                        var channel = new FileStream(image.FullName, FileMode.Open);

                        media.SetValue("umbracoFile", Path.GetFileName(image.FullName), channel);
                        mediaService.Save(media);
                    }
                }
            }
        }

        public static void RemoveSocialMediaChannelThemes()
        {
            MediaService mediaService = (MediaService)ApplicationContext.Current.Services.MediaService;
            var rootFolder = mediaService.GetMediaOfMediaType(1031).Where(x => x.Name == SOCIAL_MEDIA_THEME_FOLDER_NAME).FirstOrDefault();
            mediaService.Delete(rootFolder);
        }

        public static void AddSocialMediaChannelTheme(string theme)
        {
            string themesPath = HttpContext.Current.Server.MapPath(SOCIAL_MEDIA_CHANNELS_FOLDER);
            string newThemePath = Path.Combine(themesPath, theme);
            DirectoryInfo newThemeDir = new DirectoryInfo(newThemePath);

            MediaService mediaService = (MediaService)ApplicationContext.Current.Services.MediaService;

            var rootFolderId = mediaService.GetMediaOfMediaType(1031).Where(x => x.Name == SOCIAL_MEDIA_THEME_FOLDER_NAME).FirstOrDefault().Id;

            //Get the media type id
            ContentTypeService cts = (ContentTypeService)ApplicationContext.Current.Services.ContentTypeService;
            int mediaTypeId = cts.GetMediaType(SOCIAL_MEDIA_THEME_ALIAS).Id;


            FileInfo infoFile = newThemeDir.GetFiles("info.txt")[0];
            string themeDir = newThemeDir.Name;
            string thumbnailPath = Path.Combine(newThemeDir.Parent.FullName, themeDir + ".jpg");
            string themeNick, themeName, themeUrl, themeCreatedBy, themeCreatedDate;
            using (StreamReader reader = new StreamReader(infoFile.FullName))
            {
                themeNick = reader.ReadLine();
                themeName = reader.ReadLine();
                themeCreatedBy = reader.ReadLine();
                themeUrl = reader.ReadLine();
                themeCreatedDate = reader.ReadLine();
            }

            bool newTheme = mediaService.GetMediaOfMediaType(mediaTypeId).Where(x => x.GetValue("themeName").ToString() == themeName).Count() == 0;

            if (newTheme)//only add if it's new
            {
                var themePck = mediaService.CreateMedia(themeNick, rootFolderId, SOCIAL_MEDIA_THEME_ALIAS);
                var thumbnail = new FileStream(thumbnailPath, FileMode.Open);
                themePck.SetValue("umbracoFile", Path.GetFileName(thumbnailPath), thumbnail);
                themePck.SetValue("themeName", themeName);
                themePck.SetValue("themeUrl", themeUrl);
                themePck.SetValue("createdBy", themeCreatedBy);
                themePck.SetValue("createdDate", themeCreatedDate);

                mediaService.Save(themePck);
                int currThemeId = mediaService.GetMediaOfMediaType(mediaTypeId).Where(x => x.GetValue("themeName").ToString() == themeName).FirstOrDefault().Id;

                foreach (FileInfo image in newThemeDir.EnumerateFiles())
                {
                    if (!image.Extension.Equals(".txt") && !image.Extension.Equals(".xml"))
                    {
                        var media = mediaService.CreateMedia(image.Name, currThemeId, "Image");
                        var channel = new FileStream(image.FullName, FileMode.Open);

                        media.SetValue("umbracoFile", Path.GetFileName(image.FullName), channel);
                        mediaService.Save(media);
                    }
                }

            }
        }

        public static void RemoveSocialMediaChannelTheme(string theme)
        {
            string themesPath = HttpContext.Current.Server.MapPath(SOCIAL_MEDIA_CHANNELS_FOLDER);
            string newThemePath = Path.Combine(themesPath, theme);
            DirectoryInfo newThemeDir = new DirectoryInfo(newThemePath);
            MediaService mediaService = (MediaService)ApplicationContext.Current.Services.MediaService;

            //Get the media type id
            int mediaTypeId = ApplicationContext.Current.Services.ContentTypeService.GetMediaType(SOCIAL_MEDIA_THEME_ALIAS).Id;

            FileInfo infoFile = newThemeDir.GetFiles("info.txt")[0];
            string themeNick, themeName;
            using (StreamReader reader = new StreamReader(infoFile.FullName))
            {
                themeNick = reader.ReadLine();
                themeName = reader.ReadLine();
            }

            var currTheme = mediaService.GetMediaOfMediaType(mediaTypeId).Where(x => x.GetValue("themeName").ToString() == themeName).FirstOrDefault();
            mediaService.Delete(currTheme);
        }
        #endregion

    }
}