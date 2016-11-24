using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Tools;

namespace StardewValley.Objects
{
	// Token: 0x0200008D RID: 141
	public class Cask : Object
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x000D9F75 File Offset: 0x000D8175
		public Cask()
		{
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000DAD5F File Offset: 0x000D8F5F
		public Cask(Vector2 v) : base(v, 163, false)
		{
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x000DAD70 File Offset: 0x000D8F70
		public override bool performToolAction(Tool t)
		{
			if (t == null || !t.isHeavyHitter() || t is MeleeWeapon)
			{
				return base.performToolAction(t);
			}
			if (this.heldObject != null)
			{
				Game1.createItemDebris(this.heldObject, this.tileLocation * (float)Game1.tileSize, -1, null);
			}
			Game1.soundBank.PlayCue("woodWhack");
			if (this.heldObject == null)
			{
				return true;
			}
			this.heldObject = null;
			this.minutesUntilReady = -1;
			return false;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x000DADE8 File Offset: 0x000D8FE8
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if (dropIn != null && dropIn.bigCraftable)
			{
				return false;
			}
			if (this.heldObject != null)
			{
				return false;
			}
			if (!probe && (who == null || !(who.currentLocation is Cellar)))
			{
				Game1.showRedMessageUsingLoadString("Strings\\Objects:CaskNoCellar");
				return false;
			}
			if (this.quality >= 4)
			{
				return false;
			}
			bool goodItem = false;
			float multiplier = 1f;
			int parentSheetIndex = dropIn.parentSheetIndex;
			if (parentSheetIndex <= 348)
			{
				if (parentSheetIndex != 303)
				{
					if (parentSheetIndex != 346)
					{
						if (parentSheetIndex == 348)
						{
							goodItem = true;
							multiplier = 1f;
						}
					}
					else
					{
						goodItem = true;
						multiplier = 2f;
					}
				}
				else
				{
					goodItem = true;
					multiplier = 1.66f;
				}
			}
			else if (parentSheetIndex != 424)
			{
				if (parentSheetIndex != 426)
				{
					if (parentSheetIndex == 459)
					{
						goodItem = true;
						multiplier = 2f;
					}
				}
				else
				{
					goodItem = true;
					multiplier = 4f;
				}
			}
			else
			{
				goodItem = true;
				multiplier = 4f;
			}
			if (goodItem)
			{
				this.heldObject = (dropIn.getOne() as Object);
				if (!probe)
				{
					this.agingRate = multiplier;
					this.daysToMature = 56f;
					this.minutesUntilReady = 999999;
					if (this.heldObject.quality == 1)
					{
						this.daysToMature = 42f;
					}
					else if (this.heldObject.quality == 2)
					{
						this.daysToMature = 28f;
					}
					else if (this.heldObject.quality == 4)
					{
						this.daysToMature = 0f;
						this.minutesUntilReady = 1;
					}
					Game1.playSound("Ship");
					Game1.playSound("bubbles");
					who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(256, 1856, 64, 128), 80f, 6, 999999, this.tileLocation * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize * 2)), false, false, (this.tileLocation.Y + 1f) * (float)Game1.tileSize / 10000f + 0.0001f, 0f, Color.Yellow * 0.75f, 1f, 0f, 0f, 0f, false)
					{
						alphaFade = 0.005f
					});
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000DB020 File Offset: 0x000D9220
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			return base.checkForAction(who, justCheckingForActivity);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000DB02A File Offset: 0x000D922A
		public override void DayUpdate(GameLocation location)
		{
			base.DayUpdate(location);
			if (this.heldObject != null)
			{
				this.minutesUntilReady = 999999;
				this.daysToMature -= this.agingRate;
				this.checkForMaturity();
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000DB060 File Offset: 0x000D9260
		public void checkForMaturity()
		{
			if (this.daysToMature <= 0f)
			{
				this.minutesUntilReady = 1;
				this.heldObject.quality = 4;
				return;
			}
			if (this.daysToMature <= 28f)
			{
				this.heldObject.quality = 2;
				return;
			}
			if (this.daysToMature <= 42f)
			{
				this.heldObject.quality = 1;
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x000DB0C4 File Offset: 0x000D92C4
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			base.draw(spriteBatch, x, y, alpha);
			if (this.heldObject != null && this.heldObject.quality > 0)
			{
				Vector2 scaleFactor = (this.minutesUntilReady > 0) ? new Vector2(Math.Abs(this.scale.X - 5f), Math.Abs(this.scale.Y - 5f)) : Vector2.Zero;
				scaleFactor *= (float)Game1.pixelZoom;
				Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize)));
				Rectangle destination = new Rectangle((int)(position.X + (float)(Game1.tileSize / 2) - (float)(Game1.pixelZoom * 2) - scaleFactor.X / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)(position.Y + (float)Game1.tileSize + (float)(Game1.pixelZoom * 2) - scaleFactor.Y / 2f) + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0), (int)((float)(Game1.pixelZoom * 4) + scaleFactor.X), (int)((float)(Game1.pixelZoom * 4) + scaleFactor.Y / 2f));
				spriteBatch.Draw(Game1.mouseCursors, destination, new Rectangle?((this.heldObject.quality < 4) ? new Rectangle(338 + (this.heldObject.quality - 1) * 8, 400, 8, 8) : new Rectangle(346, 392, 8, 8)), Color.White * 0.95f, 0f, Vector2.Zero, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f);
			}
		}

		// Token: 0x04000AD6 RID: 2774
		public const int defaultDaysToMature = 56;

		// Token: 0x04000AD7 RID: 2775
		public float agingRate;

		// Token: 0x04000AD8 RID: 2776
		public float daysToMature;
	}
}
