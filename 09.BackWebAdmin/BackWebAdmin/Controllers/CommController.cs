using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.IO;

namespace BackWebAdmin.Controllers
{
    public class CommController : Controller
    {
        const int pageSize = 20;

     
       
        public ActionResult AppGetDeptChild(int? id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                if (id != null && id.HasValue)
                {
                    var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.PId == id).ToList();
                    return Json(list);
                }
                else
                {
                    var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                    return Json(list);
                }
            }
        }
        //新增自愿者
        public ActionResult AppPostAddVol(VolunteerEntity entity)
        {

            entity.State = 0;//待审核
            entity.Doing = 1;//默认接受任务
            entity.Score = 0;
            Stream imgStream = Request.Files["VolHeadImg"].InputStream;
            int length = (int)imgStream.Length;
            byte[] image = new byte[length];
            imgStream.Read(image, 0, length);
            imgStream.Close();
            entity.VolHeadImg = image;


            string nfname = Request.Files["VolHeadImg"].FileName;
            string path = AppDomain.CurrentDomain.BaseDirectory + "uploads\\";
            string nsave = System.IO.Path.Combine(path, nfname);
            Request.Files[0].SaveAs(nsave);


            using (IplusOADBContext db = new IplusOADBContext())
            {

                db.Add<VolunteerEntity>(entity);
                db.SaveChanges();
                db.Dispose();
                return Json("添加成功");
            }
        }


        public ActionResult AppVolHeadImg(string vid)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                byte[] image = (from c in db.VolunteerEntityTable where c.VID == vid select c.VolHeadImg).First<byte[]>();
                return new FileContentResult(image, "image/jpeg");
            }

           
        }

    }
}