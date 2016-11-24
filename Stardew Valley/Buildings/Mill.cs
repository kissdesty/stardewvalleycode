using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;

namespace StardewValley.Buildings
{
	// Token: 0x02000147 RID: 327
	public class Mill : Building
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00178222 File Offset: 0x00176422
		public Mill(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
			this.input = new Chest(true);
			this.output = new Chest(true);
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00178258 File Offset: 0x00176458
		public Mill()
		{
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00178274 File Offset: 0x00176474
		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, 64, this.texture.Bounds.Height);
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0017828F File Offset: 0x0017648F
		public override void load()
		{
			base.load();
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00178298 File Offset: 0x00176498
		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (this.daysOfConstructionLeft <= 0)
			{
				if (tileLocation.X == (float)(this.tileX + 1) && tileLocation.Y == (float)(this.tileY + 1))
				{
					if (who != null && who.ActiveObject != null)
					{
						bool millableItem = false;
						int parentSheetIndex = who.ActiveObject.parentSheetIndex;
						if (parentSheetIndex == 262 || parentSheetIndex == 284)
						{
							millableItem = true;
						}
						if (!millableItem)
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:CantMill", new object[0]));
							return false;
						}
						Item after = Utility.addItemToThisInventoryList(who.ActiveObject, this.input.items, 36);
						if (after != null)
						{
							who.ActiveObject = null;
							who.ActiveObject = (Object)after;
						}
						else
						{
							who.ActiveObject = null;
						}
						this.hasLoadedToday = true;
						Game1.playSound("Ship");
						if (who.ActiveObject != null)
						{
							Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:MillFull", new object[0]));
						}
					}
				}
				else if (tileLocation.X == (float)(this.tileX + 3) && tileLocation.Y == (float)(this.tileY + 1))
				{
					Game1.activeClickableMenu = new ItemGrabMenu(this.output.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromChest), false, true, true, true, true, 1, null, -1, null);
				}
			}
			return base.doAction(tileLocation, who);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00178414 File Offset: 0x00176614
		public override void dayUpdate(int dayOfMonth)
		{
			this.hasLoadedToday = false;
			for (int i = this.input.items.Count - 1; i >= 0; i--)
			{
				if (this.input.items[i] != null)
				{
					Item toAdd = null;
					int parentSheetIndex = this.input.items[i].parentSheetIndex;
					if (parentSheetIndex <= 246)
					{
						if (parentSheetIndex == 245 || parentSheetIndex == 246)
						{
							toAdd = this.input.items[i];
						}
					}
					else if (parentSheetIndex != 262)
					{
						if (parentSheetIndex == 284)
						{
							toAdd = new Object(245, 3 * this.input.items[i].Stack, false, -1, 0);
						}
					}
					else
					{
						toAdd = new Object(246, this.input.items[i].Stack, false, -1, 0);
					}
					if (toAdd != null && Utility.canItemBeAddedToThisInventoryList(toAdd, this.output.items, 36))
					{
						this.input.items[i] = Utility.addItemToThisInventoryList(toAdd, this.output.items, 36);
					}
				}
			}
			base.dayUpdate(dayOfMonth);
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0017854C File Offset: 0x0017674C
		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(this.getSourceRectForMenu()), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
			b.Draw(this.texture, new Vector2((float)(x + Game1.tileSize / 2), (float)(y + Game1.pixelZoom)), new Rectangle?(new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 % 160, (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 / 160 * 32, 32, 32)), this.color, 0f, new Vector2(0f, 0f), 4f, SpriteEffects.None, 0.9f);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00178650 File Offset: 0x00176850
		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.baseSourceRect), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize / 2), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize + Game1.pixelZoom))), new Rectangle?(new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 % 160, (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 / 160 * 32, 32, 32)), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			if (this.hasLoadedToday)
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + 13 * Game1.pixelZoom), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize + Game1.pixelZoom * 69))), new Rectangle?(new Rectangle(64 + (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 700 / 100 * 21, 72, 21, 8)), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			}
			if (this.output.items.Count > 0 && this.output.items[0] != null && (this.output.items[0].parentSheetIndex == 245 || this.output.items[0].parentSheetIndex == 246))
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize * 3), (float)(this.tileY * Game1.tileSize - Game1.tileSize * 3 / 2) + yOffset)), new Rectangle?(new Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + 1) * Game1.tileSize) / 10000f + 1E-06f + (float)this.tileX / 10000f);
				b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize * 3 + Game1.tileSize / 2 + Game1.pixelZoom), (float)(this.tileY * Game1.tileSize - Game1.tileSize + Game1.tileSize / 8) + yOffset)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.output.items[0].parentSheetIndex, 16, 16)), Color.White * 0.75f, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + 1) * Game1.tileSize) / 10000f + 1E-05f + (float)this.tileX / 10000f);
			}
		}

		// Token: 0x0400131E RID: 4894
		public Chest input;

		// Token: 0x0400131F RID: 4895
		public Chest output;

		// Token: 0x04001320 RID: 4896
		private bool hasLoadedToday;

		// Token: 0x04001321 RID: 4897
		private Rectangle baseSourceRect = new Rectangle(0, 0, 64, 128);
	}
}
