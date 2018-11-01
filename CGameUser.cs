using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;
using Newtonsoft.Json;
namespace CSampleServer
{

    using GameServer;

    /// <summary>
    /// 하나의 session객체를 나타낸다.
    /// </summary>
    class CGameUser : IPeer
    {
        DBConnecter dbc = new DBConnecter();
        CUserToken token;

        public CGameUser(CUserToken token)
        {
            this.token = token;
            this.token.set_peer(this);
        }

        void IPeer.on_message(Const<byte[]> buffer)
		{
			// ex)
			CPacket msg = new CPacket(buffer.Value, this);
			PROTOCOL protocol = (PROTOCOL)msg.pop_protocol_id();
			Console.WriteLine("------------------------------------------------------");
			Console.WriteLine("protocol id " + protocol);
			switch (protocol)
			{
				case PROTOCOL.CHAT_MSG_REQ:
					{
						string text = msg.pop_string();
						Console.WriteLine(string.Format("text {0}", text));

						CPacket response = CPacket.create((short)PROTOCOL.CHAT_MSG_ACK);
						response.push(text);
						send(response);
                        break;
					}
                case PROTOCOL.PLAYER_ID:
                    {
                        char sp = ',';
                        string text = msg.pop_string();
                        string[] idstring = text.Split(sp);
                        string id = idstring[0];
                        string password = idstring[1];
                        Console.WriteLine(string.Format("유저가 로그인을 시도했습니다. 아이디, 비밀번호 : {0}, {1}", id, password));
                        try {
                            if (dbc.ids(id, password) != null)
                            {
                                CPacket response = CPacket.create((short)PROTOCOL.PLAYER_ID);
                                response.push(dbc.ids(id, password));
                                send(response);
                            } else if(dbc.ids(id, password) == null)
                            {
                                CPacket response = CPacket.create((short)PROTOCOL.PLAYER_ID);
                                response.push("LoginFailed");
                                send(response);
                            }

                        } catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;
                    }
                case PROTOCOL.PLAYER_REGI:
                    {
                        char sp = ',';
                        string text = msg.pop_string();
                        string[] idstring = text.Split(sp);
                        string id = idstring[0];
                        string password = idstring[1];
                                                        CPacket response = CPacket.create((short)PROTOCOL.PLAYER_REGI);
                        Console.WriteLine(string.Format("유저가 회원가입을 시도했습니다. 아이디, 비밀번호 : {0}, {1}", id, password));
                        try
                        {
                            if (dbc.idregi(id, password) != null)
                            {

                                response.push(dbc.idregi(id, password));
                                send(response);
                            }
                            else if (dbc.idregi(id, password) == null)
                            {
                                response.push("RegistFailed");
                                send(response);
                            } else if(dbc.idregi(id, password).Equals("Already")) {
                                if (dbc.idregi(id, password).Equals("Already"))
                                response.push("AlreadyExist");
                                send(response);
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;
                        
                    }
			}
		}

        void IPeer.on_removed()
        {
            Console.WriteLine("클라이언트가 접속을 종료했습니다.");

            Program.remove_user(this);
        }

        public void send(CPacket msg)
        {
            this.token.send(msg);
        }

        void IPeer.disconnect()
        {
            this.token.socket.Disconnect(false);
        }

        void IPeer.process_user_operation(CPacket msg)
        {
        }
    }
}
