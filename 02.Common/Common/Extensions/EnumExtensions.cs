
using Common.CommAttribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Web.Mvc;
namespace Common
{
 public static class EnumExtensions
    {

        public static string ToLocalizable(this Enum value,Type language)
        {
           var customAttr =(Common.CommAttribute.LocalizableAttribute)Attribute.GetCustomAttribute(value.GetType(), typeof(Common.CommAttribute.LocalizableAttribute));
            var resources = new ResourceManager(language);
            var prefix = customAttr.Prefix;
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = value.GetType().Name;
            }

            string rk = String.Format("{0}_{1}", prefix, value);
            string localiza = resources.GetString(rk);
            if (localiza == null)
            {
                return !string.IsNullOrWhiteSpace(customAttr.Default)?customAttr.Default:value.ToString();
            }
            return localiza;
        }

        public static string ToLocalizable(this Enum value)
        {
            var localiza = value.ToDescription();
            return string.IsNullOrWhiteSpace(localiza) ? value.ToString() : localiza;
        }

        public static string ToDescription(this Enum value)
        {
            return EnumItemDescriptionAttribute.GetDisplayValue(value, value.GetType());
        }

        public static IEnumerable<KeyValuePair<int, Enum>> ToKeyValueParir(this Enum e)
        {
            return (from Enum d in e.GetType().GetEnumValues() select new KeyValuePair<int, Enum>(Convert.ToInt32(d), d)).ToList();
        }

        #region EnumToList

        public class EnumKeyValuePair
        {
            public string Value;
            public string Text;
            public string Description;
        }

        public static string GetEnumDescription(object e)
        {
            var ms = e.GetType().GetFields();
            foreach (
                var dscript in
                    (from f in ms
                     where f.Name == e.ToString()
                     from Attribute attr in f.GetCustomAttributes(true)
                     select attr).OfType<DescriptionAttribute>())
            {
                return dscript.Description;
            }
            return e.ToString();
        }

        public static List<EnumKeyValuePair> EnumToList<T>()
        {
            return (from int s in Enum.GetValues(typeof(T))
                    select new EnumKeyValuePair
                    {
                        Value = s.ToString(),
                        Text = Enum.GetName(typeof(T), s),
                        Description = GetEnumDescription(Enum.ToObject(typeof(T), s)),
                        //Text = GetEnumDescription(Enum.ToObject(typeof (T), s)),
                    }).ToList();
        }


        #endregion

        public static IEnumerable<SelectListItem> ToSelectItem(this Enum e) {
            var resul = new List<SelectListItem>();
            var values = e.GetType().GetEnumValues();
            foreach(var v in values) {
                var item = new SelectListItem {
                    Text = (Enum.ToObject(e.GetType(), v) as Enum).ToLocalizable(),
                    Value = ((int)v).ToString()
                };
                resul.Add(item);
            }
            return resul;
        }

    }
}

