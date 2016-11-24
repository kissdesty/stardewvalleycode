using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;

namespace StardewValley.Tools
{
	// Token: 0x0200005E RID: 94
	public class MeleeWeapon : Tool
	{
		// Token: 0x060008EE RID: 2286 RVA: 0x000C0CC8 File Offset: 0x000BEEC8
		public MeleeWeapon()
		{
			this.category = -98;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000C0CE4 File Offset: 0x000BEEE4
		public MeleeWeapon(int spriteIndex)
		{
			this.category = -98;
			int index = (spriteIndex > -10000) ? spriteIndex : (Math.Abs(spriteIndex) - -10000);
			Dictionary<int, string> weaponData = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
			if (weaponData.ContainsKey(index))
			{
				string[] split = weaponData[index].Split(new char[]
				{
					'/'
				});
				this.name = split[0];
				this.description = split[1];
				this.minDamage = Convert.ToInt32(split[2]);
				this.maxDamage = Convert.ToInt32(split[3]);
				this.knockback = (float)Convert.ToDouble(split[4], CultureInfo.InvariantCulture);
				this.speed = Convert.ToInt32(split[5]);
				this.addedPrecision = Convert.ToInt32(split[6]);
				this.addedDefense = Convert.ToInt32(split[7]);
				this.type = Convert.ToInt32(split[8]);
				if (this.type == 0)
				{
					this.type = 3;
				}
				this.addedAreaOfEffect = Convert.ToInt32(split[11]);
				this.critChance = (float)Convert.ToDouble(split[12], CultureInfo.InvariantCulture);
				this.critMultiplier = (float)Convert.ToDouble(split[13], CultureInfo.InvariantCulture);
			}
			this.Stack = 1;
			this.initialParentTileIndex = index;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.currentParentTileIndex;
			if (spriteIndex == 47)
			{
				this.category = -99;
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000C0E4B File Offset: 0x000BF04B
		public MeleeWeapon(int spriteIndex, int type)
		{
			this.type = type;
			this.name = "";
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000C0E70 File Offset: 0x000BF070
		public override string checkForSpecialItemHoldUpMeessage()
		{
			if (this.initialParentTileIndex == 4)
			{
				return "The prismatic shard changes shape before your very eyes! This power is tremendous.^^     You've found the =Galaxy Sword=  ^";
			}
			return null;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x000C0E84 File Offset: 0x000BF084
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			float coolDownLevel = 0f;
			float addedScale = 0f;
			switch (this.type)
			{
			case 0:
			case 3:
				if (MeleeWeapon.defenseCooldown > 0)
				{
					coolDownLevel = (float)MeleeWeapon.defenseCooldown / 1500f;
				}
				addedScale = MeleeWeapon.addedSwordScale;
				break;
			case 1:
				if (MeleeWeapon.daggerCooldown > 0)
				{
					coolDownLevel = (float)MeleeWeapon.daggerCooldown / 6000f;
				}
				addedScale = MeleeWeapon.addedDaggerScale;
				break;
			case 2:
				if (MeleeWeapon.clubCooldown > 0)
				{
					coolDownLevel = (float)MeleeWeapon.clubCooldown / 4000f;
				}
				addedScale = MeleeWeapon.addedClubScale;
				break;
			}
			spriteBatch.Draw(Tool.weaponsTexture, location + ((this.type == 1) ? new Vector2((float)(Game1.tileSize * 2 / 3), (float)(Game1.tileSize / 3)) : new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, this.indexOfMenuItemView, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom * (scaleSize + addedScale), SpriteEffects.None, layerDepth);
			if (coolDownLevel > 0f)
			{
				spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)location.X, (int)location.Y + (Game1.tileSize - (int)(coolDownLevel * (float)Game1.tileSize)), Game1.tileSize, (int)(coolDownLevel * (float)Game1.tileSize)), Color.Red * 0.66f);
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0000846E File Offset: 0x0000666E
		public override int maximumStackSize()
		{
			return 1;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000C0FF2 File Offset: 0x000BF1F2
		public override int salePrice()
		{
			return this.getItemLevel() * 100;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000C1000 File Offset: 0x000BF200
		public static void weaponsTypeUpdate(GameTime time)
		{
			if (MeleeWeapon.addedSwordScale > 0f)
			{
				MeleeWeapon.addedSwordScale -= 0.01f;
			}
			if (MeleeWeapon.addedClubScale > 0f)
			{
				MeleeWeapon.addedClubScale -= 0.01f;
			}
			if (MeleeWeapon.addedDaggerScale > 0f)
			{
				MeleeWeapon.addedDaggerScale -= 0.01f;
			}
			if ((float)MeleeWeapon.timedHitTimer > 0f)
			{
				MeleeWeapon.timedHitTimer -= (int)time.ElapsedGameTime.TotalMilliseconds;
			}
			if (MeleeWeapon.defenseCooldown > 0)
			{
				MeleeWeapon.defenseCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.defenseCooldown <= 0)
				{
					MeleeWeapon.addedSwordScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
			if (MeleeWeapon.attackSwordCooldown > 0)
			{
				MeleeWeapon.attackSwordCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.attackSwordCooldown <= 0)
				{
					MeleeWeapon.addedSwordScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
			if (MeleeWeapon.daggerCooldown > 0)
			{
				MeleeWeapon.daggerCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.daggerCooldown <= 0)
				{
					MeleeWeapon.addedDaggerScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
			if (MeleeWeapon.clubCooldown > 0)
			{
				MeleeWeapon.clubCooldown -= time.ElapsedGameTime.Milliseconds;
				if (MeleeWeapon.clubCooldown <= 0)
				{
					MeleeWeapon.addedClubScale = 0.5f;
					Game1.playSound("objectiveComplete");
				}
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000C117C File Offset: 0x000BF37C
		public override void tickUpdate(GameTime time, Farmer who)
		{
			base.tickUpdate(time, who);
			if (this.isOnSpecial && this.type == 1 && MeleeWeapon.daggerHitsLeft > 0 && !who.usingTool)
			{
				this.quickStab(who);
				this.doDaggerFunction(who);
			}
			if (this.anotherClick)
			{
				this.leftClick(who);
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool doesShowTileLocationMarker()
		{
			return false;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x000C11D0 File Offset: 0x000BF3D0
		public int getNumberOfDescriptionCategories()
		{
			int number = 1;
			if (this.speed != ((this.type == 2) ? -8 : 0))
			{
				number++;
			}
			if (this.addedDefense > 0)
			{
				number++;
			}
			if ((double)this.critChance / 0.02 >= 2.0)
			{
				number++;
			}
			if ((double)(this.critMultiplier - 3f) / 0.02 >= 1.0)
			{
				number++;
			}
			if (this.knockback != this.defaultKnockBackForThisType(this.type))
			{
				number++;
			}
			return number;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000C1268 File Offset: 0x000BF468
		public override void leftClick(Farmer who)
		{
			if (who.health <= 0 || Game1.activeClickableMenu != null || Game1.farmEvent != null)
			{
				return;
			}
			if (this.initialParentTileIndex != 47 && who.FarmerSprite.indexInCurrentAnimation > ((this.type == 2) ? 5 : ((this.type == 1) ? 0 : 5)))
			{
				who.completelyStopAnimatingOrDoingAction();
				Game1.player.CanMove = false;
				who.UsingTool = true;
				who.canReleaseTool = true;
				this.setFarmerAnimating(Game1.player);
				return;
			}
			if (this.initialParentTileIndex != 47 && who.FarmerSprite.indexInCurrentAnimation > ((this.type == 2) ? 3 : ((this.type == 1) ? 0 : 3)))
			{
				this.anotherClick = true;
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x000C1320 File Offset: 0x000BF520
		public int getItemLevel()
		{
			int weaponPoints = 0;
			weaponPoints += (int)((double)((this.maxDamage + this.minDamage) / 2) * (1.0 + 0.1 * (double)(Math.Max(0, this.speed) + ((this.type == 1) ? 15 : 0))));
			weaponPoints += (int)((double)(this.addedPrecision / 2 + this.addedDefense) + ((double)this.critChance - 0.02) * 100.0 + (double)((this.critMultiplier - 3f) * 20f));
			if (this.type == 2)
			{
				weaponPoints /= 2;
			}
			return weaponPoints / 5 + 1;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x000C13CC File Offset: 0x000BF5CC
		public override string getDescription()
		{
			StringBuilder b = new StringBuilder();
			b.AppendLine(Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4));
			if (this.indexOfMenuItemView != 47)
			{
				b.AppendLine();
				b.AppendLine(string.Concat(new object[]
				{
					this.minDamage,
					"-",
					this.maxDamage,
					" Damage"
				}));
				if (this.speed != 0)
				{
					b.AppendLine(((this.speed > 0) ? "+" : "") + this.speed + " Speed");
				}
				if (this.addedAreaOfEffect > 0)
				{
					b.AppendLine("+" + this.addedAreaOfEffect + " Reach");
				}
				if (this.addedPrecision > 0)
				{
					b.AppendLine("+" + this.addedPrecision + " Accuracy");
				}
				if (this.addedDefense > 0)
				{
					b.AppendLine("+" + this.addedDefense + " Defense");
				}
				if ((double)this.critChance / 0.02 >= 2.0)
				{
					b.AppendLine("+" + (double)((int)this.critChance) / 0.02 + " Crit. Chance");
				}
				if ((double)(this.critMultiplier - 3f) / 0.02 >= 1.0)
				{
					b.AppendLine("+" + (int)((double)(this.critMultiplier - 3f) / 0.02) + " Crit. Power");
				}
				if (this.knockback != this.defaultKnockBackForThisType(this.type))
				{
					b.AppendLine(((this.knockback > this.defaultKnockBackForThisType(this.type)) ? "+" : "") + (int)Math.Ceiling((double)(Math.Abs(this.knockback - this.defaultKnockBackForThisType(this.type)) * 10f)) + " Knockback");
				}
			}
			return b.ToString();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000C1621 File Offset: 0x000BF821
		public float defaultKnockBackForThisType(int type)
		{
			switch (type)
			{
			case 0:
			case 3:
				return 1f;
			case 1:
				return 0.5f;
			case 2:
				return 1.5f;
			default:
				return -1f;
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000C1654 File Offset: 0x000BF854
		public virtual Rectangle getAreaOfEffect(int x, int y, int facingDirection, ref Vector2 tileLocation1, ref Vector2 tileLocation2, Rectangle wielderBoundingBox, int indexInCurrentAnimation)
		{
			Rectangle areaOfEffect = Rectangle.Empty;
			int num = this.type;
			int width;
			int height;
			int upHeightOffset;
			int horizontalYOffset;
			if (num == 1)
			{
				width = Game1.tileSize + Game1.tileSize / 6;
				height = Game1.tileSize * 3 / 4;
				upHeightOffset = Game1.tileSize * 2 / 3;
				horizontalYOffset = -Game1.tileSize / 2;
			}
			else
			{
				width = Game1.tileSize;
				height = Game1.tileSize;
				horizontalYOffset = -Game1.tileSize / 2;
				upHeightOffset = 0;
			}
			if (this.type == 1)
			{
				switch (facingDirection)
				{
				case 0:
					areaOfEffect = new Rectangle(x - width / 2, wielderBoundingBox.Y - height - upHeightOffset, width / 2, height + upHeightOffset);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Left : areaOfEffect.Right) / Game1.tileSize), (float)(areaOfEffect.Top / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Top / Game1.tileSize));
					areaOfEffect.Offset(Game1.pixelZoom * 5, -Game1.tileSize / 2 + Game1.pixelZoom * 4);
					areaOfEffect.Height += Game1.tileSize / 4;
					areaOfEffect.Width += Game1.pixelZoom * 5;
					break;
				case 1:
					areaOfEffect = new Rectangle(wielderBoundingBox.Right, y - height / 2 + horizontalYOffset, height, width);
					tileLocation1 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Top : areaOfEffect.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					areaOfEffect.Offset(-Game1.pixelZoom, 0);
					areaOfEffect.Width += Game1.tileSize / 4;
					break;
				case 2:
					areaOfEffect = new Rectangle(x - width / 2, wielderBoundingBox.Bottom, width, height);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Left : areaOfEffect.Right) / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					areaOfEffect.Offset(Game1.pixelZoom * 3, -Game1.pixelZoom * 2);
					areaOfEffect.Width -= Game1.tileSize / 3;
					break;
				case 3:
					areaOfEffect = new Rectangle(wielderBoundingBox.Left - height, y - height / 2 + horizontalYOffset, height, width);
					tileLocation1 = new Vector2((float)(areaOfEffect.Left / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Top : areaOfEffect.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Left / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					areaOfEffect.Offset(-Game1.pixelZoom * 3, 0);
					areaOfEffect.Width += Game1.tileSize / 4;
					break;
				}
			}
			else
			{
				switch (facingDirection)
				{
				case 0:
					areaOfEffect = new Rectangle(x - width / 2, wielderBoundingBox.Y - height - upHeightOffset, width, height + upHeightOffset);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Left : areaOfEffect.Right) / Game1.tileSize), (float)(areaOfEffect.Top / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Top / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						areaOfEffect.Offset(-Game1.tileSize + Game1.pixelZoom, -Game1.pixelZoom * 3);
						break;
					case 1:
						areaOfEffect.Offset(-Game1.pixelZoom * 12, -Game1.tileSize / 2 - Game1.pixelZoom * 6);
						areaOfEffect.Height += Game1.tileSize / 2;
						break;
					case 2:
						areaOfEffect.Offset(-Game1.pixelZoom * 3, -Game1.tileSize - Game1.pixelZoom);
						areaOfEffect.Height += Game1.tileSize * 3 / 4;
						break;
					case 3:
						areaOfEffect.Offset(Game1.pixelZoom * 10, -Game1.tileSize + Game1.pixelZoom);
						areaOfEffect.Height += Game1.tileSize * 3 / 4;
						break;
					case 4:
						areaOfEffect.Offset(Game1.tileSize - Game1.pixelZoom * 2, -Game1.tileSize / 2);
						areaOfEffect.Height += Game1.tileSize / 2;
						break;
					case 5:
						areaOfEffect.Offset(Game1.tileSize + Game1.pixelZoom * 3, -Game1.tileSize + Game1.pixelZoom * 8);
						break;
					}
					break;
				case 1:
					areaOfEffect = new Rectangle(wielderBoundingBox.Right, y - height / 2 + horizontalYOffset, height, width);
					tileLocation1 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Top : areaOfEffect.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						areaOfEffect.Offset(-Game1.tileSize / 2 - Game1.pixelZoom * 3, -Game1.tileSize - Game1.pixelZoom * 5);
						break;
					case 1:
						areaOfEffect.Offset(-Game1.tileSize / 2 + Game1.pixelZoom * 9, -Game1.tileSize + Game1.pixelZoom * 5);
						break;
					case 2:
						areaOfEffect.Offset(-Game1.tileSize / 8 + Game1.pixelZoom * 5, -Game1.tileSize / 4 + Game1.pixelZoom * 3);
						break;
					case 3:
						areaOfEffect.Offset(Game1.pixelZoom * 3, Game1.tileSize / 3 + Game1.pixelZoom * 4);
						break;
					case 4:
						areaOfEffect.Offset(-Game1.pixelZoom * 7, Game1.tileSize / 2 + Game1.pixelZoom * 7);
						break;
					case 5:
						areaOfEffect.Offset(-Game1.tileSize / 2 - Game1.pixelZoom * 7, Game1.tileSize + Game1.pixelZoom * 2);
						break;
					}
					break;
				case 2:
					areaOfEffect = new Rectangle(x - width / 2, wielderBoundingBox.Bottom, width, height);
					tileLocation1 = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Left : areaOfEffect.Right) / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						areaOfEffect.Offset(Game1.tileSize + Game1.pixelZoom * 2, -Game1.tileSize - Game1.pixelZoom * 7);
						break;
					case 1:
						areaOfEffect.Offset(Game1.tileSize - Game1.pixelZoom * 2, -Game1.tileSize / 2);
						break;
					case 2:
						areaOfEffect.Offset(Game1.pixelZoom * 10, -Game1.tileSize / 2 + Game1.pixelZoom);
						break;
					case 3:
						areaOfEffect.Offset(-Game1.pixelZoom * 3, -Game1.pixelZoom * 2);
						break;
					case 4:
						areaOfEffect.Offset(-Game1.pixelZoom * 20, -Game1.tileSize / 2 + Game1.pixelZoom * 2);
						areaOfEffect.Width += Game1.tileSize / 2;
						break;
					case 5:
						areaOfEffect.Offset(-Game1.tileSize - Game1.pixelZoom, -Game1.tileSize / 2 - Game1.pixelZoom * 3);
						break;
					}
					break;
				case 3:
					areaOfEffect = new Rectangle(wielderBoundingBox.Left - height, y - height / 2 + horizontalYOffset, height, width);
					tileLocation1 = new Vector2((float)(areaOfEffect.Left / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Top : areaOfEffect.Bottom) / Game1.tileSize));
					tileLocation2 = new Vector2((float)(areaOfEffect.Left / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
					switch (indexInCurrentAnimation)
					{
					case 0:
						areaOfEffect.Offset(Game1.tileSize / 2 + Game1.pixelZoom * 6, -Game1.tileSize - Game1.pixelZoom * 3);
						break;
					case 1:
						areaOfEffect.Offset(-Game1.tileSize / 2 + Game1.pixelZoom * 6, -Game1.tileSize + Game1.pixelZoom * 2);
						break;
					case 2:
						areaOfEffect.Offset(-Game1.tileSize / 8 - Game1.pixelZoom * 2, -Game1.tileSize / 4 + Game1.pixelZoom * 3);
						break;
					case 3:
						areaOfEffect.Offset(0, Game1.tileSize / 3 + Game1.pixelZoom * 4);
						break;
					case 4:
						areaOfEffect.Offset(Game1.pixelZoom * 6, Game1.tileSize / 2 + Game1.pixelZoom * 7);
						break;
					case 5:
						areaOfEffect.Offset(Game1.tileSize, Game1.tileSize);
						break;
					}
					break;
				}
			}
			areaOfEffect.Inflate(this.addedAreaOfEffect, this.addedAreaOfEffect);
			return areaOfEffect;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x000C209A File Offset: 0x000C029A
		public void doDefenseSwordFunction(Farmer who)
		{
			this.isOnSpecial = false;
			who.UsingTool = false;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000C20AA File Offset: 0x000C02AA
		public void doStabbingSwordFunction(Farmer who)
		{
			this.isOnSpecial = false;
			who.UsingTool = false;
			who.xVelocity = 0f;
			who.yVelocity = 0f;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x000C20D0 File Offset: 0x000C02D0
		public void doDaggerFunction(Farmer who)
		{
			Vector2 v = who.getUniformPositionAwayFromBox(who.facingDirection, Game1.tileSize * 3 / 4);
			float tmpKnockBack = this.knockback;
			this.knockback = 0.1f;
			this.DoDamage(Game1.currentLocation, (int)v.X, (int)v.Y, who.facingDirection, 1, who);
			this.knockback = tmpKnockBack;
			MeleeWeapon.daggerHitsLeft--;
			this.isOnSpecial = false;
			who.UsingTool = false;
			who.CanMove = true;
			who.FarmerSprite.pauseForSingleAnimation = false;
			if (MeleeWeapon.daggerHitsLeft > 0)
			{
				this.quickStab(who);
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x000C216C File Offset: 0x000C036C
		public void doClubFunction(Farmer who)
		{
			Game1.playSound("clubSmash");
			Game1.currentLocation.damageMonster(new Rectangle((int)who.position.X - Game1.tileSize * 3, who.GetBoundingBox().Y - Game1.tileSize * 3, Game1.tileSize * 6, Game1.tileSize * 6), this.minDamage, this.maxDamage, false, 1.5f, 100, 0f, 1f, false, this.lastUser);
			Game1.viewport.Y = Game1.viewport.Y - Game1.tileSize / 3;
			Game1.viewport.X = Game1.viewport.X + Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2);
			Vector2 v = who.getUniformPositionAwayFromBox(who.facingDirection, Game1.tileSize);
			switch (who.facingDirection)
			{
			case 0:
			case 2:
				v.X -= (float)(Game1.tileSize / 2);
				v.Y -= (float)(Game1.tileSize / 2);
				break;
			case 1:
				v.X -= (float)(Game1.tileSize * 2 / 3);
				v.Y -= (float)(Game1.tileSize / 2);
				break;
			case 3:
				v.Y -= (float)(Game1.tileSize / 2);
				break;
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 128, 64, 64), 40f, 4, 0, v, false, who.facingDirection == 1));
			who.jitterStrength = 2f;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x000C230A File Offset: 0x000C050A
		private void beginSpecialMove(Farmer who)
		{
			if (!Game1.fadeToBlack)
			{
				this.isOnSpecial = true;
				who.UsingTool = true;
				who.CanMove = false;
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x000C2328 File Offset: 0x000C0528
		private void quickStab(Farmer who)
		{
			switch (who.FacingDirection)
			{
			case 0:
				((FarmerSprite)who.Sprite).animateOnce(276, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(0, 0);
				break;
			case 1:
				((FarmerSprite)who.Sprite).animateOnce(274, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(1, 0);
				break;
			case 2:
				((FarmerSprite)who.Sprite).animateOnce(272, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(2, 0);
				break;
			case 3:
				((FarmerSprite)who.Sprite).animateOnce(278, 15f, 2, new AnimatedSprite.endOfAnimationBehavior(this.doDaggerFunction));
				who.CurrentTool.Update(3, 0);
				break;
			}
			this.beginSpecialMove(who);
			Game1.playSound("daggerswipe");
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x000C2444 File Offset: 0x000C0644
		public void animateSpecialMove(Farmer who)
		{
			if ((this.type == 3 && (this.name.Contains("Scythe") || this.parentSheetIndex == 47)) || Game1.fadeToBlack)
			{
				return;
			}
			if (this.type == 3 && MeleeWeapon.defenseCooldown <= 0)
			{
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).animateOnce(252, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(0, 0);
					break;
				case 1:
					((FarmerSprite)who.Sprite).animateOnce(243, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(1, 0);
					break;
				case 2:
					((FarmerSprite)who.Sprite).animateOnce(234, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(2, 0);
					break;
				case 3:
					((FarmerSprite)who.Sprite).animateOnce(259, 500f, 1, new AnimatedSprite.endOfAnimationBehavior(this.doDefenseSwordFunction));
					who.CurrentTool.Update(3, 0);
					break;
				}
				Game1.playSound("batFlap");
				this.beginSpecialMove(who);
				MeleeWeapon.defenseCooldown = 1500;
				if (who.professions.Contains(28))
				{
					MeleeWeapon.defenseCooldown /= 2;
					return;
				}
			}
			else if (this.type == 2 && MeleeWeapon.clubCooldown <= 0)
			{
				Game1.playSound("clubswipe");
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).animateOnce(176, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(0, 0);
					break;
				case 1:
					((FarmerSprite)who.Sprite).animateOnce(168, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(1, 0);
					break;
				case 2:
					((FarmerSprite)who.Sprite).animateOnce(160, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(2, 0);
					break;
				case 3:
					((FarmerSprite)who.Sprite).animateOnce(184, 40f, 8, new AnimatedSprite.endOfAnimationBehavior(this.doClubFunction));
					who.CurrentTool.Update(3, 0);
					break;
				}
				this.beginSpecialMove(who);
				MeleeWeapon.clubCooldown = 4000;
				if (who.professions.Contains(28))
				{
					MeleeWeapon.clubCooldown /= 2;
					return;
				}
			}
			else if (this.type == 1 && MeleeWeapon.daggerCooldown <= 0)
			{
				MeleeWeapon.daggerHitsLeft = 4;
				this.quickStab(who);
				MeleeWeapon.daggerCooldown = 6000;
				if (who.professions.Contains(28))
				{
					MeleeWeapon.daggerCooldown /= 2;
				}
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x000C2758 File Offset: 0x000C0958
		public static void doSwipe(int type, Vector2 position, int facingDirection, float swipeSpeed, Farmer f)
		{
			if (f == null)
			{
				return;
			}
			f.temporaryImpassableTile = Rectangle.Empty;
			f.currentLocation.lastTouchActionLocation = Vector2.Zero;
			swipeSpeed *= 1.3f;
			if (type == 3)
			{
				if (f.IsMainPlayer && f.CurrentTool != null)
				{
					switch (f.FacingDirection)
					{
					case 0:
						((FarmerSprite)f.Sprite).animateOnce(248, swipeSpeed, 6);
						f.CurrentTool.Update(0, 0);
						Game1.swordswipe(0, swipeSpeed * 2.1f, true);
						break;
					case 1:
						((FarmerSprite)f.Sprite).animateOnce(240, swipeSpeed, 6);
						f.CurrentTool.Update(1, 0);
						Game1.swordswipe(1, swipeSpeed * 2.1f, true);
						break;
					case 2:
						((FarmerSprite)f.Sprite).animateOnce(232, swipeSpeed, 6);
						f.CurrentTool.Update(2, 0);
						Game1.swordswipe(2, swipeSpeed * 2.1f, false);
						break;
					case 3:
						((FarmerSprite)f.Sprite).animateOnce(256, swipeSpeed, 6);
						f.CurrentTool.Update(3, 0);
						Game1.swordswipe(3, swipeSpeed * 2.1f, true);
						break;
					}
				}
				else if (f.facingDirection != 0)
				{
					int arg_15A_0 = f.facingDirection;
				}
				Game1.playSound("swordswipe");
				return;
			}
			if (type == 2)
			{
				if (f.IsMainPlayer && f.CurrentTool != null)
				{
					switch (f.FacingDirection)
					{
					case 0:
						((FarmerSprite)f.Sprite).animateOnce(248, swipeSpeed, 8);
						f.CurrentTool.Update(0, 0);
						break;
					case 1:
						((FarmerSprite)f.Sprite).animateOnce(240, swipeSpeed, 8);
						f.CurrentTool.Update(1, 0);
						break;
					case 2:
						((FarmerSprite)f.Sprite).animateOnce(232, swipeSpeed, 8);
						f.CurrentTool.Update(2, 0);
						break;
					case 3:
						((FarmerSprite)f.Sprite).animateOnce(256, swipeSpeed, 8);
						f.CurrentTool.Update(3, 0);
						break;
					}
				}
				Game1.playSound("clubswipe");
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x000C29B8 File Offset: 0x000C0BB8
		public void setFarmerAnimating(Farmer who)
		{
			this.anotherClick = false;
			who.FarmerSprite.pauseForSingleAnimation = false;
			who.FarmerSprite.StopAnimation();
			this.hasBegunWeaponEndPause = false;
			this.swipeSpeed = (float)(400 - this.speed * 40 - who.addedSpeed * 40);
			this.swipeSpeed *= 1f - who.weaponSpeedModifier;
			if (this.type != 1)
			{
				MeleeWeapon.doSwipe(this.type, who.position, who.facingDirection, this.swipeSpeed / (float)((this.type == 2) ? 5 : 8), who);
				who.lastClick = Vector2.Zero;
				Vector2 actionTile = who.GetToolLocation(true);
				if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
				{
					((MeleeWeapon)who.CurrentTool).DoDamage(Game1.currentLocation, (int)actionTile.X, (int)actionTile.Y, who.FacingDirection, 1, who);
				}
			}
			else
			{
				Game1.playSound("daggerswipe");
				this.swipeSpeed /= 4f;
				switch (who.FacingDirection)
				{
				case 0:
					((FarmerSprite)who.Sprite).animateOnce(276, this.swipeSpeed, 2);
					who.CurrentTool.Update(0, 0);
					break;
				case 1:
					((FarmerSprite)who.Sprite).animateOnce(274, this.swipeSpeed, 2);
					who.CurrentTool.Update(1, 0);
					break;
				case 2:
					((FarmerSprite)who.Sprite).animateOnce(272, this.swipeSpeed, 2);
					who.CurrentTool.Update(2, 0);
					break;
				case 3:
					((FarmerSprite)who.Sprite).animateOnce(278, this.swipeSpeed, 2);
					who.CurrentTool.Update(3, 0);
					break;
				}
				Vector2 actionTile2 = who.GetToolLocation(true);
				if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
				{
					((MeleeWeapon)who.CurrentTool).DoDamage(Game1.currentLocation, (int)actionTile2.X, (int)actionTile2.Y, who.FacingDirection, 1, who);
				}
			}
			if (who.CurrentTool == null)
			{
				who.completelyStopAnimatingOrDoingAction();
				who.forceCanMove();
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000C2C00 File Offset: 0x000C0E00
		public void DoDamage(GameLocation location, int x, int y, int facingDirection, int power, Farmer who)
		{
			this.isOnSpecial = false;
			if (this.type != 2)
			{
				base.DoFunction(location, x, y, power, who);
			}
			this.lastUser = who;
			Vector2 tileLocation = Vector2.Zero;
			Vector2 tileLocation2 = Vector2.Zero;
			Rectangle areaOfEffect = this.getAreaOfEffect(x, y, facingDirection, ref tileLocation, ref tileLocation2, who.GetBoundingBox(), who.FarmerSprite.indexInCurrentAnimation);
			this.mostRecentArea = areaOfEffect;
			if (who.IsMainPlayer && location.damageMonster(areaOfEffect, (int)((float)this.minDamage * (1f + who.attackIncreaseModifier)), (int)((float)this.maxDamage * (1f + who.attackIncreaseModifier)), false, this.knockback * (1f + who.knockbackModifier), (int)((float)this.addedPrecision * (1f + who.weaponPrecisionModifier)), this.critChance * (1f + who.critChanceModifier), this.critMultiplier * (1f + who.critPowerModifier), this.type != 1 || !this.isOnSpecial, this.lastUser) && this.type == 2)
			{
				Game1.playSound("clubhit");
			}
			string soundToPlay = "";
			for (int i = location.projectiles.Count - 1; i >= 0; i--)
			{
				if (areaOfEffect.Intersects(location.projectiles[i].getBoundingBox()))
				{
					location.projectiles[i].behaviorOnCollisionWithOther(location);
				}
				if (location.projectiles[i].destroyMe)
				{
					location.projectiles.RemoveAt(i);
				}
			}
			foreach (Vector2 v in Utility.removeDuplicates(Utility.getListOfTileLocationsForBordersOfNonTileRectangle(areaOfEffect)))
			{
				if (location.terrainFeatures.ContainsKey(v) && location.terrainFeatures[v].performToolAction(this, 0, v, null))
				{
					location.terrainFeatures.Remove(v);
				}
				if (location.objects.ContainsKey(v) && location.objects[v].performToolAction(this))
				{
					location.objects.Remove(v);
				}
				if (location.performToolAction(this, (int)v.X, (int)v.Y))
				{
					break;
				}
			}
			if (!soundToPlay.Equals(""))
			{
				Game1.playSound(soundToPlay);
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
			if (who != null && who.isRidingHorse())
			{
				who.completelyStopAnimatingOrDoingAction();
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000C2EA0 File Offset: 0x000C10A0
		public static Rectangle getSourceRect(int index)
		{
			return Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, index, 16, 16);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000C2EB1 File Offset: 0x000C10B1
		public void drawDuringUse(int frameOfFarmerAnimation, int facingDirection, SpriteBatch spriteBatch, Vector2 playerPosition, Farmer f)
		{
			MeleeWeapon.drawDuringUse(frameOfFarmerAnimation, facingDirection, spriteBatch, playerPosition, f, MeleeWeapon.getSourceRect(this.initialParentTileIndex), this.type, this.isOnSpecial);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000C2ED8 File Offset: 0x000C10D8
		public static void drawDuringUse(int frameOfFarmerAnimation, int facingDirection, SpriteBatch spriteBatch, Vector2 playerPosition, Farmer f, Rectangle sourceRect, int type, bool isOnSpecial)
		{
			Tool arg_07_0 = f.CurrentTool;
			if (type != 1)
			{
				if (isOnSpecial)
				{
					if (type == 3)
					{
						switch (f.FacingDirection)
						{
						case 0:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 8f, playerPosition.Y - 44f), new Rectangle?(sourceRect), Color.White, -1.76714587f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
							return;
						case 1:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 8f, playerPosition.Y - 4f), new Rectangle?(sourceRect), Color.White, -0.5890486f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 1) / 10000f));
							return;
						case 2:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 52f, playerPosition.Y + 4f), new Rectangle?(sourceRect), Color.White, -5.105088f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 2) / 10000f));
							return;
						case 3:
							spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 56f, playerPosition.Y - 4f), new Rectangle?(sourceRect), Color.White, -0.981747746f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 1) / 10000f));
							return;
						default:
							return;
						}
					}
					else if (type == 2)
					{
						if (facingDirection == 1)
						{
							switch (frameOfFarmerAnimation)
							{
							case 0:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 2) - 12f, playerPosition.Y - (float)(Game1.tileSize * 5 / 4)), new Rectangle?(sourceRect), Color.White, -1.17809725f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 1:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize, playerPosition.Y - (float)Game1.tileSize - 48f), new Rectangle?(sourceRect), Color.White, 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 2:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 2) - (float)(Game1.pixelZoom * 4), playerPosition.Y - (float)Game1.tileSize - (float)(Game1.pixelZoom * 3)), new Rectangle?(sourceRect), Color.White, 1.17809725f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 3:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 72f, playerPosition.Y - (float)Game1.tileSize + (float)(Game1.tileSize / 4) - 32f), new Rectangle?(sourceRect), Color.White, 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 4:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2), playerPosition.Y - (float)Game1.tileSize + (float)(Game1.tileSize / 4) - 16f), new Rectangle?(sourceRect), Color.White, 0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 5:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2) - 12f, playerPosition.Y - (float)Game1.tileSize + (float)(Game1.tileSize / 4)), new Rectangle?(sourceRect), Color.White, 0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 6:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2) - 16f, playerPosition.Y - (float)Game1.tileSize + (float)Game1.tileSize * 0.625f - 8f), new Rectangle?(sourceRect), Color.White, 0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 7:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize * 3 / 2) - 8f, playerPosition.Y + (float)Game1.tileSize * 0.625f), new Rectangle?(sourceRect), Color.White, 0.981747746f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							default:
								return;
							}
						}
						else if (facingDirection == 3)
						{
							switch (frameOfFarmerAnimation)
							{
							case 0:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 4f + 8f, playerPosition.Y - 56f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, 0.3926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 1:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 2), playerPosition.Y - (float)(Game1.tileSize / 2)), new Rectangle?(sourceRect), Color.White, -1.96349549f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 2:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 12f, playerPosition.Y + (float)(Game1.pixelZoom * 2)), new Rectangle?(sourceRect), Color.White, -2.74889374f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 3:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 2) - 4f, playerPosition.Y + (float)(Game1.pixelZoom * 2)), new Rectangle?(sourceRect), Color.White, -2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 4:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(Game1.tileSize / 4) - 24f, playerPosition.Y + (float)Game1.tileSize + 12f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, 4.31969f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 5:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 20f, playerPosition.Y + (float)Game1.tileSize + 40f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, 3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 6:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y + (float)Game1.tileSize + 56f), new Rectangle?(sourceRect), Color.White, 3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							case 7:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 8f, playerPosition.Y + (float)Game1.tileSize + 64f), new Rectangle?(sourceRect), Color.White, 3.73064137f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
								return;
							default:
								return;
							}
						}
						else
						{
							switch (frameOfFarmerAnimation)
							{
							case 0:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 24f, playerPosition.Y - (float)(Game1.tileSize / 3) - 8f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								break;
							case 1:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y - (float)(Game1.tileSize / 3) - (float)Game1.tileSize + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								break;
							case 2:
								spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y - (float)(Game1.tileSize / 3) + 20f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								break;
							case 3:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 2), playerPosition.Y + (float)(Game1.tileSize / 2)), new Rectangle?(sourceRect), Color.White, -3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								else
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 16f, playerPosition.Y - (float)(Game1.tileSize / 3) + 32f - (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -0.7853982f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 4:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + (float)(Game1.pixelZoom * 2), playerPosition.Y + (float)(Game1.tileSize / 2)), new Rectangle?(sourceRect), Color.White, -3.926991f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 5:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + 12f, playerPosition.Y + (float)Game1.tileSize - 20f), new Rectangle?(sourceRect), Color.White, 2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 6:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + 12f, playerPosition.Y + (float)Game1.tileSize + 54f), new Rectangle?(sourceRect), Color.White, 2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							case 7:
								if (facingDirection == 2)
								{
									spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize + 12f, playerPosition.Y + (float)Game1.tileSize + 58f), new Rectangle?(sourceRect), Color.White, 2.3561945f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
								}
								break;
							}
							if (f.facingDirection == 0)
							{
								f.FarmerRenderer.draw(spriteBatch, f.FarmerSprite, f.FarmerSprite.SourceRect, f.getLocalPosition(Game1.viewport), new Vector2(0f, (f.yOffset + (float)(Game1.tileSize * 2) - (float)(f.GetBoundingBox().Height / 2)) / 4f + 4f), Math.Max(0f, (float)f.getStandingY() / 10000f + 0.0099f), Color.White, 0f, f);
								return;
							}
						}
					}
				}
				else if (facingDirection == 1)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 40f, playerPosition.Y - (float)Game1.tileSize + 8f), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 56f, playerPosition.Y - (float)Game1.tileSize + 28f), new Rectangle?(sourceRect), Color.White, 0f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)Game1.pixelZoom, playerPosition.Y - (float)(4 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)Game1.pixelZoom, playerPosition.Y - (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(7 * Game1.pixelZoom), playerPosition.Y + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 1.96349549f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(12 * Game1.pixelZoom), playerPosition.Y + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(12 * Game1.pixelZoom), playerPosition.Y + (float)Game1.pixelZoom), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 16f, playerPosition.Y + (float)Game1.tileSize + 12f), new Rectangle?(sourceRect), Color.White, 1.96349537f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					default:
						return;
					}
				}
				else if (facingDirection == 3)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(4 * Game1.pixelZoom), playerPosition.Y - (float)Game1.tileSize - (float)(4 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)(12 * Game1.pixelZoom), playerPosition.Y - (float)Game1.tileSize + (float)(5 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, 0f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - (float)Game1.tileSize + (float)(8 * Game1.pixelZoom), playerPosition.Y + (float)(4 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() - 1) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.pixelZoom, playerPosition.Y + (float)(11 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(11 * Game1.pixelZoom), playerPosition.Y + (float)(13 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -1.96349549f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(20 * Game1.pixelZoom), playerPosition.Y + (float)(10 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(20 * Game1.pixelZoom), playerPosition.Y + (float)(10 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipHorizontally, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X - 44f, playerPosition.Y + 96f), new Rectangle?(sourceRect), Color.White, -5.105088f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.FlipVertically, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					default:
						return;
					}
				}
				else if (facingDirection == 0)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 32f, playerPosition.Y - 32f), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 32f, playerPosition.Y - 48f), new Rectangle?(sourceRect), Color.White, -1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 48f, playerPosition.Y - 52f), new Rectangle?(sourceRect), Color.White, -1.17809725f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 48f, playerPosition.Y - 52f), new Rectangle?(sourceRect), Color.White, -0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 8f, playerPosition.Y - 40f), new Rectangle?(sourceRect), Color.White, 0f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize, playerPosition.Y - 40f), new Rectangle?(sourceRect), Color.White, 0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize, playerPosition.Y - 40f), new Rectangle?(sourceRect), Color.White, 0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 44f, playerPosition.Y + (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -1.96349537f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2 - 8) / 10000f));
						return;
					default:
						return;
					}
				}
				else if (facingDirection == 2)
				{
					switch (frameOfFarmerAnimation)
					{
					case 0:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 56f, playerPosition.Y - 16f), new Rectangle?(sourceRect), Color.White, 0.3926991f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 1:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 52f, playerPosition.Y - 8f), new Rectangle?(sourceRect), Color.White, 1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 2:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 40f, playerPosition.Y), new Rectangle?(sourceRect), Color.White, 1.57079637f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 3:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 16f, playerPosition.Y + 4f), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 4:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 8f, playerPosition.Y + 8f), new Rectangle?(sourceRect), Color.White, 3.14159274f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 5:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 12f, playerPosition.Y), new Rectangle?(sourceRect), Color.White, 3.53429174f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 6:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 12f, playerPosition.Y), new Rectangle?(sourceRect), Color.White, 3.53429174f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					case 7:
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + 44f, playerPosition.Y + (float)Game1.tileSize), new Rectangle?(sourceRect), Color.White, -5.105088f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					default:
						return;
					}
				}
			}
			else
			{
				frameOfFarmerAnimation %= 2;
				if (facingDirection == 1)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(4 * Game1.pixelZoom), playerPosition.Y - 16f), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - (float)(2 * Game1.pixelZoom), playerPosition.Y - 24f), new Rectangle?(sourceRect), Color.White, 0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
					return;
				}
				else if (facingDirection == 3)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(4 * Game1.pixelZoom), playerPosition.Y - 16f), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(2 * Game1.pixelZoom), playerPosition.Y - 24f), new Rectangle?(sourceRect), Color.White, -2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize) / 10000f));
					return;
				}
				else if (facingDirection == 0)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 4f, playerPosition.Y - (float)(10 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)Game1.tileSize - 16f, playerPosition.Y - (float)(12 * Game1.pixelZoom)), new Rectangle?(sourceRect), Color.White, -0.7853982f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() - Game1.tileSize / 2) / 10000f));
					return;
				}
				else if (facingDirection == 2)
				{
					if (frameOfFarmerAnimation == 0)
					{
						spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize / 2), playerPosition.Y - 12f), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
						return;
					}
					if (frameOfFarmerAnimation != 1)
					{
						return;
					}
					spriteBatch.Draw(Tool.weaponsTexture, new Vector2(playerPosition.X + (float)(Game1.tileSize / 3), playerPosition.Y), new Rectangle?(sourceRect), Color.White, 2.3561945f, MeleeWeapon.center, (float)Game1.pixelZoom, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + Game1.tileSize / 2) / 10000f));
				}
			}
		}

		// Token: 0x040008F7 RID: 2295
		public const int defenseCooldownTime = 1500;

		// Token: 0x040008F8 RID: 2296
		public const int attackSwordCooldownTime = 2000;

		// Token: 0x040008F9 RID: 2297
		public const int daggerCooldownTime = 6000;

		// Token: 0x040008FA RID: 2298
		public const int clubCooldownTime = 4000;

		// Token: 0x040008FB RID: 2299
		public const int millisecondsPerSpeedPoint = 40;

		// Token: 0x040008FC RID: 2300
		public const int defaultSpeed = 400;

		// Token: 0x040008FD RID: 2301
		public const int stabbingSword = 0;

		// Token: 0x040008FE RID: 2302
		public const int dagger = 1;

		// Token: 0x040008FF RID: 2303
		public const int club = 2;

		// Token: 0x04000900 RID: 2304
		public const int defenseSword = 3;

		// Token: 0x04000901 RID: 2305
		public const int baseClubSpeed = -8;

		// Token: 0x04000902 RID: 2306
		public const int scythe = 47;

		// Token: 0x04000903 RID: 2307
		public int minDamage;

		// Token: 0x04000904 RID: 2308
		public int maxDamage;

		// Token: 0x04000905 RID: 2309
		public int speed;

		// Token: 0x04000906 RID: 2310
		public int addedPrecision;

		// Token: 0x04000907 RID: 2311
		public int addedDefense;

		// Token: 0x04000908 RID: 2312
		public int type;

		// Token: 0x04000909 RID: 2313
		public int addedAreaOfEffect;

		// Token: 0x0400090A RID: 2314
		public float knockback;

		// Token: 0x0400090B RID: 2315
		public float critChance;

		// Token: 0x0400090C RID: 2316
		public float critMultiplier;

		// Token: 0x0400090D RID: 2317
		public bool isOnSpecial;

		// Token: 0x0400090E RID: 2318
		public static int defenseCooldown;

		// Token: 0x0400090F RID: 2319
		public static int attackSwordCooldown;

		// Token: 0x04000910 RID: 2320
		public static int daggerCooldown;

		// Token: 0x04000911 RID: 2321
		public static int clubCooldown;

		// Token: 0x04000912 RID: 2322
		public static int daggerHitsLeft;

		// Token: 0x04000913 RID: 2323
		public static int timedHitTimer;

		// Token: 0x04000914 RID: 2324
		private static float addedSwordScale = 0f;

		// Token: 0x04000915 RID: 2325
		private static float addedClubScale = 0f;

		// Token: 0x04000916 RID: 2326
		private static float addedDaggerScale = 0f;

		// Token: 0x04000917 RID: 2327
		private bool hasBegunWeaponEndPause;

		// Token: 0x04000918 RID: 2328
		private float swipeSpeed;

		// Token: 0x04000919 RID: 2329
		[XmlIgnore]
		public Rectangle mostRecentArea;

		// Token: 0x0400091A RID: 2330
		[XmlIgnore]
		public List<Monster> monstersHitThisSwing = new List<Monster>();

		// Token: 0x0400091B RID: 2331
		private bool anotherClick;

		// Token: 0x0400091C RID: 2332
		private static Vector2 center = new Vector2(1f, 15f);
	}
}
