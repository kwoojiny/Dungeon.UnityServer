using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CSampleServer
{


    public class DBConnecter
    {
        static String strConn = "Server=localhost;Database=Dungeon;Uid=GameServer;Pwd=teamg4;";
        MySqlCommand scom = new MySqlCommand();
        MySqlConnection conn = new MySqlConnection(strConn);
        MySqlDataReader sdr = null;

        public DBConnecter()
        {

        }
        public string registerse(string id, string password)
        {
            Console.WriteLine(id + password);
            conn.Open();
            scom.Connection = conn;
            scom.CommandText = "select * from userlist where userid = '" + id + "' and password = '" + password + "'";
            scom.ExecuteNonQuery();
            sdr = scom.ExecuteReader();
            while (sdr.Read())
            {
                var p = new Userinfo { id = sdr["userid"].ToString(), nickname = sdr["nickname"].ToString() };
                if (sdr["nickname"].ToString() == "")
                {
                    p.nickname = null;
                }
                string jsonString = JsonConvert.SerializeObject(p);
                sdr.Close();
                conn.Close();

                return jsonString;
            }
            sdr.Close();
            conn.Close();
            return null;
        }


        public string ids(string id, string password) // 회원의 닉네임과 아이디를 전송합니다.
        {
            Console.WriteLine(id + password);
            conn.Open();
            scom.Connection = conn;
            scom.CommandText = "select * from userlist where userid = '" + id + "' and password = '" + password + "'";
            scom.ExecuteNonQuery();
            sdr = scom.ExecuteReader();
            while (sdr.Read())
            {
                var p = new Userinfo { id = sdr["userid"].ToString(), nickname = sdr["nickname"].ToString() };
                if (sdr["nickname"].ToString() == "")
                {
                    p.nickname = null;
                }
                string jsonString = JsonConvert.SerializeObject(p);
                sdr.Close();
                conn.Close();
                return jsonString;
            }
            sdr.Close();
            conn.Close();
            return null;
        }

        public string userinfos(string id) // 유저의 정보를 전송합니다.(보류)
        {
            conn.Open();
            string[] infos = new string[2];
            Console.WriteLine(id);
            scom.Connection = conn;
            scom.CommandText = "select id, nickname from userinfo where userid = '" + id + "'";
            scom.ExecuteNonQuery();
            MySqlDataReader sdr = scom.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    infos[0] = sdr.GetString(0);
                    infos[1] = sdr.GetString(1);

                    Console.WriteLine(infos[0] + " , " + infos[1]);
                }
            }
            conn.Close();
            return infos[0] + "," + infos[1];
        }

        public string idregi(string id, string password) // 회원가입 메소드입니다.
        {
            Console.WriteLine(id + password);
            conn.Open();
            scom.Connection = conn;
            scom.CommandText = "select userid from userlist where userid = '" + id + "'";
            scom.ExecuteNonQuery();
            sdr = scom.ExecuteReader();
                if(!sdr.Read())
                {
                    sdr.Close();
                    scom.CommandText = "insert into userlist values ('" + id + "','" + password + "'," + "null, 1)";
                    scom.ExecuteNonQuery();
                    conn.Close();
                    return id + password;
                }
                else
                {
                    if(sdr.Read()) 
                    {
                    sdr.Close();
                    conn.Close();
                    return "IDExist";
                    }
                    else { return null; }
                  
                    

                }

            }

        }

    }




