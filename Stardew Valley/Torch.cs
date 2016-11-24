using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley
{
	// Token: 0x02000034 RID: 52
	public class Torch : Object
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0003693C File Offset: 0x00034B3C
		public Torch()
		{
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00036950 File Offset: 0x00034B50
		public Torch(Vector2 tileLocation, int initialStack) : base(tileLocation, 93, initialStack)
		{
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000369A8 File Offset: 0x00034BA8
		public Torch(Vector2 tileLocation, int initialStack, int index) : base(tileLocation, index, initialStack)
		{
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00036A00 File Offset: 0x00034C00
		public Torch(Vector2 tileLocation, int index, bool bigCraftable) : base(tileLocation, index, false)
		{
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00036A54 File Offset: 0x00034C54
		public override Item getOne()
		{
			if (this.bigCraftable)
			{
				return new Torch(this.tileLocation, this.parentSheetIndex, true)
				{
					isRecipe = this.isRecipe
				};
			}
			return new Torch(this.tileLocation, 1)
			{
				isRecipe = this.isRecipe
			};
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00036AA0 File Offset: 0x00034CA0
		public override void actionOnPlayerEntry()
		{
			base.actionOnPlayerEntry();
			if (this.bigCraftable && this.isOn)
			{
				AmbientLocationSounds.addSound(this.tileLocation, 1);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00036AC4 File Offset: 0x00034CC4
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (!this.bigCraftable)
			{
				return base.checkForAction(who, justCheckingForActivity);
			}
			if (justCheckingForActivity)
			{
				return true;
			}
			this.isOn = !this.isOn;
			if (this.isOn)
			{
				if (this.bigCraftable)
				{
					if (who != null)
					{
						Game1.playSound("fireball");
					}
					base.initializeLightSource(this.tileLocation);
					AmbientLocationSounds.addSound(this.tileLocation, 1);
				}
			}
			else if (this.bigCraftable)
			{
				this.performRemoveAction(this.tileLocation, Game1.currentLocation);
				if (who != null)
				{
					Game1.playSound("woodyHit");
				}
			}
			return true;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00036B54 File Offset: 0x00034D54
		public override bool placementAction(GameLocation location, int x, int y, Farmer who)
		{
			Vector2 placementTile = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			Torch toPlace = this.bigCraftable ? new Torch(placementTile, this.parentSheetIndex, true) : new Torch(placementTile, 1, this.parentSheetIndex);
			if (this.bigCraftable)
			{
				toPlace.isOn = false;
			}
			toPlace.tileLocation = placementTile;
			toPlace.initializeLightSource(placementTile);
			location.objects.Add(placementTile, toPlace);
			if (who != null)
			{
				Game1.playSound("woodyStep");
			}
			return true;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00036BD6 File Offset: 0x00034DD6
		public override void DayUpdate(GameLocation location)
		{
			base.DayUpdate(location);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00036BDF File Offset: 0x00034DDF
		public override bool isPassable()
		{
			return !this.bigCraftable;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00036BEA File Offset: 0x00034DEA
		public override void updateWhenCurrentLocation(GameTime time)
		{
			base.updateWhenCurrentLocation(time);
			this.updateAshes((int)(this.tileLocation.X * 2000f + this.tileLocation.Y));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00036C17 File Offset: 0x00034E17
		public override void actionWhenBeingHeld(Farmer who)
		{
			base.actionWhenBeingHeld(who);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00036C20 File Offset: 0x00034E20
		private void updateAshes(int identifier)
		{
			if (Utility.isOnScreen(this.tileLocation * (float)Game1.tileSize, 4 * Game1.tileSize))
			{
				for (int i = this.ashes.Length - 1; i >= 0; i--)
				{
					Vector2 temp = this.ashes[i];
					temp.Y -= 1f * ((float)(i + 1) * 0.25f);
					if (i % 2 != 0)
					{
						temp.X += (float)Math.Sin((double)this.ashes[i].Y / 6.2831853071795862) / 2f;
					}
					this.ashes[i] = temp;
					if (Game1.random.NextDouble() < 0.0075 && this.ashes[i].Y < -100f)
					{
						this.ashes[i] = new Vector2((float)(Game1.random.Next(-1, 3) * Game1.pixelZoom) * 0.75f, 0f);
					}
				}
				this.color = Math.Max(-0.8f, Math.Min(0.7f, this.color + this.ashes[0].Y / 1200f));
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00036D6B File Offset: 0x00034F6B
		public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
		{
			AmbientLocationSounds.removeSound(this.tileLocation);
			if (this.bigCraftable)
			{
				this.isOn = false;
			}
			base.performRemoveAction(this.tileLocation, environment);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00036D94 File Offset: 0x00034F94
		public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
		{
			Rectangle sourceRect = Game1.currentLocation.getSourceRectForObject(base.ParentSheetIndex);
			sourceRect.Y += 8;
			sourceRect.Height /= 2;
			spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)xNonTile, (float)(yNonTile + Game1.tileSize / 2))), new Rectangle?(sourceRect), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
			sourceRect.X = 276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(xNonTile * 320) + (double)(yNonTile * 49)) % 700.0 / 100.0) * 8;
			sourceRect.Y = 1965;
			sourceRect.Width = 8;
			sourceRect.Height = 8;
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + Game1.tileSize / 2 + Game1.pixelZoom), (float)(yNonTile + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(sourceRect), Color.White * 0.75f, 0f, new Vector2(4f, 4f), (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, layerDepth + 1E-05f);
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(xNonTile + Game1.tileSize / 2 + Game1.pixelZoom), (float)(yNonTile + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(new Rectangle(88, 1779, 30, 30)), Color.PaleGoldenrod * (Game1.currentLocation.IsOutdoors ? 0.35f : 0.43f), 0f, new Vector2(15f, 15f), (float)(Game1.pixelZoom * 2) + (float)((double)(Game1.tileSize / 2) * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(xNonTile * 777) + (double)(yNonTile * 9746)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00036FCC File Offset: 0x000351CC
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (!Game1.eventUp || Game1.currentLocation.IsFarm)
			{
				if (!this.bigCraftable)
				{
					Rectangle sourceRect = Game1.currentLocation.getSourceRectForObject(base.ParentSheetIndex);
					sourceRect.Y += 8;
					sourceRect.Height /= 2;
					Texture2D arg_D7_1 = Game1.objectSpriteSheet;
					Vector2 arg_D7_2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize + Game1.tileSize / 2)));
					Rectangle? arg_D7_3 = new Rectangle?(sourceRect);
					Color arg_D7_4 = Color.White;
					float arg_D7_5 = 0f;
					Vector2 arg_D7_6 = Vector2.Zero;
					Vector2 arg_92_0 = this.scale;
					spriteBatch.Draw(arg_D7_1, arg_D7_2, arg_D7_3, arg_D7_4, arg_D7_5, arg_D7_6, (this.scale.Y > 1f) ? base.getScale().Y : ((float)Game1.pixelZoom), SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom / 2), (float)(y * Game1.tileSize + Game1.tileSize / 4))), new Rectangle?(new Rectangle(88, 1779, 30, 30)), Color.PaleGoldenrod * (Game1.currentLocation.IsOutdoors ? 0.35f : 0.43f), 0f, new Vector2(15f, 15f), (float)Game1.pixelZoom + (float)((double)Game1.tileSize * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * Game1.tileSize * 777) + (double)(y * Game1.tileSize * 9746)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
					sourceRect.X = 276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 3204) + (double)(y * 49)) % 700.0 / 100.0) * 8;
					sourceRect.Y = 1965;
					sourceRect.Width = 8;
					sourceRect.Height = 8;
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float)(y * Game1.tileSize + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(sourceRect), Color.White * 0.75f, 0f, new Vector2(4f, 4f), (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom + 1) / 10000f);
					for (int i = 0; i < this.ashes.Length; i++)
					{
						spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2) + this.ashes[i].X, (float)(y * Game1.tileSize + Game1.tileSize / 2) + this.ashes[i].Y)), new Rectangle?(new Rectangle(344 + i % 3, 53, 1, 1)), Color.White * 0.5f * ((-100f - this.ashes[i].Y / 2f) / -100f), 0f, Vector2.Zero, (float)Game1.pixelZoom * 0.75f, SpriteEffects.None, (float)this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom / 10000f);
					}
					return;
				}
				base.draw(spriteBatch, x, y, alpha);
				if (this.isOn)
				{
					if (this.parentSheetIndex == 146)
					{
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 4 - Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 3047) + (double)(y * 88)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 16) / 10000f);
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 3), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 2047) + (double)(y * 98)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 15) / 10000f);
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 5), (float)(y * Game1.tileSize + Game1.pixelZoom * 3))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 2077) + (double)(y * 98)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)(Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 14) / 10000f);
						return;
					}
					spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 4 - Game1.pixelZoom * 2), (float)(y * Game1.tileSize - Game1.tileSize + Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(276 + (int)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double)(x * 3047) + (double)(y * 88)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(this.getBoundingBox(new Vector2((float)x, (float)y)).Bottom - 16) / 10000f);
				}
			}
		}

		// Token: 0x040002C7 RID: 711
		public const float yVelocity = 1f;

		// Token: 0x040002C8 RID: 712
		public const float yDissapearLevel = -100f;

		// Token: 0x040002C9 RID: 713
		public const double ashChance = 0.015;

		// Token: 0x040002CA RID: 714
		private float color;

		// Token: 0x040002CB RID: 715
		private Vector2[] ashes = new Vector2[3];
	}
}
