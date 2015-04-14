using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  public  class MicroShopEntity
    {
        public int Id { get; set; }
        public string MicroName { get; set; }
        public string Url { get; set; }
        public string ConverCommunity { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string ConverCommunityName { get; set; }

        public string BusName { get; set; }
        public string Phone { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }
        public string ImgUrl { get; set; }
    }
}
