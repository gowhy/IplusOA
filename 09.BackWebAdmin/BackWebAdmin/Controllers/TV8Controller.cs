using BackWebAdmin.CommService;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin.Controllers
{
     [SecurityModule(Name = "TV8折购")]
    public class TV8Controller : BaseController
    {
        [SecurityNode(Name = "列表页")]
        // GET: TV8
        public ActionResult Index(int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;

            BackAdminUser bUser = this.GetBackUserInfo();
            string deptId = bUser.DeptId;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var mic = db.MicroShopEntityTable;

                var list = from m in mic where m.ConverCommunity.IndexOf(deptId) > -1 select m;

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }  
        }

        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
                return View();
            }
        }

        public ActionResult PostAdd(MicroShopEntity entity)
        {

            entity.AddUserId = AdminUser.Id;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var table = db.MicroShopEntityTable;
                if (table.Count(x => x.MicroName == entity.MicroName) > 0)
                {
                    return Success("微店名称已经存在,不能添加,请使用其他名称");
                }
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }
        public ActionResult Edit(int id)
        {
            if (id<1)
            {
                return Error("参数不正确");
            }
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var table = db.MicroShopEntityTable;

                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
              

                MicroShopEntity model = table.Find(id);
                string CoverCommunityNames = "";
                IList<string> clit = model.ConverCommunity.Trim().Split(',');
                for (int i = 0; i < list.Count; i++)
                {
                    if (clit.Contains(list[i].Id))
                    {
                        list[i].IsCheck = true;
                        CoverCommunityNames += list[i].Name + ",";
                    }
                }
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
                model.ConverCommunityName = CoverCommunityNames;

            
                return View(model);
            }
        }

        public ActionResult PostEdit(MicroShopEntity entity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var table = db.MicroShopEntityTable;
                if (table.Count(x => x.MicroName == entity.MicroName) > 0)
                {
                    return Success("微店名称已经存在,不能添加,请使用其他名称");
                }
                MicroShopEntity model = table.SingleOrDefault(x => x.Id == entity.Id);

                model.MicroName = entity.MicroName;
                model.Phone = entity.Phone;
                model.Url = entity.Url;
                model.BusName = entity.BusName;
                model.ConverCommunity = entity.ConverCommunity;


                db.Update<MicroShopEntity>(model);
                db.SaveChanges();
                return Success("修改成功");
            }

        }
        public ActionResult Delete(int id)
        {
            if (id < 1)
            {
                return Error("参数不正确");
            }
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var table = db.MicroShopEntityTable;
                MicroShopEntity model = table.SingleOrDefault(x => x.Id == id);
                db.Delete<MicroShopEntity>(model);
                return Success("删除成功");
            }
        }

        public ActionResult AppIndex(string depId, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;

            //VolunteerEntity bUser = this.GetAppUserInfo();
            //string deptId = bUser.DepId;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var mic = db.MicroShopEntityTable;

                var list = from m in mic where m.ConverCommunity.IndexOf(depId) > -1 select m;

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult AppCallRecordPostAdd(MicroShopCallRecordEntity entity)
        {

            ReturnModel retunModel = new ReturnModel();
            using (IplusOADBContext db = new IplusOADBContext())
            {
              
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

                retunModel.State = 1;
                retunModel.Msg ="增加成功";
                return Json(retunModel);
            }
        }
        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];


           SocSerImgEntity res= UploadFile.SaveFile(file, "TV", "");

           return Json(res);
        }

         /// <summary>
         /// 获取对应微店电话拨打详情
         /// </summary>
        /// <param name="id">microshop表Id</param>
         /// <param name="page"></param>
         /// <param name="pageSize"></param>
         /// <returns></returns>
        public ActionResult MicroShopCallRecordIndex(int id, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;


            using (IplusOADBContext db = new IplusOADBContext())
            {
                var mic = db.MicroShopEntityTable;
                var record = db.MicroShopCallRecordEntityTable;
                var vol = db.VolunteerEntityTable;

                var list = from r in record
                           join m in mic on r.MicroShopId equals m.Id
                           join v in vol on r.UserId equals v.Id
                           where r.MicroShopId == id
                           select new MicroShopCallRecordEntityClone
                           {
                               MicroShopName = m.MicroName,
                               AddTime = r.AddTime,
                               MicroShopId = r.MicroShopId,
                               MicroShopPhone = r.MicroShopPhone,
                               Phone = r.Phone,
                               UserId = r.UserId,
                               User=v,
                               Id=r.Id

                           };

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));

            }
        }
    }
}