using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;

namespace StardewValley.Buildings
{
	// Token: 0x02000148 RID: 328
	public class Stable : Building
	{
		// Token: 0x06001290 RID: 4752 RVA: 0x00178B10 File Offset: 0x00176D10
		public Stable(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00178B1A File Offset: 0x00176D1A
		public Stable()
		{
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00178B22 File Offset: 0x00176D22
		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height);
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00178B4B File Offset: 0x00176D4B
		public override void load()
		{
			base.load();
			this.grabHorse();
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00178B5C File Offset: 0x00176D5C
		public void grabHorse()
		{
			Horse horse = Utility.findHorse();
			if (horse == null)
			{
				Game1.getFarm().characters.Add(new Horse(this.tileX + 1, this.tileY + 1));
				return;
			}
			Game1.warpCharacter(horse, "Farm", new Point(this.tileX + 1, this.tileY + 1), false, true);
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00178BB9 File Offset: 0x00176DB9
		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			if (this.daysOfConstructionLeft <= 0)
			{
				this.grabHorse();
			}
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x00178BD4 File Offset: 0x00176DD4
		public override bool intersects(Rectangle boundingBox)
		{
			return base.intersects(boundingBox) && (boundingBox.X < (this.tileX + 1) * Game1.tileSize || boundingBox.Right >= (this.tileX + 3) * Game1.tileSize || boundingBox.Y <= (this.tileY + 1) * Game1.tileSize);
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00178C34 File Offset: 0x00176E34
		public override void Update(GameTime time)
		{
			base.Update(time);
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x00178C40 File Offset: 0x00176E40
		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.texture.Bounds), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
		}
	}
}
