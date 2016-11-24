using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;

namespace StardewValley.Menus
{
	// Token: 0x02000113 RID: 275
	public class SocialPage : IClickableMenu
	{
		// Token: 0x06000FD9 RID: 4057 RVA: 0x00148B64 File Offset: 0x00146D64
		public SocialPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.friendNames = new List<ClickableTextureComponent>();
			this.npcNames = new Dictionary<string, string>();
			foreach (NPC i in Utility.getAllCharacters())
			{
				if ((!i.name.Equals("Sandy") || Game1.player.mailReceived.Contains("ccVault")) && !i.name.Equals("???") && !i.name.Equals("Bouncer") && !i.name.Equals("Marlon") && !i.name.Equals("Gil") && !i.name.Equals("Gunther") && !i.name.Equals("Henchman") && !i.IsMonster && !(i is Horse) && !(i is Pet) && !(i is JunimoHarvester))
				{
					if (Game1.player.friendships.ContainsKey(i.name))
					{
						string info = i.datingFarmer ? "true" : "false";
						info += (i.datable ? "_true" : "_false");
						this.friendNames.Add(new ClickableTextureComponent(i.name, new Rectangle(x + IClickableMenu.borderWidth + Game1.pixelZoom, 0, width, Game1.tileSize), null, info, i.sprite.Texture, i.getMugShotSourceRect(), (float)Game1.pixelZoom, false));
						this.npcNames[i.name] = i.getName();
						if (i is Child)
						{
							this.kidsNames.Add(i.name);
						}
					}
					else if (!i.name.Equals("Dwarf") && !i.name.Contains("Qi") && !(i is Pet) && !(i is Horse) && !(i is Junimo) && !(i is Child))
					{
						this.friendNames.Add(new ClickableTextureComponent(i.name, new Rectangle(x + IClickableMenu.borderWidth + Game1.pixelZoom, 0, width, Game1.tileSize), null, "false_false", i.sprite.Texture, i.getMugShotSourceRect(), (float)Game1.pixelZoom, false));
						this.npcNames[i.name] = "???";
						if (i is Child)
						{
							this.kidsNames.Add(i.name);
						}
					}
				}
			}
			IEnumerable<ClickableTextureComponent> arg_30C_0 = this.friendNames;
			Func<ClickableTextureComponent, int> arg_30C_1;
			if ((arg_30C_1 = SocialPage.<>c.<>9__12_0) == null)
			{
				arg_30C_1 = (SocialPage.<>c.<>9__12_0 = new Func<ClickableTextureComponent, int>(SocialPage.<>c.<>9.<.ctor>b__12_0));
			}
			this.friendNames = arg_30C_0.OrderBy(arg_30C_1).ToList<ClickableTextureComponent>();
			this.upButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upButton.bounds.X + Game1.pixelZoom * 3, this.upButton.bounds.Y + this.upButton.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upButton.bounds.Y + this.upButton.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upButton.bounds.Height - Game1.pixelZoom * 2);
			this.updateSlots();
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00149058 File Offset: 0x00147258
		public void updateSlots()
		{
			int index = 0;
			for (int i = this.slotPosition; i < this.slotPosition + 5; i++)
			{
				if (this.friendNames.Count > i)
				{
					int y = this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize / 2 + (Game1.tileSize + Game1.tileSize * 3 / 4) * index + Game1.pixelZoom * 8;
					this.friendNames[i].bounds.Y = y;
				}
				index++;
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x001490D8 File Offset: 0x001472D8
		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_E8_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upButton.bounds.Height + Game1.pixelZoom * 5));
				float percentage = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.slotPosition = Math.Min(this.friendNames.Count - 5, Math.Max(0, (int)((float)this.friendNames.Count * percentage)));
				this.setScrollBarToCurrentIndex();
				if (arg_E8_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
				}
			}
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x001491D9 File Offset: 0x001473D9
		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.scrolling = false;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x001491EC File Offset: 0x001473EC
		private void setScrollBarToCurrentIndex()
		{
			if (this.friendNames.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.friendNames.Count - 5 + 1) * this.slotPosition + this.upButton.bounds.Bottom + Game1.pixelZoom;
				if (this.slotPosition == this.friendNames.Count - 5)
				{
					this.scrollBar.bounds.Y = this.downButton.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
			this.updateSlots();
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x001492AC File Offset: 0x001474AC
		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.slotPosition > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (direction < 0 && this.slotPosition < Math.Max(0, this.friendNames.Count - 5))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
			}
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0014930D File Offset: 0x0014750D
		public void upArrowPressed()
		{
			this.slotPosition--;
			this.updateSlots();
			this.upButton.scale = 3.5f;
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00149339 File Offset: 0x00147539
		public void downArrowPressed()
		{
			this.slotPosition++;
			this.updateSlots();
			this.downButton.scale = 3.5f;
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00149368 File Offset: 0x00147568
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.upButton.containsPoint(x, y) && this.slotPosition > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.downButton.containsPoint(x, y) && this.slotPosition < this.friendNames.Count - 5)
			{
				this.downArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.scrollBar.containsPoint(x, y))
			{
				this.scrolling = true;
			}
			else if (!this.downButton.containsPoint(x, y) && x > this.xPositionOnScreen + this.width - Game1.tileSize * 3 / 2 && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
			{
				this.scrolling = true;
				this.leftClickHeld(x, y);
				this.releaseLeftClick(x, y);
			}
			this.slotPosition = Math.Max(0, Math.Min(this.friendNames.Count - 5, this.slotPosition));
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00149488 File Offset: 0x00147688
		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			this.upButton.tryHover(x, y, 0.1f);
			this.downButton.tryHover(x, y, 0.1f);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x001494C4 File Offset: 0x001476C4
		public override void draw(SpriteBatch b)
		{
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 2 + Game1.pixelZoom, true);
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 3 + Game1.tileSize / 2 + Game1.pixelZoom * 5, true);
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 9, true);
			base.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 6 + Game1.tileSize / 2 + Game1.pixelZoom * 13, true);
			base.drawVerticalPartition(b, this.xPositionOnScreen + Game1.tileSize * 4 + Game1.pixelZoom * 3, true);
			base.drawVerticalPartition(b, this.xPositionOnScreen + Game1.tileSize * 4 + Game1.pixelZoom * 3 + 85 * Game1.pixelZoom, true);
			for (int i = this.slotPosition; i < this.slotPosition + 5; i++)
			{
				if (i < this.friendNames.Count)
				{
					this.friendNames[i].draw(b);
					int heartLevel = Game1.player.getFriendshipHeartLevelForNPC(this.friendNames[i].name);
					bool single = this.friendNames[i].hoverText.Split(new char[]
					{
						'_'
					})[1].Equals("true");
					bool spouse = Game1.player.spouse != null && Game1.player.spouse.Equals(this.friendNames[i].name);
					b.DrawString(Game1.dialogueFont, this.npcNames[this.friendNames[i].name], new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize - Game1.pixelZoom * 5 + Game1.tileSize * 3 / 2) - Game1.dialogueFont.MeasureString(this.npcNames[this.friendNames[i].name]).X / 2f, (float)(this.friendNames[i].bounds.Y + Game1.tileSize * 3 / 4 - (single ? (Game1.pixelZoom * 6) : (Game1.pixelZoom * 5)))), Game1.textColor);
					for (int hearts = 0; hearts < 10 + (this.friendNames[i].name.Equals(Game1.player.spouse) ? 2 : 0); hearts++)
					{
						int xSource = (hearts < heartLevel) ? 211 : 218;
						if (single && !this.friendNames[i].hoverText.Split(new char[]
						{
							'_'
						})[0].Equals("true") && !spouse && hearts >= 8)
						{
							xSource = 211;
						}
						if (hearts < 10)
						{
							b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 - Game1.pixelZoom * 2 + hearts * (8 * Game1.pixelZoom)), (float)(this.friendNames[i].bounds.Y + Game1.tileSize - Game1.pixelZoom * 7)), new Rectangle?(new Rectangle(xSource, 428, 7, 6)), (single && !this.friendNames[i].hoverText.Split(new char[]
							{
								'_'
							})[0].Equals("true") && !spouse && hearts >= 8) ? (Color.Black * 0.35f) : Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						}
						else
						{
							b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 5 - Game1.pixelZoom * 2 + (hearts - 10) * (8 * Game1.pixelZoom)), (float)(this.friendNames[i].bounds.Y + Game1.tileSize)), new Rectangle?(new Rectangle(xSource, 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						}
					}
					if (single)
					{
						string text = "(single)";
						if (spouse && Game1.getCharacterFromName(this.friendNames[i].name, false) != null)
						{
							text = ((Game1.getCharacterFromName(this.friendNames[i].name, false).gender == 0) ? "(husband)" : "(wife)");
						}
						else if (!Game1.player.isMarried() && this.friendNames[i].hoverText.Split(new char[]
						{
							'_'
						})[0].Equals("true") && Game1.getCharacterFromName(this.friendNames[i].name, false) != null)
						{
							text = ((Game1.getCharacterFromName(this.friendNames[i].name, false).gender == 0) ? "(boyfriend)" : "(girlfriend)");
						}
						else if (Game1.getCharacterFromName(this.friendNames[i].name, false).divorcedFromFarmer)
						{
							text = "(ex)";
						}
						b.DrawString(Game1.smallFont, text, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 3 + Game1.pixelZoom * 2) - Game1.smallFont.MeasureString(text).X / 2f, (float)this.friendNames[i].bounds.Bottom), Game1.textColor);
					}
					if (Game1.player.friendships.ContainsKey(this.friendNames[i].name) && (Game1.player.spouse == null || !this.friendNames[i].name.Equals(Game1.player.spouse)) && !this.kidsNames.Contains(this.friendNames[i].name))
					{
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6 + 66 * Game1.pixelZoom), (float)(this.friendNames[i].bounds.Y + Game1.tileSize / 2 - Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(229, 410, 14, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6 + 81 * Game1.pixelZoom), (float)(this.friendNames[i].bounds.Y + Game1.tileSize / 2)), new Rectangle?(new Rectangle(227 + ((Game1.player.friendships[this.friendNames[i].name][1] == 2) ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + Game1.tileSize * 6 + 91 * Game1.pixelZoom), (float)(this.friendNames[i].bounds.Y + Game1.tileSize / 2)), new Rectangle?(new Rectangle(227 + ((Game1.player.friendships[this.friendNames[i].name][1] >= 1) ? 9 : 0), 425, 9, 9)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
					}
					if (Game1.player.spouse != null && this.friendNames[i].name.Equals(Game1.player.spouse))
					{
						b.Draw(Game1.objectSpriteSheet, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize * 3), (float)this.friendNames[i].bounds.Y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 460, 16, 16)), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.88f);
					}
					else if (this.friendNames[i].hoverText.Split(new char[]
					{
						'_'
					})[0].Equals("true"))
					{
						b.Draw(Game1.objectSpriteSheet, new Vector2((float)(this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize * 3), (float)this.friendNames[i].bounds.Y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 458, 16, 16)), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.88f);
					}
				}
			}
			this.upButton.draw(b);
			this.downButton.draw(b);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, true);
			this.scrollBar.draw(b);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04001142 RID: 4418
		public const int slotsOnPage = 5;

		// Token: 0x04001143 RID: 4419
		private string descriptionText = "";

		// Token: 0x04001144 RID: 4420
		private string hoverText = "";

		// Token: 0x04001145 RID: 4421
		private ClickableTextureComponent upButton;

		// Token: 0x04001146 RID: 4422
		private ClickableTextureComponent downButton;

		// Token: 0x04001147 RID: 4423
		private ClickableTextureComponent scrollBar;

		// Token: 0x04001148 RID: 4424
		private Rectangle scrollBarRunner;

		// Token: 0x04001149 RID: 4425
		private List<ClickableTextureComponent> friendNames;

		// Token: 0x0400114A RID: 4426
		private int slotPosition;

		// Token: 0x0400114B RID: 4427
		private List<string> kidsNames = new List<string>();

		// Token: 0x0400114C RID: 4428
		private Dictionary<string, string> npcNames;

		// Token: 0x0400114D RID: 4429
		private bool scrolling;

		// Token: 0x02000187 RID: 391
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060013E1 RID: 5089 RVA: 0x0018A395 File Offset: 0x00188595
			internal int <.ctor>b__12_0(ClickableTextureComponent i)
			{
				return -Game1.player.getFriendshipLevelForNPC(i.name);
			}

			// Token: 0x040014CC RID: 5324
			public static readonly SocialPage.<>c <>9 = new SocialPage.<>c();

			// Token: 0x040014CD RID: 5325
			public static Func<ClickableTextureComponent, int> <>9__12_0;
		}
	}
}
