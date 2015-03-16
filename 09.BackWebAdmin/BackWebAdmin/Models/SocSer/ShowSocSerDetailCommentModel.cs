using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin
{
    public class ShowSocSerDetailCommentModel
    {
        public int CommentTotalCount { get; set; }
        public List<ShowSocSerDetailCommentListModel> CommentList { get; set; }
      
    }
    public class ShowSocSerDetailCommentListModel
    {
        public int Id { get; set; }
        public int SdId { get; set; }
        public string Comment { get; set; }
        public int CommentUserId { get; set; }
        public DateTime CommentTime { get; set; }
        public string UserRealName { get; set; }
        public string Phone { get; set; }
    }
    
}