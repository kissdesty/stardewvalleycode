using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Tools
{
	// Token: 0x02000064 RID: 100
	public class Shears : Tool
	{
		// Token: 0x0600091E RID: 2334 RVA: 0x000C6B03 File Offset: 0x000C4D03
		public Shears() : base("Shears", -1, 7, 7, "Use this to collect wool from sheep", false, 0)
		{
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000C6B1C File Offset: 0x000C4D1C
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
			who.Halt();
			int g = who.FarmerSprite.currentFrame;
			who.FarmerSprite.animateOnce(283 + who.FacingDirection, 50f, 4);
			who.FarmerSprite.oldFrame = g;
			who.UsingTool = true;
			who.CanMove = false;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x000C6C94 File Offset: 0x000C4E94
		public static void playSnip(Farmer who)
		{
			Game1.playSound("scissors");
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000C6CA0 File Offset: 0x000C4EA0
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			who.Stamina -= 4f;
			Shears.playSnip(who);
			this.currentParentTileIndex = 7;
			this.indexOfMenuItemView = 7;
			if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
			{
				if (who.addItemToInventoryBool(new Object(Vector2.Zero, this.animal.currentProduce, null, false, true, false, false)
				{
					quality = this.animal.produceQuality
				}, false))
				{
					this.animal.currentProduce = -1;
					Game1.playSound("coin");
					this.animal.friendshipTowardFarmer = Math.Min(1000, this.animal.friendshipTowardFarmer + 5);
					if (this.animal.showDifferentTextureWhenReadyForHarvest)
					{
						this.animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + this.animal.type);
					}
					who.gainExperience(0, 5);
				}
			}
			else
			{
				string toSay = "";
				if (this.animal != null && !this.animal.toolUsedForHarvest.Equals(this.name))
				{
					toSay = this.animal.name + " doesn't produce wool.";
				}
				if (this.animal != null && this.animal.isBaby() && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					toSay = this.animal.name + " is too young to be sheared.";
				}
				if (this.animal != null && this.animal.age >= (int)this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
				{
					toSay = this.animal.name + " has no wool right now.";
				}
				if (toSay.Length > 0)
				{
					Game1.drawObjectDialogue(toSay);
				}
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

		// Token: 0x04000925 RID: 2341
		private FarmAnimal animal;
	}
}
