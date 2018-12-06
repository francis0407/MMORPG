using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
namespace Backend.DataBase
{
    class GameDataBase
    {
        static private string connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=Game-1.0-dev";
        //public NpgsqlConnection conn = null;
        static public bool Connect(string _connString)
        {
            var conn = new NpgsqlConnection(_connString);                       
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                return false;
            }
            connString = _connString;
            return true;
        }

        static public bool Connect(string host, short port, string username, string password, string dbname)
        {
            string _connString = string.Format(
                "Host={0};Port={1};Username={2};Password={3};Database={4}",
                host, port, username, password, dbname);
            var conn = new NpgsqlConnection(_connString);
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                return false;
            }
            connString = _connString;
            return true;
        }

        static public NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(connString);
            conn.Open();
            return conn;
        }

        static public NpgsqlCommand GetCmd()
        {
            var conn = new NpgsqlConnection(connString);
            conn.Open();
            return conn.CreateCommand();
        }

        static public int SQLNoneQuery(string sql)
        {
            var cmd = GetCmd();
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }

        static public NpgsqlDataReader SQLQuery(string sql)
        {
            var cmd = GetCmd();
            cmd.CommandText = sql;
            return cmd.ExecuteReader();
        }

        static public Object SQLQueryScalar(string sql)
        {
            var cmd = GetCmd();
            cmd.CommandText = sql;
            return cmd.ExecuteScalar();
        }
    }
}
