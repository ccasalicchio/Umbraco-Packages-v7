using RevistaUFO.Models;
using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RevistaUFO.PetaPoco
{
    [TableName("EventRegistrations")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class EventRegistration
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public long RegistrationNumber { get; set; }

        public int EventId { get; set; }

        public string RefCode { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string EventAddOns { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Coupon { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public bool Paid { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string TotalPaid { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public int PaymentMethod { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string PaymentConfirmation { get; set; }

        public string Session { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreatedOn { get; set; }

        [Column("RegistrationDescription")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string RegistrationDescription { get; set; }

        public static implicit operator EventRegistration(EventRegistrationModel model)
        {
            EventRegistration reg = new EventRegistration
            {
                Coupon = model.Coupon,
                CreatedOn = DateTime.Now,
                Id = model.Id,
                EventId = model.EventId,
                Session = model.Session,
                RegistrationNumber = model.RegistrationNumber,
                PaymentMethod = (int)model.PaymentMethod,
                EventAddOns = model.AddOns,
                TotalPaid = model.Total,
                RefCode = model.RefCode,
                IpAddress = model.IpAddress,
                Paid = model.Paid,
                RegistrationDescription = model.RegistrationDescription
            };
            return reg;
        }
    }
}