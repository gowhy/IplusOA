using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public partial class SocSerImgEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FTPUrl { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Module { get; set; }
        public string HttpUrl { get; set; }
        public int SocSerId { get; set; }

      //  public virtual SocServiceDetailEntity SocServiceDetailEntity { get; set; }
    }
}
