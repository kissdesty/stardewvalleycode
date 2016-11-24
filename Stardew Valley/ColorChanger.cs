using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x0200003F RID: 63
	internal class ColorChanger
	{
		// Token: 0x060002FD RID: 765 RVA: 0x0003C6FC File Offset: 0x0003A8FC
		private static bool IsSimilar(Color original, Color test, int redDelta, int blueDelta, int greenDelta)
		{
			return Math.Abs((int)(original.R - test.R)) < redDelta && Math.Abs((int)(original.G - test.G)) < greenDelta && Math.Abs((int)(original.B - test.B)) < blueDelta;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0003C754 File Offset: 0x0003A954
		public static Texture2D changeColor(Texture2D texture, int targetColorIndex, int redtint, int greentint, int bluetint, int range)
		{
			Color[] data = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(data);
			Color targetColor = data[targetColorIndex];
			for (int i = 0; i < data.Length; i++)
			{
				if (ColorChanger.IsSimilar(data[i], targetColor, range, range, range))
				{
					data[i] = new Color((int)data[i].R + redtint, (int)data[i].G + greentint, (int)data[i].B + bluetint);
				}
			}
			texture.SetData<Color>(data);
			return texture;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0003C7E8 File Offset: 0x0003A9E8
		public static Texture2D changeColor(Texture2D texture, int targetColorIndex, Color baseColor, int range)
		{
			Color[] data = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(data);
			Color targetColor = data[targetColorIndex];
			for (int i = 0; i < data.Length; i++)
			{
				if (ColorChanger.IsSimilar(data[i], targetColor, range, range, range))
				{
					data[i] = new Color((int)(baseColor.R + (data[i].R - targetColor.R)), (int)(baseColor.G + (data[i].G - targetColor.G)), (int)(baseColor.B + (data[i].B - targetColor.B)));
				}
			}
			texture.SetData<Color>(data);
			return texture;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0003C89F File Offset: 0x0003AA9F
		public static Texture2D swapColor(Texture2D texture, int targetColorIndex, int red, int green, int blue)
		{
			return ColorChanger.swapColor(texture, targetColorIndex, red, green, blue, 0, texture.Width * texture.Height);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0003C8BC File Offset: 0x0003AABC
		public static Texture2D swapColor(Texture2D texture, int targetColorIndex, int red, int green, int blue, int startPixel, int endPixel)
		{
			red = Math.Min(Math.Max(1, red), 255);
			green = Math.Min(Math.Max(1, green), 255);
			blue = Math.Min(Math.Max(1, blue), 255);
			Color[] data = new Color[texture.Width * texture.Height];
			texture.GetData<Color>(data);
			Color targetColor = data[targetColorIndex];
			for (int i = 0; i < data.Length; i++)
			{
				if (i >= startPixel && i < endPixel && data[i].R == targetColor.R && data[i].G == targetColor.G && data[i].B == targetColor.B)
				{
					data[i] = new Color(red, green, blue);
				}
			}
			texture.SetData<Color>(data);
			return texture;
		}
	}
}
