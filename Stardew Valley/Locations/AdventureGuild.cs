using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x0200011D RID: 285
	public class AdventureGuild : GameLocation
	{
		// Token: 0x06001045 RID: 4165 RVA: 0x00150C20 File Offset: 0x0014EE20
		public AdventureGuild()
		{
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00150C6C File Offset: 0x0014EE6C
		public AdventureGuild(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00150CB8 File Offset: 0x0014EEB8
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int tileIndexOfCheckLocation = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (tileIndexOfCheckLocation <= 1292)
			{
				if (tileIndexOfCheckLocation != 1291 && tileIndexOfCheckLocation != 1292)
				{
					goto IL_91;
				}
			}
			else
			{
				if (tileIndexOfCheckLocation == 1306)
				{
					this.showMonsterKillList();
					return true;
				}
				switch (tileIndexOfCheckLocation)
				{
				case 1355:
				case 1356:
				case 1357:
				case 1358:
					break;
				default:
					goto IL_91;
				}
			}
			this.gil();
			return true;
			IL_91:
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00150D5F File Offset: 0x0014EF5F
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.talkedToGil = false;
			if (!Game1.player.mailReceived.Contains("guildMember"))
			{
				Game1.player.mailReceived.Add("guildMember");
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00150D98 File Offset: 0x0014EF98
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (!Game1.player.mailReceived.Contains("checkedMonsterBoard"))
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(8 * Game1.tileSize - 8), (float)(9 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(10 * Game1.tileSize) / 10000f + 1E-06f + 0.0008f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(8 * Game1.tileSize + Game1.tileSize / 2), (float)(9 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0f, new Vector2(6f, 6f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(10 * Game1.tileSize) / 10000f + 1E-05f + 0.0008f);
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00150F28 File Offset: 0x0014F128
		private string killListLine(string monsterType, int killCount, int target)
		{
			string monsterNamePlural = Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_" + monsterType, new object[0]);
			if (killCount == 0)
			{
				return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat_None", new object[]
				{
					killCount,
					target,
					monsterNamePlural
				}) + "^";
			}
			if (killCount >= target)
			{
				return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat_OverTarget", new object[]
				{
					killCount,
					target,
					monsterNamePlural
				}) + "^";
			}
			return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat", new object[]
			{
				killCount,
				target,
				monsterNamePlural
			}) + "^";
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00150FFC File Offset: 0x0014F1FC
		public void showMonsterKillList()
		{
			if (!Game1.player.mailReceived.Contains("checkedMonsterBoard"))
			{
				Game1.player.mailReceived.Add("checkedMonsterBoard");
			}
			StringBuilder s = new StringBuilder();
			s.Append(Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_Header", new object[0]).Replace('\n', '^') + "^");
			int slimesKilled = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int shadowsKilled = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int skeletonsKilled = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			int caveInsectsKilled = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int batsKilled = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int duggyKilled = Game1.stats.getMonstersKilled("Duggy");
			int dustSpiritKilled = Game1.stats.getMonstersKilled("Dust Spirit");
			s.Append(this.killListLine("Slimes", slimesKilled, 1000));
			s.Append(this.killListLine("VoidSpirits", shadowsKilled, 150));
			s.Append(this.killListLine("Bats", batsKilled, 200));
			s.Append(this.killListLine("Skeletons", skeletonsKilled, 50));
			s.Append(this.killListLine("CaveInsects", caveInsectsKilled, 125));
			s.Append(this.killListLine("Duggies", duggyKilled, 30));
			s.Append(this.killListLine("DustSprites", dustSpiritKilled, 500));
			s.Append(Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_Footer", new object[0]).Replace('\n', '^'));
			Game1.drawLetterMessage(s.ToString());
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0015123F File Offset: 0x0014F43F
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			Game1.changeMusicTrack("none");
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00151254 File Offset: 0x0014F454
		public static bool areAllMonsterSlayerQuestsComplete()
		{
			int arg_146_0 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int shadowsKilled = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int skeletonsKilled = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			Game1.stats.getMonstersKilled("Rock Crab");
			Game1.stats.getMonstersKilled("Lava Crab");
			int caveInsectsKilled = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int batsKilled = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int duggyKilled = Game1.stats.getMonstersKilled("Duggy");
			Game1.stats.getMonstersKilled("Metal Head");
			Game1.stats.getMonstersKilled("Stone Golem");
			int dustSpiritKilled = Game1.stats.getMonstersKilled("Dust Spirit");
			return arg_146_0 >= 1000 && shadowsKilled >= 150 && skeletonsKilled >= 50 && caveInsectsKilled >= 125 && batsKilled >= 200 && duggyKilled >= 30 && dustSpiritKilled >= 500;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x001513E4 File Offset: 0x0014F5E4
		public static bool willThisKillCompleteAMonsterSlayerQuest(string nameOfMonster)
		{
			int arg_144_0 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int shadowsKilled = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int skeletonsKilled = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			int crabsKilled = Game1.stats.getMonstersKilled("Rock Crab") + Game1.stats.getMonstersKilled("Lava Crab");
			int caveInsectsKilled = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int batsKilled = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int duggyKilled = Game1.stats.getMonstersKilled("Duggy");
			int metalHeadKilled = Game1.stats.getMonstersKilled("Metal Head");
			int golemKilled = Game1.stats.getMonstersKilled("Stone Golem");
			int dustSpiritKilled = Game1.stats.getMonstersKilled("Dust Spirit");
			int slimesKilledNew = arg_144_0 + ((nameOfMonster.Equals("Green Slime") || nameOfMonster.Equals("Frost Jelly") || nameOfMonster.Equals("Sludge")) ? 1 : 0);
			int shadowsKilledNew = shadowsKilled + ((nameOfMonster.Equals("Shadow Guy") || nameOfMonster.Equals("Shadow Shaman") || nameOfMonster.Equals("Shadow Brute")) ? 1 : 0);
			int skeletonsKilledNew = skeletonsKilled + ((nameOfMonster.Equals("Skeleton") || nameOfMonster.Equals("Skeleton Mage")) ? 1 : 0);
			if (!nameOfMonster.Equals("Rock Crab"))
			{
				nameOfMonster.Equals("Lava Crab");
			}
			int caveInsectsKilledNew = caveInsectsKilled + ((nameOfMonster.Equals("Grub") || nameOfMonster.Equals("Fly") || nameOfMonster.Equals("Bug")) ? 1 : 0);
			int batsKilledNew = batsKilled + ((nameOfMonster.Equals("Bat") || nameOfMonster.Equals("Frost Bat") || nameOfMonster.Equals("Lava Bat")) ? 1 : 0);
			int duggyKilledNew = duggyKilled + (nameOfMonster.Equals("Duggy") ? 1 : 0);
			nameOfMonster.Equals("Metal Head");
			nameOfMonster.Equals("Stone Golem");
			int dustSpiritKilledNew = dustSpiritKilled + (nameOfMonster.Equals("Dust Spirit") ? 1 : 0);
			return (arg_144_0 < 1000 && slimesKilledNew >= 1000 && !Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring")) || (shadowsKilled < 150 && shadowsKilledNew >= 150 && !Game1.player.mailReceived.Contains("Gil_Savage Ring")) || (skeletonsKilled < 50 && skeletonsKilledNew >= 50 && !Game1.player.mailReceived.Contains("Gil_Skeleton Mask")) || (caveInsectsKilled < 125 && caveInsectsKilledNew >= 125 && !Game1.player.mailReceived.Contains("Gil_Insect Head")) || (batsKilled < 200 && batsKilledNew >= 200 && !Game1.player.mailReceived.Contains("Gil_Vampire Ring")) || (duggyKilled < 30 && duggyKilledNew >= 30 && !Game1.player.mailReceived.Contains("Gil_Hard Hat")) || (dustSpiritKilled < 500 && dustSpiritKilledNew >= 500 && !Game1.player.mailReceived.Contains("Gil_Burglar's Ring"));
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00151784 File Offset: 0x0014F984
		private void gil()
		{
			List<Item> rewards = new List<Item>();
			int arg_171_0 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
			int shadowsKilled = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
			int skeletonsKilled = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
			int goblinsKilled = Game1.stats.getMonstersKilled("Goblin Warrior") + Game1.stats.getMonstersKilled("Goblin Wizard");
			int crabsKilled = Game1.stats.getMonstersKilled("Rock Crab") + Game1.stats.getMonstersKilled("Lava Crab");
			int caveInsectsKilled = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
			int batsKilled = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
			int duggyKilled = Game1.stats.getMonstersKilled("Duggy");
			int metalHeadKilled = Game1.stats.getMonstersKilled("Metal Head");
			int golemKilled = Game1.stats.getMonstersKilled("Stone Golem");
			int dustSpiritKilled = Game1.stats.getMonstersKilled("Dust Spirit");
			if (arg_171_0 >= 1000 && !Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring"))
			{
				rewards.Add(new Ring(520));
			}
			if (shadowsKilled >= 150 && !Game1.player.mailReceived.Contains("Gil_Savage Ring"))
			{
				rewards.Add(new Ring(523));
			}
			if (skeletonsKilled >= 50 && !Game1.player.mailReceived.Contains("Gil_Skeleton Mask"))
			{
				rewards.Add(new Hat(8));
			}
			if (goblinsKilled >= 50)
			{
				Game1.player.specialItems.Contains(9);
			}
			if (crabsKilled >= 60)
			{
				Game1.player.specialItems.Contains(524);
			}
			if (caveInsectsKilled >= 125 && !Game1.player.mailReceived.Contains("Gil_Insect Head"))
			{
				rewards.Add(new MeleeWeapon(13));
			}
			if (batsKilled >= 200 && !Game1.player.mailReceived.Contains("Gil_Vampire Ring"))
			{
				rewards.Add(new Ring(522));
			}
			if (duggyKilled >= 30 && !Game1.player.mailReceived.Contains("Gil_Hard Hat"))
			{
				rewards.Add(new Hat(27));
			}
			if (metalHeadKilled >= 50)
			{
				Game1.player.specialItems.Contains(519);
			}
			if (golemKilled >= 50)
			{
				Game1.player.specialItems.Contains(517);
			}
			if (dustSpiritKilled >= 500 && !Game1.player.mailReceived.Contains("Gil_Burglar's Ring"))
			{
				rewards.Add(new Ring(526));
			}
			foreach (Item i in rewards)
			{
				if (i is Object)
				{
					(i as Object).specialItem = true;
				}
				else if (!Game1.player.hasOrWillReceiveMail("Gil_" + i.Name))
				{
					Game1.player.mailReceived.Add("Gil_" + i.Name);
				}
			}
			if (rewards.Count > 0)
			{
				Game1.activeClickableMenu = new ItemGrabMenu(rewards);
				return;
			}
			if (this.talkedToGil)
			{
				Game1.drawDialogue(this.Gil, Game1.content.LoadString("Characters\\Dialogue\\Gil:Snoring", new object[0]));
			}
			else
			{
				Game1.drawDialogue(this.Gil, Game1.content.LoadString("Characters\\Dialogue\\Gil:ComeBackLater", new object[0]));
			}
			this.talkedToGil = true;
		}

		// Token: 0x040011C4 RID: 4548
		private NPC Gil = new NPC(null, new Vector2(-1000f, -1000f), "AdventureGuild", 2, "Gil", false, null, Game1.content.Load<Texture2D>("Portraits\\Gil"));

		// Token: 0x040011C5 RID: 4549
		private bool talkedToGil;
	}
}
