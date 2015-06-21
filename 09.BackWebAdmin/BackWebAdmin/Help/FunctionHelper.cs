using DataLayer.IplusOADB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin
{
    public class FunctionHelper
    {
        public  Dictionary<string, string> dicSerType = new Dictionary<string, string>();
        private Dictionary<string, string> dicSorgType = new Dictionary<string, string>();
        public FunctionHelper()
        {
            //dicSerType.Add("GYFN", "法律服务");
            //dicSerType.Add("GYYLFW", "医疗服务");
            //dicSerType.Add("GYWH", "文化娱乐");
            //dicSerType.Add("GYYL", "居家养老");
            //dicSerType.Add("GYBF", "社区帮扶");
            //dicSerType.Add("GYWHKT", "文化课堂");
            //dicSerType.Add("GYNSD", "社区绿丝带");
            //dicSerType.Add("SYJZ", "社区家政");
            //dicSerType.Add("SYMY", "美食娱乐");
            //dicSerType.Add("SYWX", "家居维修");
            //dicSerType.Add("SYJR", "金融服务");
            //dicSerType.Add("SYYH", "优惠进社区");
            //dicSerType.Add("JYPX", "教育培训");
            //dicSerType.Add("MSKD", "美食快递");
            //dicSerType.Add("XLYL", "休闲娱乐");
            //dicSerType.Add("LRYS", "丽人养生");
            //dicSerType.Add("XHKD", "鲜花快递");
            //dicSerType.Add("SQCF", "社区厨房");
            //dicSerType.Add("TV8", "TV8折购物");
            //dicSerType.Add("MYQZ", "母婴亲子");
            //dicSerType.Add("SSSG", "生疏水果");


            using (IplusOADBContext db = new IplusOADBContext())
            {

               var  tmp = db.SerTypesTable.Where(x => x.Id > 0).ToList();
               foreach (var item in tmp)
               {
                   dicSerType.Add(item.Code, item.Name);
               }
            }
            dicSorgType.Add("SS", "枢纽型社会组织");
            dicSorgType.Add("MF", "民非类社会组织");
            dicSorgType.Add("ST", "社团类社会组织");
            dicSorgType.Add("SY", "商业类社会组织");

        }
        public string GetSerTypeName(string typeCode)
        {

            if (string.IsNullOrEmpty(typeCode))
            {
                return typeCode;
            }
            if (dicSerType.ContainsKey(typeCode))
            {   return dicSerType[typeCode];
                
            }
            return "";
        }
        public string GetSorgTypeName(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode))
            {
                return typeCode;
            }
            return dicSorgType[typeCode];
        }
    
    }
    public static class FunctionHelperExntensions
    {
        private static readonly FunctionHelper functionHelper = new FunctionHelper();

        public static FunctionHelper F(this HtmlHelper page)
        {
            return functionHelper;
        }
    }
}