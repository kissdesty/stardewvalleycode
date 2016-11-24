using System;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x02000158 RID: 344
	public class Lexicon
	{
		// Token: 0x06001318 RID: 4888 RVA: 0x00180E38 File Offset: 0x0017F038
		public static string getRandomNegativeItemSlanderNoun()
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeItemNoun", new object[0]).Split(new char[]
			{
				'#'
			});
			return choices[r.Next(choices.Length)];
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00180E8F File Offset: 0x0017F08F
		public static string appendArticle(string word)
		{
			return Game1.getProperArticleForWord(word) + " " + word;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00180EA4 File Offset: 0x0017F0A4
		public static string getRandomPositiveAdjectiveForEventOrPerson(NPC n = null)
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] choices;
			if (n != null && n.age != 0)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 0)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultMale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 1)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultFemale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_PlaceOrEvent", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return choices[r.Next(choices.Length)];
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00180F98 File Offset: 0x0017F198
		public static string getRandomNegativeAdjectiveForEventOrPerson(NPC n = null)
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] choices;
			if (n != null && n.age != 0)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 0)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultMale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.gender == 1)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultFemale", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_PlaceOrEvent", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return choices[r.Next(choices.Length)];
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0018108C File Offset: 0x0017F28C
		public static string getRandomDeliciousAdjective(NPC n = null)
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] choices;
			if (n != null && n.age == 2)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return choices[r.Next(choices.Length)];
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00181118 File Offset: 0x0017F318
		public static string getRandomNegativeFoodAdjective(NPC n = null)
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] choices;
			if (n != null && n.age == 2)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Child", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else if (n != null && n.manners == 1)
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Polite", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			else
			{
				choices = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective", new object[0]).Split(new char[]
				{
					'#'
				});
			}
			return choices[r.Next(choices.Length)];
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x001811D8 File Offset: 0x0017F3D8
		public static string getRandomSlightlyPositiveAdjectiveForEdibleNoun(NPC n = null)
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			string[] choices = Game1.content.LoadString("Strings\\Lexicon:RandomSlightlyPositiveFoodAdjective", new object[0]).Split(new char[]
			{
				'#'
			});
			return choices[r.Next(choices.Length)];
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0018122F File Offset: 0x0017F42F
		public static string getGenderedChildTerm(bool isMale)
		{
			if (isMale)
			{
				return Game1.content.LoadString("Strings\\Lexicon:ChildTerm_Male", new object[0]);
			}
			return Game1.content.LoadString("Strings\\Lexicon:ChildTerm_Female", new object[0]);
		}
	}
}
