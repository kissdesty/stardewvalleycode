using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000E4 RID: 228
	public class CollectionsPage : IClickableMenu
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x0011C544 File Offset: 0x0011A744
		public CollectionsPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4 + CollectionsPage.widthToMoveActiveTab, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Shipped", new object[0]), Game1.mouseCursors, new Rectangle(640, 80, 16, 16), (float)Game1.pixelZoom, false));
			this.collections.Add(0, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 3, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Fish", new object[0]), Game1.mouseCursors, new Rectangle(640, 64, 16, 16), (float)Game1.pixelZoom, false));
			this.collections.Add(1, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 4, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Artifacts", new object[0]), Game1.mouseCursors, new Rectangle(656, 64, 16, 16), (float)Game1.pixelZoom, false));
			this.collections.Add(2, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 5, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Minerals", new object[0]), Game1.mouseCursors, new Rectangle(672, 64, 16, 16), (float)Game1.pixelZoom, false));
			this.collections.Add(3, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 6, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Cooking", new object[0]), Game1.mouseCursors, new Rectangle(688, 64, 16, 16), (float)Game1.pixelZoom, false));
			this.collections.Add(4, new List<List<ClickableTextureComponent>>());
			this.sideTabs.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen - Game1.tileSize * 3 / 4, this.yPositionOnScreen + Game1.tileSize * 7, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:Collections_Achievements", new object[0]), Game1.mouseCursors, new Rectangle(656, 80, 16, 16), (float)Game1.pixelZoom, false));
			this.collections.Add(5, new List<List<ClickableTextureComponent>>());
			CollectionsPage.widthToMoveActiveTab = Game1.tileSize / 8;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + height - 20 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width - Game1.tileSize / 2 - 15 * Game1.pixelZoom, this.yPositionOnScreen + height - 20 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			int[] widthUsed = new int[this.sideTabs.Count];
			int baseX = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder;
			int baseY = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4;
			int collectionWidth = 10;
			foreach (KeyValuePair<int, string> kvp in Game1.objectInformation)
			{
				string typeString = kvp.Value.Split(new char[]
				{
					'/'
				})[3];
				bool farmerHas = false;
				int whichCollection;
				if (typeString.Contains("Arch"))
				{
					whichCollection = 2;
					if (Game1.player.archaeologyFound.ContainsKey(kvp.Key))
					{
						farmerHas = true;
					}
				}
				else if (typeString.Contains("Fish"))
				{
					if (kvp.Key >= 167 && kvp.Key < 173)
					{
						continue;
					}
					whichCollection = 1;
					if (Game1.player.fishCaught.ContainsKey(kvp.Key))
					{
						farmerHas = true;
					}
				}
				else if (typeString.Contains("Mineral") || typeString.Substring(typeString.Length - 3).Equals("-2"))
				{
					whichCollection = 3;
					if (Game1.player.mineralsFound.ContainsKey(kvp.Key))
					{
						farmerHas = true;
					}
				}
				else if (typeString.Contains("Cooking") || typeString.Substring(typeString.Length - 3).Equals("-7"))
				{
					whichCollection = 4;
					if (Game1.player.recipesCooked.ContainsKey(kvp.Key))
					{
						farmerHas = true;
					}
					if (kvp.Key == 217 || kvp.Key == 772)
					{
						continue;
					}
					if (kvp.Key == 773)
					{
						continue;
					}
				}
				else
				{
					if (!Object.isPotentialBasicShippedCategory(kvp.Key, typeString.Substring(typeString.Length - 3)))
					{
						continue;
					}
					whichCollection = 0;
					if (Game1.player.basicShipped.ContainsKey(kvp.Key))
					{
						farmerHas = true;
					}
				}
				int xPos = baseX + widthUsed[whichCollection] % collectionWidth * (Game1.tileSize + 4);
				int yPos = baseY + widthUsed[whichCollection] / collectionWidth * (Game1.tileSize + 4);
				if (yPos > this.yPositionOnScreen + height - 128)
				{
					this.collections[whichCollection].Add(new List<ClickableTextureComponent>());
					widthUsed[whichCollection] = 0;
					xPos = baseX;
					yPos = baseY;
				}
				if (this.collections[whichCollection].Count == 0)
				{
					this.collections[whichCollection].Add(new List<ClickableTextureComponent>());
				}
				this.collections[whichCollection].Last<List<ClickableTextureComponent>>().Add(new ClickableTextureComponent(kvp.Key + " " + farmerHas.ToString(), new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), null, "", Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, kvp.Key, 16, 16), (float)Game1.pixelZoom, farmerHas));
				widthUsed[whichCollection]++;
			}
			if (this.collections[5].Count == 0)
			{
				this.collections[5].Add(new List<ClickableTextureComponent>());
			}
			foreach (KeyValuePair<int, string> kvp2 in Game1.achievements)
			{
				bool farmerHas2 = Game1.player.achievements.Contains(kvp2.Key);
				string[] split = kvp2.Value.Split(new char[]
				{
					'^'
				});
				if (farmerHas2 || (split[2].Equals("true") && (split[3].Equals("-1") || this.farmerHasAchievements(split[3]))))
				{
					int xPos2 = baseX + widthUsed[5] % collectionWidth * (Game1.tileSize + 4);
					int yPos2 = baseY + widthUsed[5] / collectionWidth * (Game1.tileSize + 4);
					this.collections[5][0].Add(new ClickableTextureComponent(kvp2.Key + " " + farmerHas2.ToString(), new Rectangle(xPos2, yPos2, Game1.tileSize, Game1.tileSize), null, "", Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 25, -1, -1), 1f, false));
					widthUsed[5]++;
				}
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0011CE60 File Offset: 0x0011B060
		private bool farmerHasAchievements(string listOfAchievementNumbers)
		{
			string[] array = listOfAchievementNumbers.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string s = array[i];
				if (!Game1.player.achievements.Contains(Convert.ToInt32(s)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0011CEAC File Offset: 0x0011B0AC
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			for (int i = 0; i < this.sideTabs.Count; i++)
			{
				if (this.sideTabs[i].containsPoint(x, y) && this.currentTab != i)
				{
					Game1.playSound("smallSelect");
					ClickableTextureComponent expr_47_cp_0_cp_0 = this.sideTabs[this.currentTab];
					expr_47_cp_0_cp_0.bounds.X = expr_47_cp_0_cp_0.bounds.X - CollectionsPage.widthToMoveActiveTab;
					this.currentTab = i;
					this.currentPage = 0;
					ClickableTextureComponent expr_74_cp_0_cp_0 = this.sideTabs[i];
					expr_74_cp_0_cp_0.bounds.X = expr_74_cp_0_cp_0.bounds.X + CollectionsPage.widthToMoveActiveTab;
				}
			}
			if (this.currentPage > 0 && this.backButton.containsPoint(x, y))
			{
				this.currentPage--;
				Game1.playSound("shwip");
				this.backButton.scale = this.backButton.baseScale;
			}
			if (this.currentPage < this.collections[this.currentTab].Count - 1 && this.forwardButton.containsPoint(x, y))
			{
				this.currentPage++;
				Game1.playSound("shwip");
				this.forwardButton.scale = this.forwardButton.baseScale;
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0011CFF0 File Offset: 0x0011B1F0
		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			this.value = -1;
			foreach (ClickableTextureComponent c in this.sideTabs)
			{
				if (c.containsPoint(x, y))
				{
					this.hoverText = c.hoverText;
					return;
				}
			}
			foreach (ClickableTextureComponent c2 in this.collections[this.currentTab][this.currentPage])
			{
				if (c2.containsPoint(x, y))
				{
					c2.scale = Math.Min(c2.scale + 0.02f, c2.baseScale + 0.1f);
					if (Convert.ToBoolean(c2.name.Split(new char[]
					{
						' '
					})[1]) || this.currentTab == 5)
					{
						this.hoverText = this.createDescription(Convert.ToInt32(c2.name.Split(new char[]
						{
							' '
						})[0]));
					}
					else
					{
						this.hoverText = "???";
					}
				}
				else
				{
					c2.scale = Math.Max(c2.scale - 0.02f, c2.baseScale);
				}
			}
			this.forwardButton.tryHover(x, y, 0.5f);
			this.backButton.tryHover(x, y, 0.5f);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0011D19C File Offset: 0x0011B39C
		public string createDescription(int index)
		{
			string description = "";
			if (this.currentTab == 5)
			{
				string[] split = Game1.achievements[index].Split(new char[]
				{
					'^'
				});
				description = description + split[0] + Environment.NewLine + Environment.NewLine;
				description += split[1];
			}
			else
			{
				string[] split2 = Game1.objectInformation[index].Split(new char[]
				{
					'/'
				});
				description = string.Concat(new string[]
				{
					description,
					split2[0],
					Environment.NewLine,
					Environment.NewLine,
					Game1.parseText(split2[4], Game1.smallFont, Game1.tileSize * 4),
					Environment.NewLine,
					Environment.NewLine
				});
				if (split2[3].Contains("Arch"))
				{
					description += (Game1.player.archaeologyFound.ContainsKey(index) ? Game1.content.LoadString("Strings\\UI:Collections_Description_ArtifactsFound", new object[]
					{
						Game1.player.archaeologyFound[index][0]
					}) : "");
				}
				else if (split2[3].Contains("Cooking"))
				{
					description += (Game1.player.recipesCooked.ContainsKey(index) ? Game1.content.LoadString("Strings\\UI:Collections_Description_RecipesCooked", new object[]
					{
						Game1.player.recipesCooked[index]
					}) : "");
				}
				else if (split2[3].Contains("Fish"))
				{
					description += Game1.content.LoadString("Strings\\UI:Collections_Description_FishCaught", new object[]
					{
						Game1.player.fishCaught.ContainsKey(index) ? Game1.player.fishCaught[index][0] : 0
					});
					if (Game1.player.fishCaught.ContainsKey(index) && Game1.player.fishCaught[index][1] > 0)
					{
						description = description + Environment.NewLine + Game1.content.LoadString("Strings\\UI:Collections_Description_BiggestCatch", new object[]
						{
							Game1.player.fishCaught[index][1]
						});
					}
				}
				else if (split2[3].Contains("Minerals") || split2[3].Substring(split2[3].Length - 3).Equals("-2"))
				{
					description += Game1.content.LoadString("Strings\\UI:Collections_Description_MineralsFound", new object[]
					{
						Game1.player.mineralsFound.ContainsKey(index) ? Game1.player.mineralsFound[index] : 0
					});
				}
				else
				{
					description += Game1.content.LoadString("Strings\\UI:Collections_Description_NumberShipped", new object[]
					{
						Game1.player.basicShipped.ContainsKey(index) ? Game1.player.basicShipped[index] : 0
					});
				}
				this.value = Convert.ToInt32(split2[1]);
			}
			return description;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0011D4C0 File Offset: 0x0011B6C0
		public override void draw(SpriteBatch b)
		{
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.sideTabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			if (this.currentPage > 0)
			{
				this.backButton.draw(b);
			}
			if (this.currentPage < this.collections[this.currentTab].Count - 1)
			{
				this.forwardButton.draw(b);
			}
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
			foreach (ClickableTextureComponent c in this.collections[this.currentTab][this.currentPage])
			{
				bool drawColor = Convert.ToBoolean(c.name.Split(new char[]
				{
					' '
				})[1]);
				c.draw(b, drawColor ? Color.White : (Color.Black * 0.2f), 0.86f);
				if (this.currentTab == 5 & drawColor)
				{
					int StarPos = new Random(Convert.ToInt32(c.name.Split(new char[]
					{
						' '
					})[0])).Next(12);
					b.Draw(Game1.mouseCursors, new Vector2((float)(c.bounds.X + 16 + Game1.tileSize / 4), (float)(c.bounds.Y + 20 + Game1.tileSize / 4)), new Rectangle?(new Rectangle(256 + StarPos % 6 * Game1.tileSize / 2, 128 + StarPos / 6 * Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), c.scale, SpriteEffects.None, 0.88f);
				}
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, this.value, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04000ED3 RID: 3795
		public static int widthToMoveActiveTab = Game1.tileSize / 8;

		// Token: 0x04000ED4 RID: 3796
		public const int organicsTab = 0;

		// Token: 0x04000ED5 RID: 3797
		public const int fishTab = 1;

		// Token: 0x04000ED6 RID: 3798
		public const int archaeologyTab = 2;

		// Token: 0x04000ED7 RID: 3799
		public const int mineralsTab = 3;

		// Token: 0x04000ED8 RID: 3800
		public const int cookingTab = 4;

		// Token: 0x04000ED9 RID: 3801
		public const int achievementsTab = 5;

		// Token: 0x04000EDA RID: 3802
		public const int distanceFromMenuBottomBeforeNewPage = 128;

		// Token: 0x04000EDB RID: 3803
		private string descriptionText = "";

		// Token: 0x04000EDC RID: 3804
		private string hoverText = "";

		// Token: 0x04000EDD RID: 3805
		private ClickableTextureComponent backButton;

		// Token: 0x04000EDE RID: 3806
		private ClickableTextureComponent forwardButton;

		// Token: 0x04000EDF RID: 3807
		private List<ClickableTextureComponent> sideTabs = new List<ClickableTextureComponent>();

		// Token: 0x04000EE0 RID: 3808
		private int currentTab;

		// Token: 0x04000EE1 RID: 3809
		private int currentPage;

		// Token: 0x04000EE2 RID: 3810
		private Dictionary<int, List<List<ClickableTextureComponent>>> collections = new Dictionary<int, List<List<ClickableTextureComponent>>>();

		// Token: 0x04000EE3 RID: 3811
		private int value;
	}
}
