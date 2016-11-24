using System;
using Microsoft.Xna.Framework;

namespace StardewValley.Tools
{
	// Token: 0x02000060 RID: 96
	public class Pickaxe : Tool
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x000C5474 File Offset: 0x000C3674
		public Pickaxe() : base("Pickaxe", 0, 105, 131, "Used to break stones.", false, 0)
		{
			this.upgradeLevel = 0;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x000C5498 File Offset: 0x000C3698
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			base.Update(who.facingDirection, 0, who);
			if (who.IsMainPlayer)
			{
				Game1.releaseUseToolButton();
				return;
			}
			switch (who.FacingDirection)
			{
			case 0:
				who.FarmerSprite.setCurrentFrame(176);
				who.CurrentTool.Update(0, 0);
				return;
			case 1:
				who.FarmerSprite.setCurrentFrame(168);
				who.CurrentTool.Update(1, 0);
				return;
			case 2:
				who.FarmerSprite.setCurrentFrame(160);
				who.CurrentTool.Update(2, 0);
				return;
			case 3:
				who.FarmerSprite.setCurrentFrame(184);
				who.CurrentTool.Update(3, 0);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x000C5564 File Offset: 0x000C3764
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			base.DoFunction(location, x, y, power, who);
			power = who.toolPower;
			who.Stamina -= (float)(2 * (power + 1)) - (float)who.MiningLevel * 0.1f;
			Utility.clampToTile(new Vector2((float)x, (float)y));
			int tileX = x / Game1.tileSize;
			int tileY = y / Game1.tileSize;
			Vector2 tile = new Vector2((float)tileX, (float)tileY);
			if (location.performToolAction(this, tileX, tileY))
			{
				return;
			}
			Object o = null;
			location.Objects.TryGetValue(tile, out o);
			if (o == null)
			{
				if (who.FacingDirection == 0 || who.FacingDirection == 2)
				{
					tileX = (x - 8) / Game1.tileSize;
					location.Objects.TryGetValue(new Vector2((float)tileX, (float)tileY), out o);
					if (o == null)
					{
						tileX = (x + 8) / Game1.tileSize;
						location.Objects.TryGetValue(new Vector2((float)tileX, (float)tileY), out o);
					}
				}
				else
				{
					tileY = (y + 8) / Game1.tileSize;
					location.Objects.TryGetValue(new Vector2((float)tileX, (float)tileY), out o);
					if (o == null)
					{
						tileY = (y - 8) / Game1.tileSize;
						location.Objects.TryGetValue(new Vector2((float)tileX, (float)tileY), out o);
					}
				}
				x = tileX * Game1.tileSize;
				y = tileY * Game1.tileSize;
				if (location.terrainFeatures.ContainsKey(tile) && location.terrainFeatures[tile].performToolAction(this, 0, tile, null))
				{
					location.terrainFeatures.Remove(tile);
				}
			}
			tile = new Vector2((float)tileX, (float)tileY);
			if (o != null)
			{
				if (o.Name.Equals("Stone"))
				{
					Game1.playSound("hammer");
					if (o.minutesUntilReady > 0)
					{
						int damage = Math.Max(1, this.upgradeLevel + 1);
						o.minutesUntilReady -= damage;
						o.shakeTimer = 200;
						if (o.minutesUntilReady > 0)
						{
							Game1.createRadialDebris(Game1.currentLocation, 14, tileX, tileY, Game1.random.Next(2, 5), false, -1, false, -1);
							return;
						}
					}
					if (o.ParentSheetIndex < 200 && !Game1.objectInformation.ContainsKey(o.ParentSheetIndex + 1))
					{
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(o.ParentSheetIndex + 1, 300f, 1, 2, new Vector2((float)(x - x % Game1.tileSize), (float)(y - y % Game1.tileSize)), true, o.flipped)
						{
							alphaFade = 0.01f
						});
					}
					else
					{
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2((float)(tileX * Game1.tileSize), (float)(tileY * Game1.tileSize)), Color.Gray, 10, false, 80f, 0, -1, -1f, -1, 0));
					}
					Game1.createRadialDebris(location, 14, tileX, tileY, Game1.random.Next(2, 5), false, -1, false, -1);
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2((float)(tileX * Game1.tileSize), (float)(tileY * Game1.tileSize)), Color.White, 10, false, 80f, 0, -1, -1f, -1, 0)
					{
						motion = new Vector2(0f, -0.6f),
						acceleration = new Vector2(0f, 0.002f),
						alphaFade = 0.015f
					});
					if (!location.Name.Equals("UndergroundMine"))
					{
						if (o.parentSheetIndex == 343 || o.parentSheetIndex == 450)
						{
							Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)(tileX * 2000) + (uint)tileY));
							if (r.NextDouble() < 0.035 && Game1.stats.DaysPlayed > 1u)
							{
								Game1.createObjectDebris(535 + ((Game1.stats.DaysPlayed > 60u && r.NextDouble() < 0.2) ? 1 : ((Game1.stats.DaysPlayed > 120u && r.NextDouble() < 0.2) ? 2 : 0)), tileX, tileY, base.getLastFarmerToUse().uniqueMultiplayerID);
							}
							if (r.NextDouble() < 0.035 * (double)(who.professions.Contains(21) ? 2 : 1) && Game1.stats.DaysPlayed > 1u)
							{
								Game1.createObjectDebris(382, tileX, tileY, base.getLastFarmerToUse().uniqueMultiplayerID);
							}
							if (r.NextDouble() < 0.01 && Game1.stats.DaysPlayed > 1u)
							{
								Game1.createObjectDebris(390, tileX, tileY, base.getLastFarmerToUse().uniqueMultiplayerID);
							}
						}
						location.breakStone(o.parentSheetIndex, tileX, tileY, who, new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + (uint)(tileX * 4000) + (uint)tileY)));
					}
					else
					{
						Game1.mine.checkStoneForItems(o.ParentSheetIndex, tileX, tileY, who);
					}
					if (o.minutesUntilReady <= 0)
					{
						location.Objects.Remove(new Vector2((float)tileX, (float)tileY));
						Game1.playSound("stoneCrack");
						Stats expr_4F8 = Game1.stats;
						uint num = expr_4F8.RocksCrushed;
						expr_4F8.RocksCrushed = num + 1u;
					}
					return;
				}
				if (o.Name.Contains("Boulder"))
				{
					Game1.playSound("hammer");
					if (this.UpgradeLevel < 2)
					{
						Game1.drawObjectDialogue(Game1.parseText("Your pickaxe isn't strong enough to break this yet."));
						return;
					}
					if (tileX == this.boulderTileX && tileY == this.boulderTileY)
					{
						this.hitsToBoulder += power + 1;
						o.shakeTimer = 190;
					}
					else
					{
						this.hitsToBoulder = 0;
						this.boulderTileX = tileX;
						this.boulderTileY = tileY;
					}
					if (this.hitsToBoulder >= 4)
					{
						location.removeObject(tile, false);
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * tile.X - (float)(Game1.tileSize / 2), (float)Game1.tileSize * (tile.Y - 1f)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 0
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * tile.X + (float)(Game1.tileSize / 2), (float)Game1.tileSize * (tile.Y - 1f)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 200
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * tile.X, (float)Game1.tileSize * (tile.Y - 1f) - (float)(Game1.tileSize / 2)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 400
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float)Game1.tileSize * tile.X, (float)Game1.tileSize * tile.Y - (float)(Game1.tileSize / 2)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
						{
							delayBeforeAnimationStart = 600
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)Game1.tileSize * tile.X, (float)Game1.tileSize * tile.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0));
						location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)Game1.tileSize * tile.X + (float)(Game1.tileSize / 2), (float)Game1.tileSize * tile.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0)
						{
							delayBeforeAnimationStart = 250
						});
						location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float)Game1.tileSize * tile.X - (float)(Game1.tileSize / 2), (float)Game1.tileSize * tile.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0)
						{
							delayBeforeAnimationStart = 500
						});
						Game1.playSound("boulderBreak");
						Stats expr_8B5 = Game1.stats;
						uint num = expr_8B5.BouldersCracked;
						expr_8B5.BouldersCracked = num + 1u;
						return;
					}
				}
				else if (o.performToolAction(this))
				{
					o.performRemoveAction(tile, location);
					if (o.type.Equals("Crafting") && o.fragility != 2)
					{
						Game1.currentLocation.debris.Add(new Debris(o.bigCraftable ? (-o.ParentSheetIndex) : o.ParentSheetIndex, who.GetToolLocation(false), new Vector2((float)who.GetBoundingBox().Center.X, (float)who.GetBoundingBox().Center.Y)));
					}
					Game1.currentLocation.Objects.Remove(tile);
					return;
				}
			}
			else
			{
				Game1.playSound("woodyHit");
				if (location.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null)
				{
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2((float)(tileX * Game1.tileSize), (float)(tileY * Game1.tileSize)), Color.White, 8, false, 80f, 0, -1, -1f, -1, 0)
					{
						alphaFade = 0.015f
					});
				}
			}
		}

		// Token: 0x0400091E RID: 2334
		public const int hitMargin = 8;

		// Token: 0x0400091F RID: 2335
		public const int BoulderStrength = 4;

		// Token: 0x04000920 RID: 2336
		private int boulderTileX;

		// Token: 0x04000921 RID: 2337
		private int boulderTileY;

		// Token: 0x04000922 RID: 2338
		private int hitsToBoulder;
	}
}
