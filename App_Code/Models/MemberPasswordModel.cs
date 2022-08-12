using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace RevistaUFO.Models
{
    /// <summary>
    /// Summary description for MemberPasswordModel
    /// </summary>
    public class MemberPasswordModel
    {
        [Display(Name = "Senha Atual"), DataType(DataType.Password)]
        public string PreviousPassword { get; set; }
        [Display(Name = "Nova Senha"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}