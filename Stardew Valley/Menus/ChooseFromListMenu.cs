using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x020000E0 RID: 224
	public class ChooseFromListMenu : IClickableMenu
	{
		// Token: 0x06000DDB RID: 3547 RVA: 0x0011B700 File Offset: 0x00119900
		public ChooseFromListMenu(List<string> options, ChooseFromListMenu.actionOnChoosingListOption chooseAction, bool isJukebox = false) : base(Game1.viewport.Width / 2 - 320, Game1.viewport.Height - Game1.tileSize - 192, 640, 192, false)
		{
			this.chooseAction = chooseAction;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 640 + Game1.pixelZoom * 4 + Game1.tileSize, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Rectangle(175, 379, 16, 15), (float)Game1.pixelZoom, false);
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 3 + Game1.pixelZoom * 3, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
			Game1.playSound("bigSelect");
			this.isJukebox = isJukebox;
			if (isJukebox)
			{
				for (int i = options.Count - 1; i >= 0; i--)
				{
					if (options[i].ToLower().Contains("ambient"))
					{
						options.RemoveAt(i);
					}
					else
					{
						string text = options[i];
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num <= 1611928003u)
						{
							if (num != 575982768u)
							{
								if (num != 1176080900u)
								{
									if (num == 1611928003u)
									{
										if (text == "buglevelloop")
										{
											options.RemoveAt(i);
										}
									}
								}
								else if (text == "jojaOfficeSoundscape")
								{
									options.RemoveAt(i);
								}
							}
							else if (text == "title_day")
							{
								options.RemoveAt(i);
								options.Add("MainTheme");
							}
						}
						else if (num <= 3528263180u)
						{
							if (num != 3332712824u)
							{
								if (num == 3528263180u)
								{
									if (text == "coin")
									{
										options.RemoveAt(i);
									}
								}
							}
							else if (text == "nightTime")
							{
								options.RemoveAt(i);
							}
						}
						else if (num != 3564132753u)
						{
							if (num == 3819582179u)
							{
								if (text == "communityCenter")
								{
									options.RemoveAt(i);
								}
							}
						}
						else if (text == "ocean")
						{
							options.RemoveAt(i);
						}
					}
				}
			}
			this.options = options;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0011BA84 File Offset: 0x00119C84
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - 320;
			this.yPositionOnScreen = Game1.viewport.Height - Game1.tileSize - 192;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 640 + Game1.pixelZoom * 4 + Game1.tileSize, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, new Rectangle(175, 379, 16, 15), (float)Game1.pixelZoom, false);
			this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 3 + Game1.pixelZoom * 3, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0011BC75 File Offset: 0x00119E75
		public static void playSongAction(string s)
		{
			Game1.changeMusicTrack(s);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0011BC80 File Offset: 0x00119E80
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.okButton.tryHover(x, y, 0.1f);
			this.cancelButton.tryHover(x, y, 0.1f);
			this.backButton.tryHover(x, y, 0.1f);
			this.forwardButton.tryHover(x, y, 0.1f);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0011BCE0 File Offset: 0x00119EE0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (this.okButton.containsPoint(x, y) && this.chooseAction != null)
			{
				this.chooseAction(this.options[this.index]);
				Game1.playSound("select");
			}
			if (this.cancelButton.containsPoint(x, y))
			{
				base.exitThisMenu(true);
			}
			if (this.backButton.containsPoint(x, y))
			{
				this.index--;
				if (this.index < 0)
				{
					this.index = this.options.Count - 1;
				}
				this.backButton.scale = this.backButton.baseScale - 1f;
				Game1.playSound("shwip");
			}
			if (this.forwardButton.containsPoint(x, y))
			{
				this.index++;
				this.index %= this.options.Count;
				Game1.playSound("shwip");
				this.forwardButton.scale = this.forwardButton.baseScale - 1f;
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0011BE04 File Offset: 0x0011A004
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			string maxWidthJukeboxString = "Summer (The Sun Can Bend An Orange Sky)";
			int stringWidth = (int)Game1.dialogueFont.MeasureString(this.isJukebox ? maxWidthJukeboxString : this.options[this.index]).X;
			IClickableMenu.drawTextureBox(b, this.xPositionOnScreen + this.width / 2 - stringWidth / 2 - Game1.pixelZoom * 4, this.yPositionOnScreen + Game1.tileSize - Game1.pixelZoom, stringWidth + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 4, Color.White);
			if (this.index < this.options.Count)
			{
				Utility.drawTextWithShadow(b, this.isJukebox ? Utility.getSongTitleFromCueName(this.options[this.index]) : this.options[this.index], Game1.dialogueFont, new Vector2((float)(this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.isJukebox ? Utility.getSongTitleFromCueName(this.options[this.index]) : this.options[this.index]).X / 2f, (float)(this.yPositionOnScreen + this.height / 2 - Game1.pixelZoom * 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
			}
			this.okButton.draw(b);
			this.cancelButton.draw(b);
			this.forwardButton.draw(b);
			this.backButton.draw(b);
			if (this.isJukebox)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\UI:JukeboxMenu_Title", new object[0]), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen - Game1.tileSize / 2, "", 1f, -1, 0, 0.88f, false);
			}
			base.drawMouse(b);
		}

		// Token: 0x04000EB9 RID: 3769
		public const int w = 640;

		// Token: 0x04000EBA RID: 3770
		public const int h = 192;

		// Token: 0x04000EBB RID: 3771
		private ClickableTextureComponent backButton;

		// Token: 0x04000EBC RID: 3772
		private ClickableTextureComponent forwardButton;

		// Token: 0x04000EBD RID: 3773
		private ClickableTextureComponent okButton;

		// Token: 0x04000EBE RID: 3774
		private ClickableTextureComponent cancelButton;

		// Token: 0x04000EBF RID: 3775
		private List<string> options = new List<string>();

		// Token: 0x04000EC0 RID: 3776
		private int index;

		// Token: 0x04000EC1 RID: 3777
		private ChooseFromListMenu.actionOnChoosingListOption chooseAction;

		// Token: 0x04000EC2 RID: 3778
		private bool isJukebox;

		// Token: 0x02000180 RID: 384
		// Token: 0x060013C4 RID: 5060
		public delegate void actionOnChoosingListOption(string s);
	}
}
