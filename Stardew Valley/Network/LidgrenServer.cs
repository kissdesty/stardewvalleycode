using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Network
{
	// Token: 0x020000A0 RID: 160
	public class LidgrenServer : Server
	{
		// Token: 0x170000D0 RID: 208
		public override int connectionsCount
		{
			// Token: 0x06000B39 RID: 2873 RVA: 0x000E3D51 File Offset: 0x000E1F51
			get
			{
				if (this.server == null)
				{
					return 0;
				}
				return this.server.ConnectionsCount;
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000E3D68 File Offset: 0x000E1F68
		public LidgrenServer(string name) : base(name)
		{
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000E3D74 File Offset: 0x000E1F74
		public override void initializeConnection()
		{
			NetPeerConfiguration config = new NetPeerConfiguration("StardewValley");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
			config.Port = 24642;
			config.ConnectionTimeout = 120f;
			config.PingInterval = 5f;
			config.MaximumConnections = 4;
			config.EnableUPnP = true;
			this.server = new NetServer(config);
			this.server.Start();
			this.server.UPnP.ForwardPort(24642, "Stardew Valley Server");
			this.mapServerThread = new Thread(new ThreadStart(AsynchronousSocketListener.StartListening));
			this.mapServerThread.Start();
			Game1.player.uniqueMultiplayerID = this.server.UniqueIdentifier;
			Game1.serverHost = Game1.player;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000E3E3D File Offset: 0x000E203D
		public override void stopServer()
		{
			this.server.Shutdown("Server shutting down...");
			AsynchronousSocketListener.allDone.Close();
			AsynchronousSocketListener.active = false;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000E3E60 File Offset: 0x000E2060
		public override void receiveMessages()
		{
			NetIncomingMessage inc;
			while ((inc = this.server.ReadMessage()) != null)
			{
				NetIncomingMessageType messageType = inc.MessageType;
				if (messageType <= NetIncomingMessageType.Data)
				{
					if (messageType != NetIncomingMessageType.ConnectionApproval)
					{
						if (messageType != NetIncomingMessageType.Data)
						{
							goto IL_62;
						}
						LidgrenServer.parseDataMessageFromClient(inc);
					}
					else
					{
						inc.SenderConnection.Approve();
						this.addNewFarmer(inc.SenderConnection.RemoteUniqueIdentifier);
					}
				}
				else if (messageType != NetIncomingMessageType.DiscoveryRequest)
				{
					if (messageType != NetIncomingMessageType.ErrorMessage)
					{
						goto IL_62;
					}
					Game1.debugOutput = inc.ToString();
				}
				else
				{
					this.handshakeWithPlayer(inc);
				}
				IL_6D:
				this.server.Recycle(inc);
				continue;
				IL_62:
				Game1.debugOutput = inc.ToString();
				goto IL_6D;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000E3EF8 File Offset: 0x000E20F8
		private void addNewFarmer(long id)
		{
			Farmer f = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2((float)(Game1.tileSize * 10), (float)(Game1.tileSize * 15)), 2, "Max", new List<Item>(), true);
			f.FarmerSprite.setOwner(f);
			f.currentLocation = Game1.getLocationFromName("BusStop");
			f.uniqueMultiplayerID = id;
			Game1.getLocationFromName(f.currentLocation.name).farmers.Add(f);
			Game1.otherFarmers.Add(id, f);
			MultiplayerUtility.broadcastPlayerIntroduction(id, "Max");
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x000E3F9C File Offset: 0x000E219C
		private void handshakeWithPlayer(NetIncomingMessage message)
		{
			NetOutgoingMessage response = this.server.CreateMessage();
			response.Write(this.serverName);
			response.Write(2);
			response.Write(this.server.UniqueIdentifier);
			response.Write(Game1.player.name);
			response.Write(Game1.player.currentLocation.name);
			foreach (KeyValuePair<long, Farmer> r in Game1.otherFarmers)
			{
				response.Write(2);
				response.Write(r.Key);
				response.Write(r.Value.name);
				response.Write(r.Value.currentLocation.name);
			}
			this.server.SendDiscoveryResponse(response, message.SenderEndPoint);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x000E408C File Offset: 0x000E228C
		private static void parseDataMessageFromClient(NetIncomingMessage msg)
		{
			switch (msg.ReadByte())
			{
			case 0:
				Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].setMoving(msg.ReadByte());
				return;
			case 1:
			case 2:
			case 12:
			case 15:
			case 16:
			case 18:
				break;
			case 3:
				((FarmerSprite)Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).CurrentToolIndex = msg.ReadInt32();
				if (msg.ReadByte() == 1)
				{
					((FarmerSprite)Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).animateBackwardsOnce(msg.ReadInt32(), msg.ReadFloat());
					msg.ReadByte();
					return;
				}
				((FarmerSprite)Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).animateOnce(msg.ReadInt32(), msg.ReadFloat(), (int)msg.ReadByte());
				return;
			case 4:
				MultiplayerUtility.serverTryToPerformObjectAlteration(msg.ReadInt16(), msg.ReadInt16(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt32(), Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier]);
				return;
			case 5:
				MultiplayerUtility.warpCharacter(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 6:
				MultiplayerUtility.performSwitchHeldItem(msg.SenderConnection.RemoteUniqueIdentifier, msg.ReadByte(), (int)msg.ReadInt16());
				return;
			case 7:
				MultiplayerUtility.performToolAction(msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt16(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 8:
				MultiplayerUtility.performDebrisPickup(msg.ReadInt32(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 9:
				MultiplayerUtility.performCheckAction(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 10:
				MultiplayerUtility.receiveChatMessage(msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 11:
				MultiplayerUtility.receiveNameChange(msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 13:
				MultiplayerUtility.receiveBuildingChange(msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier, 0L);
				return;
			case 14:
				MultiplayerUtility.performDebrisCreate(msg.ReadInt16(), msg.ReadInt32(), msg.ReadInt32(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.SenderConnection.RemoteUniqueIdentifier);
				return;
			case 17:
				Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].readyConfirmation = true;
				MultiplayerUtility.allFarmersReadyCheck();
				return;
			case 19:
				MultiplayerUtility.interpretMessageToEveryone(msg.ReadInt32(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000E4388 File Offset: 0x000E2588
		public override void sendMessages(GameTime time)
		{
			LidgrenServer.messageSendCounter -= time.ElapsedGameTime.Milliseconds;
			if (LidgrenServer.messageSendCounter < 0)
			{
				LidgrenServer.messageSendCounter = 50;
				foreach (NetConnection player in this.server.Connections)
				{
					if (Game1.otherFarmers.ContainsKey(player.RemoteUniqueIdentifier) && Game1.otherFarmers[player.RemoteUniqueIdentifier].multiplayerMessage.Count > 0)
					{
						NetOutgoingMessage msg = this.server.CreateMessage();
						for (int i = 0; i < Game1.otherFarmers[player.RemoteUniqueIdentifier].multiplayerMessage.Count; i++)
						{
							MultiplayerUtility.writeData(msg, (byte)Game1.otherFarmers[player.RemoteUniqueIdentifier].multiplayerMessage[i][0], Game1.otherFarmers[player.RemoteUniqueIdentifier].multiplayerMessage[i].Skip(1).ToArray<object>());
						}
						this.server.SendMessage(msg, player, NetDeliveryMethod.ReliableOrdered);
					}
				}
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					v.Value.multiplayerMessage.Clear();
				}
			}
		}

		// Token: 0x04000B4C RID: 2892
		private NetServer server;

		// Token: 0x04000B4D RID: 2893
		private Thread mapServerThread;

		// Token: 0x04000B4E RID: 2894
		private static int messageSendCounter = 50;
	}
}
