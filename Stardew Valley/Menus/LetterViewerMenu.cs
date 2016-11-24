using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;

namespace StardewValley.Menus
{
	// Token: 0x020000FA RID: 250
	public class LetterViewerMenu : IClickableMenu
	{
		// Token: 0x06000F05 RID: 3845 RVA: 0x00133400 File Offset: 0x00131600
		public LetterViewerMenu(string text) : base((int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			Game1.playSound("shwip");
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			Game1.temporaryContent = Game1.content.CreateTemporary();
			this.letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
			this.mailMessage = SpriteText.getStringBrokenIntoSectionsOfHeight(text, this.width - Game1.tileSize / 2, this.height - Game1.tileSize * 2);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x001335E0 File Offset: 0x001317E0
		public LetterViewerMenu(string mail, string mailTitle) : base((int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			this.isMail = true;
			Game1.playSound("shwip");
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "");
			this.mailTitle = mailTitle;
			Game1.temporaryContent = Game1.content.CreateTemporary();
			this.letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
			if (mail.Contains("%item"))
			{
				string itemDescription = mail.Substring(mail.IndexOf("%item"), mail.IndexOf("%%") + 2 - mail.IndexOf("%item"));
				string[] split = itemDescription.Split(new char[]
				{
					' '
				});
				mail = mail.Replace(itemDescription, "");
				if (split[1].Equals("object"))
				{
					int maxNum = split.Length - 1;
					int which = Game1.random.Next(2, maxNum);
					which -= which % 2;
					Object o = new Object(Vector2.Zero, Convert.ToInt32(split[which]), Convert.ToInt32(split[which + 1]));
					this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), o));
				}
				else if (split[1].Equals("tools"))
				{
					for (int i = 2; i < split.Length; i++)
					{
						Item tool = null;
						string a = split[i];
						if (!(a == "Axe"))
						{
							if (!(a == "Hoe"))
							{
								if (!(a == "Can"))
								{
									if (!(a == "Scythe"))
									{
										if (a == "Pickaxe")
										{
											tool = new Pickaxe();
										}
									}
									else
									{
										tool = new MeleeWeapon(47);
									}
								}
								else
								{
									tool = new WateringCan();
								}
							}
							else
							{
								tool = new Hoe();
							}
						}
						else
						{
							tool = new Axe();
						}
						if (tool != null)
						{
							this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), tool));
						}
					}
				}
				else if (split[1].Equals("bigobject"))
				{
					int maxNum2 = split.Length - 1;
					int which2 = Game1.random.Next(2, maxNum2);
					Object o2 = new Object(Vector2.Zero, Convert.ToInt32(split[which2]), false);
					this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), o2));
				}
				else if (split[1].Equals("money"))
				{
					int moneyToAdd = (split.Length > 4) ? Game1.random.Next(Convert.ToInt32(split[2]), Convert.ToInt32(split[3])) : Convert.ToInt32(split[2]);
					moneyToAdd -= moneyToAdd % 10;
					Game1.player.Money += moneyToAdd;
					this.moneyIncluded = moneyToAdd;
				}
				else if (split[1].Equals("quest"))
				{
					this.questID = Convert.ToInt32(split[2]);
					if (split.Length > 4)
					{
						if (!Game1.player.mailReceived.Contains("NOQUEST_" + this.questID))
						{
							Game1.player.addQuest(this.questID);
						}
						this.questID = -1;
					}
				}
				else
				{
					if (split[1].Equals("cookingRecipe"))
					{
						Dictionary<string, string> cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
						using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = cookingRecipes.Keys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string s = enumerator.Current;
								string[] getConditions = cookingRecipes[s].Split(new char[]
								{
									'/'
								})[3].Split(new char[]
								{
									' '
								});
								if (getConditions[0].Equals("f") && getConditions[1].Equals(mailTitle.Replace("Cooking", "")) && Game1.player.friendships[getConditions[1]][0] >= Convert.ToInt32(getConditions[2]) * 250 && !Game1.player.cookingRecipes.ContainsKey(s))
								{
									Game1.player.cookingRecipes.Add(s, 0);
									this.learnedRecipe = s;
									this.cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_cooking", new object[0]);
									break;
								}
							}
							goto IL_704;
						}
					}
					if (split[1].Equals("craftingRecipe"))
					{
						this.learnedRecipe = split[2].Replace('_', ' ');
						Game1.player.craftingRecipes.Add(this.learnedRecipe, 0);
						this.cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_crafting", new object[0]);
					}
				}
			}
			IL_704:
			Random r = new Random((int)(Game1.uniqueIDForThisGame / 2uL) - Game1.year);
			mail = mail.Replace("%secretsanta", Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player)).name);
			this.mailMessage = SpriteText.getStringBrokenIntoSectionsOfHeight(mail, this.width - Game1.tileSize, this.height - Game1.tileSize * 2);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00133D60 File Offset: 0x00131F60
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X;
			this.yPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y;
			this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
			this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "");
			using (List<ClickableComponent>.Enumerator enumerator = this.itemsToGrab.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.bounds = new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom);
				}
			}
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00133F80 File Offset: 0x00132180
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.scale < 1f)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			if (Game1.activeClickableMenu == null && Game1.currentMinigame == null)
			{
				this.unload();
				return;
			}
			foreach (ClickableComponent c in this.itemsToGrab)
			{
				if (c.containsPoint(x, y) && c.item != null)
				{
					Game1.playSound("coin");
					Game1.player.addItemByMenuIfNecessary(c.item, null);
					c.item = null;
					return;
				}
			}
			if (this.backButton.containsPoint(x, y) && this.page > 0)
			{
				this.page--;
				Game1.playSound("shwip");
				return;
			}
			if (this.forwardButton.containsPoint(x, y) && this.page < this.mailMessage.Count - 1)
			{
				this.page++;
				Game1.playSound("shwip");
				return;
			}
			if (this.questID != -1 && this.acceptQuestButton.containsPoint(x, y))
			{
				Game1.player.addQuest(this.questID);
				this.questID = -1;
				Game1.playSound("newArtifact");
				return;
			}
			if (this.isWithinBounds(x, y))
			{
				if (this.page < this.mailMessage.Count - 1)
				{
					this.page++;
					Game1.playSound("shwip");
				}
				else if (this.page == this.mailMessage.Count - 1 && this.mailMessage.Count > 1)
				{
					this.page = 0;
					Game1.playSound("shwip");
				}
				if (this.mailMessage.Count == 1 && !this.isMail)
				{
					base.exitThisMenuNoSound();
					Game1.playSound("shwip");
					return;
				}
			}
			else if (!this.itemsLeftToGrab())
			{
				base.exitThisMenuNoSound();
				Game1.playSound("shwip");
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00134188 File Offset: 0x00132388
		public bool itemsLeftToGrab()
		{
			if (this.itemsToGrab == null)
			{
				return false;
			}
			using (List<ClickableComponent>.Enumerator enumerator = this.itemsToGrab.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x001341EC File Offset: 0x001323EC
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			foreach (ClickableComponent c in this.itemsToGrab)
			{
				if (c.containsPoint(x, y))
				{
					c.scale = Math.Min(c.scale + 0.03f, 1.1f);
				}
				else
				{
					c.scale = Math.Max(1f, c.scale - 0.03f);
				}
			}
			this.backButton.tryHover(x, y, 0.6f);
			this.forwardButton.tryHover(x, y, 0.6f);
			if (this.questID != -1)
			{
				float oldScale = this.acceptQuestButton.scale;
				this.acceptQuestButton.scale = (this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f);
				if (this.acceptQuestButton.scale > oldScale)
				{
					Game1.playSound("Cowboy_gunshot");
				}
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00134300 File Offset: 0x00132500
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.scale < 1f)
			{
				this.scale += (float)time.ElapsedGameTime.Milliseconds * 0.003f;
				if (this.scale >= 1f)
				{
					this.scale = 1f;
				}
			}
			if (this.page < this.mailMessage.Count - 1 && !this.forwardButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				this.forwardButton.scale = 4f + (float)Math.Sin((double)((float)time.TotalGameTime.Milliseconds) / 201.06192982974676) / 1.5f;
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x001343C0 File Offset: 0x001325C0
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			b.Draw(this.letterTexture, new Vector2((float)(this.xPositionOnScreen + this.width / 2), (float)(this.yPositionOnScreen + this.height / 2)), new Rectangle?(new Rectangle(0, 0, 320, 180)), Color.White, 0f, new Vector2(160f, 90f), (float)Game1.pixelZoom * this.scale, SpriteEffects.None, 0.86f);
			if (this.scale == 1f)
			{
				SpriteText.drawString(b, this.mailMessage[this.page], this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize / 2, 999999, this.width - Game1.tileSize, 999999, 0.75f, 0.865f, false, -1, "", -1);
				foreach (ClickableComponent c in this.itemsToGrab)
				{
					b.Draw(this.letterTexture, c.bounds, new Rectangle?(new Rectangle(0, 180, 24, 24)), Color.White);
					if (c.item != null)
					{
						c.item.drawInMenu(b, new Vector2((float)(c.bounds.X + 4 * Game1.pixelZoom), (float)(c.bounds.Y + 4 * Game1.pixelZoom)), c.scale);
					}
				}
				if (this.moneyIncluded > 0)
				{
					string moneyText = Game1.content.LoadString("Strings\\UI:LetterViewer_MoneyIncluded", new object[]
					{
						this.moneyIncluded
					});
					SpriteText.drawString(b, moneyText, this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString(moneyText) / 2, this.yPositionOnScreen + this.height - Game1.tileSize * 3 / 2, 999999, -1, 9999, 0.75f, 0.865f, false, -1, "", -1);
				}
				else if (this.learnedRecipe != null && this.learnedRecipe.Length > 0)
				{
					string recipeText = Game1.content.LoadString("Strings\\UI:LetterViewer_LearnedRecipe", new object[]
					{
						this.cookingOrCrafting
					});
					SpriteText.drawStringHorizontallyCenteredAt(b, recipeText, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - SpriteText.getHeightOfString(recipeText, 999999) * 2, 999999, this.width - Game1.tileSize, 9999, 0.65f, 0.865f, false, -1);
					SpriteText.drawStringHorizontallyCenteredAt(b, Game1.content.LoadString("Strings\\UI:LetterViewer_LearnedRecipeName", new object[]
					{
						this.learnedRecipe
					}), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - SpriteText.getHeightOfString("t", 999999), 999999, this.width - Game1.tileSize, 9999, 0.9f, 0.865f, false, -1);
				}
				base.draw(b);
				if (this.page < this.mailMessage.Count - 1)
				{
					this.forwardButton.draw(b);
				}
				if (this.page > 0)
				{
					this.backButton.draw(b);
				}
				if (this.questID != -1)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (this.acceptQuestButton.scale > 1f) ? Color.LightPink : Color.White, (float)Game1.pixelZoom * this.acceptQuestButton.scale, true);
					Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest", new object[0]), Game1.dialogueFont, new Vector2((float)(this.acceptQuestButton.bounds.X + Game1.pixelZoom * 3), (float)(this.acceptQuestButton.bounds.Y + Game1.pixelZoom * 3)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
				}
			}
			if (!Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x001348D4 File Offset: 0x00132AD4
		public void unload()
		{
			Game1.temporaryContent.Unload();
			Game1.temporaryContent = null;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00121B04 File Offset: 0x0011FD04
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.receiveLeftClick(x, y, playSound);
		}

		// Token: 0x0400101E RID: 4126
		public const int letterWidth = 320;

		// Token: 0x0400101F RID: 4127
		public const int letterHeight = 180;

		// Token: 0x04001020 RID: 4128
		public Texture2D letterTexture;

		// Token: 0x04001021 RID: 4129
		private int moneyIncluded;

		// Token: 0x04001022 RID: 4130
		private int questID = -1;

		// Token: 0x04001023 RID: 4131
		private string learnedRecipe = "";

		// Token: 0x04001024 RID: 4132
		private string cookingOrCrafting = "";

		// Token: 0x04001025 RID: 4133
		private string mailTitle;

		// Token: 0x04001026 RID: 4134
		private List<string> mailMessage = new List<string>();

		// Token: 0x04001027 RID: 4135
		private int page;

		// Token: 0x04001028 RID: 4136
		private List<ClickableComponent> itemsToGrab = new List<ClickableComponent>();

		// Token: 0x04001029 RID: 4137
		private float scale;

		// Token: 0x0400102A RID: 4138
		private bool isMail;

		// Token: 0x0400102B RID: 4139
		private ClickableTextureComponent backButton;

		// Token: 0x0400102C RID: 4140
		private ClickableTextureComponent forwardButton;

		// Token: 0x0400102D RID: 4141
		private ClickableComponent acceptQuestButton;

		// Token: 0x0400102E RID: 4142
		public const float scaleChange = 0.003f;
	}
}
