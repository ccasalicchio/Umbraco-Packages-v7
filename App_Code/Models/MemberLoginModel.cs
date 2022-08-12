using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace RevistaUFO.Models
{
    /// <summary>
    /// Summary description for MemberLoginModel
    /// </summary>
    public class MemberLoginModel
    {
        [Required, Display(Name = "E-mail")]
        public string Username { get; set; }
        [Required, Display(Name = "Senha"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Permanecer Conectado")]
        public bool RememberMe { get; set; }
    }
}