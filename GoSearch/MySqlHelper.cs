using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace GoSearch
{
    public class MySqlHelper
    {
        //从配置文件读取连接字符串
        private static readonly string connstr = ConfigurationManager.ConnectionStrings["connStrs"].ConnectionString;

        //创建连接
        public MySqlConnection CreateConnection()
        {
            MySqlConnection conn = new MySqlConnection(connstr);
            conn.Open();
            return conn;
        }

        public int ExecuteNonQuery(MySqlConnection conn, string sql, params MySqlParameter[] parameters)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteNonQuery(conn, sql, parameters);
            }
        }

        public object ExecuteScalar(MySqlConnection conn, string sql, params MySqlParameter[] parameters)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, sql, parameters);
            }
        }

        public DataTable ExecuteQuery(MySqlConnection conn, string sql, MySqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            return table;
        }

        public DataTable ExecuteQuery(string sql, params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteQuery(conn, sql, parameters);
            }
        }

        public DataTable ExecuteQuery(MySqlConnection conn, string sql)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }
            }
            return table;
        }
        public DataTable ExecuteQuery(string sql)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteQuery(conn, sql);
            }
        }
    }
}