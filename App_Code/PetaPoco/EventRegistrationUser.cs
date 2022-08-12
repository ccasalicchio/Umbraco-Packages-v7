using System;
using RevistaUFO.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RevistaUFO.PetaPoco
{
    [TableName("EventRegistrationUser")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class EventRegistrationUser
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [ForeignKey(typeof(EventRegistration), Name = "FK_EventRegistration_User")]
        [IndexAttribute(IndexTypes.NonClustered, Name = "IX_RegistrationID")]
        public int RegistrationId { get; set; }

        public string Fullname { get; set; }
        public string Email1 { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Email2 { get; set; }
        public string HomePhone { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Address2 { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Neighborhood { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Document { get; set; }

        public static implicit operator EventRegistrationUser(EventRegistrationModel v)
        {
            EventRegistrationUser user = new EventRegistrationUser {
                Fullname = v.Fullname,
                Address = v.Address,
                Address2 = v.Address2,
                CellPhone = v.Phone2,
                City = v.City,
                Document = v.Document,
                Email1 = v.Email1,
                Email2 = v.Email2,
                HomePhone = v.Phone1,
                Neighborhood = v.Neighborhood,
                Number = v.Number,
                PostalCode = v.Postal,
                State = v.State,
                Id = v.Id,
                RegistrationId = v.Id
            };
            return user;
        }
    }
}