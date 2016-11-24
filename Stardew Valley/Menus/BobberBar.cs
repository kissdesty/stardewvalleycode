using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;

namespace StardewValley.Menus
{
	// Token: 0x020000D8 RID: 216
	public class BobberBar : IClickableMenu
	{
		// Token: 0x06000D8A RID: 3466 RVA: 0x001124B0 File Offset: 0x001106B0
		public BobberBar(int whichFish, float fishSize, bool treasure, int bobber) : base(0, 0, 96, 636, false)
		{
			this.treasure = treasure;
			this.treasureAppearTimer = (float)Game1.random.Next(1000, 3000);
			this.fadeIn = true;
			this.scale = 0f;
			this.whichFish = whichFish;
			Dictionary<int, string> data = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
			this.bobberBarHeight = Game1.tileSize * 3 / 2 + Game1.player.FishingLevel * 8;
			this.bossFish = FishingRod.isFishBossFish(whichFish);
			if (Game1.player.fishCaught != null && Game1.player.fishCaught.Count == 0)
			{
				this.distanceFromCatching = 0.1f;
			}
			if (data.ContainsKey(whichFish))
			{
				string[] rawData = data[whichFish].Split(new char[]
				{
					'/'
				});
				this.difficulty = (float)Convert.ToInt32(rawData[1]);
				string a = rawData[2].ToLower();
				if (!(a == "mixed"))
				{
					if (!(a == "dart"))
					{
						if (!(a == "smooth"))
						{
							if (!(a == "floater"))
							{
								if (a == "sinker")
								{
									this.motionType = 3;
								}
							}
							else
							{
								this.motionType = 4;
							}
						}
						else
						{
							this.motionType = 2;
						}
					}
					else
					{
						this.motionType = 1;
					}
				}
				else
				{
					this.motionType = 0;
				}
				this.minFishSize = Convert.ToInt32(rawData[3]);
				this.maxFishSize = Convert.ToInt32(rawData[4]);
				this.fishSize = (int)((float)this.minFishSize + (float)(this.maxFishSize - this.minFishSize) * fishSize);
				this.fishSize++;
				this.perfect = true;
				this.fishQuality = (((double)fishSize < 0.33) ? 0 : (((double)fishSize < 0.66) ? 1 : 2));
				this.fishSizeReductionTimer = 800;
			}
			switch (Game1.player.FacingDirection)
			{
			case 0:
				this.xPositionOnScreen = (int)Game1.player.position.X - Game1.tileSize - 132;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				break;
			case 1:
				this.xPositionOnScreen = (int)Game1.player.position.X - Game1.tileSize - 132;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				break;
			case 2:
				this.xPositionOnScreen = (int)Game1.player.position.X - Game1.tileSize - 132;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				break;
			case 3:
				this.xPositionOnScreen = (int)Game1.player.position.X + Game1.tileSize * 2;
				this.yPositionOnScreen = (int)Game1.player.position.Y - 274;
				this.flipBubble = true;
				break;
			}
			this.xPositionOnScreen -= Game1.viewport.X;
			this.yPositionOnScreen -= Game1.viewport.Y + Game1.tileSize;
			if (this.xPositionOnScreen + 96 > Game1.viewport.Width)
			{
				this.xPositionOnScreen = Game1.viewport.Width - 96;
			}
			else if (this.xPositionOnScreen < 0)
			{
				this.xPositionOnScreen = 0;
			}
			if (this.yPositionOnScreen < 0)
			{
				this.yPositionOnScreen = 0;
			}
			else if (this.yPositionOnScreen + 636 > Game1.viewport.Height)
			{
				this.yPositionOnScreen = Game1.viewport.Height - 636;
			}
			if (bobber == 695)
			{
				this.bobberBarHeight += 24;
			}
			this.bobberBarPos = (float)(568 - this.bobberBarHeight);
			this.bobberPosition = 508f;
			this.bobberTargetPosition = (100f - this.difficulty) / 100f * 548f;
			if (Game1.soundBank != null)
			{
				BobberBar.reelSound = Game1.soundBank.GetCue("fastReel");
			}
			this.whichBobber = bobber;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x001128FC File Offset: 0x00110AFC
		public override void update(GameTime time)
		{
			if (this.sparkleText != null && this.sparkleText.update(time))
			{
				this.sparkleText = null;
			}
			if (this.everythingShakeTimer > 0f)
			{
				this.everythingShakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
				this.everythingShake = new Vector2((float)Game1.random.Next(-10, 11) / 10f, (float)Game1.random.Next(-10, 11) / 10f);
				if (this.everythingShakeTimer <= 0f)
				{
					this.everythingShake = Vector2.Zero;
				}
			}
			if (this.fadeIn)
			{
				this.scale += 0.05f;
				if (this.scale >= 1f)
				{
					this.scale = 1f;
					this.fadeIn = false;
				}
			}
			else if (this.fadeOut)
			{
				if (this.everythingShakeTimer > 0f || this.sparkleText != null)
				{
					return;
				}
				this.scale -= 0.05f;
				if (this.scale <= 0f)
				{
					this.scale = 0f;
					this.fadeOut = false;
					if (this.distanceFromCatching > 0.9f && Game1.player.CurrentTool is FishingRod)
					{
						(Game1.player.CurrentTool as FishingRod).pullFishFromWater(this.whichFish, this.fishSize, this.fishQuality, (int)this.difficulty, this.treasureCaught, this.perfect);
					}
					else
					{
						if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
						{
							(Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, true);
						}
						Game1.player.completelyStopAnimatingOrDoingAction();
					}
					Game1.exitActiveMenu();
				}
			}
			else
			{
				if (Game1.random.NextDouble() < (double)(this.difficulty * (float)((this.motionType == 2) ? 20 : 1) / 4000f) && (this.motionType != 2 || this.bobberTargetPosition == -1f))
				{
					float spaceBelow = 548f - this.bobberPosition;
					float spaceAbove = this.bobberPosition;
					float percent = Math.Min(99f, this.difficulty + (float)Game1.random.Next(10, 45)) / 100f;
					this.bobberTargetPosition = this.bobberPosition + (float)Game1.random.Next(-(int)spaceAbove, (int)spaceBelow) * percent;
				}
				if (this.motionType == 4)
				{
					this.floaterSinkerAcceleration = Math.Max(this.floaterSinkerAcceleration - 0.01f, -1.5f);
				}
				else if (this.motionType == 3)
				{
					this.floaterSinkerAcceleration = Math.Min(this.floaterSinkerAcceleration + 0.01f, 1.5f);
				}
				if (Math.Abs(this.bobberPosition - this.bobberTargetPosition) > 3f && this.bobberTargetPosition != -1f)
				{
					this.bobberAcceleration = (this.bobberTargetPosition - this.bobberPosition) / ((float)Game1.random.Next(10, 30) + (100f - Math.Min(100f, this.difficulty)));
					this.bobberSpeed += (this.bobberAcceleration - this.bobberSpeed) / 5f;
				}
				else if (this.motionType != 2 && Game1.random.NextDouble() < (double)(this.difficulty / 2000f))
				{
					this.bobberTargetPosition = this.bobberPosition + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-100, -51) : Game1.random.Next(50, 101));
				}
				else
				{
					this.bobberTargetPosition = -1f;
				}
				if (this.motionType == 1 && Game1.random.NextDouble() < (double)(this.difficulty / 1000f))
				{
					this.bobberTargetPosition = this.bobberPosition + (float)((Game1.random.NextDouble() < 0.5) ? Game1.random.Next(-100 - (int)this.difficulty * 2, -51) : Game1.random.Next(50, 101 + (int)this.difficulty * 2));
				}
				this.bobberTargetPosition = Math.Max(-1f, Math.Min(this.bobberTargetPosition, 548f));
				this.bobberPosition += this.bobberSpeed + this.floaterSinkerAcceleration;
				if (this.bobberPosition > 532f)
				{
					this.bobberPosition = 532f;
				}
				else if (this.bobberPosition < 0f)
				{
					this.bobberPosition = 0f;
				}
				this.bobberInBar = (this.bobberPosition + 16f <= this.bobberBarPos - 32f + (float)this.bobberBarHeight && this.bobberPosition - 16f >= this.bobberBarPos - 32f);
				if (this.bobberPosition >= (float)(548 - this.bobberBarHeight) && this.bobberBarPos >= (float)(568 - this.bobberBarHeight - 4))
				{
					this.bobberInBar = true;
				}
				bool arg_56C_0 = this.buttonPressed;
				this.buttonPressed = (Game1.oldMouseState.LeftButton == ButtonState.Pressed || Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton) || (Game1.options.gamepadControls && (Game1.oldPadState.IsButtonDown(Buttons.X) || Game1.oldPadState.IsButtonDown(Buttons.A))));
				if (!arg_56C_0 && this.buttonPressed)
				{
					Game1.playSound("fishingRodBend");
				}
				float gravity = this.buttonPressed ? -0.25f : 0.25f;
				if (this.buttonPressed && gravity < 0f && (this.bobberBarPos == 0f || this.bobberBarPos == (float)(568 - this.bobberBarHeight)))
				{
					this.bobberBarSpeed = 0f;
				}
				if (this.bobberInBar)
				{
					gravity *= ((this.whichBobber == 691) ? 0.3f : 0.6f);
					if (this.whichBobber == 691)
					{
						if (this.bobberPosition + 16f < this.bobberBarPos + (float)(this.bobberBarHeight / 2))
						{
							this.bobberBarSpeed -= 0.2f;
						}
						else
						{
							this.bobberBarSpeed += 0.2f;
						}
					}
				}
				float oldPos = this.bobberBarPos;
				this.bobberBarSpeed += gravity;
				this.bobberBarPos += this.bobberBarSpeed;
				if (this.bobberBarPos + (float)this.bobberBarHeight > 568f)
				{
					this.bobberBarPos = (float)(568 - this.bobberBarHeight);
					this.bobberBarSpeed = -this.bobberBarSpeed * 2f / 3f * ((this.whichBobber == 692) ? 0.1f : 1f);
					if (oldPos + (float)this.bobberBarHeight < 568f)
					{
						Game1.playSound("shiny4");
					}
				}
				else if (this.bobberBarPos < 0f)
				{
					this.bobberBarPos = 0f;
					this.bobberBarSpeed = -this.bobberBarSpeed * 2f / 3f;
					if (oldPos > 0f)
					{
						Game1.playSound("shiny4");
					}
				}
				bool treasureInBar = false;
				if (this.treasure)
				{
					float oldTreasureAppearTimer = this.treasureAppearTimer;
					this.treasureAppearTimer -= (float)time.ElapsedGameTime.Milliseconds;
					if (this.treasureAppearTimer <= 0f)
					{
						if (this.treasureScale < 1f && !this.treasureCaught)
						{
							if (oldTreasureAppearTimer > 0f)
							{
								this.treasurePosition = (float)((this.bobberBarPos > 274f) ? Game1.random.Next(8, (int)this.bobberBarPos - 20) : Game1.random.Next(Math.Min(528, (int)this.bobberBarPos + this.bobberBarHeight), 500));
								Game1.playSound("dwop");
							}
							this.treasureScale = Math.Min(1f, this.treasureScale + 0.1f);
						}
						treasureInBar = (this.treasurePosition + 16f <= this.bobberBarPos - 32f + (float)this.bobberBarHeight && this.treasurePosition - 16f >= this.bobberBarPos - 32f);
						if (treasureInBar && !this.treasureCaught)
						{
							this.treasureCatchLevel += 0.0135f;
							this.treasureShake = new Vector2((float)Game1.random.Next(-2, 3), (float)Game1.random.Next(-2, 3));
							if (this.treasureCatchLevel >= 1f)
							{
								Game1.playSound("newArtifact");
								this.treasureCaught = true;
							}
						}
						else if (this.treasureCaught)
						{
							this.treasureScale = Math.Max(0f, this.treasureScale - 0.1f);
						}
						else
						{
							this.treasureShake = Vector2.Zero;
							this.treasureCatchLevel = Math.Max(0f, this.treasureCatchLevel - 0.01f);
						}
					}
				}
				if (this.bobberInBar)
				{
					this.distanceFromCatching += 0.002f;
					this.reelRotation += 0.3926991f;
					this.fishShake.X = (float)Game1.random.Next(-10, 11) / 10f;
					this.fishShake.Y = (float)Game1.random.Next(-10, 11) / 10f;
					this.barShake = Vector2.Zero;
					Rumble.rumble(0.1f, 1000f);
					if (BobberBar.unReelSound != null)
					{
						BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
					}
					if (Game1.soundBank != null && (BobberBar.reelSound == null || BobberBar.reelSound.IsStopped || BobberBar.reelSound.IsStopping))
					{
						BobberBar.reelSound = Game1.soundBank.GetCue("fastReel");
					}
					if (BobberBar.reelSound != null && !BobberBar.reelSound.IsPlaying && !BobberBar.reelSound.IsStopping)
					{
						BobberBar.reelSound.Play();
					}
				}
				else if (!treasureInBar || this.treasureCaught || this.whichBobber != 693)
				{
					if (!this.fishShake.Equals(Vector2.Zero))
					{
						Game1.playSound("tinyWhip");
						this.perfect = false;
						Rumble.stopRumbling();
					}
					this.fishSizeReductionTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.fishSizeReductionTimer <= 0)
					{
						this.fishSize = Math.Max(this.minFishSize, this.fishSize - 1);
						this.fishSizeReductionTimer = 800;
					}
					if ((Game1.player.fishCaught != null && Game1.player.fishCaught.Count != 0) || Game1.currentMinigame != null)
					{
						this.distanceFromCatching -= ((this.whichBobber == 694) ? 0.002f : 0.003f);
					}
					float distanceAway = Math.Abs(this.bobberPosition - (this.bobberBarPos + (float)(this.bobberBarHeight / 2)));
					this.reelRotation -= 3.14159274f / Math.Max(10f, 200f - distanceAway);
					this.barShake.X = (float)Game1.random.Next(-10, 11) / 10f;
					this.barShake.Y = (float)Game1.random.Next(-10, 11) / 10f;
					this.fishShake = Vector2.Zero;
					if (BobberBar.reelSound != null)
					{
						BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
					}
					if (Game1.soundBank != null && (BobberBar.unReelSound == null || BobberBar.unReelSound.IsStopped))
					{
						BobberBar.unReelSound = Game1.soundBank.GetCue("slowReel");
						BobberBar.unReelSound.SetVariable("Pitch", 600f);
					}
					if (BobberBar.unReelSound != null && !BobberBar.unReelSound.IsPlaying && !BobberBar.unReelSound.IsStopping)
					{
						BobberBar.unReelSound.Play();
					}
				}
				this.distanceFromCatching = Math.Max(0f, Math.Min(1f, this.distanceFromCatching));
				if (Game1.player.CurrentTool != null)
				{
					Game1.player.CurrentTool.tickUpdate(time, Game1.player);
				}
				if (this.distanceFromCatching <= 0f)
				{
					this.fadeOut = true;
					this.everythingShakeTimer = 500f;
					Game1.playSound("fishEscape");
					if (BobberBar.unReelSound != null)
					{
						BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
					}
					if (BobberBar.reelSound != null)
					{
						BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
					}
				}
				else if (this.distanceFromCatching >= 1f)
				{
					this.everythingShakeTimer = 500f;
					Game1.playSound("jingle1");
					this.fadeOut = true;
					if (BobberBar.unReelSound != null)
					{
						BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
					}
					if (BobberBar.reelSound != null)
					{
						BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
					}
					if (this.perfect)
					{
						this.sparkleText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:BobberBar_Perfect", new object[0]), Color.Yellow, Color.White, false, 0.1, 1500, -1, 500);
						if (Game1.isFestival())
						{
							Game1.CurrentEvent.perfectFishing();
						}
					}
					else if (this.fishSize == this.maxFishSize)
					{
						this.fishSize--;
					}
				}
			}
			if (this.bobberPosition < 0f)
			{
				this.bobberPosition = 0f;
			}
			if (this.bobberPosition > 548f)
			{
				this.bobberPosition = 548f;
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool readyToClose()
		{
			return false;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00113680 File Offset: 0x00111880
		public override void emergencyShutDown()
		{
			base.emergencyShutDown();
			if (BobberBar.unReelSound != null)
			{
				BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
			}
			if (BobberBar.reelSound != null)
			{
				BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
			}
			this.fadeOut = true;
			this.everythingShakeTimer = 500f;
			this.distanceFromCatching = -1f;
			Game1.playSound("fishEscape");
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x001136DE File Offset: 0x001118DE
		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.menuButton.Contains(new InputButton(key)))
			{
				this.emergencyShutDown();
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00113700 File Offset: 0x00111900
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - (this.flipBubble ? 44 : 20) + 104), (float)(this.yPositionOnScreen - 16 + 314)) + this.everythingShake, new Rectangle?(new Rectangle(652, 1685, 52, 157)), Color.White * 0.6f * this.scale, 0f, new Vector2(26f, 78.5f) * this.scale, (float)Game1.pixelZoom * this.scale, this.flipBubble ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.001f);
			b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 70), (float)(this.yPositionOnScreen + 296)) + this.everythingShake, new Rectangle?(new Rectangle(644, 1999, 37, 150)), Color.White * this.scale, 0f, new Vector2(18.5f, 74f) * this.scale, (float)Game1.pixelZoom * this.scale, SpriteEffects.None, 0.01f);
			if (this.scale == 1f)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64), (float)(this.yPositionOnScreen + 12 + (int)this.bobberBarPos)) + this.barShake + this.everythingShake, new Rectangle?(new Rectangle(682, 2078, 9, 2)), this.bobberInBar ? Color.White : (Color.White * 0.25f * ((float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 100.0), 2) + 2f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64), (float)(this.yPositionOnScreen + 12 + (int)this.bobberBarPos + 8)) + this.barShake + this.everythingShake, new Rectangle?(new Rectangle(682, 2081, 9, 1)), this.bobberInBar ? Color.White : (Color.White * 0.25f * ((float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 100.0), 2) + 2f)), 0f, Vector2.Zero, new Vector2(4f, (float)(this.bobberBarHeight - 16)), SpriteEffects.None, 0.89f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64), (float)(this.yPositionOnScreen + 12 + (int)this.bobberBarPos + this.bobberBarHeight - 8)) + this.barShake + this.everythingShake, new Rectangle?(new Rectangle(682, 2085, 9, 2)), this.bobberInBar ? Color.White : (Color.White * 0.25f * ((float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 100.0), 2) + 2f)), 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.89f);
				b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 124, this.yPositionOnScreen + 4 + (int)(580f * (1f - this.distanceFromCatching)), 16, (int)(580f * this.distanceFromCatching)), Utility.getRedToGreenLerpColor(this.distanceFromCatching));
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 18), (float)(this.yPositionOnScreen + 514)) + this.everythingShake, new Rectangle?(new Rectangle(257, 1990, 5, 10)), Color.White, this.reelRotation, new Vector2(2f, 10f), 4f, SpriteEffects.None, 0.9f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64 + 18), (float)(this.yPositionOnScreen + 12 + 24) + this.treasurePosition) + this.treasureShake + this.everythingShake, new Rectangle?(new Rectangle(638, 1865, 20, 24)), Color.White, 0f, new Vector2(10f, 10f), 2f * this.treasureScale, SpriteEffects.None, 0.85f);
				if (this.treasureCatchLevel > 0f && !this.treasureCaught)
				{
					b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 64, this.yPositionOnScreen + 12 + (int)this.treasurePosition, 40, 8), Color.DimGray * 0.5f);
					b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 64, this.yPositionOnScreen + 12 + (int)this.treasurePosition, (int)(this.treasureCatchLevel * 40f), 8), Color.Orange);
				}
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + 64 + 18), (float)(this.yPositionOnScreen + 12 + 24) + this.bobberPosition) + this.fishShake + this.everythingShake, new Rectangle?(new Rectangle(614 + (this.bossFish ? 20 : 0), 1840, 20, 20)), Color.White, 0f, new Vector2(10f, 10f), 2f, SpriteEffects.None, 0.88f);
				if (this.sparkleText != null)
				{
					this.sparkleText.draw(b, new Vector2((float)(this.xPositionOnScreen - Game1.tileSize / 4), (float)(this.yPositionOnScreen - Game1.tileSize)));
				}
			}
			if (Game1.player.fishCaught != null && Game1.player.fishCaught.Count == 0)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen + (this.flipBubble ? (this.width + Game1.tileSize + Game1.pixelZoom * 2) : (-Game1.pixelZoom * 2 - 48 * Game1.pixelZoom))), (float)(this.yPositionOnScreen + Game1.tileSize * 3)), new Rectangle?(new Rectangle(644, 1330, 48, 69)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
			}
		}

		// Token: 0x04000E10 RID: 3600
		public const int timePerFishSizeReduction = 800;

		// Token: 0x04000E11 RID: 3601
		public const int bobberTrackHeight = 548;

		// Token: 0x04000E12 RID: 3602
		public const int bobberBarTrackHeight = 568;

		// Token: 0x04000E13 RID: 3603
		public const int xOffsetToBobberTrack = 64;

		// Token: 0x04000E14 RID: 3604
		public const int yOffsetToBobberTrack = 12;

		// Token: 0x04000E15 RID: 3605
		public const int mixed = 0;

		// Token: 0x04000E16 RID: 3606
		public const int dart = 1;

		// Token: 0x04000E17 RID: 3607
		public const int smooth = 2;

		// Token: 0x04000E18 RID: 3608
		public const int sink = 3;

		// Token: 0x04000E19 RID: 3609
		public const int floater = 4;

		// Token: 0x04000E1A RID: 3610
		private float difficulty;

		// Token: 0x04000E1B RID: 3611
		private int motionType;

		// Token: 0x04000E1C RID: 3612
		private int whichFish;

		// Token: 0x04000E1D RID: 3613
		private float bobberPosition = 548f;

		// Token: 0x04000E1E RID: 3614
		private float bobberSpeed;

		// Token: 0x04000E1F RID: 3615
		private float bobberAcceleration;

		// Token: 0x04000E20 RID: 3616
		private float bobberTargetPosition;

		// Token: 0x04000E21 RID: 3617
		private float scale;

		// Token: 0x04000E22 RID: 3618
		private float everythingShakeTimer;

		// Token: 0x04000E23 RID: 3619
		private float floaterSinkerAcceleration;

		// Token: 0x04000E24 RID: 3620
		private float treasurePosition;

		// Token: 0x04000E25 RID: 3621
		private float treasureCatchLevel;

		// Token: 0x04000E26 RID: 3622
		private float treasureAppearTimer;

		// Token: 0x04000E27 RID: 3623
		private float treasureScale;

		// Token: 0x04000E28 RID: 3624
		private bool bobberInBar;

		// Token: 0x04000E29 RID: 3625
		private bool buttonPressed;

		// Token: 0x04000E2A RID: 3626
		private bool flipBubble;

		// Token: 0x04000E2B RID: 3627
		private bool fadeIn;

		// Token: 0x04000E2C RID: 3628
		private bool fadeOut;

		// Token: 0x04000E2D RID: 3629
		private bool treasure;

		// Token: 0x04000E2E RID: 3630
		private bool treasureCaught;

		// Token: 0x04000E2F RID: 3631
		private bool perfect;

		// Token: 0x04000E30 RID: 3632
		private bool bossFish;

		// Token: 0x04000E31 RID: 3633
		private int bobberBarHeight;

		// Token: 0x04000E32 RID: 3634
		private int fishSize;

		// Token: 0x04000E33 RID: 3635
		private int fishQuality;

		// Token: 0x04000E34 RID: 3636
		private int minFishSize;

		// Token: 0x04000E35 RID: 3637
		private int maxFishSize;

		// Token: 0x04000E36 RID: 3638
		private int fishSizeReductionTimer;

		// Token: 0x04000E37 RID: 3639
		private int whichBobber;

		// Token: 0x04000E38 RID: 3640
		private Vector2 barShake;

		// Token: 0x04000E39 RID: 3641
		private Vector2 fishShake;

		// Token: 0x04000E3A RID: 3642
		private Vector2 everythingShake;

		// Token: 0x04000E3B RID: 3643
		private Vector2 treasureShake;

		// Token: 0x04000E3C RID: 3644
		private float reelRotation;

		// Token: 0x04000E3D RID: 3645
		private SparklingText sparkleText;

		// Token: 0x04000E3E RID: 3646
		private float bobberBarPos;

		// Token: 0x04000E3F RID: 3647
		private float bobberBarSpeed;

		// Token: 0x04000E40 RID: 3648
		private float bobberBarAcceleration;

		// Token: 0x04000E41 RID: 3649
		private float distanceFromCatching = 0.3f;

		// Token: 0x04000E42 RID: 3650
		public static Cue reelSound;

		// Token: 0x04000E43 RID: 3651
		public static Cue unReelSound;
	}
}
