using System;
using System.Collections.Generic;
using System.Net;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Network
{
	// Token: 0x0200009F RID: 159
	public class LidgrenClient : Client
	{
		// Token: 0x170000CD RID: 205
		public override bool isConnected
		{
			// Token: 0x06000B30 RID: 2864 RVA: 0x000E36CE File Offset: 0x000E18CE
			get
			{
				return this.client != null && this.client.ConnectionStatus == NetConnectionStatus.Connected;
			}
		}

		// Token: 0x170000CE RID: 206
		public override float averageRoundtripTime
		{
			// Token: 0x06000B31 RID: 2865 RVA: 0x000E36E8 File Offset: 0x000E18E8
			get
			{
				return this.client.ServerConnection.AverageRoundtripTime;
			}
		}

		// Token: 0x170000CF RID: 207
		public override IPAddress serverAddress
		{
			// Token: 0x06000B32 RID: 2866 RVA: 0x000E36FA File Offset: 0x000E18FA
			get
			{
				return this.client.ServerConnection.RemoteEndPoint.Address;
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x000E3714 File Offset: 0x000E1914
		public override void initializeConnection(string address)
		{
			NetPeerConfiguration config = new NetPeerConfiguration("StardewValley");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			config.ConnectionTimeout = 120f;
			config.PingInterval = 5f;
			this.client = new NetClient(config);
			this.client.Start();
			int port = 24642;
			if (address.Contains(":"))
			{
				string[] expr_64 = address.Split(new char[]
				{
					':'
				});
				address = expr_64[0];
				port = Convert.ToInt32(expr_64[1]);
			}
			this.client.DiscoverKnownPeer(address, port);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000E37A0 File Offset: 0x000E19A0
		public override void receiveMessages()
		{
			NetIncomingMessage inc;
			while ((inc = this.client.ReadMessage()) != null)
			{
				NetIncomingMessageType messageType = inc.MessageType;
				if (messageType <= NetIncomingMessageType.DiscoveryResponse)
				{
					if (messageType != NetIncomingMessageType.Data)
					{
						if (messageType == NetIncomingMessageType.DiscoveryResponse)
						{
							Console.WriteLine("Found server at " + inc.SenderEndPoint);
							this.serverName = inc.ReadString();
							this.receiveHandshake(inc);
						}
					}
					else
					{
						LidgrenClient.parseDataMessageFromServer(inc);
					}
				}
				else if (messageType != NetIncomingMessageType.WarningMessage)
				{
					if (messageType != NetIncomingMessageType.ErrorMessage)
					{
						if (messageType != NetIncomingMessageType.ConnectionLatencyUpdated)
						{
						}
					}
					else
					{
						Game1.debugOutput = inc.ReadString();
					}
				}
				else
				{
					Game1.debugOutput = inc.ReadString();
				}
			}
			if (this.client.ServerConnection != null && DateTime.Now.Second % 2 == 0)
			{
				Game1.debugOutput = "Ping: " + this.client.ServerConnection.AverageRoundtripTime * 1000f + "ms";
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000E388C File Offset: 0x000E1A8C
		private void receiveHandshake(NetIncomingMessage msg)
		{
			while ((long)msg.LengthBits - msg.Position >= 8L)
			{
				byte b = msg.ReadByte();
				if (b == 2)
				{
					long id = msg.ReadInt64();
					Farmer f = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2((float)(Game1.tileSize * 10), (float)(Game1.tileSize * 15)), 2, msg.ReadString(), new List<Item>(), true);
					f.FarmerSprite.setOwner(f);
					Game1.serverHost = f;
					Game1.otherFarmers.Add(id, f);
					Game1.otherFarmers[id]._tmpLocationName = msg.ReadString();
					Game1.otherFarmers[id].uniqueMultiplayerID = id;
				}
			}
			this.client.Connect(msg.SenderEndPoint.Address.ToString(), msg.SenderEndPoint.Port);
			this.hasHandshaked = true;
			if (!Game1.otherFarmers.ContainsKey(this.client.UniqueIdentifier))
			{
				Game1.otherFarmers.Add(this.client.UniqueIdentifier, Game1.player);
				Game1.player.uniqueMultiplayerID = this.client.UniqueIdentifier;
				Game1.player._tmpLocationName = "BusStop";
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000E39D0 File Offset: 0x000E1BD0
		public override void sendMessage(byte which, object[] data)
		{
			NetOutgoingMessage sendMsg = this.client.CreateMessage();
			MultiplayerUtility.writeData(sendMsg, which, data);
			this.client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x000E3A04 File Offset: 0x000E1C04
		private static void parseDataMessageFromServer(NetIncomingMessage msg)
		{
			while ((long)msg.LengthBits - msg.Position >= 8L)
			{
				switch (msg.ReadByte())
				{
				case 0:
					Game1.otherFarmers[msg.ReadInt64()].setMoving(msg.ReadByte());
					break;
				case 1:
					Game1.otherFarmers[msg.ReadInt64()].updatePositionFromServer(msg.ReadVector2());
					break;
				case 2:
					MultiplayerUtility.receivePlayerIntroduction(msg.ReadInt64(), msg.ReadString());
					break;
				case 3:
				{
					long id = msg.ReadInt64();
					Game1.otherFarmers[id].FarmerSprite.CurrentToolIndex = msg.ReadInt32();
					if (msg.ReadByte() == 1)
					{
						((FarmerSprite)Game1.otherFarmers[id].sprite).animateBackwardsOnce(msg.ReadInt32(), msg.ReadFloat());
						msg.ReadByte();
					}
					else
					{
						((FarmerSprite)Game1.otherFarmers[id].sprite).animateOnce(msg.ReadInt32(), msg.ReadFloat(), (int)msg.ReadByte());
					}
					break;
				}
				case 4:
					MultiplayerUtility.performObjectAlteration(msg.ReadInt16(), msg.ReadInt16(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt32());
					break;
				case 5:
					MultiplayerUtility.warpCharacter(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt64());
					break;
				case 6:
					MultiplayerUtility.performSwitchHeldItem(msg.ReadInt64(), msg.ReadByte(), (int)msg.ReadInt16());
					break;
				case 7:
					MultiplayerUtility.performToolAction(msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt64());
					break;
				case 8:
					MultiplayerUtility.performDebrisPickup(msg.ReadInt32(), msg.ReadString(), msg.ReadInt64());
					break;
				case 9:
					MultiplayerUtility.performCheckAction(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadInt64());
					break;
				case 10:
					MultiplayerUtility.receiveChatMessage(msg.ReadString(), msg.ReadInt64());
					break;
				case 11:
					MultiplayerUtility.receiveNameChange(msg.ReadString(), msg.ReadInt64());
					break;
				case 12:
					MultiplayerUtility.receiveTenMinuteSync(msg.ReadInt16());
					break;
				case 13:
					MultiplayerUtility.receiveBuildingChange(msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadInt64(), msg.ReadInt64());
					break;
				case 14:
					MultiplayerUtility.performDebrisCreate(msg.ReadInt16(), msg.ReadInt32(), msg.ReadInt32(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), 0L);
					break;
				case 15:
					MultiplayerUtility.performNPCMove(msg.ReadInt32(), msg.ReadInt32(), msg.ReadInt64());
					break;
				case 16:
					MultiplayerUtility.performNPCBehavior(msg.ReadInt64(), msg.ReadByte());
					break;
				case 17:
					MultiplayerUtility.allFarmersReadyCheck();
					break;
				case 18:
					MultiplayerUtility.parseServerToClientsMessage(msg.ReadString());
					break;
				case 19:
					MultiplayerUtility.interpretMessageToEveryone(msg.ReadInt32(), msg.ReadString(), msg.ReadInt64());
					break;
				}
			}
		}

		// Token: 0x04000B4B RID: 2891
		private NetClient client;
	}
}
