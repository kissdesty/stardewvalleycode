using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley.Quests
{
	// Token: 0x02000081 RID: 129
	public class SocializeQuest : Quest
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x000D4A8C File Offset: 0x000D2C8C
		public SocializeQuest()
		{
			this.whoToGreet = new List<string>();
			this.questType = 5;
			this.questTitle = "Saying 'Hello'";
			this.questDescription = string.Concat(new string[]
			{
				"Hey! Will someone say 'Hi' to everyone in town for me? I want to spread a message of ",
				(this.random.NextDouble() < 0.3) ? "pure joy" : ((this.random.NextDouble() < 0.5) ? "peace & goodwill" : "community spirit"),
				" today!   -Emily",
				Environment.NewLine,
				Environment.NewLine,
				"- Everyone will like you a little more"
			});
			using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (NPC i in enumerator.Current.characters)
					{
						if (!i.isInvisible && !i.name.Contains("Qi") && !i.name.Contains("???") && !i.name.Equals("Sandy") && !i.name.Contains("Dwarf") && !i.name.Contains("Gunther") && !i.name.Contains("Mariner") && !i.name.Contains("Henchman") && !i.name.Contains("Marlon") && !i.name.Contains("Wizard") && !i.name.Contains("Bouncer") && !i.name.Contains("Krobus") && i.isVillager())
						{
							this.whoToGreet.Add(i.name);
						}
					}
				}
			}
			this.currentObjective = "2/" + this.whoToGreet.Count + " people greeted";
			this.total = this.whoToGreet.Count;
			this.whoToGreet.Remove("Lewis");
			this.whoToGreet.Remove("Robin");
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000D4D24 File Offset: 0x000D2F24
		public override bool checkIfComplete(NPC npc = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
		{
			if (npc != null && this.whoToGreet.Remove(npc.name))
			{
				Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0f, 0f, false)
				{
					scaleChangeChange = -0.012f
				});
			}
			if (this.whoToGreet.Count == 0 && !this.completed)
			{
				foreach (string s in Game1.player.friendships.Keys)
				{
					if (Game1.player.friendships[s][0] < 2729)
					{
						Game1.player.friendships[s][0] += 100;
					}
				}
				base.questComplete();
				return true;
			}
			this.currentObjective = string.Concat(new object[]
			{
				this.total - this.whoToGreet.Count,
				"/",
				this.total,
				" people greeted"
			});
			return false;
		}

		// Token: 0x04000A66 RID: 2662
		public List<string> whoToGreet;

		// Token: 0x04000A67 RID: 2663
		public int total;
	}
}
