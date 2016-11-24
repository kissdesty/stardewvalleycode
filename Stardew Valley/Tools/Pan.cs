using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Tools
{
	// Token: 0x0200005A RID: 90
	public class Pan : Tool
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x000BB033 File Offset: 0x000B9233
		public Pan() : base("Copper Pan", -1, 12, 12, "Use to gather ore from streams.", false, 0)
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000BB04C File Offset: 0x000B924C
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			this.currentParentTileIndex = 12;
			this.indexOfMenuItemView = 12;
			bool overrideCheck = false;
			Rectangle orePanRect = new Rectangle(location.orePanPoint.X * Game1.tileSize - Game1.tileSize, location.orePanPoint.Y * Game1.tileSize - Game1.tileSize, Game1.tileSize * 4, Game1.tileSize * 4);
			if (orePanRect.Contains(x, y) && Utility.distance((float)who.getStandingX(), (float)orePanRect.Center.X, (float)who.getStandingY(), (float)orePanRect.Center.Y) <= (float)(3 * Game1.tileSize))
			{
				overrideCheck = true;
			}
			who.lastClick = Vector2.Zero;
			x = (int)who.GetToolLocation(false).X;
			y = (int)who.GetToolLocation(false).Y;
			who.lastClick = new Vector2((float)x, (float)y);
			Point arg_DD_0 = location.orePanPoint;
			if (!location.orePanPoint.Equals(Point.Zero))
			{
				Rectangle panRect = who.GetBoundingBox();
				if (overrideCheck || panRect.Intersects(orePanRect))
				{
					who.faceDirection(2);
					who.FarmerSprite.animateOnce(303, 50f, 4);
					return;
				}
			}
			who.forceCanMove();
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000BB185 File Offset: 0x000B9385
		public static void playSlosh(Farmer who)
		{
			Game1.playSound("slosh");
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000BB194 File Offset: 0x000B9394
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			x = (int)who.GetToolLocation(false).X;
			y = (int)who.GetToolLocation(false).Y;
			this.currentParentTileIndex = 12;
			this.indexOfMenuItemView = 12;
			Game1.playSound("coin");
			who.addItemsByMenuIfNecessary(this.getPanItems(location, who), null);
			location.orePanPoint = Point.Zero;
			location.orePanAnimation = null;
			who.CanMove = true;
			who.usingTool = false;
			who.canReleaseTool = true;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000BB223 File Offset: 0x000B9423
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			this.indexOfMenuItemView = 12;
			base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x000BB23C File Offset: 0x000B943C
		public List<Item> getPanItems(GameLocation location, Farmer who)
		{
			List<Item> items = new List<Item>();
			int whichOre = 378;
			int whichExtra = -1;
			Random r = new Random(location.orePanPoint.X + location.orePanPoint.Y * 1000 + (int)Game1.stats.DaysPlayed);
			double roll = r.NextDouble() - (double)who.luckLevel * 0.001 - Game1.dailyLuck;
			if (roll < 0.01)
			{
				whichOre = 386;
			}
			else if (roll < 0.241)
			{
				whichOre = 384;
			}
			else if (roll < 0.6)
			{
				whichOre = 380;
			}
			int orePieces = r.Next(5) + 1 + (int)((r.NextDouble() + 0.1 + (double)((float)who.luckLevel / 10f) + Game1.dailyLuck) * 2.0);
			int extraPieces = r.Next(5) + 1 + (int)((r.NextDouble() + 0.1 + (double)((float)who.luckLevel / 10f)) * 2.0);
			roll = r.NextDouble() - Game1.dailyLuck;
			if (roll < 0.4 + (double)who.LuckLevel * 0.04)
			{
				roll = r.NextDouble() - Game1.dailyLuck;
				whichExtra = 382;
				if (roll < 0.02 + (double)who.LuckLevel * 0.002)
				{
					whichExtra = 72;
					extraPieces = 1;
				}
				else if (roll < 0.1)
				{
					whichExtra = 60 + r.Next(5) * 2;
					extraPieces = 1;
				}
				else if (roll < 0.36)
				{
					whichExtra = 749;
					extraPieces = Math.Max(1, extraPieces / 2);
				}
				else if (roll < 0.5)
				{
					whichExtra = ((r.NextDouble() < 0.3) ? 82 : ((r.NextDouble() < 0.5) ? 84 : 86));
					extraPieces = 1;
				}
			}
			items.Add(new Object(whichOre, orePieces, false, -1, 0));
			if (whichExtra != -1)
			{
				items.Add(new Object(whichExtra, extraPieces, false, -1, 0));
			}
			return items;
		}
	}
}
