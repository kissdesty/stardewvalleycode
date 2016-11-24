using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Objects
{
	// Token: 0x02000095 RID: 149
	public class Door : Object
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x000E1E01 File Offset: 0x000E0001
		public Door()
		{
			this.name = "Door";
			this.type = "interactive";
			this.bigCraftable = true;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x000E1E28 File Offset: 0x000E0028
		public Door(Vector2 tileLocation, GameLocation environment, bool locked)
		{
			this.locked = locked;
			this.bigCraftable = true;
			this.name = "Door";
			this.type = "interactive";
			this.tileLocation = tileLocation;
			this.checkForOrientation(environment);
			this.parentSheetIndex = 79;
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000E1EA4 File Offset: 0x000E00A4
		public void checkForOrientation(GameLocation environment)
		{
			Vector2 up = new Vector2(this.tileLocation.X, this.tileLocation.Y - 1f);
			Vector2 down = new Vector2(this.tileLocation.X, this.tileLocation.Y + 1f);
			if (environment.isTileOccupiedForPlacement(up, null) && environment.isTileOccupiedForPlacement(down, null))
			{
				this.showNextIndex = true;
			}
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000E1F14 File Offset: 0x000E0114
		public override void updateWhenCurrentLocation(GameTime time)
		{
			this.doorPosition += this.doorMotion;
			if ((this.doorPosition >= 112 - (this.showNextIndex ? 40 : 0) && this.doorMotion > 0) || this.doorPosition <= 0)
			{
				this.doorMotion = 0;
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x000E1F65 File Offset: 0x000E0165
		public override bool isPassable()
		{
			return this.doorPosition >= 112 - (this.showNextIndex ? 40 : 0);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000E1F84 File Offset: 0x000E0184
		private void unlock()
		{
			this.locked = false;
			this.fragility = 1;
			Game1.playSound("openBox");
			Game1.currentLocation.debris.Add(new Debris(Game1.bigCraftableSpriteSheet, new Rectangle(88, 2606, 16, 28), 1, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize)));
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000E1FFC File Offset: 0x000E01FC
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if (!this.locked || dropIn.parentSheetIndex != 471)
			{
				return false;
			}
			if (probe)
			{
				return true;
			}
			who.consumeObject(471, 1);
			this.unlock();
			return true;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000E2030 File Offset: 0x000E0230
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			if (this.locked)
			{
				if (!who.hasItemInInventory(471, 1, 0))
				{
					Game1.playSound("woodyStep");
					return false;
				}
				who.consumeObject(471, 1);
				this.unlock();
			}
			who.temporaryImpassableTile = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			if (this.doorMotion == 0 && this.doorPosition >= 112 - (this.showNextIndex ? 40 : 0))
			{
				this.doorMotion = -4;
			}
			else if (this.doorMotion == 0)
			{
				this.doorMotion = 4;
			}
			else
			{
				this.doorMotion = -1 * this.doorMotion;
			}
			return true;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x000E20FC File Offset: 0x000E02FC
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			Rectangle sourceRect = Object.getSourceRectForBigCraftable((this.showNextIndex ? (base.ParentSheetIndex + 1) : base.ParentSheetIndex) + (this.locked ? 2 : 0));
			sourceRect.Y += 29;
			sourceRect.Height = 12;
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize + 116))), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 32) / 9999f));
			sourceRect.Y -= 29;
			sourceRect.Height = 32;
			sourceRect.Height -= this.doorPosition;
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize + this.doorPosition))), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 1) * Game1.tileSize - 32) / 10000f));
			if (this.showNextIndex)
			{
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 + this.doorPosition * 3 / 2))), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)((y + 2) * Game1.tileSize - 32) / 10000f));
			}
		}

		// Token: 0x04000B23 RID: 2851
		public const int gateClosedPosition = 0;

		// Token: 0x04000B24 RID: 2852
		public const int gateOpenedPosition = 112;

		// Token: 0x04000B25 RID: 2853
		public int doorPosition;

		// Token: 0x04000B26 RID: 2854
		public int doorMotion;

		// Token: 0x04000B27 RID: 2855
		public bool locked;
	}
}
