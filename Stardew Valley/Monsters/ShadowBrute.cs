using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
	// Token: 0x020000AA RID: 170
	public class ShadowBrute : Monster
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x000E5650 File Offset: 0x000E3850
		public ShadowBrute()
		{
			this.sprite.spriteHeight = 32;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000E69E6 File Offset: 0x000E4BE6
		public ShadowBrute(Vector2 position) : base("Shadow Brute", position)
		{
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000E6A0C File Offset: 0x000E4C0C
		public override void reloadSprite()
		{
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Brute"));
			this.sprite.spriteHeight = 32;
			this.sprite.UpdateSourceRect();
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x000E6A40 File Offset: 0x000E4C40
		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
		{
			Game1.playSound("shadowHit");
			return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000E6A5C File Offset: 0x000E4C5C
		public override void deathAnimation()
		{
			Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0), Game1.currentLocation, 4, 64, 64);
			for (int i = 1; i < 3; i++)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(0f, 1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(0f, -1f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, 0f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, 0f) * (float)Game1.tileSize * (float)i, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = i * 159
				});
			}
			Game1.playSound("shadowDie");
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, 16, 5), 16, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White, (float)Game1.pixelZoom);
			Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + 2, this.sprite.SourceRect.Y + 5, 16, 5), 10, base.getStandingX(), base.getStandingY() - Game1.tileSize / 2, 1, base.getStandingY() / Game1.tileSize, Color.White, (float)Game1.pixelZoom);
		}
	}
}
