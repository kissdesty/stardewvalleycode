using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000B7 RID: 183
	public class ShadowGirl : Monster
	{
		// Token: 0x06000BED RID: 3053 RVA: 0x000ED6D4 File Offset: 0x000EB8D4
		public ShadowGirl()
		{
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x000ED6E8 File Offset: 0x000EB8E8
		public ShadowGirl(Vector2 position) : base("Shadow Girl", position)
		{
			base.IsWalkingTowardPlayer = false;
			this.moveTowardPlayerThreshold = 8;
			if (Game1.player.friendships.ContainsKey("???") && Game1.player.friendships["???"][0] >= 1250)
			{
				this.damageToFarmer = 0;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x000ED754 File Offset: 0x000EB954
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Girl"));
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000ED770 File Offset: 0x000EB970
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			int actualDamage = Math.Max(1, damage - this.resilience);
			if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				if (Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
				{
					this.health -= damage * 3 / 4;
					Game1.currentLocation.debris.Add(new Debris(string.Concat(damage * 3 / 4), 1, new Vector2((float)base.getStandingX(), (float)base.getStandingY()), Color.LightBlue, 1f, 0f));
				}
				this.health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (this.health <= 0)
				{
					this.deathAnimation();
				}
			}
			return actualDamage;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000ED84C File Offset: 0x000EBA4C
		public override void deathAnimation()
		{
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0));
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, Game1.tileSize, Game1.tileSize / 3), Game1.tileSize, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + Game1.tileSize / 6, this.sprite.SourceRect.Y + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize / 3), Game1.tileSize * 2 / 3, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x000ED984 File Offset: 0x000EBB84
		public override void update(GameTime time, GameLocation location)
		{
			if (!location.Equals(Game1.currentLocation))
			{
				return;
			}
			if (!Game1.player.isRafting || !base.withinPlayerThreshold(4))
			{
				base.updateGlow();
				base.updateEmote(time);
				if (this.controller == null)
				{
					this.updateMovement(location, time);
				}
				if (this.controller != null && this.controller.update(time))
				{
					this.controller = null;
				}
			}
			this.behaviorAtGameTick(time);
			if (this.position.X < 0f || this.position.X > (float)(location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || this.position.Y < 0f || this.position.Y > (float)(location.map.GetLayer("Back").LayerHeight * Game1.tileSize))
			{
				location.characters.Remove(this);
			}
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000EDA78 File Offset: 0x000EBC78
		public override void behaviorAtGameTick(GameTime time)
		{
			base.behaviorAtGameTick(time);
			this.addedSpeed = 0;
			this.speed = 3;
			if (this.howLongOnThisPosition > 500 && this.controller == null)
			{
				base.IsWalkingTowardPlayer = false;
				this.controller = new PathFindController(this, Game1.currentLocation, new Point((int)Game1.player.getTileLocation().X, (int)Game1.player.getTileLocation().Y), Game1.random.Next(4), null, 300);
				this.timeBeforeAIMovementAgain = 2000f;
				this.howLongOnThisPosition = 0;
			}
			else if (this.controller == null)
			{
				base.IsWalkingTowardPlayer = true;
			}
			if (this.position.Equals(this.lastPosition))
			{
				this.howLongOnThisPosition += time.ElapsedGameTime.Milliseconds;
			}
			else
			{
				this.howLongOnThisPosition = 0;
			}
			this.lastPosition = this.position;
		}

		// Token: 0x04000BB8 RID: 3000
		public const int blockTimeBeforePathfinding = 500;

		// Token: 0x04000BB9 RID: 3001
		private new Vector2 lastPosition = Vector2.Zero;

		// Token: 0x04000BBA RID: 3002
		private int howLongOnThisPosition;
	}
}
