using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200000D RID: 13
	public class BluePrint
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000066DC File Offset: 0x000048DC
		public BluePrint(string name)
		{
			this.name = name;
			if (name.Equals("Info Tool"))
			{
				this.texture = Game1.content.Load<Texture2D>("LooseSprites\\Cursors");
				this.description = "Use to see information about your animals.";
				this.sourceRectForMenuView = new Rectangle(9 * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			Dictionary<string, string> arg_83_0 = Game1.content.Load<Dictionary<string, string>>("Data\\Blueprints");
			string rawData = null;
			arg_83_0.TryGetValue(name, out rawData);
			if (rawData != null)
			{
				string[] split = rawData.Split(new char[]
				{
					'/'
				});
				if (split[0].Equals("animal"))
				{
					try
					{
						this.texture = Game1.content.Load<Texture2D>("Animals\\" + (name.Equals("Chicken") ? "White Chicken" : name));
					}
					catch (Exception)
					{
						Game1.debugOutput = "Blueprint loaded with no texture!";
					}
					this.moneyRequired = Convert.ToInt32(split[1]);
					this.sourceRectForMenuView = new Rectangle(0, 0, Convert.ToInt32(split[2]), Convert.ToInt32(split[3]));
					this.blueprintType = "Animals";
					this.tilesWidth = 1;
					this.tilesHeight = 1;
					this.description = split[4];
					this.humanDoor = new Point(-1, -1);
					this.animalDoor = new Point(-1, -1);
					return;
				}
				try
				{
					this.texture = Game1.content.Load<Texture2D>("Buildings\\" + name);
				}
				catch (Exception)
				{
				}
				string[] recipeSplit = split[0].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < recipeSplit.Length; i += 2)
				{
					if (!recipeSplit[i].Equals(""))
					{
						this.itemsRequired.Add(Convert.ToInt32(recipeSplit[i]), Convert.ToInt32(recipeSplit[i + 1]));
					}
				}
				this.tilesWidth = Convert.ToInt32(split[1]);
				this.tilesHeight = Convert.ToInt32(split[2]);
				this.humanDoor = new Point(Convert.ToInt32(split[3]), Convert.ToInt32(split[4]));
				this.animalDoor = new Point(Convert.ToInt32(split[5]), Convert.ToInt32(split[6]));
				this.mapToWarpTo = split[7];
				this.description = split[8];
				this.blueprintType = split[9];
				if (this.blueprintType.Equals("Upgrades"))
				{
					this.nameOfBuildingToUpgrade = split[10];
				}
				this.sourceRectForMenuView = new Rectangle(0, 0, Convert.ToInt32(split[11]), Convert.ToInt32(split[12]));
				this.maxOccupants = Convert.ToInt32(split[13]);
				this.actionBehavior = split[14];
				string[] array = split[15].Split(new char[]
				{
					' '
				});
				for (int j = 0; j < array.Length; j++)
				{
					string s = array[j];
					this.namesOfOkayBuildingLocations.Add(s);
				}
				if (split.Length > 16)
				{
					this.moneyRequired = Convert.ToInt32(split[16]);
				}
				if (split.Length > 17)
				{
					this.magical = Convert.ToBoolean(split[17]);
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000069F8 File Offset: 0x00004BF8
		public void consumeResources()
		{
			foreach (KeyValuePair<int, int> kvp in this.itemsRequired)
			{
				Game1.player.consumeObject(kvp.Key, kvp.Value);
			}
			Game1.player.Money -= this.moneyRequired;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006A74 File Offset: 0x00004C74
		public int getTileSheetIndexForStructurePlacementTile(int x, int y)
		{
			if (x == this.humanDoor.X && y == this.humanDoor.Y)
			{
				return 2;
			}
			if (x == this.animalDoor.X && y == this.animalDoor.Y)
			{
				return 4;
			}
			return 0;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006AB3 File Offset: 0x00004CB3
		public bool isUpgrade()
		{
			return this.nameOfBuildingToUpgrade != null && this.nameOfBuildingToUpgrade.Length > 0;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006AD0 File Offset: 0x00004CD0
		public bool doesFarmerHaveEnoughResourcesToBuild()
		{
			foreach (KeyValuePair<int, int> kvp in this.itemsRequired)
			{
				if (!Game1.player.hasItemInInventory(kvp.Key, kvp.Value, 0))
				{
					return false;
				}
			}
			return Game1.player.Money >= this.moneyRequired;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006B54 File Offset: 0x00004D54
		public void drawDescription(SpriteBatch b, int x, int y, int width)
		{
			b.DrawString(Game1.smallFont, this.name, new Vector2((float)x, (float)y), Game1.textColor);
			string descriptionString = Game1.parseText(this.description, Game1.smallFont, width);
			b.DrawString(Game1.smallFont, descriptionString, new Vector2((float)x, (float)y + Game1.smallFont.MeasureString(this.name).Y), Game1.textColor * 0.75f);
			int yPosition = (int)((float)y + Game1.smallFont.MeasureString(this.name).Y + Game1.smallFont.MeasureString(descriptionString).Y);
			foreach (KeyValuePair<int, int> kvp in this.itemsRequired)
			{
				b.Draw(Game1.objectSpriteSheet, new Vector2((float)(x + Game1.tileSize / 8), (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, kvp.Key, 16, 16)), Color.White, 0f, new Vector2(6f, 3f), (float)Game1.pixelZoom * 0.5f, SpriteEffects.None, 0.999f);
				Color colorToDrawResource = Game1.player.hasItemInInventory(kvp.Key, kvp.Value, 0) ? Color.DarkGreen : Color.DarkRed;
				Game1.drawWithBorder(string.Concat(kvp.Value), Game1.textColor, Color.AntiqueWhite, new Vector2((float)(x + Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat(kvp.Value)).X, (float)(yPosition + Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat(kvp.Value)).Y), 0f, 1f, 0.9f, true);
				b.DrawString(Game1.smallFont, Game1.objectInformation[kvp.Key].Split(new char[]
				{
					'/'
				})[0], new Vector2((float)(x + Game1.tileSize / 2 + Game1.tileSize / 4), (float)yPosition), colorToDrawResource);
				yPosition += (int)Game1.smallFont.MeasureString("P").Y;
			}
			if (this.moneyRequired > 0)
			{
				b.Draw(Game1.debrisSpriteSheet, new Vector2((float)x, (float)yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 8), (float)(Game1.tileSize / 2 - Game1.tileSize / 3)), 0.5f, SpriteEffects.None, 0.999f);
				Color colorToDrawResource2 = (Game1.player.money >= this.moneyRequired) ? Color.DarkGreen : Color.DarkRed;
				b.DrawString(Game1.smallFont, this.moneyRequired + "g", new Vector2((float)(x + Game1.tileSize / 4 + Game1.tileSize / 8), (float)yPosition), colorToDrawResource2);
				yPosition += (int)Game1.smallFont.MeasureString(string.Concat(this.moneyRequired)).Y;
			}
		}

		// Token: 0x04000074 RID: 116
		public string name;

		// Token: 0x04000075 RID: 117
		public int woodRequired;

		// Token: 0x04000076 RID: 118
		public int stoneRequired;

		// Token: 0x04000077 RID: 119
		public int copperRequired;

		// Token: 0x04000078 RID: 120
		public int IronRequired;

		// Token: 0x04000079 RID: 121
		public int GoldRequired;

		// Token: 0x0400007A RID: 122
		public int IridiumRequired;

		// Token: 0x0400007B RID: 123
		public int tilesWidth;

		// Token: 0x0400007C RID: 124
		public int tilesHeight;

		// Token: 0x0400007D RID: 125
		public int maxOccupants;

		// Token: 0x0400007E RID: 126
		public int moneyRequired;

		// Token: 0x0400007F RID: 127
		public Point humanDoor;

		// Token: 0x04000080 RID: 128
		public Point animalDoor;

		// Token: 0x04000081 RID: 129
		public string mapToWarpTo;

		// Token: 0x04000082 RID: 130
		public string description;

		// Token: 0x04000083 RID: 131
		public string blueprintType;

		// Token: 0x04000084 RID: 132
		public string nameOfBuildingToUpgrade;

		// Token: 0x04000085 RID: 133
		public string actionBehavior;

		// Token: 0x04000086 RID: 134
		public Texture2D texture;

		// Token: 0x04000087 RID: 135
		public List<string> namesOfOkayBuildingLocations = new List<string>();

		// Token: 0x04000088 RID: 136
		public Rectangle sourceRectForMenuView;

		// Token: 0x04000089 RID: 137
		public Dictionary<int, int> itemsRequired = new Dictionary<int, int>();

		// Token: 0x0400008A RID: 138
		public bool canBuildOnCurrentMap;

		// Token: 0x0400008B RID: 139
		public bool magical;
	}
}
