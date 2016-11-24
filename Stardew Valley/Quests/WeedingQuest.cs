using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley.Quests
{
	// Token: 0x02000082 RID: 130
	public class WeedingQuest : Quest
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x000D4EB8 File Offset: 0x000D30B8
		public WeedingQuest()
		{
			GameLocation i = Game1.getLocationFromName("Town");
			for (int j = 0; j < 10; j++)
			{
				i.spawnWeeds(true);
			}
			this.target = Game1.getCharacterFromName("Lewis", false);
			this.questDescription = string.Concat(new string[]
			{
				"Hiring someone to weed the entire town. Once all the weeds are gone, report to the Mayor for payment. Thanks!",
				Environment.NewLine,
				Environment.NewLine,
				"- 300g reward",
				Environment.NewLine,
				"- Everyone will like you a little more"
			});
			this.targetMessage = "You did a great job, @! Thank you so much.$h#$b#I think everyone will be happy once they see how great the town looks!#$b#Here's your payment.";
			this.currentObjective = "";
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000D4F54 File Offset: 0x000D3154
		public override void accept()
		{
			base.accept();
			using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator = Game1.getLocationFromName("Town").Objects.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name.Contains("Weed"))
					{
						this.totalWeeds++;
					}
				}
			}
			this.checkIfComplete(null, -1, -1, null, null);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x000D4FE0 File Offset: 0x000D31E0
		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (n == null && !this.complete)
			{
				int weedsLeft = 0;
				using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator = Game1.getLocationFromName("Town").Objects.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.name.Contains("Weed"))
						{
							weedsLeft++;
						}
					}
				}
				if (weedsLeft == 0)
				{
					this.complete = true;
					this.currentObjective = "Talk to Mr. Lewis";
					Game1.playSound("jingle1");
				}
				else
				{
					this.currentObjective = string.Concat(new object[]
					{
						this.totalWeeds - weedsLeft,
						"/",
						this.totalWeeds,
						" town weeds removed"
					});
				}
				if (Game1.currentLocation.Name.Equals("Town"))
				{
					Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(220f, 260f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
					{
						scaleChangeChange = -0.015f
					});
				}
			}
			else if (n != null && n.Equals(this.target) && this.complete)
			{
				n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
				this.completed = true;
				Game1.player.Money += 300;
				foreach (string s in Game1.player.friendships.Keys)
				{
					if (Game1.player.friendships[s][0] < 2729)
					{
						Game1.player.friendships[s][0] += 20;
					}
				}
				base.questComplete();
				return true;
			}
			return false;
		}

		// Token: 0x04000A68 RID: 2664
		public NPC target;

		// Token: 0x04000A69 RID: 2665
		public string targetMessage;

		// Token: 0x04000A6A RID: 2666
		public bool complete;

		// Token: 0x04000A6B RID: 2667
		public int totalWeeds;
	}
}
