using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace StardewValley.Events
{
	// Token: 0x0200013A RID: 314
	public class QuestionEvent : FarmEvent
	{
		// Token: 0x060011E0 RID: 4576 RVA: 0x0016DA0E File Offset: 0x0016BC0E
		public QuestionEvent(int whichQuestion)
		{
			this.whichQuestion = whichQuestion;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0016DA20 File Offset: 0x0016BC20
		public bool setUp()
		{
			int num = this.whichQuestion;
			if (num != 1)
			{
				if (num == 2)
				{
					FarmAnimal a = null;
					foreach (Building b in Game1.getFarm().buildings)
					{
						if ((b.owner.Equals(Game1.uniqueIDForThisGame) || !Game1.IsMultiplayer) && b.buildingType.Contains("Barn") && !b.buildingType.Equals("Barn") && !(b.indoors as AnimalHouse).isFull() && Game1.random.NextDouble() < (double)(b.indoors as AnimalHouse).animalsThatLiveHere.Count * 0.0055)
						{
							a = Utility.getAnimal((b.indoors as AnimalHouse).animalsThatLiveHere[Game1.random.Next((b.indoors as AnimalHouse).animalsThatLiveHere.Count)]);
							this.animalHouse = (b.indoors as AnimalHouse);
							break;
						}
					}
					if (a != null && !a.isBaby() && a.allowReproduction)
					{
						Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Events:AnimalBirth", new object[]
						{
							a.name,
							a.type.Split(new char[]
							{
								' '
							}).Last<string>()
						}));
						Game1.messagePause = true;
						this.animal = a;
						return false;
					}
				}
				return true;
			}
			Response[] answers = new Response[]
			{
				new Response("Yes", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_Yes", new object[0])),
				new Response("Not", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_No", new object[0]))
			};
			if (!Game1.getCharacterFromName(Game1.player.spouse, false).isGaySpouse())
			{
				Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HaveBabyQuestion", new object[]
				{
					Game1.player.name
				}), answers, new GameLocation.afterQuestionBehavior(this.answerPregnancyQuestion), Game1.getCharacterFromName(Game1.player.spouse, false));
			}
			else
			{
				Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HaveBabyQuestion_Adoption", new object[]
				{
					Game1.player.name
				}), answers, new GameLocation.afterQuestionBehavior(this.answerPregnancyQuestion), Game1.getCharacterFromName(Game1.player.spouse, false));
			}
			Game1.messagePause = true;
			return false;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0016DCD0 File Offset: 0x0016BED0
		private void answerPregnancyQuestion(Farmer who, string answer)
		{
			if (answer.Equals(Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_Yes", new object[0])))
			{
				Game1.getCharacterFromName(who.spouse, false).daysUntilBirthing = 14;
				Game1.getCharacterFromName(who.spouse, false).isGaySpouse();
			}
			Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float)Game1.tileSize;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0016DD48 File Offset: 0x0016BF48
		public bool tickUpdate(GameTime time)
		{
			if (this.forceProceed)
			{
				return true;
			}
			if (this.whichQuestion == 2 && !Game1.dialogueUp)
			{
				if (Game1.activeClickableMenu == null)
				{
					Game1.activeClickableMenu = new NamingMenu(new NamingMenu.doneNamingBehavior(this.animalHouse.addNewHatchedAnimal), (this.animal != null) ? Game1.content.LoadString("Strings\\Events:AnimalNamingTitle", new object[]
					{
						this.animal.type
					}) : "Choose a name:", null);
				}
				return false;
			}
			return !Game1.dialogueUp;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00002834 File Offset: 0x00000A34
		public void draw(SpriteBatch b)
		{
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00002834 File Offset: 0x00000A34
		public void drawAboveEverything(SpriteBatch b)
		{
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0016DDCE File Offset: 0x0016BFCE
		public void makeChangesToLocation()
		{
			Game1.messagePause = false;
			Game1.player.position = Game1.player.mostRecentBed;
		}

		// Token: 0x040012AB RID: 4779
		public const int pregnancyQuestion = 1;

		// Token: 0x040012AC RID: 4780
		public const int barnBirth = 2;

		// Token: 0x040012AD RID: 4781
		private int whichQuestion;

		// Token: 0x040012AE RID: 4782
		private AnimalHouse animalHouse;

		// Token: 0x040012AF RID: 4783
		public FarmAnimal animal;

		// Token: 0x040012B0 RID: 4784
		public bool forceProceed;
	}
}
