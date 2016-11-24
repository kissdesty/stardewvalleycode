using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
	// Token: 0x02000117 RID: 279
	public class TextBox : IKeyboardSubscriber
	{
		// Token: 0x170000D8 RID: 216
		public int X
		{
			// Token: 0x06000FF7 RID: 4087 RVA: 0x0014B028 File Offset: 0x00149228
			get;
			// Token: 0x06000FF8 RID: 4088 RVA: 0x0014B030 File Offset: 0x00149230
			set;
		}

		// Token: 0x170000D9 RID: 217
		public int Y
		{
			// Token: 0x06000FF9 RID: 4089 RVA: 0x0014B039 File Offset: 0x00149239
			get;
			// Token: 0x06000FFA RID: 4090 RVA: 0x0014B041 File Offset: 0x00149241
			set;
		}

		// Token: 0x170000DA RID: 218
		public int Width
		{
			// Token: 0x06000FFB RID: 4091 RVA: 0x0014B04A File Offset: 0x0014924A
			get;
			// Token: 0x06000FFC RID: 4092 RVA: 0x0014B052 File Offset: 0x00149252
			set;
		}

		// Token: 0x170000DB RID: 219
		public int Height
		{
			// Token: 0x06000FFD RID: 4093 RVA: 0x0014B05B File Offset: 0x0014925B
			get;
			// Token: 0x06000FFE RID: 4094 RVA: 0x0014B063 File Offset: 0x00149263
			set;
		}

		// Token: 0x170000DC RID: 220
		public bool Highlighted
		{
			// Token: 0x06000FFF RID: 4095 RVA: 0x0014B06C File Offset: 0x0014926C
			get;
			// Token: 0x06001000 RID: 4096 RVA: 0x0014B074 File Offset: 0x00149274
			set;
		}

		// Token: 0x170000DD RID: 221
		public bool PasswordBox
		{
			// Token: 0x06001001 RID: 4097 RVA: 0x0014B07D File Offset: 0x0014927D
			get;
			// Token: 0x06001002 RID: 4098 RVA: 0x0014B085 File Offset: 0x00149285
			set;
		}

		// Token: 0x14000005 RID: 5
		// Token: 0x06001003 RID: 4099 RVA: 0x0014B090 File Offset: 0x00149290
		// Token: 0x06001004 RID: 4100 RVA: 0x0014B0C8 File Offset: 0x001492C8
		[method: CompilerGenerated]
		[CompilerGenerated]
		public event TextBoxEvent Clicked;

		// Token: 0x170000DE RID: 222
		public string Text
		{
			// Token: 0x06001005 RID: 4101 RVA: 0x0014B0FD File Offset: 0x001492FD
			get
			{
				return this._text;
			}
			// Token: 0x06001006 RID: 4102 RVA: 0x0014B108 File Offset: 0x00149308
			set
			{
				this._text = value;
				if (this._text == null)
				{
					this._text = "";
				}
				if (this._text != "")
				{
					string filtered = "";
					for (int i = 0; i < value.Length; i++)
					{
						char c = value[i];
						if (this._font.Characters.Contains(c))
						{
							filtered += c.ToString();
						}
					}
					this._text = filtered;
					if (this.limitWidth && this._font.MeasureString(this._text).X > (float)(this.Width - Game1.tileSize / 3))
					{
						this.Text = this._text.Substring(0, this._text.Length - 1);
					}
				}
			}
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0014B1DC File Offset: 0x001493DC
		public TextBox(Texture2D textBoxTexture, Texture2D caretTexture, SpriteFont font, Color textColor)
		{
			this._textBoxTexture = textBoxTexture;
			if (textBoxTexture != null)
			{
				this.Width = textBoxTexture.Width;
				this.Height = textBoxTexture.Height;
			}
			this._caretTexture = caretTexture;
			this._font = font;
			this.textColor = textColor;
			this._previousMouse = Mouse.GetState();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0014B24B File Offset: 0x0014944B
		public void SelectMe()
		{
			this.Highlighted = true;
			this.Selected = true;
			Game1.keyboardDispatcher.Subscriber = this;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0014B268 File Offset: 0x00149468
		public void Update()
		{
			MouseState mouse = Mouse.GetState();
			Point mousePoint = new Point(mouse.X, mouse.Y);
			Rectangle position = new Rectangle(this.X, this.Y, this.Width, this.Height);
			if (position.Contains(mousePoint))
			{
				this.Highlighted = true;
				this.Selected = true;
				Game1.keyboardDispatcher.Subscriber = this;
				if (this._previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed && this.Clicked != null)
				{
					this.Clicked(this);
					return;
				}
			}
			else
			{
				this.Selected = false;
				this.Highlighted = false;
			}
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0014B30C File Offset: 0x0014950C
		public void Draw(SpriteBatch spriteBatch)
		{
			bool caretVisible = DateTime.Now.Millisecond % 1000 >= 500;
			string toDraw = this.Text;
			if (this.PasswordBox)
			{
				toDraw = "";
				for (int i = 0; i < this.Text.Length; i++)
				{
					toDraw += "•";
				}
			}
			if (this._textBoxTexture != null)
			{
				spriteBatch.Draw(this._textBoxTexture, new Rectangle(this.X, this.Y, Game1.tileSize / 4, this.Height), new Rectangle?(new Rectangle(0, 0, Game1.tileSize / 4, this.Height)), Color.White);
				spriteBatch.Draw(this._textBoxTexture, new Rectangle(this.X + Game1.tileSize / 4, this.Y, this.Width - Game1.tileSize / 2, this.Height), new Rectangle?(new Rectangle(Game1.tileSize / 4, 0, 4, this.Height)), Color.White);
				spriteBatch.Draw(this._textBoxTexture, new Rectangle(this.X + this.Width - Game1.tileSize / 4, this.Y, Game1.tileSize / 4, this.Height), new Rectangle?(new Rectangle(this._textBoxTexture.Bounds.Width - Game1.tileSize / 4, 0, Game1.tileSize / 4, this.Height)), Color.White);
			}
			else
			{
				Game1.drawDialogueBox(this.X - Game1.tileSize / 2, this.Y - Game1.tileSize * 7 / 4 + 10, this.Width + Game1.tileSize * 5 / 4, this.Height, false, true, null, false);
			}
			Vector2 size = this._font.MeasureString(toDraw);
			while (size.X > (float)this.Width)
			{
				toDraw = toDraw.Substring(1);
				size = this._font.MeasureString(toDraw);
			}
			if (caretVisible && this.Selected)
			{
				spriteBatch.Draw(Game1.staminaRect, new Rectangle(this.X + Game1.tileSize / 4 + (int)size.X + 2, this.Y + 8, 4, 32), this.textColor);
			}
			Utility.drawTextWithShadow(spriteBatch, toDraw, this._font, new Vector2((float)(this.X + Game1.tileSize / 4), (float)(this.Y + ((this._textBoxTexture != null) ? (Game1.tileSize / 4 - Game1.pixelZoom) : (Game1.pixelZoom * 2)))), this.textColor, 1f, -1f, -1, -1, 1f, 3);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0014B5A8 File Offset: 0x001497A8
		public void RecieveTextInput(char inputChar)
		{
			if (this.Selected && (!this.numbersOnly || char.IsDigit(inputChar)) && (this.textLimit == -1 || this.Text.Length < this.textLimit))
			{
				if (Game1.gameMode != 3)
				{
					if (inputChar <= '*')
					{
						if (inputChar == '"')
						{
							return;
						}
						if (inputChar == '$')
						{
							Game1.playSound("money");
							goto IL_B3;
						}
						if (inputChar == '*')
						{
							Game1.playSound("hammer");
							goto IL_B3;
						}
					}
					else
					{
						if (inputChar == '+')
						{
							Game1.playSound("slimeHit");
							goto IL_B3;
						}
						if (inputChar == '<')
						{
							Game1.playSound("crystal");
							goto IL_B3;
						}
						if (inputChar == '=')
						{
							Game1.playSound("coin");
							goto IL_B3;
						}
					}
					Game1.playSound("cowboy_monsterhit");
				}
				IL_B3:
				this.Text += inputChar.ToString();
			}
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0014B680 File Offset: 0x00149880
		public void RecieveTextInput(string text)
		{
			int dummy = -1;
			if (this.Selected && (!this.numbersOnly || int.TryParse(text, out dummy)) && (this.textLimit == -1 || this.Text.Length < this.textLimit))
			{
				this.Text += text;
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0014B6D8 File Offset: 0x001498D8
		public void RecieveCommandInput(char command)
		{
			if (this.Selected)
			{
				if (command != '\b')
				{
					if (command != '\t')
					{
						if (command != '\r')
						{
							return;
						}
						if (this.OnEnterPressed != null)
						{
							this.OnEnterPressed(this);
							return;
						}
					}
					else if (this.OnTabPressed != null)
					{
						this.OnTabPressed(this);
					}
				}
				else if (this.Text.Length > 0)
				{
					this.Text = this.Text.Substring(0, this.Text.Length - 1);
					if (Game1.gameMode != 3)
					{
						Game1.playSound("tinyWhip");
						return;
					}
				}
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00002834 File Offset: 0x00000A34
		public void RecieveSpecialInput(Keys key)
		{
		}

		// Token: 0x14000006 RID: 6
		// Token: 0x0600100F RID: 4111 RVA: 0x0014B768 File Offset: 0x00149968
		// Token: 0x06001010 RID: 4112 RVA: 0x0014B7A0 File Offset: 0x001499A0
		[method: CompilerGenerated]
		[CompilerGenerated]
		public event TextBoxEvent OnEnterPressed;

		// Token: 0x14000007 RID: 7
		// Token: 0x06001011 RID: 4113 RVA: 0x0014B7D8 File Offset: 0x001499D8
		// Token: 0x06001012 RID: 4114 RVA: 0x0014B810 File Offset: 0x00149A10
		[method: CompilerGenerated]
		[CompilerGenerated]
		public event TextBoxEvent OnTabPressed;

		// Token: 0x170000DF RID: 223
		public bool Selected
		{
			// Token: 0x06001013 RID: 4115 RVA: 0x0014B845 File Offset: 0x00149A45
			get;
			// Token: 0x06001014 RID: 4116 RVA: 0x0014B84D File Offset: 0x00149A4D
			set;
		}

		// Token: 0x04001159 RID: 4441
		private Texture2D _textBoxTexture;

		// Token: 0x0400115A RID: 4442
		private Texture2D _caretTexture;

		// Token: 0x0400115B RID: 4443
		public SpriteFont _font;

		// Token: 0x0400115C RID: 4444
		public Color textColor;

		// Token: 0x04001163 RID: 4451
		public bool numbersOnly;

		// Token: 0x04001164 RID: 4452
		public int textLimit = -1;

		// Token: 0x04001165 RID: 4453
		public bool limitWidth = true;

		// Token: 0x04001167 RID: 4455
		private string _text = "";

		// Token: 0x04001168 RID: 4456
		private MouseState _previousMouse;
	}
}
