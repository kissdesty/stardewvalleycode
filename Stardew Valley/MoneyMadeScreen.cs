using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000033 RID: 51
	public class MoneyMadeScreen
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0003628C File Offset: 0x0003448C
		public MoneyMadeScreen(List<Object> shippingItems, int timeOfDay)
		{
			if (timeOfDay < 2000)
			{
				this.day = true;
			}
			int itemOfTheDay = Utility.getRandomItemFromSeason(Game1.currentSeason, 0, false);
			int cropOfTheWeek = Game1.cropsOfTheWeek[(Game1.dayOfMonth - 1) / 7];
			foreach (Object o in shippingItems)
			{
				ShippedItem s = new ShippedItem(o.ParentSheetIndex, o.Price, o.name);
				int price = o.Price * o.Stack;
				if (o.ParentSheetIndex == itemOfTheDay)
				{
					price = (int)((float)price * 1.2f);
				}
				if (o.ParentSheetIndex == cropOfTheWeek)
				{
					price = (int)((float)price * 1.1f);
				}
				if (o.Name.Contains("="))
				{
					price += price / 2;
				}
				price -= price % 5;
				if (this.shippingItems.ContainsKey(s))
				{
					Dictionary<ShippedItem, int> arg_EB_0 = this.shippingItems;
					ShippedItem key = s;
					int num = arg_EB_0[key];
					arg_EB_0[key] = num + 1;
				}
				else
				{
					this.shippingItems.Add(s, o.Stack);
				}
				this.total += price;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000363EC File Offset: 0x000345EC
		public void update(int milliseconds, bool keyDown)
		{
			if (!this.complete)
			{
				this.timeOnCurrentItem += (keyDown ? (milliseconds * 2) : milliseconds);
				if (this.timeOnCurrentItem >= 200)
				{
					this.currentItemIndex++;
					Game1.playSound("shiny4");
					this.timeOnCurrentItem = 0;
					if (this.currentItemIndex == this.shippingItems.Count)
					{
						this.complete = true;
					}
				}
			}
			else
			{
				this.timeOnCurrentItem += (keyDown ? (milliseconds * 2) : milliseconds);
				if (this.timeOnCurrentItem >= 1000)
				{
					this.canProceed = true;
				}
			}
			if (this.throbUp)
			{
				this.starScale += 0.01f;
			}
			else
			{
				this.starScale -= 0.01f;
			}
			if (this.starScale >= 1.2f)
			{
				this.throbUp = false;
				return;
			}
			if (this.starScale <= 1f)
			{
				this.throbUp = true;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000364E0 File Offset: 0x000346E0
		public void draw(GameTime gametime)
		{
			if (this.day)
			{
				Game1.graphics.GraphicsDevice.Clear(Utility.getSkyColorForSeason(Game1.currentSeason));
			}
			Game1.drawTitleScreenBackground(gametime, this.day ? "_day" : "_night", Utility.weatherDebrisOffsetForSeason(Game1.currentSeason));
			int outerheight = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.tileSize * 2;
			int x = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.X + Game1.graphics.GraphicsDevice.Viewport.Width / 2 - (int)((float)((this.shippingItems.Count / (outerheight / Game1.tileSize - 4) + 1) * Game1.tileSize) * 3f);
			int y = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + Game1.tileSize;
			int width = (int)((float)((this.shippingItems.Count / (outerheight / Game1.tileSize - 4) + 1) * Game1.tileSize) * 6f);
			Game1.drawDialogueBox(x, y, width, outerheight, false, true, null, false);
			int height = outerheight - Game1.tileSize * 3;
			Point topLeftCorner = new Point(x + Game1.tileSize, y + Game1.tileSize);
			for (int i = 0; i < this.currentItemIndex; i++)
			{
				Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float)(topLeftCorner.X + i * Game1.tileSize / (height - Game1.tileSize * 2) * Game1.tileSize * 4 + Game1.tileSize / 2), (float)(i * Game1.tileSize % (height - Game1.tileSize * 2) - i * Game1.tileSize % (height - Game1.tileSize * 2) % Game1.tileSize + Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + Game1.tileSize * 3 + Game1.tileSize / 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.shippingItems.Keys.ElementAt(i).index, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), this.shippingItems.Keys.ElementAt(i).name.Contains("=") ? this.starScale : 1f, SpriteEffects.None, 0.999999f);
				Game1.spriteBatch.DrawString(Game1.dialogueFont, string.Concat(new object[]
				{
					"x",
					this.shippingItems[this.shippingItems.Keys.ElementAt(i)],
					" : ",
					this.shippingItems.Keys.ElementAt(i).price * this.shippingItems[this.shippingItems.Keys.ElementAt(i)],
					"g"
				}), new Vector2((float)(topLeftCorner.X + i * Game1.tileSize / (height - Game1.tileSize * 2) * Game1.tileSize * 4 + Game1.tileSize), (float)(i * Game1.tileSize % (height - Game1.tileSize * 2) - i * Game1.tileSize % (height - Game1.tileSize * 2) % Game1.tileSize + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString("9").Y / 2f + (float)Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y + (float)(Game1.tileSize * 3)), Game1.textColor);
			}
			if (this.complete)
			{
				Game1.spriteBatch.DrawString(Game1.dialogueFont, "Total: " + this.total + "g", new Vector2((float)(x + width - Game1.tileSize) - Game1.dialogueFont.MeasureString("Total: " + this.total).X, (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 5 / 2)), Game1.textColor);
			}
		}

		// Token: 0x040002BD RID: 701
		private const int timeToDisplayEachItem = 200;

		// Token: 0x040002BE RID: 702
		private Dictionary<ShippedItem, int> shippingItems = new Dictionary<ShippedItem, int>();

		// Token: 0x040002BF RID: 703
		public bool complete;

		// Token: 0x040002C0 RID: 704
		public bool canProceed;

		// Token: 0x040002C1 RID: 705
		public bool throbUp;

		// Token: 0x040002C2 RID: 706
		public bool day;

		// Token: 0x040002C3 RID: 707
		private int currentItemIndex;

		// Token: 0x040002C4 RID: 708
		private int timeOnCurrentItem;

		// Token: 0x040002C5 RID: 709
		private int total;

		// Token: 0x040002C6 RID: 710
		private float starScale = 1f;
	}
}
