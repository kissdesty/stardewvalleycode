using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x020000E8 RID: 232
	public class CraftingPage : IClickableMenu
	{
		// Token: 0x06000E15 RID: 3605 RVA: 0x0011ED2C File Offset: 0x0011CF2C
		public CraftingPage(int x, int y, int width, int height, bool cooking = false) : base(x, y, width, height, false)
		{
			this.cooking = cooking;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, false, null, null, -1, 3, 0, 0, true);
			this.inventory.showGrayedOutSlots = true;
			int craftingPageX = this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth - Game1.tileSize / 4;
			int craftingPageY = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.tileSize / 4;
			int arg_CE_0 = Game1.tileSize;
			int spaceBetweenCraftingIcons = 8;
			int numInRow = 10;
			int i = -1;
			if (cooking)
			{
				base.initializeUpperRightCloseButton();
			}
			SerializableDictionary<string, int> newRecipeList = new SerializableDictionary<string, int>();
			foreach (string s in CraftingRecipe.craftingRecipes.Keys)
			{
				if (Game1.player.craftingRecipes.ContainsKey(s))
				{
					newRecipeList.Add(s, Game1.player.craftingRecipes[s]);
				}
			}
			Game1.player.craftingRecipes = newRecipeList;
			this.trashCan = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + 4, this.yPositionOnScreen + height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float)Game1.pixelZoom, false);
			List<string> playerRecipes = new List<string>();
			if (!cooking)
			{
				using (Dictionary<string, int>.KeyCollection.Enumerator enumerator2 = Game1.player.craftingRecipes.Keys.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string s2 = enumerator2.Current;
						playerRecipes.Add(new string(s2.ToCharArray()));
					}
					goto IL_262;
				}
			}
			Game1.playSound("bigSelect");
			foreach (string s3 in CraftingRecipe.cookingRecipes.Keys)
			{
				playerRecipes.Add(new string(s3.ToCharArray()));
			}
			IL_262:
			int j = 0;
			while (playerRecipes.Count > 0)
			{
				CraftingRecipe r;
				int whichPage;
				ClickableTextureComponent component;
				bool intersected;
				do
				{
					i++;
					if (i % 40 == 0)
					{
						this.pagesOfCraftingRecipes.Add(new Dictionary<ClickableTextureComponent, CraftingRecipe>());
					}
					int rowNumber = i / numInRow % (40 / numInRow);
					r = new CraftingRecipe(playerRecipes[j], cooking);
					int numRecipes = playerRecipes.Count;
					while (r.bigCraftable && rowNumber == 40 / numInRow - 1 && numRecipes > 0)
					{
						j = (j + 1) % playerRecipes.Count;
						numRecipes--;
						r = new CraftingRecipe(playerRecipes[j], false);
						if (numRecipes == 0)
						{
							i += 40 - i % 40;
							rowNumber = i / numInRow % (40 / numInRow);
							this.pagesOfCraftingRecipes.Add(new Dictionary<ClickableTextureComponent, CraftingRecipe>());
						}
					}
					whichPage = i / 40;
					component = new ClickableTextureComponent("", new Rectangle(craftingPageX + i % numInRow * (Game1.tileSize + spaceBetweenCraftingIcons), craftingPageY + rowNumber * (Game1.tileSize + 8), Game1.tileSize, r.bigCraftable ? (Game1.tileSize * 2) : Game1.tileSize), null, (cooking && !Game1.player.cookingRecipes.ContainsKey(r.name)) ? "ghosted" : "", r.bigCraftable ? Game1.bigCraftableSpriteSheet : Game1.objectSpriteSheet, r.bigCraftable ? Game1.getArbitrarySourceRect(Game1.bigCraftableSpriteSheet, 16, 32, r.getIndexOfMenuView()) : Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, r.getIndexOfMenuView(), 16, 16), (float)Game1.pixelZoom, false);
					intersected = false;
					using (Dictionary<ClickableTextureComponent, CraftingRecipe>.KeyCollection.Enumerator enumerator3 = this.pagesOfCraftingRecipes[whichPage].Keys.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.bounds.Intersects(component.bounds))
							{
								intersected = true;
								break;
							}
						}
					}
				}
				while (intersected);
				this.pagesOfCraftingRecipes[whichPage].Add(component, r);
				playerRecipes.RemoveAt(j);
				j = 0;
			}
			if (this.pagesOfCraftingRecipes.Count > 1)
			{
				this.upButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 12 + Game1.tileSize / 2, craftingPageY, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 12, -1, -1), 0.8f, false);
				this.downButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 12 + Game1.tileSize / 2, craftingPageY + Game1.tileSize * 3 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 11, -1, -1), 0.8f, false);
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0011F290 File Offset: 0x0011D490
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (key.Equals(Keys.Delete) && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is Object && Game1.player.specialItems.Contains((this.heldItem as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0011F330 File Offset: 0x0011D530
		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.currentCraftingPage > 0)
			{
				this.currentCraftingPage--;
				Game1.playSound("shwip");
				return;
			}
			if (direction < 0 && this.currentCraftingPage < this.pagesOfCraftingRecipes.Count - 1)
			{
				this.currentCraftingPage++;
				Game1.playSound("shwip");
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0011F39C File Offset: 0x0011D59C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, true);
			this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
			if (this.upButton != null && this.upButton.containsPoint(x, y))
			{
				if (this.currentCraftingPage > 0)
				{
					Game1.playSound("coin");
				}
				this.currentCraftingPage = Math.Max(0, this.currentCraftingPage - 1);
				this.upButton.scale = this.upButton.baseScale;
			}
			if (this.downButton != null && this.downButton.containsPoint(x, y))
			{
				if (this.currentCraftingPage < this.pagesOfCraftingRecipes.Count - 1)
				{
					Game1.playSound("coin");
				}
				this.currentCraftingPage = Math.Min(this.pagesOfCraftingRecipes.Count - 1, this.currentCraftingPage + 1);
				this.downButton.scale = this.downButton.baseScale;
			}
			foreach (ClickableTextureComponent c in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
			{
				int times = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? 5 : 1;
				for (int i = 0; i < times; i++)
				{
					if (c.containsPoint(x, y) && !c.hoverText.Equals("ghosted") && this.pagesOfCraftingRecipes[this.currentCraftingPage][c].doesFarmerHaveIngredientsInInventory(this.cooking ? Utility.getHomeOfFarmer(Game1.player).fridge.items : null))
					{
						this.clickCraftingRecipe(c, i == 0);
					}
				}
			}
			if (this.trashCan != null && this.trashCan.containsPoint(x, y) && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is Object && Game1.player.specialItems.Contains((this.heldItem as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
				return;
			}
			if (this.heldItem != null && !this.isWithinBounds(x, y) && this.heldItem.canBeTrashed())
			{
				Game1.playSound("throwDownITem");
				Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, null);
				this.heldItem = null;
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0011F648 File Offset: 0x0011D848
		private void clickCraftingRecipe(ClickableTextureComponent c, bool playSound = true)
		{
			Item crafted = this.pagesOfCraftingRecipes[this.currentCraftingPage][c].createItem();
			Game1.player.checkForQuestComplete(null, -1, -1, crafted, null, 2, -1);
			if (this.heldItem == null)
			{
				this.pagesOfCraftingRecipes[this.currentCraftingPage][c].consumeIngredients();
				this.heldItem = crafted;
				if (playSound)
				{
					Game1.playSound("coin");
				}
			}
			else if (this.heldItem.Name.Equals(crafted.Name) && this.heldItem.Stack + this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft - 1 < this.heldItem.maximumStackSize())
			{
				this.heldItem.Stack += this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft;
				this.pagesOfCraftingRecipes[this.currentCraftingPage][c].consumeIngredients();
				if (playSound)
				{
					Game1.playSound("coin");
				}
			}
			if (!this.cooking && Game1.player.craftingRecipes.ContainsKey(this.pagesOfCraftingRecipes[this.currentCraftingPage][c].name))
			{
				SerializableDictionary<string, int> craftingRecipes = Game1.player.craftingRecipes;
				string name = this.pagesOfCraftingRecipes[this.currentCraftingPage][c].name;
				craftingRecipes[name] += this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft;
			}
			if (this.cooking)
			{
				Game1.player.cookedRecipe(this.heldItem.parentSheetIndex);
			}
			if (!this.cooking)
			{
				Game1.stats.checkForCraftingAchievements();
			}
			else
			{
				Game1.stats.checkForCookingAchievements();
			}
			if (Game1.options.gamepadControls && this.heldItem != null && Game1.player.couldInventoryAcceptThisItem(this.heldItem))
			{
				Game1.player.addItemToInventoryBool(this.heldItem, false);
				this.heldItem = null;
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0011F874 File Offset: 0x0011DA74
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
			foreach (ClickableTextureComponent c in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
			{
				if (c.containsPoint(x, y) && !c.hoverText.Equals("ghosted") && this.pagesOfCraftingRecipes[this.currentCraftingPage][c].doesFarmerHaveIngredientsInInventory(this.cooking ? Utility.getHomeOfFarmer(Game1.player).fridge.items : null))
				{
					this.clickCraftingRecipe(c, true);
				}
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0011F94C File Offset: 0x0011DB4C
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverTitle = "";
			this.descriptionText = "";
			this.hoverText = "";
			this.hoverRecipe = null;
			this.hoverItem = this.inventory.hover(x, y, this.hoverItem);
			if (this.hoverItem != null)
			{
				this.hoverTitle = this.inventory.hoverTitle;
				this.hoverText = this.inventory.hoverText;
			}
			foreach (ClickableTextureComponent c in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
			{
				if (c.containsPoint(x, y))
				{
					if (c.hoverText.Equals("ghosted"))
					{
						this.hoverText = "???";
					}
					else
					{
						this.hoverRecipe = this.pagesOfCraftingRecipes[this.currentCraftingPage][c];
						if (this.lastCookingHover == null || !this.lastCookingHover.Name.Equals(this.hoverRecipe.name))
						{
							this.lastCookingHover = this.hoverRecipe.createItem();
						}
						c.scale = Math.Min(c.scale + 0.02f, c.baseScale + 0.1f);
					}
				}
				else
				{
					c.scale = Math.Max(c.scale - 0.02f, c.baseScale);
				}
			}
			if (this.upButton != null)
			{
				if (this.upButton.containsPoint(x, y))
				{
					this.upButton.scale = Math.Min(this.upButton.scale + 0.02f, this.upButton.baseScale + 0.1f);
				}
				else
				{
					this.upButton.scale = Math.Max(this.upButton.scale - 0.02f, this.upButton.baseScale);
				}
			}
			if (this.downButton != null)
			{
				if (this.downButton.containsPoint(x, y))
				{
					this.downButton.scale = Math.Min(this.downButton.scale + 0.02f, this.downButton.baseScale + 0.1f);
				}
				else
				{
					this.downButton.scale = Math.Max(this.downButton.scale - 0.02f, this.downButton.baseScale);
				}
			}
			if (this.trashCan != null)
			{
				if (this.trashCan.containsPoint(x, y))
				{
					if (this.trashCanLidRotation <= 0f)
					{
						Game1.playSound("trashcanlid");
					}
					this.trashCanLidRotation = Math.Min(this.trashCanLidRotation + 0.06544985f, 1.57079637f);
					return;
				}
				this.trashCanLidRotation = Math.Max(this.trashCanLidRotation - 0.06544985f, 0f);
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0011FC3C File Offset: 0x0011DE3C
		public override bool readyToClose()
		{
			return this.heldItem == null;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0011FC48 File Offset: 0x0011DE48
		public override void draw(SpriteBatch b)
		{
			if (this.cooking)
			{
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			}
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
			this.inventory.draw(b);
			if (this.trashCan != null)
			{
				this.trashCan.draw(b);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.trashCan.bounds.X + 60), (float)(this.trashCan.bounds.Y + 40)), new Rectangle?(new Rectangle(686, 256, 18, 10)), Color.White, this.trashCanLidRotation, new Vector2(16f, 10f), (float)Game1.pixelZoom, SpriteEffects.None, 0.86f);
			}
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
			foreach (ClickableTextureComponent c in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
			{
				if (c.hoverText.Equals("ghosted"))
				{
					c.draw(b, Color.Black * 0.35f, 0.89f);
				}
				else if (!this.pagesOfCraftingRecipes[this.currentCraftingPage][c].doesFarmerHaveIngredientsInInventory(this.cooking ? Utility.getHomeOfFarmer(Game1.player).fridge.items : null))
				{
					c.draw(b, Color.LightGray * 0.4f, 0.89f);
				}
				else
				{
					c.draw(b);
					if (this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft > 1)
					{
						NumberSprite.draw(this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft, b, new Vector2((float)(c.bounds.X + Game1.tileSize - 2), (float)(c.bounds.Y + Game1.tileSize - 2)), Color.Red, 0.5f * (c.scale / (float)Game1.pixelZoom), 0.97f, 1f, 0, 0);
					}
				}
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (this.hoverItem != null)
			{
				IClickableMenu.drawToolTip(b, this.hoverText, this.hoverTitle, this.hoverItem, this.heldItem != null, -1, 0, -1, -1, null, -1);
			}
			else if (this.hoverText != null)
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, (this.heldItem != null) ? Game1.tileSize : 0, (this.heldItem != null) ? Game1.tileSize : 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + Game1.tileSize / 4), (float)(Game1.getOldMouseY() + Game1.tileSize / 4)), 1f);
			}
			base.draw(b);
			if (this.downButton != null && this.currentCraftingPage < this.pagesOfCraftingRecipes.Count - 1)
			{
				this.downButton.draw(b);
			}
			if (this.upButton != null && this.currentCraftingPage > 0)
			{
				this.upButton.draw(b);
			}
			if (this.cooking)
			{
				base.drawMouse(b);
			}
			if (this.hoverRecipe != null)
			{
				IClickableMenu.drawHoverText(b, " ", Game1.smallFont, (this.heldItem != null) ? (Game1.tileSize * 3 / 4) : 0, (this.heldItem != null) ? (Game1.tileSize * 3 / 4) : 0, -1, this.hoverRecipe.name, -1, (this.cooking && this.lastCookingHover != null && Game1.objectInformation[(this.lastCookingHover as Object).parentSheetIndex].Split(new char[]
				{
					'/'
				}).Length >= 7) ? Game1.objectInformation[(this.lastCookingHover as Object).parentSheetIndex].Split(new char[]
				{
					'/'
				})[6].Split(new char[]
				{
					' '
				}) : null, this.lastCookingHover, 0, -1, -1, -1, -1, 1f, this.hoverRecipe);
			}
		}

		// Token: 0x04000EFE RID: 3838
		public const int howManyRecipesFitOnPage = 40;

		// Token: 0x04000EFF RID: 3839
		private string descriptionText = "";

		// Token: 0x04000F00 RID: 3840
		private string hoverText = "";

		// Token: 0x04000F01 RID: 3841
		private Item hoverItem;

		// Token: 0x04000F02 RID: 3842
		private Item lastCookingHover;

		// Token: 0x04000F03 RID: 3843
		private InventoryMenu inventory;

		// Token: 0x04000F04 RID: 3844
		private Item heldItem;

		// Token: 0x04000F05 RID: 3845
		private List<Dictionary<ClickableTextureComponent, CraftingRecipe>> pagesOfCraftingRecipes = new List<Dictionary<ClickableTextureComponent, CraftingRecipe>>();

		// Token: 0x04000F06 RID: 3846
		private int currentCraftingPage;

		// Token: 0x04000F07 RID: 3847
		private CraftingRecipe hoverRecipe;

		// Token: 0x04000F08 RID: 3848
		private ClickableTextureComponent upButton;

		// Token: 0x04000F09 RID: 3849
		private ClickableTextureComponent downButton;

		// Token: 0x04000F0A RID: 3850
		private bool cooking;

		// Token: 0x04000F0B RID: 3851
		public ClickableTextureComponent trashCan;

		// Token: 0x04000F0C RID: 3852
		public float trashCanLidRotation;

		// Token: 0x04000F0D RID: 3853
		private string hoverTitle = "";
	}
}
