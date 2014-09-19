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


            //string nfname = Request.Files[0].FileName;
            // string path = AppDomain.CurrentDomain.BaseDirectory + "uploads\\";
            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}
            //string nsave = System.IO.Path.Combine(path, nfname);
            //Request.Files[0].SaveAs(nsave);


            entity.State = 0;//待审核
            entity.Doing = 1;//默认接受任务
            entity.Score = 0;
            Stream imgStream = null;
            try
            {
                imgStream = Request.Files[0].InputStream;
                int length = (int)imgStream.Length;
                byte[] image = new byte[length];
                imgStream.Read(image, 0, length);
                imgStream.Close();
                entity.VolHeadImg = image;
            }
            catch (Exception e)
            {

                return Json("注册出现错误");
                throw e;
            }
            finally
            {
                if (imgStream != null)
                {
                    imgStream.Close();
                }
            }
            using (IplusOADBContext db = new IplusOADBContext())
            {

                db.Add<VolunteerEntity>(entity);
                db.SaveChanges();
                db.Dispose();
                return Json("注册成功");
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