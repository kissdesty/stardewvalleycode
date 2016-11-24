using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Minigames;

namespace StardewValley.Menus
{
	// Token: 0x02000118 RID: 280
	public class TitleMenu : IClickableMenu
	{
		// Token: 0x06001015 RID: 4117 RVA: 0x0014B858 File Offset: 0x00149A58
		public TitleMenu() : base(0, 0, Game1.viewport.Width, Game1.viewport.Height, false)
		{
			this.cloudsTexture = this.temporaryContent.Load<Texture2D>(Path.Combine("Minigames", "Clouds"));
			this.titleButtonsTexture = this.temporaryContent.Load<Texture2D>(Path.Combine("Minigames", "TitleButtons"));
			this.viewportY = 0f;
			this.fadeFromWhiteTimer = 4000;
			this.logoFadeTimer = 5000;
			this.chuckleFishTimer = 4000;
			this.bigClouds.Add(-750f);
			this.bigClouds.Add((float)(this.width * 3 / 4));
			this.shades = (Game1.random.NextDouble() < 0.5);
			this.smallClouds.Add((float)(this.width / 2));
			this.smallClouds.Add((float)(this.width - 1));
			this.smallClouds.Add(1f);
			this.smallClouds.Add((float)(this.width / 3));
			this.smallClouds.Add((float)(this.width * 2 / 3));
			this.smallClouds.Add((float)(this.width * 3 / 4));
			this.smallClouds.Add((float)(this.width / 4));
			this.smallClouds.Add((float)(this.width / 2 + 300));
			this.smallClouds.Add((float)(this.width - 1 + 300));
			this.smallClouds.Add(301f);
			this.smallClouds.Add((float)(this.width / 3 + 300));
			this.smallClouds.Add((float)(this.width * 2 / 3 + 300));
			this.smallClouds.Add((float)(this.width * 3 / 4 + 300));
			this.smallClouds.Add((float)(this.width / 4 + 300));
			if (Game1.currentSong == null && Game1.nextMusicTrack != null)
			{
				int arg_28D_0 = Game1.nextMusicTrack.Length;
			}
			this.birds.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 227, 26, 21), new Vector2((float)(this.width - 210), (float)(this.height - 390)), false, 0f, Color.White)
			{
				scale = 3f,
				pingPong = true,
				animationLength = 4,
				interval = 100f,
				totalNumberOfLoops = 9999,
				local = true,
				motion = new Vector2(-1f, 0f),
				layerDepth = 0.25f
			});
			this.birds.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 227, 26, 21), new Vector2((float)(this.width - 120), (float)(this.height - 360)), false, 0f, Color.White)
			{
				scale = 3f,
				pingPong = true,
				animationLength = 4,
				interval = 100f,
				totalNumberOfLoops = 9999,
				local = true,
				delayBeforeAnimationStart = 100,
				motion = new Vector2(-1f, 0f),
				layerDepth = 0.25f
			});
			this.setUpIcons();
			this.muteMusicButton = new ClickableTextureComponent(new Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(128, 384, 9, 9), (float)Game1.pixelZoom, false);
			this.windowedButton = new ClickableTextureComponent(new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle((Game1.options != null && !Game1.options.isCurrentlyWindowed()) ? 155 : 146, 384, 9, 9), (float)Game1.pixelZoom, false);
			this.startupPreferences = new StartupPreferences();
			this.startupPreferences.loadPreferences();
			if (this.startupPreferences.startMuted)
			{
				Utility.toggleMuteMusic();
				if (this.muteMusicButton.sourceRect.X == 128)
				{
					this.muteMusicButton.sourceRect.X = 137;
				}
				else
				{
					this.muteMusicButton.sourceRect.X = 128;
				}
			}
			if (this.startupPreferences.skipWindowPreparation)
			{
				this.windowNumber = -1;
			}
			int timesPlayed = this.startupPreferences.timesPlayed;
			if (timesPlayed <= 30)
			{
				switch (timesPlayed)
				{
				case 2:
					this.startupMessage = "Welcome back!";
					break;
				case 3:
					this.startupMessage = "Plug in an Xbox One or Xbox 360 controller to activate game pad mode.";
					break;
				case 4:
					this.startupMessage = "Tip: Giving gifts is a good way to make friends with the residents of Stardew Valley.";
					break;
				case 5:
					this.startupMessage = "Tip: Shift-click to purchase 5 items at a time.";
					break;
				case 6:
					this.startupMessage = "After upgrading your hoe or watering can, hold down the tool button to increase the area of effect!";
					break;
				case 7:
					this.startupMessage = "Tip: Right-click to use a weapon's special move.";
					break;
				case 8:
					this.startupMessage = "Watch TV shows to learn useful tips, cooking recipes, and more!";
					break;
				case 9:
					this.startupMessage = "The traveling merchant visits Stardew Valley once a week. She sells a wide variety of goods, often at exorbitant prices!";
					break;
				case 10:
					this.startupMessage = "Thanks for playing!";
					break;
				case 11:
				case 12:
				case 13:
				case 14:
				case 16:
				case 17:
				case 18:
				case 19:
					break;
				case 15:
				{
					string noun = Dialogue.getRandomNoun();
					string noun2 = Dialogue.getRandomNoun();
					this.startupMessage = string.Concat(new string[]
					{
						"Elliott's latest sentence:",
						Environment.NewLine,
						"The ",
						Dialogue.getRandomAdjective(),
						" ",
						noun,
						" ",
						Dialogue.getRandomVerb(),
						" ",
						Dialogue.getRandomPositional(),
						" the ",
						noun.Equals(noun2) ? ("other " + noun2) : noun2
					});
					break;
				}
				case 20:
					this.startupMessage = "<";
					break;
				default:
					if (timesPlayed == 30)
					{
						this.startupMessage = "Challenge: Beat Journey Of The Prairie King without dying.";
					}
					break;
				}
			}
			else if (timesPlayed != 100)
			{
				if (timesPlayed != 1000)
				{
					if (timesPlayed == 10000)
					{
						this.startupMessage = "You've started the game 10,000 times! I'm impressed. -Ape";
					}
				}
				else
				{
					this.startupMessage = "You've started the game 1000 times!";
				}
			}
			else
			{
				this.startupMessage = "You've started the game 100 times.";
			}
			this.startupPreferences.savePreferences();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0014BF98 File Offset: 0x0014A198
		public void skipToTitleButtons()
		{
			this.logoFadeTimer = 0;
			this.logoSwipeTimer = 0f;
			this.titleInPosition = false;
			this.pauseBeforeViewportRiseTimer = 0;
			this.fadeFromWhiteTimer = 0;
			this.viewportY = -999f;
			this.viewportDY = -0.01f;
			this.birds.Clear();
			this.logoSwipeTimer = 1f;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0014BFF8 File Offset: 0x0014A1F8
		public void setUpIcons()
		{
			this.buttons.Clear();
			this.buttons.Add(new ClickableTextureComponent("New", new Rectangle(this.width / 2 - 333 - 48, this.height - 174 - 24, 222, 174), null, "", this.titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false));
			this.buttons.Add(new ClickableTextureComponent("Load", new Rectangle(this.width / 2 - 111 - 24, this.height - 174 - 24, 222, 174), null, "", this.titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false));
			this.buttons.Add(new ClickableTextureComponent("Exit", new Rectangle(this.width / 2 + 111, this.height - 174 - 24, 222, 174), null, "", this.titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false));
			int zoom = (this.height < 800) ? 2 : 3;
			this.eRect = new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom, 42 * zoom, 68 * zoom);
			this.populateLeafRects();
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.width + -198 - 48, this.height - 81 - 24, 198, 81), null, "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false);
			this.aboutButton = new ClickableTextureComponent("About", new Rectangle(this.width + -66 - 48, this.height - 75 - 24, 66, 75), null, "", this.titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false);
			this.skipButton = new ClickableComponent(new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201), "Skip");
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0014C280 File Offset: 0x0014A480
		public void populateLeafRects()
		{
			int zoom = (this.height < 800) ? 2 : 3;
			this.leafRects = new List<Rectangle>();
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom - 196 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom + 109 * zoom, 17 * zoom, 30 * zoom));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom + 91 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom - 26 * zoom, 17 * zoom, 31 * zoom));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom + 79 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom + 83 * zoom, 25 * zoom, 17 * zoom));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom - 213 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom - 24 * zoom, 14 * zoom, 23 * zoom));
			this.leafRects.Add(new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom - 234 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom - 11 * zoom, 18 * zoom, 12 * zoom));
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0014C461 File Offset: 0x0014A661
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			if (this.subMenu != null)
			{
				this.subMenu.receiveRightClick(x, y, true);
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool readyToClose()
		{
			return false;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0014C482 File Offset: 0x0014A682
		public override void leftClickHeld(int x, int y)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			base.leftClickHeld(x, y);
			if (this.subMenu != null)
			{
				this.subMenu.leftClickHeld(x, y);
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0014C4AA File Offset: 0x0014A6AA
		public override void releaseLeftClick(int x, int y)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			base.releaseLeftClick(x, y);
			if (this.subMenu != null)
			{
				this.subMenu.releaseLeftClick(x, y);
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0014C4D4 File Offset: 0x0014A6D4
		public override void receiveKeyPress(Keys key)
		{
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			if (!Program.releaseBuild && key == Keys.N && Game1.oldKBState.IsKeyDown(Keys.RightShift) && Game1.oldKBState.IsKeyDown(Keys.LeftControl))
			{
				Game1.loadForNewGame(false);
				Game1.saveOnNewDay = false;
				Game1.player.eventsSeen.Add(60367);
				Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
				Game1.player.position = new Vector2(7f, 9f) * (float)Game1.tileSize;
				Game1.NewDay(0f);
				Game1.exitActiveMenu();
				Game1.setGameMode(3);
				return;
			}
			if (this.logoFadeTimer > 0 && (key == Keys.B || key == Keys.Escape))
			{
				this.bCount++;
				if (key == Keys.Escape)
				{
					this.bCount += 3;
				}
				if (this.bCount >= 3)
				{
					Game1.playSound("bigDeSelect");
					this.logoFadeTimer = 0;
					this.fadeFromWhiteTimer = 0;
					Game1.delayedActions.Clear();
					this.pauseBeforeViewportRiseTimer = 0;
					this.fadeFromWhiteTimer = 0;
					this.viewportY = -999f;
					this.viewportDY = -0.01f;
					this.birds.Clear();
					this.logoSwipeTimer = 1f;
					this.chuckleFishTimer = 0;
					Game1.changeMusicTrack("MainTheme");
				}
			}
			if (Game1.options.doesInputListContain(Game1.options.menuButton, key))
			{
				return;
			}
			if (this.subMenu != null)
			{
				this.subMenu.receiveKeyPress(key);
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0014C668 File Offset: 0x0014A868
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.muteMusicButton.containsPoint(x, y))
			{
				this.startupPreferences.startMuted = Utility.toggleMuteMusic();
				if (this.muteMusicButton.sourceRect.X == 128)
				{
					this.muteMusicButton.sourceRect.X = 137;
				}
				else
				{
					this.muteMusicButton.sourceRect.X = 128;
				}
				Game1.playSound("drumkit6");
				this.startupPreferences.savePreferences();
				return;
			}
			if (this.windowedButton.containsPoint(x, y))
			{
				if (!Game1.options.isCurrentlyWindowed())
				{
					Game1.options.setWindowedOption("Windowed");
					this.windowedButton.sourceRect.X = 146;
					this.startupPreferences.startWindowed = true;
				}
				else
				{
					Game1.options.setWindowedOption("Windowed Borderless");
					this.windowedButton.sourceRect.X = 155;
					this.startupPreferences.startWindowed = false;
				}
				this.startupPreferences.savePreferences();
				Game1.playSound("drumkit6");
				return;
			}
			if (this.logoFadeTimer > 0 && this.skipButton.containsPoint(x, y) && this.chuckleFishTimer <= 0)
			{
				if (this.logoSurprisedTimer <= 0)
				{
					this.logoSurprisedTimer = 1500;
					string soundtoPlay = "fishSlap";
					Game1.changeMusicTrack("none");
					int num = Game1.random.Next(2);
					if (num != 0)
					{
						if (num == 1)
						{
							soundtoPlay = "fishSlap";
						}
					}
					else
					{
						soundtoPlay = "Duck";
					}
					Game1.playSound(soundtoPlay);
				}
				else if (this.logoSurprisedTimer > 1)
				{
					this.logoSurprisedTimer = Math.Max(1, this.logoSurprisedTimer - 500);
				}
			}
			if (this.chuckleFishTimer > 500)
			{
				this.chuckleFishTimer = 500;
			}
			if (this.logoFadeTimer > 0 || this.fadeFromWhiteTimer > 0)
			{
				return;
			}
			if (this.transitioningCharacterCreationMenu)
			{
				return;
			}
			if (this.subMenu != null)
			{
				if (!this.isTransitioningButtons)
				{
					this.subMenu.receiveLeftClick(x, y, true);
				}
				if (this.backButton.containsPoint(x, y))
				{
					Game1.playSound("bigDeSelect");
					this.buttonsDX = -1;
					if (this.subMenu is AboutMenu)
					{
						this.subMenu = null;
						this.buttonsDX = 0;
						return;
					}
					this.isTransitioningButtons = true;
					if (this.subMenu is LoadGameMenu)
					{
						this.transitioningFromLoadScreen = true;
					}
					this.subMenu = null;
					Game1.changeMusicTrack("spring_day_ambient");
					return;
				}
			}
			else
			{
				if (this.logoFadeTimer <= 0 && !this.titleInPosition && this.logoSwipeTimer == 0f)
				{
					this.pauseBeforeViewportRiseTimer = 0;
					this.fadeFromWhiteTimer = 0;
					this.viewportY = -999f;
					this.viewportDY = -0.01f;
					this.birds.Clear();
					this.logoSwipeTimer = 1f;
					return;
				}
				foreach (ClickableTextureComponent c in this.buttons)
				{
					if (c.containsPoint(x, y))
					{
						this.performButtonAction(c.name);
					}
				}
				if (this.aboutButton.containsPoint(x, y))
				{
					this.subMenu = new AboutMenu();
					Game1.playSound("newArtifact");
				}
				if (this.clicksOnLeaf >= 10 && Game1.random.NextDouble() < 0.001)
				{
					Game1.playSound("junimoMeep1");
				}
				if (this.titleInPosition && this.eRect.Contains(x, y) && this.clicksOnE < 10)
				{
					this.clicksOnE++;
					Game1.playSound("woodyStep");
					if (this.clicksOnE == 10)
					{
						int zoom = (this.height < 800) ? 2 : 3;
						Game1.playSound("openChest");
						this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(0, 491, 42, 68), new Vector2((float)(this.width / 2 - 200 * zoom + 251 * zoom), (float)(-300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom)), false, 0f, Color.White)
						{
							scale = (float)zoom,
							animationLength = 9,
							interval = 200f,
							local = true,
							holdLastFrame = true
						});
						return;
					}
				}
				else if (this.titleInPosition)
				{
					bool clicked = false;
					foreach (Rectangle r in this.leafRects)
					{
						if (r.Contains(x, y))
						{
							clicked = true;
							break;
						}
					}
					if (clicked)
					{
						this.clicksOnLeaf++;
						if (this.clicksOnLeaf == 10)
						{
							int zoom2 = (this.height < 800) ? 2 : 3;
							Game1.playSound("discoverMineral");
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(264, 464, 16, 16), new Vector2((float)(this.width / 2 - 200 * zoom2 + 80 * zoom2), (float)(-300 * zoom2 - (int)(this.viewportY / 3f) * zoom2 + 10 * zoom2 + 2)), false, 0f, Color.White)
							{
								scale = (float)zoom2,
								animationLength = 8,
								interval = 80f,
								totalNumberOfLoops = 999999,
								local = true,
								holdLastFrame = false,
								delayBeforeAnimationStart = 200
							});
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float)(this.width / 2 - 200 * zoom2 + 80 * zoom2), (float)(-300 * zoom2 - (int)(this.viewportY / 3f) * zoom2 + 10 * zoom2)), false, 0f, Color.White)
							{
								scale = (float)zoom2,
								animationLength = 8,
								interval = 50f,
								local = true,
								holdLastFrame = false
							});
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(200, 464, 16, 16), new Vector2((float)(this.width / 2 - 200 * zoom2 + 178 * zoom2), (float)(-300 * zoom2 - (int)(this.viewportY / 3f) * zoom2 + 141 * zoom2 + 2)), false, 0f, Color.White)
							{
								scale = (float)zoom2,
								animationLength = 4,
								interval = 150f,
								totalNumberOfLoops = 999999,
								local = true,
								holdLastFrame = false,
								delayBeforeAnimationStart = 400
							});
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float)(this.width / 2 - 200 * zoom2 + 178 * zoom2), (float)(-300 * zoom2 - (int)(this.viewportY / 3f) * zoom2 + 141 * zoom2)), false, 0f, Color.White)
							{
								scale = (float)zoom2,
								animationLength = 8,
								interval = 50f,
								local = true,
								holdLastFrame = false,
								delayBeforeAnimationStart = 200
							});
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 464, 16, 16), new Vector2((float)(this.width / 2 - 200 * zoom2 + 294 * zoom2), (float)(-300 * zoom2 - (int)(this.viewportY / 3f) * zoom2 + 89 * zoom2 + 2)), false, 0f, Color.White)
							{
								scale = (float)zoom2,
								animationLength = 4,
								interval = 150f,
								totalNumberOfLoops = 999999,
								local = true,
								holdLastFrame = false,
								delayBeforeAnimationStart = 600
							});
							this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float)(this.width / 2 - 200 * zoom2 + 294 * zoom2), (float)(-300 * zoom2 - (int)(this.viewportY / 3f) * zoom2 + 89 * zoom2)), false, 0f, Color.White)
							{
								scale = (float)zoom2,
								animationLength = 8,
								interval = 50f,
								local = true,
								holdLastFrame = false,
								delayBeforeAnimationStart = 400
							});
							return;
						}
						Game1.playSound("leafrustle");
						int zoom3 = (this.height < 800) ? 2 : 3;
						for (int i = 0; i < 2; i++)
						{
							this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(355, 1199 + Game1.random.Next(-1, 2) * 16, 16, 16), new Vector2((float)(x + Game1.random.Next(-8, 9)), (float)(y + Game1.random.Next(-8, 9))), Game1.random.NextDouble() < 0.5, 0f, Color.White)
							{
								scale = (float)zoom3,
								animationLength = 11,
								interval = (float)(50 + Game1.random.Next(50)),
								totalNumberOfLoops = 999,
								motion = new Vector2((float)Game1.random.Next(-100, 101) / 100f, 1f + (float)Game1.random.Next(-100, 100) / 500f),
								xPeriodic = (Game1.random.NextDouble() < 0.5),
								xPeriodicLoopTime = (float)Game1.random.Next(6000, 16000),
								xPeriodicRange = (float)Game1.random.Next(Game1.tileSize, Game1.tileSize * 3),
								alphaFade = 0.001f,
								local = true,
								holdLastFrame = false,
								delayBeforeAnimationStart = i * 20
							});
						}
					}
				}
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0014D118 File Offset: 0x0014B318
		public void performButtonAction(string which)
		{
			this.whichSubMenu = which;
			if (which == "New")
			{
				this.buttonsDX = 1;
				this.isTransitioningButtons = true;
				Game1.resetPlayer();
				Game1.playSound("select");
				this.subMenu = new CharacterCustomization(new List<int>
				{
					0,
					1,
					2,
					3,
					4,
					5
				}, new List<int>
				{
					0,
					1,
					2,
					3,
					4,
					5
				}, new List<int>
				{
					0,
					1,
					2,
					3,
					4,
					5
				}, false);
				Game1.changeMusicTrack("CloudCountry");
				Game1.player.favoriteThing = "";
				return;
			}
			if (!(which == "Load"))
			{
				if (!(which == "Co-op"))
				{
					if (!(which == "Exit"))
					{
						return;
					}
					Game1.playSound("bigDeSelect");
					Game1.changeMusicTrack("none");
					this.quitTimer = 500;
				}
				return;
			}
			this.buttonsDX = 1;
			this.isTransitioningButtons = true;
			Game1.playSound("select");
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0014D274 File Offset: 0x0014B474
		private void addRightLeafGust()
		{
			if (this.isTransitioningButtons || this.tempSprites.Count<TemporaryAnimatedSprite>() > 0)
			{
				return;
			}
			int zoom = (this.height < 800) ? 2 : 3;
			this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 187, 27, 21), new Vector2((float)(this.width / 2 - 200 * zoom + 327 * zoom), (float)(-300 * zoom) - this.viewportY / 3f * (float)zoom + (float)(107 * zoom)), false, 0f, Color.White)
			{
				scale = (float)zoom,
				pingPong = true,
				animationLength = 3,
				interval = 100f,
				totalNumberOfLoops = 3,
				local = true
			});
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0014D34C File Offset: 0x0014B54C
		private void addLeftLeafGust()
		{
			if (this.isTransitioningButtons || this.tempSprites.Count<TemporaryAnimatedSprite>() > 0)
			{
				return;
			}
			int zoom = (this.height < 800) ? 2 : 3;
			this.tempSprites.Add(new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 208, 22, 18), new Vector2((float)(this.width / 2 - 200 * zoom + 16 * zoom), (float)(-300 * zoom) - this.viewportY / 3f * (float)zoom + (float)(16 * zoom)), false, 0f, Color.White)
			{
				scale = (float)zoom,
				pingPong = true,
				animationLength = 3,
				interval = 100f,
				totalNumberOfLoops = 3,
				local = true
			});
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0014D420 File Offset: 0x0014B620
		public void createdNewCharacter(bool skipIntro)
		{
			Game1.playSound("smallSelect");
			this.subMenu = null;
			this.transitioningCharacterCreationMenu = true;
			if (skipIntro)
			{
				Game1.loadForNewGame(false);
				Game1.saveOnNewDay = true;
				Game1.player.eventsSeen.Add(60367);
				Game1.player.currentLocation = Utility.getHomeOfFarmer(Game1.player);
				Game1.player.position = new Vector2(7f, 9f) * (float)Game1.tileSize;
				Game1.NewDay(0f);
				Game1.exitActiveMenu();
				Game1.setGameMode(3);
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0014D4B8 File Offset: 0x0014B6B8
		public override void update(GameTime time)
		{
			if (this.windowNumber > (this.startupPreferences.startWindowed ? 0 : 1))
			{
				if (this.windowNumber % 2 == 0)
				{
					Game1.options.setWindowedOption("Windowed Borderless");
				}
				else
				{
					Game1.options.setWindowedOption("Windowed");
				}
				this.windowNumber--;
			}
			base.update(time);
			if (this.subMenu != null)
			{
				this.subMenu.update(time);
			}
			if (this.transitioningCharacterCreationMenu)
			{
				this.globalCloudAlpha -= (float)time.ElapsedGameTime.Milliseconds * 0.001f;
				if (this.globalCloudAlpha <= 0f)
				{
					this.transitioningCharacterCreationMenu = false;
					this.globalCloudAlpha = 0f;
					this.subMenu = null;
					Game1.currentMinigame = new GrandpaStory();
					Game1.exitActiveMenu();
					Game1.setGameMode(3);
				}
			}
			if (this.quitTimer > 0)
			{
				this.quitTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.quitTimer <= 0)
				{
					Game1.quit = true;
					Game1.exitActiveMenu();
				}
			}
			if (this.chuckleFishTimer > 0)
			{
				this.chuckleFishTimer -= time.ElapsedGameTime.Milliseconds;
			}
			else if (this.logoFadeTimer > 0)
			{
				if (this.logoSurprisedTimer > 0)
				{
					this.logoSurprisedTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.logoSurprisedTimer <= 0)
					{
						this.logoFadeTimer = 1;
					}
				}
				else
				{
					int old = this.logoFadeTimer;
					this.logoFadeTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.logoFadeTimer < 4000 & old >= 4000)
					{
						Game1.playSound("mouseClick");
					}
					if (this.logoFadeTimer < 2500 & old >= 2500)
					{
						Game1.playSound("mouseClick");
					}
					if (this.logoFadeTimer < 2000 & old >= 2000)
					{
						Game1.playSound("mouseClick");
					}
					if (this.logoFadeTimer <= 0)
					{
						Game1.changeMusicTrack("MainTheme");
					}
				}
			}
			else if (this.fadeFromWhiteTimer > 0)
			{
				this.fadeFromWhiteTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.fadeFromWhiteTimer <= 0)
				{
					this.pauseBeforeViewportRiseTimer = 3500;
				}
			}
			else if (this.pauseBeforeViewportRiseTimer > 0)
			{
				this.pauseBeforeViewportRiseTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.pauseBeforeViewportRiseTimer <= 0)
				{
					this.viewportDY = -0.05f;
				}
			}
			this.viewportY += this.viewportDY;
			if (this.viewportDY < 0f)
			{
				this.viewportDY -= 0.006f;
			}
			if (this.viewportY <= -1000f)
			{
				if (this.viewportDY != 0f)
				{
					this.logoSwipeTimer = 1000f;
					this.showButtonsTimer = 250;
				}
				this.viewportDY = 0f;
			}
			if (this.logoSwipeTimer > 0f)
			{
				this.logoSwipeTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.logoSwipeTimer <= 0f)
				{
					this.addLeftLeafGust();
					this.addRightLeafGust();
					this.titleInPosition = true;
					this.buttonsToShow = 3;
					int zoom = (this.height < 800) ? 2 : 3;
					this.eRect = new Rectangle(this.width / 2 - 200 * zoom + 251 * zoom, -300 * zoom - (int)(this.viewportY / 3f) * zoom + 26 * zoom, 42 * zoom, 68 * zoom);
					this.populateLeafRects();
				}
			}
			if (this.showButtonsTimer > 0)
			{
				this.showButtonsTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.showButtonsTimer <= 0 && this.buttonsToShow < 3)
				{
					this.buttonsToShow++;
					Game1.playSound("Cowboy_gunshot");
					this.showButtonsTimer = 250;
				}
			}
			if (this.titleInPosition && !this.isTransitioningButtons && this.globalXOffset == 0f && Game1.random.NextDouble() < 0.005)
			{
				if (Game1.random.NextDouble() < 0.5)
				{
					this.addLeftLeafGust();
				}
				else
				{
					this.addRightLeafGust();
				}
			}
			if (this.titleInPosition && this.isTransitioningButtons)
			{
				this.globalXOffset += (float)(this.buttonsDX * time.ElapsedGameTime.Milliseconds);
				this.moveFeatures(this.buttonsDX * time.ElapsedGameTime.Milliseconds, 0);
				if (this.buttonsDX > 0 && this.globalXOffset > (float)this.width)
				{
					this.buttonsDX = 0;
					this.isTransitioningButtons = false;
					if (this.subMenu == null && this.whichSubMenu.Equals("Load"))
					{
						this.subMenu = new LoadGameMenu();
						Game1.changeMusicTrack("title_night");
					}
					this.whichSubMenu = "";
				}
				else if (this.buttonsDX < 0 && this.globalXOffset <= 0f)
				{
					this.globalXOffset = 0f;
					this.isTransitioningButtons = false;
					this.buttonsDX = 0;
					this.setUpIcons();
					this.whichSubMenu = "";
					this.transitioningFromLoadScreen = false;
				}
			}
			for (int i = this.bigClouds.Count - 1; i >= 0; i--)
			{
				List<float> list = this.bigClouds;
				int index = i;
				list[index] -= 0.1f;
				list = this.bigClouds;
				index = i;
				list[index] += (float)(this.buttonsDX * time.ElapsedGameTime.Milliseconds / 2);
				if (this.bigClouds[i] < -1536f)
				{
					this.bigClouds[i] = (float)this.width;
				}
			}
			for (int j = this.smallClouds.Count - 1; j >= 0; j--)
			{
				List<float> list = this.smallClouds;
				int index = j;
				list[index] -= 0.3f;
				list = this.smallClouds;
				index = j;
				list[index] += (float)(this.buttonsDX * time.ElapsedGameTime.Milliseconds / 2);
				if (this.smallClouds[j] < -384f)
				{
					this.smallClouds[j] = (float)this.width;
				}
			}
			for (int k = this.tempSprites.Count - 1; k >= 0; k--)
			{
				if (this.tempSprites[k].update(time))
				{
					this.tempSprites.RemoveAt(k);
				}
			}
			for (int l = this.birds.Count - 1; l >= 0; l--)
			{
				TemporaryAnimatedSprite expr_705_cp_0_cp_0 = this.birds[l];
				expr_705_cp_0_cp_0.position.Y = expr_705_cp_0_cp_0.position.Y - this.viewportDY * 2f;
				if (this.birds[l].update(time))
				{
					this.birds.RemoveAt(l);
				}
			}
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0014DC08 File Offset: 0x0014BE08
		private void moveFeatures(int dx, int dy)
		{
			foreach (TemporaryAnimatedSprite expr_15 in this.tempSprites)
			{
				expr_15.position.X = expr_15.position.X + (float)dx;
				expr_15.position.Y = expr_15.position.Y + (float)dy;
			}
			foreach (ClickableTextureComponent expr_64 in this.buttons)
			{
				expr_64.bounds.X = expr_64.bounds.X + dx;
				expr_64.bounds.Y = expr_64.bounds.Y + dy;
			}
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0014DCD0 File Offset: 0x0014BED0
		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (this.subMenu != null)
			{
				this.subMenu.receiveScrollWheelAction(direction);
			}
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0014DCF0 File Offset: 0x0014BEF0
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.muteMusicButton.tryHover(x, y, 0.1f);
			if (this.subMenu != null)
			{
				this.subMenu.performHoverAction(x, y);
				if (this.backButton.containsPoint(x, y))
				{
					if (this.backButton.sourceRect.Y == 252)
					{
						Game1.playSound("Cowboy_Footstep");
					}
					this.backButton.sourceRect.Y = 279;
				}
				else
				{
					this.backButton.sourceRect.Y = 252;
				}
				this.backButton.tryHover(x, y, 0.25f);
				return;
			}
			if (this.titleInPosition)
			{
				foreach (ClickableTextureComponent c in this.buttons)
				{
					if (c.containsPoint(x, y))
					{
						if (c.sourceRect.Y == 187)
						{
							Game1.playSound("Cowboy_Footstep");
						}
						c.sourceRect.Y = 245;
					}
					else
					{
						c.sourceRect.Y = 187;
					}
					c.tryHover(x, y, 0.25f);
				}
				this.aboutButton.tryHover(x, y, 0.25f);
				if (this.aboutButton.containsPoint(x, y))
				{
					if (this.aboutButton.sourceRect.X == 8)
					{
						Game1.playSound("Cowboy_Footstep");
					}
					this.aboutButton.sourceRect.X = 30;
					return;
				}
				this.aboutButton.sourceRect.X = 8;
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0014DE9C File Offset: 0x0014C09C
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), new Color(64, 136, 248));
			b.Draw(Game1.mouseCursors, new Rectangle(0, (int)(-900f - this.viewportY * 0.66f), this.width, 900 + this.height - 360), new Rectangle?(new Rectangle(703, 1912, 1, 264)), Color.White);
			if (!this.whichSubMenu.Equals("Load"))
			{
				b.Draw(Game1.mouseCursors, new Vector2(-30f, -1080f - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * (1f - this.globalXOffset / 1200f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
			}
			foreach (float f in this.bigClouds)
			{
				b.Draw(this.cloudsTexture, new Vector2(f, (float)(this.height - 750) - this.viewportY * 0.5f), new Rectangle?(new Rectangle(0, 0, 512, 337)), Color.White * this.globalCloudAlpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
			}
			b.Draw(Game1.mouseCursors, new Vector2(-90f, (float)(this.height - 474) - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 886, 639, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.08f);
			b.Draw(Game1.mouseCursors, new Vector2(1827f, (float)(this.height - 474) - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 886, 640, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.08f);
			for (int i = 0; i < this.smallClouds.Count; i++)
			{
				b.Draw(this.cloudsTexture, new Vector2(this.smallClouds[i], (float)(this.height - 900 - i * 16 * 3) - this.viewportY * 0.5f), new Rectangle?((i % 2 == 0) ? new Rectangle(152, 447, 123, 55) : new Rectangle(410, 467, 63, 37)), Color.White * this.globalCloudAlpha, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
			}
			b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(this.height - 444) - this.viewportY * 1f), new Rectangle?(new Rectangle(0, 737, 639, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.1f);
			b.Draw(Game1.mouseCursors, new Vector2(1917f, (float)(this.height - 444) - this.viewportY * 1f), new Rectangle?(new Rectangle(0, 737, 640, 148)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.1f);
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = this.birds.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.draw(b, false, 0, 0);
				}
			}
			b.Draw(this.cloudsTexture, new Vector2(0f, (float)(this.height - 426) - this.viewportY * 2f), new Rectangle?(new Rectangle(0, 554, 165, 142)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
			b.Draw(this.cloudsTexture, new Vector2((float)(this.width - 366), (float)(this.height - 459) - this.viewportY * 2f), new Rectangle?(new Rectangle(390, 543, 122, 153)), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
			int zoom = (this.height < 800) ? 2 : 3;
			if (this.whichSubMenu.Equals("Load") || (this.subMenu != null && this.subMenu is LoadGameMenu) || this.transitioningFromLoadScreen)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(0, 0, this.width, this.height), new Rectangle?(new Rectangle(639, 858, 1, 100)), Color.White * (this.globalXOffset / 1200f));
				b.Draw(Game1.mouseCursors, Vector2.Zero, new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * (this.globalXOffset / 1200f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.8f);
				b.Draw(Game1.mouseCursors, new Vector2(0f, (float)(195 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * (this.globalXOffset / 1200f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, 0.8f);
			}
			b.Draw(this.titleButtonsTexture, new Vector2(this.globalXOffset + (float)(this.width / 2) - (float)(200 * zoom), (float)(-300 * zoom) - this.viewportY / 3f * (float)zoom), new Rectangle?(new Rectangle(0, 0, 400, 187)), Color.White, 0f, Vector2.Zero, (float)zoom, SpriteEffects.None, 0.2f);
			if (this.logoSwipeTimer > 0f)
			{
				b.Draw(this.titleButtonsTexture, new Vector2(this.globalXOffset + (float)(this.width / 2), (float)(-300 * zoom) - this.viewportY / 3f * (float)zoom + (float)(93 * zoom)), new Rectangle?(new Rectangle(0, 0, 400, 187)), Color.White, 0f, new Vector2(200f, 93f), (float)zoom + (0.5f - Math.Abs(this.logoSwipeTimer / 1000f - 0.5f)) * 0.1f, SpriteEffects.None, 0.2f);
			}
			if (this.subMenu != null && !this.isTransitioningButtons)
			{
				this.backButton.draw(b);
				this.subMenu.draw(b);
				if (!(this.subMenu is CharacterCustomization))
				{
					this.backButton.draw(b);
				}
			}
			else if (this.subMenu == null && this.isTransitioningButtons && this.whichSubMenu.Equals("Load"))
			{
				SpriteText.drawStringWithScrollBackground(b, "Loading...", Game1.tileSize, Game1.viewport.Height - Game1.tileSize, "", 1f, -1);
			}
			else if (this.subMenu == null && !this.isTransitioningButtons && this.titleInPosition && !this.transitioningCharacterCreationMenu)
			{
				this.aboutButton.draw(b);
			}
			for (int j = 0; j < this.buttonsToShow; j++)
			{
				if (this.buttons.Count > j)
				{
					this.buttons[j].draw(b, (this.subMenu == null || !(this.subMenu is AboutMenu)) ? Color.White : (Color.LightGray * 0.4f), 1f);
				}
			}
			if (this.subMenu == null)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = this.tempSprites.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b, false, 0, 0);
					}
				}
			}
			if (this.chuckleFishTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.White);
				b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2 - 66 * Game1.pixelZoom), (float)(this.height / 2 - 48 * Game1.pixelZoom)), new Rectangle?(new Rectangle(this.chuckleFishTimer % 200 / 100 * 132, 559, 132, 96)), Color.White * Math.Min(1f, (float)this.chuckleFishTimer / 500f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.2f);
			}
			else if (this.logoFadeTimer > 0 || this.fadeFromWhiteTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.White * ((float)this.fadeFromWhiteTimer / 2000f));
				b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2), (float)(this.height / 2 - 90)), new Rectangle?(new Rectangle(171 + ((this.logoFadeTimer / 100 % 2 == 0 && this.logoSurprisedTimer <= 0) ? 111 : 0), 311, 111, 60)), Color.White * ((this.logoFadeTimer < 500) ? ((float)this.logoFadeTimer / 500f) : ((this.logoFadeTimer > 4500) ? (1f - (float)(this.logoFadeTimer - 4500) / 500f) : 1f)), 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
				if (this.logoSurprisedTimer <= 0)
				{
					b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2 - 261), (float)(this.height / 2 - 102)), new Rectangle?(new Rectangle((this.logoFadeTimer / 100 % 2 == 0) ? 85 : 0, 306 + (this.shades ? 69 : 0), 85, 69)), Color.White * ((this.logoFadeTimer < 500) ? ((float)this.logoFadeTimer / 500f) : ((this.logoFadeTimer > 4500) ? (1f - (float)(this.logoFadeTimer - 4500) / 500f) : 1f)), 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
				}
				if (this.logoSurprisedTimer > 0)
				{
					b.Draw(this.titleButtonsTexture, new Vector2((float)(this.width / 2 - 261), (float)(this.height / 2 - 102)), new Rectangle?(new Rectangle((this.logoSurprisedTimer > 800 || this.logoSurprisedTimer < 400) ? 176 : 260, 375, 85, 69)), Color.White * ((this.logoSurprisedTimer < 200) ? ((float)this.logoSurprisedTimer / 200f) : 1f), 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.22f);
				}
				if (this.startupMessage.Length > 0 && this.logoFadeTimer > 0)
				{
					b.DrawString(Game1.smallFont, Game1.parseText(this.startupMessage, Game1.smallFont, Game1.tileSize * 10), new Vector2((float)(Game1.pixelZoom * 2), (float)Game1.viewport.Height - Game1.smallFont.MeasureString(Game1.parseText(this.startupMessage, Game1.smallFont, Game1.tileSize * 10)).Y - (float)Game1.pixelZoom), Color.DeepSkyBlue * ((this.logoFadeTimer < 500) ? ((float)this.logoFadeTimer / 500f) : ((this.logoFadeTimer > 4500) ? (1f - (float)(this.logoFadeTimer - 4500) / 500f) : 1f)));
				}
			}
			if (this.logoFadeTimer > 0)
			{
				int arg_CFE_0 = this.logoSurprisedTimer;
			}
			if (this.quitTimer > 0)
			{
				b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.Black * (1f - (float)this.quitTimer / 500f));
			}
			this.muteMusicButton.draw(b);
			this.windowedButton.draw(b);
			base.drawMouse(b);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0014EC34 File Offset: 0x0014CE34
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.width = Game1.viewport.Width;
			this.height = Game1.viewport.Height;
			if (!this.isTransitioningButtons && this.subMenu == null)
			{
				this.setUpIcons();
			}
			if (this.subMenu != null)
			{
				this.subMenu.gameWindowSizeChanged(oldBounds, newBounds);
			}
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.width + -198 - 48, this.height - 81 - 24, 198, 81), null, "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false);
			this.tempSprites.Clear();
			if (this.birds.Count > 0 && !this.titleInPosition)
			{
				for (int i = 0; i < this.birds.Count; i++)
				{
					this.birds[i].position = ((i % 2 == 0) ? new Vector2((float)(this.width - 210), (float)(this.height - 360)) : new Vector2((float)(this.width - 120), (float)(this.height - 330)));
				}
			}
			this.windowedButton = new ClickableTextureComponent(new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle((Game1.options != null && !Game1.options.isCurrentlyWindowed()) ? 155 : 146, 384, 9, 9), (float)Game1.pixelZoom, false);
		}

		// Token: 0x0400116C RID: 4460
		public const int fadeFromWhiteDuration = 2000;

		// Token: 0x0400116D RID: 4461
		public const int viewportFinalPosition = -1000;

		// Token: 0x0400116E RID: 4462
		public const int logoSwipeDuration = 1000;

		// Token: 0x0400116F RID: 4463
		public const int numberOfButtons = 3;

		// Token: 0x04001170 RID: 4464
		public const int spaceBetweenButtons = 8;

		// Token: 0x04001171 RID: 4465
		public const float bigCloudDX = 0.1f;

		// Token: 0x04001172 RID: 4466
		public const float mediumCloudDX = 0.2f;

		// Token: 0x04001173 RID: 4467
		public const float smallCloudDX = 0.3f;

		// Token: 0x04001174 RID: 4468
		public const float bgmountainsParallaxSpeed = 0.66f;

		// Token: 0x04001175 RID: 4469
		public const float mountainsParallaxSpeed = 1f;

		// Token: 0x04001176 RID: 4470
		public const float foregroundJungleParallaxSpeed = 2f;

		// Token: 0x04001177 RID: 4471
		public const float cloudsParallaxSpeed = 0.5f;

		// Token: 0x04001178 RID: 4472
		public const int pixelZoom = 3;

		// Token: 0x04001179 RID: 4473
		public LocalizedContentManager temporaryContent = Game1.content.CreateTemporary();

		// Token: 0x0400117A RID: 4474
		private Texture2D cloudsTexture;

		// Token: 0x0400117B RID: 4475
		private Texture2D titleButtonsTexture;

		// Token: 0x0400117C RID: 4476
		private List<float> bigClouds = new List<float>();

		// Token: 0x0400117D RID: 4477
		private List<float> smallClouds = new List<float>();

		// Token: 0x0400117E RID: 4478
		private List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();

		// Token: 0x0400117F RID: 4479
		private List<ClickableTextureComponent> buttons = new List<ClickableTextureComponent>();

		// Token: 0x04001180 RID: 4480
		private ClickableTextureComponent backButton;

		// Token: 0x04001181 RID: 4481
		private ClickableTextureComponent muteMusicButton;

		// Token: 0x04001182 RID: 4482
		private ClickableTextureComponent aboutButton;

		// Token: 0x04001183 RID: 4483
		private ClickableTextureComponent windowedButton;

		// Token: 0x04001184 RID: 4484
		private ClickableComponent skipButton;

		// Token: 0x04001185 RID: 4485
		private List<TemporaryAnimatedSprite> birds = new List<TemporaryAnimatedSprite>();

		// Token: 0x04001186 RID: 4486
		private Rectangle eRect;

		// Token: 0x04001187 RID: 4487
		private List<Rectangle> leafRects;

		// Token: 0x04001188 RID: 4488
		private IClickableMenu subMenu;

		// Token: 0x04001189 RID: 4489
		private StartupPreferences startupPreferences;

		// Token: 0x0400118A RID: 4490
		private float viewportY;

		// Token: 0x0400118B RID: 4491
		private float viewportDY;

		// Token: 0x0400118C RID: 4492
		private float logoSwipeTimer;

		// Token: 0x0400118D RID: 4493
		private float globalXOffset;

		// Token: 0x0400118E RID: 4494
		private float globalCloudAlpha = 1f;

		// Token: 0x0400118F RID: 4495
		private int fadeFromWhiteTimer;

		// Token: 0x04001190 RID: 4496
		private int pauseBeforeViewportRiseTimer;

		// Token: 0x04001191 RID: 4497
		private int buttonsToShow;

		// Token: 0x04001192 RID: 4498
		private int showButtonsTimer;

		// Token: 0x04001193 RID: 4499
		private int logoFadeTimer;

		// Token: 0x04001194 RID: 4500
		private int logoSurprisedTimer;

		// Token: 0x04001195 RID: 4501
		private int clicksOnE;

		// Token: 0x04001196 RID: 4502
		private int clicksOnLeaf;

		// Token: 0x04001197 RID: 4503
		private int buttonsDX;

		// Token: 0x04001198 RID: 4504
		private int windowNumber = 3;

		// Token: 0x04001199 RID: 4505
		private int chuckleFishTimer;

		// Token: 0x0400119A RID: 4506
		private bool titleInPosition;

		// Token: 0x0400119B RID: 4507
		private bool isTransitioningButtons;

		// Token: 0x0400119C RID: 4508
		private bool shades;

		// Token: 0x0400119D RID: 4509
		private bool transitioningCharacterCreationMenu;

		// Token: 0x0400119E RID: 4510
		public string startupMessage = "";

		// Token: 0x0400119F RID: 4511
		private int bCount;

		// Token: 0x040011A0 RID: 4512
		private string whichSubMenu = "";

		// Token: 0x040011A1 RID: 4513
		private int quitTimer;

		// Token: 0x040011A2 RID: 4514
		private bool transitioningFromLoadScreen;
	}
}
