using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
namespace Backend.DataBase
{
    class GameDataBase:Singleton<GameDataBase>
    {
        // public const string connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=Game-1.0-dev";
        public NpgsqlConnection conn = null;
        public bool Connect(string connString)
        {
            conn = new NpgsqlConnection(connString);                       
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool Connect(string host, short port, string username, string password, string dbname)
        {
            string connString = string.Format(
                "Host={0};Port={1};Username={2};Password={3};Database={4}",
                host, port, username, password, dbname);

            conn = new NpgsqlConnection(connString);
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;


        }
        public NpgsqlCommand GetCmd()
        {
            return conn.CreateCommand();
        }
   
    }
}
