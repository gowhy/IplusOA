using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace Common
{
    public class HelpSerializer
    {
        public static object DeSerialize(string xml, Type type)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");
            if (type == null)
                throw new ArgumentNullException("type");

            return HelpSerializer.DeSerialize(xml, type, Encoding.UTF8);

        }
        public static object DeSerialize(string xml, Type type, Encoding encode)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");
            if (encode == null)
                throw new ArgumentNullException("encode");
            if (type == null)
                throw new ArgumentNullException("type");

            object result;
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(type);
                using (MemoryStream myFileStream = new MemoryStream(encode.GetBytes(xml)))
                {
                    object myObject = mySerializer.Deserialize(myFileStream);
                    myFileStream.Close();
                    myFileStream.Dispose();
                    result = myObject;
                }
            }
            catch (Exception)
            {

                result = null;
            }

            return result;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Serialize(object obj, Type type)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (type == null)
                throw new ArgumentNullException("type");

            return HelpSerializer.Serialize(obj, type, Encoding.UTF8);

        }
        public static string Serialize(object obj, Type type, Encoding encode)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (type == null)
                throw new ArgumentNullException("type");
            if (encode == null)
                throw new ArgumentNullException("encode");

            string result = "";
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(type);
                MemoryStream myStream = new MemoryStream();
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);
                mySerializer.Serialize(myStream, obj, xmlns);
                string xml = encode.GetString(myStream.GetBuffer());
                myStream.Close();
                myStream.Dispose();

                char[] trimChars = new char[1];
                result = xml.TrimEnd(trimChars);

            }
            catch (Exception)
            {
                return "";
            }
            return result;
        }

        /// <summary>
        /// XML String 反序列化成对象
        /// </summary>
        public static T Deserialize<T>(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
                throw new ArgumentNullException("xmlString");

            T t = default(T);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }
            return t;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return HelpSerializer.Serialize<T>(obj, Encoding.UTF8);
        }
        public static string Serialize<T>(T obj, Encoding encode)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (encode == null)
                throw new ArgumentNullException("encode");

            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = encode.GetString(ms.ToArray());
            }
            return xmlString;
        }

        public static T JSONDeserialize<T>(string jsonString)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(jsonString);
        }
        public static string JSONSerialize<T>(T obj)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();

            //return js.Serialize(obj);

            MemoryStream ms = null;

            try
            {
                DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
                ms = new MemoryStream();

                ds.WriteObject(ms, obj);

                string strReturn = Encoding.UTF8.GetString(ms.ToArray());

                return strReturn;
            }
            //catch (Exception ex)
            //{
            //    return "";
            //}
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }

        }
    }
}
