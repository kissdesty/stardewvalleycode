using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;

namespace StardewValley.Locations
{
	// Token: 0x0200012C RID: 300
	public class JojaMart : GameLocation
	{
		// Token: 0x06001146 RID: 4422 RVA: 0x00151B90 File Offset: 0x0014FD90
		public JojaMart()
		{
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00151B98 File Offset: 0x0014FD98
		public JojaMart(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00162EE4 File Offset: 0x001610E4
		private bool signUpForJoja(int response)
		{
			if (response == 0)
			{
				base.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:JojaMart_SignUp", new object[0])), base.createYesNoResponses(), "JojaSignUp");
				return true;
			}
			Game1.dialogueUp = false;
			Game1.player.forceCanMove();
			Game1.playSound("smallSelect");
			Game1.currentSpeaker = null;
			Game1.dialogueTyping = false;
			return true;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00162F48 File Offset: 0x00161148
		public override bool answerDialogue(Response answer)
		{
			string questionAndAnswer = this.lastQuestionKey.Split(new char[]
			{
				' '
			})[0] + "_" + answer.responseKey;
			if (questionAndAnswer == "JojaSignUp_Yes")
			{
				if (Game1.player.Money >= 5000)
				{
					Game1.player.Money -= 5000;
					Game1.addMailForTomorrow("JojaMember", true, true);
					Game1.player.removeQuest(26);
					JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_PlayerSignedUp", new object[0]), false, false);
					Game1.drawDialogue(JojaMart.Morris);
				}
				else if (Game1.player.Money < 5000)
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1", new object[0]));
				}
				return true;
			}
			return base.answerDialogue(answer);
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0016302E File Offset: 0x0016122E
		public override void cleanupBeforePlayerExit()
		{
			if (!Game1.isRaining)
			{
				Game1.changeMusicTrack("none");
			}
			if (this.tempContent != null)
			{
				this.tempContent.Unload();
			}
			this.tempContent = null;
			base.cleanupBeforePlayerExit();
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00163064 File Offset: 0x00161264
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				PropertyValue action = "";
				this.map.GetLayer("Buildings").Tiles[tileLocation].Properties.TryGetValue("Action", out action);
				if (action != null)
				{
					string a = action.ToString();
					if (!(a == "JojaShop") && a == "JoinJoja")
					{
						JojaMart.Morris.CurrentDialogue.Clear();
						if (Game1.player.mailForTomorrow.Contains("JojaMember%&NL&%"))
						{
							JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_ComeBackLater", new object[0]), false, false);
							Game1.drawDialogue(JojaMart.Morris);
						}
						else if (!Game1.player.mailReceived.Contains("JojaMember"))
						{
							if (!Game1.player.mailReceived.Contains("JojaGreeting"))
							{
								JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_Greeting", new object[0]), false, false);
								Game1.player.mailReceived.Add("JojaGreeting");
								Game1.drawDialogue(JojaMart.Morris);
							}
							else if (Game1.stats.DaysPlayed < 0u)
							{
								string greeting = (Game1.dayOfMonth % 7 == 0 || Game1.dayOfMonth % 7 == 6) ? "Data\\ExtraDialogue:Morris_WeekendGreeting" : "Data\\ExtraDialogue:Morris_FirstGreeting";
								JojaMart.Morris.setNewDialogue(Game1.content.LoadString(greeting, new object[0]), false, false);
								Game1.drawDialogue(JojaMart.Morris);
							}
							else
							{
								string greeting2 = (Game1.dayOfMonth % 7 == 0 || Game1.dayOfMonth % 7 == 6) ? "Data\\ExtraDialogue:Morris_WeekendGreeting" : "Data\\ExtraDialogue:Morris_FirstGreeting";
								if (!Game1.IsMultiplayer || Game1.IsServer)
								{
									JojaMart.Morris.setNewDialogue(Game1.content.LoadString(greeting2 + "_MembershipAvailable", new object[]
									{
										5000
									}), false, false);
									JojaMart.Morris.CurrentDialogue.Peek().answerQuestionBehavior = new Dialogue.onAnswerQuestion(this.signUpForJoja);
								}
								else
								{
									JojaMart.Morris.setNewDialogue(greeting2 + "_SecondPlayer", false, false);
								}
								Game1.drawDialogue(JojaMart.Morris);
							}
						}
						else
						{
							if (Game1.player.mailForTomorrow.Contains("jojaFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaPantry%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaBoilerRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaVault%&NL&%"))
							{
								JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_StillProcessingOrder", new object[0]), false, false);
							}
							else
							{
								if (Game1.player.isMale)
								{
									JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_CommunityDevelopmentForm_PlayerMale", new object[0]), false, false);
								}
								else
								{
									JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_CommunityDevelopmentForm_PlayerFemale", new object[0]), false, false);
								}
								JojaMart.Morris.CurrentDialogue.Peek().answerQuestionBehavior = new Dialogue.onAnswerQuestion(this.viewJojaNote);
							}
							Game1.drawDialogue(JojaMart.Morris);
						}
					}
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x001633D5 File Offset: 0x001615D5
		private bool viewJojaNote(int response)
		{
			if (response == 0)
			{
				Game1.activeClickableMenu = new JojaCDMenu(this.communityDevelopmentTexture);
			}
			Game1.dialogueUp = false;
			Game1.player.forceCanMove();
			Game1.playSound("smallSelect");
			Game1.currentSpeaker = null;
			Game1.dialogueTyping = false;
			return true;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00163414 File Offset: 0x00161614
		public override void resetForPlayerEntry()
		{
			this.tempContent = Game1.content.CreateTemporary();
			this.communityDevelopmentTexture = this.tempContent.Load<Texture2D>("LooseSprites\\JojaCDForm");
			JojaMart.Morris = new NPC(null, Vector2.Zero, "JojaMart", 2, "Morris", false, null, this.tempContent.Load<Texture2D>("Portraits\\Morris"));
			base.resetForPlayerEntry();
		}

		// Token: 0x04001249 RID: 4681
		public const int JojaMembershipPrice = 5000;

		// Token: 0x0400124A RID: 4682
		public static NPC Morris;

		// Token: 0x0400124B RID: 4683
		private LocalizedContentManager tempContent;

		// Token: 0x0400124C RID: 4684
		private Texture2D communityDevelopmentTexture;
	}
}
