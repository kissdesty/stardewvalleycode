using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000DF RID: 223
	public class ChatBox : IClickableMenu
	{
		// Token: 0x06000DD1 RID: 3537 RVA: 0x0011AFB4 File Offset: 0x001191B4
		public ChatBox() : base(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize, Game1.viewport.Height - Game1.tileSize * 2 - Game1.tileSize / 2, Game1.tileSize * 14, 56, false)
		{
			this.chatBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\chatBox"), null, Game1.smallFont, Color.White);
			this.e = new TextBoxEvent(this.textBoxEnter);
			this.chatBox.OnEnterPressed += this.e;
			this.keyboardDispatcher = Game1.keyboardDispatcher;
			this.keyboardDispatcher.Subscriber = this.chatBox;
			this.chatBox.X = this.xPositionOnScreen;
			this.chatBox.Y = this.yPositionOnScreen;
			this.chatBox.Width = this.width;
			this.chatBox.Height = 56;
			this.chatBox.Selected = false;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0011B0CC File Offset: 0x001192CC
		public void textBoxEnter(TextBox sender)
		{
			if (Game1.IsMultiplayer && sender.Text.Length > 0)
			{
				string textToSend = sender.Text.Trim();
				if (textToSend.Length < 1)
				{
					return;
				}
				if (textToSend[0] == '/' && textToSend.Split(new char[]
				{
					' '
				})[0].Length > 1)
				{
					try
					{
						string text = textToSend.Split(new char[]
						{
							' '
						})[0].Substring(1);
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num <= 1013213428u)
						{
							if (num != 355814093u)
							{
								if (num != 405908334u)
								{
									if (num != 1013213428u)
									{
										goto IL_211;
									}
									if (!(text == "texture"))
									{
										goto IL_211;
									}
									Game1.player.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + textToSend.Split(new char[]
									{
										' '
									})[1]);
									goto IL_21E;
								}
								else if (!(text == "nick"))
								{
									goto IL_211;
								}
							}
							else if (!(text == "nickname"))
							{
								goto IL_211;
							}
						}
						else if (num <= 2180167635u)
						{
							if (num != 1158129075u)
							{
								if (num != 2180167635u)
								{
									goto IL_211;
								}
								if (!(text == "rename"))
								{
									goto IL_211;
								}
							}
							else
							{
								if (!(text == "othergirl"))
								{
									goto IL_211;
								}
								Game1.otherFarmers.Values.ElementAt(0).Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\farmergirl");
								goto IL_21E;
							}
						}
						else if (num != 2369371622u)
						{
							if (num != 2723493283u)
							{
								goto IL_211;
							}
							if (!(text == "girl"))
							{
								goto IL_211;
							}
							Game1.player.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\farmergirl");
							Game1.player.isMale = false;
							goto IL_21E;
						}
						else if (!(text == "name"))
						{
							goto IL_211;
						}
						MultiplayerUtility.sendNameChange(textToSend.Substring(textToSend.IndexOf(' ') + 1), Game1.player.uniqueMultiplayerID);
						goto IL_21E;
						IL_211:
						this.receiveChatMessage(" Invalid Command ::", 0L);
						IL_21E:
						goto IL_258;
					}
					catch (Exception)
					{
						this.receiveChatMessage(" Invalid Command Arguments ::", 0L);
						goto IL_258;
					}
				}
				MultiplayerUtility.sendChatMessage(textToSend, Game1.player.uniqueMultiplayerID);
				if (Game1.IsServer)
				{
					this.receiveChatMessage(textToSend, Game1.player.uniqueMultiplayerID);
				}
			}
			IL_258:
			sender.Text = "";
			this.clickAway();
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0011B360 File Offset: 0x00119560
		public override void clickAway()
		{
			base.clickAway();
			this.chatBox.Selected = false;
			Game1.isChatting = false;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0011B37C File Offset: 0x0011957C
		public void receiveChatMessage(string message, long who)
		{
			string username = Game1.player.name;
			using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Farmer f = enumerator.Current;
					if (f.uniqueMultiplayerID == who)
					{
						username = f.name;
					}
				}
			}
			username += ":";
			if (who == 0L)
			{
				username = "::";
			}
			else if (who == -1L)
			{
				username = ">";
			}
			else if (who == -2L)
			{
				username = "";
			}
			ChatMessage c = new ChatMessage();
			c.message = Game1.parseText(username + " " + message, this.chatBox._font, Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize);
			c.timeLeftToDisplay = 600;
			c.verticalSize = (int)this.chatBox._font.MeasureString(c.message).Y;
			this.messages.Add(c);
			if (this.messages.Count > this.maxMessages)
			{
				this.messages.RemoveAt(0);
			}
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00002834 File Offset: 0x00000A34
		public override void performHoverAction(int x, int y)
		{
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0011B4B8 File Offset: 0x001196B8
		public void update()
		{
			for (int i = 0; i < this.messages.Count; i++)
			{
				if (this.messages[i].timeLeftToDisplay > 0)
				{
					this.messages[i].timeLeftToDisplay--;
				}
				if (this.messages[i].timeLeftToDisplay < 75)
				{
					this.messages[i].alpha = (float)this.messages[i].timeLeftToDisplay / 75f;
				}
			}
			if (this.chatBox.Selected)
			{
				using (List<ChatMessage>.Enumerator enumerator = this.messages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.alpha = 1f;
					}
				}
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0011B59C File Offset: 0x0011979C
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.yPositionOnScreen = newBounds.Height - Game1.tileSize * 2 - Game1.tileSize / 2;
			this.chatBox.Y = this.yPositionOnScreen;
			this.chatBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize;
			this.chatBox.Width = Game1.tileSize * 14;
			this.width = Game1.tileSize * 14;
			this.chatBox.Height = 56;
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0011B62C File Offset: 0x0011982C
		public override void draw(SpriteBatch b)
		{
			int heightSoFar = 0;
			for (int i = this.messages.Count - 1; i >= 0; i--)
			{
				heightSoFar += this.messages[i].verticalSize;
				b.DrawString(this.chatBox._font, this.messages[i].message, new Vector2(4f, (float)(Game1.viewport.Height - heightSoFar - 8)), this.chatBox.textColor * this.messages[i].alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
			}
			if (this.chatBox.Selected)
			{
				this.chatBox.Draw(b);
			}
			this.update();
		}

		// Token: 0x04000EAF RID: 3759
		public const int errorMessage = 0;

		// Token: 0x04000EB0 RID: 3760
		public const int userNotificationMessage = -1;

		// Token: 0x04000EB1 RID: 3761
		public const int blankMessage = -2;

		// Token: 0x04000EB2 RID: 3762
		public const int defaultMaxMessages = 10;

		// Token: 0x04000EB3 RID: 3763
		public const int timeToDisplayMessages = 600;

		// Token: 0x04000EB4 RID: 3764
		public TextBox chatBox;

		// Token: 0x04000EB5 RID: 3765
		private TextBoxEvent e;

		// Token: 0x04000EB6 RID: 3766
		private KeyboardDispatcher keyboardDispatcher;

		// Token: 0x04000EB7 RID: 3767
		private List<ChatMessage> messages = new List<ChatMessage>();

		// Token: 0x04000EB8 RID: 3768
		public int maxMessages = 10;
	}
}
