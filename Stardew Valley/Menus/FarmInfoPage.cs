using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;

namespace StardewValley.Menus
{
	// Token: 0x020000EE RID: 238
	public class FarmInfoPage : IClickableMenu
	{
		// Token: 0x06000E56 RID: 3670 RVA: 0x00124278 File Offset: 0x00122478
		public FarmInfoPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.moneyIcon = new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2, (Game1.player.Money > 9999) ? 18 : 20, 16), Game1.player.Money + "g", "", Game1.debrisSpriteSheet, new Rectangle(88, 280, 16, 16), 1f, false);
			this.mapX = x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + Game1.tileSize / 2 + Game1.tileSize / 4;
			this.mapY = y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3 - 4;
			this.farmMap = new ClickableTextureComponent(new Rectangle(this.mapX, this.mapY, 20, 20), Game1.content.Load<Texture2D>("LooseSprites\\farmMap"), Rectangle.Empty, 1f, false);
			int numChickens = 0;
			int numDucks = 0;
			int numRabbits = 0;
			int numOther = 0;
			int numCows = 0;
			int numSheep = 0;
			int numGoats = 0;
			int numPigs = 0;
			int chickenHeart = 0;
			int rabbitHeart = 0;
			int duckHeart = 0;
			int otherHeart = 0;
			int cowHeart = 0;
			int goatHeart = 0;
			int sheepHeart = 0;
			int pigHeart = 0;
			this.farm = (Farm)Game1.getLocationFromName("Farm");
			this.farmHouse = new ClickableTextureComponent("FarmHouse", new Rectangle(this.mapX + 443, this.mapY + 43, 80, 72), "FarmHouse", "", Game1.content.Load<Texture2D>("Buildings\\houses"), new Rectangle(0, 0, 160, 144), 0.5f, false);
			foreach (FarmAnimal a in this.farm.getAllFarmAnimals())
			{
				if (a.type.Contains("Chicken"))
				{
					numChickens++;
					chickenHeart += a.friendshipTowardFarmer;
				}
				else
				{
					string type = a.type;
					if (!(type == "Cow"))
					{
						if (!(type == "Duck"))
						{
							if (!(type == "Rabbit"))
							{
								if (!(type == "Sheep"))
								{
									if (!(type == "Goat"))
									{
										if (!(type == "Pig"))
										{
											numOther++;
											otherHeart += a.friendshipTowardFarmer;
										}
										else
										{
											numPigs++;
											pigHeart += a.friendshipTowardFarmer;
										}
									}
									else
									{
										numGoats++;
										goatHeart += a.friendshipTowardFarmer;
									}
								}
								else
								{
									numSheep++;
									sheepHeart += a.friendshipTowardFarmer;
								}
							}
							else
							{
								numRabbits++;
								rabbitHeart += a.friendshipTowardFarmer;
							}
						}
						else
						{
							numDucks++;
							duckHeart += a.friendshipTowardFarmer;
						}
					}
					else
					{
						numCows++;
						cowHeart += a.friendshipTowardFarmer;
					}
				}
			}
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize, Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numChickens), "Chickens" + ((numChickens > 0) ? (Environment.NewLine + "Avg. <: " + chickenHeart / numChickens) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numDucks), "Ducks" + ((numDucks > 0) ? (Environment.NewLine + "Avg. <: " + duckHeart / numDucks) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 2 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numRabbits), "Rabbits" + ((numRabbits > 0) ? (Environment.NewLine + "Avg. <: " + rabbitHeart / numRabbits) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 3 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numCows), "Cows" + ((numCows > 0) ? (Environment.NewLine + "Avg. <: " + cowHeart / numCows) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 4 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numGoats), "Goats" + ((numGoats > 0) ? (Environment.NewLine + "Avg. <: " + goatHeart / numGoats) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 5 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numSheep), "Sheep" + ((numSheep > 0) ? (Environment.NewLine + "Avg. <: " + sheepHeart / numSheep) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 6 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numPigs), "Pigs" + ((numPigs > 0) ? (Environment.NewLine + "Avg. <: " + pigHeart / numPigs) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 5, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 7 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(numOther), "???" + ((numOther > 0) ? (Environment.NewLine + "Avg. <: " + otherHeart / numOther) : ""), Game1.mouseCursors, new Rectangle(Game1.tileSize * 4 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 8 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(Game1.stats.CropsShipped), "Crops Shipped", Game1.mouseCursors, new Rectangle(Game1.tileSize * 7 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 9 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat(this.farm.buildings.Count), "Buildings", Game1.mouseCursors, new Rectangle(Game1.tileSize * 7, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
			int mapTileSize = 8;
			foreach (Building b in this.farm.buildings)
			{
				this.mapBuildings.Add(new ClickableTextureComponent("", new Rectangle(this.mapX + b.tileX * mapTileSize, this.mapY + b.tileY * mapTileSize + (b.tilesHigh + 1) * mapTileSize - (int)((float)b.texture.Height / 8f), b.tilesWide * mapTileSize, (int)((float)b.texture.Height / 8f)), "", b.buildingType, b.texture, b.getSourceRectForMenu(), 0.125f, false));
			}
			foreach (KeyValuePair<Vector2, TerrainFeature> kvp in this.farm.terrainFeatures)
			{
				this.mapFeatures.Add(new MiniatureTerrainFeature(kvp.Value, new Vector2(kvp.Key.X * (float)mapTileSize + (float)this.mapX, kvp.Key.Y * (float)mapTileSize + (float)this.mapY), kvp.Key, 0.125f));
			}
			if (Game1.currentLocation.GetType() == typeof(Farm))
			{
				this.mapFarmer = new ClickableTextureComponent("", new Rectangle(this.mapX + (int)(Game1.player.Position.X / 8f), this.mapY + (int)(Game1.player.position.Y / 8f), 8, 12), "", Game1.player.name, null, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize * 3 / 2), 0.125f, false);
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00124F10 File Offset: 0x00123110
		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			foreach (ClickableTextureComponent c in this.animals)
			{
				if (c.containsPoint(x, y))
				{
					this.hoverText = c.hoverText;
					return;
				}
			}
			foreach (ClickableTextureComponent b in this.mapBuildings)
			{
				if (b.containsPoint(x, y))
				{
					this.hoverText = b.hoverText;
					return;
				}
			}
			if (this.mapFarmer != null && this.mapFarmer.containsPoint(x, y))
			{
				this.hoverText = this.mapFarmer.hoverText;
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00125008 File Offset: 0x00123208
		public override void draw(SpriteBatch b)
		{
			base.drawVerticalPartition(b, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, false);
			this.moneyIcon.draw(b);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.animals.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			this.farmMap.draw(b);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.mapBuildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			this.farmMap.draw(b);
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.mapBuildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			using (List<MiniatureTerrainFeature>.Enumerator enumerator2 = this.mapFeatures.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.draw(b);
				}
			}
			this.farmHouse.draw(b);
			if (this.mapFarmer != null)
			{
				Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float)(this.mapFarmer.bounds.X - 16), (float)(this.mapFarmer.bounds.Y - 16)), 0.99f, 2f, 2, Game1.player);
			}
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.farm.animals)
			{
				b.Draw(kvp.Value.sprite.Texture, new Vector2((float)(this.mapX + (int)(kvp.Value.position.X / 8f)), (float)(this.mapY + (int)(kvp.Value.position.Y / 8f))), new Rectangle?(kvp.Value.sprite.SourceRect), Color.White, 0f, Vector2.Zero, 0.125f, SpriteEffects.None, 0.86f + kvp.Value.position.Y / 8f / 20000f + 0.0125f);
			}
			foreach (KeyValuePair<Vector2, Object> kvp2 in this.farm.objects)
			{
				kvp2.Value.drawInMenu(b, new Vector2((float)this.mapX + kvp2.Key.X * 8f, (float)this.mapY + kvp2.Key.Y * 8f), 0.125f, 1f, 0.86f + ((float)this.mapY + kvp2.Key.Y * 8f - 25f) / 20000f);
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04000F47 RID: 3911
		private string descriptionText = "";

		// Token: 0x04000F48 RID: 3912
		private string hoverText = "";

		// Token: 0x04000F49 RID: 3913
		private ClickableTextureComponent moneyIcon;

		// Token: 0x04000F4A RID: 3914
		private ClickableTextureComponent farmMap;

		// Token: 0x04000F4B RID: 3915
		private ClickableTextureComponent mapFarmer;

		// Token: 0x04000F4C RID: 3916
		private ClickableTextureComponent farmHouse;

		// Token: 0x04000F4D RID: 3917
		private List<ClickableTextureComponent> animals = new List<ClickableTextureComponent>();

		// Token: 0x04000F4E RID: 3918
		private List<ClickableTextureComponent> mapBuildings = new List<ClickableTextureComponent>();

		// Token: 0x04000F4F RID: 3919
		private List<MiniatureTerrainFeature> mapFeatures = new List<MiniatureTerrainFeature>();

		// Token: 0x04000F50 RID: 3920
		private Farm farm;

		// Token: 0x04000F51 RID: 3921
		private int mapX;

		// Token: 0x04000F52 RID: 3922
		private int mapY;
	}
}
