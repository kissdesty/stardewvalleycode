using System;
using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;

namespace StardewValley.Projectiles
{
	// Token: 0x02000089 RID: 137
	public class DebuffingProjectile : Projectile
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x000D9624 File Offset: 0x000D7824
		public DebuffingProjectile(Buff debuff, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition, Character owner = null)
		{
			this.theOneWhoFiredMe = owner;
			this.debuff = debuff;
			this.currentTileSheetIndex = parentSheetIndex;
			this.bouncesLeft = bouncesTillDestruct;
			this.tailLength = tailLength;
			this.rotationVelocity = rotationVelocity;
			this.xVelocity = xVelocity;
			this.yVelocity = yVelocity;
			this.position = startingPosition;
			Game1.playSound("debuffSpell");
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x000D9688 File Offset: 0x000D7888
		public override void updatePosition(GameTime time)
		{
			this.position.X = this.position.X + this.xVelocity;
			this.position.Y = this.position.Y + this.yVelocity;
			this.position.X = this.position.X + (float)Math.Sin((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 128.0) * 8f;
			this.position.Y = this.position.Y + (float)Math.Cos((double)time.TotalGameTime.Milliseconds * 3.1415926535897931 / 128.0) * 8f;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000D973B File Offset: 0x000D793B
		public override void behaviorOnCollisionWithPlayer(GameLocation location)
		{
			if (Game1.random.Next(10) >= Game1.player.immunity)
			{
				Game1.buffsDisplay.addOtherBuff(this.debuff);
				this.explosionAnimation(location);
				Game1.playSound("debuffHit");
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x000D9777 File Offset: 0x000D7977
		public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
		{
			this.explosionAnimation(location);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000D9780 File Offset: 0x000D7980
		public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
		{
			this.explosionAnimation(Game1.mine);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000D978D File Offset: 0x000D798D
		public override void behaviorOnCollisionWithOther(GameLocation location)
		{
			this.explosionAnimation(location);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x000D9796 File Offset: 0x000D7996
		private void explosionAnimation(GameLocation location)
		{
			location.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float)Game1.random.Next(100, 150), 2, 1, this.position, false, false));
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00002834 File Offset: 0x00000A34
		public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
		{
		}

		// Token: 0x04000AA2 RID: 2722
		private Buff debuff;
	}
}
