using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Menus;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
	// Token: 0x02000127 RID: 295
	public class CommunityCenter : GameLocation
	{
		// Token: 0x060010B2 RID: 4274 RVA: 0x0015644E File Offset: 0x0015464E
		public CommunityCenter()
		{
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00156464 File Offset: 0x00154664
		public CommunityCenter(string name) : base(Game1.content.Load<Map>("Maps\\CommunityCenter_Ruins"), name)
		{
			Dictionary<string, string> bundlesInfo = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
			this.bundles = new SerializableDictionary<int, bool[]>();
			this.bundleRewards = new SerializableDictionary<int, bool>();
			this.areaToBundleDictionary = new Dictionary<int, List<int>>();
			this.bundleToAreaDictionary = new Dictionary<int, int>();
			for (int i = 0; i < 6; i++)
			{
				this.areaToBundleDictionary.Add(i, new List<int>());
			}
			foreach (KeyValuePair<string, string> v in bundlesInfo)
			{
				this.bundles.Add(Convert.ToInt32(v.Key.Split(new char[]
				{
					'/'
				})[1]), new bool[v.Value.Split(new char[]
				{
					'/'
				})[2].Split(new char[]
				{
					' '
				}).Length]);
				this.bundleRewards.Add(Convert.ToInt32(v.Key.Split(new char[]
				{
					'/'
				})[1]), false);
				this.areaToBundleDictionary[this.getAreaNumberFromName(v.Key.Split(new char[]
				{
					'/'
				})[0])].Add(Convert.ToInt32(v.Key.Split(new char[]
				{
					'/'
				})[1]));
				this.bundleToAreaDictionary.Add(Convert.ToInt32(v.Key.Split(new char[]
				{
					'/'
				})[1]), this.getAreaNumberFromName(v.Key.Split(new char[]
				{
					'/'
				})[0]));
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00156650 File Offset: 0x00154850
		private int getAreaNumberFromName(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2486580683u)
			{
				if (num <= 696049845u)
				{
					if (num != 244995591u)
					{
						if (num != 696049845u)
						{
							return -1;
						}
						if (!(name == "Pantry"))
						{
							return -1;
						}
						return 0;
					}
					else
					{
						if (!(name == "FishTank"))
						{
							return -1;
						}
						return 2;
					}
				}
				else if (num != 1618314778u)
				{
					if (num != 1881810045u)
					{
						if (num != 2486580683u)
						{
							return -1;
						}
						if (!(name == "BoilerRoom"))
						{
							return -1;
						}
						return 3;
					}
					else if (!(name == "CraftsRoom"))
					{
						return -1;
					}
				}
				else
				{
					if (!(name == "Bulletin"))
					{
						return -1;
					}
					return 5;
				}
			}
			else if (num <= 3168044721u)
			{
				if (num != 2576994461u)
				{
					if (num != 3063871366u)
					{
						if (num != 3168044721u)
						{
							return -1;
						}
						if (!(name == "Crafts Room"))
						{
							return -1;
						}
					}
					else
					{
						if (!(name == "Bulletin Board"))
						{
							return -1;
						}
						return 5;
					}
				}
				else
				{
					if (!(name == "Fish Tank"))
					{
						return -1;
					}
					return 2;
				}
			}
			else if (num != 3170708731u)
			{
				if (num != 3560083791u)
				{
					if (num != 4104466714u)
					{
						return -1;
					}
					if (!(name == "BulletinBoard"))
					{
						return -1;
					}
					return 5;
				}
				else
				{
					if (!(name == "Vault"))
					{
						return -1;
					}
					return 4;
				}
			}
			else
			{
				if (!(name == "Boiler Room"))
				{
					return -1;
				}
				return 3;
			}
			return 1;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x001567C0 File Offset: 0x001549C0
		private Point getNotePosition(int area)
		{
			switch (area)
			{
			case 0:
				return new Point(14, 5);
			case 1:
				return new Point(14, 23);
			case 2:
				return new Point(40, 10);
			case 3:
				return new Point(63, 14);
			case 4:
				return new Point(55, 6);
			case 5:
				return new Point(46, 11);
			default:
				return Point.Zero;
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0015682C File Offset: 0x00154A2C
		public void addJunimoNote(int area)
		{
			Point position = this.getNotePosition(area);
			if (!position.Equals(Vector2.Zero))
			{
				StaticTile[] tileFrames = this.getJunimoNoteTileFrames(area);
				string layer = (area == 5) ? "Front" : "Buildings";
				this.map.GetLayer(layer).Tiles[position.X, position.Y] = new AnimatedTile(this.map.GetLayer(layer), tileFrames, 70L);
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(position.X * Game1.tileSize), (float)(position.Y * Game1.tileSize)), 1f));
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(position.X * Game1.tileSize), (float)(position.Y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f)
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(position.X * Game1.tileSize - Game1.pixelZoom * 3), (float)(position.Y * Game1.tileSize - Game1.pixelZoom * 3)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					scale = 0.75f,
					layerDepth = 1f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f),
					delayBeforeAnimationStart = 50
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(position.X * Game1.tileSize - Game1.pixelZoom * 3), (float)(position.Y * Game1.tileSize + Game1.pixelZoom * 3)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f),
					delayBeforeAnimationStart = 100
				});
				this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(position.X * Game1.tileSize), (float)(position.Y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					scale = 0.75f,
					interval = 50f,
					motion = new Vector2(1f, 0f),
					acceleration = new Vector2(-0.005f, 0f),
					delayBeforeAnimationStart = 150
				});
			}
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00156B48 File Offset: 0x00154D48
		public int numberOfCompleteBundles()
		{
			int number = 0;
			foreach (KeyValuePair<int, bool[]> v in this.bundles)
			{
				number++;
				for (int i = 0; i < v.Value.Length; i++)
				{
					if (!v.Value[i])
					{
						number--;
						break;
					}
				}
			}
			return number;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x00156BC0 File Offset: 0x00154DC0
		public void addStarToPlaque()
		{
			this.numberOfStarsOnPlaque++;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00156BD0 File Offset: 0x00154DD0
		private string getMessageForAreaCompletion()
		{
			int areasComplete = this.getNumberOfAreasComplete();
			if (areasComplete >= 1 && areasComplete <= 6)
			{
				return Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaCompletion" + areasComplete, new object[]
				{
					Game1.player.name
				});
			}
			return "";
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00156C20 File Offset: 0x00154E20
		private int getNumberOfAreasComplete()
		{
			int complete = 0;
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				if (this.areasComplete[i])
				{
					complete++;
				}
			}
			return complete;
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00156C54 File Offset: 0x00154E54
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int tileIndexOfCheckLocation = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (tileIndexOfCheckLocation != 1799)
			{
				switch (tileIndexOfCheckLocation)
				{
				case 1824:
				case 1825:
				case 1826:
				case 1827:
				case 1828:
				case 1829:
				case 1830:
				case 1831:
				case 1832:
				case 1833:
					Game1.activeClickableMenu = new JunimoNoteMenu(this.getAreaNumberFromLocation(who.getTileLocation()), this.bundles);
					break;
				}
			}
			else if (this.numberOfCompleteBundles() > 2)
			{
				Game1.activeClickableMenu = new JunimoNoteMenu(5, this.bundles);
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00156D21 File Offset: 0x00154F21
		public void addJunimoNoteViewportTarget(int area)
		{
			if (this.junimoNotesViewportTargets == null)
			{
				this.junimoNotesViewportTargets = new List<int>();
			}
			this.junimoNotesViewportTargets.Add(area);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00156D44 File Offset: 0x00154F44
		public void checkForNewJunimoNotes()
		{
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				if (!this.isJunimoNoteAtArea(i) && this.shouldNoteAppearInArea(i))
				{
					this.addJunimoNoteViewportTarget(i);
				}
			}
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00156D80 File Offset: 0x00154F80
		public void removeJunimoNote(int area)
		{
			Point p = this.getNotePosition(area);
			if (area == 5)
			{
				this.map.GetLayer("Front").Tiles[p.X, p.Y] = null;
				return;
			}
			this.map.GetLayer("Buildings").Tiles[p.X, p.Y] = null;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00156DE8 File Offset: 0x00154FE8
		public bool isJunimoNoteAtArea(int area)
		{
			Point p = this.getNotePosition(area);
			if (area == 5)
			{
				return this.map.GetLayer("Front").Tiles[p.X, p.Y] != null;
			}
			return this.map.GetLayer("Buildings").Tiles[p.X, p.Y] != null;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00156E54 File Offset: 0x00155054
		public bool shouldNoteAppearInArea(int area)
		{
			if (area >= 0 && this.areasComplete.Length > area && !this.areasComplete[area])
			{
				switch (area)
				{
				case 0:
				case 2:
					if (this.numberOfCompleteBundles() > 0)
					{
						return true;
					}
					break;
				case 1:
					return true;
				case 3:
					if (this.numberOfCompleteBundles() > 1)
					{
						return true;
					}
					break;
				case 4:
					if (this.numberOfCompleteBundles() > 3)
					{
						return true;
					}
					break;
				case 5:
					if (this.numberOfCompleteBundles() > 2)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00156ECC File Offset: 0x001550CC
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("JojaMember"))
			{
				this.map = Game1.content.Load<Map>("Maps\\CommunityCenter_Joja");
				this.warehouse = true;
				this.refurbishedLoaded = true;
			}
			else if (this.areAllAreasComplete() && !this.refurbishedLoaded)
			{
				this.map = Game1.content.Load<Map>("Maps\\CommunityCenter_Refurbished");
				this.refurbishedLoaded = true;
			}
			else
			{
				for (int i = 0; i < this.areasComplete.Length; i++)
				{
					if (this.shouldNoteAppearInArea(i))
					{
						this.addJunimoNote(i);
						this.characters.Add(new Junimo(new Vector2((float)this.getNotePosition(i).X, (float)(this.getNotePosition(i).Y + 2)) * (float)Game1.tileSize, i, false));
					}
					else if (this.areasComplete[i])
					{
						this.loadArea(i, false);
					}
				}
			}
			this.numberOfStarsOnPlaque = 0;
			for (int j = 0; j < this.areasComplete.Length; j++)
			{
				if (this.areasComplete[j])
				{
					this.numberOfStarsOnPlaque++;
				}
			}
			if (!Game1.eventUp && !this.areAllAreasComplete())
			{
				Game1.changeMusicTrack("communityCenter");
			}
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0015700C File Offset: 0x0015520C
		private int getAreaNumberFromLocation(Vector2 tileLocation)
		{
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				if (this.getAreaBounds(i).Contains((int)tileLocation.X, (int)tileLocation.Y))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00157050 File Offset: 0x00155250
		private Microsoft.Xna.Framework.Rectangle getAreaBounds(int area)
		{
			switch (area)
			{
			case 0:
				return new Microsoft.Xna.Framework.Rectangle(0, 0, 22, 11);
			case 1:
				return new Microsoft.Xna.Framework.Rectangle(0, 12, 21, 17);
			case 2:
				return new Microsoft.Xna.Framework.Rectangle(35, 4, 9, 9);
			case 3:
				return new Microsoft.Xna.Framework.Rectangle(52, 9, 16, 12);
			case 4:
				return new Microsoft.Xna.Framework.Rectangle(45, 0, 15, 9);
			case 5:
				return new Microsoft.Xna.Framework.Rectangle(22, 13, 28, 9);
			case 6:
				return new Microsoft.Xna.Framework.Rectangle(44, 10, 6, 3);
			case 7:
				return new Microsoft.Xna.Framework.Rectangle(22, 4, 13, 9);
			default:
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x001570F4 File Offset: 0x001552F4
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			for (int i = this.characters.Count - 1; i >= 0; i--)
			{
				if (this.characters[i] is Junimo)
				{
					this.characters.RemoveAt(i);
				}
			}
			Game1.changeMusicTrack("none");
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00157148 File Offset: 0x00155348
		public bool isBundleComplete(int bundleIndex)
		{
			for (int i = 0; i < this.bundles[bundleIndex].Length; i++)
			{
				if (!this.bundles[bundleIndex][i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x00157184 File Offset: 0x00155384
		public void areaCompleteReward(int whichArea)
		{
			string mailReceivedID = "";
			switch (whichArea)
			{
			case 0:
				mailReceivedID = "ccPantry";
				goto IL_A4;
			case 1:
				break;
			case 2:
				mailReceivedID = "ccFishTank";
				goto IL_A4;
			case 3:
				mailReceivedID = "ccBoilerRoom";
				goto IL_A4;
			case 4:
				mailReceivedID = "ccVault";
				goto IL_A4;
			case 5:
				mailReceivedID = "ccBulletin";
				Game1.addMailForTomorrow("ccBulletinThankYou", false, false);
				using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC i = enumerator.Current;
						if (!i.datable)
						{
							Game1.player.changeFriendship(500, i);
						}
					}
					goto IL_A4;
				}
				break;
			default:
				goto IL_A4;
			}
			mailReceivedID = "ccCraftsRoom";
			IL_A4:
			if (mailReceivedID.Length > 0 && !Game1.player.mailReceived.Contains(mailReceivedID))
			{
				Game1.player.mailForTomorrow.Add(mailReceivedID + "%&NL&%");
			}
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0015727C File Offset: 0x0015547C
		public void completeBundle(int which)
		{
			bool foundOneEmpty = false;
			for (int i = 0; i < this.bundles[which].Length; i++)
			{
				if (!foundOneEmpty && !this.bundles[which][i])
				{
					foundOneEmpty = true;
				}
				this.bundles[which][i] = true;
			}
			if (foundOneEmpty)
			{
				this.bundleRewards[which] = true;
			}
			int whichArea = this.bundleToAreaDictionary[which];
			if (!this.areasComplete[whichArea])
			{
				bool foundAnIncomplete = false;
				foreach (int j in this.areaToBundleDictionary[whichArea])
				{
					if (!this.isBundleComplete(j))
					{
						foundAnIncomplete = true;
						break;
					}
				}
				if (!foundAnIncomplete)
				{
					this.areasComplete[whichArea] = true;
					this.areaCompleteReward(whichArea);
					if (Game1.IsMultiplayer)
					{
						Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaRestored", new object[]
						{
							CommunityCenter.getAreaDisplayNameFromNumber(whichArea)
						}), -1L);
						return;
					}
					Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaRestored", new object[]
					{
						CommunityCenter.getAreaDisplayNameFromNumber(whichArea)
					}));
				}
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x001573B4 File Offset: 0x001555B4
		public void loadArea(int area, bool showEffects = true)
		{
			Microsoft.Xna.Framework.Rectangle areaToRefurbish = this.getAreaBounds(area);
			Map refurbishedMap = Game1.content.Load<Map>("Maps\\CommunityCenter_Refurbished");
			for (int x = areaToRefurbish.X; x < areaToRefurbish.Right; x++)
			{
				for (int y = areaToRefurbish.Y; y < areaToRefurbish.Bottom; y++)
				{
					if (refurbishedMap.GetLayer("Back").Tiles[x, y] != null)
					{
						this.map.GetLayer("Back").Tiles[x, y].TileIndex = refurbishedMap.GetLayer("Back").Tiles[x, y].TileIndex;
					}
					if (refurbishedMap.GetLayer("Buildings").Tiles[x, y] != null)
					{
						this.map.GetLayer("Buildings").Tiles[x, y] = new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, refurbishedMap.GetLayer("Buildings").Tiles[x, y].TileIndex);
						base.adjustMapLightPropertiesForLamp(refurbishedMap.GetLayer("Buildings").Tiles[x, y].TileIndex, x, y, "Buildings");
					}
					else
					{
						this.map.GetLayer("Buildings").Tiles[x, y] = null;
					}
					if (refurbishedMap.GetLayer("Front").Tiles[x, y] != null)
					{
						this.map.GetLayer("Front").Tiles[x, y] = new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, refurbishedMap.GetLayer("Front").Tiles[x, y].TileIndex);
						base.adjustMapLightPropertiesForLamp(refurbishedMap.GetLayer("Front").Tiles[x, y].TileIndex, x, y, "Front");
					}
					else
					{
						this.map.GetLayer("Front").Tiles[x, y] = null;
					}
					if (refurbishedMap.GetLayer("Paths").Tiles[x, y] != null && refurbishedMap.GetLayer("Paths").Tiles[x, y].TileIndex == 8)
					{
						Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize)), 2f));
					}
					if (showEffects && Game1.random.NextDouble() < 0.58 && refurbishedMap.GetLayer("Buildings").Tiles[x, y] == null)
					{
						this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
						{
							layerDepth = 1f,
							interval = 50f,
							motion = new Vector2((float)Game1.random.Next(17) / 10f, 0f),
							acceleration = new Vector2(-0.005f, 0f),
							delayBeforeAnimationStart = Game1.random.Next(500)
						});
					}
				}
			}
			if (area == 5)
			{
				this.loadArea(6, true);
			}
			base.addLightGlows();
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0015773C File Offset: 0x0015593C
		public void restoreAreaCutscene(int whichArea)
		{
			Game1.freezeControls = true;
			this.restoreAreaIndex = whichArea;
			this.restoreAreaPhase = 0;
			this.restoreAreaTimer = 1000;
			Game1.changeMusicTrack("none");
			this.areasComplete[whichArea] = true;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00157770 File Offset: 0x00155970
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.restoreAreaTimer > 0)
			{
				int old = this.restoreAreaTimer;
				this.restoreAreaTimer -= time.ElapsedGameTime.Milliseconds;
				switch (this.restoreAreaPhase)
				{
				case 0:
					if (this.restoreAreaTimer <= 0)
					{
						this.restoreAreaTimer = 3000;
						this.restoreAreaPhase = 1;
						Game1.player.faceDirection(2);
						Game1.player.jump();
						Game1.player.jitterStrength = 1f;
						Game1.player.showFrame(94, false);
						return;
					}
					break;
				case 1:
					if (Game1.random.NextDouble() < 0.4)
					{
						Vector2 v = Utility.getRandomPositionInThisRectangle(this.getAreaBounds(this.restoreAreaIndex), Game1.random);
						Junimo i = new Junimo(v * (float)Game1.tileSize, this.restoreAreaIndex, true);
						if (!base.isCollidingPosition(i.GetBoundingBox(), Game1.viewport, i))
						{
							this.characters.Add(i);
							this.temporarySprites.Add(new TemporaryAnimatedSprite((Game1.random.NextDouble() < 0.5) ? 5 : 46, v * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
							{
								layerDepth = 1f
							});
							Game1.playSound("tinyWhip");
						}
					}
					if (this.restoreAreaTimer <= 0)
					{
						this.restoreAreaTimer = 999999;
						this.restoreAreaPhase = 2;
						Game1.screenGlowOnce(Color.White, true, 0.005f, 1f);
						if (Game1.soundBank != null)
						{
							this.buildUpSound = Game1.soundBank.GetCue("wind");
							this.buildUpSound.SetVariable("Volume", 0f);
							this.buildUpSound.SetVariable("Frequency", 0f);
							this.buildUpSound.Play();
						}
						Game1.player.jitterStrength = 2f;
						Game1.player.stopShowingFrame();
					}
					Game1.drawLighting = false;
					return;
				case 2:
					if (this.buildUpSound != null)
					{
						this.buildUpSound.SetVariable("Volume", Game1.screenGlowAlpha * 150f);
						this.buildUpSound.SetVariable("Frequency", Game1.screenGlowAlpha * 150f);
					}
					if (Game1.screenGlowAlpha >= Game1.screenGlowMax)
					{
						this.messageAlpha += 0.008f;
						this.messageAlpha = Math.Min(this.messageAlpha, 1f);
					}
					if (Game1.screenGlowAlpha == Game1.screenGlowMax && this.restoreAreaTimer > 5200)
					{
						this.restoreAreaTimer = 5200;
					}
					if (this.restoreAreaTimer < 5200 && Game1.random.NextDouble() < (double)((float)(5200 - this.restoreAreaTimer) / 10000f))
					{
						Game1.playSound((Game1.random.NextDouble() < 0.5) ? "dustMeep" : "junimoMeep1");
					}
					if (this.restoreAreaTimer <= 0)
					{
						this.restoreAreaTimer = 2000;
						this.restoreAreaPhase = 3;
						Game1.screenGlowHold = false;
						this.loadArea(this.restoreAreaIndex, true);
						if (this.buildUpSound != null)
						{
							this.buildUpSound.Stop(AudioStopOptions.Immediate);
						}
						Game1.playSound("wand");
						Game1.changeMusicTrack("junimoStarSong");
						Game1.playSound("woodyHit");
						this.messageAlpha = 0f;
						Game1.flashAlpha = 1f;
						Game1.player.stopJittering();
						for (int j = this.characters.Count - 1; j >= 0; j--)
						{
							if (this.characters[j] is Junimo && (this.characters[j] as Junimo).temporaryJunimo)
							{
								this.characters.RemoveAt(j);
							}
						}
						Game1.drawLighting = true;
						return;
					}
					break;
				case 3:
					if (old > 1000 && this.restoreAreaTimer <= 1000)
					{
						Junimo k = this.getJunimoForArea(this.restoreAreaIndex);
						if (k != null)
						{
							k.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float)Game1.tileSize;
							int iter = 0;
							while (base.isCollidingPosition(k.GetBoundingBox(), Game1.viewport, k) && iter < 20)
							{
								k.position = Utility.getRandomPositionInThisRectangle(this.getAreaBounds(this.restoreAreaIndex), Game1.random);
								iter++;
							}
							if (iter < 20)
							{
								k.fadeBack();
								k.returnToJunimoHutToFetchStar(this);
							}
						}
					}
					if (this.restoreAreaTimer <= 0)
					{
						Game1.freezeControls = false;
						return;
					}
					break;
				default:
					return;
				}
			}
			else if (Game1.activeClickableMenu == null && this.junimoNotesViewportTargets != null && this.junimoNotesViewportTargets.Count > 0 && !Game1.isViewportOnCustomPath())
			{
				this.setViewportToNextJunimoNoteTarget();
			}
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00157C58 File Offset: 0x00155E58
		private void setViewportToNextJunimoNoteTarget()
		{
			if (this.junimoNotesViewportTargets.Count > 0)
			{
				Game1.freezeControls = true;
				int area = this.junimoNotesViewportTargets[0];
				Point p = this.getNotePosition(area);
				Game1.moveViewportTo(new Vector2((float)p.X, (float)p.Y) * (float)Game1.tileSize, 5f, 2000, new Game1.afterFadeFunction(this.afterViewportGetsToJunimoNotePosition), new Game1.afterFadeFunction(this.setViewportToNextJunimoNoteTarget));
				return;
			}
			Game1.viewportFreeze = true;
			Game1.viewportHold = 10000;
			Game1.globalFadeToBlack(new Game1.afterFadeFunction(Game1.afterFadeReturnViewportToPlayer), 0.02f);
			Game1.freezeControls = false;
			Game1.afterViewport = null;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00157D08 File Offset: 0x00155F08
		private void afterViewportGetsToJunimoNotePosition()
		{
			int area = this.junimoNotesViewportTargets[0];
			this.junimoNotesViewportTargets.RemoveAt(0);
			this.addJunimoNote(area);
			Game1.playSound("reward");
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00157D40 File Offset: 0x00155F40
		public Junimo getJunimoForArea(int whichArea)
		{
			foreach (Character c in this.characters)
			{
				if (c is Junimo && (c as Junimo).whichArea == whichArea)
				{
					return c as Junimo;
				}
			}
			Junimo i = new Junimo(Vector2.Zero, whichArea, false);
			base.addCharacter(i);
			return i;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00157DC4 File Offset: 0x00155FC4
		public bool areAllAreasComplete()
		{
			bool[] array = this.areasComplete;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00157DF0 File Offset: 0x00155FF0
		public void junimoGoodbyeDance()
		{
			this.getJunimoForArea(0).position = new Vector2(23f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(1).position = new Vector2(27f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(2).position = new Vector2(24f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(4).position = new Vector2(26f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(3).position = new Vector2(28f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(5).position = new Vector2(25f, 11f) * (float)Game1.tileSize;
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				this.getJunimoForArea(i).stayStill();
				this.getJunimoForArea(i).faceDirection(1);
				this.getJunimoForArea(i).fadeBack();
				this.getJunimoForArea(i).isInvisible = false;
				this.getJunimoForArea(i).setAlpha(1f);
			}
			Game1.moveViewportTo(new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()), 2f, 5000, new Game1.afterFadeFunction(this.startGoodbyeDance), new Game1.afterFadeFunction(this.endGoodbyeDance));
			Game1.viewportFreeze = false;
			Game1.freezeControls = true;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00157F88 File Offset: 0x00156188
		private void startGoodbyeDance()
		{
			Game1.freezeControls = true;
			this.getJunimoForArea(0).position = new Vector2(23f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(1).position = new Vector2(27f, 11f) * (float)Game1.tileSize;
			this.getJunimoForArea(2).position = new Vector2(24f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(4).position = new Vector2(26f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(3).position = new Vector2(28f, 12f) * (float)Game1.tileSize;
			this.getJunimoForArea(5).position = new Vector2(25f, 11f) * (float)Game1.tileSize;
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				this.getJunimoForArea(i).stayStill();
				this.getJunimoForArea(i).faceDirection(1);
				this.getJunimoForArea(i).fadeBack();
				this.getJunimoForArea(i).isInvisible = false;
				this.getJunimoForArea(i).setAlpha(1f);
				this.getJunimoForArea(i).sayGoodbye();
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x001580E4 File Offset: 0x001562E4
		private void endGoodbyeDance()
		{
			for (int i = 0; i < this.areasComplete.Length; i++)
			{
				this.getJunimoForArea(i).fadeAway();
			}
			Game1.pauseThenDoFunction(3600, new Game1.afterFadeFunction(this.loadJunimoHut));
			Game1.freezeControls = true;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0015812C File Offset: 0x0015632C
		private void loadJunimoHut()
		{
			this.loadArea(7, true);
			Game1.flashAlpha = 1f;
			Game1.playSound("wand");
			Game1.freezeControls = false;
			Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_JunimosReturned", new object[0]));
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0015816C File Offset: 0x0015636C
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			for (int i = 0; i < this.numberOfStarsOnPlaque; i++)
			{
				switch (i)
				{
				case 0:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize + 6 * Game1.pixelZoom), (float)(5 * Game1.tileSize + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 1:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize + 6 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 11 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 2:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize - 4 * Game1.pixelZoom), (float)(6 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 3:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(32 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(5 * Game1.tileSize + 11 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 4:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(32 * Game1.tileSize + 2 * Game1.pixelZoom), (float)(5 * Game1.tileSize + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				case 5:
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(33 * Game1.tileSize - 4 * Game1.pixelZoom), (float)(5 * Game1.tileSize - 3 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
					break;
				}
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00158450 File Offset: 0x00156650
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			if (this.messageAlpha > 0f)
			{
				Junimo i = this.getJunimoForArea(0);
				if (i != null)
				{
					b.Draw(i.Sprite.Texture, new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize / 2), (float)(Game1.viewport.Height * 2) / 3f - (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800.0) / 100 * 16, 0, 16, 16)), Color.Lime * this.messageAlpha, 0f, new Vector2((float)(i.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(i.sprite.spriteHeight * Game1.pixelZoom) * 3f / 4f) / (float)Game1.pixelZoom, Math.Max(0.2f, 1f) * (float)Game1.pixelZoom, i.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
				}
				b.DrawString(Game1.dialogueFont, "\"" + Game1.parseText(this.getMessageForAreaCompletion() + "\"", Game1.dialogueFont, Game1.tileSize * 10), new Vector2((float)(Game1.viewport.Width / 2 - Game1.tileSize * 5), (float)(Game1.viewport.Height * 2) / 3f), Game1.textColor * this.messageAlpha * 0.6f);
			}
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x001585F8 File Offset: 0x001567F8
		public static string getAreaNameFromNumber(int areaNumber)
		{
			switch (areaNumber)
			{
			case 0:
				return "Pantry";
			case 1:
				return "Crafts Room";
			case 2:
				return "Fish Tank";
			case 3:
				return "Boiler Room";
			case 4:
				return "Vault";
			case 5:
				return "Bulletin Board";
			default:
				return "";
			}
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0015864E File Offset: 0x0015684E
		public static string getAreaDisplayNameFromNumber(int areaNumber)
		{
			return Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaName_" + CommunityCenter.getAreaNameFromNumber(areaNumber).Replace(" ", ""), new object[0]);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00158680 File Offset: 0x00156880
		private StaticTile[] getJunimoNoteTileFrames(int area)
		{
			if (area == 5)
			{
				return new StaticTile[]
				{
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1773),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1805),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1805),
					new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1773)
				};
			}
			return new StaticTile[]
			{
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1832),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1824),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1825),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1826),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1827),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1828),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1829),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1830),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1831),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1832),
				new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833)
			};
		}

		// Token: 0x040011E9 RID: 4585
		public const int AREA_Pantry = 0;

		// Token: 0x040011EA RID: 4586
		public const int AREA_FishTank = 2;

		// Token: 0x040011EB RID: 4587
		public const int AREA_CraftsRoom = 1;

		// Token: 0x040011EC RID: 4588
		public const int AREA_BoilerRoom = 3;

		// Token: 0x040011ED RID: 4589
		public const int AREA_Vault = 4;

		// Token: 0x040011EE RID: 4590
		public const int AREA_Bulletin = 5;

		// Token: 0x040011EF RID: 4591
		public const int AREA_Bulletin2 = 6;

		// Token: 0x040011F0 RID: 4592
		public const int AREA_JunimoHut = 7;

		// Token: 0x040011F1 RID: 4593
		private bool refurbishedLoaded;

		// Token: 0x040011F2 RID: 4594
		private bool warehouse;

		// Token: 0x040011F3 RID: 4595
		public SerializableDictionary<int, bool[]> bundles;

		// Token: 0x040011F4 RID: 4596
		public SerializableDictionary<int, bool> bundleRewards;

		// Token: 0x040011F5 RID: 4597
		public bool[] areasComplete = new bool[6];

		// Token: 0x040011F6 RID: 4598
		public int numberOfStarsOnPlaque;

		// Token: 0x040011F7 RID: 4599
		private float messageAlpha;

		// Token: 0x040011F8 RID: 4600
		private List<int> junimoNotesViewportTargets;

		// Token: 0x040011F9 RID: 4601
		private Dictionary<int, List<int>> areaToBundleDictionary;

		// Token: 0x040011FA RID: 4602
		private Dictionary<int, int> bundleToAreaDictionary;

		// Token: 0x040011FB RID: 4603
		public const int PHASE_firstPause = 0;

		// Token: 0x040011FC RID: 4604
		public const int PHASE_junimoAppear = 1;

		// Token: 0x040011FD RID: 4605
		public const int PHASE_junimoDance = 2;

		// Token: 0x040011FE RID: 4606
		public const int PHASE_restore = 3;

		// Token: 0x040011FF RID: 4607
		private int restoreAreaTimer;

		// Token: 0x04001200 RID: 4608
		private int restoreAreaPhase;

		// Token: 0x04001201 RID: 4609
		private int restoreAreaIndex;

		// Token: 0x04001202 RID: 4610
		private Cue buildUpSound;
	}
}
