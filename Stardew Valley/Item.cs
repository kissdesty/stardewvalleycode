using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using StardewValley.Tools;

namespace StardewValley
{
	// Token: 0x0200002F RID: 47
	[XmlInclude(typeof(Object)), XmlInclude(typeof(Tool))]
	public abstract class Item : IComparable
	{
		// Token: 0x0600026E RID: 622
		public abstract void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber);

		// Token: 0x0600026F RID: 623 RVA: 0x00035C37 File Offset: 0x00033E37
		public void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth)
		{
			this.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, true);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00035C47 File Offset: 0x00033E47
		public void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize)
		{
			this.drawInMenu(spriteBatch, location, scaleSize, 1f, 0.9f, true);
		}

		// Token: 0x06000271 RID: 625
		public abstract int maximumStackSize();

		// Token: 0x06000272 RID: 626
		public abstract int getStack();

		// Token: 0x06000273 RID: 627
		public abstract int addToStack(int amount);

		// Token: 0x06000274 RID: 628
		public abstract string getDescription();

		// Token: 0x06000275 RID: 629
		public abstract bool isPlaceable();

		// Token: 0x06000276 RID: 630 RVA: 0x00035C5D File Offset: 0x00033E5D
		public virtual int salePrice()
		{
			return -1;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00035C60 File Offset: 0x00033E60
		public virtual bool canBeTrashed()
		{
			return !(this is Tool) || (this is MeleeWeapon && this.Name != null && !this.Name.Equals("Scythe")) || this is FishingRod;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool canBePlacedInWater()
		{
			return false;
		}

		// Token: 0x17000025 RID: 37
		public virtual int parentSheetIndex
		{
			// Token: 0x06000279 RID: 633 RVA: 0x00035C97 File Offset: 0x00033E97
			get
			{
				if (!(this is Object))
				{
					return -1;
				}
				return (this as Object).parentSheetIndex;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool actionWhenPurchased()
		{
			return false;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool canBeDropped()
		{
			return true;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void actionWhenBeingHeld(Farmer who)
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void actionWhenStopBeingHeld(Farmer who)
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00035CAE File Offset: 0x00033EAE
		public int getRemainingStackSpace()
		{
			return this.maximumStackSize() - this.Stack;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00035CBD File Offset: 0x00033EBD
		public virtual string getHoverBoxText(Item hoveredItem)
		{
			return null;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool canBeGivenAsGift()
		{
			return false;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void drawAttachments(SpriteBatch b, int x, int y)
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool canBePlacedHere(GameLocation l, Vector2 tile)
		{
			return false;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual int attachmentSlots()
		{
			return 0;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00035CC0 File Offset: 0x00033EC0
		public virtual string getCategoryName()
		{
			if (this is Boots)
			{
				return "Footwear";
			}
			return "";
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00035CD5 File Offset: 0x00033ED5
		public virtual Color getCategoryColor()
		{
			return Color.Black;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00035CDC File Offset: 0x00033EDC
		public bool canStackWith(Item other)
		{
			return other != null && ((other is Object && this is Object) || (other is ColoredObject && this is ColoredObject)) && (!(this is ColoredObject) || !(other is ColoredObject) || (this as ColoredObject).color.Equals((other as ColoredObject).color)) && (this.maximumStackSize() > 1 && other.maximumStackSize() > 1 && (this as Object).ParentSheetIndex == (other as Object).ParentSheetIndex && (this as Object).bigCraftable == (other as Object).bigCraftable && (this as Object).quality == (other as Object).quality) && this.Name.Equals(other.Name);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00035CBD File Offset: 0x00033EBD
		public virtual string checkForSpecialItemHoldUpMeessage()
		{
			return null;
		}

		// Token: 0x17000026 RID: 38
		public abstract string Name
		{
			// Token: 0x06000288 RID: 648
			get;
			// Token: 0x06000289 RID: 649
			set;
		}

		// Token: 0x17000027 RID: 39
		public abstract int Stack
		{
			// Token: 0x0600028A RID: 650
			get;
			// Token: 0x0600028B RID: 651
			set;
		}

		// Token: 0x0600028C RID: 652
		public abstract Item getOne();

		// Token: 0x0600028D RID: 653 RVA: 0x00035DB4 File Offset: 0x00033FB4
		public int CompareTo(object obj)
		{
			if (!(obj is Item))
			{
				return 0;
			}
			if ((obj as Item).category != this.category)
			{
				return (obj as Item).category - this.category;
			}
			if (this is Object && obj is Object)
			{
				return string.Compare((this as Object).type + this.Name, (obj as Object).type + (obj as Item).Name);
			}
			return string.Compare(this.Name, (obj as Item).Name);
		}

		// Token: 0x040002A7 RID: 679
		public int specialVariable;

		// Token: 0x040002A8 RID: 680
		public int category;

		// Token: 0x040002A9 RID: 681
		public bool specialItem;

		// Token: 0x040002AA RID: 682
		public bool hasBeenInInventory;
	}
}
