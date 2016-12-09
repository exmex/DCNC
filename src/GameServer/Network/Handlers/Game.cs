using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;
using Shared.Network;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class Game
    {
        /*
        000000: 2F ED 00 00  / · · · 
        000000: 56 B3 00 00  V · · ·

        Wrong Packet Size. CMD(3918) CmdLen: : 6, AnalysisSize: 4
        */
        [Packet(Packets.CmdUnknownSync)]
        public static void UnknownSync(Packet packet) // TODO: Figure out what this packet does...
        {
            packet.Reader.ReadInt32(); // always the same in session
            // hide sync packets for now

            var ack = new Packet(Packets.CmdUnknownSync+1);
            ack.Writer.Write((short)0);
        }

        [Packet(Packets.CmdMyCityPosition)] // TODO: Actual position and not just dummies
        public static void MyCityPosition(Packet packet)
        {
            //Console.WriteLine(packet.Reader.ReadUInt32());
            // -> Gate ID!
            var ack = new Packet(Packets.MyPositionAck);

            //ack.Writer.Write(256);
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.City); // City ID
            ack.Writer.Write(1); // Channel ID
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionX); // x
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionY); // y
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionZ); // z
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.Rotation); // w
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.posState); // PosState
            /*int m_CityId;
            int m_ChannelId;
            XiVec4 m_Pos;
            int m_PositionState;*/

            packet.Sender.Send(ack);
        }


        /*
        000000: 00 00 00 00 00 00 00 00 00 00 00 00 4A 70 AB 42  · · · · · · · · · · · · J p · B
        000016: 00 00 00 00 01 00 00 00 01 00 00 00  · · · · · · · · · · · ·
        */
        [Packet(Packets.CmdSaveCarPos)]
        public static void SaveCarPos(Packet packet)
        {
            var channelId = packet.Reader.ReadInt32();
            var x = packet.Reader.ReadSingle();
            var y = packet.Reader.ReadSingle();
            var z = packet.Reader.ReadSingle();
            var w = packet.Reader.ReadSingle();
            var cityId = packet.Reader.ReadInt32();
            var posState = packet.Reader.ReadInt32();

            CharacterModel.UpdatePosition(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacterId, channelId, x, y, z, w, cityId, posState);
        }

        [Packet(Packets.CmdUpdateQuickSlot)]
        public static void UpdateQuickSlot(Packet packet)
        {
            // TODO: actually update the quickslots.
            var slot1 = packet.Reader.ReadUInt32();
            var slot2 = packet.Reader.ReadUInt32();
        }

        [Packet(Packets.CmdGetDateTime)]
        public static void GetDateTime(Packet packet)
        {
            var unixTimeNow = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            var now = DateTime.UtcNow.ToLocalTime();

            var globalTime = packet.Reader.ReadUInt32(); // GlobalTime
            var localTime = packet.Reader.ReadUInt32(); // LocalTime
            var action = packet.Reader.ReadUInt32(); // Action

            var ack = new Packet(Packets.GetDateTimeAck);
            ack.Writer.Write(action);
            ack.Writer.Write(globalTime);
            ack.Writer.Write(localTime);
            ack.Writer.Write((int)unixTimeNow.TotalSeconds);
            ack.Writer.Write(0); // ServerTickTime
            ack.Writer.Write(0); // ServerTick
            ack.Writer.Write((short)now.DayOfYear);
            ack.Writer.Write((short)now.Month);
            ack.Writer.Write((short)now.Day);
            ack.Writer.Write((short)now.DayOfWeek);
            ack.Writer.Write((byte)now.Hour);
            ack.Writer.Write((byte)now.Minute);
            ack.Writer.Write((byte)now.Second);

            packet.Sender.Send(ack);

            /*
              *(_DWORD *)(msg + 2) = lpMsg->Action;
              *(_DWORD *)(msg + 6) = lpMsg->GlobalTime;
              *(float *)(msg + 10) = lpMsg->LocalTime;
              *(_DWORD *)(msg + 14) = now;
              * *(_DWORD *)(msg + 18) = GetServerTickTime();
              *(_DWORD *)(msg + 22) = GetServerTick();
              *(_WORD *)(msg + 26) = ptm->tm_yday;
              *(_WORD *)(msg + 28) = ptm->tm_mon;
              *(_WORD *)(msg + 30) = ptm->tm_mday;
              *(_WORD *)(msg + 32) = ptm->tm_wday;
              *(_BYTE *)(msg + 34) = ptm->tm_hour;
              *(_BYTE *)(msg + 35) = ptm->tm_min;
              *(_BYTE *)(msg + 36) = ptm->tm_sec;
             */
        }

        

        /*
        000000: 01 00 00 00 01 00 00 00 00 00 00 00 00 00 80 3F  · · · · · · · · · · · · · · · ?
        000016: 00 00 00 00  · · · · 
        */
        [Packet(Packets.CmdFuelChargeReq)]
        public static void FuelChargeReq(Packet packet) // TODO: Send actual data
        {
            uint CarId = packet.Reader.ReadUInt32();
            long Pay = packet.Reader.ReadInt64();
            float fuel = packet.Reader.ReadSingle();

            /*
              unsigned int CarId;
              __unaligned __declspec(align(1)) __int64 Pay;
              float DeltaFuel;
              __int64 Gold;
              float Fuel;
              float UnitPrice;
              float SaleUnitPrice;
              float FuelCapacity;
              float FuelEfficiency;
              int SaleFlag;
            */

            var ack = new Packet(Packets.FuelChargeReqAck);
            ack.Writer.Write(CarId);
            ack.Writer.Write(Pay);
            ack.Writer.Write(fuel);
            ack.Writer.Write(packet.Sender.User.ActiveCharacter.MitoMoney);
            ack.Writer.Write(packet.Sender.User.ActiveCar.Mitron+fuel);
            ack.Writer.Write(20.0f); // Mito Price per liter
            ack.Writer.Write(15.0f); // Discounted Price per liter (Normal channel, major channel is free?)
            ack.Writer.Write(packet.Sender.User.ActiveCar.MitronCapacity);
            ack.Writer.Write(packet.Sender.User.ActiveCar.MitronEfficiency);

            ack.Writer.Write(0); // 1 = Item Discount 50%
            packet.Sender.Send(ack);
            /*
            SaleFlag = 0;
            if ( pGame->m_pCharInfo->m_bHalfMitronCharge )
            {
               fSaleRate = fSaleRate - 0.5;
                v7 = SaleFlag;
                LOBYTE(v7) = SaleFlag | 1;
                SaleFlag = v7;
            }
            */
        }

        [Packet(Packets.CmdChatMsg)]
        public static void ChatMessage(Packet packet)
        {
            string type = packet.Reader.ReadUnicodeStatic(10);
            bool green = packet.Reader.ReadUInt32() == 0xFF00FF00; // ignore this, use packet.Sender.Player.User.Status
            string message = packet.Reader.ReadUnicodePrefixed();

            string sender = packet.Sender.User.ActiveCharacter.Name;

            Log.Debug($"({type}) <{sender}> {message}");

            var ack = new Packet(Packets.ChatMsgAck);
            ack.Writer.WriteUnicodeStatic(type, 10);
            ack.Writer.WriteUnicodeStatic(sender, 18);
            ack.Writer.WriteUnicode(message);

            switch (type)
            {
                case "room":
                    GameServer.Instance.Server.Broadcast(ack); // TODO: broadcast only to users in same area
                    break;

                case "channel":
                    GameServer.Instance.Server.Broadcast(ack);
                    break;

                case "party":
                    GameServer.Instance.Server.Broadcast(ack); // TODO: broadcast only to users in same party
                    break;

                case "team":
                    GameServer.Instance.Server.Broadcast(ack); // TODO: broadcast only to users in same crew
                    break;

                default:
                    Log.Error("Undefined chat message type.");
                    break;
            }
        }
    }
}
