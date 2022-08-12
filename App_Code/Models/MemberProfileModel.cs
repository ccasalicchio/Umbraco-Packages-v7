using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace RevistaUFO.Models
{
    /// <summary>
    /// Summary description for MemberProfileModel
    /// </summary>
    public class MemberProfileModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
        [Display(Name = "Data de Nascimento"), DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Gênero")]
        public Gender Gender { get; set; }
        [Display(Name = "Usuário (não é editável)")]
        public string Username { get; private set; }

        public void SetUsername(string username)
        {
            Username = username;
        }

    }
    public enum Gender
    {
        Masculino = 3052,
        Feminino = 3053,
        NA = 3054
    }
}