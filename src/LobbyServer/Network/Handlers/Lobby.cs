using System;
using System.Text;
using Shared.Models;
using Shared.Network;
using Shared.Network.LobbyServer;
using Shared.Objects;
using Shared.Util;

namespace LobbyServer.Network.Handlers
{
	public class Lobby
	{
		[Packet(Packets.CmdUserInfo)]
		public static void UserInfo(Packet packet)
		{
			UserInfoPacket userInfoPacket = new UserInfoPacket(packet);
			
			Log.Debug("UserInfo request. (Username: {0}, Ticket: {1})", userInfoPacket.Username, userInfoPacket.Ticket);

			if (userInfoPacket.Ticket != packet.Sender.User.Ticket)
			{
				Log.Error("Rejecting packet from {0} (ticket {1}) for invalid user-ticket combination.", userInfoPacket.Username,
					userInfoPacket.Ticket);
				packet.Sender.Error("Invalid ticket-user combination.");
			}

			new GameSettingsAnswerPacket().Send(Packets.GameSettingsAck, packet.Sender);

			packet.Sender.User.Characters = CharacterModel.Retrieve(LobbyServer.Instance.Database.Connection,
				packet.Sender.User.UID);

		    UserInfoAnswerPacket userInfoAnswerPacket = new UserInfoAnswerPacket
		    {
		        CharacterCount = packet.Sender.User.Characters.Count,
		        Username = packet.Sender.User.Name,
		        Characters = packet.Sender.User.Characters.ToArray()
		    };

		    userInfoAnswerPacket.Send(Packets.UserInfoAck, packet.Sender);
		}

		[Packet(Packets.CmdCheckInLobby)]
		public static void CheckInLobby(Packet packet)
		{
			uint version = packet.Reader.ReadUInt32();
			uint ticket = packet.Reader.ReadUInt32();
			string username = packet.Reader.ReadUnicodeStatic(0x28);
			uint time = packet.Reader.ReadUInt32();
			string stringTicket = packet.Reader.ReadAsciiStatic(0x40);

			User user = AccountModel.GetSession(LobbyServer.Instance.Database.Connection, username, ticket);
			if (user == null)
			{
				Log.Error("Rejecting {0} (ticket {1}) for invalid user-ticket combination.", username, ticket);
				packet.Sender.Error("Invalid ticket-user combination.");
				return;
			}
			packet.Sender.User = user;

			var ack = new Packet(Packets.CheckInLobbyAck); // CheckInLobbyAck
			ack.Writer.Write(0); // Result
			ack.Writer.Write(0); // Permission
			packet.Sender.Send(ack);

			var timeAck = new Packet(Packets.LobbyTimeAck); // LobbyTimeAck
			timeAck.Writer.Write(Environment.TickCount);
			timeAck.Writer.Write(Environment.TickCount);
			packet.Sender.Send(timeAck);

			Log.Debug("CheckInLobby {0} {1} {2} {3} {4}", version, ticket, username, time,
				BitConverter.ToString(Encoding.UTF8.GetBytes(stringTicket)));
		}

		[Packet(Packets.CmdCheckCharName)]
		public static void CheckCharacterName(Packet packet)
		{
			string characterName = packet.Reader.ReadUnicode();

			var ack = new Packet(Packets.CheckCharNameAck);
			ack.Writer.WriteUnicodeStatic(characterName, 21);
			ack.Writer.Write(!CharacterModel.Exists(LobbyServer.Instance.Database.Connection, characterName)); // Availability. true = Available, false = Unavailable.
			packet.Sender.Send(ack);
		}
		
		[Packet(Packets.CmdCreateChar)]
		public static void CreateChar(Packet packet)
		{
			string characterName = packet.Reader.ReadUnicodeStatic(21);
			short avatar = packet.Reader.ReadInt16();
			uint carType = packet.Reader.ReadUInt32();
			uint color = packet.Reader.ReadUInt32();
			Console.WriteLine(characterName + " " + avatar + " " + carType + " " + color);
			
			CharacterModel.CreateCharacter(LobbyServer.Instance.Database.Connection, packet.Sender.User.UID, characterName, avatar, carType, color);
			
			var ack = new Packet(Packets.CmdCreateChar+1);
			ack.Writer.WriteUnicodeStatic(characterName, 21);
			packet.Sender.Send(ack);
		}
		

		/*
		000000: 41 00 64 00 6D 00 69 00 6E 00 00 00 00 01 00 00  A · d · m · i · n · · · · · · ·
		000016: 00 40 00 00 00 00 00 00 01 00 00 00 00 00 00 00  · @ · · · · · · · · · · · · · ·
		000032: 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00  · · · · · · · · · · · · · · · ·
		000048: 00 00 52 00 00 00 03 00 00 00  · · R · · · · · · ·

		000000: 41 00 64 00 6D 00 69 00 6E 00 69 00 73 00 74 00  A · d · m · i · n · i · s · t ·
		000016: 72 00 61 00 74 00 6F 00 72 00 00 00 00 00 00 00  r · a · t · o · r · · · · · · ·
		000032: 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00  · · · · · · · · · · · · · · · ·
		000048: 00 00 52 00 00 00 03 00 00 00  · · R · · · · · · ·
		*/
		[Packet(Packets.CmdDeleteChar)]
		public static void DeleteCharacter(Packet packet)
		{
			//string charname = packet.Reader.ReadUnicode();
			string charname = packet.Reader.ReadUnicodeStatic(21);

			ulong charId = packet.Reader.ReadUInt64(); // Char ID?
			packet.Reader.ReadInt32(); // 82?
			packet.Reader.ReadInt32(); // 3?

			if (CharacterModel.HasCharacter(LobbyServer.Instance.Database.Connection, charId, packet.Sender.User.UID))
			{
				CharacterModel.DeleteCharacter(LobbyServer.Instance.Database.Connection, charId, packet.Sender.User.UID);
				var ack = new Packet(Packets.DeleteCharAck);
				ack.Writer.WriteUnicodeStatic(charname, 21);
				packet.Sender.Send(ack);

				return;
			}

			packet.Sender.Error("This character doesn't belong to you!");
		}
	}
}
