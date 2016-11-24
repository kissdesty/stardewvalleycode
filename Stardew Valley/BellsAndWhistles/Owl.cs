using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000152 RID: 338
	public class Owl : Critter
	{
		// Token: 0x060012FB RID: 4859 RVA: 0x0017F05F File Offset: 0x0017D25F
		public Owl()
		{
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0017F324 File Offset: 0x0017D524
		public Owl(Vector2 position)
		{
			this.baseFrame = 83;
			this.position = position;
			this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 32, 32);
			this.startingPosition = position;
			this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(83, 100),
				new FarmerSprite.AnimationFrame(84, 100),
				new FarmerSprite.AnimationFrame(85, 100),
				new FarmerSprite.AnimationFrame(86, 100)
			});
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0017F3B4 File Offset: 0x0017D5B4
		public override bool update(GameTime time, GameLocation environment)
		{
			Vector2 parallax = new Vector2((float)Game1.viewport.X - Game1.previousViewportPosition.X, (float)Game1.viewport.Y - Game1.previousViewportPosition.Y) * 0.15f;
			this.position.Y = this.position.Y + (float)time.ElapsedGameTime.TotalMilliseconds * 0.2f;
			this.position.X = this.position.X + (float)time.ElapsedGameTime.TotalMilliseconds * 0.05f;
			this.position -= parallax;
			return base.update(time, environment);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00002834 File Offset: 0x00000A34
		public override void draw(SpriteBatch b)
		{
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0017F460 File Offset: 0x0017D660
		public override void drawAboveFrontLayer(SpriteBatch b)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, -128f + this.yJumpOffset + this.yOffset)), this.position.Y / 10000f + this.position.X / 100000f, 0, 0, Color.MediumBlue, this.flip, 4f, 0f, false);
		}
	}
}
