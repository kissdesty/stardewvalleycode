using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
	// Token: 0x02000086 RID: 134
	public class ItemDeliveryQuest : Quest
	{
		// Token: 0x06000A1F RID: 2591 RVA: 0x000D6D38 File Offset: 0x000D4F38
		public ItemDeliveryQuest()
		{
			if (this.questTitle.Length > 0)
			{
				return;
			}
			this.questTitle = "Delivery";
			this.questType = 3;
			if (Game1.player.friendships == null || Game1.player.friendships.Count <= 0)
			{
				return;
			}
			this.target = Game1.player.friendships.Keys.ElementAt(this.random.Next(Game1.player.friendships.Count));
			string itemName = "";
			int tries = 0;
			NPC actualTarget = Game1.getCharacterFromName(this.target, false);
			if (actualTarget == null)
			{
				return;
			}
			while (tries < 30 && (this.target == null || actualTarget == null || actualTarget.isInvisible || actualTarget.name.Equals(Game1.player.spouse) || actualTarget.name.Equals("Krobus") || actualTarget.name.Contains("Qi") || actualTarget.name.Contains("Dwarf") || actualTarget.name.Contains("Gunther") || actualTarget.age == 2 || actualTarget.name.Contains("Bouncer") || actualTarget.name.Contains("Henchman") || actualTarget.name.Contains("Marlon") || actualTarget.name.Contains("Mariner") || !actualTarget.isVillager() || (actualTarget.name.Equals("Sandy") && !Game1.player.eventsSeen.Contains(67))))
			{
				tries++;
				this.target = Game1.player.friendships.Keys.ElementAt(this.random.Next(Game1.player.friendships.Count));
				actualTarget = Game1.getCharacterFromName(this.target, false);
			}
			if (actualTarget == null)
			{
				return;
			}
			if (tries >= 30 || (this.target.Equals("Wizard") && !Game1.player.mailReceived.Contains("wizardJunimoNote") && !Game1.player.mailReceived.Contains("JojaMember")))
			{
				this.target = "Demetrius";
				actualTarget = Game1.getCharacterFromName(this.target, false);
			}
			string article;
			if (!Game1.currentSeason.Equals("winter") && this.random.NextDouble() < 0.15)
			{
				List<int> crops = Utility.possibleCropsAtThisTime(Game1.currentSeason, Game1.dayOfMonth <= 7);
				this.item = crops.ElementAt(this.random.Next(crops.Count));
				itemName = Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[0];
				article = Game1.getProperArticleForWord(itemName);
				this.questDescription = string.Concat(new string[]
				{
					(this.random.NextDouble() < 0.3) ? "Looking for" : ((this.random.NextDouble() < 0.5) ? "I'm in the market for" : "I could really go for"),
					(this.random.NextDouble() < 0.3) ? " a farm-fresh " : ((this.random.NextDouble() < 0.5) ? " a delicious " : " a ripe "),
					itemName,
					". ",
					(this.random.NextDouble() < 0.25) ? "Could a local farmer deliver one to me?" : ((this.random.NextDouble() < 0.33) ? "I'll pay you when you bring it." : ((this.random.NextDouble() < 0.5) ? "" : "I would be so happy if someone delivered one to me.")),
					Environment.NewLine,
					"        -",
					this.target
				});
				if (this.target.Equals("Demetrius"))
				{
					this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
					{
						"Greetings! Demetrius here. Could someone please bring me ",
						article,
						" ",
						itemName,
						"?",
						Environment.NewLine,
						"I'm studying them as part of my biological research."
					}) : string.Concat(new string[]
					{
						"Looking to dissect ",
						article,
						" ",
						itemName,
						". Please deliver to Demetrius at 24 Mountain Road."
					}));
				}
				if (this.target.Equals("Marnie"))
				{
					this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
					{
						"Howdy neighbors! My goats are really hankerin' for ",
						article,
						" ",
						itemName,
						".I can't find any myself! Help?     -Marnie"
					}) : string.Concat(new string[]
					{
						"Howdy neighbors! I want to try giving my cows ",
						itemName,
						" to increase their milk production. If you find one could you swing it by for me?.",
						Environment.NewLine,
						"     -Marnie"
					}));
				}
				if (this.target.Equals("Sebastian"))
				{
					this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
					{
						"Feeling gloomy... ",
						article,
						" ",
						itemName,
						" might cheer me up.",
						Environment.NewLine,
						"     -Sebastian"
					}) : string.Concat(new string[]
					{
						"...feel like throwing ",
						article,
						" ",
						itemName,
						" as hard as I can against the mountain face. Please deliver to Sebastian at 24 Mountain Road."
					}));
				}
			}
			else
			{
				this.item = Utility.getRandomItemFromSeason(Game1.currentSeason, 1000, true);
				if (this.item > 0)
				{
					itemName = Game1.objectInformation[this.item].Split(new char[]
					{
						'/'
					})[0];
				}
				else
				{
					int num = this.item;
					if (num != -6)
					{
						if (num == -5)
						{
							itemName = "Egg";
						}
					}
					else
					{
						itemName = "jug of Milk";
					}
				}
				article = Game1.getProperArticleForWord(itemName);
				string[] questDescriptions = null;
				if (Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[3].Split(new char[]
				{
					' '
				})[0].Equals("Cooking") && !this.target.Equals("Wizard"))
				{
					if (this.random.NextDouble() < 0.33)
					{
						this.questDescription = string.Concat(new string[]
						{
							(this.random.NextDouble() < 0.5) ? "I am looking for " : "Need ",
							article,
							" ",
							itemName,
							" to ",
							new string[]
							{
								"serve at my afternoon tea party",
								"pair with a dry red wine this evening",
								"enjoy with some fresh bread",
								"bring to my evening book club",
								"enjoy after a hard day's work",
								"eat in front of the TV",
								"enjoy while " + (Game1.samBandName.Equals("The Alfalfas") ? ((!Game1.elliottBookName.Equals("Blue Tower")) ? ("reading " + Game1.elliottBookName) : "listening to my favorite radio program.") : ("listening to the new '" + Game1.samBandName + "' record.")),
								"serve for dinner tonight",
								"freeze in case I need a quick snack",
								"have for lunch tomorrow",
								Game1.currentSeason.Equals("winter") ? "add some variety to my winter feast" : (Game1.currentSeason.Equals("summer") ? "enjoy as a summer treat" : "cheer me up"),
								"serve at breakfast tomorrow"
							}.ElementAt(this.random.Next(12)),
							".",
							Environment.NewLine,
							"      -",
							this.target
						});
					}
					else
					{
						questDescriptions = new string[]
						{
							string.Concat(new string[]
							{
								"I woke up thinking about how much I want ",
								article,
								" ",
								itemName,
								" for dinner.",
								Environment.NewLine,
								"Could someone make it for me?",
								Environment.NewLine,
								"      -",
								this.target
							}),
							string.Concat(new string[]
							{
								"I have a craving for ",
								article,
								" ",
								itemName,
								". Please bring one by for me.",
								Environment.NewLine,
								"       -",
								this.target
							}),
							string.Concat(new string[]
							{
								"Need ",
								article,
								" ",
								itemName,
								" for my compost pile. Thanks.",
								Environment.NewLine,
								"      -",
								this.target
							}),
							string.Concat(new string[]
							{
								"SO hungry. Need ",
								itemName,
								".",
								Environment.NewLine,
								"    -",
								this.target
							}),
							string.Concat(new string[]
							{
								"Stardew Valley Meals Service is looking for a temporary worker on ",
								Environment.NewLine,
								Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth),
								" to deliver ",
								article,
								" ",
								itemName,
								" to ",
								this.target,
								". Compensation upon delivery."
							})
						};
					}
					this.questDescription = questDescriptions[this.random.Next(questDescriptions.Length)];
					if (this.target.Equals("Sebastian"))
					{
						this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
						{
							"Feeling gloomy... ",
							article,
							" ",
							itemName,
							" might cheer me up.",
							Environment.NewLine,
							"     -Sebastian"
						}) : string.Concat(new string[]
						{
							"...feel like ",
							article,
							" ",
							itemName,
							" for some reason. Please bring to Sebastian at 24 Mountain Road."
						}));
					}
				}
				else if (this.random.NextDouble() < 0.5 && Convert.ToInt32(Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[2]) > 0)
				{
					questDescriptions = new string[]
					{
						string.Concat(new string[]
						{
							"I need ",
							article,
							" ",
							itemName,
							" for a ",
							new string[]
							{
								"soup",
								"pizza",
								"salad",
								"stew",
								"casserole",
								"stir-fry",
								"calzone",
								"taco",
								"burrito",
								"dish",
								"dessert",
								"new recipe"
							}.ElementAt(this.random.Next(12)),
							" I'm making. ",
							(this.random.NextDouble() < 0.5) ? "" : "Could someone bring me one?",
							Environment.NewLine,
							"        -",
							this.target
						}),
						string.Concat(new string[]
						{
							"I have a craving for ",
							itemName,
							". ",
							(this.random.NextDouble() < 0.5) ? "" : "Please bring one by for me.",
							Environment.NewLine,
							"       -",
							this.target
						})
					};
					if (this.random.NextDouble() < 0.33)
					{
						this.questDescription = string.Concat(new string[]
						{
							(this.random.NextDouble() < 0.5) ? "I am looking for " : "Need ",
							article,
							" ",
							itemName,
							" to ",
							new string[]
							{
								"serve at my afternoon tea party",
								"pair with a dry red wine this evening",
								"enjoy with some fresh bread",
								"bring to my evening book club",
								"enjoy after a hard day's work",
								"eat in front of the TV",
								"enjoy while " + (Game1.samBandName.Equals("The Alfalfas") ? ((!Game1.elliottBookName.Equals("Blue Tower")) ? ("reading " + Game1.elliottBookName) : "listening to my favorite radio program.") : ("listening to the new '" + Game1.samBandName + "' record.")),
								"roast up for dinner tonight",
								"pickle",
								"dry for next winter",
								Game1.currentSeason.Equals("winter") ? "put in a hearty winter pie" : (Game1.currentSeason.Equals("summer") ? "grill up for a summer BBQ" : "skewer on a shishkebab"),
								"serve at breakfast tomorrow"
							}.ElementAt(this.random.Next(12)),
							".",
							Environment.NewLine,
							"      -",
							this.target
						});
					}
					else
					{
						this.questDescription = questDescriptions[this.random.Next(questDescriptions.Length)];
					}
					if (this.target.Equals("Demetrius"))
					{
						this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
						{
							"Greetings! Demetrius here. Could someone please bring me ",
							article,
							" ",
							itemName,
							"?",
							Environment.NewLine,
							"I'm studying them as part of my biological research."
						}) : string.Concat(new string[]
						{
							"Looking to dissect ",
							article,
							" ",
							itemName,
							". Please deliver to Demetrius at 24 Mountain Road."
						}));
					}
					if (this.target.Equals("Marnie"))
					{
						this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
						{
							"Howdy neighbors! My goats are really hankerin' for ",
							article,
							" ",
							itemName,
							".",
							Environment.NewLine,
							"I can't find any myself! Help?",
							Environment.NewLine,
							"     -Marnie"
						}) : string.Concat(new string[]
						{
							"Howdy neighbors! I want to try giving my cows ",
							itemName,
							" to increase their milk production. If you find one could you swing it by for me?",
							Environment.NewLine,
							"     -Marnie"
						}));
					}
					if (this.target.Equals("Harvey"))
					{
						this.questDescription = string.Concat(new string[]
						{
							"A paste of fresh ",
							itemName,
							" makes a wonderful tonic for ",
							new string[]
							{
								"swollen tongues",
								"the common cold",
								"bed sores",
								"gout",
								"halitosis",
								"hair loss",
								"acne",
								"nasal drip",
								"toothaches",
								"indigestion",
								"migraines",
								"skin rashes"
							}.ElementAt(this.random.Next(12)),
							". If you find one, I could put it to good use.       -Dr. Harvey "
						});
					}
					if (this.target.Equals("Gus") && this.random.NextDouble() < 0.6)
					{
						this.questDescription = string.Concat(new string[]
						{
							"Hi! I need ",
							article,
							" ",
							itemName,
							" for a new appetizer I'm thinking about. Drop one off at the saloon and I'll be very grateful.     -Gus"
						});
					}
				}
				else if (this.random.NextDouble() < 0.5 && Convert.ToInt32(Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[2]) < 0)
				{
					questDescriptions = new string[]
					{
						string.Concat(new string[]
						{
							"I'd like to put ",
							article,
							" ",
							itemName,
							" ",
							new string[]
							{
								"on my shelf",
								"above the mantle",
								"under my pillow",
								"on a chain",
								"on my great-grandfather's tombstone"
							}.ElementAt(this.random.Next(5)),
							". Please deliver it some time today.",
							Environment.NewLine,
							"        -",
							this.target
						})
					};
					this.questDescription = questDescriptions[this.random.Next(questDescriptions.Length)];
					if (this.target.Equals("Emily"))
					{
						this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
						{
							"Hello, it's Emily. I'd like to buy ",
							article,
							" ",
							itemName,
							" from someone,",
							Environment.NewLine,
							"to craft into some fine jewelry. Thanks!"
						}) : string.Concat(new string[]
						{
							"Looking for ",
							article,
							" ",
							itemName,
							" to make into buttons for a dress",
							Environment.NewLine,
							" I'm making. If you find one could you bring it to me?.",
							Environment.NewLine,
							"     -Emily"
						}));
					}
				}
				else
				{
					questDescriptions = new string[]
					{
						string.Concat(new string[]
						{
							this.target,
							" is in need of ",
							article,
							" ",
							itemName
						}),
						string.Concat(new string[]
						{
							"Looking for ",
							article,
							" ",
							itemName,
							(this.random.NextDouble() < 0.3) ? ". Will pay a flat rate on delivery." : ((this.random.NextDouble() < 0.5) ? "" : ". Cash payment on delivery."),
							"          -",
							this.target
						}),
						string.Concat(new string[]
						{
							". Could someone bring me ",
							article,
							" ",
							itemName,
							"? ",
							(this.random.NextDouble() < 0.25) ? ". I love them." : ((this.random.NextDouble() < 0.33) ? ". I can't get enough of them!" : ((this.random.NextDouble() < 0.5) ? "" : ". I could use one.")),
							Environment.NewLine,
							"          -",
							this.target
						}),
						string.Concat(new string[]
						{
							"Does anyone have an extra ",
							itemName,
							(this.random.NextDouble() < 0.4) ? " laying around" : "",
							"?  -",
							this.target
						}),
						string.Concat(new string[]
						{
							"I need ",
							article,
							" ",
							itemName,
							", if It's not too inconvenient. ",
							Environment.NewLine,
							"          -",
							this.target
						}),
						string.Concat(new string[]
						{
							"I'm running low on ",
							itemName,
							". If someone could bring me one, ",
							Environment.NewLine,
							"it would be much appreciated.  -",
							this.target
						}),
						string.Concat(new string[]
						{
							"I need ",
							article,
							" ",
							itemName,
							" to rub on my sore ",
							new string[]
							{
								"knee",
								"elbow",
								"back",
								"head",
								"nose",
								"toe",
								"gums",
								"ear",
								"legs",
								"tooth",
								"hip",
								"calves"
							}.ElementAt(this.random.Next(12)),
							". Please bring one ",
							(this.random.NextDouble() < 0.5) ? "as soon as you can!" : "ASAP!!",
							Environment.NewLine,
							"      -",
							this.target
						}),
						string.Concat(new string[]
						{
							this.target,
							" would like to hire someone to fetch ",
							article,
							" ",
							itemName,
							". You will be paid for your time."
						}),
						string.Concat(new string[]
						{
							"Buying 1 ",
							itemName,
							" at twice the market value! Please deliver directly ",
							(this.random.NextDouble() < 0.5) ? "to me" : "into my hands.",
							"     -",
							this.target
						})
					};
					this.questDescription = questDescriptions[this.random.Next(questDescriptions.Length)];
				}
			}
			this.targetMessage = string.Concat(new string[]
			{
				(this.random.NextDouble() < 0.3 || this.target.Equals("Evelyn")) ? "Oh!" : ((this.random.NextDouble() < 0.5) ? "Hey!" : ("Hello " + Game1.player.name + "!")),
				(this.random.NextDouble() < 0.3) ? (" Is that the " + itemName + " I requested?") : ((this.random.NextDouble() < 0.5) ? " You brought me the item I asked for!" : (" So you saw the " + ((this.random.NextDouble() < 0.3) ? "bulletin" : ((this.random.NextDouble() < 0.5) ? "notice" : "ad")) + " I posted?")),
				(this.random.NextDouble() < 0.3) ? " Thanks so much!" : ((this.random.NextDouble() < 0.5) ? " I really appreciate it." : " It looks perfect."),
				"$h#$b#",
				(this.random.NextDouble() < 0.3) ? " Here's a little something for your trouble." : ((this.random.NextDouble() < 0.5) ? " Here's your payment, as promised." : " ...Let's see. Here's what I owe you.")
			});
			if (this.target.Equals("Wizard"))
			{
				if (this.random.NextDouble() < 0.5)
				{
					this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
					{
						"I require ",
						article,
						" ",
						itemName,
						" for potion-making. Please deliver directly to my study.     -M. Rasmodius, Wizard"
					}) : string.Concat(new string[]
					{
						"The Wizard, M. Rasmodius, wishes to study the arcane properties of the ",
						itemName,
						".",
						Environment.NewLine,
						"You will be rewarded."
					}));
				}
				else
				{
					this.questDescription = ((this.random.NextDouble() < 0.5) ? ("Need " + itemName + " for experiment.    -M. Rasmodius, Wizard") : ("Local wizard in search of a fresh " + itemName + "."));
				}
				this.targetMessage = "Ah, the item I requested.#$b#Your work was satisfactory. Here is your compensation.";
			}
			if (this.target.Equals("Haley"))
			{
				this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
				{
					"I will < you forever if you bring me ",
					article,
					" ",
					itemName,
					"!",
					Environment.NewLine,
					"    -Haley"
				}) : (Game1.player.isMale ? string.Concat(new string[]
				{
					"Looking for a handsome young man to bring me ",
					article,
					" ",
					itemName,
					".",
					Environment.NewLine,
					"       -Haley"
				}) : string.Concat(new string[]
				{
					"FOR GIRLS ONLY: psst... I need ",
					article,
					" ",
					itemName,
					"... you know what it's for. Keep it secret, okay?",
					Environment.NewLine,
					"      -Haley"
				})));
				this.targetMessage = "Oh! That's exactly what I needed! Heehee! I'm so happy!$h";
			}
			if (this.target.Equals("Sam"))
			{
				this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
				{
					"Need ",
					article,
					" ",
					itemName,
					" for a project I'm working on",
					Environment.NewLine,
					"    -Sam"
				}) : (Game1.player.isMale ? string.Concat(new string[]
				{
					"Looking for a buddy to bring me ",
					article,
					" ",
					itemName,
					".",
					Environment.NewLine,
					"       -Sam"
				}) : string.Concat(new string[]
				{
					"I would like a cute girl to bring me ",
					article,
					" ",
					itemName,
					".  (^_-)",
					Environment.NewLine,
					"      -Sam"
				})));
				this.targetMessage = "Hey, thanks a million, " + Game1.player.name + "! I should've known you'd be the one to take this job.$h";
			}
			if (this.target.Equals("Maru"))
			{
				this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
				{
					"Need ",
					article,
					" ",
					itemName,
					" for my latest project. Help!",
					Environment.NewLine,
					"    -Maru"
				}) : string.Concat(new string[]
				{
					"I need ",
					article,
					" ",
					itemName,
					" to power my latest invention. It's the ONLY thing that will work!       -Maru"
				}));
				this.targetMessage = ((this.questDescription[0] == 'N') ? ("Oh! Perfect! Now I can continue the project I was working on.$h#$b#Thanks a bunch, " + Game1.player.name + ".$h") : ("Oh, that's exactly what I need to power my latest invention, " + Game1.player.name + "!#$b#Thank you!$h"));
			}
			if (this.target.Equals("Abigail"))
			{
				this.questDescription = ((this.random.NextDouble() < 0.5) ? string.Concat(new string[]
				{
					"Bored... could someone bring me ",
					article,
					" ",
					itemName,
					"?",
					Environment.NewLine,
					"    -Abby"
				}) : string.Concat(new string[]
				{
					"I want to pull a prank on my Dad. I'll need ",
					article,
					" ",
					itemName,
					". Keep it secret :)",
					Environment.NewLine,
					"      -Abigail"
				}));
				this.targetMessage = ((this.questDescription[0] == 'B') ? ("Hmm... this isn't as exciting as I thought it would be...$s#$b#But that's not your fault! Haha. Thanks for responding to my bulletin, " + Game1.player.name + ".$h") : ("Heh heh... perfect. Here's your reward, " + Game1.player.name + ". Don't say a word to my Dad. $h"));
			}
			if (this.target.Equals("Sebastian"))
			{
				this.targetMessage = "Hey, thanks for the help. This is just what I wanted.$h";
			}
			if (this.target.Equals("Elliott"))
			{
				this.targetMessage = "Ah, the " + itemName + " I requested! And it's a beautiful one, too. Thank you very much, " + Game1.player.name;
			}
			this.questDescription = string.Concat(new object[]
			{
				this.questDescription,
				Environment.NewLine,
				Environment.NewLine,
				"- ",
				Convert.ToInt32(Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[1]) * 3,
				"g on delivery",
				Environment.NewLine,
				(this.random.NextDouble() < 0.3) ? ("- Makes " + this.target + " happy") : ((this.random.NextDouble() < 0.5) ? ("- " + this.target + " will be thankful") : ("- " + this.target + " will be pleased"))
			});
			this.currentObjective = string.Concat(new string[]
			{
				"Bring ",
				this.target,
				" ",
				Game1.getProperArticleForWord(Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[0]),
				" ",
				Game1.objectInformation[this.item].Split(new char[]
				{
					'/'
				})[0]
			});
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000D8B00 File Offset: 0x000D6D00
		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (this.completed)
			{
				return false;
			}
			if (item == null || !(item is Object) || n == null || !n.isVillager() || !n.name.Equals(this.target) || ((item as Object).ParentSheetIndex != this.item && (item as Object).Category != this.item))
			{
				return false;
			}
			if (item.Stack >= this.number)
			{
				Game1.player.ActiveObject.Stack -= this.number - 1;
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				Game1.drawDialogue(n);
				Game1.player.reduceActiveItemByOne();
				if (this.dailyQuest)
				{
					Game1.player.changeFriendship(150, n);
					this.moneyReward = (item as Object).Price * 3;
				}
				else
				{
					Game1.player.changeFriendship(255, n);
				}
				base.questComplete();
				return true;
			}
			n.CurrentDialogue.Push(new Dialogue("That's not enough... I need " + this.number + " of them.", n));
			Game1.drawDialogue(n);
			return false;
		}

		// Token: 0x04000A7F RID: 2687
		public string targetMessage;

		// Token: 0x04000A80 RID: 2688
		public string target;

		// Token: 0x04000A81 RID: 2689
		public int item;

		// Token: 0x04000A82 RID: 2690
		public int number = 1;
	}
}
