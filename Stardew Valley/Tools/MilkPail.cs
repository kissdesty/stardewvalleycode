using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Tools
{
	// Token: 0x0200005F RID: 95
	public class MilkPail : Tool
	{
		// Token: 0x0600090C RID: 2316 RVA: 0x000C4F20 File Offset: 0x000C3120
		public MilkPail() : base("Milk Pail", -1, 6, 6, "Gather milk from your animals.", false, 0)
		{
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000C4F38 File Offset: 0x000C3138
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			x = (int)who.GetToolLocation(false).X;
			y = (int)who.GetToolLocation(false).Y;
			Rectangle r = new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize);
			if (location is Farm)
			{
				using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator = (location as Farm).animals.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FarmAnimal a = enumerator.Current;
						if (a.GetBoundingBox().Intersects(r))
						{
							this.animal = a;
							break;
						}
					}
					goto IL_FE;
				}
			}
			if (location is AnimalHouse)
			{
				foreach (FarmAnimal a2 in (location as AnimalHouse).animals.Values)
				{
					if (a2.GetBoundingBox().Intersects(r))
					{
						this.animal = a2;
						break;
					}
				}
			}
			IL_FE:
			if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name) && who.couldInventoryAcceptThisObject(this.animal.currentProduce, 1, 0))
			{
				this.animal.doEmote(20, true);
				this.animal.friendshipTowardFarmer = Math.Min(1000, this.animal.friendshipTowardFarmer + 5);
				Game1.playSound("Milking");
				this.animal.pauseTimer = 1500;
			}
			else if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature)
			{
				if (!this.animal.toolUsedForHarvest.Equals(this.name))
				{
					if (this.animal.toolUsedForHarvest != null && !this.animal.toolUsedForHarvest.Equals("null"))
					{
						Game1.showRedMessage(this.animal.toolUsedForHarvest + " Required");
					}
				}
				else if (!who.couldInventoryAcceptThisObject(this.animal.currentProduce, 1, 0))
				{
					Game1.showRedMessage("Inventory Full");
				}
			}
			else
			{
				DelayedAction.playSoundAfterDelay("fishingRodBend", 300);
				DelayedAction.playSoundAfterDelay("fishingRodBend", 1200);
				string toSay = "";
				if (this.animal != null && !this.animal.toolUsedForHarvest.Equals(this.name))
				{
					toSay = this.animal.name + " doesn't produce milk.";
				}
				if (this.animal != null && this.animal.isBaby() && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					toSay = this.animal.name + " is too young to produce milk.";
				}
				if (this.animal != null && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					toSay = this.animal.name + " has no milk right now.";
				}
				if (toSay.Length > 0)
				{
					DelayedAction.showDialogueAfterDelay(toSay, 1000);
				}
			}
			who.Halt();
			int g = who.FarmerSprite.currentFrame;
			who.FarmerSprite.animateOnce(287 + who.FacingDirection, 50f, 4);
			who.FarmerSprite.oldFrame = g;
			who.UsingTool = true;
			who.CanMove = false;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000C5328 File Offset: 0x000C3528
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			who.Stamina -= 4f;
			this.currentParentTileIndex = 6;
			this.indexOfMenuItemView = 6;
			if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name) && who.addItemToInventoryBool(new Object(Vector2.Zero, this.animal.currentProduce, null, false, true, false, false)
			{
				quality = this.animal.produceQuality
			}, false))
			{
				Game1.playSound("coin");
				this.animal.currentProduce = -1;
				if (this.animal.showDifferentTextureWhenReadyForHarvest)
				{
					this.animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + this.animal.type);
				}
				who.gainExperience(0, 5);
			}
			this.animal = null;
			if (Game1.activeClickableMenu == null)
			{
				who.CanMove = true;
			}
			else
			{
				who.Halt();
			}
			who.usingTool = false;
			who.canReleaseTool = true;
		}

		// Token: 0x0400091D RID: 2333
		private FarmAnimal animal;
	}
}
