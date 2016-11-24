using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x020000F3 RID: 243
	public class InventoryMenu : IClickableMenu
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x0012A8F0 File Offset: 0x00128AF0
		public InventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true) : base(xPosition, yPosition, Game1.tileSize * (((capacity == -1) ? 36 : capacity) / rows), Game1.tileSize * rows + Game1.tileSize / 4, false)
		{
			this.drawSlots = drawSlots;
			this.horizontalGap = horizontalGap;
			this.verticalGap = verticalGap;
			this.rows = rows;
			this.capacity = ((capacity == -1) ? 36 : capacity);
			this.playerInventory = playerInventory;
			this.actualInventory = actualInventory;
			if (actualInventory == null)
			{
				this.actualInventory = Game1.player.items;
			}
			for (int i = 0; i < Game1.player.maxItems; i++)
			{
				if (Game1.player.items.Count <= i)
				{
					Game1.player.items.Add(null);
				}
			}
			for (int j = 0; j < this.actualInventory.Count; j++)
			{
				this.inventory.Add(new ClickableComponent(new Rectangle(xPosition + j % (this.capacity / rows) * Game1.tileSize + horizontalGap * (j % (this.capacity / rows)), this.yPositionOnScreen + j / (this.capacity / rows) * (Game1.tileSize + verticalGap) + (j / (this.capacity / rows) - 1) * Game1.pixelZoom - ((j > this.capacity / rows || !playerInventory || verticalGap != 0) ? 0 : (Game1.tileSize / 5)), Game1.tileSize, Game1.tileSize), string.Concat(j)));
			}
			this.highlightMethod = highlightMethod;
			if (highlightMethod == null)
			{
				this.highlightMethod = new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems);
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0000846E File Offset: 0x0000666E
		public static bool highlightAllItems(Item i)
		{
			return true;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0012AAC0 File Offset: 0x00128CC0
		public void movePosition(int x, int y)
		{
			this.xPositionOnScreen += x;
			this.yPositionOnScreen += y;
			foreach (ClickableComponent expr_31 in this.inventory)
			{
				expr_31.bounds.X = expr_31.bounds.X + x;
				expr_31.bounds.Y = expr_31.bounds.Y + y;
			}
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0012AB48 File Offset: 0x00128D48
		public Item tryToAddItem(Item toPlace, string sound = "coin")
		{
			if (toPlace == null)
			{
				return null;
			}
			int originalStack = toPlace.Stack;
			using (List<ClickableComponent>.Enumerator enumerator = this.inventory.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int slotNumber = Convert.ToInt32(enumerator.Current.name);
					if (slotNumber < this.actualInventory.Count && this.actualInventory[slotNumber] != null && this.highlightMethod(this.actualInventory[slotNumber]) && this.actualInventory[slotNumber].canStackWith(toPlace))
					{
						toPlace.Stack = this.actualInventory[slotNumber].addToStack(toPlace.Stack);
						if (toPlace.Stack <= 0)
						{
							try
							{
								Game1.playSound(sound);
								if (this.onAddItem != null)
								{
									this.onAddItem(toPlace, this.playerInventory ? Game1.player : null);
								}
							}
							catch (Exception)
							{
							}
							Item result = null;
							return result;
						}
					}
				}
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.inventory.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int slotNumber2 = Convert.ToInt32(enumerator.Current.name);
					if (slotNumber2 < this.actualInventory.Count && (this.actualInventory[slotNumber2] == null || this.highlightMethod(this.actualInventory[slotNumber2])) && this.actualInventory[slotNumber2] == null)
					{
						try
						{
							Game1.playSound(sound);
						}
						catch (Exception)
						{
						}
						Item result = Utility.addItemToInventory(toPlace, slotNumber2, this.actualInventory, this.onAddItem);
						return result;
					}
				}
			}
			if (toPlace.Stack < originalStack)
			{
				Game1.playSound(sound);
			}
			return toPlace;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0012AD38 File Offset: 0x00128F38
		public int getInventoryPositionOfClick(int x, int y)
		{
			for (int i = 0; i < this.inventory.Count; i++)
			{
				if (this.inventory[i] != null && this.inventory[i].bounds.Contains(x, y))
				{
					return Convert.ToInt32(this.inventory[i].name);
				}
			}
			return -1;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0012AD9C File Offset: 0x00128F9C
		public Item leftClick(int x, int y, Item toPlace, bool playSound = true)
		{
			foreach (ClickableComponent c in this.inventory)
			{
				if (c.containsPoint(x, y))
				{
					int slotNumber = Convert.ToInt32(c.name);
					if (slotNumber < this.actualInventory.Count && (this.actualInventory[slotNumber] == null || this.highlightMethod(this.actualInventory[slotNumber]) || this.actualInventory[slotNumber].canStackWith(toPlace)))
					{
						if (this.actualInventory[slotNumber] != null)
						{
							Item result;
							if (toPlace != null)
							{
								if (playSound)
								{
									Game1.playSound("stoneStep");
								}
								result = Utility.addItemToInventory(toPlace, slotNumber, this.actualInventory, this.onAddItem);
								return result;
							}
							if (playSound)
							{
								Game1.playSound("dwop");
							}
							result = Utility.removeItemFromInventory(slotNumber, this.actualInventory);
							return result;
						}
						else if (toPlace != null)
						{
							if (playSound)
							{
								Game1.playSound("stoneStep");
							}
							Item result = Utility.addItemToInventory(toPlace, slotNumber, this.actualInventory, this.onAddItem);
							return result;
						}
					}
				}
			}
			return toPlace;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0012AED0 File Offset: 0x001290D0
		public Vector2 snapToClickableComponent(int x, int y)
		{
			foreach (ClickableComponent c in this.inventory)
			{
				if (c.containsPoint(x, y))
				{
					return new Vector2((float)c.bounds.X, (float)c.bounds.Y);
				}
			}
			return new Vector2((float)x, (float)y);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0012AF54 File Offset: 0x00129154
		public Item getItemAt(int x, int y)
		{
			foreach (ClickableComponent c in this.inventory)
			{
				if (c.containsPoint(x, y))
				{
					return this.getItemFromClickableComponent(c);
				}
			}
			return null;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0012AFB8 File Offset: 0x001291B8
		public Item getItemFromClickableComponent(ClickableComponent c)
		{
			if (c != null)
			{
				int slotNumber = Convert.ToInt32(c.name);
				if (slotNumber < this.actualInventory.Count)
				{
					return this.actualInventory[slotNumber];
				}
			}
			return null;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0012AFF0 File Offset: 0x001291F0
		public Item rightClick(int x, int y, Item toAddTo, bool playSound = true)
		{
			foreach (ClickableComponent expr_18 in this.inventory)
			{
				int slotNumber = Convert.ToInt32(expr_18.name);
				if (expr_18.containsPoint(x, y) && (this.actualInventory[slotNumber] == null || this.highlightMethod(this.actualInventory[slotNumber])) && slotNumber < this.actualInventory.Count && this.actualInventory[slotNumber] != null)
				{
					if (this.actualInventory[slotNumber] is Tool && (toAddTo == null || toAddTo is Object) && (this.actualInventory[slotNumber] as Tool).canThisBeAttached((Object)toAddTo))
					{
						Item result = (this.actualInventory[slotNumber] as Tool).attach((toAddTo == null) ? null : ((Object)toAddTo));
						return result;
					}
					if (toAddTo == null)
					{
						if (this.actualInventory[slotNumber].maximumStackSize() != -1)
						{
							if (slotNumber == Game1.player.CurrentToolIndex && this.actualInventory[slotNumber] != null && this.actualInventory[slotNumber].Stack == 1)
							{
								this.actualInventory[slotNumber].actionWhenStopBeingHeld(Game1.player);
							}
							Item tmp = this.actualInventory[slotNumber].getOne();
							if (this.actualInventory[slotNumber].Stack > 1 && Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
							{
								new InputButton(Keys.LeftShift)
							}))
							{
								tmp.Stack = (int)Math.Ceiling((double)this.actualInventory[slotNumber].Stack / 2.0);
								this.actualInventory[slotNumber].Stack = this.actualInventory[slotNumber].Stack / 2;
							}
							else if (this.actualInventory[slotNumber].Stack == 1)
							{
								this.actualInventory[slotNumber] = null;
							}
							else
							{
								Item expr_208 = this.actualInventory[slotNumber];
								int stack = expr_208.Stack;
								expr_208.Stack = stack - 1;
							}
							if (this.actualInventory[slotNumber] != null && this.actualInventory[slotNumber].Stack <= 0)
							{
								this.actualInventory[slotNumber] = null;
							}
							if (playSound)
							{
								Game1.playSound("dwop");
							}
							Item result = tmp;
							return result;
						}
					}
					else if (this.actualInventory[slotNumber].canStackWith(toAddTo) && toAddTo.Stack < toAddTo.maximumStackSize())
					{
						if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[]
						{
							new InputButton(Keys.LeftShift)
						}))
						{
							toAddTo.Stack += (int)Math.Ceiling((double)this.actualInventory[slotNumber].Stack / 2.0);
							this.actualInventory[slotNumber].Stack = this.actualInventory[slotNumber].Stack / 2;
						}
						else
						{
							int stack = toAddTo.Stack;
							toAddTo.Stack = stack + 1;
							Item expr_31B = this.actualInventory[slotNumber];
							stack = expr_31B.Stack;
							expr_31B.Stack = stack - 1;
						}
						if (playSound)
						{
							Game1.playSound("dwop");
						}
						if (this.actualInventory[slotNumber].Stack <= 0)
						{
							if (slotNumber == Game1.player.CurrentToolIndex)
							{
								this.actualInventory[slotNumber].actionWhenStopBeingHeld(Game1.player);
							}
							this.actualInventory[slotNumber] = null;
						}
						Item result = toAddTo;
						return result;
					}
				}
			}
			return toAddTo;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0012B3BC File Offset: 0x001295BC
		public Item hover(int x, int y, Item heldItem)
		{
			this.descriptionText = "";
			this.descriptionTitle = "";
			this.hoverText = "";
			this.hoverTitle = "";
			Item toReturn = null;
			foreach (ClickableComponent c in this.inventory)
			{
				int slotNumber = Convert.ToInt32(c.name);
				c.scale = Math.Max(1f, c.scale - 0.025f);
				if (c.containsPoint(x, y) && (this.actualInventory[slotNumber] == null || this.highlightMethod(this.actualInventory[slotNumber])) && slotNumber < this.actualInventory.Count && this.actualInventory[slotNumber] != null)
				{
					this.descriptionTitle = this.actualInventory[slotNumber].Name;
					this.descriptionText = Environment.NewLine + this.actualInventory[slotNumber].getDescription();
					c.scale = Math.Min(c.scale + 0.05f, 1.1f);
					string s = this.actualInventory[slotNumber].getHoverBoxText(heldItem);
					if (s != null)
					{
						this.hoverText = s;
					}
					else
					{
						this.hoverText = this.actualInventory[slotNumber].getDescription();
						this.hoverTitle = this.actualInventory[slotNumber].Name;
					}
					if (toReturn == null)
					{
						toReturn = this.actualInventory[slotNumber];
					}
				}
			}
			return toReturn;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0012B580 File Offset: 0x00129780
		public override void setUpForGamePadMode()
		{
			base.setUpForGamePadMode();
			if (this.inventory != null && this.inventory.Count > 0)
			{
				Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
			}
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0012B60C File Offset: 0x0012980C
		public override int moveCursorInDirection(int direction)
		{
			Rectangle bounds = new Rectangle(this.inventory[0].bounds.X, this.inventory[0].bounds.Y, this.inventory.Last<ClickableComponent>().bounds.X + this.inventory.Last<ClickableComponent>().bounds.Width - this.inventory[0].bounds.X, this.inventory.Last<ClickableComponent>().bounds.Y + this.inventory.Last<ClickableComponent>().bounds.Height - this.inventory[0].bounds.Y);
			if (!bounds.Contains(Game1.getMousePosition()))
			{
				Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
			}
			Point oldPosition = Game1.getMousePosition();
			switch (direction)
			{
			case 0:
				Game1.setMousePosition(oldPosition.X, oldPosition.Y - Game1.tileSize - this.verticalGap);
				break;
			case 1:
				Game1.setMousePosition(oldPosition.X + Game1.tileSize + this.horizontalGap, oldPosition.Y);
				break;
			case 2:
				Game1.setMousePosition(oldPosition.X, oldPosition.Y + Game1.tileSize + this.verticalGap);
				break;
			case 3:
				Game1.setMousePosition(oldPosition.X - Game1.tileSize - this.horizontalGap, oldPosition.Y);
				break;
			}
			if (!bounds.Contains(Game1.getMousePosition()))
			{
				Game1.setMousePosition(oldPosition);
				return direction;
			}
			return -1;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0012B7F4 File Offset: 0x001299F4
		public override void draw(SpriteBatch b)
		{
			if (this.drawSlots)
			{
				for (int i = 0; i < this.capacity; i++)
				{
					Vector2 toDraw = new Vector2((float)(this.xPositionOnScreen + i % (this.capacity / this.rows) * Game1.tileSize + this.horizontalGap * (i % (this.capacity / this.rows))), (float)(this.yPositionOnScreen + i / (this.capacity / this.rows) * (Game1.tileSize + this.verticalGap) + (i / (this.capacity / this.rows) - 1) * Game1.pixelZoom - ((i >= this.capacity / this.rows || !this.playerInventory || this.verticalGap != 0) ? 0 : (Game1.tileSize / 5))));
					b.Draw(Game1.menuTexture, toDraw, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
					if ((this.playerInventory || this.showGrayedOutSlots) && i >= Game1.player.maxItems)
					{
						b.Draw(Game1.menuTexture, toDraw, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 57, -1, -1)), Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
					}
					if (i < 12 && this.playerInventory)
					{
						string strToDraw = (i == 9) ? "0" : ((i == 10) ? "-" : ((i == 11) ? "=" : string.Concat(i + 1)));
						b.DrawString(Game1.tinyFont, strToDraw, toDraw + new Vector2((float)(Game1.tileSize / 2 - 4), (float)(-(float)Game1.tileSize * 2 / 5 - 4)), (i == Game1.player.CurrentToolIndex) ? Color.Red : Color.DimGray);
					}
					if (this.actualInventory.Count > i && this.actualInventory.ElementAt(i) != null)
					{
						this.actualInventory[i].drawInMenu(b, toDraw, (this.inventory.Count > i) ? this.inventory[i].scale : 1f, (!this.highlightMethod(this.actualInventory[i])) ? 0.2f : 1f, 0.865f);
					}
				}
			}
			for (int j = 0; j < this.capacity; j++)
			{
				Vector2 toDraw2 = new Vector2((float)(this.xPositionOnScreen + j % (this.capacity / this.rows) * Game1.tileSize + this.horizontalGap * (j % (this.capacity / this.rows))), (float)(this.yPositionOnScreen + j / (this.capacity / this.rows) * (Game1.tileSize + this.verticalGap) + (j / (this.capacity / this.rows) - 1) * Game1.pixelZoom - ((j >= this.capacity / this.rows || !this.playerInventory || this.verticalGap != 0) ? 0 : (Game1.tileSize / 5))));
				if (this.actualInventory.Count > j && this.actualInventory.ElementAt(j) != null)
				{
					this.actualInventory[j].drawInMenu(b, toDraw2, (this.inventory.Count > j) ? this.inventory[j].scale : 1f, (!this.highlightMethod(this.actualInventory[j])) ? 0.2f : 1f, 0.865f);
				}
			}
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0012BB9F File Offset: 0x00129D9F
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x04000FA6 RID: 4006
		public string hoverText = "";

		// Token: 0x04000FA7 RID: 4007
		public string hoverTitle = "";

		// Token: 0x04000FA8 RID: 4008
		public string descriptionTitle = "";

		// Token: 0x04000FA9 RID: 4009
		public string descriptionText = "";

		// Token: 0x04000FAA RID: 4010
		public List<ClickableComponent> inventory = new List<ClickableComponent>();

		// Token: 0x04000FAB RID: 4011
		public List<Item> actualInventory;

		// Token: 0x04000FAC RID: 4012
		public InventoryMenu.highlightThisItem highlightMethod;

		// Token: 0x04000FAD RID: 4013
		public ItemGrabMenu.behaviorOnItemSelect onAddItem;

		// Token: 0x04000FAE RID: 4014
		private bool playerInventory;

		// Token: 0x04000FAF RID: 4015
		private bool drawSlots;

		// Token: 0x04000FB0 RID: 4016
		public bool showGrayedOutSlots;

		// Token: 0x04000FB1 RID: 4017
		public int capacity;

		// Token: 0x04000FB2 RID: 4018
		public int rows;

		// Token: 0x04000FB3 RID: 4019
		public int horizontalGap;

		// Token: 0x04000FB4 RID: 4020
		public int verticalGap;

		// Token: 0x02000183 RID: 387
		// Token: 0x060013D0 RID: 5072
		public delegate bool highlightThisItem(Item i);
	}
}
