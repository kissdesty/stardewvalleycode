using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x0200012E RID: 302
	public class Town : GameLocation
	{
		// Token: 0x0600115E RID: 4446 RVA: 0x00163EC0 File Offset: 0x001620C0
		public Town()
		{
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00163F64 File Offset: 0x00162164
		public Town(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00164008 File Offset: 0x00162208
		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (!Game1.isStartingToGetDarkOut())
			{
				this.addClintMachineGraphics();
				return;
			}
			AmbientLocationSounds.removeSound(new Vector2(100f, 79f));
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00164033 File Offset: 0x00162233
		public void checkedBoard()
		{
			this.playerCheckedBoard = true;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0016403C File Offset: 0x0016223C
		private void addClintMachineGraphics()
		{
			this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(302, 1946, 15, 16), (float)(7000 - Game1.gameTimeInterval), 1, 1, new Vector2(100f, 79f) * (float)Game1.tileSize + new Vector2(9f, 6f) * (float)Game1.pixelZoom, false, false, (float)(81 * Game1.tileSize + Game1.pixelZoom) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
			{
				shakeIntensity = 1f
			});
			for (int i = 0; i < 10; i++)
			{
				Utility.addSmokePuff(this, new Vector2(101f, 78f) * (float)Game1.tileSize + new Vector2(4f, 4f) * (float)Game1.pixelZoom, i * ((7000 - Game1.gameTimeInterval) / 16));
			}
			for (int j = 0; j < Game1.random.Next(1, 4); j++)
			{
				for (int k = 0; k < 16; k++)
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(643, 1305, 5, 18), 50f, 4, 1, new Vector2(100f, 78f) * (float)Game1.tileSize + new Vector2((float)(-5 - k * 4), 0f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = j * 1500 + 100 * k
					});
				}
				Utility.addSmokePuff(this, new Vector2(100f, 78f) * (float)Game1.tileSize + new Vector2(-70f, -6f) * (float)Game1.pixelZoom, j * 1500 + 1600);
			}
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00164274 File Offset: 0x00162474
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			for (int i = 0; i < this.garbageChecked.Length; i++)
			{
				this.garbageChecked[i] = false;
			}
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x001642A4 File Offset: 0x001624A4
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex <= 1080)
				{
					if (tileIndex <= 620)
					{
						if (tileIndex != 78)
						{
							if (tileIndex != 620)
							{
								goto IL_677;
							}
							if (Game1.player.eventsSeen.Contains(191393))
							{
								Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_SeedShopSign", new object[0]).Replace('\n', '^'));
							}
							return true;
						}
						else
						{
							string s = base.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
							int whichCan = (s != null) ? Convert.ToInt32(s.Split(new char[]
							{
								' '
							})[1]) : -1;
							if (whichCan < 0 || whichCan >= this.garbageChecked.Length || this.garbageChecked[whichCan])
							{
								goto IL_677;
							}
							this.garbageChecked[whichCan] = true;
							Game1.playSound("trashcan");
							Character c = Utility.isThereAFarmerOrCharacterWithinDistance(new Vector2((float)tileLocation.X, (float)tileLocation.Y), 7, this);
							if (c != null && c is NPC && !c.name.Equals("Linus") && !(c is Horse))
							{
								if ((c as NPC).age == 2)
								{
									c.doEmote(28, true);
									(c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Child", new object[0]), true, true);
								}
								else if ((c as NPC).age == 1)
								{
									c.doEmote(8, true);
									(c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Teen", new object[0]), true, true);
								}
								else
								{
									c.doEmote(12, true);
									(c as NPC).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Town_DumpsterDiveComment_Adult", new object[0]), true, true);
								}
								who.changeFriendship(-25, c as NPC);
								Game1.drawDialogue(c as NPC);
							}
							Random garbageRandom = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + 777 + whichCan);
							if (garbageRandom.NextDouble() < 0.2 + Game1.dailyLuck)
							{
								int item = 168;
								switch (garbageRandom.Next(10))
								{
								case 0:
									item = 168;
									break;
								case 1:
									item = 167;
									break;
								case 2:
									item = 170;
									break;
								case 3:
									item = 171;
									break;
								case 4:
									item = 172;
									break;
								case 5:
									item = 216;
									break;
								case 6:
									item = Utility.getRandomItemFromSeason(Game1.currentSeason, tileLocation.X * 653 + tileLocation.Y * 777, false);
									break;
								case 7:
									item = 403;
									break;
								case 8:
									item = 309 + garbageRandom.Next(3);
									break;
								case 9:
									item = 153;
									break;
								}
								if (whichCan == 3 && garbageRandom.NextDouble() < 0.2 + Game1.dailyLuck)
								{
									item = 535;
									if (garbageRandom.NextDouble() < 0.05)
									{
										item = 749;
									}
								}
								if (whichCan == 4 && garbageRandom.NextDouble() < 0.2 + Game1.dailyLuck)
								{
									item = 378 + garbageRandom.Next(3) * 2;
									garbageRandom.Next(1, 5);
								}
								if (whichCan == 5 && garbageRandom.NextDouble() < 0.2 + Game1.dailyLuck && Game1.dishOfTheDay != null)
								{
									if (Game1.dishOfTheDay.parentSheetIndex == 217)
									{
										item = 216;
									}
									else
									{
										item = Game1.dishOfTheDay.parentSheetIndex;
									}
								}
								if (whichCan == 6 && garbageRandom.NextDouble() < 0.2 + Game1.dailyLuck)
								{
									item = 223;
								}
								who.addItemByMenuIfNecessary(new Object(item, 1, false, -1, 0), null);
								goto IL_677;
							}
							goto IL_677;
						}
					}
					else if (tileIndex != 958 && tileIndex != 1080)
					{
						goto IL_677;
					}
				}
				else
				{
					if (tileIndex <= 1925)
					{
						if (tileIndex == 1081)
						{
							goto IL_49A;
						}
						if (tileIndex != 1925)
						{
							goto IL_677;
						}
					}
					else if (tileIndex != 1926 && tileIndex != 1945 && tileIndex != 1946)
					{
						goto IL_677;
					}
					if (this.isShowingDestroyedJoja)
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_JojaSign_Destroyed", new object[0]));
						return true;
					}
					goto IL_677;
				}
				IL_49A:
				if (Game1.player.getMount() != null)
				{
					return true;
				}
				if (Game1.player.getTileX() <= 70)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_PickupTruck", new object[0]));
					return true;
				}
				if (!Game1.player.mailReceived.Contains("ccBoilerRoom"))
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder", new object[0]));
					return true;
				}
				if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
				{
					Game1.player.getMount().checkAction(Game1.player, this);
				}
				else
				{
					Response[] destinations;
					if (Game1.player.mailReceived.Contains("ccCraftsRoom"))
					{
						destinations = new Response[]
						{
							new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
							new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
							new Response("Quarry", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Quarry", new object[0])),
							new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
						};
					}
					else
					{
						destinations = new Response[]
						{
							new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
							new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
							new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
						};
					}
					base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination", new object[0]), destinations, "Minecart");
				}
			}
			IL_677:
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00164934 File Offset: 0x00162B34
		private void refurbishCommunityCenter()
		{
			if (this.ccRefurbished)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle ccBounds = new Microsoft.Xna.Framework.Rectangle(47, 11, 11, 9);
			for (int x = ccBounds.X; x <= ccBounds.Right; x++)
			{
				for (int y = ccBounds.Y; y <= ccBounds.Bottom; y++)
				{
					if (this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Back").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("Back").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("Back").Tiles[x, y].TileIndex += 12;
					}
					if (this.map.GetLayer("Buildings").Tiles[x, y] != null && this.map.GetLayer("Buildings").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("Buildings").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("Buildings").Tiles[x, y].TileIndex += 12;
					}
					if (this.map.GetLayer("Front").Tiles[x, y] != null && this.map.GetLayer("Front").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("Front").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("Front").Tiles[x, y].TileIndex += 12;
					}
					if (this.map.GetLayer("AlwaysFront").Tiles[x, y] != null && this.map.GetLayer("AlwaysFront").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("AlwaysFront").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("AlwaysFront").Tiles[x, y].TileIndex += 12;
					}
				}
			}
			this.ccRefurbished = true;
			if (Game1.player.mailReceived.Contains("JojaMember"))
			{
				this.ccJoja = true;
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00164C4C File Offset: 0x00162E4C
		private void showDestroyedJoja()
		{
			if (this.isShowingDestroyedJoja)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle jojaBounds = new Microsoft.Xna.Framework.Rectangle(90, 42, 11, 9);
			for (int x = jojaBounds.X; x <= jojaBounds.Right; x++)
			{
				for (int y = jojaBounds.Y; y <= jojaBounds.Bottom; y++)
				{
					bool draw = false;
					if (x > jojaBounds.X + 6 || y < jojaBounds.Y + 9)
					{
						draw = true;
					}
					if (draw && this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Back").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("Back").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("Back").Tiles[x, y].TileIndex += 20;
					}
					if (draw && this.map.GetLayer("Buildings").Tiles[x, y] != null && this.map.GetLayer("Buildings").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("Buildings").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("Buildings").Tiles[x, y].TileIndex += 20;
					}
					if (draw && ((x != 93 && y != 50) || (x != 94 && y != 50)) && this.map.GetLayer("Front").Tiles[x, y] != null && this.map.GetLayer("Front").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("Front").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("Front").Tiles[x, y].TileIndex += 20;
					}
					if (draw && this.map.GetLayer("AlwaysFront").Tiles[x, y] != null && this.map.GetLayer("AlwaysFront").Tiles[x, y].TileSheet.Id.Equals("Town") && this.map.GetLayer("AlwaysFront").Tiles[x, y].TileIndex > 1200)
					{
						this.map.GetLayer("AlwaysFront").Tiles[x, y].TileIndex += 20;
					}
				}
			}
			this.isShowingDestroyedJoja = true;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00164F94 File Offset: 0x00163194
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
			{
				this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float)(107 * Game1.tileSize + Game1.pixelZoom * 2), (float)(79 * Game1.tileSize) - (float)Game1.tileSize * 3f / 4f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					totalNumberOfLoops = 999999,
					interval = 60f,
					flipped = true
				};
			}
			if (Game1.player.mailReceived.Contains("ccIsComplete") || Game1.player.mailReceived.Contains("JojaMember") || Game1.player.hasCompletedCommunityCenter())
			{
				this.refurbishCommunityCenter();
			}
			if (Game1.player.eventsSeen.Contains(191393))
			{
				this.showDestroyedJoja();
			}
			if (!Game1.currentSeason.Equals("winter"))
			{
				AmbientLocationSounds.addSound(new Vector2(26f, 26f), 0);
				AmbientLocationSounds.addSound(new Vector2(26f, 28f), 0);
			}
			if (!Game1.isStartingToGetDarkOut())
			{
				AmbientLocationSounds.addSound(new Vector2(100f, 79f), 2);
				this.addClintMachineGraphics();
			}
			if (Game1.player.mailReceived.Contains("checkedBulletinOnce"))
			{
				this.playerCheckedBoard = true;
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00165104 File Offset: 0x00163304
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.minecartSteam = null;
			if ((Game1.currentSong != null && (Game1.locationAfterWarp == null || Game1.locationAfterWarp.IsOutdoors) && Game1.currentSong.Name.ToLower().Contains("town")) || (Game1.nextMusicTrack != null && Game1.nextMusicTrack.ToLower().Contains("town")))
			{
				Game1.changeMusicTrack("none");
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0016517A File Offset: 0x0016337A
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.update(time);
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00165198 File Offset: 0x00163398
		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.draw(spriteBatch, false, 0, 0);
			}
			if (this.ccJoja)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.ccFacadePositionBottom), new Microsoft.Xna.Framework.Rectangle?(Town.jojaFacadeBottom), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(20 * Game1.tileSize) / 10000f);
			}
			if (!this.playerCheckedBoard)
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(41 * Game1.tileSize - 8), (float)(56 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(41 * Game1.tileSize + Game1.tileSize / 2), (float)(56 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0f, new Vector2(6f, 6f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			if (Game1.questOfTheDay != null && !Game1.questOfTheDay.accepted && Game1.questOfTheDay.questDescription.Length > 0)
			{
				float yOffset2 = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(42 * Game1.tileSize + Game1.pixelZoom), (float)(56 * Game1.tileSize - Game1.tileSize + Game1.tileSize / 8) + yOffset2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(395, 497, 3, 8)), Color.White, 0f, new Vector2(1f, 4f), (float)Game1.pixelZoom + Math.Max(0f, 0.25f - yOffset2 / 16f), SpriteEffects.None, 1f);
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0016544C File Offset: 0x0016364C
		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("fall") && who.getTileLocation().Y < 15f && who.FishingLevel >= 3 && !who.fishCaught.ContainsKey(160) && Game1.random.NextDouble() < 0.2)
			{
				return new Object(160, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x001654CC File Offset: 0x001636CC
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (this.ccJoja)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.ccFacadePosition), new Microsoft.Xna.Framework.Rectangle?(Town.jojaFacadeTop), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(20 * Game1.tileSize) / 10000f);
				if (Game1.IsWinter)
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.ccFacadePosition), new Microsoft.Xna.Framework.Rectangle?(Town.jojaFacadeWinterOverlay), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(20 * Game1.tileSize + 1) / 10000f);
				}
			}
			else if (this.ccRefurbished)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.clockCenter), new Microsoft.Xna.Framework.Rectangle?(Town.hourHandSource), Color.White, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1200) / 1200f) + (double)((float)Game1.gameTimeInterval / 7000f / 23f)), new Vector2(2.5f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, 0.98f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.clockCenter), new Microsoft.Xna.Framework.Rectangle?(Town.minuteHandSource), Color.White, (float)(6.2831853071795862 * (double)((float)(Game1.timeOfDay % 1000 % 100 % 60) / 60f) + (double)((float)Game1.gameTimeInterval / 7000f * 1.02f)), new Vector2(2.5f, 12f), (float)Game1.pixelZoom, SpriteEffects.None, 0.99f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.clockCenter), new Microsoft.Xna.Framework.Rectangle?(Town.clockNub), Color.White, 0f, new Vector2(2f, 2f), (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			base.drawAboveAlwaysFrontLayer(b);
		}

		// Token: 0x04001257 RID: 4695
		private TemporaryAnimatedSprite minecartSteam;

		// Token: 0x04001258 RID: 4696
		private bool ccRefurbished;

		// Token: 0x04001259 RID: 4697
		private bool ccJoja;

		// Token: 0x0400125A RID: 4698
		private bool playerCheckedBoard;

		// Token: 0x0400125B RID: 4699
		private bool isShowingDestroyedJoja;

		// Token: 0x0400125C RID: 4700
		private bool[] garbageChecked = new bool[7];

		// Token: 0x0400125D RID: 4701
		private Vector2 clockCenter = new Vector2((float)(53 * Game1.tileSize), (float)(16 * Game1.tileSize + Game1.tileSize / 2));

		// Token: 0x0400125E RID: 4702
		private Vector2 ccFacadePosition = new Vector2((float)(47 * Game1.tileSize + 9 * Game1.pixelZoom), (float)(11 * Game1.tileSize + 59 * Game1.pixelZoom));

		// Token: 0x0400125F RID: 4703
		private Vector2 ccFacadePositionBottom = new Vector2((float)(47 * Game1.tileSize + 9 * Game1.pixelZoom), (float)(11 * Game1.tileSize + 109 * Game1.pixelZoom));

		// Token: 0x04001260 RID: 4704
		public static Microsoft.Xna.Framework.Rectangle minuteHandSource = new Microsoft.Xna.Framework.Rectangle(363, 395, 5, 13);

		// Token: 0x04001261 RID: 4705
		public static Microsoft.Xna.Framework.Rectangle hourHandSource = new Microsoft.Xna.Framework.Rectangle(369, 399, 5, 9);

		// Token: 0x04001262 RID: 4706
		public static Microsoft.Xna.Framework.Rectangle clockNub = new Microsoft.Xna.Framework.Rectangle(375, 404, 4, 4);

		// Token: 0x04001263 RID: 4707
		public static Microsoft.Xna.Framework.Rectangle jojaFacadeTop = new Microsoft.Xna.Framework.Rectangle(424, 1275, 174, 50);

		// Token: 0x04001264 RID: 4708
		public static Microsoft.Xna.Framework.Rectangle jojaFacadeBottom = new Microsoft.Xna.Framework.Rectangle(424, 1325, 174, 51);

		// Token: 0x04001265 RID: 4709
		public static Microsoft.Xna.Framework.Rectangle jojaFacadeWinterOverlay = new Microsoft.Xna.Framework.Rectangle(66, 1678, 174, 25);
	}
}
