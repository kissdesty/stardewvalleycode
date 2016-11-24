using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x0200010B RID: 267
	public class OptionsPage : IClickableMenu
	{
		// Token: 0x06000F84 RID: 3972 RVA: 0x0013DB9C File Offset: 0x0013BD9C
		public OptionsPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upArrow.bounds.Height - Game1.pixelZoom * 2);
			for (int i = 0; i < 7; i++)
			{
				this.optionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((height - Game1.tileSize * 2) / 7), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom), string.Concat(i)));
			}
			this.options.Add(new OptionsElement("General:"));
			this.options.Add(new OptionsCheckbox("Auto Run", 0, -1, -1));
			this.options.Add(new OptionsCheckbox("Show Portraits", 7, -1, -1));
			this.options.Add(new OptionsCheckbox("Show Merchant Portraits", 8, -1, -1));
			this.options.Add(new OptionsCheckbox("Always Show Tool Hit Location", 11, -1, -1));
			this.options.Add(new OptionsCheckbox("Hide Tool Hit Location When Moving", 12, -1, -1));
			this.options.Add(new OptionsCheckbox("Gamepad Placement Tile Indicator", 27, -1, -1));
			this.options.Add(new OptionsCheckbox("Pause When Game Window Is Inactive", 14, -1, -1));
			this.options.Add(new OptionsElement("Sound:"));
			this.options.Add(new OptionsSlider("Music Volume", 1, -1, -1));
			this.options.Add(new OptionsSlider("Sound Volume", 2, -1, -1));
			this.options.Add(new OptionsSlider("Ambient Volume", 20, -1, -1));
			this.options.Add(new OptionsSlider("Footstep Volume", 21, -1, -1));
			this.options.Add(new OptionsCheckbox("Dialogue Typing Sound", 3, -1, -1));
			this.options.Add(new OptionsElement("Graphics:"));
			if (!Game1.conventionMode)
			{
				this.options.Add(new OptionsDropDown("Window Mode", 13, -1, -1));
				this.options.Add(new OptionsDropDown("Resolution", 6, -1, -1));
			}
			this.options.Add(new OptionsCheckbox("Menu Backgrounds", 9, -1, -1));
			this.options.Add(new OptionsCheckbox("Lock Toolbar", 15, -1, -1));
			this.options.Add(new OptionsPlusMinus("Zoom Level", 18, new List<string>
			{
				"75%",
				"80%",
				"85%",
				"90%",
				"95%",
				"100%",
				"105%",
				"110%",
				"115%",
				"120%",
				"125%"
			}, -1, -1));
			this.options.Add(new OptionsCheckbox("Zoom Buttons", 19, -1, -1));
			this.options.Add(new OptionsPlusMinus("Lighting Quality", 25, new List<string>
			{
				"Lowest",
				"Low",
				"Med.",
				"High",
				"Ultra"
			}, -1, -1));
			this.options.Add(new OptionsSlider("Snow Transparency", 23, -1, -1));
			this.options.Add(new OptionsCheckbox("Show Flash Effects", 24, -1, -1));
			this.options.Add(new OptionsCheckbox("Use Hardware Cursor", 26, -1, -1));
			this.options.Add(new OptionsCheckbox("Show Sharper Digits", 28, -1, -1));
			this.options.Add(new OptionsElement("Controls:"));
			this.options.Add(new OptionsCheckbox("Controller Rumble", 16, -1, -1));
			this.options.Add(new OptionsCheckbox("Invert Toolbar Scroll Direction", 22, -1, -1));
			this.options.Add(new OptionsInputListener("Reset Controls To Default", -1, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Check/Do Action", 7, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Use Tool", 10, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Access Menu", 15, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Access Journal", 18, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Access Map", 19, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Move Up", 11, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Move Left", 14, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Move Down", 13, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Move Right", 12, this.optionSlots[0].bounds.Width, -1, -1));
			if (Game1.IsMultiplayer)
			{
				this.options.Add(new OptionsInputListener("Chat Box", 17, this.optionSlots[0].bounds.Width, -1, -1));
			}
			this.options.Add(new OptionsInputListener("Run", 16, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #1", 20, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #2", 21, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #3", 22, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #4", 23, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #5", 24, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #6", 25, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #7", 26, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #8", 27, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #9", 28, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #10", 29, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #11", 30, this.optionSlots[0].bounds.Width, -1, -1));
			this.options.Add(new OptionsInputListener("Inventory Slot #12", 31, this.optionSlots[0].bounds.Width, -1, -1));
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0013E5F8 File Offset: 0x0013C7F8
		private void setScrollBarToCurrentIndex()
		{
			if (this.options.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.options.Count - 7 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
				if (this.currentItemIndex == this.options.Count - 7)
				{
					this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0013E6B4 File Offset: 0x0013C8B4
		public override void leftClickHeld(int x, int y)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_F0_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
				float percentage = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.currentItemIndex = Math.Min(this.options.Count - 7, Math.Max(0, (int)((float)this.options.Count * percentage)));
				this.setScrollBarToCurrentIndex();
				if (arg_F0_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
					return;
				}
			}
			else if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count)
			{
				this.options[this.currentItemIndex + this.optionsSlotHeld].leftClickHeld(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0013E83C File Offset: 0x0013CA3C
		public override void receiveKeyPress(Keys key)
		{
			if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count)
			{
				this.options[this.currentItemIndex + this.optionsSlotHeld].receiveKeyPress(key);
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0013E88C File Offset: 0x0013CA8C
		public override void receiveScrollWheelAction(int direction)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (direction < 0 && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0013E8F8 File Offset: 0x0013CAF8
		public override void releaseLeftClick(int x, int y)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			base.releaseLeftClick(x, y);
			if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count)
			{
				this.options[this.currentItemIndex + this.optionsSlotHeld].leftClickReleased(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
			}
			this.optionsSlotHeld = -1;
			this.scrolling = false;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0013E99D File Offset: 0x0013CB9D
		private void downArrowPressed()
		{
			this.downArrow.scale = this.downArrow.baseScale;
			this.currentItemIndex++;
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0013E9C9 File Offset: 0x0013CBC9
		private void upArrowPressed()
		{
			this.upArrow.scale = this.upArrow.baseScale;
			this.currentItemIndex--;
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0013E9F8 File Offset: 0x0013CBF8
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
			{
				this.downArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.scrollBar.containsPoint(x, y))
			{
				this.scrolling = true;
			}
			else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
			{
				this.scrolling = true;
				this.leftClickHeld(x, y);
				this.releaseLeftClick(x, y);
			}
			this.currentItemIndex = Math.Max(0, Math.Min(this.options.Count - 7, this.currentItemIndex));
			for (int i = 0; i < this.optionSlots.Count; i++)
			{
				if (this.optionSlots[i].bounds.Contains(x, y) && this.currentItemIndex + i < this.options.Count && this.options[this.currentItemIndex + i].bounds.Contains(x - this.optionSlots[i].bounds.X, y - this.optionSlots[i].bounds.Y))
				{
					this.options[this.currentItemIndex + i].receiveLeftClick(x - this.optionSlots[i].bounds.X, y - this.optionSlots[i].bounds.Y);
					this.optionsSlotHeld = i;
					return;
				}
			}
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0013EC0C File Offset: 0x0013CE0C
		public override void performHoverAction(int x, int y)
		{
			if (GameMenu.forcePreventClose)
			{
				return;
			}
			this.descriptionText = "";
			this.hoverText = "";
			this.upArrow.tryHover(x, y, 0.1f);
			this.downArrow.tryHover(x, y, 0.1f);
			this.scrollBar.tryHover(x, y, 0.1f);
			bool arg_5A_0 = this.scrolling;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0013EC74 File Offset: 0x0013CE74
		public override void draw(SpriteBatch b)
		{
			b.End();
			b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
			for (int i = 0; i < this.optionSlots.Count; i++)
			{
				if (this.currentItemIndex >= 0 && this.currentItemIndex + i < this.options.Count)
				{
					this.options[this.currentItemIndex + i].draw(b, this.optionSlots[i].bounds.X, this.optionSlots[i].bounds.Y);
				}
			}
			b.End();
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			if (!GameMenu.forcePreventClose)
			{
				this.upArrow.draw(b);
				this.downArrow.draw(b);
				if (this.options.Count > 7)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, false);
					this.scrollBar.draw(b);
				}
			}
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x040010BF RID: 4287
		public const int itemsPerPage = 7;

		// Token: 0x040010C0 RID: 4288
		public const int indexOfGraphicsPage = 6;

		// Token: 0x040010C1 RID: 4289
		private string descriptionText = "";

		// Token: 0x040010C2 RID: 4290
		private string hoverText = "";

		// Token: 0x040010C3 RID: 4291
		private List<ClickableComponent> optionSlots = new List<ClickableComponent>();

		// Token: 0x040010C4 RID: 4292
		public int currentItemIndex;

		// Token: 0x040010C5 RID: 4293
		private ClickableTextureComponent upArrow;

		// Token: 0x040010C6 RID: 4294
		private ClickableTextureComponent downArrow;

		// Token: 0x040010C7 RID: 4295
		private ClickableTextureComponent scrollBar;

		// Token: 0x040010C8 RID: 4296
		private bool scrolling;

		// Token: 0x040010C9 RID: 4297
		private List<OptionsElement> options = new List<OptionsElement>();

		// Token: 0x040010CA RID: 4298
		private Rectangle scrollBarRunner;

		// Token: 0x040010CB RID: 4299
		private int optionsSlotHeld = -1;
	}
}
