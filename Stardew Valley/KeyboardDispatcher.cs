using System;
using System.Threading;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
	// Token: 0x0200001A RID: 26
	public class KeyboardDispatcher
	{
		// Token: 0x0600011E RID: 286 RVA: 0x0000C545 File Offset: 0x0000A745
		public KeyboardDispatcher(GameWindow window)
		{
			KeyboardInput.Initialize(window);
			KeyboardInput.CharEntered += new CharEnteredHandler(this.EventInput_CharEntered);
			KeyboardInput.KeyDown += new KeyEventHandler(this.EventInput_KeyDown);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000C580 File Offset: 0x0000A780
		private void Event_KeyDown(object sender, Keys key)
		{
			if (this._subscriber == null)
			{
				return;
			}
			if (key == Keys.Back)
			{
				this._subscriber.RecieveCommandInput('\b');
			}
			if (key == Keys.Enter)
			{
				this._subscriber.RecieveCommandInput('\r');
			}
			if (key == Keys.Tab)
			{
				this._subscriber.RecieveCommandInput('\t');
			}
			this._subscriber.RecieveSpecialInput(key);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000C5D6 File Offset: 0x0000A7D6
		private void EventInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (this._subscriber == null)
			{
				return;
			}
			this._subscriber.RecieveSpecialInput(e.KeyCode);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		private void EventInput_CharEntered(object sender, CharacterEventArgs e)
		{
			if (this._subscriber == null)
			{
				return;
			}
			if (!char.IsControl(e.Character))
			{
				this._subscriber.RecieveTextInput(e.Character);
				return;
			}
			if (e.Character == '\u0016')
			{
				Thread expr_31 = new Thread(new ThreadStart(this.PasteThread));
				expr_31.SetApartmentState(ApartmentState.STA);
				expr_31.Start();
				expr_31.Join();
				this._subscriber.RecieveTextInput(this._pasteResult);
				return;
			}
			this._subscriber.RecieveCommandInput(e.Character);
		}

		// Token: 0x17000018 RID: 24
		public IKeyboardSubscriber Subscriber
		{
			// Token: 0x06000122 RID: 290 RVA: 0x0000C679 File Offset: 0x0000A879
			get
			{
				return this._subscriber;
			}
			// Token: 0x06000123 RID: 291 RVA: 0x0000C681 File Offset: 0x0000A881
			set
			{
				if (this._subscriber != null)
				{
					this._subscriber.Selected = false;
				}
				this._subscriber = value;
				if (value != null)
				{
					value.Selected = true;
				}
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000C6A8 File Offset: 0x0000A8A8
		[STAThread]
		private void PasteThread()
		{
			if (Clipboard.ContainsText())
			{
				this._pasteResult = Clipboard.GetText();
				return;
			}
			this._pasteResult = "";
		}

		// Token: 0x0400013F RID: 319
		private IKeyboardSubscriber _subscriber;

		// Token: 0x04000140 RID: 320
		private string _pasteResult = "";
	}
}
