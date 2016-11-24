using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace StardewValley.Quests
{
	// Token: 0x02000084 RID: 132
	public class SlayMonsterQuest : Quest
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x000D5A28 File Offset: 0x000D3C28
		public SlayMonsterQuest()
		{
			if (this.questTitle.Length > 0)
			{
				return;
			}
			this.questTitle = "Slay Monsters";
			this.questType = 4;
			List<string> possibleMonsters = new List<string>();
			int mineLevel = Game1.player.deepestMineLevel;
			if (mineLevel < 39)
			{
				possibleMonsters.Add("Slime");
				if (mineLevel > 10)
				{
					possibleMonsters.Add("Rock Crab");
				}
				if (mineLevel > 30)
				{
					possibleMonsters.Add("Duggy");
				}
			}
			else if (mineLevel < 79)
			{
				possibleMonsters.Add("Frost Jelly");
				possibleMonsters.Add("Skeleton");
				possibleMonsters.Add("Dust Spirit");
			}
			else
			{
				possibleMonsters.Add("Sludge");
				possibleMonsters.Add("Ghost");
				possibleMonsters.Add("Lava Crab");
				possibleMonsters.Add("Squid Kid");
			}
			this.monsterName = possibleMonsters.ElementAt(this.random.Next(possibleMonsters.Count));
			string text = this.monsterName;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 703662834u)
			{
				if (num <= 503018864u)
				{
					if (num != 165007071u)
					{
						if (num == 503018864u)
						{
							if (text == "Ghost")
							{
								this.numberToKill = this.random.Next(1, 3);
								this.reward = this.numberToKill * 250;
							}
						}
					}
					else if (text == "Lava Crab")
					{
						this.numberToKill = this.random.Next(2, 6);
						this.reward = this.numberToKill * 180;
					}
				}
				else if (num != 510600819u)
				{
					if (num == 703662834u)
					{
						if (text == "Rock Crab")
						{
							this.numberToKill = this.random.Next(2, 6);
							this.reward = this.numberToKill * 75;
						}
					}
				}
				else if (text == "Duggy")
				{
					this.questDescription = "The monsters known as 'Duggies' are making a mess of the local mine! The holes they create could weaken the structural integrity of the mines. Could someone defeat " + this.numberToKill + " of these creatures?";
					this.target = "Clint";
					this.numberToKill = this.random.Next(2, 4);
					this.reward = this.numberToKill * 150;
				}
			}
			else if (num <= 1104688147u)
			{
				if (num != 733101947u)
				{
					if (num == 1104688147u)
					{
						if (text == "Sludge")
						{
							this.numberToKill = this.random.Next(4, 9);
							this.numberToKill -= this.numberToKill % 2;
							this.reward = this.numberToKill * 125;
						}
					}
				}
				else if (text == "Slime")
				{
					this.numberToKill = this.random.Next(4, 9);
					this.numberToKill -= this.numberToKill % 2;
					this.reward = this.numberToKill * 60;
				}
			}
			else if (num != 2124830350u)
			{
				if (num != 2223526605u)
				{
					if (num == 3125849181u)
					{
						if (text == "Frost Jelly")
						{
							this.numberToKill = this.random.Next(4, 9);
							this.numberToKill -= this.numberToKill % 2;
							this.reward = this.numberToKill * 85;
						}
					}
				}
				else if (text == "Squid Kid")
				{
					this.numberToKill = this.random.Next(1, 3);
					this.reward = this.numberToKill * 350;
				}
			}
			else if (text == "Skeleton")
			{
				this.numberToKill = this.random.Next(1, 4);
				this.reward = this.numberToKill * 120;
			}
			if (this.monsterName.Equals("Slime") || this.monsterName.Equals("Frost Jelly") || this.monsterName.Equals("Sludge"))
			{
				this.questDescription = string.Concat(new object[]
				{
					"Wanted: Slime hunter to slay ",
					this.numberToKill,
					" ",
					this.monsterName.Equals("Frost Jelly") ? "Frost Jellies in the frozen depths of the mine." : (this.monsterName.Equals("Sludge") ? "Red Slimes in the deep lava caverns of the local mine." : " Green Slimes in the local mine.")
				});
				this.target = "Lewis";
				this.targetMessage = "Ah, @! So you helped us with our slime problem? Thank you!$h#$b#I hope you didn't go through too much trouble..." + ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
				{
					" I know it can be really hard to get that sticky slime out your clothes.#$b# I've definitely ruined a few good pairs of pants trying to catch those little ",
					(this.random.NextDouble() < 0.5) ? "squirmers" : "wigglers",
					". I can remember ",
					(this.random.NextDouble() < 0.5) ? "Mama throwing a fit" : "Papa going into a rage",
					" after I got ",
					Dialogue.colors[this.random.Next(Dialogue.colors.Length)].Replace("/", ""),
					" slime all over my brand new ",
					(this.random.NextDouble() < 0.3) ? "dress" : ((this.random.NextDouble() < 0.5) ? "silk" : "twill"),
					" pants... $h#$b# Well, anyways... enjoy your reward."
				}) : "Those slimes can be pretty dangerous in groups.#$b#It should be a little easier to go spelunking now, thanks to your efforts. Enjoy your reward!");
			}
			else if (this.monsterName.Equals("Rock Crab") || this.monsterName.Equals("Lava Crab"))
			{
				this.questDescription = "An invasive crab species is living in the local mine, threating the native wildlife! These creatures are known for disguising themselves as stones. I'll pay someone to slay " + this.numberToKill + " of them.  -Demetrius";
				this.target = "Demetrius";
				this.targetMessage = "Hey, I see you culled the " + this.monsterName + " population a bit. They've been multiplying quicker than normal due to human activity in the caves, so I'm hoping our efforts prevent them from threatening any other species.#$b#The local wildlife thanks you for what you did today, @. Enjoy your reward.$h";
			}
			else
			{
				this.questDescription = string.Concat(new object[]
				{
					"The monsters known as ",
					this.monsterName,
					" are throwing the elemental balance into disarray. I would like an adventurer to enter the mines and slay ",
					this.numberToKill,
					" of these ",
					(this.random.NextDouble() < 0.3) ? "beasts" : ((this.random.NextDouble() < 0.5) ? "fiends" : "creatures"),
					".  -M. Rasmodius, Wizard"
				});
				this.target = "Wizard";
				this.targetMessage = "The elementals are pleased with the job you did. Here's your payment, as promised.";
			}
			if (this.target.Equals("Wizard") && !Game1.player.mailReceived.Contains("wizardJunimoNote") && !Game1.player.mailReceived.Contains("JojaMember"))
			{
				this.questDescription = string.Concat(new object[]
				{
					"Wanted: Monster hunter to slay ",
					this.numberToKill,
					" ",
					this.monsterName,
					"s in the local mines."
				});
				this.target = "Lewis";
				this.targetMessage = "Ah, @! So you helped us with our monster problem? Thank you!$h#$b#I hope you didn't go through too much trouble...";
			}
			this.questDescription = string.Concat(new object[]
			{
				this.questDescription,
				Environment.NewLine,
				Environment.NewLine,
				"- ",
				this.reward,
				"g reward"
			});
			this.currentObjective = string.Concat(new object[]
			{
				"0/",
				this.numberToKill,
				" ",
				this.monsterName.Equals("Frost Jelly") ? "Frost Jellies" : (this.monsterName + "s"),
				" defeated"
			});
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x000D6248 File Offset: 0x000D4448
		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (this.completed)
			{
				return false;
			}
			if (monsterName == null)
			{
				monsterName = "Slime";
			}
			if (n == null && monsterName != null && monsterName.Contains(this.monsterName) && this.numberKilled < this.numberToKill)
			{
				this.numberKilled = Math.Min(this.numberToKill, this.numberKilled + 1);
				if (this.numberKilled >= this.numberToKill)
				{
					if (this.target == null || this.target.Equals("null"))
					{
						base.questComplete();
					}
					else
					{
						this.currentObjective = "Talk to " + this.target;
						Game1.playSound("jingle1");
					}
				}
				else
				{
					this.currentObjective = string.Concat(new object[]
					{
						this.numberKilled,
						"/",
						this.numberToKill,
						" ",
						monsterName.Equals("Frost Jelly") ? "Frost Jellies" : (monsterName + "s"),
						" defeated"
					});
				}
				Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
				{
					scaleChangeChange = -0.012f
				});
			}
			else if (n != null && this.target != null && !this.target.Equals("null") && this.numberKilled >= this.numberToKill && n.name.Equals(this.target) && n.isVillager())
			{
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				this.moneyReward = this.reward;
				base.questComplete();
				Game1.drawDialogue(n);
				return true;
			}
			return false;
		}

		// Token: 0x04000A73 RID: 2675
		public string targetMessage;

		// Token: 0x04000A74 RID: 2676
		public string monsterName;

		// Token: 0x04000A75 RID: 2677
		public string target;

		// Token: 0x04000A76 RID: 2678
		public int numberToKill;

		// Token: 0x04000A77 RID: 2679
		public int reward;

		// Token: 0x04000A78 RID: 2680
		public int numberKilled;
	}
}
