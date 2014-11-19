using Common;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BackWebAdmin.CommService
{
    public class UploadFile
    {
        public static SocSerImgEntity SaveFile(System.Web.HttpPostedFileBase file, string dir, string fileName)
        {

            string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
            string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
            string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

            string Dir = DateTime.Now.ToString("yyyyMMdd");
            FTPHelper ftp = new FTPHelper(FtpServer, dir+ "/" + Dir, FtpUser, FtpPassWord);

         
            if (string.IsNullOrEmpty(fileName))
            {
                FileInfo file2 = new FileInfo(file.FileName);
                 fileName =Guid.NewGuid() + file2.Extension;
            }
         

            ftp.Upload(file, fileName);

            SocSerImgEntity img = new SocSerImgEntity();
            img.FTPUrl = ftp.FtpURI;
            img.HttpUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
            img.Name = file.FileName;
            img.Module = "保存服务";
            img.AddTime = DateTime.Now;
            return img;
        }
    }
}