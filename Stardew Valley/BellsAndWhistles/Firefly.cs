using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000151 RID: 337
	public class Firefly : Critter
	{
		// Token: 0x060012F7 RID: 4855 RVA: 0x0017F05F File Offset: 0x0017D25F
		public Firefly()
		{
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0017F068 File Offset: 0x0017D268
		public Firefly(Vector2 position)
		{
			this.baseFrame = -1;
			this.position = position * (float)Game1.tileSize;
			this.startingPosition = position * (float)Game1.tileSize;
			this.motion = new Vector2((float)Game1.random.Next(-10, 11) * 0.1f, (float)Game1.random.Next(-10, 11) * 0.1f);
			this.id = (int)(position.X * 10099f + position.Y * 77f + (float)Game1.random.Next(99999));
			this.light = new LightSource(4, position, (float)Game1.random.Next(4, 6) * 0.1f, Color.Purple * 0.8f, this.id);
			this.glowing = true;
			Game1.currentLightSources.Add(this.light);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0017F15C File Offset: 0x0017D35C
		public override bool update(GameTime time, GameLocation environment)
		{
			this.position += this.motion;
			this.motion.X = this.motion.X + (float)Game1.random.Next(-1, 2) * 0.1f;
			this.motion.Y = this.motion.Y + (float)Game1.random.Next(-1, 2) * 0.1f;
			if (this.motion.X < -1f)
			{
				this.motion.X = -1f;
			}
			if (this.motion.X > 1f)
			{
				this.motion.X = 1f;
			}
			if (this.motion.Y < -1f)
			{
				this.motion.Y = -1f;
			}
			if (this.motion.Y > 1f)
			{
				this.motion.Y = 1f;
			}
			if (this.glowing)
			{
				this.light.position = this.position;
			}
			return this.position.X < (float)(-(float)Game1.tileSize * 2) || this.position.Y < (float)(-(float)Game1.tileSize * 2) || this.position.X > (float)environment.map.DisplayWidth || this.position.Y > (float)environment.map.DisplayHeight;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0017F2C8 File Offset: 0x0017D4C8
		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			b.Draw(Game1.staminaRect, Game1.GlobalToLocal(this.position), new Rectangle?(Game1.staminaRect.Bounds), this.glowing ? Color.White : Color.Brown, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
		}

		// Token: 0x04001376 RID: 4982
		private bool glowing;

		// Token: 0x04001377 RID: 4983
		private int glowTimer;

		// Token: 0x04001378 RID: 4984
		private int id;

		// Token: 0x04001379 RID: 4985
		private Vector2 motion;

		// Token: 0x0400137A RID: 4986
		private LightSource light;
	}
}
