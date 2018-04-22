using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using GoSearch;
using static GoSearch.Service.Common;

namespace GoSearch.Controllers
{
    public class SearchController : ApiController
    {
       
        private const int SessionIdPositionInResponse = 0;

        /// <summary>
        /// OpenIdPositionInResponse use in request we-chat service url 
        /// </summary>
        private const int OpenIdPositionInResponse = 2;
        private TimeConversion timeConversion = new TimeConversion();
        /// <summary>
        /// UserIdPositionInResponse use in request we-chat service url 
        /// </summary>
        private const int UserIdPositionInResponse = 0;
        private readonly SiteConfigurations siteConfigurations;
        private static string strcon = "Data Source=CS-WSJDESKTOP\\SQLEXPRESS;Initial Catalog=GoSearch;Integrated Security=True";
        public SearchController()
            : this(Singleton<SiteConfigurations>.Instance)
        {
        }
        public SearchController(SiteConfigurations siteConfigurations)

        {
            this.siteConfigurations = siteConfigurations;
        }
        [System.Web.Http.HttpGet]
        public string Login(string code, string session)
        {
        
            //如果没有session,说明是新用户 ,跳转到注册
            if (session == null)
            {
                return this.Login(code);
            }
            //如果有session,检验是否过期
            else
            {
                int? userId = null;
                CheckSession checkSession = new CheckSession();
                int userId_fromSession = checkSession.GetUserId(session);
                string sql = "is_user_exit_by_userid_procedure";
                MySqlParameter[] para =    //基于mysql的实现参数传入的方法，要定义类型和长度
                {
                new MySqlParameter("@user_id",MySqlDbType.String),
            };
                para[0].Value = userId_fromSession;
                MySqlHelper sqlHelper = new MySqlHelper();
                DataTable table = sqlHelper.ExecuteQuery(sql, para);
                userId = Convert.ToInt32(table.Rows[0][0].ToString());
                if (userId.HasValue)
                {
                    return Convert.ToString(userId) + ';' + DateTime.Now.ToUniversalTime().AddHours(this.timeConversion.BeiJing).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return this.Login(code);
                }
            }
        }
        public string Login(string code)
        {
            //与微信交互,获取openId和session
            string url = string.Format(
                              this.siteConfigurations.RemoteAuthenticationServerUrlTemplate,
                              this.siteConfigurations.ApplicationId,
                              this.siteConfigurations.ApplicationSecret,
                              code);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string value;
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    value = streamReader.ReadToEnd();
                }
            }
            JObject jo = JObject.Parse(value);
            string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
            string session_key = values[SessionIdPositionInResponse];
            string openid = values[OpenIdPositionInResponse];
            //通过openid查看是否存在user
            int? userId = null;
            string sql = "get_userId_by_openid_procedure";
            MySqlParameter[] para =    //基于mysql的实现参数传入的方法，要定义类型和长度
            {
                new MySqlParameter("@open_id",MySqlDbType.
                String),
            };
            para[0].Value = openid;
            MySqlHelper sqlHelper = new MySqlHelper();
            DataTable table = sqlHelper.ExecuteQuery(sql, para);
            //获取到user
            userId = Convert.ToInt32(table.Rows[0][0]);
            //return Convert.ToString(userId);
            return Convert.ToString(userId) + ';' + DateTime.Now.ToUniversalTime().AddHours(this.timeConversion.BeiJing).ToString("yyyy-MM-dd HH:mm:ss");
        }
        //
        //获取用户的id
        public int GetUserId() {
            int id = 0;
            using (SqlConnection mycon = new SqlConnection(strcon)) {
                mycon.Open();
                String selectString = "SELECT Client.client_id FROM Client JOIN Account ON Client.client_id = Account.owner_Id WHERE Account.account_id = 1253; ";
                SqlCommand sqlcmd = new SqlCommand(selectString, mycon);
                try
                  {
                    object o= sqlcmd.ExecuteScalar();
                    if (o != null)
                    {
                        id = (int)o;
                        return id;
                    }
                }
                catch
                {

                }
            }
            return id;
        }
        //注册
        [HttpGet]
        public int Register(int userId)
        {
            int x = 1;
            using (SqlConnection mycon = new SqlConnection(strcon))
            {
                mycon.Open();
                String selectString = "SELECT Client.client_id FROM Client JOIN Account ON Client.client_id = Account.owner_Id WHERE Account.account_id = 1253; ";
                SqlCommand sqlcmd = new SqlCommand(selectString, mycon);
                try
                {
                    object o = sqlcmd.ExecuteScalar();
             
                }
                catch
                {

                }
            }
            return 1;
        }
    }

}
