using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000AF RID: 175
	public class DustSpirit : Monster
	{
		// Token: 0x06000BAA RID: 2986 RVA: 0x000E48B9 File Offset: 0x000E2AB9
		public DustSpirit()
		{
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000E926C File Offset: 0x000E746C
		public DustSpirit(Vector2 position) : base("Dust Spirit", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.sprite.interval = 45f;
			this.scale = (float)Game1.random.Next(75, 101) / 100f;
			this.voice = (byte)Game1.random.Next(1, 24);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000E92CC File Offset: 0x000E74CC
		public DustSpirit(Vector2 position, bool chargingTowardFarmer) : base("Dust Spirit", position)
		{
			base.IsWalkingTowardPlayer = false;
			if (chargingTowardFarmer)
			{
				this.chargingFarmer = true;
				this.seenFarmer = true;
			}
			this.sprite.interval = 45f;
			this.scale = (float)Game1.random.Next(75, 101) / 100f;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x000E9328 File Offset: 0x000E7528
		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + this.yJumpOffset)), new Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), new Vector2(this.scale + (float)Math.Max(-0.1, (double)(this.yJumpOffset + 32) / 128.0), this.scale - Math.Max(-0.1f, (float)this.yJumpOffset / 256f)) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize + this.yJumpOffset)), new Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
				b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize * 5 / 4)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f + (float)this.yJumpOffset / (float)Game1.tileSize, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
			}
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x000E95B8 File Offset: 0x000E77B8
		public override void deathAnimation()
		{
			Game1.playSound("dustMeep");
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0));
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 150,
				scale = 0.5f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 300,
				scale = 0.5f
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				delayBeforeAnimationStart = 450,
				scale = 0.5f
			});
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000E97AC File Offset: 0x000E79AC
		public override void shedChunks(int number, float scale)
		{
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 16, 16, 16), 8, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, number, (int)base.getTileLocation().Y, Color.White, (this.health <= 0) ? ((float)Game1.pixelZoom) : ((float)Game1.pixelZoom * 0.5f));
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00002834 File Offset: 0x00000A34
		public void offScreenBehavior(Character c, GameLocation l)
		{
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x000E9830 File Offset: 0x000E7A30
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			this.sprite.AnimateDown(time, 0, "");
			if (this.yJumpOffset == 0)
			{
				this.jumpWithoutSound(8f);
				this.yJumpVelocity = (float)Game1.random.Next(50, 70) / 10f;
				if (Game1.random.NextDouble() < 0.1 && (this.meep == null || !this.meep.IsPlaying) && Utility.isOnScreen(this.position, 64) && Game1.soundBank != null)
				{
					this.meep = Game1.soundBank.GetCue("dustMeep");
					this.meep.SetVariable("Pitch", (float)((int)(this.voice * 100) + Game1.random.Next(-100, 100)));
					this.meep.Play();
				}
				if (Game1.random.NextDouble() < 0.01)
				{
					Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 128, 64, 64), 40f, 4, 0, base.getStandingPosition(), false, false));
					foreach (Vector2 v in Utility.getAdjacentTileLocations(base.getTileLocation()))
					{
						if (Game1.currentLocation.objects.ContainsKey(v) && Game1.currentLocation.objects[v].Name.Contains("Stone"))
						{
							Game1.currentLocation.destroyObject(v, null);
						}
					}
					this.yJumpVelocity *= 2f;
				}
				if (!this.chargingFarmer)
				{
					this.xVelocity = (float)Game1.random.Next(-20, 21) / 5f;
				}
			}
			if (this.chargingFarmer)
			{
				this.slipperiness = 10;
				Vector2 v2 = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
				this.xVelocity += -v2.X / 150f + ((Game1.random.NextDouble() < 0.01) ? ((float)Game1.random.Next(-50, 50) / 10f) : 0f);
				if (Math.Abs(this.xVelocity) > 5f)
				{
					this.xVelocity = (float)(Math.Sign(this.xVelocity) * 5);
				}
				this.yVelocity += -v2.Y / 150f + ((Game1.random.NextDouble() < 0.01) ? ((float)Game1.random.Next(-50, 50) / 10f) : 0f);
				if (Math.Abs(this.yVelocity) > 5f)
				{
					this.yVelocity = (float)(Math.Sign(this.yVelocity) * 5);
				}
				if (Game1.random.NextDouble() < 0.0001)
				{
					this.controller = new PathFindController(this, Game1.currentLocation, new Point((int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y), Game1.random.Next(4), null, 300);
					this.chargingFarmer = false;
					return;
				}
			}
			else
			{
				if (!this.seenFarmer && Utility.doesPointHaveLineOfSightInMine(base.getStandingPosition() / (float)Game1.tileSize, Game1.player.getStandingPosition() / (float)Game1.tileSize, 8))
				{
					this.seenFarmer = true;
					return;
				}
				if (this.seenFarmer && this.controller == null && !this.runningAwayFromFarmer)
				{
					this.addedSpeed = 2;
					this.controller = new PathFindController(this, Game1.currentLocation, new PathFindController.isAtEnd(Utility.isOffScreenEndFunction), -1, false, new PathFindController.endBehavior(this.offScreenBehavior), 350, Point.Zero);
					this.runningAwayFromFarmer = true;
					return;
				}
				if (this.controller == null && this.runningAwayFromFarmer)
				{
					this.chargingFarmer = true;
				}
			}
		}

		// Token: 0x04000B80 RID: 2944
		private bool seenFarmer;

		// Token: 0x04000B81 RID: 2945
		private bool runningAwayFromFarmer;

		// Token: 0x04000B82 RID: 2946
		private bool chargingFarmer;

		// Token: 0x04000B83 RID: 2947
		public byte voice;

		// Token: 0x04000B84 RID: 2948
		private Cue meep;
	}
}
