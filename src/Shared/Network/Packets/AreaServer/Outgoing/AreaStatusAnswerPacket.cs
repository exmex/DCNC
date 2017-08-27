using System.IO;
using System.Runtime.Remoting.Messaging;
using Shared.Util;

namespace Shared.Network.AreaServer
{
    public class AreaStatusAnswerPacket : OutPacket
    {
	    /// <summary>
	    ///     The user count for 100 areas
	    /// </summary>
	    public uint[] UserCount;

	    public override Packet CreatePacket()
	    {
		    /*
			for ( i = 0; i < 100; ++i )
			{
			  pArea = BS_AreaGet(i);
			  if ( pArea )
				lpAckMsg->m_UserCnt[i] = pArea->m_member.m_nCount;
			  else
				lpAckMsg->m_UserCnt[i] = 0;
			}
			*/
		    var ack = new Packet(Packets.AreaStatusAck);
		    ack.Writer.Write(GetBytes());
		    return ack;
	    }

	    public override byte[] GetBytes()
	    {
		    using (var ms = new MemoryStream())
		    {
			    using (var bs = new BinaryWriterExt(ms))
			    {
				    foreach (var count in UserCount)
					    bs.Write(count);
			    }
			    return ms.GetBuffer();
		    }
	    }
    }
}