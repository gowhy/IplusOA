using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Common
{

    public class BaseExcelHelper
    {
        /// <summary>
        /// 读取Excel的连接串
        /// </summary>
        public string ExcelConnectionString;
        /// <summary>
        /// 本地Excel的完整文件名，即Url全路径
        /// </summary>
        public string ExcelFileName;
        /// <summary>
        /// 要读取Excel的表格名称，即读取的sheet
        /// </summary>
        public string ExceltableName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="excelFileName"> 本地Excel的完整文件名，即Url全路径</param>
        /// <param name="tableName">要读取Excel的表格名称，即读取的sheet</param>
        public BaseExcelHelper(string excelFileName, string tableName)
        {
            ExcelFileName = excelFileName;
            ExceltableName = tableName;

            FileInfo file = new FileInfo(excelFileName);
            if (!file.Exists) { throw new Exception("文件不存在"); }
            string extension = file.Extension;
            switch (extension)
            {
                case ".xls":
                    ExcelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    break;
                case ".xlsx":
                    ExcelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=Yes;\"";
                    break;
                default:
                    ExcelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFileName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                    break;
            }

        }

    }

    /// <summary>
    /// Excel操作类
    /// </summary>
    public class ExcelHelper : BaseExcelHelper
    {

        public ExcelHelper(string excelFileName, string tableName)
            : base(excelFileName, tableName)
        {

        }

        #region 数据导出至Excel文件
        /// </summary> 
        /// 导出Excel文件，自动返回可下载的文件流 
        /// </summary> 
        public static void DownloadExcel(System.Data.DataTable dtData, string fileName)
        {

            GridView gvExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;
            if (dtData != null)
            {
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8).Replace("+", "%20") + ".xls");
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.Default;
                curContext.Response.Charset = "gb2312";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                gvExport = new GridView();
                gvExport.DataSource = dtData.DefaultView;
                gvExport.AllowPaging = false;
                gvExport.Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                gvExport.DataBind();
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\"/>" + strWriter.ToString());
                curContext.Response.End();
            }
        }


        #endregion

        /// <summary>
        /// 获取Excel文件数据表列表
        /// </summary>
        public ArrayList GetExcelTables()
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection(ExcelConnectionString))
                {
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //获取数据表个数
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (TablesList.IndexOf(tablename) < 0)
                        {
                            TablesList.Add(tablename);
                        }
                    }
                }
            }
            return TablesList;
        }

        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// </summary>
        public DataTable InputFromExcel()
        {

            //如果数据表名不存在，则数据表名为Excel文件的第一个数据表
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables();

            if (TableList.IndexOf(ExceltableName) < 0)
            {
                ExceltableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection(ExcelConnectionString);
            OleDbCommand cmd = new OleDbCommand("select * from [" + ExceltableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State != ConnectionState.Closed)
                {
                    dbcon.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// 获取Excel文件指定数据表的数据列表
        /// </summary>

        public ArrayList GetExcelTableColumns()
        {
            DataTable dt = new DataTable();
            ArrayList ColsList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection(ExcelConnectionString))
                {
                    conn.Open();
                    dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, ExceltableName, null });

                    //获取列个数
                    int colcount = dt.Rows.Count;
                    for (int i = 0; i < colcount; i++)
                    {
                        string colname = dt.Rows[i]["Column_Name"].ToString().Trim();
                        ColsList.Add(colname);
                    }
                }
            }
            return ColsList;
        }
    }
}