using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using Common;
using Backend.Network;
using Npgsql;
using Backend.DataBase;
using FrontEnd.Item;
class Program
{
    static public int Main(String[] args)
    {
        if (args.Length != 2)
        {
            System.Console.WriteLine("backend.exe {Configure File Path} {ItemTemplate Configure File Path}");
            return 0;
        }


        string confPath = args[0];
        XmlSerializer serializer = new XmlSerializer(typeof(BackendConf));
        StreamReader reader = new StreamReader(confPath);
        BackendConf conf = (BackendConf)serializer.Deserialize(reader);
        

        string itemConf = args[1];
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(FrontendConf));
        StreamReader streamReader = new StreamReader(itemConf);
        FrontendConf itemTemplate = (FrontendConf)xmlSerializer.Deserialize(streamReader);

        FrontEnd.Item.FItem.itemConfs = itemTemplate.item_template;

        if (!GameDataBase.Connect(conf.db_host, conf.db_port, conf.db_username, conf.db_password, conf.db_dbname))
        {
            System.Console.WriteLine("Can't connect to the database");
            return 0;
        }
        GameServer gs = new GameServer(conf);
        gs.StartUp();

        return 0;
    }
}
