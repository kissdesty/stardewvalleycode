using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x02000126 RID: 294
	public class Sewer : GameLocation
	{
		// Token: 0x060010A7 RID: 4263 RVA: 0x00155CAC File Offset: 0x00153EAC
		public Sewer()
		{
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00155D44 File Offset: 0x00153F44
		public Sewer(Map map, string name) : base(map, name)
		{
			this.waterColor = Color.LimeGreen;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00155DE8 File Offset: 0x00153FE8
		public Dictionary<Item, int[]> getShadowShopStock()
		{
			return this.dailyShadowStock;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00155DF0 File Offset: 0x00153FF0
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			this.populateShopStock(dayOfMonth);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00155E00 File Offset: 0x00154000
		public void populateShopStock(int dayOfMonth)
		{
			this.dailyShadowStock.Clear();
			this.dailyShadowStock.Add(new Object(769, 1, false, -1, 0), new int[]
			{
				100,
				10
			});
			this.dailyShadowStock.Add(new Object(768, 1, false, -1, 0), new int[]
			{
				80,
				10
			});
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			switch (dayOfMonth % 7)
			{
			case 0:
				this.dailyShadowStock.Add(new Object(767, 1, false, -1, 0), new int[]
				{
					30,
					10
				});
				break;
			case 1:
				this.dailyShadowStock.Add(new Object(766, 1, false, -1, 0), new int[]
				{
					10,
					50
				});
				break;
			case 2:
				this.dailyShadowStock.Add(new Object(749, 1, false, -1, 0), new int[]
				{
					300,
					1
				});
				break;
			case 3:
				this.dailyShadowStock.Add(new Object(r.Next(698, 709), 1, false, -1, 0), new int[]
				{
					200,
					5
				});
				break;
			case 4:
				this.dailyShadowStock.Add(new Object(770, 1, false, -1, 0), new int[]
				{
					30,
					10
				});
				break;
			case 5:
				this.dailyShadowStock.Add(new Object(645, 1, false, -1, 0), new int[]
				{
					10000,
					1
				});
				break;
			case 6:
			{
				int index = r.Next(194, 245);
				if (index == 217)
				{
					index = 216;
				}
				this.dailyShadowStock.Add(new Object(index, 1, false, -1, 0), new int[]
				{
					r.Next(5, 51) * 10,
					5
				});
				break;
			}
			}
			this.dailyShadowStock.Add(new Object(305, 1, false, -1, 0), new int[]
			{
				5000,
				2147483647
			});
			if (!Game1.player.hasOrWillReceiveMail("CF_Sewer"))
			{
				this.dailyShadowStock.Add(new Object(434, 1, false, -1, 0), new int[]
				{
					20000,
					1
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Crystal Floor"))
			{
				this.dailyShadowStock.Add(new Object(333, 1, true, -1, 0), new int[]
				{
					500,
					1
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Wicked Statue"))
			{
				this.dailyShadowStock.Add(new Object(Vector2.Zero, 83, true), new int[]
				{
					1000,
					1
				});
			}
			if (!Game1.player.hasOrWillReceiveMail("ReturnScepter"))
			{
				this.dailyShadowStock.Add(new Wand(), new int[]
				{
					2000000,
					1
				});
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0015613C File Offset: 0x0015433C
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			for (float x = -512f + this.steamPosition.X; x < (float)Game1.graphics.GraphicsDevice.Viewport.Width + 256f; x += 256f)
			{
				for (float y = -256f + this.steamPosition.Y; y < (float)(Game1.graphics.GraphicsDevice.Viewport.Height + 128); y += 256f)
				{
					b.Draw(this.steamAnimation, new Vector2(x, y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)), this.steamColor * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00156218 File Offset: 0x00154418
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			this.steamPosition.Y = this.steamPosition.Y - (float)time.ElapsedGameTime.Milliseconds * 0.1f;
			this.steamPosition.Y = this.steamPosition.Y % -256f;
			this.steamPosition -= Game1.getMostRecentViewportMotion();
			if (Game1.random.NextDouble() < 0.001)
			{
				Game1.playSound("cavedrip");
			}
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0015629C File Offset: 0x0015449C
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int tileIndexOfCheckLocation = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (tileIndexOfCheckLocation == 21)
			{
				Game1.warpFarmer("Town", 35, 97, 2);
				DelayedAction.playSoundAfterDelay("stairsdown", 250);
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00156318 File Offset: 0x00154518
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.waterColor = Color.LimeGreen * 0.75f;
			this.characters.Clear();
			this.characters.Add(this.Krobus);
			Game1.temporaryContent = Game1.content.CreateTemporary();
			this.steamPosition = new Vector2(0f, 0f);
			this.steamAnimation = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\steamAnimation");
			Game1.ambientLight = new Color(250, 140, 160);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x001563B0 File Offset: 0x001545B0
		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (!who.fishCaught.ContainsKey(682) && Game1.random.NextDouble() < 0.1 + ((who.getTileX() > 14 && who.getTileY() > 42) ? 0.08 : 0.0))
			{
				return new Object(682, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0015642C File Offset: 0x0015462C
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.temporaryContent.Unload();
			Game1.temporaryContent = null;
			Game1.changeMusicTrack("none");
		}

		// Token: 0x040011E1 RID: 4577
		private NPC Krobus = new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Krobus"), 0, Game1.tileSize / 4, 24), new Vector2(31f, 17f) * (float)Game1.tileSize, "Sewer", 2, "Krobus", false, null, Game1.content.Load<Texture2D>("Portraits\\Krobus"));

		// Token: 0x040011E2 RID: 4578
		private Dictionary<Item, int[]> dailyShadowStock = new Dictionary<Item, int[]>();

		// Token: 0x040011E3 RID: 4579
		public const float steamZoom = 4f;

		// Token: 0x040011E4 RID: 4580
		public const float steamYMotionPerMillisecond = 0.1f;

		// Token: 0x040011E5 RID: 4581
		public const float millisecondsPerSteamFrame = 50f;

		// Token: 0x040011E6 RID: 4582
		private Texture2D steamAnimation;

		// Token: 0x040011E7 RID: 4583
		private Vector2 steamPosition;

		// Token: 0x040011E8 RID: 4584
		private Color steamColor = new Color(200, 255, 200);
	}
}
