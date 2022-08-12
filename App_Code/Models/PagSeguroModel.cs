using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RevistaUFO.Models
{
    /// <summary>
    /// Summary description for EventRegistrationModel
    /// </summary>
    public class PagSeguroModel
    {
        public string Receiver { get; set; }
        public string Token { get; set; }
        public string ReturnUrl { get; set; }
    }

}