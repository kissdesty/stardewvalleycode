using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Objects;

namespace StardewValley.Tools
{
	// Token: 0x0200005B RID: 91
	public class FishingRod : Tool
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x000BB470 File Offset: 0x000B9670
		public FishingRod() : base("Fishing Rod", 0, 189, 8, "Use in the water to catch fish.", false, 0)
		{
			this.numAttachmentSlots = 2;
			this.attachments = new Object[this.numAttachmentSlots];
			this.indexOfMenuItemView = 8 + this.upgradeLevel;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000BB4F4 File Offset: 0x000B96F4
		public override int salePrice()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return 500;
			case 1:
				return 2000;
			case 2:
				return 5000;
			case 3:
				return 15000;
			default:
				return 500;
			}
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000BB53D File Offset: 0x000B973D
		public override int attachmentSlots()
		{
			if (this.upgradeLevel > 2)
			{
				return 2;
			}
			if (this.upgradeLevel <= 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000BB558 File Offset: 0x000B9758
		public FishingRod(int upgradeLevel) : base("Fishing Rod", upgradeLevel, 189, 8, "Use in the water to catch fish.", false, 0)
		{
			this.numAttachmentSlots = 2;
			this.attachments = new Object[this.numAttachmentSlots];
			this.indexOfMenuItemView = 8 + upgradeLevel;
			this.upgradeLevel = upgradeLevel;
		}

		// Token: 0x170000BC RID: 188
		public override string Name
		{
			// Token: 0x060008CC RID: 2252 RVA: 0x000BB5E0 File Offset: 0x000B97E0
			get
			{
				switch (this.upgradeLevel)
				{
				case 0:
					return "Bamboo Pole";
				case 1:
					return "Yew Rod";
				case 2:
					return "Fiberglass Rod";
				case 3:
					return "Iridium Rod";
				default:
					return this.name;
				}
			}
			// Token: 0x060008CD RID: 2253 RVA: 0x000A81A0 File Offset: 0x000A63A0
			set
			{
				this.name = value;
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000BB62A File Offset: 0x000B982A
		private int getAddedDistance(Farmer who)
		{
			if (who.FishingLevel >= 8)
			{
				return 3;
			}
			if (who.FishingLevel >= 4)
			{
				return 2;
			}
			if (who.FishingLevel >= 1)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000BB650 File Offset: 0x000B9850
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			if (this.fishCaught)
			{
				return;
			}
			this.hasDoneFucntionYet = true;
			int tileX = (int)(this.bobber.X / (float)Game1.tileSize);
			int tileY = (int)((this.bobber.Y - (float)(Game1.tileSize / 2)) / (float)Game1.tileSize);
			base.DoFunction(location, x, y, power, who);
			if (this.doneWithAnimation && who.IsMainPlayer)
			{
				who.canReleaseTool = true;
			}
			if (Game1.isAnyGamePadButtonBeingPressed())
			{
				Game1.lastCursorMotionWasMouse = false;
			}
			if (!this.isFishing && !this.castedButBobberStillInAir && !this.pullingOutOfWater && !this.isNibbling && !this.hit)
			{
				if (!Game1.eventUp)
				{
					float oldStamina = who.Stamina;
					who.Stamina -= 8f - (float)who.FishingLevel * 0.1f;
					who.checkForExhaustion(oldStamina);
				}
				if ((location.doesTileHaveProperty(tileX, tileY, "Water", "Back") != null && location.doesTileHaveProperty(tileX, tileY, "NoFishing", "Back") == null && location.getTileIndexAt(tileX, tileY, "Buildings") == -1) || location.doesTileHaveProperty(tileX, tileY, "Water", "Buildings") != null)
				{
					this.isFishing = true;
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 100f, 2, 1, new Vector2(this.bobber.X - (float)Game1.tileSize - 16f, this.bobber.Y - (float)Game1.tileSize - 16f), false, false));
					Game1.playSound("dropItemInWater");
					this.timeUntilFishingBite = (float)Game1.random.Next(FishingRod.minFishingBiteTime, FishingRod.maxFishingBiteTime - 250 * who.FishingLevel - ((this.attachments[1] != null && this.attachments[1].ParentSheetIndex == 686) ? 5000 : ((this.attachments[1] != null && this.attachments[1].ParentSheetIndex == 687) ? 10000 : 0)));
					this.timeUntilFishingBite *= 0.75f;
					if (this.attachments[0] != null)
					{
						this.timeUntilFishingBite *= 0.5f;
						if (this.attachments[0].parentSheetIndex == 774)
						{
							this.timeUntilFishingBite *= 0.75f;
						}
					}
					this.timeUntilFishingBite = Math.Max(500f, this.timeUntilFishingBite);
					Stats expr_279 = Game1.stats;
					uint timesFished = expr_279.TimesFished;
					expr_279.TimesFished = timesFished + 1u;
					float arg_2A3_0 = (this.bobber.X - (float)(Game1.tileSize / 2)) / (float)Game1.tileSize;
					float arg_2BF_0 = (this.bobber.Y - (float)(Game1.tileSize / 2)) / (float)Game1.tileSize;
					Point arg_2C6_0 = location.fishSplashPoint;
					Rectangle fishSplashRect = new Rectangle(location.fishSplashPoint.X * Game1.tileSize, location.fishSplashPoint.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					Rectangle bobberRect = new Rectangle((int)this.bobber.X - Game1.tileSize * 5 / 4, (int)this.bobber.Y - Game1.tileSize * 5 / 4, Game1.tileSize, Game1.tileSize);
					if (bobberRect.Intersects(fishSplashRect))
					{
						this.timeUntilFishingBite /= 4f;
						location.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.bobber - new Vector2((float)Game1.tileSize, (float)(Game1.tileSize * 2)), Color.Cyan, 8, false, 100f, 0, -1, -1f, -1, 0));
					}
					if (!who.IsMainPlayer)
					{
						who.Halt();
						who.FarmerSprite.PauseForSingleAnimation = false;
					}
					who.UsingTool = true;
					if (who.IsMainPlayer)
					{
						who.canMove = false;
						return;
					}
				}
				else
				{
					if (this.doneWithAnimation && who.IsMainPlayer)
					{
						who.usingTool = false;
					}
					if (this.doneWithAnimation && who.IsMainPlayer)
					{
						who.canMove = true;
						return;
					}
				}
			}
			else if (!this.isCasting && !this.pullingOutOfWater)
			{
				who.FarmerSprite.pauseForSingleAnimation = false;
				switch (who.FacingDirection)
				{
				case 0:
					who.FarmerSprite.animateBackwardsOnce(299, 35f);
					break;
				case 1:
					who.FarmerSprite.animateBackwardsOnce(300, 35f);
					break;
				case 2:
					who.FarmerSprite.animateBackwardsOnce(301, 35f);
					break;
				case 3:
					who.FarmerSprite.animateBackwardsOnce(302, 35f);
					break;
				}
				if (this.isNibbling)
				{
					double baitPotency = (double)((this.attachments[0] != null) ? ((float)this.attachments[0].Price / 10f) : 0f);
					Point arg_4EB_0 = location.fishSplashPoint;
					Rectangle fishSplashRect2 = new Rectangle(location.fishSplashPoint.X * Game1.tileSize, location.fishSplashPoint.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					Rectangle bobberRect2 = new Rectangle((int)this.bobber.X - Game1.tileSize * 5 / 4, (int)this.bobber.Y - Game1.tileSize * 5 / 4, Game1.tileSize, Game1.tileSize);
					bool splashPoint = fishSplashRect2.Intersects(bobberRect2);
					Object o = location.getFish(this.fishingNibbleAccumulator, (this.attachments[0] != null) ? this.attachments[0].ParentSheetIndex : -1, this.clearWaterDistance + (splashPoint ? 1 : 0), this.lastUser, baitPotency + (splashPoint ? 0.4 : 0.0));
					if (o == null || o.ParentSheetIndex <= 0)
					{
						o = new Object(Game1.random.Next(167, 173), 1, false, -1, 0);
					}
					if (o.scale.X == 1f)
					{
						this.favBait = true;
					}
					if (o.Category == -20 || o.ParentSheetIndex == 152 || o.ParentSheetIndex == 153 || o.parentSheetIndex == 157)
					{
						this.pullFishFromWater(o.ParentSheetIndex, -1, 0, 0, false, false);
						return;
					}
					if (!this.hit)
					{
						this.hit = true;
						Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(612, 1913, 74, 30), 1500f, 1, 0, Game1.GlobalToLocal(Game1.viewport, this.bobber + new Vector2(-140f, (float)(-(float)Game1.tileSize * 5 / 2))), false, false, 1f, 0.005f, Color.White, 4f, 0.075f, 0f, 0f, true)
						{
							scaleChangeChange = -0.005f,
							motion = new Vector2(0f, -0.1f),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.startMinigameEndFunction),
							extraInfoForEndBehavior = o.ParentSheetIndex
						});
						Game1.playSound("FishHit");
					}
					return;
				}
				else
				{
					Game1.playSound("pullItemFromWater");
					this.isFishing = false;
					this.pullingOutOfWater = true;
					if (this.lastUser.FacingDirection == 1 || this.lastUser.FacingDirection == 3)
					{
						double arg_785_0 = (double)Math.Abs(this.bobber.X - (float)this.lastUser.getStandingX());
						float gravity = 0.005f;
						float velocity = -(float)Math.Sqrt(arg_785_0 * (double)gravity / (double)2f);
						float t = 2f * (Math.Abs(velocity - 0.5f) / gravity);
						t *= 1.2f;
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), t, 1, 0, this.bobber + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 3 / 4)), false, false, (float)who.getStandingY() / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							motion = new Vector2((float)((who.FacingDirection == 3) ? -1 : 1) * (velocity + 0.2f), velocity - 0.8f),
							acceleration = new Vector2(0f, gravity),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.donefishingEndFunction),
							timeBasedMotion = true,
							alphaFade = 0.001f
						});
					}
					else
					{
						float distance = this.bobber.Y - (float)this.lastUser.getStandingY();
						float height = Math.Abs(distance + (float)(Game1.tileSize * 4));
						float gravity2 = 0.005f;
						float velocity2 = (float)Math.Sqrt((double)(2f * gravity2 * height));
						float t2 = (float)(Math.Sqrt((double)(2f * (height - distance) / gravity2)) + (double)(velocity2 / gravity2));
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), t2, 1, 0, this.bobber + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 3 / 4)), false, false, this.bobber.Y / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							motion = new Vector2(0f, -velocity2),
							acceleration = new Vector2(0f, gravity2),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.donefishingEndFunction),
							timeBasedMotion = true,
							alphaFade = 0.001f
						});
					}
					who.UsingTool = true;
					who.canReleaseTool = false;
				}
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000BC058 File Offset: 0x000BA258
		public Color getColor()
		{
			switch (this.upgradeLevel)
			{
			case 0:
				return Color.Goldenrod;
			case 1:
				return Color.RosyBrown;
			case 2:
				return Color.White;
			case 3:
				return Color.Violet;
			default:
				return Color.White;
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000BC0A4 File Offset: 0x000BA2A4
		public static int distanceToLand(int tileX, int tileY, GameLocation location)
		{
			Rectangle r = new Rectangle(tileX - 1, tileY - 1, 3, 3);
			bool foundLand = false;
			int distance = 1;
			while (!foundLand && r.Width <= 11)
			{
				foreach (Vector2 v in Utility.getBorderOfThisRectangle(r))
				{
					if (location.isTileOnMap(v) && location.doesTileHaveProperty((int)v.X, (int)v.Y, "Water", "Back") == null)
					{
						foundLand = true;
						distance = r.Width / 2;
						break;
					}
				}
				r.Inflate(1, 1);
			}
			if (r.Width > 11)
			{
				distance = 6;
			}
			distance--;
			return distance;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x000BC168 File Offset: 0x000BA368
		public void startMinigameEndFunction(int extra)
		{
			this.isReeling = true;
			this.hit = false;
			int facingDirection = this.lastUser.FacingDirection;
			if (facingDirection != 1)
			{
				if (facingDirection == 3)
				{
					this.lastUser.FarmerSprite.setCurrentSingleFrame(48, 32000, false, true);
				}
			}
			else
			{
				this.lastUser.FarmerSprite.setCurrentSingleFrame(48, 32000, false, false);
			}
			this.lastUser.FarmerSprite.pauseForSingleAnimation = true;
			this.clearWaterDistance = FishingRod.distanceToLand((int)(this.bobber.X / (float)Game1.tileSize - 1f), (int)(this.bobber.Y / (float)Game1.tileSize - 1f), this.lastUser.currentLocation);
			float fishSize = 1f;
			fishSize *= (float)this.clearWaterDistance / 5f;
			fishSize *= (float)Game1.random.Next(1 + Math.Min(10, this.lastUser.FishingLevel) / 2, 6) / 5f;
			if (this.favBait)
			{
				fishSize *= 1.2f;
			}
			fishSize *= 1f + (float)Game1.random.Next(-10, 10) / 100f;
			fishSize = Math.Max(0f, Math.Min(1f, fishSize));
			bool treasure = false;
			if (!Game1.isFestival() && this.lastUser.fishCaught != null && this.lastUser.fishCaught.Count > 1 && Game1.random.NextDouble() < FishingRod.baseChanceForTreasure + (double)this.lastUser.LuckLevel * 0.005 + ((this.getBaitAttachmentIndex() == 703) ? FishingRod.baseChanceForTreasure : 0.0) + ((this.getBobberAttachmentIndex() == 693) ? (FishingRod.baseChanceForTreasure / 3.0) : 0.0) + Game1.dailyLuck / 2.0 + (this.lastUser.professions.Contains(9) ? FishingRod.baseChanceForTreasure : 0.0))
			{
				treasure = true;
			}
			Game1.activeClickableMenu = new BobberBar(extra, fishSize, treasure, (this.attachments[1] != null) ? this.attachments[1].ParentSheetIndex : -1);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000BC3AA File Offset: 0x000BA5AA
		public int getBobberAttachmentIndex()
		{
			if (this.attachments[1] == null)
			{
				return -1;
			}
			return this.attachments[1].ParentSheetIndex;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000BC3C5 File Offset: 0x000BA5C5
		public int getBaitAttachmentIndex()
		{
			if (this.attachments[0] == null)
			{
				return -1;
			}
			return this.attachments[0].ParentSheetIndex;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000BC3E0 File Offset: 0x000BA5E0
		public bool inUse()
		{
			return this.isFishing || this.isCasting || this.isTimingCast || this.isNibbling || this.isReeling || this.fishCaught;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000BC414 File Offset: 0x000BA614
		public void donefishingEndFunction(int extra)
		{
			this.isFishing = false;
			this.isReeling = false;
			this.lastUser.canReleaseTool = true;
			this.lastUser.canMove = true;
			this.lastUser.usingTool = false;
			this.lastUser.FarmerSprite.pauseForSingleAnimation = false;
			this.pullingOutOfWater = false;
			this.doneFishing(this.lastUser, false);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00002834 File Offset: 0x00000A34
		public static void endOfAnimationBehavior(Farmer f)
		{
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000BC478 File Offset: 0x000BA678
		public override Object attach(Object o)
		{
			if (o != null && o.Category == -21 && this.upgradeLevel > 0)
			{
				Object tmp = this.attachments[0];
				if (tmp != null && tmp.canStackWith(o))
				{
					tmp.Stack = o.addToStack(tmp.Stack);
					if (tmp.Stack <= 0)
					{
						tmp = null;
					}
				}
				this.attachments[0] = o;
				Game1.playSound("button1");
				return tmp;
			}
			if (o != null && o.Category == -22 && this.upgradeLevel > 2)
			{
				Object arg_8E_0 = this.attachments[1];
				this.attachments[1] = o;
				Game1.playSound("button1");
				return arg_8E_0;
			}
			if (o == null)
			{
				if (this.attachments[0] != null)
				{
					Object arg_B7_0 = this.attachments[0];
					this.attachments[0] = null;
					Game1.playSound("dwop");
					return arg_B7_0;
				}
				if (this.attachments[1] != null)
				{
					Object arg_DD_0 = this.attachments[1];
					this.attachments[1] = null;
					Game1.playSound("dwop");
					return arg_DD_0;
				}
			}
			return null;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x000BC564 File Offset: 0x000BA764
		public override void drawAttachments(SpriteBatch b, int x, int y)
		{
			if (this.upgradeLevel > 0)
			{
				if (this.attachments[0] == null)
				{
					b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 36, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
				}
				else
				{
					b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
					this.attachments[0].drawInMenu(b, new Vector2((float)x, (float)y), 1f);
				}
			}
			if (this.upgradeLevel > 2)
			{
				if (this.attachments[1] == null)
				{
					b.Draw(Game1.menuTexture, new Vector2((float)x, (float)(y + Game1.tileSize + 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 37, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
					return;
				}
				b.Draw(Game1.menuTexture, new Vector2((float)x, (float)(y + Game1.tileSize + 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
				this.attachments[1].drawInMenu(b, new Vector2((float)x, (float)(y + Game1.tileSize + 4)), 1f);
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000BC6F4 File Offset: 0x000BA8F4
		public override bool canThisBeAttached(Object o)
		{
			return o == null || (o.category == -21 && this.upgradeLevel > 0) || (o.Category == -22 && this.upgradeLevel > 2);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000BC728 File Offset: 0x000BA928
		public void playerCaughtFishEndFunction(int extraData)
		{
			this.lastUser.Halt();
			this.lastUser.armOffset = Vector2.Zero;
			this.castedButBobberStillInAir = false;
			this.fishCaught = true;
			this.isReeling = false;
			this.isFishing = false;
			this.pullingOutOfWater = false;
			this.lastUser.canReleaseTool = false;
			if (!Game1.isFestival())
			{
				this.recordSize = this.lastUser.caughtFish(this.whichFish, this.fishSize);
				this.lastUser.faceDirection(2);
			}
			else
			{
				Game1.currentLocation.currentEvent.caughtFish(this.whichFish, this.fishSize, this.lastUser);
				this.fishCaught = false;
				this.doneFishing(Game1.player, false);
			}
			if (FishingRod.isFishBossFish(this.whichFish))
			{
				Game1.showGlobalMessage("You've caught one of the legendary fish.");
				return;
			}
			if (this.recordSize)
			{
				this.sparklingText = new SparklingText(Game1.dialogueFont, "New Record!", Color.LimeGreen, Color.Azure, false, 0.1, 2500, -1, 500);
				Game1.playSound("newRecord");
				return;
			}
			Game1.playSound("fishSlap");
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x000BC84E File Offset: 0x000BAA4E
		public static bool isFishBossFish(int index)
		{
			switch (index)
			{
			case 159:
			case 160:
			case 163:
				break;
			case 161:
			case 162:
				return false;
			default:
				if (index != 682 && index != 775)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x000BC884 File Offset: 0x000BAA84
		public void pullFishFromWater(int whichFish, int fishSize, int fishQuality, int fishDifficulty, bool treasureCaught, bool wasPerfect = false)
		{
			this.treasureCaught = treasureCaught;
			this.fishSize = fishSize;
			this.fishQuality = fishQuality;
			this.whichFish = whichFish;
			if (!Game1.isFestival())
			{
				this.bossFish = FishingRod.isFishBossFish(whichFish);
				int experience = Math.Max(1, (fishQuality + 1) * 3 + fishDifficulty / 3);
				if (treasureCaught)
				{
					experience += (int)((float)experience * 1.2f);
				}
				if (wasPerfect)
				{
					experience += (int)((float)experience * 1.4f);
				}
				if (this.bossFish)
				{
					experience *= 5;
				}
				this.lastUser.gainExperience(1, experience);
			}
			float t;
			if (this.lastUser.FacingDirection == 1 || this.lastUser.FacingDirection == 3)
			{
				float distance = Vector2.Distance(this.bobber, this.lastUser.position);
				float gravity = 0.001f;
				float height = (float)(Game1.tileSize * 2) - (this.lastUser.position.Y - this.bobber.Y + 10f);
				double angle = 1.1423973285781066;
				float yVelocity = (float)((double)(distance * gravity) * Math.Tan(angle) / Math.Sqrt((double)(2f * distance * gravity) * Math.Tan(angle) - (double)(2f * gravity * height)));
				if (float.IsNaN(yVelocity))
				{
					yVelocity = 0.6f;
				}
				float xVelocity = (float)((double)yVelocity * (1.0 / Math.Tan(angle)));
				t = distance / xVelocity;
				this.animations.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, whichFish, 16, 16), t, 1, 0, this.bobber, false, false, this.bobber.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					motion = new Vector2((float)((this.lastUser.FacingDirection == 3) ? -1 : 1) * -xVelocity, -yVelocity),
					acceleration = new Vector2(0f, gravity),
					timeBasedMotion = true,
					endFunction = new TemporaryAnimatedSprite.endBehavior(this.playerCaughtFishEndFunction),
					extraInfoForEndBehavior = whichFish,
					endSound = "tinyWhip"
				});
			}
			else
			{
				float distance2 = this.bobber.Y - (float)this.lastUser.getStandingY();
				float height2 = Math.Abs(distance2 + (float)(Game1.tileSize * 4) + (float)(Game1.tileSize / 2));
				if (this.lastUser.FacingDirection == 0)
				{
					height2 += (float)(Game1.tileSize * 3 / 2);
				}
				float gravity2 = 0.005f;
				float velocity = (float)Math.Sqrt((double)(2f * gravity2 * height2));
				t = (float)(Math.Sqrt((double)(2f * (height2 - distance2) / gravity2)) + (double)(velocity / gravity2));
				float xVelocity2 = 0f;
				if (t != 0f)
				{
					xVelocity2 = (this.lastUser.position.X - this.bobber.X) / t;
				}
				this.animations.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, whichFish, 16, 16), t, 1, 0, new Vector2(this.bobber.X, this.bobber.Y), false, false, this.bobber.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					motion = new Vector2(xVelocity2, -velocity),
					acceleration = new Vector2(0f, gravity2),
					timeBasedMotion = true,
					endFunction = new TemporaryAnimatedSprite.endBehavior(this.playerCaughtFishEndFunction),
					extraInfoForEndBehavior = whichFish,
					endSound = "tinyWhip"
				});
			}
			Game1.playSound("pullItemFromWater");
			Game1.playSound("dwop");
			this.castedButBobberStillInAir = false;
			this.pullingOutOfWater = true;
			this.isFishing = false;
			this.isReeling = false;
			switch (this.lastUser.FacingDirection)
			{
			case 0:
				this.lastUser.FarmerSprite.animateBackwardsOnce(299, t);
				return;
			case 1:
				this.lastUser.FarmerSprite.animateBackwardsOnce(300, t);
				return;
			case 2:
				this.lastUser.FarmerSprite.animateBackwardsOnce(301, t);
				return;
			case 3:
				this.lastUser.FarmerSprite.animateBackwardsOnce(302, t);
				return;
			default:
				return;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x000BCCD8 File Offset: 0x000BAED8
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (!this.bobber.Equals(Vector2.Zero) && this.isFishing)
			{
				Vector2 bobberPos = this.bobber;
				float scale = 4f;
				if (this.bobberTimeAccumulator > this.timePerBobberBob)
				{
					if ((!this.isNibbling && !this.isReeling) || Game1.random.NextDouble() < 0.05)
					{
						Game1.playSound("waterSlosh");
						this.lastUser.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f, 8, 0, new Vector2(this.bobber.X - (float)Game1.tileSize - 14f, this.bobber.Y - (float)Game1.tileSize - 10f), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 0.75f, 0.003f, 0f, 0f, false));
					}
					this.timePerBobberBob = (float)((this.bobberBob == 0) ? Game1.random.Next(1500, 3500) : Game1.random.Next(350, 750));
					this.bobberTimeAccumulator = 0f;
					if (this.isNibbling || this.isReeling)
					{
						this.timePerBobberBob = (float)Game1.random.Next(25, 75);
						bobberPos.X += (float)Game1.random.Next(-5, 5);
						bobberPos.Y += (float)Game1.random.Next(-5, 5);
						if (!this.isReeling)
						{
							scale += (float)Game1.random.Next(-20, 20) / 100f;
						}
					}
					else if (Game1.random.NextDouble() < 0.1)
					{
						Game1.playSound("bob");
					}
				}
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, bobberPos), new Rectangle?(new Rectangle(179 + this.bobberBob * 9, 1903, 9, 9)), Color.White, 0f, new Vector2(4f, 4f) * scale, scale, SpriteEffects.None, 0.1f);
			}
			else if (this.isTimingCast || this.castingChosenCountdown > 0f)
			{
				int yOffset = (int)(-Math.Abs(this.castingChosenCountdown / 2f - this.castingChosenCountdown) / 50f);
				float alpha = (this.castingChosenCountdown > 0f && this.castingChosenCountdown < 100f) ? (this.castingChosenCountdown / 100f) : 1f;
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position + new Vector2((float)(-(float)Game1.tileSize / 2 - 16), (float)(-(float)Game1.tileSize * 2 - Game1.tileSize / 2 + yOffset))), new Rectangle?(new Rectangle(193, 1868, 47, 12)), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.885f);
				b.Draw(Game1.staminaRect, new Rectangle((int)Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position).X - Game1.tileSize / 2 - 4, (int)Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position).Y + yOffset - Game1.tileSize * 2 - Game1.tileSize / 2 + 12, (int)(164f * this.castingPower), 25), new Rectangle?(Game1.staminaRect.Bounds), Utility.getRedToGreenLerpColor(this.castingPower) * alpha, 0f, Vector2.Zero, SpriteEffects.None, 0.887f);
			}
			for (int i = this.animations.Count - 1; i >= 0; i--)
			{
				this.animations[i].draw(b, false, 0, 0);
			}
			if (this.sparklingText != null && !this.fishCaught)
			{
				this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position + new Vector2((float)(-(float)Game1.tileSize / 2 + 8), (float)(-(float)Game1.tileSize * 2 - Game1.tileSize))));
			}
			else if (this.sparklingText != null && this.fishCaught)
			{
				this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, base.getLastFarmerToUse().position + new Vector2((float)(-(float)Game1.tileSize / 2 - 32), (float)(-(float)Game1.tileSize * 4) - (float)Game1.tileSize * 1.5f)));
			}
			if ((!this.isFishing && !this.pullingOutOfWater && !this.castedButBobberStillInAir) || this.lastUser.FarmerSprite.CurrentFrame == 57 || (this.lastUser.FacingDirection == 0 && this.pullingOutOfWater && this.whichFish != -1))
			{
				if (this.fishCaught)
				{
					float yOffset2 = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8), (float)(-(float)Game1.tileSize * 5 + Game1.tileSize / 2) + yOffset2)), new Rectangle?(new Rectangle(31, 1870, 73, 49)), Color.White * 0.8f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.06f);
					b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 4), (float)(-(float)Game1.tileSize * 5 + Game1.tileSize / 2 + 4) + yOffset2) + new Vector2(44f, 68f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.whichFish, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.0001f + 0.06f);
					b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize / 8 - 8), (float)(-(float)Game1.tileSize / 4 - Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.whichFish, 16, 16)), Color.White, (this.fishSize == -1) ? 0f : 2.3561945f, new Vector2(8f, 8f), (float)Game1.pixelZoom * 0.75f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
					string name = Game1.objectInformation[this.whichFish].Split(new char[]
					{
						'/'
					})[0];
					b.DrawString(Game1.smallFont, name, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8 + 146) - Game1.smallFont.MeasureString(name).X / 2f, (float)(-(float)Game1.tileSize * 5 + Game1.tileSize * 2 / 3) + yOffset2)), this.bossFish ? new Color(126, 61, 237) : Game1.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
					if (this.fishSize != -1)
					{
						b.DrawString(Game1.smallFont, "Length:", Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8 + 160), (float)(-(float)Game1.tileSize * 5 + Game1.tileSize * 5 / 3) + yOffset2)), Game1.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
						b.DrawString(Game1.smallFont, this.fishSize + " in.", Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(-(float)Game1.tileSize * 2 + 8 + 200) - Game1.smallFont.MeasureString(this.fishSize + "in.").X / 2f, (float)(-(float)Game1.tileSize * 5 + Game1.tileSize * 7 / 3 - 8) + yOffset2)), this.recordSize ? (Color.Blue * Math.Min(1f, yOffset2 / 8f + 1.5f)) : Game1.textColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)this.lastUser.getStandingY() / 10000f + 0.002f + 0.06f);
					}
				}
				return;
			}
			Vector2 bobberPos2 = this.isFishing ? this.bobber : ((this.animations.Count > 0) ? (this.animations[0].position + new Vector2((float)(Game1.tileSize / 2 + 8), (float)(Game1.tileSize + 4))) : Vector2.Zero);
			if (this.whichFish != -1)
			{
				bobberPos2 += new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2));
			}
			Vector2 lastPosition = Vector2.Zero;
			if (this.castedButBobberStillInAir)
			{
				switch (this.lastUser.FacingDirection)
				{
				case 0:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 1:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(32f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 2:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - (float)Game1.tileSize + 40f));
						break;
					case 3:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 16f));
						break;
					case 4:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 32f));
						break;
					case 5:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 32f));
						break;
					default:
						lastPosition = Vector2.Zero;
						break;
					}
					break;
				case 1:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-48f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					case 1:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-16f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 2:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize + 20), this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 3:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 16), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) - 20f));
						break;
					case 4:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 8), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					case 5:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 8), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					default:
						lastPosition = Vector2.Zero;
						break;
					}
					break;
				case 2:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(8f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 1:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 2:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - (float)Game1.tileSize + 40f));
						break;
					case 3:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - 8f));
						break;
					case 4:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + 32f));
						break;
					case 5:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + 32f));
						break;
					default:
						lastPosition = Vector2.Zero;
						break;
					}
					break;
				case 3:
					switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
					{
					case 0:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(112f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					case 1:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(80f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 2:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize + 20))), this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 20f));
						break;
					case 3:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 16))), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) - 20f));
						break;
					case 4:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 8))), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					case 5:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 8))), this.lastUser.armOffset.Y - (float)(Game1.tileSize / 2) + 8f));
						break;
					}
					break;
				default:
					lastPosition = Vector2.Zero;
					break;
				}
			}
			else if (this.isReeling)
			{
				if (Game1.didPlayerJustClickAtAll())
				{
					switch (this.lastUser.FacingDirection)
					{
					case 0:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(24f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 12f));
						break;
					case 1:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(20f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 12f));
						break;
					case 2:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(12f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 8f));
						break;
					case 3:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(48f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 12f));
						break;
					}
				}
				else
				{
					switch (this.lastUser.FacingDirection)
					{
					case 0:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(25f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 1:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					case 2:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(12f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f));
						break;
					case 3:
						lastPosition = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f));
						break;
					}
				}
			}
			else
			{
				switch (this.lastUser.FacingDirection)
				{
				case 0:
					lastPosition = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - (float)Game1.tileSize - 12f)));
					break;
				case 1:
					lastPosition = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-48f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(Game1.tileSize * 2 - 8), this.lastUser.armOffset.Y - (float)Game1.tileSize + 16f)));
					break;
				case 2:
					lastPosition = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(8f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) + 4f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + (float)Game1.tileSize - 12f)));
					break;
				case 3:
					lastPosition = (this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(112f, this.lastUser.armOffset.Y - (float)(Game1.tileSize * 3 / 2) - 8f)) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float)(32 + (32 - (Game1.tileSize * 2 - 8))), this.lastUser.armOffset.Y - (float)Game1.tileSize + 16f)));
					break;
				default:
					lastPosition = Vector2.Zero;
					break;
				}
			}
			Vector2 localBobber = Game1.GlobalToLocal(Game1.viewport, bobberPos2 + new Vector2(-36f, (float)(-(float)Game1.tileSize / 2 - 24 + ((this.bobberBob == 1) ? 4 : 0))));
			if (this.isReeling)
			{
				Utility.drawLineWithScreenCoordinates((int)lastPosition.X, (int)lastPosition.Y, (int)localBobber.X, (int)localBobber.Y, b, Color.White * 0.5f, 1f);
				return;
			}
			Vector2 v = lastPosition;
			Vector2 v2 = new Vector2(lastPosition.X + (localBobber.X - lastPosition.X) / 3f, lastPosition.Y + (localBobber.Y - lastPosition.Y) * 2f / 3f);
			Vector2 v3 = new Vector2(lastPosition.X + (localBobber.X - lastPosition.X) * 2f / 3f, lastPosition.Y + (localBobber.Y - lastPosition.Y) * (float)(this.isFishing ? 6 : 2) / 5f);
			Vector2 v4 = localBobber;
			for (float j = 0f; j < 1f; j += 0.025f)
			{
				Vector2 current = Utility.GetCurvePoint(j, v, v2, v3, v4);
				Utility.drawLineWithScreenCoordinates((int)lastPosition.X, (int)lastPosition.Y, (int)current.X, (int)current.Y, b, Color.White * 0.5f, (float)this.lastUser.getStandingY() / 10000f + ((this.lastUser.FacingDirection != 0) ? 0.005f : -0.001f));
				lastPosition = current;
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000BE684 File Offset: 0x000BC884
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			if (who.Stamina <= 1f)
			{
				if (!who.isEmoting)
				{
					who.doEmote(36);
				}
				who.CanMove = !Game1.eventUp;
				who.UsingTool = false;
				who.canReleaseTool = false;
				this.doneFishing(null, false);
				return;
			}
			this.usedGamePadToCast = false;
			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X))
			{
				this.usedGamePadToCast = true;
			}
			this.bossFish = false;
			this.originalFacingDirection = who.FacingDirection;
			who.Halt();
			this.treasureCaught = false;
			this.showingTreasure = false;
			this.isFishing = false;
			this.hit = false;
			this.favBait = false;
			if (this.attachments != null && this.attachments.Length > 1 && this.attachments[1] != null)
			{
				this.hadBobber = true;
			}
			this.isNibbling = false;
			this.lastUser = who;
			this.isTimingCast = true;
			who.usingTool = true;
			this.whichFish = -1;
			who.canMove = false;
			this.fishCaught = false;
			this.doneWithAnimation = false;
			who.canReleaseTool = false;
			this.hasDoneFucntionYet = false;
			this.isReeling = false;
			this.pullingOutOfWater = false;
			this.castingPower = 0f;
			this.castingChosenCountdown = 0f;
			this.animations.Clear();
			this.sparklingText = null;
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentFrame(295);
				Game1.player.CurrentTool.Update(0, 0);
				return;
			case 1:
				who.FarmerSprite.setCurrentFrame(296);
				Game1.player.CurrentTool.Update(1, 0);
				return;
			case 2:
				who.FarmerSprite.setCurrentFrame(297);
				Game1.player.CurrentTool.Update(2, 0);
				return;
			case 3:
				who.FarmerSprite.setCurrentFrame(298);
				Game1.player.CurrentTool.Update(3, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x000BE888 File Offset: 0x000BCA88
		public void doneFishing(Farmer who, bool consumeBaitAndTackle = false)
		{
			if (consumeBaitAndTackle)
			{
				if (this.attachments[0] != null)
				{
					Object expr_18 = this.attachments[0];
					int stack = expr_18.Stack;
					expr_18.Stack = stack - 1;
					if (this.attachments[0].Stack <= 0)
					{
						this.attachments[0] = null;
						Game1.showGlobalMessage("You've used your last piece of bait.");
					}
				}
				if (this.attachments[1] != null)
				{
					Object expr_66_cp_0_cp_0 = this.attachments[1];
					expr_66_cp_0_cp_0.scale.Y = expr_66_cp_0_cp_0.scale.Y - 0.05f;
					if (this.attachments[1].scale.Y <= 0f)
					{
						this.attachments[1] = null;
						Game1.showGlobalMessage("Your fishing tackle has worn out.");
					}
				}
			}
			this.bobber = Vector2.Zero;
			this.isNibbling = false;
			this.fishCaught = false;
			this.isFishing = false;
			this.isReeling = false;
			this.doneWithAnimation = false;
			this.pullingOutOfWater = false;
			this.fishingBiteAccumulator = 0f;
			this.fishingNibbleAccumulator = 0f;
			this.timeUntilFishingBite = -1f;
			this.timeUntilFishingNibbleDone = -1f;
			this.bobberTimeAccumulator = 0f;
			if (FishingRod.chargeSound != null && FishingRod.chargeSound.IsPlaying)
			{
				FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
			}
			if (FishingRod.reelSound != null && FishingRod.reelSound.IsPlaying)
			{
				FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
			}
			if (who != null)
			{
				who.UsingTool = false;
				who.Halt();
				who.faceDirection(this.originalFacingDirection);
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x000BE9F4 File Offset: 0x000BCBF4
		public static void doneWithCastingAnimation(Farmer who)
		{
			if (who.CurrentTool != null && who.CurrentTool is FishingRod)
			{
				(who.CurrentTool as FishingRod).doneWithAnimation = true;
				if ((who.CurrentTool as FishingRod).hasDoneFucntionYet)
				{
					who.canReleaseTool = true;
					who.usingTool = false;
					who.canMove = true;
					Farmer.canMoveNow(who);
				}
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000BEA54 File Offset: 0x000BCC54
		public void castingEndFunction(int extraInfo)
		{
			this.castedButBobberStillInAir = false;
			if (this.lastUser != null)
			{
				float oldStamina = this.lastUser.Stamina;
				this.lastUser.CurrentTool.DoFunction(this.lastUser.currentLocation, (int)this.bobber.X, (int)this.bobber.Y, 1, this.lastUser);
				this.lastUser.lastClick = Vector2.Zero;
				if (FishingRod.reelSound != null)
				{
					FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
				}
				FishingRod.reelSound = null;
				if (this.lastUser.Stamina <= 0f && oldStamina > 0f)
				{
					this.lastUser.doEmote(36);
				}
				Game1.toolHold = 0f;
				if (!this.isFishing && this.doneWithAnimation)
				{
					Farmer.canMoveNow(this.lastUser);
				}
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x000BEB30 File Offset: 0x000BCD30
		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (Game1.paused)
			{
				return;
			}
			if (who.CurrentTool != null && who.CurrentTool.Equals(this) && who.usingTool)
			{
				who.CanMove = false;
			}
			else if (Game1.currentMinigame == null && (who.CurrentTool == null || !(who.CurrentTool is FishingRod) || !who.usingTool))
			{
				if (FishingRod.chargeSound != null && FishingRod.chargeSound.IsPlaying)
				{
					FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
					FishingRod.chargeSound = null;
				}
				return;
			}
			for (int i = this.animations.Count - 1; i >= 0; i--)
			{
				if (this.animations[i].update(time))
				{
					this.animations.RemoveAt(i);
				}
			}
			if (this.sparklingText != null && this.sparklingText.update(time))
			{
				this.sparklingText = null;
			}
			if (this.castingChosenCountdown > 0f)
			{
				this.castingChosenCountdown -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.castingChosenCountdown <= 0f)
				{
					switch (who.FacingDirection)
					{
					case 0:
						who.FarmerSprite.animateOnce(295, 1f, 1);
						Game1.player.CurrentTool.Update(0, 0);
						break;
					case 1:
						who.FarmerSprite.animateOnce(296, 1f, 1);
						Game1.player.CurrentTool.Update(1, 0);
						break;
					case 2:
						who.FarmerSprite.animateOnce(297, 1f, 1);
						Game1.player.CurrentTool.Update(2, 0);
						break;
					case 3:
						who.FarmerSprite.animateOnce(298, 1f, 1);
						Game1.player.CurrentTool.Update(3, 0);
						break;
					}
					if (who.FacingDirection == 1 || who.FacingDirection == 3)
					{
						float distance = Math.Max((float)(Game1.tileSize * 2), this.castingPower * (float)(this.getAddedDistance(who) + 4) * (float)Game1.tileSize);
						if (who.FacingDirection == 3)
						{
							distance = Math.Max((float)(Game1.tileSize * 2), distance - (float)Game1.tileSize);
						}
						distance -= 8f;
						float gravity = 0.005f;
						float velocity = (float)((double)distance * Math.Sqrt((double)(gravity / (2f * (distance + (float)(Game1.tileSize * 3 / 2))))));
						float t = 2f * (velocity / gravity) + (float)((Math.Sqrt((double)(velocity * velocity + 2f * gravity * (float)(Game1.tileSize * 3 / 2))) - (double)velocity) / (double)gravity);
						this.bobber = new Vector2((float)who.getStandingX() + (float)((who.FacingDirection == 3) ? -1 : 1) * distance + (float)(Game1.tileSize / 2), (float)(who.getStandingY() + Game1.tileSize / 2));
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), t, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, (float)who.getStandingY() / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							motion = new Vector2((float)((who.FacingDirection == 3) ? -1 : 1) * velocity, -velocity),
							acceleration = new Vector2(0f, gravity),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.castingEndFunction),
							timeBasedMotion = true
						});
					}
					else
					{
						float distance2 = -Math.Max((float)(Game1.tileSize * 2), this.castingPower * (float)(this.getAddedDistance(who) + 3) * (float)Game1.tileSize);
						float height = Math.Abs(distance2 - (float)Game1.tileSize);
						if (this.lastUser.FacingDirection == 0)
						{
							distance2 = -distance2;
							height += (float)Game1.tileSize;
						}
						float gravity2 = 0.005f;
						float velocity2 = (float)Math.Sqrt((double)(2f * gravity2 * height));
						float t2 = (float)(Math.Sqrt((double)(2f * (height - distance2) / gravity2)) + (double)(velocity2 / gravity2));
						t2 *= 1.05f;
						if (this.lastUser.FacingDirection == 0)
						{
							t2 *= 1.05f;
						}
						this.bobber = new Vector2((float)(who.getStandingX() + Game1.random.Next(48)), (float)(who.getStandingY() + Game1.tileSize / 2) - distance2);
						this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), t2, 1, 0, who.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, false, this.bobber.Y / 10000f, 0f, Color.White, 4f, 0f, 0f, (float)Game1.random.Next(-20, 20) / 100f, false)
						{
							alphaFade = 0.0001f,
							motion = new Vector2(0f, -velocity2),
							acceleration = new Vector2(0f, gravity2),
							endFunction = new TemporaryAnimatedSprite.endBehavior(this.castingEndFunction),
							timeBasedMotion = true
						});
					}
					this.castedButBobberStillInAir = true;
					this.isCasting = false;
					Game1.playSound("cast");
					if (Game1.soundBank != null)
					{
						FishingRod.reelSound = Game1.soundBank.GetCue("slowReel");
						FishingRod.reelSound.SetVariable("Pitch", 1600f);
						FishingRod.reelSound.Play();
					}
				}
			}
			else if (!this.isTimingCast && this.castingChosenCountdown <= 0f)
			{
				who.jitterStrength = 0f;
			}
			if (this.isTimingCast)
			{
				if (FishingRod.chargeSound == null && Game1.soundBank != null)
				{
					FishingRod.chargeSound = Game1.soundBank.GetCue("SinWave");
				}
				if (this.castingPower > 0f && FishingRod.chargeSound != null && !FishingRod.chargeSound.IsPlaying && !FishingRod.chargeSound.IsStopped)
				{
					FishingRod.chargeSound.Play();
				}
				this.castingPower = Math.Max(0f, Math.Min(1f, this.castingPower + this.castingTimerSpeed * (float)time.ElapsedGameTime.Milliseconds));
				if (FishingRod.chargeSound != null)
				{
					FishingRod.chargeSound.SetVariable("Pitch", 2400f * this.castingPower);
				}
				if (this.castingPower == 1f || this.castingPower == 0f)
				{
					this.castingTimerSpeed = -this.castingTimerSpeed;
				}
				who.armOffset.Y = 2f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				who.jitterStrength = Math.Max(0f, this.castingPower - 0.5f);
				if (((!this.usedGamePadToCast && Mouse.GetState().LeftButton == ButtonState.Released) || (this.usedGamePadToCast && Game1.options.gamepadControls && GamePad.GetState(PlayerIndex.One).IsButtonUp(Buttons.X))) && Game1.areAllOfTheseKeysUp(Keyboard.GetState(), Game1.options.useToolButton))
				{
					if (FishingRod.chargeSound != null)
					{
						FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
						FishingRod.chargeSound = null;
					}
					Game1.playSound("button1");
					Rumble.rumble(0.5f, 150f);
					this.isTimingCast = false;
					this.isCasting = true;
					this.castingChosenCountdown = 350f;
					who.armOffset.Y = 0f;
					if (this.castingPower > 0.99f)
					{
						Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(545, 1921, 53, 19), 800f, 1, 0, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3))), false, false, 1f, 0.01f, Color.White, 2f, 0f, 0f, 0f, true)
						{
							motion = new Vector2(0f, -4f),
							acceleration = new Vector2(0f, 0.2f),
							delayBeforeAnimationStart = 200
						});
						DelayedAction.playSoundAfterDelay("crit", 200);
						return;
					}
				}
			}
			else
			{
				if (this.isReeling)
				{
					if (Game1.didPlayerJustClickAtAll())
					{
						if (Game1.isAnyGamePadButtonBeingPressed())
						{
							Game1.lastCursorMotionWasMouse = false;
						}
						switch (who.FacingDirection)
						{
						case 0:
							who.FarmerSprite.setCurrentSingleFrame(76, 32000, false, false);
							break;
						case 1:
							who.FarmerSprite.setCurrentSingleFrame(72, 100, false, false);
							break;
						case 2:
							who.FarmerSprite.setCurrentSingleFrame(75, 32000, false, false);
							break;
						case 3:
							who.FarmerSprite.setCurrentSingleFrame(72, 100, false, true);
							break;
						}
						who.armOffset.Y = (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
						who.jitterStrength = 1f;
					}
					else
					{
						switch (who.FacingDirection)
						{
						case 0:
							who.FarmerSprite.setCurrentSingleFrame(36, 32000, false, false);
							break;
						case 1:
							who.FarmerSprite.setCurrentSingleFrame(48, 100, false, false);
							break;
						case 2:
							who.FarmerSprite.setCurrentSingleFrame(66, 32000, false, false);
							break;
						case 3:
							who.FarmerSprite.setCurrentSingleFrame(48, 100, false, true);
							break;
						}
						who.stopJittering();
					}
					who.armOffset = new Vector2((float)Game1.random.Next(-10, 11) / 10f, (float)Game1.random.Next(-10, 11) / 10f);
					this.bobberTimeAccumulator += (float)time.ElapsedGameTime.Milliseconds;
					return;
				}
				if (this.isFishing)
				{
					this.bobber.Y = this.bobber.Y + (float)(0.10000000149011612 * Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0));
					who.canReleaseTool = true;
					this.bobberTimeAccumulator += (float)time.ElapsedGameTime.Milliseconds;
					switch (who.FacingDirection)
					{
					case 0:
						who.FarmerSprite.setCurrentFrame(44);
						break;
					case 1:
						who.FarmerSprite.setCurrentFrame(89);
						break;
					case 2:
						who.FarmerSprite.setCurrentFrame(70);
						break;
					case 3:
						who.FarmerSprite.setCurrentFrame(89, 0, 10, 1, true, false);
						break;
					}
					who.armOffset.Y = (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2) + (float)((who.FacingDirection == 1 || who.FacingDirection == 3) ? 1 : -1);
					if (who.IsMainPlayer)
					{
						if (this.timeUntilFishingBite != -1f)
						{
							this.fishingBiteAccumulator += (float)time.ElapsedGameTime.Milliseconds;
							if (this.fishingBiteAccumulator > this.timeUntilFishingBite)
							{
								this.fishingBiteAccumulator = 0f;
								this.timeUntilFishingBite = -1f;
								this.isNibbling = true;
								Game1.playSound("fishBite");
								Rumble.rumble(0.75f, 250f);
								this.timeUntilFishingNibbleDone = (float)FishingRod.maxTimeToNibble;
								if (Game1.currentMinigame == null)
								{
									Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(395, 497, 3, 8), new Vector2((float)(this.lastUser.getStandingX() - Game1.viewport.X), (float)(this.lastUser.getStandingY() - Game1.tileSize * 2 - Game1.pixelZoom * 2 - Game1.viewport.Y)), false, 0.02f, Color.White)
									{
										scale = (float)(Game1.pixelZoom + 1),
										scaleChange = -0.01f,
										motion = new Vector2(0f, -0.5f),
										shakeIntensityChange = -0.005f,
										shakeIntensity = 1f
									});
								}
								this.timePerBobberBob = 1f;
							}
						}
						if (this.timeUntilFishingNibbleDone != -1f)
						{
							this.fishingNibbleAccumulator += (float)time.ElapsedGameTime.Milliseconds;
							if (this.fishingNibbleAccumulator > this.timeUntilFishingNibbleDone)
							{
								this.fishingNibbleAccumulator = 0f;
								this.timeUntilFishingNibbleDone = -1f;
								this.isNibbling = false;
								this.timeUntilFishingBite = (float)Game1.random.Next(FishingRod.minFishingBiteTime, FishingRod.maxFishingBiteTime);
								return;
							}
						}
					}
				}
				else if (who.usingTool && this.castedButBobberStillInAir)
				{
					Vector2 motion = Vector2.Zero;
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton) && who.FacingDirection != 2 && who.FacingDirection != 0)
					{
						motion.Y += 4f;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton) && who.FacingDirection != 1 && who.FacingDirection != 3)
					{
						motion.X += 2f;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton) && who.FacingDirection != 0 && who.FacingDirection != 2)
					{
						motion.Y -= 4f;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton) && who.FacingDirection != 3 && who.FacingDirection != 1)
					{
						motion.X -= 2f;
					}
					this.bobber += motion;
					if (this.animations.Count > 0)
					{
						this.animations[0].position += motion;
						return;
					}
				}
				else
				{
					if (this.showingTreasure)
					{
						who.FarmerSprite.setCurrentSingleFrame(0, 32000, false, false);
						return;
					}
					if (this.fishCaught)
					{
						if (!Game1.isFestival())
						{
							who.faceDirection(2);
							who.FarmerSprite.setCurrentFrame(84);
						}
						if (Game1.random.NextDouble() < 0.025)
						{
							who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(653, 858, 1, 1), 9999f, 1, 1, who.position + new Vector2((float)(Game1.random.Next(-3, 2) * 4), (float)(-(float)Game1.tileSize / 2)), false, false, (float)who.getStandingY() / 10000f + 0.002f, 0.04f, Color.LightBlue, 5f, 0f, 0f, 0f, false)
							{
								acceleration = new Vector2(0f, 0.25f)
							});
						}
						if (Mouse.GetState().LeftButton == ButtonState.Pressed || Game1.didPlayerJustClickAtAll() || Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton))
						{
							Game1.playSound("coin");
							if (this.treasureCaught)
							{
								this.fishCaught = false;
								this.showingTreasure = true;
								bool hadroomForfish = this.lastUser.addItemToInventoryBool(new Object(this.whichFish, 1, false, -1, this.fishQuality), false);
								this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(64, 1920, 32, 32), 500f, 1, 0, this.lastUser.position + new Vector2(-32f, -160f), false, false, (float)this.lastUser.getStandingY() / 10000f + 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -0.128f),
									timeBasedMotion = true,
									endFunction = new TemporaryAnimatedSprite.endBehavior(this.openChestEndFunction),
									extraInfoForEndBehavior = (hadroomForfish ? 0 : 1),
									alpha = 0f,
									alphaFade = -0.002f
								});
								return;
							}
							this.doneFishing(this.lastUser, true);
							this.lastUser.completelyStopAnimatingOrDoingAction();
							if (!Game1.isFestival() && !this.lastUser.addItemToInventoryBool(new Object(this.whichFish, 1, false, -1, this.fishQuality), false))
							{
								Game1.activeClickableMenu = new ItemGrabMenu(new List<Item>
								{
									new Object(this.whichFish, 1, false, -1, this.fishQuality)
								});
								return;
							}
						}
					}
					else
					{
						if (who.usingTool && this.castedButBobberStillInAir && this.doneWithAnimation)
						{
							switch (who.FacingDirection)
							{
							case 0:
								who.FarmerSprite.setCurrentFrame(39);
								break;
							case 1:
								who.FarmerSprite.setCurrentFrame(89);
								break;
							case 2:
								who.FarmerSprite.setCurrentFrame(28);
								break;
							case 3:
								who.FarmerSprite.setCurrentFrame(89, 0, 10, 1, true, false);
								break;
							}
							who.armOffset.Y = (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
							return;
						}
						if (!this.castedButBobberStillInAir && this.whichFish != -1 && this.animations.Count > 0 && this.animations[0].timer > 500f && !Game1.eventUp)
						{
							this.lastUser.faceDirection(2);
							this.lastUser.FarmerSprite.setCurrentFrame(57);
						}
					}
				}
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000BFD64 File Offset: 0x000BDF64
		public void openChestEndFunction(int extra)
		{
			Game1.playSound("openChest");
			this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(64, 1920, 32, 32), 200f, 4, 0, this.lastUser.position + new Vector2(-32f, -228f), false, false, (float)this.lastUser.getStandingY() / 10000f + 0.001f, 0f, Color.White, 4f, 0f, 0f, 0f, false)
			{
				endFunction = new TemporaryAnimatedSprite.endBehavior(this.openTreasureMenuEndFunction),
				extraInfoForEndBehavior = extra
			});
			this.sparklingText = null;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool doesShowTileLocationMarker()
		{
			return false;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000BFE20 File Offset: 0x000BE020
		public void openTreasureMenuEndFunction(int extra)
		{
			this.lastUser.gainExperience(5, 10 * (this.clearWaterDistance + 1));
			this.doneFishing(this.lastUser, true);
			this.lastUser.completelyStopAnimatingOrDoingAction();
			List<Item> treasures = new List<Item>();
			if (extra == 1)
			{
				treasures.Add(new Object(this.whichFish, 1, false, -1, this.fishQuality));
			}
			float chance = 1f;
			while (Game1.random.NextDouble() <= (double)chance)
			{
				chance *= 0.4f;
				switch (Game1.random.Next(4))
				{
				case 0:
					if (this.clearWaterDistance >= 5 && Game1.random.NextDouble() < 0.03)
					{
						treasures.Add(new Object(386, Game1.random.Next(1, 3), false, -1, 0));
					}
					else
					{
						List<int> possibles = new List<int>();
						if (this.clearWaterDistance >= 4)
						{
							possibles.Add(384);
						}
						if (this.clearWaterDistance >= 3 && (possibles.Count == 0 || Game1.random.NextDouble() < 0.6))
						{
							possibles.Add(380);
						}
						if (possibles.Count == 0 || Game1.random.NextDouble() < 0.6)
						{
							possibles.Add(378);
						}
						if (possibles.Count == 0 || Game1.random.NextDouble() < 0.6)
						{
							possibles.Add(388);
						}
						if (possibles.Count == 0 || Game1.random.NextDouble() < 0.6)
						{
							possibles.Add(390);
						}
						possibles.Add(382);
						treasures.Add(new Object(possibles.ElementAt(Game1.random.Next(possibles.Count)), Game1.random.Next(2, 7) * ((Game1.random.NextDouble() < 0.05 + (double)this.lastUser.luckLevel * 0.015) ? 2 : 1), false, -1, 0));
						if (Game1.random.NextDouble() < 0.05 + (double)this.lastUser.LuckLevel * 0.03)
						{
							treasures.Last<Item>().Stack *= 2;
						}
					}
					break;
				case 1:
					if (this.clearWaterDistance >= 4 && Game1.random.NextDouble() < 0.1 && this.lastUser.FishingLevel >= 6)
					{
						treasures.Add(new Object(687, 1, false, -1, 0));
					}
					else if (this.lastUser.FishingLevel >= 6)
					{
						treasures.Add(new Object(685, 1, false, -1, 0));
					}
					else
					{
						treasures.Add(new Object(685, 10, false, -1, 0));
					}
					break;
				case 2:
					if (Game1.random.NextDouble() < 0.1 && this.lastUser != null && this.lastUser.archaeologyFound != null && this.lastUser.archaeologyFound.ContainsKey(102) && this.lastUser.archaeologyFound[102][0] < 21)
					{
						treasures.Add(new Object(102, 1, false, -1, 0));
						Game1.showGlobalMessage("You found a lost book. The library has been expanded.");
					}
					else if (Game1.player.archaeologyFound.Count > 0)
					{
						if (Game1.random.NextDouble() < 0.25 && this.lastUser.FishingLevel > 1)
						{
							treasures.Add(new Object(Game1.random.Next(585, 588), 1, false, -1, 0));
						}
						else if (Game1.random.NextDouble() < 0.5 && this.lastUser.FishingLevel > 1)
						{
							treasures.Add(new Object(Game1.random.Next(103, 120), 1, false, -1, 0));
						}
						else
						{
							treasures.Add(new Object(535, 1, false, -1, 0));
						}
					}
					else
					{
						treasures.Add(new Object(382, Game1.random.Next(1, 3), false, -1, 0));
					}
					break;
				case 3:
					switch (Game1.random.Next(3))
					{
					case 0:
						if (this.clearWaterDistance >= 4)
						{
							treasures.Add(new Object(537 + ((Game1.random.NextDouble() < 0.4) ? Game1.random.Next(-2, 0) : 0), Game1.random.Next(1, 4), false, -1, 0));
						}
						else if (this.clearWaterDistance >= 3)
						{
							treasures.Add(new Object(536 + ((Game1.random.NextDouble() < 0.4) ? -1 : 0), Game1.random.Next(1, 4), false, -1, 0));
						}
						else
						{
							treasures.Add(new Object(535, Game1.random.Next(1, 4), false, -1, 0));
						}
						if (Game1.random.NextDouble() < 0.05 + (double)this.lastUser.LuckLevel * 0.03)
						{
							treasures.Last<Item>().Stack *= 2;
						}
						break;
					case 1:
						if (this.lastUser.FishingLevel < 2)
						{
							treasures.Add(new Object(382, Game1.random.Next(1, 4), false, -1, 0));
						}
						else
						{
							if (this.clearWaterDistance >= 4)
							{
								treasures.Add(new Object((Game1.random.NextDouble() < 0.3) ? 82 : ((Game1.random.NextDouble() < 0.5) ? 64 : 60), Game1.random.Next(1, 3), false, -1, 0));
							}
							else if (this.clearWaterDistance >= 3)
							{
								treasures.Add(new Object((Game1.random.NextDouble() < 0.3) ? 84 : ((Game1.random.NextDouble() < 0.5) ? 70 : 62), Game1.random.Next(1, 3), false, -1, 0));
							}
							else
							{
								treasures.Add(new Object((Game1.random.NextDouble() < 0.3) ? 86 : ((Game1.random.NextDouble() < 0.5) ? 66 : 68), Game1.random.Next(1, 3), false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.028 * (double)((float)this.clearWaterDistance / 5f))
							{
								treasures.Add(new Object(72, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.05)
							{
								treasures.Last<Item>().Stack *= 2;
							}
						}
						break;
					case 2:
						if (this.lastUser.FishingLevel < 2)
						{
							treasures.Add(new Object(770, Game1.random.Next(1, 4), false, -1, 0));
						}
						else
						{
							float luckModifier = (1f + (float)Game1.dailyLuck) * ((float)this.clearWaterDistance / 5f);
							if (Game1.random.NextDouble() < 0.05 * (double)luckModifier && !this.lastUser.specialItems.Contains(14))
							{
								treasures.Add(new MeleeWeapon(14)
								{
									specialItem = true
								});
							}
							if (Game1.random.NextDouble() < 0.05 * (double)luckModifier && !this.lastUser.specialItems.Contains(51))
							{
								treasures.Add(new MeleeWeapon(51)
								{
									specialItem = true
								});
							}
							if (Game1.random.NextDouble() < 0.07 * (double)luckModifier)
							{
								switch (Game1.random.Next(3))
								{
								case 0:
									treasures.Add(new Ring(516 + ((Game1.random.NextDouble() < (double)((float)this.lastUser.LuckLevel / 11f)) ? 1 : 0)));
									break;
								case 1:
									treasures.Add(new Ring(518 + ((Game1.random.NextDouble() < (double)((float)this.lastUser.LuckLevel / 11f)) ? 1 : 0)));
									break;
								case 2:
									treasures.Add(new Ring(Game1.random.Next(529, 535)));
									break;
								}
							}
							if (Game1.random.NextDouble() < 0.02 * (double)luckModifier)
							{
								treasures.Add(new Object(166, 1, false, -1, 0));
							}
							if (this.lastUser.FishingLevel > 5 && Game1.random.NextDouble() < 0.001 * (double)luckModifier)
							{
								treasures.Add(new Object(74, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)luckModifier)
							{
								treasures.Add(new Object(127, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)luckModifier)
							{
								treasures.Add(new Object(126, 1, false, -1, 0));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)luckModifier)
							{
								treasures.Add(new Ring(527));
							}
							if (Game1.random.NextDouble() < 0.01 * (double)luckModifier)
							{
								treasures.Add(new Boots(Game1.random.Next(504, 514)));
							}
							if (treasures.Count == 1)
							{
								treasures.Add(new Object(72, 1, false, -1, 0));
							}
						}
						break;
					}
					break;
				}
			}
			if (treasures.Count == 0)
			{
				treasures.Add(new Object(685, Game1.random.Next(1, 4) * 5, false, -1, 0));
			}
			Game1.activeClickableMenu = new ItemGrabMenu(treasures);
			(Game1.activeClickableMenu as ItemGrabMenu).source = 3;
			this.lastUser.completelyStopAnimatingOrDoingAction();
		}

		// Token: 0x040008C3 RID: 2243
		public const int sizeOfLandCheckRectangle = 11;

		// Token: 0x040008C4 RID: 2244
		private Vector2 bobber;

		// Token: 0x040008C5 RID: 2245
		public static int minFishingBiteTime = 600;

		// Token: 0x040008C6 RID: 2246
		public static int maxFishingBiteTime = 30000;

		// Token: 0x040008C7 RID: 2247
		public static int minTimeToNibble = 340;

		// Token: 0x040008C8 RID: 2248
		public static int maxTimeToNibble = 800;

		// Token: 0x040008C9 RID: 2249
		public static double baseChanceForTreasure = 0.15;

		// Token: 0x040008CA RID: 2250
		private int bobberBob;

		// Token: 0x040008CB RID: 2251
		[XmlIgnore]
		public float bobberTimeAccumulator;

		// Token: 0x040008CC RID: 2252
		[XmlIgnore]
		public float timePerBobberBob = 2000f;

		// Token: 0x040008CD RID: 2253
		[XmlIgnore]
		public float timeUntilFishingBite = -1f;

		// Token: 0x040008CE RID: 2254
		[XmlIgnore]
		public float fishingBiteAccumulator;

		// Token: 0x040008CF RID: 2255
		[XmlIgnore]
		public float fishingNibbleAccumulator;

		// Token: 0x040008D0 RID: 2256
		[XmlIgnore]
		public float timeUntilFishingNibbleDone = -1f;

		// Token: 0x040008D1 RID: 2257
		[XmlIgnore]
		public float castingPower;

		// Token: 0x040008D2 RID: 2258
		[XmlIgnore]
		public float castingChosenCountdown;

		// Token: 0x040008D3 RID: 2259
		[XmlIgnore]
		public float castingTimerSpeed = 0.001f;

		// Token: 0x040008D4 RID: 2260
		[XmlIgnore]
		public float fishWiggle;

		// Token: 0x040008D5 RID: 2261
		[XmlIgnore]
		public float fishWiggleIntensity;

		// Token: 0x040008D6 RID: 2262
		[XmlIgnore]
		public bool isFishing;

		// Token: 0x040008D7 RID: 2263
		[XmlIgnore]
		public bool hit;

		// Token: 0x040008D8 RID: 2264
		[XmlIgnore]
		public bool isNibbling;

		// Token: 0x040008D9 RID: 2265
		[XmlIgnore]
		public bool favBait;

		// Token: 0x040008DA RID: 2266
		[XmlIgnore]
		public bool isTimingCast;

		// Token: 0x040008DB RID: 2267
		[XmlIgnore]
		public bool isCasting;

		// Token: 0x040008DC RID: 2268
		[XmlIgnore]
		public bool castedButBobberStillInAir;

		// Token: 0x040008DD RID: 2269
		[XmlIgnore]
		public bool doneWithAnimation;

		// Token: 0x040008DE RID: 2270
		[XmlIgnore]
		public bool hasDoneFucntionYet;

		// Token: 0x040008DF RID: 2271
		[XmlIgnore]
		public bool pullingOutOfWater;

		// Token: 0x040008E0 RID: 2272
		[XmlIgnore]
		public bool isReeling;

		// Token: 0x040008E1 RID: 2273
		[XmlIgnore]
		public bool fishCaught;

		// Token: 0x040008E2 RID: 2274
		[XmlIgnore]
		public bool recordSize;

		// Token: 0x040008E3 RID: 2275
		[XmlIgnore]
		public bool treasureCaught;

		// Token: 0x040008E4 RID: 2276
		[XmlIgnore]
		public bool showingTreasure;

		// Token: 0x040008E5 RID: 2277
		[XmlIgnore]
		public bool hadBobber;

		// Token: 0x040008E6 RID: 2278
		[XmlIgnore]
		public bool bossFish;

		// Token: 0x040008E7 RID: 2279
		[XmlIgnore]
		public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();

		// Token: 0x040008E8 RID: 2280
		[XmlIgnore]
		public SparklingText sparklingText;

		// Token: 0x040008E9 RID: 2281
		[XmlIgnore]
		private int fishSize;

		// Token: 0x040008EA RID: 2282
		[XmlIgnore]
		private int whichFish;

		// Token: 0x040008EB RID: 2283
		[XmlIgnore]
		private int fishQuality;

		// Token: 0x040008EC RID: 2284
		[XmlIgnore]
		private int clearWaterDistance;

		// Token: 0x040008ED RID: 2285
		[XmlIgnore]
		private int originalFacingDirection;

		// Token: 0x040008EE RID: 2286
		public static Cue chargeSound;

		// Token: 0x040008EF RID: 2287
		public static Cue reelSound;

		// Token: 0x040008F0 RID: 2288
		private bool usedGamePadToCast;
	}
}
