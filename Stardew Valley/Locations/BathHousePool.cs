using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace StardewValley.Locations
{
	// Token: 0x02000120 RID: 288
	public class BathHousePool : GameLocation
	{
		// Token: 0x0600106C RID: 4204 RVA: 0x0015306D File Offset: 0x0015126D
		public BathHousePool()
		{
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00153085 File Offset: 0x00151285
		public BathHousePool(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x001530A0 File Offset: 0x001512A0
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			Game1.changeMusicTrack("pool_ambient");
			this.steamPosition = new Vector2(0f, 0f);
			this.steamAnimation = this.tempManager.Load<Texture2D>("LooseSprites\\steamAnimation");
			this.swimShadow = this.tempManager.Load<Texture2D>("LooseSprites\\swimShadow");
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x001530FE File Offset: 0x001512FE
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.tempManager.Unload();
			Game1.changeMusicTrack("none");
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0015311C File Offset: 0x0015131C
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.currentEvent != null)
			{
				using (List<NPC>.Enumerator enumerator = this.currentEvent.actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC i = enumerator.Current;
						if (i.swimming)
						{
							b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, i.position + new Vector2(0f, (float)(i.sprite.spriteHeight / 3 * Game1.pixelZoom + Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
						}
					}
					goto IL_237;
				}
			}
			foreach (NPC j in this.characters)
			{
				if (j.swimming)
				{
					b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, j.position + new Vector2(0f, (float)(j.sprite.spriteHeight / 3 * Game1.pixelZoom + Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				}
			}
			foreach (Farmer f in this.farmers)
			{
				if (f.swimming)
				{
					b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, f.position + new Vector2(0f, (float)(f.sprite.spriteHeight / 3 * Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
				}
			}
			IL_237:
			if (Game1.player.swimming)
			{
				b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(0f, (float)(Game1.player.sprite.spriteHeight / 4 * Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.Blue * 0.75f, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0015341C File Offset: 0x0015161C
		public override void checkForMusic(GameTime time)
		{
			base.checkForMusic(time);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00153428 File Offset: 0x00151628
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			for (float x = this.steamPosition.X; x < (float)Game1.graphics.GraphicsDevice.Viewport.Width + 256f; x += 256f)
			{
				for (float y = this.steamPosition.Y; y < (float)(Game1.graphics.GraphicsDevice.Viewport.Height + 128); y += 256f)
				{
					b.Draw(this.steamAnimation, new Vector2(x, y), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White * 0.8f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
				}
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x001534F8 File Offset: 0x001516F8
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			this.steamPosition.Y = this.steamPosition.Y - (float)time.ElapsedGameTime.Milliseconds * 0.1f;
			this.steamPosition.Y = this.steamPosition.Y % -256f;
			this.steamPosition -= Game1.getMostRecentViewportMotion();
			this.swimShadowTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.swimShadowTimer <= 0)
			{
				this.swimShadowTimer = 70;
				this.swimShadowFrame++;
				this.swimShadowFrame %= 10;
			}
		}

		// Token: 0x040011D0 RID: 4560
		public const float steamZoom = 4f;

		// Token: 0x040011D1 RID: 4561
		public const float steamYMotionPerMillisecond = 0.1f;

		// Token: 0x040011D2 RID: 4562
		public const float millisecondsPerSteamFrame = 50f;

		// Token: 0x040011D3 RID: 4563
		private LocalizedContentManager tempManager = Game1.content.CreateTemporary();

		// Token: 0x040011D4 RID: 4564
		private Texture2D steamAnimation;

		// Token: 0x040011D5 RID: 4565
		private Texture2D swimShadow;

		// Token: 0x040011D6 RID: 4566
		private Vector2 steamPosition;

		// Token: 0x040011D7 RID: 4567
		private int swimShadowTimer;

		// Token: 0x040011D8 RID: 4568
		private int swimShadowFrame;
	}
}
