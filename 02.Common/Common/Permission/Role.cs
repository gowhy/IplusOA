using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Role 
    {
        //public Role()
        //{
        //    Accesses = new List<RoleAccess>();
        //}
        public int ID { get; set; }
        public string Name { get; set; }

        public bool IsEffect { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<RoleAccess> Accesses { get; set; }
    }
}
