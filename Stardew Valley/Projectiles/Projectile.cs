using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace StardewValley.Projectiles
{
	// Token: 0x0200008A RID: 138
	public abstract class Projectile
	{
		// Token: 0x06000A3A RID: 2618 RVA: 0x000D97CC File Offset: 0x000D79CC
		private bool behaviorOnCollision(GameLocation location)
		{
			foreach (Vector2 tile in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(this.getBoundingBox()))
			{
				if (!this.damagesMonsters && Game1.player.GetBoundingBox().Intersects(this.getBoundingBox()))
				{
					this.behaviorOnCollisionWithPlayer(location);
					bool result = true;
					return result;
				}
				if (location.terrainFeatures.ContainsKey(tile) && !location.terrainFeatures[tile].isPassable(null))
				{
					this.behaviorOnCollisionWithTerrainFeature(location.terrainFeatures[tile], tile, location);
					bool result = true;
					return result;
				}
				if (this.damagesMonsters)
				{
					NPC i = location.doesPositionCollideWithCharacter(this.getBoundingBox(), false);
					if (i != null)
					{
						this.behaviorOnCollisionWithMonster(i, location);
						bool result = true;
						return result;
					}
				}
			}
			this.behaviorOnCollisionWithOther(location);
			return true;
		}

		// Token: 0x06000A3B RID: 2619
		public abstract void behaviorOnCollisionWithPlayer(GameLocation location);

		// Token: 0x06000A3C RID: 2620
		public abstract void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location);

		// Token: 0x06000A3D RID: 2621
		public abstract void behaviorOnCollisionWithMineWall(int tileX, int tileY);

		// Token: 0x06000A3E RID: 2622
		public abstract void behaviorOnCollisionWithOther(GameLocation location);

		// Token: 0x06000A3F RID: 2623
		public abstract void behaviorOnCollisionWithMonster(NPC n, GameLocation location);

		// Token: 0x06000A40 RID: 2624 RVA: 0x000D98C0 File Offset: 0x000D7AC0
		public bool update(GameTime time, GameLocation location)
		{
			this.rotation += this.rotationVelocity;
			this.travelTime += time.ElapsedGameTime.Milliseconds;
			this.updatePosition(time);
			this.updateTail(time);
			if (this.isColliding(location) && this.travelTime > 100)
			{
				if (this.bouncesLeft <= 0 || Game1.player.GetBoundingBox().Intersects(this.getBoundingBox()))
				{
					return this.behaviorOnCollision(location);
				}
				this.bouncesLeft--;
				bool[] expr_92 = Utility.horizontalOrVerticalCollisionDirections(this.getBoundingBox(), this.theOneWhoFiredMe, true);
				if (expr_92[0])
				{
					this.xVelocity = -this.xVelocity;
				}
				if (expr_92[1])
				{
					this.yVelocity = -this.yVelocity;
				}
			}
			return false;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000D9990 File Offset: 0x000D7B90
		private void updateTail(GameTime time)
		{
			this.tailCounter -= time.ElapsedGameTime.Milliseconds;
			if (this.tailCounter <= 0)
			{
				this.tailCounter = 50;
				this.tail.Enqueue(this.position);
				if (this.tail.Count > this.tailLength)
				{
					this.tail.Dequeue();
				}
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000D99FC File Offset: 0x000D7BFC
		public virtual bool isColliding(GameLocation location)
		{
			return !location.isTileOnMap(this.position / (float)Game1.tileSize) || (!this.ignoreLocationCollision && location.isCollidingPosition(this.getBoundingBox(), Game1.viewport, false, 0, true, this.theOneWhoFiredMe, false, true, false)) || (!this.damagesMonsters && Game1.player.GetBoundingBox().Intersects(this.getBoundingBox())) || (this.damagesMonsters && location.doesPositionCollideWithCharacter(this.getBoundingBox(), false) != null);
		}

		// Token: 0x06000A43 RID: 2627
		public abstract void updatePosition(GameTime time);

		// Token: 0x06000A44 RID: 2628 RVA: 0x000D9A88 File Offset: 0x000D7C88
		public virtual Rectangle getBoundingBox()
		{
			return new Rectangle((int)this.position.X + Game1.tileSize / 2 - (Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0)) / 2, (int)this.position.Y + Game1.tileSize / 2 - (Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0)) / 2, Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0), Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0));
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000D9B10 File Offset: 0x000D7D10
		public virtual void draw(SpriteBatch b)
		{
			b.Draw(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, 16, 16)), Color.White, this.rotation, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (this.position.Y + (float)(Game1.tileSize * 3 / 2)) / 10000f);
			float scale = (float)Game1.pixelZoom;
			float alpha = 1f;
			for (int i = this.tail.Count - 1; i >= 0; i--)
			{
				b.Draw(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, this.tail.ElementAt(i) + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, 16, 16)), Color.White * alpha, this.rotation, new Vector2(8f, 8f), scale, SpriteEffects.None, (this.tail.ElementAt(i).Y + (float)(Game1.tileSize * 3 / 2)) / 10000f);
				scale = 0.8f * (float)(Game1.pixelZoom - Game1.pixelZoom / (i + Game1.pixelZoom));
				alpha -= 0.1f;
			}
		}

		// Token: 0x04000AA3 RID: 2723
		public const int travelTimeBeforeCollisionPossible = 100;

		// Token: 0x04000AA4 RID: 2724
		public const int goblinsCurseIndex = 0;

		// Token: 0x04000AA5 RID: 2725
		public const int flameBallIndex = 1;

		// Token: 0x04000AA6 RID: 2726
		public const int fearBolt = 2;

		// Token: 0x04000AA7 RID: 2727
		public const int shadowBall = 3;

		// Token: 0x04000AA8 RID: 2728
		public const int bone = 4;

		// Token: 0x04000AA9 RID: 2729
		public const int throwingKnife = 5;

		// Token: 0x04000AAA RID: 2730
		public const int snowBall = 6;

		// Token: 0x04000AAB RID: 2731
		public const int shamanBolt = 7;

		// Token: 0x04000AAC RID: 2732
		public const int frostBall = 8;

		// Token: 0x04000AAD RID: 2733
		public const int frozenBolt = 9;

		// Token: 0x04000AAE RID: 2734
		public const int fireball = 10;

		// Token: 0x04000AAF RID: 2735
		public const int timePerTailUpdate = 50;

		// Token: 0x04000AB0 RID: 2736
		public static int boundingBoxWidth = Game1.tileSize / 3;

		// Token: 0x04000AB1 RID: 2737
		public static int boundingBoxHeight = Game1.tileSize / 3;

		// Token: 0x04000AB2 RID: 2738
		public static Texture2D projectileSheet = Game1.content.Load<Texture2D>("TileSheets\\Projectiles");

		// Token: 0x04000AB3 RID: 2739
		protected int currentTileSheetIndex;

		// Token: 0x04000AB4 RID: 2740
		protected Vector2 position;

		// Token: 0x04000AB5 RID: 2741
		protected int tailLength;

		// Token: 0x04000AB6 RID: 2742
		protected int tailCounter = 50;

		// Token: 0x04000AB7 RID: 2743
		protected int bouncesLeft;

		// Token: 0x04000AB8 RID: 2744
		protected int travelTime;

		// Token: 0x04000AB9 RID: 2745
		protected float rotation;

		// Token: 0x04000ABA RID: 2746
		protected float rotationVelocity;

		// Token: 0x04000ABB RID: 2747
		protected float xVelocity;

		// Token: 0x04000ABC RID: 2748
		protected float yVelocity;

		// Token: 0x04000ABD RID: 2749
		private Queue<Vector2> tail = new Queue<Vector2>();

		// Token: 0x04000ABE RID: 2750
		protected bool damagesMonsters;

		// Token: 0x04000ABF RID: 2751
		protected bool spriteFromObjectSheet;

		// Token: 0x04000AC0 RID: 2752
		protected Character theOneWhoFiredMe;

		// Token: 0x04000AC1 RID: 2753
		public bool ignoreLocationCollision;

		// Token: 0x04000AC2 RID: 2754
		public bool destroyMe;
	}
}
