using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin
{
    public class FunctionHelper
    {
        private Dictionary<string, string> dicSerType = new Dictionary<string, string>();
        public FunctionHelper()
        {
            dicSerType.Add("GYFN", "法律服务");
            dicSerType.Add("GYYLFW", "医疗服务");
            dicSerType.Add("GYWH", "文化娱乐");
            dicSerType.Add("GYYL", "居家养老");
            dicSerType.Add("GYBF", "社区帮扶");
            dicSerType.Add("GYWHKT", "文化课堂");
            dicSerType.Add("GYNSD", "社区绿丝带");
            dicSerType.Add("SYJZ", "社区家政");
            dicSerType.Add("SYMY", "美食娱乐");
            dicSerType.Add("SYWX", "家居维修");
            dicSerType.Add("SYJR", "金融服务");
            dicSerType.Add("SYYH", "优惠进社区");
      
        }
        public string GetSerTypeName(string typeCode)
        {
            if (string.IsNullOrEmpty(typeCode))
            {
                return typeCode;
            }
            return dicSerType[typeCode];
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