using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace StardewValley.Characters
{
	// Token: 0x02000144 RID: 324
	public class JunimoHarvester : NPC
	{
		// Token: 0x06001250 RID: 4688 RVA: 0x00175C18 File Offset: 0x00173E18
		public JunimoHarvester(Vector2 position, JunimoHut myHome, int whichJunimoNumberFromThisHut) : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Junimo"), 0, 16, 16), position, 2, "Junimo", null)
		{
			this.home = myHome;
			this.whichJunimoFromThisHut = whichJunimoNumberFromThisHut;
			Random r = new Random(myHome.tileX + myHome.tileY * 777 + whichJunimoNumberFromThisHut);
			this.nextPosition = this.GetBoundingBox();
			this.breather = false;
			this.speed = 3;
			this.forceUpdateTimer = 9999;
			this.collidesWithOtherCharacters = true;
			this.ignoreMovementAnimation = true;
			this.farmerPassesThrough = true;
			this.scale = 0.75f;
			this.alpha = 0f;
			this.hideShadow = true;
			this.alphaChange = 0.05f;
			if (r.NextDouble() < 0.25)
			{
				switch (r.Next(8))
				{
				case 0:
					this.color = Color.Red;
					break;
				case 1:
					this.color = Color.Goldenrod;
					break;
				case 2:
					this.color = Color.Yellow;
					break;
				case 3:
					this.color = Color.Lime;
					break;
				case 4:
					this.color = new Color(0, 255, 180);
					break;
				case 5:
					this.color = new Color(0, 100, 255);
					break;
				case 6:
					this.color = Color.MediumPurple;
					break;
				case 7:
					this.color = Color.Salmon;
					break;
				}
				if (r.NextDouble() < 0.01)
				{
					this.color = Color.White;
				}
			}
			else
			{
				switch (r.Next(8))
				{
				case 0:
					this.color = Color.LimeGreen;
					break;
				case 1:
					this.color = Color.Orange;
					break;
				case 2:
					this.color = Color.LightGreen;
					break;
				case 3:
					this.color = Color.Tan;
					break;
				case 4:
					this.color = Color.GreenYellow;
					break;
				case 5:
					this.color = Color.LawnGreen;
					break;
				case 6:
					this.color = Color.PaleGreen;
					break;
				case 7:
					this.color = Color.Turquoise;
					break;
				}
			}
			this.willDestroyObjectsUnderfoot = false;
			this.currentLocation = Game1.getFarm();
			Vector2 tileToPathfindTo = Vector2.Zero;
			switch (whichJunimoNumberFromThisHut)
			{
			case 0:
				tileToPathfindTo = Utility.recursiveFindOpenTileForCharacter(this, this.currentLocation, new Vector2((float)(this.home.tileX + 1), (float)(this.home.tileY + this.home.tilesHigh + 1)), 30);
				break;
			case 1:
				tileToPathfindTo = Utility.recursiveFindOpenTileForCharacter(this, this.currentLocation, new Vector2((float)(this.home.tileX - 1), (float)this.home.tileY), 30);
				break;
			case 2:
				tileToPathfindTo = Utility.recursiveFindOpenTileForCharacter(this, this.currentLocation, new Vector2((float)(this.home.tileX + this.home.tilesWide), (float)this.home.tileY), 30);
				break;
			}
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.DoWork += new DoWorkEventHandler(this.pathFindToNewCrop_doWork);
			this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.pathFindToNewCrop_done);
			if (tileToPathfindTo != Vector2.Zero)
			{
				this.controller = new PathFindController(this, this.currentLocation, Utility.Vector2ToPoint(tileToPathfindTo), -1, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100);
			}
			if (this.controller == null || this.controller.pathToEndPoint == null)
			{
				this.pathfindToRandomSpotAroundHut();
				if (this.controller == null || this.controller.pathToEndPoint == null)
				{
					this.destroy = true;
				}
			}
			this.collidesWithOtherCharacters = false;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00175FE4 File Offset: 0x001741E4
		public void reachFirstDestinationFromHut(Character c, GameLocation l)
		{
			this.tryToHarvestHere();
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00175FEC File Offset: 0x001741EC
		public void tryToHarvestHere()
		{
			if (this.currentLocation != null)
			{
				if (this.currentLocation.terrainFeatures.ContainsKey(base.getTileLocation()) && this.currentLocation.terrainFeatures[base.getTileLocation()] is HoeDirt && (this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).readyForHarvest())
				{
					this.harvestTimer = 2000;
					return;
				}
				this.pokeToHarvest();
			}
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0017606C File Offset: 0x0017426C
		public void pokeToHarvest()
		{
			if (!this.home.isTilePassable(base.getTileLocation()))
			{
				this.destroy = true;
				return;
			}
			if (this.harvestTimer <= 0 && Game1.random.NextDouble() < 0.7)
			{
				this.pathfindToNewCrop();
			}
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x001760B8 File Offset: 0x001742B8
		public void fadeAway()
		{
			this.collidesWithOtherCharacters = false;
			this.alphaChange = -0.015f;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x001760CC File Offset: 0x001742CC
		public void setAlpha(float a)
		{
			this.alpha = a;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x001760D5 File Offset: 0x001742D5
		public void fadeBack()
		{
			this.alpha = 0f;
			this.alphaChange = 0.02f;
			this.isInvisible = false;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x001760F4 File Offset: 0x001742F4
		public void setMoving(int xSpeed, int ySpeed)
		{
			this.motion.X = (float)xSpeed;
			this.motion.Y = (float)ySpeed;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00176110 File Offset: 0x00174310
		public void setMoving(Vector2 motion)
		{
			this.motion = motion;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00176119 File Offset: 0x00174319
		public override void Halt()
		{
			base.Halt();
			this.motion = Vector2.Zero;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0017612C File Offset: 0x0017432C
		public void junimoReachedHut(Character c, GameLocation l)
		{
			this.controller = null;
			this.motion.X = 0f;
			this.motion.Y = -1f;
			this.destroy = true;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0017615C File Offset: 0x0017435C
		public bool foundCropEndFunction(PathNode currentNode, Point endPoint, GameLocation location, Character c)
		{
			return location.isCropAtTile(currentNode.x, currentNode.y) && (location.terrainFeatures[new Vector2((float)currentNode.x, (float)currentNode.y)] as HoeDirt).readyForHarvest();
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x001761AC File Offset: 0x001743AC
		public void pathFindToNewCrop_doWork(object sender, DoWorkEventArgs e)
		{
			if (!this.thinking)
			{
				this.thinking = true;
				if (Game1.timeOfDay > 1900)
				{
					if (this.controller == null)
					{
						this.returnToJunimoHut(this.currentLocation);
					}
					return;
				}
				if (Game1.random.NextDouble() < 0.035 || this.home.noHarvest)
				{
					this.pathfindToRandomSpotAroundHut();
					return;
				}
				this.controller = new PathFindController(this, this.currentLocation, new PathFindController.isAtEnd(this.foundCropEndFunction), -1, false, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100, Point.Zero);
				if (this.controller.pathToEndPoint == null || Math.Abs(this.controller.pathToEndPoint.Last<Point>().X - (this.home.tileX + 1)) > 8 || Math.Abs(this.controller.pathToEndPoint.Last<Point>().Y - (this.home.tileY + 1)) > 8)
				{
					if (Game1.random.NextDouble() < 0.5 && !this.home.lastKnownCropLocation.Equals(Point.Zero))
					{
						this.controller = new PathFindController(this, this.currentLocation, this.home.lastKnownCropLocation, -1, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100);
						return;
					}
					if (Game1.random.NextDouble() < 0.25)
					{
						this.returnToJunimoHut(this.currentLocation);
						return;
					}
					this.pathfindToRandomSpotAroundHut();
					return;
				}
				else
				{
					this.sprite.CurrentAnimation = null;
				}
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0017633C File Offset: 0x0017453C
		public void pathFindToNewCrop_done(object sender, RunWorkerCompletedEventArgs e)
		{
			this.thinking = false;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00176345 File Offset: 0x00174545
		public void pathfindToNewCrop()
		{
			if (!this.thinking && !this.backgroundWorker.IsBusy)
			{
				this.backgroundWorker.RunWorkerAsync();
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00176368 File Offset: 0x00174568
		public void returnToJunimoHut(GameLocation location)
		{
			if (Utility.isOnScreen(Utility.Vector2ToPoint(this.position / (float)Game1.tileSize), Game1.tileSize, this.currentLocation))
			{
				this.jump();
			}
			this.collidesWithOtherCharacters = false;
			this.controller = new PathFindController(this, location, new Point(this.home.tileX + 1, this.home.tileY + 1), 0, new PathFindController.endBehavior(this.junimoReachedHut));
			if (this.controller.pathToEndPoint == null || this.controller.pathToEndPoint.Count<Point>() == 0 || location.isCollidingPosition(this.nextPosition, Game1.viewport, false, 0, false, this))
			{
				this.destroy = true;
			}
			if (Utility.isOnScreen(Utility.Vector2ToPoint(this.position / (float)Game1.tileSize), Game1.tileSize, this.currentLocation))
			{
				Game1.playSound("junimoMeep1");
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00002834 File Offset: 0x00000A34
		public override void faceDirection(int direction)
		{
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00176454 File Offset: 0x00174654
		public override void update(GameTime time, GameLocation location)
		{
			if (this.thinking)
			{
				this.sprite.Animate(time, 8, 4, 100f);
				return;
			}
			base.update(time, location);
			this.forceUpdateTimer = 99999;
			if (this.eventActor)
			{
				return;
			}
			if (this.destroy)
			{
				this.alphaChange = -0.05f;
			}
			this.alpha += this.alphaChange;
			if (this.alpha > 1f)
			{
				this.alpha = 1f;
				this.hideShadow = false;
			}
			else if (this.alpha < 0f)
			{
				this.alpha = 0f;
				this.isInvisible = true;
				this.hideShadow = true;
				if (this.destroy)
				{
					location.characters.Remove(this);
					this.home.myJunimos.Remove(this);
				}
			}
			if (this.harvestTimer > 0)
			{
				int oldTimer = this.harvestTimer;
				this.harvestTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.harvestTimer > 1800)
				{
					this.sprite.CurrentFrame = 0;
				}
				else if (this.harvestTimer > 1600)
				{
					this.sprite.CurrentFrame = 1;
				}
				else if (this.harvestTimer > 1000)
				{
					this.sprite.CurrentFrame = 2;
					base.shake(50);
				}
				else if (oldTimer >= 1000 && this.harvestTimer < 1000)
				{
					this.sprite.CurrentFrame = 0;
					if (this.currentLocation != null && this.currentLocation.terrainFeatures.ContainsKey(base.getTileLocation()) && this.currentLocation.terrainFeatures[base.getTileLocation()] is HoeDirt && (this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).readyForHarvest())
					{
						this.sprite.CurrentFrame = 44;
						this.lastItemHarvested = null;
						if ((this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).crop.harvest(base.getTileX(), base.getTileY(), this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt, this))
						{
							(this.currentLocation.terrainFeatures[base.getTileLocation()] as HoeDirt).destroyCrop(base.getTileLocation(), Game1.currentLocation.Equals(this.currentLocation));
						}
						if (this.lastItemHarvested != null && this.currentLocation.Equals(Game1.currentLocation))
						{
							this.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.lastItemHarvested.parentSheetIndex, 16, 16), 1000f, 1, 0, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float)base.getStandingY() / 10000f + 0.01f, 0.02f, Color.White, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
							{
								motion = new Vector2(0.08f, -0.25f)
							});
							if (this.lastItemHarvested is ColoredObject)
							{
								this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.lastItemHarvested.parentSheetIndex + 1, 16, 16), 1000f, 1, 0, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float)base.getStandingY() / 10000f + 0.015f, 0.02f, (this.lastItemHarvested as ColoredObject).color, (float)Game1.pixelZoom, -0.01f, 0f, 0f, false)
								{
									motion = new Vector2(0.08f, -0.25f)
								});
							}
						}
					}
				}
				else if (this.harvestTimer <= 0)
				{
					this.pokeToHarvest();
				}
			}
			else if (!this.isInvisible && this.controller == null)
			{
				if (this.addedSpeed > 0 || this.speed > 2 || this.isCharging)
				{
					this.destroy = true;
				}
				this.nextPosition = this.GetBoundingBox();
				this.nextPosition.X = this.nextPosition.X + (int)this.motion.X;
				bool sparkle = false;
				if (!location.isCollidingPosition(this.nextPosition, Game1.viewport, this))
				{
					this.position.X = this.position.X + (float)((int)this.motion.X);
					sparkle = true;
				}
				this.nextPosition.X = this.nextPosition.X - (int)this.motion.X;
				this.nextPosition.Y = this.nextPosition.Y + (int)this.motion.Y;
				if (!location.isCollidingPosition(this.nextPosition, Game1.viewport, this))
				{
					this.position.Y = this.position.Y + (float)((int)this.motion.Y);
					sparkle = true;
				}
				if ((!this.motion.Equals(Vector2.Zero) & sparkle) && Game1.random.NextDouble() < 0.005)
				{
					location.temporarySprites.Add(new TemporaryAnimatedSprite((Game1.random.NextDouble() < 0.5) ? 10 : 11, this.position, this.color, 8, false, 100f, 0, -1, -1f, -1, 0)
					{
						motion = this.motion / 4f,
						alphaFade = 0.01f,
						layerDepth = 0.8f,
						scale = 0.75f,
						alpha = 0.75f
					});
				}
				if (Game1.random.NextDouble() < 0.002)
				{
					switch (Game1.random.Next(6))
					{
					case 0:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(12, 200),
							new FarmerSprite.AnimationFrame(13, 200),
							new FarmerSprite.AnimationFrame(14, 200),
							new FarmerSprite.AnimationFrame(15, 200)
						});
						break;
					case 1:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(44, 200),
							new FarmerSprite.AnimationFrame(45, 200),
							new FarmerSprite.AnimationFrame(46, 200),
							new FarmerSprite.AnimationFrame(47, 200)
						});
						break;
					case 2:
						this.sprite.CurrentAnimation = null;
						break;
					case 3:
						this.jumpWithoutSound(8f);
						this.yJumpVelocity /= 2f;
						this.sprite.CurrentAnimation = null;
						break;
					case 4:
						if (!this.home.noHarvest)
						{
							this.pathfindToNewCrop();
						}
						break;
					case 5:
						this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(28, 100),
							new FarmerSprite.AnimationFrame(29, 100),
							new FarmerSprite.AnimationFrame(30, 100),
							new FarmerSprite.AnimationFrame(31, 100)
						});
						break;
					}
				}
			}
			if (this.controller != null || !this.motion.Equals(Vector2.Zero))
			{
				this.sprite.CurrentAnimation = null;
				if (this.moveRight || (Math.Abs(this.motion.X) > Math.Abs(this.motion.Y) && this.motion.X > 0f))
				{
					this.flip = false;
					if (this.sprite.Animate(time, 16, 8, 50f))
					{
						this.sprite.CurrentFrame = 16;
						return;
					}
				}
				else
				{
					if (this.moveLeft || (Math.Abs(this.motion.X) > Math.Abs(this.motion.Y) && this.motion.X < 0f))
					{
						if (this.sprite.Animate(time, 16, 8, 50f))
						{
							this.sprite.CurrentFrame = 16;
						}
						this.flip = true;
						return;
					}
					if (this.moveUp || (Math.Abs(this.motion.Y) > Math.Abs(this.motion.X) && this.motion.Y < 0f))
					{
						if (this.sprite.Animate(time, 32, 8, 50f))
						{
							this.sprite.CurrentFrame = 32;
							return;
						}
					}
					else if (this.moveDown)
					{
						this.sprite.Animate(time, 0, 8, 50f);
						return;
					}
				}
			}
			else if (this.sprite.CurrentAnimation == null && this.harvestTimer <= 0)
			{
				this.sprite.Animate(time, 8, 4, 100f);
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00176D88 File Offset: 0x00174F88
		public void pathfindToRandomSpotAroundHut()
		{
			Vector2 tileToPathfindTo = new Vector2((float)(this.home.tileX + 1 + Game1.random.Next(-8, 9)), (float)(this.home.tileY + 1 + Game1.random.Next(-8, 9)));
			this.controller = new PathFindController(this, this.currentLocation, Utility.Vector2ToPoint(tileToPathfindTo), -1, new PathFindController.endBehavior(this.reachFirstDestinationFromHut), 100);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00176E00 File Offset: 0x00175000
		public void tryToAddItemToHut(Item i)
		{
			this.lastItemHarvested = i;
			Item result = this.home.output.addItem(i);
			if (result != null && i is Object)
			{
				for (int j = 0; j < result.Stack; j++)
				{
					Game1.createObjectDebris(i.parentSheetIndex, base.getTileX(), base.getTileY(), -1, (i as Object).quality, 1f, this.currentLocation);
				}
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00176E70 File Offset: 0x00175070
		public override void draw(SpriteBatch b, float alpha = 1f)
		{
			if (!this.isInvisible)
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)this.sprite.spriteHeight * 3f / 4f * (float)Game1.pixelZoom / (float)Math.Pow((double)(this.sprite.spriteHeight / 16), 2.0) + (float)this.yJumpOffset - (float)(Game1.pixelZoom * 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(base.Sprite.SourceRect), this.color * this.alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.sprite.spriteHeight * Game1.pixelZoom) * 3f / 4f) / (float)Game1.pixelZoom, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : (((float)(base.getStandingY() + this.whichJunimoFromThisHut) + (float)base.getStandingX() / 10000f) / 10000f)));
				if (!this.swimming && !this.hideShadow)
				{
					b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(Game1.tileSize * 3) / 4f - (float)Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), this.color * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)this.yJumpOffset / 40f) * this.scale, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f) - 1E-06f);
				}
			}
		}

		// Token: 0x040012FE RID: 4862
		private float alpha = 1f;

		// Token: 0x040012FF RID: 4863
		private float alphaChange;

		// Token: 0x04001300 RID: 4864
		private Vector2 motion = Vector2.Zero;

		// Token: 0x04001301 RID: 4865
		private new Rectangle nextPosition;

		// Token: 0x04001302 RID: 4866
		private Color color;

		// Token: 0x04001303 RID: 4867
		private JunimoHut home;

		// Token: 0x04001304 RID: 4868
		private bool destroy;

		// Token: 0x04001305 RID: 4869
		private Item lastItemHarvested;

		// Token: 0x04001306 RID: 4870
		private BackgroundWorker backgroundWorker;

		// Token: 0x04001307 RID: 4871
		public int whichJunimoFromThisHut;

		// Token: 0x04001308 RID: 4872
		private int harvestTimer;

		// Token: 0x04001309 RID: 4873
		private bool thinking;
	}
}
