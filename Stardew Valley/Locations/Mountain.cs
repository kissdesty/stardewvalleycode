using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
	// Token: 0x0200012F RID: 303
	public class Mountain : GameLocation
	{
		// Token: 0x0600116E RID: 4462 RVA: 0x0016577C File Offset: 0x0016397C
		public Mountain()
		{
			if (Game1.stats.DaysPlayed >= 5u)
			{
				this.landslide = false;
			}
			if (Game1.stats.DaysPlayed >= 31u)
			{
				this.railroadAreaBlocked = false;
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x001658B4 File Offset: 0x00163AB4
		public Mountain(Map map, string name) : base(map, name)
		{
			for (int i = 0; i < 10; i++)
			{
				this.quarryDayUpdate();
			}
			if (Game1.stats.DaysPlayed >= 5u)
			{
				this.landslide = false;
			}
			if (Game1.stats.DaysPlayed >= 31u)
			{
				this.railroadAreaBlocked = false;
			}
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00165A00 File Offset: 0x00163C00
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex <= 1080)
				{
					if (tileIndex != 958 && tileIndex != 1080)
					{
						goto IL_1E0;
					}
				}
				else if (tileIndex != 1081)
				{
					if (tileIndex == 1136 && !who.mailReceived.Contains("guildMember") && !who.hasQuest(16))
					{
						Game1.drawLetterMessage(Game1.content.LoadString("Strings\\Locations:Mountain_AdventurersGuildNote", new object[0]).Replace('\n', '^'));
						return true;
					}
					goto IL_1E0;
				}
				if (Game1.player.getMount() != null)
				{
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
					Response[] destinations = new Response[]
					{
						new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop", new object[0])),
						new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
						new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town", new object[0])),
						new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
					};
					base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination", new object[0]), destinations, "Minecart");
				}
			}
			IL_1E0:
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00165BF8 File Offset: 0x00163DF8
		private void restoreBridge()
		{
			LocalizedContentManager temp = Game1.content.CreateTemporary();
			Map i = temp.Load<Map>("Maps\\Mountain-BridgeFixed");
			int xOffset = 92;
			int yOffset = 24;
			for (int x = 0; x < i.GetLayer("Back").LayerWidth; x++)
			{
				for (int y = 0; y < i.GetLayer("Back").LayerHeight; y++)
				{
					this.map.GetLayer("Back").Tiles[x + xOffset, y + yOffset] = ((i.GetLayer("Back").Tiles[x, y] == null) ? null : new StaticTile(this.map.GetLayer("Back"), this.map.TileSheets[0], BlendMode.Alpha, i.GetLayer("Back").Tiles[x, y].TileIndex));
					this.map.GetLayer("Buildings").Tiles[x + xOffset, y + yOffset] = ((i.GetLayer("Buildings").Tiles[x, y] == null) ? null : new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, i.GetLayer("Buildings").Tiles[x, y].TileIndex));
					this.map.GetLayer("Front").Tiles[x + xOffset, y + yOffset] = ((i.GetLayer("Front").Tiles[x, y] == null) ? null : new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, i.GetLayer("Front").Tiles[x, y].TileIndex));
				}
			}
			this.bridgeRestored = true;
			temp.Unload();
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00165E0C File Offset: 0x0016400C
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
			{
				this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float)(126 * Game1.tileSize + Game1.pixelZoom * 2), (float)(11 * Game1.tileSize) - (float)Game1.tileSize * 3f / 4f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					totalNumberOfLoops = 999999,
					interval = 60f,
					flipped = true
				};
			}
			if (!this.bridgeRestored && Game1.player.mailReceived.Contains("ccCraftsRoom"))
			{
				this.restoreBridge();
			}
			this.oreBoulderPresent = (!Game1.player.mailReceived.Contains("ccFishTank") || Game1.farmEvent != null);
			this.boulderSourceRect = new Microsoft.Xna.Framework.Rectangle(439 + (Game1.currentSeason.Equals("winter") ? 39 : 0), 1385, 39, 48);
			if (!this.objects.ContainsKey(new Vector2(29f, 9f)))
			{
				Vector2 tile = new Vector2(29f, 9f);
				this.objects.Add(tile, new Torch(tile, 146, true)
				{
					isOn = false,
					fragility = 2
				});
				this.objects[tile].checkForAction(null, false);
			}
			if (Game1.IsSpring)
			{
				this.raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 2176, 64, 80);
			}
			else
			{
				this.raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 1453, 64, 80);
			}
			base.addFrog();
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00165FCC File Offset: 0x001641CC
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.quarryDayUpdate();
			if (Game1.stats.DaysPlayed >= 31u)
			{
				this.railroadAreaBlocked = false;
			}
			if (Game1.stats.DaysPlayed >= 5u)
			{
				this.landslide = false;
				if (!Game1.player.hasOrWillReceiveMail("landslideDone"))
				{
					Game1.mailbox.Enqueue("landslideDone");
				}
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00166030 File Offset: 0x00164230
		private void quarryDayUpdate()
		{
			Microsoft.Xna.Framework.Rectangle quarryBounds = new Microsoft.Xna.Framework.Rectangle(106, 13, 21, 21);
			int numberOfAdditionsToTry = 5;
			for (int i = 0; i < numberOfAdditionsToTry; i++)
			{
				Vector2 position = Utility.getRandomPositionInThisRectangle(quarryBounds, Game1.random);
				if (this.isTileOpenForQuarryStone((int)position.X, (int)position.Y))
				{
					if (Game1.random.NextDouble() < 0.06)
					{
						if (this.isTileOpenForQuarryStone((int)position.X + 1, (int)position.Y) && this.isTileOpenForQuarryStone((int)position.Y, (int)position.Y + 1) && this.isTileOpenForQuarryStone((int)position.X + 1, (int)position.Y + 1))
						{
						}
					}
					else if (Game1.random.NextDouble() < 0.02)
					{
						if (Game1.random.NextDouble() < 0.1)
						{
							this.objects.Add(position, new Object(position, 46, "Stone", true, false, false, false)
							{
								minutesUntilReady = 12
							});
						}
						else
						{
							this.objects.Add(position, new Object(position, (Game1.random.Next(7) + 1) * 2, "Stone", true, false, false, false)
							{
								minutesUntilReady = 5
							});
						}
					}
					else if (Game1.random.NextDouble() < 0.1)
					{
						if (Game1.random.NextDouble() < 0.001)
						{
							this.objects.Add(position, new Object(position, 765, 1)
							{
								minutesUntilReady = 16
							});
						}
						else if (Game1.random.NextDouble() < 0.1)
						{
							this.objects.Add(position, new Object(position, 764, 1)
							{
								minutesUntilReady = 8
							});
						}
						else if (Game1.random.NextDouble() < 0.33)
						{
							this.objects.Add(position, new Object(position, 290, 1)
							{
								minutesUntilReady = 5
							});
						}
						else
						{
							this.objects.Add(position, new Object(position, 751, 1)
							{
								minutesUntilReady = 3
							});
						}
					}
					else
					{
						this.objects.Add(position, new Object(position, (Game1.random.NextDouble() < 0.25) ? 32 : ((Game1.random.NextDouble() < 0.33) ? 38 : ((Game1.random.NextDouble() < 0.5) ? 40 : 42)), 1)
						{
							minutesUntilReady = 2,
							name = "Stone"
						});
					}
				}
			}
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x001662D3 File Offset: 0x001644D3
		private bool isTileOpenForQuarryStone(int tileX, int tileY)
		{
			return base.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null && this.isTileLocationTotallyClearAndPlaceable(new Vector2((float)tileX, (float)tileY));
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x001662FA File Offset: 0x001644FA
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.minecartSteam = null;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0016630C File Offset: 0x0016450C
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.update(time);
			}
			if (this.landslide && (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds - 400.0) / 1600.0) % 2 != 0 && Utility.isOnScreen(new Point(this.landSlideRect.X / Game1.tileSize, this.landSlideRect.Y / Game1.tileSize), Game1.tileSize * 2, null))
			{
				if (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 < (double)(this.oldTime % 400))
				{
					Game1.playSound("hammer");
				}
				this.oldTime = (int)time.TotalGameTime.TotalMilliseconds;
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x001663F0 File Offset: 0x001645F0
		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("spring") && Game1.isRaining && who.FishingLevel >= 10 && !who.fishCaught.ContainsKey(163) && waterDepth >= 4 && Game1.random.NextDouble() < 0.1)
			{
				return new Object(163, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00166468 File Offset: 0x00164668
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return (this.landslide && position.Intersects(this.landSlideRect)) || (this.railroadAreaBlocked && position.Intersects(this.railroadBlockRect)) || base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x001664B8 File Offset: 0x001646B8
		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.draw(spriteBatch, false, 0, 0);
			}
			if (this.oreBoulderPresent)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.boulderPosition), new Microsoft.Xna.Framework.Rectangle?(this.boulderSourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
			}
			if (this.railroadAreaBlocked)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.railroadBlockRect), new Microsoft.Xna.Framework.Rectangle?(this.raildroadBlocksourceRect), Color.White, 0f, Vector2.Zero, SpriteEffects.None, (float)(3 * Game1.tileSize) / 10000f + 0.0001f);
			}
			if (this.landslide)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.landSlideRect), new Microsoft.Xna.Framework.Rectangle?(this.landSlideSourceRect), Color.White, 0f, Vector2.Zero, SpriteEffects.None, (float)(3 * Game1.tileSize) / 10000f);
				spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(new Vector2((float)(this.landSlideRect.X + Game1.tileSize * 3 - Game1.pixelZoom * 5), (float)(this.landSlideRect.Y + Game1.tileSize * 3 + Game1.pixelZoom * 5)) + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), (float)Game1.pixelZoom, SpriteEffects.None, 3.5f * (float)Game1.tileSize / 10000f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(this.landSlideRect.X + Game1.tileSize * 3 - Game1.pixelZoom * 5), (float)(this.landSlideRect.Y + Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(288 + (((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 1600.0 % 2.0) == 0) ? 0 : ((int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 / 100.0) * 19)), 1349, 19, 28)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(4 * Game1.tileSize) / 10000f);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(this.landSlideRect.X + Game1.tileSize * 4 - Game1.pixelZoom * 5), (float)(this.landSlideRect.Y + Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(335, 1410, 21, 21)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(2 * Game1.tileSize) / 10000f);
			}
		}

		// Token: 0x04001266 RID: 4710
		public const int daysBeforeLandslide = 31;

		// Token: 0x04001267 RID: 4711
		private TemporaryAnimatedSprite minecartSteam;

		// Token: 0x04001268 RID: 4712
		private bool bridgeRestored;

		// Token: 0x04001269 RID: 4713
		private bool oreBoulderPresent;

		// Token: 0x0400126A RID: 4714
		private bool railroadAreaBlocked = Game1.stats.DaysPlayed < 31u;

		// Token: 0x0400126B RID: 4715
		private bool landslide = Game1.stats.DaysPlayed < 5u;

		// Token: 0x0400126C RID: 4716
		private Microsoft.Xna.Framework.Rectangle landSlideRect = new Microsoft.Xna.Framework.Rectangle(50 * Game1.tileSize, 4 * Game1.tileSize, 3 * Game1.tileSize, 5 * Game1.tileSize);

		// Token: 0x0400126D RID: 4717
		private Microsoft.Xna.Framework.Rectangle railroadBlockRect = new Microsoft.Xna.Framework.Rectangle(8 * Game1.tileSize, 0, 4 * Game1.tileSize, 5 * Game1.tileSize);

		// Token: 0x0400126E RID: 4718
		private int oldTime;

		// Token: 0x0400126F RID: 4719
		private Microsoft.Xna.Framework.Rectangle boulderSourceRect = new Microsoft.Xna.Framework.Rectangle(439, 1385, 39, 48);

		// Token: 0x04001270 RID: 4720
		private Microsoft.Xna.Framework.Rectangle raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 2176, 64, 80);

		// Token: 0x04001271 RID: 4721
		private Microsoft.Xna.Framework.Rectangle landSlideSourceRect = new Microsoft.Xna.Framework.Rectangle(646, 1218, 48, 80);

		// Token: 0x04001272 RID: 4722
		private Vector2 boulderPosition = new Vector2(47f, 3f) * (float)Game1.tileSize - new Vector2(4f, 3f) * (float)Game1.pixelZoom;
	}
}
