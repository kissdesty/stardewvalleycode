using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x020000F0 RID: 240
	public class GameMenu : IClickableMenu
	{
		// Token: 0x06000E5F RID: 3679 RVA: 0x00125C44 File Offset: 0x00123E44
		public GameMenu() : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
		{
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "inventory", Game1.content.LoadString("Strings\\UI:GameMenu_Inventory", new object[0])));
			this.pages.Add(new InventoryPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "skills", Game1.content.LoadString("Strings\\UI:GameMenu_Skills", new object[0])));
			this.pages.Add(new SkillsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "social", Game1.content.LoadString("Strings\\UI:GameMenu_Social", new object[0])));
			this.pages.Add(new SocialPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "map", Game1.content.LoadString("Strings\\UI:GameMenu_Map", new object[0])));
			this.pages.Add(new MapPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 5, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "crafting", Game1.content.LoadString("Strings\\UI:GameMenu_Crafting", new object[0])));
			this.pages.Add(new CraftingPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 6, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "collections", Game1.content.LoadString("Strings\\UI:GameMenu_Collections", new object[0])));
			this.pages.Add(new CollectionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 7, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "options", Game1.content.LoadString("Strings\\UI:GameMenu_Options", new object[0])));
			this.pages.Add(new OptionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
			this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 8, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "exit", Game1.content.LoadString("Strings\\UI:GameMenu_Exit", new object[0])));
			this.pages.Add(new ExitPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
			if (Game1.activeClickableMenu == null)
			{
				Game1.playSound("bigSelect");
			}
			if (Game1.player.hasOrWillReceiveMail("canReadJunimoText") && !Game1.player.hasOrWillReceiveMail("JojaMember") && !Game1.player.hasCompletedCommunityCenter())
			{
				this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
			}
			GameMenu.forcePreventClose = false;
			if (Game1.options.gamepadControls && Game1.isAnyGamePadButtonBeingPressed())
			{
				this.setUpForGamePadMode();
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x001261E0 File Offset: 0x001243E0
		public GameMenu(int startingTab, int extra = -1) : this()
		{
			this.changeTab(startingTab);
			if (startingTab == 6 && extra != -1)
			{
				(this.pages[6] as OptionsPage).currentItemIndex = extra;
			}
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00126210 File Offset: 0x00124410
		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (b == Buttons.RightTrigger)
			{
				if (this.currentTab == 3)
				{
					Game1.activeClickableMenu = new GameMenu(4, -1);
					return;
				}
				if (this.currentTab < 6 && this.pages[this.currentTab].readyToClose())
				{
					this.changeTab(this.currentTab + 1);
					return;
				}
			}
			else if (b == Buttons.LeftTrigger)
			{
				if (this.currentTab == 3)
				{
					Game1.activeClickableMenu = new GameMenu(2, -1);
					return;
				}
				if (this.currentTab > 0 && this.pages[this.currentTab].readyToClose())
				{
					this.changeTab(this.currentTab - 1);
					return;
				}
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x001262C0 File Offset: 0x001244C0
		public override void setUpForGamePadMode()
		{
			base.setUpForGamePadMode();
			if (this.pages.Count > this.currentTab)
			{
				this.pages[this.currentTab].setUpForGamePadMode();
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x001262F4 File Offset: 0x001244F4
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, playSound);
			if (!this.invisible && !GameMenu.forcePreventClose)
			{
				for (int i = 0; i < this.tabs.Count; i++)
				{
					if (this.tabs[i].containsPoint(x, y) && this.currentTab != i && this.pages[this.currentTab].readyToClose())
					{
						this.changeTab(this.getTabNumberFromName(this.tabs[i].name));
						return;
					}
				}
				if (this.junimoNoteIcon != null && this.junimoNoteIcon.containsPoint(x, y))
				{
					Game1.activeClickableMenu = new JunimoNoteMenu(true, 1, false);
				}
			}
			this.pages[this.currentTab].receiveLeftClick(x, y, true);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x001263C8 File Offset: 0x001245C8
		public static string getLabelOfTabFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Inventory", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Skills", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Social", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Map", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Crafting", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Collections", new object[0]);
			case 6:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Options", new object[0]);
			case 7:
				return Game1.content.LoadString("Strings\\UI:GameMenu_Exit", new object[0]);
			default:
				return "";
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x001264B5 File Offset: 0x001246B5
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.pages[this.currentTab].receiveRightClick(x, y, true);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x001264D0 File Offset: 0x001246D0
		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			this.pages[this.currentTab].receiveScrollWheelAction(direction);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x001264F0 File Offset: 0x001246F0
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.hoverText = "";
			this.pages[this.currentTab].performHoverAction(x, y);
			foreach (ClickableComponent c in this.tabs)
			{
				if (c.containsPoint(x, y))
				{
					this.hoverText = c.label;
					return;
				}
			}
			if (this.junimoNoteIcon != null)
			{
				this.junimoNoteIcon.tryHover(x, y, 0.1f);
				if (this.junimoNoteIcon.containsPoint(x, y))
				{
					this.hoverText = this.junimoNoteIcon.hoverText;
				}
			}
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x001265BC File Offset: 0x001247BC
		public int getTabNumberFromName(string name)
		{
			int whichTab = -1;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 3454868101u)
			{
				if (num <= 3001865938u)
				{
					if (num != 1700191391u)
					{
						if (num == 3001865938u)
						{
							if (name == "social")
							{
								whichTab = 2;
							}
						}
					}
					else if (name == "skills")
					{
						whichTab = 1;
					}
				}
				else if (num != 3048072735u)
				{
					if (num == 3454868101u)
					{
						if (name == "exit")
						{
							whichTab = 7;
						}
					}
				}
				else if (name == "crafting")
				{
					whichTab = 4;
				}
			}
			else if (num <= 3760730054u)
			{
				if (num != 3751997361u)
				{
					if (num == 3760730054u)
					{
						if (name == "collections")
						{
							whichTab = 5;
						}
					}
				}
				else if (name == "map")
				{
					whichTab = 3;
				}
			}
			else if (num != 4012403877u)
			{
				if (num == 4244489279u)
				{
					if (name == "inventory")
					{
						whichTab = 0;
					}
				}
			}
			else if (name == "options")
			{
				whichTab = 6;
			}
			return whichTab;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x001266D9 File Offset: 0x001248D9
		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.pages[this.currentTab].releaseLeftClick(x, y);
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x001266FB File Offset: 0x001248FB
		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			this.pages[this.currentTab].leftClickHeld(x, y);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0012671D File Offset: 0x0012491D
		public override bool readyToClose()
		{
			return !GameMenu.forcePreventClose && this.pages[this.currentTab].readyToClose();
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00126740 File Offset: 0x00124940
		public void changeTab(int whichTab)
		{
			if (this.currentTab == 2)
			{
				if (this.junimoNoteIcon != null)
				{
					this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover", new object[0]), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float)Game1.pixelZoom, false);
				}
			}
			else if (whichTab == 2 && this.junimoNoteIcon != null)
			{
				ClickableTextureComponent expr_AA_cp_0_cp_0 = this.junimoNoteIcon;
				expr_AA_cp_0_cp_0.bounds.X = expr_AA_cp_0_cp_0.bounds.X + Game1.tileSize;
			}
			this.currentTab = this.getTabNumberFromName(this.tabs[whichTab].name);
			if (this.currentTab == 3)
			{
				this.invisible = true;
				this.width += Game1.tileSize * 2;
				base.initializeUpperRightCloseButton();
			}
			else
			{
				this.width = 800 + IClickableMenu.borderWidth * 2;
				base.initializeUpperRightCloseButton();
				this.invisible = false;
			}
			Game1.playSound("smallSelect");
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00126874 File Offset: 0x00124A74
		public override void draw(SpriteBatch b)
		{
			if (!this.invisible)
			{
				if (!Game1.options.showMenuBackground)
				{
					b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
				}
				Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.pages[this.currentTab].width, this.pages[this.currentTab].height, false, true, null, false);
				this.pages[this.currentTab].draw(b);
				b.End();
				b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
				if (!GameMenu.forcePreventClose)
				{
					foreach (ClickableComponent c in this.tabs)
					{
						int sheetIndex = 0;
						string name = c.name;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
						if (num <= 3048072735u)
						{
							if (num <= 2237694710u)
							{
								if (num != 1700191391u)
								{
									if (num == 2237694710u)
									{
										if (name == "catalogue")
										{
											sheetIndex = 7;
										}
									}
								}
								else if (name == "skills")
								{
									sheetIndex = 1;
								}
							}
							else if (num != 3001865938u)
							{
								if (num == 3048072735u)
								{
									if (name == "crafting")
									{
										sheetIndex = 4;
									}
								}
							}
							else if (name == "social")
							{
								sheetIndex = 2;
							}
						}
						else if (num <= 3751997361u)
						{
							if (num != 3454868101u)
							{
								if (num == 3751997361u)
								{
									if (name == "map")
									{
										sheetIndex = 3;
									}
								}
							}
							else if (name == "exit")
							{
								sheetIndex = 7;
							}
						}
						else if (num != 3760730054u)
						{
							if (num != 4012403877u)
							{
								if (num == 4244489279u)
								{
									if (name == "inventory")
									{
										sheetIndex = 0;
									}
								}
							}
							else if (name == "options")
							{
								sheetIndex = 6;
							}
						}
						else if (name == "collections")
						{
							sheetIndex = 5;
						}
						b.Draw(Game1.mouseCursors, new Vector2((float)c.bounds.X, (float)(c.bounds.Y + ((this.currentTab == this.getTabNumberFromName(c.name)) ? 8 : 0))), new Rectangle?(new Rectangle(sheetIndex * 16, 368, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
						if (c.name.Equals("skills"))
						{
							Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float)(c.bounds.X + 8), (float)(c.bounds.Y + 12 + ((this.currentTab == this.getTabNumberFromName(c.name)) ? 8 : 0))), 0.00011f, 3f, 2, Game1.player);
						}
					}
					b.End();
					b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
					if (this.junimoNoteIcon != null)
					{
						this.junimoNoteIcon.draw(b);
					}
					if (!this.hoverText.Equals(""))
					{
						IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
					}
				}
			}
			else
			{
				this.pages[this.currentTab].draw(b);
			}
			if (!GameMenu.forcePreventClose)
			{
				base.draw(b);
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00126CD8 File Offset: 0x00124ED8
		public override bool areGamePadControlsImplemented()
		{
			return this.pages[this.currentTab].gamePadControlsImplemented;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00126CF0 File Offset: 0x00124EF0
		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.menuButton.Contains(new InputButton(key)) && this.readyToClose())
			{
				Game1.exitActiveMenu();
				Game1.playSound("bigDeSelect");
			}
			this.pages[this.currentTab].receiveKeyPress(key);
		}

		// Token: 0x04000F70 RID: 3952
		public const int inventoryTab = 0;

		// Token: 0x04000F71 RID: 3953
		public const int skillsTab = 1;

		// Token: 0x04000F72 RID: 3954
		public const int socialTab = 2;

		// Token: 0x04000F73 RID: 3955
		public const int mapTab = 3;

		// Token: 0x04000F74 RID: 3956
		public const int craftingTab = 4;

		// Token: 0x04000F75 RID: 3957
		public const int collectionsTab = 5;

		// Token: 0x04000F76 RID: 3958
		public const int optionsTab = 6;

		// Token: 0x04000F77 RID: 3959
		public const int exitTab = 7;

		// Token: 0x04000F78 RID: 3960
		public const int numberOfTabs = 7;

		// Token: 0x04000F79 RID: 3961
		public int currentTab;

		// Token: 0x04000F7A RID: 3962
		private string hoverText = "";

		// Token: 0x04000F7B RID: 3963
		private string descriptionText = "";

		// Token: 0x04000F7C RID: 3964
		private List<ClickableComponent> tabs = new List<ClickableComponent>();

		// Token: 0x04000F7D RID: 3965
		private List<IClickableMenu> pages = new List<IClickableMenu>();

		// Token: 0x04000F7E RID: 3966
		public bool invisible;

		// Token: 0x04000F7F RID: 3967
		public static bool forcePreventClose;

		// Token: 0x04000F80 RID: 3968
		private ClickableTextureComponent junimoNoteIcon;
	}
}
