using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RevistaUFO.Helpers;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RevistaUFO.Models
{
    /// <summary>
    /// Summary description for EventRegistrationModel
    /// </summary>
    public class EventRegistrationModel
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public long RegistrationNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string AddOns { get; set; }
        [Required, Display(Name = "Logradouro")]
        public string Address { get; set; }
        [Display(Name = "Complemento")]
        public string Address2 { get; set; }
        [Required, Display(Name = "Cidade")]
        public string City { get; set; }
        public string Coupon { get; set; }
        [Required, Display(Name = "E-mail Principal")]
        public string Email1 { get; set; }
        [Display(Name = "E-mail")]
        public string Email2 { get; set; }
        [Required, Display(Name = "Nome Completo")]
        public string Fullname { get; set; }
        [Display(Name = "Bairro")]
        public string Neighborhood { get; set; }
        [Required, Display(Name = "Número")]
        public string Number { get; set; }
        [Required, Display(Name = "Fone Residencial com DDD")]
        public string Phone1 { get; set; }
        [Display(Name = "Fone Celular com DDD")]
        public string Phone2 { get; set; }
        [Required, Display(Name = "Código Postal")]
        public string Postal { get; set; }
        [Required, Display(Name = "Estado")]
        public string State { get; set; }

        public string Document { get; set; }

        public string ReturnUrl { get; set; }

        public string Session { get; set; }

        public string IpAddress { get; set; }

        public string RefCode { get; set; }

        public string Receiver { get; set; }

        public bool Paid { get; set; }

        public string Token { get; set; }

        public string PaymentConfirmation { get; set; }

        public string RegistrationDescription { get; set; }

        public string CreatedOn { get; set; }

        public string Total { get; set; }

        public List<string> AddOnListNames
        {
            get
            {
                return this.PopulateAddOnListNames();
            }
        }
        public List<IPublishedContent> AddOnList { get; set; }

        public string Dob { get; set; }

        public static explicit operator EventRegistrationModel(EventRegistration registration)
        {
            var model = new EventRegistrationModel
            {
                AddOns = registration.EventAddOns,
                Coupon = registration.Coupon,
                PaymentMethod = (PaymentMethod)registration.PaymentMethod,
                RegistrationNumber = registration.RegistrationNumber,
                Session = registration.Session,
                EventId = registration.EventId,
                Id = registration.Id,
                RegistrationDescription = registration.RegistrationDescription,
                PaymentConfirmation = registration.PaymentConfirmation,
                IpAddress = registration.IpAddress,
                RefCode = registration.RefCode,
                Paid = registration.Paid,
                Total = registration.TotalPaid

            };
            model.PopulateAddOnListNames();
            return model;
        }

        public static explicit operator EventRegistrationModel(EventRegistrationUser registration)
        {
            var model = new EventRegistrationModel
            {
                Fullname = registration.Fullname,
                Address = registration.Address,
                Address2 = registration.Address2,
                Email1 = registration.Email1,
                Email2 = registration.Email2,
                Phone1 = registration.HomePhone,
                Phone2 = registration.CellPhone,
                City = registration.City,
                State = registration.State,
                Neighborhood = registration.Neighborhood,
                Postal = registration.PostalCode,
                Number = registration.Number,
                Document = registration.Document
            };
            return model;
        }
    }
    public enum PaymentMethod
    {
        PayPal = 0,
        PagSeguro = 1,
        Free = 999
    }

    public class AddOn
    {
        public string Name { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }
}

