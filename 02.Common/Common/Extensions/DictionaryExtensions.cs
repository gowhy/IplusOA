﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Common
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        public static void Merge(this IDictionary<string, object> instance, string key, object value, bool replaceExisting)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNull(value, "value");

            if (replaceExisting || !instance.ContainsKey(key))
            {
                instance[key] = value;
            }
        }

        /// <summary>
        /// Appends the in value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="value">The value.</param>
        public static void AppendInValue(this IDictionary<string, object> instance, string key, string separator, object value)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotEmpty(separator, "separator");
            Check.Argument.IsNotNull(value, "value");

            instance[key] = instance.ContainsKey(key) ? instance[key] + separator + value : value.ToString();
        }

        public static void AddStyleAttribute(this IDictionary<string, object> instance, string key, string value)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNull(value, "value");

            string style = string.Empty;

            if (instance.ContainsKey("style"))
            {
                style = (string)instance["style"];
            }

            var builder = new StringBuilder(style);
            builder.Append(key);
            builder.Append(":");
            builder.Append(value);
            builder.Append(";");

            instance["style"] = builder.ToString();
        }
        /// <summary>
        /// Appends the specified value at the beginning of the existing value
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        /// <param name="separator"></param>
        /// <param name="value"></param>
        public static void PrependInValue(this IDictionary<string, object> instance, string key, string separator, object value)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotEmpty(separator, "separator");
            Check.Argument.IsNotNull(value, "value");

            instance[key] = instance.ContainsKey(key) ? value + separator + instance[key] : value.ToString();
        }

        /// <summary>
        /// Toes the attribute string.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string ToAttributeString(this IDictionary<string, object> instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            var attributes = new StringBuilder();

            foreach (KeyValuePair<string, object> attribute in instance)
            {
                attributes.Append(" {0}=\"{1}\"".FormatWith(HttpUtility.HtmlAttributeEncode(attribute.Key), HttpUtility.HtmlAttributeEncode(attribute.Value.ToString())));
            }

            return attributes.ToString();
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="from">From.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        public static void Merge(this IDictionary<string, object> instance, IDictionary<string, object> from, bool replaceExisting)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(from, "from");

            foreach (KeyValuePair<string, object> pair in from)
            {
                if (!replaceExisting && instance.ContainsKey(pair.Key))
                {
                    continue; // Try the next
                }

                instance[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="from">From.</param>
        public static void Merge(this IDictionary<string, object> instance, IDictionary<string, object> from)
        {
            Merge(instance, from, true);
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        public static void Merge(this IDictionary<string, object> instance, object values, bool replaceExisting)
        {
            Merge(instance, new RouteValueDictionary(values), replaceExisting);
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        public static void Merge(this IDictionary<string, object> instance, object values)
        {
            Merge(instance, values, true);
        }
    }
}
