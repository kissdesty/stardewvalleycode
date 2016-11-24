using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace StardewValley.Locations
{
	// Token: 0x02000134 RID: 308
	public class Summit : GameLocation
	{
		// Token: 0x060011A8 RID: 4520 RVA: 0x00151B90 File Offset: 0x0014FD90
		public Summit()
		{
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00151B98 File Offset: 0x0014FD98
		public Summit(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00002834 File Offset: 0x00000A34
		public override void checkForMusic(GameTime time)
		{
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0016A3E8 File Offset: 0x001685E8
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.temporarySprites.Count == 0 && Game1.random.NextDouble() < ((Game1.timeOfDay >= 1800) ? ((Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20) ? 1.0 : 0.001) : 0.0006))
			{
				Rectangle sourceRect = Rectangle.Empty;
				Vector2 startingPosition = new Vector2((float)Game1.viewport.Width, (float)Game1.random.Next(0, 200));
				float speed = -4f;
				int loops = 100;
				float animationSpeed = 100f;
				if (Game1.timeOfDay < 1800)
				{
					if (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("fall"))
					{
						sourceRect = new Rectangle(640, 736, 16, 16);
						int rows = Game1.random.Next(1, 4);
						speed = -1f;
						for (int i = 0; i < rows; i++)
						{
							TemporaryAnimatedSprite bird = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(80, 121), 4, 100, startingPosition + new Vector2((float)((i + 1) * Game1.random.Next(15, 18)), (float)((i + 1) * -20)), false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
							bird.motion = new Vector2(-1f, 0f);
							this.temporarySprites.Add(bird);
							bird = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(80, 121), 4, 100, startingPosition + new Vector2((float)((i + 1) * Game1.random.Next(15, 18)), (float)((i + 1) * 20)), false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
							bird.motion = new Vector2(-1f, 0f);
							this.temporarySprites.Add(bird);
						}
					}
					else if (Game1.currentSeason.Equals("summer"))
					{
						sourceRect = new Rectangle(640, 752 + ((Game1.random.NextDouble() < 0.5) ? 16 : 0), 16, 16);
						speed = -0.5f;
						animationSpeed = 150f;
					}
				}
				else if (Game1.timeOfDay >= 1900)
				{
					sourceRect = new Rectangle(640, 816, 16, 16);
					speed = -2f;
					loops = 0;
					startingPosition.X -= (float)Game1.random.Next(Game1.tileSize, Game1.viewport.Width);
					if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20)
					{
						int numExtra = Game1.random.Next(3);
						for (int j = 0; j < numExtra; j++)
						{
							TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float)Game1.random.Next(80, 121), Game1.currentSeason.Equals("winter") ? 2 : 4, loops, startingPosition, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
							t.motion = new Vector2(speed, 0f);
							this.temporarySprites.Add(t);
							startingPosition.X -= (float)Game1.random.Next(Game1.tileSize, Game1.viewport.Width);
							startingPosition.Y = (float)Game1.random.Next(0, 200);
						}
					}
					else if (Game1.currentSeason.Equals("winter") && Game1.timeOfDay >= 1700 && Game1.random.NextDouble() < 0.1)
					{
						sourceRect = new Rectangle(640, 800, 32, 16);
						loops = 1000;
						startingPosition.X = (float)Game1.viewport.Width;
					}
					else if (Game1.currentSeason.Equals("winter"))
					{
						sourceRect = Rectangle.Empty;
					}
				}
				if (Game1.timeOfDay >= 2200 && !Game1.currentSeason.Equals("winter") && Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20 && Game1.random.NextDouble() < 0.05)
				{
					sourceRect = new Rectangle(640, 784, 16, 16);
					loops = 100;
					startingPosition.X = (float)Game1.viewport.Width;
					speed = -3f;
				}
				if (!sourceRect.Equals(Rectangle.Empty))
				{
					TemporaryAnimatedSprite t2 = new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, animationSpeed, Game1.currentSeason.Equals("winter") ? 2 : 4, loops, startingPosition, false, false, 0.01f, 0f, Color.White, 4f, 0f, 0f, 0f, true);
					t2.motion = new Vector2(speed, 0f);
					this.temporarySprites.Add(t2);
				}
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0016A93F File Offset: 0x00168B3F
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.background = null;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0016A94D File Offset: 0x00168B4D
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			Game1.background = new Background();
			this.temporarySprites.Clear();
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0016A96A File Offset: 0x00168B6A
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
		}
	}
}
