using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Quests
{
	// Token: 0x02000087 RID: 135
	[XmlInclude(typeof(SocializeQuest)), XmlInclude(typeof(SlayMonsterQuest)), XmlInclude(typeof(ResourceCollectionQuest)), XmlInclude(typeof(ItemDeliveryQuest)), XmlInclude(typeof(ItemHarvestQuest)), XmlInclude(typeof(CraftingQuest)), XmlInclude(typeof(FishingQuest)), XmlInclude(typeof(GoSomewhereQuest)), XmlInclude(typeof(LostItemQuest))]
	public class Quest
	{
		// Token: 0x06000A22 RID: 2594 RVA: 0x000D8CAC File Offset: 0x000D6EAC
		public static Quest getQuestFromId(int id)
		{
			if (Game1.temporaryContent == null)
			{
				Game1.temporaryContent = Game1.content.CreateTemporary();
			}
			Dictionary<int, string> questData = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
			if (questData != null && questData.ContainsKey(id))
			{
				string[] rawData = questData[id].Split(new char[]
				{
					'/'
				});
				string questType = rawData[0];
				Quest q = null;
				string[] conditionsSplit = rawData[4].Split(new char[]
				{
					' '
				});
				uint num = <PrivateImplementationDetails>.ComputeStringHash(questType);
				if (num <= 1539345862u)
				{
					if (num <= 133275711u)
					{
						if (num != 126609884u)
						{
							if (num == 133275711u)
							{
								if (questType == "Crafting")
								{
									q = new CraftingQuest(Convert.ToInt32(conditionsSplit[0]), conditionsSplit[1].ToLower().Equals("true"));
									q.questType = 2;
								}
							}
						}
						else if (questType == "LostItem")
						{
							q = new LostItemQuest(conditionsSplit[0], conditionsSplit[2], Convert.ToInt32(conditionsSplit[1]), Convert.ToInt32(conditionsSplit[3]), Convert.ToInt32(conditionsSplit[4]));
						}
					}
					else if (num != 1217142150u)
					{
						if (num == 1539345862u)
						{
							if (questType == "Location")
							{
								q = new GoSomewhereQuest(conditionsSplit[0]);
								q.questType = 6;
							}
						}
					}
					else if (questType == "ItemDelivery")
					{
						q = new ItemDeliveryQuest();
						(q as ItemDeliveryQuest).target = conditionsSplit[0];
						(q as ItemDeliveryQuest).item = Convert.ToInt32(conditionsSplit[1]);
						(q as ItemDeliveryQuest).targetMessage = rawData[9];
						if (conditionsSplit.Length > 2)
						{
							(q as ItemDeliveryQuest).number = Convert.ToInt32(conditionsSplit[2]);
						}
						q.questType = 3;
					}
				}
				else if (num <= 2324152213u)
				{
					if (num != 1629445681u)
					{
						if (num == 2324152213u)
						{
							if (questType == "Building")
							{
								q = new Quest();
								q.questType = 8;
								q.completionString = conditionsSplit[0];
							}
						}
					}
					else if (questType == "Monster")
					{
						q = new SlayMonsterQuest();
						(q as SlayMonsterQuest).monsterName = conditionsSplit[0].Replace('_', ' ');
						(q as SlayMonsterQuest).numberToKill = Convert.ToInt32(conditionsSplit[1]);
						if (conditionsSplit.Length > 2)
						{
							(q as SlayMonsterQuest).target = conditionsSplit[2];
						}
						else
						{
							(q as SlayMonsterQuest).target = "null";
						}
						q.questType = 4;
					}
				}
				else if (num != 3610215645u)
				{
					if (num != 4023868591u)
					{
						if (num == 4177547506u)
						{
							if (questType == "Social")
							{
								q = new SocializeQuest();
							}
						}
					}
					else if (questType == "ItemHarvest")
					{
						q = new ItemHarvestQuest(Convert.ToInt32(conditionsSplit[0]), (conditionsSplit.Length > 1) ? Convert.ToInt32(conditionsSplit[1]) : 1);
					}
				}
				else if (questType == "Basic")
				{
					q = new Quest();
					q.questType = 1;
				}
				q.id = id;
				q.questTitle = rawData[1];
				q.questDescription = rawData[2];
				if (rawData[3].Length > 1)
				{
					q.currentObjective = rawData[3];
				}
				string[] nextQuestsSplit = rawData[5].Split(new char[]
				{
					' '
				});
				for (int i = 0; i < nextQuestsSplit.Length; i++)
				{
					q.nextQuests.Add(Convert.ToInt32(nextQuestsSplit[i]));
				}
				q.showNew = true;
				q.moneyReward = Convert.ToInt32(rawData[6]);
				q.rewardDescription = (rawData[6].Equals("-1") ? null : rawData[7]);
				if (rawData.Length > 8)
				{
					q.canBeCancelled = rawData[8].Equals("true");
				}
				return q;
			}
			return null;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void adjustGameLocation(GameLocation location)
		{
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x000D90B7 File Offset: 0x000D72B7
		public virtual void accept()
		{
			this.accepted = true;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x000D90C0 File Offset: 0x000D72C0
		public virtual bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (this.completionString != null && str != null && str.Equals(this.completionString))
			{
				this.questComplete();
				return true;
			}
			return false;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x000D90E6 File Offset: 0x000D72E6
		public bool hasReward()
		{
			return this.moneyReward > 0 || (this.rewardDescription != null && this.rewardDescription.Length > 2);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x000D910C File Offset: 0x000D730C
		public void questComplete()
		{
			if (!this.completed)
			{
				if (this.dailyQuest || this.questType == 7)
				{
					Stats expr_21 = Game1.stats;
					uint questsCompleted = expr_21.QuestsCompleted;
					expr_21.QuestsCompleted = questsCompleted + 1u;
				}
				this.completed = true;
				if (this.nextQuests.Count > 0)
				{
					foreach (int i in this.nextQuests)
					{
						if (i > 0)
						{
							Game1.player.questLog.Add(Quest.getQuestFromId(i));
						}
					}
					Game1.addHUDMessage(new HUDMessage("Journal Updated", 2));
				}
				if (this.moneyReward <= 0 && (this.rewardDescription == null || this.rewardDescription.Length <= 2))
				{
					Game1.player.questLog.Remove(this);
				}
				else
				{
					Game1.addHUDMessage(new HUDMessage("Journal Updated", 2));
				}
				Game1.playSound("questcomplete");
			}
		}

		// Token: 0x04000A83 RID: 2691
		public const int type_basic = 1;

		// Token: 0x04000A84 RID: 2692
		public const int type_crafting = 2;

		// Token: 0x04000A85 RID: 2693
		public const int type_itemDelivery = 3;

		// Token: 0x04000A86 RID: 2694
		public const int type_monster = 4;

		// Token: 0x04000A87 RID: 2695
		public const int type_socialize = 5;

		// Token: 0x04000A88 RID: 2696
		public const int type_location = 6;

		// Token: 0x04000A89 RID: 2697
		public const int type_fishing = 7;

		// Token: 0x04000A8A RID: 2698
		public const int type_building = 8;

		// Token: 0x04000A8B RID: 2699
		public const int type_harvest = 9;

		// Token: 0x04000A8C RID: 2700
		public const int type_resource = 10;

		// Token: 0x04000A8D RID: 2701
		public string currentObjective = "";

		// Token: 0x04000A8E RID: 2702
		public string questDescription = "";

		// Token: 0x04000A8F RID: 2703
		public string questTitle = "";

		// Token: 0x04000A90 RID: 2704
		public string rewardDescription;

		// Token: 0x04000A91 RID: 2705
		public string completionString;

		// Token: 0x04000A92 RID: 2706
		protected Random random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);

		// Token: 0x04000A93 RID: 2707
		public bool accepted;

		// Token: 0x04000A94 RID: 2708
		public bool completed;

		// Token: 0x04000A95 RID: 2709
		public bool dailyQuest;

		// Token: 0x04000A96 RID: 2710
		public bool showNew;

		// Token: 0x04000A97 RID: 2711
		public bool canBeCancelled;

		// Token: 0x04000A98 RID: 2712
		public bool destroy;

		// Token: 0x04000A99 RID: 2713
		public int id;

		// Token: 0x04000A9A RID: 2714
		public int moneyReward;

		// Token: 0x04000A9B RID: 2715
		public int questType;

		// Token: 0x04000A9C RID: 2716
		public int daysLeft;

		// Token: 0x04000A9D RID: 2717
		public List<int> nextQuests = new List<int>();
	}
}
