using System.Web.Mvc;

namespace QDT.MVC.Result
{
    public class RestUnauthorizedResult : HttpStatusCodeResult
    {
        private const int UnauthorizedCode = 403;

        public RestUnauthorizedResult()
            : this("没有权限访问或者登录已过期;")
        {
        }

        public RestUnauthorizedResult(string statusDescription)
            : base(UnauthorizedCode, statusDescription)
        {
        }
    }
}
