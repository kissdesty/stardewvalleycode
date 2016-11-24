using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;

namespace StardewValley.Events
{
	// Token: 0x02000137 RID: 311
	public class BirthingEvent : FarmEvent
	{
		// Token: 0x060011CC RID: 4556 RVA: 0x0016C774 File Offset: 0x0016A974
		public BirthingEvent()
		{
			this.babyNameBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Game1.textColor)
			{
				X = Game1.graphics.GraphicsDevice.Viewport.Width / 2 - Game1.tileSize * 3,
				Y = Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.tileSize,
				Width = Game1.tileSize * 2
			};
			this.okButton = new ClickableTextureComponent(new Rectangle(this.babyNameBox.X + this.babyNameBox.Width + Game1.tileSize / 2, this.babyNameBox.Y - Game1.tileSize / 8, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0016C86C File Offset: 0x0016AA6C
		public bool setUp()
		{
			Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			Utility.getHomeOfFarmer(Game1.player);
			NPC spouse = Game1.getCharacterFromName(Game1.player.spouse, false);
			Game1.player.CanMove = false;
			if (Game1.player.getNumberOfChildren() == 0)
			{
				this.isMale = (r.NextDouble() < 0.5);
			}
			else
			{
				this.isMale = (Game1.player.getChildren()[0].gender == 1);
			}
			if (spouse.isGaySpouse())
			{
				this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_Adoption", new object[]
				{
					Lexicon.getGenderedChildTerm(this.isMale)
				});
			}
			else if (spouse.gender == 0)
			{
				this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_PlayerMother", new object[]
				{
					Lexicon.getGenderedChildTerm(this.isMale)
				});
			}
			else
			{
				this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_SpouseMother", new object[]
				{
					Lexicon.getGenderedChildTerm(this.isMale),
					spouse.name
				});
			}
			return false;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0016C993 File Offset: 0x0016AB93
		public void afterMessage()
		{
			this.getBabyName = true;
			this.babyNameBox.SelectMe();
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0016C9A8 File Offset: 0x0016ABA8
		public bool tickUpdate(GameTime time)
		{
			Game1.player.CanMove = false;
			this.timer += time.ElapsedGameTime.Milliseconds;
			Game1.fadeToBlackAlpha = 1f;
			if (this.timer > 1500 && !this.playedSound && !this.getBabyName)
			{
				if (this.soundName != null && !this.soundName.Equals(""))
				{
					Game1.playSound(this.soundName);
					this.playedSound = true;
				}
				if (!this.playedSound && this.message != null && !Game1.dialogueUp && Game1.activeClickableMenu == null)
				{
					Game1.drawObjectDialogue(this.message);
					Game1.afterDialogues = new Game1.afterFadeFunction(this.afterMessage);
					Game1.globalFadeToClear(null, 0.02f);
				}
			}
			else if (this.getBabyName && Game1.oldMouseState.LeftButton == ButtonState.Pressed && this.okButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && this.babyNameBox.Text.Length > 0)
			{
				double chance = Game1.player.spouse.Equals("Maru") ? 0.5 : 0.0;
				chance += (Game1.player.hasDarkSkin() ? 0.5 : 0.0);
				bool isDarkSkinned = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed).NextDouble() < chance;
				string babyName = this.babyNameBox.Text;
				using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.name.Equals(babyName))
						{
							babyName += " ";
							break;
						}
					}
				}
				Utility.getHomeOfFarmer(Game1.player).characters.Add(new Child(babyName, this.isMale, isDarkSkinned, Game1.player));
				Game1.playSound("smallSelect");
				Game1.player.getSpouse().daysAfterLastBirth = 5;
				Game1.player.getSpouse().daysUntilBirthing = -1;
				if (Game1.player.getChildren().Count == 2)
				{
					Game1.player.getSpouse().setNewDialogue((Game1.random.NextDouble() < 0.5) ? Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild1", new object[0]) : Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild2", new object[0]), false, false);
					Game1.getSteamAchievement("Achievement_FullHouse");
				}
				else if (Game1.player.getSpouse().isGaySpouse())
				{
					Game1.player.getSpouse().setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:NewChild_Adoption", new object[]
					{
						this.babyNameBox.Text
					}), false, false);
				}
				else
				{
					Game1.player.getSpouse().setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:NewChild_FirstChild", new object[]
					{
						this.babyNameBox.Text
					}), false, false);
				}
				if (Game1.keyboardDispatcher != null)
				{
					Game1.keyboardDispatcher.Subscriber = null;
				}
				Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float)Game1.tileSize;
				return true;
			}
			return false;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00002834 File Offset: 0x00000A34
		public void draw(SpriteBatch b)
		{
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00002834 File Offset: 0x00000A34
		public void makeChangesToLocation()
		{
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0016CD24 File Offset: 0x0016AF24
		public void drawAboveEverything(SpriteBatch b)
		{
			if (this.getBabyName)
			{
				Game1.drawWithBorder(Game1.content.LoadString(this.isMale ? "Strings\\Events:BabyNamingTitle_Male" : "Strings\\Events:BabyNamingTitle_Female", new object[0]), Color.Black, Color.White, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - Game1.tileSize * 4), (float)(Game1.graphics.GraphicsDevice.Viewport.Height / 2 - Game1.tileSize * 2)));
				Game1.drawDialogueBox(this.babyNameBox.X - Game1.tileSize / 2, this.babyNameBox.Y - Game1.tileSize * 3 / 2, this.babyNameBox.Width + Game1.tileSize, this.babyNameBox.Height + Game1.tileSize * 2, false, true, null, false);
				this.babyNameBox.Draw(b);
				this.okButton.draw(b);
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x04001294 RID: 4756
		private int behavior;

		// Token: 0x04001295 RID: 4757
		private int timer;

		// Token: 0x04001296 RID: 4758
		private string soundName;

		// Token: 0x04001297 RID: 4759
		private string message;

		// Token: 0x04001298 RID: 4760
		private bool playedSound;

		// Token: 0x04001299 RID: 4761
		private bool showedMessage;

		// Token: 0x0400129A RID: 4762
		private bool isMale;

		// Token: 0x0400129B RID: 4763
		private bool getBabyName;

		// Token: 0x0400129C RID: 4764
		private Vector2 targetLocation;

		// Token: 0x0400129D RID: 4765
		private TextBox babyNameBox;

		// Token: 0x0400129E RID: 4766
		private ClickableTextureComponent okButton;
	}
}
