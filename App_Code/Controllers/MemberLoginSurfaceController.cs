using RevistaUFO.Helpers;
using RevistaUFO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Security;

namespace RevistaUFO.Controllers
{
    /// <summary>
    /// Summary description for MemberLoginSurfaceController
    /// </summary>
    public class MemberLoginSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        private readonly UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        private readonly Smtp email;
        private readonly string _adminEmail = "webmaster@ufo.com.br";
        private readonly IPublishedContent home;
        private static readonly Regex _regex = new Regex("[^a-zA-Z0-9]");

        public MemberLoginSurfaceController()
        {
            home = umbracoHelper.TypedContentAtRoot().First();
            _adminEmail = home.GetPropertyValue<string>("from");
            email = new Smtp();
        }
        // The MemberLogout Action signs out the user and redirects to the site home page:

        [HttpGet]
        public ActionResult MemberLogout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        // The MemberLoginPost Action checks the entered credentials using the standard Asp Net membership provider and redirects the user to the same page. Either as logged in, or with a message set in the TempData dictionary:

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("MemberLogin")]
        public ActionResult MemberLoginPost(MemberLoginModel model, string returnUrl = "/")
        {
            var cMember = Services.MemberService.GetByUsername(model.Username);
            if (cMember.IsApproved)
            {
                if (Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["Status"] = "Usuário ou senha inválida";
                    return CurrentUmbracoPage();
                }
            }
            TempData["Status"] = "Conta ainda não aprovada, verifique seu inbox";
            return CurrentUmbracoPage();
        }

        private string GeneratePassword()
        {
            // Generate a password which we'll email the member
            var password = Membership.GeneratePassword(10, 1);
            return _regex.Replace(password, "9");
        }
        // http://fee-dev.org/Umbraco/Api/UserAPI/ResetPasswordForUser?email=website@fee.org
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("PasswordReset")]
        public ActionResult ResetPasswordForMember(string email)
        {
            var cMember = Services.MemberService.GetByEmail(email);

            if (cMember != null)
            {
                // Found the user
                var password = GeneratePassword();

                // Change the password to the new generated one above
                Services.MemberService.SavePassword(cMember, password);
                // Save the password/member

                dynamic message = new
                {
                    NewPassword = password,
                    cMember.Email,
                    cMember.Name,
                    Subject = "Redefinir Senha"

                };
                if (SendUserEmail(message))
                    TempData["Status"] = "Nova senha enviada para " + cMember.Email;
                else TempData["Status"] = "Erro ao enviar senha, tente mais tarde ";

                return Redirect("/minha-conta/redefinirsenha/");
            }
            TempData["Status"] = "Erro ao redefinir senha, tente mais tarde ";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public ActionResult Register(string Email, string Password)
        {
            RegisterModel newMember = Members.CreateRegistrationModel("Member");
            string GUID = Guid.NewGuid().ToString();

            newMember.Name = Email;
            newMember.Email = Email;
            newMember.Password = Password;
            newMember.UsernameIsEmail = true;
            newMember.Username = Email;
            newMember.LoginOnSuccess = false;
            UmbracoProperty activation = newMember.MemberProperties.Single(p => p.Alias == "activationCode");
            activation.Value = GUID;

            MembershipCreateStatus status = new MembershipCreateStatus();
            MembershipUser member = Members.RegisterMember(newMember, out status);
            Roles.AddUserToRole(newMember.Username, "Registrados");
            member.IsApproved = false;
            Membership.UpdateUser(member);

            //signout before activation
            Session.Clear();
            FormsAuthentication.SignOut();

            dynamic message = new
            {
                ValidationUrl = GUID,
                Email,
                Name = Email,
                Subject = "Validar Conta Revista UFO"

            };
            if (SendValidationEmail(message))
                TempData["Status"] = "Validação enviada para " + Email;
            else TempData["Status"] = "Erro ao enviar validação, tente mais tarde ";

            return Redirect("/minha-conta/login/");
            TempData["Status"] = "Erro ao criar conta, tente mais tarde ";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("RegisterCustom")]
        public ActionResult RegisterCustom(string Name, string Email, string Password, string groupName, string redirectTo)
        {
            RegisterModel newMember = Members.CreateRegistrationModel("Member");
            string GUID = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(Email))
            {
                newMember.Name = Name;
                newMember.Email = Email;
                newMember.Password = Password;
                newMember.UsernameIsEmail = true;
                newMember.Username = Email;
                newMember.LoginOnSuccess = false;
                UmbracoProperty activation = newMember.MemberProperties.Single(p => p.Alias == "activationCode");
                activation.Value = GUID;

                MembershipCreateStatus status = new MembershipCreateStatus();
                MembershipUser member = Members.RegisterMember(newMember, out status);
                Roles.AddUserToRole(newMember.Username, groupName);
                member.IsApproved = false;
                Membership.UpdateUser(member);

                //signout before activation
                Session.Clear();
                FormsAuthentication.SignOut();

                dynamic message = new
                {
                    ValidationUrl = GUID,
                    Email,
                    Name = Email,
                    Subject = "Validar Conta CBPDV"

                };
                if (SendValidationEmail(message))
                    TempData["Status"] = "Validação enviada para " + Email;
                else TempData["Status"] = "Erro ao enviar validação, tente mais tarde ";

                return Redirect(redirectTo);
            }
            TempData["Status"] = "Erro ao criar conta, tente mais tarde ";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToGroup( string groupName, string redirectTo)
        {
            Roles.AddUserToRole(Membership.GetUser().UserName, groupName);
            return Redirect(redirectTo);
        }

        public ActionResult RegisterViaExternal(string Email)
        {
            RegisterModel newMember = Members.CreateRegistrationModel("Member");

            if (!string.IsNullOrEmpty(Email))
            {
                newMember.Name = Email;
                newMember.Email = Email;
                newMember.Password = GeneratePassword();
                newMember.UsernameIsEmail = true;
                newMember.Username = Email;
                newMember.LoginOnSuccess = true;

                MembershipCreateStatus status = new MembershipCreateStatus();
                MembershipUser member = Members.RegisterMember(newMember, out status);
                Roles.AddUserToRole(newMember.Username, "Registrados");
                member.IsApproved = true;
                Membership.UpdateUser(member);

                TempData["Status"] = "Conta criada e novo login para " + Email;
                return Redirect("/minha-conta/login/");
            }
            TempData["Status"] = "Erro ao fazer login externo, tente mais tarde ";
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DisconnectSocial")]
        public ActionResult DisconnectSocial(string name, string token)
        {
            var user = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var member = Services.MemberService.GetByEmail(user.Email);
            member.Properties[token].Value = "";
            Services.MemberService.Save(member);

            TempData["Status"] = "Vinculo " + name + " Removida!";
            return Redirect("/minha-conta/perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RemoveMyAccount")]
        public ActionResult RemoveMyAccount()
        {
            var member = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var delete = Services.MemberService.GetByEmail(member.Email);
            Services.MemberService.Delete(delete);
            //Logout
            Session.Clear();
            FormsAuthentication.SignOut();

            TempData["Status"] = "Conta Removida!";
            return Redirect("/minha-conta/login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdateAvatar")]
        public ActionResult UpdateAvatar(HttpPostedFileBase file)
        {
            var user = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();

            if (user != null)
            {
                var member = Services.MemberService.GetByEmail(user.Email);
                var _mediaService = Services.MediaService;
                var previousAvatar = member.GetValue("avatar") != null ? _mediaService.GetMediaByPath(member.Properties["avatar"].Value.ToString()) : null;
                int mediaId = 0;

                if (previousAvatar == null)
                {
                    var media = _mediaService.CreateMedia(user.UserName, 8617, "Image");
                    media.SetValue("umbracoFile", file.FileName, file.InputStream);
                    Services.MediaService.Save(media);
                    mediaId = media.Id;
                }
                else
                {
                    previousAvatar.SetValue("umbracoFile", file.FileName, file.InputStream);
                    Services.MediaService.Save(previousAvatar);
                    mediaId = previousAvatar.Id;
                }
                dynamic savedMedia = Umbraco.Media(mediaId);
                member.Properties["avatar"].Value = savedMedia.UmbracoFile;
                Services.MemberService.Save(member);

                TempData["Status"] = "Avatar atualizado";
                return Redirect("/minha-conta/perfil");
            }

            TempData["Status"] = "Erro ao atualizar avatar, tente mais tarde ";
            return Redirect("/minha-conta/perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdateMember")]
        public ActionResult UpdateMember([Bind(Include = "Name,Email,DateOfBirth,Gender")] MemberProfileModel profile)
        {
            var u = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var member = Services.MemberService.GetByUsername(u.UserName);

            if (member != null)
            {
                member.Name = profile.Name;
                member.Email = profile.Email;
                member.Properties["dateOfBirth"].Value = profile.DateOfBirth;
                member.Properties["gender"].Value = profile.Gender;
                Services.MemberService.Save(member);

                TempData["Status"] = "Usuário atualizado";
                return Redirect("/minha-conta/perfil");
            }

            TempData["Status"] = "Erro ao atualizar perfil, tente mais tarde ";
            return Redirect("/minha-conta/perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(MemberPasswordModel model)
        {
            var u = Umbraco.MembershipHelper.GetCurrentMemberProfileModel();
            var member = Services.MemberService.GetByUsername(u.UserName);

            if (member != null)
            {
                if (!string.IsNullOrEmpty(model.Password) && Membership.ValidateUser(member.Username, model.PreviousPassword))
                    Services.MemberService.SavePassword(member, model.Password);

                TempData["Status"] = "Senha atualizada";
                return Redirect("/minha-conta/senha");
            }

            TempData["Status"] = "Erro ao atualizar senha, tente mais tarde ";
            return Redirect("/minha-conta/senha");
        }

        public bool SendValidationConfirmed(string emailAddress, string subject)
        {
            var emailMessage = umbracoHelper.GetDictionaryValue("[Emails]AccountConfirmed");
            SetupSmtp();
            email.To = emailAddress;
            email.ToName = emailAddress;
            email.Subject = subject;
            email.Message = email.UsingTemplate(emailMessage,
                new
                {
                    Website = "Site Revista UFO",
                    Email = emailAddress
                });

            return email.Send();
        }

        protected void SetupSmtp()
        {
            email.Hostname = home.GetPropertyValue<string>("hostname");
            email.Port = home.GetPropertyValue<int>("port");
            email.Username = home.GetPropertyValue<string>("username");
            email.Password = home.GetPropertyValue<string>("password");
            email.UseSSL = home.GetPropertyValue<bool>("useSSL");
            email.From = _adminEmail;
            email.FromName = home.GetPropertyValue<string>("fromName");
        }

        protected bool SendUserEmail(dynamic message)
        {
            var emailMessage = umbracoHelper.GetDictionaryValue("[Emails]PasswordReset");
            SetupSmtp();
            email.To = message.Email;
            email.ToName = message.Name;
            email.Subject = message.Subject;
            email.Message = email.UsingTemplate(emailMessage,
                new
                {
                    Website = "Site Revista UFO",
                    AdminEmail = _adminEmail,
                    Password = message.NewPassword,
                    message.Name
                });

            return email.Send();
        }

        protected bool SendValidationEmail(dynamic message)
        {
            var emailMessage = umbracoHelper.GetDictionaryValue("[Emails]ValidateAccount");
            SetupSmtp();
            email.To = message.Email;
            email.ToName = message.Name;
            email.Subject = message.Subject;
            email.Message = email.UsingTemplate(emailMessage,
                new
                {
                    Website = "Site Revista UFO",
                    ValidationUrl = string.Format("http://{0}?u={1}&a={2}", RootDomain() + "/minha-conta/verificar", message.Email, message.ValidationUrl),
                    message.Name
                });

            return email.Send();
        }

        protected string RootDomain()
        {
            return Request.Url.Authority;
        }
    }
}