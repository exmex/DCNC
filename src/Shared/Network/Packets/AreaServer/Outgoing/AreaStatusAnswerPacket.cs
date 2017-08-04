namespace Shared.Network.AreaServer
{
    public class AreaStatusAnswerPacket
    {
	    /// <summary>
	    ///     The user count for 100 areas
	    /// </summary>
	    public uint[] UserCount = new uint[100];

        public void Send(Client client)
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
            for (var i = 0; i < 100; ++i)
                ack.Writer.Write(UserCount[i]);
            client.Send(ack);
        }
    }
}