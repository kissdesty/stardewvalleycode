using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000D9 RID: 217
	public class BuffsDisplay : IClickableMenu
	{
		// Token: 0x06000D93 RID: 3475 RVA: 0x00113E0C File Offset: 0x0011200C
		public BuffsDisplay() : base(Game1.viewport.Width - 320 - BuffsDisplay.sideSpace - BuffsDisplay.width, Game1.tileSize / 8, BuffsDisplay.width, Game1.tileSize, false)
		{
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00113E6E File Offset: 0x0011206E
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = Game1.viewport.Width - 320 - BuffsDisplay.sideSpace - BuffsDisplay.width;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00113E94 File Offset: 0x00112094
		public override void performHoverAction(int x, int y)
		{
			this.hoverText = "";
			foreach (KeyValuePair<ClickableTextureComponent, Buff> c in this.buffs)
			{
				if (c.Key.containsPoint(x, y))
				{
					this.hoverText = c.Key.hoverText + Environment.NewLine + c.Value.getTimeLeft();
					c.Key.scale = Math.Min(c.Key.baseScale + 0.1f, c.Key.scale + 0.02f);
					break;
				}
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00113F5C File Offset: 0x0011215C
		public void arrangeTheseComponentsInThisRectangle(int rectangleX, int rectangleY, int rectangleWidthInComponentWidthUnits, int componentWidth, int componentHeight, int buffer, bool rightToLeft)
		{
			int x = 0;
			int y = 0;
			foreach (ClickableTextureComponent c in this.buffs.Keys)
			{
				if (rightToLeft)
				{
					c.bounds = new Rectangle(rectangleX + rectangleWidthInComponentWidthUnits * componentWidth - (x + 1) * (componentWidth + buffer), rectangleY + y * (componentHeight + buffer), componentWidth, componentHeight);
				}
				else
				{
					c.bounds = new Rectangle(rectangleX + x * (componentWidth + buffer), rectangleY + y * (componentHeight + buffer), componentWidth, componentHeight);
				}
				x++;
				if (x > rectangleWidthInComponentWidthUnits)
				{
					y++;
					x %= rectangleWidthInComponentWidthUnits;
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00114010 File Offset: 0x00112210
		public void syncIcons()
		{
			this.buffs.Clear();
			if (this.food != null)
			{
				foreach (ClickableTextureComponent c in this.food.getClickableComponents())
				{
					this.buffs.Add(c, this.food);
				}
			}
			if (this.drink != null)
			{
				foreach (ClickableTextureComponent c2 in this.drink.getClickableComponents())
				{
					this.buffs.Add(c2, this.drink);
				}
			}
			foreach (Buff b in this.otherBuffs)
			{
				foreach (ClickableTextureComponent c3 in b.getClickableComponents())
				{
					this.buffs.Add(c3, b);
				}
			}
			this.arrangeTheseComponentsInThisRectangle(Game1.viewport.Width - 320 - BuffsDisplay.sideSpace - BuffsDisplay.width, Game1.tileSize / 8, BuffsDisplay.width / Game1.tileSize, Game1.tileSize, Game1.tileSize, Game1.tileSize / 8, true);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x001141B0 File Offset: 0x001123B0
		public bool hasBuff(int which)
		{
			using (List<Buff>.Enumerator enumerator = this.otherBuffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.which == which)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0011420C File Offset: 0x0011240C
		public bool tryToAddFoodBuff(Buff b, int duration)
		{
			if (b.total > 0 && this.fullnessLeft <= 0)
			{
				if (this.food != null)
				{
					this.food.removeBuff();
				}
				this.food = b;
				this.food.addBuff();
				this.syncIcons();
				return true;
			}
			return false;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0011425C File Offset: 0x0011245C
		public bool tryToAddDrinkBuff(Buff b)
		{
			if (b.source.Contains("Beer") || b.source.Contains("Wine") || b.source.Contains("Mead"))
			{
				this.addOtherBuff(new Buff(17));
			}
			else if (b.source.Equals("Oil of Garlic"))
			{
				this.addOtherBuff(new Buff(23));
			}
			else if (b.source.Equals("Life Elixir"))
			{
				Game1.player.health = Game1.player.maxHealth;
			}
			else if (b.source.Equals("Muscle Remedy"))
			{
				Game1.player.exhausted = false;
			}
			if (b.total > 0 && this.quenchedLeft <= 0)
			{
				if (this.drink != null)
				{
					this.drink.removeBuff();
				}
				this.drink = b;
				this.drink.addBuff();
				this.syncIcons();
				return true;
			}
			return false;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00114358 File Offset: 0x00112558
		public bool addOtherBuff(Buff buff)
		{
			if (buff.which != -1)
			{
				foreach (KeyValuePair<ClickableTextureComponent, Buff> kvp in this.buffs)
				{
					if (buff.which == kvp.Value.which)
					{
						kvp.Value.millisecondsDuration = buff.millisecondsDuration;
						kvp.Key.scale = kvp.Key.baseScale + 0.2f;
						return false;
					}
				}
			}
			this.otherBuffs.Add(buff);
			buff.addBuff();
			this.syncIcons();
			return true;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00114410 File Offset: 0x00112610
		public new void update(GameTime time)
		{
			if (this.food != null && this.food.update(time))
			{
				this.food.removeBuff();
				this.food = null;
				this.syncIcons();
			}
			if (this.drink != null && this.drink.update(time))
			{
				this.drink.removeBuff();
				this.drink = null;
				this.syncIcons();
			}
			for (int i = this.otherBuffs.Count - 1; i >= 0; i--)
			{
				if (this.otherBuffs[i].update(time))
				{
					this.otherBuffs[i].removeBuff();
					this.otherBuffs.RemoveAt(i);
					this.syncIcons();
				}
			}
			foreach (ClickableTextureComponent c in this.buffs.Keys)
			{
				c.scale = Math.Max(c.baseScale, c.scale - 0.01f);
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0011452C File Offset: 0x0011272C
		public void clearAllBuffs()
		{
			this.otherBuffs.Clear();
			if (this.food != null)
			{
				this.food.removeBuff();
				this.food = null;
			}
			if (this.drink != null)
			{
				this.drink.removeBuff();
				this.drink = null;
			}
			this.buffs.Clear();
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00114584 File Offset: 0x00112784
		public override void draw(SpriteBatch b)
		{
			using (Dictionary<ClickableTextureComponent, Buff>.KeyCollection.Enumerator enumerator = this.buffs.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
			if (!this.hoverText.Equals("") && this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04000E44 RID: 3652
		public const int fullnessLength = 180000;

		// Token: 0x04000E45 RID: 3653
		public const int quenchedLength = 60000;

		// Token: 0x04000E46 RID: 3654
		public static int sideSpace = Game1.tileSize / 2;

		// Token: 0x04000E47 RID: 3655
		public new static int width = Game1.tileSize * 4 + Game1.tileSize / 2;

		// Token: 0x04000E48 RID: 3656
		private Dictionary<ClickableTextureComponent, Buff> buffs = new Dictionary<ClickableTextureComponent, Buff>();

		// Token: 0x04000E49 RID: 3657
		public Buff food;

		// Token: 0x04000E4A RID: 3658
		public Buff drink;

		// Token: 0x04000E4B RID: 3659
		public List<Buff> otherBuffs = new List<Buff>();

		// Token: 0x04000E4C RID: 3660
		public int fullnessLeft;

		// Token: 0x04000E4D RID: 3661
		public int quenchedLeft;

		// Token: 0x04000E4E RID: 3662
		public string hoverText = "";
	}
}
