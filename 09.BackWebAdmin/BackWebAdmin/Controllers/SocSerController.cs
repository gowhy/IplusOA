using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace BackWebAdmin.Controllers
{
    public class SocSerController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;
        //
        // GET: /SocSer/
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            IplusOADBContext db = new IplusOADBContext();
            var list = db.SocServiceDetailEntityTable.AsQueryable().ToList();
            return View(list.ToPagedList(pageNumber - 1, pageSize));
        }

        public ActionResult Add()
        {

           
            IplusOADBContext db = new IplusOADBContext();
            var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();

            ViewData["Department_List"] =HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list); ;

            return View();
        }


        /// <summary>
        /// 发布社会服务内容详细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult PostAdd(SocServiceDetailEntity entity)
        {
            entity.AddTime = DateTime.Now;
            entity.AddUser = AdminUser.UserName;

            IplusOADBContext db = new IplusOADBContext();
            SocialOrgEntity soc = db.SocialOrgEntityTable.SingleOrDefault(x => x.Id == AdminUser.SocOrgId);
            entity.SocialNo = soc.SocialNO;

            entity.SerNum = entity.Type + DateTime.Now.ToString("yyyyMMdd") + (db.SocServiceDetailEntityTable.Max(x => x.Id) + 1).ToString("D2");
            db.Add<SocServiceDetailEntity>(entity);
            db.SaveChanges();
            db.Dispose();

            return Success("添加成功");
        }

        public ActionResult Edit(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
            var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
            db.Dispose();

            string CoverCommunityNames = "";
            IList<string> clit = entity.CoverCommunity.Trim().Split(',');
            for (int i = 0; i < list.Count; i++)
            {
                if (clit.Contains(list[i].Id.ToString()))
                {
                    list[i].IsCheck = true;
                    CoverCommunityNames += list[i].Name + ",";
                }
            }
            entity.CoverCommunityNames = CoverCommunityNames;
            ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

            return View(entity);
        }

        public ActionResult PostEdit(SocServiceDetailEntity entity)
        {
            IplusOADBContext db = new IplusOADBContext();
            db.Update<SocServiceDetailEntity>(entity);
            db.SaveChanges();
            db.Dispose();
            return Success("修改成功");
          
        }

        public ActionResult Delete(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
            db.Delete<SocServiceDetailEntity>(entity);
            db.SaveChanges();
            return Success("操作成功");

        }
	}
}