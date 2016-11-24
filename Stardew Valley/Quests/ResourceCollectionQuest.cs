using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace StardewValley.Quests
{
	// Token: 0x02000085 RID: 133
	public class ResourceCollectionQuest : Quest
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x000D6470 File Offset: 0x000D4670
		public ResourceCollectionQuest()
		{
			if (Game1.gameMode != 6)
			{
				this.questType = 10;
				this.questTitle = "Gathering";
				this.resource = this.random.Next(6) * 2;
				switch (this.resource)
				{
				case 0:
					this.resource = 378;
					this.number = 20 + Game1.player.MiningLevel * 2 + this.random.Next(-2, 4) * 2;
					this.reward = this.number * 10;
					this.number -= this.number % 5;
					this.target = "Clint";
					break;
				case 2:
					this.resource = 380;
					this.number = 15 + Game1.player.MiningLevel + this.random.Next(-1, 3) * 2;
					this.reward = this.number * 15;
					this.number = (int)((float)this.number * 0.75f);
					this.number -= this.number % 5;
					this.target = "Clint";
					break;
				case 4:
					this.resource = 382;
					this.number = 10 + Game1.player.MiningLevel + this.random.Next(-1, 3) * 2;
					this.reward = this.number * 25;
					this.number = (int)((float)this.number * 0.75f);
					this.number -= this.number % 5;
					this.target = "Clint";
					break;
				case 6:
					this.resource = ((Game1.player.deepestMineLevel > 40) ? 384 : 378);
					this.number = 8 + Game1.player.MiningLevel / 2 + this.random.Next(-1, 1) * 2;
					this.reward = this.number * 30;
					this.number = (int)((float)this.number * 0.75f);
					this.number -= this.number % 2;
					this.target = "Clint";
					break;
				case 8:
					this.resource = 388;
					this.number = 25 + Game1.player.ForagingLevel + this.random.Next(-3, 3) * 2;
					this.number -= this.number % 5;
					this.reward = this.number * 8;
					this.target = "Robin";
					break;
				case 10:
					this.resource = 390;
					this.number = 25 + Game1.player.MiningLevel + this.random.Next(-3, 3) * 2;
					this.number -= this.number % 5;
					this.reward = this.number * 8;
					this.target = "Robin";
					break;
				}
				if (this.target == null)
				{
					return;
				}
				if (this.resource < 388)
				{
					this.questDescription = string.Concat(new object[]
					{
						"I am looking for someone to bring me ",
						this.number,
						" ",
						Game1.objectInformation[this.resource].Split(new char[]
						{
							'/'
						})[0],
						"s",
						new string[]
						{
							", to aid in the understanding of local minerals.",
							", for inspection.",
							", as part of a local geological survey.",
							", to see if any rare gems are hidden inside."
						}.ElementAt(this.random.Next(4)),
						"    -Clint"
					});
					if (this.questDescription.Contains("gems"))
					{
						this.targetMessage = "You brought the ores I requested. " + ((this.random.NextDouble() < 0.3) ? "Excellent." : ((this.random.NextDouble() < 0.5) ? "Thank you." : "Great.")) + "#$b#Hmm... It seems these ores don't have any rare gems hidden inside. That's okay.#$b#Here's your ores back, and your payment... as promised.";
					}
					else
					{
						this.targetMessage = string.Concat(new string[]
						{
							"You brought the ores I requested! ",
							(this.random.NextDouble() < 0.3) ? "Excellent." : ((this.random.NextDouble() < 0.5) ? "Thank you." : "Great."),
							"#$b#",
							(this.random.NextDouble() < 0.5) ? ("I like to inspect local ores from time to time, to keep track of quality and abundance.#$b#Let me just take a look at these... Hmm... I see.#$b#Thank you. You can keep these ores. They're of " + ((this.random.NextDouble() < 0.3) ? "excellent" : ((this.random.NextDouble() < 0.5) ? "decent" : "good")) + " quality.") : "These ores look to be of good quality... You can keep them, of course. I just wanted to see how the local geology is doing.",
							"#$b#...And here's your payment, as promised."
						});
					}
				}
				else
				{
					this.questDescription = string.Concat(new object[]
					{
						"Hello! It's Robin. Could someone bring me ",
						this.number,
						" pieces of ",
						Game1.objectInformation[this.resource].Split(new char[]
						{
							'/'
						})[0],
						"? They need to be fresh... that means gathered today. Thanks!"
					});
					this.targetMessage = string.Concat(new string[]
					{
						"Hey, looks like you got the ",
						(this.resource == 13) ? "lumber" : "stone",
						" I asked for.",
						(this.random.NextDouble() < 0.3) ? "Wonderful!" : ((this.random.NextDouble() < 0.5) ? "That's a big help... thanks!" : "Thank you!"),
						"$h"
					});
				}
				this.questDescription = string.Concat(new object[]
				{
					this.questDescription,
					Environment.NewLine,
					Environment.NewLine,
					"- ",
					this.reward,
					"g on delivery.",
					this.target.Equals("Clint") ? (Environment.NewLine + "- You can keep the ores after Clint inspects them.") : ""
				});
				this.currentObjective = string.Concat(new object[]
				{
					"0/",
					this.number,
					" ",
					Game1.objectInformation[this.resource].Split(new char[]
					{
						'/'
					})[0],
					" collected"
				});
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x000D6B3C File Offset: 0x000D4D3C
		public override bool checkIfComplete(NPC n = null, int resourceCollected = -1, int amount = -1, Item item = null, string monsterName = null)
		{
			if (this.completed)
			{
				return false;
			}
			if (n == null && resourceCollected != -1 && amount != -1 && resourceCollected == this.resource && this.numberCollected < this.number)
			{
				this.numberCollected = Math.Min(this.number, this.numberCollected + amount);
				if (this.numberCollected < this.number)
				{
					this.currentObjective = string.Concat(new object[]
					{
						this.numberCollected,
						"/",
						this.number,
						" ",
						Game1.objectInformation[this.resource].Split(new char[]
						{
							'/'
						})[0],
						" collected"
					});
				}
				else
				{
					this.currentObjective = "Talk to " + this.target;
					Game1.playSound("jingle1");
				}
				Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
				{
					scaleChangeChange = -0.012f
				});
			}
			else if (n != null && this.target != null && this.numberCollected >= this.number && n.name.Equals(this.target) && n.isVillager())
			{
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				this.moneyReward = this.reward;
				n.name.Equals("Robin");
				base.questComplete();
				Game1.drawDialogue(n);
				return true;
			}
			return false;
		}

		// Token: 0x04000A79 RID: 2681
		public string target;

		// Token: 0x04000A7A RID: 2682
		public string targetMessage;

		// Token: 0x04000A7B RID: 2683
		public int numberCollected;

		// Token: 0x04000A7C RID: 2684
		public int number;

		// Token: 0x04000A7D RID: 2685
		public int reward;

		// Token: 0x04000A7E RID: 2686
		public int resource;
	}
}
