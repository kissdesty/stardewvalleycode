using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using Steamworks;
using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
	// Token: 0x0200004A RID: 74
	public class Game1 : Game
	{
		// Token: 0x17000075 RID: 117
		public static bool IsMultiplayer
		{
			// Token: 0x060005F3 RID: 1523 RVA: 0x0007F8E4 File Offset: 0x0007DAE4
			get
			{
				return Game1.multiplayerMode > 0;
			}
		}

		// Token: 0x17000076 RID: 118
		public static bool IsClient
		{
			// Token: 0x060005F4 RID: 1524 RVA: 0x0007F8EE File Offset: 0x0007DAEE
			get
			{
				return Game1.multiplayerMode == 1;
			}
		}

		// Token: 0x17000077 RID: 119
		public static bool IsServer
		{
			// Token: 0x060005F5 RID: 1525 RVA: 0x0007F8F8 File Offset: 0x0007DAF8
			get
			{
				return Game1.multiplayerMode == 2;
			}
		}

		// Token: 0x17000078 RID: 120
		public static bool IsMasterGame
		{
			// Token: 0x060005F6 RID: 1526 RVA: 0x0007F902 File Offset: 0x0007DB02
			get
			{
				return Game1.multiplayerMode == 0 || Game1.multiplayerMode == 2;
			}
		}

		// Token: 0x17000079 RID: 121
		public static ChatBox ChatBox
		{
			// Token: 0x060005F7 RID: 1527 RVA: 0x0007F918 File Offset: 0x0007DB18
			get
			{
				foreach (IClickableMenu i in Game1.onScreenMenus)
				{
					if (i is ChatBox)
					{
						return (ChatBox)i;
					}
				}
				return null;
			}
		}

		// Token: 0x1700007A RID: 122
		public static Event CurrentEvent
		{
			// Token: 0x060005F8 RID: 1528 RVA: 0x0007F978 File Offset: 0x0007DB78
			get
			{
				if (Game1.currentLocation == null)
				{
					return null;
				}
				return Game1.currentLocation.currentEvent;
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0007F990 File Offset: 0x0007DB90
		public Game1()
		{
			Game1.game1 = this;
			if (!Program.releaseBuild)
			{
				if (Program.buildType == 0)
				{
					SteamHelper.steamworksEnabled = true;
				}
				else if (Program.buildType == 1)
				{
					GOGHelper.gogEnabled = true;
				}
				if (Program.buildType == 0)
				{
					try
					{
						SteamHelper.initialize();
						goto IL_71;
					}
					catch (Exception)
					{
						goto IL_71;
					}
				}
				int arg_70_0 = Program.buildType;
			}
			IL_71:
			Game1.graphics = new GraphicsDeviceManager(this);
			Game1.graphics.PreferredBackBufferWidth = 1280;
			Game1.graphics.PreferredBackBufferHeight = 720;
			base.Window.AllowUserResizing = true;
			base.Content.RootDirectory = "Content";
			Game1.temporaryContent = new LocalizedContentManager(base.Content.ServiceProvider, base.Content.RootDirectory);
			base.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
			base.Exiting += new EventHandler<EventArgs>(this.exitEvent);
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0007FABC File Offset: 0x0007DCBC
		public void exitEvent(object sender, EventArgs e)
		{
			if (Game1.IsServer && Game1.server != null)
			{
				Game1.server.stopServer();
			}
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0007FAE0 File Offset: 0x0007DCE0
		public void refreshWindowSettings()
		{
			this.Window_ClientSizeChanged(null, null);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0007FAEC File Offset: 0x0007DCEC
		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			if (Game1.options == null)
			{
				return;
			}
			Microsoft.Xna.Framework.Rectangle oldWindow = new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height);
			base.Window.ClientSizeChanged -= new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
			int w = Game1.graphics.IsFullScreen ? Game1.graphics.PreferredBackBufferWidth : base.Window.ClientBounds.Width;
			int h = Game1.graphics.IsFullScreen ? Game1.graphics.PreferredBackBufferHeight : base.Window.ClientBounds.Height;
			try
			{
				Viewport arg_AF_0 = Game1.graphics.GraphicsDevice.Viewport;
				if (w < 1280)
				{
					Game1.graphics.PreferredBackBufferWidth = 1280;
					w = 1280;
				}
				if (h < 720)
				{
					Game1.graphics.PreferredBackBufferHeight = 720;
					h = 720;
				}
			}
			catch (Exception)
			{
				Game1.graphics.PreferredBackBufferWidth = 1280;
				Game1.graphics.PreferredBackBufferHeight = 720;
			}
			Game1.updateViewportForScreenSizeChange(false, w, h);
			if (Game1.bloom != null)
			{
				Game1.bloom.reload();
			}
			Game1.graphics.ApplyChanges();
			try
			{
				this.screen = new RenderTarget2D(Game1.graphics.GraphicsDevice, Math.Min(4096, (int)((float)base.Window.ClientBounds.Width * (1f / Game1.options.zoomLevel))), Math.Min(4096, (int)((float)base.Window.ClientBounds.Height * (1f / Game1.options.zoomLevel))));
				Game1.viewport = new xTile.Dimensions.Rectangle((int)Game1.player.position.X - Game1.viewport.Width / 2, (int)Game1.player.position.Y - Game1.viewport.Height / 2, (int)((float)base.Window.ClientBounds.Width * (1f / Game1.options.zoomLevel)), (int)((float)base.Window.ClientBounds.Height * (1f / Game1.options.zoomLevel)));
				Game1.previousViewportPosition = new Vector2(Game1.player.position.X - (float)(Game1.viewport.Width / 2), Game1.player.position.Y - (float)(Game1.viewport.Height / 2));
			}
			catch (Exception)
			{
			}
			try
			{
				if (Game1.graphics.IsFullScreen)
				{
					Game1.graphics.GraphicsDevice.Viewport = new Viewport(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight));
				}
				else
				{
					Game1.graphics.GraphicsDevice.Viewport = new Viewport(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height));
				}
			}
			catch (Exception)
			{
			}
			using (List<IClickableMenu>.Enumerator enumerator = Game1.onScreenMenus.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.gameWindowSizeChanged(oldWindow, new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
				}
			}
			if (Game1.currentMinigame != null)
			{
				Game1.currentMinigame.changeScreenSize();
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.gameWindowSizeChanged(oldWindow, new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
			}
			if (Game1.activeClickableMenu is GameMenu && !Game1.overrideGameMenuReset)
			{
				Game1.activeClickableMenu = new GameMenu((Game1.activeClickableMenu as GameMenu).currentTab, -1);
			}
			base.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0007FF38 File Offset: 0x0007E138
		private void Game1_Exiting(object sender, EventArgs e)
		{
			if (Program.buildType == 0)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0007FF48 File Offset: 0x0007E148
		public static void setGameMode(byte mode)
		{
			try
			{
				Game1.gameMode = mode;
				if (Game1.temporaryContent != null)
				{
					Game1.temporaryContent.Unload();
				}
				if (mode == 0)
				{
					bool arg_44_0 = Game1.activeClickableMenu != null && Game1.currentGameTime.TotalGameTime.Seconds > 10;
					Game1.activeClickableMenu = new TitleMenu();
					if (arg_44_0)
					{
						(Game1.activeClickableMenu as TitleMenu).skipToTitleButtons();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0007FFC0 File Offset: 0x0007E1C0
		public static void updateViewportForScreenSizeChange(bool fullscreenChange, int width, int height)
		{
			Point center = new Point(Game1.viewport.X + Game1.viewport.Width / 2, Game1.viewport.Y + Game1.viewport.Height / 2);
			Game1.viewport = new xTile.Dimensions.Rectangle(center.X - width / 2, center.Y - height / 2, width + width % Game1.tileSize, height + height % Game1.tileSize);
			Game1.lightmap = new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)((float)width * (1f / Game1.options.zoomLevel) + (float)Game1.tileSize) / (Game1.options.lightingQuality / 2), (int)((float)height * (1f / Game1.options.zoomLevel) + (float)Game1.tileSize) / (Game1.options.lightingQuality / 2));
			if (Game1.currentLocation == null)
			{
				return;
			}
			if ((Game1.viewport.X >= 0 || !Game1.currentLocation.IsOutdoors) | fullscreenChange)
			{
				if (Game1.eventUp)
				{
					if (Game1.currentLocation.map.DisplayHeight < height && Game1.currentLocation.map.DisplayWidth < width)
					{
						Game1.viewport = new xTile.Dimensions.Rectangle(Game1.graphics.GraphicsDevice.Viewport.X, Game1.graphics.GraphicsDevice.Viewport.Y, width + width % Game1.tileSize, height + height % Game1.tileSize);
						Game1.UpdateViewPort(true, new Point(Game1.player.getStandingX(), Game1.player.getStandingY()));
						return;
					}
					center = new Point(Game1.viewport.X + Game1.viewport.Width / 2, Game1.viewport.Y + Game1.viewport.Height / 2);
					Game1.viewport = new xTile.Dimensions.Rectangle(center.X - width / 2, center.Y - height / 2, width + width % Game1.tileSize, height + height % Game1.tileSize);
					Game1.UpdateViewPort(true, center);
					return;
				}
				else
				{
					center = new Point(Game1.viewport.X + Game1.viewport.Width / 2, Game1.viewport.Y + Game1.viewport.Height / 2);
					Game1.viewport = new xTile.Dimensions.Rectangle(center.X - width / 2, center.Y - height / 2, width + width % Game1.tileSize, height + height % Game1.tileSize);
					Game1.UpdateViewPort(true, center);
				}
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00080234 File Offset: 0x0007E434
		protected override void Initialize()
		{
			Game1.viewport = new xTile.Dimensions.Rectangle(new Size(1280, 720));
			base.Initialize();
			Game1.keyboardDispatcher = new KeyboardDispatcher(base.Window);
			Game1.mapDisplayDevice = new XnaDisplayDevice(base.Content, base.GraphicsDevice);
			base.IsFixedTimeStep = true;
			Game1.graphics.SynchronizeWithVerticalRetrace = true;
			this.screen = new RenderTarget2D(Game1.graphics.GraphicsDevice, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height);
			string rootpath = base.Content.RootDirectory;
			if (!File.Exists(Path.Combine(rootpath, "XACT", "FarmerSounds.xgs")))
			{
				if (Program.buildType == 0)
				{
					rootpath = "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Stardew Valley\\Content";
					if (!Directory.Exists(rootpath))
					{
						rootpath = "C:\\Program Files\\Steam\\SteamApps\\common\\Stardew Valley\\Content";
					}
				}
				else if (Program.buildType == 1)
				{
					rootpath = "C:\\GOG Games\\Stardew Valley\\Content";
				}
			}
			Game1.audioEngine = new AudioEngine(Path.Combine(rootpath, "XACT", "FarmerSounds.xgs"));
			Game1.waveBank = new WaveBank(Game1.audioEngine, Path.Combine(rootpath, "XACT", "Wave Bank.xwb"));
			Game1.soundBank = new SoundBank(Game1.audioEngine, Path.Combine(rootpath, "XACT", "Sound Bank.xsb"));
			Game1.audioEngine.Update();
			Game1.musicCategory = Game1.audioEngine.GetCategory("Music");
			Game1.soundCategory = Game1.audioEngine.GetCategory("Sound");
			Game1.ambientCategory = Game1.audioEngine.GetCategory("Ambient");
			Game1.footstepCategory = Game1.audioEngine.GetCategory("Footsteps");
			Game1.currentSong = null;
			if (Game1.soundBank != null)
			{
				Game1.fuseSound = Game1.soundBank.GetCue("fuse");
				Game1.wind = Game1.soundBank.GetCue("wind");
				Game1.chargeUpSound = Game1.soundBank.GetCue("toolCharge");
			}
			Game1.setGameMode(0);
			if (Program.releaseBuild)
			{
				if (Program.buildType == 0)
				{
					SteamHelper.steamworksEnabled = true;
				}
				else if (Program.buildType == 1)
				{
					GOGHelper.gogEnabled = true;
				}
				if (Program.buildType == 0)
				{
					try
					{
						SteamHelper.initialize();
						goto IL_226;
					}
					catch (Exception)
					{
						goto IL_226;
					}
				}
				int arg_225_0 = Program.buildType;
			}
			IL_226:
			Game1.previousViewportPosition = Vector2.Zero;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00080484 File Offset: 0x0007E684
		public static void pauseThenDoFunction(int pauseTime, Game1.afterFadeFunction function)
		{
			Game1.afterPause = function;
			Game1.pauseThenDoFunctionTimer = pauseTime;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00080494 File Offset: 0x0007E694
		public static string dayOrNight()
		{
			string dayOrNight = "_day";
			int dayOfYear = DateTime.Now.DayOfYear;
			int sunset = (int)(1.75 * Math.Sin(0.017214206321039961 * (double)dayOfYear - 79.0) + 18.75);
			if (DateTime.Now.TimeOfDay.TotalHours >= (double)sunset || DateTime.Now.TimeOfDay.TotalHours < 5.0)
			{
				dayOrNight = "_night";
			}
			return dayOrNight;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00080528 File Offset: 0x0007E728
		public void dummyLoad()
		{
			Game1.content.Unload();
			Game1.content.Dispose();
			Game1.game1.Content = new LocalizedContentManager(Game1.content.ServiceProvider, Game1.content.RootDirectory);
			Game1.temporaryContent.Unload();
			Game1.temporaryContent.Dispose();
			Game1.temporaryContent = ((LocalizedContentManager)Game1.game1.Content).CreateTemporary();
			Game1.mapDisplayDevice = new XnaDisplayDevice(base.Content, base.GraphicsDevice);
			this.LoadContent();
			Game1.exitActiveMenu();
			Game1.setGameMode(3);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000805C0 File Offset: 0x0007E7C0
		protected override void LoadContent()
		{
			Game1.options = new Options();
			Game1.options.musicVolumeLevel = 1f;
			Game1.options.soundVolumeLevel = 1f;
			Game1.content = new LocalizedContentManager(base.Content.ServiceProvider, base.Content.RootDirectory);
			Game1.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			Game1.otherFarmers = new Dictionary<long, Farmer>();
			Game1.daybg = base.Content.Load<Texture2D>("LooseSprites\\daybg");
			Game1.nightbg = base.Content.Load<Texture2D>("LooseSprites\\nightbg");
			Game1.menuTexture = base.Content.Load<Texture2D>("Maps\\MenuTiles");
			Game1.lantern = base.Content.Load<Texture2D>("LooseSprites\\Lighting\\lantern");
			Game1.windowLight = base.Content.Load<Texture2D>("LooseSprites\\Lighting\\windowLight");
			Game1.sconceLight = base.Content.Load<Texture2D>("LooseSprites\\Lighting\\sconceLight");
			Game1.cauldronLight = base.Content.Load<Texture2D>("LooseSprites\\Lighting\\greenLight");
			Game1.indoorWindowLight = base.Content.Load<Texture2D>("LooseSprites\\Lighting\\indoorWindowLight");
			Game1.shadowTexture = base.Content.Load<Texture2D>("LooseSprites\\shadow");
			Game1.mouseCursors = base.Content.Load<Texture2D>("LooseSprites\\Cursors");
			Game1.animations = base.Content.Load<Texture2D>("TileSheets\\animations");
			Game1.achievements = Game1.content.Load<Dictionary<int, string>>("Data\\Achievements");
			if (Game1.bloom != null)
			{
				Game1.bloom.Visible = false;
			}
			Game1.fadeToBlackRect = new Texture2D(base.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Color[] white = new Color[]
			{
				Color.White
			};
			Game1.fadeToBlackRect.SetData<Color>(white);
			Game1.dialogueWidth = Math.Min(1280 - Game1.tileSize * 4, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.tileSize * 4);
			NameSelect.load();
			Game1.eventConditions = Game1.content.Load<Dictionary<string, bool>>("Data\\eventConditions");
			Game1.NPCGiftTastes = Game1.content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
			white = new Color[1];
			Game1.staminaRect = new Texture2D(base.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Game1.onScreenMenus.Clear();
			Game1.onScreenMenus.Add(new Toolbar());
			for (int i = 0; i < white.Length; i++)
			{
				white[i] = new Color(255, 255, 255, 255);
			}
			Game1.staminaRect.SetData<Color>(white);
			Game1.lightmap = new RenderTarget2D(base.GraphicsDevice, (base.GraphicsDevice.Viewport.Width + Game1.tileSize) / 32, (base.GraphicsDevice.Viewport.Height + Game1.tileSize) / 32 + Game1.tileSize, true, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);
			Game1.saveOnNewDay = true;
			Game1.littleEffect = new Texture2D(base.GraphicsDevice, 4, 4, false, SurfaceFormat.Color);
			white = new Color[16];
			for (int j = 0; j < white.Length; j++)
			{
				white[j] = new Color(255, 255, 255, 255);
			}
			Game1.littleEffect.SetData<Color>(white);
			for (int k = 0; k < 70; k++)
			{
				Game1.rainDrops[k] = new RainDrop(Game1.random.Next(Game1.viewport.Width), Game1.random.Next(Game1.viewport.Height), Game1.random.Next(4), Game1.random.Next(70));
			}
			Game1.player = new Farmer(new FarmerSprite(null), new Vector2(-1000f, -1000f), 0, "", new List<Item>(), true);
			Game1.player.FarmerSprite.setOwner(Game1.player);
			Game1.dayTimeMoneyBox = new DayTimeMoneyBox();
			Game1.onScreenMenus.Add(Game1.dayTimeMoneyBox);
			Game1.buffsDisplay = new BuffsDisplay();
			Game1.onScreenMenus.Add(Game1.buffsDisplay);
			Game1.dialogueFont = base.Content.Load<SpriteFont>("Fonts\\SpriteFont1");
			Game1.dialogueFont.LineSpacing = 42;
			Game1.smallFont = base.Content.Load<SpriteFont>("Fonts\\SmallFont");
			Game1.smallFont.LineSpacing = 26;
			Game1.borderFont = base.Content.Load<SpriteFont>("Fonts\\BorderFont");
			Game1.tinyFont = base.Content.Load<SpriteFont>("Fonts\\tinyFont");
			Game1.tinyFontBorder = base.Content.Load<SpriteFont>("Fonts\\tinyFontBorder");
			Game1.smoothFont = base.Content.Load<SpriteFont>("Fonts\\smoothFont");
			Game1.objectSpriteSheet = base.Content.Load<Texture2D>("Maps\\springobjects");
			Game1.toolSpriteSheet = base.Content.Load<Texture2D>("TileSheets\\tools");
			Game1.cropSpriteSheet = base.Content.Load<Texture2D>("TileSheets\\crops");
			Game1.emoteSpriteSheet = base.Content.Load<Texture2D>("TileSheets\\emotes");
			Game1.debrisSpriteSheet = base.Content.Load<Texture2D>("TileSheets\\debris");
			Game1.bigCraftableSpriteSheet = base.Content.Load<Texture2D>("TileSheets\\Craftables");
			Game1.rainTexture = base.Content.Load<Texture2D>("TileSheets\\rain");
			Game1.buffsIcons = base.Content.Load<Texture2D>("TileSheets\\BuffsIcons");
			Game1.objectInformation = base.Content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
			Game1.bigCraftablesInformation = base.Content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
			if (Game1.gameMode == 4)
			{
				Game1.fadeToBlackAlpha = -0.5f;
				Game1.fadeIn = true;
			}
			if (Game1.random.NextDouble() < 0.7)
			{
				Game1.isDebrisWeather = true;
				Game1.populateDebrisWeatherArray();
			}
			Game1.resetPlayer();
			Game1.viewport.X = 128;
			Game1.viewport.Y = 0;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00080B84 File Offset: 0x0007ED84
		public static void resetPlayer()
		{
			List<Item> farmersInitialTools = new List<Item>();
			farmersInitialTools.Add(new Axe());
			farmersInitialTools.Add(new Hoe());
			farmersInitialTools.Add(new WateringCan());
			farmersInitialTools.Add(new Pickaxe());
			farmersInitialTools.Add(new MeleeWeapon(47));
			farmersInitialTools.Add(null);
			Game1.player = new Farmer(new FarmerSprite(null), new Vector2(192f, 192f), 1, "Max", farmersInitialTools, true);
			Game1.player.Name = "";
			long id = Game1.player.uniqueMultiplayerID;
			Game1.player.FarmerSprite.setOwner(Game1.player);
			Game1.player.uniqueMultiplayerID = id;
			Game1.player.craftingRecipes.Add("Chest", 0);
			Game1.player.craftingRecipes.Add("Wood Fence", 0);
			Game1.player.craftingRecipes.Add("Gate", 0);
			Game1.player.craftingRecipes.Add("Torch", 0);
			Game1.player.craftingRecipes.Add("Campfire", 0);
			Game1.player.craftingRecipes.Add("Wood Path", 0);
			Game1.player.craftingRecipes.Add("Cobblestone Path", 0);
			Game1.player.craftingRecipes.Add("Gravel Path", 0);
			Game1.player.cookingRecipes.Add("Fried Egg", 0);
			Game1.player.songsHeard.Add("title_day");
			Game1.player.songsHeard.Add("title_night");
			Game1.player.changeShirt(0);
			Game1.player.changeSkinColor(0);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00080D34 File Offset: 0x0007EF34
		public static void resetVariables()
		{
			Game1.xLocationAfterWarp = 0;
			Game1.yLocationAfterWarp = 0;
			Game1.gameTimeInterval = 0;
			Game1.currentQuestionChoice = 0;
			Game1.currentDialogueCharacterIndex = 0;
			Game1.dialogueTypingInterval = 0;
			Game1.dayOfMonth = 0;
			Game1.year = 1;
			Game1.timeOfDay = 600;
			Game1.numberOfSelectedItems = -1;
			Game1.priceOfSelectedItem = 0;
			Game1.currentWallpaper = 0;
			Game1.farmerWallpaper = 22;
			Game1.wallpaperPrice = 75;
			Game1.currentFloor = 3;
			Game1.FarmerFloor = 29;
			Game1.floorPrice = 75;
			Game1.countdownToWedding = 0;
			Game1.facingDirectionAfterWarp = 0;
			Game1.dialogueWidth = 0;
			Game1.menuChoice = 0;
			Game1.tvStation = -1;
			Game1.currentBillboard = 0;
			Game1.facingDirectionAfterWarp = 0;
			Game1.tmpTimeOfDay = 0;
			Game1.percentageToWinStardewHero = 70;
			Game1.mouseClickPolling = 0;
			Game1.weatherIcon = 0;
			Game1.hitShakeTimer = 0;
			Game1.staminaShakeTimer = 0;
			Game1.pauseThenDoFunctionTimer = 0;
			Game1.weatherForTomorrow = 0;
			Game1.currentSongIndex = 3;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00080E10 File Offset: 0x0007F010
		public static void playSound(string cueName)
		{
			if (Game1.soundBank != null)
			{
				Game1.soundBank.PlayCue(cueName);
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00080E24 File Offset: 0x0007F024
		public static void loadForNewGame(bool loadedGame = false)
		{
			Game1.locations.Clear();
			Game1.mailbox.Clear();
			Game1.currentLightSources.Clear();
			if (Game1.dealerCalicoJackTotal != null)
			{
				Game1.dealerCalicoJackTotal.Clear();
			}
			Game1.questionChoices.Clear();
			Game1.hudMessages.Clear();
			Game1.weddingToday = false;
			Game1.countdownToWedding = 0;
			Game1.timeOfDay = 600;
			Game1.currentSeason = "spring";
			if (!loadedGame)
			{
				Game1.year = 1;
			}
			Game1.dayOfMonth = 0;
			Game1.pickingTool = false;
			Game1.isQuestion = false;
			Game1.nonWarpFade = false;
			Game1.particleRaining = false;
			Game1.newDay = false;
			Game1.inMine = false;
			Game1.isEating = false;
			Game1.menuUp = false;
			Game1.eventUp = false;
			Game1.viewportFreeze = false;
			Game1.eventOver = false;
			Game1.nameSelectUp = false;
			Game1.screenGlow = false;
			Game1.screenGlowHold = false;
			Game1.screenGlowUp = false;
			Game1.progressBar = false;
			Game1.isRaining = false;
			Game1.killScreen = false;
			Game1.coopDwellerBorn = false;
			Game1.messagePause = false;
			Game1.isDebrisWeather = false;
			Game1.boardingBus = false;
			Game1.listeningForKeyControlDefinitions = false;
			Game1.weddingToday = false;
			Game1.exitToTitle = false;
			Game1.messageAfterPause = "";
			Game1.fertilizer = "";
			Game1.samBandName = "The Alfalfas";
			Game1.slotResult = "";
			Game1.resetVariables();
			Game1.chanceToRainTomorrow = 0.0;
			Game1.dailyLuck = 0.001;
			if (!loadedGame)
			{
				Game1.stats = new Stats();
				Game1.options = new Options();
			}
			Game1.cropsOfTheWeek = Utility.cropsOfTheWeek();
			if (Game1.IsMultiplayer)
			{
				Game1.onScreenMenus.Add(new ChatBox());
			}
			AmbientLocationSounds.initialize();
			Game1.outdoorLight = Color.White;
			Game1.ambientLight = Color.White;
			int dishOfTheDayIndex = Game1.random.Next(194, 240);
			if (dishOfTheDayIndex == 217)
			{
				dishOfTheDayIndex = 216;
			}
			Game1.dishOfTheDay = new Object(Vector2.Zero, dishOfTheDayIndex, Game1.random.Next(1, 4 + ((Game1.random.NextDouble() < 0.08) ? 10 : 0)));
			Game1.locations.Clear();
			Map expr_214 = Game1.content.Load<Map>("Maps\\FarmHouse");
			expr_214.LoadTileSheets(Game1.mapDisplayDevice);
			Game1.currentLocation = new FarmHouse(expr_214, "FarmHouse");
			Game1.locations.Add(Game1.currentLocation);
			Game1.locations.Add(new Farm(Game1.content.Load<Map>("Maps\\" + Farm.getMapNameFromTypeInt(Game1.whichFarm)), "Farm"));
			if (Game1.whichFarm == 3)
			{
				for (int i = 0; i < 28; i++)
				{
					Game1.getFarm().doDailyMountainFarmUpdate();
				}
			}
			Game1.locations.Add(new FarmCave(Game1.content.Load<Map>("Maps\\FarmCave"), "FarmCave"));
			Game1.locations.Add(new Town(Game1.content.Load<Map>("Maps\\Town"), "Town"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\JoshHouse"), "JoshHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\George"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(16 * Game1.tileSize), (float)(22 * Game1.tileSize)), "JoshHouse", 0, "George", false, null, Game1.content.Load<Texture2D>("Portraits\\George")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Evelyn"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(2 * Game1.tileSize), (float)(17 * Game1.tileSize)), "JoshHouse", 1, "Evelyn", false, null, Game1.content.Load<Texture2D>("Portraits\\Evelyn")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Alex"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(19 * Game1.tileSize), (float)(5 * Game1.tileSize)), "JoshHouse", 3, "Alex", true, null, Game1.content.Load<Texture2D>("Portraits\\Alex")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\HaleyHouse"), "HaleyHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Emily"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(16 * Game1.tileSize), (float)(5 * Game1.tileSize)), "HaleyHouse", 2, "Emily", true, null, Game1.content.Load<Texture2D>("Portraits\\Emily")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Haley"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(8 * Game1.tileSize), (float)(7 * Game1.tileSize)), "HaleyHouse", 1, "Haley", true, null, Game1.content.Load<Texture2D>("Portraits\\Haley")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\SamHouse"), "SamHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Jodi"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(4 * Game1.tileSize), (float)(5 * Game1.tileSize)), "SamHouse", 0, "Jodi", false, null, Game1.content.Load<Texture2D>("Portraits\\Jodi")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Sam"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(22 * Game1.tileSize), (float)(13 * Game1.tileSize)), "SamHouse", 1, "Sam", true, null, Game1.content.Load<Texture2D>("Portraits\\Sam")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Vincent"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(10 * Game1.tileSize), (float)(23 * Game1.tileSize)), "SamHouse", 2, "Vincent", false, null, Game1.content.Load<Texture2D>("Portraits\\Vincent")));
			if (Game1.year > 1)
			{
				Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Kent"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(8 * Game1.tileSize), (float)(13 * Game1.tileSize)), "SamHouse", 2, "Kent", false, null, Game1.content.Load<Texture2D>("Portraits\\Kent")));
			}
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Blacksmith"), "Blacksmith"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Clint"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(3 * Game1.tileSize), (float)(13 * Game1.tileSize)), "Blacksmith", 2, "Clint", false, null, Game1.content.Load<Texture2D>("Portraits\\Clint")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\ManorHouse"), "ManorHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Lewis"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(8 * Game1.tileSize), (float)(5 * Game1.tileSize)), "ManorHouse", 0, "Lewis", false, null, Game1.content.Load<Texture2D>("Portraits\\Lewis")));
			Game1.locations.Add(new SeedShop(Game1.content.Load<Map>("Maps\\SeedShop"), "SeedShop"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Caroline"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(22 * Game1.tileSize), (float)(5 * Game1.tileSize)), "SeedShop", 2, "Caroline", false, null, Game1.content.Load<Texture2D>("Portraits\\Caroline")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Abigail"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)Game1.tileSize, (float)(9 * Game1.tileSize + Game1.pixelZoom)), "SeedShop", 3, "Abigail", true, null, Game1.content.Load<Texture2D>("Portraits\\Abigail")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Pierre"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(4 * Game1.tileSize), (float)(17 * Game1.tileSize)), "SeedShop", 2, "Pierre", false, null, Game1.content.Load<Texture2D>("Portraits\\Pierre")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Saloon"), "Saloon"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Gus"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(18 * Game1.tileSize), (float)(6 * Game1.tileSize)), "Saloon", 2, "Gus", false, null, Game1.content.Load<Texture2D>("Portraits\\Gus")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Trailer"), "Trailer"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Pam"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(15 * Game1.tileSize), (float)(4 * Game1.tileSize)), "Trailer", 2, "Pam", false, null, Game1.content.Load<Texture2D>("Portraits\\Pam")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Penny"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(4 * Game1.tileSize), (float)(9 * Game1.tileSize)), "Trailer", 1, "Penny", true, null, Game1.content.Load<Texture2D>("Portraits\\Penny")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Hospital"), "Hospital"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\HarveyRoom"), "HarveyRoom"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Harvey"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(13 * Game1.tileSize), (float)(4 * Game1.tileSize)), "HarveyRoom", 1, "Harvey", true, null, Game1.content.Load<Texture2D>("Portraits\\Harvey")));
			Game1.locations.Add(new Beach(Game1.content.Load<Map>("Maps\\Beach"), "Beach"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\ElliottHouse"), "ElliottHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Elliott"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)Game1.tileSize, (float)(5 * Game1.tileSize)), "ElliottHouse", 0, "Elliott", true, null, Game1.content.Load<Texture2D>("Portraits\\Elliott")));
			Game1.locations.Add(new Mountain(Game1.content.Load<Map>("Maps\\Mountain"), "Mountain"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\ScienceHouse"), "ScienceHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Maru"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(2 * Game1.tileSize), (float)(4 * Game1.tileSize)), "ScienceHouse", 3, "Maru", true, null, Game1.content.Load<Texture2D>("Portraits\\Maru")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Robin"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(21 * Game1.tileSize), (float)(4 * Game1.tileSize)), "ScienceHouse", 1, "Robin", false, null, Game1.content.Load<Texture2D>("Portraits\\Robin")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Demetrius"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(19 * Game1.tileSize), (float)(4 * Game1.tileSize)), "ScienceHouse", 1, "Demetrius", false, null, Game1.content.Load<Texture2D>("Portraits\\Demetrius")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\SebastianRoom"), "SebastianRoom"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Sebastian"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(10 * Game1.tileSize), (float)(9 * Game1.tileSize)), "SebastianRoom", 1, "Sebastian", true, null, Game1.content.Load<Texture2D>("Portraits\\Sebastian")));
			GameLocation tent = new GameLocation(Game1.content.Load<Map>("Maps\\Tent"), "Tent");
			tent.addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Linus"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2(2f, 2f) * (float)Game1.tileSize, "Tent", 2, "Linus", false, null, Game1.content.Load<Texture2D>("Portraits\\Linus")));
			Game1.locations.Add(tent);
			Game1.locations.Add(new Forest(Game1.content.Load<Map>("Maps\\Forest"), "Forest"));
			Game1.locations.Add(new WizardHouse(Game1.content.Load<Map>("Maps\\WizardHouse"), "WizardHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Wizard"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(3 * Game1.tileSize), (float)(17 * Game1.tileSize)), "WizardHouse", 2, "Wizard", false, null, Game1.content.Load<Texture2D>("Portraits\\Wizard")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\AnimalShop"), "AnimalShop"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Marnie"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(12 * Game1.tileSize), (float)(14 * Game1.tileSize)), "AnimalShop", 2, "Marnie", false, null, Game1.content.Load<Texture2D>("Portraits\\Marnie")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Shane"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(25 * Game1.tileSize), (float)(6 * Game1.tileSize)), "AnimalShop", 3, "Shane", true, null, Game1.content.Load<Texture2D>("Portraits\\Shane")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Jas"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(4 * Game1.tileSize), (float)(6 * Game1.tileSize)), "AnimalShop", 2, "Jas", false, null, Game1.content.Load<Texture2D>("Portraits\\Jas")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\LeahHouse"), "LeahHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Leah"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(3 * Game1.tileSize), (float)(7 * Game1.tileSize)), "LeahHouse", 3, "Leah", true, null, Game1.content.Load<Texture2D>("Portraits\\Leah")));
			Game1.locations.Add(new BusStop(Game1.content.Load<Map>("Maps\\BusStop"), "BusStop"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Mine"), "Mine"));
			Game1.locations[Game1.locations.Count - 1].objects.Add(new Vector2(27f, 8f), new Object(Vector2.Zero, 78, false));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Dwarf"), 0, Game1.tileSize / 4, 24), new Vector2((float)(43 * Game1.tileSize), (float)(6 * Game1.tileSize)), "Mine", 2, "Dwarf", false, null, Game1.content.Load<Texture2D>("Portraits\\Dwarf"))
			{
				breather = false
			});
			Game1.locations.Add(new Sewer(Game1.content.Load<Map>("Maps\\Sewer"), "Sewer"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\BugLand"), "BugLand"));
			Game1.locations.Add(new Desert(Game1.content.Load<Map>("Maps\\Desert"), "Desert"));
			Game1.locations.Add(new Club(Game1.content.Load<Map>("Maps\\Club"), "Club"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\MrQi"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(8 * Game1.tileSize), (float)(4 * Game1.tileSize)), "Club", 0, "Mister Qi", false, null, Game1.content.Load<Texture2D>("Portraits\\MrQi")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\SandyHouse"), "SandyHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Sandy"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(2 * Game1.tileSize), (float)(5 * Game1.tileSize)), "SandyHouse", 2, "Sandy", false, null, Game1.content.Load<Texture2D>("Portraits\\Sandy")));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Bouncer"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(17 * Game1.tileSize), (float)(3 * Game1.tileSize)), "SandyHouse", 2, "Bouncer", false, null, Game1.content.Load<Texture2D>("Portraits\\Bouncer")));
			Game1.locations.Add(new LibraryMuseum(Game1.content.Load<Map>("Maps\\ArchaeologyHouse"), "ArchaeologyHouse"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Gunther"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(3 * Game1.tileSize), (float)(8 * Game1.tileSize)), "ArchaeologyHouse", 2, "Gunther", false, null, Game1.content.Load<Texture2D>("Portraits\\Gunther")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\WizardHouseBasement"), "WizardHouseBasement"));
			Game1.locations.Add(new AdventureGuild(Game1.content.Load<Map>("Maps\\AdventureGuild"), "AdventureGuild"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Marlon"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(5 * Game1.tileSize), (float)(11 * Game1.tileSize)), "AdventureGuild", 2, "Marlon", false, null, Game1.content.Load<Texture2D>("Portraits\\Marlon")));
			Game1.locations.Add(new Woods(Game1.content.Load<Map>("Maps\\Woods"), "Woods"));
			Game1.locations.Add(new Railroad(Game1.content.Load<Map>("Maps\\Railroad"), "Railroad"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\WitchSwamp"), "WitchSwamp"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Henchman"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(20 * Game1.tileSize), (float)(29 * Game1.tileSize)), "WitchSwamp", 2, "Henchman", false, null, Game1.content.Load<Texture2D>("Portraits\\Henchman")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\WitchHut"), "WitchHut"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\WitchWarpCave"), "WitchWarpCave"));
			Game1.locations.Add(new Summit(Game1.content.Load<Map>("Maps\\Summit"), "Summit"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\FishShop"), "FishShop"));
			Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Willy"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(5 * Game1.tileSize), (float)(4 * Game1.tileSize)), "FishShop", 2, "Willy", false, null, Game1.content.Load<Texture2D>("Portraits\\Willy")));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\BathHouse_Entry"), "BathHouse_Entry"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\BathHouse_MensLocker"), "BathHouse_MensLocker"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\BathHouse_WomensLocker"), "BathHouse_WomensLocker"));
			Game1.locations.Add(new BathHousePool(Game1.content.Load<Map>("Maps\\BathHouse_Pool"), "BathHouse_Pool"));
			Game1.locations.Add(new CommunityCenter("CommunityCenter"));
			Game1.locations.Add(new JojaMart(Game1.content.Load<Map>("Maps\\JojaMart"), "JojaMart"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Greenhouse"), "Greenhouse"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\SkullCave"), "SkullCave"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Backwoods"), "Backwoods"));
			Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Tunnel"), "Tunnel"));
			Game1.locations.Add(new Cellar(Game1.content.Load<Map>("Maps\\Cellar"), "Cellar"));
			NPC.populateRoutesFromLocationToLocationList();
			Game1.player.addQuest(9);
			Game1.player.currentLocation = Game1.getLocationFromName("FarmHouse");
			Game1.hudMessages.Clear();
			Game1.hasLoadedGame = true;
			Game1.setGraphicsForSeason();
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00082925 File Offset: 0x00080B25
		protected override void UnloadContent()
		{
			base.UnloadContent();
			Game1.spriteBatch.Dispose();
			Game1.content.Unload();
			if (Game1.server != null)
			{
				Game1.server.stopServer();
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00082954 File Offset: 0x00080B54
		public void errorUpdateLoop()
		{
			if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
			{
				Program.GameTesterMode = false;
				Game1.gameMode = 3;
			}
			if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				Program.gamePtr.Exit();
				Environment.Exit(1);
			}
			this.Update(new GameTime());
			this.BeginDraw();
			this.Draw(new GameTime());
			this.EndDraw();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000829C4 File Offset: 0x00080BC4
		public static void showRedMessage(string message)
		{
			Game1.addHUDMessage(new HUDMessage(message, 3));
			if (!message.Contains("Inventory"))
			{
				Game1.playSound("cancel");
				return;
			}
			if (!Game1.player.mailReceived.Contains("BackpackTip"))
			{
				Game1.player.mailReceived.Add("BackpackTip");
				Game1.addMailForTomorrow("pierreBackpack", false, false);
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00082A2B File Offset: 0x00080C2B
		public static void showRedMessageUsingLoadString(string loadString)
		{
			Game1.showRedMessage(Game1.content.LoadString(loadString, new object[0]));
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00082A44 File Offset: 0x00080C44
		public static bool didPlayerJustLeftClick()
		{
			return (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00082A84 File Offset: 0x00080C84
		public static bool didPlayerJustRightClick()
		{
			return (Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00082AC3 File Offset: 0x00080CC3
		public static bool didPlayerJustClickAtAll()
		{
			return Game1.didPlayerJustLeftClick() || Game1.didPlayerJustRightClick();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00082AD3 File Offset: 0x00080CD3
		public static void showGlobalMessage(string message)
		{
			Game1.addHUDMessage(new HUDMessage(message, ""));
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00082AE5 File Offset: 0x00080CE5
		public static void globalFadeToBlack(Game1.afterFadeFunction afterFade = null, float fadeSpeed = 0.02f)
		{
			Game1.globalFade = true;
			Game1.fadeIn = false;
			Game1.afterFade = afterFade;
			Game1.globalFadeSpeed = fadeSpeed;
			Game1.fadeToBlackAlpha = 0f;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00082B09 File Offset: 0x00080D09
		public static void globalFadeToClear(Game1.afterFadeFunction afterFade = null, float fadeSpeed = 0.02f)
		{
			Game1.globalFade = true;
			Game1.fadeIn = true;
			Game1.afterFade = afterFade;
			Game1.globalFadeSpeed = fadeSpeed;
			Game1.fadeToBlackAlpha = 1f;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00082B30 File Offset: 0x00080D30
		protected override void Update(GameTime gameTime)
		{
			if (Program.buildType == 0)
			{
				SteamHelper.update();
			}
			if ((Game1.paused || !base.IsActive) && (Game1.options == null || Game1.options.pauseWhenOutOfFocus || Game1.paused))
			{
				return;
			}
			if (Game1.quit)
			{
				base.Exit();
			}
			Game1.currentGameTime = gameTime;
			if (Game1.gameMode != 11)
			{
				if (Game1.IsMultiplayer && Game1.gameMode == 3)
				{
					if (Game1.multiplayerMode == 2)
					{
						Game1.server.receiveMessages();
					}
					else
					{
						Game1.client.receiveMessages();
					}
				}
				if (base.IsActive)
				{
					this.checkForEscapeKeys();
				}
				Game1.updateMusic();
				Game1.updateRaindropPosition();
				if (Game1.bloom != null)
				{
					Game1.bloom.tick(gameTime);
				}
				if (Game1.globalFade)
				{
					if (!Game1.dialogueUp)
					{
						if (Game1.fadeIn)
						{
							Game1.fadeToBlackAlpha = Math.Max(0f, Game1.fadeToBlackAlpha - Game1.globalFadeSpeed);
							if (Game1.fadeToBlackAlpha <= 0f)
							{
								Game1.globalFade = false;
								if (Game1.afterFade != null)
								{
									Game1.afterFadeFunction tmp = Game1.afterFade;
									Game1.afterFade();
									if (Game1.afterFade != null && Game1.afterFade.Equals(tmp))
									{
										Game1.afterFade = null;
									}
									if (Game1.nonWarpFade)
									{
										Game1.fadeToBlack = false;
									}
								}
							}
						}
						else
						{
							Game1.fadeToBlackAlpha = Math.Min(1f, Game1.fadeToBlackAlpha + Game1.globalFadeSpeed);
							if (Game1.fadeToBlackAlpha >= 1f)
							{
								Game1.globalFade = false;
								if (Game1.afterFade != null)
								{
									Game1.afterFadeFunction tmp2 = Game1.afterFade;
									Game1.afterFade();
									if (Game1.afterFade != null && Game1.afterFade.Equals(tmp2))
									{
										Game1.afterFade = null;
									}
									if (Game1.nonWarpFade)
									{
										Game1.fadeToBlack = false;
									}
								}
							}
						}
					}
					else if (Game1.farmEvent == null)
					{
						this.UpdateControlInput(gameTime);
					}
				}
				else if (Game1.pauseThenDoFunctionTimer > 0)
				{
					Game1.freezeControls = true;
					Game1.pauseThenDoFunctionTimer -= gameTime.ElapsedGameTime.Milliseconds;
					if (Game1.pauseThenDoFunctionTimer <= 0)
					{
						Game1.freezeControls = false;
						if (Game1.afterPause != null)
						{
							Game1.afterPause();
						}
					}
				}
				if (Game1.gameMode == 3 || Game1.gameMode == 2)
				{
					Game1.player.millisecondsPlayed += (uint)gameTime.ElapsedGameTime.Milliseconds;
					bool doMainGameUpdates = true;
					if (Game1.currentMinigame != null)
					{
						if (Game1.pauseTime > 0f)
						{
							Game1.updatePause(gameTime);
						}
						if (Game1.fadeToBlack)
						{
							Game1.updateScreenFade(gameTime);
							if (Game1.fadeToBlackAlpha >= 1f)
							{
								Game1.fadeToBlack = false;
							}
						}
						else
						{
							if (Game1.thumbstickMotionMargin > 0)
							{
								Game1.thumbstickMotionMargin -= gameTime.ElapsedGameTime.Milliseconds;
							}
							if (base.IsActive)
							{
								KeyboardState currentKBState = Keyboard.GetState();
								MouseState currentMouseState = Mouse.GetState();
								GamePadState currentPadState = GamePad.GetState(PlayerIndex.One);
								Microsoft.Xna.Framework.Input.Keys[] pressedKeys = currentKBState.GetPressedKeys();
								for (int k = 0; k < pressedKeys.Length; k++)
								{
									Microsoft.Xna.Framework.Input.Keys i = pressedKeys[k];
									if (!Game1.oldKBState.IsKeyDown(i))
									{
										Game1.currentMinigame.receiveKeyPress(i);
									}
								}
								if (Game1.options.gamepadControls)
								{
									if (Game1.currentMinigame == null)
									{
										Game1.oldMouseState = currentMouseState;
										Game1.oldKBState = currentKBState;
										Game1.oldPadState = currentPadState;
										return;
									}
									foreach (Buttons b in Utility.getPressedButtons(currentPadState, Game1.oldPadState))
									{
										Game1.currentMinigame.receiveKeyPress(Utility.mapGamePadButtonToKey(b));
									}
									if (Game1.currentMinigame == null)
									{
										Game1.oldMouseState = currentMouseState;
										Game1.oldKBState = currentKBState;
										Game1.oldPadState = currentPadState;
										return;
									}
									if (currentPadState.ThumbSticks.Right.Y < -0.2f && Game1.oldPadState.ThumbSticks.Right.Y >= -0.2f)
									{
										Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Down);
									}
									if (currentPadState.ThumbSticks.Right.Y > 0.2f && Game1.oldPadState.ThumbSticks.Right.Y <= 0.2f)
									{
										Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Up);
									}
									if (currentPadState.ThumbSticks.Right.X < -0.2f && Game1.oldPadState.ThumbSticks.Right.X >= -0.2f)
									{
										Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Left);
									}
									if (currentPadState.ThumbSticks.Right.X > 0.2f && Game1.oldPadState.ThumbSticks.Right.X <= 0.2f)
									{
										Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Right);
									}
									if (Game1.oldPadState.ThumbSticks.Right.Y < -0.2f && currentPadState.ThumbSticks.Right.Y >= -0.2f)
									{
										Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Down);
									}
									if (Game1.oldPadState.ThumbSticks.Right.Y > 0.2f && currentPadState.ThumbSticks.Right.Y <= 0.2f)
									{
										Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Up);
									}
									if (Game1.oldPadState.ThumbSticks.Right.X < -0.2f && currentPadState.ThumbSticks.Right.X >= -0.2f)
									{
										Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Left);
									}
									if (Game1.oldPadState.ThumbSticks.Right.X > 0.2f && currentPadState.ThumbSticks.Right.X <= 0.2f)
									{
										Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Right);
									}
									if (Game1.isGamePadThumbstickInMotion())
									{
										Game1.setMousePosition(Game1.getMouseX() + (int)(currentPadState.ThumbSticks.Left.X * 16f), Game1.getMouseY() - (int)(currentPadState.ThumbSticks.Left.Y * 16f));
										Game1.lastCursorMotionWasMouse = false;
									}
									else if (Game1.getMousePosition().X != Game1.getOldMouseX() || Game1.getMousePosition().Y != Game1.getOldMouseY())
									{
										Game1.lastCursorMotionWasMouse = true;
									}
								}
								pressedKeys = Game1.oldKBState.GetPressedKeys();
								for (int k = 0; k < pressedKeys.Length; k++)
								{
									Microsoft.Xna.Framework.Input.Keys j = pressedKeys[k];
									if (!currentKBState.IsKeyDown(j) && Game1.currentMinigame != null)
									{
										Game1.currentMinigame.receiveKeyRelease(j);
									}
								}
								if (Game1.options.gamepadControls)
								{
									if (Game1.currentMinigame == null)
									{
										Game1.oldMouseState = currentMouseState;
										Game1.oldKBState = currentKBState;
										Game1.oldPadState = currentPadState;
										return;
									}
									if (currentPadState.IsConnected && currentPadState.IsButtonDown(Buttons.X) && !Game1.oldPadState.IsButtonDown(Buttons.X))
									{
										Game1.currentMinigame.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
									}
									else if (currentPadState.IsConnected && currentPadState.IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))
									{
										Game1.currentMinigame.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
									}
									else if (currentPadState.IsConnected && !currentPadState.IsButtonDown(Buttons.X) && Game1.oldPadState.IsButtonDown(Buttons.X))
									{
										Game1.currentMinigame.releaseRightClick(Game1.getMouseX(), Game1.getMouseY());
									}
									else if (currentPadState.IsConnected && !currentPadState.IsButtonDown(Buttons.A) && Game1.oldPadState.IsButtonDown(Buttons.A))
									{
										Game1.currentMinigame.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
									}
									foreach (Buttons b2 in Utility.getPressedButtons(Game1.oldPadState, currentPadState))
									{
										Game1.currentMinigame.receiveKeyRelease(Utility.mapGamePadButtonToKey(b2));
									}
									if (currentPadState.IsConnected && currentPadState.IsButtonDown(Buttons.A) && Game1.currentMinigame != null)
									{
										Game1.currentMinigame.leftClickHeld(0, 0);
									}
								}
								if (Game1.currentMinigame == null)
								{
									Game1.oldMouseState = currentMouseState;
									Game1.oldKBState = currentKBState;
									Game1.oldPadState = currentPadState;
									return;
								}
								if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
								{
									Game1.currentMinigame.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
								}
								if (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
								{
									Game1.currentMinigame.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
								}
								if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
								{
									Game1.currentMinigame.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
								}
								if (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
								{
									Game1.currentMinigame.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
								}
								if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
								{
									Game1.currentMinigame.leftClickHeld(Game1.getMouseX(), Game1.getMouseY());
								}
								Game1.oldMouseState = currentMouseState;
								Game1.oldKBState = currentKBState;
								Game1.oldPadState = currentPadState;
							}
							if (Game1.currentMinigame != null && Game1.currentMinigame.tick(gameTime))
							{
								Game1.currentMinigame.unload();
								Game1.currentMinigame = null;
								Game1.fadeIn = true;
								Game1.fadeToBlackAlpha = 1f;
								return;
							}
						}
						doMainGameUpdates = Game1.IsMultiplayer;
					}
					else if (Game1.farmEvent != null && Game1.farmEvent.tickUpdate(gameTime))
					{
						Game1.farmEvent.makeChangesToLocation();
						Game1.timeOfDay = 600;
						Game1.UpdateOther(gameTime);
						Game1.displayHUD = true;
						Game1.farmEvent = null;
						Game1.currentLocation = Game1.getLocationFromName("FarmHouse");
						Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float)Game1.tileSize;
						Farmer expr_9F3_cp_0_cp_0 = Game1.player;
						expr_9F3_cp_0_cp_0.position.X = expr_9F3_cp_0_cp_0.position.X - (float)Game1.tileSize;
						Game1.changeMusicTrack("none");
						Game1.currentLocation.resetForPlayerEntry();
						Game1.player.forceCanMove();
						Game1.freezeControls = false;
						Game1.displayFarmer = true;
						Game1.outdoorLight = Color.White;
						Game1.viewportFreeze = false;
						Game1.fadeToBlackAlpha = 0f;
						Game1.fadeToBlack = false;
						Game1.globalFadeToClear(null, 0.02f);
						Game1.player.mailForTomorrow.Clear();
						Game1.showEndOfNightStuff();
					}
					if (doMainGameUpdates)
					{
						if (Game1.endOfNightMenus.Count > 0 && Game1.activeClickableMenu == null)
						{
							Game1.activeClickableMenu = Game1.endOfNightMenus.Pop();
						}
						if (Game1.activeClickableMenu != null)
						{
							Game1.updateActiveMenu(gameTime);
						}
						else
						{
							if (Game1.pauseTime > 0f)
							{
								Game1.updatePause(gameTime);
							}
							if (!Game1.globalFade && !Game1.freezeControls && Game1.activeClickableMenu == null && base.IsActive)
							{
								this.UpdateControlInput(gameTime);
							}
						}
						if (Game1.showingEndOfNightStuff && Game1.endOfNightMenus.Count == 0 && Game1.activeClickableMenu == null)
						{
							Game1.showingEndOfNightStuff = false;
							Game1.globalFadeToClear(new Game1.afterFadeFunction(Game1.playMorningSong), 0.02f);
						}
						if (!Game1.showingEndOfNightStuff)
						{
							if (Game1.IsMultiplayer || (Game1.activeClickableMenu == null && Game1.currentMinigame == null))
							{
								Game1.UpdateGameClock(gameTime);
							}
							this.UpdateCharacters(gameTime);
							this.UpdateLocations(gameTime);
							Game1.UpdateViewPort(false, this.getViewportCenter());
						}
						Game1.UpdateOther(gameTime);
						if (Game1.messagePause)
						{
							KeyboardState tmp3 = Keyboard.GetState();
							MouseState tmp4 = Mouse.GetState();
							GamePadState tmp5 = GamePad.GetState(PlayerIndex.One);
							if (Game1.isOneOfTheseKeysDown(tmp3, Game1.options.actionButton) && !Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.actionButton))
							{
								Game1.pressActionButton(tmp3, tmp4, tmp5);
							}
							Game1.oldKBState = tmp3;
							Game1.oldPadState = tmp5;
						}
					}
				}
				else
				{
					this.UpdateTitleScreen(gameTime);
					if (Game1.activeClickableMenu != null)
					{
						Game1.updateActiveMenu(gameTime);
					}
					if (Game1.gameMode == 10)
					{
						Game1.UpdateOther(gameTime);
					}
				}
				if (Game1.audioEngine != null)
				{
					Game1.audioEngine.Update();
				}
				if (Game1.multiplayerMode == 2 && Game1.gameMode == 3)
				{
					Game1.server.sendMessages(gameTime);
				}
			}
			base.Update(gameTime);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00083760 File Offset: 0x00081960
		public static bool isDarkOut()
		{
			return Game1.timeOfDay >= Game1.getTrulyDarkTime();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00083771 File Offset: 0x00081971
		public static bool isStartingToGetDarkOut()
		{
			return Game1.timeOfDay >= Game1.getStartingToGetDarkTime();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00083784 File Offset: 0x00081984
		public static int getStartingToGetDarkTime()
		{
			string a = Game1.currentSeason;
			if (a == "spring" || a == "summer")
			{
				return 1800;
			}
			if (a == "fall")
			{
				return 1700;
			}
			if (!(a == "winter"))
			{
				return 1800;
			}
			return 1600;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000837E4 File Offset: 0x000819E4
		public static int getModeratelyDarkTime()
		{
			return (Game1.getTrulyDarkTime() + Game1.getStartingToGetDarkTime()) / 2;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000837F3 File Offset: 0x000819F3
		public static int getTrulyDarkTime()
		{
			return Game1.getStartingToGetDarkTime() + 200;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00083800 File Offset: 0x00081A00
		public static void playMorningSong()
		{
			if (!Game1.isRaining && !Game1.isLightning && !Game1.eventUp && Game1.dayOfMonth > 0 && !Game1.currentLocation.Name.Equals("Desert"))
			{
				DelayedAction.playMusicAfterDelay(Game1.currentSeason + Math.Max(1, Game1.currentSongIndex), 500);
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00083864 File Offset: 0x00081A64
		private Point getViewportCenter()
		{
			if (Game1.previousViewportPosition.Equals(Game1.viewportCenter))
			{
				Game1.viewportTarget = new Vector2(-2.14748365E+09f, -2.14748365E+09f);
			}
			if (Game1.viewportTarget.X != -2.14748365E+09f)
			{
				if (Math.Abs((float)Game1.viewportCenter.X - Game1.viewportTarget.X) > Game1.viewportSpeed || Math.Abs((float)Game1.viewportCenter.Y - Game1.viewportTarget.Y) > Game1.viewportSpeed)
				{
					Vector2 velocity = Utility.getVelocityTowardPoint(Game1.viewportCenter, Game1.viewportTarget, Game1.viewportSpeed);
					Game1.viewportCenter.X = Game1.viewportCenter.X + (int)Math.Round((double)velocity.X);
					Game1.viewportCenter.Y = Game1.viewportCenter.Y + (int)Math.Round((double)velocity.Y);
				}
				else
				{
					if (Game1.viewportReachedTarget != null)
					{
						Game1.viewportReachedTarget();
						Game1.viewportReachedTarget = null;
					}
					Game1.viewportHold -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
					if (Game1.viewportHold <= 0)
					{
						Game1.viewportTarget = new Vector2(-2.14748365E+09f, -2.14748365E+09f);
						if (Game1.afterViewport != null)
						{
							Game1.afterViewport();
						}
					}
				}
				return Game1.viewportCenter;
			}
			Game1.viewportCenter.X = Game1.player.getStandingX();
			Game1.viewportCenter.Y = Game1.player.getStandingY();
			return Game1.viewportCenter;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000839DC File Offset: 0x00081BDC
		public static void afterFadeReturnViewportToPlayer()
		{
			Game1.viewportTarget = new Vector2(-2.14748365E+09f, -2.14748365E+09f);
			Game1.viewportHold = 0;
			Game1.viewportFreeze = false;
			Game1.viewportCenter.X = Game1.player.getStandingX();
			Game1.viewportCenter.Y = Game1.player.getStandingY();
			Game1.globalFadeToClear(null, 0.02f);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00083A3C File Offset: 0x00081C3C
		public static bool isViewportOnCustomPath()
		{
			return Game1.viewportTarget.X != -2.14748365E+09f;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00083A52 File Offset: 0x00081C52
		public static void moveViewportTo(Vector2 target, float speed, int holdTimer = 0, Game1.afterFadeFunction reachedTarget = null, Game1.afterFadeFunction endFunction = null)
		{
			Game1.viewportTarget = target;
			Game1.viewportSpeed = speed;
			Game1.viewportHold = holdTimer;
			Game1.afterViewport = endFunction;
			Game1.viewportReachedTarget = reachedTarget;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00083A73 File Offset: 0x00081C73
		public static Farm getFarm()
		{
			return Game1.getLocationFromName("Farm") as Farm;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00083A84 File Offset: 0x00081C84
		public static void setMousePosition(int x, int y)
		{
			Mouse.SetPosition((int)((float)x * Game1.options.zoomLevel), (int)((float)y * Game1.options.zoomLevel));
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00083AA7 File Offset: 0x00081CA7
		public static void setMousePosition(Point position)
		{
			Mouse.SetPosition((int)((float)position.X * Game1.options.zoomLevel), (int)((float)position.Y * Game1.options.zoomLevel));
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00083AD4 File Offset: 0x00081CD4
		public static Point getMousePosition()
		{
			return new Point(Game1.getMouseX(), Game1.getMouseY());
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00083AE8 File Offset: 0x00081CE8
		public static void updateActiveMenu(GameTime gameTime)
		{
			if (!Program.gamePtr.IsActive)
			{
				return;
			}
			MouseState mouseState = Mouse.GetState();
			KeyboardState keyState = Keyboard.GetState();
			GamePadState padState = GamePad.GetState(PlayerIndex.One);
			if (padState.IsConnected && !Game1.options.gamepadControls)
			{
				Game1.options.gamepadControls = true;
				Game1.showGlobalMessage("Gamepad mode activated");
				Game1.activeClickableMenu.setUpForGamePadMode();
			}
			else if (!padState.IsConnected && Game1.options.gamepadControls)
			{
				Game1.options.gamepadControls = false;
				Game1.showGlobalMessage("Gamepad disconnected");
				if (Game1.activeClickableMenu == null)
				{
					Game1.activeClickableMenu = new GameMenu();
				}
			}
			if (Game1.CurrentEvent != null && ((mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released) || (Game1.options.gamepadControls && padState.IsButtonDown(Buttons.A) && Game1.oldPadState.IsButtonUp(Buttons.A))))
			{
				Game1.CurrentEvent.receiveMouseClick(Game1.getMouseX(), Game1.getMouseY());
				if (Game1.CurrentEvent != null && Game1.CurrentEvent.skipped)
				{
					Game1.oldMouseState = mouseState;
					Game1.oldKBState = keyState;
					Game1.oldPadState = padState;
					return;
				}
			}
			if (padState.IsConnected && Game1.activeClickableMenu != null)
			{
				if (Game1.getMousePosition().Equals(Point.Zero) && Game1.activeClickableMenu.autoCenterMouseCursorForGamepad())
				{
					Game1.setMousePosition(Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2);
				}
				if (Game1.isGamePadThumbstickInMotion())
				{
					Mouse.SetPosition((int)((float)mouseState.X + padState.ThumbSticks.Left.X * 16f), (int)((float)mouseState.Y - padState.ThumbSticks.Left.Y * 16f));
					Game1.lastCursorMotionWasMouse = false;
				}
				if (Game1.activeClickableMenu != null)
				{
					foreach (Buttons b in Utility.getPressedButtons(padState, Game1.oldPadState))
					{
						Game1.activeClickableMenu.receiveGamePadButton(b);
						if (Game1.activeClickableMenu == null)
						{
							break;
						}
					}
				}
			}
			if ((Game1.getMouseX() != Game1.getOldMouseX() || Game1.getMouseY() != Game1.getOldMouseY()) && !Game1.isGamePadThumbstickInMotion())
			{
				Game1.lastCursorMotionWasMouse = true;
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.performHoverAction(Game1.getMouseX(), Game1.getMouseY());
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.update(gameTime);
			}
			if (Game1.activeClickableMenu != null && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
			{
				Game1.activeClickableMenu.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
			}
			else if (Game1.activeClickableMenu != null && mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && (Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released || ((float)Game1.mouseClickPolling > 650f && !(Game1.activeClickableMenu is DialogueBox))))
			{
				Game1.activeClickableMenu.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
				if ((float)Game1.mouseClickPolling > 650f)
				{
					Game1.mouseClickPolling = 600;
				}
				if (Game1.activeClickableMenu == null)
				{
					Game1.rightClickPolling = 500;
				}
			}
			if (mouseState.ScrollWheelValue != Game1.oldMouseState.ScrollWheelValue && Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.receiveScrollWheelAction(mouseState.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue);
			}
			if (Game1.options.gamepadControls && Game1.activeClickableMenu != null)
			{
				Game1.thumbstickPollingTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (Game1.thumbstickPollingTimer <= 0)
				{
					if (padState.ThumbSticks.Right.Y > 0.2f)
					{
						Game1.activeClickableMenu.receiveScrollWheelAction(1);
					}
					else if (padState.ThumbSticks.Right.Y < -0.2f)
					{
						Game1.activeClickableMenu.receiveScrollWheelAction(-1);
					}
				}
				if (Game1.thumbstickPollingTimer <= 0)
				{
					Game1.thumbstickPollingTimer = 220 - (int)(Math.Abs(padState.ThumbSticks.Right.Y) * 170f);
				}
				if (Math.Abs(padState.ThumbSticks.Right.Y) < 0.2f)
				{
					Game1.thumbstickPollingTimer = 0;
				}
			}
			if (Game1.activeClickableMenu != null && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Game1.activeClickableMenu.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
			}
			else if (Game1.activeClickableMenu != null && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Game1.activeClickableMenu.leftClickHeld(Game1.getMouseX(), Game1.getMouseY());
			}
			Microsoft.Xna.Framework.Input.Keys[] pressedKeys = keyState.GetPressedKeys();
			for (int j = 0; j < pressedKeys.Length; j++)
			{
				Microsoft.Xna.Framework.Input.Keys i = pressedKeys[j];
				if (Game1.activeClickableMenu != null && !Game1.oldKBState.GetPressedKeys().Contains(i))
				{
					Game1.activeClickableMenu.receiveKeyPress(i);
				}
			}
			if (Game1.options.gamepadControls)
			{
				if (padState.IsConnected && Game1.activeClickableMenu != null && !Game1.activeClickableMenu.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))
				{
					Game1.activeClickableMenu.receiveLeftClick(Game1.getMousePosition().X, Game1.getMousePosition().Y, true);
				}
				else if (padState.IsConnected && Game1.activeClickableMenu != null && !Game1.activeClickableMenu.areGamePadControlsImplemented() && padState.IsButtonDown(Buttons.X) && !Game1.oldPadState.IsButtonDown(Buttons.X))
				{
					Game1.activeClickableMenu.receiveRightClick(Game1.getMousePosition().X, Game1.getMousePosition().Y, true);
				}
				else
				{
					foreach (Buttons b2 in Utility.getPressedButtons(padState, Game1.oldPadState))
					{
						if (Game1.activeClickableMenu != null)
						{
							Game1.activeClickableMenu.receiveKeyPress(Utility.mapGamePadButtonToKey(b2));
						}
					}
				}
			}
			if (mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Game1.mouseClickPolling += gameTime.ElapsedGameTime.Milliseconds;
			}
			else
			{
				Game1.mouseClickPolling = 0;
			}
			Game1.oldMouseState = mouseState;
			Game1.oldKBState = keyState;
			Game1.oldPadState = padState;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00084160 File Offset: 0x00082360
		public static string DateCompiled()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			return string.Concat(new object[]
			{
				version.Major,
				".",
				version.Minor,
				".",
				version.Build,
				".",
				version.Revision
			});
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000841D8 File Offset: 0x000823D8
		public static void updatePause(GameTime gameTime)
		{
			Game1.pauseTime -= (float)gameTime.ElapsedGameTime.Milliseconds;
			if (Game1.player.isCrafting && Game1.random.NextDouble() < 0.007)
			{
				Game1.playSound("crafting");
			}
			if (Game1.pauseTime <= 0f)
			{
				if (Game1.currentObjectDialogue.Count == 0)
				{
					Game1.messagePause = false;
				}
				Game1.pauseTime = 0f;
				if (Game1.messageAfterPause != null && !Game1.messageAfterPause.Equals(""))
				{
					Game1.player.isCrafting = false;
					Game1.drawObjectDialogue(Game1.messageAfterPause);
					Game1.messageAfterPause = "";
					if (Game1.player.ActiveObject != null)
					{
						bool arg_C2_0 = Game1.player.ActiveObject.bigCraftable;
					}
					if (Game1.killScreen)
					{
						Game1.killScreen = false;
						Game1.player.health = 10;
					}
				}
				else if (Game1.killScreen)
				{
					Game1.screenGlow = false;
					if (Game1.currentLocation.Name.Equals("UndergroundMine") && Game1.mine.getMineArea(-1) != 121)
					{
						Game1.warpFarmer("Mine", 22, 9, false);
					}
					else
					{
						Game1.warpFarmer("Hospital", 20, 12, false);
					}
				}
				Game1.progressBar = false;
				if (Game1.currentLocation.currentEvent != null)
				{
					Event expr_14C = Game1.currentLocation.currentEvent;
					int currentCommand = expr_14C.CurrentCommand;
					expr_14C.CurrentCommand = currentCommand + 1;
				}
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00084340 File Offset: 0x00082540
		public static void initializeMultiplayerServer()
		{
			Game1.player.currentLocation = Game1.getLocationFromName("FarmHouse");
			Game1.server.initializeConnection();
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00084360 File Offset: 0x00082560
		public static void initializeMultiplayerClient()
		{
			Game1.client.receiveMessages();
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				v.Value.currentLocation = Game1.getLocationFromName(v.Value._tmpLocationName);
				v.Value.currentLocation.farmers.Add(v.Value);
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000843F0 File Offset: 0x000825F0
		public static void toggleNonBorderlessWindowedFullscreen(int width = -1, int height = -1)
		{
			if (width == -1)
			{
				width = Game1.options.preferredResolutionX;
			}
			if (height == -1)
			{
				height = Game1.options.preferredResolutionY;
			}
			Game1.graphics.PreferredBackBufferWidth = width;
			Game1.graphics.PreferredBackBufferHeight = height;
			Game1.graphics.ToggleFullScreen();
			Game1.graphics.ApplyChanges();
			Game1.updateViewportForScreenSizeChange(true, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
			Game1.isFullscreen = Game1.graphics.IsFullScreen;
			Form expr_8C = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
			expr_8C.FormBorderStyle = FormBorderStyle.Sizable;
			expr_8C.WindowState = FormWindowState.Normal;
			Program.gamePtr.Window_ClientSizeChanged(null, null);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000844A4 File Offset: 0x000826A4
		public static void toggleFullscreen()
		{
			Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
			if (Game1.options.windowedBorderlessFullscreen || form.WindowState == FormWindowState.Maximized)
			{
				if (form.WindowState != FormWindowState.Maximized || form.FormBorderStyle != FormBorderStyle.None)
				{
					form.FormBorderStyle = FormBorderStyle.None;
					form.WindowState = FormWindowState.Maximized;
					Game1.isFullscreen = true;
				}
				else
				{
					form.FormBorderStyle = FormBorderStyle.Sizable;
					form.WindowState = FormWindowState.Normal;
					Game1.isFullscreen = false;
					if (Game1.options.fullscreen && !Game1.options.windowedBorderlessFullscreen)
					{
						Program.gamePtr.Window_ClientSizeChanged(null, null);
						Game1.graphics.PreferredBackBufferWidth = Game1.options.preferredResolutionX;
						Game1.graphics.PreferredBackBufferHeight = Game1.options.preferredResolutionY;
						Game1.toggleNonBorderlessWindowedFullscreen(-1, -1);
						return;
					}
				}
			}
			else
			{
				Game1.toggleNonBorderlessWindowedFullscreen(-1, -1);
			}
			Program.gamePtr.Window_ClientSizeChanged(null, null);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00084588 File Offset: 0x00082788
		private void checkForEscapeKeys()
		{
			KeyboardState kbState = Keyboard.GetState();
			if (Game1.toggleFullScreen || (kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && (Game1.oldKBState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftAlt) || Game1.oldKBState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter))))
			{
				Game1.toggleFullscreen();
				if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is GameMenu)
				{
					Game1.exitActiveMenu();
					Game1.activeClickableMenu = new GameMenu(6, -1);
				}
				Game1.toggleFullScreen = false;
			}
			if ((Game1.player.UsingTool || Game1.freezeControls) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R) && kbState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete))
			{
				Game1.freezeControls = false;
				Game1.player.forceCanMove();
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.player.usingTool = false;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00084668 File Offset: 0x00082868
		public static bool isOneOfTheseKeysDown(KeyboardState state, InputButton[] keys)
		{
			for (int j = 0; j < keys.Length; j++)
			{
				InputButton i = keys[j];
				if (i.key != Microsoft.Xna.Framework.Input.Keys.None && state.IsKeyDown(i.key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000846A8 File Offset: 0x000828A8
		public static bool areAllOfTheseKeysUp(KeyboardState state, InputButton[] keys)
		{
			for (int j = 0; j < keys.Length; j++)
			{
				InputButton i = keys[j];
				if (i.key != Microsoft.Xna.Framework.Input.Keys.None && !state.IsKeyUp(i.key))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000846E8 File Offset: 0x000828E8
		private void UpdateTitleScreen(GameTime time)
		{
			if (Game1.quit)
			{
				base.Exit();
				Game1.changeMusicTrack("none");
			}
			if (Game1.gameMode == 6)
			{
				Game1.nextMusicTrack = "none";
				if (!Game1.currentLoader.MoveNext())
				{
					Game1.setGameMode(3);
					Game1.fadeIn = true;
					Game1.fadeToBlackAlpha = 0.99f;
				}
				return;
			}
			if (Game1.gameMode == 7)
			{
				Game1.currentLoader.MoveNext();
				return;
			}
			if (Game1.gameMode == 8)
			{
				Game1.pauseAccumulator -= (float)time.ElapsedGameTime.Milliseconds;
				if (Game1.pauseAccumulator <= 0f)
				{
					Game1.pauseAccumulator = 0f;
					Game1.setGameMode(3);
					if (Game1.currentObjectDialogue.Count > 0)
					{
						Game1.messagePause = true;
						Game1.pauseTime = 1E+10f;
						Game1.fadeToBlackAlpha = 1f;
						Game1.player.CanMove = false;
					}
				}
				return;
			}
			if (Game1.fadeToBlackAlpha < 1f && Game1.fadeIn)
			{
				Game1.fadeToBlackAlpha += 0.02f;
			}
			else if (Game1.fadeToBlackAlpha > 0f && Game1.fadeToBlack)
			{
				Game1.fadeToBlackAlpha -= 0.02f;
			}
			if (Game1.pauseTime > 0f)
			{
				Game1.pauseTime = Math.Max(0f, Game1.pauseTime - (float)time.ElapsedGameTime.Milliseconds);
			}
			if (Game1.gameMode == 0 && (double)Game1.fadeToBlackAlpha >= 0.98)
			{
				float arg_16C_0 = Game1.fadeToBlackAlpha;
			}
			if (Game1.fadeToBlackAlpha >= 1f)
			{
				if (Game1.gameMode == 4 && !Game1.fadeToBlack)
				{
					Game1.fadeIn = false;
					Game1.fadeToBlack = true;
					Game1.fadeToBlackAlpha = 2.5f;
				}
				else if (Game1.gameMode == 0 && Game1.currentSong == null && Game1.soundBank != null && Game1.pauseTime <= 0f && Game1.soundBank != null)
				{
					Game1.currentSong = Game1.soundBank.GetCue("spring_day_ambient");
					Game1.currentSong.Play();
				}
				if (Game1.gameMode == 0 && Game1.activeClickableMenu == null && !Game1.quit)
				{
					Game1.activeClickableMenu = new TitleMenu();
				}
			}
			else if (Game1.fadeToBlackAlpha <= 0f)
			{
				if (Game1.gameMode == 4 && Game1.fadeToBlack)
				{
					Game1.fadeIn = true;
					Game1.fadeToBlack = false;
					Game1.setGameMode(0);
					Game1.pauseTime = 2000f;
				}
				else if (Game1.gameMode == 0 && Game1.fadeToBlack && Game1.menuChoice == 0)
				{
					Game1.currentLoader = Utility.generateNewFarm(Game1.IsClient);
					Game1.setGameMode(6);
					Game1.loadingMessage = (Game1.IsClient ? ("Connecting to " + Game1.client.serverName + "...") : "Melting Winter Snow...");
					Game1.exitActiveMenu();
				}
			}
			bool arg_2B3_0 = base.IsActive;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000849AC File Offset: 0x00082BAC
		public static void colorFarmer()
		{
			Utility.changeFarmerEyeColor(Utility.getEyeColors()[Game1.player.eyeColor]);
			Utility.changeFarmerHairColor(Utility.getHairColors()[Game1.player.hairColor]);
			Utility.changeFarmerOverallsColor(Utility.getOverallsColors()[Game1.player.overallsColor]);
			Utility.changeFarmerShirtColor(Utility.getShirtColors()[Game1.player.shirtColor]);
			Utility.changeFarmerSkinColor(Utility.getSkinColors()[Game1.player.skinColor]);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00084A38 File Offset: 0x00082C38
		private void UpdateLocations(GameTime time)
		{
			if (!Game1.menuUp)
			{
				Game1.currentLocation.UpdateWhenCurrentLocation(time);
				if (Game1.IsServer)
				{
					using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.currentLocation.UpdateWhenCurrentLocation(time);
						}
					}
				}
				for (int i = 0; i < Game1.locations.Count; i++)
				{
					Game1.locations[i].updateEvenIfFarmerIsntHere(time, false);
				}
				if (Game1.currentLocation.Name.Equals("Temp"))
				{
					Game1.currentLocation.updateEvenIfFarmerIsntHere(time, false);
				}
				if (Game1.mine != null)
				{
					Game1.mine.updateEvenIfFarmerIsntHere(time, false);
				}
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00084B0C File Offset: 0x00082D0C
		public static void performTenMinuteClockUpdate()
		{
			if (Game1.IsServer)
			{
				MultiplayerUtility.broadcastGameClock();
			}
			int startToGetReallyDark = Game1.getTrulyDarkTime();
			Game1.gameTimeInterval = 0;
			Game1.timeOfDay += 10;
			if (Game1.timeOfDay % 100 >= 60)
			{
				Game1.timeOfDay = Game1.timeOfDay - Game1.timeOfDay % 100 + 100;
			}
			if (Game1.isLightning && Game1.timeOfDay < 2400)
			{
				Utility.performLightningUpdate();
			}
			if (Game1.timeOfDay == startToGetReallyDark)
			{
				Game1.currentLocation.switchOutNightTiles();
			}
			else if (Game1.timeOfDay == Game1.getModeratelyDarkTime())
			{
				if (Game1.currentLocation.IsOutdoors && !Game1.isRaining)
				{
					Game1.ambientLight = Color.White;
				}
				if (!Game1.isRaining && !(Game1.currentLocation is MineShaft) && Game1.currentSong != null && !Game1.currentSong.Name.Contains("ambient") && Game1.currentLocation is Town)
				{
					Game1.changeMusicTrack("none");
				}
			}
			if (Game1.currentLocation.isOutdoors && !Game1.isRaining && !Game1.eventUp && Game1.currentSong != null && Game1.currentSong.Name.Contains("day") && Game1.isDarkOut())
			{
				Game1.changeMusicTrack("none");
			}
			if (Game1.weatherIcon == 1)
			{
				if (Game1.temporaryContent == null)
				{
					Game1.temporaryContent = Game1.content.CreateTemporary();
				}
				int startTime = Convert.ToInt32(Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth)["conditions"].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[0]);
				if (Game1.whereIsTodaysFest == null)
				{
					Game1.whereIsTodaysFest = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth)["conditions"].Split(new char[]
					{
						'/'
					})[0];
				}
				if (Game1.timeOfDay == startTime)
				{
					string where = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth)["conditions"].Split(new char[]
					{
						'/'
					})[0];
					if (!(where == "Forest"))
					{
						if (!(where == "Town"))
						{
							if (where == "Beach")
							{
								where = "at the beach.";
							}
						}
						else
						{
							where = "in the town square.";
						}
					}
					else
					{
						where = (Game1.currentSeason.Equals("winter") ? "near Marnie's ranch." : "in the forest.");
					}
					Game1.showGlobalMessage("The " + Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth)["name"] + " has begun " + where);
				}
			}
			Game1.player.performTenMinuteUpdate();
			int num = Game1.timeOfDay;
			if (num <= 2400)
			{
				if (num != 1200)
				{
					if (num != 2000)
					{
						if (num == 2400)
						{
							Game1.dayTimeMoneyBox.timeShakeTimer = 2000;
							Game1.player.doEmote(24);
							Game1.showGlobalMessage("It's getting late...");
						}
					}
					else if (!Game1.isRaining && Game1.currentLocation is Town)
					{
						Game1.changeMusicTrack("none");
					}
				}
				else if (Game1.currentLocation.isOutdoors && !Game1.isRaining && (Game1.currentSong == null || Game1.currentSong.IsStopped || Game1.currentSong.Name.ToLower().Contains("ambient")))
				{
					Game1.playMorningSong();
				}
			}
			else if (num != 2500)
			{
				if (num != 2600)
				{
					if (num == 2800)
					{
						Game1.exitActiveMenu();
						Game1.player.faceDirection(2);
						Game1.player.completelyStopAnimatingOrDoingAction();
						Game1.player.animateOnce(293);
						if (Game1.player.getMount() != null)
						{
							Game1.player.getMount().dismount();
						}
					}
				}
				else
				{
					Game1.dayTimeMoneyBox.timeShakeTimer = 2000;
					Game1.farmerShouldPassOut = true;
					if (Game1.player.getMount() != null)
					{
						Game1.player.getMount().dismount();
					}
				}
			}
			else
			{
				Game1.dayTimeMoneyBox.timeShakeTimer = 2000;
				Game1.player.doEmote(24);
			}
			if (Game1.timeOfDay >= 2600)
			{
				Game1.farmerShouldPassOut = true;
			}
			foreach (GameLocation g in Game1.locations)
			{
				g.performTenMinuteUpdate(Game1.timeOfDay);
				if (g.GetType() == typeof(Farm))
				{
					((Farm)g).timeUpdate(10);
				}
			}
			if (Game1.mine != null)
			{
				Game1.mine.performTenMinuteUpdate(Game1.timeOfDay);
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00085034 File Offset: 0x00083234
		public static void UpdateGameClock(GameTime time)
		{
			if (Game1.shouldTimePass())
			{
				Game1.gameTimeInterval += (Game1.IsClient ? 0 : time.ElapsedGameTime.Milliseconds);
			}
			if (Game1.timeOfDay >= Game1.getTrulyDarkTime())
			{
				int adjustedTime = (int)((float)(Game1.timeOfDay - Game1.timeOfDay % 100) + (float)(Game1.timeOfDay % 100 / 10) * 16.66f);
				float transparency = Math.Min(0.93f, 0.75f + ((float)(adjustedTime - Game1.getTrulyDarkTime()) + (float)Game1.gameTimeInterval / 7000f * 16.6f) * 0.000625f);
				Game1.outdoorLight = (Game1.isRaining ? Game1.ambientLight : Game1.eveningColor) * transparency;
			}
			else if (Game1.timeOfDay >= Game1.getStartingToGetDarkTime())
			{
				int adjustedTime2 = (int)((float)(Game1.timeOfDay - Game1.timeOfDay % 100) + (float)(Game1.timeOfDay % 100 / 10) * 16.66f);
				float transparency2 = Math.Min(0.93f, 0.3f + ((float)(adjustedTime2 - Game1.getStartingToGetDarkTime()) + (float)Game1.gameTimeInterval / 7000f * 16.6f) * 0.00225f);
				Game1.outdoorLight = (Game1.isRaining ? Game1.ambientLight : Game1.eveningColor) * transparency2;
			}
			else if (Game1.bloom != null && Game1.timeOfDay >= Game1.getStartingToGetDarkTime() - 100 && Game1.bloom.Visible)
			{
				Game1.bloom.Settings.BloomThreshold = Math.Min(1f, Game1.bloom.Settings.BloomThreshold + 0.0004f);
			}
			else if (Game1.isRaining)
			{
				Game1.outdoorLight = Game1.ambientLight * 0.3f;
			}
			if (Game1.gameTimeInterval > 7000 + Game1.currentLocation.getExtraMillisecondsPerInGameMinuteForThisLocation())
			{
				if (Game1.panMode)
				{
					Game1.gameTimeInterval = 0;
					return;
				}
				Game1.performTenMinuteClockUpdate();
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0008520C File Offset: 0x0008340C
		public static void checkForWedding()
		{
			if (Game1.weddingToday)
			{
				Game1.player.faceDirection(2);
				Game1.currentLocation = Game1.getLocationFromName("Town");
				Game1.currentLocation.resetForPlayerEntry();
				Game1.getLocationFromName("Town").currentEvent = new Event(Utility.getWeddingEvent(), -1);
				Game1.eventUp = true;
				Game1.player.CanMove = false;
				Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
				Game1.player.position = new Vector2((float)(22 * Game1.tileSize), (float)(19 * Game1.tileSize));
				Game1.locationAfterWarp = null;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000852B0 File Offset: 0x000834B0
		public static void checkForNewLevelPerks()
		{
			Dictionary<string, string> cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			int farmerLevel = Game1.player.Level;
			foreach (string s in cookingRecipes.Keys)
			{
				string[] getConditions = cookingRecipes[s].Split(new char[]
				{
					'/'
				})[3].Split(new char[]
				{
					' '
				});
				if (getConditions[0].Equals("l") && Convert.ToInt32(getConditions[1]) <= farmerLevel && !Game1.player.cookingRecipes.ContainsKey(s))
				{
					Game1.player.cookingRecipes.Add(s, 0);
					Game1.currentObjectDialogue.Enqueue(Game1.parseText("You've learned a new cooking recipe: " + s));
					Game1.currentDialogueCharacterIndex = 1;
					Game1.dialogueUp = true;
					Game1.dialogueTyping = true;
				}
				else if (getConditions[0].Equals("s"))
				{
					int levelRequired = Convert.ToInt32(getConditions[2]);
					bool success = false;
					string a = getConditions[1];
					if (!(a == "Farming"))
					{
						if (!(a == "Fishing"))
						{
							if (!(a == "Mining"))
							{
								if (!(a == "Combat"))
								{
									if (!(a == "Foraging"))
									{
										if (a == "Luck")
										{
											if (Game1.player.LuckLevel >= levelRequired)
											{
												success = true;
											}
										}
									}
									else if (Game1.player.ForagingLevel >= levelRequired)
									{
										success = true;
									}
								}
								else if (Game1.player.CombatLevel >= levelRequired)
								{
									success = true;
								}
							}
							else if (Game1.player.MiningLevel >= levelRequired)
							{
								success = true;
							}
						}
						else if (Game1.player.FishingLevel >= levelRequired)
						{
							success = true;
						}
					}
					else if (Game1.player.FarmingLevel >= levelRequired)
					{
						success = true;
					}
					if (success && !Game1.player.cookingRecipes.ContainsKey(s))
					{
						Game1.player.cookingRecipes.Add(s, 0);
						Game1.currentObjectDialogue.Enqueue(Game1.parseText("You've learned a new cooking recipe: " + s));
						Game1.currentDialogueCharacterIndex = 1;
						Game1.dialogueUp = true;
						Game1.dialogueTyping = true;
					}
				}
			}
			Dictionary<string, string> craftingRecipes = Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
			foreach (string s2 in craftingRecipes.Keys)
			{
				string[] getConditions2 = craftingRecipes[s2].Split(new char[]
				{
					'/'
				})[4].Split(new char[]
				{
					' '
				});
				if (getConditions2[0].Equals("l") && Convert.ToInt32(getConditions2[1]) <= farmerLevel && !Game1.player.craftingRecipes.ContainsKey(s2))
				{
					Game1.player.craftingRecipes.Add(s2, 0);
					Game1.currentObjectDialogue.Enqueue(Game1.parseText("You've learned how to craft a new item: " + s2));
					Game1.currentDialogueCharacterIndex = 1;
					Game1.dialogueUp = true;
					Game1.dialogueTyping = true;
				}
				else if (getConditions2[0].Equals("s"))
				{
					int levelRequired2 = Convert.ToInt32(getConditions2[2]);
					bool success2 = false;
					string a = getConditions2[1];
					if (!(a == "Farming"))
					{
						if (!(a == "Fishing"))
						{
							if (!(a == "Mining"))
							{
								if (!(a == "Combat"))
								{
									if (!(a == "Foraging"))
									{
										if (a == "Luck")
										{
											if (Game1.player.LuckLevel >= levelRequired2)
											{
												success2 = true;
											}
										}
									}
									else if (Game1.player.ForagingLevel >= levelRequired2)
									{
										success2 = true;
									}
								}
								else if (Game1.player.CombatLevel >= levelRequired2)
								{
									success2 = true;
								}
							}
							else if (Game1.player.MiningLevel >= levelRequired2)
							{
								success2 = true;
							}
						}
						else if (Game1.player.FishingLevel >= levelRequired2)
						{
							success2 = true;
						}
					}
					else if (Game1.player.FarmingLevel >= levelRequired2)
					{
						success2 = true;
					}
					if (success2 && !Game1.player.craftingRecipes.ContainsKey(s2))
					{
						Game1.player.craftingRecipes.Add(s2, 0);
						Game1.currentObjectDialogue.Enqueue(Game1.parseText("You've learned how to craft a new item: " + s2));
						Game1.currentDialogueCharacterIndex = 1;
						Game1.dialogueUp = true;
						Game1.dialogueTyping = true;
					}
				}
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0008574C File Offset: 0x0008394C
		public static void exitActiveMenu()
		{
			Game1.activeClickableMenu = null;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00085754 File Offset: 0x00083954
		public static void receiveNewLocationInfoFromServer(GameLocation location)
		{
			if (Game1.locationAfterWarp != null)
			{
				Game1.client.sendMessage(5, new object[]
				{
					(short)Game1.xLocationAfterWarp,
					(short)Game1.yLocationAfterWarp,
					Game1.currentLocation.isStructure ? Game1.currentLocation.uniqueName : Game1.currentLocation.name,
					location.isStructure ? 1 : 0
				});
			}
			GetMapClient.receiveMapFromServer(location, location.isStructure);
			Game1.player.currentLocation = location;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000857E8 File Offset: 0x000839E8
		public static void updateScreenFade(GameTime time)
		{
			if (Game1.fadeIn)
			{
				Game1.fadeToBlackAlpha += ((Game1.eventUp || Game1.farmEvent != null) ? 0.0008f : 0.0019f) * (float)time.ElapsedGameTime.Milliseconds;
				return;
			}
			if (!Game1.menuUp && !Game1.messagePause && !Game1.dialogueUp)
			{
				Game1.fadeToBlackAlpha -= ((Game1.eventUp || Game1.farmEvent != null) ? 0.0008f : 0.0019f) * (float)time.ElapsedGameTime.Milliseconds;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0008587C File Offset: 0x00083A7C
		public static void fadeBlack()
		{
			Game1.fadeToBlack = true;
			Game1.fadeIn = true;
			Game1.fadeToBlackAlpha = 0f;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00085894 File Offset: 0x00083A94
		public static void fadeClear()
		{
			Game1.fadeIn = false;
			Game1.fadeToBlack = true;
			Game1.fadeToBlackAlpha = 1f;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000858AC File Offset: 0x00083AAC
		public static void UpdateOther(GameTime time)
		{
			if (Game1.fadeToBlack && (Game1.pauseTime == 0f || Game1.eventUp))
			{
				Game1.updateScreenFade(time);
				if (Game1.fadeToBlackAlpha > 1f && !Game1.messagePause)
				{
					if (Game1.killScreen)
					{
						Game1.viewportFreeze = true;
						Game1.viewport.X = -10000;
					}
					if (Game1.exitToTitle)
					{
						Game1.menuUp = false;
						Game1.setGameMode(4);
						Game1.menuChoice = 0;
						Game1.fadeIn = false;
						Game1.fadeToBlack = true;
						Game1.fadeToBlackAlpha = 0.01f;
						Game1.exitToTitle = false;
						Game1.changeMusicTrack("none");
						Game1.debrisWeather.Clear();
						return;
					}
					if (!Game1.nonWarpFade && Game1.locationAfterWarp != null && !Game1.menuUp)
					{
						Game1.currentLocation.cleanupBeforePlayerExit();
						Game1.displayFarmer = true;
						if (Game1.eventOver)
						{
							Game1.eventFinished();
							if (Game1.dayOfMonth == 0)
							{
								Game1.newDayAfterFade();
								Game1.player.position = new Vector2((float)(5 * Game1.tileSize), (float)(5 * Game1.tileSize));
							}
							return;
						}
						if (Game1.locationAfterWarp.Equals(Game1.currentLocation) && !Game1.eventUp && !Game1.currentLocation.Name.Equals("UndergroundMine"))
						{
							Game1.player.Position = new Vector2((float)(Game1.xLocationAfterWarp * Game1.tileSize), (float)(Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
							Game1.viewportFreeze = false;
							Game1.currentLocation.resetForPlayerEntry();
						}
						else
						{
							if (Game1.locationAfterWarp.Name.Equals("UndergroundMine"))
							{
								if (!Game1.currentLocation.Name.Equals("UndergroundMine"))
								{
									Game1.changeMusicTrack("none");
								}
								Vector2 playerMinePosition;
								if (Game1.currentLocation.Name.Equals("UndergroundMine") && Game1.mine.mineLevel < 120)
								{
									playerMinePosition = Game1.mine.enterMine(Game1.player, Game1.mine.mineLevel, Game1.player.ridingMineElevator);
									Game1.player.ridingMineElevator = false;
								}
								else
								{
									playerMinePosition = Game1.mine.enterMine(Game1.player, Game1.mine.mineLevel, Game1.player.ridingMineElevator);
									Game1.player.ridingMineElevator = false;
								}
								Game1.player.Halt();
								Game1.player.forceCanMove();
								Game1.xLocationAfterWarp = (int)playerMinePosition.X;
								Game1.yLocationAfterWarp = (int)playerMinePosition.Y;
								Game1.currentLocation = Game1.mine;
								Game1.currentLocation.resetForPlayerEntry();
								Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
								Game1.checkForRunButton(Keyboard.GetState(), false);
							}
							if (!Game1.eventUp && !Game1.menuUp)
							{
								Game1.player.Position = new Vector2((float)(Game1.xLocationAfterWarp * Game1.tileSize), (float)(Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
							}
							if (!Game1.locationAfterWarp.name.Equals("UndergroundMine") && Game1.locationAfterWarp != null)
							{
								Game1.currentLocation = Game1.locationAfterWarp;
								Game1.currentLocation.resetForPlayerEntry();
								Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
								if (!Game1.viewportFreeze && Game1.currentLocation.Map.DisplayWidth <= Game1.viewport.Width)
								{
									Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
								}
								if (!Game1.viewportFreeze && Game1.currentLocation.Map.DisplayHeight <= Game1.viewport.Height)
								{
									Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
								}
								if (Game1.player.isRidingHorse())
								{
									if (Game1.player.position.X / (float)Game1.tileSize >= (float)(Game1.currentLocation.map.Layers[0].LayerWidth - 1))
									{
										Farmer expr_434_cp_0_cp_0 = Game1.player;
										expr_434_cp_0_cp_0.position.X = expr_434_cp_0_cp_0.position.X - (float)Game1.tileSize;
									}
									else if (Game1.player.position.Y / (float)Game1.tileSize >= (float)(Game1.currentLocation.map.Layers[0].LayerHeight - 1))
									{
										Farmer expr_484_cp_0_cp_0 = Game1.player;
										expr_484_cp_0_cp_0.position.Y = expr_484_cp_0_cp_0.position.Y - (float)(Game1.tileSize / 2);
									}
									if (Game1.player.position.Y / (float)Game1.tileSize >= (float)(Game1.currentLocation.map.Layers[0].LayerHeight - 2))
									{
										Farmer expr_4D4_cp_0_cp_0 = Game1.player;
										expr_4D4_cp_0_cp_0.position.X = expr_4D4_cp_0_cp_0.position.X - (float)(Game1.tileSize * 3 / 4);
									}
									Game1.warpCharacter(Game1.player.getMount(), Game1.currentLocation.Name, new Point(Game1.xLocationAfterWarp, Game1.yLocationAfterWarp), false, true);
									Game1.player.Halt();
								}
								Game1.checkForRunButton(Keyboard.GetState(), true);
							}
							if (!Game1.eventUp)
							{
								Game1.viewportFreeze = false;
							}
						}
						Game1.player.faceDirection(Game1.facingDirectionAfterWarp);
						if (Game1.player.ActiveObject != null)
						{
							Game1.player.showCarrying();
						}
						else
						{
							Game1.player.showNotCarrying();
						}
						if (Game1.IsClient)
						{
							Game1.receiveNewLocationInfoFromServer(Game1.locationAfterWarp);
						}
						else if (Game1.IsServer)
						{
							Game1.player.currentLocation = Game1.currentLocation;
							MultiplayerUtility.broadcastFarmerWarp((short)Game1.xLocationAfterWarp, (short)Game1.yLocationAfterWarp, Game1.currentLocation.isStructure ? Game1.currentLocation.uniqueName : Game1.currentLocation.name, Game1.currentLocation.isStructure, Game1.player.uniqueMultiplayerID);
						}
						else
						{
							Game1.player.currentLocation = Game1.currentLocation;
						}
					}
					if (Game1.newDay)
					{
						Game1.newDayAfterFade();
						Game1.checkForWedding();
						if (Game1.eventOver)
						{
							Game1.eventFinished();
							if (Game1.dayOfMonth == 0)
							{
								Game1.newDayAfterFade();
								Game1.player.position = new Vector2((float)(5 * Game1.tileSize), (float)(5 * Game1.tileSize));
							}
						}
					}
					else if (Game1.eventOver)
					{
						Game1.eventFinished();
						if (Game1.dayOfMonth == 0)
						{
							Game1.newDayAfterFade();
							Game1.currentLocation.resetForPlayerEntry();
						}
					}
					Game1.nonWarpFade = false;
					Game1.fadeIn = false;
					if (Game1.boardingBus)
					{
						Game1.boardingBus = false;
						Game1.drawObjectDialogue("Next stop: " + (Game1.currentLocation.Name.Equals("Desert") ? "Calico Desert." : "Stardew Valley."));
						Game1.messagePause = true;
						Game1.viewportFreeze = false;
					}
					if (Game1.isRaining && Game1.currentSong != null && Game1.currentSong != null && Game1.currentSong.Name.Equals("rain"))
					{
						if (Game1.currentLocation.IsOutdoors)
						{
							Game1.currentSong.SetVariable("Frequency", 100f);
						}
						else if (!Game1.currentLocation.Name.Equals("UndergroundMine"))
						{
							Game1.currentSong.SetVariable("Frequency", 15f);
						}
					}
				}
				if (Game1.fadeToBlackAlpha < 0f)
				{
					Game1.fadeToBlack = false;
					if (Game1.killScreen)
					{
						Game1.pauseThenMessage(1500, "..." + Game1.player.Name + "?", false);
					}
					else if (!Game1.eventUp)
					{
						Game1.player.CanMove = true;
					}
					Game1.checkForRunButton(Game1.oldKBState, true);
				}
			}
			if (Game1.dialogueUp || Game1.currentBillboard != 0)
			{
				Game1.player.CanMove = false;
			}
			else
			{
				Game1.player.FarmerSprite.freezeUntilDialogueIsOver = false;
			}
			for (int i = Game1.delayedActions.Count - 1; i >= 0; i--)
			{
				if (Game1.delayedActions[i].update(time))
				{
					Game1.delayedActions.RemoveAt(i);
				}
			}
			if (Game1.farmerShouldPassOut && Game1.player.canMove && Game1.player.freezePause <= 0 && !Game1.player.UsingTool)
			{
				Game1.exitActiveMenu();
				Game1.player.faceDirection(2);
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.player.animateOnce(293);
				Game1.farmerShouldPassOut = false;
				Game1.player.freezePause = 7000;
			}
			for (int j = Game1.screenOverlayTempSprites.Count - 1; j >= 0; j--)
			{
				if (Game1.screenOverlayTempSprites[j].update(time))
				{
					Game1.screenOverlayTempSprites.RemoveAt(j);
				}
			}
			if (Game1.pickingTool)
			{
				Game1.pickToolInterval += (float)time.ElapsedGameTime.Milliseconds;
				if (Game1.pickToolInterval > 500f)
				{
					Game1.pickingTool = false;
					Game1.pickToolInterval = 0f;
					if (!Game1.eventUp)
					{
						Game1.player.CanMove = true;
					}
					Game1.player.UsingTool = false;
					switch (Game1.player.FacingDirection)
					{
					case 0:
						Game1.player.Sprite.CurrentFrame = 16;
						break;
					case 1:
						Game1.player.Sprite.CurrentFrame = 8;
						break;
					case 2:
						Game1.player.Sprite.CurrentFrame = 0;
						break;
					case 3:
						Game1.player.Sprite.CurrentFrame = 24;
						break;
					}
					if (!Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
					{
						Game1.player.setRunning(Game1.options.autoRun, false);
					}
				}
				else if (Game1.pickToolInterval > 83.3333359f)
				{
					switch (Game1.player.FacingDirection)
					{
					case 0:
						Game1.player.FarmerSprite.setCurrentFrame(196);
						break;
					case 1:
						Game1.player.FarmerSprite.setCurrentFrame(194);
						break;
					case 2:
						Game1.player.FarmerSprite.setCurrentFrame(192);
						break;
					case 3:
						Game1.player.FarmerSprite.setCurrentFrame(198);
						break;
					}
				}
			}
			if ((Game1.player.CanMove || Game1.player.UsingTool) && Game1.shouldTimePass())
			{
				Game1.buffsDisplay.update(time);
			}
			if (Game1.player.CurrentItem != null)
			{
				Game1.player.CurrentItem.actionWhenBeingHeld(Game1.player);
			}
			float tmp = Game1.dialogueButtonScale;
			Game1.dialogueButtonScale = (float)((double)(Game1.tileSize / 4) * Math.Sin(time.TotalGameTime.TotalMilliseconds % 1570.0 / 500.0));
			if (tmp > Game1.dialogueButtonScale && !Game1.dialogueButtonShrinking)
			{
				Game1.dialogueButtonShrinking = true;
			}
			else if (tmp < Game1.dialogueButtonScale && Game1.dialogueButtonShrinking)
			{
				Game1.dialogueButtonShrinking = false;
			}
			if (Game1.player.currentUpgrade != null && Game1.currentLocation.Name.Equals("Farm") && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3)
			{
				Game1.player.currentUpgrade.update((float)time.ElapsedGameTime.Milliseconds);
			}
			if (Game1.screenGlow)
			{
				if (Game1.screenGlowUp || Game1.screenGlowHold)
				{
					if (Game1.screenGlowHold)
					{
						Game1.screenGlowAlpha = Math.Min(Game1.screenGlowAlpha + Game1.screenGlowRate, Game1.screenGlowMax);
					}
					else
					{
						Game1.screenGlowAlpha = Math.Min(Game1.screenGlowAlpha + 0.03f, 0.6f);
						if (Game1.screenGlowAlpha >= 0.6f)
						{
							Game1.screenGlowUp = false;
						}
					}
				}
				else
				{
					Game1.screenGlowAlpha -= 0.01f;
					if (Game1.screenGlowAlpha <= 0f)
					{
						Game1.screenGlow = false;
					}
				}
			}
			for (int k = Game1.hudMessages.Count - 1; k >= 0; k--)
			{
				if (Game1.hudMessages.ElementAt(k).update(time))
				{
					Game1.hudMessages.RemoveAt(k);
				}
			}
			if (Game1.isRaining && Game1.currentLocation.IsOutdoors)
			{
				for (int l = 0; l < Game1.rainDrops.Length; l++)
				{
					if (Game1.rainDrops[l].frame == 0)
					{
						RainDrop[] expr_C0F_cp_0_cp_0 = Game1.rainDrops;
						int expr_C0F_cp_0_cp_1 = l;
						expr_C0F_cp_0_cp_0[expr_C0F_cp_0_cp_1].accumulator = expr_C0F_cp_0_cp_0[expr_C0F_cp_0_cp_1].accumulator + time.ElapsedGameTime.Milliseconds;
						if (Game1.rainDrops[l].accumulator >= 70)
						{
							RainDrop[] expr_C4B_cp_0_cp_0 = Game1.rainDrops;
							int expr_C4B_cp_0_cp_1 = l;
							expr_C4B_cp_0_cp_0[expr_C4B_cp_0_cp_1].position = expr_C4B_cp_0_cp_0[expr_C4B_cp_0_cp_1].position + new Vector2((float)(-(float)Game1.tileSize / 4 + l * 8 / Game1.rainDrops.Length), (float)(Game1.tileSize / 2 - l * 8 / Game1.rainDrops.Length));
							Game1.rainDrops[l].accumulator = 0;
							if (Game1.random.NextDouble() < 0.1)
							{
								RainDrop[] expr_CC3_cp_0_cp_0 = Game1.rainDrops;
								int expr_CC3_cp_0_cp_1 = l;
								expr_CC3_cp_0_cp_0[expr_CC3_cp_0_cp_1].frame = expr_CC3_cp_0_cp_0[expr_CC3_cp_0_cp_1].frame + 1;
							}
							if (Game1.rainDrops[l].position.Y > (float)(Game1.viewport.Height + Game1.tileSize))
							{
								Game1.rainDrops[l].position.Y = (float)(-(float)Game1.tileSize);
							}
						}
					}
					else
					{
						RainDrop[] expr_D27_cp_0_cp_0 = Game1.rainDrops;
						int expr_D27_cp_0_cp_1 = l;
						expr_D27_cp_0_cp_0[expr_D27_cp_0_cp_1].accumulator = expr_D27_cp_0_cp_0[expr_D27_cp_0_cp_1].accumulator + time.ElapsedGameTime.Milliseconds;
						if (Game1.rainDrops[l].accumulator > 70)
						{
							Game1.rainDrops[l].frame = (Game1.rainDrops[l].frame + 1) % 4;
							Game1.rainDrops[l].accumulator = 0;
							if (Game1.rainDrops[l].frame == 0)
							{
								Game1.rainDrops[l].position = new Vector2((float)Game1.random.Next(Game1.viewport.Width), (float)Game1.random.Next(Game1.viewport.Height));
							}
						}
					}
				}
			}
			else if (Game1.isDebrisWeather && Game1.currentLocation.IsOutdoors && !Game1.currentLocation.ignoreDebrisWeather)
			{
				if (Game1.currentSeason.Equals("fall") && Game1.random.NextDouble() < 0.001 && Game1.windGust == 0f && WeatherDebris.globalWind >= -0.5f)
				{
					Game1.windGust += (float)Game1.random.Next(-10, -1) / 100f;
					if (Game1.soundBank != null)
					{
						Game1.wind = Game1.soundBank.GetCue("wind");
						Game1.wind.Play();
					}
				}
				else if (Game1.windGust != 0f)
				{
					Game1.windGust = Math.Max(-5f, Game1.windGust * 1.02f);
					WeatherDebris.globalWind = -0.5f + Game1.windGust;
					if (Game1.windGust < -0.2f && Game1.random.NextDouble() < 0.007)
					{
						Game1.windGust = 0f;
					}
				}
				using (List<WeatherDebris>.Enumerator enumerator = Game1.debrisWeather.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.update();
					}
				}
			}
			if (WeatherDebris.globalWind < -0.5f && Game1.wind != null)
			{
				WeatherDebris.globalWind = Math.Min(-0.5f, WeatherDebris.globalWind + 0.015f);
				Game1.wind.SetVariable("Volume", -WeatherDebris.globalWind * 20f);
				Game1.wind.SetVariable("Frequency", -WeatherDebris.globalWind * 20f);
				if (WeatherDebris.globalWind == -0.5f)
				{
					Game1.wind.Stop(AudioStopOptions.AsAuthored);
				}
			}
			if (!Game1.fadeToBlack)
			{
				Game1.currentLocation.checkForMusic(time);
			}
			if (Game1.debrisSoundInterval > 0f)
			{
				Game1.debrisSoundInterval -= (float)time.ElapsedGameTime.Milliseconds;
			}
			Game1.noteBlockTimer += (float)time.ElapsedGameTime.Milliseconds;
			if (Game1.noteBlockTimer > 1000f)
			{
				Game1.noteBlockTimer = 0f;
				if (Game1.player.health < 20 && Game1.CurrentEvent == null)
				{
					Game1.hitShakeTimer = 250;
					if (Game1.player.health <= 10)
					{
						Game1.hitShakeTimer = 500;
						for (int m = 0; m < 3; m++)
						{
							Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(366, 412, 5, 6), new Vector2((float)(Game1.random.Next(Game1.tileSize / 2) + Game1.viewport.Width - (48 + Game1.tileSize / 8) * 2), (float)(Game1.viewport.Height - 224 - (Game1.player.maxHealth - 100) - Game1.tileSize / 4 + 4)), false, 0.017f, Color.Red)
							{
								motion = new Vector2(-1.5f, (float)(-8 + Game1.random.Next(-1, 2))),
								acceleration = new Vector2(0f, 0.5f),
								local = true,
								scale = (float)Game1.pixelZoom,
								delayBeforeAnimationStart = m * 150
							});
						}
					}
				}
			}
			if (Game1.showKeyHelp && !Game1.eventUp)
			{
				Game1.keyHelpString = "";
				if (Game1.dialogueUp)
				{
					Game1.keyHelpString += "X or Left Click - Advance Dialogue";
				}
				else if (Game1.menuUp)
				{
					Game1.keyHelpString += "X or Left Click - Select";
					Game1.keyHelpString += "C or Right Click - Cancel";
				}
				else if (Game1.player.ActiveObject != null)
				{
					Game1.keyHelpString += "X or Left Click - Drop Item";
					Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "C or Right Click - Use Item";
					if (Game1.player.numberOfItemsInInventory() < Game1.player.maxItems)
					{
						Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "E, F or Mouse Mid. - Store Item";
					}
					if (Game1.player.numberOfItemsInInventory() > 0)
					{
						Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "Z or Mouse Wheel - Switch Item";
					}
					Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "Q or Enter - Open Menu";
					Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "Shift - Run/Walk";
				}
				else
				{
					Game1.keyHelpString += "X or Left Click - Grab | Use";
					if (Game1.player.CurrentTool != null)
					{
						Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "C or Right Click - Use " + Game1.player.CurrentTool.Name;
					}
					if (Game1.player.numberOfItemsInInventory() > 0)
					{
						Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "E, F, or Mouse Mid. - Remove Item From Backpack";
					}
					Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "Q or Enter - Open Menu";
					Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + "Shift - Run/Walk";
				}
			}
			Game1.drawLighting = ((Game1.currentLocation.IsOutdoors && !Game1.outdoorLight.Equals(Color.White)) || !Game1.ambientLight.Equals(Color.White) || (Game1.currentLocation is MineShaft && !((MineShaft)Game1.currentLocation).getLightingColor(time).Equals(Color.White)));
			if (Game1.hitShakeTimer > 0)
			{
				Game1.hitShakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (Game1.staminaShakeTimer > 0)
			{
				Game1.staminaShakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (Game1.background != null)
			{
				Game1.background.update(Game1.viewport);
			}
			Game1.cursorTileHintCheckTimer -= (int)time.ElapsedGameTime.TotalMilliseconds;
			Game1.currentCursorTile.X = (float)((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize);
			Game1.currentCursorTile.Y = (float)((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize);
			if (Game1.cursorTileHintCheckTimer <= 0 || !Game1.currentCursorTile.Equals(Game1.lastCursorTile))
			{
				Game1.cursorTileHintCheckTimer = 250;
				Game1.updateCursorTileHint();
				if (Game1.player.CanMove)
				{
					Game1.checkForRunButton(Game1.oldKBState, true);
				}
			}
			if (Game1.options.gamepadControls)
			{
				Rumble.update((float)time.ElapsedGameTime.Milliseconds);
				if (Game1.thumbstickMotionMargin > 0)
				{
					Game1.thumbstickMotionMargin -= time.ElapsedGameTime.Milliseconds;
				}
			}
			if (Game1.activeClickableMenu == null && Game1.farmEvent == null && Game1.keyboardDispatcher != null)
			{
				Game1.keyboardDispatcher.Subscriber = null;
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00086DBC File Offset: 0x00084FBC
		public static void updateCursorTileHint()
		{
			if (Game1.activeClickableMenu == null)
			{
				Game1.mouseCursorTransparency = 1f;
				Game1.isActionAtCurrentCursorTile = false;
				Game1.isInspectionAtCurrentCursorTile = false;
				int xTile = (Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize;
				int yTile = (Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize;
				if (Game1.currentLocation != null)
				{
					Game1.isActionAtCurrentCursorTile = Game1.currentLocation.isActionableTile(xTile, yTile, Game1.player);
					if (!Game1.isActionAtCurrentCursorTile)
					{
						Game1.isActionAtCurrentCursorTile = Game1.currentLocation.isActionableTile(xTile, yTile + 1, Game1.player);
					}
				}
				Game1.lastCursorTile = Game1.currentCursorTile;
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00086E60 File Offset: 0x00085060
		public static void updateMusic()
		{
			if (Game1.soundBank == null)
			{
				return;
			}
			if (!Game1.nextMusicTrack.Equals(""))
			{
				Game1.musicPlayerVolume = Math.Max(0f, Game1.musicPlayerVolume - 0.01f);
				Game1.ambientPlayerVolume = Math.Max(0f, Game1.ambientPlayerVolume - 0.01f);
				Game1.musicCategory.SetVolume(Game1.musicPlayerVolume);
				Game1.ambientCategory.SetVolume(Game1.ambientPlayerVolume);
				if (Game1.musicPlayerVolume == 0f && Game1.ambientPlayerVolume == 0f && Game1.currentSong != null)
				{
					if (Game1.nextMusicTrack.Equals("none"))
					{
						Game1.jukeboxPlaying = false;
						Game1.currentSong.Stop(AudioStopOptions.Immediate);
					}
					else if ((Game1.options.musicVolumeLevel != 0f || Game1.options.ambientVolumeLevel != 0f) && (!Game1.nextMusicTrack.Equals("rain") || Game1.endOfNightMenus.Count == 0))
					{
						Game1.currentSong.Stop(AudioStopOptions.Immediate);
						Game1.currentSong.Dispose();
						Game1.currentSong = Game1.soundBank.GetCue(Game1.nextMusicTrack);
						Game1.currentSong.Play();
						if (Game1.isRaining && Game1.currentSong != null && Game1.currentSong.Name.Equals("rain"))
						{
							if (Game1.currentLocation.IsOutdoors)
							{
								Game1.currentSong.SetVariable("Frequency", 100f);
							}
							else if (!Game1.currentLocation.Name.Equals("UndergroundMine"))
							{
								Game1.currentSong.SetVariable("Frequency", 15f);
							}
						}
					}
					else
					{
						Game1.currentSong.Stop(AudioStopOptions.Immediate);
					}
					Game1.nextMusicTrack = "";
					return;
				}
			}
			else if (Game1.musicPlayerVolume < Game1.options.musicVolumeLevel || Game1.ambientPlayerVolume < Game1.options.ambientVolumeLevel)
			{
				if (Game1.musicPlayerVolume < Game1.options.musicVolumeLevel)
				{
					Game1.musicPlayerVolume = Math.Min(1f, Game1.musicPlayerVolume += 0.01f);
					Game1.musicCategory.SetVolume(Game1.options.musicVolumeLevel);
				}
				if (Game1.ambientPlayerVolume < Game1.options.ambientVolumeLevel)
				{
					Game1.ambientPlayerVolume = Math.Min(1f, Game1.ambientPlayerVolume += 0.015f);
					Game1.ambientCategory.SetVolume(Game1.ambientPlayerVolume);
					return;
				}
			}
			else if (Game1.currentSong != null && !Game1.currentSong.IsPlaying && !Game1.currentSong.IsStopped)
			{
				Game1.currentSong = Game1.soundBank.GetCue(Game1.currentSong.Name);
				Game1.currentSong.Play();
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00087117 File Offset: 0x00085317
		public static void updateRainDropPositionForPlayerMovement(int direction)
		{
			Game1.updateRainDropPositionForPlayerMovement(direction, false);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00087120 File Offset: 0x00085320
		public static void updateRainDropPositionForPlayerMovement(int direction, bool overrideConstraints)
		{
			Game1.updateRainDropPositionForPlayerMovement(direction, overrideConstraints, (float)Game1.player.speed);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00087134 File Offset: 0x00085334
		public static void updateRainDropPositionForPlayerMovement(int direction, bool overrideConstraints, float speed)
		{
			if (overrideConstraints || ((Game1.isRaining || Game1.isDebrisWeather) && Game1.currentLocation.IsOutdoors && (direction == 0 || direction == 2 || (Game1.player.getStandingX() >= Game1.viewport.Width / 2 && Game1.player.getStandingX() <= Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width / 2)) && (direction == 1 || direction == 3 || (Game1.player.getStandingY() >= Game1.viewport.Height / 2 && Game1.player.getStandingY() <= Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height / 2))))
			{
				if (Game1.isRaining)
				{
					for (int i = 0; i < Game1.rainDrops.Length; i++)
					{
						if (direction == 0)
						{
							RainDrop[] expr_EA_cp_0_cp_0_cp_0 = Game1.rainDrops;
							int expr_EA_cp_0_cp_0_cp_1 = i;
							expr_EA_cp_0_cp_0_cp_0[expr_EA_cp_0_cp_0_cp_1].position.Y = expr_EA_cp_0_cp_0_cp_0[expr_EA_cp_0_cp_0_cp_1].position.Y + speed;
							if (Game1.rainDrops[i].position.Y > (float)(Game1.viewport.Height + Game1.tileSize))
							{
								Game1.rainDrops[i].position.Y = (float)(-(float)Game1.tileSize);
							}
						}
						else if (direction == 1)
						{
							RainDrop[] expr_154_cp_0_cp_0_cp_0 = Game1.rainDrops;
							int expr_154_cp_0_cp_0_cp_1 = i;
							expr_154_cp_0_cp_0_cp_0[expr_154_cp_0_cp_0_cp_1].position.X = expr_154_cp_0_cp_0_cp_0[expr_154_cp_0_cp_0_cp_1].position.X - speed;
							if (Game1.rainDrops[i].position.X < (float)(-(float)Game1.tileSize))
							{
								Game1.rainDrops[i].position.X = (float)Game1.viewport.Width;
							}
						}
						else if (direction == 2)
						{
							RainDrop[] expr_1B8_cp_0_cp_0_cp_0 = Game1.rainDrops;
							int expr_1B8_cp_0_cp_0_cp_1 = i;
							expr_1B8_cp_0_cp_0_cp_0[expr_1B8_cp_0_cp_0_cp_1].position.Y = expr_1B8_cp_0_cp_0_cp_0[expr_1B8_cp_0_cp_0_cp_1].position.Y - speed;
							if (Game1.rainDrops[i].position.Y < (float)(-(float)Game1.tileSize))
							{
								Game1.rainDrops[i].position.Y = (float)Game1.viewport.Height;
							}
						}
						else if (direction == 3)
						{
							RainDrop[] expr_219_cp_0_cp_0_cp_0 = Game1.rainDrops;
							int expr_219_cp_0_cp_0_cp_1 = i;
							expr_219_cp_0_cp_0_cp_0[expr_219_cp_0_cp_0_cp_1].position.X = expr_219_cp_0_cp_0_cp_0[expr_219_cp_0_cp_0_cp_1].position.X + speed;
							if (Game1.rainDrops[i].position.X > (float)(Game1.viewport.Width + Game1.tileSize))
							{
								Game1.rainDrops[i].position.X = (float)(-(float)Game1.tileSize);
							}
						}
					}
					return;
				}
				Game1.updateDebrisWeatherForMovement(Game1.debrisWeather, direction, overrideConstraints, speed);
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000873C4 File Offset: 0x000855C4
		public static void updateDebrisWeatherForMovement(List<WeatherDebris> debris, int direction, bool overrideConstraints, float speed)
		{
			if (Game1.fadeToBlackAlpha <= 0f && debris != null)
			{
				foreach (WeatherDebris w in debris)
				{
					if (direction == 0)
					{
						WeatherDebris expr_37_cp_0_cp_0 = w;
						expr_37_cp_0_cp_0.position.Y = expr_37_cp_0_cp_0.position.Y + speed;
						if (w.position.Y > (float)(Game1.viewport.Height + Game1.tileSize))
						{
							w.position.Y = (float)(-(float)Game1.tileSize);
						}
					}
					else if (direction == 1)
					{
						WeatherDebris expr_83_cp_0_cp_0 = w;
						expr_83_cp_0_cp_0.position.X = expr_83_cp_0_cp_0.position.X - speed;
						if (w.position.X < (float)(-(float)Game1.tileSize))
						{
							w.position.X = (float)Game1.viewport.Width;
						}
					}
					else if (direction == 2)
					{
						WeatherDebris expr_C9_cp_0_cp_0 = w;
						expr_C9_cp_0_cp_0.position.Y = expr_C9_cp_0_cp_0.position.Y - speed;
						if (w.position.Y < (float)(-(float)Game1.tileSize))
						{
							w.position.Y = (float)Game1.viewport.Height;
						}
					}
					else if (direction == 3)
					{
						WeatherDebris expr_109_cp_0_cp_0 = w;
						expr_109_cp_0_cp_0.position.X = expr_109_cp_0_cp_0.position.X + speed;
						if (w.position.X > (float)(Game1.viewport.Width + Game1.tileSize))
						{
							w.position.X = (float)(-(float)Game1.tileSize);
						}
					}
				}
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00087548 File Offset: 0x00085748
		public static Vector2 updateFloatingObjectPositionForMovement(Vector2 w, Vector2 current, Vector2 previous, float speed)
		{
			if (current.Y < previous.Y)
			{
				w.Y -= Math.Abs(current.Y - previous.Y) * speed;
			}
			else if (current.Y > previous.Y)
			{
				w.Y += Math.Abs(current.Y - previous.Y) * speed;
			}
			if (current.X > previous.X)
			{
				w.X += Math.Abs(current.X - previous.X) * speed;
			}
			else if (current.X < previous.X)
			{
				w.X -= Math.Abs(current.X - previous.X) * speed;
			}
			return w;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00087610 File Offset: 0x00085810
		public static void updateRaindropPosition()
		{
			if (Game1.isRaining)
			{
				int xOffset = Game1.viewport.X - (int)Game1.previousViewportPosition.X;
				int yOffset = Game1.viewport.Y - (int)Game1.previousViewportPosition.Y;
				for (int i = 0; i < Game1.rainDrops.Length; i++)
				{
					RainDrop[] expr_54_cp_0_cp_0_cp_0 = Game1.rainDrops;
					int expr_54_cp_0_cp_0_cp_1 = i;
					expr_54_cp_0_cp_0_cp_0[expr_54_cp_0_cp_0_cp_1].position.X = expr_54_cp_0_cp_0_cp_0[expr_54_cp_0_cp_0_cp_1].position.X - (float)xOffset * 1f;
					RainDrop[] expr_75_cp_0_cp_0_cp_0 = Game1.rainDrops;
					int expr_75_cp_0_cp_0_cp_1 = i;
					expr_75_cp_0_cp_0_cp_0[expr_75_cp_0_cp_0_cp_1].position.Y = expr_75_cp_0_cp_0_cp_0[expr_75_cp_0_cp_0_cp_1].position.Y - (float)yOffset * 1f;
					if (Game1.rainDrops[i].position.Y > (float)(Game1.viewport.Height + Game1.tileSize))
					{
						Game1.rainDrops[i].position.Y = (float)(-(float)Game1.tileSize);
					}
					else if (Game1.rainDrops[i].position.X < (float)(-(float)Game1.tileSize))
					{
						Game1.rainDrops[i].position.X = (float)Game1.viewport.Width;
					}
					else if (Game1.rainDrops[i].position.Y < (float)(-(float)Game1.tileSize))
					{
						Game1.rainDrops[i].position.Y = (float)Game1.viewport.Height;
					}
					else if (Game1.rainDrops[i].position.X > (float)(Game1.viewport.Width + Game1.tileSize))
					{
						Game1.rainDrops[i].position.X = (float)(-(float)Game1.tileSize);
					}
				}
				return;
			}
			Game1.updateDebrisWeatherForMovement(Game1.debrisWeather);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000877CC File Offset: 0x000859CC
		public static void updateDebrisWeatherForMovement(List<WeatherDebris> debris)
		{
			if (!Game1.fadeToBlack && Game1.fadeToBlackAlpha <= 0f && debris != null)
			{
				int xOffset = Game1.viewport.X - (int)Game1.previousViewportPosition.X;
				int yOffset = Game1.viewport.Y - (int)Game1.previousViewportPosition.Y;
				foreach (WeatherDebris w in debris)
				{
					WeatherDebris expr_6C_cp_0_cp_0 = w;
					expr_6C_cp_0_cp_0.position.X = expr_6C_cp_0_cp_0.position.X - (float)xOffset * 1f;
					WeatherDebris expr_83_cp_0_cp_0 = w;
					expr_83_cp_0_cp_0.position.Y = expr_83_cp_0_cp_0.position.Y - (float)yOffset * 1f;
					if (w.position.Y > (float)(Game1.viewport.Height + Game1.tileSize))
					{
						w.position.Y = (float)(-(float)Game1.tileSize);
					}
					else if (w.position.X < (float)(-(float)Game1.tileSize))
					{
						w.position.X = (float)Game1.viewport.Width;
					}
					else if (w.position.Y < (float)(-(float)Game1.tileSize))
					{
						w.position.Y = (float)Game1.viewport.Height;
					}
					else if (w.position.X > (float)(Game1.viewport.Width + Game1.tileSize))
					{
						w.position.X = (float)(-(float)Game1.tileSize);
					}
				}
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00087960 File Offset: 0x00085B60
		public static void randomizeDebrisWeatherPositions(List<WeatherDebris> debris)
		{
			if (debris != null)
			{
				using (List<WeatherDebris>.Enumerator enumerator = debris.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.position = Utility.getRandomPositionOnScreen();
					}
				}
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000879B4 File Offset: 0x00085BB4
		public static void eventFinished()
		{
			Game1.player.canOnlyWalk = false;
			bool wasFestival = Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.isFestival;
			Game1.eventOver = false;
			Game1.eventUp = false;
			Game1.player.CanMove = true;
			Game1.displayHUD = true;
			Game1.player.position = new Vector2(Game1.player.positionBeforeEvent.X * (float)Game1.tileSize, Game1.player.positionBeforeEvent.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2));
			if (Game1.locationAfterWarp == null || Game1.locationAfterWarp.Equals(Game1.currentLocation))
			{
				Game1.xLocationAfterWarp = (int)Game1.player.positionBeforeEvent.X;
				Game1.yLocationAfterWarp = (int)Game1.player.positionBeforeEvent.Y;
			}
			Game1.player.faceDirection(Game1.player.orientationBeforeEvent);
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.viewportFreeze = false;
			if (Game1.currentLocation.currentEvent != null)
			{
				Game1.currentLocation.currentEvent.cleanup();
				Game1.currentLocation.currentEvent = null;
			}
			if (Game1.player.ActiveObject != null)
			{
				Game1.player.showCarrying();
			}
			if (Game1.isRaining && (Game1.currentSong == null || !Game1.currentSong.Name.Equals("rain")) && !Game1.currentLocation.Name.Equals("UndergroundMine"))
			{
				Game1.changeMusicTrack("rain");
			}
			else if (!Game1.isRaining && (Game1.currentSong == null || Game1.currentSong.Name == null || !Game1.currentSong.Name.Contains(Game1.currentSeason)))
			{
				Game1.changeMusicTrack("none");
			}
			if (Game1.dayOfMonth != 0)
			{
				Game1.currentLightSources.Clear();
				Game1.currentLocation.resetForPlayerEntry();
			}
			if (Game1.locationAfterWarp != null)
			{
				if (wasFestival)
				{
					foreach (Farmer f in Game1.otherFarmers.Values)
					{
						if (!f.IsMainPlayer)
						{
							f.Position = new Vector2((float)(Game1.xLocationAfterWarp * Game1.tileSize), (float)(Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
							f.currentLocation.farmers.Remove(f);
							f.currentLocation = Game1.locationAfterWarp;
							f.currentLocation.farmers.Add(f);
						}
					}
				}
				Game1.player.Position = new Vector2((float)(Game1.xLocationAfterWarp * Game1.tileSize), (float)(Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
				Game1.currentLocation.cleanupBeforePlayerExit();
				Game1.currentLocation = Game1.locationAfterWarp;
				Game1.currentLocation.resetForPlayerEntry();
				Game1.player.currentLocation = Game1.currentLocation;
				Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
				if (Game1.currentLocation.Map.DisplayWidth <= Game1.viewport.Width)
				{
					Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
				}
				if (Game1.currentLocation.Map.DisplayHeight <= Game1.viewport.Height)
				{
					Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
				}
				Game1.currentLocation.currentEvent = null;
				Game1.eventUp = false;
				Game1.displayHUD = true;
				if (Game1.timeOfDay >= 2000)
				{
					Game1.currentLocation.switchOutNightTiles();
					Game1.player.canMove = true;
					Game1.UpdateGameClock(Game1.currentGameTime);
				}
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00087DA8 File Offset: 0x00085FA8
		public static void populateDebrisWeatherArray()
		{
			Game1.debrisWeather.Clear();
			Game1.isDebrisWeather = true;
			int debrisToMake = Game1.random.Next(16, 64);
			int baseIndex = Game1.currentSeason.Equals("spring") ? 0 : (Game1.currentSeason.Equals("winter") ? 3 : 2);
			for (int i = 0; i < debrisToMake; i++)
			{
				Game1.debrisWeather.Add(new WeatherDebris(new Vector2((float)Game1.random.Next(0, Game1.viewport.Width), (float)Game1.random.Next(0, Game1.viewport.Height)), baseIndex, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00087E88 File Offset: 0x00086088
		private static void newSeason()
		{
			string a = Game1.currentSeason;
			if (!(a == "spring"))
			{
				if (!(a == "summer"))
				{
					if (!(a == "fall"))
					{
						if (a == "winter")
						{
							Game1.currentSeason = "spring";
						}
					}
					else
					{
						Game1.currentSeason = "winter";
					}
				}
				else
				{
					Game1.currentSeason = "fall";
				}
			}
			else
			{
				Game1.currentSeason = "summer";
			}
			Game1.setGraphicsForSeason();
			Game1.dayOfMonth = 1;
			using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.seasonUpdate(Game1.currentSeason, false);
				}
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00087F54 File Offset: 0x00086154
		public static void playItemNumberSelectSound()
		{
			if (Game1.selectedItemsType.Equals("flutePitch"))
			{
				if (Game1.soundBank != null)
				{
					Cue expr_27 = Game1.soundBank.GetCue("flute");
					expr_27.SetVariable("Pitch", (float)(100 * Game1.numberOfSelectedItems));
					expr_27.Play();
					return;
				}
			}
			else
			{
				if (Game1.selectedItemsType.Equals("drumTone"))
				{
					Game1.playSound("drumkit" + Game1.numberOfSelectedItems);
					return;
				}
				Game1.playSound("toolSwap");
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00087FD8 File Offset: 0x000861D8
		public static void slotsDone()
		{
			Response[] playAgainOptions = new Response[]
			{
				new Response("Play", "Play Again"),
				new Response("Leave", "Leave")
			};
			if (Game1.slotResult[3] == 'x')
			{
				Game1.currentLocation.createQuestionDialogue("You have " + Game1.player.clubCoins + " club coins.", playAgainOptions, "PlaySlots" + Game1.currentLocation.map.GetLayer("Buildings").PickTile(new Location((int)(Game1.player.GetGrabTile().X * (float)Game1.tileSize), (int)(Game1.player.GetGrabTile().Y * (float)Game1.tileSize)), Game1.viewport.Size).Properties["Action"].ToString().Split(new char[]
				{
					' '
				})[1]);
				Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
				return;
			}
			Game1.playSound("money");
			string specialMessage = Game1.slotResult.Substring(0, 3).Equals("===") ? "JACKPOT WINNER!!" : "";
			Game1.player.clubCoins += Convert.ToInt32(Game1.slotResult.Substring(3));
			Game1.currentLocation.createQuestionDialogue(Game1.parseText(specialMessage + " You win " + Game1.slotResult.Substring(3) + " Club Coins."), playAgainOptions, "PlaySlots" + Game1.currentLocation.map.GetLayer("Buildings").PickTile(new Location((int)(Game1.player.GetGrabTile().X * (float)Game1.tileSize), (int)(Game1.player.GetGrabTile().Y * (float)Game1.tileSize)), Game1.viewport.Size).Properties["Action"].ToString().Split(new char[]
			{
				' '
			})[1]);
			Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00088200 File Offset: 0x00086400
		public static void prepareSpouseForWedding()
		{
			Game1.weddingToday = true;
			NPC spouse = Game1.getCharacterFromName(Game1.player.spouse, false);
			spouse.Schedule = null;
			spouse.CurrentDialogue.Clear();
			spouse.CurrentDialogue.Push(new Dialogue("The wedding was wonderful... wasn't it, dear?$h#$e#Well, we can't forget about the farm... time to get to work.", spouse));
			spouse.DefaultMap = "FarmHouse";
			spouse.DefaultPosition = Utility.PointToVector2((Game1.getLocationFromName("FarmHouse") as FarmHouse).getSpouseBedSpot()) * (float)Game1.tileSize;
			spouse.DefaultFacingDirection = 2;
			spouse.setMarried(true);
			Game1.weatherForTomorrow = 6;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00088298 File Offset: 0x00086498
		public static void fixProblems()
		{
			List<NPC> allCharacters = Utility.getAllCharacters();
			Dictionary<string, string> NPCDispositions = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
			foreach (string s in NPCDispositions.Keys)
			{
				bool found = false;
				if (Game1.player.friendships.ContainsKey(s))
				{
					foreach (NPC i in allCharacters)
					{
						if (i.isVillager() && i.name.Equals(s))
						{
							found = true;
							if (NPCDispositions[s].Split(new char[]
							{
								'/'
							})[5].Equals("datable"))
							{
								i.datable = true;
								break;
							}
							break;
						}
					}
					if (!found)
					{
						try
						{
							Game1.getLocationFromName(NPCDispositions[s].Split(new char[]
							{
								'/'
							})[10].Split(new char[]
							{
								' '
							})[0]).addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + s), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float)(Convert.ToInt32(NPCDispositions[s].Split(new char[]
							{
								'/'
							})[10].Split(new char[]
							{
								' '
							})[1]) * Game1.tileSize), (float)(Convert.ToInt32(NPCDispositions[s].Split(new char[]
							{
								'/'
							})[10].Split(new char[]
							{
								' '
							})[2]) * Game1.tileSize)), NPCDispositions[s].Split(new char[]
							{
								'/'
							})[10].Split(new char[]
							{
								' '
							})[0], 0, s, null, Game1.content.Load<Texture2D>("Portraits\\" + s), false));
						}
						catch (Exception)
						{
						}
					}
				}
			}
			Dictionary<Type, bool> foundTool = new Dictionary<Type, bool>();
			foundTool.Add(new Axe().GetType(), false);
			foundTool.Add(new Pickaxe().GetType(), false);
			foundTool.Add(new Hoe().GetType(), false);
			foundTool.Add(new WateringCan().GetType(), false);
			if (Game1.player.hasOrWillReceiveMail("ReturnScepter"))
			{
				foundTool.Add(new Wand().GetType(), false);
			}
			bool foundScythe = false;
			for (int j = 0; j < Game1.player.items.Count; j++)
			{
				if (Game1.player.items[j] != null)
				{
					for (int k = 0; k < foundTool.Count; k++)
					{
						if (Game1.player.items[j].GetType() == foundTool.ElementAt(k).Key)
						{
							foundTool[foundTool.ElementAt(k).Key] = true;
						}
						else if (Game1.player.items[j] is MeleeWeapon && (Game1.player.items[j] as MeleeWeapon).Name.Equals("Scythe"))
						{
							foundScythe = true;
						}
					}
				}
			}
			bool allFound = true;
			for (int l = 0; l < foundTool.Count; l++)
			{
				if (!foundTool.ElementAt(l).Value)
				{
					allFound = false;
					break;
				}
			}
			if (!foundScythe)
			{
				allFound = false;
			}
			if (!allFound)
			{
				foreach (GameLocation m in Game1.locations)
				{
					foreach (Object o in m.objects.Values)
					{
						if (o is Chest)
						{
							for (int n = 0; n < foundTool.Count; n++)
							{
								foreach (Item item in (o as Chest).items)
								{
									if (item.GetType() == foundTool.ElementAt(n).Key)
									{
										foundTool[foundTool.ElementAt(n).Key] = true;
									}
									else if (item is MeleeWeapon && (item as MeleeWeapon).Name.Equals("Scythe"))
									{
										foundScythe = true;
									}
								}
							}
						}
					}
					if (m is FarmHouse)
					{
						for (int j2 = 0; j2 < foundTool.Count; j2++)
						{
							foreach (Item item2 in (m as FarmHouse).fridge.items)
							{
								if (item2.GetType() == foundTool.ElementAt(j2).Key)
								{
									foundTool[foundTool.ElementAt(j2).Key] = true;
								}
								else if (item2 is MeleeWeapon && (item2 as MeleeWeapon).Name.Equals("Scythe"))
								{
									foundScythe = true;
								}
							}
						}
					}
					if (m is BuildableGameLocation)
					{
						foreach (Building b in (m as BuildableGameLocation).buildings)
						{
							if (b.indoors != null)
							{
								using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator4 = b.indoors.objects.Values.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										Object o2 = enumerator4.Current;
										if (o2 is Chest)
										{
											for (int j3 = 0; j3 < foundTool.Count; j3++)
											{
												foreach (Item item3 in (o2 as Chest).items)
												{
													if (item3.GetType() == foundTool.ElementAt(j3).Key)
													{
														foundTool[foundTool.ElementAt(j3).Key] = true;
													}
													else if (item3 is MeleeWeapon && (item3 as MeleeWeapon).Name.Equals("Scythe"))
													{
														foundScythe = true;
													}
												}
											}
										}
									}
									continue;
								}
							}
							if (b is Mill)
							{
								for (int j4 = 0; j4 < foundTool.Count; j4++)
								{
									foreach (Item item4 in (b as Mill).output.items)
									{
										if (item4.GetType() == foundTool.ElementAt(j4).Key)
										{
											foundTool[foundTool.ElementAt(j4).Key] = true;
										}
										else if (item4 is MeleeWeapon && (item4 as MeleeWeapon).Name.Equals("Scythe"))
										{
											foundScythe = true;
										}
									}
								}
							}
							else if (b is JunimoHut)
							{
								for (int j5 = 0; j5 < foundTool.Count; j5++)
								{
									foreach (Item item5 in (b as JunimoHut).output.items)
									{
										if (item5.GetType() == foundTool.ElementAt(j5).Key)
										{
											foundTool[foundTool.ElementAt(j5).Key] = true;
										}
										else if (item5 is MeleeWeapon && (item5 as MeleeWeapon).Name.Equals("Scythe"))
										{
											foundScythe = true;
										}
									}
								}
							}
						}
					}
				}
				if (Game1.player.toolBeingUpgraded != null)
				{
					foundTool[Game1.player.toolBeingUpgraded.GetType()] = true;
				}
				List<string> toAdd = new List<string>();
				for (int j6 = 0; j6 < foundTool.Count; j6++)
				{
					if (!foundTool.ElementAt(j6).Value)
					{
						toAdd.Add(foundTool.ElementAt(j6).Key.ToString());
					}
				}
				if (!foundScythe)
				{
					toAdd.Add("Scythe");
				}
				if (toAdd.Count > 0)
				{
					Game1.addMailForTomorrow("foundLostTools", false, false);
				}
				for (int i2 = 0; i2 < toAdd.Count; i2++)
				{
					Item tool = null;
					string a = toAdd[i2];
					if (!(a == "StardewValley.Tools.Axe"))
					{
						if (!(a == "StardewValley.Tools.Hoe"))
						{
							if (!(a == "StardewValley.Tools.WateringCan"))
							{
								if (!(a == "Scythe"))
								{
									if (!(a == "StardewValley.Tools.Pickaxe"))
									{
										if (a == "StardewValley.Tools.Wand")
										{
											tool = new Wand();
										}
									}
									else
									{
										tool = new Pickaxe();
									}
								}
								else
								{
									tool = new MeleeWeapon(47);
								}
							}
							else
							{
								tool = new WateringCan();
							}
						}
						else
						{
							tool = new Hoe();
						}
					}
					else
					{
						tool = new Axe();
					}
					if (tool != null)
					{
						Utility.getHomeOfFarmer(Game1.player).debris.Add(new Debris(tool, Game1.player.position + new Vector2((float)(-(float)Game1.tileSize), 0f)));
					}
				}
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00088DB0 File Offset: 0x00086FB0
		public static void newDayAfterFade()
		{
			Game1.farmerShouldPassOut = false;
			try
			{
				Game1.fixProblems();
			}
			catch (Exception)
			{
			}
			Game1.whereIsTodaysFest = null;
			if (Game1.wind != null)
			{
				Game1.wind.Stop(AudioStopOptions.Immediate);
				Game1.wind = null;
			}
			Game1.player.currentEyes = 0;
			Game1.random = new Random((int)Game1.uniqueIDForThisGame / 100 + (int)(Game1.stats.DaysPlayed * 10u) + 1 + (int)Game1.stats.StepsTaken);
			for (int i = 0; i < Game1.dayOfMonth; i++)
			{
				Game1.random.Next();
			}
			Game1.gameTimeInterval = 0;
			Game1.player.CanMove = true;
			Game1.player.FarmerSprite.pauseForSingleAnimation = false;
			Game1.player.FarmerSprite.StopAnimation();
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.changeMusicTrack("none");
			Game1.dishOfTheDay = new Object(Vector2.Zero, Game1.random.Next(194, 240), Game1.random.Next(1, 4 + ((Game1.random.NextDouble() < 0.08) ? 10 : 0)));
			if (Game1.dishOfTheDay.parentSheetIndex == 217)
			{
				Game1.dishOfTheDay = new Object(Vector2.Zero, 216, Game1.random.Next(1, 4 + ((Game1.random.NextDouble() < 0.08) ? 10 : 0)));
			}
			using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.updatedDialogueYet = false;
				}
			}
			foreach (GameLocation j in Game1.locations)
			{
				j.currentEvent = null;
				for (int k = j.objects.Count - 1; k >= 0; k--)
				{
					if (j.objects[j.objects.Keys.ElementAt(k)].minutesElapsed(3000 - Game1.timeOfDay, j))
					{
						j.objects.Remove(j.objects.Keys.ElementAt(k));
					}
				}
			}
			foreach (Building b in Game1.getFarm().buildings)
			{
				if (b.indoors != null)
				{
					for (int l = b.indoors.objects.Count - 1; l >= 0; l--)
					{
						if (b.indoors.objects[b.indoors.objects.Keys.ElementAt(l)].minutesElapsed(3000 - Game1.timeOfDay, b.indoors))
						{
							b.indoors.objects.Remove(b.indoors.objects.Keys.ElementAt(l));
						}
					}
				}
			}
			Game1.globalOutdoorLighting = 0f;
			Game1.outdoorLight = Color.White;
			Game1.ambientLight = Color.White;
			if (Game1.isLightning)
			{
				Utility.overnightLightning();
			}
			Game1.tmpTimeOfDay = Game1.timeOfDay;
			Game1.weddingToday = false;
			if (Game1.player.spouse != null && Game1.player.spouse.Contains("engaged"))
			{
				Game1.countdownToWedding--;
				if (Game1.countdownToWedding == 0)
				{
					Game1.player.spouse = Game1.player.spouse.Replace("engaged", "");
					Game1.prepareSpouseForWedding();
				}
			}
			foreach (string s in Game1.player.mailForTomorrow)
			{
				if (s.Contains("%&NL&%"))
				{
					string stripped = s.Replace("%&NL&%", "");
					if (!Game1.player.mailReceived.Contains(stripped))
					{
						Game1.player.mailReceived.Add(stripped);
					}
				}
				else
				{
					Game1.mailbox.Enqueue(s);
				}
			}
			if (Game1.player.friendships.Count > 0)
			{
				string whichFriend = Game1.player.friendships.Keys.ElementAt(Game1.random.Next(Game1.player.friendships.Keys.Count));
				if (Game1.random.NextDouble() < (double)(Game1.player.friendships[whichFriend][0] / 250) * 0.1 && (Game1.player.spouse == null || !Game1.player.spouse.Equals(whichFriend)) && Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(whichFriend))
				{
					Game1.mailbox.Enqueue(whichFriend);
				}
			}
			Game1.player.dayupdate();
			Game1.dailyLuck = ((Game1.player.hasSpecialCharm && Game1.random.NextDouble() < 0.8) ? 0.1 : ((double)Game1.random.Next(-100, 101) / 1000.0));
			Game1.dayOfMonth++;
			Stats expr_551 = Game1.stats;
			uint daysPlayed = expr_551.DaysPlayed;
			expr_551.DaysPlayed = daysPlayed + 1u;
			Game1.player.dateStringForSaveGame = Utility.getDateString(0);
			if (Game1.dayOfMonth == 27 && Game1.currentSeason.Equals("spring"))
			{
				int arg_593_0 = Game1.year;
			}
			if (Game1.dayOfMonth == 29)
			{
				Game1.newSeason();
				if (!Game1.currentSeason.Equals("winter"))
				{
					Game1.cropsOfTheWeek = Utility.cropsOfTheWeek();
				}
				if (Game1.currentSeason.Equals("spring"))
				{
					Game1.year++;
					if (Game1.year == 2)
					{
						Game1.getLocationFromName("SamHouse").characters.Add(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Kent"), 0, 16, 32), new Vector2((float)(8 * Game1.tileSize), (float)(13 * Game1.tileSize)), "SamHouse", 3, "Kent", false, Game1.content.Load<Dictionary<int, int[]>>("Characters\\schedules\\spring\\Kent"), Game1.content.Load<Texture2D>("Portraits\\Kent")));
						if (!Game1.player.friendships.ContainsKey("Kent"))
						{
							Game1.player.friendships.Add("Kent", new int[5]);
						}
					}
				}
				int arg_694_0 = Game1.year;
			}
			if (Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(string.Concat(new object[]
			{
				Game1.currentSeason,
				"_",
				Game1.dayOfMonth,
				"_",
				Game1.year
			})))
			{
				Game1.mailbox.Enqueue(string.Concat(new object[]
				{
					Game1.currentSeason,
					"_",
					Game1.dayOfMonth,
					"_",
					Game1.year
				}));
			}
			else if (Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(Game1.currentSeason + "_" + Game1.dayOfMonth))
			{
				Game1.mailbox.Enqueue(Game1.currentSeason + "_" + Game1.dayOfMonth);
			}
			Game1.questOfTheDay = Utility.getQuestOfTheDay();
			if (Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
			{
				Game1.questOfTheDay = null;
			}
			Game1.player.resetFriendshipsForNewDay();
			if (Game1.dayOfMonth == 1 || Game1.stats.DaysPlayed <= 4u)
			{
				Game1.weatherForTomorrow = 0;
			}
			if (Game1.bloom != null)
			{
				Game1.bloomDay = false;
			}
			if (Game1.stats.DaysPlayed == 3u)
			{
				Game1.weatherForTomorrow = 1;
			}
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth % 13 == 0)
			{
				Game1.weatherForTomorrow = 3;
			}
			if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
			{
				Game1.weatherForTomorrow = 4;
				Game1.questOfTheDay = null;
			}
			if (Game1.weddingToday)
			{
				Game1.weatherForTomorrow = 6;
			}
			Game1.wasRainingYesterday = (Game1.isRaining || Game1.isLightning);
			if (Game1.weatherForTomorrow == 1 || Game1.weatherForTomorrow == 3)
			{
				Game1.isRaining = true;
			}
			if (Game1.weatherForTomorrow == 3)
			{
				Game1.isLightning = true;
			}
			if (Game1.weatherForTomorrow == 0 || Game1.weatherForTomorrow == 2 || Game1.weatherForTomorrow == 4 || Game1.weatherForTomorrow == 5 || Game1.weatherForTomorrow == 6)
			{
				Game1.isRaining = false;
				Game1.isLightning = false;
				Game1.isSnowing = false;
				Game1.changeMusicTrack("none");
				if (Game1.weatherForTomorrow == 5)
				{
					Game1.isSnowing = true;
				}
			}
			if (!Game1.isRaining && !Game1.isLightning)
			{
				Game1.currentSongIndex++;
				if (Game1.currentSongIndex > 3 || Game1.dayOfMonth == 1)
				{
					Game1.currentSongIndex = 1;
				}
			}
			Game1.debrisWeather.Clear();
			Game1.isDebrisWeather = false;
			if (Game1.bloom != null)
			{
				Game1.bloom.Visible = false;
			}
			if (Game1.weatherForTomorrow == 2)
			{
				Game1.populateDebrisWeatherArray();
			}
			else if (!Game1.isRaining && Game1.chanceToRainTomorrow <= 0.1 && Game1.bloom != null)
			{
				Game1.bloomDay = true;
				Game1.bloom.Settings = BloomSettings.PresetSettings[5];
			}
			if (Game1.currentSeason.Equals("summer"))
			{
				Game1.chanceToRainTomorrow = ((Game1.dayOfMonth > 1) ? (0.12 + (double)((float)Game1.dayOfMonth * 0.003f)) : 0.0);
			}
			else if (Game1.currentSeason.Equals("winter"))
			{
				Game1.chanceToRainTomorrow = 0.63;
			}
			else
			{
				Game1.chanceToRainTomorrow = 0.183;
			}
			if (Game1.random.NextDouble() < Game1.chanceToRainTomorrow)
			{
				Game1.weatherForTomorrow = 1;
				if ((Game1.currentSeason.Equals("summer") && Game1.random.NextDouble() < 0.85) || (!Game1.currentSeason.Equals("winter") && Game1.random.NextDouble() < 0.25 && Game1.dayOfMonth > 2 && Game1.stats.DaysPlayed > 27u))
				{
					Game1.weatherForTomorrow = 3;
				}
				if (Game1.currentSeason.Equals("winter"))
				{
					Game1.weatherForTomorrow = 5;
				}
			}
			else if (Game1.stats.DaysPlayed > 2u && ((Game1.currentSeason.Equals("spring") && Game1.random.NextDouble() < 0.2) || (Game1.currentSeason.Equals("fall") && Game1.random.NextDouble() < 0.6)) && !Game1.weddingToday)
			{
				Game1.weatherForTomorrow = 2;
			}
			else
			{
				Game1.weatherForTomorrow = 0;
			}
			if (Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
			{
				Game1.weatherForTomorrow = 4;
			}
			if (Game1.stats.DaysPlayed == 2u)
			{
				Game1.weatherForTomorrow = 1;
			}
			foreach (GameLocation expr_B0F in Game1.locations)
			{
				expr_B0F.UpdateCharacterDialogues();
				expr_B0F.DayUpdate(Game1.dayOfMonth);
			}
			using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.dayUpdate(Game1.dayOfMonth);
				}
			}
			GameLocation shop = Game1.getLocationFromName("SeedShop");
			if (shop != null)
			{
				SeedShop seedShop = shop as SeedShop;
				for (int m = seedShop.itemsFromPlayerToSell.Count - 1; m >= 0; m--)
				{
					for (int n = 0; n < seedShop.itemsFromPlayerToSell[m].Stack; n++)
					{
						if (Game1.random.NextDouble() < 0.04 && seedShop.itemsFromPlayerToSell[m] is Object && (seedShop.itemsFromPlayerToSell[m] as Object).edibility != -300)
						{
							NPC n2 = Utility.getRandomTownNPC();
							if (n2.age != 2)
							{
								n2.addExtraDialogues(seedShop.getPurchasedItemDialogueForNPC(seedShop.itemsFromPlayerToSell[m] as Object, n2));
								Item expr_C33 = seedShop.itemsFromPlayerToSell[m];
								int num = expr_C33.Stack;
								expr_C33.Stack = num - 1;
							}
						}
						else if (Game1.random.NextDouble() < 0.15)
						{
							Item expr_C69 = seedShop.itemsFromPlayerToSell[m];
							int num = expr_C69.Stack;
							expr_C69.Stack = num - 1;
						}
						if (seedShop.itemsFromPlayerToSell[m].Stack <= 0)
						{
							seedShop.itemsFromPlayerToSell.RemoveAt(m);
							break;
						}
					}
				}
			}
			if (Game1.isRaining)
			{
				foreach (KeyValuePair<Vector2, TerrainFeature> kvp in Game1.getLocationFromName("Farm").terrainFeatures)
				{
					if (kvp.Value.GetType() == typeof(HoeDirt))
					{
						((HoeDirt)kvp.Value).state = 1;
					}
				}
			}
			if (Game1.player.currentUpgrade != null)
			{
				Game1.player.currentUpgrade.daysLeftTillUpgradeDone--;
				if (Game1.getLocationFromName("Farm").objects.ContainsKey(new Vector2(Game1.player.currentUpgrade.positionOfCarpenter.X / (float)Game1.tileSize, Game1.player.currentUpgrade.positionOfCarpenter.Y / (float)Game1.tileSize)))
				{
					Game1.getLocationFromName("Farm").objects.Remove(new Vector2(Game1.player.currentUpgrade.positionOfCarpenter.X / (float)Game1.tileSize, Game1.player.currentUpgrade.positionOfCarpenter.Y / (float)Game1.tileSize));
				}
				if (Game1.player.currentUpgrade.daysLeftTillUpgradeDone == 0)
				{
					string whichBuilding = Game1.player.currentUpgrade.whichBuilding;
					if (!(whichBuilding == "House"))
					{
						if (!(whichBuilding == "Coop"))
						{
							if (!(whichBuilding == "Barn"))
							{
								if (whichBuilding == "Greenhouse")
								{
									Game1.player.hasGreenhouse = true;
									Game1.greenhouseTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Greenhouse");
								}
							}
							else
							{
								Farmer expr_EF5 = Game1.player;
								int num = expr_EF5.BarnUpgradeLevel;
								expr_EF5.BarnUpgradeLevel = num + 1;
								Game1.currentBarnTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Barn" + Game1.player.BarnUpgradeLevel);
							}
						}
						else
						{
							Farmer expr_EB5 = Game1.player;
							int num = expr_EB5.CoopUpgradeLevel;
							expr_EB5.CoopUpgradeLevel = num + 1;
							Game1.currentCoopTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Coop" + Game1.player.CoopUpgradeLevel);
						}
					}
					else
					{
						Farmer expr_E72 = Game1.player;
						int num = expr_E72.HouseUpgradeLevel;
						expr_E72.HouseUpgradeLevel = num + 1;
						Game1.currentHouseTexture = Game1.content.Load<Texture2D>("Buildings\\House" + Game1.player.HouseUpgradeLevel);
					}
					Game1.stats.checkForBuildingUpgradeAchievements();
					Game1.removeFrontLayerForFarmBuildings();
					Game1.addNewFarmBuildingMaps();
					Game1.player.currentUpgrade = null;
					Game1.changeInvisibility("Robin", false);
				}
				else if (Game1.player.currentUpgrade.daysLeftTillUpgradeDone == 3)
				{
					Game1.changeInvisibility("Robin", true);
				}
			}
			Game1.stats.AverageBedtime = (uint)Game1.timeOfDay;
			Game1.timeOfDay = 600;
			Game1.newDay = false;
			Game1.sellShippedItems();
			if (Game1.player.currentLocation != null && Game1.player.currentLocation is FarmHouse)
			{
				Game1.player.position = Utility.PointToVector2((Game1.getLocationFromName("FarmHouse") as FarmHouse).getBedSpot()) * (float)Game1.tileSize;
				Farmer expr_1019_cp_0_cp_0 = Game1.player;
				expr_1019_cp_0_cp_0.position.Y = expr_1019_cp_0_cp_0.position.Y + (float)(Game1.tileSize / 2);
				Farmer expr_1034_cp_0_cp_0 = Game1.player;
				expr_1034_cp_0_cp_0.position.X = expr_1034_cp_0_cp_0.position.X - (float)(Game1.tileSize / 2);
				Game1.player.faceDirection(1);
			}
			int arg_1050_0 = Game1.currentWallpaper;
			Game1.wallpaperPrice = Game1.random.Next(75, 500) + Game1.player.HouseUpgradeLevel * 100;
			Game1.wallpaperPrice -= Game1.wallpaperPrice % 5;
			int arg_108C_0 = Game1.currentFloor;
			Game1.floorPrice = Game1.random.Next(75, 500) + Game1.player.HouseUpgradeLevel * 100;
			Game1.floorPrice -= Game1.floorPrice % 5;
			Game1.updateWeatherIcon();
			Game1.freezeControls = false;
			if (Game1.stats.DaysPlayed > 1u)
			{
				Game1.farmEvent = Utility.pickFarmEvent();
				if (Game1.farmEvent != null && Game1.farmEvent.setUp())
				{
					Game1.farmEvent = null;
				}
			}
			Game1.player.mailForTomorrow.Clear();
			if (Game1.farmEvent == null)
			{
				Game1.showEndOfNightStuff();
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00089F3C File Offset: 0x0008813C
		public static void updateWeatherIcon()
		{
			if (Game1.isSnowing)
			{
				Game1.weatherIcon = 7;
			}
			else if (Game1.isRaining)
			{
				Game1.weatherIcon = 4;
			}
			else if (Game1.isDebrisWeather && Game1.currentSeason.Equals("spring"))
			{
				Game1.weatherIcon = 3;
			}
			else if (Game1.isDebrisWeather && Game1.currentSeason.Equals("fall"))
			{
				Game1.weatherIcon = 6;
			}
			else if (Game1.isDebrisWeather && Game1.currentSeason.Equals("winter"))
			{
				Game1.weatherIcon = 7;
			}
			else if (Game1.weddingToday)
			{
				Game1.weatherIcon = 0;
			}
			else
			{
				Game1.weatherIcon = 2;
			}
			if (Game1.isLightning)
			{
				Game1.weatherIcon = 5;
			}
			if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
			{
				Game1.weatherIcon = 1;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0008A004 File Offset: 0x00088204
		public static void showEndOfNightStuff()
		{
			bool shippingMenu = false;
			if (Game1.getFarm().shippingBin.Count > 0)
			{
				Game1.endOfNightMenus.Push(new ShippingMenu(Game1.getFarm().shippingBin));
				Game1.getFarm().shippingBin.Clear();
				shippingMenu = true;
			}
			bool levelUp = false;
			if (Game1.player.newLevels.Count > 0 && !shippingMenu)
			{
				Game1.endOfNightMenus.Push(new SaveGameMenu());
			}
			while (Game1.player.newLevels.Count > 0)
			{
				Game1.endOfNightMenus.Push(new LevelUpMenu(Game1.player.newLevels.Last<Point>().X, Game1.player.newLevels.Last<Point>().Y));
				Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
				levelUp = true;
			}
			if (levelUp)
			{
				Game1.playSound("newRecord");
			}
			if (Game1.endOfNightMenus.Count > 0)
			{
				Game1.showingEndOfNightStuff = true;
				Game1.activeClickableMenu = Game1.endOfNightMenus.Pop();
				return;
			}
			if (Game1.saveOnNewDay)
			{
				Game1.showingEndOfNightStuff = true;
				Game1.activeClickableMenu = new SaveGameMenu();
				return;
			}
			Game1.currentLocation.resetForPlayerEntry();
			Game1.globalFadeToClear(new Game1.afterFadeFunction(Game1.playMorningSong), 0.02f);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0008A14C File Offset: 0x0008834C
		public static void playerEatObject(Object o, bool overrideFullness = false)
		{
			if (o.ParentSheetIndex == 434)
			{
				Game1.changeMusicTrack("none");
			}
			if (Game1.player.getFacingDirection() != 2)
			{
				Game1.player.faceDirection(2);
			}
			Game1.player.itemToEat = o;
			Game1.player.mostRecentlyGrabbedItem = o;
			string[] objectDescription = Game1.objectInformation[o.ParentSheetIndex].Split(new char[]
			{
				'/'
			});
			Game1.player.forceCanMove();
			Game1.player.completelyStopAnimatingOrDoingAction();
			if (objectDescription.Length > 5 && objectDescription[5].Equals("drink"))
			{
				if (Game1.buffsDisplay.hasBuff(7) && !overrideFullness)
				{
					Game1.addHUDMessage(new HUDMessage("I'm not thirsty right now...", Color.OrangeRed, 3500f));
					return;
				}
				((FarmerSprite)Game1.player.Sprite).animateOnce(294, 80f, 8);
			}
			else if (Convert.ToInt32(objectDescription[2]) != -300)
			{
				if (Game1.buffsDisplay.hasBuff(6) && !overrideFullness)
				{
					Game1.addHUDMessage(new HUDMessage("I'm too full...", Color.OrangeRed, 3500f));
					return;
				}
				((FarmerSprite)Game1.player.Sprite).animateOnce(216, 80f, 8);
			}
			Game1.player.freezePause = 20000;
			Game1.player.CanMove = false;
			Game1.isEating = true;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0008A2AC File Offset: 0x000884AC
		public static void eatHeldObject()
		{
			if (Game1.fadeToBlack)
			{
				return;
			}
			if (Game1.player.ActiveObject == null)
			{
				Game1.player.ActiveObject = (Object)Game1.player.mostRecentlyGrabbedItem;
			}
			Game1.playerEatObject(Game1.player.ActiveObject, false);
			if (Game1.isEating)
			{
				Game1.player.reduceActiveItemByOne();
				Game1.player.CanMove = false;
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0008A314 File Offset: 0x00088514
		private static void updateWallpaperInSeedShop()
		{
			GameLocation seedShop = Game1.getLocationFromName("SeedShop");
			for (int i = 9; i < 12; i++)
			{
				seedShop.Map.GetLayer("Back").Tiles[i, 15] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, Game1.currentWallpaper);
				seedShop.Map.GetLayer("Back").Tiles[i, 16] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, Game1.currentWallpaper);
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0008A3D0 File Offset: 0x000885D0
		public static void setGraphicsForSeason()
		{
			foreach (GameLocation i in Game1.locations)
			{
				i.seasonUpdate(Game1.currentSeason, true);
				if (i.IsOutdoors)
				{
					if (!i.Name.Equals("Desert"))
					{
						for (int j = 0; j < i.Map.TileSheets.Count; j++)
						{
							if (!i.Map.TileSheets[j].ImageSource.Contains("path") && !i.Map.TileSheets[j].ImageSource.Contains("object"))
							{
								i.Map.TileSheets[j].ImageSource = "Maps\\" + Game1.currentSeason + "_" + i.Map.TileSheets[j].ImageSource.Split(new char[]
								{
									'_'
								})[1];
								i.Map.DisposeTileSheets(Game1.mapDisplayDevice);
								i.Map.LoadTileSheets(Game1.mapDisplayDevice);
							}
						}
					}
					if (Game1.currentSeason.Equals("spring"))
					{
						foreach (KeyValuePair<Vector2, Object> o in i.Objects)
						{
							if ((o.Value.Name.Contains("Stump") || o.Value.Name.Contains("Boulder") || o.Value.Name.Equals("Stick") || o.Value.Name.Equals("Stone")) && o.Value.ParentSheetIndex >= 378 && o.Value.ParentSheetIndex <= 391)
							{
								o.Value.ParentSheetIndex -= 376;
							}
						}
						Game1.eveningColor = new Color(255, 255, 0);
					}
					else if (Game1.currentSeason.Equals("summer"))
					{
						foreach (KeyValuePair<Vector2, Object> o2 in i.Objects)
						{
							if (o2.Value.Name.Contains("Weed"))
							{
								if (o2.Value.parentSheetIndex == 792)
								{
									Object expr_280 = o2.Value;
									int parentSheetIndex = expr_280.ParentSheetIndex;
									expr_280.ParentSheetIndex = parentSheetIndex + 1;
								}
								else if (Game1.random.NextDouble() < 0.3)
								{
									o2.Value.ParentSheetIndex = 676;
								}
								else if (Game1.random.NextDouble() < 0.3)
								{
									o2.Value.ParentSheetIndex = 677;
								}
							}
						}
						Game1.eveningColor = new Color(255, 255, 0);
					}
					else
					{
						if (Game1.currentSeason.Equals("fall"))
						{
							foreach (KeyValuePair<Vector2, Object> o3 in i.Objects)
							{
								if (o3.Value.Name.Contains("Weed"))
								{
									if (o3.Value.parentSheetIndex == 793)
									{
										Object expr_377 = o3.Value;
										int parentSheetIndex = expr_377.ParentSheetIndex;
										expr_377.ParentSheetIndex = parentSheetIndex + 1;
									}
									else if (Game1.random.NextDouble() < 0.5)
									{
										o3.Value.ParentSheetIndex = 678;
									}
									else
									{
										o3.Value.ParentSheetIndex = 679;
									}
								}
							}
							Game1.eveningColor = new Color(255, 255, 0);
							using (List<WeatherDebris>.Enumerator enumerator3 = Game1.debrisWeather.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									enumerator3.Current.which = 2;
								}
								continue;
							}
						}
						if (Game1.currentSeason.Equals("winter"))
						{
							for (int k = i.Objects.Count - 1; k >= 0; k--)
							{
								Object o4 = i.Objects[i.Objects.Keys.ElementAt(k)];
								if (o4.Name.Contains("Weed"))
								{
									i.Objects.Remove(i.Objects.Keys.ElementAt(k));
								}
								else if (((!o4.Name.Contains("Stump") && !o4.Name.Contains("Boulder") && !o4.Name.Equals("Stick") && !o4.Name.Equals("Stone")) || o4.ParentSheetIndex > 100) && i.IsOutdoors && !o4.isHoedirt)
								{
									o4.name.Equals("HoeDirt");
								}
							}
							using (List<WeatherDebris>.Enumerator enumerator3 = Game1.debrisWeather.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									enumerator3.Current.which = 3;
								}
							}
							Game1.eveningColor = new Color(245, 225, 170);
						}
					}
				}
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0008AA08 File Offset: 0x00088C08
		private static void updateFloorInSeedShop()
		{
			GameLocation seedShop = Game1.getLocationFromName("SeedShop");
			for (int i = 9; i < 12; i++)
			{
				seedShop.Map.GetLayer("Back").Tiles[i, 17] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Floors"), BlendMode.Alpha, Game1.currentFloor);
				seedShop.Map.GetLayer("Back").Tiles[i, 18] = new StaticTile(seedShop.Map.GetLayer("Back"), seedShop.Map.GetTileSheet("Floors"), BlendMode.Alpha, Game1.currentFloor);
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0008AAC4 File Offset: 0x00088CC4
		public static void pauseThenMessage(int millisecondsPause, string message, bool showProgressBar)
		{
			Game1.messageAfterPause = message;
			Game1.pauseTime = (float)millisecondsPause;
			Game1.progressBar = showProgressBar;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0008AADC File Offset: 0x00088CDC
		public static void updateWallpaperInFarmHouse(int wallpaper)
		{
			GameLocation farmhouse = Game1.getLocationFromName("FarmHouse");
			PropertyValue wallpaperArea;
			farmhouse.Map.Properties.TryGetValue("Wallpaper", out wallpaperArea);
			if (wallpaperArea != null)
			{
				string[] split = wallpaperArea.ToString().Split(new char[]
				{
					' '
				});
				for (int i = 0; i < split.Length; i += 4)
				{
					int topLeftX = Convert.ToInt32(split[i]);
					int topLeftY = Convert.ToInt32(split[i + 1]);
					int width = Convert.ToInt32(split[i + 2]);
					int height = Convert.ToInt32(split[i + 3]);
					for (int j = topLeftX; j < topLeftX + width; j++)
					{
						for (int k = topLeftY; k < topLeftY + height; k++)
						{
							farmhouse.Map.GetLayer("Back").Tiles[j, k] = new StaticTile(farmhouse.Map.GetLayer("Back"), farmhouse.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, wallpaper);
						}
					}
				}
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0008ABDC File Offset: 0x00088DDC
		public static void updateFloorInFarmHouse(int floor)
		{
			GameLocation farmhouse = Game1.getLocationFromName("FarmHouse");
			PropertyValue floorArea;
			farmhouse.Map.Properties.TryGetValue("Floor", out floorArea);
			if (floorArea != null)
			{
				string[] split = floorArea.ToString().Split(new char[]
				{
					' '
				});
				for (int i = 0; i < split.Length; i += 4)
				{
					int topLeftX = Convert.ToInt32(split[i]);
					int topLeftY = Convert.ToInt32(split[i + 1]);
					int width = Convert.ToInt32(split[i + 2]);
					int height = Convert.ToInt32(split[i + 3]);
					for (int j = topLeftX; j < topLeftX + width; j++)
					{
						for (int k = topLeftY; k < topLeftY + height; k++)
						{
							farmhouse.Map.GetLayer("Back").Tiles[j, k] = new StaticTile(farmhouse.Map.GetLayer("Back"), farmhouse.Map.GetTileSheet("Floors"), BlendMode.Alpha, floor);
						}
					}
				}
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0008ACDC File Offset: 0x00088EDC
		public static bool shouldTimePass()
		{
			return (Game1.IsMultiplayer || Game1.player.CanMove || Game1.player.usingTool || Game1.player.forceTimePass) && !Game1.paused && !Game1.isFestival() && !Game1.freezeControls && (Game1.activeClickableMenu == null || Game1.activeClickableMenu is BobberBar);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0008AD44 File Offset: 0x00088F44
		public static void UpdateViewPort(bool overrideFreeze, Point centerPoint)
		{
			Game1.previousViewportPosition.X = (float)Game1.viewport.X;
			Game1.previousViewportPosition.Y = (float)Game1.viewport.Y;
			if (!Game1.viewportFreeze | overrideFreeze)
			{
				bool snapBack = Math.Abs(Game1.currentViewportTarget.X + (float)(Game1.viewport.Width / 2) - (float)Game1.player.getStandingX()) > (float)Game1.tileSize || Math.Abs(Game1.currentViewportTarget.Y + (float)(Game1.viewport.Height / 2) - (float)Game1.player.getStandingY()) > (float)Game1.tileSize;
				if (centerPoint.X >= Game1.viewport.Width / 2 && centerPoint.X <= Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width / 2)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.X = (float)(centerPoint.X - Game1.viewport.Width / 2);
					}
					else if (Math.Abs(Game1.currentViewportTarget.X - (Game1.currentViewportTarget.X = (float)(centerPoint.X - Game1.viewport.Width / 2))) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.X = Game1.currentViewportTarget.X + (float)Math.Sign(Game1.currentViewportTarget.X - (Game1.currentViewportTarget.X = (float)(centerPoint.X - Game1.viewport.Width / 2))) * Game1.player.getMovementSpeed();
					}
				}
				else if (centerPoint.X < Game1.viewport.Width / 2 && Game1.viewport.Width <= Game1.currentLocation.Map.DisplayWidth)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.X = 0f;
					}
					else if (Math.Abs(Game1.currentViewportTarget.X - 0f) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.X = Game1.currentViewportTarget.X - (float)Math.Sign(Game1.currentViewportTarget.X - 0f) * Game1.player.getMovementSpeed();
					}
				}
				else if (Game1.viewport.Width <= Game1.currentLocation.Map.DisplayWidth)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.X = (float)(Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width);
					}
					else if (Math.Abs(Game1.currentViewportTarget.X - (float)(Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width)) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.X = Game1.currentViewportTarget.X + (float)Math.Sign(Game1.currentViewportTarget.X - (float)(Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width)) * Game1.player.getMovementSpeed();
					}
				}
				else if (Game1.currentLocation.Map.DisplayWidth < Game1.viewport.Width)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.X = (float)((Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2);
					}
					else if (Math.Abs(Game1.currentViewportTarget.X - (float)((Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2)) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.X = Game1.currentViewportTarget.X - (float)Math.Sign(Game1.currentViewportTarget.X - (float)((Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2)) * Game1.player.getMovementSpeed();
					}
				}
				if (centerPoint.Y >= Game1.viewport.Height / 2 && centerPoint.Y <= Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height / 2)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.Y = (float)(centerPoint.Y - Game1.viewport.Height / 2);
					}
					else if (Math.Abs(Game1.currentViewportTarget.Y - (float)(centerPoint.Y - Game1.viewport.Height / 2)) >= Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.Y = Game1.currentViewportTarget.Y - (float)Math.Sign(Game1.currentViewportTarget.Y - (float)(centerPoint.Y - Game1.viewport.Height / 2)) * Game1.player.getMovementSpeed();
					}
				}
				else if (centerPoint.Y < Game1.viewport.Height / 2 && Game1.viewport.Height <= Game1.currentLocation.Map.DisplayHeight)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.Y = 0f;
					}
					else if (Math.Abs(Game1.currentViewportTarget.Y - 0f) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.Y = Game1.currentViewportTarget.Y - (float)Math.Sign(Game1.currentViewportTarget.Y - 0f) * Game1.player.getMovementSpeed();
					}
					Game1.currentViewportTarget.Y = 0f;
				}
				else if (Game1.viewport.Height <= Game1.currentLocation.Map.DisplayHeight)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.Y = (float)(Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height);
					}
					else if (Math.Abs(Game1.currentViewportTarget.Y - (float)(Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height)) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.Y = Game1.currentViewportTarget.Y - (float)Math.Sign(Game1.currentViewportTarget.Y - (float)(Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height)) * Game1.player.getMovementSpeed();
					}
				}
				else if (Game1.currentLocation.Map.DisplayHeight < Game1.viewport.Height)
				{
					if (Game1.player.isRafting | snapBack)
					{
						Game1.currentViewportTarget.Y = (float)((Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2);
					}
					else if (Math.Abs(Game1.currentViewportTarget.Y - (float)((Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2)) > Game1.player.getMovementSpeed())
					{
						Game1.currentViewportTarget.Y = Game1.currentViewportTarget.Y - (float)Math.Sign(Game1.currentViewportTarget.Y - (float)((Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2)) * Game1.player.getMovementSpeed();
					}
				}
			}
			if (Game1.currentLocation.forceViewportPlayerFollow)
			{
				Game1.currentViewportTarget.X = Game1.player.position.X - (float)(Game1.viewport.Width / 2);
				Game1.currentViewportTarget.Y = Game1.player.position.Y - (float)(Game1.viewport.Height / 2);
			}
			if (Game1.currentViewportTarget.X == -2.14748365E+09f || (Game1.viewportFreeze && !overrideFreeze))
			{
				return;
			}
			int difference = (int)(Game1.currentViewportTarget.X - (float)Game1.viewport.X);
			if (Math.Abs(difference) > Game1.tileSize * 2)
			{
				Game1.viewportPositionLerp.X = Game1.currentViewportTarget.X;
			}
			else
			{
				Game1.viewportPositionLerp.X = Game1.viewportPositionLerp.X + (float)difference * Game1.player.getMovementSpeed() * 0.03f;
			}
			difference = (int)(Game1.currentViewportTarget.Y - (float)Game1.viewport.Y);
			if (Math.Abs(difference) > Game1.tileSize * 2)
			{
				Game1.viewportPositionLerp.Y = (float)((int)Game1.currentViewportTarget.Y);
			}
			else
			{
				Game1.viewportPositionLerp.Y = Game1.viewportPositionLerp.Y + (float)difference * Game1.player.getMovementSpeed() * 0.03f;
			}
			Game1.viewport.X = (int)Game1.viewportPositionLerp.X;
			Game1.viewport.Y = (int)Game1.viewportPositionLerp.Y;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0008B5FC File Offset: 0x000897FC
		private void UpdateCharacters(GameTime time)
		{
			Game1.player.Update(time, Game1.currentLocation);
			foreach (KeyValuePair<long, Farmer> v in Game1.otherFarmers)
			{
				if (v.Key != Game1.player.uniqueMultiplayerID)
				{
					v.Value.UpdateIfOtherPlayer(time);
				}
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0008B678 File Offset: 0x00089878
		public static void addMailForTomorrow(string mailName, bool noLetter = false, bool sendToEveryone = false)
		{
			mailName = mailName.Trim();
			mailName = mailName.Replace(Environment.NewLine, "");
			if (!Game1.player.hasOrWillReceiveMail(mailName))
			{
				if (noLetter)
				{
					mailName += "%&NL&%";
				}
				Game1.player.mailForTomorrow.Add(mailName);
				if (sendToEveryone && Game1.IsMultiplayer)
				{
					MultiplayerUtility.sendMessageToEveryone(7, mailName, Game1.player.uniqueMultiplayerID);
				}
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0008B6E7 File Offset: 0x000898E7
		public static void fadeScreenToBlack()
		{
			Game1.fadeToBlack = true;
			Game1.fadeIn = true;
			Game1.fadeToBlackAlpha = 0f;
			Game1.player.CanMove = false;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0008B70C File Offset: 0x0008990C
		public static void drawDialogue(NPC speaker)
		{
			Game1.activeClickableMenu = new DialogueBox(speaker.CurrentDialogue.Peek());
			Game1.dialogueUp = true;
			if (!Game1.eventUp)
			{
				Game1.player.Halt();
				Game1.player.CanMove = false;
			}
			if (speaker != null)
			{
				Game1.currentSpeaker = speaker;
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0008B75C File Offset: 0x0008995C
		public static void drawDialogueNoTyping(NPC speaker, string dialogue)
		{
			if (speaker == null)
			{
				Game1.currentObjectDialogue.Enqueue(dialogue);
			}
			else if (dialogue != null)
			{
				speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker));
			}
			Game1.activeClickableMenu = new DialogueBox(speaker.CurrentDialogue.Peek());
			Game1.dialogueUp = true;
			Game1.player.CanMove = false;
			if (speaker != null)
			{
				Game1.currentSpeaker = speaker;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0008B7BD File Offset: 0x000899BD
		public static void multipleDialogues(string[] messages)
		{
			Game1.activeClickableMenu = new DialogueBox(messages.ToList<string>());
			Game1.dialogueUp = true;
			Game1.player.CanMove = false;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0008B7E0 File Offset: 0x000899E0
		public static void drawDialogueNoTyping(string dialogue)
		{
			Game1.drawObjectDialogue(dialogue);
			if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is DialogueBox)
			{
				(Game1.activeClickableMenu as DialogueBox).finishTyping();
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0008B80A File Offset: 0x00089A0A
		public static void drawDialogue(NPC speaker, string dialogue)
		{
			speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker));
			Game1.drawDialogue(speaker);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0008B824 File Offset: 0x00089A24
		public static void drawItemNumberSelection(string itemType, int price)
		{
			Game1.selectedItemsType = itemType;
			Game1.numberOfSelectedItems = 0;
			Game1.priceOfSelectedItem = price;
			if (itemType.Equals("calicoJackBet"))
			{
				Game1.currentObjectDialogue.Enqueue("How much do you want to bet? (You have " + Game1.player.clubCoins + " club coins).");
				return;
			}
			if (itemType.Equals("flutePitch"))
			{
				Game1.currentObjectDialogue.Enqueue("Semitones above C3:");
				Game1.numberOfSelectedItems = (int)Game1.currentLocation.actionObjectForQuestionDialogue.scale.X / 100;
				return;
			}
			if (itemType.Equals("drumTone"))
			{
				Game1.currentObjectDialogue.Enqueue("Drum Sound:");
				Game1.numberOfSelectedItems = (int)Game1.currentLocation.actionObjectForQuestionDialogue.scale.X;
				return;
			}
			if (itemType.Equals("jukebox"))
			{
				Game1.currentObjectDialogue.Enqueue("Choose song: ");
				return;
			}
			if (itemType.Equals("Fuel"))
			{
				Game1.drawObjectDialogue("How many units of fuel do you want to buy?");
				return;
			}
			if (Game1.currentSpeaker != null)
			{
				Game1.setDialogue("How many would you like?", false);
				return;
			}
			Game1.currentObjectDialogue.Enqueue("How many would you like to buy?");
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0008B944 File Offset: 0x00089B44
		public static void setDialogue(string dialogue, bool typing)
		{
			if (Game1.currentSpeaker != null)
			{
				Game1.currentSpeaker.CurrentDialogue.Peek().setCurrentDialogue(dialogue);
				if (typing)
				{
					Game1.drawDialogue(Game1.currentSpeaker);
					return;
				}
				Game1.drawDialogueNoTyping(Game1.currentSpeaker, null);
				return;
			}
			else
			{
				if (typing)
				{
					Game1.drawObjectDialogue(dialogue);
					return;
				}
				Game1.drawDialogueNoTyping(dialogue);
				return;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0008B998 File Offset: 0x00089B98
		private static void checkIfDialogueIsQuestion()
		{
			if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0 && Game1.currentSpeaker.CurrentDialogue.Peek().isCurrentDialogueAQuestion())
			{
				Game1.questionChoices.Clear();
				Game1.isQuestion = true;
				List<NPCDialogueResponse> questions = Game1.currentSpeaker.CurrentDialogue.Peek().getNPCResponseOptions();
				for (int i = 0; i < questions.Count; i++)
				{
					Game1.questionChoices.Add(questions[i]);
				}
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0008BA1B File Offset: 0x00089C1B
		public static void drawLetterMessage(string message)
		{
			Game1.activeClickableMenu = new LetterViewerMenu(message);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0008BA28 File Offset: 0x00089C28
		public static void drawObjectDialogue(string dialogue)
		{
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.emergencyShutDown();
			}
			Game1.activeClickableMenu = new DialogueBox(dialogue);
			Game1.player.CanMove = false;
			Game1.dialogueUp = true;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0008BA57 File Offset: 0x00089C57
		public static void drawObjectQuestionDialogue(string dialogue, List<Response> choices)
		{
			Game1.activeClickableMenu = new DialogueBox(dialogue, choices);
			Game1.dialogueUp = true;
			Game1.player.CanMove = false;
		}

		// Token: 0x1700007B RID: 123
		public static bool IsSummer
		{
			// Token: 0x06000665 RID: 1637 RVA: 0x0008BA76 File Offset: 0x00089C76
			get
			{
				return Game1.currentSeason.Equals("summer");
			}
		}

		// Token: 0x1700007C RID: 124
		public static bool IsSpring
		{
			// Token: 0x06000666 RID: 1638 RVA: 0x0008BA87 File Offset: 0x00089C87
			get
			{
				return Game1.currentSeason.Equals("spring");
			}
		}

		// Token: 0x1700007D RID: 125
		public static bool IsFall
		{
			// Token: 0x06000667 RID: 1639 RVA: 0x0008BA98 File Offset: 0x00089C98
			get
			{
				return Game1.currentSeason.Equals("fall");
			}
		}

		// Token: 0x1700007E RID: 126
		public static bool IsWinter
		{
			// Token: 0x06000668 RID: 1640 RVA: 0x0008BAA9 File Offset: 0x00089CA9
			get
			{
				return Game1.currentSeason.Equals("winter");
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0008BABC File Offset: 0x00089CBC
		public static void removeThisCharacterFromAllLocations(NPC toDelete)
		{
			for (int i = 0; i < Game1.locations.Count; i++)
			{
				if (Game1.locations[i].characters.Contains(toDelete))
				{
					Game1.locations[i].characters.Remove(toDelete);
				}
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0008BB0D File Offset: 0x00089D0D
		public static void warpCharacter(NPC character, string targetLocationName, Point location, bool returnToDefault, bool wasOutdoors)
		{
			Game1.warpCharacter(character, targetLocationName, new Vector2((float)location.X, (float)location.Y), returnToDefault, wasOutdoors);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0008BB2C File Offset: 0x00089D2C
		public static void warpCharacter(NPC character, string targetLocationName, Vector2 location, bool returnToDefault, bool wasOutdoors)
		{
			for (int i = 0; i < Game1.locations.Count; i++)
			{
				if (Game1.locations[i].Name.Equals(targetLocationName))
				{
					if (!Game1.locations[i].characters.Contains(character))
					{
						Game1.locations[i].addCharacter(character);
					}
					character.isCharging = false;
					character.speed = 2;
					character.blockedInterval = 0;
					string textureFileName = character.name;
					bool load = false;
					if (character.name.Equals("Maru"))
					{
						if (targetLocationName.Equals("Hospital"))
						{
							textureFileName = character.name + "_" + Game1.locations[i].Name;
							load = true;
						}
						else if (targetLocationName.Equals("Town"))
						{
							textureFileName = character.name;
							load = true;
						}
					}
					else if (character.name.Equals("Shane"))
					{
						if (targetLocationName.Equals("JojaMart"))
						{
							textureFileName = character.name + "_" + Game1.locations[i].Name;
							load = true;
						}
						else if (targetLocationName.Equals("Town"))
						{
							textureFileName = character.name;
							load = true;
						}
					}
					if (load)
					{
						character.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + textureFileName);
					}
					character.position.X = location.X * (float)Game1.tileSize;
					character.position.Y = location.Y * (float)Game1.tileSize;
					if (character.CurrentDialogue.Count > 0 && character.CurrentDialogue.Peek().removeOnNextMove && !character.getTileLocation().Equals(character.DefaultPosition / (float)Game1.tileSize))
					{
						character.CurrentDialogue.Pop();
					}
					if (Game1.locations[i] is FarmHouse)
					{
						character.arriveAtFarmHouse();
					}
					else
					{
						character.arriveAt(Game1.locations[i]);
					}
					if (character.currentLocation != null && !character.currentLocation.Equals(Game1.locations[i]))
					{
						character.currentLocation.characters.Remove(character);
					}
					character.currentLocation = Game1.locations[i];
					return;
				}
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0008BD7B File Offset: 0x00089F7B
		public static void warpFarmer(string locationName, int tileX, int tileY, bool flip)
		{
			Game1.warpFarmer(Game1.getLocationFromName(locationName), tileX, tileY, flip ? ((Game1.player.facingDirection + 2) % 4) : Game1.player.facingDirection, false);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0008BDA8 File Offset: 0x00089FA8
		public static void warpFarmer(string locationName, int tileX, int tileY, int facingDirectionAfterWarp)
		{
			Game1.warpFarmer(Game1.getLocationFromName(locationName), tileX, tileY, facingDirectionAfterWarp, false);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0008BDBC File Offset: 0x00089FBC
		public static void warpFarmer(GameLocation locationAfterWarp, int tileX, int tileY, int facingDirectionAfterWarp, bool isStructure)
		{
			if (Game1.weatherIcon == 1 && Game1.whereIsTodaysFest != null && locationAfterWarp.name.Equals(Game1.whereIsTodaysFest) && Game1.timeOfDay < Convert.ToInt32(Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth)["conditions"].Split(new char[]
			{
				'/'
			})[1].Split(new char[]
			{
				' '
			})[0]))
			{
				Game1.player.position = Game1.player.lastPosition;
				Game1.drawObjectDialogue("Today's festival is being set up. Come back later.");
				return;
			}
			Game1.player.previousLocationName = Game1.player.currentLocation.name;
			Game1.locationAfterWarp = locationAfterWarp;
			Game1.xLocationAfterWarp = tileX;
			Game1.yLocationAfterWarp = tileY;
			Game1.facingDirectionAfterWarp = facingDirectionAfterWarp;
			Game1.fadeScreenToBlack();
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0008BEAA File Offset: 0x0008A0AA
		public static void changeInvisibility(string name, bool invisibility)
		{
			Game1.getCharacterFromName(name, false).isInvisible = invisibility;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0008BEBC File Offset: 0x0008A0BC
		public static NPC getCharacterFromName(string name, bool mustBeVillager = false)
		{
			if (Game1.currentLocation != null)
			{
				for (int i = 0; i < Game1.currentLocation.getCharacters().Count; i++)
				{
					if (Game1.currentLocation.getCharacters()[i].name.Equals(name) && (!mustBeVillager || Game1.currentLocation.getCharacters()[i].isVillager()))
					{
						return Game1.currentLocation.getCharacters()[i];
					}
				}
			}
			for (int j = 0; j < Game1.locations.Count; j++)
			{
				for (int k = 0; k < Game1.locations[j].getCharacters().Count; k++)
				{
					if (Game1.locations[j].getCharacters()[k].name.Equals(name) && (!mustBeVillager || Game1.locations[j].getCharacters()[k].isVillager()))
					{
						return Game1.locations[j].getCharacters()[k];
					}
				}
			}
			if (Game1.mine != null)
			{
				for (int l = 0; l < Game1.mine.getCharacters().Count; l++)
				{
					if (Game1.mine.getCharacters()[l].name.Equals(name) && (!mustBeVillager || Game1.mine.getCharacters()[l].isVillager()))
					{
						return Game1.mine.getCharacters()[l];
					}
				}
			}
			if (Game1.getFarm() != null)
			{
				foreach (Building b in Game1.getFarm().buildings)
				{
					if (b.indoors != null)
					{
						foreach (NPC m in b.indoors.characters)
						{
							if (m.name.Equals(name) && (!mustBeVillager || m.isVillager()))
							{
								return m;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0008C0F0 File Offset: 0x0008A2F0
		public static NPC removeCharacterFromItsLocation(string name)
		{
			for (int i = 0; i < Game1.locations.Count; i++)
			{
				for (int j = 0; j < Game1.locations[i].getCharacters().Count; j++)
				{
					if (Game1.locations[i].getCharacters()[j].name.Equals(name))
					{
						NPC arg_57_0 = Game1.locations[i].characters[j];
						Game1.locations[i].characters.RemoveAt(j);
						return arg_57_0;
					}
				}
			}
			return null;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0008C186 File Offset: 0x0008A386
		public static GameLocation getLocationFromName(string name)
		{
			return Game1.getLocationFromName(name, false);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0008C190 File Offset: 0x0008A390
		public static GameLocation getLocationFromName(string name, bool isStructure)
		{
			if (name == null)
			{
				return null;
			}
			for (int i = 0; i < Game1.locations.Count; i++)
			{
				if (!isStructure)
				{
					if (Game1.locations[i].Name.ToLower().Equals(name.ToLower()))
					{
						return Game1.locations[i];
					}
				}
				else if (Game1.locations[i] is Farm)
				{
					for (int j = 0; j < (Game1.locations[i] as Farm).buildings.Count; j++)
					{
						if ((Game1.locations[i] as Farm).buildings.ElementAt(j).nameOfIndoors.Equals(name))
						{
							return (Game1.locations[i] as Farm).buildings.ElementAt(j).indoors;
						}
					}
				}
			}
			if (name.Equals("UndergroundMine"))
			{
				return Game1.mine;
			}
			if (!isStructure)
			{
				return Game1.getLocationFromName(name, true);
			}
			return null;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0008C290 File Offset: 0x0008A490
		public static void addNewFarmBuildingMaps()
		{
			if (Game1.player.CoopUpgradeLevel >= 1 && Game1.getLocationFromName("Coop") == null)
			{
				Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Coop" + Game1.player.CoopUpgradeLevel), "Coop"));
				Game1.getLocationFromName("Farm").setTileProperty(21, 10, "Buildings", "Action", "Warp 2 9 Coop");
				Game1.currentCoopTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Coop" + Game1.player.coopUpgradeLevel);
			}
			else if (Game1.getLocationFromName("Coop") != null)
			{
				Game1.getLocationFromName("Coop").map = Game1.content.Load<Map>("Maps\\Coop" + Game1.player.CoopUpgradeLevel);
				Game1.currentCoopTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Coop" + Game1.player.coopUpgradeLevel);
			}
			if (Game1.player.BarnUpgradeLevel >= 1 && Game1.getLocationFromName("Barn") == null)
			{
				Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Barn" + Game1.player.BarnUpgradeLevel), "Barn"));
				Game1.getLocationFromName("Farm").warps.Add(new Warp(14, 9, "Barn", 11, 14, false));
				Game1.currentBarnTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Barn" + Game1.player.barnUpgradeLevel);
			}
			else if (Game1.getLocationFromName("Barn") != null)
			{
				Game1.getLocationFromName("Barn").map = Game1.content.Load<Map>("Maps\\Barn" + Game1.player.BarnUpgradeLevel);
				Game1.currentBarnTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Barn" + Game1.player.barnUpgradeLevel);
			}
			if (Game1.player.HouseUpgradeLevel >= 1 && Game1.getLocationFromName("FarmHouse").Map.Id.Equals("FarmHouse"))
			{
				Game1.getLocationFromName("FarmHouse").Map = Game1.content.Load<Map>("Maps\\FarmHouse" + Game1.player.HouseUpgradeLevel);
				Game1.getLocationFromName("FarmHouse").Map.LoadTileSheets(Game1.mapDisplayDevice);
				int arg_2C6_0 = Game1.currentWallpaper;
				int curFloor = Game1.currentFloor;
				Game1.currentWallpaper = Game1.farmerWallpaper;
				Game1.currentFloor = Game1.FarmerFloor;
				Game1.updateFloorInFarmHouse(Game1.currentFloor);
				Game1.updateWallpaperInFarmHouse(Game1.currentWallpaper);
				Game1.currentWallpaper = arg_2C6_0;
				Game1.currentFloor = curFloor;
			}
			if (Game1.player.hasGreenhouse && Game1.getLocationFromName("FarmGreenHouse") == null)
			{
				Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\FarmGreenHouse"), "FarmGreenHouse"));
				Game1.getLocationFromName("Farm").setTileProperty(3, 10, "Buildings", "Action", "Warp 5 15 FarmGreenHouse");
				Game1.greenhouseTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Greenhouse");
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0008C5E0 File Offset: 0x0008A7E0
		public static void NewDay(float timeToPause)
		{
			Game1.nonWarpFade = true;
			Game1.fadeScreenToBlack();
			Game1.newDay = true;
			Game1.player.Halt();
			Game1.player.currentEyes = 1;
			Game1.player.blinkTimer = -4000;
			Game1.player.CanMove = false;
			Game1.pauseTime = timeToPause;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0008C634 File Offset: 0x0008A834
		public static void setUpSpouse()
		{
			NPC spouse = Game1.getCharacterFromName(Game1.player.spouse, false);
			Game1.getLocationFromName(spouse.DefaultMap).characters.Remove(Game1.getLocationFromName(spouse.DefaultMap).getCharacterFromName(Game1.player.spouse));
			spouse.Schedule = null;
			spouse.CurrentDialogue.Clear();
			spouse.DefaultMap = "FarmHouse";
			spouse.DefaultPosition = new Vector2((float)(9 * Game1.tileSize), (float)(4 * Game1.tileSize - Game1.tileSize));
			spouse.DefaultFacingDirection = 0;
			Game1.getLocationFromName("FarmHouse").characters.Add(spouse);
			spouse.position = new Vector2((float)(9 * Game1.tileSize), (float)(4 * Game1.tileSize - Game1.tileSize));
			spouse.faceDirection(2);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0008C706 File Offset: 0x0008A906
		public static void screenGlowOnce(Color glowColor, bool hold, float rate = 0.005f, float maxAlpha = 0.3f)
		{
			Game1.screenGlowMax = maxAlpha;
			Game1.screenGlowRate = rate;
			Game1.screenGlowAlpha = 0f;
			Game1.screenGlowUp = true;
			Game1.screenGlowColor = glowColor;
			Game1.screenGlow = true;
			Game1.screenGlowHold = hold;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0008C738 File Offset: 0x0008A938
		public static void removeTilesFromLayer(GameLocation l, string layer, Microsoft.Xna.Framework.Rectangle area)
		{
			for (int i = area.X; i < area.Right; i++)
			{
				for (int j = area.Y; j < area.Bottom; j++)
				{
					l.Map.GetLayer(layer).Tiles[i, j] = null;
				}
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0008C78C File Offset: 0x0008A98C
		public static void removeFrontLayerForFarmBuildings()
		{
			GameLocation farm = Game1.getLocationFromName("Farm");
			if (Game1.player.CoopUpgradeLevel > 0)
			{
				Game1.removeTilesFromLayer(farm, "Front", new Microsoft.Xna.Framework.Rectangle(20, 5, 4, 6));
			}
			int barnUpgradeLevel = Game1.player.BarnUpgradeLevel;
			if (barnUpgradeLevel != 1)
			{
				if (barnUpgradeLevel == 2)
				{
					Game1.removeTilesFromLayer(farm, "Front", new Microsoft.Xna.Framework.Rectangle(9, 4, 8, 7));
				}
			}
			else
			{
				Game1.removeTilesFromLayer(farm, "Front", new Microsoft.Xna.Framework.Rectangle(12, 5, 5, 6));
			}
			switch (Game1.player.HouseUpgradeLevel)
			{
			case 1:
				Game1.removeTilesFromLayer(farm, "Front", new Microsoft.Xna.Framework.Rectangle(31, 6, 5, 5));
				return;
			case 2:
				Game1.removeTilesFromLayer(farm, "Front", new Microsoft.Xna.Framework.Rectangle(31, 5, 5, 6));
				return;
			case 3:
				Game1.removeTilesFromLayer(farm, "Front", new Microsoft.Xna.Framework.Rectangle(31, 5, 5, 6));
				return;
			default:
				return;
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0008C868 File Offset: 0x0008AA68
		public static string shortDayNameFromDayOfSeason(int dayOfSeason)
		{
			switch (dayOfSeason % 7)
			{
			case 0:
				return "Sun";
			case 1:
				return "Mon";
			case 2:
				return "Tue";
			case 3:
				return "Wed";
			case 4:
				return "Thu";
			case 5:
				return "Fri";
			case 6:
				return "Sat";
			default:
				return "";
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0008C8CC File Offset: 0x0008AACC
		public static void shipObject(Object item)
		{
			bool addedToStack = false;
			for (int i = 0; i < Game1.shippingBin.Count; i++)
			{
				if (Game1.shippingBin[i].Name.Equals(item.Name))
				{
					Object expr_2E = Game1.shippingBin[i];
					int stack = expr_2E.Stack;
					expr_2E.Stack = stack + 1;
					addedToStack = true;
					break;
				}
			}
			if (!addedToStack)
			{
				Game1.shippingBin.Add(item);
			}
			if (item.type.Equals("Basic"))
			{
				Game1.player.shippedBasic(item.ParentSheetIndex, 1);
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0008C95C File Offset: 0x0008AB5C
		public static void shipHeldItem()
		{
			if (Game1.player.ActiveObject != null)
			{
				Game1.shipObject((Object)Game1.player.ActiveObject.getOne());
				Game1.playSound("Ship");
				return;
			}
			if (Game1.player.numberOfItemsInInventory() > 0)
			{
				Game1.currentLocation.createQuestionDialogue(Game1.parseText("Empty the contents of your backpack into the shipping box?"), Game1.currentLocation.createYesNoResponses(), "Shipping");
				return;
			}
			Game1.drawObjectDialogue("Place anything you want to sell in here.");
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0008C9D4 File Offset: 0x0008ABD4
		public static void sellShippedItems()
		{
			int itemOfTheDay = Utility.getRandomItemFromSeason(Game1.currentSeason, 0, false);
			int cropOfTheWeek = Game1.cropsOfTheWeek[(Game1.dayOfMonth - 1) / 7];
			for (int i = Game1.shippingBin.Count - 1; i >= 0; i--)
			{
				int price = Game1.shippingBin[i].Price * Game1.shippingBin[i].Stack;
				if (Game1.shippingBin[i].ParentSheetIndex == itemOfTheDay)
				{
					price = (int)((float)price * 1.2f);
				}
				if (Game1.shippingBin[i].ParentSheetIndex == cropOfTheWeek)
				{
					price = (int)((float)price * 1.1f);
				}
				if (Game1.shippingBin[i].Name.Contains("="))
				{
					price += price / 2;
					Game1.stats.StarLevelCropsShipped += (uint)Game1.shippingBin[i].Stack;
				}
				if (Game1.shippingBin[i].category == -75)
				{
					Game1.stats.CropsShipped += (uint)Game1.shippingBin[i].Stack;
				}
				price -= price % 5;
				Game1.player.Money += (Game1.shippingTax ? ((int)((float)price * 0.97f)) : price);
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0008CB19 File Offset: 0x0008AD19
		public static void showNameSelectScreen(string type)
		{
			Game1.nameSelectType = type;
			Game1.nameSelectUp = true;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00002834 File Offset: 0x00000A34
		public static void nameSelectionDone()
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0008CB28 File Offset: 0x0008AD28
		public static void tryToBuySelectedItems()
		{
			if (Game1.selectedItemsType.Equals("flutePitch"))
			{
				Game1.currentObjectDialogue.Clear();
				Game1.currentLocation.actionObjectForQuestionDialogue.scale.X = (float)(Game1.numberOfSelectedItems * 100);
				Game1.dialogueUp = false;
				Game1.player.CanMove = true;
				Game1.numberOfSelectedItems = -1;
				return;
			}
			if (Game1.selectedItemsType.Equals("drumTone"))
			{
				Game1.currentObjectDialogue.Clear();
				Game1.currentLocation.actionObjectForQuestionDialogue.scale.X = (float)Game1.numberOfSelectedItems;
				Game1.dialogueUp = false;
				Game1.player.CanMove = true;
				Game1.numberOfSelectedItems = -1;
				return;
			}
			if (Game1.selectedItemsType.Equals("jukebox"))
			{
				Game1.changeMusicTrack(Game1.player.songsHeard.ElementAt(Game1.numberOfSelectedItems));
				Game1.dialogueUp = false;
				Game1.player.CanMove = true;
				Game1.numberOfSelectedItems = -1;
				return;
			}
			if (Game1.player.Money >= Game1.priceOfSelectedItem * Game1.numberOfSelectedItems && Game1.numberOfSelectedItems > 0)
			{
				bool success = true;
				string a = Game1.selectedItemsType;
				if (!(a == "Animal Food"))
				{
					if (!(a == "Fuel"))
					{
						if (a == "Star Token")
						{
							Game1.player.festivalScore += Game1.numberOfSelectedItems;
							Game1.dialogueUp = false;
							Game1.player.canMove = true;
						}
					}
					else
					{
						((Lantern)Game1.player.getToolFromName("Lantern")).fuelLeft += Game1.numberOfSelectedItems;
					}
				}
				else
				{
					Game1.player.Feed += Game1.numberOfSelectedItems;
					Game1.setDialogue("Thank you!", false);
				}
				if (success)
				{
					Game1.player.Money -= Game1.priceOfSelectedItem * Game1.numberOfSelectedItems;
					Game1.numberOfSelectedItems = -1;
					Game1.playSound("purchase");
					return;
				}
			}
			else if (Game1.player.Money < Game1.priceOfSelectedItem * Game1.numberOfSelectedItems)
			{
				Game1.currentObjectDialogue.Dequeue();
				Game1.setDialogue("You don't have enough money to buy that.", false);
				Game1.numberOfSelectedItems = -1;
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0008CD40 File Offset: 0x0008AF40
		public static void throwActiveObjectDown()
		{
			Game1.player.CanMove = false;
			switch (Game1.player.FacingDirection)
			{
			case 0:
				((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(80, 50f);
				break;
			case 1:
				((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(72, 50f);
				break;
			case 2:
				((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(64, 50f);
				break;
			case 3:
				((FarmerSprite)Game1.player.Sprite).animateBackwardsOnce(88, 50f);
				break;
			}
			Game1.player.reduceActiveItemByOne();
			Game1.playSound("throwDownITem");
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0008CE04 File Offset: 0x0008B004
		public static void changeMusicTrack(string newTrackName)
		{
			if (!Game1.player.songsHeard.Contains(newTrackName))
			{
				Utility.farmerHeardSong(newTrackName);
			}
			if (Game1.currentSong == null || Game1.currentSong.IsStopped || !Game1.currentSong.Name.Equals(newTrackName))
			{
				Game1.nextMusicTrack = newTrackName;
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0008CE54 File Offset: 0x0008B054
		public static void doneEating()
		{
			Game1.isEating = false;
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.player.forceCanMove();
			if (Game1.player.mostRecentlyGrabbedItem == null)
			{
				return;
			}
			Object consumed = Game1.player.itemToEat as Object;
			if (consumed.ParentSheetIndex == 434)
			{
				if (Utility.foundAllStardrops())
				{
					Game1.getSteamAchievement("Achievement_Stardrop");
				}
				Game1.player.yOffset = 0f;
				Game1.player.yJumpOffset = 0;
				Game1.changeMusicTrack("none");
				Game1.playSound("stardrop");
				string mid = (Game1.random.NextDouble() < 0.5) ? "Your mind is filled with thoughts of " : "It's strange, but the taste reminds you of ";
				if (Game1.player.favoriteThing.Contains("Stardew"))
				{
					mid = "You feel an unwavering connection to the valley itself.";
				}
				if (Game1.player.favoriteThing.Equals("ConcernedApe"))
				{
					mid = "Your mind is filled with thoughts of... ConcernedApe? (Well, thanks!)";
				}
				DelayedAction.showDialogueAfterDelay("You found a =stardrop! " + mid + Game1.player.favoriteThing + ". ^^Your maximum energy level has increased.", 6000);
				Game1.player.MaxStamina += 34;
				Game1.player.Stamina = (float)Game1.player.MaxStamina;
				Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
				{
					new FarmerSprite.AnimationFrame(57, 6000)
				});
				Game1.player.startGlowing(new Color(200, 0, 255), false, 0.1f);
				Game1.player.jitterStrength = 1f;
				Game1.staminaShakeTimer = 12000;
				Game1.screenGlowOnce(new Color(200, 0, 255), true, 0.005f, 0.3f);
				Game1.player.CanMove = false;
				Game1.player.freezePause = 8000;
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 60f, 8, 40, Game1.player.position + new Vector2((float)(-(float)Game1.pixelZoom * 2), (float)(-(float)Game1.tileSize * 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0.0075f, 0f, 0f, false)
				{
					alpha = 0.75f,
					alphaFade = 0.0025f,
					motion = new Vector2(0f, -0.25f)
				});
				for (int i = 0; i < 40; i++)
				{
					Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.random.Next(10, 12), new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right - 48 - Game1.tileSize / 8 - Game1.random.Next(Game1.tileSize)), (float)(Game1.random.Next(-Game1.tileSize, Game1.tileSize) + Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - 224 - Game1.tileSize / 4 - (int)((double)(Game1.player.MaxStamina - 270) * 0.715))), (Game1.random.NextDouble() < 0.5) ? Color.White : Color.Lime, 8, false, 50f, 0, -1, -1f, -1, 0)
					{
						layerDepth = 1f,
						delayBeforeAnimationStart = 200 * i,
						interval = 100f,
						local = true
					});
				}
				Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 9, 9, 6000, 100, new Color(200, 0, 255), null, true);
				DelayedAction.stopFarmerGlowing(6000);
				Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 9, 9, 6000, 300, Color.Cyan, null, true);
				Game1.player.mostRecentlyGrabbedItem = null;
			}
			else
			{
				string[] objectDescription = Game1.objectInformation[consumed.ParentSheetIndex].Split(new char[]
				{
					'/'
				});
				if (Convert.ToInt32(objectDescription[2]) > 0)
				{
					string[] arg_4DE_0;
					if (objectDescription.Length <= 6)
					{
						string[] expr_465 = new string[12];
						expr_465[0] = "0";
						expr_465[1] = "0";
						expr_465[2] = "0";
						expr_465[3] = "0";
						expr_465[4] = "0";
						expr_465[5] = "0";
						expr_465[6] = "0";
						expr_465[7] = "0";
						expr_465[8] = "0";
						expr_465[9] = "0";
						expr_465[10] = "0";
						arg_4DE_0 = expr_465;
						expr_465[11] = "0";
					}
					else
					{
						arg_4DE_0 = objectDescription[6].Split(new char[]
						{
							' '
						});
					}
					string[] whatToBuff = arg_4DE_0;
					if (objectDescription.Length > 5 && objectDescription[5].Equals("drink"))
					{
						if (Game1.buffsDisplay.tryToAddDrinkBuff(new Buff(Convert.ToInt32(whatToBuff[0]), Convert.ToInt32(whatToBuff[1]), Convert.ToInt32(whatToBuff[2]), Convert.ToInt32(whatToBuff[3]), Convert.ToInt32(whatToBuff[4]), Convert.ToInt32(whatToBuff[5]), Convert.ToInt32(whatToBuff[6]), Convert.ToInt32(whatToBuff[7]), Convert.ToInt32(whatToBuff[8]), Convert.ToInt32(whatToBuff[9]), Convert.ToInt32(whatToBuff[10]), (whatToBuff.Length > 10) ? Convert.ToInt32(whatToBuff[10]) : 0, (objectDescription.Length > 7) ? Convert.ToInt32(objectDescription[7]) : -1, objectDescription[0])))
						{
						}
					}
					else if (Convert.ToInt32(objectDescription[2]) > 0)
					{
						Game1.buffsDisplay.tryToAddFoodBuff(new Buff(Convert.ToInt32(whatToBuff[0]), Convert.ToInt32(whatToBuff[1]), Convert.ToInt32(whatToBuff[2]), Convert.ToInt32(whatToBuff[3]), Convert.ToInt32(whatToBuff[4]), Convert.ToInt32(whatToBuff[5]), Convert.ToInt32(whatToBuff[6]), Convert.ToInt32(whatToBuff[7]), Convert.ToInt32(whatToBuff[8]), Convert.ToInt32(whatToBuff[9]), Convert.ToInt32(whatToBuff[10]), (whatToBuff.Length > 11) ? Convert.ToInt32(whatToBuff[11]) : 0, (objectDescription.Length > 7) ? Convert.ToInt32(objectDescription[7]) : -1, objectDescription[0]), Math.Min(120000, (int)((float)Convert.ToInt32(objectDescription[2]) / 20f * 30000f)));
					}
				}
				float oldStam = Game1.player.Stamina;
				int oldHealth = Game1.player.health;
				int staminaToHeal = (int)Math.Ceiling((double)consumed.Edibility * 2.5) + consumed.quality * consumed.Edibility;
				Game1.player.Stamina = Math.Min((float)Game1.player.MaxStamina, Game1.player.Stamina + (float)staminaToHeal);
				Game1.player.health = Math.Min(Game1.player.maxHealth, Game1.player.health + ((consumed.Edibility < 0) ? 0 : ((int)((float)staminaToHeal * 0.45f))));
				if (oldStam < Game1.player.Stamina)
				{
					Game1.addHUDMessage(new HUDMessage("+" + (int)(Game1.player.Stamina - oldStam) + " Energy", 4));
				}
				if (oldHealth < Game1.player.health)
				{
					Game1.addHUDMessage(new HUDMessage("+" + (Game1.player.health - oldHealth) + " Health", 5));
				}
			}
			if (consumed.Edibility < 0)
			{
				Game1.player.CanMove = false;
				((FarmerSprite)Game1.player.Sprite).animateOnce(224, 350f, 4);
				Game1.player.doEmote(12);
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0008D62E File Offset: 0x0008B82E
		public static void enterMine(bool isEnteringFromTopFloor, int whatLevel, string forceLevelType)
		{
			Game1.inMine = true;
			if (Game1.mine == null)
			{
				Game1.mine = new MineShaft();
			}
			Game1.mine.setNextLevel(whatLevel);
			Game1.warpFarmer("UndergroundMine", 6, 6, 2);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0008D65F File Offset: 0x0008B85F
		public static void getSteamAchievement(string which)
		{
			if (Program.buildType == 0)
			{
				if (which.Equals("0"))
				{
					which = "a0";
				}
				SteamHelper.getAchievement(which);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0008D684 File Offset: 0x0008B884
		public static void getAchievement(int which)
		{
			if (!Game1.player.achievements.Contains(which) && Game1.gameMode == 3)
			{
				Dictionary<int, string> achievementData = Game1.content.Load<Dictionary<int, string>>("Data\\Achievements");
				if (achievementData.ContainsKey(which))
				{
					string arg_8B_0 = achievementData[which].Split(new char[]
					{
						'^'
					})[0];
					Game1.player.achievements.Add(which);
					Game1.playSound("achievement");
					if (Program.buildType == 0 && SteamHelper.active)
					{
						SteamHelper.getAchievement(string.Concat(which));
					}
					Game1.addHUDMessage(new HUDMessage(arg_8B_0, true));
					if (!Game1.player.hasOrWillReceiveMail("hatter"))
					{
						Game1.addMailForTomorrow("hatter", false, false);
					}
				}
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0008D744 File Offset: 0x0008B944
		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number)
		{
			for (int i = 0; i < number; i++)
			{
				Game1.createObjectDebris(index, xTile, yTile, -1, 0, 1f, null);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0008D770 File Offset: 0x0008B970
		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, float velocityMultiplier)
		{
			for (int i = 0; i < number; i++)
			{
				Game1.createObjectDebris(index, xTile, yTile, -1, 0, velocityMultiplier, null);
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0008D798 File Offset: 0x0008B998
		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, long who)
		{
			for (int i = 0; i < number; i++)
			{
				Game1.createObjectDebris(index, xTile, yTile, who);
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0008D7BC File Offset: 0x0008B9BC
		public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, long who, GameLocation location)
		{
			for (int i = 0; i < number; i++)
			{
				Game1.createObjectDebris(index, xTile, yTile, who, location);
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0008D7E4 File Offset: 0x0008B9E4
		public static void createDebris(int debrisType, int xTile, int yTile, int numberOfChunks, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			location.debris.Add(new Debris(debrisType, numberOfChunks, new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize / 2), (float)(yTile * Game1.tileSize + Game1.tileSize / 2)), new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY())));
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0008D850 File Offset: 0x0008BA50
		public static void createItemDebris(Item item, Vector2 origin, int direction, GameLocation location = null)
		{
			if (location == null)
			{
				location = Game1.currentLocation;
			}
			if (Game1.IsMultiplayer)
			{
				short expr_26 = (short)Game1.random.Next(-32768, 32767);
				Game1.recentMultiplayerRandom = new Random((int)expr_26);
				MultiplayerUtility.broadcastDebrisCreate(expr_26, origin, direction, item, Game1.player.uniqueMultiplayerID);
			}
			Vector2 targetLocation = new Vector2(origin.X, origin.Y);
			switch (direction)
			{
			case -1:
				targetLocation = Game1.player.getStandingPosition();
				break;
			case 0:
				origin.X -= (float)(Game1.tileSize / 2);
				origin.Y -= (float)(Game1.tileSize * 2 + Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2));
				targetLocation.Y -= (float)(Game1.tileSize * 3);
				break;
			case 1:
				origin.X += (float)(Game1.tileSize * 2 / 3);
				origin.Y -= (float)(Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
				targetLocation.X += (float)(Game1.tileSize * 4);
				break;
			case 2:
				origin.X -= (float)(Game1.tileSize / 2);
				origin.Y += (float)Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2);
				targetLocation.Y += (float)(Game1.tileSize * 3 / 2);
				break;
			case 3:
				origin.X -= (float)Game1.tileSize;
				origin.Y -= (float)(Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
				targetLocation.X -= (float)(Game1.tileSize * 4);
				break;
			}
			location.debris.Add(new Debris(item, origin, targetLocation));
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0008DA28 File Offset: 0x0008BC28
		public static void createRadialDebris(GameLocation location, int debrisType, int xTile, int yTile, int numberOfChunks, bool resource, int groundLevel = -1, bool item = false, int color = -1)
		{
			if (groundLevel == -1)
			{
				groundLevel = yTile * Game1.tileSize + Game1.tileSize / 2;
			}
			Vector2 debrisOrigin = new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize), (float)(yTile * Game1.tileSize + Game1.tileSize));
			if (item)
			{
				while (numberOfChunks > 0)
				{
					switch (Game1.random.Next(4))
					{
					case 0:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2((float)(-(float)Game1.tileSize), 0f)));
						break;
					case 1:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2((float)Game1.tileSize, 0f)));
						break;
					case 2:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(0f, (float)Game1.tileSize)));
						break;
					case 3:
						location.debris.Add(new Debris(new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(0f, (float)(-(float)Game1.tileSize))));
						break;
					}
					numberOfChunks--;
				}
			}
			if (resource)
			{
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)(-(float)Game1.tileSize), 0f)));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)Game1.tileSize, 0f)));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)(-(float)Game1.tileSize))));
				numberOfChunks++;
				location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)Game1.tileSize)));
				return;
			}
			location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)(-(float)Game1.tileSize), 0f), groundLevel, color));
			numberOfChunks++;
			location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)Game1.tileSize, 0f), groundLevel, color));
			numberOfChunks++;
			location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)(-(float)Game1.tileSize)), groundLevel, color));
			numberOfChunks++;
			location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)Game1.tileSize), groundLevel, color));
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0008DD26 File Offset: 0x0008BF26
		public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xTile, int yTile, int numberOfChunks)
		{
			Game1.createRadialDebris(location, texture, sourcerectangle, xTile, yTile, numberOfChunks, yTile);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0008DD38 File Offset: 0x0008BF38
		public static void createWaterDroplets(Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile)
		{
			Vector2 debrisOrigin = new Vector2((float)xPosition, (float)yPosition);
			Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)(-(float)Game1.tileSize), 0f), groundLevelTile * Game1.tileSize));
			Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)Game1.tileSize, 0f), groundLevelTile * Game1.tileSize));
			Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)(-(float)Game1.tileSize)), groundLevelTile * Game1.tileSize));
			Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)Game1.tileSize), groundLevelTile * Game1.tileSize));
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0008DE38 File Offset: 0x0008C038
		public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xTile, int yTile, int numberOfChunks, int groundLevelTile)
		{
			Game1.createRadialDebris(location, texture, sourcerectangle, 8, xTile * Game1.tileSize + Game1.tileSize / 2 + Game1.random.Next(Game1.tileSize / 2), yTile * Game1.tileSize + Game1.tileSize / 2 + Game1.random.Next(Game1.tileSize / 2), numberOfChunks, groundLevelTile);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0008DE98 File Offset: 0x0008C098
		public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile)
		{
			Vector2 debrisOrigin = new Vector2((float)xPosition, (float)yPosition);
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)(-(float)Game1.tileSize), 0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float)Game1.tileSize, 0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)(-(float)Game1.tileSize)), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
			location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0f, (float)Game1.tileSize), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0008DF8C File Offset: 0x0008C18C
		public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile, Color color)
		{
			Game1.createRadialDebris(location, texture, sourcerectangle, sizeOfSourceRectSquares, xPosition, yPosition, numberOfChunks, groundLevelTile, color, 1f);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0008DFB4 File Offset: 0x0008C1B4
		public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile, Color color, float scale)
		{
			Vector2 debrisOrigin = new Vector2((float)xPosition, (float)yPosition);
			while (numberOfChunks > 0)
			{
				switch (Game1.random.Next(4))
				{
				case 0:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float)(-(float)Game1.tileSize), 0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor = color;
					location.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				case 1:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float)Game1.tileSize, 0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor = color;
					location.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				case 2:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float)Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float)(-(float)Game1.tileSize)), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor = color;
					location.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				case 3:
				{
					Debris d = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float)Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float)Game1.tileSize), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
					d.nonSpriteChunkColor = color;
					location.debris.Add(d);
					d.Chunks[0].scale = scale;
					break;
				}
				}
				numberOfChunks--;
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0008E178 File Offset: 0x0008C378
		public static void createObjectDebris(int objectIndex, int xTile, int yTile, long whichPlayer)
		{
			Game1.currentLocation.debris.Add(new Debris(objectIndex, new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize / 2), (float)(yTile * Game1.tileSize + Game1.tileSize / 2)), Game1.getFarmer(whichPlayer).getStandingPosition()));
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0008E1CC File Offset: 0x0008C3CC
		public static void createObjectDebris(int objectIndex, int xTile, int yTile, long whichPlayer, GameLocation location)
		{
			location.debris.Add(new Debris(objectIndex, new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize / 2), (float)(yTile * Game1.tileSize + Game1.tileSize / 2)), Game1.getFarmer(whichPlayer).getStandingPosition()));
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0008E21C File Offset: 0x0008C41C
		public static void createObjectDebris(int objectIndex, int xTile, int yTile, int groundLevel = -1, int itemQuality = 0, float velocityMultiplyer = 1f, GameLocation location = null)
		{
			Debris d = new Debris(objectIndex, new Vector2((float)(xTile * Game1.tileSize + Game1.tileSize / 2), (float)(yTile * Game1.tileSize + Game1.tileSize / 2)), new Vector2((float)Game1.player.getStandingX(), (float)Game1.player.getStandingY()))
			{
				itemQuality = itemQuality
			};
			foreach (Chunk expr_64 in d.Chunks)
			{
				expr_64.xVelocity *= velocityMultiplyer;
				expr_64.yVelocity *= velocityMultiplyer;
			}
			if (groundLevel != -1)
			{
				d.chunkFinalYLevel = groundLevel;
			}
			((location == null) ? Game1.currentLocation : location).debris.Add(d);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0008E2F8 File Offset: 0x0008C4F8
		public static Farmer getFarmer(long id)
		{
			if (Game1.player.uniqueMultiplayerID == id)
			{
				return Game1.player;
			}
			foreach (Farmer f in Game1.otherFarmers.Values)
			{
				if (f.uniqueMultiplayerID == id)
				{
					return f;
				}
			}
			return null;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0008E36C File Offset: 0x0008C56C
		public static List<Farmer> getAllFarmers()
		{
			List<Farmer> farmers = new List<Farmer>();
			farmers.Add(Game1.player);
			if (Game1.otherFarmers != null)
			{
				farmers.AddRange(Game1.otherFarmers.Values);
			}
			return farmers;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0008E3A4 File Offset: 0x0008C5A4
		public static void farmerFindsArtifact(int objectIndex)
		{
			Game1.player.addItemToInventoryBool(new Object(objectIndex, 1, false, -1, 0), false);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0008E3C8 File Offset: 0x0008C5C8
		public static void addHUDMessage(HUDMessage message)
		{
			if (message.type != null || message.whatType != 0)
			{
				for (int i = 0; i < Game1.hudMessages.Count; i++)
				{
					if (message.type != null && Game1.hudMessages[i].type != null && Game1.hudMessages[i].type.Equals(message.type) && Game1.hudMessages[i].add == message.add)
					{
						Game1.hudMessages[i].number = (message.add ? (Game1.hudMessages[i].number + message.number) : (Game1.hudMessages[i].number - message.number));
						Game1.hudMessages[i].timeLeft = 3500f;
						Game1.hudMessages[i].transparency = 1f;
						return;
					}
					if (message.whatType == Game1.hudMessages[i].whatType && message.whatType != 1 && message.message != null && message.message.Equals(Game1.hudMessages[i].message))
					{
						Game1.hudMessages[i].timeLeft = message.timeLeft;
						Game1.hudMessages[i].transparency = 1f;
						return;
					}
				}
			}
			Game1.hudMessages.Add(message);
			for (int j = Game1.hudMessages.Count - 1; j >= 0; j--)
			{
				if (Game1.hudMessages[j].noIcon)
				{
					HUDMessage tmp = Game1.hudMessages[j];
					Game1.hudMessages.RemoveAt(j);
					Game1.hudMessages.Add(tmp);
				}
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0008E596 File Offset: 0x0008C796
		public static void nextMineLevel()
		{
			Game1.warpFarmer("UndergroundMine", 16, 16, false);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00002834 File Offset: 0x00000A34
		public static void swordswipe(int direction, float animationSpeed, bool flip)
		{
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0008E5A8 File Offset: 0x0008C7A8
		public static void showSwordswipeAnimation(int direction, Vector2 source, float animationSpeed, bool flip)
		{
			switch (direction)
			{
			case 0:
				Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + (float)(Game1.tileSize / 2), source.Y), false, false, !flip, -1.57079637f));
				return;
			case 1:
				Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + (float)(Game1.tileSize * 3 / 2) + (float)(Game1.tileSize / 4), source.Y + (float)(Game1.tileSize * 3 / 4)), false, flip, false, flip ? -3.14159274f : 0f));
				return;
			case 2:
				Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + (float)(Game1.tileSize / 2), source.Y + (float)(Game1.tileSize * 2)), false, false, !flip, 1.57079637f));
				return;
			case 3:
				Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X - (float)(Game1.tileSize / 2) - (float)(Game1.tileSize / 4), source.Y + (float)(Game1.tileSize * 3 / 4)), false, !flip, false, flip ? -3.14159274f : 0f));
				return;
			default:
				return;
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0008E708 File Offset: 0x0008C908
		public static void farmerTakeDamage(int damage, bool overrideParry, Monster damager)
		{
			if (damager != null && !damager.isInvincible() && !overrideParry && Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && ((MeleeWeapon)Game1.player.CurrentTool).isOnSpecial && ((MeleeWeapon)Game1.player.CurrentTool).type == 3)
			{
				Rumble.rumble(0.75f, 150f);
				Game1.playSound("parry");
				float oldXVel = damager.xVelocity;
				float oldYVel = damager.yVelocity;
				if (damager.xVelocity != 0f || damager.yVelocity != 0f)
				{
					Game1.currentLocation.damageMonster(damager.GetBoundingBox(), damage / 2, damage / 2 + 1, false, 0f, 0, 0f, 0f, false, Game1.player);
				}
				damager.xVelocity = -oldXVel;
				damager.yVelocity = -oldYVel;
				damager.xVelocity *= (damager.isGlider ? 2f : 3.5f);
				damager.yVelocity *= (damager.isGlider ? 2f : 3.5f);
				bool arg_136_0 = damager.isGlider;
				damager.setInvincibleCountdown(450);
				return;
			}
			if (!Game1.player.temporarilyInvincible && (damager == null || !damager.isInvincible()) && !Game1.isEating && !Game1.fadeToBlack && !Game1.buffsDisplay.hasBuff(21))
			{
				if (damager != null && (damager is GreenSlime || damager is BigSlime) && Game1.player.isWearingRing(520))
				{
					return;
				}
				if (Game1.player.isWearingRing(524) && !Game1.buffsDisplay.hasBuff(21) && Game1.random.NextDouble() < (0.9 - (double)((float)Game1.player.health / 100f)) / (double)(3 - Game1.player.LuckLevel / 10) + ((Game1.player.health <= 15) ? 0.2 : 0.0))
				{
					Game1.playSound("yoba");
					Game1.buffsDisplay.addOtherBuff(new Buff(21));
					return;
				}
				Rumble.rumble(0.75f, 150f);
				damage += Game1.random.Next(Math.Min(-1, -damage / 8), Math.Max(1, damage / 8));
				damage = Math.Max(1, damage - Game1.player.resilience);
				Game1.player.health = Math.Max(0, Game1.player.health - damage);
				Game1.player.temporarilyInvincible = true;
				Game1.currentLocation.debris.Add(new Debris(damage, new Vector2((float)(Game1.player.getStandingX() + 8), (float)Game1.player.getStandingY()), Color.Red, 1f, Game1.player));
				Game1.playSound("ow");
				Game1.hitShakeTimer = 100 * damage;
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0008EA1C File Offset: 0x0008CC1C
		public static void removeSquareDebrisFromTile(int tileX, int tileY)
		{
			for (int i = Game1.currentLocation.debris.Count - 1; i >= 0; i--)
			{
				if (Game1.currentLocation.debris[i].debrisType == Debris.DebrisType.SQUARES && (int)(Game1.currentLocation.debris[i].Chunks[0].position.X / (float)Game1.tileSize) == tileX && Game1.currentLocation.debris[i].chunkFinalYLevel / Game1.tileSize == tileY)
				{
					Game1.currentLocation.debris.RemoveAt(i);
				}
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0008EABC File Offset: 0x0008CCBC
		public static void removeDebris(Debris.DebrisType type)
		{
			for (int i = Game1.currentLocation.debris.Count - 1; i >= 0; i--)
			{
				if (Game1.currentLocation.debris[i].debrisType == type)
				{
					Game1.currentLocation.debris.RemoveAt(i);
				}
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0008EB0D File Offset: 0x0008CD0D
		public static void toolAnimationDone()
		{
			Game1.toolAnimationDone(Game1.player);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0008EB1C File Offset: 0x0008CD1C
		public static void toolAnimationDone(Farmer who)
		{
			float oldStamina = Game1.player.Stamina;
			if (who.CurrentTool == null)
			{
				return;
			}
			if (who.Stamina > 0f)
			{
				int powerupLevel = (int)((Game1.toolHold + 20f) / 600f) + 1;
				Vector2 actionTile = who.GetToolLocation(false);
				if (who.CurrentTool.GetType() == typeof(FishingRod) && ((FishingRod)who.CurrentTool).isFishing)
				{
					who.canReleaseTool = false;
				}
				else if (who.CurrentTool.GetType() != typeof(FishingRod))
				{
					who.UsingTool = false;
					if (who.CurrentTool.Name.Contains("Seeds"))
					{
						if (!Game1.eventUp)
						{
							who.CurrentTool.DoFunction(Game1.currentLocation, who.getStandingX(), who.getStandingY(), powerupLevel, who);
							if (((Seeds)who.CurrentTool).NumberInStack <= 0)
							{
								who.removeItemFromInventory(who.CurrentTool);
							}
						}
					}
					else if (who.CurrentTool.Name.Equals("Watering Can"))
					{
						switch (who.FacingDirection)
						{
						case 0:
						case 2:
							who.CurrentTool.DoFunction(Game1.currentLocation, (int)actionTile.X, (int)actionTile.Y, powerupLevel, who);
							break;
						case 1:
						case 3:
							who.CurrentTool.DoFunction(Game1.currentLocation, (int)actionTile.X, (int)actionTile.Y, powerupLevel, who);
							break;
						}
					}
					else if (who.CurrentTool is MeleeWeapon)
					{
						who.CurrentTool.CurrentParentTileIndex = who.CurrentTool.indexOfMenuItemView;
					}
					else
					{
						if (who.CurrentTool.Name.Equals("Wand"))
						{
							who.CurrentTool.CurrentParentTileIndex = who.CurrentTool.indexOfMenuItemView;
						}
						who.CurrentTool.DoFunction(Game1.currentLocation, (int)actionTile.X, (int)actionTile.Y, powerupLevel, who);
					}
				}
				else
				{
					who.usingTool = false;
				}
			}
			else if (who.CurrentTool.instantUse)
			{
				who.CurrentTool.DoFunction(Game1.currentLocation, 0, 0, 0, who);
			}
			else
			{
				who.UsingTool = false;
			}
			who.lastClick = Vector2.Zero;
			Game1.toolHold = 0f;
			if (who.IsMainPlayer && !Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			{
				who.setRunning(Game1.options.autoRun, false);
			}
			if (!who.UsingTool)
			{
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).CurrentFrame = 16;
					break;
				case 1:
					((FarmerSprite)who.Sprite).CurrentFrame = 8;
					break;
				case 2:
					((FarmerSprite)who.Sprite).CurrentFrame = 0;
					break;
				case 3:
					((FarmerSprite)who.Sprite).CurrentFrame = 24;
					break;
				}
			}
			if (Game1.player.Stamina <= 0f && oldStamina > 0f)
			{
				Game1.player.doEmote(36);
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0008EE38 File Offset: 0x0008D038
		public static bool pressActionButton(KeyboardState currentKBState, MouseState currentMouseState, GamePadState currentPadState)
		{
			if (Game1.dialogueTyping)
			{
				bool consume = true;
				Game1.dialogueTyping = false;
				if (Game1.currentSpeaker != null)
				{
					Game1.currentDialogueCharacterIndex = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length;
				}
				else if (Game1.currentObjectDialogue.Count > 0)
				{
					Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length;
				}
				else
				{
					consume = false;
				}
				Game1.dialogueTypingInterval = 0;
				Game1.oldKBState = currentKBState;
				Game1.oldMouseState = currentMouseState;
				Game1.oldPadState = currentPadState;
				if (consume)
				{
					Game1.playSound("dialogueCharacterClose");
					return false;
				}
			}
			if (Game1.dialogueUp && Game1.numberOfSelectedItems == -1)
			{
				if (Game1.isQuestion)
				{
					Game1.isQuestion = false;
					if (Game1.currentSpeaker != null)
					{
						if (Game1.currentSpeaker.CurrentDialogue.Peek().chooseResponse(Game1.questionChoices[Game1.currentQuestionChoice]))
						{
							Game1.currentDialogueCharacterIndex = 1;
							Game1.dialogueTyping = true;
							Game1.oldKBState = currentKBState;
							Game1.oldMouseState = currentMouseState;
							Game1.oldPadState = currentPadState;
							return false;
						}
					}
					else
					{
						Game1.dialogueUp = false;
						if (Game1.eventUp)
						{
							Game1.currentLocation.currentEvent.answerDialogue(Game1.currentLocation.lastQuestionKey, Game1.currentQuestionChoice);
							Game1.currentQuestionChoice = 0;
							Game1.oldKBState = currentKBState;
							Game1.oldMouseState = currentMouseState;
							Game1.oldPadState = currentPadState;
						}
						else if (Game1.currentLocation.answerDialogue(Game1.questionChoices[Game1.currentQuestionChoice]))
						{
							Game1.currentQuestionChoice = 0;
							Game1.oldKBState = currentKBState;
							Game1.oldMouseState = currentMouseState;
							Game1.oldPadState = currentPadState;
							return false;
						}
						if (Game1.dialogueUp)
						{
							Game1.currentDialogueCharacterIndex = 1;
							Game1.dialogueTyping = true;
							Game1.oldKBState = currentKBState;
							Game1.oldMouseState = currentMouseState;
							Game1.oldPadState = currentPadState;
							return false;
						}
					}
					Game1.currentQuestionChoice = 0;
				}
				string exitDialogue = null;
				if (Game1.currentSpeaker != null)
				{
					if (Game1.currentSpeaker.immediateSpeak)
					{
						Game1.currentSpeaker.immediateSpeak = false;
						return false;
					}
					exitDialogue = ((Game1.currentSpeaker.CurrentDialogue.Count > 0) ? Game1.currentSpeaker.CurrentDialogue.Peek().exitCurrentDialogue() : null);
				}
				if (exitDialogue == null)
				{
					if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0 && Game1.currentSpeaker.CurrentDialogue.Peek().isOnFinalDialogue() && Game1.currentSpeaker.CurrentDialogue.Count > 0)
					{
						Game1.currentSpeaker.CurrentDialogue.Pop();
					}
					Game1.dialogueUp = false;
					if (Game1.messagePause)
					{
						Game1.pauseTime = 500f;
					}
					if (Game1.currentObjectDialogue.Count > 0)
					{
						Game1.currentObjectDialogue.Dequeue();
					}
					Game1.currentDialogueCharacterIndex = 0;
					if (Game1.currentObjectDialogue.Count > 0)
					{
						Game1.dialogueUp = true;
						Game1.questionChoices.Clear();
						Game1.oldKBState = currentKBState;
						Game1.oldMouseState = currentMouseState;
						Game1.oldPadState = currentPadState;
						Game1.dialogueTyping = true;
						return false;
					}
					Game1.tvStation = -1;
					if (Game1.currentSpeaker != null && !Game1.currentSpeaker.name.Equals("Gunther") && !Game1.eventUp && !Game1.currentSpeaker.doingEndOfRouteAnimation)
					{
						Game1.currentSpeaker.doneFacingPlayer(Game1.player);
					}
					Game1.currentSpeaker = null;
					if (!Game1.eventUp)
					{
						Game1.player.CanMove = true;
					}
					else if (Game1.currentLocation.currentEvent.CurrentCommand > 0 || Game1.currentLocation.currentEvent.specialEventVariable1)
					{
						if (!Game1.isFestival() || !Game1.currentLocation.currentEvent.canMoveAfterDialogue())
						{
							Event expr_352 = Game1.currentLocation.currentEvent;
							int currentCommand = expr_352.CurrentCommand;
							expr_352.CurrentCommand = currentCommand + 1;
						}
						else
						{
							Game1.player.CanMove = true;
						}
					}
					Game1.questionChoices.Clear();
					Game1.playSound("smallSelect");
				}
				else
				{
					Game1.playSound("smallSelect");
					Game1.currentDialogueCharacterIndex = 0;
					Game1.dialogueTyping = true;
					Game1.checkIfDialogueIsQuestion();
				}
				Game1.oldKBState = currentKBState;
				Game1.oldMouseState = currentMouseState;
				Game1.oldPadState = currentPadState;
				if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && Game1.questOfTheDay.GetType().Name.Equals("SocializeQuest"))
				{
					((SocializeQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
				}
				Game1.afterFadeFunction arg_3F9_0 = Game1.afterDialogues;
				return false;
			}
			if (Game1.currentBillboard != 0)
			{
				Game1.currentBillboard = 0;
				Game1.player.CanMove = true;
				Game1.oldKBState = currentKBState;
				Game1.oldMouseState = currentMouseState;
				Game1.oldPadState = currentPadState;
				return false;
			}
			if (!Game1.player.UsingTool && !Game1.pickingTool && !Game1.menuUp && (!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence) && !Game1.nameSelectUp && Game1.numberOfSelectedItems == -1 && !Game1.fadeToBlack)
			{
				Vector2 grabTile = new Vector2((float)(Game1.getOldMouseX() + Game1.viewport.X), (float)(Game1.getOldMouseY() + Game1.viewport.Y)) / (float)Game1.tileSize;
				if (Game1.mouseCursorTransparency == 0f || (!Game1.lastCursorMotionWasMouse && (Game1.player.ActiveObject == null || (!Game1.player.ActiveObject.isPlaceable() && Game1.player.ActiveObject.category != -74))))
				{
					grabTile = Game1.player.GetGrabTile();
					if (grabTile.Equals(Game1.player.getTileLocation()))
					{
						grabTile = Utility.getTranslatedVector2(grabTile, Game1.player.facingDirection, 1f);
					}
				}
				if (!Utility.tileWithinRadiusOfPlayer((int)grabTile.X, (int)grabTile.Y, 1, Game1.player))
				{
					grabTile = Game1.player.GetGrabTile();
					if (grabTile.Equals(Game1.player.getTileLocation()) && Game1.isAnyGamePadButtonBeingPressed())
					{
						grabTile = Utility.getTranslatedVector2(grabTile, Game1.player.facingDirection, 1f);
					}
				}
				if (Game1.eventUp && !Game1.isFestival())
				{
					Game1.currentLocation.currentEvent.receiveActionPress((int)grabTile.X, (int)grabTile.Y);
					Game1.oldKBState = currentKBState;
					Game1.oldMouseState = currentMouseState;
					Game1.oldPadState = currentPadState;
					return false;
				}
				if (Game1.tryToCheckAt(grabTile, Game1.player))
				{
					return false;
				}
				if (Game1.player.isRidingHorse())
				{
					Game1.player.getMount().checkAction(Game1.player, Game1.player.currentLocation);
					return false;
				}
				if (!Game1.player.canMove)
				{
					return false;
				}
				bool tmp = false;
				if (Game1.player.ActiveObject != null && !(Game1.player.ActiveObject is Furniture))
				{
					if (Game1.player.ActiveObject.performUseAction())
					{
						Game1.player.reduceActiveItemByOne();
						Game1.oldKBState = currentKBState;
						Game1.oldMouseState = currentMouseState;
						Game1.oldPadState = currentPadState;
						return false;
					}
					int stack = Game1.player.ActiveObject.Stack;
					Utility.tryToPlaceItem(Game1.currentLocation, Game1.player.ActiveObject, (int)grabTile.X * Game1.tileSize + Game1.tileSize / 2, (int)grabTile.Y * Game1.tileSize + Game1.tileSize / 2);
					if (Game1.player.ActiveObject == null || Game1.player.ActiveObject.Stack < stack)
					{
						tmp = true;
					}
				}
				if (!tmp)
				{
					grabTile.Y += 1f;
					if (Game1.tryToCheckAt(grabTile, Game1.player))
					{
						return false;
					}
					if (Game1.player.ActiveObject != null && Game1.player.ActiveObject is Furniture)
					{
						(Game1.player.ActiveObject as Furniture).rotate();
						Game1.playSound("dwoop");
						Game1.oldKBState = currentKBState;
						Game1.oldMouseState = currentMouseState;
						Game1.oldPadState = currentPadState;
						return false;
					}
				}
				if (!Game1.isEating && Game1.player.ActiveObject != null && !Game1.dialogueUp && !Game1.eventUp && !Game1.player.canOnlyWalk && !Game1.player.FarmerSprite.pauseForSingleAnimation && !Game1.fadeToBlack && Game1.player.ActiveObject.Edibility != -300)
				{
					Game1.player.faceDirection(2);
					Game1.isEating = true;
					Game1.player.itemToEat = Game1.player.ActiveObject;
					Game1.player.FarmerSprite.setCurrentSingleAnimation(304);
					Game1.currentLocation.createQuestionDialogue(string.Concat(new string[]
					{
						(Game1.objectInformation[Game1.player.ActiveObject.parentSheetIndex].Split(new char[]
						{
							'/'
						}).Length > 5 && Game1.objectInformation[Game1.player.ActiveObject.parentSheetIndex].Split(new char[]
						{
							'/'
						})[5].Equals("drink")) ? "Drink " : "Eat ",
						(Game1.player.ActiveObject.stack > 1) ? Game1.getProperArticleForWord(Game1.player.ActiveObject.name) : "this",
						" ",
						Game1.player.ActiveObject.name,
						"?"
					}), Game1.currentLocation.createYesNoResponses(), "Eat");
					Game1.oldKBState = currentKBState;
					Game1.oldMouseState = currentMouseState;
					Game1.oldPadState = currentPadState;
					return false;
				}
			}
			else if (Game1.numberOfSelectedItems != -1)
			{
				Game1.tryToBuySelectedItems();
				Game1.playSound("smallSelect");
				Game1.oldKBState = currentKBState;
				Game1.oldMouseState = currentMouseState;
				Game1.oldPadState = currentPadState;
				return false;
			}
			if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && Game1.player.CanMove && !Game1.player.canOnlyWalk && !Game1.eventUp)
			{
				((MeleeWeapon)Game1.player.CurrentTool).animateSpecialMove(Game1.player);
			}
			return true;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0008F7BC File Offset: 0x0008D9BC
		public static bool tryToCheckAt(Vector2 grabTile, Farmer who)
		{
			Game1.haltAfterCheck = true;
			if (Utility.tileWithinRadiusOfPlayer((int)grabTile.X, (int)grabTile.Y, 1, Game1.player) && Game1.currentLocation.checkAction(new Location((int)grabTile.X, (int)grabTile.Y), Game1.viewport, who))
			{
				Game1.updateCursorTileHint();
				who.lastGrabTile = grabTile;
				if (who.CanMove && Game1.haltAfterCheck)
				{
					who.faceGeneralDirection(grabTile * (float)Game1.tileSize, 0);
					who.Halt();
				}
				MultiplayerUtility.broadcastCheckAction((int)grabTile.X, (int)grabTile.Y, who.uniqueMultiplayerID, Game1.currentLocation.name);
				Game1.oldKBState = Keyboard.GetState();
				Game1.oldMouseState = Mouse.GetState();
				Game1.oldPadState = GamePad.GetState(PlayerIndex.One);
				return true;
			}
			return false;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0008F88C File Offset: 0x0008DA8C
		public static void pressSwitchToolButton()
		{
			int whichWay = (Mouse.GetState().ScrollWheelValue > Game1.oldMouseState.ScrollWheelValue) ? -1 : ((Mouse.GetState().ScrollWheelValue < Game1.oldMouseState.ScrollWheelValue) ? 1 : 0);
			if (Game1.options.gamepadControls && whichWay == 0)
			{
				whichWay = (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftTrigger) ? -1 : 1);
			}
			if (Game1.options.invertScrollDirection)
			{
				whichWay *= -1;
			}
			Game1.player.CurrentToolIndex = (Game1.player.CurrentToolIndex + whichWay) % 12;
			if (Game1.player.CurrentToolIndex < 0)
			{
				Game1.player.CurrentToolIndex = 11;
			}
			int i = 0;
			while (i < 12 && Game1.player.CurrentItem == null)
			{
				Game1.player.CurrentToolIndex = (whichWay + Game1.player.CurrentToolIndex) % 12;
				if (Game1.player.CurrentToolIndex < 0)
				{
					Game1.player.CurrentToolIndex = 11;
				}
				i++;
			}
			Game1.playSound("toolSwap");
			if (Game1.player.ActiveObject != null)
			{
				Game1.player.showCarrying();
			}
			else
			{
				Game1.player.showNotCarrying();
			}
			if (Game1.player.CurrentTool != null && !Game1.player.CurrentTool.Name.Equals("Seeds") && !Game1.player.CurrentTool.Name.Contains("Sword") && !Game1.player.CurrentTool.instantUse)
			{
				Game1.player.CurrentTool.CurrentParentTileIndex = Game1.player.CurrentTool.CurrentParentTileIndex - Game1.player.CurrentTool.CurrentParentTileIndex % 8 + 2;
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0008FA3C File Offset: 0x0008DC3C
		public static void switchToolAnimation()
		{
			Game1.pickToolInterval = 0f;
			Game1.player.CanMove = false;
			Game1.pickingTool = true;
			Game1.playSound("toolSwap");
			switch (Game1.player.FacingDirection)
			{
			case 0:
				Game1.player.FarmerSprite.setCurrentFrame(196);
				break;
			case 1:
				Game1.player.FarmerSprite.setCurrentFrame(194);
				break;
			case 2:
				Game1.player.FarmerSprite.setCurrentFrame(192);
				break;
			case 3:
				Game1.player.FarmerSprite.setCurrentFrame(198);
				break;
			}
			if (Game1.player.CurrentTool != null && !Game1.player.CurrentTool.Name.Equals("Seeds") && !Game1.player.CurrentTool.Name.Contains("Sword") && !Game1.player.CurrentTool.instantUse)
			{
				Game1.player.CurrentTool.CurrentParentTileIndex = Game1.player.CurrentTool.CurrentParentTileIndex - Game1.player.CurrentTool.CurrentParentTileIndex % 8 + 2;
			}
			if (Game1.player.ActiveObject != null)
			{
				Game1.player.showCarrying();
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0008FB84 File Offset: 0x0008DD84
		public static bool pressUseToolButton()
		{
			if (Game1.fadeToBlack)
			{
				return false;
			}
			Game1.player.toolPower = 0;
			Game1.player.toolHold = 0;
			if (Game1.player.CurrentTool == null && Game1.player.ActiveObject == null)
			{
				Vector2 c = Game1.player.GetToolLocation(false) / (float)Game1.tileSize;
				c.X = (float)((int)c.X);
				c.Y = (float)((int)c.Y);
				if (Game1.currentLocation.Objects.ContainsKey(c))
				{
					Object o = Game1.currentLocation.Objects[c];
					if (!o.readyForHarvest && o.heldObject == null && !(o is Fence) && !(o is CrabPot) && o.type != null && (o.type.Equals("Crafting") || o.type.Equals("interactive")) && !o.name.Equals("Twig"))
					{
						o.setHealth(o.getHealth() - 1);
						o.shakeTimer = 300;
						Game1.playSound("hammer");
						if (o.getHealth() < 2)
						{
							Game1.playSound("hammer");
							if (o.getHealth() < 1)
							{
								Tool t = new Pickaxe();
								t.DoFunction(Game1.currentLocation, -1, -1, 0, Game1.player);
								if (o.performToolAction(t))
								{
									o.performRemoveAction(o.tileLocation, Game1.currentLocation);
									if (o.type.Equals("Crafting") && o.fragility != 2)
									{
										Game1.currentLocation.debris.Add(new Debris(o.bigCraftable ? (-o.ParentSheetIndex) : o.ParentSheetIndex, Game1.player.GetToolLocation(false), new Vector2((float)Game1.player.GetBoundingBox().Center.X, (float)Game1.player.GetBoundingBox().Center.Y)));
									}
									Game1.currentLocation.Objects.Remove(c);
									return true;
								}
							}
						}
					}
				}
			}
			if (Game1.currentMinigame == null && !Game1.player.usingTool && (Game1.player.isRidingHorse() || Game1.dialogueUp || (Game1.eventUp && !Game1.CurrentEvent.canPlayerUseTool() && (!Game1.currentLocation.currentEvent.playerControlSequence || (Game1.activeClickableMenu == null && Game1.currentMinigame == null))) || (Game1.player.CurrentTool != null && Game1.currentLocation.doesPositionCollideWithCharacter(Utility.getRectangleCenteredAt(Game1.player.GetToolLocation(false), Game1.tileSize), true) != null && Game1.currentLocation.doesPositionCollideWithCharacter(Utility.getRectangleCenteredAt(Game1.player.GetToolLocation(false), Game1.tileSize), true).isVillager())))
			{
				Game1.pressActionButton(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(PlayerIndex.One));
				return false;
			}
			if (Game1.player.canOnlyWalk)
			{
				return true;
			}
			Vector2 position = (Game1.mouseCursorTransparency == 0f) ? Game1.player.GetToolLocation(false) : new Vector2((float)(Game1.getOldMouseX() + Game1.viewport.X), (float)(Game1.getOldMouseY() + Game1.viewport.Y));
			if (Utility.canGrabSomethingFromHere((int)position.X, (int)position.Y, Game1.player))
			{
				Vector2 tile = new Vector2((float)((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize), (float)((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize));
				if (Game1.currentLocation.checkAction(new Location((int)tile.X, (int)tile.Y), Game1.viewport, Game1.player))
				{
					Game1.updateCursorTileHint();
					return true;
				}
				if (Game1.currentLocation.terrainFeatures.ContainsKey(tile))
				{
					Game1.currentLocation.terrainFeatures[tile].performUseAction(tile);
					return true;
				}
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastCheckAction((int)tile.X, (int)tile.Y, Game1.player.uniqueMultiplayerID, Game1.currentLocation.name);
				}
				return false;
			}
			else
			{
				if (Game1.currentLocation.leftClick((int)position.X, (int)position.Y, Game1.player))
				{
					return true;
				}
				if (Game1.player.ActiveObject != null)
				{
					if (Utility.withinRadiusOfPlayer((int)position.X, (int)position.Y, 1, Game1.player) && Game1.currentLocation.checkAction(new Location((int)position.X / Game1.tileSize, (int)position.Y / Game1.tileSize), Game1.viewport, Game1.player))
					{
						if (Game1.IsMultiplayer)
						{
							MultiplayerUtility.broadcastCheckAction((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize, (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize, Game1.player.uniqueMultiplayerID, Game1.currentLocation.name);
						}
						return true;
					}
					Utility.tryToPlaceItem(Game1.currentLocation, Game1.player.ActiveObject, (int)position.X, (int)position.Y);
				}
				if (Game1.player.UsingTool)
				{
					Game1.player.lastClick = new Vector2((float)((int)position.X), (float)((int)position.Y));
					Game1.player.CurrentTool.DoFunction(Game1.player.currentLocation, (int)Game1.player.lastClick.X, (int)Game1.player.lastClick.Y, 1, Game1.player);
					return true;
				}
				if (Game1.player.ActiveObject == null && !Game1.isEating && Game1.player.CurrentTool != null)
				{
					if (Game1.player.Stamina <= 20f && Game1.player.CurrentTool != null && !(Game1.player.CurrentTool is MeleeWeapon))
					{
						Game1.staminaShakeTimer = 1000;
						for (int i = 0; i < 4; i++)
						{
							Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(366, 412, 5, 6), new Vector2((float)(Game1.random.Next(Game1.tileSize / 2) + Game1.viewport.Width - (48 + Game1.tileSize / 8)), (float)(Game1.viewport.Height - 224 - Game1.tileSize / 4 - (int)((double)(Game1.player.MaxStamina - 270) * 0.715))), false, 0.012f, Color.SkyBlue)
							{
								motion = new Vector2(-2f, -10f),
								acceleration = new Vector2(0f, 0.5f),
								local = true,
								scale = (float)(Game1.pixelZoom + Game1.random.Next(-1, 0)),
								delayBeforeAnimationStart = i * 30
							});
						}
					}
					Game1.player.CanMove = false;
					Game1.player.UsingTool = true;
					Game1.player.canReleaseTool = true;
					if (Utility.withinRadiusOfPlayer((int)position.X, (int)position.Y, 1, Game1.player) && (Game1.player.CurrentTool is WateringCan || Math.Abs(position.X - (float)Game1.player.getStandingX()) >= (float)(Game1.tileSize / 2) || Math.Abs(position.Y - (float)Game1.player.getStandingY()) >= (float)(Game1.tileSize / 2)))
					{
						Game1.player.Halt();
						if (Game1.mouseCursorTransparency != 0f && !Game1.isAnyGamePadButtonBeingHeld())
						{
							Game1.player.faceGeneralDirection(new Vector2((float)((int)position.X), (float)((int)position.Y)), 0);
						}
						Game1.player.lastClick = new Vector2((float)((int)position.X), (float)((int)position.Y));
					}
					bool result;
					try
					{
						Game1.player.CurrentTool.beginUsing(Game1.currentLocation, (int)Game1.player.lastClick.X, (int)Game1.player.lastClick.Y, Game1.player);
						result = false;
						return result;
					}
					catch (Exception)
					{
					}
					if (!Game1.player.CurrentTool.instantUse)
					{
						Game1.player.Halt();
						Game1.player.CurrentTool.Update(Game1.player.FacingDirection, 0, Game1.player);
						if ((!(Game1.player.CurrentTool is FishingRod) && Game1.player.CurrentTool.upgradeLevel <= 0 && !(Game1.player.CurrentTool is MeleeWeapon)) || Game1.player.CurrentTool is Pickaxe)
						{
							Game1.releaseUseToolButton();
							return false;
						}
					}
					if (Game1.player.CurrentTool.Name.Equals("Wand"))
					{
						if (!((Wand)Game1.player.CurrentTool).charged)
						{
							Game1.drawObjectDialogue(Game1.parseText("Nothing happens. Sandy said something about 'infusing it with the power of Yoba'."));
							Game1.player.UsingTool = false;
							Game1.player.canReleaseTool = false;
							return false;
						}
						Game1.toolAnimationDone();
						Game1.player.canReleaseTool = false;
						if (!Game1.fadeToBlack)
						{
							Game1.player.CanMove = true;
							Game1.player.UsingTool = false;
							return false;
						}
						return false;
					}
					else
					{
						if (Game1.player.CurrentTool.instantUse)
						{
							Game1.toolAnimationDone();
							Game1.player.canReleaseTool = false;
							Game1.player.UsingTool = false;
							return false;
						}
						if (Game1.player.CurrentTool.Name.Equals("Seeds"))
						{
							switch (Game1.player.FacingDirection)
							{
							case 0:
								Game1.player.Sprite.CurrentFrame = 208;
								Game1.player.CurrentTool.Update(0, 0);
								return false;
							case 1:
								Game1.player.Sprite.CurrentFrame = 204;
								Game1.player.CurrentTool.Update(1, 0);
								return false;
							case 2:
								Game1.player.Sprite.CurrentFrame = 200;
								Game1.player.CurrentTool.Update(2, 0);
								return false;
							case 3:
								Game1.player.Sprite.CurrentFrame = 212;
								Game1.player.CurrentTool.Update(3, 0);
								return false;
							default:
								return false;
							}
						}
						else
						{
							if (Game1.player.CurrentTool is WateringCan && Game1.currentLocation.doesTileHaveProperty(((int)Game1.player.GetToolLocation(false).X + Game1.tileSize / 2) / Game1.tileSize, (int)Game1.player.GetToolLocation(false).Y / Game1.tileSize, "Water", "Back") != null)
							{
								switch (Game1.player.FacingDirection)
								{
								case 0:
									((FarmerSprite)Game1.player.Sprite).animateOnce(182, 250f, 2);
									Game1.player.CurrentTool.Update(0, 1);
									break;
								case 1:
									((FarmerSprite)Game1.player.Sprite).animateOnce(174, 250f, 2);
									Game1.player.CurrentTool.Update(1, 0);
									break;
								case 2:
									((FarmerSprite)Game1.player.Sprite).animateOnce(166, 250f, 2);
									Game1.player.CurrentTool.Update(2, 1);
									break;
								case 3:
									((FarmerSprite)Game1.player.Sprite).animateOnce(190, 250f, 2);
									Game1.player.CurrentTool.Update(3, 0);
									break;
								}
								Game1.player.canReleaseTool = false;
								return false;
							}
							if (Game1.player.CurrentTool is WateringCan && ((WateringCan)Game1.player.CurrentTool).WaterLeft <= 0)
							{
								Game1.toolAnimationDone();
								Game1.player.CanMove = true;
								Game1.player.canReleaseTool = false;
								return false;
							}
							if (Game1.player.CurrentTool is WateringCan)
							{
								Game1.player.jitterStrength = 0.25f;
								switch (Game1.player.FacingDirection)
								{
								case 0:
									Game1.player.FarmerSprite.setCurrentFrame(180);
									Game1.player.CurrentTool.Update(0, 0);
									return false;
								case 1:
									Game1.player.FarmerSprite.setCurrentFrame(172);
									Game1.player.CurrentTool.Update(1, 0);
									return false;
								case 2:
									Game1.player.FarmerSprite.setCurrentFrame(164);
									Game1.player.CurrentTool.Update(2, 0);
									return false;
								case 3:
									Game1.player.FarmerSprite.setCurrentFrame(188);
									Game1.player.CurrentTool.Update(3, 0);
									return false;
								default:
									return false;
								}
							}
							else
							{
								if (Game1.player.CurrentTool is FishingRod)
								{
									switch (Game1.player.FacingDirection)
									{
									case 0:
										((FarmerSprite)Game1.player.Sprite).animateOnce(295, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
										Game1.player.CurrentTool.Update(0, 0);
										break;
									case 1:
										((FarmerSprite)Game1.player.Sprite).animateOnce(296, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
										Game1.player.CurrentTool.Update(1, 0);
										break;
									case 2:
										((FarmerSprite)Game1.player.Sprite).animateOnce(297, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
										Game1.player.CurrentTool.Update(2, 0);
										break;
									case 3:
										((FarmerSprite)Game1.player.Sprite).animateOnce(298, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
										Game1.player.CurrentTool.Update(3, 0);
										break;
									}
									Game1.player.canReleaseTool = false;
									return false;
								}
								if (Game1.player.CurrentTool is MeleeWeapon)
								{
									((MeleeWeapon)Game1.player.CurrentTool).setFarmerAnimating(Game1.player);
									return false;
								}
								switch (Game1.player.FacingDirection)
								{
								case 0:
									Game1.player.FarmerSprite.setCurrentFrame(176);
									Game1.player.CurrentTool.Update(0, 0);
									return false;
								case 1:
									Game1.player.FarmerSprite.setCurrentFrame(168);
									Game1.player.CurrentTool.Update(1, 0);
									return false;
								case 2:
									Game1.player.FarmerSprite.setCurrentFrame(160);
									Game1.player.CurrentTool.Update(2, 0);
									return false;
								case 3:
									Game1.player.FarmerSprite.setCurrentFrame(184);
									Game1.player.CurrentTool.Update(3, 0);
									return false;
								default:
									return false;
								}
							}
						}
					}
					return result;
				}
				return false;
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00090AD0 File Offset: 0x0008ECD0
		public static void releaseUseToolButton()
		{
			Game1.player.stopJittering();
			Game1.player.canReleaseTool = false;
			int addedAnimationMultiplayer = (Game1.player.Stamina <= 0f) ? 2 : 1;
			if (Game1.isAnyGamePadButtonBeingPressed())
			{
				Game1.player.lastClick = Game1.player.GetToolLocation(false);
			}
			if (Game1.player.CurrentTool.Name.Equals("Seeds"))
			{
				switch (Game1.player.FacingDirection)
				{
				case 0:
					((FarmerSprite)Game1.player.Sprite).animateOnce(208, 150f, 4);
					return;
				case 1:
					((FarmerSprite)Game1.player.Sprite).animateOnce(204, 150f, 4);
					return;
				case 2:
					((FarmerSprite)Game1.player.Sprite).animateOnce(200, 150f, 4);
					return;
				case 3:
					((FarmerSprite)Game1.player.Sprite).animateOnce(212, 150f, 4);
					return;
				default:
					return;
				}
			}
			else
			{
				if (!(Game1.player.CurrentTool is WateringCan))
				{
					if (Game1.player.CurrentTool.GetType() == typeof(FishingRod) && Game1.activeClickableMenu == null)
					{
						if (!(Game1.player.CurrentTool as FishingRod).hit)
						{
							Game1.player.CurrentTool.DoFunction(Game1.player.currentLocation, (int)Game1.player.lastClick.X, (int)Game1.player.lastClick.Y, 1, Game1.player);
							return;
						}
					}
					else
					{
						Game1.player.FarmerSprite.nextOffset = 0;
						switch (Game1.player.FacingDirection)
						{
						case 0:
							((FarmerSprite)Game1.player.Sprite).animateOnce(176, 60f * (float)addedAnimationMultiplayer, 8);
							return;
						case 1:
							((FarmerSprite)Game1.player.Sprite).animateOnce(168, 60f * (float)addedAnimationMultiplayer, 8);
							return;
						case 2:
							((FarmerSprite)Game1.player.Sprite).animateOnce(160, 60f * (float)addedAnimationMultiplayer, 8);
							return;
						case 3:
							((FarmerSprite)Game1.player.Sprite).animateOnce(184, 60f * (float)addedAnimationMultiplayer, 8);
							break;
						default:
							return;
						}
					}
					return;
				}
				if ((Game1.player.CurrentTool as WateringCan).WaterLeft > 0)
				{
					Game1.playSound("wateringCan");
				}
				switch (Game1.player.FacingDirection)
				{
				case 0:
					((FarmerSprite)Game1.player.Sprite).animateOnce(180, 125f * (float)addedAnimationMultiplayer, 3);
					return;
				case 1:
					((FarmerSprite)Game1.player.Sprite).animateOnce(172, 125f * (float)addedAnimationMultiplayer, 3);
					return;
				case 2:
					((FarmerSprite)Game1.player.Sprite).animateOnce(164, 125f * (float)addedAnimationMultiplayer, 3);
					return;
				case 3:
					((FarmerSprite)Game1.player.Sprite).animateOnce(188, 125f * (float)addedAnimationMultiplayer, 3);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00090E04 File Offset: 0x0008F004
		public static int getMouseX()
		{
			return (int)((float)Mouse.GetState().X * (1f / Game1.options.zoomLevel));
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00090E31 File Offset: 0x0008F031
		public static int getOldMouseX()
		{
			return (int)((float)Game1.oldMouseState.X * (1f / Game1.options.zoomLevel));
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00090E50 File Offset: 0x0008F050
		public static int getMouseY()
		{
			return (int)((float)Mouse.GetState().Y * (1f / Game1.options.zoomLevel));
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00090E7D File Offset: 0x0008F07D
		public static int getOldMouseY()
		{
			return (int)((float)Game1.oldMouseState.Y * (1f / Game1.options.zoomLevel));
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00002834 File Offset: 0x00000A34
		public static void pressAddItemToInventoryButton()
		{
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00090E9C File Offset: 0x0008F09C
		public static int numberOfPlayers()
		{
			if (Game1.IsServer)
			{
				return Game1.otherFarmers.Count + 1;
			}
			if (!Game1.IsMultiplayer)
			{
				return 1;
			}
			return Game1.otherFarmers.Count;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00090EC5 File Offset: 0x0008F0C5
		public static bool isFestival()
		{
			return Game1.currentLocation != null && Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.isFestival;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00090EEC File Offset: 0x0008F0EC
		public void parseDebugInput(string debugInput)
		{
			Game1.exitActiveMenu();
			Game1.lastDebugInput = debugInput;
			debugInput = debugInput.Trim();
			string[] debugSplit = debugInput.Split(new char[]
			{
				' '
			});
			try
			{
				if (Game1.panMode)
				{
					if (debugSplit[0].Equals("exit") || debugSplit[0].ToLower().Equals("panmode"))
					{
						Game1.panMode = false;
						Game1.viewportFreeze = false;
						this.panModeString = "";
						Game1.debugMode = false;
						Game1.debugOutput = "";
						this.panFacingDirectionWait = false;
					}
					else if (debugSplit[0].Equals("clear"))
					{
						this.panModeString = "";
						Game1.debugOutput = "";
						this.panFacingDirectionWait = false;
					}
					else if (!this.panFacingDirectionWait)
					{
						int time = 0;
						if (int.TryParse(debugSplit[0], out time))
						{
							this.panModeString = string.Concat(new object[]
							{
								this.panModeString,
								(this.panModeString.Length > 0) ? "/" : "",
								time,
								" "
							});
							Game1.debugOutput = this.panModeString + " ...Waiting for tile location click...";
						}
					}
				}
				else
				{
					string text = debugSplit[0];
					uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
					if (num <= 2066479103u)
					{
						if (num > 1145980326u)
						{
							if (num > 1547417870u)
							{
								if (num <= 1735922541u)
								{
									if (num <= 1644312386u)
									{
										if (num <= 1581757135u)
										{
											if (num <= 1564253156u)
											{
												if (num != 1553132714u)
												{
													if (num != 1564253156u)
													{
														goto IL_6415;
													}
													if (!(text == "time"))
													{
														goto IL_6415;
													}
													Game1.timeOfDay = Convert.ToInt32(debugSplit[1]);
													Game1.outdoorLight = Color.White;
													goto IL_6415;
												}
												else
												{
													if (!(text == "removeLargeTF"))
													{
														goto IL_6415;
													}
													Game1.currentLocation.largeTerrainFeatures.Clear();
													goto IL_6415;
												}
											}
											else if (num != 1573653271u)
											{
												if (num != 1581757135u)
												{
													goto IL_6415;
												}
												if (!(text == "wc"))
												{
													goto IL_6415;
												}
												goto IL_5F7B;
											}
											else
											{
												if (!(text == "speech"))
												{
													goto IL_6415;
												}
												Game1.getCharacterFromName(debugSplit[1], false).CurrentDialogue.Push(new Dialogue(debugInput.Substring(debugInput.IndexOf("0") + 1), Game1.getCharacterFromName(debugSplit[1], false)));
												Game1.drawDialogue(Game1.getCharacterFromName(debugSplit[1], false));
												goto IL_6415;
											}
										}
										else if (num <= 1629900385u)
										{
											if (num != 1582198420u)
											{
												if (num != 1629900385u)
												{
													goto IL_6415;
												}
												if (!(text == "skullkey"))
												{
													goto IL_6415;
												}
												Game1.player.hasSkullKey = true;
												goto IL_6415;
											}
											else
											{
												if (!(text == "ps"))
												{
													goto IL_6415;
												}
												goto IL_4EA0;
											}
										}
										else if (num != 1638190062u)
										{
											if (num != 1644312386u)
											{
												goto IL_6415;
											}
											if (!(text == "kms"))
											{
												goto IL_6415;
											}
											goto IL_2F28;
										}
										else
										{
											if (!(text == "fruitTrees"))
											{
												goto IL_6415;
											}
											using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = Game1.currentLocation.terrainFeatures.GetEnumerator())
											{
												while (enumerator.MoveNext())
												{
													KeyValuePair<Vector2, TerrainFeature> t = enumerator.Current;
													if (t.Value is FruitTree)
													{
														(t.Value as FruitTree).daysUntilMature -= 27;
														t.Value.dayUpdate(Game1.currentLocation, t.Key);
													}
												}
												goto IL_6415;
											}
										}
									}
									else if (num <= 1684539335u)
									{
										if (num <= 1665100777u)
										{
											if (num != 1646454850u)
											{
												if (num != 1665100777u)
												{
													goto IL_6415;
												}
												if (!(text == "jn"))
												{
													goto IL_6415;
												}
												goto IL_486F;
											}
											else
											{
												if (!(text == "fo"))
												{
													goto IL_6415;
												}
												goto IL_3404;
											}
										}
										else if (num != 1680451373u)
										{
											if (num != 1684539335u)
											{
												goto IL_6415;
											}
											if (!(text == "nosave"))
											{
												goto IL_6415;
											}
											goto IL_331D;
										}
										else
										{
											if (!(text == "cm"))
											{
												goto IL_6415;
											}
											goto IL_5CD5;
										}
									}
									else if (num <= 1697114278u)
									{
										if (num != 1697079146u)
										{
											if (num != 1697114278u)
											{
												goto IL_6415;
											}
											if (!(text == "upgradecoop"))
											{
												goto IL_6415;
											}
											goto IL_5ABE;
										}
										else
										{
											if (!(text == "toggleCatPerson"))
											{
												goto IL_6415;
											}
											Game1.player.catPerson = !Game1.player.catPerson;
											goto IL_6415;
										}
									}
									else if (num != 1730342945u)
									{
										if (num != 1735922541u)
										{
											goto IL_6415;
										}
										if (!(text == "animalInfo"))
										{
											goto IL_6415;
										}
										Game1.showGlobalMessage(string.Concat(Game1.getFarm().getAllFarmAnimals().Count));
										goto IL_6415;
									}
									else
									{
										if (!(text == "fb"))
										{
											goto IL_6415;
										}
										goto IL_35C3;
									}
								}
								else if (num <= 1840121496u)
								{
									if (num <= 1787721130u)
									{
										if (num <= 1763898183u)
										{
											if (num != 1758887219u)
											{
												if (num != 1763898183u)
												{
													goto IL_6415;
												}
												if (!(text == "fd"))
												{
													goto IL_6415;
												}
												goto IL_6356;
											}
											else
											{
												if (!(text == "pole"))
												{
													goto IL_6415;
												}
												Game1.player.addItemToInventoryBool(new FishingRod((debugSplit.Length > 1) ? Convert.ToInt32(debugSplit[1]) : 0), false);
												goto IL_6415;
											}
										}
										else if (num != 1769407897u)
										{
											if (num != 1787721130u)
											{
												goto IL_6415;
											}
											if (!(text == "end"))
											{
												goto IL_6415;
											}
											Game1.warpFarmer("Town", 20, 20, false);
											Game1.getLocationFromName("Town").currentEvent = new Event(Utility.getStardewHeroCelebrationEventString(90), -1);
											this.makeCelebrationWeatherDebris();
											Utility.perpareDayForStardewCelebration(90);
											goto IL_6415;
										}
										else
										{
											if (!(text == "pickax"))
											{
												goto IL_6415;
											}
											goto IL_62F5;
										}
									}
									else if (num <= 1811284518u)
									{
										if (num != 1797453421u)
										{
											if (num != 1811284518u)
											{
												goto IL_6415;
											}
											if (!(text == "musicvolume"))
											{
												goto IL_6415;
											}
											goto IL_5615;
										}
										else
										{
											if (!(text == "ff"))
											{
												goto IL_6415;
											}
											goto IL_40A2;
										}
									}
									else if (num != 1828144719u)
									{
										if (num != 1840121496u)
										{
											goto IL_6415;
										}
										if (!(text == "pixelZoom"))
										{
											goto IL_6415;
										}
										Game1.pixelZoom = Convert.ToInt32(debugSplit[1]);
										Game1.tileSize = Game1.pixelZoom * 16;
										Layer.m_tileSize = new Size(Game1.tileSize, Game1.tileSize);
										Projectile.boundingBoxWidth = Game1.tileSize / 4;
										Projectile.boundingBoxHeight = Game1.tileSize / 4;
										GreenSlime.matingRange = Game1.tileSize * 3;
										SliderBar.defaultWidth = Game1.tileSize * 2;
										IClickableMenu.borderWidth = Game1.tileSize / 2 + Game1.tileSize / 8;
										IClickableMenu.tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
										IClickableMenu.spaceToClearTopBorder = Game1.tileSize * 3 / 2;
										IClickableMenu.spaceToClearSideBorder = Game1.tileSize / 4;
										BuffsDisplay.width = Game1.tileSize * 4 + Game1.tileSize / 2;
										BuffsDisplay.sideSpace = Game1.tileSize / 2;
										Farmer.tileSlideThreshold = Game1.tileSize / 2;
										Coop.openAnimalDoorPosition = -Game1.tileSize + Game1.pixelZoom * 3;
										Barn.openAnimalDoorPosition = -Game1.tileSize - 24 + Game1.pixelZoom * 3;
										goto IL_6415;
									}
									else
									{
										if (!(text == "mainMenu"))
										{
											goto IL_6415;
										}
										goto IL_5427;
									}
								}
								else if (num <= 2016676370u)
								{
									if (num <= 1865621569u)
									{
										if (num != 1848911934u)
										{
											if (num != 1865621569u)
											{
												goto IL_6415;
											}
											if (!(text == "weapon"))
											{
												goto IL_6415;
											}
											Game1.player.addItemToInventoryBool(new MeleeWeapon(Convert.ToInt32(debugSplit[1])), false);
											goto IL_6415;
										}
										else
										{
											if (!(text == "darkTalisman"))
											{
												goto IL_6415;
											}
											Game1.player.hasDarkTalisman = true;
											Game1.getLocationFromName("Railroad").setMapTile(54, 35, 287, "Buildings", "", 1);
											Game1.getLocationFromName("Railroad").setMapTile(54, 34, 262, "Front", "", 1);
											Game1.getLocationFromName("WitchHut").setMapTile(4, 11, 114, "Buildings", "", 1);
											Game1.getLocationFromName("WitchHut").setTileProperty(4, 11, "Buildings", "Action", "MagicInk");
											Game1.player.hasMagicInk = false;
											Game1.player.mailReceived.Clear();
											goto IL_6415;
										}
									}
									else if (num != 1883325193u)
									{
										if (num != 2016676370u)
										{
											goto IL_6415;
										}
										if (!(text == "bigitem"))
										{
											goto IL_6415;
										}
										goto IL_5C91;
									}
									else
									{
										if (!(text == "wallpaper"))
										{
											goto IL_6415;
										}
										goto IL_403C;
									}
								}
								else if (num <= 2033203167u)
								{
									if (num != 2018864233u)
									{
										if (num != 2033203167u)
										{
											goto IL_6415;
										}
										if (!(text == "specialItem"))
										{
											goto IL_6415;
										}
										Game1.player.specialItems.Add(Convert.ToInt32(debugSplit[1]));
										goto IL_6415;
									}
									else if (!(text == "train"))
									{
										goto IL_6415;
									}
								}
								else if (num != 2042762978u)
								{
									if (num != 2044027800u)
									{
										if (num != 2066479103u)
										{
											goto IL_6415;
										}
										if (!(text == "wand"))
										{
											goto IL_6415;
										}
										Game1.player.addItemToInventoryBool(new Wand(), false);
										Game1.playSound("coin");
										goto IL_6415;
									}
									else
									{
										if (!(text == "levelup"))
										{
											goto IL_6415;
										}
										Game1.activeClickableMenu = new LevelUpMenu(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]));
										goto IL_6415;
									}
								}
								else
								{
									if (!(text == "morepollen"))
									{
										goto IL_6415;
									}
									for (int i = 0; i < Convert.ToInt32(debugSplit[1]); i++)
									{
										Game1.debrisWeather.Add(new WeatherDebris(new Vector2((float)Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Width), (float)Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Height)), 0, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
									}
									goto IL_6415;
								}
								(Game1.getLocationFromName("Railroad") as Railroad).setTrainComing(7500);
								goto IL_6415;
							}
							if (num <= 1302939849u)
							{
								if (num <= 1198881991u)
								{
									if (num <= 1163008208u)
									{
										if (num <= 1160050994u)
										{
											if (num != 1147755170u)
											{
												if (num != 1160050994u)
												{
													goto IL_6415;
												}
												if (!(text == "aj"))
												{
													goto IL_6415;
												}
												goto IL_47C6;
											}
											else
											{
												if (!(text == "removeDebris"))
												{
													goto IL_6415;
												}
												Game1.currentLocation.debris.Clear();
												goto IL_6415;
											}
										}
										else if (num != 1161685201u)
										{
											if (num != 1163008208u)
											{
												goto IL_6415;
											}
											if (!(text == "sr"))
											{
												goto IL_6415;
											}
											Game1.player.shiftToolbar(true);
											goto IL_6415;
										}
										else
										{
											if (!(text == "facePlayer"))
											{
												goto IL_6415;
											}
											Game1.getCharacterFromName(debugSplit[1], false).faceTowardFarmer = true;
											goto IL_6415;
										}
									}
									else if (num <= 1177961446u)
									{
										if (num != 1174793874u)
										{
											if (num != 1177961446u)
											{
												goto IL_6415;
											}
											if (!(text == "ns"))
											{
												goto IL_6415;
											}
											goto IL_331D;
										}
										else
										{
											if (!(text == "deleteArch"))
											{
												goto IL_6415;
											}
											Game1.player.archaeologyFound.Clear();
											Game1.player.fishCaught.Clear();
											Game1.player.mineralsFound.Clear();
											Game1.player.mailReceived.Clear();
											goto IL_6415;
										}
									}
									else if (num != 1181855383u)
									{
										if (num != 1198881991u)
										{
											goto IL_6415;
										}
										if (!(text == "killMonsterStat"))
										{
											goto IL_6415;
										}
									}
									else
									{
										if (!(text == "version"))
										{
											goto IL_6415;
										}
										Game1.debugOutput = string.Concat(typeof(Game1).Assembly.GetName().Version);
										goto IL_6415;
									}
								}
								else if (num <= 1274636741u)
								{
									if (num <= 1237752336u)
									{
										if (num != 1226617017u)
										{
											if (num != 1237752336u)
											{
												goto IL_6415;
											}
											if (!(text == "water"))
											{
												goto IL_6415;
											}
											using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator2 = Game1.currentLocation.terrainFeatures.Values.GetEnumerator())
											{
												while (enumerator2.MoveNext())
												{
													TerrainFeature t2 = enumerator2.Current;
													if (t2 is HoeDirt)
													{
														(t2 as HoeDirt).state = 1;
													}
												}
												goto IL_6415;
											}
											goto IL_5BCC;
										}
										else
										{
											if (!(text == "dp"))
											{
												goto IL_6415;
											}
											Game1.stats.daysPlayed = (uint)Convert.ToInt32(debugSplit[1]);
											goto IL_6415;
										}
									}
									else if (num != 1266391031u)
									{
										if (num != 1274636741u)
										{
											goto IL_6415;
										}
										if (!(text == "eventOver"))
										{
											goto IL_6415;
										}
										Game1.eventFinished();
										goto IL_6415;
									}
									else
									{
										if (!(text == "wedding"))
										{
											goto IL_6415;
										}
										Game1.player.spouse = debugSplit[1];
										Game1.prepareSpouseForWedding();
										Game1.checkForWedding();
										goto IL_6415;
									}
								}
								else if (num <= 1279593123u)
								{
									if (num != 1278921350u)
									{
										if (num != 1279593123u)
										{
											goto IL_6415;
										}
										if (!(text == "growAnimalsFarm"))
										{
											goto IL_6415;
										}
										goto IL_511E;
									}
									else
									{
										if (!(text == "hu"))
										{
											goto IL_6415;
										}
										goto IL_4108;
									}
								}
								else if (num != 1298661701u)
								{
									if (num != 1302939849u)
									{
										goto IL_6415;
									}
									if (!(text == "seenmail"))
									{
										goto IL_6415;
									}
									Game1.player.mailReceived.Add(debugSplit[1]);
									goto IL_6415;
								}
								else
								{
									if (!(text == "festivalScore"))
									{
										goto IL_6415;
									}
									Game1.player.festivalScore += Convert.ToInt32(debugSplit[1]);
									goto IL_6415;
								}
							}
							else if (num <= 1448470768u)
							{
								if (num <= 1383527311u)
								{
									if (num <= 1331031788u)
									{
										if (num != 1330092850u)
										{
											if (num != 1331031788u)
											{
												goto IL_6415;
											}
											if (!(text == "pan"))
											{
												goto IL_6415;
											}
											Game1.player.addItemToInventoryBool(new Pan(), false);
											goto IL_6415;
										}
										else
										{
											if (!(text == "wp"))
											{
												goto IL_6415;
											}
											goto IL_403C;
										}
									}
									else if (num != 1366625991u)
									{
										if (num != 1383527311u)
										{
											goto IL_6415;
										}
										if (!(text == "panmode"))
										{
											goto IL_6415;
										}
										goto IL_42C9;
									}
									else
									{
										if (!(text == "tool"))
										{
											goto IL_6415;
										}
										Game1.player.getToolFromName(debugSplit[1]).UpgradeLevel = Convert.ToInt32(debugSplit[2]);
										goto IL_6415;
									}
								}
								else if (num <= 1395526040u)
								{
									if (num != 1394937660u)
									{
										if (num != 1395526040u)
										{
											goto IL_6415;
										}
										if (!(text == "mp"))
										{
											goto IL_6415;
										}
										Game1.player.addItemToInventoryBool(new MilkPail(), false);
										goto IL_6415;
									}
									else
									{
										if (!(text == "ax"))
										{
											goto IL_6415;
										}
										Game1.player.addItemToInventoryBool(new Axe(), false);
										Game1.playSound("coin");
										goto IL_6415;
									}
								}
								else if (num != 1405799865u)
								{
									if (num != 1448470768u)
									{
										goto IL_6415;
									}
									if (!(text == "whereis"))
									{
										goto IL_6415;
									}
									goto IL_4254;
								}
								else
								{
									if (!(text == "big"))
									{
										goto IL_6415;
									}
									goto IL_5C91;
								}
							}
							else if (num <= 1490091188u)
							{
								if (num <= 1461503683u)
								{
									if (num != 1457159580u)
									{
										if (num != 1461503683u)
										{
											goto IL_6415;
										}
										if (!(text == "db"))
										{
											goto IL_6415;
										}
										Game1.activeClickableMenu = new DialogueBox(Game1.getCharacterFromName((debugSplit.Length > 1) ? debugSplit[1] : "Pierre", false).CurrentDialogue.Peek());
										goto IL_6415;
									}
									else
									{
										if (!(text == "barn"))
										{
											goto IL_6415;
										}
										goto IL_5AE9;
									}
								}
								else if (num != 1475248161u)
								{
									if (num != 1490091188u)
									{
										goto IL_6415;
									}
									if (!(text == "refuel"))
									{
										goto IL_6415;
									}
									if (Game1.player.getToolFromName("Lantern") != null)
									{
										((Lantern)Game1.player.getToolFromName("Lantern")).fuelLeft = 100;
										goto IL_6415;
									}
									goto IL_6415;
								}
								else
								{
									if (!(text == "frameOffset"))
									{
										goto IL_6415;
									}
									goto IL_3404;
								}
							}
							else if (num <= 1523588291u)
							{
								if (num != 1496191754u)
								{
									if (num != 1523588291u)
									{
										goto IL_6415;
									}
									if (!(text == "emote"))
									{
										goto IL_6415;
									}
									Game1.player.doEmote(Convert.ToInt32(debugSplit[1]));
									goto IL_6415;
								}
								else
								{
									if (!(text == "mv"))
									{
										goto IL_6415;
									}
									goto IL_5615;
								}
							}
							else if (num != 1534898856u)
							{
								if (num != 1546774874u)
								{
									if (num != 1547417870u)
									{
										goto IL_6415;
									}
									if (!(text == "spawnweeds"))
									{
										goto IL_6415;
									}
									for (int j = 0; j < Convert.ToInt32(debugSplit[1]); j++)
									{
										Game1.currentLocation.spawnWeedsAndStones(1, false, true);
									}
									goto IL_6415;
								}
								else
								{
									if (!(text == "lu"))
									{
										goto IL_6415;
									}
									goto IL_4682;
								}
							}
							else
							{
								if (!(text == "readyForHarvest"))
								{
									goto IL_6415;
								}
								goto IL_334F;
							}
							IL_2F28:
							string monster = debugSplit[1].Replace("0", " ");
							int kills = Convert.ToInt32(debugSplit[2]);
							if (Game1.stats.specificMonstersKilled.ContainsKey(monster))
							{
								Game1.stats.specificMonstersKilled[monster] = kills;
							}
							else
							{
								Game1.stats.specificMonstersKilled.Add(monster, kills);
							}
							Game1.debugOutput = monster + " killed: " + kills;
							goto IL_6415;
							IL_3404:
							int modifier = debugSplit[2].Contains('s') ? -1 : 1;
							FarmerRenderer.featureXOffsetPerFrame[Convert.ToInt32(debugSplit[1])] = (int)((short)(modifier * Convert.ToInt32(debugSplit[2].Last<char>().ToString() ?? "")));
							modifier = (debugSplit[3].Contains('s') ? -1 : 1);
							FarmerRenderer.featureYOffsetPerFrame[Convert.ToInt32(debugSplit[1])] = (int)((short)(modifier * Convert.ToInt32(debugSplit[3].Last<char>().ToString() ?? "")));
							if (debugSplit.Length > 4)
							{
								int arg_34A3_0 = debugSplit[4].Contains('s') ? -1 : 1;
								goto IL_6415;
							}
							goto IL_6415;
							IL_403C:
							bool floor = Game1.random.NextDouble() < 0.5;
							Game1.player.addItemToInventoryBool(new Wallpaper(floor ? Game1.random.Next(40) : Game1.random.Next(112), floor), false);
							goto IL_6415;
						}
						if (num <= 654091955u)
						{
							if (num <= 292255708u)
							{
								if (num <= 108289031u)
								{
									if (num <= 60638767u)
									{
										if (num <= 11649459u)
										{
											if (num != 9940504u)
											{
												if (num != 11649459u)
												{
													goto IL_6415;
												}
												if (!(text == "removeObjects"))
												{
													goto IL_6415;
												}
												Game1.currentLocation.objects.Clear();
												goto IL_6415;
											}
											else
											{
												if (!(text == "customize"))
												{
													goto IL_6415;
												}
												goto IL_54BA;
											}
										}
										else if (num != 39742388u)
										{
											if (num != 60638767u)
											{
												goto IL_6415;
											}
											if (!(text == "mainmenu"))
											{
												goto IL_6415;
											}
											goto IL_5427;
										}
										else
										{
											if (!(text == "pickaxe"))
											{
												goto IL_6415;
											}
											goto IL_62F5;
										}
									}
									else if (num <= 90969176u)
									{
										if (num != 68193782u)
										{
											if (num != 90969176u)
											{
												goto IL_6415;
											}
											if (!(text == "where"))
											{
												goto IL_6415;
											}
											goto IL_4254;
										}
										else
										{
											if (!(text == "bloomDay"))
											{
												goto IL_6415;
											}
											Game1.bloomDay = !Game1.bloomDay;
											Game1.bloom.Visible = Game1.bloomDay;
											Game1.bloom.reload();
											goto IL_6415;
										}
									}
									else if (num != 105537319u)
									{
										if (num != 108289031u)
										{
											goto IL_6415;
										}
										if (!(text == "cat"))
										{
											goto IL_6415;
										}
										Game1.currentLocation.characters.Add(new StardewValley.Characters.Cat(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2])));
										goto IL_6415;
									}
									else
									{
										if (!(text == "museumloot"))
										{
											goto IL_6415;
										}
										using (Dictionary<int, string>.Enumerator enumerator3 = Game1.objectInformation.GetEnumerator())
										{
											while (enumerator3.MoveNext())
											{
												KeyValuePair<int, string> v = enumerator3.Current;
												string type = v.Value.Split(new char[]
												{
													'/'
												})[3];
												if ((type.Contains("Arch") || type.Contains("Minerals")) && !Game1.player.mineralsFound.ContainsKey(v.Key) && !Game1.player.archaeologyFound.ContainsKey(v.Key))
												{
													if (type.Contains("Arch"))
													{
														Game1.player.foundArtifact(v.Key, 1);
													}
													else
													{
														Game1.player.addItemToInventoryBool(new Object(v.Key, 1, false, -1, 0), false);
													}
												}
												if (Game1.player.freeSpotsInInventory() == 0)
												{
													break;
												}
											}
											goto IL_6415;
										}
										goto IL_53B7;
									}
								}
								else if (num <= 217793883u)
								{
									if (num <= 164679485u)
									{
										if (num != 114665541u)
										{
											if (num != 164679485u)
											{
												goto IL_6415;
											}
											if (!(text == "quest"))
											{
												goto IL_6415;
											}
											Game1.player.questLog.Add(Quest.getQuestFromId(Convert.ToInt32(debugSplit[1])));
											goto IL_6415;
										}
										else
										{
											if (!(text == "question"))
											{
												goto IL_6415;
											}
											Game1.player.dialogueQuestionsAnswered.Add(Convert.ToInt32(debugSplit[1]));
											goto IL_6415;
										}
									}
									else if (num != 166771958u)
									{
										if (num != 217793883u)
										{
											goto IL_6415;
										}
										if (!(text == "house"))
										{
											goto IL_6415;
										}
										goto IL_4108;
									}
									else if (!(text == "scissors"))
									{
										goto IL_6415;
									}
								}
								else if (num <= 268356225u)
								{
									if (num != 250384600u)
									{
										if (num != 268356225u)
										{
											goto IL_6415;
										}
										if (!(text == "houseUpgrade"))
										{
											goto IL_6415;
										}
										goto IL_4108;
									}
									else
									{
										if (!(text == "growgrass"))
										{
											goto IL_6415;
										}
										Game1.currentLocation.spawnWeeds(false);
										Game1.currentLocation.growWeedGrass(Convert.ToInt32(debugSplit[1]));
										goto IL_6415;
									}
								}
								else if (num != 269790383u)
								{
									if (num != 292255708u)
									{
										goto IL_6415;
									}
									if (!(text == "face"))
									{
										goto IL_6415;
									}
									goto IL_6356;
								}
								else
								{
									if (!(text == "petToFarm"))
									{
										goto IL_6415;
									}
									goto IL_32D6;
								}
							}
							else if (num <= 453405023u)
							{
								if (num <= 360098152u)
								{
									if (num <= 307718101u)
									{
										if (num != 304701189u)
										{
											if (num != 307718101u)
											{
												goto IL_6415;
											}
											if (!(text == "junimoStar"))
											{
												goto IL_6415;
											}
											((Game1.getLocationFromName("CommunityCenter") as CommunityCenter).characters[0] as Junimo).returnToJunimoHutToFetchStar(Game1.getLocationFromName("CommunityCenter") as CommunityCenter);
											goto IL_6415;
										}
										else
										{
											if (!(text == "bundle"))
											{
												goto IL_6415;
											}
											for (int k = 0; k < (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[Convert.ToInt32(debugSplit[1])].Length; k++)
											{
												(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[Convert.ToInt32(debugSplit[1])][k] = true;
											}
											Game1.playSound("crystal");
											goto IL_6415;
										}
									}
									else if (num != 341878775u)
									{
										if (num != 360098152u)
										{
											goto IL_6415;
										}
										if (!(text == "leaveEvent"))
										{
											goto IL_6415;
										}
										goto IL_4D04;
									}
									else
									{
										if (!(text == "cooking"))
										{
											goto IL_6415;
										}
										goto IL_4F0A;
									}
								}
								else if (num <= 417438434u)
								{
									if (num != 408921239u)
									{
										if (num != 417438434u)
										{
											goto IL_6415;
										}
										if (!(text == "stoprafting"))
										{
											goto IL_6415;
										}
										Game1.player.isRafting = false;
										goto IL_6415;
									}
									else
									{
										if (!(text == "growAnimals"))
										{
											goto IL_6415;
										}
										using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = (Game1.currentLocation as AnimalHouse).animals.Values.GetEnumerator())
										{
											while (enumerator4.MoveNext())
											{
												FarmAnimal expr_50EA = enumerator4.Current;
												expr_50EA.ageWhenMature -= 1;
												expr_50EA.dayUpdate(Game1.currentLocation);
											}
											goto IL_6415;
										}
										goto IL_511E;
									}
								}
								else if (num != 445690992u)
								{
									if (num != 453405023u)
									{
										goto IL_6415;
									}
									if (!(text == "shears"))
									{
										goto IL_6415;
									}
								}
								else
								{
									if (!(text == "bluebook"))
									{
										goto IL_6415;
									}
									Game1.player.items.Add(new Blueprints());
									goto IL_6415;
								}
							}
							else if (num <= 501437184u)
							{
								if (num <= 486219809u)
								{
									if (num != 474674083u)
									{
										if (num != 486219809u)
										{
											goto IL_6415;
										}
										if (!(text == "removeTerrainFeatures"))
										{
											goto IL_6415;
										}
										goto IL_5185;
									}
									else
									{
										if (!(text == "caughtFish"))
										{
											goto IL_6415;
										}
										goto IL_4F86;
									}
								}
								else if (num != 499086637u)
								{
									if (num != 501437184u)
									{
										goto IL_6415;
									}
									if (!(text == "pregnant"))
									{
										goto IL_6415;
									}
									Game1.player.getSpouse().daysUntilBirthing = 1;
									Game1.player.getRidOfChildren();
									goto IL_6415;
								}
								else
								{
									if (!(text == "removeFurniture"))
									{
										goto IL_6415;
									}
									(Game1.currentLocation as DecoratableLocation).furniture.Clear();
									goto IL_6415;
								}
							}
							else if (num <= 605731668u)
							{
								if (num != 604705968u)
								{
									if (num != 605731668u)
									{
										goto IL_6415;
									}
									if (!(text == "localInfo"))
									{
										goto IL_6415;
									}
									Game1.debugOutput = "";
									int grass = 0;
									int trees = 0;
									int other = 0;
									foreach (TerrainFeature t3 in Game1.currentLocation.terrainFeatures.Values)
									{
										if (t3 is Grass)
										{
											grass++;
										}
										else if (t3 is Tree)
										{
											trees++;
										}
										else
										{
											other++;
										}
									}
									Game1.debugOutput = string.Concat(new object[]
									{
										Game1.debugOutput,
										"Grass:",
										grass,
										",  "
									});
									Game1.debugOutput = string.Concat(new object[]
									{
										Game1.debugOutput,
										"Trees:",
										trees,
										",  "
									});
									Game1.debugOutput = string.Concat(new object[]
									{
										Game1.debugOutput,
										"Other Terrain Features:",
										other,
										",  "
									});
									Game1.debugOutput = string.Concat(new object[]
									{
										Game1.debugOutput,
										"Objects: ",
										Game1.currentLocation.objects.Count,
										",  "
									});
									Game1.debugOutput = string.Concat(new object[]
									{
										Game1.debugOutput,
										"temporarySprites: ",
										Game1.currentLocation.temporarySprites.Count,
										",  "
									});
									Game1.drawObjectDialogue(Game1.debugOutput);
									goto IL_6415;
								}
								else
								{
									if (!(text == "completeCC"))
									{
										goto IL_6415;
									}
									Game1.player.mailReceived.Add("ccCraftsRoom");
									Game1.player.mailReceived.Add("ccVault");
									Game1.player.mailReceived.Add("ccFishTank");
									Game1.player.mailReceived.Add("ccBoilerRoom");
									Game1.player.mailReceived.Add("ccPantry");
									Game1.player.mailReceived.Add("ccBulletin");
									CommunityCenter ccc = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
									for (int l = 0; l < ccc.areasComplete.Length; l++)
									{
										ccc.areasComplete[l] = true;
									}
									goto IL_6415;
								}
							}
							else if (num != 627798748u)
							{
								if (num != 635243468u)
								{
									if (num != 654091955u)
									{
										goto IL_6415;
									}
									if (!(text == "rfh"))
									{
										goto IL_6415;
									}
									goto IL_334F;
								}
								else
								{
									if (!(text == "dayUpdate"))
									{
										goto IL_6415;
									}
									Game1.currentLocation.DayUpdate(Game1.dayOfMonth);
									if (debugSplit.Length > 1)
									{
										for (int m = 0; m < Convert.ToInt32(debugSplit[1]) - 1; m++)
										{
											Game1.currentLocation.DayUpdate(Game1.dayOfMonth);
										}
										goto IL_6415;
									}
									goto IL_6415;
								}
							}
							else
							{
								if (!(text == "friendAll"))
								{
									goto IL_6415;
								}
								using (List<NPC>.Enumerator enumerator5 = Utility.getAllCharacters().GetEnumerator())
								{
									while (enumerator5.MoveNext())
									{
										NPC n = enumerator5.Current;
										if (n != null && !Game1.player.friendships.ContainsKey(n.name))
										{
											Game1.player.friendships.Add(n.name, new int[6]);
										}
										Game1.player.changeFriendship(2500, n);
									}
									goto IL_6415;
								}
								goto IL_5951;
							}
							Game1.player.addItemToInventoryBool(new Shears(), false);
							goto IL_6415;
						}
						if (num <= 941250399u)
						{
							if (num <= 795343909u)
							{
								if (num <= 730356610u)
								{
									if (num <= 702513141u)
									{
										if (num != 680058905u)
										{
											if (num != 702513141u)
											{
												goto IL_6415;
											}
											if (!(text == "resource"))
											{
												goto IL_6415;
											}
											Debris.getDebris(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]));
											goto IL_6415;
										}
										else
										{
											if (!(text == "craftingrecipe"))
											{
												goto IL_6415;
											}
											Game1.player.craftingRecipes.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), 0);
											goto IL_6415;
										}
									}
									else if (num != 720728591u)
									{
										if (num != 730356610u)
										{
											goto IL_6415;
										}
										if (!(text == "clone"))
										{
											goto IL_6415;
										}
										Game1.currentLocation.characters.Add(Game1.getCharacterFromName(debugSplit[1], false));
										goto IL_6415;
									}
									else
									{
										if (!(text == "TV"))
										{
											goto IL_6415;
										}
										Game1.player.addItemToInventoryBool(new TV((Game1.random.NextDouble() < 0.5) ? 1466 : 1468, Vector2.Zero), false);
										goto IL_6415;
									}
								}
								else if (num <= 760682489u)
								{
									if (num != 759299508u)
									{
										if (num != 760682489u)
										{
											goto IL_6415;
										}
										if (!(text == "setstat"))
										{
											goto IL_6415;
										}
										Game1.stats.GetType().GetProperty(debugSplit[1]).SetValue(Game1.stats, Convert.ToUInt32(debugSplit[2]), null);
										goto IL_6415;
									}
									else
									{
										if (!(text == "canmove"))
										{
											goto IL_6415;
										}
										goto IL_5CD5;
									}
								}
								else if (num != 788059154u)
								{
									if (num != 795343909u)
									{
										goto IL_6415;
									}
									if (!(text == "growCrops"))
									{
										goto IL_6415;
									}
									goto IL_5BCC;
								}
								else
								{
									if (!(text == "upgradebarn"))
									{
										goto IL_6415;
									}
									goto IL_5AE9;
								}
							}
							else if (num <= 847060761u)
							{
								if (num <= 821490340u)
								{
									if (num != 814732343u)
									{
										if (num != 821490340u)
										{
											goto IL_6415;
										}
										if (!(text == "busDriveBack"))
										{
											goto IL_6415;
										}
										(Game1.getLocationFromName("BusStop") as BusStop).busDriveBack();
										goto IL_6415;
									}
									else
									{
										if (!(text == "removeTF"))
										{
											goto IL_6415;
										}
										goto IL_5185;
									}
								}
								else if (num != 838844761u)
								{
									if (num != 847060761u)
									{
										goto IL_6415;
									}
									if (!(text == "conventionMode"))
									{
										goto IL_6415;
									}
									Game1.conventionMode = !Game1.conventionMode;
									goto IL_6415;
								}
								else
								{
									if (!(text == "junimoNote"))
									{
										goto IL_6415;
									}
									goto IL_486F;
								}
							}
							else if (num <= 894566304u)
							{
								if (num != 893774473u)
								{
									if (num != 894566304u)
									{
										goto IL_6415;
									}
									if (!(text == "sb"))
									{
										goto IL_6415;
									}
								}
								else
								{
									if (!(text == "resetJunimoNotes"))
									{
										goto IL_6415;
									}
									using (Dictionary<int, bool[]>.ValueCollection.Enumerator enumerator6 = (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles.Values.GetEnumerator())
									{
										while (enumerator6.MoveNext())
										{
											bool[] b = enumerator6.Current;
											for (int i2 = 0; i2 < b.Length; i2++)
											{
												b[i2] = false;
											}
										}
										goto IL_6415;
									}
									goto IL_486F;
								}
							}
							else if (num != 908429225u)
							{
								if (num != 941250399u)
								{
									goto IL_6415;
								}
								if (!(text == "ee"))
								{
									goto IL_6415;
								}
								Game1.pauseTime = 0f;
								Game1.nonWarpFade = true;
								Game1.eventFinished();
								Game1.fadeScreenToBlack();
								Game1.viewportFreeze = false;
								goto IL_6415;
							}
							else
							{
								if (!(text == "hurry"))
								{
									goto IL_6415;
								}
								Game1.getCharacterFromName(debugSplit[1], false).warpToPathControllerDestination();
								goto IL_6415;
							}
						}
						else if (num <= 1059385280u)
						{
							if (num <= 1001919309u)
							{
								if (num <= 961676780u)
								{
									if (num != 945450609u)
									{
										if (num != 961676780u)
										{
											goto IL_6415;
										}
										if (!(text == "sf"))
										{
											goto IL_6415;
										}
										goto IL_4CD8;
									}
									else
									{
										if (!(text == "upgradehouse"))
										{
											goto IL_6415;
										}
										Game1.player.HouseUpgradeLevel = Math.Min(3, Game1.player.HouseUpgradeLevel + 1);
										Game1.removeFrontLayerForFarmBuildings();
										Game1.addNewFarmBuildingMaps();
										goto IL_6415;
									}
								}
								else if (num != 979982427u)
								{
									if (num != 1001919309u)
									{
										goto IL_6415;
									}
									if (!(text == "makeInedible"))
									{
										goto IL_6415;
									}
									if (Game1.player.ActiveObject != null)
									{
										Game1.player.ActiveObject.edibility = -300;
										goto IL_6415;
									}
									goto IL_6415;
								}
								else
								{
									if (!(text == "heal"))
									{
										goto IL_6415;
									}
									Game1.player.health = Game1.player.maxHealth;
									goto IL_6415;
								}
							}
							else if (num <= 1049719538u)
							{
								if (num != 1043048946u)
								{
									if (num != 1049719538u)
									{
										goto IL_6415;
									}
									if (!(text == "toss"))
									{
										goto IL_6415;
									}
									Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(738, 2700f, 1, 0, Game1.player.getTileLocation() * (float)Game1.tileSize, false, false)
									{
										rotationChange = 0.09817477f,
										motion = new Vector2(0f, -6f),
										acceleration = new Vector2(0f, 0.08f)
									});
									goto IL_6415;
								}
								else
								{
									if (!(text == "bc"))
									{
										goto IL_6415;
									}
									goto IL_3A4E;
								}
							}
							else if (num != 1051169212u)
							{
								if (num != 1059385280u)
								{
									goto IL_6415;
								}
								if (!(text == "al"))
								{
									goto IL_6415;
								}
								goto IL_3C33;
							}
							else
							{
								if (!(text == "fenceDecay"))
								{
									goto IL_6415;
								}
								using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator7 = Game1.currentLocation.objects.Values.GetEnumerator())
								{
									while (enumerator7.MoveNext())
									{
										object o = enumerator7.Current;
										if (o is Fence)
										{
											(o as Fence).health -= (float)Convert.ToInt32(debugSplit[1]);
										}
									}
									goto IL_6415;
								}
							}
						}
						else if (num <= 1129452970u)
						{
							if (num <= 1097324755u)
							{
								if (num != 1094220446u)
								{
									if (num != 1097324755u)
									{
										goto IL_6415;
									}
									if (!(text == "zl"))
									{
										goto IL_6415;
									}
									goto IL_2D4B;
								}
								else
								{
									if (!(text == "in"))
									{
										goto IL_6415;
									}
									goto IL_56D5;
								}
							}
							else if (num != 1101462628u)
							{
								if (num != 1129452970u)
								{
									goto IL_6415;
								}
								if (!(text == "sl"))
								{
									goto IL_6415;
								}
								Game1.player.shiftToolbar(false);
								goto IL_6415;
							}
							else
							{
								if (!(text == "completeJoja"))
								{
									goto IL_6415;
								}
								Game1.player.mailReceived.Add("ccCraftsRoom");
								Game1.player.mailReceived.Add("ccVault");
								Game1.player.mailReceived.Add("ccFishTank");
								Game1.player.mailReceived.Add("ccBoilerRoom");
								Game1.player.mailReceived.Add("ccPantry");
								Game1.player.mailReceived.Add("jojaCraftsRoom");
								Game1.player.mailReceived.Add("jojaVault");
								Game1.player.mailReceived.Add("jojaFishTank");
								Game1.player.mailReceived.Add("jojaBoilerRoom");
								Game1.player.mailReceived.Add("jojaPantry");
								Game1.player.mailReceived.Add("JojaMember");
								goto IL_6415;
							}
						}
						else if (num <= 1138976003u)
						{
							if (num != 1130187492u)
							{
								if (num != 1138976003u)
								{
									goto IL_6415;
								}
								if (!(text == "shirt"))
								{
									goto IL_6415;
								}
								Game1.player.changeShirt(Convert.ToInt32(debugSplit[1]));
								goto IL_6415;
							}
							else
							{
								if (!(text == "eventseen"))
								{
									goto IL_6415;
								}
								goto IL_5A10;
							}
						}
						else if (num != 1141880953u)
						{
							if (num != 1143714660u)
							{
								if (num != 1145980326u)
								{
									goto IL_6415;
								}
								if (!(text == "pm"))
								{
									goto IL_6415;
								}
								goto IL_42C9;
							}
							else
							{
								if (!(text == "bi"))
								{
									goto IL_6415;
								}
								goto IL_5C91;
							}
						}
						else
						{
							if (!(text == "specials"))
							{
								goto IL_6415;
							}
							Game1.player.hasRustyKey = true;
							Game1.player.hasSkullKey = true;
							Game1.player.hasSpecialCharm = true;
							Game1.player.hasDarkTalisman = true;
							Game1.player.hasMagicInk = true;
							Game1.player.hasClubCard = true;
							Game1.player.canUnderstandDwarves = true;
							goto IL_6415;
						}
						Game1.getCharacterFromName(debugSplit[1], false).showTextAboveHead("Hello! This is a test", -1, 2, 3000, 0);
						goto IL_6415;
						IL_334F:
						Game1.currentLocation.objects[new Vector2((float)Convert.ToInt32(debugSplit[1]), (float)Convert.ToInt32(debugSplit[2]))].minutesUntilReady = 1;
						goto IL_6415;
						IL_4108:
						(Game1.getLocationFromName("FarmHouse") as FarmHouse).moveObjectsForHouseUpgrade(Convert.ToInt32(debugSplit[1]));
						(Game1.getLocationFromName("FarmHouse") as FarmHouse).setMapForUpgradeLevel(Convert.ToInt32(debugSplit[1]), true);
						Game1.player.HouseUpgradeLevel = Convert.ToInt32(debugSplit[1]);
						Game1.removeFrontLayerForFarmBuildings();
						Game1.addNewFarmBuildingMaps();
						goto IL_6415;
						IL_4254:
						Game1.debugOutput = string.Concat(new object[]
						{
							debugSplit[1],
							" is at ",
							Utility.getGameLocationOfCharacter(Game1.getCharacterFromName(debugSplit[1], false)).Name,
							", ",
							Game1.getCharacterFromName(debugSplit[1], false).getTileX(),
							",",
							Game1.getCharacterFromName(debugSplit[1], false).getTileY()
						});
						goto IL_6415;
						IL_486F:
						(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addJunimoNote(Convert.ToInt32(debugSplit[1]));
						goto IL_6415;
						IL_511E:
						using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = (Game1.currentLocation as Farm).animals.Values.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								FarmAnimal a = enumerator4.Current;
								if (a.isBaby())
								{
									a.age = (int)(a.ageWhenMature - 1);
									a.dayUpdate(Game1.currentLocation);
								}
							}
							goto IL_6415;
						}
						IL_5185:
						Game1.currentLocation.terrainFeatures.Clear();
						goto IL_6415;
						IL_5AE9:
						Game1.player.BarnUpgradeLevel = Math.Min(3, Game1.player.BarnUpgradeLevel + 1);
						Game1.removeFrontLayerForFarmBuildings();
						Game1.addNewFarmBuildingMaps();
						goto IL_6415;
						IL_5BCC:
						using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = Game1.currentLocation.terrainFeatures.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<Vector2, TerrainFeature> t4 = enumerator.Current;
								if (t4.Value is HoeDirt && (t4.Value as HoeDirt).crop != null)
								{
									for (int i3 = 0; i3 < Convert.ToInt32(debugSplit[1]); i3++)
									{
										if ((t4.Value as HoeDirt).crop != null)
										{
											(t4.Value as HoeDirt).crop.newDay(1, -1, (int)t4.Key.X, (int)t4.Key.Y, Game1.getLocationFromName("Farm"));
										}
									}
								}
							}
							goto IL_6415;
						}
						goto IL_5C91;
					}
					Farm farm2;
					if (num > 3429009752u)
					{
						if (num <= 3836459715u)
						{
							if (num <= 3612346725u)
							{
								if (num <= 3535957898u)
								{
									if (num <= 3473442495u)
									{
										if (num <= 3439296072u)
										{
											if (num != 3429690440u)
											{
												if (num != 3439296072u)
												{
													goto IL_6415;
												}
												if (!(text == "save"))
												{
													goto IL_6415;
												}
												Game1.saveOnNewDay = !Game1.saveOnNewDay;
												if (Game1.saveOnNewDay)
												{
													Game1.playSound("bigSelect");
													goto IL_6415;
												}
												Game1.playSound("bigDeSelect");
												goto IL_6415;
											}
											else
											{
												if (!(text == "gamePad"))
												{
													goto IL_6415;
												}
												Game1.options.gamepadControls = !Game1.options.gamepadControls;
												Game1.options.mouseControls = !Game1.options.gamepadControls;
												Game1.showGlobalMessage(Game1.options.gamepadControls ? "using gamepad" : "not using gamepad");
												goto IL_6415;
											}
										}
										else if (num != 3454868101u)
										{
											if (num != 3473442495u)
											{
												goto IL_6415;
											}
											if (!(text == "customizeMenu"))
											{
												goto IL_6415;
											}
											goto IL_54BA;
										}
										else
										{
											if (!(text == "exit"))
											{
												goto IL_6415;
											}
											goto IL_5427;
										}
									}
									else if (num <= 3499467944u)
									{
										if (num != 3482252359u)
										{
											if (num != 3499467944u)
											{
												goto IL_6415;
											}
											if (!(text == "addJunimo"))
											{
												goto IL_6415;
											}
											goto IL_47C6;
										}
										else
										{
											if (!(text == "steaminfo"))
											{
												goto IL_6415;
											}
											if (!SteamAPI.IsSteamRunning())
											{
												Game1.debugOutput = "steam is not running";
												SteamAPI.Init();
												goto IL_6415;
											}
											Game1.debugOutput = "steam is running";
											if (SteamUser.BLoggedOn())
											{
												Game1.debugOutput += ", user logged on";
												goto IL_6415;
											}
											goto IL_6415;
										}
									}
									else if (num != 3517781723u)
									{
										if (num != 3535957898u)
										{
											goto IL_6415;
										}
										if (!(text == "hairStyle"))
										{
											goto IL_6415;
										}
										Game1.player.changeHairStyle(Convert.ToInt32(debugSplit[1]));
										goto IL_6415;
									}
									else
									{
										if (!(text == "coopDweller"))
										{
											goto IL_6415;
										}
										Game1.player.coopDwellers.Add(new CoopDweller(debugSplit[1], debugSplit[2]));
										goto IL_6415;
									}
								}
								else if (num <= 3577379802u)
								{
									if (num <= 3569420156u)
									{
										if (num != 3552237772u)
										{
											if (num != 3569420156u)
											{
												goto IL_6415;
											}
											if (!(text == "spreadSeeds"))
											{
												goto IL_6415;
											}
											farm2 = Game1.getFarm();
											using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = farm2.terrainFeatures.GetEnumerator())
											{
												while (enumerator.MoveNext())
												{
													KeyValuePair<Vector2, TerrainFeature> t5 = enumerator.Current;
													if (t5.Value is HoeDirt)
													{
														(t5.Value as HoeDirt).crop = new Crop(Convert.ToInt32(debugSplit[1]), (int)t5.Key.X, (int)t5.Key.Y);
													}
												}
												goto IL_6415;
											}
											goto IL_2A7D;
										}
										else
										{
											if (!(text == "characterInfo"))
											{
												goto IL_6415;
											}
											Game1.showGlobalMessage(Game1.currentLocation.characters.Count + " characters on this map");
											goto IL_6415;
										}
									}
									else if (num != 3570692901u)
									{
										if (num != 3577379802u)
										{
											goto IL_6415;
										}
										if (!(text == "removeBuildings"))
										{
											goto IL_6415;
										}
										Game1.getFarm().buildings.Clear();
										goto IL_6415;
									}
									else if (!(text == "daysPlayed"))
									{
										goto IL_6415;
									}
								}
								else if (num <= 3602912557u)
								{
									if (num != 3602710866u)
									{
										if (num != 3602912557u)
										{
											goto IL_6415;
										}
										if (!(text == "getstat"))
										{
											goto IL_6415;
										}
										Game1.debugOutput = Game1.stats.GetType().GetProperty(debugSplit[1]).GetValue(Game1.stats, null).ToString();
										goto IL_6415;
									}
									else
									{
										if (!(text == "buildcoop"))
										{
											goto IL_6415;
										}
										goto IL_3A4E;
									}
								}
								else if (num != 3609007298u)
								{
									if (num != 3612346725u)
									{
										goto IL_6415;
									}
									if (!(text == "furniture"))
									{
										goto IL_6415;
									}
									goto IL_40A2;
								}
								else
								{
									if (!(text == "addKent"))
									{
										goto IL_6415;
									}
									Game1.getLocationFromName("SamHouse").characters.Add(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Kent")), new Vector2((float)(8 * Game1.tileSize), (float)(13 * Game1.tileSize)), "SamHouse", 3, "Kent", false, Game1.content.Load<Dictionary<int, int[]>>("Characters\\schedules\\spring\\Kent"), Game1.content.Load<Texture2D>("Portraits\\Kent")));
									Game1.player.friendships.Add("Kent", new int[5]);
									goto IL_6415;
								}
							}
							else if (num <= 3780168015u)
							{
								if (num <= 3697756736u)
								{
									if (num <= 3679392722u)
									{
										if (num != 3655829174u)
										{
											if (num != 3679392722u)
											{
												goto IL_6415;
											}
											if (!(text == "dap"))
											{
												goto IL_6415;
											}
										}
										else
										{
											if (!(text == "doesItemExist"))
											{
												goto IL_6415;
											}
											Game1.showGlobalMessage(Utility.doesItemWithThisIndexExistAnywhere(Convert.ToInt32(debugSplit[1]), debugSplit.Length > 2) ? "Yes" : "No");
											goto IL_6415;
										}
									}
									else if (num != 3679930478u)
									{
										if (num != 3697756736u)
										{
											goto IL_6415;
										}
										if (!(text == "zoomLevel"))
										{
											goto IL_6415;
										}
										goto IL_2D4B;
									}
									else
									{
										if (!(text == "clearSpecials"))
										{
											goto IL_6415;
										}
										Game1.player.hasRustyKey = false;
										Game1.player.hasSkullKey = false;
										Game1.player.hasSpecialCharm = false;
										Game1.player.hasDarkTalisman = false;
										Game1.player.hasMagicInk = false;
										Game1.player.hasClubCard = false;
										Game1.player.canUnderstandDwarves = false;
										goto IL_6415;
									}
								}
								else if (num <= 3723353184u)
								{
									if (num != 3710806992u)
									{
										if (num != 3723353184u)
										{
											goto IL_6415;
										}
										if (!(text == "gamemode"))
										{
											goto IL_6415;
										}
										Game1.setGameMode(Convert.ToByte(debugSplit[1]));
										goto IL_6415;
									}
									else
									{
										if (!(text == "clearLightGlows"))
										{
											goto IL_6415;
										}
										Game1.currentLocation.lightGlows.Clear();
										goto IL_6415;
									}
								}
								else if (num != 3726850017u)
								{
									if (num != 3780168015u)
									{
										goto IL_6415;
									}
									if (!(text == "money"))
									{
										goto IL_6415;
									}
									Game1.player.Money = Convert.ToInt32(debugSplit[1]);
									goto IL_6415;
								}
								else
								{
									if (!(text == "pants"))
									{
										goto IL_6415;
									}
									Game1.player.changePants(new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])));
									goto IL_6415;
								}
							}
							else if (num <= 3809224601u)
							{
								if (num <= 3782741031u)
								{
									if (num != 3780611722u)
									{
										if (num != 3782741031u)
										{
											goto IL_6415;
										}
										if (!(text == "barnDweller"))
										{
											goto IL_6415;
										}
										Game1.player.barnDwellers.Add(new BarnDweller(debugSplit[1], debugSplit[2]));
										goto IL_6415;
									}
									else
									{
										if (!(text == "wateringcan"))
										{
											goto IL_6415;
										}
										goto IL_62D5;
									}
								}
								else if (num != 3786391242u)
								{
									if (num != 3809224601u)
									{
										goto IL_6415;
									}
									if (!(text == "f"))
									{
										goto IL_6415;
									}
									goto IL_41A9;
								}
								else
								{
									if (!(text == "mft"))
									{
										goto IL_6415;
									}
									goto IL_4237;
								}
							}
							else if (num <= 3829016743u)
							{
								if (num != 3820166566u)
								{
									if (num != 3829016743u)
									{
										goto IL_6415;
									}
									if (!(text == "noSave"))
									{
										goto IL_6415;
									}
									goto IL_331D;
								}
								else
								{
									if (!(text == "marry"))
									{
										goto IL_6415;
									}
									goto IL_3FF9;
								}
							}
							else if (num != 3829274584u)
							{
								if (num != 3830391293u)
								{
									if (num != 3836459715u)
									{
										goto IL_6415;
									}
									if (!(text == "viewport"))
									{
										goto IL_6415;
									}
									Game1.viewport.X = Convert.ToInt32(debugSplit[1]) * Game1.tileSize;
									Game1.viewport.Y = Convert.ToInt32(debugSplit[2]) * Game1.tileSize;
									goto IL_6415;
								}
								else
								{
									if (!(text == "day"))
									{
										goto IL_6415;
									}
									Game1.stats.DaysPlayed = (uint)((Utility.getSeasonNumber(Game1.currentSeason) * 28 + Convert.ToInt32(debugSplit[1])) * Game1.year);
									Game1.dayOfMonth = Convert.ToInt32(debugSplit[1]);
									goto IL_6415;
								}
							}
							else
							{
								if (!(text == "coop"))
								{
									goto IL_6415;
								}
								goto IL_5ABE;
							}
							Game1.showGlobalMessage((int)Game1.stats.DaysPlayed + " days played.");
							goto IL_6415;
						}
						if (num <= 4022571063u)
						{
							if (num <= 3888581224u)
							{
								if (num <= 3859557458u)
								{
									if (num <= 3841694854u)
									{
										if (num != 3840843484u)
										{
											if (num != 3841694854u)
											{
												goto IL_6415;
											}
											if (!(text == "upgradeCoop"))
											{
												goto IL_6415;
											}
											goto IL_3699;
										}
										else
										{
											if (!(text == "bloom"))
											{
												goto IL_6415;
											}
											goto IL_613D;
										}
									}
									else if (num != 3852476509u)
									{
										if (num != 3859557458u)
										{
											goto IL_6415;
										}
										if (!(text == "c"))
										{
											goto IL_6415;
										}
										goto IL_5CD5;
									}
									else
									{
										if (!(text == "child"))
										{
											goto IL_6415;
										}
										goto IL_3E41;
									}
								}
								else if (num <= 3869759516u)
								{
									if (num != 3865623817u)
									{
										if (num != 3869759516u)
										{
											goto IL_6415;
										}
										if (!(text == "removeLights"))
										{
											goto IL_6415;
										}
										Game1.currentLightSources.Clear();
										goto IL_6415;
									}
									else
									{
										if (!(text == "dog"))
										{
											goto IL_6415;
										}
										Game1.currentLocation.characters.Add(new Dog(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2])));
										goto IL_6415;
									}
								}
								else if (num != 3876335077u)
								{
									if (num != 3888581224u)
									{
										goto IL_6415;
									}
									if (!(text == "busDriveOff"))
									{
										goto IL_6415;
									}
									(Game1.getLocationFromName("BusStop") as BusStop).busDriveOff();
									goto IL_6415;
								}
								else
								{
									if (!(text == "b"))
									{
										goto IL_6415;
									}
									goto IL_5C91;
								}
							}
							else if (num <= 3960223172u)
							{
								if (num <= 3927117501u)
								{
									if (num != 3893112696u)
									{
										if (num != 3927117501u)
										{
											goto IL_6415;
										}
										if (!(text == "child2"))
										{
											goto IL_6415;
										}
										if (Game1.player.getChildren().Count > 1)
										{
											Game1.player.getChildren()[1].age++;
											Game1.player.getChildren()[1].reloadSprite();
											goto IL_6415;
										}
										(Game1.getLocationFromName("FarmHouse") as FarmHouse).characters.Add(new Child("Baby2", Game1.random.NextDouble() < 0.5, Game1.random.NextDouble() < 0.5, Game1.player));
										goto IL_6415;
									}
									else
									{
										if (!(text == "m"))
										{
											goto IL_6415;
										}
										goto IL_5615;
									}
								}
								else if (num != 3952565359u)
								{
									if (num != 3960223172u)
									{
										goto IL_6415;
									}
									if (!(text == "i"))
									{
										goto IL_6415;
									}
									goto IL_5672;
								}
								else
								{
									if (!(text == "panMode"))
									{
										goto IL_6415;
									}
									goto IL_42C9;
								}
							}
							else if (num <= 3970545608u)
							{
								if (num != 3966747043u)
								{
									if (num != 3970545608u)
									{
										goto IL_6415;
									}
									if (!(text == "setFrame"))
									{
										goto IL_6415;
									}
									goto IL_4CD8;
								}
								else
								{
									if (!(text == "fishCaught"))
									{
										goto IL_6415;
									}
									goto IL_4F86;
								}
							}
							else if (num != 4010556029u)
							{
								if (num != 4020148446u)
								{
									if (num != 4022571063u)
									{
										goto IL_6415;
									}
									if (!(text == "hoe"))
									{
										goto IL_6415;
									}
									Game1.player.addItemToInventoryBool(new Hoe(), false);
									Game1.playSound("coin");
									goto IL_6415;
								}
								else
								{
									if (!(text == "playMusic"))
									{
										goto IL_6415;
									}
									Game1.changeMusicTrack(debugSplit[1]);
									goto IL_6415;
								}
							}
							else
							{
								if (!(text == "j"))
								{
									goto IL_6415;
								}
								goto IL_47C6;
							}
						}
						else if (num <= 4149934955u)
						{
							if (num <= 4072609730u)
							{
								if (num <= 4037672760u)
								{
									if (num != 4034148709u)
									{
										if (num != 4037672760u)
										{
											goto IL_6415;
										}
										if (!(text == "resetMines"))
										{
											goto IL_6415;
										}
										(Game1.getLocationFromName("UndergroundMine") as MineShaft).permanentMineChanges.Clear();
										Game1.playSound("jingle1");
										goto IL_6415;
									}
									else if (!(text == "can"))
									{
										goto IL_6415;
									}
								}
								else if (num != 4060888886u)
								{
									if (num != 4072609730u)
									{
										goto IL_6415;
									}
									if (!(text == "hat"))
									{
										goto IL_6415;
									}
									Game1.player.changeHat(Convert.ToInt32(debugSplit[1]));
									Game1.playSound("coin");
									goto IL_6415;
								}
								else
								{
									if (!(text == "w"))
									{
										goto IL_6415;
									}
									goto IL_4162;
								}
							}
							else if (num <= 4144776981u)
							{
								if (num != 4125344348u)
								{
									if (num != 4144776981u)
									{
										goto IL_6415;
									}
									if (!(text == "r"))
									{
										goto IL_6415;
									}
									Game1.currentLocation.cleanupBeforePlayerExit();
									Game1.currentLocation.resetForPlayerEntry();
									goto IL_6415;
								}
								else
								{
									if (!(text == "hairColor"))
									{
										goto IL_6415;
									}
									Game1.player.changeHairColor(new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])));
									goto IL_6415;
								}
							}
							else if (num != 4149773707u)
							{
								if (num != 4149934955u)
								{
									goto IL_6415;
								}
								if (!(text == "ring"))
								{
									goto IL_6415;
								}
								Game1.player.addItemToInventoryBool(new Ring(Convert.ToInt32(debugSplit[1])), false);
								Game1.playSound("coin");
								goto IL_6415;
							}
							else
							{
								if (!(text == "lookup"))
								{
									goto IL_6415;
								}
								goto IL_4682;
							}
						}
						else if (num <= 4198624760u)
						{
							if (num <= 4180716550u)
							{
								if (num != 4176079303u)
								{
									if (num != 4180716550u)
									{
										goto IL_6415;
									}
									if (!(text == "seenevent"))
									{
										goto IL_6415;
									}
									goto IL_5A10;
								}
								else
								{
									if (!(text == "waterColor"))
									{
										goto IL_6415;
									}
									Game1.currentLocation.waterColor = new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3])) * 0.5f;
									goto IL_6415;
								}
							}
							else if (num != 4183284832u)
							{
								if (num != 4198624760u)
								{
									goto IL_6415;
								}
								if (!(text == "pick"))
								{
									goto IL_6415;
								}
								goto IL_62F5;
							}
							else
							{
								if (!(text == "clearFurniture"))
								{
									goto IL_6415;
								}
								(Game1.currentLocation as FarmHouse).furniture.Clear();
								goto IL_6415;
							}
						}
						else if (num <= 4237905830u)
						{
							if (num != 4216511432u)
							{
								if (num != 4237905830u)
								{
									goto IL_6415;
								}
								if (!(text == "endEvent"))
								{
									goto IL_6415;
								}
								goto IL_4D04;
							}
							else
							{
								if (!(text == "removeDirt"))
								{
									goto IL_6415;
								}
								for (int i4 = Game1.currentLocation.terrainFeatures.Count - 1; i4 >= 0; i4--)
								{
									if (Game1.currentLocation.terrainFeatures.ElementAt(i4).Value is HoeDirt)
									{
										Game1.currentLocation.terrainFeatures.Remove(Game1.currentLocation.terrainFeatures.ElementAt(i4).Key);
									}
								}
								goto IL_6415;
							}
						}
						else if (num != 4264611999u)
						{
							if (num != 4273865935u)
							{
								if (num != 4294604882u)
								{
									goto IL_6415;
								}
								if (!(text == "boots"))
								{
									goto IL_6415;
								}
								Game1.player.addItemToInventoryBool(new Boots(Convert.ToInt32(debugSplit[1])), false);
								Game1.playSound("coin");
								goto IL_6415;
							}
							else
							{
								if (!(text == "ccloadcutscene"))
								{
									goto IL_6415;
								}
								goto IL_4714;
							}
						}
						else
						{
							if (!(text == "event"))
							{
								goto IL_6415;
							}
							if (debugSplit.Length <= 3)
							{
								Game1.player.eventsSeen.Clear();
							}
							string locationName = debugSplit[1];
							if (locationName == "Pool")
							{
								locationName = "BathHouse_Pool";
							}
							if (Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + locationName).ElementAt(Convert.ToInt32(debugSplit[2])).Key.Contains('/'))
							{
								Game1.getLocationFromName(locationName).currentEvent = new Event(Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + locationName).ElementAt(Convert.ToInt32(debugSplit[2])).Value, -1);
								Game1.warpFarmer(locationName, 8, 8, false);
								goto IL_6415;
							}
							goto IL_6415;
						}
						IL_62D5:
						Game1.player.addItemToInventoryBool(new WateringCan(), false);
						Game1.playSound("coin");
						goto IL_6415;
					}
					if (num > 2671260646u)
					{
						if (num <= 2947385699u)
						{
							if (num <= 2854486906u)
							{
								if (num <= 2804296981u)
								{
									if (num <= 2749624805u)
									{
										if (num != 2686971291u)
										{
											if (num != 2749624805u)
											{
												goto IL_6415;
											}
											if (!(text == "removeQuest"))
											{
												goto IL_6415;
											}
											Game1.player.removeQuest(Convert.ToInt32(debugSplit[1]));
											goto IL_6415;
										}
										else
										{
											if (!(text == "sprinkle"))
											{
												goto IL_6415;
											}
											Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 7, 7, 2000, 100, Color.White, null, false);
											goto IL_6415;
										}
									}
									else if (num != 2803217125u)
									{
										if (num != 2804296981u)
										{
											goto IL_6415;
										}
										if (!(text == "wall"))
										{
											goto IL_6415;
										}
										goto IL_4162;
									}
									else
									{
										if (!(text == "owl"))
										{
											goto IL_6415;
										}
										Game1.currentLocation.addOwl();
										goto IL_6415;
									}
								}
								else if (num <= 2825673007u)
								{
									if (num != 2805947405u)
									{
										if (num != 2825673007u)
										{
											goto IL_6415;
										}
										if (!(text == "lantern"))
										{
											goto IL_6415;
										}
										Game1.player.items.Add(new Lantern());
										goto IL_6415;
									}
									else
									{
										if (!(text == "jump"))
										{
											goto IL_6415;
										}
										float jumpV = 8f;
										if (debugSplit.Length > 2)
										{
											jumpV = (float)Convert.ToDouble(debugSplit[2]);
										}
										if (debugSplit[1].Equals("farmer"))
										{
											Game1.player.jump(jumpV);
											goto IL_6415;
										}
										Game1.getCharacterFromName(debugSplit[1], false).jump(jumpV);
										goto IL_6415;
									}
								}
								else if (num != 2850206306u)
								{
									if (num != 2854486906u)
									{
										goto IL_6415;
									}
									if (!(text == "debrisWeather"))
									{
										goto IL_6415;
									}
									Game1.isDebrisWeather = !Game1.isDebrisWeather;
									goto IL_6415;
								}
								else if (!(text == "KillAllHorses"))
								{
									goto IL_6415;
								}
							}
							else if (num <= 2899391285u)
							{
								if (num <= 2888452705u)
								{
									if (num != 2885078319u)
									{
										if (num != 2888452705u)
										{
											goto IL_6415;
										}
										if (!(text == "cookingrecipe"))
										{
											goto IL_6415;
										}
										Game1.player.cookingRecipes.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), 0);
										goto IL_6415;
									}
									else
									{
										if (!(text == "skinColor"))
										{
											goto IL_6415;
										}
										Game1.player.changeSkinColor(Convert.ToInt32(debugSplit[1]));
										goto IL_6415;
									}
								}
								else if (num != 2896420465u)
								{
									if (num != 2899391285u)
									{
										goto IL_6415;
									}
									if (!(text == "fishing"))
									{
										goto IL_6415;
									}
									Game1.player.FishingLevel = Convert.ToInt32(debugSplit[1]);
									goto IL_6415;
								}
								else
								{
									if (!(text == "clearMail"))
									{
										goto IL_6415;
									}
									Game1.player.mailReceived.Clear();
									goto IL_6415;
								}
							}
							else if (num <= 2923112787u)
							{
								if (num != 2900856178u)
								{
									if (num != 2923112787u)
									{
										goto IL_6415;
									}
									if (!(text == "dialogue"))
									{
										goto IL_6415;
									}
									Game1.getCharacterFromName(debugSplit[1], false).CurrentDialogue.Push(new Dialogue(debugInput.Substring(debugInput.IndexOf("0") + 1), Game1.getCharacterFromName(debugSplit[1], false)));
									goto IL_6415;
								}
								else
								{
									if (!(text == "clearCharacters"))
									{
										goto IL_6415;
									}
									Game1.currentLocation.characters.Clear();
									goto IL_6415;
								}
							}
							else if (num != 2927578396u)
							{
								if (num != 2947385699u)
								{
									goto IL_6415;
								}
								if (!(text == "fish"))
								{
									goto IL_6415;
								}
								Game1.activeClickableMenu = new BobberBar(Convert.ToInt32(debugSplit[1]), 0.5f, true, ((Game1.player.CurrentTool as FishingRod).attachments[1] != null) ? (Game1.player.CurrentTool as FishingRod).attachments[1].ParentSheetIndex : -1);
								goto IL_6415;
							}
							else
							{
								if (!(text == "year"))
								{
									goto IL_6415;
								}
								Game1.year = Convert.ToInt32(debugSplit[1]);
								goto IL_6415;
							}
						}
						else if (num <= 3251403663u)
						{
							if (num <= 3102149661u)
							{
								if (num <= 3048072735u)
								{
									if (num != 2949673445u)
									{
										if (num != 3048072735u)
										{
											goto IL_6415;
										}
										if (!(text == "crafting"))
										{
											goto IL_6415;
										}
										using (Dictionary<string, string>.KeyCollection.Enumerator enumerator8 = CraftingRecipe.craftingRecipes.Keys.GetEnumerator())
										{
											while (enumerator8.MoveNext())
											{
												string s = enumerator8.Current;
												if (!Game1.player.craftingRecipes.ContainsKey(s))
												{
													Game1.player.craftingRecipes.Add(s, 0);
												}
											}
											goto IL_6415;
										}
										goto IL_4F0A;
									}
									else
									{
										if (!(text == "test"))
										{
											goto IL_6415;
										}
										Game1.currentMinigame = new Test();
										goto IL_6415;
									}
								}
								else if (num != 3057832448u)
								{
									if (num != 3102149661u)
									{
										goto IL_6415;
									}
									if (!(text == "floor"))
									{
										goto IL_6415;
									}
									goto IL_41A9;
								}
								else
								{
									if (!(text == "killAll"))
									{
										goto IL_6415;
									}
									string safeCharacter = debugSplit[1];
									using (List<GameLocation>.Enumerator enumerator9 = Game1.locations.GetEnumerator())
									{
										while (enumerator9.MoveNext())
										{
											GameLocation l2 = enumerator9.Current;
											if (!l2.Equals(Game1.currentLocation))
											{
												l2.characters.Clear();
											}
											else
											{
												for (int i5 = l2.characters.Count - 1; i5 >= 0; i5--)
												{
													if (!l2.characters[i5].name.Equals(safeCharacter))
													{
														l2.characters.RemoveAt(i5);
													}
												}
											}
										}
										goto IL_6415;
									}
								}
							}
							else if (num <= 3171639549u)
							{
								if (num != 3103933528u)
								{
									if (num != 3171639549u)
									{
										goto IL_6415;
									}
									if (!(text == "beachBridge"))
									{
										goto IL_6415;
									}
									(Game1.getLocationFromName("Beach") as Beach).bridgeFixed = !(Game1.getLocationFromName("Beach") as Beach).bridgeFixed;
									if (!(Game1.getLocationFromName("Beach") as Beach).bridgeFixed)
									{
										(Game1.getLocationFromName("Beach") as Beach).setMapTile(58, 13, 284, "Buildings", null, 1);
										goto IL_6415;
									}
									goto IL_6415;
								}
								else
								{
									if (!(text == "achievement"))
									{
										goto IL_6415;
									}
									goto IL_5796;
								}
							}
							else if (num != 3210569028u)
							{
								if (num != 3251403663u)
								{
									goto IL_6415;
								}
								if (!(text == "slimecraft"))
								{
									goto IL_6415;
								}
								Game1.player.craftingRecipes.Add("Slime Incubator", 0);
								Game1.player.craftingRecipes.Add("Slime Egg-Press", 0);
								Game1.playSound("crystal");
								goto IL_6415;
							}
							else
							{
								if (!(text == "LSD"))
								{
									goto IL_6415;
								}
								Game1.bloom.startShifting((float)Convert.ToDouble(debugSplit[1]), (float)Convert.ToDouble(debugSplit[2]), (float)Convert.ToDouble(debugSplit[3]) / 1000f, (float)Convert.ToDouble(debugSplit[4]) / 100f, (float)Convert.ToDouble(debugSplit[5]) / 100f, (float)Convert.ToDouble(debugSplit[6]) / 100f, (float)Convert.ToDouble(debugSplit[7]) / 100f, (float)Convert.ToDouble(debugSplit[8]) / 100f, (float)Convert.ToDouble(debugSplit[9]) / 100f, (float)Convert.ToDouble(debugSplit[10]) / 100f, (float)Convert.ToDouble(debugSplit[11]), true);
								goto IL_6415;
							}
						}
						else if (num <= 3313734240u)
						{
							if (num <= 3281777315u)
							{
								if (num != 3277883797u)
								{
									if (num != 3281777315u)
									{
										goto IL_6415;
									}
									if (!(text == "build"))
									{
										goto IL_6415;
									}
									Game1.getFarm().buildStructure(new BluePrint(debugSplit[1].Replace('9', ' ')), (debugSplit.Length > 3) ? new Vector2((float)Convert.ToInt32(debugSplit[2]), (float)Convert.ToInt32(debugSplit[3])) : new Vector2((float)(Game1.player.getTileX() + 1), (float)Game1.player.getTileY()), false, Game1.player, false);
									Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
									goto IL_6415;
								}
								else
								{
									if (!(text == "farmMap"))
									{
										goto IL_6415;
									}
									for (int i6 = 0; i6 < Game1.locations.Count; i6++)
									{
										if (Game1.locations[i6] is Farm)
										{
											Game1.locations.RemoveAt(i6);
										}
										if (Game1.locations[i6] is FarmHouse)
										{
											Game1.locations.RemoveAt(i6);
										}
									}
									Game1.whichFarm = Convert.ToInt32(debugSplit[1]);
									Game1.locations.Add(new Farm(Game1.content.Load<Map>("Maps\\" + Farm.getMapNameFromTypeInt(Game1.whichFarm)), "Farm"));
									Game1.locations.Add(new FarmHouse(Game1.content.Load<Map>("Maps\\FarmHouse"), "FarmHouse"));
									goto IL_6415;
								}
							}
							else if (num != 3295710157u)
							{
								if (num != 3313734240u)
								{
									goto IL_6415;
								}
								if (!(text == "blueprint"))
								{
									goto IL_6415;
								}
								Game1.player.blueprints.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim());
								goto IL_6415;
							}
							else
							{
								if (!(text == "faceDirection"))
								{
									goto IL_6415;
								}
								goto IL_6356;
							}
						}
						else if (num <= 3385614082u)
						{
							if (num != 3356994034u)
							{
								if (num != 3385614082u)
								{
									goto IL_6415;
								}
								if (!(text == "playSound"))
								{
									goto IL_6415;
								}
								goto IL_4EA0;
							}
							else
							{
								if (!(text == "upgradeBarn"))
								{
									goto IL_6415;
								}
								using (List<Building>.Enumerator enumerator10 = Game1.getFarm().buildings.GetEnumerator())
								{
									while (enumerator10.MoveNext())
									{
										Building b2 = enumerator10.Current;
										if (b2 is Barn)
										{
											b2.daysUntilUpgrade = 1;
										}
									}
									goto IL_6415;
								}
								goto IL_3699;
							}
						}
						else if (num != 3393529720u)
						{
							if (num != 3416301453u)
							{
								if (num != 3429009752u)
								{
									goto IL_6415;
								}
								if (!(text == "season"))
								{
									goto IL_6415;
								}
								Game1.currentSeason = debugSplit[1];
								Game1.setGraphicsForSeason();
								goto IL_6415;
							}
							else
							{
								if (!(text == "friend"))
								{
									goto IL_6415;
								}
								goto IL_5951;
							}
						}
						else
						{
							if (!(text == "energize"))
							{
								goto IL_6415;
							}
							Game1.player.Stamina = (float)Game1.player.MaxStamina;
							if (debugSplit.Length > 1)
							{
								Game1.player.Stamina = (float)Convert.ToInt32(debugSplit[1]);
								goto IL_6415;
							}
							goto IL_6415;
						}
						using (List<GameLocation>.Enumerator enumerator9 = Game1.locations.GetEnumerator())
						{
							while (enumerator9.MoveNext())
							{
								GameLocation l3 = enumerator9.Current;
								for (int i7 = l3.characters.Count - 1; i7 >= 0; i7--)
								{
									if (l3.characters[i7] is Horse)
									{
										l3.characters.RemoveAt(i7);
										Game1.playSound("drumkit0");
									}
								}
							}
							goto IL_6415;
						}
						goto IL_3FF9;
					}
					if (num <= 2442791997u)
					{
						if (num <= 2212047380u)
						{
							if (num <= 2096616551u)
							{
								if (num <= 2075513521u)
								{
									if (num != 2072037248u)
									{
										if (num != 2075513521u)
										{
											goto IL_6415;
										}
										if (!(text == "fixAnimals"))
										{
											goto IL_6415;
										}
										Farm farm3 = Game1.getFarm();
										for (int i8 = farm3.buildings.Count - 1; i8 >= 0; i8--)
										{
											if (farm3.buildings[i8].indoors != null && farm3.buildings[i8].indoors is AnimalHouse)
											{
												foreach (FarmAnimal a2 in (farm3.buildings[i8].indoors as AnimalHouse).animals.Values)
												{
													for (int j2 = farm3.buildings.Count - 1; j2 >= 0; j2--)
													{
														if (farm3.buildings[j2].indoors != null && farm3.buildings[j2].indoors is AnimalHouse && (farm3.buildings[j2].indoors as AnimalHouse).animalsThatLiveHere.Contains(a2.myID) && !farm3.buildings[j2].Equals(a2.home))
														{
															for (int k2 = (farm3.buildings[j2].indoors as AnimalHouse).animalsThatLiveHere.Count - 1; k2 >= 0; k2--)
															{
																if ((farm3.buildings[j2].indoors as AnimalHouse).animalsThatLiveHere[k2] == a2.myID)
																{
																	(farm3.buildings[j2].indoors as AnimalHouse).animalsThatLiveHere.RemoveAt(k2);
																	Game1.playSound("crystal");
																}
															}
														}
													}
												}
												for (int k3 = (farm3.buildings[i8].indoors as AnimalHouse).animalsThatLiveHere.Count - 1; k3 >= 0; k3--)
												{
													if (Utility.getAnimal((farm3.buildings[i8].indoors as AnimalHouse).animalsThatLiveHere[k3]) == null)
													{
														(farm3.buildings[i8].indoors as AnimalHouse).animalsThatLiveHere.RemoveAt(k3);
														Game1.playSound("crystal");
													}
												}
											}
										}
										goto IL_6415;
									}
									else
									{
										if (!(text == "speed"))
										{
											goto IL_6415;
										}
										Game1.player.addedSpeed = Convert.ToInt32(debugSplit[1]);
										goto IL_6415;
									}
								}
								else if (num != 2095482337u)
								{
									if (num != 2096616551u)
									{
										goto IL_6415;
									}
									if (!(text == "kid"))
									{
										goto IL_6415;
									}
									goto IL_3E41;
								}
								else
								{
									if (!(text == "fillbin"))
									{
										goto IL_6415;
									}
									goto IL_35C3;
								}
							}
							else if (num <= 2137427461u)
							{
								if (num != 2105453515u)
								{
									if (num != 2137427461u)
									{
										goto IL_6415;
									}
									if (!(text == "divorce"))
									{
										goto IL_6415;
									}
									Game1.player.divorceTonight = true;
									goto IL_6415;
								}
								else
								{
									if (!(text == "backpack"))
									{
										goto IL_6415;
									}
									Game1.player.increaseBackpackSize(Math.Min(20 - Game1.player.maxItems, Convert.ToInt32(debugSplit[1])));
									goto IL_6415;
								}
							}
							else if (num != 2160778853u)
							{
								if (num != 2212047380u)
								{
									goto IL_6415;
								}
								if (!(text == "movebuilding"))
								{
									goto IL_6415;
								}
								Game1.getFarm().getBuildingAt(new Vector2((float)Convert.ToInt32(debugSplit[1]), (float)Convert.ToInt32(debugSplit[2]))).tileX = Convert.ToInt32(debugSplit[3]);
								Game1.getFarm().getBuildingAt(new Vector2((float)Convert.ToInt32(debugSplit[1]), (float)Convert.ToInt32(debugSplit[2]))).tileY = Convert.ToInt32(debugSplit[4]);
								goto IL_6415;
							}
							else
							{
								if (!(text == "allbundles"))
								{
									goto IL_6415;
								}
								foreach (KeyValuePair<int, bool[]> b3 in (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles)
								{
									for (int j3 = 0; j3 < b3.Value.Length; j3++)
									{
										b3.Value[j3] = true;
									}
								}
								Game1.playSound("crystal");
								goto IL_6415;
							}
						}
						else if (num <= 2336959222u)
						{
							if (num <= 2275093638u)
							{
								if (num != 2268828259u)
								{
									if (num != 2275093638u)
									{
										goto IL_6415;
									}
									if (!(text == "mailForTomorrow"))
									{
										goto IL_6415;
									}
									goto IL_4237;
								}
								else if (!(text == "spreadDirt"))
								{
									goto IL_6415;
								}
							}
							else if (num != 2309207507u)
							{
								if (num != 2336959222u)
								{
									goto IL_6415;
								}
								if (!(text == "warpCharacter"))
								{
									goto IL_6415;
								}
								goto IL_5F7B;
							}
							else
							{
								if (!(text == "rain"))
								{
									goto IL_6415;
								}
								Game1.isRaining = !Game1.isRaining;
								Game1.isDebrisWeather = false;
								goto IL_6415;
							}
						}
						else if (num <= 2393514947u)
						{
							if (num != 2382613700u)
							{
								if (num != 2393514947u)
								{
									goto IL_6415;
								}
								if (!(text == "ccload"))
								{
									goto IL_6415;
								}
								(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).loadArea(Convert.ToInt32(debugSplit[1]), true);
								(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).areasComplete[Convert.ToInt32(debugSplit[1])] = true;
								goto IL_6415;
							}
							else
							{
								if (!(text == "minelevel"))
								{
									goto IL_6415;
								}
								if (Game1.mine != null)
								{
									Game1.enterMine(false, Convert.ToInt32(debugSplit[1]), null);
									goto IL_6415;
								}
								goto IL_6415;
							}
						}
						else if (num != 2398780079u)
						{
							if (num != 2442791997u)
							{
								goto IL_6415;
							}
							if (!(text == "note"))
							{
								goto IL_6415;
							}
							if (!Game1.player.archaeologyFound.ContainsKey(102))
							{
								Game1.player.archaeologyFound.Add(102, new int[2]);
							}
							Game1.player.archaeologyFound[102][0] = 18;
							Game1.currentLocation.readNote(Convert.ToInt32(debugSplit[1]));
							goto IL_6415;
						}
						else
						{
							if (!(text == "warp"))
							{
								goto IL_6415;
							}
							Game1.warpFarmer(debugSplit[1], Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3]), false);
							goto IL_6415;
						}
					}
					else if (num <= 2537565969u)
					{
						if (num <= 2513359314u)
						{
							if (num <= 2448968664u)
							{
								if (num != 2445746674u)
								{
									if (num != 2448968664u)
									{
										goto IL_6415;
									}
									if (!(text == "minigame"))
									{
										goto IL_6415;
									}
									text = debugSplit[1];
									if (text == "cowboy")
									{
										Game1.updateViewportForScreenSizeChange(false, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
										Game1.currentMinigame = new AbigailGame(false);
										goto IL_6415;
									}
									if (text == "blastoff")
									{
										Game1.currentMinigame = new RobotBlastoff();
										goto IL_6415;
									}
									if (text == "minecart")
									{
										Game1.currentMinigame = new MineCart(5, 4);
										goto IL_6415;
									}
									if (!(text == "grandpa"))
									{
										goto IL_6415;
									}
									Game1.currentMinigame = new GrandpaStory();
									goto IL_6415;
								}
								else
								{
									if (!(text == "clearMuseum"))
									{
										goto IL_6415;
									}
									(Game1.getLocationFromName("ArchaeologyHouse") as LibraryMuseum).museumPieces.Clear();
									goto IL_6415;
								}
							}
							else if (num != 2465329961u)
							{
								if (num != 2513359314u)
								{
									goto IL_6415;
								}
								if (!(text == "makeEx"))
								{
									goto IL_6415;
								}
								Game1.getCharacterFromName(debugSplit[1], false).divorcedFromFarmer = true;
								goto IL_6415;
							}
							else
							{
								if (!(text == "experience"))
								{
									goto IL_6415;
								}
								goto IL_4F67;
							}
						}
						else if (num <= 2528307764u)
						{
							if (num != 2515311091u)
							{
								if (num != 2528307764u)
								{
									goto IL_6415;
								}
								if (!(text == "killNPC"))
								{
									goto IL_6415;
								}
								for (int i9 = Game1.locations.Count - 1; i9 >= 0; i9--)
								{
									for (int j4 = 0; j4 < Game1.locations[i9].characters.Count; j4++)
									{
										if (Game1.locations[i9].characters[j4].name.Equals(debugSplit[1]))
										{
											Game1.locations[i9].characters.RemoveAt(j4);
											break;
										}
									}
								}
								goto IL_6415;
							}
							else
							{
								if (!(text == "itemnamed"))
								{
									goto IL_6415;
								}
								goto IL_56D5;
							}
						}
						else if (num != 2533728522u)
						{
							if (num != 2537565969u)
							{
								goto IL_6415;
							}
							if (!(text == "befriendAnimals"))
							{
								goto IL_6415;
							}
							if (Game1.currentLocation is AnimalHouse)
							{
								using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator4 = (Game1.currentLocation as AnimalHouse).animals.Values.GetEnumerator())
								{
									while (enumerator4.MoveNext())
									{
										enumerator4.Current.friendshipTowardFarmer = ((debugSplit.Length > 1) ? Convert.ToInt32(debugSplit[1]) : 1000);
									}
									goto IL_6415;
								}
								goto IL_32D6;
							}
							goto IL_6415;
						}
						else
						{
							if (!(text == "setUpFarm"))
							{
								goto IL_6415;
							}
							goto IL_36E2;
						}
					}
					else if (num <= 2594769213u)
					{
						if (num <= 2568788477u)
						{
							if (num != 2562909269u)
							{
								if (num != 2568788477u)
								{
									goto IL_6415;
								}
								if (!(text == "addOtherFarmer"))
								{
									goto IL_6415;
								}
								Farmer f = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2(Game1.player.position.X - (float)Game1.tileSize, Game1.player.position.Y), 2, Dialogue.randomName(), null, true);
								f.changeShirt(Game1.random.Next(40));
								f.changePants(new Color(Game1.random.Next(255), Game1.random.Next(255), Game1.random.Next(255)));
								f.changeHairStyle(Game1.random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
								if (Game1.random.NextDouble() < 0.5)
								{
									f.changeHat(Game1.random.Next(-1, FarmerRenderer.hatsTexture.Height / 80 * 12));
								}
								else
								{
									Game1.player.changeHat(-1);
								}
								f.changeHairColor(new Color(Game1.random.Next(255), Game1.random.Next(255), Game1.random.Next(255)));
								f.changeSkinColor(Game1.random.Next(16));
								f.FarmerSprite.setOwner(f);
								f.currentLocation = Game1.currentLocation;
								Game1.currentLocation.farmers.Add(f);
								Game1.otherFarmers.Add((long)Game1.random.Next(), f);
								goto IL_6415;
							}
							else
							{
								if (!(text == "ambientLight"))
								{
									goto IL_6415;
								}
								goto IL_3C33;
							}
						}
						else if (num != 2573284398u)
						{
							if (num != 2594769213u)
							{
								goto IL_6415;
							}
							if (!(text == "plaque"))
							{
								goto IL_6415;
							}
							(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addStarToPlaque();
							goto IL_6415;
						}
						else
						{
							if (!(text == "slingshot"))
							{
								goto IL_6415;
							}
							goto IL_53B7;
						}
					}
					else if (num <= 2609369173u)
					{
						if (num != 2606942237u)
						{
							if (num != 2609369173u)
							{
								goto IL_6415;
							}
							if (!(text == "cmenu"))
							{
								goto IL_6415;
							}
							goto IL_54BA;
						}
						else
						{
							if (!(text == "addAllCrafting"))
							{
								goto IL_6415;
							}
							using (Dictionary<string, string>.KeyCollection.Enumerator enumerator8 = CraftingRecipe.craftingRecipes.Keys.GetEnumerator())
							{
								while (enumerator8.MoveNext())
								{
									string s2 = enumerator8.Current;
									Game1.player.craftingRecipes.Add(s2, 0);
								}
								goto IL_6415;
							}
							goto IL_613D;
						}
					}
					else if (num != 2609467814u)
					{
						if (num != 2668390727u)
						{
							if (num != 2671260646u)
							{
								goto IL_6415;
							}
							if (!(text == "item"))
							{
								goto IL_6415;
							}
							goto IL_5672;
						}
						else
						{
							if (!(text == "friendship"))
							{
								goto IL_6415;
							}
							goto IL_5951;
						}
					}
					else
					{
						if (!(text == "horse"))
						{
							goto IL_6415;
						}
						Game1.currentLocation.characters.Add(new Horse(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2])));
						goto IL_6415;
					}
					IL_2A7D:
					farm2 = Game1.getFarm();
					for (int x = 0; x < farm2.map.Layers[0].LayerWidth; x++)
					{
						for (int y = 0; y < farm2.map.Layers[0].LayerHeight; y++)
						{
							if (!farm2.terrainFeatures.ContainsKey(new Vector2((float)x, (float)y)) && farm2.doesTileHaveProperty(x, y, "Diggable", "Back") != null && farm2.isTileLocationTotallyClearAndPlaceable(new Vector2((float)x, (float)y)))
							{
								farm2.terrainFeatures.Add(new Vector2((float)x, (float)y), new HoeDirt());
							}
						}
					}
					goto IL_6415;
					IL_3699:
					using (List<Building>.Enumerator enumerator10 = Game1.getFarm().buildings.GetEnumerator())
					{
						while (enumerator10.MoveNext())
						{
							Building b4 = enumerator10.Current;
							if (b4 is Coop)
							{
								b4.daysUntilUpgrade = 1;
							}
						}
						goto IL_6415;
					}
					IL_36E2:
					Game1.getFarm().buildings.Clear();
					for (int x2 = 0; x2 < Game1.getFarm().map.Layers[0].LayerWidth; x2++)
					{
						for (int y2 = 0; y2 < 16 + ((debugSplit.Length > 1) ? 32 : 0); y2++)
						{
							Game1.getFarm().removeEverythingExceptCharactersFromThisTile(x2, y2);
						}
					}
					for (int x3 = 56; x3 < 71; x3++)
					{
						for (int y3 = 17; y3 < 34; y3++)
						{
							Game1.getFarm().removeEverythingExceptCharactersFromThisTile(x3, y3);
							if (x3 > 57 && y3 > 18 && x3 < 70 && y3 < 29)
							{
								Game1.getFarm().terrainFeatures.Add(new Vector2((float)x3, (float)y3), new HoeDirt());
							}
						}
					}
					Game1.getFarm().buildStructure(new BluePrint("Coop"), new Vector2(52f, 11f), false, Game1.player, false);
					Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
					Game1.getFarm().buildStructure(new BluePrint("Silo"), new Vector2(36f, 9f), false, Game1.player, false);
					Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
					Game1.getFarm().buildStructure(new BluePrint("Barn"), new Vector2(42f, 10f), false, Game1.player, false);
					Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
					Game1.player.getToolFromName("Ax").UpgradeLevel = 4;
					Game1.player.getToolFromName("Watering Can").UpgradeLevel = 4;
					Game1.player.getToolFromName("Hoe").UpgradeLevel = 4;
					Game1.player.getToolFromName("Pickaxe").UpgradeLevel = 4;
					Game1.player.Money += 20000;
					Game1.player.addItemToInventoryBool(new Shears(), false);
					Game1.player.addItemToInventoryBool(new MilkPail(), false);
					Game1.player.addItemToInventoryBool(new Object(472, 999, false, -1, 0), false);
					Game1.player.addItemToInventoryBool(new Object(473, 999, false, -1, 0), false);
					Game1.player.addItemToInventoryBool(new Object(322, 999, false, -1, 0), false);
					Game1.player.addItemToInventoryBool(new Object(388, 999, false, -1, 0), false);
					Game1.player.addItemToInventoryBool(new Object(390, 999, false, -1, 0), false);
					goto IL_6415;
					IL_3E41:
					if (Game1.player.getChildren().Count > 0)
					{
						Game1.player.getChildren()[0].age++;
						Game1.player.getChildren()[0].reloadSprite();
						goto IL_6415;
					}
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).characters.Add(new Child("Baby", Game1.random.NextDouble() < 0.5, Game1.random.NextDouble() < 0.5, Game1.player));
					goto IL_6415;
					IL_3FF9:
					Game1.player.changeFriendship(2500, Game1.getCharacterFromName(debugSplit[1], false));
					Game1.player.spouse = debugSplit[1];
					Game1.prepareSpouseForWedding();
					goto IL_6415;
					IL_4162:
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).setWallpaper((debugSplit.Length > 1) ? Convert.ToInt32(debugSplit[1]) : ((Game1.getLocationFromName("FarmHouse") as FarmHouse).wallPaper[0] + 1), -1, true);
					goto IL_6415;
					IL_41A9:
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).setFloor((debugSplit.Length > 1) ? Convert.ToInt32(debugSplit[1]) : ((Game1.getLocationFromName("FarmHouse") as FarmHouse).floor[0] + 1), -1, true);
					goto IL_6415;
					IL_4237:
					Game1.addMailForTomorrow(debugSplit[1].Replace('0', '_'), debugSplit.Length > 2, false);
					goto IL_6415;
					IL_5672:
					if (Game1.objectInformation.ContainsKey(Convert.ToInt32(debugSplit[1])))
					{
						Game1.playSound("coin");
						Game1.player.addItemToInventoryBool(new Object(Convert.ToInt32(debugSplit[1]), (debugSplit.Length >= 3) ? Convert.ToInt32(debugSplit[2]) : 1, false, -1, (debugSplit.Length >= 4) ? Convert.ToInt32(debugSplit[3]) : 0), false);
						goto IL_6415;
					}
					goto IL_6415;
					IL_613D:
					Game1.bloomDay = true;
					Game1.bloom.Visible = true;
					Game1.bloom.Settings.BloomThreshold = (float)(Convert.ToDouble(debugSplit[1]) / 10.0);
					Game1.bloom.Settings.BlurAmount = (float)(Convert.ToDouble(debugSplit[2]) / 10.0);
					Game1.bloom.Settings.BloomIntensity = (float)(Convert.ToDouble(debugSplit[3]) / 10.0);
					Game1.bloom.Settings.BaseIntensity = (float)(Convert.ToDouble(debugSplit[4]) / 10.0);
					Game1.bloom.Settings.BloomSaturation = (float)(Convert.ToDouble(debugSplit[5]) / 10.0);
					Game1.bloom.Settings.BaseSaturation = (float)(Convert.ToDouble(debugSplit[6]) / 10.0);
					Game1.bloom.Settings.brightWhiteOnly = (debugSplit.Length > 7);
					goto IL_6415;
					IL_2D4B:
					Game1.options.zoomLevel = (float)Convert.ToInt32(debugSplit[1]) / 100f;
					this.Window_ClientSizeChanged(null, null);
					goto IL_6415;
					IL_32D6:
					(Game1.getCharacterFromName(Game1.player.getPetName(), false) as Pet).setAtFarmPosition();
					goto IL_6415;
					IL_331D:
					Game1.saveOnNewDay = !Game1.saveOnNewDay;
					if (!Game1.saveOnNewDay)
					{
						Game1.playSound("bigDeSelect");
						goto IL_6415;
					}
					Game1.playSound("bigSelect");
					goto IL_6415;
					IL_35C3:
					Game1.getFarm().shippingBin.Add(new Object(24, 1, false, -1, 0));
					Game1.getFarm().shippingBin.Add(new Object(82, 1, false, -1, 0));
					Game1.getFarm().shippingBin.Add(new Object(136, 1, false, -1, 0));
					Game1.getFarm().shippingBin.Add(new Object(16, 1, false, -1, 0));
					Game1.getFarm().shippingBin.Add(new Object(388, 1, false, -1, 0));
					goto IL_6415;
					IL_3A4E:
					Game1.getFarm().buildStructure(new BluePrint("Coop"), new Vector2((float)Convert.ToInt32(debugSplit[1]), (float)Convert.ToInt32(debugSplit[2])), false, Game1.player, false);
					Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
					goto IL_6415;
					IL_3C33:
					Game1.ambientLight = new Color(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]), Convert.ToInt32(debugSplit[3]));
					goto IL_6415;
					IL_40A2:
					if (debugSplit.Length < 2)
					{
						Furniture fu = null;
						while (fu == null)
						{
							try
							{
								fu = new Furniture(Game1.random.Next(1613), Vector2.Zero);
							}
							catch (Exception)
							{
							}
						}
						Game1.player.addItemToInventoryBool(fu, false);
						goto IL_6415;
					}
					Game1.player.addItemToInventoryBool(new Furniture(Convert.ToInt32(debugSplit[1]), Vector2.Zero), false);
					goto IL_6415;
					IL_42C9:
					Game1.panMode = true;
					Game1.viewportFreeze = true;
					Game1.debugMode = true;
					this.panFacingDirectionWait = false;
					this.panModeString = "";
					goto IL_6415;
					IL_4682:
					using (Dictionary<int, string>.KeyCollection.Enumerator enumerator12 = Game1.objectInformation.Keys.GetEnumerator())
					{
						while (enumerator12.MoveNext())
						{
							int i10 = enumerator12.Current;
							if (Game1.objectInformation[i10].Substring(0, Game1.objectInformation[i10].IndexOf('/')).ToLower().Equals(debugInput.Substring(debugInput.IndexOf(' ') + 1)))
							{
								Game1.debugOutput = debugSplit[1] + " " + i10;
							}
						}
						goto IL_6415;
					}
					IL_4714:
					(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).restoreAreaCutscene(Convert.ToInt32(debugSplit[1]));
					goto IL_6415;
					IL_47C6:
					(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addCharacter(new Junimo(new Vector2((float)Convert.ToInt32(debugSplit[1]), (float)Convert.ToInt32(debugSplit[2])) * (float)Game1.tileSize, Convert.ToInt32(debugSplit[3]), false));
					goto IL_6415;
					IL_4CD8:
					Game1.player.FarmerSprite.pauseForSingleAnimation = true;
					Game1.player.FarmerSprite.setCurrentSingleAnimation(Convert.ToInt32(debugSplit[1]));
					goto IL_6415;
					IL_4D04:
					Game1.pauseTime = 0f;
					Game1.player.eventsSeen.Clear();
					Game1.player.dialogueQuestionsAnswered.Clear();
					Game1.player.mailReceived.Clear();
					Game1.nonWarpFade = true;
					Game1.eventFinished();
					Game1.fadeScreenToBlack();
					Game1.viewportFreeze = false;
					goto IL_6415;
					IL_4EA0:
					Game1.playSound(debugSplit[1]);
					goto IL_6415;
					IL_4F0A:
					using (Dictionary<string, string>.KeyCollection.Enumerator enumerator8 = CraftingRecipe.cookingRecipes.Keys.GetEnumerator())
					{
						while (enumerator8.MoveNext())
						{
							string s3 = enumerator8.Current;
							if (!Game1.player.cookingRecipes.ContainsKey(s3))
							{
								Game1.player.cookingRecipes.Add(s3, 0);
							}
						}
						goto IL_6415;
					}
					IL_4F67:
					Game1.player.gainExperience(Convert.ToInt32(debugSplit[1]), Convert.ToInt32(debugSplit[2]));
					goto IL_6415;
					IL_4F86:
					Game1.stats.FishCaught = (uint)Convert.ToInt32(debugSplit[1]);
					goto IL_6415;
					IL_53B7:
					Game1.player.addItemToInventoryBool(new Slingshot(), false);
					Game1.playSound("coin");
					goto IL_6415;
					IL_5427:
					Game1.activeClickableMenu = new TitleScreenMenu();
					goto IL_6415;
					IL_54BA:
					Game1.activeClickableMenu = new CharacterCustomization(new List<int>
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
					goto IL_6415;
					IL_5615:
					Game1.musicPlayerVolume = (float)Convert.ToDouble(debugSplit[1]);
					Game1.options.musicVolumeLevel = (float)Convert.ToDouble(debugSplit[1]);
					Game1.musicCategory.SetVolume(Game1.options.musicVolumeLevel);
					goto IL_6415;
					IL_56D5:
					using (Dictionary<int, string>.KeyCollection.Enumerator enumerator12 = Game1.objectInformation.Keys.GetEnumerator())
					{
						while (enumerator12.MoveNext())
						{
							int i11 = enumerator12.Current;
							if (Game1.objectInformation[i11].Substring(0, Game1.objectInformation[i11].IndexOf('/')).ToLower().Replace(" ", "").Equals(debugSplit[1]))
							{
								Game1.player.addItemToInventory(new Object(i11, (debugSplit.Length >= 3) ? Convert.ToInt32(debugSplit[2]) : 1, false, -1, (debugSplit.Length >= 4) ? Convert.ToInt32(debugSplit[3]) : 0));
								Game1.playSound("coin");
							}
						}
						goto IL_6415;
					}
					IL_5796:
					Game1.getAchievement(Convert.ToInt32(debugSplit[1]));
					goto IL_6415;
					IL_5951:
					NPC npc = Game1.getCharacterFromName(debugSplit[1], false);
					if (npc != null && !Game1.player.friendships.ContainsKey(npc.name))
					{
						Game1.player.friendships.Add(npc.name, new int[6]);
					}
					Game1.player.friendships[debugSplit[1]][0] = Convert.ToInt32(debugSplit[2]);
					goto IL_6415;
					IL_5A10:
					Game1.player.eventsSeen.Add(Convert.ToInt32(debugSplit[1]));
					goto IL_6415;
					IL_5ABE:
					Game1.player.CoopUpgradeLevel = Math.Min(3, Game1.player.CoopUpgradeLevel + 1);
					Game1.removeFrontLayerForFarmBuildings();
					Game1.addNewFarmBuildingMaps();
					goto IL_6415;
					IL_5C91:
					if (Game1.bigCraftablesInformation.ContainsKey(Convert.ToInt32(debugSplit[1])))
					{
						Game1.playSound("coin");
						Game1.player.addItemToInventory(new Object(Vector2.Zero, Convert.ToInt32(debugSplit[1]), false));
						goto IL_6415;
					}
					goto IL_6415;
					IL_5CD5:
					Game1.isEating = false;
					Game1.player.CanMove = true;
					Game1.player.usingTool = false;
					Game1.player.usingSlingshot = false;
					Game1.player.FarmerSprite.pauseForSingleAnimation = false;
					if (Game1.player.CurrentTool is FishingRod)
					{
						(Game1.player.CurrentTool as FishingRod).isFishing = false;
					}
					if (Game1.player.getMount() != null)
					{
						Game1.player.getMount().dismount();
						goto IL_6415;
					}
					goto IL_6415;
					IL_5F7B:
					if (Game1.getCharacterFromName(debugSplit[1], false) != null)
					{
						Game1.warpCharacter(Game1.getCharacterFromName(debugSplit[1], false), Game1.currentLocation.Name, new Vector2((float)Convert.ToInt32(debugSplit[2]), (float)Convert.ToInt32(debugSplit[3])), false, false);
						Game1.getCharacterFromName(debugSplit[1], false).faceDirection(Convert.ToInt32(debugSplit[4]));
						Game1.getCharacterFromName(debugSplit[1], false).controller = null;
						Game1.getCharacterFromName(debugSplit[1], false).Halt();
						goto IL_6415;
					}
					goto IL_6415;
					IL_62F5:
					Game1.player.addItemToInventoryBool(new Pickaxe(), false);
					Game1.playSound("coin");
					goto IL_6415;
					IL_6356:
					if (debugSplit[1].Equals("farmer"))
					{
						Game1.player.Halt();
						Game1.player.completelyStopAnimatingOrDoingAction();
						Game1.player.faceDirection(Convert.ToInt32(debugSplit[2]));
					}
					else
					{
						Game1.getCharacterFromName(debugSplit[1], false).faceDirection(Convert.ToInt32(debugSplit[2]));
					}
					IL_6415:;
				}
			}
			catch (Exception e)
			{
				Game1.debugOutput = Game1.parseText("Input parsing error... did you type your command correctly? - " + e.Message);
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0009758C File Offset: 0x0009578C
		public void requestDebugInput()
		{
			Game1.activeClickableMenu = new DebugInputMenu(new NamingMenu.doneNamingBehavior(this.parseDebugInput));
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000975A4 File Offset: 0x000957A4
		private void makeCelebrationWeatherDebris()
		{
			Game1.debrisWeather.Clear();
			Game1.isDebrisWeather = true;
			int debrisToMake = Game1.random.Next(80, 100);
			int baseIndex = 22;
			for (int i = 0; i < debrisToMake; i++)
			{
				Game1.debrisWeather.Add(new WeatherDebris(new Vector2((float)Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Width), (float)Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Height)), baseIndex + Game1.random.Next(2), (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00097688 File Offset: 0x00095888
		private void panModeSuccess(KeyboardState currentKBState)
		{
			this.panFacingDirectionWait = false;
			Game1.playSound("smallSelect");
			if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			{
				this.panModeString += " (animation_name_here)";
			}
			Thread expr_46 = new Thread(delegate
			{
				Clipboard.SetText(this.panModeString);
			});
			expr_46.SetApartmentState(ApartmentState.STA);
			expr_46.Start();
			expr_46.Join();
			Game1.debugOutput = this.panModeString;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000976F8 File Offset: 0x000958F8
		private void updatePanModeControls(MouseState currentMouseState, KeyboardState currentKBState)
		{
			if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
			{
				this.requestDebugInput();
				return;
			}
			if (!this.panFacingDirectionWait)
			{
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
				{
					Game1.viewport.Y = Game1.viewport.Y - 16;
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
				{
					Game1.viewport.X = Game1.viewport.X - 16;
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
				{
					Game1.viewport.Y = Game1.viewport.Y + 16;
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
				{
					Game1.viewport.X = Game1.viewport.X + 16;
				}
			}
			else
			{
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
				{
					this.panModeString += "0";
					this.panModeSuccess(currentKBState);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
				{
					this.panModeString += "3";
					this.panModeSuccess(currentKBState);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
				{
					this.panModeString += "2";
					this.panModeSuccess(currentKBState);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
				{
					this.panModeString += "1";
					this.panModeSuccess(currentKBState);
				}
			}
			if (Game1.getMouseX() < Game1.tileSize * 3)
			{
				Game1.viewport.X = Game1.viewport.X - 8;
				Game1.viewport.X = Game1.viewport.X - (Game1.tileSize * 3 - Game1.getMouseX()) / 8;
			}
			if (Game1.getMouseX() > Game1.viewport.Width - Game1.tileSize * 3)
			{
				Game1.viewport.X = Game1.viewport.X + 8;
				Game1.viewport.X = Game1.viewport.X + (Game1.getMouseX() - Game1.viewport.Width + Game1.tileSize * 3) / 8;
			}
			if (Game1.getMouseY() < Game1.tileSize * 3)
			{
				Game1.viewport.Y = Game1.viewport.Y - 8;
				Game1.viewport.Y = Game1.viewport.Y - (Game1.tileSize * 3 - Game1.getMouseY()) / 8;
			}
			if (Game1.getMouseY() > Game1.viewport.Height - Game1.tileSize * 3)
			{
				Game1.viewport.Y = Game1.viewport.Y + 8;
				Game1.viewport.Y = Game1.viewport.Y + (Game1.getMouseY() - Game1.viewport.Height + Game1.tileSize * 3) / 8;
			}
			if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && this.panModeString != null && this.panModeString.Length > 0)
			{
				int x = (Game1.getMouseX() + Game1.viewport.X) / Game1.tileSize;
				int y = (Game1.getMouseY() + Game1.viewport.Y) / Game1.tileSize;
				this.panModeString = string.Concat(new object[]
				{
					this.panModeString,
					Game1.currentLocation.Name,
					" ",
					x,
					" ",
					y,
					" "
				});
				this.panFacingDirectionWait = true;
				Game1.currentLocation.playTerrainSound(new Vector2((float)x, (float)y), null, true);
				Game1.debugOutput = this.panModeString;
			}
			if (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
			{
				int x2 = Game1.getMouseX() + Game1.viewport.X;
				int y2 = Game1.getMouseY() + Game1.viewport.Y;
				Warp w = Game1.currentLocation.isCollidingWithWarpOrDoor(new Microsoft.Xna.Framework.Rectangle(x2, y2, 1, 1));
				if (w != null)
				{
					Game1.currentLocation = Game1.getLocationFromName(w.TargetName);
					Game1.currentLocation.map.LoadTileSheets(Game1.mapDisplayDevice);
					Game1.viewport.X = w.TargetX * Game1.tileSize - Game1.viewport.Width / 2;
					Game1.viewport.Y = w.TargetY * Game1.tileSize - Game1.viewport.Height / 2;
					Game1.playSound("dwop");
				}
			}
			if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				Warp w2 = Game1.currentLocation.warps[0];
				Game1.currentLocation = Game1.getLocationFromName(w2.TargetName);
				Game1.currentLocation.map.LoadTileSheets(Game1.mapDisplayDevice);
				Game1.viewport.X = w2.TargetX * Game1.tileSize - Game1.viewport.Width / 2;
				Game1.viewport.Y = w2.TargetY * Game1.tileSize - Game1.viewport.Height / 2;
				Game1.playSound("dwop");
			}
			if (Game1.viewport.X < -Game1.tileSize)
			{
				Game1.viewport.X = -Game1.tileSize;
			}
			if (Game1.viewport.X + Game1.viewport.Width > Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + Game1.tileSize * 2)
			{
				Game1.viewport.X = Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + Game1.tileSize * 2 - Game1.viewport.Width;
			}
			if (Game1.viewport.Y < -Game1.tileSize)
			{
				Game1.viewport.Y = -Game1.tileSize;
			}
			if (Game1.viewport.Y + Game1.viewport.Height > Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + Game1.tileSize * 2)
			{
				Game1.viewport.Y = Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + Game1.tileSize * 2 - Game1.viewport.Height;
			}
			Game1.oldMouseState = currentMouseState;
			Game1.oldKBState = currentKBState;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00097D18 File Offset: 0x00095F18
		public static bool isLocationAccessible(string locationName)
		{
			if (!(locationName == "CommunityCenter"))
			{
				if (!(locationName == "JojaMart"))
				{
					if (locationName == "Railroad")
					{
						if (Game1.stats.DaysPlayed > 31u)
						{
							return true;
						}
					}
				}
				else if (!Game1.player.eventsSeen.Contains(191393))
				{
					return true;
				}
			}
			else if (Game1.player.eventsSeen.Contains(191393))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00097D90 File Offset: 0x00095F90
		public static bool isGamePadThumbstickInMotion()
		{
			bool release = false;
			if (Game1.thumbstickMotionMargin > 0)
			{
				return true;
			}
			GamePadState p = GamePad.GetState(PlayerIndex.One);
			if ((double)p.ThumbSticks.Left.X < -0.2 || p.IsButtonDown(Buttons.LeftThumbstickLeft))
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Left.X > 0.2 || p.IsButtonDown(Buttons.LeftThumbstickRight))
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Left.Y < -0.2 || p.IsButtonDown(Buttons.LeftThumbstickUp))
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Left.Y > 0.2 || p.IsButtonDown(Buttons.LeftThumbstickDown))
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Right.X < -0.2)
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Right.X > 0.2)
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Right.Y < -0.2)
			{
				release = true;
			}
			if ((double)p.ThumbSticks.Right.Y > 0.2)
			{
				release = true;
			}
			if (release)
			{
				Game1.thumbstickMotionMargin = 50;
			}
			return release;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00097F03 File Offset: 0x00096103
		public static bool isAnyGamePadButtonBeingPressed()
		{
			return Utility.getPressedButtons(GamePad.GetState(PlayerIndex.One), Game1.oldPadState).Count > 0;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00097F1D File Offset: 0x0009611D
		public static bool isAnyGamePadButtonBeingHeld()
		{
			return Utility.getHeldButtons(GamePad.GetState(PlayerIndex.One)).Count > 0;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00097F34 File Offset: 0x00096134
		private void UpdateControlInput(GameTime time)
		{
			if (Game1.paused)
			{
				return;
			}
			KeyboardState currentKBState = Keyboard.GetState();
			MouseState currentMouseState = Mouse.GetState();
			GamePadState currentPadState = GamePad.GetState(PlayerIndex.One);
			if (currentPadState.IsConnected && !Game1.options.gamepadControls)
			{
				Game1.options.gamepadControls = true;
				Game1.showGlobalMessage("Gamepad mode activated");
			}
			else if (!currentPadState.IsConnected && Game1.options.gamepadControls)
			{
				Game1.options.gamepadControls = false;
				Game1.showGlobalMessage("Gamepad disconnected");
				if (Game1.activeClickableMenu == null)
				{
					Game1.activeClickableMenu = new GameMenu();
				}
			}
			bool actionButtonPressed = false;
			bool switchToolButtonPressed = false;
			bool useToolButtonPressed = false;
			bool useToolButtonReleased = false;
			bool addItemToInventoryButtonPressed = false;
			bool cancelButtonPressed = false;
			bool moveupPressed = false;
			bool moverightPressed = false;
			bool moveleftPressed = false;
			bool movedownPressed = false;
			bool moveupReleased = false;
			bool moverightReleased = false;
			bool movedownReleased = false;
			bool moveleftReleased = false;
			bool moveupHeld = false;
			bool moverightHeld = false;
			bool movedownHeld = false;
			bool moveleftHeld = false;
			bool useToolHeld = false;
			bool chatButtonPressed = false;
			if ((Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.actionButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.actionButton)) || (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
			{
				actionButtonPressed = true;
				Game1.rightClickPolling = 250;
			}
			if ((Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.useToolButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.useToolButton)) || (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
			{
				useToolButtonPressed = true;
			}
			if ((Game1.areAllOfTheseKeysUp(currentKBState, Game1.options.useToolButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton)) || (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed))
			{
				useToolButtonReleased = true;
			}
			if ((Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.toolSwapButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.toolSwapButton)) || currentMouseState.ScrollWheelValue != Game1.oldMouseState.ScrollWheelValue)
			{
				switchToolButtonPressed = true;
			}
			if ((Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.cancelButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.cancelButton)) || (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released))
			{
				cancelButtonPressed = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveUpButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveUpButton))
			{
				moveupPressed = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveRightButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveRightButton))
			{
				moverightPressed = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveDownButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveDownButton))
			{
				movedownPressed = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveLeftButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveLeftButton))
			{
				moveleftPressed = true;
			}
			if (Game1.areAllOfTheseKeysUp(currentKBState, Game1.options.moveUpButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton))
			{
				moveupReleased = true;
			}
			if (Game1.areAllOfTheseKeysUp(currentKBState, Game1.options.moveRightButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton))
			{
				moverightReleased = true;
			}
			if (Game1.areAllOfTheseKeysUp(currentKBState, Game1.options.moveDownButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton))
			{
				movedownReleased = true;
			}
			if (Game1.areAllOfTheseKeysUp(currentKBState, Game1.options.moveLeftButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton))
			{
				moveleftReleased = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveUpButton))
			{
				moveupHeld = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveRightButton))
			{
				moverightHeld = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveDownButton))
			{
				movedownHeld = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.moveLeftButton))
			{
				moveleftHeld = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.useToolButton) || currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				useToolHeld = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.chatButton))
			{
				chatButtonPressed = true;
			}
			if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.actionButton) || currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Game1.rightClickPolling -= time.ElapsedGameTime.Milliseconds;
				if (Game1.rightClickPolling <= 0)
				{
					Game1.rightClickPolling = 100;
					actionButtonPressed = true;
				}
			}
			if (Game1.options.gamepadControls)
			{
				if (Game1.isGamePadThumbstickInMotion())
				{
					Mouse.SetPosition((int)((float)currentMouseState.X + currentPadState.ThumbSticks.Right.X * 16f), (int)((float)currentMouseState.Y - currentPadState.ThumbSticks.Right.Y * 16f));
					Game1.lastCursorMotionWasMouse = false;
				}
				if (((Game1.getMouseX() != Game1.getOldMouseX() || Game1.getMouseY() != Game1.getOldMouseY()) && Game1.getMouseX() != 0 && Game1.getMouseY() != 0) || Math.Abs(currentPadState.ThumbSticks.Right.X) > 0f || Math.Abs(currentPadState.ThumbSticks.Right.Y) > 0f)
				{
					if (Game1.timerUntilMouseFade <= 0 && base.IsActive)
					{
						Mouse.SetPosition(Game1.lastMousePositionBeforeFade.X, Game1.lastMousePositionBeforeFade.Y);
					}
					if (!Game1.isGamePadThumbstickInMotion())
					{
						Game1.lastCursorMotionWasMouse = true;
					}
					Game1.timerUntilMouseFade = 4000;
				}
				if (currentKBState.GetPressedKeys().Length != 0 || currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed || currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
				{
					Game1.timerUntilMouseFade = 4000;
				}
				if (currentPadState.IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))
				{
					actionButtonPressed = true;
					Game1.lastCursorMotionWasMouse = false;
					Game1.rightClickPolling = 250;
				}
				if (currentPadState.IsButtonDown(Buttons.X) && !Game1.oldPadState.IsButtonDown(Buttons.X))
				{
					useToolButtonPressed = true;
					Game1.lastCursorMotionWasMouse = false;
				}
				if (!currentPadState.IsButtonDown(Buttons.X) && Game1.oldPadState.IsButtonDown(Buttons.X))
				{
					useToolButtonReleased = true;
				}
				if (currentPadState.IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger))
				{
					switchToolButtonPressed = true;
					Game1.triggerPolling = 300;
				}
				else if (currentPadState.IsButtonDown(Buttons.LeftTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.LeftTrigger))
				{
					switchToolButtonPressed = true;
					Game1.triggerPolling = 300;
				}
				if (currentPadState.IsButtonDown(Buttons.X))
				{
					useToolHeld = true;
				}
				if (currentPadState.IsButtonDown(Buttons.A))
				{
					Game1.rightClickPolling -= time.ElapsedGameTime.Milliseconds;
					if (Game1.rightClickPolling <= 0)
					{
						Game1.rightClickPolling = 100;
						actionButtonPressed = true;
					}
				}
				if (currentPadState.IsButtonDown(Buttons.RightTrigger) || currentPadState.IsButtonDown(Buttons.LeftTrigger))
				{
					Game1.triggerPolling -= time.ElapsedGameTime.Milliseconds;
					if (Game1.triggerPolling <= 0)
					{
						Game1.triggerPolling = 100;
						switchToolButtonPressed = true;
					}
				}
				if (currentPadState.IsButtonDown(Buttons.DPadUp) && !Game1.oldPadState.IsButtonDown(Buttons.DPadUp))
				{
					moveupPressed = true;
				}
				else if (!currentPadState.IsButtonDown(Buttons.DPadUp) && Game1.oldPadState.IsButtonDown(Buttons.DPadUp))
				{
					moveupReleased = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadRight) && !Game1.oldPadState.IsButtonDown(Buttons.DPadRight))
				{
					moverightPressed = true;
				}
				else if (!currentPadState.IsButtonDown(Buttons.DPadRight) && Game1.oldPadState.IsButtonDown(Buttons.DPadRight))
				{
					moverightReleased = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadDown) && !Game1.oldPadState.IsButtonDown(Buttons.DPadDown))
				{
					movedownPressed = true;
				}
				else if (!currentPadState.IsButtonDown(Buttons.DPadDown) && Game1.oldPadState.IsButtonDown(Buttons.DPadDown))
				{
					movedownReleased = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadLeft) && !Game1.oldPadState.IsButtonDown(Buttons.DPadLeft))
				{
					moveleftPressed = true;
				}
				else if (!currentPadState.IsButtonDown(Buttons.DPadLeft) && Game1.oldPadState.IsButtonDown(Buttons.DPadLeft))
				{
					moveleftReleased = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadUp))
				{
					moveupHeld = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadRight))
				{
					moverightHeld = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadDown))
				{
					movedownHeld = true;
				}
				if (currentPadState.IsButtonDown(Buttons.DPadLeft))
				{
					moveleftHeld = true;
				}
				if ((double)currentPadState.ThumbSticks.Left.X < -0.2)
				{
					moveleftPressed = true;
					moveleftHeld = true;
				}
				else if ((double)currentPadState.ThumbSticks.Left.X > 0.2)
				{
					moverightPressed = true;
					moverightHeld = true;
				}
				if ((double)currentPadState.ThumbSticks.Left.Y < -0.2)
				{
					movedownPressed = true;
					movedownHeld = true;
				}
				else if ((double)currentPadState.ThumbSticks.Left.Y > 0.2)
				{
					moveupPressed = true;
					moveupHeld = true;
				}
				if ((double)Game1.oldPadState.ThumbSticks.Left.X < -0.2 && !moveleftHeld)
				{
					moveleftReleased = true;
				}
				if ((double)Game1.oldPadState.ThumbSticks.Left.X > 0.2 && !moverightHeld)
				{
					moverightReleased = true;
				}
				if ((double)Game1.oldPadState.ThumbSticks.Left.Y < -0.2 && !movedownHeld)
				{
					movedownReleased = true;
				}
				if ((double)Game1.oldPadState.ThumbSticks.Left.Y > 0.2 && !moveupHeld)
				{
					moveupReleased = true;
				}
			}
			if (useToolHeld)
			{
				Game1.mouseClickPolling += time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				Game1.mouseClickPolling = 0;
			}
			if (Game1.mouseClickPolling > 250 && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is MeleeWeapon)) && (Game1.player.CurrentTool == null || Game1.player.CurrentTool.GetType() != typeof(FishingRod) || Game1.player.CurrentTool.upgradeLevel <= 0))
			{
				useToolButtonPressed = true;
				Game1.mouseClickPolling = 100;
			}
			if (Game1.displayHUD)
			{
				foreach (IClickableMenu menu in Game1.onScreenMenus)
				{
					if (menu.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
					{
						menu.performHoverAction(Game1.getMouseX(), Game1.getMouseY());
					}
				}
				if (chatButtonPressed)
				{
					using (List<IClickableMenu>.Enumerator enumerator = Game1.onScreenMenus.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IClickableMenu menu2 = enumerator.Current;
							if (menu2 is ChatBox)
							{
								((ChatBox)menu2).chatBox.Selected = true;
								Game1.isChatting = true;
								if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemQuestion))
								{
									((ChatBox)menu2).chatBox.Text = "/";
								}
								return;
							}
						}
						goto IL_ACB;
					}
				}
				if (Game1.isChatting && currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
				{
					Game1.ChatBox.chatBox.Selected = false;
					Game1.isChatting = false;
					Game1.oldKBState = currentKBState;
					return;
				}
			}
			IL_ACB:
			if (Game1.panMode)
			{
				this.updatePanModeControls(currentMouseState, currentKBState);
				return;
			}
			if (useToolButtonReleased && Game1.player.CurrentTool != null && Game1.CurrentEvent == null && Game1.pauseTime <= 0f && Game1.player.CurrentTool.onRelease(Game1.currentLocation, Game1.getMouseX(), Game1.getMouseY(), Game1.player))
			{
				Game1.oldMouseState = currentMouseState;
				Game1.oldKBState = currentKBState;
				Game1.oldPadState = currentPadState;
				Game1.player.usingSlingshot = false;
				Game1.player.canReleaseTool = true;
				Game1.player.usingTool = false;
				Game1.player.CanMove = true;
				return;
			}
			if (((useToolButtonPressed && !Game1.isAnyGamePadButtonBeingPressed()) || (actionButtonPressed && Game1.isAnyGamePadButtonBeingPressed())) && Game1.pauseTime <= 0f && Game1.displayHUD)
			{
				foreach (IClickableMenu menu3 in Game1.onScreenMenus)
				{
					if (!(menu3 is ChatBox) && (!(menu3 is LevelUpMenu) || (menu3 as LevelUpMenu).informationUp) && menu3.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
					{
						menu3.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
						Game1.oldMouseState = currentMouseState;
						if (!Game1.isAnyGamePadButtonBeingPressed())
						{
							return;
						}
					}
					menu3.clickAway();
				}
			}
			if (Game1.isChatting || Game1.player.freezePause > 0)
			{
				Game1.oldMouseState = currentMouseState;
				Game1.oldKBState = currentKBState;
				Game1.oldPadState = currentPadState;
				return;
			}
			if (Game1.eventUp && (actionButtonPressed | useToolButtonPressed))
			{
				Game1.currentLocation.currentEvent.receiveMouseClick(Game1.getMouseX(), Game1.getMouseY());
			}
			if (actionButtonPressed || (Game1.dialogueUp & useToolButtonPressed))
			{
				foreach (IClickableMenu menu4 in Game1.onScreenMenus)
				{
					if (Game1.displayHUD && menu4.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()) && (!(menu4 is LevelUpMenu) || (menu4 as LevelUpMenu).informationUp))
					{
						menu4.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
						Game1.oldMouseState = currentMouseState;
						if (!Game1.isAnyGamePadButtonBeingPressed())
						{
							return;
						}
					}
				}
				if (!Game1.pressActionButton(currentKBState, currentMouseState, currentPadState))
				{
					return;
				}
			}
			else
			{
				if (useToolButtonPressed && (!Game1.player.UsingTool || (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon)) && !Game1.pickingTool && !Game1.dialogueUp && !Game1.menuUp && (Game1.player.CanMove || (Game1.player.CurrentTool != null && (Game1.player.CurrentTool.Name.Equals("Fishing Rod") || Game1.player.CurrentTool is MeleeWeapon))))
				{
					if (Game1.player.CurrentTool != null)
					{
						Game1.player.CurrentTool.leftClick(Game1.player);
					}
					if (!Game1.pressUseToolButton() && Game1.player.canReleaseTool && Game1.player.usingTool)
					{
						Tool arg_DFB_0 = Game1.player.CurrentTool;
					}
					Game1.oldMouseState = currentMouseState;
					if (Game1.mouseClickPolling < 100)
					{
						Game1.oldKBState = currentKBState;
					}
					Game1.oldPadState = currentPadState;
					return;
				}
				if (useToolButtonReleased && Game1.player.canReleaseTool && Game1.player.usingTool && Game1.player.CurrentTool != null)
				{
					Game1.releaseUseToolButton();
				}
				else if (switchToolButtonPressed && !Game1.player.UsingTool && !Game1.dialogueUp && (Game1.pickingTool || Game1.player.CanMove) && !Game1.player.areAllItemsNull() && !Game1.eventUp)
				{
					Game1.pressSwitchToolButton();
				}
				else if ((!switchToolButtonPressed || Game1.player.ActiveObject == null || Game1.dialogueUp || Game1.eventUp) && addItemToInventoryButtonPressed && !Game1.pickingTool && !Game1.eventUp && !Game1.player.UsingTool && Game1.player.CanMove)
				{
					Game1.pressAddItemToInventoryButton();
				}
			}
			if (cancelButtonPressed)
			{
				if (Game1.numberOfSelectedItems != -1)
				{
					Game1.numberOfSelectedItems = -1;
					Game1.dialogueUp = false;
					Game1.player.CanMove = true;
				}
				else if (Game1.nameSelectUp && NameSelect.cancel())
				{
					Game1.nameSelectUp = false;
					Game1.playSound("bigDeSelect");
				}
			}
			if ((Game1.player.CurrentTool != null & useToolHeld) && Game1.player.canReleaseTool && !Game1.eventUp && !Game1.dialogueUp && !Game1.menuUp && Game1.player.Stamina >= 1f && !(Game1.player.CurrentTool is FishingRod))
			{
				if (Game1.player.toolHold <= 0 && Game1.player.CurrentTool.upgradeLevel > Game1.player.toolPower)
				{
					Game1.player.toolHold = 600;
				}
				else if (Game1.player.CurrentTool.upgradeLevel > Game1.player.toolPower)
				{
					Game1.player.toolHold -= time.ElapsedGameTime.Milliseconds;
					if (Game1.player.toolHold <= 0)
					{
						Game1.player.toolPowerIncrease();
					}
				}
			}
			if (Game1.upPolling >= 650f)
			{
				moveupPressed = true;
				Game1.upPolling -= 100f;
			}
			else if (Game1.downPolling >= 650f)
			{
				movedownPressed = true;
				Game1.downPolling -= 100f;
			}
			else if (Game1.rightPolling >= 650f)
			{
				moverightPressed = true;
				Game1.rightPolling -= 100f;
			}
			else if (Game1.leftPolling >= 650f)
			{
				moveleftPressed = true;
				Game1.leftPolling -= 100f;
			}
			else if (!Game1.nameSelectUp && Game1.pauseTime <= 0f && (!Game1.eventUp || (Game1.CurrentEvent != null && Game1.CurrentEvent.playerControlSequence && !Game1.player.usingTool)))
			{
				if (Game1.player.movementDirections.Count < 2)
				{
					int old = Game1.player.movementDirections.Count;
					if (moveupHeld)
					{
						Game1.player.setMoving(1);
						if (Game1.IsClient)
						{
							Game1.client.sendMessage(0, new object[]
							{
								1
							});
						}
					}
					if (moverightHeld)
					{
						Game1.player.setMoving(2);
						if (Game1.IsClient)
						{
							Game1.client.sendMessage(0, new object[]
							{
								2
							});
						}
					}
					if (movedownHeld)
					{
						Game1.player.setMoving(4);
						if (Game1.IsClient)
						{
							Game1.client.sendMessage(0, new object[]
							{
								4
							});
						}
					}
					if (moveleftHeld)
					{
						Game1.player.setMoving(8);
						if (Game1.IsClient)
						{
							Game1.client.sendMessage(0, new object[]
							{
								8
							});
						}
					}
					if (old == 0 && Game1.player.movementDirections.Count > 0 && Game1.player.running)
					{
						Game1.player.FarmerSprite.nextOffset = 1;
					}
				}
				if (moveupReleased || (Game1.player.movementDirections.Contains(0) && !moveupHeld))
				{
					Game1.player.setMoving(33);
					if (Game1.IsClient)
					{
						Game1.client.sendMessage(0, new object[]
						{
							33
						});
					}
					else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
					{
						Game1.player.setMoving(64);
					}
				}
				if (moverightReleased || (Game1.player.movementDirections.Contains(1) && !moverightHeld))
				{
					Game1.player.setMoving(34);
					if (Game1.IsClient)
					{
						Game1.client.sendMessage(0, new object[]
						{
							34
						});
					}
					else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
					{
						Game1.player.setMoving(64);
					}
				}
				if (movedownReleased || (Game1.player.movementDirections.Contains(2) && !movedownHeld))
				{
					Game1.player.setMoving(36);
					if (Game1.IsClient)
					{
						Game1.client.sendMessage(0, new object[]
						{
							36
						});
					}
					else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
					{
						Game1.player.setMoving(64);
					}
				}
				if (moveleftReleased || (Game1.player.movementDirections.Contains(3) && !moveleftHeld))
				{
					Game1.player.setMoving(40);
					if (Game1.IsClient)
					{
						Game1.client.sendMessage(0, new object[]
						{
							40
						});
					}
					else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
					{
						Game1.player.setMoving(64);
					}
				}
				if (!moveupHeld && !moverightHeld && !movedownHeld && !moveleftHeld && !Game1.player.UsingTool)
				{
					Game1.player.Halt();
				}
			}
			else if (Game1.isQuestion)
			{
				if (moveupPressed)
				{
					Game1.currentQuestionChoice = Math.Max(Game1.currentQuestionChoice - 1, 0);
					Game1.playSound("toolSwap");
				}
				else if (movedownPressed)
				{
					Game1.currentQuestionChoice = Math.Min(Game1.currentQuestionChoice + 1, Game1.questionChoices.Count - 1);
					Game1.playSound("toolSwap");
				}
			}
			else if (Game1.numberOfSelectedItems != -1 && !Game1.dialogueTyping)
			{
				int max = 99;
				if (Game1.selectedItemsType.Equals("Animal Food"))
				{
					max = 999 - Game1.player.Feed;
				}
				else if (Game1.selectedItemsType.Equals("calicoJackBet"))
				{
					max = Math.Min(Game1.player.clubCoins, 999);
				}
				else if (Game1.selectedItemsType.Equals("flutePitch"))
				{
					max = 26;
				}
				else if (Game1.selectedItemsType.Equals("drumTone"))
				{
					max = 6;
				}
				else if (Game1.selectedItemsType.Equals("jukebox"))
				{
					max = Game1.player.songsHeard.Count - 1;
				}
				else if (Game1.selectedItemsType.Equals("Fuel"))
				{
					max = 100 - ((Lantern)Game1.player.getToolFromName("Lantern")).fuelLeft;
				}
				if (moverightPressed)
				{
					Game1.numberOfSelectedItems = Math.Min(Game1.numberOfSelectedItems + 1, max);
					Game1.playItemNumberSelectSound();
				}
				else if (moveleftPressed)
				{
					Game1.numberOfSelectedItems = Math.Max(Game1.numberOfSelectedItems - 1, 0);
					Game1.playItemNumberSelectSound();
				}
				else if (moveupPressed)
				{
					Game1.numberOfSelectedItems = Math.Min(Game1.numberOfSelectedItems + 10, max);
					Game1.playItemNumberSelectSound();
				}
				else if (movedownPressed)
				{
					Game1.numberOfSelectedItems = Math.Max(Game1.numberOfSelectedItems - 10, 0);
					Game1.playItemNumberSelectSound();
				}
			}
			if (moveupHeld && !Game1.player.CanMove)
			{
				Game1.upPolling += (float)time.ElapsedGameTime.Milliseconds;
			}
			else if (movedownHeld && !Game1.player.CanMove)
			{
				Game1.downPolling += (float)time.ElapsedGameTime.Milliseconds;
			}
			else if (moverightHeld && !Game1.player.CanMove)
			{
				Game1.rightPolling += (float)time.ElapsedGameTime.Milliseconds;
			}
			else if (moveleftHeld && !Game1.player.CanMove)
			{
				Game1.leftPolling += (float)time.ElapsedGameTime.Milliseconds;
			}
			else if (moveupReleased)
			{
				Game1.upPolling = 0f;
			}
			else if (movedownReleased)
			{
				Game1.downPolling = 0f;
			}
			else if (moverightReleased)
			{
				Game1.rightPolling = 0f;
			}
			else if (moveleftReleased)
			{
				Game1.leftPolling = 0f;
			}
			if (Game1.debugMode)
			{
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
				{
					Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P))
				{
					Game1.NewDay(0f);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M))
				{
					Game1.dayOfMonth = 28;
					Game1.NewDay(0f);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T))
				{
					Game1.timeOfDay += 100;
					foreach (GameLocation g in Game1.locations)
					{
						for (int i = 0; i < g.getCharacters().Count; i++)
						{
							g.getCharacters()[i].checkSchedule(Game1.timeOfDay);
							g.getCharacters()[i].checkSchedule(Game1.timeOfDay - 50);
							g.getCharacters()[i].checkSchedule(Game1.timeOfDay - 60);
							g.getCharacters()[i].checkSchedule(Game1.timeOfDay - 70);
							g.getCharacters()[i].checkSchedule(Game1.timeOfDay - 80);
							g.getCharacters()[i].checkSchedule(Game1.timeOfDay - 90);
						}
					}
					int num = Game1.timeOfDay;
					if (num <= 2000)
					{
						if (num != 1900)
						{
							if (num == 2000)
							{
								Game1.globalOutdoorLighting = 0.7f;
								if (!Game1.isRaining)
								{
									Game1.changeMusicTrack("none");
								}
							}
						}
						else
						{
							Game1.globalOutdoorLighting = 0.5f;
							Game1.currentLocation.switchOutNightTiles();
						}
					}
					else if (num != 2100)
					{
						if (num == 2200)
						{
							Game1.globalOutdoorLighting = 1f;
						}
					}
					else
					{
						Game1.globalOutdoorLighting = 0.9f;
					}
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
				{
					if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
					{
						Game1.timeOfDay -= 10;
					}
					else
					{
						Game1.timeOfDay += 10;
					}
					if (Game1.timeOfDay % 100 == 60)
					{
						Game1.timeOfDay += 40;
					}
					if (Game1.timeOfDay % 100 == 90)
					{
						Game1.timeOfDay -= 40;
					}
					Game1.currentLocation.performTenMinuteUpdate(Game1.timeOfDay);
					foreach (GameLocation g2 in Game1.locations)
					{
						for (int j = 0; j < g2.getCharacters().Count; j++)
						{
							g2.getCharacters()[j].checkSchedule(Game1.timeOfDay);
						}
					}
					if (Game1.isLightning)
					{
						Utility.performLightningUpdate();
					}
					int num = Game1.timeOfDay;
					if (num <= 1900)
					{
						if (num != 1750)
						{
							if (num == 1900)
							{
								Game1.globalOutdoorLighting = 0.5f;
								Game1.currentLocation.switchOutNightTiles();
							}
						}
						else
						{
							Game1.globalOutdoorLighting = 0f;
							Game1.outdoorLight = Color.White;
						}
					}
					else if (num != 2000)
					{
						if (num != 2100)
						{
							if (num == 2200)
							{
								Game1.globalOutdoorLighting = 1f;
							}
						}
						else
						{
							Game1.globalOutdoorLighting = 0.9f;
						}
					}
					else
					{
						Game1.globalOutdoorLighting = 0.7f;
						if (!Game1.isRaining)
						{
							Game1.changeMusicTrack("none");
						}
					}
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1))
				{
					Game1.warpFarmer("Mountain", 15, 35, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2))
				{
					Game1.warpFarmer("Town", 35, 35, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3))
				{
					Game1.warpFarmer("Farm", 64, 15, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4))
				{
					Game1.warpFarmer("Forest", 34, 13, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D5) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4))
				{
					Game1.warpFarmer("Beach", 34, 10, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D6) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D6))
				{
					Game1.warpFarmer("Mine", 18, 12, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D7) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D7))
				{
					Game1.warpFarmer("SandyHouse", 16, 3, false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.K) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.K))
				{
					if (Game1.mine == null)
					{
						Game1.mine = new MineShaft();
					}
					Game1.enterMine(false, Game1.mine.mineLevel + 1, null);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H))
				{
					Game1.player.changeHat(Game1.random.Next(FarmerRenderer.hatsTexture.Height / 80 * 12));
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I))
				{
					Game1.player.changeHairStyle(Game1.random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J))
				{
					Game1.player.changeShirt(Game1.random.Next(40));
					Game1.player.changePants(new Color(Game1.random.Next(255), Game1.random.Next(255), Game1.random.Next(255)));
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L))
				{
					Game1.player.changeShirt(Game1.random.Next(40));
					Game1.player.changePants(new Color(Game1.random.Next(255), Game1.random.Next(255), Game1.random.Next(255)));
					Game1.player.changeHairStyle(Game1.random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
					if (Game1.random.NextDouble() < 0.5)
					{
						Game1.player.changeHat(Game1.random.Next(-1, FarmerRenderer.hatsTexture.Height / 80 * 12));
					}
					else
					{
						Game1.player.changeHat(-1);
					}
					Game1.player.changeHairColor(new Color(Game1.random.Next(255), Game1.random.Next(255), Game1.random.Next(255)));
					Game1.player.changeSkinColor(Game1.random.Next(16));
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U))
				{
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).setWallpaper(Game1.random.Next(112), -1, true);
					(Game1.getLocationFromName("FarmHouse") as FarmHouse).setFloor(Game1.random.Next(40), -1, true);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2))
				{
					Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F5) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F5))
				{
					Game1.displayFarmer = !Game1.displayFarmer;
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F6))
				{
					Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F6);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F7) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F7))
				{
					Game1.drawGrid = !Game1.drawGrid;
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
				{
					Game1.player.shiftToolbar(false);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N))
				{
					Game1.player.shiftToolbar(true);
				}
				if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F10) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F10))
				{
					if (Game1.server == null)
					{
						Game1.multiplayerMode = 2;
						Game1.server.initializeConnection();
					}
					if (Game1.ChatBox == null)
					{
						Game1.onScreenMenus.Add(new ChatBox());
					}
				}
			}
			else if (!Game1.player.UsingTool)
			{
				if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot1) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot1))
				{
					Game1.player.CurrentToolIndex = 0;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot2) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot2))
				{
					Game1.player.CurrentToolIndex = 1;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot3) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot3))
				{
					Game1.player.CurrentToolIndex = 2;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot4) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot4))
				{
					Game1.player.CurrentToolIndex = 3;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot5) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot5))
				{
					Game1.player.CurrentToolIndex = 4;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot6) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot6))
				{
					Game1.player.CurrentToolIndex = 5;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot7) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot7))
				{
					Game1.player.CurrentToolIndex = 6;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot8) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot8))
				{
					Game1.player.CurrentToolIndex = 7;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot9) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot9))
				{
					Game1.player.CurrentToolIndex = 8;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot10) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot10))
				{
					Game1.player.CurrentToolIndex = 9;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot11) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot11))
				{
					Game1.player.CurrentToolIndex = 10;
				}
				else if (Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.inventorySlot12) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot12))
				{
					Game1.player.CurrentToolIndex = 11;
				}
			}
			if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F3) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F3) && !Program.releaseBuild)
			{
				Game1.debugMode = !Game1.debugMode;
				if (Game1.gameMode == 11)
				{
					Game1.gameMode = 3;
				}
			}
			if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8) && !Program.releaseBuild)
			{
				this.requestDebugInput();
			}
			if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F4) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F4))
			{
				Game1.displayHUD = !Game1.displayHUD;
				Game1.playSound("smallSelect");
				if (!Game1.displayHUD)
				{
					Game1.showGlobalMessage("Screenshot mode activated. Press 'F4' to exit.");
				}
			}
			bool menuButtonPressed = Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.menuButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.menuButton);
			bool journalButtonPressed = Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.journalButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.journalButton);
			bool mapButtonPressed = Game1.isOneOfTheseKeysDown(currentKBState, Game1.options.mapButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.mapButton);
			if (Game1.options.gamepadControls && !menuButtonPressed)
			{
				menuButtonPressed = ((currentPadState.IsButtonDown(Buttons.Start) && !Game1.oldPadState.IsButtonDown(Buttons.Start)) || (currentPadState.IsButtonDown(Buttons.B) && !Game1.oldPadState.IsButtonDown(Buttons.B)));
			}
			if (Game1.options.gamepadControls && !journalButtonPressed)
			{
				journalButtonPressed = (currentPadState.IsButtonDown(Buttons.Back) && !Game1.oldPadState.IsButtonDown(Buttons.Back));
			}
			if (Game1.options.gamepadControls && !mapButtonPressed)
			{
				mapButtonPressed = (currentPadState.IsButtonDown(Buttons.Y) && !Game1.oldPadState.IsButtonDown(Buttons.Y));
			}
			if (((Game1.dayOfMonth > 0 && Game1.player.CanMove) & menuButtonPressed) && !Game1.dialogueUp && (!Game1.eventUp || (Game1.isFestival() && Game1.CurrentEvent.festivalTimer <= 0)) && Game1.currentMinigame == null)
			{
				if (Game1.activeClickableMenu == null)
				{
					Game1.activeClickableMenu = new GameMenu();
				}
				else if (Game1.activeClickableMenu.readyToClose())
				{
					Game1.exitActiveMenu();
				}
			}
			if (((Game1.dayOfMonth > 0 && Game1.player.CanMove) & journalButtonPressed) && !Game1.dialogueUp && !Game1.eventUp && Game1.activeClickableMenu == null)
			{
				Game1.activeClickableMenu = new QuestLog();
			}
			if (((Game1.options.gamepadControls && Game1.dayOfMonth > 0 && Game1.player.CanMove && Game1.isAnyGamePadButtonBeingPressed()) & mapButtonPressed) && !Game1.dialogueUp && !Game1.eventUp)
			{
				if (Game1.activeClickableMenu == null)
				{
					Game1.activeClickableMenu = new GameMenu(4, -1);
				}
			}
			else if (((Game1.dayOfMonth > 0 && Game1.player.CanMove) & mapButtonPressed) && !Game1.dialogueUp && !Game1.eventUp && Game1.activeClickableMenu == null)
			{
				Game1.activeClickableMenu = new GameMenu(3, -1);
			}
			Game1.checkForRunButton(currentKBState, false);
			Game1.oldKBState = currentKBState;
			Game1.oldMouseState = currentMouseState;
			Game1.oldPadState = currentPadState;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0009A488 File Offset: 0x00098688
		public static void checkForRunButton(KeyboardState kbState, bool ignoreKeyPressQualifier = false)
		{
			bool wasRunning = Game1.player.running;
			bool runPressed = Game1.isOneOfTheseKeysDown(kbState, Game1.options.runButton) && (!Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.runButton) | ignoreKeyPressQualifier);
			bool runReleased = !Game1.isOneOfTheseKeysDown(kbState, Game1.options.runButton) && (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.runButton) | ignoreKeyPressQualifier);
			if (Game1.options.gamepadControls)
			{
				if (!Game1.options.autoRun && Math.Abs(Vector2.Distance(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left, Vector2.Zero)) > 0.9f)
				{
					runPressed = true;
				}
				else if (Math.Abs(Vector2.Distance(Game1.oldPadState.ThumbSticks.Left, Vector2.Zero)) > 0.9f && Math.Abs(Vector2.Distance(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left, Vector2.Zero)) <= 0.9f)
				{
					runReleased = true;
				}
			}
			if (runPressed && !Game1.player.canOnlyWalk)
			{
				Game1.player.setRunning(!Game1.options.autoRun, false);
				if (Game1.IsClient)
				{
					Game1.client.sendMessage(0, new object[]
					{
						Game1.player.running ? 16 : 48
					});
				}
				else if (Game1.IsServer)
				{
					Game1.player.setMoving(Game1.player.running ? 16 : 48);
				}
			}
			else if (runReleased && !Game1.player.canOnlyWalk)
			{
				Game1.player.setRunning(Game1.options.autoRun, false);
				if (Game1.IsClient)
				{
					Game1.client.sendMessage(0, new object[]
					{
						Game1.player.running ? 16 : 48
					});
				}
				else if (Game1.IsServer)
				{
					Game1.player.setMoving(Game1.player.running ? 16 : 48);
				}
			}
			if (Game1.player.running != wasRunning && !Game1.player.usingTool)
			{
				Game1.player.Halt();
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00002834 File Offset: 0x00000A34
		public static void drawTitleScreenBackground(GameTime gameTime, string dayNight, int weatherDebrisOffsetDay)
		{
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0009A6C8 File Offset: 0x000988C8
		public static Vector2 getMostRecentViewportMotion()
		{
			return new Vector2((float)Game1.viewport.X - Game1.previousViewportPosition.X, (float)Game1.viewport.Y - Game1.previousViewportPosition.Y);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0009A6FC File Offset: 0x000988FC
		protected override void Draw(GameTime gameTime)
		{
			if (Game1.options.zoomLevel != 1f)
			{
				base.GraphicsDevice.SetRenderTarget(this.screen);
			}
			Game1.framesThisSecond++;
			base.GraphicsDevice.Clear(this.bgColor);
			if (Game1.options.showMenuBackground && Game1.activeClickableMenu != null && Game1.activeClickableMenu.showWithoutTransparencyIfOptionIsSet())
			{
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.activeClickableMenu.drawBackground(Game1.spriteBatch);
				Game1.activeClickableMenu.draw(Game1.spriteBatch);
				Game1.spriteBatch.End();
				if (Game1.options.zoomLevel != 1f)
				{
					base.GraphicsDevice.SetRenderTarget(null);
					base.GraphicsDevice.Clear(this.bgColor);
					Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					Game1.spriteBatch.Draw(this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
					Game1.spriteBatch.End();
				}
				return;
			}
			if (Game1.gameMode == 11)
			{
				Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				Game1.spriteBatch.DrawString(Game1.smoothFont, "Stardew Valley has crashed...", new Vector2(16f, 16f), Color.HotPink);
				Game1.spriteBatch.DrawString(Game1.smoothFont, "Please send the error report or a screenshot of this message to @ConcernedApe. (http://stardewvalley.net/contact/)", new Vector2(16f, 32f), new Color(0, 255, 0));
				Game1.spriteBatch.DrawString(Game1.smoothFont, Game1.parseText(Game1.errorMessage, Game1.smoothFont, Game1.graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
				Game1.spriteBatch.End();
				return;
			}
			if (Game1.currentMinigame != null)
			{
				Game1.currentMinigame.draw(Game1.spriteBatch);
				if (Game1.globalFade && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
				{
					Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((Game1.gameMode == 0) ? (1f - Game1.fadeToBlackAlpha) : Game1.fadeToBlackAlpha));
					Game1.spriteBatch.End();
				}
				if (Game1.options.zoomLevel != 1f)
				{
					base.GraphicsDevice.SetRenderTarget(null);
					base.GraphicsDevice.Clear(this.bgColor);
					Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					Game1.spriteBatch.Draw(this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
					Game1.spriteBatch.End();
				}
				return;
			}
			if (Game1.showingEndOfNightStuff)
			{
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				if (Game1.activeClickableMenu != null)
				{
					Game1.activeClickableMenu.draw(Game1.spriteBatch);
				}
				Game1.spriteBatch.End();
				if (Game1.options.zoomLevel != 1f)
				{
					base.GraphicsDevice.SetRenderTarget(null);
					base.GraphicsDevice.Clear(this.bgColor);
					Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					Game1.spriteBatch.Draw(this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
					Game1.spriteBatch.End();
				}
				return;
			}
			if (Game1.gameMode == 6)
			{
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				string addOn = "";
				int i = 0;
				while ((double)i < gameTime.TotalGameTime.TotalMilliseconds % 999.0 / 333.0)
				{
					addOn += ".";
					i++;
				}
				SpriteText.drawString(Game1.spriteBatch, "Loading" + addOn, 64, Game1.graphics.GraphicsDevice.Viewport.Height - 64, 999, -1, 999, 1f, 1f, false, 0, "Loading...", -1);
				Game1.spriteBatch.End();
				if (Game1.options.zoomLevel != 1f)
				{
					base.GraphicsDevice.SetRenderTarget(null);
					base.GraphicsDevice.Clear(this.bgColor);
					Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
					Game1.spriteBatch.Draw(this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
					Game1.spriteBatch.End();
				}
				return;
			}
			if (Game1.gameMode == 0)
			{
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			}
			else
			{
				if (Game1.drawLighting)
				{
					base.GraphicsDevice.SetRenderTarget(Game1.lightmap);
					base.GraphicsDevice.Clear(Color.White * 0f);
					Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
					Game1.spriteBatch.Draw(Game1.staminaRect, Game1.lightmap.Bounds, Game1.currentLocation.name.Equals("UndergroundMine") ? Game1.mine.getLightingColor(gameTime) : ((!Game1.ambientLight.Equals(Color.White) && (!Game1.isRaining || !Game1.currentLocation.isOutdoors)) ? Game1.ambientLight : Game1.outdoorLight));
					for (int j = 0; j < Game1.currentLightSources.Count; j++)
					{
						if (Utility.isOnScreen(Game1.currentLightSources.ElementAt(j).position, (int)(Game1.currentLightSources.ElementAt(j).radius * (float)Game1.tileSize * 4f)))
						{
							Game1.spriteBatch.Draw(Game1.currentLightSources.ElementAt(j).lightTexture, Game1.GlobalToLocal(Game1.viewport, Game1.currentLightSources.ElementAt(j).position) / (float)(Game1.options.lightingQuality / 2), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLightSources.ElementAt(j).lightTexture.Bounds), Game1.currentLightSources.ElementAt(j).color, 0f, new Vector2((float)Game1.currentLightSources.ElementAt(j).lightTexture.Bounds.Center.X, (float)Game1.currentLightSources.ElementAt(j).lightTexture.Bounds.Center.Y), Game1.currentLightSources.ElementAt(j).radius / (float)(Game1.options.lightingQuality / 2), SpriteEffects.None, 0.9f);
						}
					}
					Game1.spriteBatch.End();
					base.GraphicsDevice.SetRenderTarget((Game1.options.zoomLevel == 1f) ? null : this.screen);
				}
				if (Game1.bloomDay && Game1.bloom != null)
				{
					Game1.bloom.BeginDraw();
				}
				base.GraphicsDevice.Clear(this.bgColor);
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				if (Game1.background != null)
				{
					Game1.background.draw(Game1.spriteBatch);
				}
				Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
				Game1.currentLocation.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.currentLocation.drawWater(Game1.spriteBatch);
				if (Game1.CurrentEvent == null)
				{
					using (List<NPC>.Enumerator enumerator = Game1.currentLocation.characters.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NPC k = enumerator.Current;
							if (!k.swimming && !k.hideShadow && !k.isInvisible && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(k.getTileLocation()))
							{
								Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, k.position + new Vector2((float)(k.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(k.GetBoundingBox().Height + (k.IsMonster ? 0 : (Game1.pixelZoom * 3))))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)k.yJumpOffset / 40f) * k.scale, SpriteEffects.None, Math.Max(0f, (float)k.getStandingY() / 10000f) - 1E-06f);
							}
						}
						goto IL_B5E;
					}
				}
				foreach (NPC l in Game1.CurrentEvent.actors)
				{
					if (!l.swimming && !l.hideShadow && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(l.getTileLocation()))
					{
						Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, l.position + new Vector2((float)(l.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(l.GetBoundingBox().Height + (l.IsMonster ? 0 : ((l.sprite.spriteHeight <= 16) ? (-Game1.pixelZoom) : (Game1.pixelZoom * 3)))))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)l.yJumpOffset / 40f) * l.scale, SpriteEffects.None, Math.Max(0f, (float)l.getStandingY() / 10000f) - 1E-06f);
					}
				}
				IL_B5E:
				if (Game1.displayFarmer && !Game1.player.swimming && !Game1.player.isRidingHorse() && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation()))
				{
					Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f - (((Game1.player.running || Game1.player.usingTool) && Game1.player.FarmerSprite.indexInCurrentAnimation > 1) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, 0f);
				}
				Game1.currentLocation.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.mapDisplayDevice.EndScene();
				Game1.spriteBatch.End();
				Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				if (Game1.CurrentEvent == null)
				{
					using (List<NPC>.Enumerator enumerator = Game1.currentLocation.characters.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NPC m = enumerator.Current;
							if (!m.swimming && !m.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(m.getTileLocation()))
							{
								Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, m.position + new Vector2((float)(m.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(m.GetBoundingBox().Height + (m.IsMonster ? 0 : (Game1.pixelZoom * 3))))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)m.yJumpOffset / 40f) * m.scale, SpriteEffects.None, Math.Max(0f, (float)m.getStandingY() / 10000f) - 1E-06f);
							}
						}
						goto IL_F97;
					}
				}
				foreach (NPC n in Game1.CurrentEvent.actors)
				{
					if (!n.swimming && !n.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(n.getTileLocation()))
					{
						Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, n.position + new Vector2((float)(n.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(n.GetBoundingBox().Height + (n.IsMonster ? 0 : (Game1.pixelZoom * 3))))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)n.yJumpOffset / 40f) * n.scale, SpriteEffects.None, Math.Max(0f, (float)n.getStandingY() / 10000f) - 1E-06f);
					}
				}
				IL_F97:
				if (Game1.displayFarmer && !Game1.player.swimming && !Game1.player.isRidingHorse() && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation()))
				{
					Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f)), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f - (((Game1.player.running || Game1.player.usingTool) && Game1.player.FarmerSprite.indexInCurrentAnimation > 1) ? ((float)Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5f) : 0f), SpriteEffects.None, Math.Max(0.0001f, (float)Game1.player.getStandingY() / 10000f + 0.00011f) - 0.0001f);
				}
				if (Game1.displayFarmer)
				{
					Game1.player.draw(Game1.spriteBatch);
				}
				if ((Game1.eventUp || Game1.killScreen) && !Game1.killScreen && Game1.currentLocation.currentEvent != null)
				{
					Game1.currentLocation.currentEvent.draw(Game1.spriteBatch);
				}
				if (Game1.player.currentUpgrade != null && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && Game1.currentLocation.Name.Equals("Farm"))
				{
					Game1.spriteBatch.Draw(Game1.player.currentUpgrade.workerTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.currentUpgrade.positionOfCarpenter), new Microsoft.Xna.Framework.Rectangle?(Game1.player.currentUpgrade.getSourceRectangle()), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (Game1.player.currentUpgrade.positionOfCarpenter.Y + (float)(Game1.tileSize * 3 / 4)) / 10000f);
				}
				Game1.currentLocation.draw(Game1.spriteBatch);
				if (Game1.eventUp && Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.messageToScreen != null)
				{
					Game1.drawWithBorder(Game1.currentLocation.currentEvent.messageToScreen, Color.Black, Color.White, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width / 2) - Game1.borderFont.MeasureString(Game1.currentLocation.currentEvent.messageToScreen).X / 2f, (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height - Game1.tileSize)), 0f, 1f, 0.999f);
				}
				if (Game1.player.ActiveObject == null && (Game1.player.UsingTool || Game1.pickingTool) && Game1.player.CurrentTool != null && (!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool))
				{
					Game1.drawTool(Game1.player);
				}
				if (Game1.currentLocation.Name.Equals("Farm"))
				{
					this.drawFarmBuildings();
				}
				if (Game1.tvStation >= 0)
				{
					Game1.spriteBatch.Draw(Game1.tvStationTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(6 * Game1.tileSize + Game1.tileSize / 4), (float)(2 * Game1.tileSize + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Game1.tvStation * 24, 0, 24, 15)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
				}
				if (Game1.panMode)
				{
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int)Math.Floor((double)(Game1.getOldMouseX() + Game1.viewport.X) / (double)Game1.tileSize) * Game1.tileSize - Game1.viewport.X, (int)Math.Floor((double)(Game1.getOldMouseY() + Game1.viewport.Y) / (double)Game1.tileSize) * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Lime * 0.75f);
					foreach (Warp w in Game1.currentLocation.warps)
					{
						Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(w.X * Game1.tileSize - Game1.viewport.X, w.Y * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Red * 0.75f);
					}
				}
				Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
				Game1.currentLocation.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
				Game1.mapDisplayDevice.EndScene();
				Game1.currentLocation.drawAboveFrontLayer(Game1.spriteBatch);
				Game1.spriteBatch.End();
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				if (Game1.currentLocation.Name.Equals("Farm") && Game1.stats.SeedsSown >= 200u)
				{
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(3 * Game1.tileSize + Game1.tileSize / 4), (float)(Game1.tileSize + Game1.tileSize / 3))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(4 * Game1.tileSize + Game1.tileSize), (float)(2 * Game1.tileSize + Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(5 * Game1.tileSize), (float)(2 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(3 * Game1.tileSize + Game1.tileSize / 2), (float)(3 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(5 * Game1.tileSize - Game1.tileSize / 4), (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(4 * Game1.tileSize), (float)(3 * Game1.tileSize + Game1.tileSize / 6))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
					Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(4 * Game1.tileSize + Game1.tileSize / 5), (float)(2 * Game1.tileSize + Game1.tileSize / 3))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
				}
				if (Game1.displayFarmer && Game1.player.ActiveObject != null && Game1.player.ActiveObject.bigCraftable && this.checkBigCraftableBoundariesForFrontLayer() && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)
				{
					Game1.drawPlayerHeldObject(Game1.player);
				}
				else if (Game1.displayFarmer && Game1.player.ActiveObject != null && ((Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int)Game1.player.position.X, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && !Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int)Game1.player.position.X, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways")) || (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.GetBoundingBox().Right, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && !Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.GetBoundingBox().Right, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways"))))
				{
					Game1.drawPlayerHeldObject(Game1.player);
				}
				if ((Game1.player.UsingTool || Game1.pickingTool) && Game1.player.CurrentTool != null && (!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool) && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)
				{
					Game1.drawTool(Game1.player);
				}
				if (Game1.currentLocation.Map.GetLayer("AlwaysFront") != null)
				{
					Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
					Game1.currentLocation.Map.GetLayer("AlwaysFront").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
					Game1.mapDisplayDevice.EndScene();
				}
				if (Game1.toolHold > 400f && Game1.player.CurrentTool.UpgradeLevel >= 1 && Game1.player.canReleaseTool)
				{
					Color barColor = Color.White;
					switch ((int)(Game1.toolHold / 600f) + 2)
					{
					case 1:
						barColor = Tool.copperColor;
						break;
					case 2:
						barColor = Tool.steelColor;
						break;
					case 3:
						barColor = Tool.goldColor;
						break;
					case 4:
						barColor = Tool.iridiumColor;
						break;
					}
					Game1.spriteBatch.Draw(Game1.littleEffect, new Microsoft.Xna.Framework.Rectangle((int)Game1.player.getLocalPosition(Game1.viewport).X - 2, (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize) - 2, (int)(Game1.toolHold % 600f * 0.08f) + 4, Game1.tileSize / 8 + 4), Color.Black);
					Game1.spriteBatch.Draw(Game1.littleEffect, new Microsoft.Xna.Framework.Rectangle((int)Game1.player.getLocalPosition(Game1.viewport).X, (int)Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize), (int)(Game1.toolHold % 600f * 0.08f), Game1.tileSize / 8), barColor);
				}
				if (Game1.isDebrisWeather && Game1.currentLocation.IsOutdoors && !Game1.currentLocation.ignoreDebrisWeather && !Game1.currentLocation.Name.Equals("Desert") && Game1.viewport.X > -10)
				{
					using (List<WeatherDebris>.Enumerator enumerator3 = Game1.debrisWeather.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							enumerator3.Current.draw(Game1.spriteBatch);
						}
					}
				}
				if (Game1.farmEvent != null)
				{
					Game1.farmEvent.draw(Game1.spriteBatch);
				}
				if (Game1.currentLocation.LightLevel > 0f && Game1.timeOfDay < 2000)
				{
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * Game1.currentLocation.LightLevel);
				}
				if (Game1.screenGlow)
				{
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Game1.screenGlowColor * Game1.screenGlowAlpha);
				}
				Game1.currentLocation.drawAboveAlwaysFrontLayer(Game1.spriteBatch);
				if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod && ((Game1.player.CurrentTool as FishingRod).isTimingCast || (Game1.player.CurrentTool as FishingRod).castingChosenCountdown > 0f || (Game1.player.CurrentTool as FishingRod).fishCaught || (Game1.player.CurrentTool as FishingRod).showingTreasure))
				{
					Game1.player.CurrentTool.draw(Game1.spriteBatch);
				}
				if (Game1.isRaining && Game1.currentLocation.IsOutdoors && !Game1.currentLocation.Name.Equals("Desert") && !(Game1.currentLocation is Summit) && (!Game1.eventUp || Game1.currentLocation.isTileOnMap(new Vector2((float)(Game1.viewport.X / Game1.tileSize), (float)(Game1.viewport.Y / Game1.tileSize)))))
				{
					for (int i2 = 0; i2 < Game1.rainDrops.Length; i2++)
					{
						Game1.spriteBatch.Draw(Game1.rainTexture, Game1.rainDrops[i2].position, new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.rainTexture, Game1.rainDrops[i2].frame, -1, -1)), Color.White);
					}
				}
				Game1.spriteBatch.End();
				base.Draw(gameTime);
				Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
				{
					foreach (NPC n2 in Game1.currentLocation.currentEvent.actors)
					{
						if (n2.isEmoting)
						{
							Vector2 emotePosition = n2.getLocalPosition(Game1.viewport);
							emotePosition.Y -= (float)(Game1.tileSize * 2 + Game1.pixelZoom * 3);
							if (n2.age == 2)
							{
								emotePosition.Y += (float)(Game1.tileSize / 2);
							}
							else if (n2.gender == 1)
							{
								emotePosition.Y += (float)(Game1.tileSize / 6);
							}
							Game1.spriteBatch.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(n2.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, n2.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)n2.getStandingY() / 10000f);
						}
					}
				}
				Game1.spriteBatch.End();
				if (Game1.drawLighting)
				{
					Game1.spriteBatch.Begin(SpriteSortMode.Deferred, new BlendState
					{
						ColorBlendFunction = BlendFunction.ReverseSubtract,
						ColorDestinationBlend = Blend.One,
						ColorSourceBlend = Blend.SourceColor
					}, SamplerState.LinearClamp, null, null);
					Game1.spriteBatch.Draw(Game1.lightmap, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(Game1.lightmap.Bounds), Color.White, 0f, Vector2.Zero, (float)(Game1.options.lightingQuality / 2), SpriteEffects.None, 1f);
					if (Game1.isRaining && Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert))
					{
						Game1.spriteBatch.Draw(Game1.staminaRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.OrangeRed * 0.45f);
					}
					Game1.spriteBatch.End();
				}
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				if (Game1.drawGrid)
				{
					int startingX = -Game1.viewport.X % Game1.tileSize;
					float startingY = (float)(-(float)Game1.viewport.Y % Game1.tileSize);
					for (int x = startingX; x < Game1.graphics.GraphicsDevice.Viewport.Width; x += Game1.tileSize)
					{
						Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle(x, (int)startingY, 1, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Red * 0.5f);
					}
					for (float y = startingY; y < (float)Game1.graphics.GraphicsDevice.Viewport.Height; y += (float)Game1.tileSize)
					{
						Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle(startingX, (int)y, Game1.graphics.GraphicsDevice.Viewport.Width, 1), Color.Red * 0.5f);
					}
				}
				if (Game1.currentBillboard != 0)
				{
					this.drawBillboard();
				}
				if ((Game1.displayHUD || Game1.eventUp) && Game1.currentBillboard == 0 && Game1.gameMode == 3 && !Game1.freezeControls && !Game1.panMode)
				{
					this.drawHUD();
				}
				else if (Game1.activeClickableMenu == null && Game1.farmEvent == null)
				{
					Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, 4f + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
				if (Game1.hudMessages.Count > 0 && (!Game1.eventUp || Game1.isFestival()))
				{
					for (int i3 = Game1.hudMessages.Count - 1; i3 >= 0; i3--)
					{
						Game1.hudMessages[i3].draw(Game1.spriteBatch, i3);
					}
				}
			}
			if (Game1.farmEvent != null)
			{
				Game1.farmEvent.draw(Game1.spriteBatch);
			}
			if (Game1.dialogueUp && !Game1.nameSelectUp && !Game1.messagePause && (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is DialogueBox)))
			{
				this.drawDialogueBox();
			}
			if (Game1.progressBar)
			{
				Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 2, Game1.dialogueWidth, Game1.tileSize / 2), Color.LightGray);
				Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize * 2, (int)(Game1.pauseAccumulator / Game1.pauseTime * (float)Game1.dialogueWidth), Game1.tileSize / 2), Color.DimGray);
			}
			if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
			{
				Game1.currentLocation.currentEvent.drawAfterMap(Game1.spriteBatch);
			}
			if (Game1.isRaining && Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert))
			{
				Game1.spriteBatch.Draw(Game1.staminaRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Blue * 0.2f);
			}
			if ((Game1.fadeToBlack || Game1.globalFade) && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
			{
				Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((Game1.gameMode == 0) ? (1f - Game1.fadeToBlackAlpha) : Game1.fadeToBlackAlpha));
			}
			else if (Game1.flashAlpha > 0f)
			{
				if (Game1.options.screenFlash)
				{
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.White * Math.Min(1f, Game1.flashAlpha));
				}
				Game1.flashAlpha -= 0.1f;
			}
			if ((Game1.messagePause || Game1.globalFade) && Game1.dialogueUp)
			{
				this.drawDialogueBox();
			}
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator4 = Game1.screenOverlayTempSprites.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					enumerator4.Current.draw(Game1.spriteBatch, true, 0, 0);
				}
			}
			if (Game1.debugMode)
			{
				Game1.spriteBatch.DrawString(Game1.smallFont, string.Concat(new object[]
				{
					Game1.panMode ? ((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize + "," + (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize) : string.Concat(new object[]
					{
						"player: ",
						Game1.player.getStandingX() / Game1.tileSize,
						", ",
						Game1.player.getStandingY() / Game1.tileSize
					}),
					" backIndex:",
					Game1.currentLocation.getTileIndexAt(Game1.player.getTileX(), Game1.player.getTileY(), "Back"),
					Environment.NewLine,
					"debugOutput: ",
					Game1.debugOutput
				}), new Vector2((float)base.GraphicsDevice.Viewport.TitleSafeArea.X, (float)base.GraphicsDevice.Viewport.TitleSafeArea.Y), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
			}
			if (Game1.showKeyHelp)
			{
				Game1.spriteBatch.DrawString(Game1.smallFont, Game1.keyHelpString, new Vector2((float)Game1.tileSize, (float)(Game1.viewport.Height - Game1.tileSize - (Game1.dialogueUp ? (Game1.tileSize * 3 + (Game1.isQuestion ? (Game1.questionChoices.Count * Game1.tileSize) : 0)) : 0)) - Game1.smallFont.MeasureString(Game1.keyHelpString).Y), Color.LightGray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
			}
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.draw(Game1.spriteBatch);
			}
			else if (Game1.farmEvent != null)
			{
				Game1.farmEvent.drawAboveEverything(Game1.spriteBatch);
			}
			Game1.spriteBatch.End();
			if (Game1.options.zoomLevel != 1f)
			{
				base.GraphicsDevice.SetRenderTarget(null);
				base.GraphicsDevice.Clear(this.bgColor);
				Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
				Game1.spriteBatch.Draw(this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
				Game1.spriteBatch.End();
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0009D0F8 File Offset: 0x0009B2F8
		public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position)
		{
			Game1.drawWithBorder(message, borderColor, insideColor, position, 0f, 1f, 1f, false);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0009D113 File Offset: 0x0009B313
		public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position, float rotate, float scale, float layerDepth)
		{
			Game1.drawWithBorder(message, borderColor, insideColor, position, rotate, scale, layerDepth, false);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0009D128 File Offset: 0x0009B328
		public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position, float rotate, float scale, float layerDepth, bool tiny)
		{
			string[] words = message.Split(new char[]
			{
				' '
			});
			int offset = 0;
			int certainCharacters = 0;
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains("="))
				{
					Game1.spriteBatch.DrawString(tiny ? Game1.tinyFont : Game1.dialogueFont, words[i], new Vector2(position.X + (float)offset, position.Y), Color.Purple, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					offset += (int)((tiny ? Game1.tinyFont : Game1.dialogueFont).MeasureString(words[i]).X + 8f);
				}
				else
				{
					if (i == 0)
					{
						Game1.spriteBatch.DrawString(tiny ? Game1.tinyFontBorder : Game1.borderFont, words[i], new Vector2(position.X + (float)offset + (float)certainCharacters + (tiny ? (-2f * scale) : 0f), position.Y - (float)(tiny ? 0 : 1)), borderColor, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					}
					else
					{
						Game1.spriteBatch.DrawString(tiny ? Game1.tinyFontBorder : Game1.borderFont, words[i], new Vector2(position.X + (float)offset + (tiny ? (-2f * scale) : 0f), position.Y - (float)(tiny ? 0 : 1)), borderColor, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					}
					Game1.spriteBatch.DrawString(tiny ? Game1.tinyFont : Game1.dialogueFont, words[i], new Vector2(position.X + (float)offset, position.Y), insideColor, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					offset += (int)((tiny ? Game1.tinyFont : Game1.dialogueFont).MeasureString(words[i]).X + 8f);
				}
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0009D304 File Offset: 0x0009B504
		public static bool isOutdoorMapSmallerThanViewport()
		{
			return Game1.currentLocation != null && Game1.currentLocation.IsOutdoors && Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize < Game1.viewport.Width;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0009D354 File Offset: 0x0009B554
		private void drawHUD()
		{
			if (!Game1.eventUp)
			{
				float modifier = 0.625f;
				Vector2 topOfBar = new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right - 48 - Game1.tileSize / 8), (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - 224 - Game1.tileSize / 4 - (int)((float)(Game1.player.MaxStamina - 270) * modifier)));
				if (Game1.isOutdoorMapSmallerThanViewport())
				{
					topOfBar.X = Math.Min(topOfBar.X, (float)(-(float)Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize - 48));
				}
				if (Game1.staminaShakeTimer > 0)
				{
					topOfBar.X += (float)Game1.random.Next(-3, 4);
					topOfBar.Y += (float)Game1.random.Next(-3, 4);
				}
				Game1.spriteBatch.Draw(Game1.mouseCursors, topOfBar, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(256, 408, 12, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				Game1.spriteBatch.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X, (int)(topOfBar.Y + (float)Game1.tileSize), 48, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int)(topOfBar.Y + (float)Game1.tileSize - 8f)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(256, 424, 12, 16)), Color.White);
				Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(topOfBar.X, topOfBar.Y + 224f + (float)((int)((float)(Game1.player.MaxStamina - 270) * modifier)) - (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(256, 448, 12, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + 16 + Game1.tileSize / 2 + (int)((float)(Game1.player.MaxStamina - (int)Math.Max(0f, Game1.player.Stamina)) * modifier), 24, (int)(Game1.player.Stamina * modifier));
				if ((float)Game1.getOldMouseX() >= topOfBar.X && (float)Game1.getOldMouseY() >= topOfBar.Y)
				{
					Game1.drawWithBorder((int)Math.Max(0f, Game1.player.Stamina) + "/" + Game1.player.MaxStamina, Color.Black, Color.White, topOfBar + new Vector2(-Game1.dialogueFont.MeasureString("999/999").X - (float)(Game1.tileSize / 4) - (float)((Game1.currentLocation.Name.Equals("UndergroundMine") || Game1.player.health < 100) ? Game1.tileSize : 0), (float)Game1.tileSize));
				}
				Color c = Utility.getRedToGreenLerpColor(Game1.player.stamina / (float)Game1.player.maxStamina);
				Game1.spriteBatch.Draw(Game1.staminaRect, r, c);
				r.Height = Game1.pixelZoom;
				c.R = (byte)Math.Max(0, (int)(c.R - 50));
				c.G = (byte)Math.Max(0, (int)(c.G - 50));
				Game1.spriteBatch.Draw(Game1.staminaRect, r, c);
				if (Game1.player.exhausted)
				{
					Game1.spriteBatch.Draw(Game1.mouseCursors, topOfBar - new Vector2(0f, 11f) * (float)Game1.pixelZoom, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(191, 406, 12, 11)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					if ((float)Game1.getOldMouseX() >= topOfBar.X && (float)Game1.getOldMouseY() >= topOfBar.Y - (float)(11 * Game1.pixelZoom))
					{
						Game1.drawWithBorder("Exhausted", Color.Black, Color.White, topOfBar + new Vector2(-Game1.dialogueFont.MeasureString("Exhausted").X - (float)(Game1.tileSize / 4), (float)(Game1.tileSize * 3 / 2)));
					}
				}
				if (Game1.currentLocation is MineShaft || Game1.currentLocation is Woods || Game1.currentLocation is SlimeHutch || Game1.player.health < Game1.player.maxHealth)
				{
					topOfBar.X -= (float)(48 + Game1.tileSize / 8 + ((Game1.hitShakeTimer > 0) ? Game1.random.Next(-3, 4) : 0));
					topOfBar.Y = (float)(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - 224 - (Game1.player.maxHealth - 100) - Game1.tileSize / 4 + 4);
					Game1.spriteBatch.Draw(Game1.mouseCursors, topOfBar, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(268, 408, 12, 16)), (Game1.player.health < 20) ? (Color.Pink * ((float)Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / (double)((float)Game1.player.health * 50f)) / 4f + 0.9f)) : Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					Game1.spriteBatch.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X, (int)(topOfBar.Y + (float)Game1.tileSize), 48, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int)(topOfBar.Y + (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(268, 424, 12, 16)), (Game1.player.health < 20) ? (Color.Pink * ((float)Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / (double)((float)Game1.player.health * 50f)) / 4f + 0.9f)) : Color.White);
					Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(topOfBar.X, topOfBar.Y + 220f + (float)(Game1.player.maxHealth - 100) - 64f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(268, 448, 12, 16)), (Game1.player.health < 20) ? (Color.Pink * ((float)Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / (double)((float)Game1.player.health * 50f)) / 4f + 0.9f)) : Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
					int height = (int)((float)Game1.player.health / (float)Game1.player.maxHealth * (float)(168 + (Game1.player.maxHealth - 100)));
					c = Utility.getRedToGreenLerpColor((float)Game1.player.health / (float)Game1.player.maxHealth);
					Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + Game1.tileSize / 2 + (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int)topOfBar.Y + 24 - height), 24, height), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), c, 0f, Vector2.Zero, SpriteEffects.None, 1f);
					c.R = (byte)Math.Max(0, (int)(c.R - 50));
					c.G = (byte)Math.Max(0, (int)(c.G - 50));
					if ((float)Game1.getOldMouseX() >= topOfBar.X && (float)Game1.getOldMouseY() >= topOfBar.Y && (float)Game1.getOldMouseX() < topOfBar.X + (float)(Game1.tileSize / 2))
					{
						Game1.drawWithBorder(Math.Max(0, Game1.player.health) + "/" + Game1.player.maxHealth, Color.Black, Color.Red, topOfBar + new Vector2(-Game1.dialogueFont.MeasureString("999/999").X - (float)(Game1.tileSize / 2), (float)Game1.tileSize));
					}
					Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)topOfBar.X + 12, (int)topOfBar.Y + Game1.tileSize / 2 + (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int)topOfBar.Y + 24 - height), 24, Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), c, 0f, Vector2.Zero, SpriteEffects.None, 1f);
				}
				Object arg_9F5_0 = Game1.player.ActiveObject;
				if (Game1.debugMode)
				{
					Game1.spriteBatch.DrawString(Game1.dialogueFont, string.Concat(Game1.currentfps), new Vector2(16f, 64f), Color.Red);
				}
				if (Game1.isOutdoorMapSmallerThanViewport())
				{
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, -Math.Min(Game1.viewport.X, 4096), Game1.graphics.GraphicsDevice.Viewport.Height), Color.Black);
					Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(-Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize, 0, Math.Min(4096, Game1.graphics.GraphicsDevice.Viewport.Width - (-Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize)), Game1.graphics.GraphicsDevice.Viewport.Height), Color.Black);
				}
				foreach (IClickableMenu expr_B43 in Game1.onScreenMenus)
				{
					expr_B43.update(Game1.currentGameTime);
					expr_B43.draw(Game1.spriteBatch);
				}
				if (Game1.player.professions.Contains(17) && Game1.currentLocation.IsOutdoors)
				{
					foreach (KeyValuePair<Vector2, Object> v in Game1.currentLocation.objects)
					{
						if ((v.Value.isSpawnedObject || v.Value.ParentSheetIndex == 590) && !Utility.isOnScreen(v.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 64))
						{
							Vector2 onScreenPosition = default(Vector2);
							float rotation = 0f;
							if (v.Key.X * (float)Game1.tileSize > (float)(Game1.viewport.MaxCorner.X - 64))
							{
								onScreenPosition.X = (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Right - 8);
								rotation = 1.57079637f;
							}
							else if (v.Key.X * (float)Game1.tileSize < (float)Game1.viewport.X)
							{
								onScreenPosition.X = 8f;
								rotation = -1.57079637f;
							}
							else
							{
								onScreenPosition.X = v.Key.X * (float)Game1.tileSize - (float)Game1.viewport.X;
							}
							if (v.Key.Y * (float)Game1.tileSize > (float)(Game1.viewport.MaxCorner.Y - 64))
							{
								onScreenPosition.Y = (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8);
								rotation = 3.14159274f;
							}
							else if (v.Key.Y * (float)Game1.tileSize < (float)Game1.viewport.Y)
							{
								onScreenPosition.Y = 8f;
							}
							else
							{
								onScreenPosition.Y = v.Key.Y * (float)Game1.tileSize - (float)Game1.viewport.Y;
							}
							if (onScreenPosition.X == 8f && onScreenPosition.Y == 8f)
							{
								rotation += 0.7853982f;
							}
							if (onScreenPosition.X == 8f && onScreenPosition.Y == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8))
							{
								rotation += 0.7853982f;
							}
							if (onScreenPosition.X == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Right - 8) && onScreenPosition.Y == 8f)
							{
								rotation -= 0.7853982f;
							}
							if (onScreenPosition.X == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Right - 8) && onScreenPosition.Y == (float)(Game1.graphics.GraphicsDevice.Viewport.Bounds.Bottom - 8))
							{
								rotation -= 0.7853982f;
							}
							Game1.spriteBatch.Draw(Game1.mouseCursors, onScreenPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(412, 495, 5, 4)), Color.White, rotation, new Vector2(2f, 2f), 4f, SpriteEffects.None, 1f);
						}
					}
				}
			}
			if (Game1.timerUntilMouseFade > 0)
			{
				Game1.timerUntilMouseFade -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
				if (Game1.timerUntilMouseFade <= 0)
				{
					Game1.lastMousePositionBeforeFade = Game1.getMousePosition();
				}
			}
			if (Game1.options.gamepadControls && Game1.timerUntilMouseFade <= 0 && Game1.activeClickableMenu == null)
			{
				Game1.mouseCursorTransparency = 0f;
				if (base.IsActive)
				{
					IClickableMenu arg_F69_0 = Game1.activeClickableMenu;
				}
			}
			if (Game1.activeClickableMenu == null && Game1.mouseCursor > -1 && (Mouse.GetState().X != 0 || Mouse.GetState().Y != 0) && (Game1.getOldMouseX() != 0 || Game1.getOldMouseY() != 0))
			{
				if (!Utility.canGrabSomethingFromHere(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, Game1.player) || Game1.mouseCursor == 3)
				{
					if (Game1.player.ActiveObject != null && Game1.mouseCursor != 3 && !Game1.eventUp)
					{
						if (Game1.mouseCursorTransparency > 0f || Game1.options.showPlacementTileForGamepad)
						{
							Game1.player.ActiveObject.drawPlacementBounds(Game1.spriteBatch, Game1.currentLocation);
							if (Game1.mouseCursorTransparency > 0f)
							{
								bool canPlace = Utility.playerCanPlaceItemHere(Game1.currentLocation, Game1.player.CurrentItem, Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y, Game1.player) || (Utility.isThereAnObjectHereWhichAcceptsThisItem(Game1.currentLocation, Game1.player.CurrentItem, Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y) && Utility.withinRadiusOfPlayer(Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y, 1, Game1.player));
								Game1.player.CurrentItem.drawInMenu(Game1.spriteBatch, new Vector2((float)(Game1.getMouseX() + Game1.tileSize / 4), (float)(Game1.getMouseY() + Game1.tileSize / 4)), canPlace ? (Game1.dialogueButtonScale / 75f + 1f) : 1f, canPlace ? 1f : 0.5f, 0.999f);
							}
						}
					}
					else if (Game1.mouseCursor == 0 && Game1.isActionAtCurrentCursorTile)
					{
						Game1.mouseCursor = (Game1.isInspectionAtCurrentCursorTile ? 5 : 2);
					}
				}
				if (!Game1.options.hardwareCursor)
				{
					Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.mouseCursor, 16, 16)), Color.White * Game1.mouseCursorTransparency, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
				}
			}
			Game1.mouseCursor = 0;
			if (!Game1.isActionAtCurrentCursorTile && Game1.activeClickableMenu == null)
			{
				Game1.mouseCursorTransparency = 1f;
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0009E5A4 File Offset: 0x0009C7A4
		public static void panScreen(int x, int y)
		{
			Game1.previousViewportPosition.X = (float)Game1.viewport.Location.X;
			Game1.previousViewportPosition.Y = (float)Game1.viewport.Location.Y;
			Game1.viewport.X = Game1.viewport.X + x;
			Game1.viewport.Y = Game1.viewport.Y + y;
			Game1.clampViewportToGameMap();
			Game1.updateRaindropPosition();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0009E614 File Offset: 0x0009C814
		public static void clampViewportToGameMap()
		{
			if (Game1.viewport.X < 0)
			{
				Game1.viewport.X = 0;
			}
			if (Game1.viewport.X > Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width)
			{
				Game1.viewport.X = Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width;
			}
			if (Game1.viewport.Y < 0)
			{
				Game1.viewport.Y = 0;
			}
			if (Game1.viewport.Y > Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height)
			{
				Game1.viewport.Y = Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00002834 File Offset: 0x00000A34
		public void drawBillboard()
		{
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0009E6E8 File Offset: 0x0009C8E8
		private void drawDialogueBox()
		{
			int messageHeight = 5 * Game1.tileSize;
			if (Game1.currentSpeaker != null)
			{
				messageHeight = (int)Game1.dialogueFont.MeasureString(Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).Y;
				messageHeight = Math.Max(messageHeight, 5 * Game1.tileSize);
				Game1.drawDialogueBox((base.GraphicsDevice.Viewport.TitleSafeArea.Width - Math.Min(1280, base.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.tileSize * 2)) / 2, base.GraphicsDevice.Viewport.TitleSafeArea.Height - messageHeight, Math.Min(1280, base.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.tileSize * 2), messageHeight, true, false, null, Game1.objectDialoguePortraitPerson != null && Game1.currentSpeaker == null);
				return;
			}
			int arg_42_0 = Game1.currentObjectDialogue.Count;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0009E7EB File Offset: 0x0009C9EB
		public static void drawDialogueBox(string message)
		{
			Game1.drawDialogueBox(Game1.viewport.Width / 2, Game1.viewport.Height / 2, false, false, message);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0009E810 File Offset: 0x0009CA10
		public static void drawDialogueBox(int centerX, int centerY, bool speaker, bool drawOnlyBox, string message)
		{
			string text = null;
			if (speaker && Game1.currentSpeaker != null)
			{
				text = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue();
			}
			else if (message != null)
			{
				text = message;
			}
			else if (Game1.currentObjectDialogue.Count > 0)
			{
				text = Game1.currentObjectDialogue.Peek();
			}
			if (text == null)
			{
				return;
			}
			Vector2 expr_53 = Game1.dialogueFont.MeasureString(text);
			int width = (int)expr_53.X + Game1.tileSize * 2;
			int height = (int)expr_53.Y + Game1.tileSize * 2;
			int arg_92_0 = centerX - width / 2;
			int y = centerY - height / 2;
			Game1.drawDialogueBox(arg_92_0, y, width, height, speaker, drawOnlyBox, message, Game1.objectDialoguePortraitPerson != null && !speaker);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0009E8B4 File Offset: 0x0009CAB4
		public static void drawDialogueBox(int x, int y, int width, int height, bool speaker, bool drawOnlyBox, string message = null, bool objectDialogueWithPortrait = false)
		{
			if (!drawOnlyBox)
			{
				return;
			}
			int screenHeight = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height;
			int screenWidth = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width;
			int dialogueX = 0;
			int dialogueY = (y > Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y) ? 0 : Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y;
			int everythingYOffset = 0;
			width = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width, width);
			if (!Game1.isQuestion && Game1.currentSpeaker == null && Game1.currentObjectDialogue.Count > 0 && !drawOnlyBox)
			{
				width = (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).X + Game1.tileSize * 2;
				height = (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y + Game1.tileSize;
				x = screenWidth / 2 - width / 2;
				everythingYOffset = ((height > Game1.tileSize * 4) ? (-(height - Game1.tileSize * 4)) : 0);
			}
			Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize);
			int addedTileHeightForQuestions = -1;
			if (Game1.questionChoices.Count >= 3)
			{
				addedTileHeightForQuestions = Game1.questionChoices.Count - 3;
			}
			if (!drawOnlyBox && Game1.currentObjectDialogue.Count > 0)
			{
				if (Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y >= (float)(height - Game1.tileSize * 2))
				{
					addedTileHeightForQuestions -= (int)(((float)(height - Game1.tileSize * 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y) / (float)Game1.tileSize) - 1;
				}
				else
				{
					height += (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
					everythingYOffset -= (int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
					if ((int)Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2 > Game1.tileSize)
					{
						addedTileHeightForQuestions = 0;
					}
				}
			}
			if (Game1.currentSpeaker != null && Game1.isQuestion && Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex).Contains(Environment.NewLine))
			{
				addedTileHeightForQuestions++;
			}
			sourceRect.Width = Game1.tileSize;
			sourceRect.Height = Game1.tileSize;
			sourceRect.X = Game1.tileSize;
			sourceRect.Y = Game1.tileSize * 2;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(28 + x + dialogueX, 28 + y - Game1.tileSize * addedTileHeightForQuestions + dialogueY + everythingYOffset, width - Game1.tileSize, height - Game1.tileSize + addedTileHeightForQuestions * Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.Y = 0;
			sourceRect.X = 0;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + dialogueX), (float)(y - Game1.tileSize * addedTileHeightForQuestions + dialogueY + everythingYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.X = Game1.tileSize * 3;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + width + dialogueX - Game1.tileSize), (float)(y - Game1.tileSize * addedTileHeightForQuestions + dialogueY + everythingYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.Y = Game1.tileSize * 3;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + width + dialogueX - Game1.tileSize), (float)(y + height + dialogueY - Game1.tileSize + everythingYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.X = 0;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float)(x + dialogueX), (float)(y + height + dialogueY - Game1.tileSize + everythingYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.X = Game1.tileSize * 2;
			sourceRect.Y = 0;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize + x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + dialogueY + everythingYOffset, width - Game1.tileSize * 2, Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.Y = 3 * Game1.tileSize;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize + x + dialogueX, y + height + dialogueY - Game1.tileSize + everythingYOffset, width - Game1.tileSize * 2, Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.Y = Game1.tileSize * 2;
			sourceRect.X = 0;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + dialogueY + Game1.tileSize + everythingYOffset, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			sourceRect.X = 3 * Game1.tileSize;
			Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(x + width + dialogueX - Game1.tileSize, y - Game1.tileSize * addedTileHeightForQuestions + dialogueY + Game1.tileSize + everythingYOffset, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.White);
			if ((objectDialogueWithPortrait && Game1.objectDialoguePortraitPerson != null) || (speaker && Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0 && Game1.currentSpeaker.CurrentDialogue.Peek().showPortrait))
			{
				Microsoft.Xna.Framework.Rectangle portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
				NPC theSpeaker = objectDialogueWithPortrait ? Game1.objectDialoguePortraitPerson : Game1.currentSpeaker;
				string text2 = objectDialogueWithPortrait ? (Game1.objectDialoguePortraitPerson.name.Equals(Game1.player.spouse) ? "$l" : "$neutral") : theSpeaker.CurrentDialogue.Peek().CurrentEmotion;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
				if (num > 1488727062u)
				{
					if (num <= 1639725633u)
					{
						if (num != 1589392776u)
						{
							if (num != 1639725633u)
							{
								goto IL_78B;
							}
							if (!(text2 == "$h"))
							{
								goto IL_78B;
							}
							portraitRect = new Microsoft.Xna.Framework.Rectangle(64, 0, 64, 64);
							goto IL_7B7;
						}
						else if (!(text2 == "$k"))
						{
							goto IL_78B;
						}
					}
					else if (num != 1706836109u)
					{
						if (num != 2684935346u)
						{
							goto IL_78B;
						}
						if (!(text2 == "$neutral"))
						{
							goto IL_78B;
						}
					}
					else
					{
						if (!(text2 == "$l"))
						{
							goto IL_78B;
						}
						portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 128, 64, 64);
						goto IL_7B7;
					}
					portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
					goto IL_7B7;
				}
				if (num != 1186729920u)
				{
					if (num != 1287395634u)
					{
						if (num == 1488727062u)
						{
							if (text2 == "$a")
							{
								portraitRect = new Microsoft.Xna.Framework.Rectangle(64, 128, 64, 64);
								goto IL_7B7;
							}
						}
					}
					else if (text2 == "$u")
					{
						portraitRect = new Microsoft.Xna.Framework.Rectangle(64, 64, 64, 64);
						goto IL_7B7;
					}
				}
				else if (text2 == "$s")
				{
					portraitRect = new Microsoft.Xna.Framework.Rectangle(0, 64, 64, 64);
					goto IL_7B7;
				}
				IL_78B:
				portraitRect = Game1.getSourceRectForStandardTileSheet(theSpeaker.Portrait, Convert.ToInt32(theSpeaker.CurrentDialogue.Peek().CurrentEmotion.Substring(1)), -1, -1);
				IL_7B7:
				Game1.spriteBatch.End();
				Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
				if (theSpeaker.Portrait != null)
				{
					Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(dialogueX + x + Game1.tileSize * 12), (float)(screenHeight - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + dialogueY + Game1.tileSize / 4 - 60 + everythingYOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(333, 305, 80, 87)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
					Game1.spriteBatch.Draw(theSpeaker.Portrait, new Vector2((float)(dialogueX + x + Game1.tileSize * 12 + 32), (float)(screenHeight - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + dialogueY + Game1.tileSize / 4 - 60 + everythingYOffset)), new Microsoft.Xna.Framework.Rectangle?(portraitRect), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.99f);
				}
				Game1.spriteBatch.End();
				Game1.spriteBatch.Begin();
				if (Game1.isQuestion)
				{
					Game1.spriteBatch.DrawString(Game1.dialogueFont, theSpeaker.name, new Vector2((float)(Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(theSpeaker.name).X / 2f + (float)dialogueX + (float)x, (float)(screenHeight - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(theSpeaker.name).Y + (float)dialogueY + (float)(Game1.tileSize / 3) + (float)everythingYOffset) + new Vector2(2f, 2f), new Color(150, 150, 150));
				}
				Game1.spriteBatch.DrawString(Game1.dialogueFont, theSpeaker.name.Equals("DwarfKing") ? "Dwarf King" : (theSpeaker.name.Equals("Lewis") ? "Mr. Lewis" : theSpeaker.name), new Vector2((float)(dialogueX + x + Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(theSpeaker.name.Equals("Lewis") ? "Mr. Lewis" : theSpeaker.name).X / 2f, (float)(screenHeight - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(theSpeaker.name.Equals("Lewis") ? "Mr. Lewis" : theSpeaker.name).Y + (float)dialogueY + (float)(Game1.tileSize / 3) + (float)(Game1.tileSize / 8) + (float)everythingYOffset), Game1.textColor);
			}
			if (!drawOnlyBox && (!Game1.nameSelectUp || (Game1.messagePause && Game1.currentObjectDialogue != null)))
			{
				string text = "";
				if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0)
				{
					if (Game1.currentSpeaker.CurrentDialogue.Peek() == null || Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length < Game1.currentDialogueCharacterIndex - 1)
					{
						Game1.dialogueUp = false;
						Game1.currentDialogueCharacterIndex = 0;
						Game1.playSound("dialogueCharacterClose");
						Game1.player.forceCanMove();
						return;
					}
					text = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex);
				}
				else if (message != null)
				{
					text = message;
				}
				else if (Game1.currentObjectDialogue.Count > 0)
				{
					text = ((Game1.currentObjectDialogue.Peek().Length <= 1) ? "" : Game1.currentObjectDialogue.Peek().Substring(0, Game1.currentDialogueCharacterIndex));
				}
				Vector2 textPosition;
				if (Game1.dialogueFont.MeasureString(text).X > (float)(screenWidth - Game1.tileSize * 4 - dialogueX))
				{
					textPosition = new Vector2((float)(Game1.tileSize * 2 + dialogueX), (float)(screenHeight - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + dialogueY + everythingYOffset));
				}
				else if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0)
				{
					textPosition = new Vector2((float)(screenWidth / 2) - Game1.dialogueFont.MeasureString(Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).X / 2f + (float)dialogueX, (float)(screenHeight - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + dialogueY + everythingYOffset));
				}
				else if (message != null)
				{
					textPosition = new Vector2((float)(screenWidth / 2) - Game1.dialogueFont.MeasureString(text).X / 2f + (float)dialogueX, (float)(y + Game1.tileSize * 3 / 2 + Game1.pixelZoom));
				}
				else if (Game1.isQuestion)
				{
					textPosition = new Vector2((float)(screenWidth / 2) - Game1.dialogueFont.MeasureString((Game1.currentObjectDialogue.Count == 0) ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, (float)(screenHeight - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - (Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + dialogueY + everythingYOffset));
				}
				else
				{
					textPosition = new Vector2((float)(screenWidth / 2) - Game1.dialogueFont.MeasureString((Game1.currentObjectDialogue.Count == 0) ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float)dialogueX, (float)(y + Game1.pixelZoom + everythingYOffset));
				}
				if (!drawOnlyBox)
				{
					Game1.spriteBatch.DrawString(Game1.dialogueFont, text, textPosition + new Vector2(3f, 0f), Game1.textShadowColor);
					Game1.spriteBatch.DrawString(Game1.dialogueFont, text, textPosition + new Vector2(3f, 3f), Game1.textShadowColor);
					Game1.spriteBatch.DrawString(Game1.dialogueFont, text, textPosition + new Vector2(0f, 3f), Game1.textShadowColor);
					Game1.spriteBatch.DrawString(Game1.dialogueFont, text, textPosition, Game1.textColor);
				}
				if (Game1.dialogueFont.MeasureString(text).Y <= (float)Game1.tileSize)
				{
					dialogueY += Game1.tileSize;
				}
				if (Game1.isQuestion && !Game1.dialogueTyping)
				{
					for (int i = 0; i < Game1.questionChoices.Count; i++)
					{
						if (Game1.currentQuestionChoice == i)
						{
							textPosition.X = (float)(Game1.tileSize * 5 / 4 + dialogueX + x);
							textPosition.Y = (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + ((text.Trim().Length > 0) ? Game1.dialogueFont.MeasureString(text).Y : 0f) + (float)(Game1.tileSize * 2) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * i) - (float)(Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)dialogueY + (float)everythingYOffset;
							Game1.spriteBatch.End();
							Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
							Game1.spriteBatch.Draw(Game1.objectSpriteSheet, textPosition + new Vector2((float)Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * 3.1415926535897931 / 512.0) * 3f, 0f), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(26)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
							Game1.spriteBatch.End();
							Game1.spriteBatch.Begin();
							textPosition.X = (float)(Game1.tileSize * 5 / 2 + dialogueX + x);
							textPosition.Y = (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + ((text.Trim().Length > 1) ? Game1.dialogueFont.MeasureString(text).Y : 0f) + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float)((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * i) + (float)dialogueY + (float)everythingYOffset;
							Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[i].responseText, textPosition, Game1.textColor);
						}
						else
						{
							textPosition.X = (float)(Game1.tileSize * 2 + dialogueX + x);
							textPosition.Y = (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + ((text.Trim().Length > 1) ? Game1.dialogueFont.MeasureString(text).Y : 0f) + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float)((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float)((Game1.tileSize / 2 + Game1.tileSize / 4) * i) + (float)dialogueY + (float)everythingYOffset;
							Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[i].responseText, textPosition, Game1.unselectedOptionColor);
						}
					}
				}
				else if (Game1.numberOfSelectedItems != -1 && !Game1.dialogueTyping)
				{
					Game1.drawItemSelectDialogue(x, y, dialogueX, dialogueY + everythingYOffset, screenHeight, addedTileHeightForQuestions, text);
				}
				if (!drawOnlyBox && !Game1.dialogueTyping && message == null)
				{
					Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(x + dialogueX + width - Game1.tileSize * 3 / 2), (float)(y + height + dialogueY + everythingYOffset - Game1.tileSize * 3 / 2) - Game1.dialogueButtonScale), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, (!Game1.dialogueButtonShrinking && Game1.dialogueButtonScale < (float)(Game1.tileSize / 8)) ? 3 : 2, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
				}
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0009FAB8 File Offset: 0x0009DCB8
		private static void drawItemSelectDialogue(int x, int y, int dialogueX, int dialogueY, int screenHeight, int addedTileHeightForQuestions, string text)
		{
			string a = Game1.selectedItemsType;
			string whatToDraw;
			if (!(a == "flutePitch") && !(a == "drumTome"))
			{
				if (!(a == "jukebox"))
				{
					whatToDraw = string.Concat(new object[]
					{
						"@ ",
						Game1.numberOfSelectedItems,
						" >  ",
						Game1.priceOfSelectedItem * Game1.numberOfSelectedItems,
						"g"
					});
				}
				else
				{
					whatToDraw = "@ " + Game1.player.songsHeard.ElementAt(Game1.numberOfSelectedItems) + " >  ";
				}
			}
			else
			{
				whatToDraw = "@ " + Game1.numberOfSelectedItems + " >  ";
			}
			if (Game1.currentLocation.Name.Equals("Club"))
			{
				whatToDraw = "@ " + Game1.numberOfSelectedItems + " >  ";
			}
			Game1.spriteBatch.DrawString(Game1.dialogueFont, whatToDraw, new Vector2((float)(dialogueX + x + Game1.tileSize), (float)(screenHeight - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + Game1.dialogueFont.MeasureString(text).Y + (float)(Game1.tileSize * 3 / 2 + Game1.tileSize / 8) + (float)dialogueY), Game1.textColor);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0009FC0C File Offset: 0x0009DE0C
		private void drawFarmBuildings()
		{
			if (Game1.player.CoopUpgradeLevel > 0)
			{
				Game1.spriteBatch.Draw(Game1.currentCoopTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(20 * Game1.tileSize), (float)(5 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.currentCoopTexture.Bounds), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, (float)(9 * Game1.tileSize) / 10000f));
			}
			int barnUpgradeLevel = Game1.player.BarnUpgradeLevel;
			if (barnUpgradeLevel != 1)
			{
				if (barnUpgradeLevel == 2)
				{
					Game1.spriteBatch.Draw(Game1.currentBarnTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(10 * Game1.tileSize), (float)(4 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.currentBarnTexture.Bounds), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, (float)(9 * Game1.tileSize) / 10000f));
				}
			}
			else
			{
				Game1.spriteBatch.Draw(Game1.currentBarnTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(12 * Game1.tileSize), (float)(5 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.currentBarnTexture.Bounds), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, (float)(9 * Game1.tileSize) / 10000f));
			}
			if (Game1.player.hasGreenhouse)
			{
				Game1.spriteBatch.Draw(Game1.greenhouseTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)Game1.tileSize, (float)(5 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.greenhouseTexture.Bounds), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, (float)(9 * Game1.tileSize) / 10000f));
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0009FDFC File Offset: 0x0009DFFC
		public static void drawPlayerHeldObject(Farmer f)
		{
			if ((!Game1.eventUp || (Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.showActiveObject)) && !f.FarmerSprite.pauseForSingleAnimation && !f.isRidingHorse() && !f.bathingClothes)
			{
				float xPosition = f.getLocalPosition(Game1.viewport).X + (float)((f.rotation < 0f) ? -8 : ((f.rotation > 0f) ? 8 : 0)) + (float)(f.FarmerSprite.CurrentAnimationFrame.xOffset * Game1.pixelZoom);
				float objectYLoc = f.getLocalPosition(Game1.viewport).Y - (float)(Game1.tileSize * 2) + (float)(f.FarmerSprite.CurrentAnimationFrame.positionOffset * 4) + (float)(FarmerRenderer.featureYOffsetPerFrame[f.FarmerSprite.CurrentAnimationFrame.frame] * 4);
				if (f.ActiveObject.bigCraftable)
				{
					objectYLoc -= (float)Game1.tileSize;
				}
				if (Game1.isEating)
				{
					xPosition = f.getLocalPosition(Game1.viewport).X - (float)(Game1.tileSize / 3);
					objectYLoc = f.getLocalPosition(Game1.viewport).Y - (float)(Game1.tileSize * 2) + (float)(Game1.pixelZoom * 3);
				}
				if (!Game1.isEating || (Game1.isEating && f.Sprite.CurrentFrame <= 218))
				{
					f.ActiveObject.drawWhenHeld(Game1.spriteBatch, new Vector2((float)((int)xPosition), (float)((int)objectYLoc)), f);
				}
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0009FF84 File Offset: 0x0009E184
		public static void drawTool(Farmer f)
		{
			Game1.drawTool(f, f.CurrentTool.currentParentTileIndex);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0009FF98 File Offset: 0x0009E198
		public static void drawTool(Farmer f, int currentToolIndex)
		{
			Microsoft.Xna.Framework.Rectangle sourceRectangleForTool = new Microsoft.Xna.Framework.Rectangle(currentToolIndex * (Game1.tileSize / 4) % Game1.toolSpriteSheet.Width, currentToolIndex * (Game1.tileSize / 4) / Game1.toolSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 2);
			Vector2 fPosition = f.getLocalPosition(Game1.viewport) + f.jitter + f.armOffset;
			if (Game1.pickingTool)
			{
				int yLocation = (int)fPosition.Y - Game1.tileSize * 2;
				Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X, (float)yLocation), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
				return;
			}
			if (f.CurrentTool is MeleeWeapon)
			{
				((MeleeWeapon)f.CurrentTool).drawDuringUse(((FarmerSprite)f.Sprite).animatingBackwards ? (5 - ((FarmerSprite)f.Sprite).indexInCurrentAnimation) : ((FarmerSprite)f.Sprite).indexInCurrentAnimation, f.FacingDirection, Game1.spriteBatch, fPosition, f);
				return;
			}
			if (f.FarmerSprite.isUsingWeapon())
			{
				MeleeWeapon.drawDuringUse(((FarmerSprite)f.Sprite).indexInCurrentAnimation, f.FacingDirection, Game1.spriteBatch, fPosition, f, MeleeWeapon.getSourceRect(f.FarmerSprite.CurrentToolIndex), f.FarmerSprite.getWeaponTypeFromAnimation(), false);
				return;
			}
			if (f.CurrentTool is FishingRod)
			{
				if ((f.CurrentTool as FishingRod).fishCaught || (f.CurrentTool as FishingRod).showingTreasure)
				{
					f.CurrentTool.draw(Game1.spriteBatch);
					return;
				}
				sourceRectangleForTool = new Microsoft.Xna.Framework.Rectangle(((FarmerSprite)f.Sprite).indexInCurrentAnimation * 48, 288, 48, 48);
				if (f.FacingDirection == 2 || f.FacingDirection == 0)
				{
					sourceRectangleForTool.Y += 48;
				}
				else if ((f.CurrentTool as FishingRod).isFishing && (!(f.CurrentTool as FishingRod).isReeling || (f.CurrentTool as FishingRod).hit))
				{
					fPosition.Y += (float)(Game1.pixelZoom * 2);
				}
				if ((f.CurrentTool as FishingRod).isFishing)
				{
					sourceRectangleForTool.X += (5 - ((FarmerSprite)f.Sprite).indexInCurrentAnimation) * 48;
				}
				if ((f.CurrentTool as FishingRod).isReeling)
				{
					if (f.FacingDirection == 2 || f.FacingDirection == 0)
					{
						sourceRectangleForTool.X = 288;
						if (Game1.didPlayerJustClickAtAll())
						{
							sourceRectangleForTool.X = 0;
						}
					}
					else
					{
						sourceRectangleForTool.X = 288;
						sourceRectangleForTool.Y = 240;
						if (Game1.didPlayerJustClickAtAll())
						{
							sourceRectangleForTool.Y += 48;
						}
					}
				}
				if (f.FarmerSprite.CurrentFrame == 57)
				{
					sourceRectangleForTool.Height = 0;
				}
				if (f.FacingDirection == 0)
				{
					fPosition.X += (float)(Game1.tileSize / 4);
				}
			}
			if (f.CurrentTool != null)
			{
				f.CurrentTool.draw(Game1.spriteBatch);
			}
			if (f.CurrentTool is Slingshot || f.CurrentTool is Shears || f.CurrentTool is MilkPail || f.CurrentTool is Pan)
			{
				return;
			}
			int toolYOffset = 0;
			int toolXOffset = 0;
			if (f.CurrentTool is WateringCan)
			{
				toolYOffset += Game1.tileSize + Game1.tileSize / 4;
				toolXOffset = ((f.FacingDirection == 1) ? (Game1.tileSize / 2) : ((f.FacingDirection == 3) ? (-Game1.tileSize / 2) : 0));
				if (((FarmerSprite)f.Sprite).indexInCurrentAnimation == 0 || ((FarmerSprite)f.Sprite).indexInCurrentAnimation == 1)
				{
					toolXOffset = toolXOffset * 3 / 2;
				}
			}
			if (f.FacingDirection == 1)
			{
				int layerDepthOffset = 0;
				if (((FarmerSprite)f.Sprite).indexInCurrentAnimation > 2)
				{
					Point tileLocation = f.getTileLocationPoint();
					tileLocation.X++;
					tileLocation.Y--;
					if (!(f.CurrentTool is WateringCan) && f.currentLocation.getTileIndexAt(tileLocation, "Front") != -1)
					{
						return;
					}
					tileLocation.Y++;
					if (f.currentLocation.getTileIndexAt(tileLocation, "Front") == -1)
					{
						layerDepthOffset += Game1.tileSize / 4;
					}
				}
				else if (f.CurrentTool is WateringCan && ((FarmerSprite)f.Sprite).indexInCurrentAnimation == 1)
				{
					Point tileLocation2 = f.getTileLocationPoint();
					tileLocation2.X--;
					tileLocation2.Y--;
					if (f.currentLocation.getTileIndexAt(tileLocation2, "Front") != -1 && f.position.Y % (float)Game1.tileSize < (float)(Game1.tileSize / 2))
					{
						return;
					}
				}
				if (f.CurrentTool != null && f.CurrentTool is FishingRod)
				{
					Color color = (f.CurrentTool as FishingRod).getColor();
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						if ((f.CurrentTool as FishingRod).isReeling)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						else if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						else if (!(f.CurrentTool as FishingRod).hasDoneFucntionYet || (f.CurrentTool as FishingRod).pullingOutOfWater)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						break;
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + 8f + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)(Game1.tileSize * 3 / 2) + 32f + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) - 24f + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 3:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)(Game1.tileSize * 3 / 2) + 24f + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) - 32f + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 4:
						if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						else
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + 4f + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						break;
					case 5:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool.Name.Contains("Sword"))
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 20f, fPosition.Y + 28f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -0.3926991f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 12f, fPosition.Y + (float)Game1.tileSize - 8f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 12f, fPosition.Y + (float)Game1.tileSize - 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.3926991f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 3:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 12f, fPosition.Y + (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.7853981f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 4:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 16f, fPosition.Y + (float)Game1.tileSize + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 1.17809725f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 5:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 16f, fPosition.Y + (float)Game1.tileSize + 8f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 1.57079637f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 6:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 16f, fPosition.Y + (float)Game1.tileSize + 12f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 1.96349537f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 7:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize - 16f, fPosition.Y + (float)Game1.tileSize + 12f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 1.96349537f, new Vector2(4f, (float)(Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float)((int)(fPosition.X + (float)toolXOffset - 4f)), (float)((int)(fPosition.Y - (float)(Game1.tileSize * 2) + 8f + (float)toolYOffset))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float)((int)fPosition.X + toolXOffset + 24), (float)((int)(fPosition.Y - (float)(Game1.tileSize * 2) - 8f + (float)toolYOffset))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.2617994f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 3:
						sourceRectangleForTool.X += Game1.tileSize / 4;
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float)((int)(fPosition.X + (float)toolXOffset + 8f)), (float)((int)(fPosition.Y - (float)(Game1.tileSize * 2) - 24f + (float)toolYOffset))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					}
				}
				else
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)(Game1.tileSize / 2) - 4f + (float)toolXOffset - (float)Math.Min(8, f.toolPower * 4), fPosition.Y - (float)(Game1.tileSize * 2) + 24f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -0.2617994f - (float)Math.Min(f.toolPower, 2) * 0.0490873866f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)(Game1.tileSize / 2) - 24f + (float)toolXOffset, fPosition.Y - 124f + (float)toolYOffset + (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.2617994f, new Vector2(0f, (float)(Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)(Game1.tileSize / 2) + (float)toolXOffset - 4f, fPosition.Y - 132f + (float)toolYOffset + (float)Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.7853982f, new Vector2(0f, (float)(Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 3:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)(Game1.tileSize / 2) + 28f + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 1.83259583f, new Vector2(0f, (float)(Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 4:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)(Game1.tileSize / 2) + 28f + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + 4f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 1.83259583f, new Vector2(0f, (float)(Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 5:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)Game1.tileSize + 12f + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) + 32f + (float)toolYOffset + (float)(Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.7853982f, new Vector2(0f, (float)(Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					case 6:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)(Game1.tileSize * 2 / 3) + 8f + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + 24f + (float)toolYOffset + (float)(Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)(Game1.tileSize * 2)), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset) / 10000f));
						break;
					}
				}
			}
			else if (f.FacingDirection == 3)
			{
				int layerDepthOffset2 = 0;
				if (((FarmerSprite)f.Sprite).indexInCurrentAnimation > 2)
				{
					Point tileLocation3 = f.getTileLocationPoint();
					tileLocation3.X--;
					tileLocation3.Y--;
					if (!(f.CurrentTool is WateringCan) && f.currentLocation.getTileIndexAt(tileLocation3, "Front") != -1 && f.position.Y % (float)Game1.tileSize < (float)(Game1.tileSize / 2))
					{
						return;
					}
					tileLocation3.Y++;
					if (f.currentLocation.getTileIndexAt(tileLocation3, "Front") == -1)
					{
						layerDepthOffset2 += Game1.tileSize / 4;
					}
				}
				else if (f.CurrentTool is WateringCan && ((FarmerSprite)f.Sprite).indexInCurrentAnimation == 1)
				{
					Point tileLocation4 = f.getTileLocationPoint();
					tileLocation4.X--;
					tileLocation4.Y--;
					if (f.currentLocation.getTileIndexAt(tileLocation4, "Front") != -1 && f.position.Y % (float)Game1.tileSize < (float)(Game1.tileSize / 2))
					{
						return;
					}
				}
				if ((f.CurrentTool != null && f.CurrentTool is FishingRod) || (currentToolIndex >= 48 && currentToolIndex <= 55))
				{
					Color color2 = (f.CurrentTool as FishingRod).getColor();
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						if ((f.CurrentTool as FishingRod).isReeling)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						else if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						else if (!(f.CurrentTool as FishingRod).hasDoneFucntionYet || (f.CurrentTool as FishingRod).pullingOutOfWater)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						break;
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + 8f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)(Game1.tileSize * 3 / 2) + 32f + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) - 24f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 3:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)(Game1.tileSize * 3 / 2) + 24f + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) - 32f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					case 4:
						if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						else
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + 4f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						}
						break;
					case 5:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 5 / 2) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color2, 0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 4f, fPosition.Y - (float)(Game1.tileSize * 2) + 8f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 16f, fPosition.Y - (float)(Game1.tileSize * 2) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -0.2617994f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 3:
						sourceRectangleForTool.X += Game1.tileSize / 4;
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 16f, fPosition.Y - (float)(Game1.tileSize * 2) - 24f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					}
				}
				else
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)(Game1.tileSize / 2) + 8f + (float)toolXOffset + (float)Math.Min(8, f.toolPower * 4), fPosition.Y - (float)(Game1.tileSize * 2) + 8f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0.2617994f + (float)Math.Min(f.toolPower, 2) * 0.0490873866f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)(Game1.tileSize / 4) + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) + 16f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -0.2617994f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + 4f + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) + 60f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -0.7853982f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 3:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + 20f + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + 76f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -1.83259583f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					case 4:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - (float)Game1.tileSize + 24f + (float)toolXOffset, fPosition.Y + 24f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -1.83259583f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.GetBoundingBox().Bottom + layerDepthOffset2) / 10000f));
						break;
					}
				}
			}
			else if (!(f.CurrentTool is MeleeWeapon) || f.FacingDirection != 0)
			{
				if (((FarmerSprite)f.Sprite).indexInCurrentAnimation > 2 && (!(f.CurrentTool is FishingRod) || (f.CurrentTool as FishingRod).isCasting || (f.CurrentTool as FishingRod).castedButBobberStillInAir || (f.CurrentTool as FishingRod).isTimingCast))
				{
					Point tileLocation5 = f.getTileLocationPoint();
					if (f.currentLocation.getTileIndexAt(tileLocation5, "Front") != -1 && f.position.Y % (float)Game1.tileSize < (float)(Game1.tileSize / 2) && f.position.Y % (float)Game1.tileSize > (float)(Game1.tileSize / 4))
					{
						return;
					}
				}
				else if (f.CurrentTool is FishingRod && ((FarmerSprite)f.Sprite).indexInCurrentAnimation <= 2)
				{
					Point tileLocation6 = f.getTileLocationPoint();
					tileLocation6.Y--;
					if (f.currentLocation.getTileIndexAt(tileLocation6, "Front") != -1)
					{
						return;
					}
				}
				if ((f.CurrentTool != null && f.CurrentTool is FishingRod) || (currentToolIndex >= 48 && currentToolIndex <= 55 && !(f.CurrentTool as FishingRod).fishCaught))
				{
					Color color3 = (f.CurrentTool as FishingRod).getColor();
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						if (!(f.CurrentTool as FishingRod).showingTreasure && !(f.CurrentTool as FishingRod).fishCaught && (f.FacingDirection != 0 || !(f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).isReeling))
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - (float)(Game1.tileSize * 2) + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + ((f.FacingDirection == 0) ? 0 : (Game1.tileSize * 2))) / 10000f));
						}
						break;
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - (float)(Game1.tileSize * 2) + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + ((f.FacingDirection == 0) ? 0 : (Game1.tileSize * 2))) / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - (float)(Game1.tileSize * 2) + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + ((f.FacingDirection == 0) ? 0 : (Game1.tileSize * 2))) / 10000f));
						break;
					case 3:
						if (f.FacingDirection == 2)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - (float)(Game1.tileSize * 2) + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + ((f.FacingDirection == 0) ? 0 : (Game1.tileSize * 2))) / 10000f));
						}
						break;
					case 4:
						if (f.FacingDirection == 0 && (f.CurrentTool as FishingRod).isFishing)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 80f, fPosition.Y - 96f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.FlipVertically, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize * 2) / 10000f));
						}
						else if (f.FacingDirection == 2)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - (float)(Game1.tileSize * 2) + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + ((f.FacingDirection == 0) ? 0 : (Game1.tileSize * 2))) / 10000f));
						}
						break;
					case 5:
						if (f.FacingDirection == 2 && !(f.CurrentTool as FishingRod).showingTreasure && !(f.CurrentTool as FishingRod).fishCaught)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X - 64f, fPosition.Y - (float)(Game1.tileSize * 2) + 4f)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), color3, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + ((f.FacingDirection == 0) ? 0 : (Game1.tileSize * 2))) / 10000f));
						}
						break;
					}
				}
				else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
					case 1:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) + 16f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)f.GetBoundingBox().Bottom / 10000f));
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) - (float)((f.FacingDirection == 2) ? -4 : 32) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)f.GetBoundingBox().Bottom / 10000f));
						break;
					case 3:
						if (f.FacingDirection == 2)
						{
							sourceRectangleForTool.X += Game1.tileSize / 4;
						}
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - (float)((f.FacingDirection == 2) ? 4 : 0), fPosition.Y - (float)(Game1.tileSize * 2) - (float)((f.FacingDirection == 2) ? -24 : 64) + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)f.GetBoundingBox().Bottom / 10000f));
						break;
					}
				}
				else
				{
					switch (((FarmerSprite)f.Sprite).indexInCurrentAnimation)
					{
					case 0:
						if (f.FacingDirection == 0)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) - 8f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 8) / 10000f));
						}
						else
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 20f, fPosition.Y - (float)(Game1.tileSize * 2) + 12f + (float)toolYOffset + (float)Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 8) / 10000f));
						}
						break;
					case 1:
						if (f.FacingDirection == 0)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset + (float)Game1.pixelZoom, fPosition.Y - (float)(Game1.tileSize * 2) + 40f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 8) / 10000f));
						}
						else
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset - 12f, fPosition.Y - (float)(Game1.tileSize * 2) + 32f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, -0.1308997f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 8) / 10000f));
						}
						break;
					case 2:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)(Game1.tileSize * 2) + 64f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)((f.getStandingY() + f.FacingDirection == 0) ? (-(float)Game1.tileSize / 8) : (Game1.tileSize / 8)) / 10000f));
						break;
					case 3:
						if (f.FacingDirection != 0)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + 44f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 8) / 10000f));
						}
						break;
					case 4:
						if (f.FacingDirection != 0)
						{
							Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + 48f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 8) / 10000f));
						}
						break;
					case 5:
						Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(fPosition.X + (float)toolXOffset, fPosition.Y - (float)Game1.tileSize + 32f + (float)toolYOffset)), new Microsoft.Xna.Framework.Rectangle?(sourceRectangleForTool), Color.White, 0f, new Vector2(0f, (float)Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 8) / 10000f));
						break;
					}
				}
			}
			if (f.FacingDirection == 0)
			{
				f.FarmerRenderer.draw(Game1.spriteBatch, f.FarmerSprite, f.FarmerSprite.SourceRect, f.getLocalPosition(Game1.viewport) + f.jitter, new Vector2(0f, (f.yOffset + (float)(Game1.tileSize * 2) - (float)(f.GetBoundingBox().Height / 2)) / 4f + 4f), Math.Max(0f, ((float)f.GetBoundingBox().Bottom + 1f) / 10000f), Color.White, 0f, f);
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000A2E75 File Offset: 0x000A1075
		public static Vector2 GlobalToLocal(xTile.Dimensions.Rectangle viewport, Vector2 globalPosition)
		{
			return new Vector2(globalPosition.X - (float)viewport.X, globalPosition.Y - (float)viewport.Y);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000A2E9A File Offset: 0x000A109A
		public static Vector2 GlobalToLocal(Vector2 globalPosition)
		{
			return new Vector2(globalPosition.X - (float)Game1.viewport.X, globalPosition.Y - (float)Game1.viewport.Y);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000A2EC5 File Offset: 0x000A10C5
		public static Microsoft.Xna.Framework.Rectangle GlobalToLocal(xTile.Dimensions.Rectangle viewport, Microsoft.Xna.Framework.Rectangle globalPosition)
		{
			return new Microsoft.Xna.Framework.Rectangle(globalPosition.X - viewport.X, globalPosition.Y - viewport.Y, globalPosition.Width, globalPosition.Height);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000A2EF4 File Offset: 0x000A10F4
		public static string parseText(string text, SpriteFont whichFont, int width)
		{
			if (text == null)
			{
				return "";
			}
			string line = string.Empty;
			string returnString = string.Empty;
			string[] array = text.Split(new char[]
			{
				' '
			});
			for (int i = 0; i < array.Length; i++)
			{
				string word = array[i];
				if (whichFont.MeasureString(line + word).Length() > (float)width || word.Equals(Environment.NewLine))
				{
					returnString = returnString + line + Environment.NewLine;
					line = string.Empty;
				}
				line = line + word + " ";
			}
			return returnString + line;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000A2F8C File Offset: 0x000A118C
		public static string parseText(string text)
		{
			return Game1.parseText(text, Game1.dialogueFont, Game1.dialogueWidth);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000A2FA0 File Offset: 0x000A11A0
		public static bool isThisPositionVisibleToPlayer(string locationName, Vector2 position)
		{
			return locationName.Equals(Game1.currentLocation.Name) && new Microsoft.Xna.Framework.Rectangle((int)(Game1.player.position.X - (float)(Game1.viewport.Width / 2)), (int)(Game1.player.position.Y - (float)(Game1.viewport.Height / 2)), Game1.viewport.Width, Game1.viewport.Height).Contains(new Point((int)position.X, (int)position.Y));
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000A3034 File Offset: 0x000A1234
		public static string getProperArticleForWord(string word)
		{
			string article = "a";
			if (word != null)
			{
				char c = word.ToLower()[0];
				if (c <= 'e')
				{
					if (c != 'a')
					{
						if (c == 'e')
						{
							article += "n";
						}
					}
					else
					{
						article += "n";
					}
				}
				else if (c != 'i')
				{
					if (c != 'o')
					{
						if (c == 'u')
						{
							article += "n";
						}
					}
					else
					{
						article += "n";
					}
				}
				else
				{
					article += "n";
				}
			}
			return article;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000A30BE File Offset: 0x000A12BE
		public static Microsoft.Xna.Framework.Rectangle getSourceRectForStandardTileSheet(Texture2D tileSheet, int tilePosition, int width = -1, int height = -1)
		{
			if (width == -1)
			{
				width = Game1.tileSize;
			}
			if (height == -1)
			{
				height = Game1.tileSize;
			}
			return new Microsoft.Xna.Framework.Rectangle(tilePosition * width % tileSheet.Width, tilePosition * width / tileSheet.Width * height, width, height);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000A30F3 File Offset: 0x000A12F3
		public static Microsoft.Xna.Framework.Rectangle getSquareSourceRectForNonStandardTileSheet(Texture2D tileSheet, int tileWidth, int tileHeight, int tilePosition)
		{
			return new Microsoft.Xna.Framework.Rectangle(tilePosition * tileWidth % tileSheet.Width, tilePosition * tileWidth / tileSheet.Width * tileHeight, tileWidth, tileHeight);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000A3112 File Offset: 0x000A1312
		public static Microsoft.Xna.Framework.Rectangle getArbitrarySourceRect(Texture2D tileSheet, int tileWidth, int tileHeight, int tilePosition)
		{
			if (tileSheet != null)
			{
				return new Microsoft.Xna.Framework.Rectangle(tilePosition * tileWidth % tileSheet.Width, tilePosition * tileWidth / tileSheet.Width * tileHeight, tileWidth, tileHeight);
			}
			return Microsoft.Xna.Framework.Rectangle.Empty;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000A313C File Offset: 0x000A133C
		public static string getTimeOfDayString(int time)
		{
			string zeroPad = (time % 100 == 0) ? "0" : "";
			string hours = (time / 100 % 12 == 0) ? "12" : string.Concat(time / 100 % 12);
			return string.Concat(new object[]
			{
				hours,
				":",
				time % 100,
				zeroPad,
				(time < 1200 || time >= 2400) ? " am" : " pm"
			});
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000A31C4 File Offset: 0x000A13C4
		public bool checkBigCraftableBoundariesForFrontLayer()
		{
			return Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() - Game1.tileSize / 2, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null || Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() + Game1.tileSize / 2, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null || Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() - Game1.tileSize / 2, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5 - Game1.tileSize), Game1.viewport.Size) != null || Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() + Game1.tileSize / 2, (int)Game1.player.position.Y - Game1.tileSize * 3 / 5 - Game1.tileSize), Game1.viewport.Size) != null;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000A3340 File Offset: 0x000A1540
		public static bool[,] getCircleOutlineGrid(int radius)
		{
			bool[,] circleGrid = new bool[radius * 2 + 1, radius * 2 + 1];
			int f = 1 - radius;
			int ddF_x = 1;
			int ddF_y = -2 * radius;
			int x = 0;
			int y = radius;
			circleGrid[radius, radius + radius] = true;
			circleGrid[radius, radius - radius] = true;
			circleGrid[radius + radius, radius] = true;
			circleGrid[radius - radius, radius] = true;
			while (x < y)
			{
				if (f >= 0)
				{
					y--;
					ddF_y += 2;
					f += ddF_y;
				}
				x++;
				ddF_x += 2;
				f += ddF_x;
				circleGrid[radius + x, radius + y] = true;
				circleGrid[radius - x, radius + y] = true;
				circleGrid[radius + x, radius - y] = true;
				circleGrid[radius - x, radius - y] = true;
				circleGrid[radius + y, radius + x] = true;
				circleGrid[radius - y, radius + x] = true;
				circleGrid[radius + y, radius - x] = true;
				circleGrid[radius - y, radius - x] = true;
			}
			return circleGrid;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x000A3460 File Offset: 0x000A1660
		public static Color getColorForTreasureType(string type)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(type);
			if (num <= 872197005u)
			{
				if (num != 116937720u)
				{
					if (num != 849800425u)
					{
						if (num == 872197005u)
						{
							if (type == "Coins")
							{
								return Color.Yellow;
							}
						}
					}
					else if (type == "Arch")
					{
						return Color.White;
					}
				}
				else if (type == "Iridium")
				{
					return Color.Purple;
				}
			}
			else if (num <= 1952841722u)
			{
				if (num != 1821685427u)
				{
					if (num == 1952841722u)
					{
						if (type == "Coal")
						{
							return Color.Black;
						}
					}
				}
				else if (type == "Gold")
				{
					return Color.Gold;
				}
			}
			else if (num != 3420196507u)
			{
				if (num == 3821421172u)
				{
					if (type == "Copper")
					{
						return Color.Sienna;
					}
				}
			}
			else if (type == "Iron")
			{
				return Color.LightSlateGray;
			}
			return Color.SaddleBrown;
		}

		// Token: 0x0400065A RID: 1626
		public const int defaultResolutionX = 1280;

		// Token: 0x0400065B RID: 1627
		public const int defaultResolutionY = 720;

		// Token: 0x0400065C RID: 1628
		public static int pixelZoom = 4;

		// Token: 0x0400065D RID: 1629
		public static int tileSize = 64;

		// Token: 0x0400065E RID: 1630
		public const int smallestTileSize = 16;

		// Token: 0x0400065F RID: 1631
		public const int up = 0;

		// Token: 0x04000660 RID: 1632
		public const int right = 1;

		// Token: 0x04000661 RID: 1633
		public const int down = 2;

		// Token: 0x04000662 RID: 1634
		public const int left = 3;

		// Token: 0x04000663 RID: 1635
		public const int spriteIndexForOveralls = 3854;

		// Token: 0x04000664 RID: 1636
		public const int colorToleranceForOveralls = 60;

		// Token: 0x04000665 RID: 1637
		public const int spriteIndexForOverallsBorder = 3846;

		// Token: 0x04000666 RID: 1638
		public const int colorToloranceForOverallsBorder = 20;

		// Token: 0x04000667 RID: 1639
		public const int dialogueBoxTileHeight = 5;

		// Token: 0x04000668 RID: 1640
		public const int realMilliSecondsPerGameTenMinutes = 7000;

		// Token: 0x04000669 RID: 1641
		public const int rainDensity = 70;

		// Token: 0x0400066A RID: 1642
		public const int millisecondsPerDialogueLetterType = 30;

		// Token: 0x0400066B RID: 1643
		public const float pickToolDelay = 500f;

		// Token: 0x0400066C RID: 1644
		public const int defaultMinFishingBiteTime = 600;

		// Token: 0x0400066D RID: 1645
		public const int defaultMaxFishingBiteTime = 30000;

		// Token: 0x0400066E RID: 1646
		public const int defaultMinFishingNibbleTime = 340;

		// Token: 0x0400066F RID: 1647
		public const int defaultMaxFishingNibbleTime = 800;

		// Token: 0x04000670 RID: 1648
		public const int minWallpaperPrice = 75;

		// Token: 0x04000671 RID: 1649
		public const int maxWallpaperPrice = 500;

		// Token: 0x04000672 RID: 1650
		public const int rainLoopLength = 70;

		// Token: 0x04000673 RID: 1651
		public const int weather_sunny = 0;

		// Token: 0x04000674 RID: 1652
		public const int weather_rain = 1;

		// Token: 0x04000675 RID: 1653
		public const int weather_debris = 2;

		// Token: 0x04000676 RID: 1654
		public const int weather_lightning = 3;

		// Token: 0x04000677 RID: 1655
		public const int weather_festival = 4;

		// Token: 0x04000678 RID: 1656
		public const int weather_snow = 5;

		// Token: 0x04000679 RID: 1657
		public const int weather_wedding = 6;

		// Token: 0x0400067A RID: 1658
		public const byte singlePlayer = 0;

		// Token: 0x0400067B RID: 1659
		public const byte multiplayerClient = 1;

		// Token: 0x0400067C RID: 1660
		public const byte multiplayerServer = 2;

		// Token: 0x0400067D RID: 1661
		public const byte logoScreenGameMode = 4;

		// Token: 0x0400067E RID: 1662
		public const byte titleScreenGameMode = 0;

		// Token: 0x0400067F RID: 1663
		public const byte loadScreenGameMode = 1;

		// Token: 0x04000680 RID: 1664
		public const byte newGameMode = 2;

		// Token: 0x04000681 RID: 1665
		public const byte playingGameMode = 3;

		// Token: 0x04000682 RID: 1666
		public const byte characterSelectMode = 5;

		// Token: 0x04000683 RID: 1667
		public const byte loadingMode = 6;

		// Token: 0x04000684 RID: 1668
		public const byte saveMode = 7;

		// Token: 0x04000685 RID: 1669
		public const byte saveCompleteMode = 8;

		// Token: 0x04000686 RID: 1670
		public const byte selectGameScreen = 9;

		// Token: 0x04000687 RID: 1671
		public const byte creditsMode = 10;

		// Token: 0x04000688 RID: 1672
		public const byte errorLogMode = 11;

		// Token: 0x04000689 RID: 1673
		public static string version = "1.11";

		// Token: 0x0400068A RID: 1674
		public const float keyPollingThreshold = 650f;

		// Token: 0x0400068B RID: 1675
		public const float toolHoldPerPowerupLevel = 600f;

		// Token: 0x0400068C RID: 1676
		public const float startingMusicVolume = 1f;

		// Token: 0x0400068D RID: 1677
		public static GraphicsDeviceManager graphics;

		// Token: 0x0400068E RID: 1678
		public static LocalizedContentManager content;

		// Token: 0x0400068F RID: 1679
		public static LocalizedContentManager temporaryContent;

		// Token: 0x04000690 RID: 1680
		public static SpriteBatch spriteBatch;

		// Token: 0x04000691 RID: 1681
		public static GamePadState oldPadState;

		// Token: 0x04000692 RID: 1682
		public static float thumbStickSensitivity = 0.1f;

		// Token: 0x04000693 RID: 1683
		public static float runThreshold = 0.5f;

		// Token: 0x04000694 RID: 1684
		public static KeyboardState oldKBState;

		// Token: 0x04000695 RID: 1685
		public static MouseState oldMouseState;

		// Token: 0x04000696 RID: 1686
		public static List<GameLocation> locations = new List<GameLocation>();

		// Token: 0x04000697 RID: 1687
		public static GameLocation currentLocation;

		// Token: 0x04000698 RID: 1688
		public static GameLocation locationAfterWarp;

		// Token: 0x04000699 RID: 1689
		public static IDisplayDevice mapDisplayDevice;

		// Token: 0x0400069A RID: 1690
		public static Farmer player;

		// Token: 0x0400069B RID: 1691
		public static Farmer serverHost;

		// Token: 0x0400069C RID: 1692
		public static xTile.Dimensions.Rectangle viewport;

		// Token: 0x0400069D RID: 1693
		public static Texture2D objectSpriteSheet;

		// Token: 0x0400069E RID: 1694
		public static Texture2D toolSpriteSheet;

		// Token: 0x0400069F RID: 1695
		public static Texture2D cropSpriteSheet;

		// Token: 0x040006A0 RID: 1696
		public static Texture2D mailboxTexture;

		// Token: 0x040006A1 RID: 1697
		public static Texture2D emoteSpriteSheet;

		// Token: 0x040006A2 RID: 1698
		public static Texture2D debrisSpriteSheet;

		// Token: 0x040006A3 RID: 1699
		public static Texture2D toolIconBox;

		// Token: 0x040006A4 RID: 1700
		public static Texture2D rainTexture;

		// Token: 0x040006A5 RID: 1701
		public static Texture2D bigCraftableSpriteSheet;

		// Token: 0x040006A6 RID: 1702
		public static Texture2D swordSwipe;

		// Token: 0x040006A7 RID: 1703
		public static Texture2D swordSwipeDark;

		// Token: 0x040006A8 RID: 1704
		public static Texture2D buffsIcons;

		// Token: 0x040006A9 RID: 1705
		public static Texture2D daybg;

		// Token: 0x040006AA RID: 1706
		public static Texture2D nightbg;

		// Token: 0x040006AB RID: 1707
		public static Texture2D logoScreenTexture;

		// Token: 0x040006AC RID: 1708
		public static Texture2D tvStationTexture;

		// Token: 0x040006AD RID: 1709
		public static Texture2D cloud;

		// Token: 0x040006AE RID: 1710
		public static Texture2D menuTexture;

		// Token: 0x040006AF RID: 1711
		public static Texture2D lantern;

		// Token: 0x040006B0 RID: 1712
		public static Texture2D windowLight;

		// Token: 0x040006B1 RID: 1713
		public static Texture2D sconceLight;

		// Token: 0x040006B2 RID: 1714
		public static Texture2D cauldronLight;

		// Token: 0x040006B3 RID: 1715
		public static Texture2D shadowTexture;

		// Token: 0x040006B4 RID: 1716
		public static Texture2D mouseCursors;

		// Token: 0x040006B5 RID: 1717
		public static Texture2D indoorWindowLight;

		// Token: 0x040006B6 RID: 1718
		public static Texture2D animations;

		// Token: 0x040006B7 RID: 1719
		public static Texture2D titleScreenBG;

		// Token: 0x040006B8 RID: 1720
		public static Texture2D logo;

		// Token: 0x040006B9 RID: 1721
		public static RenderTarget2D lightmap;

		// Token: 0x040006BA RID: 1722
		public static Texture2D fadeToBlackRect;

		// Token: 0x040006BB RID: 1723
		public static Texture2D staminaRect;

		// Token: 0x040006BC RID: 1724
		public static Texture2D currentCoopTexture;

		// Token: 0x040006BD RID: 1725
		public static Texture2D currentBarnTexture;

		// Token: 0x040006BE RID: 1726
		public static Texture2D currentHouseTexture;

		// Token: 0x040006BF RID: 1727
		public static Texture2D greenhouseTexture;

		// Token: 0x040006C0 RID: 1728
		public static Texture2D littleEffect;

		// Token: 0x040006C1 RID: 1729
		public static SpriteFont dialogueFont;

		// Token: 0x040006C2 RID: 1730
		public static SpriteFont smallFont;

		// Token: 0x040006C3 RID: 1731
		public static SpriteFont borderFont;

		// Token: 0x040006C4 RID: 1732
		public static SpriteFont tinyFont;

		// Token: 0x040006C5 RID: 1733
		public static SpriteFont tinyFontBorder;

		// Token: 0x040006C6 RID: 1734
		public static SpriteFont smoothFont;

		// Token: 0x040006C7 RID: 1735
		public static float fadeToBlackAlpha;

		// Token: 0x040006C8 RID: 1736
		public static float pickToolInterval;

		// Token: 0x040006C9 RID: 1737
		public static float screenGlowAlpha = 0f;

		// Token: 0x040006CA RID: 1738
		public static float flashAlpha = 0f;

		// Token: 0x040006CB RID: 1739
		public static float starCropShimmerPause;

		// Token: 0x040006CC RID: 1740
		public static float noteBlockTimer;

		// Token: 0x040006CD RID: 1741
		public static float globalFadeSpeed;

		// Token: 0x040006CE RID: 1742
		public static bool fadeToBlack = false;

		// Token: 0x040006CF RID: 1743
		public static bool fadeIn = true;

		// Token: 0x040006D0 RID: 1744
		public static bool dialogueUp = false;

		// Token: 0x040006D1 RID: 1745
		public static bool dialogueTyping = false;

		// Token: 0x040006D2 RID: 1746
		public static bool pickingTool = false;

		// Token: 0x040006D3 RID: 1747
		public static bool isQuestion = false;

		// Token: 0x040006D4 RID: 1748
		public static bool nonWarpFade = false;

		// Token: 0x040006D5 RID: 1749
		public static bool particleRaining = false;

		// Token: 0x040006D6 RID: 1750
		public static bool newDay = false;

		// Token: 0x040006D7 RID: 1751
		public static bool inMine = false;

		// Token: 0x040006D8 RID: 1752
		public static bool isEating = false;

		// Token: 0x040006D9 RID: 1753
		public static bool menuUp = false;

		// Token: 0x040006DA RID: 1754
		public static bool eventUp = false;

		// Token: 0x040006DB RID: 1755
		public static bool viewportFreeze = false;

		// Token: 0x040006DC RID: 1756
		public static bool eventOver = false;

		// Token: 0x040006DD RID: 1757
		public static bool nameSelectUp = false;

		// Token: 0x040006DE RID: 1758
		public static bool screenGlow = false;

		// Token: 0x040006DF RID: 1759
		public static bool screenGlowHold = false;

		// Token: 0x040006E0 RID: 1760
		public static bool screenGlowUp;

		// Token: 0x040006E1 RID: 1761
		public static bool progressBar = false;

		// Token: 0x040006E2 RID: 1762
		public static bool isRaining = false;

		// Token: 0x040006E3 RID: 1763
		public static bool isSnowing = false;

		// Token: 0x040006E4 RID: 1764
		public static bool killScreen = false;

		// Token: 0x040006E5 RID: 1765
		public static bool coopDwellerBorn;

		// Token: 0x040006E6 RID: 1766
		public static bool messagePause;

		// Token: 0x040006E7 RID: 1767
		public static bool isDebrisWeather;

		// Token: 0x040006E8 RID: 1768
		public static bool boardingBus;

		// Token: 0x040006E9 RID: 1769
		public static bool listeningForKeyControlDefinitions;

		// Token: 0x040006EA RID: 1770
		public static bool weddingToday;

		// Token: 0x040006EB RID: 1771
		public static bool exitToTitle;

		// Token: 0x040006EC RID: 1772
		public static bool debugMode;

		// Token: 0x040006ED RID: 1773
		public static bool isLightning;

		// Token: 0x040006EE RID: 1774
		public static bool displayHUD = true;

		// Token: 0x040006EF RID: 1775
		public static bool displayFarmer = true;

		// Token: 0x040006F0 RID: 1776
		public static bool showKeyHelp;

		// Token: 0x040006F1 RID: 1777
		public static bool shippingTax;

		// Token: 0x040006F2 RID: 1778
		public static bool dialogueButtonShrinking;

		// Token: 0x040006F3 RID: 1779
		public static bool jukeboxPlaying;

		// Token: 0x040006F4 RID: 1780
		public static bool drawLighting;

		// Token: 0x040006F5 RID: 1781
		public static bool bloomDay;

		// Token: 0x040006F6 RID: 1782
		public static bool quit;

		// Token: 0x040006F7 RID: 1783
		public static bool isChatting;

		// Token: 0x040006F8 RID: 1784
		public static bool globalFade;

		// Token: 0x040006F9 RID: 1785
		public static bool drawGrid;

		// Token: 0x040006FA RID: 1786
		public static bool freezeControls;

		// Token: 0x040006FB RID: 1787
		public static bool saveOnNewDay;

		// Token: 0x040006FC RID: 1788
		public static bool panMode;

		// Token: 0x040006FD RID: 1789
		public static bool showingEndOfNightStuff;

		// Token: 0x040006FE RID: 1790
		public static bool wasRainingYesterday;

		// Token: 0x040006FF RID: 1791
		public static bool hasLoadedGame;

		// Token: 0x04000700 RID: 1792
		public static bool isActionAtCurrentCursorTile;

		// Token: 0x04000701 RID: 1793
		public static bool isInspectionAtCurrentCursorTile;

		// Token: 0x04000702 RID: 1794
		public static bool paused;

		// Token: 0x04000703 RID: 1795
		public static bool lastCursorMotionWasMouse;

		// Token: 0x04000704 RID: 1796
		public static bool spawnMonstersAtNight = false;

		// Token: 0x04000705 RID: 1797
		public static string currentSeason = "spring";

		// Token: 0x04000706 RID: 1798
		public static string debugOutput;

		// Token: 0x04000707 RID: 1799
		public static string nextMusicTrack = "";

		// Token: 0x04000708 RID: 1800
		public static string selectedItemsType;

		// Token: 0x04000709 RID: 1801
		public static string nameSelectType;

		// Token: 0x0400070A RID: 1802
		public static string messageAfterPause = "";

		// Token: 0x0400070B RID: 1803
		public static string fertilizer = "";

		// Token: 0x0400070C RID: 1804
		public static string samBandName = "The Alfalfas";

		// Token: 0x0400070D RID: 1805
		public static string elliottBookName = "Blue Tower";

		// Token: 0x0400070E RID: 1806
		public static string slotResult;

		// Token: 0x0400070F RID: 1807
		public static string keyHelpString = "";

		// Token: 0x04000710 RID: 1808
		public static string lastDebugInput = "";

		// Token: 0x04000711 RID: 1809
		public static string loadingMessage = "";

		// Token: 0x04000712 RID: 1810
		public static string errorMessage = "";

		// Token: 0x04000713 RID: 1811
		public static Queue<string> currentObjectDialogue = new Queue<string>();

		// Token: 0x04000714 RID: 1812
		public static Queue<string> mailbox = new Queue<string>();

		// Token: 0x04000715 RID: 1813
		public static List<Response> questionChoices = new List<Response>();

		// Token: 0x04000716 RID: 1814
		public static int xLocationAfterWarp;

		// Token: 0x04000717 RID: 1815
		public static int yLocationAfterWarp;

		// Token: 0x04000718 RID: 1816
		public static int gameTimeInterval;

		// Token: 0x04000719 RID: 1817
		public static int currentQuestionChoice;

		// Token: 0x0400071A RID: 1818
		public static int currentDialogueCharacterIndex;

		// Token: 0x0400071B RID: 1819
		public static int dialogueTypingInterval;

		// Token: 0x0400071C RID: 1820
		public static int dayOfMonth = 0;

		// Token: 0x0400071D RID: 1821
		public static int year = 1;

		// Token: 0x0400071E RID: 1822
		public static int timeOfDay = 600;

		// Token: 0x0400071F RID: 1823
		public static int numberOfSelectedItems = -1;

		// Token: 0x04000720 RID: 1824
		public static int priceOfSelectedItem;

		// Token: 0x04000721 RID: 1825
		public static int currentWallpaper;

		// Token: 0x04000722 RID: 1826
		public static int farmerWallpaper = 22;

		// Token: 0x04000723 RID: 1827
		public static int wallpaperPrice = 75;

		// Token: 0x04000724 RID: 1828
		public static int currentFloor = 3;

		// Token: 0x04000725 RID: 1829
		public static int FarmerFloor = 29;

		// Token: 0x04000726 RID: 1830
		public static int floorPrice = 75;

		// Token: 0x04000727 RID: 1831
		public static int dialogueWidth;

		// Token: 0x04000728 RID: 1832
		public static int countdownToWedding;

		// Token: 0x04000729 RID: 1833
		public static int menuChoice;

		// Token: 0x0400072A RID: 1834
		public static int tvStation = -1;

		// Token: 0x0400072B RID: 1835
		public static int currentBillboard;

		// Token: 0x0400072C RID: 1836
		public static int facingDirectionAfterWarp;

		// Token: 0x0400072D RID: 1837
		public static int tmpTimeOfDay;

		// Token: 0x0400072E RID: 1838
		public static int percentageToWinStardewHero = 70;

		// Token: 0x0400072F RID: 1839
		public static int mouseClickPolling;

		// Token: 0x04000730 RID: 1840
		public static int weatherIcon;

		// Token: 0x04000731 RID: 1841
		public static int hitShakeTimer;

		// Token: 0x04000732 RID: 1842
		public static int staminaShakeTimer;

		// Token: 0x04000733 RID: 1843
		public static int pauseThenDoFunctionTimer;

		// Token: 0x04000734 RID: 1844
		public static int weatherForTomorrow;

		// Token: 0x04000735 RID: 1845
		public static int currentSongIndex = 3;

		// Token: 0x04000736 RID: 1846
		public static int cursorTileHintCheckTimer;

		// Token: 0x04000737 RID: 1847
		public static int timerUntilMouseFade;

		// Token: 0x04000738 RID: 1848
		public static int minecartHighScore;

		// Token: 0x04000739 RID: 1849
		public static int whichFarm;

		// Token: 0x0400073A RID: 1850
		public static List<int> dealerCalicoJackTotal;

		// Token: 0x0400073B RID: 1851
		public static Color morningColor = Color.LightBlue;

		// Token: 0x0400073C RID: 1852
		public static Color eveningColor = new Color(255, 255, 0);

		// Token: 0x0400073D RID: 1853
		public static Color unselectedOptionColor = new Color(100, 100, 100);

		// Token: 0x0400073E RID: 1854
		public static Color screenGlowColor;

		// Token: 0x0400073F RID: 1855
		public static NPC currentSpeaker;

		// Token: 0x04000740 RID: 1856
		public static Random random = new Random(DateTime.Now.Millisecond);

		// Token: 0x04000741 RID: 1857
		public static Random recentMultiplayerRandom = new Random();

		// Token: 0x04000742 RID: 1858
		public static Dictionary<int, string> objectInformation;

		// Token: 0x04000743 RID: 1859
		public static Dictionary<int, string> bigCraftablesInformation;

		// Token: 0x04000744 RID: 1860
		public static List<Object> shippingBin = new List<Object>();

		// Token: 0x04000745 RID: 1861
		public static MineShaft mine;

		// Token: 0x04000746 RID: 1862
		public static List<HUDMessage> hudMessages = new List<HUDMessage>();

		// Token: 0x04000747 RID: 1863
		public static Dictionary<string, bool> eventConditions;

		// Token: 0x04000748 RID: 1864
		public static Dictionary<string, string> NPCGiftTastes;

		// Token: 0x04000749 RID: 1865
		public static float musicPlayerVolume;

		// Token: 0x0400074A RID: 1866
		public static float ambientPlayerVolume;

		// Token: 0x0400074B RID: 1867
		public static float pauseAccumulator;

		// Token: 0x0400074C RID: 1868
		public static float pauseTime;

		// Token: 0x0400074D RID: 1869
		public static float upPolling;

		// Token: 0x0400074E RID: 1870
		public static float downPolling;

		// Token: 0x0400074F RID: 1871
		public static float rightPolling;

		// Token: 0x04000750 RID: 1872
		public static float leftPolling;

		// Token: 0x04000751 RID: 1873
		public static float debrisSoundInterval;

		// Token: 0x04000752 RID: 1874
		public static float toolHold;

		// Token: 0x04000753 RID: 1875
		public static float windGust;

		// Token: 0x04000754 RID: 1876
		public static float dialogueButtonScale = 1f;

		// Token: 0x04000755 RID: 1877
		public static float creditsTimer;

		// Token: 0x04000756 RID: 1878
		public static float globalOutdoorLighting;

		// Token: 0x04000757 RID: 1879
		public static Cue currentSong;

		// Token: 0x04000758 RID: 1880
		public static AudioCategory musicCategory;

		// Token: 0x04000759 RID: 1881
		public static AudioCategory soundCategory;

		// Token: 0x0400075A RID: 1882
		public static AudioCategory ambientCategory;

		// Token: 0x0400075B RID: 1883
		public static AudioCategory footstepCategory;

		// Token: 0x0400075C RID: 1884
		public static PlayerIndex playerOneIndex = PlayerIndex.One;

		// Token: 0x0400075D RID: 1885
		public static AudioEngine audioEngine;

		// Token: 0x0400075E RID: 1886
		public static WaveBank waveBank;

		// Token: 0x0400075F RID: 1887
		public static SoundBank soundBank;

		// Token: 0x04000760 RID: 1888
		public static Vector2 shiny = Vector2.Zero;

		// Token: 0x04000761 RID: 1889
		public static Vector2 previousViewportPosition;

		// Token: 0x04000762 RID: 1890
		public static Vector2 currentCursorTile;

		// Token: 0x04000763 RID: 1891
		public static Vector2 lastCursorTile = Vector2.Zero;

		// Token: 0x04000764 RID: 1892
		public static RainDrop[] rainDrops = new RainDrop[70];

		// Token: 0x04000765 RID: 1893
		public static double chanceToRainTomorrow = 0.0;

		// Token: 0x04000766 RID: 1894
		public static Cue fuseSound;

		// Token: 0x04000767 RID: 1895
		public static Cue chargeUpSound;

		// Token: 0x04000768 RID: 1896
		public static Cue wind;

		// Token: 0x04000769 RID: 1897
		public static double dailyLuck = 0.001;

		// Token: 0x0400076A RID: 1898
		public static List<WeatherDebris> debrisWeather = new List<WeatherDebris>();

		// Token: 0x0400076B RID: 1899
		public static List<TemporaryAnimatedSprite> screenOverlayTempSprites = new List<TemporaryAnimatedSprite>();

		// Token: 0x0400076C RID: 1900
		public static byte gameMode;

		// Token: 0x0400076D RID: 1901
		public static byte multiplayerMode;

		// Token: 0x0400076E RID: 1902
		public static IEnumerator<int> currentLoader;

		// Token: 0x0400076F RID: 1903
		public static ulong uniqueIDForThisGame = (ulong)(DateTime.UtcNow - new DateTime(2012, 6, 22)).TotalSeconds;

		// Token: 0x04000770 RID: 1904
		public static LoadGameScreen loadGameScreen;

		// Token: 0x04000771 RID: 1905
		public static Stats stats = new Stats();

		// Token: 0x04000772 RID: 1906
		public static int[] cropsOfTheWeek;

		// Token: 0x04000773 RID: 1907
		public static Quest questOfTheDay;

		// Token: 0x04000774 RID: 1908
		public static MoneyMadeScreen moneyMadeScreen;

		// Token: 0x04000775 RID: 1909
		public static HashSet<LightSource> currentLightSources = new HashSet<LightSource>();

		// Token: 0x04000776 RID: 1910
		public static Color ambientLight;

		// Token: 0x04000777 RID: 1911
		public static Color outdoorLight = new Color(255, 255, 0);

		// Token: 0x04000778 RID: 1912
		public static Color textColor = new Color(34, 17, 34);

		// Token: 0x04000779 RID: 1913
		public static Color textShadowColor = new Color(206, 156, 95);

		// Token: 0x0400077A RID: 1914
		public static IClickableMenu activeClickableMenu;

		// Token: 0x0400077B RID: 1915
		public static IMinigame currentMinigame = null;

		// Token: 0x0400077C RID: 1916
		public static List<IClickableMenu> onScreenMenus = new List<IClickableMenu>();

		// Token: 0x0400077D RID: 1917
		private static int framesThisSecond;

		// Token: 0x0400077E RID: 1918
		private static int secondCounter;

		// Token: 0x0400077F RID: 1919
		private static int currentfps;

		// Token: 0x04000780 RID: 1920
		public static BloomComponent bloom;

		// Token: 0x04000781 RID: 1921
		public static Dictionary<int, string> achievements;

		// Token: 0x04000782 RID: 1922
		public static Object dishOfTheDay;

		// Token: 0x04000783 RID: 1923
		public static BuffsDisplay buffsDisplay;

		// Token: 0x04000784 RID: 1924
		public static DayTimeMoneyBox dayTimeMoneyBox;

		// Token: 0x04000785 RID: 1925
		public static Dictionary<long, Farmer> otherFarmers;

		// Token: 0x04000786 RID: 1926
		public static Server server;

		// Token: 0x04000787 RID: 1927
		public static Client client;

		// Token: 0x04000788 RID: 1928
		public static KeyboardDispatcher keyboardDispatcher;

		// Token: 0x04000789 RID: 1929
		public static Background background;

		// Token: 0x0400078A RID: 1930
		public static FarmEvent farmEvent;

		// Token: 0x0400078B RID: 1931
		public static Game1.afterFadeFunction afterFade;

		// Token: 0x0400078C RID: 1932
		public static Game1.afterFadeFunction afterDialogues;

		// Token: 0x0400078D RID: 1933
		public static Game1.afterFadeFunction afterViewport;

		// Token: 0x0400078E RID: 1934
		public static Game1.afterFadeFunction viewportReachedTarget;

		// Token: 0x0400078F RID: 1935
		public static Game1.afterFadeFunction afterPause;

		// Token: 0x04000790 RID: 1936
		public static GameTime currentGameTime;

		// Token: 0x04000791 RID: 1937
		public static List<DelayedAction> delayedActions = new List<DelayedAction>();

		// Token: 0x04000792 RID: 1938
		public static Stack<IClickableMenu> endOfNightMenus = new Stack<IClickableMenu>();

		// Token: 0x04000793 RID: 1939
		public static Options options;

		// Token: 0x04000794 RID: 1940
		public static Game1 game1;

		// Token: 0x04000795 RID: 1941
		public static Point lastMousePositionBeforeFade;

		// Token: 0x04000796 RID: 1942
		public static bool overrideGameMenuReset;

		// Token: 0x04000797 RID: 1943
		public static Point viewportCenter;

		// Token: 0x04000798 RID: 1944
		public static Vector2 viewportTarget = new Vector2(-2.14748365E+09f, -2.14748365E+09f);

		// Token: 0x04000799 RID: 1945
		public static float viewportSpeed = 2f;

		// Token: 0x0400079A RID: 1946
		public static int viewportHold;

		// Token: 0x0400079B RID: 1947
		public const float thumbstickToMouseModifier = 16f;

		// Token: 0x0400079C RID: 1948
		private static int thumbstickPollingTimer;

		// Token: 0x0400079D RID: 1949
		public static bool toggleFullScreen;

		// Token: 0x0400079E RID: 1950
		public static bool isFullscreen;

		// Token: 0x0400079F RID: 1951
		public static bool setToWindowedMode;

		// Token: 0x040007A0 RID: 1952
		public static bool setToFullscreen;

		// Token: 0x040007A1 RID: 1953
		public static string whereIsTodaysFest;

		// Token: 0x040007A2 RID: 1954
		public static bool farmerShouldPassOut;

		// Token: 0x040007A3 RID: 1955
		public const string NO_LETTER_MAIL = "%&NL&%";

		// Token: 0x040007A4 RID: 1956
		public static Vector2 currentViewportTarget;

		// Token: 0x040007A5 RID: 1957
		public static Vector2 viewportPositionLerp;

		// Token: 0x040007A6 RID: 1958
		public static float screenGlowRate = 0.005f;

		// Token: 0x040007A7 RID: 1959
		public static float screenGlowMax;

		// Token: 0x040007A8 RID: 1960
		public static bool haltAfterCheck = false;

		// Token: 0x040007A9 RID: 1961
		private string panModeString;

		// Token: 0x040007AA RID: 1962
		public static bool conventionMode = false;

		// Token: 0x040007AB RID: 1963
		private bool panFacingDirectionWait;

		// Token: 0x040007AC RID: 1964
		public static int thumbstickMotionMargin;

		// Token: 0x040007AD RID: 1965
		public static int triggerPolling;

		// Token: 0x040007AE RID: 1966
		public static int rightClickPolling;

		// Token: 0x040007AF RID: 1967
		private RenderTarget2D screen;

		// Token: 0x040007B0 RID: 1968
		private Matrix scaleMatrix = Matrix.CreateScale(32f, 32f, 1f);

		// Token: 0x040007B1 RID: 1969
		private Color bgColor = new Color(5, 3, 4);

		// Token: 0x040007B2 RID: 1970
		public static int mouseCursor = 0;

		// Token: 0x040007B3 RID: 1971
		public static float mouseCursorTransparency = 1f;

		// Token: 0x040007B4 RID: 1972
		public static NPC objectDialoguePortraitPerson;

		// Token: 0x0200016F RID: 367
		// Token: 0x06001377 RID: 4983
		public delegate void afterFadeFunction();
	}
}
