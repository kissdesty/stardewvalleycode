using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x0200011B RID: 283
	public class TutorialMenu : IClickableMenu
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x0014FC90 File Offset: 0x0014DE90
		public TutorialMenu() : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2 - Game1.tileSize * 3, 600 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2 + Game1.tileSize * 3, false)
		{
			int xPos = this.xPositionOnScreen + Game1.tileSize + Game1.tileSize * 2 / 3 - 2;
			int yPos = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Crop Basics", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 276, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Fishing", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 142, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Mining", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 334, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Crafting", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 308, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Construction", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 395, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Making Friends", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 458, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Town Information", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 102, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.topics.Add(new ClickableTextureComponent("", new Rectangle(xPos, yPos, this.width, Game1.tileSize), "Raising Animals", "", Game1.content.Load<Texture2D>("LooseSprites\\TutorialImages\\FarmTut"), Rectangle.Empty, 1f, false));
			this.icons.Add(new ClickableTextureComponent(new Rectangle(xPos, yPos, Game1.tileSize, Game1.tileSize), Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 403, -1, -1), 1f, false));
			yPos += Game1.tileSize + 4;
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 3 / 4, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x001502DC File Offset: 0x0014E4DC
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.currentTab == -1)
			{
				for (int i = 0; i < this.topics.Count; i++)
				{
					if (this.topics[i].containsPoint(x, y))
					{
						this.currentTab = i;
						Game1.playSound("smallSelect");
						break;
					}
				}
			}
			if (this.currentTab != -1 && this.backButton.containsPoint(x, y))
			{
				this.currentTab = -1;
				Game1.playSound("bigDeSelect");
				return;
			}
			if (this.currentTab == -1 && this.okButton.containsPoint(x, y))
			{
				Game1.playSound("bigDeSelect");
				Game1.exitActiveMenu();
				if (Game1.currentLocation.currentEvent != null)
				{
					Event expr_AE = Game1.currentLocation.currentEvent;
					int currentCommand = expr_AE.CurrentCommand;
					expr_AE.CurrentCommand = currentCommand + 1;
				}
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x001503A8 File Offset: 0x0014E5A8
		public override void performHoverAction(int x, int y)
		{
			foreach (ClickableComponent c in this.topics)
			{
				if (c.containsPoint(x, y))
				{
					c.scale = 2f;
				}
				else
				{
					c.scale = 1f;
				}
			}
			if (this.okButton.containsPoint(x, y))
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
			}
			else
			{
				this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
			}
			if (this.backButton.containsPoint(x, y))
			{
				this.backButton.scale = Math.Min(this.backButton.scale + 0.02f, this.backButton.baseScale + 0.1f);
				return;
			}
			this.backButton.scale = Math.Max(this.backButton.scale - 0.02f, this.backButton.baseScale);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x001504F4 File Offset: 0x0014E6F4
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, null, false);
			if (this.currentTab != -1)
			{
				this.backButton.draw(b);
				b.Draw(this.topics[this.currentTab].texture, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder), (float)(this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder - Game1.tileSize / 4)), new Rectangle?(this.topics[this.currentTab].texture.Bounds), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.89f);
			}
			else
			{
				foreach (ClickableTextureComponent c in this.topics)
				{
					Color color = (c.scale > 1f) ? Color.Blue : Game1.textColor;
					b.DrawString(Game1.smallFont, c.label, new Vector2((float)(c.bounds.X + Game1.tileSize + 16), (float)(c.bounds.Y + Game1.tileSize / 3)), color);
				}
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.icons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				this.okButton.draw(b);
			}
			base.drawMouse(b);
		}

		// Token: 0x040011AE RID: 4526
		public const int farmingTab = 0;

		// Token: 0x040011AF RID: 4527
		public const int fishingTab = 1;

		// Token: 0x040011B0 RID: 4528
		public const int miningTab = 2;

		// Token: 0x040011B1 RID: 4529
		public const int craftingTab = 3;

		// Token: 0x040011B2 RID: 4530
		public const int constructionTab = 4;

		// Token: 0x040011B3 RID: 4531
		public const int friendshipTab = 5;

		// Token: 0x040011B4 RID: 4532
		public const int townTab = 6;

		// Token: 0x040011B5 RID: 4533
		public const int animalsTab = 7;

		// Token: 0x040011B6 RID: 4534
		private int currentTab = -1;

		// Token: 0x040011B7 RID: 4535
		private List<ClickableTextureComponent> topics = new List<ClickableTextureComponent>();

		// Token: 0x040011B8 RID: 4536
		private ClickableTextureComponent backButton;

		// Token: 0x040011B9 RID: 4537
		private ClickableTextureComponent okButton;

		// Token: 0x040011BA RID: 4538
		private List<ClickableTextureComponent> icons = new List<ClickableTextureComponent>();
	}
}
