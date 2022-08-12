using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using ExifLib;

namespace RevistaUFO.Controllers
{
    /// <summary>
    /// Summary description for SubmitSurfaceController
    /// </summary>
    public class SubmitSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        private readonly UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        private readonly IPublishedContent home;

        public SubmitSurfaceController()
        {
            home = umbracoHelper.TypedContentAtRoot().First();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UploadFile")]
        public ActionResult UploadFile(string type)
        {
            var user = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var member = Services.MemberService.GetByEmail(user.Email);
            var _mediaService = Services.MediaService;
            var exception = "";
            var folderId = CurrentPage.GetPropertyValue<int>("sendFormPath");
            var contentService = Services.ContentService;
            var folderPath = CurrentPage.GetPropertyValue<int>("sendFormContentNode");
            var typeOfMedia = string.Empty;
            var submissionAlias = string.Empty;
            var mediaList = string.Empty;

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                if (type == "Image_EXIF")
                {
                    typeOfMedia = "Submission_Photo";
                    submissionAlias = "submission_others";
                }
                else
                {
                    typeOfMedia = "Submission_Video";
                    submissionAlias = "MemberSubmission_videos";
                }
                //Create Content Node
                var newMediaContent = contentService.CreateContent(string.Format("{0}-{1}", DateTime.Now.ToString("dd-MMMM-yyyy"), member.Name), folderPath, typeOfMedia);
                newMediaContent.SetValue("MemberSubmission_Person_automated_", member.Name);
                contentService.Save(newMediaContent);

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = string.Format("{0} {1}", member.Name, file.FileName);
                    if (file != null && file.ContentLength > 0)
                    {
                        var media = _mediaService.CreateMedia(fName, folderId, type);
                        media.SetValue("umbracoFile_Credit", member.Name);
                        if (type == "Image_EXIF")
                        {
                            media.SetValue("umbracoFile", file.FileName, file.InputStream);
                            typeOfMedia = "Submission_Photo";
                            submissionAlias = "submission_others";
                            _mediaService.Save(media);
                            //TODO: ADD EXIF
                            using (ExifReader reader = new ExifReader(Server.MapPath(Umbraco.Media(media.Id).Url)))
                            {
                                // Extract the tag data using the ExifTags enumeration
                                string model;
                                string orientation;
                                string software;
                                DateTime dateAndTime;
                                string ycbcrPositioning;
                                string compression;
                                string xresolution;

                                if (reader.GetTagValue<string>(ExifTags.Model, out model)) media.SetValue("EXIF_model", model);
                                if (reader.GetTagValue<string>(ExifTags.Model, out orientation)) media.SetValue("EXIF_orientation", orientation);
                                if (reader.GetTagValue<string>(ExifTags.Model, out software)) media.SetValue("EXIF_software", software);
                                if (reader.GetTagValue<DateTime>(ExifTags.Model, out dateAndTime)) media.SetValue("EXIF_dateAndTime", dateAndTime);
                                if (reader.GetTagValue<string>(ExifTags.Model, out ycbcrPositioning)) media.SetValue("EXIF_ycbcrPositioning", ycbcrPositioning);
                                if (reader.GetTagValue<string>(ExifTags.Model, out compression)) media.SetValue("EXIF_compression", compression);
                                if (reader.GetTagValue<string>(ExifTags.Model, out xresolution)) media.SetValue("EXIF_xResolution", xresolution);
                            }
                            _mediaService.Save(media);
                        }
                        else
                        {
                            media.SetValue("videoFile", file.FileName, file.InputStream);
                            typeOfMedia = "Submission_Video";
                            submissionAlias = "MemberSubmission_videos";
                            _mediaService.Save(media);

                        }
                        mediaList += media.Id + ",";
                    }
                }

                newMediaContent.SetValue(submissionAlias, mediaList);
                contentService.Save(newMediaContent);
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
                exception = ex.Message;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = fName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file: " + exception
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDescription(int id, string description)
        {
            var mediaService = Services.MediaService;
            var media = mediaService.GetById(id);
            media.SetValue("umbracoFile_Caption", description);
            mediaService.Save(media);
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportAccount(string account)
        {
            if (!ModelState.IsValid)
                return RedirectToCurrentUmbracoPage();

            var user = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var member = Services.MemberService.GetByEmail(user.Email);
            var contentService = ApplicationContext.Services.ContentService;
            var folderId = CurrentPage.GetPropertyValue<int>("sendFormContentNode");
            var newAccount = contentService.CreateContent(string.Format("{0}-{1}", DateTime.Now.ToString("dd-MMMM-yyyy"), member.Name), folderId, "Submission_Account");
            newAccount.SetValue("MemberSubmission_Person_automated_", member.Name);
            newAccount.SetValue("MemberSubmission_text", account);

            contentService.Save(newAccount);

            TempData["Status"] = "Relato registrado, obrigado!";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAccount(int id, string account)
        {
            var contentService = Services.ContentService;
            var content = contentService.GetById(id);
            content.SetValue("MemberSubmission_text", account);
            contentService.Save(content);
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportEvent(string type, string details, DateTime datetime)
        {
            if (!ModelState.IsValid)
                return RedirectToCurrentUmbracoPage();

            var user = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var member = Services.MemberService.GetByEmail(user.Email);
            var contentService = ApplicationContext.Services.ContentService;
            var folderId = CurrentPage.GetPropertyValue<int>("sendFormContentNode");
            var newEvent = contentService.CreateContent(string.Format("{0}-{1}", type, datetime.ToString("dd-MMMM-yyyy")), folderId, "eventSent");
            newEvent.SetValue("MemberSubmission_Person_automated_", member.Name);
            newEvent.SetValue("MemberSubmission_text", details);
            newEvent.SetValue("sentEventType", type);
            newEvent.SetValue("sentEventDate", datetime);

            contentService.Save(newEvent);

            TempData["Status"] = "Evento registrado, obrigado!";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEvent(int id, string type, string details, DateTime datetime)
        {
            var contentService = Services.ContentService;
            var content = contentService.GetById(id);
            content.SetValue("MemberSubmission_text", details);
            content.SetValue("sentEventType", type);
            content.SetValue("sentEventDate", datetime);
            contentService.Save(content);
            return CurrentUmbracoPage();
        }
    }
}