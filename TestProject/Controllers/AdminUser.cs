using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QDT.Common;

namespace QDT.Core.Domain
{
    public class AdminUser 
    {
        public AdminUser()
        {
            
        }
        public AdminUser(string name,string password)
        {
            Name = name;
           // Password = password.Hash();
            IsEffect = true;
            IsDelete = false;
            LoginTime = DateTime.Now;
            LoginIP = "0.0.0.0";
        }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsEffect { get; set; }

        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否是默认管理员，默认管理员拥有超级权限
        /// </summary>
        public bool IsDefaultAdmin { get; set; }

        public int RoleID { get; set; }

        public DateTime LoginTime { get; set; }

        public string LoginIP { get; set; }
        
        public void ChangePwd(string newPassword)
        {
            //Password = newPassword.Hash();
        }
    }
}
