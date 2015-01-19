using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Common
{

    /// <summary>
    /// 上传下载文件帮助类
    /// </summary>
    public class FileHelper
    {

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="myFile">文件内容</param>
        /// <param name="filePathFullName">保存文件完整路径名称，包括文件名</param>
        /// <returns>true:上传成功;false上传失败</returns>
        public static bool Upload(HttpPostedFileBase myFile, string filePathFullName)
        {
            bool result = false;
           
            if (myFile != null)
            {
                if (myFile.InputStream.Length != 0)
                {
                    try
                    {
                        FileInfo file = new FileInfo(filePathFullName);
                        DirectoryInfo dir = new DirectoryInfo(file.DirectoryName);
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }
                        myFile.SaveAs(filePathFullName);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="myFile">文件内容</param>
        /// <param name="filePathFullName">保存文件完整路径名称，包括文件名</param>
        /// <returns>true:上传成功;false上传失败</returns>
        public static bool  Upload(HttpPostedFile myFile,  string filePathFullName)
        {
            bool result = false;
           
            if (myFile != null)
            {
                if (myFile.InputStream.Length != 0)
                {
                    try
                    {
                        FileInfo file = new FileInfo(filePathFullName);
                        DirectoryInfo dir = new DirectoryInfo(file.DirectoryName);
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }
                        myFile.SaveAs(filePathFullName);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileFullName">文件在服务器上的完整名称</param>
        /// <param name="downloadFileName"></param>
        /// <returns></returns>
        public static bool Download(string fileFullName,string downloadFileName)
        {
            bool result = false;
          HttpResponse Response=  HttpContext.Current.Response;

          
            string filePath = fileFullName;//路径

            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(downloadFileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();



            return result;
        }
    }
}
