using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;

namespace StardewValley
{
	// Token: 0x02000036 RID: 54
	public class Options
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x000377EC File Offset: 0x000359EC
		public Options()
		{
			this.setToDefaults();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00037ADC File Offset: 0x00035CDC
		public Microsoft.Xna.Framework.Input.Keys getFirstKeyboardKeyFromInputButtonList(InputButton[] inputButton)
		{
			for (int i = 0; i < inputButton.Length; i++)
			{
				if (inputButton[i].key != Microsoft.Xna.Framework.Input.Keys.None)
				{
					return inputButton[i].key;
				}
			}
			return Microsoft.Xna.Framework.Input.Keys.None;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00037B1C File Offset: 0x00035D1C
		public void reApplySetOptions()
		{
			if (this.fullscreen && !Game1.graphics.IsFullScreen)
			{
				if (!this.windowedBorderlessFullscreen)
				{
					Game1.graphics.PreferredBackBufferWidth = this.preferredResolutionX;
					Game1.graphics.PreferredBackBufferHeight = this.preferredResolutionY;
				}
				if (Game1.currentGameTime != null && Game1.currentGameTime.TotalGameTime.Seconds >= 10)
				{
					Game1.toggleFullScreen = true;
				}
			}
			if (this.zoomLevel != 1f || this.lightingQuality != 32)
			{
				Program.gamePtr.refreshWindowSettings();
			}
			Program.gamePtr.IsMouseVisible = this.hardwareCursor;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00037BBC File Offset: 0x00035DBC
		public void setToDefaults()
		{
			this.playFootstepSounds = true;
			this.showMenuBackground = false;
			this.showMerchantPortraits = true;
			this.showPortraits = true;
			this.autoRun = true;
			this.alwaysShowToolHitLocation = false;
			this.hideToolHitLocationWhenInMotion = true;
			this.dialogueTyping = true;
			this.rumble = true;
			this.fullscreen = false;
			this.pinToolbarToggle = false;
			this.zoomLevel = 1f;
			this.zoomButtons = false;
			this.pauseWhenOutOfFocus = true;
			this.screenFlash = true;
			this.snowTransparency = 1f;
			this.invertScrollDirection = false;
			this.useCrisperNumberFont = false;
			this.ambientOnlyToggle = false;
			this.windowedBorderlessFullscreen = true;
			this.showPlacementTileForGamepad = true;
			this.lightingQuality = 32;
			this.hardwareCursor = false;
			this.musicVolumeLevel = 0.75f;
			this.ambientVolumeLevel = 0.75f;
			this.footstepVolumeLevel = 0.9f;
			this.soundVolumeLevel = 1f;
			this.preferredResolutionX = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last<DisplayMode>().Width;
			this.preferredResolutionY = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last<DisplayMode>().Height;
			this.reApplySetOptions();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00037CDC File Offset: 0x00035EDC
		public void setControlsToDefault()
		{
			this.actionButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.X),
				new InputButton(false)
			};
			this.toolSwapButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.Z)
			};
			this.cancelButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.V)
			};
			this.useToolButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.C),
				new InputButton(true)
			};
			this.moveUpButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.W)
			};
			this.moveRightButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D)
			};
			this.moveDownButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.S)
			};
			this.moveLeftButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.A)
			};
			this.menuButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.E),
				new InputButton(Microsoft.Xna.Framework.Input.Keys.Escape)
			};
			this.runButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.LeftShift)
			};
			this.tmpKeyToReplace = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.None)
			};
			this.chatButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.T),
				new InputButton(Microsoft.Xna.Framework.Input.Keys.OemQuestion)
			};
			this.mapButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.M)
			};
			this.journalButton = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.F)
			};
			this.inventorySlot1 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D1)
			};
			this.inventorySlot2 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D2)
			};
			this.inventorySlot3 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D3)
			};
			this.inventorySlot4 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D4)
			};
			this.inventorySlot5 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D5)
			};
			this.inventorySlot6 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D6)
			};
			this.inventorySlot7 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D7)
			};
			this.inventorySlot8 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D8)
			};
			this.inventorySlot9 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D9)
			};
			this.inventorySlot10 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.D0)
			};
			this.inventorySlot11 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.OemMinus)
			};
			this.inventorySlot12 = new InputButton[]
			{
				new InputButton(Microsoft.Xna.Framework.Input.Keys.OemPlus)
			};
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00037FD0 File Offset: 0x000361D0
		public string getNameOfOptionFromIndex(int index)
		{
			switch (index)
			{
			case 0:
				return "Auto Run";
			case 1:
				return "Music Volume";
			case 2:
				return "Sound Effects Volume";
			case 3:
				return "Typing Sound";
			case 4:
				return "Fullscreen";
			case 5:
				return "Windowed Borderless";
			case 6:
				return "Resolution";
			default:
				return "";
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00038030 File Offset: 0x00036230
		public int whatTypeOfOption(int index)
		{
			if (index == 1 || index == 2)
			{
				return 2;
			}
			if (index != 6)
			{
				return 1;
			}
			return 3;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00038048 File Offset: 0x00036248
		public void changeCheckBoxOption(int which, bool value)
		{
			switch (which)
			{
			case 0:
				this.autoRun = value;
				Game1.player.setRunning(this.autoRun, false);
				return;
			case 1:
			case 2:
			case 6:
			case 13:
			case 18:
			case 20:
			case 21:
			case 23:
			case 25:
				break;
			case 3:
				this.dialogueTyping = value;
				return;
			case 4:
				if ((Game1.isFullscreen && !value) || (!Game1.isFullscreen & value))
				{
					Game1.toggleFullScreen = true;
					Game1.graphics.PreferredBackBufferWidth = this.preferredResolutionX;
					Game1.graphics.PreferredBackBufferHeight = this.preferredResolutionY;
				}
				this.fullscreen = value;
				return;
			case 5:
				if (!value && Game1.isFullscreen && this.windowedBorderlessFullscreen)
				{
					Game1.toggleFullScreen = true;
				}
				else if (value && Game1.isFullscreen && !this.windowedBorderlessFullscreen)
				{
					Game1.toggleNonBorderlessWindowedFullscreen(-1, -1);
					Game1.toggleFullScreen = true;
				}
				this.windowedBorderlessFullscreen = value;
				return;
			case 7:
				this.showPortraits = value;
				return;
			case 8:
				this.showMerchantPortraits = value;
				return;
			case 9:
				this.showMenuBackground = value;
				return;
			case 10:
				this.playFootstepSounds = value;
				return;
			case 11:
				this.alwaysShowToolHitLocation = value;
				return;
			case 12:
				this.hideToolHitLocationWhenInMotion = value;
				return;
			case 14:
				this.pauseWhenOutOfFocus = value;
				return;
			case 15:
				this.pinToolbarToggle = value;
				return;
			case 16:
				this.rumble = value;
				return;
			case 17:
				this.ambientOnlyToggle = value;
				return;
			case 19:
				this.zoomButtons = value;
				return;
			case 22:
				this.invertScrollDirection = value;
				return;
			case 24:
				this.screenFlash = value;
				return;
			case 26:
				this.hardwareCursor = value;
				Program.gamePtr.IsMouseVisible = this.hardwareCursor;
				return;
			case 27:
				this.showPlacementTileForGamepad = value;
				return;
			case 28:
				this.useCrisperNumberFont = value;
				break;
			default:
				return;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00038208 File Offset: 0x00036408
		public void changeSliderOption(int which, int value)
		{
			if (which == 1)
			{
				this.musicVolumeLevel = (float)value / 100f;
				Game1.musicCategory.SetVolume(this.musicVolumeLevel);
				return;
			}
			if (which != 2)
			{
				switch (which)
				{
				case 18:
				{
					int zoomlvl = (int)(this.zoomLevel * 100f);
					int oldZoom = zoomlvl;
					int newValue = (int)((float)value * 100f);
					if (newValue >= zoomlvl + 10 || newValue >= 100)
					{
						zoomlvl += 10;
						zoomlvl = Math.Min(100, zoomlvl);
					}
					else if (newValue <= zoomlvl - 10 || newValue <= 50)
					{
						zoomlvl -= 10;
						zoomlvl = Math.Max(50, zoomlvl);
					}
					if (zoomlvl != oldZoom)
					{
						this.zoomLevel = (float)zoomlvl / 100f;
						Game1.overrideGameMenuReset = true;
						Program.gamePtr.refreshWindowSettings();
						Game1.overrideGameMenuReset = false;
						Game1.showGlobalMessage("Zoom Level: " + this.zoomLevel);
					}
					break;
				}
				case 19:
				case 22:
					break;
				case 20:
					this.ambientVolumeLevel = (float)value / 100f;
					Game1.ambientCategory.SetVolume(this.ambientVolumeLevel);
					return;
				case 21:
					this.footstepVolumeLevel = (float)value / 100f;
					Game1.footstepCategory.SetVolume(this.footstepVolumeLevel);
					return;
				case 23:
					this.snowTransparency = (float)value / 100f;
					return;
				default:
					return;
				}
				return;
			}
			this.soundVolumeLevel = (float)value / 100f;
			Game1.soundCategory.SetVolume(this.soundVolumeLevel);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0003835C File Offset: 0x0003655C
		public void setWindowedOption(string setting)
		{
			this.windowedBorderlessFullscreen = this.isCurrentlyWindowedBorderless();
			this.fullscreen = (!this.windowedBorderlessFullscreen && Game1.graphics.IsFullScreen);
			if (!(setting == "Windowed"))
			{
				if (!(setting == "Fullscreen"))
				{
					if (setting == "Windowed Borderless")
					{
						if (!this.windowedBorderlessFullscreen)
						{
							if (this.fullscreen)
							{
								Game1.toggleNonBorderlessWindowedFullscreen(-1, -1);
							}
							this.windowedBorderlessFullscreen = true;
							Game1.toggleFullscreen();
							this.fullscreen = false;
						}
					}
				}
				else if (this.windowedBorderlessFullscreen)
				{
					this.fullscreen = true;
					this.windowedBorderlessFullscreen = false;
					Game1.toggleFullscreen();
				}
				else if (!Game1.graphics.IsFullScreen)
				{
					Game1.toggleNonBorderlessWindowedFullscreen(-1, -1);
					this.fullscreen = true;
					this.windowedBorderlessFullscreen = false;
					this.hardwareCursor = false;
					Program.gamePtr.IsMouseVisible = false;
				}
			}
			else if (Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen)
			{
				Game1.toggleNonBorderlessWindowedFullscreen(-1, -1);
				this.fullscreen = false;
				this.windowedBorderlessFullscreen = false;
			}
			else if (this.windowedBorderlessFullscreen)
			{
				this.fullscreen = false;
				this.windowedBorderlessFullscreen = false;
				Game1.toggleFullscreen();
			}
			if (Game1.gameMode == 3)
			{
				Game1.exitActiveMenu();
				Game1.activeClickableMenu = new GameMenu(6, 6);
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000384A4 File Offset: 0x000366A4
		public void changeDropDownOption(int which, int selection, List<string> options)
		{
			if (which <= 13)
			{
				if (which == 6)
				{
					Rectangle oldWindow = new Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height);
					string resolution = options[selection];
					int width = Convert.ToInt32(resolution.Split(new char[]
					{
						' '
					})[0]);
					int height = Convert.ToInt32(resolution.Split(new char[]
					{
						' '
					})[2]);
					this.preferredResolutionX = width;
					this.preferredResolutionY = height;
					Game1.graphics.PreferredBackBufferWidth = width;
					Game1.graphics.PreferredBackBufferHeight = height;
					Game1.graphics.ApplyChanges();
					Game1.updateViewportForScreenSizeChange(true, width, height);
					using (List<IClickableMenu>.Enumerator enumerator = Game1.onScreenMenus.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.gameWindowSizeChanged(oldWindow, new Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
						}
					}
					if (Game1.currentMinigame != null)
					{
						Game1.currentMinigame.changeScreenSize();
					}
					Game1.exitActiveMenu();
					Game1.activeClickableMenu = new GameMenu(6, 6);
					return;
				}
				if (which != 13)
				{
					return;
				}
				this.setWindowedOption(options[selection]);
			}
			else if (which != 18)
			{
				if (which == 25)
				{
					string a = options[selection];
					if (!(a == "Lowest"))
					{
						if (!(a == "Low"))
						{
							if (!(a == "Med."))
							{
								if (!(a == "High"))
								{
									if (a == "Ultra")
									{
										this.lightingQuality = 8;
									}
								}
								else
								{
									this.lightingQuality = 16;
								}
							}
							else
							{
								this.lightingQuality = 32;
							}
						}
						else
						{
							this.lightingQuality = 64;
						}
					}
					else
					{
						this.lightingQuality = 128;
					}
					Game1.overrideGameMenuReset = true;
					Program.gamePtr.refreshWindowSettings();
					Game1.overrideGameMenuReset = false;
					Game1.activeClickableMenu = new GameMenu(6, 19);
					return;
				}
			}
			else
			{
				int newZoom = Convert.ToInt32(options[selection].Replace("%", ""));
				this.zoomLevel = (float)newZoom / 100f;
				Game1.overrideGameMenuReset = true;
				Program.gamePtr.refreshWindowSettings();
				Game1.overrideGameMenuReset = false;
				Game1.activeClickableMenu = new GameMenu(6, 13);
				if (Game1.debrisWeather != null)
				{
					Game1.randomizeDebrisWeatherPositions(Game1.debrisWeather);
					return;
				}
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00038724 File Offset: 0x00036924
		public bool isKeyInUse(Microsoft.Xna.Framework.Input.Keys key)
		{
			using (List<InputButton>.Enumerator enumerator = this.getAllUsedInputButtons().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.key == key)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00038780 File Offset: 0x00036980
		public List<InputButton> getAllUsedInputButtons()
		{
			List<InputButton> expr_05 = new List<InputButton>();
			expr_05.AddRange(this.useToolButton);
			expr_05.AddRange(this.actionButton);
			expr_05.AddRange(this.moveUpButton);
			expr_05.AddRange(this.moveRightButton);
			expr_05.AddRange(this.moveDownButton);
			expr_05.AddRange(this.moveLeftButton);
			expr_05.AddRange(this.runButton);
			expr_05.AddRange(this.menuButton);
			expr_05.AddRange(this.journalButton);
			expr_05.AddRange(this.mapButton);
			expr_05.AddRange(this.toolSwapButton);
			expr_05.AddRange(this.chatButton);
			expr_05.AddRange(this.inventorySlot1);
			expr_05.AddRange(this.inventorySlot2);
			expr_05.AddRange(this.inventorySlot3);
			expr_05.AddRange(this.inventorySlot4);
			expr_05.AddRange(this.inventorySlot5);
			expr_05.AddRange(this.inventorySlot6);
			expr_05.AddRange(this.inventorySlot7);
			expr_05.AddRange(this.inventorySlot8);
			expr_05.AddRange(this.inventorySlot9);
			expr_05.AddRange(this.inventorySlot10);
			expr_05.AddRange(this.inventorySlot11);
			expr_05.AddRange(this.inventorySlot12);
			return expr_05;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000388B4 File Offset: 0x00036AB4
		public void setCheckBoxToProperValue(OptionsCheckbox checkbox)
		{
			switch (checkbox.whichOption)
			{
			case 0:
				checkbox.isChecked = this.autoRun;
				return;
			case 1:
			case 2:
			case 6:
			case 13:
			case 18:
			case 20:
			case 21:
			case 23:
			case 25:
				break;
			case 3:
				checkbox.isChecked = this.dialogueTyping;
				return;
			case 4:
			{
				Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
				this.windowedBorderlessFullscreen = (form.FormBorderStyle == FormBorderStyle.None);
				this.fullscreen = (Game1.graphics.IsFullScreen || this.windowedBorderlessFullscreen);
				checkbox.isChecked = this.fullscreen;
				return;
			}
			case 5:
				checkbox.isChecked = this.windowedBorderlessFullscreen;
				checkbox.greyedOut = !this.fullscreen;
				return;
			case 7:
				checkbox.isChecked = this.showPortraits;
				return;
			case 8:
				checkbox.isChecked = this.showMerchantPortraits;
				return;
			case 9:
				checkbox.isChecked = this.showMenuBackground;
				return;
			case 10:
				checkbox.isChecked = this.playFootstepSounds;
				return;
			case 11:
				checkbox.isChecked = this.alwaysShowToolHitLocation;
				return;
			case 12:
				checkbox.isChecked = this.hideToolHitLocationWhenInMotion;
				return;
			case 14:
				checkbox.isChecked = this.pauseWhenOutOfFocus;
				return;
			case 15:
				checkbox.isChecked = this.pinToolbarToggle;
				return;
			case 16:
				checkbox.isChecked = this.rumble;
				checkbox.greyedOut = !this.gamepadControls;
				return;
			case 17:
				checkbox.isChecked = this.ambientOnlyToggle;
				return;
			case 19:
				checkbox.isChecked = this.zoomButtons;
				return;
			case 22:
				checkbox.isChecked = this.invertScrollDirection;
				return;
			case 24:
				checkbox.isChecked = this.screenFlash;
				return;
			case 26:
				checkbox.isChecked = this.hardwareCursor;
				checkbox.greyedOut = this.fullscreen;
				return;
			case 27:
				checkbox.isChecked = this.showPlacementTileForGamepad;
				checkbox.greyedOut = !this.gamepadControls;
				return;
			case 28:
				checkbox.isChecked = this.useCrisperNumberFont;
				break;
			default:
				return;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00038AC4 File Offset: 0x00036CC4
		public void setPlusMinusToProperValue(OptionsPlusMinus plusMinus)
		{
			int whichOption = plusMinus.whichOption;
			if (whichOption == 18)
			{
				string currentZoom = (int)((decimal)this.zoomLevel * 100m) + "%";
				for (int i = 0; i < plusMinus.options.Count; i++)
				{
					if (plusMinus.options[i].Equals(currentZoom))
					{
						plusMinus.selected = i;
						return;
					}
				}
				return;
			}
			if (whichOption != 25)
			{
				return;
			}
			string currentQuality = "";
			whichOption = this.lightingQuality;
			if (whichOption <= 16)
			{
				if (whichOption != 8)
				{
					if (whichOption == 16)
					{
						currentQuality = "High";
					}
				}
				else
				{
					currentQuality = "Ultra";
				}
			}
			else if (whichOption != 32)
			{
				if (whichOption != 64)
				{
					if (whichOption == 128)
					{
						currentQuality = "Lowest";
					}
				}
				else
				{
					currentQuality = "Low";
				}
			}
			else
			{
				currentQuality = "Med.";
			}
			for (int j = 0; j < plusMinus.options.Count; j++)
			{
				if (plusMinus.options[j].Equals(currentQuality))
				{
					plusMinus.selected = j;
					return;
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00038BD0 File Offset: 0x00036DD0
		public void setSliderToProperValue(OptionsSlider slider)
		{
			int whichOption = slider.whichOption;
			if (whichOption == 1)
			{
				slider.value = (int)(this.musicVolumeLevel * 100f);
				return;
			}
			if (whichOption != 2)
			{
				switch (whichOption)
				{
				case 18:
					slider.value = (int)(this.zoomLevel * 100f);
					break;
				case 19:
				case 22:
					break;
				case 20:
					slider.value = (int)(this.ambientVolumeLevel * 100f);
					return;
				case 21:
					slider.value = (int)(this.footstepVolumeLevel * 100f);
					return;
				case 23:
					slider.value = (int)(this.snowTransparency * 100f);
					return;
				default:
					return;
				}
				return;
			}
			slider.value = (int)(this.soundVolumeLevel * 100f);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00038C88 File Offset: 0x00036E88
		public bool doesInputListContain(InputButton[] list, Microsoft.Xna.Framework.Input.Keys key)
		{
			for (int i = 0; i < list.Length; i++)
			{
				if (list[i].key == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00038CB8 File Offset: 0x00036EB8
		public void changeInputListenerValue(int whichListener, Microsoft.Xna.Framework.Input.Keys key)
		{
			switch (whichListener)
			{
			case 7:
				this.actionButton[0] = new InputButton(key);
				return;
			case 8:
				this.toolSwapButton[0] = new InputButton(key);
				return;
			case 9:
				break;
			case 10:
				this.useToolButton[0] = new InputButton(key);
				return;
			case 11:
				this.moveUpButton[0] = new InputButton(key);
				return;
			case 12:
				this.moveRightButton[0] = new InputButton(key);
				return;
			case 13:
				this.moveDownButton[0] = new InputButton(key);
				return;
			case 14:
				this.moveLeftButton[0] = new InputButton(key);
				return;
			case 15:
				this.menuButton[0] = new InputButton(key);
				return;
			case 16:
				this.runButton[0] = new InputButton(key);
				return;
			case 17:
				this.chatButton[0] = new InputButton(key);
				return;
			case 18:
				this.journalButton[0] = new InputButton(key);
				return;
			case 19:
				this.mapButton[0] = new InputButton(key);
				return;
			case 20:
				this.inventorySlot1[0] = new InputButton(key);
				return;
			case 21:
				this.inventorySlot2[0] = new InputButton(key);
				return;
			case 22:
				this.inventorySlot3[0] = new InputButton(key);
				return;
			case 23:
				this.inventorySlot4[0] = new InputButton(key);
				return;
			case 24:
				this.inventorySlot5[0] = new InputButton(key);
				return;
			case 25:
				this.inventorySlot6[0] = new InputButton(key);
				return;
			case 26:
				this.inventorySlot7[0] = new InputButton(key);
				return;
			case 27:
				this.inventorySlot8[0] = new InputButton(key);
				return;
			case 28:
				this.inventorySlot9[0] = new InputButton(key);
				return;
			case 29:
				this.inventorySlot10[0] = new InputButton(key);
				return;
			case 30:
				this.inventorySlot11[0] = new InputButton(key);
				return;
			case 31:
				this.inventorySlot12[0] = new InputButton(key);
				break;
			default:
				return;
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00038EFC File Offset: 0x000370FC
		public void setInputListenerToProperValue(OptionsInputListener inputListener)
		{
			inputListener.buttonNames.Clear();
			switch (inputListener.whichOption)
			{
			case 7:
			{
				InputButton[] array = this.actionButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b = array[i];
					inputListener.buttonNames.Add(b.ToString());
				}
				return;
			}
			case 8:
			{
				InputButton[] array = this.toolSwapButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b2 = array[i];
					inputListener.buttonNames.Add(b2.ToString());
				}
				return;
			}
			case 9:
				break;
			case 10:
			{
				InputButton[] array = this.useToolButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b3 = array[i];
					inputListener.buttonNames.Add(b3.ToString());
				}
				return;
			}
			case 11:
			{
				InputButton[] array = this.moveUpButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b4 = array[i];
					inputListener.buttonNames.Add(b4.ToString());
				}
				return;
			}
			case 12:
			{
				InputButton[] array = this.moveRightButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b5 = array[i];
					inputListener.buttonNames.Add(b5.ToString());
				}
				return;
			}
			case 13:
			{
				InputButton[] array = this.moveDownButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b6 = array[i];
					inputListener.buttonNames.Add(b6.ToString());
				}
				return;
			}
			case 14:
			{
				InputButton[] array = this.moveLeftButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b7 = array[i];
					inputListener.buttonNames.Add(b7.ToString());
				}
				return;
			}
			case 15:
			{
				InputButton[] array = this.menuButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b8 = array[i];
					inputListener.buttonNames.Add(b8.ToString());
				}
				return;
			}
			case 16:
			{
				InputButton[] array = this.runButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b9 = array[i];
					inputListener.buttonNames.Add(b9.ToString());
				}
				return;
			}
			case 17:
			{
				InputButton[] array = this.chatButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b10 = array[i];
					inputListener.buttonNames.Add(b10.ToString());
				}
				return;
			}
			case 18:
			{
				InputButton[] array = this.journalButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b11 = array[i];
					inputListener.buttonNames.Add(b11.ToString());
				}
				return;
			}
			case 19:
			{
				InputButton[] array = this.mapButton;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b12 = array[i];
					inputListener.buttonNames.Add(b12.ToString());
				}
				return;
			}
			case 20:
			{
				InputButton[] array = this.inventorySlot1;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b13 = array[i];
					inputListener.buttonNames.Add(b13.ToString());
				}
				return;
			}
			case 21:
			{
				InputButton[] array = this.inventorySlot2;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b14 = array[i];
					inputListener.buttonNames.Add(b14.ToString());
				}
				return;
			}
			case 22:
			{
				InputButton[] array = this.inventorySlot3;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b15 = array[i];
					inputListener.buttonNames.Add(b15.ToString());
				}
				return;
			}
			case 23:
			{
				InputButton[] array = this.inventorySlot4;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b16 = array[i];
					inputListener.buttonNames.Add(b16.ToString());
				}
				return;
			}
			case 24:
			{
				InputButton[] array = this.inventorySlot5;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b17 = array[i];
					inputListener.buttonNames.Add(b17.ToString());
				}
				return;
			}
			case 25:
			{
				InputButton[] array = this.inventorySlot6;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b18 = array[i];
					inputListener.buttonNames.Add(b18.ToString());
				}
				return;
			}
			case 26:
			{
				InputButton[] array = this.inventorySlot7;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b19 = array[i];
					inputListener.buttonNames.Add(b19.ToString());
				}
				return;
			}
			case 27:
			{
				InputButton[] array = this.inventorySlot8;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b20 = array[i];
					inputListener.buttonNames.Add(b20.ToString());
				}
				return;
			}
			case 28:
			{
				InputButton[] array = this.inventorySlot9;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b21 = array[i];
					inputListener.buttonNames.Add(b21.ToString());
				}
				return;
			}
			case 29:
			{
				InputButton[] array = this.inventorySlot10;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b22 = array[i];
					inputListener.buttonNames.Add(b22.ToString());
				}
				return;
			}
			case 30:
			{
				InputButton[] array = this.inventorySlot11;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b23 = array[i];
					inputListener.buttonNames.Add(b23.ToString());
				}
				return;
			}
			case 31:
			{
				InputButton[] array = this.inventorySlot12;
				for (int i = 0; i < array.Length; i++)
				{
					InputButton b24 = array[i];
					inputListener.buttonNames.Add(b24.ToString());
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000394B0 File Offset: 0x000376B0
		public void setDropDownToProperValue(OptionsDropDown dropDown)
		{
			int whichOption = dropDown.whichOption;
			if (whichOption == 6)
			{
				int i = 0;
				foreach (DisplayMode v in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
				{
					if (v.Width >= 1280)
					{
						dropDown.dropDownOptions.Add(v.Width + " x " + v.Height);
						if (v.Width == this.preferredResolutionX && v.Height == this.preferredResolutionY)
						{
							dropDown.selectedOption = i;
						}
						i++;
					}
				}
				dropDown.greyedOut = (!this.fullscreen || this.windowedBorderlessFullscreen);
				return;
			}
			if (whichOption != 13)
			{
				return;
			}
			this.windowedBorderlessFullscreen = this.isCurrentlyWindowedBorderless();
			this.fullscreen = (Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen);
			dropDown.dropDownOptions.Add("Windowed");
			dropDown.dropDownOptions.Add("Fullscreen");
			dropDown.dropDownOptions.Add("Windowed Borderless");
			if (Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen)
			{
				dropDown.selectedOption = 1;
				return;
			}
			if (this.windowedBorderlessFullscreen)
			{
				dropDown.selectedOption = 2;
				return;
			}
			dropDown.selectedOption = 0;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00039614 File Offset: 0x00037814
		public bool isCurrentlyWindowedBorderless()
		{
			return Control.FromHandle(Program.gamePtr.Window.Handle).FindForm().FormBorderStyle == FormBorderStyle.None;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00039637 File Offset: 0x00037837
		public bool isCurrentlyFullscreen()
		{
			return Game1.graphics.IsFullScreen && !this.windowedBorderlessFullscreen;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00039650 File Offset: 0x00037850
		public bool isCurrentlyWindowed()
		{
			return !this.isCurrentlyWindowedBorderless() && !this.isCurrentlyFullscreen();
		}

		// Token: 0x040002CF RID: 719
		public const float minZoom = 0.75f;

		// Token: 0x040002D0 RID: 720
		public const float maxZoom = 1f;

		// Token: 0x040002D1 RID: 721
		public const int toggleAutoRun = 0;

		// Token: 0x040002D2 RID: 722
		public const int musicVolume = 1;

		// Token: 0x040002D3 RID: 723
		public const int soundVolume = 2;

		// Token: 0x040002D4 RID: 724
		public const int toggleDialogueTypingSounds = 3;

		// Token: 0x040002D5 RID: 725
		public const int toggleFullscreen = 4;

		// Token: 0x040002D6 RID: 726
		public const int toggleWindowedOrTrueFullscreen = 5;

		// Token: 0x040002D7 RID: 727
		public const int screenResolution = 6;

		// Token: 0x040002D8 RID: 728
		public const int showPortraitsToggle = 7;

		// Token: 0x040002D9 RID: 729
		public const int showMerchantPortraitsToggle = 8;

		// Token: 0x040002DA RID: 730
		public const int menuBG = 9;

		// Token: 0x040002DB RID: 731
		public const int toggleFootsteps = 10;

		// Token: 0x040002DC RID: 732
		public const int alwaysShowToolHitLocationToggle = 11;

		// Token: 0x040002DD RID: 733
		public const int hideToolHitLocationWhenInMotionToggle = 12;

		// Token: 0x040002DE RID: 734
		public const int windowMode = 13;

		// Token: 0x040002DF RID: 735
		public const int pauseWhenUnfocused = 14;

		// Token: 0x040002E0 RID: 736
		public const int pinToolbar = 15;

		// Token: 0x040002E1 RID: 737
		public const int toggleRumble = 16;

		// Token: 0x040002E2 RID: 738
		public const int ambientOnly = 17;

		// Token: 0x040002E3 RID: 739
		public const int zoom = 18;

		// Token: 0x040002E4 RID: 740
		public const int zoomButtonsToggle = 19;

		// Token: 0x040002E5 RID: 741
		public const int ambientVolume = 20;

		// Token: 0x040002E6 RID: 742
		public const int footstepVolume = 21;

		// Token: 0x040002E7 RID: 743
		public const int invertScrollDirectionToggle = 22;

		// Token: 0x040002E8 RID: 744
		public const int snowTransparencyToggle = 23;

		// Token: 0x040002E9 RID: 745
		public const int screenFlashToggle = 24;

		// Token: 0x040002EA RID: 746
		public const int lightingQualityToggle = 25;

		// Token: 0x040002EB RID: 747
		public const int toggleHardwareCursor = 26;

		// Token: 0x040002EC RID: 748
		public const int toggleShowPlacementTileGamepad = 27;

		// Token: 0x040002ED RID: 749
		public const int toggleUseCrisperNumberFont = 28;

		// Token: 0x040002EE RID: 750
		public const int input_actionButton = 7;

		// Token: 0x040002EF RID: 751
		public const int input_toolSwapButton = 8;

		// Token: 0x040002F0 RID: 752
		public const int input_cancelButton = 9;

		// Token: 0x040002F1 RID: 753
		public const int input_useToolButton = 10;

		// Token: 0x040002F2 RID: 754
		public const int input_moveUpButton = 11;

		// Token: 0x040002F3 RID: 755
		public const int input_moveRightButton = 12;

		// Token: 0x040002F4 RID: 756
		public const int input_moveDownButton = 13;

		// Token: 0x040002F5 RID: 757
		public const int input_moveLeftButton = 14;

		// Token: 0x040002F6 RID: 758
		public const int input_menuButton = 15;

		// Token: 0x040002F7 RID: 759
		public const int input_runButton = 16;

		// Token: 0x040002F8 RID: 760
		public const int input_chatButton = 17;

		// Token: 0x040002F9 RID: 761
		public const int input_journalButton = 18;

		// Token: 0x040002FA RID: 762
		public const int input_mapButton = 19;

		// Token: 0x040002FB RID: 763
		public const int input_slot1 = 20;

		// Token: 0x040002FC RID: 764
		public const int input_slot2 = 21;

		// Token: 0x040002FD RID: 765
		public const int input_slot3 = 22;

		// Token: 0x040002FE RID: 766
		public const int input_slot4 = 23;

		// Token: 0x040002FF RID: 767
		public const int input_slot5 = 24;

		// Token: 0x04000300 RID: 768
		public const int input_slot6 = 25;

		// Token: 0x04000301 RID: 769
		public const int input_slot7 = 26;

		// Token: 0x04000302 RID: 770
		public const int input_slot8 = 27;

		// Token: 0x04000303 RID: 771
		public const int input_slot9 = 28;

		// Token: 0x04000304 RID: 772
		public const int input_slot10 = 29;

		// Token: 0x04000305 RID: 773
		public const int input_slot11 = 30;

		// Token: 0x04000306 RID: 774
		public const int input_slot12 = 31;

		// Token: 0x04000307 RID: 775
		public const int checkBoxOption = 1;

		// Token: 0x04000308 RID: 776
		public const int sliderOption = 2;

		// Token: 0x04000309 RID: 777
		public const int dropDownOption = 3;

		// Token: 0x0400030A RID: 778
		public bool autoRun;

		// Token: 0x0400030B RID: 779
		public bool dialogueTyping;

		// Token: 0x0400030C RID: 780
		public bool fullscreen;

		// Token: 0x0400030D RID: 781
		public bool windowedBorderlessFullscreen;

		// Token: 0x0400030E RID: 782
		public bool showPortraits;

		// Token: 0x0400030F RID: 783
		public bool showMerchantPortraits;

		// Token: 0x04000310 RID: 784
		public bool showMenuBackground;

		// Token: 0x04000311 RID: 785
		public bool playFootstepSounds;

		// Token: 0x04000312 RID: 786
		public bool alwaysShowToolHitLocation;

		// Token: 0x04000313 RID: 787
		public bool hideToolHitLocationWhenInMotion;

		// Token: 0x04000314 RID: 788
		public bool pauseWhenOutOfFocus;

		// Token: 0x04000315 RID: 789
		public bool pinToolbarToggle;

		// Token: 0x04000316 RID: 790
		public bool mouseControls;

		// Token: 0x04000317 RID: 791
		public bool keyboardControls;

		// Token: 0x04000318 RID: 792
		public bool gamepadControls;

		// Token: 0x04000319 RID: 793
		public bool rumble;

		// Token: 0x0400031A RID: 794
		public bool ambientOnlyToggle;

		// Token: 0x0400031B RID: 795
		public bool zoomButtons;

		// Token: 0x0400031C RID: 796
		public bool invertScrollDirection;

		// Token: 0x0400031D RID: 797
		public bool screenFlash;

		// Token: 0x0400031E RID: 798
		public bool hardwareCursor;

		// Token: 0x0400031F RID: 799
		public bool showPlacementTileForGamepad;

		// Token: 0x04000320 RID: 800
		public bool useCrisperNumberFont;

		// Token: 0x04000321 RID: 801
		public float musicVolumeLevel;

		// Token: 0x04000322 RID: 802
		public float soundVolumeLevel;

		// Token: 0x04000323 RID: 803
		public float zoomLevel;

		// Token: 0x04000324 RID: 804
		public float footstepVolumeLevel;

		// Token: 0x04000325 RID: 805
		public float ambientVolumeLevel;

		// Token: 0x04000326 RID: 806
		public float snowTransparency;

		// Token: 0x04000327 RID: 807
		public int preferredResolutionX;

		// Token: 0x04000328 RID: 808
		public int preferredResolutionY;

		// Token: 0x04000329 RID: 809
		public int lightingQuality;

		// Token: 0x0400032A RID: 810
		public InputButton[] actionButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.X),
			new InputButton(false)
		};

		// Token: 0x0400032B RID: 811
		public InputButton[] toolSwapButton = new InputButton[0];

		// Token: 0x0400032C RID: 812
		public InputButton[] cancelButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.V)
		};

		// Token: 0x0400032D RID: 813
		public InputButton[] useToolButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.C),
			new InputButton(true)
		};

		// Token: 0x0400032E RID: 814
		public InputButton[] moveUpButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.W)
		};

		// Token: 0x0400032F RID: 815
		public InputButton[] moveRightButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D)
		};

		// Token: 0x04000330 RID: 816
		public InputButton[] moveDownButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.S)
		};

		// Token: 0x04000331 RID: 817
		public InputButton[] moveLeftButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.A)
		};

		// Token: 0x04000332 RID: 818
		public InputButton[] menuButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.E),
			new InputButton(Microsoft.Xna.Framework.Input.Keys.Escape)
		};

		// Token: 0x04000333 RID: 819
		public InputButton[] runButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.LeftShift)
		};

		// Token: 0x04000334 RID: 820
		public InputButton[] tmpKeyToReplace = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.None)
		};

		// Token: 0x04000335 RID: 821
		public InputButton[] chatButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.T),
			new InputButton(Microsoft.Xna.Framework.Input.Keys.OemQuestion)
		};

		// Token: 0x04000336 RID: 822
		public InputButton[] mapButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.M)
		};

		// Token: 0x04000337 RID: 823
		public InputButton[] journalButton = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.F)
		};

		// Token: 0x04000338 RID: 824
		public InputButton[] inventorySlot1 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D1)
		};

		// Token: 0x04000339 RID: 825
		public InputButton[] inventorySlot2 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D2)
		};

		// Token: 0x0400033A RID: 826
		public InputButton[] inventorySlot3 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D3)
		};

		// Token: 0x0400033B RID: 827
		public InputButton[] inventorySlot4 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D4)
		};

		// Token: 0x0400033C RID: 828
		public InputButton[] inventorySlot5 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D5)
		};

		// Token: 0x0400033D RID: 829
		public InputButton[] inventorySlot6 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D6)
		};

		// Token: 0x0400033E RID: 830
		public InputButton[] inventorySlot7 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D7)
		};

		// Token: 0x0400033F RID: 831
		public InputButton[] inventorySlot8 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D8)
		};

		// Token: 0x04000340 RID: 832
		public InputButton[] inventorySlot9 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D9)
		};

		// Token: 0x04000341 RID: 833
		public InputButton[] inventorySlot10 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.D0)
		};

		// Token: 0x04000342 RID: 834
		public InputButton[] inventorySlot11 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.OemMinus)
		};

		// Token: 0x04000343 RID: 835
		public InputButton[] inventorySlot12 = new InputButton[]
		{
			new InputButton(Microsoft.Xna.Framework.Input.Keys.OemPlus)
		};
	}
}
