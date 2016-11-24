using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Objects
{
	// Token: 0x02000090 RID: 144
	public class SpecialItem : Item
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x000DD730 File Offset: 0x000DB930
		public SpecialItem(int category, int which, string name = "")
		{
			this.category = category;
			this.which = which;
			this.Name = name;
			if (name.Length < 1)
			{
				switch (which)
				{
				case 2:
					this.Name = "Club Card";
					return;
				case 3:
				case 5:
					break;
				case 4:
					this.Name = "Skull Key";
					return;
				case 6:
					this.Name = Game1.content.LoadString("Strings\\Objects:DarkTalisman", new object[0]);
					return;
				case 7:
					this.Name = Game1.content.LoadString("Strings\\Objects:MagicInk", new object[0]);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000DD7D1 File Offset: 0x000DB9D1
		public SpecialItem()
		{
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x000DD7DC File Offset: 0x000DB9DC
		public void actionWhenReceived(Farmer who)
		{
			switch (this.which)
			{
			case 4:
				who.hasSkullKey = true;
				who.addQuest(19);
				return;
			case 5:
				break;
			case 6:
				who.hasDarkTalisman = true;
				return;
			case 7:
				who.hasMagicInk = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000DD828 File Offset: 0x000DBA28
		public TemporaryAnimatedSprite getTemporarySpriteForHoldingUp(Vector2 position)
		{
			if (this.category == 1)
			{
				return new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(129 + 16 * this.which, 320, 16, 16), position, false, 0f, Color.White)
				{
					layerDepth = 1f
				};
			}
			if (this.which == 99)
			{
				return new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle((Game1.player.maxItems == 36) ? 268 : 257, 1436, (Game1.player.maxItems == 36) ? 11 : 9, 13), position + new Vector2((float)(Game1.tileSize / 4), 0f), false, 0f, Color.White)
				{
					scale = (float)Game1.pixelZoom,
					layerDepth = 1f
				};
			}
			return null;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000DD908 File Offset: 0x000DBB08
		public override string checkForSpecialItemHoldUpMeessage()
		{
			if (this.category == 1)
			{
				switch (this.which)
				{
				case 2:
					return "Under a piece of wood, you found a " + this.Name + "! You're not sure what it's for, but it seems important. It's been added to your wallet.";
				case 4:
					return "You've found the " + this.Name + "! You're not sure what it's for, but it seems important. It's been added to your wallet.";
				case 6:
					return Game1.content.LoadString("Strings\\Objects:DarkTalismanDescription", new object[]
					{
						this.Name
					});
				case 7:
					return Game1.content.LoadString("Strings\\Objects:MagicInkDescription", new object[]
					{
						this.Name
					});
				}
			}
			if (this.which == 99)
			{
				return string.Concat(new object[]
				{
					"You got the ",
					this.name,
					"! Your inventory space has increased to ",
					Game1.player.maxItems,
					"."
				});
			}
			return base.checkForSpecialItemHoldUpMeessage();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00002834 File Offset: 0x00000A34
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0000846E File Offset: 0x0000666E
		public override int maximumStackSize()
		{
			return 1;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0000846E File Offset: 0x0000666E
		public override int getStack()
		{
			return 1;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00035C5D File Offset: 0x00033E5D
		public override int addToStack(int amount)
		{
			return -1;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00035CBD File Offset: 0x00033EBD
		public override string getDescription()
		{
			return null;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPlaceable()
		{
			return false;
		}

		// Token: 0x170000C4 RID: 196
		public override string Name
		{
			// Token: 0x06000A8F RID: 2703 RVA: 0x000DDA03 File Offset: 0x000DBC03
			get
			{
				return this.name;
			}
			// Token: 0x06000A90 RID: 2704 RVA: 0x000DDA0B File Offset: 0x000DBC0B
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		public override int Stack
		{
			// Token: 0x06000A91 RID: 2705 RVA: 0x0000846E File Offset: 0x0000666E
			get
			{
				return 1;
			}
			// Token: 0x06000A92 RID: 2706 RVA: 0x00002834 File Offset: 0x00000A34
			set
			{
			}
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000A8788 File Offset: 0x000A6988
		public override Item getOne()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000AE6 RID: 2790
		public const int permanentChangeItem = 1;

		// Token: 0x04000AE7 RID: 2791
		public const int skullKey = 4;

		// Token: 0x04000AE8 RID: 2792
		public const int clubCard = 2;

		// Token: 0x04000AE9 RID: 2793
		public const int backpack = 99;

		// Token: 0x04000AEA RID: 2794
		public const int darkTalisman = 6;

		// Token: 0x04000AEB RID: 2795
		public const int magicInk = 7;

		// Token: 0x04000AEC RID: 2796
		public string name;

		// Token: 0x04000AED RID: 2797
		public int which;

		// Token: 0x04000AEE RID: 2798
		public new int category;
	}
}
