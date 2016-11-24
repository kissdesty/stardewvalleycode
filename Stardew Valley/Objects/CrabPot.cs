using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace StardewValley.Objects
{
	// Token: 0x02000094 RID: 148
	public class CrabPot : Object
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x000D9F75 File Offset: 0x000D8175
		public CrabPot()
		{
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000E0FB2 File Offset: 0x000DF1B2
		public CrabPot(Vector2 tileLocation, int stack = 1) : base(tileLocation, 710, "Crab Pot", true, false, false, false)
		{
			this.type = "interactive";
			this.tileIndexToShow = this.parentSheetIndex;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000E0FE0 File Offset: 0x000DF1E0
		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			Vector2 placementTile = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			if (who != null)
			{
				this.owner = who.uniqueMultiplayerID;
			}
			if (location.objects.ContainsKey(placementTile) || location.doesTileHaveProperty((int)placementTile.X, (int)placementTile.Y, "Water", "Back") == null)
			{
				return false;
			}
			this.tileLocation = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			location.objects.Add(this.tileLocation, this);
			Game1.playSound("waterSlosh");
			DelayedAction.playSoundAfterDelay("slosh", 150);
			if (location.doesTileHaveProperty((int)this.tileLocation.X - 1, (int)this.tileLocation.Y, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X - 1, (int)this.tileLocation.Y, "Passable", "Buildings") != null)
			{
				this.directionOffset = new Vector2((float)(Game1.tileSize / 2), 0f);
			}
			else if (location.doesTileHaveProperty((int)this.tileLocation.X + 1, (int)this.tileLocation.Y, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X + 1, (int)this.tileLocation.Y, "Passable", "Buildings") != null)
			{
				this.directionOffset = new Vector2((float)(-(float)Game1.tileSize / 2), 0f);
			}
			else if (location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y - 1, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y - 1, "Passable", "Buildings") != null)
			{
				this.directionOffset = new Vector2(0f, (float)(Game1.tileSize / 2));
			}
			else if (location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y + 1, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y + 1, "Passable", "Buildings") != null)
			{
				this.directionOffset = new Vector2(0f, (float)(-(float)Game1.tileSize * 2 / 3));
			}
			else if (location.doesTileHaveProperty((int)this.tileLocation.X + 1, (int)this.tileLocation.Y + 1, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X + 1, (int)this.tileLocation.Y + 1, "Passable", "Buildings") != null)
			{
				this.directionOffset = new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 4));
			}
			else if (location.doesTileHaveProperty((int)this.tileLocation.X - 1, (int)this.tileLocation.Y + 1, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X - 1, (int)this.tileLocation.Y + 1, "Passable", "Buildings") != null)
			{
				this.directionOffset = new Vector2((float)(Game1.tileSize / 4), (float)(-(float)Game1.tileSize / 4));
			}
			if ((location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y - 1, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y - 1, "Passable", "Buildings") != null) && (location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y + 1, "Water", "Back") == null || location.doesTileHaveProperty((int)this.tileLocation.X, (int)this.tileLocation.Y + 1, "Passable", "Buildings") != null))
			{
				this.directionOffset = new Vector2(0f, (float)(-(float)Game1.tileSize / 2));
			}
			return true;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool canBePlacedInWater()
		{
			return true;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000E1420 File Offset: 0x000DF620
		public override Item getOne()
		{
			return new Object(this.parentSheetIndex, 1, false, -1, 0);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000E1434 File Offset: 0x000DF634
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if (dropIn.category == -21 && this.bait == null && !who.professions.Contains(11))
			{
				if (!probe)
				{
					this.bait = dropIn;
					Game1.playSound("Ship");
					this.lidFlapping = true;
					this.lidFlapTimer = 60f;
				}
				else
				{
					this.heldObject = dropIn;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000E1494 File Offset: 0x000DF694
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (this.tileIndexToShow != 714)
			{
				if (this.bait == null)
				{
					if (justCheckingForActivity)
					{
						return true;
					}
					if (Game1.player.addItemToInventoryBool(this.getOne(), false))
					{
						Game1.playSound("coin");
						Game1.currentLocation.objects.Remove(this.tileLocation);
						return true;
					}
					Game1.showRedMessage("Inventory Full.");
				}
				return false;
			}
			if (justCheckingForActivity)
			{
				return true;
			}
			if (who.IsMainPlayer && !who.addItemToInventoryBool(this.heldObject, false))
			{
				Game1.addHUDMessage(new HUDMessage("Inventory Full", Color.Red, 3500f));
				return false;
			}
			Dictionary<int, string> data = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
			if (data.ContainsKey(this.heldObject.parentSheetIndex))
			{
				string[] rawData = data[this.heldObject.parentSheetIndex].Split(new char[]
				{
					'/'
				});
				int minFishSize = (rawData.Length > 5) ? Convert.ToInt32(rawData[5]) : 1;
				int maxFishSize = (rawData.Length > 5) ? Convert.ToInt32(rawData[6]) : 10;
				who.caughtFish(this.heldObject.parentSheetIndex, Game1.random.Next(minFishSize, maxFishSize + 1));
			}
			this.readyForHarvest = false;
			this.heldObject = null;
			this.tileIndexToShow = 710;
			this.lidFlapping = true;
			this.lidFlapTimer = 60f;
			this.bait = null;
			who.animateOnce(279 + who.FacingDirection);
			Game1.playSound("fishingRodBend");
			DelayedAction.playSoundAfterDelay("coin", 500);
			who.gainExperience(1, 5);
			this.shake = Vector2.Zero;
			this.shakeTimer = 0f;
			return true;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000E163C File Offset: 0x000DF83C
		public override void DayUpdate(GameLocation location)
		{
			bool isLuremaster = Game1.getFarmer(this.owner) != null && Game1.getFarmer(this.owner).professions.Contains(11);
			bool isMariner = Game1.getFarmer(this.owner) != null && Game1.getFarmer(this.owner).professions.Contains(10);
			if (this.owner == 0L && Game1.player.professions.Contains(11))
			{
				isMariner = true;
			}
			if ((this.bait != null | isLuremaster) && this.heldObject == null)
			{
				this.tileIndexToShow = 714;
				this.readyForHarvest = true;
				Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)((int)this.tileLocation.X * 1000) + (uint)((int)this.tileLocation.Y)));
				Dictionary<int, string> fishData = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
				List<int> marinerList = new List<int>();
				double chanceForJunk = isMariner ? 0.0 : 0.2;
				if (r.NextDouble() > chanceForJunk)
				{
					foreach (KeyValuePair<int, string> v in fishData)
					{
						if (v.Value.Contains("trap"))
						{
							string[] rawSplit = v.Value.Split(new char[]
							{
								'/'
							});
							if ((!rawSplit[4].Equals("ocean") || location is Beach) && (!rawSplit[4].Equals("freshwater") || !(location is Beach)))
							{
								if (isMariner)
								{
									marinerList.Add(v.Key);
								}
								else
								{
									double chanceForCatch = Convert.ToDouble(rawSplit[2]);
									if (r.NextDouble() < chanceForCatch)
									{
										this.heldObject = new Object(v.Key, 1, false, -1, 0);
										break;
									}
								}
							}
						}
					}
				}
				if (this.heldObject == null)
				{
					if (isMariner)
					{
						this.heldObject = new Object(marinerList[r.Next(marinerList.Count)], 1, false, -1, 0);
						return;
					}
					this.heldObject = new Object(r.Next(168, 173), 1, false, -1, 0);
				}
			}
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000E1888 File Offset: 0x000DFA88
		public override void updateWhenCurrentLocation(GameTime time)
		{
			if (this.lidFlapping)
			{
				this.lidFlapTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.lidFlapTimer <= 0f)
				{
					this.tileIndexToShow += (this.lidClosing ? -1 : 1);
					if (this.tileIndexToShow == 713 && !this.lidClosing)
					{
						this.lidClosing = true;
						this.tileIndexToShow--;
					}
					else if (this.tileIndexToShow == 709 && this.lidClosing)
					{
						this.lidClosing = false;
						this.tileIndexToShow++;
						this.lidFlapping = false;
						if (this.bait != null)
						{
							this.tileIndexToShow = 713;
						}
					}
					this.lidFlapTimer = 60f;
				}
			}
			if (this.readyForHarvest && this.heldObject != null)
			{
				this.shakeTimer -= (float)time.ElapsedGameTime.Milliseconds;
				if (this.shakeTimer < 0f)
				{
					this.shakeTimer = (float)Game1.random.Next(2800, 3200);
				}
			}
			if (this.shakeTimer > 2000f)
			{
				this.shake.X = (float)Game1.random.Next(-1, 2);
				return;
			}
			this.shake.X = 0f;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000E19EC File Offset: 0x000DFBEC
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			this.yBob = (float)(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 500.0 + (double)(x * Game1.tileSize)) * 8.0 + 8.0);
			if (this.yBob <= 0.001f)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f, 8, 0, this.directionOffset + new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 16), (float)(y * Game1.tileSize + Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.001f, 0.01f, Color.White, 0.75f, 0.003f, 0f, 0f, false));
			}
			spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.directionOffset + new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize + (int)this.yBob))) + this.shake, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.tileIndexToShow, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + x % 4) / 10000f);
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.directionOffset + new Vector2((float)(x * Game1.tileSize + 4), (float)(y * Game1.tileSize + Game1.tileSize * 3 / 4))) + this.shake, new Rectangle?(new Rectangle(Game1.currentLocation.waterAnimationIndex * 64, 2064 + Game1.tileSize * 3 / 4 + (((x + y) % 2 == 0) ? (Game1.currentLocation.waterTileFlip ? 128 : 0) : (Game1.currentLocation.waterTileFlip ? 0 : 128)), 56, Game1.tileSize / 4 + (int)this.yBob)), Game1.currentLocation.waterColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)(y * Game1.tileSize + x % 4) / 9999f);
			if (this.readyForHarvest && this.heldObject != null)
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.directionOffset + new Vector2((float)(x * Game1.tileSize - 8), (float)(y * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + yOffset)), new Rectangle?(new Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f + 1E-06f + this.tileLocation.X / 10000f);
				spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.directionOffset + new Vector2((float)(x * Game1.tileSize + Game1.tileSize / 2), (float)(y * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + yOffset)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.heldObject.parentSheetIndex, 16, 16)), Color.White * 0.75f, 0f, new Vector2(8f, 8f), (float)Game1.pixelZoom, SpriteEffects.None, (float)((y + 1) * Game1.tileSize) / 10000f + 1E-05f + this.tileLocation.X / 10000f);
			}
		}

		// Token: 0x04000B19 RID: 2841
		public const int lidFlapTimerInterval = 60;

		// Token: 0x04000B1A RID: 2842
		private float yBob;

		// Token: 0x04000B1B RID: 2843
		public Vector2 directionOffset;

		// Token: 0x04000B1C RID: 2844
		public Object bait;

		// Token: 0x04000B1D RID: 2845
		public int tileIndexToShow;

		// Token: 0x04000B1E RID: 2846
		private bool lidFlapping;

		// Token: 0x04000B1F RID: 2847
		private bool lidClosing;

		// Token: 0x04000B20 RID: 2848
		private float lidFlapTimer;

		// Token: 0x04000B21 RID: 2849
		private new float shakeTimer;

		// Token: 0x04000B22 RID: 2850
		private Vector2 shake;
	}
}
