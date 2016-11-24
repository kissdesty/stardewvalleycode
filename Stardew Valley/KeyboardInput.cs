using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
	// Token: 0x02000018 RID: 24
	public static class KeyboardInput
	{
		// Token: 0x14000001 RID: 1
		// Token: 0x0600010C RID: 268 RVA: 0x0000C298 File Offset: 0x0000A498
		// Token: 0x0600010D RID: 269 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event CharEnteredHandler CharEntered;

		// Token: 0x14000002 RID: 2
		// Token: 0x0600010E RID: 270 RVA: 0x0000C300 File Offset: 0x0000A500
		// Token: 0x0600010F RID: 271 RVA: 0x0000C334 File Offset: 0x0000A534
		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event KeyEventHandler KeyDown;

		// Token: 0x14000003 RID: 3
		// Token: 0x06000110 RID: 272 RVA: 0x0000C368 File Offset: 0x0000A568
		// Token: 0x06000111 RID: 273 RVA: 0x0000C39C File Offset: 0x0000A59C
		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event KeyEventHandler KeyUp;

		// Token: 0x06000112 RID: 274
		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr ImmGetContext(IntPtr hWnd);

		// Token: 0x06000113 RID: 275
		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		// Token: 0x06000114 RID: 276
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000115 RID: 277
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x06000116 RID: 278 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		public static void Initialize(GameWindow window)
		{
			if (KeyboardInput.initialized)
			{
				throw new InvalidOperationException("TextInput.Initialize can only be called once!");
			}
			KeyboardInput.hookProcDelegate = new KeyboardInput.WndProc(KeyboardInput.HookProc);
			KeyboardInput.prevWndProc = (IntPtr)KeyboardInput.SetWindowLong(window.Handle, -4, (int)Marshal.GetFunctionPointerForDelegate(KeyboardInput.hookProcDelegate));
			KeyboardInput.hIMC = KeyboardInput.ImmGetContext(window.Handle);
			KeyboardInput.initialized = true;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000C43C File Offset: 0x0000A63C
		private static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr returnCode = KeyboardInput.CallWindowProc(KeyboardInput.prevWndProc, hWnd, msg, wParam, lParam);
			if (msg <= 135u)
			{
				if (msg != 81u)
				{
					if (msg == 135u)
					{
						returnCode = (IntPtr)(returnCode.ToInt32() | 4);
					}
				}
				else
				{
					KeyboardInput.ImmAssociateContext(hWnd, KeyboardInput.hIMC);
					returnCode = (IntPtr)1;
				}
			}
			else
			{
				switch (msg)
				{
				case 256u:
					if (KeyboardInput.KeyDown != null)
					{
						KeyboardInput.KeyDown(null, new KeyEventArgs((Keys)((int)wParam)));
					}
					break;
				case 257u:
					if (KeyboardInput.KeyUp != null)
					{
						KeyboardInput.KeyUp(null, new KeyEventArgs((Keys)((int)wParam)));
					}
					break;
				case 258u:
					if (KeyboardInput.CharEntered != null)
					{
						KeyboardInput.CharEntered(null, new CharacterEventArgs((char)((int)wParam), lParam.ToInt32()));
					}
					break;
				default:
					if (msg == 641u)
					{
						if (wParam.ToInt32() == 1)
						{
							KeyboardInput.ImmAssociateContext(hWnd, KeyboardInput.hIMC);
						}
					}
					break;
				}
			}
			return returnCode;
		}

		// Token: 0x04000132 RID: 306
		private static bool initialized;

		// Token: 0x04000133 RID: 307
		private static IntPtr prevWndProc;

		// Token: 0x04000134 RID: 308
		private static KeyboardInput.WndProc hookProcDelegate;

		// Token: 0x04000135 RID: 309
		private static IntPtr hIMC;

		// Token: 0x04000136 RID: 310
		private const int GWL_WNDPROC = -4;

		// Token: 0x04000137 RID: 311
		private const int WM_KEYDOWN = 256;

		// Token: 0x04000138 RID: 312
		private const int WM_KEYUP = 257;

		// Token: 0x04000139 RID: 313
		private const int WM_CHAR = 258;

		// Token: 0x0400013A RID: 314
		private const int WM_IME_SETCONTEXT = 641;

		// Token: 0x0400013B RID: 315
		private const int WM_INPUTLANGCHANGE = 81;

		// Token: 0x0400013C RID: 316
		private const int WM_GETDLGCODE = 135;

		// Token: 0x0400013D RID: 317
		private const int WM_IME_COMPOSITION = 271;

		// Token: 0x0400013E RID: 318
		private const int DLGC_WANTALLKEYS = 4;

		// Token: 0x02000165 RID: 357
		// Token: 0x06001358 RID: 4952
		private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
	}
}
