using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace StardewValley.Menus
{
	// Token: 0x020000F8 RID: 248
	public class Bundle : ClickableComponent
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x001328B0 File Offset: 0x00130AB0
		public Bundle(int bundleIndex, string rawBundleInfo, bool[] completedIngredientsList, Point position, Texture2D texture, JunimoNoteMenu menu) : base(new Rectangle(position.X, position.Y, Game1.tileSize, Game1.tileSize), "")
		{
			if (menu.fromGameMenu)
			{
				this.depositsAllowed = false;
			}
			this.bundleIndex = bundleIndex;
			string[] split = rawBundleInfo.Split(new char[]
			{
				'/'
			});
			this.name = split[0];
			this.label = split[0];
			this.rewardDescription = split[1];
			string[] ingredientsSplit = split[2].Split(new char[]
			{
				' '
			});
			this.complete = true;
			this.ingredients = new List<BundleIngredientDescription>();
			int tally = 0;
			for (int i = 0; i < ingredientsSplit.Length; i += 3)
			{
				this.ingredients.Add(new BundleIngredientDescription(Convert.ToInt32(ingredientsSplit[i]), Convert.ToInt32(ingredientsSplit[i + 1]), Convert.ToInt32(ingredientsSplit[i + 2]), completedIngredientsList[i / 3]));
				if (!completedIngredientsList[i / 3])
				{
					this.complete = false;
				}
				else
				{
					tally++;
				}
			}
			this.bundleColor = Convert.ToInt32(split[3]);
			this.numberOfIngredientSlots = ((split.Length > 4) ? Convert.ToInt32(split[4]) : this.ingredients.Count);
			if (tally >= this.numberOfIngredientSlots)
			{
				this.complete = true;
			}
			this.sprite = new TemporaryAnimatedSprite(texture, new Rectangle(this.bundleColor * 256 % 512, 244 + this.bundleColor * 256 / 512 * 16, 16, 16), 70f, 3, 99999, new Vector2((float)this.bounds.X, (float)this.bounds.Y), false, false, 0.8f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
			{
				pingPong = true
			};
			this.sprite.paused = true;
			TemporaryAnimatedSprite expr_1E2_cp_0_cp_0 = this.sprite;
			expr_1E2_cp_0_cp_0.sourceRect.X = expr_1E2_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width;
			if (this.name.ToLower().Contains(Game1.currentSeason) && !this.complete)
			{
				this.shake(0.07363108f);
			}
			if (this.complete)
			{
				this.completionAnimation(menu, false, 0);
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00132AEF File Offset: 0x00130CEF
		public Item getReward()
		{
			return Utility.getItemFromStandardTextDescription(this.rewardDescription, Game1.player, ' ');
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00132B03 File Offset: 0x00130D03
		public void shake(float force = 0.07363108f)
		{
			if (this.sprite.paused)
			{
				this.maxShake = force;
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00132B1C File Offset: 0x00130D1C
		public void shake(int extraInfo)
		{
			this.maxShake = 0.07363108f;
			if (extraInfo == 1)
			{
				Game1.playSound("leafrustle");
				JunimoNoteMenu.tempSprites.Add(new TemporaryAnimatedSprite(50, this.sprite.position, Bundle.getColorFromColorIndex(this.bundleColor), 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					motion = new Vector2(-1f, 0.5f),
					acceleration = new Vector2(0f, 0.02f)
				});
				JunimoNoteMenu.tempSprites.Add(new TemporaryAnimatedSprite(50, this.sprite.position, Bundle.getColorFromColorIndex(this.bundleColor), 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					motion = new Vector2(1f, 0.5f),
					acceleration = new Vector2(0f, 0.02f),
					flipped = true,
					delayBeforeAnimationStart = 50
				});
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00132C16 File Offset: 0x00130E16
		public void shakeAndAllowClicking(int extraInfo)
		{
			this.maxShake = 0.07363108f;
			JunimoNoteMenu.canClick = true;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00132C2C File Offset: 0x00130E2C
		public void tryHoverAction(int x, int y)
		{
			if (this.bounds.Contains(x, y) && !this.complete)
			{
				this.sprite.paused = false;
				JunimoNoteMenu.hoverText = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
				{
					this.label
				});
				return;
			}
			if (!this.complete)
			{
				this.sprite.reset();
				TemporaryAnimatedSprite expr_6A_cp_0_cp_0 = this.sprite;
				expr_6A_cp_0_cp_0.sourceRect.X = expr_6A_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width;
				this.sprite.paused = true;
			}
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00132CC4 File Offset: 0x00130EC4
		public bool canAcceptThisItem(Item item, ClickableTextureComponent slot)
		{
			if (!this.depositsAllowed)
			{
				return false;
			}
			if (item is Object)
			{
				Object o = item as Object;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (!this.ingredients[i].completed && this.ingredients[i].index == item.parentSheetIndex && this.ingredients[i].stack <= item.Stack && this.ingredients[i].quality <= o.quality && slot.item == null)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00132D70 File Offset: 0x00130F70
		public Item tryToDepositThisItem(Item item, ClickableTextureComponent slot, Texture2D noteTexture)
		{
			if (!this.depositsAllowed)
			{
				Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:JunimoNote_MustBeAtCC", new object[0]));
				return item;
			}
			if (!(item is Object))
			{
				return item;
			}
			Object o = item as Object;
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (!this.ingredients[i].completed && this.ingredients[i].index == item.parentSheetIndex && item.Stack >= this.ingredients[i].stack && o.quality >= this.ingredients[i].quality && slot.item == null)
				{
					item.Stack -= this.ingredients[i].stack;
					this.ingredients[i] = new BundleIngredientDescription(this.ingredients[i].index, this.ingredients[i].stack, this.ingredients[i].quality, true);
					this.ingredientDepositAnimation(slot, noteTexture, false);
					slot.item = new Object(this.ingredients[i].index, this.ingredients[i].stack, false, -1, this.ingredients[i].quality);
					Game1.playSound("newArtifact");
					(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[this.bundleIndex][i] = true;
					slot.sourceRect.X = 512;
					slot.sourceRect.Y = 244;
					break;
				}
			}
			if (item.Stack > 0)
			{
				return item;
			}
			return null;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00132F4C File Offset: 0x0013114C
		public void ingredientDepositAnimation(ClickableTextureComponent slot, Texture2D noteTexture, bool skipAnimation = false)
		{
			TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(noteTexture, new Rectangle(530, 244, 18, 18), 50f, 6, 1, new Vector2((float)slot.bounds.X, (float)slot.bounds.Y), false, false, 0.88f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
			{
				holdLastFrame = true,
				endSound = "cowboy_monsterhit"
			};
			if (skipAnimation)
			{
				t.sourceRect.Offset(t.sourceRect.Width * 5, 0);
				t.sourceRectStartingPos = new Vector2((float)t.sourceRect.X, (float)t.sourceRect.Y);
				t.animationLength = 1;
			}
			JunimoNoteMenu.tempSprites.Add(t);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00133021 File Offset: 0x00131221
		public bool canBeClicked()
		{
			return !this.complete;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0013302C File Offset: 0x0013122C
		public void completionAnimation(JunimoNoteMenu menu, bool playSound = true, int delay = 0)
		{
			if (delay <= 0)
			{
				this.completionAnimation(playSound);
				return;
			}
			this.completionTimer = delay;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00133044 File Offset: 0x00131244
		private void completionAnimation(bool playSound = true)
		{
			if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is JunimoNoteMenu)
			{
				(Game1.activeClickableMenu as JunimoNoteMenu).takeDownBundleSpecificPage(null);
			}
			this.sprite.pingPong = false;
			this.sprite.paused = false;
			this.sprite.sourceRect.X = (int)this.sprite.sourceRectStartingPos.X;
			TemporaryAnimatedSprite expr_6C_cp_0_cp_0 = this.sprite;
			expr_6C_cp_0_cp_0.sourceRect.X = expr_6C_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width;
			this.sprite.animationLength = 15;
			this.sprite.interval = 50f;
			this.sprite.totalNumberOfLoops = 0;
			this.sprite.holdLastFrame = true;
			this.sprite.endFunction = new TemporaryAnimatedSprite.endBehavior(this.shake);
			this.sprite.extraInfoForEndBehavior = 1;
			if (this.complete)
			{
				TemporaryAnimatedSprite expr_F3_cp_0_cp_0 = this.sprite;
				expr_F3_cp_0_cp_0.sourceRect.X = expr_F3_cp_0_cp_0.sourceRect.X + this.sprite.sourceRect.Width * 14;
				this.sprite.sourceRectStartingPos = new Vector2((float)this.sprite.sourceRect.X, (float)this.sprite.sourceRect.Y);
				this.sprite.currentParentTileIndex = 14;
				this.sprite.interval = 0f;
				this.sprite.animationLength = 1;
				this.sprite.extraInfoForEndBehavior = 0;
			}
			else
			{
				if (playSound)
				{
					Game1.playSound("dwop");
				}
				this.bounds.Inflate(Game1.tileSize, Game1.tileSize);
				JunimoNoteMenu.tempSprites.AddRange(Utility.sparkleWithinArea(this.bounds, 8, Bundle.getColorFromColorIndex(this.bundleColor) * 0.5f, 100, 0, ""));
				this.bounds.Inflate(-Game1.tileSize, -Game1.tileSize);
			}
			this.complete = true;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00133238 File Offset: 0x00131438
		public void update(GameTime time)
		{
			this.sprite.update(time);
			if (this.completionTimer > 0 && JunimoNoteMenu.screenSwipe == null)
			{
				this.completionTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.completionTimer <= 0)
				{
					this.completionAnimation(true);
				}
			}
			if (Game1.random.NextDouble() < 0.005 && (this.complete || this.name.ToLower().Contains(Game1.currentSeason)))
			{
				this.shake(0.07363108f);
			}
			if (this.maxShake > 0f)
			{
				if (this.shakeLeft)
				{
					this.sprite.rotation -= 0.0157079641f;
					if (this.sprite.rotation <= -this.maxShake)
					{
						this.shakeLeft = false;
					}
				}
				else
				{
					this.sprite.rotation += 0.0157079641f;
					if (this.sprite.rotation >= this.maxShake)
					{
						this.shakeLeft = true;
					}
				}
			}
			if (this.maxShake > 0f)
			{
				this.maxShake = Math.Max(0f, this.maxShake - 0.0007669904f);
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0013336F File Offset: 0x0013156F
		public void draw(SpriteBatch b)
		{
			this.sprite.draw(b, true, 0, 0);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00133380 File Offset: 0x00131580
		public static Color getColorFromColorIndex(int color)
		{
			switch (color)
			{
			case 0:
				return Color.Lime;
			case 1:
				return Color.DeepPink;
			case 2:
				return Color.Orange;
			case 3:
				return Color.Orange;
			case 4:
				return Color.Red;
			case 5:
				return Color.LightBlue;
			case 6:
				return Color.Cyan;
			default:
				return Color.Lime;
			}
		}

		// Token: 0x04001005 RID: 4101
		public const float shakeRate = 0.0157079641f;

		// Token: 0x04001006 RID: 4102
		public const float shakeDecayRate = 0.00306796166f;

		// Token: 0x04001007 RID: 4103
		public const int Color_Green = 0;

		// Token: 0x04001008 RID: 4104
		public const int Color_Purple = 1;

		// Token: 0x04001009 RID: 4105
		public const int Color_Orange = 2;

		// Token: 0x0400100A RID: 4106
		public const int Color_Yellow = 3;

		// Token: 0x0400100B RID: 4107
		public const int Color_Red = 4;

		// Token: 0x0400100C RID: 4108
		public const int Color_Blue = 5;

		// Token: 0x0400100D RID: 4109
		public const int Color_Teal = 6;

		// Token: 0x0400100E RID: 4110
		public const float DefaultShakeForce = 0.07363108f;

		// Token: 0x0400100F RID: 4111
		public string rewardDescription;

		// Token: 0x04001010 RID: 4112
		public List<BundleIngredientDescription> ingredients;

		// Token: 0x04001011 RID: 4113
		public int bundleColor;

		// Token: 0x04001012 RID: 4114
		public int numberOfIngredientSlots;

		// Token: 0x04001013 RID: 4115
		public int bundleIndex;

		// Token: 0x04001014 RID: 4116
		public int completionTimer;

		// Token: 0x04001015 RID: 4117
		public bool complete;

		// Token: 0x04001016 RID: 4118
		public bool depositsAllowed = true;

		// Token: 0x04001017 RID: 4119
		public TemporaryAnimatedSprite sprite;

		// Token: 0x04001018 RID: 4120
		private float maxShake;

		// Token: 0x04001019 RID: 4121
		private bool shakeLeft;
	}
}
