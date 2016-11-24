using System;
using Microsoft.Xna.Framework;
using StardewValley.Quests;

namespace StardewValley.Tools
{
	// Token: 0x02000067 RID: 103
	public class Sword : Tool
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x000C7DA5 File Offset: 0x000C5FA5
		public Sword()
		{
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x000C7DBF File Offset: 0x000C5FBF
		public Sword(string name, int spriteIndex, string description) : base(name, 0, spriteIndex, spriteIndex, description, false, 0)
		{
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x000C7DD0 File Offset: 0x000C5FD0
		public void DoFunction(GameLocation location, int x, int y, int facingDirection, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			Vector2 tileLocation = Vector2.Zero;
			Vector2 tileLocation2 = Vector2.Zero;
			Rectangle areaOfEffect = Rectangle.Empty;
			Rectangle playerBoundingBox = who.GetBoundingBox();
			switch (facingDirection)
			{
			case 0:
				areaOfEffect = new Rectangle(x - Game1.tileSize, playerBoundingBox.Y - Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
				tileLocation = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Left : areaOfEffect.Right) / Game1.tileSize), (float)(areaOfEffect.Top / Game1.tileSize));
				tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Top / Game1.tileSize));
				break;
			case 1:
				areaOfEffect = new Rectangle(playerBoundingBox.Right, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
				tileLocation = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Top : areaOfEffect.Bottom) / Game1.tileSize));
				tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
				break;
			case 2:
				areaOfEffect = new Rectangle(x - Game1.tileSize, playerBoundingBox.Bottom, Game1.tileSize * 2, Game1.tileSize);
				tileLocation = new Vector2((float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Left : areaOfEffect.Right) / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
				tileLocation2 = new Vector2((float)(areaOfEffect.Center.X / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
				break;
			case 3:
				areaOfEffect = new Rectangle(playerBoundingBox.Left - Game1.tileSize, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
				tileLocation = new Vector2((float)(areaOfEffect.Left / Game1.tileSize), (float)(((Game1.random.NextDouble() < 0.5) ? areaOfEffect.Top : areaOfEffect.Bottom) / Game1.tileSize));
				tileLocation2 = new Vector2((float)(areaOfEffect.Left / Game1.tileSize), (float)(areaOfEffect.Center.Y / Game1.tileSize));
				break;
			}
			int minDamage = ((this.whichUpgrade == 2) ? 3 : ((this.whichUpgrade == 4) ? 6 : this.whichUpgrade)) + 1;
			int maxDamage = 4 * (((this.whichUpgrade == 2) ? 3 : ((this.whichUpgrade == 4) ? 5 : this.whichUpgrade)) + 1);
			bool dontdestroyObjects = location.damageMonster(areaOfEffect, minDamage, maxDamage, false, who);
			if (this.whichUpgrade == 4 && !dontdestroyObjects)
			{
				location.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float)Game1.random.Next(50, 120), 2, 1, new Vector2((float)(areaOfEffect.Center.X - Game1.tileSize / 2), (float)(areaOfEffect.Center.Y - Game1.tileSize / 2)) + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5));
			}
			string soundToPlay = "";
			if (!dontdestroyObjects)
			{
				if (location.objects.ContainsKey(tileLocation) && !location.Objects[tileLocation].Name.Contains("Stone") && !location.Objects[tileLocation].Name.Contains("Stick") && !location.Objects[tileLocation].Name.Contains("Stump") && !location.Objects[tileLocation].Name.Contains("Boulder") && !location.Objects[tileLocation].Name.Contains("Lumber") && !location.Objects[tileLocation].IsHoeDirt)
				{
					if (location.Objects[tileLocation].Name.Contains("Weed"))
					{
						if (who.Stamina <= 0f)
						{
							return;
						}
						Stats expr_4B7 = Game1.stats;
						uint weedsEliminated = expr_4B7.WeedsEliminated;
						expr_4B7.WeedsEliminated = weedsEliminated + 1u;
						if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest"))
						{
							((WeedingQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
						}
						this.checkWeedForTreasure(tileLocation, who);
						int category = location.Objects[tileLocation].Category;
						if (category == -2)
						{
							soundToPlay = "stoneCrack";
						}
						else
						{
							soundToPlay = "cut";
						}
						location.removeObject(tileLocation, true);
					}
					else
					{
						location.objects[tileLocation].performToolAction(this);
					}
				}
				if (location.objects.ContainsKey(tileLocation2) && !location.Objects[tileLocation2].Name.Contains("Stone") && !location.Objects[tileLocation2].Name.Contains("Stick") && !location.Objects[tileLocation2].Name.Contains("Stump") && !location.Objects[tileLocation2].Name.Contains("Boulder") && !location.Objects[tileLocation2].Name.Contains("Lumber") && !location.Objects[tileLocation2].IsHoeDirt)
				{
					if (location.Objects[tileLocation2].Name.Contains("Weed"))
					{
						if (who.Stamina <= 0f)
						{
							return;
						}
						Stats expr_661 = Game1.stats;
						uint weedsEliminated = expr_661.WeedsEliminated;
						expr_661.WeedsEliminated = weedsEliminated + 1u;
						if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest"))
						{
							((WeedingQuest)Game1.questOfTheDay).checkIfComplete(null, -1, -1, null, null);
						}
						this.checkWeedForTreasure(tileLocation2, who);
					}
					else
					{
						location.objects[tileLocation2].performToolAction(this);
					}
				}
			}
			foreach (Vector2 v in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(areaOfEffect))
			{
				if (location.terrainFeatures.ContainsKey(v) && location.terrainFeatures[v].performToolAction(this, 0, v, null))
				{
					location.terrainFeatures.Remove(v);
				}
			}
			if (!soundToPlay.Equals(""))
			{
				Game1.playSound(soundToPlay);
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000C8560 File Offset: 0x000C6760
		public void checkWeedForTreasure(Vector2 tileLocation, Farmer who)
		{
			Random r = new Random((int)(Game1.uniqueIDForThisGame + (ulong)Game1.stats.DaysPlayed + tileLocation.X * 13f + tileLocation.Y * 29f));
			if (r.NextDouble() < 0.07)
			{
				Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, r.Next(1, 3), null);
				return;
			}
			if (r.NextDouble() < 0.02 + (double)who.LuckLevel / 10.0)
			{
				Game1.createDebris((r.NextDouble() < 0.5) ? 4 : 8, (int)tileLocation.X, (int)tileLocation.Y, r.Next(1, 4), null);
				return;
			}
			if (r.NextDouble() < 0.006 + (double)who.LuckLevel / 20.0)
			{
				Game1.createObjectDebris(114, (int)tileLocation.X, (int)tileLocation.Y, -1, 0, 1f, null);
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000C8668 File Offset: 0x000C6868
		public void upgrade(int which)
		{
			if (which > this.whichUpgrade)
			{
				this.whichUpgrade = which;
				switch (which)
				{
				case 1:
					this.name = "Hero's Sword";
					this.description = "A famous hero once owned this sword.";
					this.indexOfMenuItemView = 68;
					break;
				case 2:
					this.name = "Holy Sword";
					this.description = "A powerful relic infused with ancient energy.";
					this.indexOfMenuItemView = 70;
					break;
				case 3:
					this.name = "Dark Sword";
					this.description = "A powerful relic infused with evil energy.";
					this.indexOfMenuItemView = 69;
					break;
				case 4:
					this.name = "Galaxy Sword";
					this.description = "The ultimate cosmic weapon.";
					this.indexOfMenuItemView = 71;
					break;
				}
				this.upgradeLevel = which;
			}
			base.CurrentParentTileIndex = this.indexOfMenuItemView;
		}

		// Token: 0x04000933 RID: 2355
		public const double baseCritChance = 0.02;

		// Token: 0x04000934 RID: 2356
		public int whichUpgrade;
	}
}
