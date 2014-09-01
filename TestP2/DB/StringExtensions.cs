using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Drawing;
using System.Text.RegularExpressions;

namespace QDT.Core.MSData
{
    public static class StringExtensions
    {
/*
        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };
*/

        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string FormatWith(this string target, params object[] args)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException("target");
            return string.Format(target, args);
        }

        public static string FormatWith(this string target, IFormatProvider provider, params object[] args)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException("target");
            return string.Format(provider, target, args);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="target"></param>
        /// <param name="Hex">是否以16进制显示字符串</param>
        /// <param name="utf8">以Utf8转码，false以Unicode转码</param>
        /// <returns></returns>
        public static string Hash(this string target, bool Hex = true, bool utf8 = true)
        {
            if (String.IsNullOrEmpty(target))
                throw new ArgumentNullException("target");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = utf8 ? Encoding.UTF8.GetBytes(target) : Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);
                if (Hex)
                {
                    string result = "";
                    for (int i = 0; i < hash.Length; i++)
                    {
                        result += hash[i].ToString("X2");
                    }
                    return result;
                }
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// salt hash加密（非对称加密），实用于membership
        /// </summary>
        /// <returns></returns>
        public static string SaltAndHash(this string rawString, string salt)
        {
            byte[] salted = Encoding.UTF8.GetBytes(string.Concat(rawString, salt));
            SHA256 hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(salted);
            return Convert.ToBase64String(hashed);
        }


        public static string WrapAt(this string target, int index)
        {
            const int DotCount = 3;

            if (String.IsNullOrEmpty(target))
                return "";
            if (index <= 0)
                return "";

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
        }

        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                }
            }

            return result;
        }

        public static string ToConfuse(this string target, int before, int after, char value, int length=-1)
        {
            if (string.IsNullOrWhiteSpace(target)) return target;
            if (length==-1)
            {
                length = target.Length - before - after;
            }
            if (length <= 0)
            {
                return target;
            }
            var confuseStr = "";
            for (var i = 0; i < length; i++)
            { 
                confuseStr += value;
            }
            var beforeStr = target.Substring(0, before);
            var start = target.Length - after;
            if (start < 0) start = 0;
            var afterStr = target.Substring(start, after>target.Length?target.Length:after);
            return beforeStr + confuseStr + afterStr;
        }

        public static string ToConfuse(this string target, int before, int after)
        {
            return ToConfuse(target, before, after, '*');
        }

        public static string ToConfuse(this string target, int before, int after, int length)
        {
            return ToConfuse(target, before, after, '*', length);
        }

        public static string ToDesEncrypt(this string target,string key=Des.KeyString)
        {
            return Des.DesEncrypt(target,key); 
        }

        public static string ToDesDecrypt(this string target,string key=Des.KeyString)
        {
            return Des.DesDecrypt(target,key);
        }

        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            //oldValues.Each(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        public static string ReplaceStart(this string target,string oldValue,string newValue) {
            var start = new Regex("^" + oldValue, RegexOptions.Compiled);
            return start.Replace(target, newValue);
        }

        public static string ReplaceEnd(this string target,string oldValue,string newValue)
        {
            var end = new Regex(oldValue + "$", RegexOptions.Compiled);
            return end.Replace(target, newValue);
        }

        #region  通过字符串配置数据类型
        /*
        public static Object Format(this string str, Type type)
        {
            if (String.IsNullOrEmpty(str))
                return null;
            if (type == null)
                return str;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                String[] strs = str.Split(new char[] { ';' });
                Array array = Array.CreateInstance(elementType, strs.Length);
                for (int i = 0, c = strs.Length; i < c; ++i)
                {
                    array.SetValue(ConvertSimpleType(strs[i], elementType), i);
                }
                return array;
            }
            return ConvertSimpleType(str, type);
        }

        private static object ConvertSimpleType(object value, Type destinationType)
        {
            object returnValue;
            if (value == null || destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;

            if ((str != null) && (str.Length == 0))
            {
                return null;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);

            bool flag = converter.CanConvertFrom(value.GetType());

            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                throw new InvalidOperationException("无法转换成类型：" + value.ToString() + "==>" + destinationType);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>" + destinationType, e);
            }
            return returnValue;
        }
         */
        #endregion

        /// <summary>
        /// 获得文本的宽度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static int GetTextWidth(string text, Font font)
        {
            var bitmap = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bitmap);

            SizeF textsize = graphics.MeasureString(text, font);

            return (int)Math.Ceiling(textsize.Width);

        }

        /// <summary>
        /// 获得在最大宽度下的可显示文本（带省略号或者不带省略号）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="maxWidth"></param>
        /// <param name="ellipsisEnd"></param>
        /// <returns></returns>
        public static int GetTextPosofWord(string text, Font font, int maxWidth, bool ellipsisEnd)
        {
            if (maxWidth <= 0)
                return 0;

            if (GetTextWidth(text, font) < maxWidth)
                return text.Length;


            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            int charactersFitted;
            int linesFilled;

            var layoutSize = new SizeF(maxWidth, 10);

            var sf = new StringFormat
                         {
                             FormatFlags = StringFormatFlags.NoWrap,
                             Trimming = ellipsisEnd ? StringTrimming.EllipsisWord : StringTrimming.Word
                         };


            graphics.MeasureString(text, font, layoutSize, sf, out charactersFitted, out linesFilled);

            return GetTextWidth(text.Substring(0, charactersFitted), font);
        }

        /// <summary>
        /// 在最大宽度下获得可显示的文本，以省略号结束
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static string GetEllipsisText(this string text, Font font, int maxWidth)
        {
            int txtLen = GetTextPosofWord(text, font, maxWidth, true);
            if (text.Length > txtLen)
                return text.Substring(0, txtLen) + "...";
            return text.Substring(0, txtLen);
        }


        /// <summary>
        /// 在最大宽度下获得可显示的文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static string GetMaxText(this string text, Font font, int maxWidth)
        {
            return text.Substring(0, GetTextPosofWord(text, font, maxWidth, false));
        }

        /// <summary>
        /// Email掩码转换，默认情况下为切除@和@后面的email域
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string EmailMask(this string email)
        {
          //  if (!email.IsEmail()) return email;
            string name = email.Split('@')[0];
            return name;
        }

        /// <summary>
        /// 用指定的分隔符，将字符串分割为数组，并排除数组中的空字符项。
        /// </summary>
        /// <param name="source">需要分割的字符串</param>
        /// <param name="separator">分割字符</param>
        /// <returns></returns>
        public static string[] SplitNull(this string source,char separator)
        {
            if(source==null) return new string[]{};
            string[] target = source.Split(separator);
            return target.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        /// <summary>
        /// 用指定的分隔符，将字符串数组组合成字符串
        /// 注意，对数组中空字符串项将忽略
        /// </summary>
        /// <param name="array">需要组合的数组</param>
        /// <param name="separator">分割字符</param>
        /// <param name="startAndEnd">是否首位需要增加分隔字符</param>
        /// <returns></returns>
        public static string JoinArray(this string[] array, char separator, bool startAndEnd = false)
        {
            var sb = new StringBuilder();
            foreach (string item in array)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                sb.AppendFormat("{0}{1}",item,separator);
            }
            if (sb.Length>0)
            {
                if (startAndEnd)
                {
                    sb.Insert(0, separator);
                }
                else
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 比较两个字符串完全相同，区分大小写
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="comparing"></param>
        /// <returns></returns>
        public static bool IsCaseSensitiveEqual(this string instance, string comparing)
        {
            return string.CompareOrdinal(instance, comparing) == 0;
        }

        /// <summary>
        /// 比较两个字符串完全相同,不区分大小写
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="comparing"></param>
        /// <returns></returns>
        public static bool IsCaseInsensitiveEqual(this string instance, string comparing)
        {
            return string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
