using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
	public enum PROTOCOL : short
	{
		BEGIN = 0,

		CHAT_MSG_REQ = 1,
		CHAT_MSG_ACK = 2,
        MYS = 3,
        MYC = 4,
        
        PLAYER_ID = 5,
        PLAYER_REGI = 6,

		END
	}
}
