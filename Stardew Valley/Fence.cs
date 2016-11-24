using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley
{
	// Token: 0x0200002C RID: 44
	public class Fence : Object
	{
		// Token: 0x0600024E RID: 590 RVA: 0x000330FC File Offset: 0x000312FC
		public Fence(Vector2 tileLocation, int whichType, bool isGate)
		{
			this.whichType = whichType;
			switch (whichType)
			{
			case 1:
				this.health = 28f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Wood Fence";
				this.parentSheetIndex = -5;
				break;
			case 2:
				this.health = 60f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Stone Fence";
				this.parentSheetIndex = -6;
				break;
			case 3:
				this.health = 125f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Iron Fence";
				this.parentSheetIndex = -7;
				break;
			case 4:
				this.health = 100f;
				this.name = "Gate";
				this.parentSheetIndex = -9;
				break;
			case 5:
				this.health = 280f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Hardwood Fence";
				this.parentSheetIndex = -8;
				break;
			}
			this.health *= 2f;
			this.maxHealth = this.health;
			this.price = whichType;
			this.isGate = isGate;
			this.reloadSprite();
			if (Fence.fenceDrawGuide == null)
			{
				Fence.populateFenceDrawGuide();
			}
			this.tileLocation = tileLocation;
			this.canBeSetDown = true;
			this.canBeGrabbed = true;
			this.price = 1;
			if (isGate)
			{
				this.health *= 2f;
			}
			base.Type = "Crafting";
			this.boundingBox = new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000332DC File Offset: 0x000314DC
		public Fence()
		{
			if (Fence.fenceDrawGuide == null)
			{
				Fence.populateFenceDrawGuide();
			}
			this.price = 1;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000332F8 File Offset: 0x000314F8
		public void repair()
		{
			switch (this.whichType)
			{
			case 1:
				this.health = 28f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Wood Fence";
				this.parentSheetIndex = -5;
				return;
			case 2:
				this.health = 60f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Stone Fence";
				this.parentSheetIndex = -6;
				return;
			case 3:
				this.health = 125f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Iron Fence";
				this.parentSheetIndex = -7;
				return;
			case 4:
				this.health = 100f;
				this.name = "Gate";
				this.parentSheetIndex = -9;
				return;
			case 5:
				this.health = 280f + (float)Game1.random.Next(-100, 101) / 100f;
				this.name = "Hardwood Fence";
				this.parentSheetIndex = -8;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0003341C File Offset: 0x0003161C
		public static void populateFenceDrawGuide()
		{
			Fence.fenceDrawGuide = new Dictionary<int, int>();
			Fence.fenceDrawGuide.Add(0, 5);
			Fence.fenceDrawGuide.Add(10, 9);
			Fence.fenceDrawGuide.Add(100, 10);
			Fence.fenceDrawGuide.Add(1000, 3);
			Fence.fenceDrawGuide.Add(500, 5);
			Fence.fenceDrawGuide.Add(1010, 8);
			Fence.fenceDrawGuide.Add(1100, 6);
			Fence.fenceDrawGuide.Add(1500, 3);
			Fence.fenceDrawGuide.Add(600, 0);
			Fence.fenceDrawGuide.Add(510, 2);
			Fence.fenceDrawGuide.Add(110, 7);
			Fence.fenceDrawGuide.Add(1600, 0);
			Fence.fenceDrawGuide.Add(1610, 4);
			Fence.fenceDrawGuide.Add(1510, 2);
			Fence.fenceDrawGuide.Add(1110, 7);
			Fence.fenceDrawGuide.Add(610, 4);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00033528 File Offset: 0x00031728
		public override void updateWhenCurrentLocation(GameTime time)
		{
			this.gatePosition += this.gateMotion;
			if (this.gatePosition >= 88 || this.gatePosition <= 0)
			{
				this.gateMotion = 0;
			}
			if (this.heldObject != null)
			{
				this.heldObject.updateWhenCurrentLocation(time);
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00033578 File Offset: 0x00031778
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (!justCheckingForActivity && who != null && who.currentLocation.objects.ContainsKey(new Vector2((float)who.getTileX(), (float)(who.getTileY() - 1))) && who.currentLocation.objects.ContainsKey(new Vector2((float)who.getTileX(), (float)(who.getTileY() + 1))) && who.currentLocation.objects.ContainsKey(new Vector2((float)(who.getTileX() + 1), (float)who.getTileY())) && who.currentLocation.objects.ContainsKey(new Vector2((float)(who.getTileX() - 1), (float)who.getTileY())))
			{
				this.performToolAction(null);
			}
			if (this.health <= 1f)
			{
				return false;
			}
			if (this.isGate)
			{
				if (justCheckingForActivity)
				{
					return true;
				}
				int drawSum = 0;
				Vector2 surroundingLocations = this.tileLocation;
				surroundingLocations.X += 1f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 100;
				}
				surroundingLocations.X -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 10;
				}
				surroundingLocations.X += 1f;
				surroundingLocations.Y += 1f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 500;
				}
				surroundingLocations.Y -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 1000;
				}
				if (this.isGate)
				{
					if (drawSum == 110 || drawSum == 1500)
					{
						who.temporaryImpassableTile = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
						if (this.gatePosition == 0)
						{
							this.gatePosition = 88;
						}
						else
						{
							this.gatePosition = 0;
						}
						Game1.playSound("doorClose");
					}
					else
					{
						who.temporaryImpassableTile = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
						this.gatePosition = 0;
					}
				}
				return true;
			}
			else
			{
				if (justCheckingForActivity)
				{
					return false;
				}
				foreach (Vector2 v in Utility.getAdjacentTileLocations(this.tileLocation))
				{
					if (Game1.currentLocation.objects.ContainsKey(v) && Game1.currentLocation.objects[v].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[v]).isGate)
					{
						((Fence)Game1.currentLocation.objects[v]).checkForAction(who, false);
						return true;
					}
				}
				return this.health <= 0f;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000339BC File Offset: 0x00031BBC
		public override bool performToolAction(Tool t)
		{
			if (this.heldObject != null && t != null && !(t is MeleeWeapon) && t.isHeavyHitter())
			{
				this.heldObject.performRemoveAction(this.tileLocation, Game1.currentLocation);
				this.heldObject = null;
				Game1.playSound("axchop");
				return false;
			}
			if (this.isGate && t != null && (t.GetType() == typeof(Axe) || t is Pickaxe))
			{
				Game1.playSound("axchop");
				Game1.createObjectDebris(325, (int)this.tileLocation.X, (int)this.tileLocation.Y, Game1.player.uniqueMultiplayerID, Game1.player.currentLocation);
				Game1.currentLocation.objects.Remove(this.tileLocation);
				Game1.createRadialDebris(Game1.currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, 6, false, -1, false, -1);
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
			}
			if ((this.whichType == 1 || this.whichType == 5) && (t == null || t.GetType() == typeof(Axe)))
			{
				Game1.playSound("axchop");
				Game1.currentLocation.objects.Remove(this.tileLocation);
				for (int i = 0; i < 4; i++)
				{
					Game1.currentLocation.temporarySprites.Add(new CosmeticDebris(this.fenceTexture, new Vector2(this.tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), (float)Game1.random.Next(-5, 5) / 100f, (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize) / 30f, (float)Game1.random.Next(-800, -100) / 100f, (int)((this.tileLocation.Y + 1f) * (float)Game1.tileSize), new Rectangle(32 + Game1.random.Next(2) * 16 / 2, 96 + Game1.random.Next(2) * 16 / 2, 8, 8), Color.White, (Game1.soundBank != null) ? Game1.soundBank.GetCue("shiny4") : null, null, 0, 200));
				}
				Game1.createRadialDebris(Game1.currentLocation, 12, (int)this.tileLocation.X, (int)this.tileLocation.Y, 6, false, -1, false, -1);
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
				if (this.maxHealth - this.health < 0.5f)
				{
					int num = this.whichType;
					if (num != 1)
					{
						if (num == 5)
						{
							Game1.currentLocation.debris.Add(new Debris(new Object(298, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
						}
					}
					else
					{
						Game1.currentLocation.debris.Add(new Debris(new Object(322, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
					}
				}
			}
			else if ((this.whichType == 2 || this.whichType == 3) && (t == null || t.GetType() == typeof(Pickaxe)))
			{
				Game1.playSound("hammer");
				Game1.currentLocation.objects.Remove(this.tileLocation);
				for (int j = 0; j < 4; j++)
				{
					Game1.currentLocation.temporarySprites.Add(new CosmeticDebris(this.fenceTexture, new Vector2(this.tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), this.tileLocation.Y * (float)Game1.tileSize + (float)(Game1.tileSize / 2)), (float)Game1.random.Next(-5, 5) / 100f, (float)Game1.random.Next(-Game1.tileSize, Game1.tileSize) / 30f, (float)Game1.random.Next(-800, -100) / 100f, (int)((this.tileLocation.Y + 1f) * (float)Game1.tileSize), new Rectangle(32 + Game1.random.Next(2) * 16 / 2, 96 + Game1.random.Next(2) * 16 / 2, 8, 8), Color.White, (Game1.soundBank != null) ? Game1.soundBank.GetCue("shiny4") : null, null, 0, 200));
				}
				Game1.createRadialDebris(Game1.currentLocation, 14, (int)this.tileLocation.X, (int)this.tileLocation.Y, 6, false, -1, false, -1);
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
				if (this.maxHealth - this.health < 0.5f)
				{
					int num = this.whichType;
					if (num != 2)
					{
						if (num == 3)
						{
							Game1.currentLocation.debris.Add(new Debris(new Object(324, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
						}
					}
					else
					{
						Game1.currentLocation.debris.Add(new Debris(new Object(323, 1, false, -1, 0), this.tileLocation * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2))));
					}
				}
			}
			return false;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x000340B8 File Offset: 0x000322B8
		public override bool minutesElapsed(int minutes, GameLocation l)
		{
			if (!Game1.getFarm().isBuildingConstructed("Gold Clock"))
			{
				this.health -= (float)minutes / 1440f;
				if (this.health <= -1f && (Game1.timeOfDay <= 610 || Game1.timeOfDay > 1800))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00034113 File Offset: 0x00032313
		public override void actionOnPlayerEntry()
		{
			base.actionOnPlayerEntry();
			if (this.heldObject != null)
			{
				this.heldObject.actionOnPlayerEntry();
				this.heldObject.isOn = true;
				this.heldObject.initializeLightSource(this.tileLocation);
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0003414C File Offset: 0x0003234C
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			if (dropIn.parentSheetIndex == 325)
			{
				if (probe)
				{
					return false;
				}
				if (!this.isGate)
				{
					Vector2 surroundingLocations = this.tileLocation;
					int drawSum = 0;
					surroundingLocations.X += 1f;
					if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
					{
						drawSum += 100;
					}
					surroundingLocations.X -= 2f;
					if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
					{
						drawSum += 10;
					}
					surroundingLocations.X += 1f;
					surroundingLocations.Y += 1f;
					if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
					{
						drawSum += 500;
					}
					surroundingLocations.Y -= 2f;
					if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
					{
						drawSum += 1000;
					}
					if (drawSum == 1500 || drawSum == 110)
					{
						this.isGate = true;
						Game1.playSound("axe");
						return true;
					}
				}
			}
			else if (dropIn.parentSheetIndex == 93 && this.heldObject == null && !probe && !this.isGate)
			{
				Game1.playSound("axe");
				this.heldObject = new Torch(this.tileLocation, 93);
				this.heldObject.name = "Torch";
				this.heldObject.initializeLightSource(this.tileLocation);
				return true;
			}
			if (this.health <= 1f && !probe)
			{
				int parentSheetIndex = dropIn.parentSheetIndex;
				if (parentSheetIndex != 298)
				{
					switch (parentSheetIndex)
					{
					case 322:
						if (this.whichType == 1)
						{
							this.health = 28f + (float)Game1.random.Next(-500, 500) / 100f;
							Game1.playSound("axe");
							return true;
						}
						break;
					case 323:
						if (this.whichType == 2)
						{
							this.health = 60f + (float)Game1.random.Next(-500, 600) / 100f;
							Game1.playSound("stoneStep");
							return true;
						}
						break;
					case 324:
						if (this.whichType == 3)
						{
							this.health = 125f + (float)Game1.random.Next(-500, 700) / 100f;
							Game1.playSound("hammer");
							return true;
						}
						break;
					}
				}
				else if (this.whichType == 5)
				{
					this.health = 280f + (float)Game1.random.Next(-2000, 2000) / 100f;
					Game1.playSound("axe");
					return true;
				}
			}
			return base.performObjectDropInAction(dropIn, probe, who);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00034518 File Offset: 0x00032718
		public override bool performDropDownAction(Farmer who)
		{
			Vector2 dropTileLocation = new Vector2((float)((int)(Game1.player.GetDropLocation().X / (float)Game1.tileSize)), (float)((int)(Game1.player.GetDropLocation().Y / (float)Game1.tileSize)));
			this.tileLocation = dropTileLocation;
			return false;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00034564 File Offset: 0x00032764
		public override void reloadSprite()
		{
			this.fenceTexture = Game1.content.Load<Texture2D>("LooseSprites\\Fence" + Math.Max(1, this.isGate ? 1 : this.whichType));
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0003459C File Offset: 0x0003279C
		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			spriteBatch.Draw(this.fenceTexture, objectPosition - new Vector2(0f, (float)Game1.tileSize), new Rectangle?(new Rectangle(5 * Fence.fencePieceWidth % this.fenceTexture.Bounds.Width, 5 * Fence.fencePieceWidth / this.fenceTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, (float)(f.getStandingY() + 1) / 10000f);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0003463C File Offset: 0x0003283C
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scale, float transparency, float layerDepth, bool drawStackNumber)
		{
			location.Y -= (float)Game1.tileSize * scale;
			int drawSum = 0;
			Vector2 surroundingLocations = this.tileLocation;
			surroundingLocations.X += 1f;
			if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
			{
				drawSum += 100;
			}
			surroundingLocations.X -= 2f;
			if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
			{
				drawSum += 10;
			}
			surroundingLocations.X += 1f;
			surroundingLocations.Y += 1f;
			if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
			{
				drawSum += 500;
			}
			surroundingLocations.Y -= 2f;
			if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
			{
				drawSum += 1000;
			}
			int sourceRectPosition = Fence.fenceDrawGuide[drawSum];
			if (this.isGate)
			{
				if (drawSum == 110)
				{
					spriteBatch.Draw(this.fenceTexture, location + new Vector2(6f, 6f), new Rectangle?(new Rectangle(0, 512, 88, 24)), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					return;
				}
				if (drawSum == 1500)
				{
					spriteBatch.Draw(this.fenceTexture, location + new Vector2(6f, 6f), new Rectangle?(new Rectangle(112, 512, 16, 64)), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
					return;
				}
			}
			spriteBatch.Draw(this.fenceTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scale, new Rectangle?(Game1.getArbitrarySourceRect(this.fenceTexture, Game1.tileSize, Game1.tileSize * 2, sourceRectPosition)), Color.White * transparency, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)) * scale, scale, SpriteEffects.None, layerDepth);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0003496A File Offset: 0x00032B6A
		public bool countsForDrawing(int type)
		{
			return this.health > 1f && !this.isGate && (type == this.whichType || type == 4);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00034992 File Offset: 0x00032B92
		public override bool isPassable()
		{
			return this.isGate && this.gatePosition >= 88;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x000349AC File Offset: 0x00032BAC
		public override void draw(SpriteBatch b, int x, int y, float alpha = 1f)
		{
			int sourceRectPosition = 1;
			if (this.health > 1f)
			{
				int drawSum = 0;
				Vector2 surroundingLocations = this.tileLocation;
				surroundingLocations.X += 1f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 100;
				}
				surroundingLocations.X -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 10;
				}
				surroundingLocations.X += 1f;
				surroundingLocations.Y += 1f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 500;
				}
				surroundingLocations.Y -= 2f;
				if (Game1.currentLocation.objects.ContainsKey(surroundingLocations) && Game1.currentLocation.objects[surroundingLocations].GetType() == typeof(Fence) && ((Fence)Game1.currentLocation.objects[surroundingLocations]).countsForDrawing(this.whichType))
				{
					drawSum += 1000;
				}
				sourceRectPosition = Fence.fenceDrawGuide[drawSum];
				if (this.isGate)
				{
					if (drawSum == 110)
					{
						b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize - 4 * Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.tileSize))), new Rectangle?(new Rectangle((this.gatePosition == 88) ? 24 : 0, 128, 24, 32)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize / 2 + 1) / 10000f);
						return;
					}
					if (drawSum == 1500)
					{
						b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + 5 * Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.tileSize - 6 * Game1.pixelZoom))), new Rectangle?(new Rectangle((this.gatePosition == 88) ? 16 : 0, 160, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize - Game1.tileSize / 2 + 1) / 10000f);
						b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + 5 * Game1.pixelZoom), (float)(y * Game1.tileSize - Game1.tileSize + 10 * Game1.pixelZoom))), new Rectangle?(new Rectangle((this.gatePosition == 88) ? 16 : 0, 176, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize * 3 / 2 - 1) / 10000f);
						return;
					}
					sourceRectPosition = 17;
				}
				else if (this.heldObject != null)
				{
					this.heldObject.draw(b, x * Game1.tileSize + ((drawSum == 100) ? (-Game1.tileSize / 3) : 0) + ((drawSum == 10) ? (Game1.tileSize / 3 - Game1.pixelZoom * 2) : 0), (y - 1) * Game1.tileSize - Game1.pixelZoom * 4 + ((this.whichType == 2) ? (Game1.tileSize / 4) : 0), (float)(y * Game1.tileSize + Game1.tileSize) / 10000f, 1f);
				}
			}
			b.Draw(this.fenceTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize - Game1.tileSize))), new Rectangle?(new Rectangle(sourceRectPosition * Fence.fencePieceWidth % this.fenceTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / this.fenceTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + Game1.tileSize / 2) / 10000f);
		}

		// Token: 0x0400027C RID: 636
		public const int debrisPieces = 4;

		// Token: 0x0400027D RID: 637
		public static int fencePieceWidth = Game1.tileSize / Game1.pixelZoom;

		// Token: 0x0400027E RID: 638
		public static int fencePieceHeight = Game1.tileSize * 2 / Game1.pixelZoom;

		// Token: 0x0400027F RID: 639
		public const int gateClosedPosition = 0;

		// Token: 0x04000280 RID: 640
		public const int gateOpenedPosition = 88;

		// Token: 0x04000281 RID: 641
		public const int sourceRectForSoloGate = 17;

		// Token: 0x04000282 RID: 642
		public const int globalHealthMultiplier = 2;

		// Token: 0x04000283 RID: 643
		public const int N = 1000;

		// Token: 0x04000284 RID: 644
		public const int E = 100;

		// Token: 0x04000285 RID: 645
		public const int S = 500;

		// Token: 0x04000286 RID: 646
		public const int W = 10;

		// Token: 0x04000287 RID: 647
		public new const int wood = 1;

		// Token: 0x04000288 RID: 648
		public new const int stone = 2;

		// Token: 0x04000289 RID: 649
		public const int steel = 3;

		// Token: 0x0400028A RID: 650
		public const int gate = 4;

		// Token: 0x0400028B RID: 651
		public new const int gold = 5;

		// Token: 0x0400028C RID: 652
		private Texture2D fenceTexture;

		// Token: 0x0400028D RID: 653
		public new float health;

		// Token: 0x0400028E RID: 654
		public float maxHealth;

		// Token: 0x0400028F RID: 655
		public int whichType;

		// Token: 0x04000290 RID: 656
		public int gatePosition;

		// Token: 0x04000291 RID: 657
		public int gateMotion;

		// Token: 0x04000292 RID: 658
		public static Dictionary<int, int> fenceDrawGuide;

		// Token: 0x04000293 RID: 659
		public bool isGate;
	}
}
