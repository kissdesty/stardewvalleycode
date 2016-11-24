using System;
using System.Linq;

namespace StardewValley.Quests
{
	// Token: 0x02000083 RID: 131
	public class FishingQuest : Quest
	{
		// Token: 0x06000A19 RID: 2585 RVA: 0x000D523C File Offset: 0x000D343C
		public FishingQuest()
		{
			if (this.questTitle.Length > 0)
			{
				return;
			}
			this.questTitle = "Fishing";
			this.questType = 7;
			int priceOfFish;
			if (this.random.NextDouble() < 0.5)
			{
				string currentSeason = Game1.currentSeason;
				if (!(currentSeason == "spring"))
				{
					if (!(currentSeason == "summer"))
					{
						if (!(currentSeason == "fall"))
						{
							if (currentSeason == "winter")
							{
								int[] fish = new int[]
								{
									130,
									131,
									136,
									141,
									143,
									144,
									146,
									147,
									150,
									151
								};
								this.whichFish = fish[this.random.Next(fish.Length)];
							}
						}
						else
						{
							int[] fish = new int[]
							{
								129,
								131,
								136,
								137,
								139,
								142,
								143,
								150
							};
							this.whichFish = fish[this.random.Next(fish.Length)];
						}
					}
					else
					{
						int[] fish = new int[]
						{
							130,
							131,
							136,
							138,
							142,
							144,
							145,
							146,
							149,
							150
						};
						this.whichFish = fish[this.random.Next(fish.Length)];
					}
				}
				else
				{
					int[] fish = new int[]
					{
						129,
						131,
						136,
						137,
						142,
						143,
						145,
						147
					};
					this.whichFish = fish[this.random.Next(fish.Length)];
				}
				priceOfFish = Convert.ToInt32(Game1.objectInformation[this.whichFish].Split(new char[]
				{
					'/'
				})[1]);
				this.numberToFish = (int)Math.Ceiling(90.0 / (double)Math.Max(1, priceOfFish)) + Game1.player.FishingLevel / 5;
				this.target = "Demetrius";
				this.nameOfFish = Game1.objectInformation[this.whichFish].Split(new char[]
				{
					'/'
				})[0];
				this.questDescription = string.Concat(new object[]
				{
					"The local ",
					this.nameOfFish,
					" population is starting to threaten other species. If you can fish ",
					this.numberToFish,
					" of them for me, it would be a big help.   -Demetrius"
				});
				this.targetMessage = string.Concat(new string[]
				{
					"Thanks, @!$h#$b#The ",
					this.nameOfFish,
					" population was getting out of hand because ",
					new string[]
					{
						"pollutants in the water accelerated their reprodutive cycle.",
						"careless fishermen released way too many for a local tournament.",
						"overfishing of their competitor species allowed them complete access to their favorite food.",
						"a genetically modified 'super" + this.nameOfFish + "' escaped from a fishery and started multiplying."
					}.ElementAt(this.random.Next(4)),
					"$s#$b#Anyways... here's your reward. It's straight from the Stardew Valley Wildlife Conservation fund!"
				});
				this.currentObjective = string.Concat(new object[]
				{
					"0/",
					this.numberToFish,
					" ",
					this.nameOfFish.Equals("Octopus") ? "Octopi" : this.nameOfFish,
					" caught"
				});
			}
			else
			{
				string currentSeason = Game1.currentSeason;
				if (!(currentSeason == "spring"))
				{
					if (!(currentSeason == "summer"))
					{
						if (!(currentSeason == "fall"))
						{
							if (currentSeason == "winter")
							{
								int[] fish2 = new int[]
								{
									130,
									131,
									136,
									141,
									143,
									144,
									146,
									147,
									150,
									151,
									699,
									702,
									705
								};
								this.whichFish = fish2[this.random.Next(fish2.Length)];
							}
						}
						else
						{
							int[] fish2 = new int[]
							{
								129,
								131,
								136,
								137,
								139,
								142,
								143,
								150,
								699,
								702,
								705
							};
							this.whichFish = fish2[this.random.Next(fish2.Length)];
						}
					}
					else
					{
						int[] fish2 = new int[]
						{
							128,
							130,
							131,
							136,
							138,
							142,
							144,
							145,
							146,
							149,
							150,
							702
						};
						this.whichFish = fish2[this.random.Next(fish2.Length)];
					}
				}
				else
				{
					int[] fish2 = new int[]
					{
						129,
						131,
						136,
						137,
						142,
						143,
						145,
						147,
						702
					};
					this.whichFish = fish2[this.random.Next(fish2.Length)];
				}
				this.target = "Willy";
				priceOfFish = Convert.ToInt32(Game1.objectInformation[this.whichFish].Split(new char[]
				{
					'/'
				})[1]);
				this.numberToFish = (int)Math.Ceiling(90.0 / (double)Math.Max(1, priceOfFish)) + Game1.player.FishingLevel / 5;
				this.nameOfFish = Game1.objectInformation[this.whichFish].Split(new char[]
				{
					'/'
				})[0];
				this.questDescription = string.Concat(new object[]
				{
					"Trying to keep the art o' fishing alive... I'll pay ",
					priceOfFish * this.numberToFish,
					"g to any ",
					Game1.player.isMale ? "fisherman" : "fishing enthusiast",
					"  who catches ",
					this.numberToFish,
					" ",
					this.nameOfFish.Equals("Squid") ? "Squids" : this.nameOfFish,
					". Good luck!  -Willy"
				});
				this.targetMessage = string.Concat(new string[]
				{
					"Hey, you suceeded, @!$h#$b#The ",
					this.nameOfFish,
					" is an exciting catch, don't you think? ",
					new string[]
					{
						"They're always hiding in the most peculiar places.",
						"They're such strong swimmers.",
						"One time I caught one the size of a" + new string[]
						{
							" small motorcycle!",
							" folding chair!",
							" lawnmower!",
							"n encyclopedia set!",
							"n arcade machine!",
							" baby cow!"
						}.ElementAt(this.random.Next(6)),
						"Sometimes they seem so smart, it's scary.$u"
					}.ElementAt(this.random.Next(4)),
					"#$b#Well, here's your reward. Congratulations.$h"
				});
				this.currentObjective = string.Concat(new object[]
				{
					"0/",
					this.numberToFish,
					" ",
					this.nameOfFish.Equals("Squid") ? "Squid" : this.nameOfFish,
					" caught"
				});
			}
			this.reward = this.numberToFish * priceOfFish;
			this.questDescription = string.Concat(new object[]
			{
				this.questDescription,
				Environment.NewLine,
				Environment.NewLine,
				"- ",
				this.reward,
				"g reward.",
				Environment.NewLine,
				"- You get to keep the fish."
			});
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000D5898 File Offset: 0x000D3A98
		public override bool checkIfComplete(NPC n = null, int fish = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (n == null && fish != -1 && fish == this.whichFish && this.numberFished < this.numberToFish)
			{
				this.numberFished = Math.Min(this.numberToFish, this.numberFished + 1);
				if (this.numberFished >= this.numberToFish)
				{
					this.dailyQuest = false;
					if (this.target == null)
					{
						this.target = "Willy";
					}
					this.currentObjective = "Talk to " + this.target;
					Game1.playSound("jingle1");
				}
				else
				{
					this.currentObjective = string.Concat(new object[]
					{
						this.numberFished,
						"/",
						this.numberToFish,
						" ",
						this.nameOfFish.Equals("Squid") ? "Squids" : (this.nameOfFish.Equals("Octopus") ? "Octopi" : this.nameOfFish),
						" caught"
					});
				}
			}
			else if (n != null && this.numberFished >= this.numberToFish && this.target != null && n.name.Equals(this.target) && n.isVillager() && !this.completed)
			{
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				this.moneyReward = this.reward;
				base.questComplete();
				Game1.drawDialogue(n);
				return true;
			}
			return false;
		}

		// Token: 0x04000A6C RID: 2668
		public string target;

		// Token: 0x04000A6D RID: 2669
		public string targetMessage;

		// Token: 0x04000A6E RID: 2670
		public string nameOfFish;

		// Token: 0x04000A6F RID: 2671
		public int numberToFish;

		// Token: 0x04000A70 RID: 2672
		public int reward;

		// Token: 0x04000A71 RID: 2673
		public int numberFished;

		// Token: 0x04000A72 RID: 2674
		public int whichFish;
	}
}
