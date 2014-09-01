using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommAttribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EnumItemDescriptionAttribute : System.Attribute
    {
        public EnumItemDescriptionAttribute(string desc)
        {
            Description = desc;
        }

        private static readonly Dictionary<Type, Dictionary<object, EnumItemDescriptionAttribute>> _cache
            = new Dictionary<Type, Dictionary<object, EnumItemDescriptionAttribute>>();

        public string Description { get; set; }

        public bool HasValue
        {
            get { return !String.IsNullOrWhiteSpace(Description); }
        }

        public static string GetDisplayValue(object value, Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
            {
                throw new NotSupportedException("enumType is not a Enum");
            }
            if (value == null || !Enum.IsDefined(enumType, value))
            {
                throw new ArgumentException("value is not defined in " + enumType.FullName);
            }
            var disc = GetDisplayValuesImp(value, enumType);
            if (disc != null && disc.HasValue) return disc.Description;
            return value.ToString();
        }

        private static EnumItemDescriptionAttribute GetDisplayValuesImp(object value, Type enumType)
        {
            if (!_cache.ContainsKey(enumType))
            {
                var displayValues =
                    new Dictionary<object, EnumItemDescriptionAttribute>();
                foreach (Enum item in Enum.GetValues(enumType))
                {
                    var enumField = enumType.GetField(item.ToString());
                    if (enumField != null)
                    {
                        object[] attrs = enumField.GetCustomAttributes(typeof(EnumItemDescriptionAttribute), false);
                        foreach (var attr in attrs)
                        {
                            displayValues.Add(item, (EnumItemDescriptionAttribute)attr);
                            break;
                        }

                    }
                }
                _cache.Add(enumType, displayValues);
            }

            return _cache[enumType][value];
        }

    }
}
