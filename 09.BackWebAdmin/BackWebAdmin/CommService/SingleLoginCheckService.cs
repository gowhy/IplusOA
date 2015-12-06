using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.IplusOADB;
using System.Data.Entity;
using IplusOAEntity;
using BusLogic.Login;

namespace BackWebAdmin.CommService
{
    public class SingleLoginCheckService
    {
        /// <summary>
        ///true： 该手机已经登陆过
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static bool Check(SingleLoginCheck model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
               //每次登陆记录登陆信息
                //model.AddTime = DateTime.Now;
                //db.Add(model);
                //db.SaveChangesAsync();

                var slc = db.SingleLoginCheckTable;

                SingleLoginCheck entity = slc.Where(x => x.UserId == model.UserId && x.PCode != "" && x.PCode != null).OrderByDescending(x => x.Id).FirstOrDefault();
                if (entity != null)
                {
                    if (!string.IsNullOrEmpty(entity.PCode) && model.PCode == entity.PCode && entity.State == 0)//上一次和这一次的编码一致
                    {
                        return true;
                    }
                    else
                    {                    
                        return false;
                    }             
                }
                else
                {
                    return false;
                }
              
            }
        }

        public static void SaveLoginRecord(SingleLoginCheck model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //每次登陆记录登陆信息
                model.AddTime = DateTime.Now;
                db.Add(model);
                db.SaveChangesAsync();
            }
        }
    }
}