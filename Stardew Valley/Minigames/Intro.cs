using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace StardewValley.Minigames
{
	// Token: 0x020000CD RID: 205
	public class Intro : IMinigame
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x00108A98 File Offset: 0x00106C98
		public Intro()
		{
			new List<int>();
			new List<int>();
			new List<int>();
			this.texture = Game1.content.Load<Texture2D>("Minigames\\Intro");
			this.roadsideTexture = Game1.content.Load<Texture2D>("Maps\\spring_outdoorsTileSheet");
			this.cloudTexture = Game1.content.Load<Texture2D>("Minigames\\Clouds");
			this.transformMatrix = Matrix.CreateScale(3f);
			this.skyColor = new Color(64, 136, 248);
			this.roadColor = new Color(130, 130, 130);
			this.createBeginningOfLevel();
			Game1.player.FarmerSprite.SourceRect = new Rectangle(0, 0, 16, 32);
			this.bigCloudPosition = (float)this.cloudTexture.Width;
			if (Game1.soundBank != null)
			{
				Intro.roadNoise = Game1.soundBank.GetCue("roadnoise");
			}
			this.currentState = 1;
			Game1.changeMusicTrack("spring_day_ambient");
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00108C48 File Offset: 0x00106E48
		public Intro(int startingGameMode)
		{
			this.texture = Game1.content.Load<Texture2D>("Minigames\\Intro");
			this.roadsideTexture = Game1.content.Load<Texture2D>("Maps\\spring_outdoorsTileSheet");
			this.cloudTexture = Game1.content.Load<Texture2D>("Minigames\\Clouds");
			this.transformMatrix = Matrix.CreateScale(3f);
			this.skyColor = new Color(102, 181, 255);
			this.roadColor = new Color(130, 130, 130);
			this.createBeginningOfLevel();
			this.currentState = startingGameMode;
			if (this.currentState == 4)
			{
				this.fadeAlpha = 1f;
			}
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00108DA8 File Offset: 0x00106FA8
		public void createBeginningOfLevel()
		{
			this.backClouds.Clear();
			this.road.Clear();
			this.sky.Clear();
			this.roadsideObjects.Clear();
			this.roadsideFences.Clear();
			for (int i = 0; i < this.screenWidth / this.tileSize + 6; i++)
			{
				this.road.Add((Game1.random.NextDouble() < 0.7) ? 0 : Game1.random.Next(0, 3));
				this.roadsideObjects.Add(-1);
				this.roadsideFences.Add(-1);
			}
			for (int j = 0; j < this.screenWidth / 112 + 2; j++)
			{
				this.sky.Add((Game1.random.NextDouble() < 0.5) ? 1 : Game1.random.Next(2));
			}
			for (int k = 0; k < this.screenWidth / 170 + 2; k++)
			{
				this.backClouds.Add(new Point(Game1.random.Next(3), Game1.random.Next(this.screenHeight / 2)));
			}
			this.roadsideObjects.Add(-1);
			this.roadsideObjects.Add(-1);
			this.roadsideObjects.Add(-1);
			this.busPosition = new Vector2((float)(this.tileSize * 8), 240f);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00108F14 File Offset: 0x00107114
		public void updateRoad(GameTime time)
		{
			this.roadPosition += (float)time.ElapsedGameTime.Milliseconds * this.speed;
			if (this.roadPosition >= (float)(this.tileSize * 3))
			{
				this.roadPosition -= (float)(this.tileSize * 3);
				for (int i = 0; i < 3; i++)
				{
					this.road.Add((Game1.random.NextDouble() < 0.7) ? 0 : Game1.random.Next(0, 3));
				}
				this.road.RemoveRange(0, 3);
				if (this.fenceBuildStatus != -1 || (this.cameraCenteredOnBus && Game1.random.NextDouble() < 0.1))
				{
					for (int j = 0; j < 3; j++)
					{
						switch (this.fenceBuildStatus)
						{
						case -1:
							this.fenceBuildStatus = 0;
							this.roadsideFences.Add(0);
							break;
						case 0:
							this.fenceBuildStatus = 1;
							this.roadsideFences.Add(Game1.random.Next(3));
							break;
						case 1:
							if (Game1.random.NextDouble() < 0.1)
							{
								this.roadsideFences.Add(2);
								this.fenceBuildStatus = 2;
							}
							else
							{
								this.fenceBuildStatus = 1;
								this.roadsideFences.Add((Game1.random.NextDouble() < 0.1) ? 3 : Game1.random.Next(3));
							}
							break;
						case 2:
							this.fenceBuildStatus = -1;
							for (int k = j; k < 3; k++)
							{
								this.roadsideFences.Add(-1);
							}
							break;
						}
						if (this.fenceBuildStatus == -1)
						{
							break;
						}
					}
				}
				else
				{
					this.roadsideFences.Add(-1);
					this.roadsideFences.Add(-1);
					this.roadsideFences.Add(-1);
				}
				this.roadsideFences.RemoveRange(0, 3);
				if (this.cameraCenteredOnBus && !this.addedSign && Game1.random.NextDouble() < 0.25)
				{
					for (int l = 0; l < 3; l++)
					{
						if (l == 0 && Game1.random.NextDouble() < 0.3)
						{
							this.roadsideObjects.Add(Game1.random.Next(2));
							for (int m = l; m < 3; m++)
							{
								this.roadsideObjects.Add(-1);
							}
							break;
						}
						if (Game1.random.NextDouble() < 0.5)
						{
							this.roadsideObjects.Add(Game1.random.Next(2, 5));
						}
						else
						{
							this.roadsideObjects.Add(-1);
						}
					}
				}
				else
				{
					this.roadsideObjects.Add(-1);
					this.roadsideObjects.Add(-1);
					this.roadsideObjects.Add(-1);
				}
				this.roadsideObjects.RemoveRange(0, 3);
			}
			this.skyPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 12f);
			if (this.skyPosition >= 112f)
			{
				this.skyPosition -= 112f;
				this.sky.Add(Game1.random.Next(2));
				this.sky.RemoveAt(0);
			}
			this.valleyPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 6f);
			if (this.carPosition.Equals(Vector2.Zero) && Game1.random.NextDouble() < 0.002 && !this.addedSign)
			{
				this.carPosition = new Vector2((float)this.screenWidth, 200f);
				this.carColor = new Color(Game1.random.Next(100, 255), Game1.random.Next(100, 255), Game1.random.Next(100, 255));
				return;
			}
			if (!this.carPosition.Equals(Vector2.Zero))
			{
				this.carPosition.X = this.carPosition.X - 0.1f * (float)time.ElapsedGameTime.Milliseconds * ((float)this.carColor.G / 60f);
				if (this.carPosition.X < -200f)
				{
					this.carPosition = Vector2.Zero;
				}
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00109390 File Offset: 0x00107590
		public void updateUpperClouds(GameTime time)
		{
			this.bigCloudPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 24f);
			if (this.bigCloudPosition >= (float)(this.cloudTexture.Width * 3))
			{
				this.bigCloudPosition -= (float)(this.cloudTexture.Width * 3);
			}
			this.backCloudPosition += (float)time.ElapsedGameTime.Milliseconds * (this.speed / 36f);
			if (this.backCloudPosition > 170f)
			{
				this.backCloudPosition %= 170f;
				this.backClouds.Add(new Point(Game1.random.Next(3), Game1.random.Next(this.screenHeight / 2)));
				this.backClouds.RemoveAt(0);
			}
			if (Game1.random.NextDouble() < 0.0002)
			{
				this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
				if (Game1.random.NextDouble() < 0.1)
				{
					Vector2 position = new Vector2((float)Game1.random.Next(this.screenWidth / 3, this.screenWidth), (float)this.screenHeight);
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(position.X + (float)Game1.random.Next(-16, 16), position.Y + (float)Game1.random.Next(8));
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(position.X + (float)Game1.random.Next(-16, 16), position.Y + (float)Game1.random.Next(8));
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(position.X + (float)Game1.random.Next(-16, 16), position.Y + (float)Game1.random.Next(8));
					this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
					this.balloons.Last<Intro.Balloon>().position = new Vector2(position.X + (float)Game1.random.Next(-16, 16), position.Y + (float)Game1.random.Next(8));
				}
			}
			for (int i = this.balloons.Count - 1; i >= 0; i--)
			{
				this.balloons[i].update(this.speed, time);
				if (this.balloons[i].position.X < (float)(-(float)this.tileSize) || this.balloons[i].position.Y < (float)(-(float)this.tileSize))
				{
					this.balloons.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x001096D0 File Offset: 0x001078D0
		public bool tick(GameTime time)
		{
			if (this.hasQuit)
			{
				return true;
			}
			if (this.quit && !this.hasQuit)
			{
				Game1.warpFarmer("BusStop", 12, 11, false);
				if (Intro.roadNoise != null)
				{
					Intro.roadNoise.Stop(AudioStopOptions.Immediate);
				}
				Game1.exitActiveMenu();
				this.hasQuit = true;
				return true;
			}
			switch (this.currentState)
			{
			case 0:
				this.updateUpperClouds(time);
				break;
			case 1:
				this.globalYPanDY = Math.Min(4f, this.globalYPanDY + (float)time.ElapsedGameTime.Milliseconds * (this.speed / 140f));
				this.globalYPan -= this.globalYPanDY;
				this.updateUpperClouds(time);
				if (this.globalYPan < -1f)
				{
					this.globalYPan = (float)(this.screenHeight * 3);
					this.currentState = 2;
					this.transformMatrix = Matrix.CreateScale(3f);
					this.transformMatrix.Translation = new Vector3(0f, this.globalYPan, 0f);
					if (Game1.soundBank != null && Intro.roadNoise != null)
					{
						Intro.roadNoise.SetVariable("Volume", 0f);
						Intro.roadNoise.Play();
					}
					Game1.loadForNewGame(false);
				}
				break;
			case 2:
				this.globalYPanDY = Math.Max(0.5f, this.globalYPan / 100f);
				if (Game1.soundBank != null && Intro.roadNoise != null)
				{
					Intro.roadNoise.SetVariable("Volume", Math.Max(90f, 1f / (this.globalYPan / (float)(this.screenHeight * 3)) * 90f));
				}
				this.globalYPan -= this.globalYPanDY;
				this.transformMatrix = Matrix.CreateScale(3f);
				this.transformMatrix.Translation = new Vector3(0f, this.globalYPan, 0f);
				this.updateRoad(time);
				if (this.globalYPan <= (float)(0 - Math.Max(0, 900 - Game1.graphics.GraphicsDevice.Viewport.Height)))
				{
					this.globalYPan = (float)(-(float)Math.Max(0, 900 - Game1.graphics.GraphicsDevice.Viewport.Height));
					this.currentState = 3;
					if (Game1.soundBank != null && Intro.roadNoise != null)
					{
						Intro.roadNoise.SetVariable("Volume", 100f);
					}
				}
				break;
			case 3:
				this.updateRoad(time);
				this.drivingTimer += (float)time.ElapsedGameTime.Milliseconds;
				if (this.drivingTimer > 5700f)
				{
					this.drivingTimer = 0f;
					this.currentState = 4;
				}
				break;
			case 4:
				this.updateRoad(time);
				this.drivingTimer += (float)time.ElapsedGameTime.Milliseconds;
				if (this.drivingTimer > 2000f)
				{
					this.busPosition.X = this.busPosition.X + (float)time.ElapsedGameTime.Milliseconds / 8f;
					if (Game1.soundBank != null && Intro.roadNoise != null)
					{
						Intro.roadNoise.SetVariable("Volume", Math.Max(0f, Intro.roadNoise.GetVariable("Volume") - 1f));
					}
					this.speed = Math.Max(0f, this.speed - (float)time.ElapsedGameTime.Milliseconds / 70000f);
					if (!this.addedSign)
					{
						this.addedSign = true;
						this.roadsideObjects.RemoveAt(this.roadsideObjects.Count - 1);
						this.roadsideObjects.Add(5);
						Game1.playSound("busDriveOff");
					}
					if (this.speed <= 0f && this.birdPosition.Equals(Vector2.Zero))
					{
						int position = 0;
						for (int i = 0; i < this.roadsideObjects.Count; i++)
						{
							if (this.roadsideObjects[i] == 5)
							{
								position = i;
								break;
							}
						}
						this.birdPosition = new Vector2((float)(position * 16) - this.roadPosition - 32f + 16f, -16f);
						Game1.playSound("SpringBirds");
						this.fadeAlpha = 0f;
					}
					if (!this.birdPosition.Equals(Vector2.Zero) && this.birdPosition.Y < 116f)
					{
						float dy = Math.Max(0.5f, (116f - this.birdPosition.Y) / 116f * 2f);
						this.birdPosition.Y = this.birdPosition.Y + dy;
						this.birdPosition.X = this.birdPosition.X + (float)Math.Sin((double)this.birdXTimer / 50.26548245743669) * dy / 2f;
						this.birdTimer += time.ElapsedGameTime.Milliseconds;
						this.birdXTimer += time.ElapsedGameTime.Milliseconds;
						if (this.birdTimer >= 100)
						{
							this.birdFrame = (this.birdFrame + 1) % 4;
							this.birdTimer = 0;
						}
					}
					else if (!this.birdPosition.Equals(Vector2.Zero))
					{
						this.birdFrame = ((this.birdTimer > 1500) ? 5 : 4);
						this.birdTimer += time.ElapsedGameTime.Milliseconds;
						if (this.birdTimer > 2400 || (this.birdTimer > 1800 && Game1.random.NextDouble() < 0.006))
						{
							this.birdTimer = 0;
							if (Game1.random.NextDouble() < 0.5)
							{
								Game1.playSound("SpringBirds");
								this.birdPosition.Y = this.birdPosition.Y - 4f;
							}
						}
					}
					if (this.drivingTimer > 14000f)
					{
						this.fadeAlpha += (float)time.ElapsedGameTime.Milliseconds * 0.1f / 128f;
						if (this.fadeAlpha >= 1f)
						{
							Game1.warpFarmer("BusStop", 12, 11, false);
							if (Intro.roadNoise != null)
							{
								Intro.roadNoise.Stop(AudioStopOptions.Immediate);
							}
							Game1.exitActiveMenu();
							return true;
						}
					}
				}
				break;
			}
			return false;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00109D4E File Offset: 0x00107F4E
		public void doneCreatingCharacter()
		{
			this.characterCreateMenu = null;
			this.currentState = 1;
			Game1.changeMusicTrack("spring_day_ambient");
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00109D68 File Offset: 0x00107F68
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.receiveLeftClick(x, y, true);
			}
			for (int i = this.balloons.Count - 1; i >= 0; i--)
			{
				if (new Rectangle((int)this.balloons[i].position.X * 4 + 16, (int)this.balloons[i].position.Y * 4 + 16, 32, 32).Contains(x, y))
				{
					this.balloons.RemoveAt(i);
					Game1.playSound("coin");
				}
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00109E06 File Offset: 0x00108006
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.receiveRightClick(x, y, true);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00109E1E File Offset: 0x0010801E
		public void releaseLeftClick(int x, int y)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.releaseLeftClick(x, y);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00109E35 File Offset: 0x00108035
		public void leftClickHeld(int x, int y)
		{
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.leftClickHeld(x, y);
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00109E4C File Offset: 0x0010804C
		public void receiveKeyPress(Keys k)
		{
			if (k == Keys.Escape && this.currentState != 1)
			{
				if (!this.quit)
				{
					Game1.playSound("bigDeSelect");
				}
				this.quit = true;
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00109E78 File Offset: 0x00108078
		public void draw(SpriteBatch b)
		{
			switch (this.currentState)
			{
			case 0:
				break;
			case 1:
				b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
				b.GraphicsDevice.Clear(this.skyColor);
				SpriteText.drawString(b, "Loading...", 64, Game1.graphics.GraphicsDevice.Viewport.Height - 64, 999, -1, 999, 1f, 1f, false, 0, "", -1);
				b.End();
				return;
			case 2:
			case 3:
			case 4:
				this.drawRoadArea(b);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00109F1D File Offset: 0x0010811D
		public void drawChoosingCharacterArea(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.GraphicsDevice.Clear(this.skyColor);
			if (this.characterCreateMenu != null)
			{
				this.characterCreateMenu.draw(b);
			}
			b.End();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00109F60 File Offset: 0x00108160
		public void drawRoadArea(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, this.transformMatrix);
			b.GraphicsDevice.Clear(this.roadColor);
			b.Draw(Game1.staminaRect, new Rectangle(0, -this.screenHeight * 2, this.screenWidth, this.screenHeight * 3), this.skyColor);
			b.Draw(Game1.staminaRect, new Rectangle(0, this.screenHeight / 2, this.screenWidth, this.screenHeight * 4), this.roadColor);
			for (int i = 0; i < this.screenWidth / 112 + 2; i++)
			{
				if (this.sky[i] == 0)
				{
					b.Draw(this.texture, new Vector2(-this.skyPosition + (float)(i * 112), -16f), new Rectangle?(new Rectangle(128, 0, 112, 96)), Color.White);
				}
				else
				{
					b.Draw(this.texture, new Rectangle((int)(-(int)this.skyPosition) - 1 + i * 112, -16, 114, 96), new Rectangle?(new Rectangle(128, 0, 1, 96)), Color.White);
				}
			}
			for (int j = 0; j < 8; j++)
			{
				b.Draw(Game1.mouseCursors, new Vector2(-10f + -this.valleyPosition / 2f + (float)(j * 639), 70f), new Rectangle?(new Rectangle(0, 886, 639, 148)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.08f);
				b.Draw(Game1.mouseCursors, new Vector2(-this.valleyPosition + (float)(j * 639), 80f), new Rectangle?(new Rectangle(0, 737, 639, 120)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.08f);
			}
			for (int k = 0; k < this.road.Count; k++)
			{
				if (k % 3 == 0)
				{
					b.Draw(this.texture, new Vector2((float)(k * 16) - this.roadPosition, 160f), new Rectangle?(new Rectangle(0, 176, 48, 48)), Color.White);
					b.Draw(this.texture, new Vector2((float)(k * 16 + this.tileSize) - this.roadPosition, 272f), new Rectangle?(new Rectangle(0, 64, 16, 16)), Color.White);
				}
				b.Draw(this.texture, new Vector2((float)(k * 16) - this.roadPosition, 208f), new Rectangle?(new Rectangle(this.road[k] * 16, 240, 16, 16)), Color.White);
			}
			for (int l = 0; l < this.roadsideObjects.Count; l++)
			{
				switch (this.roadsideObjects[l])
				{
				case 0:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 96f), new Rectangle?(new Rectangle(48, 0, 48, 96)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 1:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 96f), new Rectangle?(new Rectangle(0, 0, 48, 64)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 16f, 160f), new Rectangle?(new Rectangle(16, 64, 16, 32)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 2:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 176f), new Rectangle?(new Rectangle(112, 144, 16, 16)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 3:
					b.Draw(this.roadsideTexture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 176f), new Rectangle?(new Rectangle(112, 160, 16, 16)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				case 5:
					b.Draw(this.texture, new Vector2((float)(l * 16) - this.roadPosition - 32f, 128f), new Rectangle?(new Rectangle(48, 176, 64, 64)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					break;
				}
			}
			for (int m = 0; m < this.roadsideFences.Count; m++)
			{
				if (this.roadsideFences[m] != -1)
				{
					if (this.roadsideFences[m] == 3)
					{
						b.Draw(this.roadsideTexture, new Vector2((float)(m * 16) - this.roadPosition, 176f), new Rectangle?(new Rectangle(144, 256, 16, 32)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					else
					{
						b.Draw(this.roadsideTexture, new Vector2((float)(m * 16) - this.roadPosition, 176f), new Rectangle?(new Rectangle(128 + this.roadsideFences[m] * 16, 224, 16, 32)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}
			if (!this.carPosition.Equals(Vector2.Zero))
			{
				b.Draw(this.texture, this.carPosition, new Rectangle?(new Rectangle(160, 112, 80, 64)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				b.Draw(this.texture, this.carPosition, new Rectangle?(new Rectangle(160, 176, 80, 64)), this.carColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			b.Draw(this.texture, this.busPosition, new Rectangle?(new Rectangle(0, 0, 128, 64)), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
			b.Draw(this.texture, this.busPosition + new Vector2(23.5f, 56.5f) * 1.5f, new Rectangle?(new Rectangle(21, 54, 5, 5)), Color.White, (float)((double)(this.roadPosition / 3f / 16f) * 3.1415926535897931 * 2.0), new Vector2(2.5f, 2.5f), 1.5f, SpriteEffects.None, 0f);
			b.Draw(this.texture, this.busPosition + new Vector2(87.5f, 56.5f) * 1.5f, new Rectangle?(new Rectangle(21, 54, 5, 5)), Color.White, (float)((double)((this.roadPosition + 4f) / 3f / 16f) * 3.1415926535897931 * 2.0), new Vector2(2.5f, 2.5f), 1.5f, SpriteEffects.None, 0f);
			if (!this.birdPosition.Equals(Vector2.Zero))
			{
				b.Draw(this.texture, this.birdPosition, new Rectangle?(new Rectangle(16 + this.birdFrame * 16, 64, 16, 16)), Color.White);
			}
			if (this.fadeAlpha > 0f)
			{
				b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, this.screenWidth + 2, this.screenHeight * 2), Color.Black * this.fadeAlpha);
			}
			b.End();
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0010A820 File Offset: 0x00108A20
		public void changeScreenSize()
		{
			this.screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width / 3;
			this.screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height / 3;
			this.createBeginningOfLevel();
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00002834 File Offset: 0x00000A34
		public void unload()
		{
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000A8788 File Offset: 0x000A6988
		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D3A RID: 3386
		public const int pixelScale = 3;

		// Token: 0x04000D3B RID: 3387
		public const int valleyLoopWidth = 160;

		// Token: 0x04000D3C RID: 3388
		public const int skyLoopWidth = 112;

		// Token: 0x04000D3D RID: 3389
		public const int cloudLoopWidth = 170;

		// Token: 0x04000D3E RID: 3390
		public const int tilesBeyondViewportToSimulate = 6;

		// Token: 0x04000D3F RID: 3391
		public const int leftFence = 0;

		// Token: 0x04000D40 RID: 3392
		public const int centerFence = 1;

		// Token: 0x04000D41 RID: 3393
		public const int rightFence = 2;

		// Token: 0x04000D42 RID: 3394
		public const int busYRest = 240;

		// Token: 0x04000D43 RID: 3395
		public const int choosingCharacterState = 0;

		// Token: 0x04000D44 RID: 3396
		public const int panningDownFromCloudsState = 1;

		// Token: 0x04000D45 RID: 3397
		public const int panningDownToRoadState = 2;

		// Token: 0x04000D46 RID: 3398
		public const int drivingState = 3;

		// Token: 0x04000D47 RID: 3399
		public const int stardewInViewState = 4;

		// Token: 0x04000D48 RID: 3400
		public float speed = 0.1f;

		// Token: 0x04000D49 RID: 3401
		private float valleyPosition;

		// Token: 0x04000D4A RID: 3402
		private float skyPosition;

		// Token: 0x04000D4B RID: 3403
		private float roadPosition;

		// Token: 0x04000D4C RID: 3404
		private float bigCloudPosition;

		// Token: 0x04000D4D RID: 3405
		private float frontCloudPosition;

		// Token: 0x04000D4E RID: 3406
		private float backCloudPosition;

		// Token: 0x04000D4F RID: 3407
		private float globalYPan;

		// Token: 0x04000D50 RID: 3408
		private float globalYPanDY;

		// Token: 0x04000D51 RID: 3409
		private float drivingTimer;

		// Token: 0x04000D52 RID: 3410
		private float fadeAlpha;

		// Token: 0x04000D53 RID: 3411
		private int screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width / 3;

		// Token: 0x04000D54 RID: 3412
		private int screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height / 3;

		// Token: 0x04000D55 RID: 3413
		private int tileSize = 16;

		// Token: 0x04000D56 RID: 3414
		private Matrix transformMatrix;

		// Token: 0x04000D57 RID: 3415
		private Texture2D texture;

		// Token: 0x04000D58 RID: 3416
		private Texture2D roadsideTexture;

		// Token: 0x04000D59 RID: 3417
		private Texture2D cloudTexture;

		// Token: 0x04000D5A RID: 3418
		private List<Point> backClouds = new List<Point>();

		// Token: 0x04000D5B RID: 3419
		private List<int> road = new List<int>();

		// Token: 0x04000D5C RID: 3420
		private List<int> sky = new List<int>();

		// Token: 0x04000D5D RID: 3421
		private List<int> roadsideObjects = new List<int>();

		// Token: 0x04000D5E RID: 3422
		private List<int> roadsideFences = new List<int>();

		// Token: 0x04000D5F RID: 3423
		private Color skyColor;

		// Token: 0x04000D60 RID: 3424
		private Color roadColor;

		// Token: 0x04000D61 RID: 3425
		private Color carColor;

		// Token: 0x04000D62 RID: 3426
		private bool cameraCenteredOnBus = true;

		// Token: 0x04000D63 RID: 3427
		private bool addedSign;

		// Token: 0x04000D64 RID: 3428
		private Vector2 busPosition;

		// Token: 0x04000D65 RID: 3429
		private Vector2 carPosition;

		// Token: 0x04000D66 RID: 3430
		private Vector2 birdPosition = Vector2.Zero;

		// Token: 0x04000D67 RID: 3431
		private CharacterCustomization characterCreateMenu;

		// Token: 0x04000D68 RID: 3432
		private List<Intro.Balloon> balloons = new List<Intro.Balloon>();

		// Token: 0x04000D69 RID: 3433
		private int birdFrame;

		// Token: 0x04000D6A RID: 3434
		private int birdTimer;

		// Token: 0x04000D6B RID: 3435
		private int birdXTimer;

		// Token: 0x04000D6C RID: 3436
		public static Cue roadNoise;

		// Token: 0x04000D6D RID: 3437
		private int fenceBuildStatus = -1;

		// Token: 0x04000D6E RID: 3438
		private int currentState;

		// Token: 0x04000D6F RID: 3439
		private bool quit;

		// Token: 0x04000D70 RID: 3440
		private bool hasQuit;

		// Token: 0x0200017E RID: 382
		public class Balloon
		{
			// Token: 0x060013C0 RID: 5056 RVA: 0x0018A28C File Offset: 0x0018848C
			public Balloon(int screenWidth, int screenHeight)
			{
				int g = Game1.random.Next(255);
				int b = 255 - g;
				int r = (Game1.random.NextDouble() < 0.5) ? 255 : 0;
				this.position = new Vector2((float)Game1.random.Next(screenWidth / 5, screenWidth), (float)screenHeight);
				this.color = new Color(r, g, b);
			}

			// Token: 0x060013C1 RID: 5057 RVA: 0x0018A300 File Offset: 0x00188500
			public void update(float speed, GameTime time)
			{
				this.position.Y = this.position.Y - speed * (float)time.ElapsedGameTime.Milliseconds / 16f;
				this.position.X = this.position.X - speed * (float)time.ElapsedGameTime.Milliseconds / 32f;
			}

			// Token: 0x040014C5 RID: 5317
			public Vector2 position;

			// Token: 0x040014C6 RID: 5318
			public Color color;
		}
	}
}
