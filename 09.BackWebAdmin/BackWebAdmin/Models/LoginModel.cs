using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataLayer.IplusOADB;
namespace BackWebAdmin.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public int RoleId { get; set; }

        public int Id { get; set; }
    }
}