using System;
using System.Collections.Generic;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x0200001F RID: 31
	public class MultiplayerUtility
	{
		// Token: 0x0600013D RID: 317 RVA: 0x0000D725 File Offset: 0x0000B925
		public static long getNewID()
		{
			long expr_05 = MultiplayerUtility.latestID;
			MultiplayerUtility.latestID = expr_05 + 1L;
			return expr_05;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000D738 File Offset: 0x0000B938
		public static void broadcastFarmerPosition(long f, Vector2 position, string currentLocation)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.name.Equals(currentLocation))
				{
					for (int i = 0; i < v.Value.multiplayerMessage.Count; i++)
					{
						if ((byte)v.Value.multiplayerMessage[i][0] == 1 && (long)v.Value.multiplayerMessage[i][1] == f)
						{
							v.Value.multiplayerMessage.RemoveAt(i);
							break;
						}
					}
					v.Value.multiplayerMessage.Add(new object[]
					{
						1,
						f,
						position
					});
				}
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000D840 File Offset: 0x0000BA40
		public static void broadcastFarmerAnimation(long f, int startingFrame, int numberOfFrames, float animationSpeed, bool backwards, string currentLocation, int currentToolIndex)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.name.Equals(currentLocation) && v.Value.uniqueMultiplayerID != f)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						3,
						f,
						currentToolIndex,
						backwards ? 1 : 0,
						startingFrame,
						animationSpeed,
						(byte)numberOfFrames
					});
				}
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000D91C File Offset: 0x0000BB1C
		public static void broadcastFarmerMovement(long f, byte command, string currentLocation)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.name.Equals(currentLocation))
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						0,
						f,
						command
					});
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		public static void broadcastObjectChange(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo, string location)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.name.Equals(location))
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						4,
						x,
						y,
						command,
						terrainFeatureIndex,
						extraInfo
					});
				}
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public static void broadcastFarmerWarp(short x, short y, string nameOfNewLocation, bool isStructure, long id)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.uniqueMultiplayerID != id)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						5,
						x,
						y,
						nameOfNewLocation,
						isStructure ? 1 : 0,
						id
					});
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000DB14 File Offset: 0x0000BD14
		public static void broadcastSwitchHeldItem(byte bigCraftable, short heldItem, long whichPlayer, string location)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.name.Equals(location) && whichPlayer != v.Value.uniqueMultiplayerID)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						6,
						whichPlayer,
						bigCraftable,
						heldItem
					});
				}
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000DBC4 File Offset: 0x0000BDC4
		public static void broadcastToolAction(Tool t, int tileX, int tileY, string location, byte facingDirection, short seed, long whichPlayer)
		{
			ToolDescription tDesc = ToolFactory.getIndexFromTool(t);
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.name.Equals(location) && whichPlayer != v.Value.uniqueMultiplayerID)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						7,
						tDesc.index,
						tDesc.upgradeLevel,
						(short)tileX,
						(short)tileY,
						location,
						facingDirection,
						seed,
						whichPlayer
					});
				}
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		public static NetOutgoingMessage writeData(NetOutgoingMessage sendMsg, byte which, object[] data)
		{
			sendMsg.Write(which);
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i].GetType() == typeof(Vector2))
				{
					sendMsg.Write((Vector2)data[i]);
				}
				else if (data[i] is byte)
				{
					sendMsg.Write((byte)data[i]);
				}
				else if (data[i] is int)
				{
					sendMsg.Write((int)data[i]);
				}
				else if (data[i] is short)
				{
					sendMsg.Write((short)data[i]);
				}
				else if (data[i] is float)
				{
					sendMsg.Write((float)data[i]);
				}
				else if (data[i] is long)
				{
					sendMsg.Write((long)data[i]);
				}
				else if (data[i] is string)
				{
					sendMsg.Write((string)data[i]);
				}
			}
			return sendMsg;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		public static void broadcastGameClock()
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				v.Value.multiplayerMessage.Add(new object[]
				{
					12,
					(short)Game1.timeOfDay
				});
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000DE2C File Offset: 0x0000C02C
		public static void receiveTenMinuteSync(short time)
		{
			Game1.timeOfDay = (int)time;
			Game1.performTenMinuteClockUpdate();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000DE3C File Offset: 0x0000C03C
		public static void sendAnimationMessageToServer(int startingFrame, int numberOfFrames, float animationSpeed, bool backwards, int currentToolIndex)
		{
			Game1.client.sendMessage(3, new object[]
			{
				currentToolIndex,
				backwards ? 1 : 0,
				startingFrame,
				animationSpeed,
				(byte)numberOfFrames
			});
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000DE8F File Offset: 0x0000C08F
		private static int translateObjectIndex(int index)
		{
			switch (index)
			{
			case -9:
				return 325;
			case -7:
				return 324;
			case -6:
				return 323;
			case -5:
				return 322;
			}
			return index;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000DECC File Offset: 0x0000C0CC
		public static void performObjectAlteration(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo)
		{
			switch (command)
			{
			case 0:
			{
				extraInfo = MultiplayerUtility.translateObjectIndex(extraInfo);
				Object o;
				if (terrainFeatureIndex == 0)
				{
					o = new Object(Vector2.Zero, extraInfo, null, true, false, false, false);
				}
				else
				{
					o = new Object(Vector2.Zero, extraInfo, false);
				}
				o.placementAction(Game1.currentLocation, (int)x * Game1.tileSize, (int)y * Game1.tileSize, null);
				return;
			}
			case 1:
			{
				Object o;
				Game1.currentLocation.objects.TryGetValue(new Vector2((float)x, (float)y), out o);
				if (o != null)
				{
					Game1.currentLocation.objects.Remove(new Vector2((float)x, (float)y));
					o.performRemoveAction(new Vector2((float)x, (float)y), Game1.currentLocation);
					return;
				}
				break;
			}
			case 2:
				if (Game1.currentLocation.terrainFeatures.ContainsKey(new Vector2((float)x, (float)y)))
				{
					Game1.currentLocation.terrainFeatures[new Vector2((float)x, (float)y)] = TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo);
					return;
				}
				Game1.currentLocation.terrainFeatures.Add(new Vector2((float)x, (float)y), TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo));
				return;
			case 3:
				Game1.currentLocation.terrainFeatures.Remove(new Vector2((float)x, (float)y));
				break;
			default:
				return;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000E000 File Offset: 0x0000C200
		public static void serverTryToPerformObjectAlteration(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo, Farmer actionPerformer)
		{
			switch (command)
			{
			case 0:
			{
				extraInfo = MultiplayerUtility.translateObjectIndex(extraInfo);
				Object o;
				if (terrainFeatureIndex == 0)
				{
					o = new Object(Vector2.Zero, extraInfo, null, true, false, false, false);
				}
				else
				{
					o = new Object(Vector2.Zero, extraInfo, false);
				}
				if (Utility.playerCanPlaceItemHere(actionPerformer.currentLocation, o, (int)x, (int)y, actionPerformer))
				{
					o.placementAction(Game1.currentLocation, (int)x, (int)y, null);
					return;
				}
				break;
			}
			case 1:
			{
				Object o = Game1.currentLocation.objects[new Vector2((float)x, (float)y)];
				Game1.currentLocation.objects.Remove(new Vector2((float)x, (float)y));
				if (o != null)
				{
					o.performRemoveAction(new Vector2((float)x, (float)y), Game1.currentLocation);
					return;
				}
				break;
			}
			case 2:
				Game1.currentLocation.terrainFeatures.Add(new Vector2((float)x, (float)y), TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo));
				return;
			case 3:
				Game1.currentLocation.terrainFeatures.Remove(new Vector2((float)x, (float)y));
				break;
			default:
				return;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000E0FC File Offset: 0x0000C2FC
		public static void performSwitchHeldItem(long id, byte bigCraftable, int index)
		{
			if (index == -1)
			{
				Game1.otherFarmers[id].showNotCarrying();
				if (Game1.otherFarmers[id].ActiveObject != null)
				{
					Game1.otherFarmers[id].ActiveObject.actionWhenStopBeingHeld(Game1.otherFarmers[id]);
				}
				Game1.otherFarmers[id].items[Game1.otherFarmers[id].CurrentToolIndex] = null;
			}
			else
			{
				Game1.otherFarmers[id].showCarrying();
				Game1.otherFarmers[id].ActiveObject = ((bigCraftable == 1) ? new Object(Vector2.Zero, index, false) : new Object(Vector2.Zero, index, 1));
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendSwitchHeldItemMessage(index, bigCraftable, id);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		public static void sendSwitchHeldItemMessage(int heldItemIndex, byte bigCraftable, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(6, new object[]
				{
					bigCraftable,
					(short)heldItemIndex
				});
				return;
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastSwitchHeldItem(bigCraftable, (short)heldItemIndex, whichPlayer, Game1.currentLocation.name);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000E218 File Offset: 0x0000C418
		public static void sendMessageToEveryone(int messageCategory, string message, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(19, new object[]
				{
					messageCategory,
					message
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						19,
						messageCategory,
						message,
						whichPlayer
					});
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000E2C8 File Offset: 0x0000C4C8
		public static void warpCharacter(short x, short y, string name, byte isStructure, long id)
		{
			if (Game1.otherFarmers.ContainsKey(id))
			{
				if (Game1.otherFarmers[id].currentLocation == null)
				{
					Game1.otherFarmers[id]._tmpLocationName = name;
					return;
				}
				Game1.otherFarmers[id].currentLocation.farmers.Remove(Game1.otherFarmers[id]);
				Game1.otherFarmers[id].currentLocation = Game1.getLocationFromName(name, isStructure == 1);
				Game1.otherFarmers[id].position.X = (float)((int)x * Game1.tileSize);
				Game1.otherFarmers[id].position.Y = (float)((int)y * Game1.tileSize - Game1.tileSize / 2);
				GameLocation location = Game1.getLocationFromName(name, isStructure == 1);
				location.farmers.Add(Game1.otherFarmers[id]);
				if (location.farmers.Count.Equals(Game1.numberOfPlayers() - 1))
				{
					location.checkForEvents();
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastFarmerWarp(x, y, name, isStructure == 1, id);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		public static void performToolAction(byte toolIndex, byte toolUpgradeLevel, short xTile, short yTile, string locationName, byte facingDirection, short seed, long who)
		{
			Tool t = ToolFactory.getToolFromDescription(toolIndex, (int)toolUpgradeLevel);
			GameLocation i = Game1.getLocationFromName(locationName);
			Game1.otherFarmers[who].CurrentTool = t;
			Game1.recentMultiplayerRandom = new Random((int)seed);
			if (i == null)
			{
				if (t is MeleeWeapon)
				{
					Game1.otherFarmers[who].faceDirection((int)facingDirection);
					(t as MeleeWeapon).DoDamage(Game1.currentLocation, (int)xTile, (int)yTile, Game1.otherFarmers[who].facingDirection, 1, Game1.otherFarmers[who]);
				}
				else
				{
					t.DoFunction(Game1.currentLocation, (int)xTile, (int)yTile, 1, Game1.otherFarmers[who]);
				}
			}
			else if (t is MeleeWeapon)
			{
				Game1.otherFarmers[who].faceDirection((int)facingDirection);
				(t as MeleeWeapon).DoDamage(i, (int)xTile, (int)yTile, Game1.otherFarmers[who].facingDirection, 1, Game1.otherFarmers[who]);
			}
			else
			{
				t.DoFunction(i, (int)xTile, (int)yTile, 1, Game1.otherFarmers[who]);
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastToolAction(t, (int)xTile, (int)yTile, locationName, facingDirection, seed, who);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000E50C File Offset: 0x0000C70C
		public static void broadcastBuildingChange(byte whatChange, Vector2 tileLocation, string name, string locationName, long who)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(13, new object[]
				{
					whatChange,
					(short)tileLocation.X,
					(short)tileLocation.Y,
					name
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					if (v.Value.currentLocation.name.Equals(locationName) && (whatChange != 2 || v.Value.uniqueMultiplayerID != who))
					{
						v.Value.multiplayerMessage.Add(new object[]
						{
							13,
							whatChange,
							(short)tileLocation.X,
							(short)tileLocation.Y,
							name,
							who,
							MultiplayerUtility.recentMultiplayerEntityID
						});
					}
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000E63C File Offset: 0x0000C83C
		public static void receiveBuildingChange(byte whatChange, short tileX, short tileY, string name, long who, long id)
		{
			if (Game1.IsClient)
			{
				MultiplayerUtility.recentMultiplayerEntityID = id;
			}
			else
			{
				MultiplayerUtility.recentMultiplayerEntityID = MultiplayerUtility.getNewID();
			}
			if (Game1.currentLocation is Farm || Game1.IsServer)
			{
				Farm farm = (Farm)Game1.currentLocation;
				if (!(Game1.currentLocation is Farm))
				{
					farm = (Farm)Game1.otherFarmers[id].currentLocation;
				}
				Farmer f = Game1.getFarmer(who);
				switch (whatChange)
				{
				case 0:
				{
					BluePrint b = new BluePrint(name);
					if (b.blueprintType.Equals("Animals") && farm.placeAnimal(b, new Vector2((float)tileX, (float)tileY), true, who) && f.IsMainPlayer)
					{
						b.consumeResources();
					}
					else if (!b.blueprintType.Equals("Animals") && farm.buildStructure(b, new Vector2((float)tileX, (float)tileY), true, f, false) && f.IsMainPlayer)
					{
						b.consumeResources();
					}
					else if (f.IsMainPlayer)
					{
						Game1.addHUDMessage(new HUDMessage("Can't Build There", Color.Red, 3500f));
						return;
					}
					if (!b.blueprintType.Equals("Animals"))
					{
						Game1.playSound("axe");
						return;
					}
					break;
				}
				case 1:
				{
					Building destroyed = farm.getBuildingAt(new Vector2((float)tileX, (float)tileY));
					if (farm.destroyStructure(new Vector2((float)tileX, (float)tileY)))
					{
						int groundYTile = destroyed.tileY + destroyed.tilesHigh;
						for (int i = 0; i < destroyed.texture.Bounds.Height / Game1.tileSize; i++)
						{
							Game1.createRadialDebris(farm, destroyed.texture, new Microsoft.Xna.Framework.Rectangle(destroyed.texture.Bounds.Center.X, destroyed.texture.Bounds.Center.Y, Game1.tileSize / 16, Game1.tileSize / 16), destroyed.tileX + Game1.random.Next(destroyed.tilesWide), destroyed.tileY + destroyed.tilesHigh - i, Game1.random.Next(20, 45), groundYTile);
						}
						Game1.playSound("explosion");
						Utility.spreadAnimalsAround(destroyed, farm);
						return;
					}
					break;
				}
				case 2:
				{
					BluePrint b = new BluePrint(name);
					Building toUpgrade = farm.getBuildingAt(new Vector2((float)tileX, (float)tileY));
					farm.tryToUpgrade(toUpgrade, b);
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000E894 File Offset: 0x0000CA94
		public static void broadcastDebrisPickup(int uniqueID, string locationName, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(8, new object[]
				{
					uniqueID,
					locationName
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					if (v.Value.currentLocation.name.Equals(locationName) && whichPlayer != v.Value.uniqueMultiplayerID)
					{
						v.Value.multiplayerMessage.Add(new object[]
						{
							8,
							uniqueID,
							locationName,
							whichPlayer
						});
					}
				}
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000E970 File Offset: 0x0000CB70
		public static void receivePlayerIntroduction(long id, string name)
		{
			Farmer f = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\farmer")), new Vector2((float)(Game1.tileSize * 5), (float)(Game1.tileSize * 5)), 2, name, new List<Item>(), true);
			f.FarmerSprite.setOwner(f);
			f.currentLocation = Game1.getLocationFromName("FarmHouse");
			f.uniqueMultiplayerID = id;
			Game1.otherFarmers.Add(id, f);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000E9E8 File Offset: 0x0000CBE8
		public static void broadcastPlayerIntroduction(long id, string name)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (id != v.Value.uniqueMultiplayerID)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						2,
						id,
						name
					});
				}
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000EA70 File Offset: 0x0000CC70
		public static void performCheckAction(short x, short y, string location, long who)
		{
			if (!Utility.canGrabSomethingFromHere((int)x * Game1.tileSize, (int)y * Game1.tileSize, Game1.otherFarmers[who]) || !Game1.getLocationFromName(location).objects.ContainsKey(new Vector2((float)x, (float)y)) || !Game1.getLocationFromName(location).objects[new Vector2((float)x, (float)y)].checkForAction(Game1.otherFarmers[who], false))
			{
				if (Game1.isFestival())
				{
					Game1.currentLocation.checkAction(new Location((int)x, (int)y), Game1.viewport, Game1.otherFarmers[who]);
				}
				else
				{
					Game1.getLocationFromName(location).checkAction(new Location((int)x, (int)y), Game1.viewport, Game1.otherFarmers[who]);
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastCheckAction((int)x, (int)y, who, location);
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000EB44 File Offset: 0x0000CD44
		public static void broadcastCheckAction(int x, int y, long who, string location)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(9, new object[]
				{
					(short)x,
					(short)y,
					location
				});
				return;
			}
			if (Game1.IsServer)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"Server Received Check Action message @ X:",
					x,
					" Y:",
					y
				}));
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					if (v.Value.currentLocation.name.Equals(location) && who != v.Value.uniqueMultiplayerID)
					{
						v.Value.multiplayerMessage.Add(new object[]
						{
							9,
							(short)x,
							(short)y,
							location,
							who
						});
					}
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000EC68 File Offset: 0x0000CE68
		public static void sendReadyConfirmation(long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(17, new object[]
				{
					0
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						17,
						whichPlayer
					});
				}
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000ED08 File Offset: 0x0000CF08
		public static void sendServerToClientsMessage(string message)
		{
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						18,
						message
					});
				}
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000ED80 File Offset: 0x0000CF80
		public static void sendChatMessage(string message, long whichPlayer)
		{
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(10, new object[]
				{
					message
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						10,
						message,
						whichPlayer
					});
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000EE20 File Offset: 0x0000D020
		public static void sendNameChange(string name, long who)
		{
			if (who == Game1.player.uniqueMultiplayerID)
			{
				Game1.player.name = name;
			}
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(11, new object[]
				{
					name
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					if (who != v.Value.uniqueMultiplayerID)
					{
						v.Value.multiplayerMessage.Add(new object[]
						{
							11,
							name,
							who
						});
					}
				}
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		public static void receiveNameChange(string message, long who)
		{
			Game1.ChatBox.receiveChatMessage(string.Concat(new string[]
			{
				Game1.otherFarmers[who].name,
				" changed ",
				Game1.otherFarmers[who].getHisOrHer(),
				" name to '",
				message,
				"'"
			}), -1L);
			Game1.otherFarmers[who].name = message;
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendNameChange(message, who);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000EF70 File Offset: 0x0000D170
		public static void receiveChatMessage(string message, long whichPlayer)
		{
			foreach (IClickableMenu i in Game1.onScreenMenus)
			{
				if (i is ChatBox)
				{
					((ChatBox)i).receiveChatMessage(message, whichPlayer);
					break;
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendChatMessage(message, whichPlayer);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000EFE0 File Offset: 0x0000D1E0
		public static void allFarmersReadyCheck()
		{
			if (Game1.IsServer)
			{
				using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.readyConfirmation)
						{
							return;
						}
					}
				}
				if (Game1.player.readyConfirmation)
				{
					MultiplayerUtility.sendReadyConfirmation(Game1.player.uniqueMultiplayerID);
					using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.readyConfirmation = false;
						}
					}
					Game1.player.readyConfirmation = false;
					if (Game1.currentLocation.currentEvent != null)
					{
						Event expr_BF = Game1.currentLocation.currentEvent;
						int currentCommand = expr_BF.CurrentCommand;
						expr_BF.CurrentCommand = currentCommand + 1;
						return;
					}
				}
			}
			else
			{
				if (Game1.isFestival())
				{
					Game1.currentLocation.currentEvent.allPlayersReady = true;
				}
				using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.readyConfirmation = false;
					}
				}
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000F134 File Offset: 0x0000D334
		public static void parseServerToClientsMessage(string message)
		{
			if (Game1.IsClient)
			{
				if (!(message == "festivalEvent"))
				{
					if (!(message == "endFest"))
					{
						return;
					}
					if (Game1.CurrentEvent != null)
					{
						Game1.CurrentEvent.forceEndFestival(Game1.player);
					}
				}
				else if (Game1.currentLocation.currentEvent != null)
				{
					Game1.currentLocation.currentEvent.forceFestivalContinue();
					return;
				}
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000F198 File Offset: 0x0000D398
		public static void interpretMessageToEveryone(int messageCategory, string message, long who)
		{
			switch (messageCategory)
			{
			case 0:
				if (Game1.isFestival())
				{
					Game1.otherFarmers[who].dancePartner = Game1.currentLocation.currentEvent.getActorByName(message);
				}
				Game1.currentLocation.currentEvent.getActorByName(message).hasPartnerForDance = true;
				break;
			case 1:
				if (Game1.isFestival())
				{
					Game1.currentLocation.currentEvent.addItemToLuauSoup(new Object(Convert.ToInt32(message.Split(new char[]
					{
						' '
					})[0]), 1, false, -1, Convert.ToInt32(message.Split(new char[]
					{
						' '
					})[1])), Game1.otherFarmers[who]);
				}
				break;
			case 2:
				if (Game1.isFestival())
				{
					Game1.CurrentEvent.setGrangeDisplayUser(message.Equals("null") ? null : Game1.getFarmer(who));
				}
				break;
			case 3:
				if (Game1.isFestival())
				{
					string[] parse = message.Split(new char[]
					{
						' '
					});
					int position = Convert.ToInt32(parse[0]);
					if (parse[1].Equals("null"))
					{
						Game1.CurrentEvent.addItemToGrangeDisplay(null, position, true);
					}
					else
					{
						Game1.CurrentEvent.addItemToGrangeDisplay(new Object(Convert.ToInt32(parse[1]), Convert.ToInt32(parse[2]), false, -1, 0), position, true);
					}
				}
				break;
			case 4:
				Game1.CurrentEvent.grangeScore = Convert.ToInt32(message);
				Game1.ChatBox.receiveChatMessage("Your grange display has been judged. Return to Mayor Lewis for the result!", -1L);
				Game1.CurrentEvent.interpretGrangeResults();
				break;
			case 5:
				if (!Game1.player.mailReceived.Contains(message))
				{
					Game1.player.mailReceived.Add(message);
				}
				break;
			case 6:
				(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).completeBundle(Convert.ToInt32(message));
				break;
			case 7:
				Game1.addMailForTomorrow(message, false, false);
				break;
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendMessageToEveryone(messageCategory, message, who);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000F394 File Offset: 0x0000D594
		public static void broadcastDebrisCreate(short seed, Vector2 position, int facingDirection, Item i, long who)
		{
			ItemDescription description = ObjectFactory.getDescriptionFromItem(i);
			if (Game1.IsClient)
			{
				Game1.client.sendMessage(14, new object[]
				{
					seed,
					(int)position.X,
					(int)position.Y,
					(byte)facingDirection,
					description.type,
					(short)description.index,
					(short)description.stack
				});
				return;
			}
			if (Game1.IsServer)
			{
				foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
				{
					if (who != v.Value.uniqueMultiplayerID && (who == Game1.player.uniqueMultiplayerID || Game1.otherFarmers[who].currentLocation.Equals(v.Value.currentLocation)))
					{
						v.Value.multiplayerMessage.Add(new object[]
						{
							14,
							seed,
							(int)position.X,
							(int)position.Y,
							(byte)facingDirection,
							description.type,
							(short)description.index,
							(short)description.stack
						});
					}
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000F530 File Offset: 0x0000D730
		public static void performDebrisCreate(short seed, int xPosition, int yPosition, byte facingDirection, byte type, short index, short stack, long who)
		{
			Game1.recentMultiplayerRandom = new Random((int)seed);
			Vector2 targetLocation = new Vector2((float)xPosition, (float)yPosition);
			Vector2 origin = new Vector2((float)xPosition, (float)yPosition);
			Item item = ObjectFactory.getItemFromDescription(type, (int)index, (int)stack);
			switch (facingDirection)
			{
			case 0:
				origin.X -= (float)(Game1.tileSize / 2);
				origin.Y -= (float)(Game1.tileSize * 2 + Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2));
				targetLocation.Y -= (float)(Game1.tileSize * 3);
				break;
			case 1:
				origin.X += (float)(Game1.tileSize * 2 / 3);
				origin.Y -= (float)(Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
				targetLocation.X += (float)(Game1.tileSize * 4);
				break;
			case 2:
				origin.X -= (float)(Game1.tileSize / 2);
				origin.Y += (float)Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2);
				targetLocation.Y += (float)(Game1.tileSize * 3 / 2);
				break;
			case 3:
				origin.X -= (float)Game1.tileSize;
				origin.Y -= (float)(Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
				targetLocation.X -= (float)(Game1.tileSize * 4);
				break;
			}
			if (Game1.IsClient)
			{
				Game1.currentLocation.debris.Add(new Debris(item, origin, targetLocation));
				return;
			}
			if (Game1.IsServer)
			{
				Game1.otherFarmers[who].currentLocation.debris.Add(new Debris(item, origin, targetLocation));
				MultiplayerUtility.broadcastDebrisCreate(seed, targetLocation, (int)facingDirection, item, who);
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000F710 File Offset: 0x0000D910
		public static void performDebrisPickup(int uniqueID, string locationName, long whichPlayer)
		{
			GameLocation location = Game1.getLocationFromName(locationName);
			for (int i = 0; i < location.debris.Count; i++)
			{
				if (location.debris[i].uniqueID == uniqueID)
				{
					location.debris.RemoveAt(i);
					break;
				}
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastDebrisPickup(uniqueID, locationName, whichPlayer);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000F76C File Offset: 0x0000D96C
		public static void broadcastNPCMove(int x, int y, long id, GameLocation location)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.Equals(location))
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						15,
						x,
						y,
						id
					});
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000F808 File Offset: 0x0000DA08
		public static void broadcastNPCBehavior(long npcID, GameLocation location, byte behavior)
		{
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Value.currentLocation.Equals(location))
				{
					v.Value.multiplayerMessage.Add(new object[]
					{
						16,
						npcID,
						behavior
					});
				}
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000F89C File Offset: 0x0000DA9C
		public static void performNPCBehavior(long npcID, byte behavior)
		{
			Character c = MultiplayerUtility.getCharacterFromID(npcID);
			if (c != null && !c.ignoreMultiplayerUpdates)
			{
				c.performBehavior(behavior);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
		public static void performNPCMove(int x, int y, long id)
		{
			Character c = MultiplayerUtility.getCharacterFromID(id);
			if (c != null && !c.ignoreMultiplayerUpdates)
			{
				c.updatePositionFromServer(new Vector2((float)x, (float)y));
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000F8F4 File Offset: 0x0000DAF4
		public static Character getCharacterFromID(long id)
		{
			if (Game1.currentLocation is Farm)
			{
				if ((Game1.currentLocation as Farm).animals.ContainsKey(id))
				{
					return (Game1.currentLocation as Farm).animals[id];
				}
				foreach (Building b in (Game1.currentLocation as Farm).buildings)
				{
					if (b.indoors is AnimalHouse && (b.indoors as AnimalHouse).animals.ContainsKey(id))
					{
						return (b.indoors as AnimalHouse).animals[id];
					}
				}
			}
			return null;
		}

		// Token: 0x04000156 RID: 342
		public const byte movement = 0;

		// Token: 0x04000157 RID: 343
		public const byte position = 1;

		// Token: 0x04000158 RID: 344
		public const byte playerIntroduction = 2;

		// Token: 0x04000159 RID: 345
		public const byte animation = 3;

		// Token: 0x0400015A RID: 346
		public const byte objectAlteration = 4;

		// Token: 0x0400015B RID: 347
		public const byte warpFarmer = 5;

		// Token: 0x0400015C RID: 348
		public const byte switchHeldItem = 6;

		// Token: 0x0400015D RID: 349
		public const byte toolAction = 7;

		// Token: 0x0400015E RID: 350
		public const byte debrisPickup = 8;

		// Token: 0x0400015F RID: 351
		public const byte checkAction = 9;

		// Token: 0x04000160 RID: 352
		public const byte chatMessage = 10;

		// Token: 0x04000161 RID: 353
		public const byte nameChange = 11;

		// Token: 0x04000162 RID: 354
		public const byte tenMinSync = 12;

		// Token: 0x04000163 RID: 355
		public const byte building = 13;

		// Token: 0x04000164 RID: 356
		public const byte debrisCreate = 14;

		// Token: 0x04000165 RID: 357
		public const byte npcMove = 15;

		// Token: 0x04000166 RID: 358
		public const byte npcBehavior = 16;

		// Token: 0x04000167 RID: 359
		public const byte readyConfirmation = 17;

		// Token: 0x04000168 RID: 360
		public const byte serverToClientsMessage = 18;

		// Token: 0x04000169 RID: 361
		public const byte messageToEveryone = 19;

		// Token: 0x0400016A RID: 362
		public const byte addObject = 0;

		// Token: 0x0400016B RID: 363
		public const byte removeObject = 1;

		// Token: 0x0400016C RID: 364
		public const byte addTerrainFeature = 2;

		// Token: 0x0400016D RID: 365
		public const byte removeTerrainFeature = 3;

		// Token: 0x0400016E RID: 366
		public const byte addBuilding = 0;

		// Token: 0x0400016F RID: 367
		public const byte removeBuilding = 1;

		// Token: 0x04000170 RID: 368
		public const byte upgradeBuilding = 2;

		// Token: 0x04000171 RID: 369
		public static long recentMultiplayerEntityID;

		// Token: 0x04000172 RID: 370
		public static long latestID = -9223372036854775808L + (long)Game1.random.Next(1000);

		// Token: 0x04000173 RID: 371
		public const string MSG_START_FESTIVAL_EVENT = "festivalEvent";

		// Token: 0x04000174 RID: 372
		public const string MSG_END_FESTIVAL = "endFest";

		// Token: 0x04000175 RID: 373
		public const string MSG_PLACEHOLDER = "[replace me]";

		// Token: 0x04000176 RID: 374
		public const int DANCE_PARTNER = 0;

		// Token: 0x04000177 RID: 375
		public const int LUAU_ITEM = 1;

		// Token: 0x04000178 RID: 376
		public const int GRANGE_DISPLAY_USER = 2;

		// Token: 0x04000179 RID: 377
		public const int GRANGE_DISPLAY_CHANGE = 3;

		// Token: 0x0400017A RID: 378
		public const int MSGE_GRANGE_SCORE = 4;

		// Token: 0x0400017B RID: 379
		public const int MSGE_ADD_MAIL_RECEIVED = 5;

		// Token: 0x0400017C RID: 380
		public const int MSGE_BUNDLE_COMPLETE = 6;

		// Token: 0x0400017D RID: 381
		public const int MSGE_ADD_MAIL_FOR_TOMORROW = 7;
	}
}
