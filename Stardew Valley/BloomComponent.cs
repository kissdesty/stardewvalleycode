using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200000B RID: 11
	public class BloomComponent : DrawableGameComponent
	{
		// Token: 0x17000005 RID: 5
		public BloomSettings Settings
		{
			// Token: 0x0600006D RID: 109 RVA: 0x000056E9 File Offset: 0x000038E9
			get
			{
				return this.settings;
			}
			// Token: 0x0600006E RID: 110 RVA: 0x000056F1 File Offset: 0x000038F1
			set
			{
				this.settings = value;
			}
		}

		// Token: 0x17000006 RID: 6
		public BloomComponent.IntermediateBuffer ShowBuffer
		{
			// Token: 0x0600006F RID: 111 RVA: 0x000056FA File Offset: 0x000038FA
			get
			{
				return this.showBuffer;
			}
			// Token: 0x06000070 RID: 112 RVA: 0x00005702 File Offset: 0x00003902
			set
			{
				this.showBuffer = value;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000570C File Offset: 0x0000390C
		public BloomComponent(Game game) : base(game)
		{
			if (game == null)
			{
				throw new ArgumentNullException("game");
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000575C File Offset: 0x0000395C
		public void startShifting(float howLongMilliseconds, float shiftRate, float shiftFade, float globalIntensityMax, float blurShiftLevel, float saturationShiftLevel, float contrastShiftLevel, float bloomIntensityShift, float brightnessShift, float globalIntensityStart = 1f, float offsetShift = 3000f, bool cyclingShift = true)
		{
			this.timeLeftForShifting = howLongMilliseconds;
			this.totalTime = howLongMilliseconds;
			this.shiftRate = shiftRate;
			this.blurLevel = blurShiftLevel;
			this.saturationLevel = saturationShiftLevel;
			this.contrastLevel = contrastShiftLevel;
			this.bloomLevel = bloomIntensityShift;
			this.brightnessLevel = brightnessShift;
			base.Visible = true;
			this.oldSetting = new BloomSettings("old", this.settings.BloomThreshold, this.settings.BlurAmount, this.settings.BloomIntensity, this.settings.BaseIntensity, this.settings.BloomSaturation, this.settings.BaseSaturation, false);
			this.targetSettings = new BloomSettings("old", this.settings.BloomThreshold, this.settings.BlurAmount, this.settings.BloomIntensity, this.settings.BaseIntensity, this.settings.BloomSaturation, this.settings.BaseSaturation, false);
			this.cyclingShift = cyclingShift;
			this.shiftFade = shiftFade;
			this.globalIntensity = globalIntensityStart;
			this.globalIntensityMax = globalIntensityMax / 2f;
			this.offsetShift = offsetShift;
			Game1.debugOutput = string.Concat(new object[]
			{
				howLongMilliseconds,
				" ",
				shiftRate,
				" ",
				shiftFade,
				" ",
				globalIntensityMax,
				" ",
				blurShiftLevel,
				" ",
				saturationShiftLevel,
				" ",
				contrastShiftLevel,
				" ",
				bloomIntensityShift,
				" ",
				brightnessShift,
				" ",
				globalIntensityStart,
				" ",
				offsetShift
			});
			this.hueShiftR = 0f;
			this.hueShiftB = 0f;
			this.hueShiftG = 0f;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000597C File Offset: 0x00003B7C
		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			this.bloomExtractEffect = base.Game.Content.Load<Effect>("BloomExtract");
			this.bloomCombineEffect = base.Game.Content.Load<Effect>("BloomCombine");
			this.gaussianBlurEffect = base.Game.Content.Load<Effect>("GaussianBlur");
			this.brightWhiteEffect = base.Game.Content.Load<Effect>("BrightWhite");
			PresentationParameters pp = base.GraphicsDevice.PresentationParameters;
			int width = pp.BackBufferWidth;
			int height = pp.BackBufferHeight;
			SurfaceFormat format = pp.BackBufferFormat;
			this.sceneRenderTarget = new RenderTarget2D(base.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
			width /= 2;
			height /= 2;
			this.renderTarget1 = new RenderTarget2D(base.GraphicsDevice, width, height, false, format, DepthFormat.None);
			this.renderTarget2 = new RenderTarget2D(base.GraphicsDevice, width, height, false, format, DepthFormat.None);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005A80 File Offset: 0x00003C80
		public void reload()
		{
			PresentationParameters pp = base.GraphicsDevice.PresentationParameters;
			int width = pp.BackBufferWidth;
			int height = pp.BackBufferHeight;
			SurfaceFormat format = pp.BackBufferFormat;
			this.sceneRenderTarget = new RenderTarget2D(base.GraphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
			width /= 2;
			height /= 2;
			this.renderTarget1 = new RenderTarget2D(base.GraphicsDevice, width, height, false, format, DepthFormat.None);
			this.renderTarget2 = new RenderTarget2D(base.GraphicsDevice, width, height, false, format, DepthFormat.None);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005B04 File Offset: 0x00003D04
		protected override void UnloadContent()
		{
			this.sceneRenderTarget.Dispose();
			this.renderTarget1.Dispose();
			this.renderTarget2.Dispose();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005B27 File Offset: 0x00003D27
		public void BeginDraw()
		{
			if (base.Visible)
			{
				base.GraphicsDevice.SetRenderTarget(this.sceneRenderTarget);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005B44 File Offset: 0x00003D44
		public void tick(GameTime time)
		{
			if (this.timeLeftForShifting > 0f)
			{
				base.Visible = true;
				this.timeLeftForShifting -= (float)time.ElapsedGameTime.Milliseconds;
				this.shiftRate = Math.Max(0.0001f, this.shiftRate + this.shiftFade * (float)time.ElapsedGameTime.Milliseconds);
				if (this.cyclingShift)
				{
					this.offsetShift += (float)time.ElapsedGameTime.Milliseconds / 10f;
					this.globalIntensity = this.globalIntensityMax * (float)Math.Cos(((double)this.timeLeftForShifting - (double)this.totalTime * 3.1415926535897931 * 4.0) * (6.2831853071795862 / (double)this.totalTime)) + this.globalIntensityMax;
					float offset = this.offsetShift * (float)Math.Sin((double)this.timeLeftForShifting * 6.2831853071795862 / (double)this.totalTime);
					this.targetSettings.BaseSaturation = Math.Max(1f, 0.25f * this.globalIntensity * (this.saturationLevel * (float)Math.Sin((double)(this.timeLeftForShifting - offset / 2f) * 6.2831853071795862 / (double)this.shiftRate) + (0.25f * this.globalIntensity + this.saturationLevel)));
					this.targetSettings.BloomIntensity = Math.Max(0f, 0.5f * this.globalIntensity * (this.bloomLevel / 2f * (float)Math.Sin((double)(this.timeLeftForShifting - offset * 2f) * 6.2831853071795862 / (double)this.shiftRate) + (0.5f * this.globalIntensity + this.bloomLevel / 2f)));
					this.targetSettings.BlurAmount = Math.Max(0f, 1f * this.globalIntensity * (this.blurLevel * (float)Math.Sin((double)this.timeLeftForShifting * 6.2831853071795862 / (double)(this.shiftRate / 2f))) + (1f * this.globalIntensity + this.blurLevel));
					this.settings.BaseSaturation += (this.targetSettings.BaseSaturation - this.settings.BaseSaturation) / 10f;
					this.settings.BloomIntensity += (this.targetSettings.BloomIntensity - this.settings.BloomIntensity) / 10f;
					this.settings.BaseIntensity += (this.targetSettings.BaseIntensity - this.settings.BaseIntensity) / 10f;
					this.settings.BlurAmount += (this.targetSettings.BaseSaturation - this.settings.BlurAmount) / 10f;
					this.hueShiftR = this.globalIntensity / 2f * (float)(Math.Cos((double)(this.timeLeftForShifting - offset / 2f) * 6.2831853071795862 / (double)(this.shiftRate / 2f)) + 1.0) / 4f;
					this.hueShiftG = this.globalIntensity / 2f * (float)(Math.Sin((double)(this.timeLeftForShifting - offset / 2f) * 6.2831853071795862 / (double)(this.shiftRate / 2f)) + 1.0) / 4f;
					this.hueShiftB = this.globalIntensity / 2f * (float)(Math.Cos((double)(this.timeLeftForShifting - offset / 2f - this.totalTime / 2f) * 6.2831853071795862 / (double)this.shiftRate) + 1.0) / 4f;
					this.rabbitHoleTimer -= (float)time.ElapsedGameTime.Milliseconds;
					if (this.rabbitHoleTimer <= 0f)
					{
						this.rabbitHoleTimer = 1000f;
						Console.WriteLine(string.Concat(new object[]
						{
							"timeLeft: ",
							this.timeLeftForShifting,
							" shiftRate: ",
							this.shiftRate,
							" globalIntensity: ",
							this.globalIntensity,
							" settings.BloomThreshold: ",
							this.settings.BloomThreshold,
							" settings.BaseSaturation: ",
							this.settings.BaseSaturation,
							" settings.BloomIntensity: ",
							this.settings.BloomIntensity,
							" settings.BaseIntensity: ",
							this.settings.BaseIntensity,
							" settings.BlurAmount: ",
							this.settings.BlurAmount,
							" hueShift: ",
							this.hueShiftR,
							",",
							this.hueShiftG,
							",",
							this.hueShiftB,
							" x,y: "
						}));
					}
				}
				if (this.timeLeftForShifting <= 0f)
				{
					this.hueShiftR = 0f;
					this.hueShiftG = 0f;
					this.hueShiftB = 0f;
					this.settings = this.oldSetting;
					if (Game1.bloomDay && Game1.currentLocation.isOutdoors)
					{
						base.Visible = true;
						return;
					}
					base.Visible = false;
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006104 File Offset: 0x00004304
		public override void Draw(GameTime gameTime)
		{
			if (this.settings == null)
			{
				return;
			}
			base.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			if (this.settings.brightWhiteOnly)
			{
				this.DrawFullscreenQuad(this.sceneRenderTarget, this.renderTarget1, this.brightWhiteEffect, BloomComponent.IntermediateBuffer.PreBloom);
			}
			else
			{
				this.bloomExtractEffect.Parameters["BloomThreshold"].SetValue(this.Settings.BloomThreshold);
				this.DrawFullscreenQuad(this.sceneRenderTarget, this.renderTarget1, this.bloomExtractEffect, BloomComponent.IntermediateBuffer.PreBloom);
			}
			this.SetBlurEffectParameters(1f / (float)this.renderTarget1.Width, 0f);
			this.DrawFullscreenQuad(this.renderTarget1, this.renderTarget2, this.gaussianBlurEffect, BloomComponent.IntermediateBuffer.BlurredHorizontally);
			this.SetBlurEffectParameters(0f, 1f / (float)this.renderTarget1.Height);
			this.DrawFullscreenQuad(this.renderTarget2, this.renderTarget1, this.gaussianBlurEffect, BloomComponent.IntermediateBuffer.BlurredBothWays);
			base.GraphicsDevice.SetRenderTarget(null);
			EffectParameterCollection expr_108 = this.bloomCombineEffect.Parameters;
			expr_108["BloomIntensity"].SetValue(this.Settings.BloomIntensity);
			expr_108["BaseIntensity"].SetValue(this.Settings.BaseIntensity);
			expr_108["BloomSaturation"].SetValue(this.Settings.BloomSaturation);
			expr_108["BaseSaturation"].SetValue(this.Settings.BaseSaturation);
			expr_108["HueR"].SetValue((float)Math.Round((double)this.hueShiftR, 2));
			expr_108["HueG"].SetValue((float)Math.Round((double)this.hueShiftG, 2));
			expr_108["HueB"].SetValue((float)Math.Round((double)this.hueShiftB, 2));
			base.GraphicsDevice.Textures[1] = this.sceneRenderTarget;
			Viewport viewport = base.GraphicsDevice.Viewport;
			this.DrawFullscreenQuad(this.renderTarget1, viewport.Width, viewport.Height, this.bloomCombineEffect, BloomComponent.IntermediateBuffer.FinalResult);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006322 File Offset: 0x00004522
		private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect, BloomComponent.IntermediateBuffer currentBuffer)
		{
			base.GraphicsDevice.SetRenderTarget(renderTarget);
			this.DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect, currentBuffer);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006348 File Offset: 0x00004548
		private void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect, BloomComponent.IntermediateBuffer currentBuffer)
		{
			if (this.showBuffer < currentBuffer)
			{
				effect = null;
			}
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
			this.spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
			this.spriteBatch.End();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000063A0 File Offset: 0x000045A0
		private void SetBlurEffectParameters(float dx, float dy)
		{
			EffectParameter weightsParameter = this.gaussianBlurEffect.Parameters["SampleWeights"];
			EffectParameter offsetsParameter = this.gaussianBlurEffect.Parameters["SampleOffsets"];
			int sampleCount = weightsParameter.Elements.Count;
			float[] sampleWeights = new float[sampleCount];
			Vector2[] sampleOffsets = new Vector2[sampleCount];
			sampleWeights[0] = this.ComputeGaussian(0f);
			sampleOffsets[0] = new Vector2(0f);
			float totalWeights = sampleWeights[0];
			for (int i = 0; i < sampleCount / 2; i++)
			{
				float weight = this.ComputeGaussian((float)(i + 1));
				sampleWeights[i * 2 + 1] = weight;
				sampleWeights[i * 2 + 2] = weight;
				totalWeights += weight * 2f;
				float sampleOffset = (float)(i * 2) + 1.5f;
				Vector2 delta = new Vector2(dx, dy) * sampleOffset;
				sampleOffsets[i * 2 + 1] = delta;
				sampleOffsets[i * 2 + 2] = -delta;
			}
			for (int j = 0; j < sampleWeights.Length; j++)
			{
				sampleWeights[j] /= totalWeights;
			}
			weightsParameter.SetValue(sampleWeights);
			offsetsParameter.SetValue(sampleOffsets);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000064C8 File Offset: 0x000046C8
		private float ComputeGaussian(float n)
		{
			float theta = this.Settings.BlurAmount;
			return (float)(1.0 / Math.Sqrt(6.2831853071795862 * (double)theta) * Math.Exp((double)(-(double)(n * n) / (2f * theta * theta))));
		}

		// Token: 0x0400004D RID: 77
		private SpriteBatch spriteBatch;

		// Token: 0x0400004E RID: 78
		private Effect bloomExtractEffect;

		// Token: 0x0400004F RID: 79
		private Effect brightWhiteEffect;

		// Token: 0x04000050 RID: 80
		private Effect bloomCombineEffect;

		// Token: 0x04000051 RID: 81
		private Effect gaussianBlurEffect;

		// Token: 0x04000052 RID: 82
		private RenderTarget2D sceneRenderTarget;

		// Token: 0x04000053 RID: 83
		private RenderTarget2D renderTarget1;

		// Token: 0x04000054 RID: 84
		private RenderTarget2D renderTarget2;

		// Token: 0x04000055 RID: 85
		public float hueShiftR;

		// Token: 0x04000056 RID: 86
		public float hueShiftG;

		// Token: 0x04000057 RID: 87
		public float hueShiftB;

		// Token: 0x04000058 RID: 88
		public float timeLeftForShifting;

		// Token: 0x04000059 RID: 89
		public float totalTime;

		// Token: 0x0400005A RID: 90
		public float shiftRate;

		// Token: 0x0400005B RID: 91
		public float offsetShift;

		// Token: 0x0400005C RID: 92
		public float shiftFade;

		// Token: 0x0400005D RID: 93
		public float blurLevel;

		// Token: 0x0400005E RID: 94
		public float saturationLevel;

		// Token: 0x0400005F RID: 95
		public float contrastLevel;

		// Token: 0x04000060 RID: 96
		public float bloomLevel;

		// Token: 0x04000061 RID: 97
		public float brightnessLevel;

		// Token: 0x04000062 RID: 98
		public float globalIntensity;

		// Token: 0x04000063 RID: 99
		public float globalIntensityMax;

		// Token: 0x04000064 RID: 100
		public float rabbitHoleTimer;

		// Token: 0x04000065 RID: 101
		private bool cyclingShift;

		// Token: 0x04000066 RID: 102
		private BloomSettings settings = BloomSettings.PresetSettings[5];

		// Token: 0x04000067 RID: 103
		private BloomSettings targetSettings = BloomSettings.PresetSettings[5];

		// Token: 0x04000068 RID: 104
		private BloomSettings oldSetting = BloomSettings.PresetSettings[5];

		// Token: 0x04000069 RID: 105
		private BloomComponent.IntermediateBuffer showBuffer = BloomComponent.IntermediateBuffer.FinalResult;

		// Token: 0x02000164 RID: 356
		public enum IntermediateBuffer
		{
			// Token: 0x04001433 RID: 5171
			PreBloom,
			// Token: 0x04001434 RID: 5172
			BlurredHorizontally,
			// Token: 0x04001435 RID: 5173
			BlurredBothWays,
			// Token: 0x04001436 RID: 5174
			FinalResult
		}
	}
}
