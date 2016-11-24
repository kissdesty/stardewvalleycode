using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley.Objects;

namespace StardewValley.Menus
{
	// Token: 0x020000F5 RID: 245
	public class ItemGrabMenu : MenuWithInventory
	{
		// Token: 0x06000EC0 RID: 3776 RVA: 0x0012DAA0 File Offset: 0x0012BCA0
		public ItemGrabMenu(List<Item> inventory) : base(null, true, true, 0, 0)
		{
			this.ItemsToGrabMenu = new InventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, inventory, null, -1, 3, 0, 0, true);
			this.inventory.showGrayedOutSlots = true;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0012DAFC File Offset: 0x0012BCFC
		public ItemGrabMenu(List<Item> inventory, bool reverseGrab, bool showReceivingMenu, InventoryMenu.highlightThisItem highlightFunction, ItemGrabMenu.behaviorOnItemSelect behaviorOnItemSelectFunction, string message, ItemGrabMenu.behaviorOnItemSelect behaviorOnItemGrab = null, bool snapToBottom = false, bool canBeExitedWithKey = false, bool playRightClickSound = true, bool allowRightClick = true, bool showOrganizeButton = false, int source = 0, Item sourceItem = null, int whichSpecialButton = -1, object specialObject = null) : base(highlightFunction, true, true, 0, 0)
		{
			this.source = source;
			this.message = message;
			this.reverseGrab = reverseGrab;
			this.showReceivingMenu = showReceivingMenu;
			this.playRightClickSound = playRightClickSound;
			this.allowRightClick = allowRightClick;
			this.inventory.showGrayedOutSlots = true;
			this.sourceItem = sourceItem;
			if (source == 1 && sourceItem != null && sourceItem is Chest)
			{
				this.chestColorPicker = new DiscreteColorPicker(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize - IClickableMenu.borderWidth * 2, 0, new Chest(true));
				this.chestColorPicker.colorSelection = this.chestColorPicker.getSelectionFromColor((sourceItem as Chest).playerChoiceColor);
				(this.chestColorPicker.itemToDrawColored as Chest).playerChoiceColor = this.chestColorPicker.getColorFromSelection(this.chestColorPicker.colorSelection);
				this.colorPickerToggleButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 5, 16 * Game1.pixelZoom, 16 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(119, 469, 16, 16), (float)Game1.pixelZoom, false)
				{
					hoverText = Game1.content.LoadString("Strings\\UI:Toggle_ColorPicker", new object[0])
				};
			}
			this.whichSpecialButton = whichSpecialButton;
			this.specialObject = specialObject;
			if (whichSpecialButton == 1)
			{
				this.specialButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 5, 16 * Game1.pixelZoom, 16 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(108, 491, 16, 16), (float)Game1.pixelZoom, false);
				if (specialObject != null && specialObject is JunimoHut)
				{
					this.specialButton.sourceRect.X = ((specialObject as JunimoHut).noHarvest ? 124 : 108);
				}
			}
			if (snapToBottom)
			{
				base.movePosition(0, Game1.viewport.Height - (this.yPositionOnScreen + this.height - IClickableMenu.spaceToClearTopBorder));
				this.snappedtoBottom = true;
			}
			this.ItemsToGrabMenu = new InventoryMenu(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen, false, inventory, highlightFunction, -1, 3, 0, 0, true);
			this.behaviorFunction = behaviorOnItemSelectFunction;
			this.behaviorOnItemGrab = behaviorOnItemGrab;
			this.canExitOnKey = canBeExitedWithKey;
			if (showOrganizeButton)
			{
				this.organizeButton = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + this.height / 3 - Game1.tileSize, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:ItemGrab_Organize", new object[0]), Game1.mouseCursors, new Rectangle(162, 440, 16, 16), (float)Game1.pixelZoom, false);
			}
			if ((Game1.isAnyGamePadButtonBeingPressed() || !Game1.lastCursorMotionWasMouse) && this.ItemsToGrabMenu.actualInventory.Count > 0 && Game1.activeClickableMenu == null)
			{
				Game1.setMousePosition(this.inventory.inventory[0].bounds.Center);
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0012DE50 File Offset: 0x0012C050
		public void initializeShippingBin()
		{
			this.shippingBin = true;
			this.lastShippedHolder = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height / 2 - 20 * Game1.pixelZoom - Game1.tileSize, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), "", Game1.content.LoadString("Strings\\UI:ShippingBin_LastItem", new object[0]), Game1.mouseCursors, new Rectangle(293, 360, 24, 24), (float)Game1.pixelZoom, false);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0012DEF8 File Offset: 0x0012C0F8
		public void setSourceItem(Item item)
		{
			this.sourceItem = item;
			this.chestColorPicker = null;
			this.colorPickerToggleButton = null;
			if (this.source == 1 && this.sourceItem != null && this.sourceItem is Chest)
			{
				this.chestColorPicker = new DiscreteColorPicker(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize - IClickableMenu.borderWidth * 2, 0, new Chest(true));
				this.chestColorPicker.colorSelection = this.chestColorPicker.getSelectionFromColor((this.sourceItem as Chest).playerChoiceColor);
				(this.chestColorPicker.itemToDrawColored as Chest).playerChoiceColor = this.chestColorPicker.getColorFromSelection(this.chestColorPicker.colorSelection);
				this.colorPickerToggleButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 5, 16 * Game1.pixelZoom, 16 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(119, 469, 16, 16), (float)Game1.pixelZoom, false)
				{
					hoverText = Game1.content.LoadString("Strings\\UI:Toggle_ColorPicker", new object[0])
				};
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0012E038 File Offset: 0x0012C238
		public void setBackgroundTransparency(bool b)
		{
			this.drawBG = b;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0012E041 File Offset: 0x0012C241
		public void setDestroyItemOnClick(bool b)
		{
			this.destroyItemOnClick = b;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0012E04C File Offset: 0x0012C24C
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (!this.allowRightClick)
			{
				return;
			}
			base.receiveRightClick(x, y, playSound && this.playRightClickSound);
			if (this.heldItem == null && this.showReceivingMenu)
			{
				this.heldItem = this.ItemsToGrabMenu.rightClick(x, y, this.heldItem, false);
				if (this.heldItem != null && this.behaviorOnItemGrab != null)
				{
					this.behaviorOnItemGrab(this.heldItem, Game1.player);
					if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ItemGrabMenu)
					{
						(Game1.activeClickableMenu as ItemGrabMenu).setSourceItem(this.sourceItem);
					}
				}
				if (this.heldItem is Object && (this.heldItem as Object).parentSheetIndex == 326)
				{
					this.heldItem = null;
					Game1.player.canUnderstandDwarves = true;
					this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
					Game1.playSound("fireball");
					return;
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
				if (Game1.player.addItemToInventoryBool(this.heldItem, false))
				{
					this.heldItem = null;
					Game1.playSound("coin");
					return;
				}
			}
			else if (this.reverseGrab || this.behaviorFunction != null)
			{
				this.behaviorFunction(this.heldItem, Game1.player);
				if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ItemGrabMenu)
				{
					(Game1.activeClickableMenu as ItemGrabMenu).setSourceItem(this.sourceItem);
				}
				if (this.destroyItemOnClick)
				{
					this.heldItem = null;
					return;
				}
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0012E30C File Offset: 0x0012C50C
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			if (this.snappedtoBottom)
			{
				base.movePosition((newBounds.Width - oldBounds.Width) / 2, Game1.viewport.Height - (this.yPositionOnScreen + this.height - IClickableMenu.spaceToClearTopBorder));
			}
			if (this.ItemsToGrabMenu != null)
			{
				this.ItemsToGrabMenu.gameWindowSizeChanged(oldBounds, newBounds);
			}
			if (this.organizeButton != null)
			{
				this.organizeButton = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + this.height / 3 - Game1.tileSize, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:ItemGrab_Organize", new object[0]), Game1.mouseCursors, new Rectangle(162, 440, 16, 16), (float)Game1.pixelZoom, false);
			}
			if (this.source == 1 && this.sourceItem != null && this.sourceItem is Chest)
			{
				this.chestColorPicker = new DiscreteColorPicker(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize - IClickableMenu.borderWidth * 2, 0, null);
				this.chestColorPicker.colorSelection = this.chestColorPicker.getSelectionFromColor((this.sourceItem as Chest).playerChoiceColor);
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0012E458 File Offset: 0x0012C658
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, !this.destroyItemOnClick);
			if (this.shippingBin && this.lastShippedHolder.containsPoint(x, y))
			{
				if (Game1.getFarm().lastItemShipped != null && Game1.player.addItemToInventoryBool(Game1.getFarm().lastItemShipped, false))
				{
					Game1.playSound("coin");
					Game1.getFarm().shippingBin.Remove(Game1.getFarm().lastItemShipped);
					Game1.getFarm().lastItemShipped = null;
					if (Game1.player.ActiveObject != null)
					{
						Game1.player.showCarrying();
						Game1.player.Halt();
					}
				}
				return;
			}
			if (this.chestColorPicker != null)
			{
				this.chestColorPicker.receiveLeftClick(x, y, true);
				if (this.sourceItem != null && this.sourceItem is Chest)
				{
					(this.sourceItem as Chest).playerChoiceColor = this.chestColorPicker.getColorFromSelection(this.chestColorPicker.colorSelection);
				}
			}
			if (this.colorPickerToggleButton != null && this.colorPickerToggleButton.containsPoint(x, y))
			{
				Game1.player.showChestColorPicker = !Game1.player.showChestColorPicker;
				this.chestColorPicker.visible = Game1.player.showChestColorPicker;
				Game1.soundBank.PlayCue("drumkit6");
			}
			if (this.whichSpecialButton != -1 && this.specialButton != null && this.specialButton.containsPoint(x, y))
			{
				Game1.soundBank.PlayCue("drumkit6");
				int num = this.whichSpecialButton;
				if (num == 1 && this.specialObject != null && this.specialObject is JunimoHut)
				{
					(this.specialObject as JunimoHut).noHarvest = !(this.specialObject as JunimoHut).noHarvest;
					this.specialButton.sourceRect.X = ((this.specialObject as JunimoHut).noHarvest ? 124 : 108);
				}
			}
			if (this.heldItem == null && this.showReceivingMenu)
			{
				this.heldItem = this.ItemsToGrabMenu.leftClick(x, y, this.heldItem, false);
				if (this.heldItem != null && this.behaviorOnItemGrab != null)
				{
					this.behaviorOnItemGrab(this.heldItem, Game1.player);
					if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ItemGrabMenu)
					{
						(Game1.activeClickableMenu as ItemGrabMenu).setSourceItem(this.sourceItem);
					}
				}
				if (this.heldItem is Object && (this.heldItem as Object).parentSheetIndex == 326)
				{
					this.heldItem = null;
					Game1.player.canUnderstandDwarves = true;
					this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
					Game1.playSound("fireball");
				}
				else if (this.heldItem is Object && (this.heldItem as Object).parentSheetIndex == 102)
				{
					this.heldItem = null;
					Game1.player.foundArtifact(102, 1);
					this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float)(x - x % Game1.tileSize + Game1.tileSize / 4), (float)(y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
					Game1.playSound("fireball");
				}
				else if (this.heldItem is Object && (this.heldItem as Object).isRecipe)
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
				else if (Game1.player.addItemToInventoryBool(this.heldItem, false))
				{
					this.heldItem = null;
					Game1.playSound("coin");
				}
			}
			else if ((this.reverseGrab || this.behaviorFunction != null) && this.isWithinBounds(x, y))
			{
				this.behaviorFunction(this.heldItem, Game1.player);
				if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ItemGrabMenu)
				{
					(Game1.activeClickableMenu as ItemGrabMenu).setSourceItem(this.sourceItem);
				}
				if (this.destroyItemOnClick)
				{
					this.heldItem = null;
					return;
				}
			}
			if (this.organizeButton != null && this.organizeButton.containsPoint(x, y))
			{
				ItemGrabMenu.organizeItemsInList(this.ItemsToGrabMenu.actualInventory);
				Game1.activeClickableMenu = new ItemGrabMenu(this.ItemsToGrabMenu.actualInventory, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), this.behaviorFunction, null, this.behaviorOnItemGrab, false, true, true, true, true, this.source, this.sourceItem, -1, null);
				Game1.playSound("Ship");
				return;
			}
			if (this.heldItem != null && !this.isWithinBounds(x, y) && this.heldItem.canBeTrashed())
			{
				Game1.playSound("throwDownITem");
				Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, null);
				this.heldItem = null;
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0012EA50 File Offset: 0x0012CC50
		public static void organizeItemsInList(List<Item> items)
		{
			items.Sort();
			items.Reverse();
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0012EA60 File Offset: 0x0012CC60
		public bool areAllItemsTaken()
		{
			for (int i = 0; i < this.ItemsToGrabMenu.actualInventory.Count; i++)
			{
				if (this.ItemsToGrabMenu.actualInventory[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0012EAA0 File Offset: 0x0012CCA0
		public override void receiveKeyPress(Keys key)
		{
			if ((this.canExitOnKey || this.areAllItemsTaken()) && Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
			{
				base.exitThisMenu(true);
				if (Game1.currentLocation.currentEvent != null)
				{
					Event expr_4C = Game1.currentLocation.currentEvent;
					int currentCommand = expr_4C.CurrentCommand;
					expr_4C.CurrentCommand = currentCommand + 1;
				}
			}
			else if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.heldItem != null)
			{
				Game1.setMousePosition(this.trashCan.bounds.Center);
			}
			if (key == Keys.Delete && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is Object && Game1.player.specialItems.Contains((this.heldItem as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
			}
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0012EBB7 File Offset: 0x0012CDB7
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.poof != null && this.poof.update(time))
			{
				this.poof = null;
			}
			if (this.chestColorPicker != null)
			{
				this.chestColorPicker.update(time);
			}
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0012EBF4 File Offset: 0x0012CDF4
		public override void performHoverAction(int x, int y)
		{
			if (this.colorPickerToggleButton != null)
			{
				this.colorPickerToggleButton.tryHover(x, y, 0.25f);
				if (this.colorPickerToggleButton.containsPoint(x, y))
				{
					this.hoverText = this.colorPickerToggleButton.hoverText;
					return;
				}
			}
			if (this.specialButton != null)
			{
				this.specialButton.tryHover(x, y, 0.25f);
			}
			if (this.ItemsToGrabMenu.isWithinBounds(x, y) && this.showReceivingMenu)
			{
				this.hoveredItem = this.ItemsToGrabMenu.hover(x, y, this.heldItem);
			}
			else
			{
				base.performHoverAction(x, y);
			}
			if (this.organizeButton != null)
			{
				this.hoverText = null;
				this.organizeButton.tryHover(x, y, 0.1f);
				if (this.organizeButton.containsPoint(x, y))
				{
					this.hoverText = this.organizeButton.hoverText;
				}
			}
			if (this.shippingBin)
			{
				this.hoverText = null;
				if (this.lastShippedHolder.containsPoint(x, y) && Game1.getFarm().lastItemShipped != null)
				{
					this.hoverText = this.lastShippedHolder.hoverText;
				}
			}
			if (this.chestColorPicker != null)
			{
				this.chestColorPicker.performHoverAction(x, y);
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0012ED24 File Offset: 0x0012CF24
		public override void draw(SpriteBatch b)
		{
			if (this.drawBG)
			{
				b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			}
			base.draw(b, false, false);
			if (this.showReceivingMenu)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 16 * Game1.pixelZoom), (float)(this.yPositionOnScreen + this.height / 2 + Game1.tileSize + Game1.pixelZoom * 4)), new Rectangle?(new Rectangle(16, 368, 12, 16)), Color.White, 4.712389f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 16 * Game1.pixelZoom), (float)(this.yPositionOnScreen + this.height / 2 + Game1.tileSize - Game1.pixelZoom * 4)), new Rectangle?(new Rectangle(21, 368, 11, 16)), Color.White, 4.712389f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 10 * Game1.pixelZoom), (float)(this.yPositionOnScreen + this.height / 2 + Game1.tileSize - Game1.pixelZoom * 11)), new Rectangle?(new Rectangle(4, 372, 8, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				if (this.source != 0)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 18 * Game1.pixelZoom), (float)(this.yPositionOnScreen + Game1.tileSize + Game1.pixelZoom * 4)), new Rectangle?(new Rectangle(16, 368, 12, 16)), Color.White, 4.712389f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 18 * Game1.pixelZoom), (float)(this.yPositionOnScreen + Game1.tileSize - Game1.pixelZoom * 4)), new Rectangle?(new Rectangle(21, 368, 11, 16)), Color.White, 4.712389f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
					Rectangle sourceRect = new Rectangle(127, 412, 10, 11);
					int num = this.source;
					if (num != 2)
					{
						if (num == 3)
						{
							sourceRect.X += 10;
						}
					}
					else
					{
						sourceRect.X += 20;
					}
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 13 * Game1.pixelZoom), (float)(this.yPositionOnScreen + Game1.tileSize - Game1.pixelZoom * 11)), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, null, false);
				this.ItemsToGrabMenu.draw(b);
			}
			else if (this.message != null)
			{
				Game1.drawDialogueBox(Game1.viewport.Width / 2, this.ItemsToGrabMenu.yPositionOnScreen + this.ItemsToGrabMenu.height / 2, false, false, this.message);
			}
			if (this.poof != null)
			{
				this.poof.draw(b, true, 0, 0);
			}
			if (this.shippingBin && Game1.getFarm().lastItemShipped != null)
			{
				this.lastShippedHolder.draw(b);
				Game1.getFarm().lastItemShipped.drawInMenu(b, new Vector2((float)(this.lastShippedHolder.bounds.X + Game1.pixelZoom * 4), (float)(this.lastShippedHolder.bounds.Y + Game1.pixelZoom * 4)), 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.lastShippedHolder.bounds.X + Game1.pixelZoom * -2), (float)(this.lastShippedHolder.bounds.Bottom - Game1.pixelZoom * 25)), new Rectangle?(new Rectangle(325, 448, 5, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.lastShippedHolder.bounds.X + Game1.pixelZoom * 21), (float)(this.lastShippedHolder.bounds.Bottom - Game1.pixelZoom * 25)), new Rectangle?(new Rectangle(325, 448, 5, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.lastShippedHolder.bounds.X + Game1.pixelZoom * -2), (float)(this.lastShippedHolder.bounds.Bottom - Game1.pixelZoom * 11)), new Rectangle?(new Rectangle(325, 452, 5, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.lastShippedHolder.bounds.X + Game1.pixelZoom * 21), (float)(this.lastShippedHolder.bounds.Bottom - Game1.pixelZoom * 11)), new Rectangle?(new Rectangle(325, 452, 5, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			if (this.colorPickerToggleButton != null)
			{
				this.colorPickerToggleButton.draw(b);
			}
			else if (this.specialButton != null)
			{
				this.specialButton.draw(b);
			}
			if (this.chestColorPicker != null)
			{
				this.chestColorPicker.draw(b);
			}
			if (this.organizeButton != null)
			{
				this.organizeButton.draw(b);
			}
			if (this.hoverText != null && (this.hoveredItem == null || this.hoveredItem == null || this.ItemsToGrabMenu == null))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.hoveredItem != null)
			{
				IClickableMenu.drawToolTip(b, this.hoveredItem.getDescription(), this.hoveredItem.Name, this.hoveredItem, this.heldItem != null, -1, 0, -1, -1, null, -1);
			}
			else if (this.hoveredItem != null && this.ItemsToGrabMenu != null)
			{
				IClickableMenu.drawToolTip(b, this.ItemsToGrabMenu.descriptionText, this.ItemsToGrabMenu.descriptionTitle, this.hoveredItem, this.heldItem != null, -1, 0, -1, -1, null, -1);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
			}
			Game1.mouseCursorTransparency = 1f;
			base.drawMouse(b);
		}

		// Token: 0x04000FC7 RID: 4039
		public const int source_none = 0;

		// Token: 0x04000FC8 RID: 4040
		public const int source_chest = 1;

		// Token: 0x04000FC9 RID: 4041
		public const int source_gift = 2;

		// Token: 0x04000FCA RID: 4042
		public const int source_fishingChest = 3;

		// Token: 0x04000FCB RID: 4043
		public const int specialButton_junimotoggle = 1;

		// Token: 0x04000FCC RID: 4044
		private InventoryMenu ItemsToGrabMenu;

		// Token: 0x04000FCD RID: 4045
		private TemporaryAnimatedSprite poof;

		// Token: 0x04000FCE RID: 4046
		public bool reverseGrab;

		// Token: 0x04000FCF RID: 4047
		public bool showReceivingMenu = true;

		// Token: 0x04000FD0 RID: 4048
		public bool drawBG = true;

		// Token: 0x04000FD1 RID: 4049
		public bool destroyItemOnClick;

		// Token: 0x04000FD2 RID: 4050
		public bool canExitOnKey;

		// Token: 0x04000FD3 RID: 4051
		public bool playRightClickSound;

		// Token: 0x04000FD4 RID: 4052
		public bool allowRightClick;

		// Token: 0x04000FD5 RID: 4053
		public bool shippingBin;

		// Token: 0x04000FD6 RID: 4054
		private string message;

		// Token: 0x04000FD7 RID: 4055
		private ItemGrabMenu.behaviorOnItemSelect behaviorFunction;

		// Token: 0x04000FD8 RID: 4056
		public ItemGrabMenu.behaviorOnItemSelect behaviorOnItemGrab;

		// Token: 0x04000FD9 RID: 4057
		private Item hoverItem;

		// Token: 0x04000FDA RID: 4058
		private Item sourceItem;

		// Token: 0x04000FDB RID: 4059
		private ClickableTextureComponent organizeButton;

		// Token: 0x04000FDC RID: 4060
		private ClickableTextureComponent colorPickerToggleButton;

		// Token: 0x04000FDD RID: 4061
		private ClickableTextureComponent specialButton;

		// Token: 0x04000FDE RID: 4062
		private ClickableTextureComponent lastShippedHolder;

		// Token: 0x04000FDF RID: 4063
		public int source;

		// Token: 0x04000FE0 RID: 4064
		public int whichSpecialButton;

		// Token: 0x04000FE1 RID: 4065
		public object specialObject;

		// Token: 0x04000FE2 RID: 4066
		private bool snappedtoBottom;

		// Token: 0x04000FE3 RID: 4067
		private DiscreteColorPicker chestColorPicker;

		// Token: 0x02000184 RID: 388
		// Token: 0x060013D4 RID: 5076
		public delegate void behaviorOnItemSelect(Item item, Farmer who);
	}
}
