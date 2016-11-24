using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x02000136 RID: 310
	public class Beach : GameLocation
	{
		// Token: 0x060011BD RID: 4541 RVA: 0x00151B90 File Offset: 0x0014FD90
		public Beach()
		{
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00151B98 File Offset: 0x0014FD98
		public Beach(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0016BAE8 File Offset: 0x00169CE8
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			if (this.oldMariner != null)
			{
				this.oldMariner.update(time, this);
			}
			if (!Game1.eventUp && Game1.random.NextDouble() < 1E-06)
			{
				Vector2 position = new Vector2((float)(Game1.random.Next(15, 47) * Game1.tileSize), (float)(Game1.random.Next(29, 42) * Game1.tileSize));
				bool draw = true;
				for (float i = position.Y / (float)Game1.tileSize; i < (float)this.map.GetLayer("Back").LayerHeight; i += 1f)
				{
					if (base.doesTileHaveProperty((int)position.X / Game1.tileSize, (int)i, "Water", "Back") == null || base.doesTileHaveProperty((int)position.X / Game1.tileSize - 1, (int)i, "Water", "Back") == null || base.doesTileHaveProperty((int)position.X / Game1.tileSize + 1, (int)i, "Water", "Back") == null)
					{
						draw = false;
						break;
					}
				}
				if (draw)
				{
					this.temporarySprites.Add(new SeaMonsterTemporarySprite(250f, 4, Game1.random.Next(7), position));
				}
			}
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0016BC33 File Offset: 0x00169E33
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (!Game1.isRaining && !Game1.isFestival())
			{
				Game1.changeMusicTrack("none");
			}
			this.oldMariner = null;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0016BC5C File Offset: 0x00169E5C
		public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
		{
			if (Game1.currentSeason.Equals("summer") && who.getTileX() >= 82 && who.FishingLevel >= 5 && !who.fishCaught.ContainsKey(159) && waterDepth >= 3 && Game1.random.NextDouble() < 0.18)
			{
				return new Object(159, 1, false, -1, 0);
			}
			return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0016BCD8 File Offset: 0x00169ED8
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			Microsoft.Xna.Framework.Rectangle tidePools = new Microsoft.Xna.Framework.Rectangle(65, 11, 25, 12);
			float chance = 1f;
			while (Game1.random.NextDouble() < (double)chance)
			{
				int index = 393;
				if (Game1.random.NextDouble() < 0.2)
				{
					index = 397;
				}
				Vector2 position = new Vector2((float)Game1.random.Next(tidePools.X, tidePools.Right), (float)Game1.random.Next(tidePools.Y, tidePools.Bottom));
				if (this.isTileLocationTotallyClearAndPlaceable(position))
				{
					this.dropObject(new Object(index, 1, false, -1, 0), position * (float)Game1.tileSize, Game1.viewport, true, null);
				}
				chance /= 2f;
			}
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth >= 12 && Game1.dayOfMonth <= 14)
			{
				for (int i = 0; i < 5; i++)
				{
					this.spawnObjects();
				}
				chance = 1.5f;
				while (Game1.random.NextDouble() < (double)chance)
				{
					int index2 = 393;
					if (Game1.random.NextDouble() < 0.2)
					{
						index2 = 397;
					}
					Vector2 position2 = base.getRandomTile();
					position2.Y /= 2f;
					string prop = base.doesTileHaveProperty((int)position2.X, (int)position2.Y, "Type", "Back");
					if (this.isTileLocationTotallyClearAndPlaceable(position2) && (prop == null || !prop.Equals("Wood")))
					{
						this.dropObject(new Object(index2, 1, false, -1, 0), position2 * (float)Game1.tileSize, Game1.viewport, true, null);
					}
					chance /= 1.1f;
				}
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0016BEA9 File Offset: 0x0016A0A9
		public void doneWithBridgeFix()
		{
			Game1.globalFadeToClear(null, 0.02f);
			Game1.viewportFreeze = false;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0016BEBC File Offset: 0x0016A0BC
		public void fadedForBridgeFix()
		{
			DelayedAction.playSoundAfterDelay("crafting", 1000);
			DelayedAction.playSoundAfterDelay("crafting", 1500);
			DelayedAction.playSoundAfterDelay("crafting", 2000);
			DelayedAction.playSoundAfterDelay("crafting", 2500);
			DelayedAction.playSoundAfterDelay("axchop", 3000);
			DelayedAction.playSoundAfterDelay("Ship", 3200);
			Game1.viewportFreeze = true;
			Game1.viewport.X = -10000;
			this.bridgeFixed = true;
			Game1.pauseThenDoFunction(4000, new Game1.afterFadeFunction(this.doneWithBridgeFix));
			this.fixBridge();
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0016BF5C File Offset: 0x0016A15C
		public override bool answerDialogueAction(string questionAndAnswer, string[] questionParams)
		{
			if (questionAndAnswer != null && questionAndAnswer.Equals("BeachBridge_Yes"))
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.fadedForBridgeFix), 0.02f);
				Game1.player.removeItemsFromInventory(388, 300);
				return true;
			}
			return base.answerDialogueAction(questionAndAnswer, questionParams);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0016BFB0 File Offset: 0x0016A1B0
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			int tileIndexOfCheckLocation = (this.map.GetLayer("Buildings").Tiles[tileLocation] != null) ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1;
			if (tileIndexOfCheckLocation != 284)
			{
				if (tileIndexOfCheckLocation == 496)
				{
					if (Game1.stats.DaysPlayed <= 1u)
					{
						Game1.drawLetterMessage(Game1.content.LoadString("Strings\\Locations:Beach_GoneFishingMessage", new object[0]).Replace('\n', '^'));
						return false;
					}
				}
			}
			else if (who.hasItemInInventory(388, 300, 0))
			{
				base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Beach_FixBridge_Question", new object[0]), base.createYesNoResponses(), "BeachBridge");
			}
			else
			{
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Beach_FixBridge_Hint", new object[0]));
			}
			if (this.oldMariner != null && this.oldMariner.getTileX() == tileLocation.X && this.oldMariner.getTileY() == tileLocation.Y)
			{
				string playerTerm = Game1.content.LoadString("Strings\\Locations:Beach_Mariner_Player_" + (who.isMale ? "Male" : "Female"), new object[0]);
				if (!who.isMarried() && who.specialItems.Contains(460) && !Utility.doesItemWithThisIndexExistAnywhere(460, false))
				{
					for (int i = who.specialItems.Count - 1; i >= 0; i--)
					{
						if (who.specialItems[i] == 460)
						{
							who.specialItems.RemoveAt(i);
						}
					}
				}
				if (who.isMarried())
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerMarried", new object[]
					{
						playerTerm
					})));
				}
				else if (who.specialItems.Contains(460))
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerHasItem", new object[]
					{
						playerTerm
					})));
				}
				else if (who.hasAFriendWithHeartLevel(10, true) && who.houseUpgradeLevel == 0)
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerNotUpgradedHouse", new object[]
					{
						playerTerm
					})));
				}
				else if (who.hasAFriendWithHeartLevel(10, true))
				{
					Response[] answers = new Response[]
					{
						new Response("Buy", Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerBuyItem_AnswerYes", new object[0])),
						new Response("Not", Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerBuyItem_AnswerNo", new object[0]))
					};
					base.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerBuyItem_Question", new object[]
					{
						playerTerm
					})), answers, "mariner");
				}
				else
				{
					Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Beach_Mariner_PlayerNoRelationship", new object[]
					{
						playerTerm
					})));
				}
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0016C2B5 File Offset: 0x0016A4B5
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			return (this.oldMariner != null && position.Intersects(this.oldMariner.GetBoundingBox())) || base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0016C2E4 File Offset: 0x0016A4E4
		public override void checkForMusic(GameTime time)
		{
			if (Game1.random.NextDouble() < 0.003 && Game1.timeOfDay < 1900)
			{
				Game1.playSound("seagulls");
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0016C314 File Offset: 0x0016A514
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (!Game1.isRaining && !Game1.isFestival())
			{
				Game1.changeMusicTrack("ocean");
			}
			int numSeagulls = Game1.random.Next(6);
			foreach (Vector2 tile in Utility.getPositionsInClusterAroundThisTile(new Vector2((float)Game1.random.Next(this.map.DisplayWidth / Game1.tileSize), (float)Game1.random.Next(12, this.map.DisplayHeight / Game1.tileSize)), numSeagulls))
			{
				if (base.isTileOnMap(tile) && (this.isTileLocationTotallyClearAndPlaceable(tile) || base.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Water", "Back") != null))
				{
					int state = 3;
					if (base.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Water", "Back") != null)
					{
						state = 2;
						if (Game1.random.NextDouble() < 0.5)
						{
							continue;
						}
					}
					this.critters.Add(new Seagull(tile * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), state));
				}
			}
			if (Game1.isRaining && Game1.timeOfDay < 1900)
			{
				this.oldMariner = new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Mariner"), 0, 16, 32), new Vector2(80f, 5f) * (float)Game1.tileSize, 2, "Old Mariner", null);
			}
			if (this.bridgeFixed)
			{
				this.fixBridge();
			}
			if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth >= 12 && Game1.dayOfMonth <= 14)
			{
				this.waterColor = new Color(0, 255, 0) * 0.4f;
			}
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0016C51C File Offset: 0x0016A71C
		public void fixBridge()
		{
			base.setMapTile(58, 13, 301, "Buildings", null, 1);
			base.setMapTile(59, 13, 301, "Buildings", null, 1);
			base.setMapTile(60, 13, 301, "Buildings", null, 1);
			base.setMapTile(61, 13, 301, "Buildings", null, 1);
			base.setMapTile(58, 14, 336, "Back", null, 1);
			base.setMapTile(59, 14, 336, "Back", null, 1);
			base.setMapTile(60, 14, 336, "Back", null, 1);
			base.setMapTile(61, 14, 336, "Back", null, 1);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0016C5DC File Offset: 0x0016A7DC
		public override void draw(SpriteBatch b)
		{
			if (this.oldMariner != null)
			{
				this.oldMariner.draw(b);
			}
			base.draw(b);
			if (!this.bridgeFixed)
			{
				float yOffset = 4f * (float)Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(58 * Game1.tileSize - 8), (float)(13 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(14 * Game1.tileSize) / 10000f + 1E-06f + 0.0058f);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(58 * Game1.tileSize + Game1.tileSize / 2), (float)(13 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + yOffset)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0f, new Vector2(6f, 6f), (float)Game1.pixelZoom, SpriteEffects.None, (float)(14 * Game1.tileSize) / 10000f + 1E-05f + 0.0058f);
			}
		}

		// Token: 0x04001292 RID: 4754
		private NPC oldMariner;

		// Token: 0x04001293 RID: 4755
		public bool bridgeFixed;
	}
}
