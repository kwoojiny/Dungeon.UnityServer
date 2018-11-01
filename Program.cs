using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeNet;


namespace CSampleServer
{
	class Program
	{
		static List<CGameUser> userlist;

		static void Main(string[] args)
		{
			CPacketBufferManager.initialize(2000);
			userlist = new List<CGameUser>();

			CNetworkService service = new CNetworkService();
			// 콜백 매소드 설정.
			service.session_created_callback += on_session_created;
			// 초기화.
			service.initialize();
            service.listen("0.0.0.0", 7979, 100); //  (0.0.0.0) 에서 변경하지말것 0.0.0.0으로 설정할경우 -> 모든 IP에서 들어오는 값을 true로 처리함
            // 만약 위의 IP를 바꾸면 바꾼 IP에 대한 response만 보냄. 이외의 값은 전부 false로 처리하여 반응하지 않음.
            


			Console.WriteLine("현재 서버가 기동중입니다.");
			while (true)
			{
				System.Threading.Thread.Sleep(1000);
			}

			Console.ReadKey();
		}

		/// <summary>
		/// 클라이언트가 접속 완료 하였을 때 호출됩니다.
		/// n개의 워커 스레드에서 호출될 수 있으므로 공유 자원 접근시 동기화 처리를 해줘야 합니다.
		/// </summary>
		/// <returns></returns>
		static void on_session_created(CUserToken token)
		{
			CGameUser user = new CGameUser(token);
			lock (userlist)
			{
				userlist.Add(user);
			}
		}

		public static void remove_user(CGameUser user)
		{
			lock (userlist)
			{
				userlist.Remove(user);
			}
		}

            public void DBConnecter()
            {

            }
        }
    }

