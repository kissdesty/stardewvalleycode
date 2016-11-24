using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Objects
{
	// Token: 0x02000091 RID: 145
	public class TV : Furniture
	{
		// Token: 0x06000A94 RID: 2708 RVA: 0x000DDA14 File Offset: 0x000DBC14
		public TV()
		{
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000DDA1C File Offset: 0x000DBC1C
		public TV(int which, Vector2 tile) : base(which, tile)
		{
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x000DDA28 File Offset: 0x000DBC28
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			List<Response> channels = new List<Response>();
			channels.Add(new Response("Weather", "Weather Report"));
			channels.Add(new Response("Fortune", "Fortune Teller"));
			string day = Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth);
			if (day.Equals("Mon") || day.Equals("Thu"))
			{
				channels.Add(new Response("Livin'", "Livin' Off The Land"));
			}
			if (day.Equals("Sun"))
			{
				channels.Add(new Response("The", "The Queen Of Sauce"));
			}
			if (day.Equals("Wed") && Game1.stats.DaysPlayed > 7u)
			{
				channels.Add(new Response("The", "The Queen Of Sauce (Re-run)"));
			}
			channels.Add(new Response("(Leave)", "(Leave)"));
			Game1.currentLocation.createQuestionDialogue("Select channel:", channels.ToArray(), new GameLocation.afterQuestionBehavior(this.selectChannel), null);
			Game1.player.Halt();
			return true;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x000DDB38 File Offset: 0x000DBD38
		public override Item getOne()
		{
			TV expr_11 = new TV(this.parentSheetIndex, this.tileLocation);
			expr_11.drawPosition = this.drawPosition;
			expr_11.defaultBoundingBox = this.defaultBoundingBox;
			expr_11.boundingBox = this.boundingBox;
			expr_11.currentRotation = this.currentRotation - 1;
			expr_11.rotations = this.rotations;
			expr_11.rotate();
			return expr_11;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x000DDB9A File Offset: 0x000DBD9A
		public override void updateWhenCurrentLocation(GameTime time)
		{
			base.updateWhenCurrentLocation(time);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000DDBA4 File Offset: 0x000DBDA4
		public void selectChannel(Farmer who, string answer)
		{
			string a = answer.Split(new char[]
			{
				' '
			})[0];
			if (a == "Weather")
			{
				this.currentChannel = 2;
				this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 305, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				Game1.drawObjectDialogue(Game1.parseText(this.getWeatherChannelOpening()));
				Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
				return;
			}
			if (a == "Fortune")
			{
				this.currentChannel = 3;
				this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 305, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				Game1.drawObjectDialogue(Game1.parseText(this.getFortuneTellerOpening()));
				Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
				return;
			}
			if (a == "Livin'")
			{
				this.currentChannel = 4;
				this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(517, 361, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				Game1.drawObjectDialogue(Game1.parseText("Welcome to \"Livin' Off The Land\". We're back again with another tip for y'all. Now listen up:"));
				Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
				return;
			}
			if (!(a == "The"))
			{
				return;
			}
			this.currentChannel = 5;
			this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(602, 361, 42, 28), 150f, 2, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
			Game1.drawObjectDialogue(Game1.parseText("Greetings! It is I, the queen of sauce... here to teach you a new mouth-watering recipe from my secret cookbook. This week's dish..."));
			Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000DDE64 File Offset: 0x000DC064
		private string getFortuneTellerOpening()
		{
			switch (Game1.random.Next(5))
			{
			case 0:
				return "Ah... I sense that a new viewer has joined us. A young " + (Game1.player.isMale ? "man" : "lady") + " from... Stardew Valley? Welcome, welcome!";
			case 1:
				return "Ah... yes, I can hear the spirits whispering something to me... ";
			case 2:
				return "Welcome back to 'Welwick's Oracle'... If you seek hidden knowledge of the future, well you've come to the right place.";
			case 3:
				return "Hoo.. I see a glimmer within my scrying orb... A shard of knowledge from the future!";
			case 4:
				return "Welcome to Welwick's Oracle... the ONLY show where the voice of the spirits is channeled directly... to YOU.";
			default:
				return "";
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000DDEE0 File Offset: 0x000DC0E0
		private string getWeatherChannelOpening()
		{
			return "Welcome to KOZU 5... your number one source for weather, news, and entertainment.^And now, the weather forecast for tomorrow...";
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000DDEF2 File Offset: 0x000DC0F2
		public float getScreenSizeModifier()
		{
			if (this.parentSheetIndex != 1468)
			{
				return (float)Game1.pixelZoom / 2f;
			}
			return (float)Game1.pixelZoom;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000DDF14 File Offset: 0x000DC114
		public Vector2 getScreenPosition()
		{
			if (this.parentSheetIndex == 1466)
			{
				return new Vector2((float)(this.boundingBox.X + 6 * Game1.pixelZoom), (float)this.boundingBox.Y);
			}
			if (this.parentSheetIndex == 1468)
			{
				return new Vector2((float)(this.boundingBox.X + 3 * Game1.pixelZoom), (float)(this.boundingBox.Y - Game1.tileSize * 2 + Game1.pixelZoom * 8));
			}
			if (this.parentSheetIndex == 1680)
			{
				return new Vector2((float)(this.boundingBox.X + 6 * Game1.pixelZoom), (float)(this.boundingBox.Y - 3 * Game1.pixelZoom));
			}
			return Vector2.Zero;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000DDFD8 File Offset: 0x000DC1D8
		public void proceedToNextScene()
		{
			if (this.currentChannel == 2)
			{
				if (this.screenOverlay == null)
				{
					this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(497, 305, 42, 28), 9999f, 1, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
					Game1.drawObjectDialogue(Game1.parseText(this.getWeatherForecast()));
					this.setWeatherOverlay();
					Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
					return;
				}
				this.turnOffTV();
				return;
			}
			else if (this.currentChannel == 3)
			{
				if (this.screenOverlay == null)
				{
					this.screen = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(624, 305, 42, 28), 9999f, 1, 999999, this.getScreenPosition(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 1E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
					Game1.drawObjectDialogue(Game1.parseText(this.getFortuneForecast()));
					this.setFortuneOverlay();
					Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
					return;
				}
				this.turnOffTV();
				return;
			}
			else
			{
				if (this.currentChannel != 4)
				{
					if (this.currentChannel == 5)
					{
						if (this.screenOverlay == null)
						{
							Game1.multipleDialogues(this.getWeeklyRecipe());
							Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
							this.screenOverlay = new TemporaryAnimatedSprite
							{
								alpha = 1E-07f
							};
							return;
						}
						this.turnOffTV();
					}
					return;
				}
				if (this.screenOverlay == null)
				{
					Game1.drawObjectDialogue(Game1.parseText(this.getTodaysTip()));
					Game1.afterDialogues = new Game1.afterFadeFunction(this.proceedToNextScene);
					this.screenOverlay = new TemporaryAnimatedSprite
					{
						alpha = 1E-07f
					};
					return;
				}
				this.turnOffTV();
				return;
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x000DE1EB File Offset: 0x000DC3EB
		public void turnOffTV()
		{
			this.screen = null;
			this.screenOverlay = null;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000DE1FC File Offset: 0x000DC3FC
		private void setWeatherOverlay()
		{
			switch (Game1.weatherForTomorrow)
			{
			case 0:
			case 6:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 1:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(465, 333, 13, 13), 70f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 2:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, Game1.currentSeason.Equals("spring") ? new Rectangle(465, 359, 13, 13) : (Game1.currentSeason.Equals("fall") ? new Rectangle(413, 359, 13, 13) : new Rectangle(465, 346, 13, 13)), 70f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 3:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 346, 13, 13), 120f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 4:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(413, 372, 13, 13), 120f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			case 5:
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(465, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(3f, 3f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000DE5E0 File Offset: 0x000DC7E0
		private string getTodaysTip()
		{
			if (Game1.temporaryContent == null)
			{
				Game1.temporaryContent = Game1.content.CreateTemporary();
			}
			Dictionary<string, string> tips = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\TV\\TipChannel");
			if (!tips.ContainsKey(string.Concat(Game1.stats.DaysPlayed % 224u)))
			{
				return "Just kidding... I'm not ready. I don't have anything for you today.";
			}
			return tips[string.Concat(Game1.stats.DaysPlayed % 224u)];
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000DE65C File Offset: 0x000DC85C
		private string[] getWeeklyRecipe()
		{
			if (Game1.temporaryContent == null)
			{
				Game1.temporaryContent = Game1.content.CreateTemporary();
			}
			string[] text = new string[2];
			int whichWeek = (int)(Game1.stats.DaysPlayed % 224u / 7u);
			Dictionary<string, string> cookingRecipeChannel = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\TV\\CookingChannel");
			if (Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Wed"))
			{
				Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
				whichWeek = Math.Max(1, 1 + r.Next((int)(Game1.stats.DaysPlayed % 224u)) / 7);
			}
			try
			{
				string recipeName = cookingRecipeChannel[string.Concat(whichWeek)].Split(new char[]
				{
					'/'
				})[0];
				text[0] = cookingRecipeChannel[string.Concat(whichWeek)].Split(new char[]
				{
					'/'
				})[1];
				text[1] = (Game1.player.cookingRecipes.ContainsKey(cookingRecipeChannel[string.Concat(whichWeek)].Split(new char[]
				{
					'/'
				})[0]) ? ("You already know how to cook " + recipeName + ".") : ("You learned how to cook '" + recipeName + "'!"));
				if (!Game1.player.cookingRecipes.ContainsKey(recipeName))
				{
					Game1.player.cookingRecipes.Add(recipeName, 0);
				}
			}
			catch (Exception)
			{
				string recipeName2 = cookingRecipeChannel["1"].Split(new char[]
				{
					'/'
				})[0];
				text[0] = cookingRecipeChannel["1"].Split(new char[]
				{
					'/'
				})[1];
				text[1] = (Game1.player.cookingRecipes.ContainsKey(cookingRecipeChannel["1"].Split(new char[]
				{
					'/'
				})[0]) ? ("You already know how to cook " + recipeName2 + ".") : ("You learned how to cook '" + recipeName2 + "'!"));
				if (!Game1.player.cookingRecipes.ContainsKey(recipeName2))
				{
					Game1.player.cookingRecipes.Add(recipeName2, 0);
				}
			}
			return text;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000DE898 File Offset: 0x000DCA98
		private string getWeatherForecast()
		{
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth % 12 == 0)
			{
				Game1.weatherForTomorrow = 3;
			}
			if (Game1.stats.DaysPlayed == 2u)
			{
				Game1.weatherForTomorrow = 1;
			}
			if (Game1.dayOfMonth == 28)
			{
				Game1.weatherForTomorrow = 0;
			}
			switch (Game1.weatherForTomorrow)
			{
			case 0:
			case 6:
				if (Game1.random.NextDouble() >= 0.5)
				{
					return "It's going to be clear and sunny all day.";
				}
				return "It's going to be a beautiful, sunny day tomorrow!";
			case 1:
				return "It's going to rain all day tomorrow.";
			case 2:
				if (Game1.currentSeason.Equals("spring"))
				{
					return "Partially cloudy with a light breeze. Expect lots of pollen!";
				}
				if (!Game1.currentSeason.Equals("fall"))
				{
					return "It's going to snow all day. Make sure you bundle up, folks!";
				}
				return "It's going to be cloudy, with gusts of wind throughout the day.";
			case 3:
				return "Looks like a storm is approaching. Thunder and lightning is expected.";
			case 4:
			{
				LocalizedContentManager temporaryContent = Game1.content.CreateTemporary();
				Dictionary<string, string> festivalData;
				try
				{
					festivalData = temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (Game1.dayOfMonth + 1));
				}
				catch (Exception)
				{
					return "Um... that's odd. My information sheet just says 'null'. This is embarrassing... ";
				}
				string festName = festivalData["name"];
				string locationName = festivalData["conditions"].Split(new char[]
				{
					'/'
				})[0];
				int startTime = Convert.ToInt32(festivalData["conditions"].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[0]);
				int endTime = Convert.ToInt32(festivalData["conditions"].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[1]);
				string locationFullName = "";
				if (!(locationName == "Town"))
				{
					if (!(locationName == "Beach"))
					{
						if (locationName == "Forest")
						{
							locationFullName = "in the forest";
						}
					}
					else
					{
						locationFullName = "on the beach";
					}
				}
				else
				{
					locationFullName = "in Pelican Town";
				}
				return string.Concat(new string[]
				{
					"It's going to be clear and sunny tomorrow... perfect weather for the ",
					festName,
					"! The event will take place ",
					locationFullName,
					", starting between ",
					Game1.getTimeOfDayString(startTime),
					" and ",
					Game1.getTimeOfDayString(endTime),
					". Don't be late!"
				});
			}
			case 5:
				if (Game1.random.NextDouble() >= 0.5)
				{
					return "Expect a few inches of snow tomorrow.";
				}
				return "Bundle up, folks. It's going to snow tomorrow!";
			default:
				return "";
			}
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000DEB28 File Offset: 0x000DCD28
		private void setFortuneOverlay()
		{
			if (Game1.dailyLuck < -0.07)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			if (Game1.dailyLuck < -0.02)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 346, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			if (Game1.dailyLuck > 0.07)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(644, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			if (Game1.dailyLuck > 0.02)
			{
				this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(592, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
				return;
			}
			this.screenOverlay = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(540, 333, 13, 13), 100f, 4, 999999, this.getScreenPosition() + new Vector2(15f, 1f) * this.getScreenSizeModifier(), false, false, (float)(this.boundingBox.Bottom - 1) / 10000f + 2E-05f, 0f, Color.White, this.getScreenSizeModifier(), 0f, 0f, 0f, false);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000DEE50 File Offset: 0x000DD050
		private string getFortuneForecast()
		{
			string fortune;
			if (Game1.dailyLuck == -0.12)
			{
				fortune = "The spirits are furious. Apparently someone tried to give them a worthless offering... Hmm. It's unwise to play jokes on the spirits!";
			}
			else if (Game1.dailyLuck < -0.07)
			{
				fortune = "The spirits are very displeased today. They will do their best to make your life difficult.";
			}
			else if (Game1.dailyLuck < -0.02)
			{
				fortune = "The spirits are " + ((Game1.random.NextDouble() < 0.5) ? "somewhat annoyed" : "mildly perturbed") + " today. Luck will not be on your side.";
			}
			else if (Game1.dailyLuck == 0.12)
			{
				fortune = "The spirits are joyous! Someone gave them a nice offering today, and they are very pleased.";
			}
			else if (Game1.dailyLuck > 0.07)
			{
				fortune = "The spirits are very happy today! They will do their best to shower everyone with good fortune.";
			}
			else if (Game1.dailyLuck > 0.02)
			{
				fortune = "The spirits are in good humor today. I think you'll have a little extra luck.";
			}
			else
			{
				fortune = "The spirits feel neutral today. The day is in your hands.";
			}
			if (Game1.dailyLuck == 0.0)
			{
				fortune = "This is rare. The spirits feel absolutely neutral today.";
			}
			return fortune;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000DEF44 File Offset: 0x000DD144
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			base.draw(spriteBatch, x, y, alpha);
			if (this.screen != null)
			{
				this.screen.update(Game1.currentGameTime);
				this.screen.draw(spriteBatch, false, 0, 0);
				if (this.screenOverlay != null)
				{
					this.screenOverlay.update(Game1.currentGameTime);
					this.screenOverlay.draw(spriteBatch, false, 0, 0);
				}
			}
		}

		// Token: 0x04000AEF RID: 2799
		public const int customChannel = 1;

		// Token: 0x04000AF0 RID: 2800
		public const int weatherChannel = 2;

		// Token: 0x04000AF1 RID: 2801
		public const int fortuneTellerChannel = 3;

		// Token: 0x04000AF2 RID: 2802
		public const int tipsChannel = 4;

		// Token: 0x04000AF3 RID: 2803
		public const int cookingChannel = 5;

		// Token: 0x04000AF4 RID: 2804
		private int currentChannel;

		// Token: 0x04000AF5 RID: 2805
		private TemporaryAnimatedSprite screen;

		// Token: 0x04000AF6 RID: 2806
		private TemporaryAnimatedSprite screenOverlay;
	}
}
