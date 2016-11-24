using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Monsters;

namespace StardewValley.Objects
{
	// Token: 0x02000099 RID: 153
	public class Ring : Item
	{
		// Token: 0x170000CA RID: 202
		public override int parentSheetIndex
		{
			// Token: 0x06000B07 RID: 2823 RVA: 0x000E254C File Offset: 0x000E074C
			get
			{
				return this.indexInTileSheet;
			}
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x000DD7D1 File Offset: 0x000DB9D1
		public Ring()
		{
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x000E2554 File Offset: 0x000E0754
		public Ring(int which)
		{
			string[] data = Game1.objectInformation[which].Split(new char[]
			{
				'/'
			});
			this.category = -96;
			this.Name = data[0];
			this.description = data[1];
			this.price = Convert.ToInt32(data[2]);
			this.indexInTileSheet = which;
			this.uniqueID = Game1.year + Game1.dayOfMonth + Game1.timeOfDay + this.indexInTileSheet + Game1.player.getTileX() + (int)Game1.stats.MonstersKilled + (int)Game1.stats.itemsCrafted;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000E25F4 File Offset: 0x000E07F4
		public void onEquip(Farmer who)
		{
			switch (this.indexInTileSheet)
			{
			case 516:
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 5f, new Color(0, 50, 170), this.uniqueID));
				return;
			case 517:
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 10f, new Color(0, 30, 150), this.uniqueID));
				return;
			case 518:
				who.magneticRadius += 64;
				return;
			case 519:
				who.magneticRadius += 128;
				return;
			case 520:
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 528:
				break;
			case 527:
				Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 10f, new Color(0, 80, 0), this.uniqueID));
				who.magneticRadius += 128;
				who.attackIncreaseModifier += 0.1f;
				return;
			case 529:
				who.knockbackModifier += 0.1f;
				return;
			case 530:
				who.weaponPrecisionModifier += 0.1f;
				return;
			case 531:
				who.critChanceModifier += 0.1f;
				return;
			case 532:
				who.critPowerModifier += 0.1f;
				return;
			case 533:
				who.weaponSpeedModifier += 0.1f;
				return;
			case 534:
				who.attackIncreaseModifier += 0.1f;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x000E282C File Offset: 0x000E0A2C
		public void onUnequip(Farmer who)
		{
			switch (this.indexInTileSheet)
			{
			case 516:
			case 517:
				Utility.removeLightSource(this.uniqueID);
				return;
			case 518:
				who.magneticRadius -= 64;
				return;
			case 519:
				who.magneticRadius -= 128;
				return;
			case 520:
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 528:
				break;
			case 527:
				who.magneticRadius -= 128;
				Utility.removeLightSource(this.uniqueID);
				who.attackIncreaseModifier -= 0.1f;
				return;
			case 529:
				who.knockbackModifier -= 0.1f;
				return;
			case 530:
				who.weaponPrecisionModifier -= 0.1f;
				return;
			case 531:
				who.critChanceModifier -= 0.1f;
				return;
			case 532:
				who.critPowerModifier -= 0.1f;
				return;
			case 533:
				who.weaponSpeedModifier -= 0.1f;
				return;
			case 534:
				who.attackIncreaseModifier -= 0.1f;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x000E296B File Offset: 0x000E0B6B
		public override string getCategoryName()
		{
			return "Ring";
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x000E2974 File Offset: 0x000E0B74
		public void onNewLocation(Farmer who, GameLocation environment)
		{
			int num = this.indexInTileSheet;
			if (num == 516 || num == 517)
			{
				this.onEquip(who);
				return;
			}
			if (num != 527)
			{
				return;
			}
			Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y + (float)Game1.tileSize), 10f, new Color(0, 30, 150), this.uniqueID));
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x000E2A08 File Offset: 0x000E0C08
		public void onLeaveLocation(Farmer who, GameLocation environment)
		{
			int num = this.indexInTileSheet;
			if (num == 516 || num == 517)
			{
				this.onUnequip(who);
				return;
			}
			if (num != 527)
			{
				return;
			}
			Utility.removeLightSource(this.uniqueID);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x000E2A49 File Offset: 0x000E0C49
		public override int salePrice()
		{
			return this.price;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x000E2A54 File Offset: 0x000E0C54
		public void onMonsterSlay(Monster m)
		{
			switch (this.indexInTileSheet)
			{
			case 521:
				if (Game1.random.NextDouble() < 0.1 + (double)((float)Game1.player.LuckLevel / 100f))
				{
					Game1.buffsDisplay.addOtherBuff(new Buff(20));
					Game1.playSound("warrior");
					return;
				}
				break;
			case 522:
				Game1.player.health = Math.Min(Game1.player.maxHealth, Game1.player.health + 2);
				return;
			case 523:
				Game1.buffsDisplay.addOtherBuff(new Buff(22));
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000E2B00 File Offset: 0x000E0D00
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scaleSize, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.indexInTileSheet, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f) * scaleSize, scaleSize * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000E2B84 File Offset: 0x000E0D84
		public void update(GameTime time, GameLocation environment, Farmer who)
		{
			int num = this.indexInTileSheet;
			if (num <= 517)
			{
				if (num != 516 && num != 517)
				{
					return;
				}
			}
			else if (num != 527)
			{
				return;
			}
			Utility.repositionLightSource(this.uniqueID, new Vector2(Game1.player.position.X + (float)(Game1.tileSize / 3), who.position.Y));
			if (!environment.isOutdoors && !(environment is MineShaft))
			{
				LightSource i = Utility.getLightSource(this.uniqueID);
				if (i != null)
				{
					i.radius = 3f;
				}
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0000846E File Offset: 0x0000666E
		public override int maximumStackSize()
		{
			return 1;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0000846E File Offset: 0x0000666E
		public override int getStack()
		{
			return 1;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0000846E File Offset: 0x0000666E
		public override int addToStack(int amount)
		{
			return 1;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x000E2C1F File Offset: 0x000E0E1F
		public override string getDescription()
		{
			return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPlaceable()
		{
			return false;
		}

		// Token: 0x170000CB RID: 203
		public override string Name
		{
			// Token: 0x06000B18 RID: 2840 RVA: 0x000E2C40 File Offset: 0x000E0E40
			get
			{
				return this.name;
			}
			// Token: 0x06000B19 RID: 2841 RVA: 0x000E2C48 File Offset: 0x000E0E48
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000CC RID: 204
		public override int Stack
		{
			// Token: 0x06000B1A RID: 2842 RVA: 0x0000846E File Offset: 0x0000666E
			get
			{
				return 1;
			}
			// Token: 0x06000B1B RID: 2843 RVA: 0x00002834 File Offset: 0x00000A34
			set
			{
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x000E2C51 File Offset: 0x000E0E51
		public override Item getOne()
		{
			return new Ring(this.indexInTileSheet);
		}

		// Token: 0x04000B38 RID: 2872
		public const int ringLowerIndexRange = 516;

		// Token: 0x04000B39 RID: 2873
		public const int slimeCharmer = 520;

		// Token: 0x04000B3A RID: 2874
		public const int yobaRing = 524;

		// Token: 0x04000B3B RID: 2875
		public const int sturdyRing = 525;

		// Token: 0x04000B3C RID: 2876
		public const int burglarsRing = 526;

		// Token: 0x04000B3D RID: 2877
		public const int jukeboxRing = 528;

		// Token: 0x04000B3E RID: 2878
		public const int ringUpperIndexRange = 534;

		// Token: 0x04000B3F RID: 2879
		public string description;

		// Token: 0x04000B40 RID: 2880
		public string name;

		// Token: 0x04000B41 RID: 2881
		public int price;

		// Token: 0x04000B42 RID: 2882
		public int indexInTileSheet;

		// Token: 0x04000B43 RID: 2883
		public int uniqueID;
	}
}
