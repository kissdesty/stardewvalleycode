using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x02000115 RID: 277
	public class StrengthGame : IClickableMenu
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x0014A7C8 File Offset: 0x001489C8
		public StrengthGame() : base(31 * Game1.tileSize + 6 * Game1.pixelZoom, 56 * Game1.tileSize + 10 * Game1.pixelZoom, 5 * Game1.pixelZoom, 34 * Game1.pixelZoom, false)
		{
			this.power = 0f;
			this.changeSpeed = (float)(3 + Game1.random.Next(2));
			this.barColor = Color.Red;
			Game1.playSound("cowboy_monsterhit");
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0014A84C File Offset: 0x00148A4C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.clicked)
			{
				Game1.player.faceDirection(1);
				Game1.player.CurrentToolIndex = 107;
				Game1.player.FarmerSprite.animateOnce(168, 80f, 8);
				Game1.player.toolOverrideFunction = new AnimatedSprite.endOfAnimationBehavior(this.afterSwingAnimation);
				Game1.player.FarmerSprite.ignoreDefaultActionThisTime = false;
				this.clicked = true;
			}
			if (this.showedResult && Game1.dialogueTyping)
			{
				Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
			}
			if (this.showedResult && !Game1.dialogueTyping)
			{
				Game1.player.toolOverrideFunction = null;
				Game1.exitActiveMenu();
				Game1.afterDialogues = null;
				Game1.pressActionButton(Game1.oldKBState, Game1.oldMouseState, Game1.oldPadState);
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0014A920 File Offset: 0x00148B20
		public void afterSwingAnimation(Farmer who)
		{
			if (!Game1.isFestival())
			{
				who.toolOverrideFunction = null;
				return;
			}
			this.changeSpeed = 0f;
			Game1.playSound("hammer");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2(30f, 56f) * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
			if (this.power >= 99f)
			{
				this.endTimer = 2000f;
				return;
			}
			this.endTimer = 1000f;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0012FA83 File Offset: 0x0012DC83
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0014A9BC File Offset: 0x00148BBC
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.changeSpeed == 0f)
			{
				this.endTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.power >= 99f)
				{
					if (this.endTimer < 1500f)
					{
						if (!this.victorySound)
						{
							this.victorySound = true;
							Game1.playSound("getNewSpecialItem");
							this.barColor = Color.Orange;
						}
						if (!this.showedResult && Game1.random.NextDouble() < 0.08)
						{
							Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(10 + Game1.random.Next(2), new Vector2(31f, 55f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize)), Color.Yellow, 8, false, 100f, 0, -1, -1f, -1, 0)
							{
								layerDepth = 1f
							});
						}
					}
				}
				else
				{
					this.transparency = Math.Max(0f, this.transparency - 0.02f);
				}
				if (this.endTimer <= 0f && !this.showedResult)
				{
					this.showedResult = true;
					if (this.power >= 99f)
					{
						Game1.player.festivalScore++;
						Game1.playSound("purchase");
						Game1.drawObjectDialogue("HA! Now that's what I like to see! Here, have a star token.");
					}
					else if (this.power >= 2f)
					{
						string strengthLevel = "";
						switch ((int)this.power)
						{
						case 2:
						case 3:
							strengthLevel = "Plankton";
							break;
						case 4:
						case 5:
							strengthLevel = "Skim Milk";
							break;
						case 6:
						case 7:
							strengthLevel = "Wet Tissue";
							break;
						case 8:
						case 9:
							strengthLevel = "Alfredo Sauce";
							break;
						case 10:
						case 11:
							strengthLevel = "Toothpick";
							break;
						case 12:
						case 13:
							strengthLevel = "Shrimp";
							break;
						case 14:
						case 15:
							strengthLevel = "Goldfish";
							break;
						case 16:
						case 17:
							strengthLevel = "Soggy Spaghetti";
							break;
						case 18:
						case 19:
							strengthLevel = "Baby Duck";
							break;
						case 20:
						case 21:
							strengthLevel = "George's Knee";
							break;
						case 22:
						case 23:
							strengthLevel = "Mouse";
							break;
						case 24:
						case 25:
							strengthLevel = "Carrot Stick";
							break;
						case 26:
						case 27:
							strengthLevel = "Twig";
							break;
						case 28:
						case 29:
							strengthLevel = "Trout";
							break;
						case 30:
						case 31:
							strengthLevel = "Paper Mache";
							break;
						case 32:
						case 33:
							strengthLevel = "Buttermilk";
							break;
						case 34:
						case 35:
							strengthLevel = "Pancake";
							break;
						case 36:
						case 37:
							strengthLevel = "Cardboard";
							break;
						case 38:
						case 39:
							strengthLevel = "Balsa Wood";
							break;
						case 40:
						case 41:
							strengthLevel = "Hardened Clay";
							break;
						case 42:
						case 43:
							strengthLevel = "Prairie Dog";
							break;
						case 44:
						case 45:
							strengthLevel = "Leather Boot";
							break;
						case 46:
						case 47:
							strengthLevel = "Hot Mustard";
							break;
						case 48:
						case 49:
							strengthLevel = "Scrap Metal";
							break;
						case 50:
						case 51:
							strengthLevel = "Sandstone";
							break;
						case 52:
						case 53:
							strengthLevel = "Sheep Dog";
							break;
						case 54:
						case 55:
							strengthLevel = "Small Donkey";
							break;
						case 56:
						case 57:
						case 58:
						case 59:
							strengthLevel = "Angry Hog";
							break;
						case 60:
						case 61:
						case 62:
						case 63:
							strengthLevel = "Boulder";
							break;
						case 64:
						case 65:
						case 66:
						case 67:
						case 68:
							strengthLevel = "Gym Teacher";
							break;
						case 69:
						case 70:
						case 71:
						case 72:
							strengthLevel = "Orc";
							break;
						case 73:
						case 74:
						case 75:
						case 76:
							strengthLevel = "Tree Trunk";
							break;
						case 77:
						case 78:
						case 79:
							strengthLevel = "Iron";
							break;
						case 80:
						case 81:
							strengthLevel = "Bodybuilder";
							break;
						case 82:
						case 83:
							strengthLevel = "Mountain Troll";
							break;
						case 84:
						case 85:
						case 86:
							strengthLevel = "Lumberjack";
							break;
						case 87:
						case 89:
							strengthLevel = "Horse";
							break;
						case 88:
						case 90:
							strengthLevel = "Ox";
							break;
						case 91:
						case 92:
							strengthLevel = "Bulldozer";
							break;
						case 93:
						case 94:
						case 95:
						case 96:
							strengthLevel = "Gorilla";
							break;
						case 97:
						case 98:
							strengthLevel = "Mammoth";
							break;
						}
						Game1.playSound("dwop");
						Game1.drawObjectDialogue("Strength Level: " + strengthLevel);
					}
					else
					{
						Game1.player.festivalScore++;
						Game1.playSound("purchase");
						Game1.drawObjectDialogue(Game1.parseText("Wow. Zero strength. I'm actually impressed with how weak you are. Here's a star token."));
					}
					Game1.afterDialogues = new Game1.afterFadeFunction(base.exitThisMenuNoSound);
					return;
				}
			}
			else
			{
				this.power += this.changeSpeed;
				if (this.power > 100f)
				{
					this.power = 100f;
					this.changeSpeed = -this.changeSpeed;
					return;
				}
				if (this.power < 0f)
				{
					this.power = 0f;
					this.changeSpeed = -this.changeSpeed;
				}
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0014AF64 File Offset: 0x00149164
		public override void draw(SpriteBatch b)
		{
			if (!Game1.dialogueUp)
			{
				b.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Rectangle(this.xPositionOnScreen, (int)((float)this.yPositionOnScreen - this.power / 100f * (float)this.height), this.width, (int)(this.power / 100f * (float)this.height))), new Rectangle?(Game1.staminaRect.Bounds), this.barColor * this.transparency, 0f, Vector2.Zero, SpriteEffects.None, 1E-05f);
			}
			if (Game1.player.FarmerSprite.isOnToolAnimation())
			{
				Game1.drawTool(Game1.player, Game1.player.CurrentToolIndex);
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x04001151 RID: 4433
		private float power;

		// Token: 0x04001152 RID: 4434
		private float changeSpeed;

		// Token: 0x04001153 RID: 4435
		private float endTimer;

		// Token: 0x04001154 RID: 4436
		private float transparency = 1f;

		// Token: 0x04001155 RID: 4437
		private Color barColor;

		// Token: 0x04001156 RID: 4438
		private bool victorySound;

		// Token: 0x04001157 RID: 4439
		private bool clicked;

		// Token: 0x04001158 RID: 4440
		private bool showedResult;
	}
}
