using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO
{
  public  class AdminUserEntity
    {
        public AdminUserEntity()
        {
            
        }
        public AdminUserEntity(string name, string password)
        {
            Name = name;
            Password = password;
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

        public string LoginToken { get; set; }

         public string Msg { get; set; }
    }
}
