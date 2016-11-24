using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewValley
{
	// Token: 0x0200000E RID: 14
	public class Buff
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00006EAC File Offset: 0x000050AC
		public Buff(string description, int millisecondsDuration, string source, int index)
		{
			this.description = description;
			this.millisecondsDuration = millisecondsDuration;
			this.sheetIndex = index;
			this.source = source;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00006EEC File Offset: 0x000050EC
		public Buff(int which)
		{
			this.which = which;
			this.sheetIndex = which;
			bool negative = true;
			switch (which)
			{
			case 6:
				this.description = "Full";
				this.millisecondsDuration = 180000;
				negative = false;
				break;
			case 7:
				this.description = "Quenched";
				this.millisecondsDuration = 60000;
				negative = false;
				break;
			case 12:
				this.description = string.Concat(new string[]
				{
					"Goblin's Curse",
					Environment.NewLine,
					"-3 Speed",
					Environment.NewLine,
					"-3 Defense"
				});
				this.buffAttributes[9] = -3;
				this.buffAttributes[10] = -3;
				this.buffAttributes[11] = -3;
				this.glow = Color.Yellow;
				this.millisecondsDuration = 6000;
				break;
			case 13:
				this.description = "Slimed" + Environment.NewLine + "-4 Speed";
				this.buffAttributes[9] = -4;
				this.glow = Color.Green;
				this.millisecondsDuration = 2500 + Game1.random.Next(500);
				break;
			case 14:
				this.description = "Jinxed" + Environment.NewLine + "-8 Defense";
				this.buffAttributes[10] = -8;
				this.glow = Color.HotPink;
				this.millisecondsDuration = 8000;
				break;
			case 17:
				this.description = "Tipsy" + Environment.NewLine + "-1 Speed";
				this.buffAttributes[9] = -1;
				this.glow = Color.OrangeRed * 0.5f;
				this.millisecondsDuration = 30000;
				break;
			case 18:
				this.description = "Spooked" + Environment.NewLine + "-8 Attack";
				this.buffAttributes[11] = -8;
				this.glow = new Color(50, 0, 30);
				this.millisecondsDuration = 8000;
				break;
			case 19:
				this.description = "Frozen" + Environment.NewLine + "-8 Speed";
				this.buffAttributes[9] = -8;
				this.glow = Color.LightBlue;
				this.millisecondsDuration = 2000;
				break;
			case 20:
				this.description = "Warrior Energy" + Environment.NewLine + "+10 Attack";
				this.buffAttributes[11] = 10;
				this.glow = Color.Red;
				this.millisecondsDuration = 5000;
				negative = false;
				break;
			case 21:
				this.description = "Yoba's Blessing" + Environment.NewLine + "Invincible";
				this.glow = Color.Orange;
				this.millisecondsDuration = 5000;
				negative = false;
				break;
			case 22:
				this.description = "Adrenaline Rush" + Environment.NewLine + "+2 Speed";
				this.glow = Color.Cyan;
				this.millisecondsDuration = 3000;
				this.sheetIndex = 9;
				this.buffAttributes[9] = 2;
				negative = false;
				break;
			case 23:
				this.description = "Oil of Garlic" + Environment.NewLine + "Your skin exudes a pungent aroma";
				this.glow = Color.LightGreen * 0.25f;
				this.millisecondsDuration = 600000;
				negative = false;
				break;
			}
			if (negative && Game1.player.isWearingRing(525))
			{
				this.millisecondsDuration /= 2;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000072AC File Offset: 0x000054AC
		public Buff(int farming, int fishing, int mining, int digging, int luck, int foraging, int crafting, int maxStamina, int magneticRadius, int speed, int defense, int attack, int minutesDuration, string source)
		{
			this.buffAttributes[0] = farming;
			this.buffAttributes[1] = fishing;
			this.buffAttributes[2] = mining;
			this.buffAttributes[4] = luck;
			this.buffAttributes[5] = foraging;
			this.buffAttributes[6] = crafting;
			this.buffAttributes[7] = maxStamina;
			this.buffAttributes[8] = magneticRadius;
			this.buffAttributes[9] = speed;
			this.buffAttributes[10] = defense;
			this.buffAttributes[11] = attack;
			this.total = Math.Abs(this.buffAttributes[0]) + Math.Abs(this.buffAttributes[2]) + Math.Abs(this.buffAttributes[1]) + Math.Abs(this.buffAttributes[4]) + Math.Abs(this.buffAttributes[5]) + Math.Abs(this.buffAttributes[6]) + Math.Abs(this.buffAttributes[7]) + Math.Abs(this.buffAttributes[8]) + Math.Abs(this.buffAttributes[9]) + Math.Abs(this.buffAttributes[10]) + Math.Abs(this.buffAttributes[11]);
			this.millisecondsDuration = Math.Min(2400 - Game1.timeOfDay, minutesDuration) / 10 * 7000;
			this.source = source;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00007414 File Offset: 0x00005614
		public string getTimeLeft()
		{
			return string.Concat(new object[]
			{
				"Duration: ",
				this.millisecondsDuration / 60000,
				":",
				this.millisecondsDuration % 60000 / 10000,
				this.millisecondsDuration % 60000 % 10000 / 1000
			});
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000748C File Offset: 0x0000568C
		public bool update(GameTime time)
		{
			int old = this.millisecondsDuration;
			this.millisecondsDuration -= time.ElapsedGameTime.Milliseconds;
			if (this.which == 13 && old % 500 < this.millisecondsDuration % 500 && old < 3000)
			{
				Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, Game1.player.getStandingPosition() + new Vector2((float)(-(float)Game1.tileSize / 2 - Game1.pixelZoom * 2 + Game1.random.Next(-Game1.pixelZoom * 2, Game1.pixelZoom * 3)), (float)Game1.random.Next(-Game1.tileSize / 2, -Game1.tileSize / 4)), Color.Green * 0.5f, 8, Game1.random.NextDouble() < 0.5, 70f, 0, -1, -1f, -1, 0)
				{
					scale = 1f
				});
			}
			return this.millisecondsDuration <= 0;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000075AC File Offset: 0x000057AC
		public void addBuff()
		{
			Game1.player.addedFarmingLevel += this.buffAttributes[0];
			Game1.player.addedFishingLevel += this.buffAttributes[1];
			Game1.player.addedMiningLevel += this.buffAttributes[2];
			Game1.player.addedLuckLevel += this.buffAttributes[4];
			Game1.player.addedForagingLevel += this.buffAttributes[5];
			Game1.player.CraftingTime -= this.buffAttributes[6];
			Game1.player.MaxStamina += this.buffAttributes[7];
			Game1.player.MagneticRadius += this.buffAttributes[8];
			Game1.player.resilience += this.buffAttributes[10];
			Game1.player.attack += this.buffAttributes[11];
			Game1.player.addedSpeed += this.buffAttributes[9];
			Color arg_11C_0 = this.glow;
			if (!this.glow.Equals(Color.White))
			{
				Game1.player.startGlowing(this.glow, false, 0.05f);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00007700 File Offset: 0x00005900
		public string getDescription(int which)
		{
			StringBuilder s = new StringBuilder();
			if (this.description != null && this.description.Length > 1)
			{
				s.AppendLine(this.description);
			}
			else
			{
				if (which == 0)
				{
					s.AppendLine(((this.buffAttributes[0] > 0) ? "+" : "-") + this.buffAttributes[0] + " Farming");
				}
				if (which == 1)
				{
					s.AppendLine(((this.buffAttributes[1] > 0) ? "+" : "-") + this.buffAttributes[1] + " Fishing");
				}
				if (which == 2)
				{
					s.AppendLine(((this.buffAttributes[2] > 0) ? "+" : "-") + this.buffAttributes[2] + " Mining");
				}
				if (which == 4)
				{
					s.AppendLine(((this.buffAttributes[4] > 0) ? "+" : "-") + this.buffAttributes[4] + " Luck");
				}
				if (which == 5)
				{
					s.AppendLine(((this.buffAttributes[5] > 0) ? "+" : "-") + this.buffAttributes[5] + " Foraging");
				}
				if (which == 7)
				{
					s.AppendLine(((this.buffAttributes[7] > 0) ? "+" : "-") + this.buffAttributes[7] + " Max Energy");
				}
				if (which == 8)
				{
					s.AppendLine(((this.buffAttributes[8] > 0) ? "+" : "-") + this.buffAttributes[8] + " Magnetic Radius");
				}
				if (which == 10)
				{
					s.AppendLine(((this.buffAttributes[10] > 0) ? "+" : "-") + this.buffAttributes[10] + " Defense");
				}
				if (which == 11)
				{
					s.AppendLine(((this.buffAttributes[11] > 0) ? "+" : "-") + this.buffAttributes[11] + " Attack");
				}
				if (which == 9)
				{
					s.AppendLine(((this.buffAttributes[9] > 0) ? "+" : "-") + this.buffAttributes[9] + " Speed");
				}
			}
			if (this.source != null && !this.source.Equals(""))
			{
				s.AppendLine("Source: " + this.source);
			}
			return s.ToString();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000079B4 File Offset: 0x00005BB4
		public bool betterThan(Buff other)
		{
			return this.total > 0 && (other == null || this.total > other.total);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000079D8 File Offset: 0x00005BD8
		public void removeBuff()
		{
			if (this.buffAttributes[0] != 0)
			{
				Game1.player.addedFarmingLevel = Math.Max(0, Game1.player.addedFarmingLevel - this.buffAttributes[0]);
			}
			if (this.buffAttributes[1] != 0)
			{
				Game1.player.addedFishingLevel = Math.Max(0, Game1.player.addedFishingLevel - this.buffAttributes[1]);
			}
			if (this.buffAttributes[2] != 0)
			{
				Game1.player.addedMiningLevel = Math.Max(0, Game1.player.addedMiningLevel - this.buffAttributes[2]);
			}
			if (this.buffAttributes[4] != 0)
			{
				Game1.player.addedLuckLevel = Math.Max(0, Game1.player.addedLuckLevel - this.buffAttributes[4]);
			}
			if (this.buffAttributes[5] != 0)
			{
				Game1.player.addedForagingLevel = Math.Max(0, Game1.player.addedForagingLevel - this.buffAttributes[5]);
			}
			if (this.buffAttributes[6] != 0)
			{
				Game1.player.CraftingTime = Math.Max(0, Game1.player.CraftingTime - this.buffAttributes[6]);
			}
			if (this.buffAttributes[7] != 0)
			{
				Game1.player.MaxStamina = Math.Max(0, Game1.player.MaxStamina - this.buffAttributes[7]);
			}
			if (this.buffAttributes[8] != 0)
			{
				Game1.player.MagneticRadius = Math.Max(0, Game1.player.MagneticRadius - this.buffAttributes[8]);
			}
			if (this.buffAttributes[10] != 0)
			{
				Game1.player.resilience = Math.Max(0, Game1.player.resilience - this.buffAttributes[10]);
			}
			if (this.buffAttributes[9] != 0)
			{
				if (this.buffAttributes[9] < 0)
				{
					Game1.player.addedSpeed += Math.Abs(this.buffAttributes[9]);
				}
				else
				{
					Game1.player.addedSpeed -= this.buffAttributes[9];
				}
			}
			if (this.buffAttributes[11] != 0)
			{
				if (this.buffAttributes[11] < 0)
				{
					Game1.player.attack += Math.Abs(this.buffAttributes[11]);
				}
				else
				{
					Game1.player.attack -= this.buffAttributes[11];
				}
			}
			Color arg_241_0 = this.glow;
			if (!this.glow.Equals(Color.White))
			{
				Game1.player.stopGlowing();
				foreach (Buff b in Game1.buffsDisplay.otherBuffs)
				{
					if (!b.Equals(this))
					{
						Color arg_287_0 = b.glow;
						if (!b.glow.Equals(Color.White))
						{
							Game1.player.startGlowing(b.glow, false, 0.05f);
						}
					}
				}
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00007CC0 File Offset: 0x00005EC0
		public List<ClickableTextureComponent> getClickableComponents()
		{
			Dictionary<int, int> sourceRects = new Dictionary<int, int>();
			if (this.sheetIndex != -1)
			{
				sourceRects.Add(this.sheetIndex, 0);
			}
			else
			{
				if (this.buffAttributes[0] != 0)
				{
					sourceRects.Add(0, this.buffAttributes[0]);
				}
				if (this.buffAttributes[1] != 0)
				{
					sourceRects.Add(1, this.buffAttributes[1]);
				}
				if (this.buffAttributes[2] != 0)
				{
					sourceRects.Add(2, this.buffAttributes[2]);
				}
				if (this.buffAttributes[4] != 0)
				{
					sourceRects.Add(4, this.buffAttributes[4]);
				}
				if (this.buffAttributes[5] != 0)
				{
					sourceRects.Add(5, this.buffAttributes[5]);
				}
				if (this.buffAttributes[7] != 0)
				{
					sourceRects.Add(16, this.buffAttributes[7]);
				}
				if (this.buffAttributes[11] != 0)
				{
					sourceRects.Add(11, this.buffAttributes[11]);
				}
				if (this.buffAttributes[8] != 0)
				{
					sourceRects.Add(8, this.buffAttributes[8]);
				}
				if (this.buffAttributes[10] != 0)
				{
					sourceRects.Add(10, this.buffAttributes[10]);
				}
				if (this.buffAttributes[9] != 0)
				{
					sourceRects.Add(9, this.buffAttributes[9]);
				}
			}
			List<ClickableTextureComponent> components = new List<ClickableTextureComponent>();
			foreach (KeyValuePair<int, int> kvp in sourceRects)
			{
				components.Add(new ClickableTextureComponent("", Rectangle.Empty, null, this.getDescription(Buff.getAttributeIndexFromSourceRectIndex(kvp.Key)), Game1.buffsIcons, Game1.getSourceRectForStandardTileSheet(Game1.buffsIcons, kvp.Key, 16, 16), (float)Game1.pixelZoom, false));
			}
			return components;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00007E7C File Offset: 0x0000607C
		public static int getAttributeIndexFromSourceRectIndex(int index)
		{
			if (index == 16)
			{
				return 7;
			}
			return index;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007E88 File Offset: 0x00006088
		public static string getBuffTypeFromBuffDescriptionIndex(int index)
		{
			string type = "";
			switch (index)
			{
			case 0:
				type = "farming";
				break;
			case 1:
				type = "fishing";
				break;
			case 2:
				type = "mining";
				break;
			case 3:
				type = "digging";
				break;
			case 4:
				type = "luck";
				break;
			case 5:
				type = "foraging";
				break;
			case 6:
				type = "crafting speed";
				break;
			case 7:
				type = "max energy";
				break;
			case 8:
				type = "magnetism";
				break;
			case 9:
				type = "speed";
				break;
			case 10:
				type = "defense";
				break;
			case 11:
				type = "attack";
				break;
			}
			return type;
		}

		// Token: 0x0400008C RID: 140
		public const float glowRate = 0.05f;

		// Token: 0x0400008D RID: 141
		public const int farming = 0;

		// Token: 0x0400008E RID: 142
		public const int fishing = 1;

		// Token: 0x0400008F RID: 143
		public const int mining = 2;

		// Token: 0x04000090 RID: 144
		public const int luck = 4;

		// Token: 0x04000091 RID: 145
		public const int foraging = 5;

		// Token: 0x04000092 RID: 146
		public const int crafting = 6;

		// Token: 0x04000093 RID: 147
		public const int maxStamina = 7;

		// Token: 0x04000094 RID: 148
		public const int magneticRadius = 8;

		// Token: 0x04000095 RID: 149
		public const int speed = 9;

		// Token: 0x04000096 RID: 150
		public const int defense = 10;

		// Token: 0x04000097 RID: 151
		public const int attack = 11;

		// Token: 0x04000098 RID: 152
		public const int totalNumberOfBuffableAttriutes = 12;

		// Token: 0x04000099 RID: 153
		public const int goblinsCurse = 12;

		// Token: 0x0400009A RID: 154
		public const int slimed = 13;

		// Token: 0x0400009B RID: 155
		public const int evilEye = 14;

		// Token: 0x0400009C RID: 156
		public const int chickenedOut = 15;

		// Token: 0x0400009D RID: 157
		public const int tipsy = 17;

		// Token: 0x0400009E RID: 158
		public const int fear = 18;

		// Token: 0x0400009F RID: 159
		public const int frozen = 19;

		// Token: 0x040000A0 RID: 160
		public const int warriorEnergy = 20;

		// Token: 0x040000A1 RID: 161
		public const int yobaBlessing = 21;

		// Token: 0x040000A2 RID: 162
		public const int adrenalineRush = 22;

		// Token: 0x040000A3 RID: 163
		public const int avoidMonsters = 23;

		// Token: 0x040000A4 RID: 164
		public const int full = 6;

		// Token: 0x040000A5 RID: 165
		public const int quenched = 7;

		// Token: 0x040000A6 RID: 166
		public int millisecondsDuration;

		// Token: 0x040000A7 RID: 167
		private int[] buffAttributes = new int[12];

		// Token: 0x040000A8 RID: 168
		public string description;

		// Token: 0x040000A9 RID: 169
		public string source;

		// Token: 0x040000AA RID: 170
		public int total;

		// Token: 0x040000AB RID: 171
		public int sheetIndex = -1;

		// Token: 0x040000AC RID: 172
		public int which = -1;

		// Token: 0x040000AD RID: 173
		public Color glow;
	}
}
