using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x0200010F RID: 271
	public class ShippingMenu : IClickableMenu
	{
		// Token: 0x06000FA9 RID: 4009 RVA: 0x00141BE4 File Offset: 0x0013FDE4
		public ShippingMenu(List<Item> items) : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, false)
		{
			this.parseItems(items);
			if (!Game1.wasRainingYesterday)
			{
				Game1.changeMusicTrack(Game1.currentSeason.Equals("summer") ? "nightTime" : "none");
			}
			this.categoryLabelsWidth = Game1.tileSize * 7;
			this.plusButtonWidth = 10 * Game1.pixelZoom;
			this.itemSlotWidth = 24 * Game1.pixelZoom;
			this.itemAndPlusButtonWidth = this.plusButtonWidth + this.itemSlotWidth + 2 * Game1.pixelZoom;
			this.totalWidth = this.categoryLabelsWidth + this.itemAndPlusButtonWidth;
			this.centerX = Game1.viewport.Width / 2;
			this.centerY = Game1.viewport.Height / 2;
			for (int i = 0; i < 6; i++)
			{
				this.categories.Add(new ClickableTextureComponent("", new Rectangle(this.centerX + this.totalWidth / 2 - this.plusButtonWidth, this.centerY - 25 * Game1.pixelZoom * 3 + i * 27 * Game1.pixelZoom, this.plusButtonWidth, 11 * Game1.pixelZoom), "", this.getCategoryName(i), Game1.mouseCursors, new Rectangle(392, 361, 10, 11), (float)Game1.pixelZoom, false)
				{
					visible = (i < 5 && this.categoryItems[i].Count > 0)
				});
			}
			this.dayPlaqueY = this.categories[0].bounds.Y - Game1.tileSize * 2;
			this.okButton = new ClickableTextureComponent("Done", new Rectangle(this.centerX + this.totalWidth / 2 - this.itemAndPlusButtonWidth + Game1.tileSize / 2, this.centerY + 25 * Game1.pixelZoom * 3 - Game1.tileSize, Game1.tileSize, Game1.tileSize), null, "Done", Game1.mouseCursors, new Rectangle(128, 256, 64, 64), 1f, false);
			this.backButton = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), null, "", Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), null, "", Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			if (Game1.dayOfMonth == 25 && Game1.currentSeason.Equals("winter"))
			{
				Vector2 startingPosition = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(0, 200));
				Rectangle sourceRect = new Rectangle(640, 800, 32, 16);
				int loops = 1000;
				TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 80f, 2, loops, startingPosition, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
				t.motion = new Vector2(-4f, 0f);
				t.delayBeforeAnimationStart = 3000;
				this.animations.Add(t);
			}
			Game1.stats.checkForShippingAchievements();
			if (!Game1.player.achievements.Contains(34) && Utility.hasFarmerShippedAllItems())
			{
				Game1.getAchievement(34);
			}
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00142050 File Offset: 0x00140250
		public void parseItems(List<Item> items)
		{
			Utility.consolidateStacks(items);
			for (int i = 0; i < 6; i++)
			{
				this.categoryItems.Add(new List<Item>());
				this.categoryTotals.Add(0);
				this.categoryDials.Add(new MoneyDial(7, i == 5));
			}
			foreach (Item j in items)
			{
				if (j is Object)
				{
					Object o = j as Object;
					int category = this.getCategoryIndexForObject(o);
					this.categoryItems[category].Add(o);
					List<int> list = this.categoryTotals;
					int index = category;
					list[index] += o.sellToStorePrice() * o.Stack;
					Game1.stats.itemsShipped += (uint)o.Stack;
					if (o.countsForShippedCollection())
					{
						Game1.player.shippedBasic(o.parentSheetIndex, o.stack);
					}
				}
			}
			for (int k = 0; k < 5; k++)
			{
				List<int> list = this.categoryTotals;
				list[5] = list[5] + this.categoryTotals[k];
				this.categoryItems[5].AddRange(this.categoryItems[k]);
				this.categoryDials[k].currentValue = this.categoryTotals[k];
				this.categoryDials[k].previousTargetValue = this.categoryDials[k].currentValue;
			}
			this.categoryDials[5].currentValue = this.categoryTotals[5];
			Game1.player.Money += this.categoryTotals[5];
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00142248 File Offset: 0x00140448
		public int getCategoryIndexForObject(Object o)
		{
			int num = o.parentSheetIndex;
			if (num <= 402)
			{
				if (num != 296 && num != 396 && num != 402)
				{
					goto IL_55;
				}
			}
			else if (num <= 410)
			{
				if (num != 406 && num != 410)
				{
					goto IL_55;
				}
			}
			else if (num != 414 && num != 418)
			{
				goto IL_55;
			}
			return 1;
			IL_55:
			num = o.category;
			if (num <= -23)
			{
				switch (num)
				{
				case -81:
					break;
				case -80:
				case -79:
				case -75:
					return 0;
				case -78:
				case -77:
				case -76:
					return 4;
				default:
					switch (num)
					{
					case -27:
					case -23:
						break;
					case -26:
						return 0;
					case -25:
					case -24:
						return 4;
					default:
						return 4;
					}
					break;
				}
				return 1;
			}
			if (num != -20)
			{
				switch (num)
				{
				case -15:
				case -12:
					break;
				case -14:
					return 0;
				case -13:
					return 4;
				default:
					switch (num)
					{
					case -6:
					case -5:
						return 0;
					case -4:
						return 2;
					case -3:
						return 4;
					case -2:
						break;
					default:
						return 4;
					}
					break;
				}
				return 3;
			}
			return 2;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00142340 File Offset: 0x00140540
		public string getCategoryName(int index)
		{
			switch (index)
			{
			case 0:
				return "Farming";
			case 1:
				return "Foraging";
			case 2:
				return "Fishing";
			case 3:
				return "Mining";
			case 4:
				return "Other";
			case 5:
				return "Total";
			default:
				return "";
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00142398 File Offset: 0x00140598
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.saveGameMenu != null)
			{
				this.saveGameMenu.update(time);
				if (this.saveGameMenu.quit)
				{
					this.saveGameMenu = null;
					this.savedYet = true;
				}
			}
			this.weatherX += (float)time.ElapsedGameTime.Milliseconds * 0.03f;
			for (int i = this.animations.Count - 1; i >= 0; i--)
			{
				if (this.animations[i].update(time))
				{
					this.animations.RemoveAt(i);
				}
			}
			if (this.outro)
			{
				if (this.outroFadeTimer > 0)
				{
					this.outroFadeTimer -= time.ElapsedGameTime.Milliseconds;
				}
				else if (this.outroFadeTimer <= 0 && this.dayPlaqueY < this.centerY - Game1.tileSize)
				{
					if (this.animations.Count > 0)
					{
						this.animations.Clear();
					}
					this.dayPlaqueY += (int)Math.Ceiling((double)((float)time.ElapsedGameTime.Milliseconds * 0.35f));
					if (this.dayPlaqueY >= this.centerY - Game1.tileSize)
					{
						this.outroPauseBeforeDateChange = 700;
					}
				}
				else if (this.outroPauseBeforeDateChange > 0)
				{
					this.outroPauseBeforeDateChange -= time.ElapsedGameTime.Milliseconds;
					if (this.outroPauseBeforeDateChange <= 0)
					{
						this.newDayPlaque = true;
						Game1.playSound("newRecipe");
						if (!Game1.currentSeason.Equals("winter"))
						{
							DelayedAction.playSoundAfterDelay(Game1.isRaining ? "rainsound" : "rooster", 1500);
						}
						this.finalOutroTimer = 2000;
						this.animations.Clear();
						if (!this.savedYet)
						{
							if (this.saveGameMenu == null)
							{
								this.saveGameMenu = new SaveGameMenu();
							}
							return;
						}
					}
				}
				else if (this.finalOutroTimer > 0 && this.savedYet)
				{
					this.finalOutroTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.finalOutroTimer <= 0)
					{
						base.exitThisMenu(false);
					}
				}
			}
			if (this.introTimer >= 0)
			{
				int arg_26E_0 = this.introTimer;
				this.introTimer -= time.ElapsedGameTime.Milliseconds * ((Game1.oldMouseState.LeftButton == ButtonState.Pressed) ? 3 : 1);
				if (arg_26E_0 % 500 < this.introTimer % 500 && this.introTimer <= 3000)
				{
					int categoryThatPoppedUp = 4 - this.introTimer / 500;
					if (categoryThatPoppedUp < 6 && categoryThatPoppedUp > -1)
					{
						if (this.categoryItems[categoryThatPoppedUp].Count > 0)
						{
							Game1.playSound(this.getCategorySound(categoryThatPoppedUp));
							this.categoryDials[categoryThatPoppedUp].currentValue = 0;
							this.categoryDials[categoryThatPoppedUp].previousTargetValue = 0;
						}
						else
						{
							Game1.playSound("stoneStep");
						}
					}
				}
				if (this.introTimer < 0)
				{
					Game1.playSound("money");
					this.categoryDials[5].currentValue = 0;
					this.categoryDials[5].previousTargetValue = 0;
					return;
				}
			}
			else if (Game1.dayOfMonth != 28 && !this.outro)
			{
				if (!Game1.wasRainingYesterday)
				{
					Vector2 startingPosition = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(200));
					Rectangle sourceRect = new Rectangle(640, 752, 16, 16);
					int rows = Game1.random.Next(1, 4);
					if (Game1.random.NextDouble() < 0.001)
					{
						bool flip = Game1.random.NextDouble() < 0.5;
						if (Game1.random.NextDouble() < 0.5)
						{
							this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(640, 826, 16, 8), 40f, 4, 0, new Vector2((float)Game1.random.Next(this.centerX * 2), (float)Game1.random.Next(this.centerY)), false, flip)
							{
								rotation = 3.14159274f,
								scale = (float)Game1.pixelZoom,
								motion = new Vector2((float)(flip ? -8 : 8), 8f),
								local = true
							});
						}
						else
						{
							this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(258, 1680, 16, 16), 40f, 4, 0, new Vector2((float)Game1.random.Next(this.centerX * 2), (float)Game1.random.Next(this.centerY)), false, flip)
							{
								scale = (float)Game1.pixelZoom,
								motion = new Vector2((float)(flip ? -8 : 8), 8f),
								local = true
							});
						}
					}
					else if (Game1.random.NextDouble() < 0.0002)
					{
						startingPosition = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(4, Game1.tileSize * 4));
						TemporaryAnimatedSprite bird = new TemporaryAnimatedSprite(Game1.staminaRect, new Rectangle(0, 0, 1, 1), 9999f, 1, 10000, startingPosition, false, false, 0.01f, 0f, Color.White * (0.25f + (float)Game1.random.NextDouble()), 4f, 0f, 0f, 0f, true);
						bird.motion = new Vector2(-0.25f, 0f);
						this.animations.Add(bird);
					}
					else if (Game1.random.NextDouble() < 5E-05)
					{
						startingPosition = new Vector2((float)Game1.viewport.Width, (float)(Game1.viewport.Height - Game1.tileSize * 3));
						for (int j = 0; j < rows; j++)
						{
							TemporaryAnimatedSprite bird2 = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(60, 101), 4, 100, startingPosition + new Vector2((float)((j + 1) * Game1.random.Next(15, 18)), (float)((j + 1) * -20)), false, false, 0.01f, 0f, Color.Black, 4f, 0f, 0f, 0f, true);
							bird2.motion = new Vector2(-1f, 0f);
							this.animations.Add(bird2);
							bird2 = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(60, 101), 4, 100, startingPosition + new Vector2((float)((j + 1) * Game1.random.Next(15, 18)), (float)((j + 1) * 20)), false, false, 0.01f, 0f, Color.Black, 4f, 0f, 0f, 0f, true);
							bird2.motion = new Vector2(-1f, 0f);
							this.animations.Add(bird2);
						}
					}
					else if (Game1.random.NextDouble() < 1E-05)
					{
						sourceRect = new Rectangle(640, 784, 16, 16);
						TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 75f, 4, 1000, startingPosition, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
						t.motion = new Vector2(-3f, 0f);
						t.yPeriodic = true;
						t.yPeriodicLoopTime = 1000f;
						t.yPeriodicRange = (float)(Game1.tileSize / 8);
						t.shakeIntensity = 0.5f;
						this.animations.Add(t);
					}
				}
				this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.smokeTimer <= 0)
				{
					this.smokeTimer = 50;
					this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(684, 1075, 1, 1), 1000f, 1, 1000, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize * 3 / 4 + Game1.pixelZoom * 3), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.pixelZoom * 5)), false, false)
					{
						color = (Game1.wasRainingYesterday ? Color.SlateGray : Color.White),
						scale = (float)Game1.pixelZoom,
						scaleChange = 0f,
						alphaFade = 0.0025f,
						motion = new Vector2(0f, (float)(-(float)Game1.random.Next(25, 75)) / 100f / 4f),
						acceleration = new Vector2(-0.001f, 0f)
					});
				}
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00142CB4 File Offset: 0x00140EB4
		public string getCategorySound(int which)
		{
			switch (which)
			{
			case 0:
				if (!(this.categoryItems[0][0] as Object).isAnimalProduct())
				{
					return "harvest";
				}
				return "cluck";
			case 1:
				return "leafrustle";
			case 2:
				return "button1";
			case 3:
				return "hammer";
			case 4:
				return "coin";
			case 5:
				return "money";
			default:
				return "stoneStep";
			}
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00142D30 File Offset: 0x00140F30
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			if (this.currentPage == -1)
			{
				this.okButton.tryHover(x, y, 0.1f);
				using (List<ClickableTextureComponent>.Enumerator enumerator = this.categories.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ClickableTextureComponent c = enumerator.Current;
						if (c.containsPoint(x, y))
						{
							c.sourceRect.X = 402;
						}
						else
						{
							c.sourceRect.X = 392;
						}
					}
					return;
				}
			}
			this.backButton.tryHover(x, y, 0.5f);
			this.forwardButton.tryHover(x, y, 0.5f);
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00142DF0 File Offset: 0x00140FF0
		public override void receiveKeyPress(Keys key)
		{
			if (this.introTimer <= 0 && key.Equals(Keys.Escape))
			{
				this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
			}
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00142E50 File Offset: 0x00141050
		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (b == Buttons.B && this.currentPage != -1)
			{
				if (this.currentTab == 0)
				{
					this.currentPage = -1;
				}
				else
				{
					this.currentTab--;
				}
				Game1.playSound("shwip");
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00142EA0 File Offset: 0x001410A0
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.outro && !this.savedYet)
			{
				SaveGameMenu arg_16_0 = this.saveGameMenu;
				return;
			}
			if (this.savedYet)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			if (this.currentPage == -1 && this.okButton.containsPoint(x, y))
			{
				this.outro = true;
				this.outroFadeTimer = 800;
				Game1.playSound("bigDeSelect");
				Game1.changeMusicTrack("none");
			}
			if (this.currentPage == -1)
			{
				for (int i = 0; i < this.categories.Count; i++)
				{
					if (this.categories[i].visible && this.categories[i].containsPoint(x, y))
					{
						this.currentPage = i;
						Game1.playSound("shwip");
						return;
					}
				}
				return;
			}
			if (this.backButton.containsPoint(x, y))
			{
				if (this.currentTab == 0)
				{
					this.currentPage = -1;
				}
				else
				{
					this.currentTab--;
				}
				Game1.playSound("shwip");
				return;
			}
			if (this.showForwardButton() && this.forwardButton.containsPoint(x, y))
			{
				this.currentTab++;
				Game1.playSound("shwip");
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00142FD7 File Offset: 0x001411D7
		public bool showForwardButton()
		{
			return this.categoryItems[this.currentPage].Count > 9 * (this.currentTab + 1);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00142FFC File Offset: 0x001411FC
		public override void draw(SpriteBatch b)
		{
			if (Game1.wasRainingYesterday)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Game1.currentSeason.Equals("winter") ? Color.LightSlateGray : (Color.SlateGray * (1f - (float)this.introTimer / 3500f)));
				b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Game1.currentSeason.Equals("winter") ? Color.LightSlateGray : (Color.SlateGray * (1f - (float)this.introTimer / 3500f)));
				for (int x = -61 * Game1.pixelZoom; x < Game1.viewport.Width + 61 * Game1.pixelZoom; x += 61 * Game1.pixelZoom)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)x + this.weatherX / 2f % (float)(61 * Game1.pixelZoom), (float)(Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.DarkSlateGray * 1f * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(30, 62, 50)) * (0.5f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(30, 62, 50)) * (0.5f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(30, 62, 50)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(30, 62, 50)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize / 2), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				for (int x2 = -61 * Game1.pixelZoom; x2 < Game1.viewport.Width + 61 * Game1.pixelZoom; x2 += 61 * Game1.pixelZoom)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)x2 + this.weatherX % (float)(61 * Game1.pixelZoom), (float)(-(float)Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.SlateGray * 0.85f * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.animations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
				for (int x3 = -61 * Game1.pixelZoom; x3 < Game1.viewport.Width + 61 * Game1.pixelZoom; x3 += 61 * Game1.pixelZoom)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)x3 + this.weatherX * 1.5f % (float)(61 * Game1.pixelZoom), (float)(-(float)Game1.tileSize * 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.LightSlateGray * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
				}
			}
			else
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (1f - (float)this.introTimer / 3500f));
				b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (1f - (float)this.introTimer / 3500f));
				b.Draw(Game1.mouseCursors, new Vector2(0f, 0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), 0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				if (Game1.dayOfMonth == 28)
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.viewport.Width - 44 * Game1.pixelZoom), (float)Game1.pixelZoom), new Rectangle?(new Rectangle(642, 835, 43, 43)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				}
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(0, 20, 40)) * (0.65f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.25f) : new Color(0, 20, 40)) * (0.65f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(0, 32, 20)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(639 * Game1.pixelZoom), (float)(Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? (Color.White * 0.5f) : new Color(0, 32, 20)) * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize * 2 + Game1.tileSize / 2), (float)(Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (1f - (float)this.introTimer / 3500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			}
			if (!this.outro && !Game1.wasRainingYesterday)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.animations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
			}
			if (this.currentPage == -1)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.categories[0].bounds.Y - Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
				int yOffset = -5 * Game1.pixelZoom;
				int i = 0;
				foreach (ClickableTextureComponent c in this.categories)
				{
					if (this.introTimer < 2500 - i * 500)
					{
						Vector2 start = c.getVector2() + new Vector2((float)(Game1.pixelZoom * 3), (float)(-(float)Game1.pixelZoom * 2));
						if (c.visible)
						{
							c.draw(b);
							b.Draw(Game1.mouseCursors, start + new Vector2((float)(-26 * Game1.pixelZoom), (float)(yOffset + Game1.pixelZoom)), new Rectangle?(new Rectangle(293, 360, 24, 24)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
							this.categoryItems[i][0].drawInMenu(b, start + new Vector2((float)(-22 * Game1.pixelZoom), (float)(yOffset + Game1.pixelZoom * 4)), 1f, 1f, 0.9f, false);
						}
						IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), (int)(start.X + (float)(-(float)this.itemSlotWidth) - (float)this.categoryLabelsWidth - (float)(Game1.pixelZoom * 3)), (int)(start.Y + (float)yOffset), this.categoryLabelsWidth, 26 * Game1.pixelZoom, Color.White, (float)Game1.pixelZoom, false);
						SpriteText.drawString(b, c.hoverText, (int)start.X - this.itemSlotWidth - this.categoryLabelsWidth + Game1.pixelZoom * 2, (int)start.Y + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						for (int j = 0; j < 6; j++)
						{
							b.Draw(Game1.mouseCursors, start + new Vector2((float)(-(float)this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 6 + j * 6 * Game1.pixelZoom), (float)(3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(355, 476, 7, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
						}
						this.categoryDials[i].draw(b, start + new Vector2((float)(-(float)this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 12 + Game1.pixelZoom), (float)(5 * Game1.pixelZoom)), this.categoryTotals[i]);
						b.Draw(Game1.mouseCursors, start + new Vector2((float)(-(float)this.itemSlotWidth - Game1.tileSize - Game1.pixelZoom), (float)(3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(408, 476, 9, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
					}
					i++;
				}
				if (this.introTimer <= 0)
				{
					this.okButton.draw(b);
				}
			}
			else
			{
				IClickableMenu.drawTextureBox(b, Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, Color.White);
				Vector2 position = new Vector2((float)(this.xPositionOnScreen + Game1.tileSize / 2), (float)(this.yPositionOnScreen + Game1.tileSize / 2));
				for (int k = this.currentTab * 9; k < this.currentTab * 9 + 9; k++)
				{
					if (this.categoryItems[this.currentPage].Count > k)
					{
						this.categoryItems[this.currentPage][k].drawInMenu(b, position, 1f, 1f, 1f, true);
						SpriteText.drawString(b, this.categoryItems[this.currentPage][k].Name + ((this.categoryItems[this.currentPage][k].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][k].Stack) : ""), (int)position.X + Game1.tileSize + Game1.pixelZoom * 3, (int)position.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						string dots = ".";
						for (int l = 0; l < this.width - Game1.tileSize * 3 / 2 - SpriteText.getWidthOfString(string.Concat(new object[]
						{
							this.categoryItems[this.currentPage][k].Name,
							(this.categoryItems[this.currentPage][k].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][k].Stack) : "",
							(this.categoryItems[this.currentPage][k] as Object).sellToStorePrice() * (this.categoryItems[this.currentPage][k] as Object).Stack,
							"g"
						})); l += SpriteText.getWidthOfString(" ."))
						{
							dots += " .";
						}
						SpriteText.drawString(b, dots, (int)position.X + Game1.tileSize * 5 / 4 + SpriteText.getWidthOfString(this.categoryItems[this.currentPage][k].Name + ((this.categoryItems[this.currentPage][k].Stack > 1) ? (" x" + this.categoryItems[this.currentPage][k].Stack) : "")), (int)position.Y + Game1.tileSize / 8, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						SpriteText.drawString(b, (this.categoryItems[this.currentPage][k] as Object).sellToStorePrice() * (this.categoryItems[this.currentPage][k] as Object).Stack + "g", (int)position.X + this.width - Game1.tileSize - SpriteText.getWidthOfString((this.categoryItems[this.currentPage][k] as Object).sellToStorePrice() * (this.categoryItems[this.currentPage][k] as Object).Stack + "g"), (int)position.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
						position.Y += (float)(Game1.tileSize + Game1.pixelZoom);
					}
				}
				this.backButton.draw(b);
				if (this.showForwardButton())
				{
					this.forwardButton.draw(b);
				}
			}
			if (this.outro)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.Black * (1f - (float)this.outroFadeTimer / 800f));
				SpriteText.drawStringWithScrollCenteredAt(b, this.newDayPlaque ? string.Concat(new object[]
				{
					Game1.dayOfMonth,
					Utility.getNumberEnding(Game1.dayOfMonth),
					" of ",
					Utility.getSeasonNameFromNumber(Utility.getSeasonNumber(Game1.currentSeason)),
					", Year ",
					Game1.year
				}) : Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.dayPlaqueY, "", 1f, -1, 0, 0.88f, false);
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.animations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, true, 0, 0);
					}
				}
				if (this.finalOutroTimer > 0)
				{
					b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * (1f - (float)this.finalOutroTimer / 2000f));
				}
			}
			if (this.saveGameMenu != null)
			{
				this.saveGameMenu.draw(b);
			}
			base.drawMouse(b);
		}

		// Token: 0x040010EB RID: 4331
		public const int farming_category = 0;

		// Token: 0x040010EC RID: 4332
		public const int foraging_category = 1;

		// Token: 0x040010ED RID: 4333
		public const int fishing_category = 2;

		// Token: 0x040010EE RID: 4334
		public const int mining_category = 3;

		// Token: 0x040010EF RID: 4335
		public const int other_category = 4;

		// Token: 0x040010F0 RID: 4336
		public const int total_category = 5;

		// Token: 0x040010F1 RID: 4337
		public const int timePerIntroCategory = 500;

		// Token: 0x040010F2 RID: 4338
		public const int outroFadeTime = 800;

		// Token: 0x040010F3 RID: 4339
		public const int smokeRate = 100;

		// Token: 0x040010F4 RID: 4340
		public const int categorylabelHeight = 25;

		// Token: 0x040010F5 RID: 4341
		public const int itemsPerCategoryPage = 9;

		// Token: 0x040010F6 RID: 4342
		public int currentPage = -1;

		// Token: 0x040010F7 RID: 4343
		public int currentTab;

		// Token: 0x040010F8 RID: 4344
		private List<ClickableTextureComponent> categories = new List<ClickableTextureComponent>();

		// Token: 0x040010F9 RID: 4345
		private ClickableTextureComponent okButton;

		// Token: 0x040010FA RID: 4346
		private ClickableTextureComponent forwardButton;

		// Token: 0x040010FB RID: 4347
		private ClickableTextureComponent backButton;

		// Token: 0x040010FC RID: 4348
		private List<int> categoryTotals = new List<int>();

		// Token: 0x040010FD RID: 4349
		private List<MoneyDial> categoryDials = new List<MoneyDial>();

		// Token: 0x040010FE RID: 4350
		private List<List<Item>> categoryItems = new List<List<Item>>();

		// Token: 0x040010FF RID: 4351
		private int categoryLabelsWidth;

		// Token: 0x04001100 RID: 4352
		private int plusButtonWidth;

		// Token: 0x04001101 RID: 4353
		private int itemSlotWidth;

		// Token: 0x04001102 RID: 4354
		private int itemAndPlusButtonWidth;

		// Token: 0x04001103 RID: 4355
		private int totalWidth;

		// Token: 0x04001104 RID: 4356
		private int centerX;

		// Token: 0x04001105 RID: 4357
		private int centerY;

		// Token: 0x04001106 RID: 4358
		private int introTimer = 3500;

		// Token: 0x04001107 RID: 4359
		private int outroFadeTimer;

		// Token: 0x04001108 RID: 4360
		private int outroPauseBeforeDateChange;

		// Token: 0x04001109 RID: 4361
		private int finalOutroTimer;

		// Token: 0x0400110A RID: 4362
		private int smokeTimer;

		// Token: 0x0400110B RID: 4363
		private int dayPlaqueY;

		// Token: 0x0400110C RID: 4364
		private float weatherX;

		// Token: 0x0400110D RID: 4365
		private bool outro;

		// Token: 0x0400110E RID: 4366
		private bool newDayPlaque;

		// Token: 0x0400110F RID: 4367
		private bool savedYet;

		// Token: 0x04001110 RID: 4368
		public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();

		// Token: 0x04001111 RID: 4369
		private SaveGameMenu saveGameMenu;
	}
}
