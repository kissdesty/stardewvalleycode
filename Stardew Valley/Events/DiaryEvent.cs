using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Events
{
	// Token: 0x02000139 RID: 313
	public class DiaryEvent : FarmEvent
	{
		// Token: 0x060011DA RID: 4570 RVA: 0x0016D7B8 File Offset: 0x0016B9B8
		public bool setUp()
		{
			if (Game1.player.isMarried())
			{
				return true;
			}
			foreach (string s in Game1.player.mailReceived)
			{
				if (s.Contains("diary"))
				{
					string name = s.Split(new char[]
					{
						'_'
					})[1];
					if (!Game1.player.mailReceived.Contains("diary_" + name + "_finished"))
					{
						Convert.ToInt32(name.Split(new char[]
						{
							'/'
						})[1]);
						this.NPCname = name.Split(new char[]
						{
							'/'
						})[0];
						NPC expr_B1 = Game1.getCharacterFromName(this.NPCname, false);
						string thirdPersonObjectivePronoun = (expr_B1.gender == 0) ? "him" : "her";
						int arg_CB_0 = expr_B1.gender;
						Game1.player.mailReceived.Add("diary_" + name + "_finished");
						string question = string.Concat(new object[]
						{
							"I think I'll write in my ",
							Game1.player.isMale ? "journal" : "diary",
							" tonight...",
							Environment.NewLine,
							Environment.NewLine,
							"-",
							Utility.capitalizeFirstLetter(Game1.currentSeason),
							" ",
							Game1.dayOfMonth,
							"-",
							Environment.NewLine,
							"I've been spending a lot of time with ",
							this.NPCname,
							" lately..."
						});
						Response[] diaryOptions = new Response[]
						{
							new Response("...We're", "...We're becoming good friends. I hope things stay that way."),
							new Response("...I", "...I like " + thirdPersonObjectivePronoun + " more than just a friend."),
							new Response("(Write", "(Write Nothing)")
						};
						Game1.currentLocation.createQuestionDialogue(Game1.parseText(question), diaryOptions, "diary");
						Game1.messagePause = true;
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0016D9FC File Offset: 0x0016BBFC
		public bool tickUpdate(GameTime time)
		{
			return !Game1.dialogueUp;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00002834 File Offset: 0x00000A34
		public void draw(SpriteBatch b)
		{
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0016DA06 File Offset: 0x0016BC06
		public void makeChangesToLocation()
		{
			Game1.messagePause = false;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00002834 File Offset: 0x00000A34
		public void drawAboveEverything(SpriteBatch b)
		{
		}

		// Token: 0x040012AA RID: 4778
		public string NPCname;
	}
}
