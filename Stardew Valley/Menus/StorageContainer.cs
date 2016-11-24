using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x02000114 RID: 276
	public class StorageContainer : MenuWithInventory
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x00149EBC File Offset: 0x001480BC
		public StorageContainer(List<Item> inventory, int capacity, int rows = 3, StorageContainer.behaviorOnItemChange itemChangeBehavior = null, InventoryMenu.highlightThisItem highlightMethod = null) : base(highlightMethod, true, true, 0, 0)
		{
			this.itemChangeBehavior = itemChangeBehavior;
			int containerWidth = Game1.tileSize * (capacity / rows);
			int arg_23_0 = Game1.tileSize;
			int arg_2B_0 = Game1.tileSize / 4;
			this.ItemsToGrabMenu = new InventoryMenu(Game1.viewport.Width / 2 - containerWidth / 2, this.yPositionOnScreen + Game1.tileSize, false, inventory, null, capacity, rows, 0, 0, true);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00149F24 File Offset: 0x00148124
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			Item old = this.heldItem;
			int oldStack = (old != null) ? old.Stack : -1;
			if (base.isWithinBounds(x, y))
			{
				base.receiveLeftClick(x, y, false);
				if (this.itemChangeBehavior == null && old == null && this.heldItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
				{
					this.heldItem = this.ItemsToGrabMenu.tryToAddItem(this.heldItem, "Ship");
				}
			}
			bool sound = true;
			if (this.ItemsToGrabMenu.isWithinBounds(x, y))
			{
				this.heldItem = this.ItemsToGrabMenu.leftClick(x, y, this.heldItem, false);
				if ((this.heldItem != null && old == null) || (this.heldItem != null && old != null && !this.heldItem.Equals(old)))
				{
					if (this.itemChangeBehavior != null)
					{
						sound = this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), old, this, true);
					}
					if (sound)
					{
						Game1.playSound("dwop");
					}
				}
				if ((this.heldItem == null && old != null) || (this.heldItem != null && old != null && !this.heldItem.Equals(old)))
				{
					Item tmp = this.heldItem;
					if (this.heldItem == null && this.ItemsToGrabMenu.getItemAt(x, y) != null && oldStack < this.ItemsToGrabMenu.getItemAt(x, y).Stack)
					{
						tmp = old.getOne();
						tmp.Stack = oldStack;
					}
					if (this.itemChangeBehavior != null)
					{
						sound = this.itemChangeBehavior(old, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), tmp, this, false);
					}
					if (sound)
					{
						Game1.playSound("Ship");
					}
				}
				if (this.heldItem is Object && (this.heldItem as Object).isRecipe)
				{
					string recipeName = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
					try
					{
						if ((this.heldItem as Object).category == -7)
						{
							Game1.player.cookingRecipes.Add(recipeName, 0);
						}
						else
						{
							Game1.player.craftingRecipes.Add(recipeName, 0);
						}
						this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
						Game1.playSound("newRecipe");
					}
					catch (Exception)
					{
					}
					this.heldItem = null;
				}
				else if (Game1.oldKBState.IsKeyDown(Keys.LeftShift) && Game1.player.addItemToInventoryBool(this.heldItem, false))
				{
					this.heldItem = null;
					if (this.itemChangeBehavior != null)
					{
						sound = this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), old, this, true);
					}
					if (sound)
					{
						Game1.playSound("coin");
					}
				}
			}
			if (this.okButton.containsPoint(x, y) && this.readyToClose())
			{
				Game1.playSound("bigDeSelect");
				Game1.exitActiveMenu();
			}
			if (this.trashCan.containsPoint(x, y) && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is Object && Game1.player.specialItems.Contains((this.heldItem as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
			}
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0014A2CC File Offset: 0x001484CC
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			int oldStack = (this.heldItem != null) ? this.heldItem.Stack : 0;
			Item old = this.heldItem;
			if (base.isWithinBounds(x, y))
			{
				base.receiveRightClick(x, y, true);
				if (this.itemChangeBehavior == null && old == null && this.heldItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
				{
					this.heldItem = this.ItemsToGrabMenu.tryToAddItem(this.heldItem, "Ship");
				}
			}
			if (this.ItemsToGrabMenu.isWithinBounds(x, y))
			{
				this.heldItem = this.ItemsToGrabMenu.rightClick(x, y, this.heldItem, false);
				if ((this.heldItem != null && old == null) || (this.heldItem != null && old != null && !this.heldItem.Equals(old)) || (this.heldItem != null && old != null && this.heldItem.Equals(old) && this.heldItem.Stack != oldStack))
				{
					if (this.itemChangeBehavior != null)
					{
						this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), old, this, true);
					}
					Game1.playSound("dwop");
				}
				if ((this.heldItem == null && old != null) || (this.heldItem != null && old != null && !this.heldItem.Equals(old)))
				{
					if (this.itemChangeBehavior != null)
					{
						this.itemChangeBehavior(old, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), this.heldItem, this, false);
					}
					Game1.playSound("Ship");
				}
				if (this.heldItem is Object && (this.heldItem as Object).isRecipe)
				{
					string recipeName = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
					try
					{
						if ((this.heldItem as Object).category == -7)
						{
							Game1.player.cookingRecipes.Add(recipeName, 0);
						}
						else
						{
							Game1.player.craftingRecipes.Add(recipeName, 0);
						}
						this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
						Game1.playSound("newRecipe");
					}
					catch (Exception)
					{
					}
					this.heldItem = null;
					return;
				}
				if (Game1.oldKBState.IsKeyDown(Keys.LeftShift) && Game1.player.addItemToInventoryBool(this.heldItem, false))
				{
					this.heldItem = null;
					Game1.playSound("coin");
					if (this.itemChangeBehavior != null)
					{
						this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), old, this, true);
					}
				}
			}
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0014A5AC File Offset: 0x001487AC
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.poof != null && this.poof.update(time))
			{
				this.poof = null;
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0014A5D2 File Offset: 0x001487D2
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.ItemsToGrabMenu.hover(x, y, this.heldItem);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0014A5F0 File Offset: 0x001487F0
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			base.draw(b, false, false);
			Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, null, false);
			this.ItemsToGrabMenu.draw(b);
			if (this.poof != null)
			{
				this.poof.draw(b, true, 0, 0);
			}
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + Game1.pixelZoom * 4), (float)(Game1.getOldMouseY() + Game1.pixelZoom * 4)), 1f);
			}
			base.drawMouse(b);
			if (this.ItemsToGrabMenu.descriptionTitle != null && this.ItemsToGrabMenu.descriptionTitle.Length > 1)
			{
				IClickableMenu.drawHoverText(b, this.ItemsToGrabMenu.descriptionTitle, Game1.smallFont, Game1.tileSize / 2 + ((this.heldItem != null) ? (Game1.tileSize / 4) : (-Game1.tileSize / 3)), Game1.tileSize / 2 + ((this.heldItem != null) ? (Game1.tileSize / 4) : (-Game1.tileSize / 3)), -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x0400114E RID: 4430
		public InventoryMenu ItemsToGrabMenu;

		// Token: 0x0400114F RID: 4431
		private TemporaryAnimatedSprite poof;

		// Token: 0x04001150 RID: 4432
		private StorageContainer.behaviorOnItemChange itemChangeBehavior;

		// Token: 0x02000188 RID: 392
		// Token: 0x060013E3 RID: 5091
		public delegate bool behaviorOnItemChange(Item i, int position, Item old, StorageContainer container, bool onRemoval = false);
	}
}
