using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley
{
	// Token: 0x02000012 RID: 18
	public class FarmerRenderer
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x0000A19C File Offset: 0x0000839C
		public FarmerRenderer(Texture2D baseTexture)
		{
			this.baseTexture = baseTexture;
			if (FarmerRenderer.hairStylesTexture == null)
			{
				FarmerRenderer.hairStylesTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\hairstyles");
			}
			if (FarmerRenderer.shirtsTexture == null)
			{
				FarmerRenderer.shirtsTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\shirts");
			}
			if (FarmerRenderer.hatsTexture == null)
			{
				FarmerRenderer.hatsTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\hats");
			}
			if (FarmerRenderer.accessoriesTexture == null)
			{
				FarmerRenderer.accessoriesTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\accessories");
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000A224 File Offset: 0x00008424
		public void recolorEyes(Color lightestColor)
		{
			Color[] tmp = new Color[23];
			this.baseTexture.GetData<Color>(0, new Rectangle?(new Rectangle(256, 0, 23, 1)), tmp, 0, 23);
			Color darkerColor = FarmerRenderer.changeBrightness(lightestColor, -75);
			if (lightestColor.Equals(darkerColor))
			{
				lightestColor.B += 10;
			}
			for (int i = 256; i < tmp.Length; i++)
			{
				if (lightestColor.Equals(tmp[i]))
				{
					lightestColor.G += 1;
				}
				if (darkerColor.Equals(tmp[i]))
				{
					darkerColor.G += 1;
				}
			}
			ColorChanger.swapColor(this.baseTexture, 276, (int)lightestColor.R, (int)lightestColor.G, (int)lightestColor.B);
			ColorChanger.swapColor(this.baseTexture, 277, (int)darkerColor.R, (int)darkerColor.G, (int)darkerColor.B);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000A320 File Offset: 0x00008520
		public void recolorShoes(int which)
		{
			Texture2D shoeColors = Game1.content.Load<Texture2D>("Characters\\Farmer\\shoeColors");
			Color[] shoeColorsData = new Color[shoeColors.Width * shoeColors.Height];
			shoeColors.GetData<Color>(shoeColorsData);
			Color darkest = shoeColorsData[which * Game1.pixelZoom % (shoeColors.Height * Game1.pixelZoom)];
			Color medium = shoeColorsData[which * Game1.pixelZoom % (shoeColors.Height * Game1.pixelZoom) + 1];
			Color lightest = shoeColorsData[which * Game1.pixelZoom % (shoeColors.Height * Game1.pixelZoom) + 2];
			Color lightest2 = shoeColorsData[which * Game1.pixelZoom % (shoeColors.Height * Game1.pixelZoom) + 3];
			ColorChanger.swapColor(this.baseTexture, 268, (int)darkest.R, (int)darkest.G, (int)darkest.B);
			ColorChanger.swapColor(this.baseTexture, 269, (int)medium.R, (int)medium.G, (int)medium.B);
			ColorChanger.swapColor(this.baseTexture, 270, (int)lightest.R, (int)lightest.G, (int)lightest.B);
			ColorChanger.swapColor(this.baseTexture, 271, (int)lightest2.R, (int)lightest2.G, (int)lightest2.B);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000A464 File Offset: 0x00008664
		public int recolorSkin(int which)
		{
			Texture2D skinColors = Game1.content.Load<Texture2D>("Characters\\Farmer\\skinColors");
			Color[] skinColorsData = new Color[skinColors.Width * skinColors.Height];
			if (which < 0)
			{
				which = skinColors.Height - 1;
			}
			if (which > skinColors.Height - 1)
			{
				which = 0;
			}
			skinColors.GetData<Color>(skinColorsData);
			Color darkest = skinColorsData[which * 3 % (skinColors.Height * 3)];
			Color medium = skinColorsData[which * 3 % (skinColors.Height * 3) + 1];
			Color lightest = skinColorsData[which * 3 % (skinColors.Height * 3) + 2];
			ColorChanger.swapColor(this.baseTexture, 260, (int)darkest.R, (int)darkest.G, (int)darkest.B);
			ColorChanger.swapColor(this.baseTexture, 261, (int)medium.R, (int)medium.G, (int)medium.B);
			ColorChanger.swapColor(this.baseTexture, 262, (int)lightest.R, (int)lightest.G, (int)lightest.B);
			return which;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000A568 File Offset: 0x00008768
		public void changeShirt(int whichShirt)
		{
			Color[] shirtData = new Color[FarmerRenderer.shirtsTexture.Bounds.Width * FarmerRenderer.shirtsTexture.Bounds.Height];
			FarmerRenderer.shirtsTexture.GetData<Color>(shirtData);
			int index = whichShirt * 8 / FarmerRenderer.shirtsTexture.Bounds.Width * 32 * 128 + whichShirt * 8 % FarmerRenderer.shirtsTexture.Bounds.Width + FarmerRenderer.shirtsTexture.Width * Game1.pixelZoom;
			Color shirtSleeveColor = shirtData[index];
			ColorChanger.swapColor(this.baseTexture, 256, (int)shirtSleeveColor.R, (int)shirtSleeveColor.G, (int)shirtSleeveColor.B);
			shirtSleeveColor = shirtData[index - FarmerRenderer.shirtsTexture.Width];
			ColorChanger.swapColor(this.baseTexture, 257, (int)shirtSleeveColor.R, (int)shirtSleeveColor.G, (int)shirtSleeveColor.B);
			shirtSleeveColor = shirtData[index - FarmerRenderer.shirtsTexture.Width * 2];
			ColorChanger.swapColor(this.baseTexture, 258, (int)shirtSleeveColor.R, (int)shirtSleeveColor.G, (int)shirtSleeveColor.B);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000A68C File Offset: 0x0000888C
		public static Color changeBrightness(Color c, int brightness)
		{
			c.R = (byte)Math.Min(255, Math.Max(0, (int)c.R + brightness));
			c.G = (byte)Math.Min(255, Math.Max(0, (int)c.G + brightness));
			c.B = (byte)Math.Min(255, Math.Max(0, (int)c.B + ((brightness > 0) ? (brightness * 5 / 6) : (brightness * 8 / 7))));
			return c;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000A70C File Offset: 0x0000890C
		public void draw(SpriteBatch b, Farmer who, int whichFrame, Vector2 position, float layerDepth = 1f, bool flip = false)
		{
			who.FarmerSprite.setCurrentSingleFrame(whichFrame, 32000, false, flip);
			this.draw(b, who.FarmerSprite, who.FarmerSprite.SourceRect, position, Vector2.Zero, layerDepth, Color.White, 0f, who);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000A75C File Offset: 0x0000895C
		public void draw(SpriteBatch b, FarmerSprite farmerSprite, Rectangle sourceRect, Vector2 position, Vector2 origin, float layerDepth, Color overrideColor, float rotation, Farmer who)
		{
			this.draw(b, farmerSprite.CurrentAnimationFrame, farmerSprite.CurrentFrame, sourceRect, position, origin, layerDepth, overrideColor, rotation, 1f, who);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000A790 File Offset: 0x00008990
		public void drawMiniPortrat(SpriteBatch b, Vector2 position, float layerDepth, float scale, int facingDirection, Farmer who)
		{
			facingDirection = 2;
			bool flip = false;
			int yOffset = 0;
			position.Y += 0f;
			this.hairstyleSourceRect = Game1.getSquareSourceRectForNonStandardTileSheet(FarmerRenderer.hairStylesTexture, 16, 96, who.hair);
			this.hairstyleSourceRect.Height = 15;
			switch (facingDirection)
			{
			case 0:
				yOffset = 64;
				this.hairstyleSourceRect.Offset(0, 64);
				break;
			case 1:
				yOffset = 32;
				this.hairstyleSourceRect.Offset(0, 32);
				break;
			case 3:
				flip = true;
				yOffset = 32;
				this.hairstyleSourceRect.Offset(0, 32);
				break;
			}
			b.Draw(this.baseTexture, position, new Rectangle?(new Rectangle(0, yOffset, 16, who.isMale ? 15 : 16)), Color.White, 0f, Vector2.Zero, scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
			b.Draw(FarmerRenderer.hairStylesTexture, position + new Vector2(0f, 4f), new Rectangle?(this.hairstyleSourceRect), who.hairstyleColor, 0f, Vector2.Zero, scale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + 1.1E-07f);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000A8C0 File Offset: 0x00008AC0
		public void draw(SpriteBatch b, FarmerSprite.AnimationFrame animationFrame, int currentFrame, Rectangle sourceRect, Vector2 position, Vector2 origin, float layerDepth, Color overrideColor, float rotation, float scale, Farmer who)
		{
			this.draw(b, animationFrame, currentFrame, sourceRect, position, origin, layerDepth, who.facingDirection, overrideColor, rotation, scale, who);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000A8F0 File Offset: 0x00008AF0
		public void drawHairAndAccesories(SpriteBatch b, int facingDirection, Farmer who, Vector2 position, Vector2 origin, float scale, int currentFrame, float rotation, Color overrideColor, float layerDepth)
		{
			this.shirtSourceRect = new Rectangle(who.shirt * 8 % FarmerRenderer.shirtsTexture.Width, who.shirt * 8 / FarmerRenderer.shirtsTexture.Width * 32, 8, 8);
			this.hairstyleSourceRect = new Rectangle(who.getHair() * 16 % FarmerRenderer.hairStylesTexture.Width, who.getHair() * 16 / FarmerRenderer.hairStylesTexture.Width * 96, 16, 32);
			if (who.accessory >= 0)
			{
				this.accessorySourceRect = new Rectangle(who.accessory * 16 % FarmerRenderer.accessoriesTexture.Width, who.accessory * 16 / FarmerRenderer.accessoriesTexture.Width * 32, 16, 16);
			}
			if (who.hat != null)
			{
				this.hatSourceRect = new Rectangle(20 * who.hat.which % FarmerRenderer.hatsTexture.Width, 20 * who.hat.which / FarmerRenderer.hatsTexture.Width * 20 * Game1.pixelZoom, 20, 20);
			}
			switch (facingDirection)
			{
			case 0:
				this.shirtSourceRect.Offset(0, 24);
				this.hairstyleSourceRect.Offset(0, 64);
				if (who.hat != null)
				{
					this.hatSourceRect.Offset(0, 60);
				}
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + new Vector2(16f * scale + (float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(56 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom) + (float)this.heightOffset * scale), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.8E-07f);
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + 4 + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 2.2E-07f);
				break;
			case 1:
				this.shirtSourceRect.Offset(0, 8);
				this.hairstyleSourceRect.Offset(0, 32);
				if (who.accessory >= 0)
				{
					this.accessorySourceRect.Offset(0, 16);
				}
				if (who.hat != null)
				{
					this.hatSourceRect.Offset(0, 20);
				}
				if (rotation == -0.09817477f)
				{
					this.rotationAdjustment.X = 6f;
					this.rotationAdjustment.Y = -2f;
				}
				else if (rotation == 0.09817477f)
				{
					this.rotationAdjustment.X = -6f;
					this.rotationAdjustment.Y = 1f;
				}
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2(16f * scale + (float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), 56f * scale + (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom) + (float)this.heightOffset * scale), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + 1.8E-07f);
				}
				if (who.accessory >= 0)
				{
					b.Draw(FarmerRenderer.accessoriesTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(4 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset)), new Rectangle?(this.accessorySourceRect), (overrideColor.Equals(Color.White) && who.accessory < 6) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + ((who.accessory < 8) ? 1.9E-05f : 2.9E-05f));
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 2.2E-07f);
				break;
			case 2:
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + new Vector2((float)(16 + FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(56 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom) + (float)this.heightOffset * scale - (float)(who.isMale ? 0 : 0)), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.5E-07f);
				}
				if (who.accessory >= 0)
				{
					b.Draw(FarmerRenderer.accessoriesTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(8 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset - 4)), new Rectangle?(this.accessorySourceRect), (overrideColor.Equals(Color.White) && who.accessory < 6) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + ((who.accessory < 8) ? 1.9E-05f : 2.9E-05f));
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 2.2E-05f);
				break;
			case 3:
				this.shirtSourceRect.Offset(0, 16);
				if (who.accessory >= 0)
				{
					this.accessorySourceRect.Offset(0, 16);
				}
				this.hairstyleSourceRect.Offset(0, 32);
				if (who.hat != null)
				{
					this.hatSourceRect.Offset(0, 40);
				}
				if (rotation == -0.09817477f)
				{
					this.rotationAdjustment.X = 6f;
					this.rotationAdjustment.Y = -2f;
				}
				else if (rotation == 0.09817477f)
				{
					this.rotationAdjustment.X = -5f;
					this.rotationAdjustment.Y = 1f;
				}
				if (!who.bathingClothes)
				{
					b.Draw(FarmerRenderer.shirtsTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(16 - FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(56 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset)), new Rectangle?(this.shirtSourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.None, layerDepth + 1.5E-07f);
				}
				if (who.accessory >= 0)
				{
					b.Draw(FarmerRenderer.accessoriesTexture, position + origin + this.positionOffset + this.rotationAdjustment + new Vector2((float)(-(float)FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(4 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + this.heightOffset)), new Rectangle?(this.accessorySourceRect), (overrideColor.Equals(Color.White) && who.accessory < 6) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale + ((rotation != 0f) ? 0f : 0f), SpriteEffects.FlipHorizontally, layerDepth + ((who.accessory < 8) ? 1.9E-05f : 2.9E-05f));
				}
				b.Draw(FarmerRenderer.hairStylesTexture, position + origin + this.positionOffset + new Vector2((float)(-(float)FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.isMale && who.hair >= 16) ? -4 : ((!who.isMale && who.hair < 16) ? 4 : 0)))), new Rectangle?(this.hairstyleSourceRect), overrideColor.Equals(Color.White) ? who.hairstyleColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.FlipHorizontally, layerDepth + 2.2E-05f);
				break;
			}
			if (who.hat != null && !who.bathingClothes)
			{
				bool flip = who.FarmerSprite.CurrentAnimationFrame.flip;
				b.Draw(FarmerRenderer.hatsTexture, position + origin + this.positionOffset + new Vector2((float)(-(float)Game1.pixelZoom * 2 + (flip ? -1 : 1) * FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom), (float)(-(float)Game1.pixelZoom * 4 + FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + (who.hat.ignoreHairstyleOffset ? 0 : FarmerRenderer.hairstyleHatOffset[who.hair % 16]) + 4 + this.heightOffset)), new Rectangle?(this.hatSourceRect), Color.White, rotation, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 3.9E-05f);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000B40C File Offset: 0x0000960C
		public void draw(SpriteBatch b, FarmerSprite.AnimationFrame animationFrame, int currentFrame, Rectangle sourceRect, Vector2 position, Vector2 origin, float layerDepth, int facingDirection, Color overrideColor, float rotation, float scale, Farmer who)
		{
			position = new Vector2((float)Math.Floor((double)position.X), (float)Math.Floor((double)position.Y));
			this.rotationAdjustment = Vector2.Zero;
			this.positionOffset.Y = (float)(animationFrame.positionOffset * Game1.pixelZoom);
			this.positionOffset.X = (float)(animationFrame.xOffset * Game1.pixelZoom);
			if (who.swimming)
			{
				sourceRect.Height /= 2;
				sourceRect.Height -= (int)who.yOffset / Game1.pixelZoom;
				position.Y += (float)Game1.tileSize;
			}
			b.Draw(this.baseTexture, position + origin + this.positionOffset, new Rectangle?(sourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, animationFrame.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
			if (who.swimming)
			{
				if (who.currentEyes != 0 && who.facingDirection != 0 && Game1.timeOfDay < 2600 && (!who.FarmerSprite.pauseForSingleAnimation || (who.usingTool && who.CurrentTool is FishingRod)))
				{
					b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((who.FacingDirection == 1) ? (3 * Game1.pixelZoom) : ((who.FacingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + (who.IsMale ? (9 * Game1.pixelZoom) : (10 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(5, 16, (who.FacingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 5E-08f);
					b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((who.FacingDirection == 1) ? (3 * Game1.pixelZoom) : ((who.FacingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + (who.IsMale ? (9 * Game1.pixelZoom) : (10 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(264 + ((who.FacingDirection == 3) ? 4 : 0), 2 + (who.currentEyes - 1) * 2, (who.FacingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.2E-07f);
				}
				this.drawHairAndAccesories(b, facingDirection, who, position, origin, scale, currentFrame, rotation, overrideColor, layerDepth);
				b.Draw(Game1.staminaRect, new Rectangle((int)position.X + (int)who.yOffset + Game1.pixelZoom * 2, (int)position.Y - 32 * Game1.pixelZoom + sourceRect.Height * Game1.pixelZoom + (int)origin.Y - (int)who.yOffset, sourceRect.Width * Game1.pixelZoom - (int)who.yOffset * 2 - Game1.pixelZoom * Game1.pixelZoom, Game1.pixelZoom), new Rectangle?(Game1.staminaRect.Bounds), Color.White * 0.75f, 0f, Vector2.Zero, SpriteEffects.None, layerDepth + 0.001f);
				return;
			}
			sourceRect.Offset(288, 0);
			b.Draw(this.baseTexture, position + origin + this.positionOffset, new Rectangle?(sourceRect), overrideColor.Equals(Color.White) ? who.pantsColor : overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, animationFrame.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + ((who.FarmerSprite.CurrentAnimationFrame.frame == 5) ? 0.00092f : 9.2E-08f));
			if (who.currentEyes != 0 && facingDirection != 0 && !who.isRidingHorse() && Game1.timeOfDay < 2600 && (!who.FarmerSprite.pauseForSingleAnimation || (who.usingTool && who.CurrentTool is FishingRod)))
			{
				b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((facingDirection == 1) ? (3 * Game1.pixelZoom) : ((facingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.IsMale && who.facingDirection != 2) ? (9 * Game1.pixelZoom) : (10 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(5, 16, (facingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 5E-08f);
				b.Draw(this.baseTexture, position + origin + this.positionOffset + new Vector2((float)(FarmerRenderer.featureXOffsetPerFrame[currentFrame] * Game1.pixelZoom + 5 * Game1.pixelZoom + ((facingDirection == 1) ? (3 * Game1.pixelZoom) : ((facingDirection == 3) ? Game1.pixelZoom : 0))), (float)(FarmerRenderer.featureYOffsetPerFrame[currentFrame] * Game1.pixelZoom + ((who.facingDirection == 1 || who.facingDirection == 3) ? (10 * Game1.pixelZoom) : (11 * Game1.pixelZoom)))), new Rectangle?(new Rectangle(264 + ((facingDirection == 3) ? 4 : 0), 2 + (who.currentEyes - 1) * 2, (facingDirection == 2) ? 6 : 2, 2)), overrideColor, 0f, origin, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + 1.2E-07f);
			}
			this.drawHairAndAccesories(b, facingDirection, who, position, origin, scale, currentFrame, rotation, overrideColor, layerDepth);
			sourceRect.Offset(-288 + (animationFrame.secondaryArm ? 192 : 96), 0);
			b.Draw(this.baseTexture, position + origin + this.positionOffset + who.armOffset, new Rectangle?(sourceRect), overrideColor, rotation, origin, (float)Game1.pixelZoom * scale, animationFrame.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 4.9E-05f : 0f));
			if (who.usingSlingshot)
			{
				int mouseX = Game1.getOldMouseX() + Game1.viewport.X;
				int mouseY = Game1.getOldMouseY() + Game1.viewport.Y;
				if ((who.CurrentTool as Slingshot).didStartWithGamePad())
				{
					Point expr_770 = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					mouseX = expr_770.X;
					mouseY = expr_770.Y;
				}
				int backArmDistance = Math.Min(20, (int)Vector2.Distance(who.getStandingPosition(), new Vector2((float)mouseX, (float)mouseY)) / 20);
				float frontArmRotation = (float)Math.Atan2((double)((float)mouseY - who.getStandingPosition().Y - (float)Game1.tileSize), (double)((float)mouseX - who.getStandingPosition().X)) + 3.14159274f;
				switch (facingDirection)
				{
				case 0:
					b.Draw(this.baseTexture, position + new Vector2(4f + frontArmRotation * 8f, (float)(-(float)Game1.tileSize * 3 / 4 + 4)), new Rectangle?(new Rectangle(173, 238, 9, 14)), Color.White, 0f, new Vector2(4f, 11f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : -0.0005f));
					return;
				case 1:
				{
					b.Draw(this.baseTexture, position + new Vector2((float)(52 - backArmDistance), (float)(-(float)Game1.tileSize / 2)), new Rectangle?(new Rectangle(147, 237, 10, 4)), Color.White, 0f, new Vector2(8f, 3f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					b.Draw(this.baseTexture, position + new Vector2(36f, (float)(-(float)Game1.tileSize / 2 - 12)), new Rectangle?(new Rectangle(156, 244, 9, 10)), Color.White, frontArmRotation, new Vector2(0f, 3f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 1E-08f : 0f));
					int slingshotAttachX = (int)(Math.Cos((double)(frontArmRotation + 1.57079637f)) * (double)(20 - backArmDistance - 8) - Math.Sin((double)(frontArmRotation + 1.57079637f)) * -68.0);
					int slingshotAttachY = (int)(Math.Sin((double)(frontArmRotation + 1.57079637f)) * (double)(20 - backArmDistance - 8) + Math.Cos((double)(frontArmRotation + 1.57079637f)) * -68.0);
					Utility.drawLineWithScreenCoordinates((int)(position.X + 52f - (float)backArmDistance), (int)(position.Y - (float)(Game1.tileSize / 2) - 4f), (int)(position.X + 32f + (float)(slingshotAttachX / 2)), (int)(position.Y - (float)(Game1.tileSize / 2) - 12f + (float)(slingshotAttachY / 2)), b, Color.White, 1f);
					return;
				}
				case 2:
					b.Draw(this.baseTexture, position + new Vector2(4f, (float)(-(float)Game1.tileSize / 2 - backArmDistance / 2)), new Rectangle?(new Rectangle(148, 244, 4, 4)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					Utility.drawLineWithScreenCoordinates((int)(position.X + 16f), (int)(position.Y - 28f - (float)(backArmDistance / 2)), (int)(position.X + 44f - frontArmRotation * 10f), (int)(position.Y - (float)(Game1.tileSize / 4) - 8f), b, Color.White, 1f);
					Utility.drawLineWithScreenCoordinates((int)(position.X + 16f), (int)(position.Y - 28f - (float)(backArmDistance / 2)), (int)(position.X + 56f - frontArmRotation * 10f), (int)(position.Y - (float)(Game1.tileSize / 4) - 8f), b, Color.White, 1f);
					b.Draw(this.baseTexture, position + new Vector2(44f - frontArmRotation * 10f, (float)(-(float)Game1.tileSize / 4)), new Rectangle?(new Rectangle(167, 235, 7, 9)), Color.White, 0f, new Vector2(3f, 5f), (float)Game1.pixelZoom * scale, SpriteEffects.None, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					break;
				case 3:
				{
					b.Draw(this.baseTexture, position + new Vector2((float)(40 + backArmDistance), (float)(-(float)Game1.tileSize / 2)), new Rectangle?(new Rectangle(147, 237, 10, 4)), Color.White, 0f, new Vector2(9f, 4f), (float)Game1.pixelZoom * scale, SpriteEffects.FlipHorizontally, layerDepth + ((facingDirection != 0) ? 5.9E-05f : 0f));
					b.Draw(this.baseTexture, position + new Vector2(24f, (float)(-(float)Game1.tileSize / 2 - 8)), new Rectangle?(new Rectangle(156, 244, 9, 10)), Color.White, frontArmRotation + 3.14159274f, new Vector2(8f, 3f), (float)Game1.pixelZoom * scale, SpriteEffects.FlipHorizontally, layerDepth + ((facingDirection != 0) ? 1E-08f : 0f));
					int slingshotAttachX = (int)(Math.Cos((double)(frontArmRotation + 1.2566371f)) * (double)(20 + backArmDistance - 8) - Math.Sin((double)(frontArmRotation + 1.2566371f)) * -68.0);
					int slingshotAttachY = (int)(Math.Sin((double)(frontArmRotation + 1.2566371f)) * (double)(20 + backArmDistance - 8) + Math.Cos((double)(frontArmRotation + 1.2566371f)) * -68.0);
					Utility.drawLineWithScreenCoordinates((int)(position.X + 4f + (float)backArmDistance), (int)(position.Y - (float)(Game1.tileSize / 2) - 8f), (int)(position.X + 26f + (float)slingshotAttachX * 4f / 10f), (int)(position.Y - (float)(Game1.tileSize / 2) - 8f + (float)slingshotAttachY * 4f / 10f), b, Color.White, 1f);
					return;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x0400010E RID: 270
		public const int sleeveDarkestColorIndex = 256;

		// Token: 0x0400010F RID: 271
		public const int skinDarkestColorIndex = 260;

		// Token: 0x04000110 RID: 272
		public const int shoeDarkestColorIndex = 268;

		// Token: 0x04000111 RID: 273
		public const int eyeLightestColorIndex = 276;

		// Token: 0x04000112 RID: 274
		public const int accessoryDrawBelowHairThreshold = 8;

		// Token: 0x04000113 RID: 275
		public const int accessoryFacialHairThreshold = 6;

		// Token: 0x04000114 RID: 276
		public const int pantsOffset = 288;

		// Token: 0x04000115 RID: 277
		public const int armOffset = 96;

		// Token: 0x04000116 RID: 278
		public const int secondaryArmOffset = 192;

		// Token: 0x04000117 RID: 279
		public const int shirtXOffset = 16;

		// Token: 0x04000118 RID: 280
		public const int shirtYOffset = 56;

		// Token: 0x04000119 RID: 281
		public static int[] featureYOffsetPerFrame = new int[]
		{
			1,
			2,
			2,
			0,
			5,
			6,
			1,
			2,
			2,
			1,
			0,
			2,
			0,
			1,
			1,
			0,
			1,
			2,
			3,
			3,
			2,
			2,
			1,
			1,
			0,
			0,
			2,
			2,
			4,
			4,
			0,
			0,
			1,
			2,
			1,
			1,
			1,
			1,
			0,
			0,
			1,
			1,
			1,
			0,
			0,
			-2,
			-1,
			1,
			1,
			0,
			-1,
			-2,
			-1,
			-1,
			5,
			4,
			0,
			0,
			3,
			2,
			-1,
			0,
			4,
			2,
			0,
			0,
			2,
			1,
			0,
			-1,
			1,
			-2,
			0,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			0,
			0,
			0,
			1,
			-1,
			-1,
			-1,
			-1,
			1,
			1,
			0,
			0,
			0,
			0,
			4,
			1,
			0,
			1,
			2,
			1,
			0,
			1,
			0,
			1,
			2,
			-3,
			-4,
			-1,
			0,
			0,
			2,
			1,
			-4,
			-1,
			0,
			0,
			-3,
			0,
			0,
			-1,
			0,
			0,
			2,
			1,
			1
		};

		// Token: 0x0400011A RID: 282
		public static int[] featureXOffsetPerFrame = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			4,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			-1,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		// Token: 0x0400011B RID: 283
		public static int[] hairstyleHatOffset = new int[]
		{
			0,
			0,
			0,
			4,
			0,
			0,
			3,
			0,
			4,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		// Token: 0x0400011C RID: 284
		public Texture2D baseTexture;

		// Token: 0x0400011D RID: 285
		public static Texture2D hairStylesTexture;

		// Token: 0x0400011E RID: 286
		public static Texture2D shirtsTexture;

		// Token: 0x0400011F RID: 287
		public static Texture2D hatsTexture;

		// Token: 0x04000120 RID: 288
		public static Texture2D accessoriesTexture;

		// Token: 0x04000121 RID: 289
		public int heightOffset;

		// Token: 0x04000122 RID: 290
		private Rectangle shirtSourceRect;

		// Token: 0x04000123 RID: 291
		private Rectangle hairstyleSourceRect;

		// Token: 0x04000124 RID: 292
		private Rectangle hatSourceRect;

		// Token: 0x04000125 RID: 293
		private Rectangle accessorySourceRect;

		// Token: 0x04000126 RID: 294
		private Vector2 rotationAdjustment;

		// Token: 0x04000127 RID: 295
		private Vector2 positionOffset;
	}
}
