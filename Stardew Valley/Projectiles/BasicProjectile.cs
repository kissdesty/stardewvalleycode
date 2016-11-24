using System;
using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;

namespace StardewValley.Projectiles
{
	// Token: 0x02000088 RID: 136
	public class BasicProjectile : Projectile
	{
		// Token: 0x06000A28 RID: 2600 RVA: 0x000D9214 File Offset: 0x000D7414
		public BasicProjectile(int damageToFarmer, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition, string collisionSound, string firingSound, bool explode, bool damagesMonsters = false, Character firer = null, bool spriteFromObjectSheet = false, BasicProjectile.onCollisionBehavior collisionBehavior = null)
		{
			this.damageToFarmer = damageToFarmer;
			this.currentTileSheetIndex = parentSheetIndex;
			this.bouncesLeft = bouncesTillDestruct;
			this.tailLength = tailLength;
			this.rotationVelocity = rotationVelocity;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.position = startingPosition;
			if (firingSound != null && !firingSound.Equals(""))
			{
				Game1.playSound(firingSound);
			}
			this.explode = explode;
			this.collisionSound = collisionSound;
			this.damagesMonsters = damagesMonsters;
			this.theOneWhoFiredMe = firer;
			this.spriteFromObjectSheet = spriteFromObjectSheet;
			this.collisionBehavior = collisionBehavior;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x000D92B0 File Offset: 0x000D74B0
		public BasicProjectile(int damageToFarmer, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition) : this(damageToFarmer, parentSheetIndex, bouncesTillDestruct, tailLength, rotationVelocity, xVelocity, yVelocity, startingPosition, "flameSpellHit", "flameSpell", true, false, null, false, null)
		{
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x000D92DF File Offset: 0x000D74DF
		public override void updatePosition(GameTime time)
		{
			this.position.X = this.position.X + this.xVelocity;
			this.position.Y = this.position.Y + this.yVelocity;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000D930B File Offset: 0x000D750B
		public override void behaviorOnCollisionWithPlayer(GameLocation location)
		{
			if (!this.damagesMonsters)
			{
				Game1.farmerTakeDamage(this.damageToFarmer, false, null);
				this.explosionAnimation(location);
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000D9329 File Offset: 0x000D7529
		public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
		{
			t.performUseAction(tileLocation);
			this.explosionAnimation(location);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000D933A File Offset: 0x000D753A
		public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
		{
			this.explosionAnimation(Game1.mine);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000D9347 File Offset: 0x000D7547
		public override void behaviorOnCollisionWithOther(GameLocation location)
		{
			this.explosionAnimation(location);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000D9350 File Offset: 0x000D7550
		public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
		{
			if (this.damagesMonsters)
			{
				this.explosionAnimation(location);
				if (n is Monster)
				{
					location.damageMonster(n.GetBoundingBox(), this.damageToFarmer, this.damageToFarmer + 1, false, (this.theOneWhoFiredMe is Farmer) ? (this.theOneWhoFiredMe as Farmer) : Game1.player);
					return;
				}
				n.getHitByPlayer((this.theOneWhoFiredMe == null || !(this.theOneWhoFiredMe is Farmer)) ? Game1.player : (this.theOneWhoFiredMe as Farmer), location);
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000D93E0 File Offset: 0x000D75E0
		private void explosionAnimation(GameLocation location)
		{
			Rectangle sourceRect = Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, -1, -1);
			sourceRect.X += Game1.tileSize / 2 - 4;
			sourceRect.Y += Game1.tileSize / 2 - 4;
			sourceRect.Width = 8;
			sourceRect.Height = 8;
			int whichDebris = 12;
			int currentTileSheetIndex = this.currentTileSheetIndex;
			switch (currentTileSheetIndex)
			{
			case 378:
				whichDebris = 0;
				break;
			case 379:
			case 381:
			case 383:
			case 385:
				break;
			case 380:
				whichDebris = 2;
				break;
			case 382:
				whichDebris = 4;
				break;
			case 384:
				whichDebris = 6;
				break;
			case 386:
				whichDebris = 10;
				break;
			default:
				if (currentTileSheetIndex == 390)
				{
					whichDebris = 14;
				}
				break;
			}
			if (this.spriteFromObjectSheet)
			{
				Game1.createRadialDebris(location, whichDebris, (int)(this.position.X + (float)(Game1.tileSize / 2)) / Game1.tileSize, (int)(this.position.Y + (float)(Game1.tileSize / 2)) / Game1.tileSize, 6, false, -1, false, -1);
			}
			else
			{
				Game1.createRadialDebris(location, Projectile.projectileSheet, sourceRect, 4, (int)this.position.X + Game1.tileSize / 2, (int)this.position.Y + Game1.tileSize / 2, 12, (int)(this.position.Y / (float)Game1.tileSize) + 1);
			}
			if (this.collisionSound != null && !this.collisionSound.Equals(""))
			{
				Game1.playSound(this.collisionSound);
			}
			if (this.explode)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, this.position, false, Game1.random.NextDouble() < 0.5));
			}
			if (this.collisionBehavior != null)
			{
				this.collisionBehavior(location, this.getBoundingBox().Center.X, this.getBoundingBox().Center.Y, this.theOneWhoFiredMe);
			}
			this.destroyMe = true;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000D95F3 File Offset: 0x000D77F3
		public static void explodeOnImpact(GameLocation location, int x, int y, Character who)
		{
			location.explode(new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize)), 3, (who is Farmer) ? ((Farmer)who) : null);
		}

		// Token: 0x04000A9E RID: 2718
		public int damageToFarmer;

		// Token: 0x04000A9F RID: 2719
		private string collisionSound;

		// Token: 0x04000AA0 RID: 2720
		private bool explode;

		// Token: 0x04000AA1 RID: 2721
		private BasicProjectile.onCollisionBehavior collisionBehavior;

		// Token: 0x02000175 RID: 373
		// Token: 0x06001399 RID: 5017
		public delegate void onCollisionBehavior(GameLocation location, int xPosition, int yPosition, Character who);
	}
}
