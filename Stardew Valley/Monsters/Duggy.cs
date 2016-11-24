using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
	// Token: 0x020000BE RID: 190
	public class Duggy : Monster
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x000EF6E1 File Offset: 0x000ED8E1
		public Duggy()
		{
			this.hideShadow = true;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x000EF700 File Offset: 0x000ED900
		public Duggy(Vector2 position) : base("Duggy", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.isInvisible = true;
			this.damageToFarmer = 0;
			this.sprite.loop = false;
			this.sprite.CurrentFrame = 0;
			this.hideShadow = true;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x000EF75C File Offset: 0x000ED95C
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				this.health -= actualDamage;
				Game1.playSound("hitEnemy");
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000EF7C0 File Offset: 0x000ED9C0
		public override void deathAnimation()
		{
			Game1.playSound("monsterdead");
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(44, this.position, Color.DarkRed, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				holdLastFrame = true,
				alphaFade = 0.01f,
				interval = 70f
			}, Game1.currentLocation, 4, 64, 64);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x000EF828 File Offset: 0x000EDA28
		public override void update(GameTime time, GameLocation location)
		{
			if (this.invincibleCountdown > 0)
			{
				this.glowingColor = Color.Cyan;
				this.invincibleCountdown -= time.ElapsedGameTime.Milliseconds;
				if (this.invincibleCountdown <= 0)
				{
					base.stopGlowing();
				}
			}
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			this.behaviorAtGameTick(time);
			if (this.position.X < 0f || this.position.X > (float)(location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || this.position.Y < 0f || this.position.Y > (float)(location.map.GetLayer("Back").LayerHeight * Game1.tileSize))
			{
				location.characters.Remove(this);
			}
			base.updateGlow();
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x000EF914 File Offset: 0x000EDB14
		public override void draw(SpriteBatch b)
		{
			if (!this.isInvisible && Utility.isOnScreen(this.position, 2 * Game1.tileSize))
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yJumpOffset)), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (this.isGlowing)
				{
					b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2), (float)(this.GetBoundingBox().Height / 2 + this.yJumpOffset)), new Microsoft.Xna.Framework.Rectangle?(base.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f + 0.001f)));
				}
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000EFACC File Offset: 0x000EDCCC
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			this.isEmoting = false;
			Microsoft.Xna.Framework.Rectangle r = this.GetBoundingBox();
			if (this.sprite.currentFrame < 4)
			{
				r.Inflate(Game1.tileSize * 2, Game1.tileSize * 2);
				if (!this.isInvisible || r.Contains(Game1.player.getStandingX(), Game1.player.getStandingY()))
				{
					if (this.isInvisible)
					{
						if (Game1.currentLocation.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].Properties.ContainsKey("NPCBarrier") || (!Game1.currentLocation.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].TileIndexProperties.ContainsKey("Diggable") && Game1.currentLocation.map.GetLayer("Back").Tiles[(int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y].TileIndex != 0))
						{
							return;
						}
						this.position = new Vector2(Game1.player.position.X, Game1.player.position.Y + (float)Game1.player.sprite.spriteHeight - (float)this.sprite.spriteHeight);
						Game1.playSound("Duggy");
						this.sprite.interval = 100f;
						this.position = Game1.player.getTileLocation() * (float)Game1.tileSize;
					}
					this.isInvisible = false;
					this.sprite.AnimateDown(time, 0, "");
				}
			}
			if (this.sprite.currentFrame >= 4 && this.sprite.CurrentFrame < 8)
			{
				if (!this.hasDugForTreasure)
				{
					base.getTileLocation();
				}
				r.Inflate(-Game1.tileSize * 2, -Game1.tileSize * 2);
				Game1.currentLocation.isCollidingPosition(r, Game1.viewport, false, 8, false, this);
				this.sprite.AnimateRight(time, 0, "");
				this.sprite.interval = 220f;
			}
			if (this.sprite.currentFrame >= 8)
			{
				this.sprite.AnimateUp(time, 0, "");
			}
			if (this.sprite.currentFrame >= 10)
			{
				this.isInvisible = true;
				this.sprite.currentFrame = 0;
				this.hasDugForTreasure = false;
				int attempts = 0;
				Vector2 tile = base.getTileLocation();
				Game1.currentLocation.map.GetLayer("Back").Tiles[(int)tile.X, (int)tile.Y].TileIndex = 0;
				Game1.currentLocation.removeEverythingExceptCharactersFromThisTile((int)tile.X, (int)tile.Y);
				Vector2 attemptedPosition = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12)));
				while (attempts < 4 && (attemptedPosition.X <= 0f || attemptedPosition.X >= (float)Game1.currentLocation.map.Layers[0].LayerWidth || attemptedPosition.Y <= 0f || attemptedPosition.Y >= (float)Game1.currentLocation.map.Layers[0].LayerHeight || Game1.currentLocation.map.GetLayer("Back").Tiles[(int)attemptedPosition.X, (int)attemptedPosition.Y] == null || Game1.currentLocation.isTileOccupied(attemptedPosition, "") || !Game1.currentLocation.isTilePassable(new Location((int)attemptedPosition.X, (int)attemptedPosition.Y), Game1.viewport) || attemptedPosition.Equals(new Vector2((float)(Game1.player.getStandingX() / Game1.tileSize), (float)(Game1.player.getStandingY() / Game1.tileSize))) || Game1.currentLocation.map.GetLayer("Back").Tiles[(int)attemptedPosition.X, (int)attemptedPosition.Y].Properties.ContainsKey("NPCBarrier") || (!Game1.currentLocation.map.GetLayer("Back").Tiles[(int)attemptedPosition.X, (int)attemptedPosition.Y].TileIndexProperties.ContainsKey("Diggable") && Game1.currentLocation.map.GetLayer("Back").Tiles[(int)attemptedPosition.X, (int)attemptedPosition.Y].TileIndex != 0)))
				{
					attemptedPosition = new Vector2((float)(Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-2, 2)), (float)(Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-2, 2)));
					attempts++;
				}
			}
		}

		// Token: 0x04000BC9 RID: 3017
		private double chanceToDisappear = 0.03;

		// Token: 0x04000BCA RID: 3018
		private bool hasDugForTreasure;
	}
}
