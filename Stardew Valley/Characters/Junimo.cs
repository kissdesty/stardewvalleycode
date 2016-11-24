using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;

namespace StardewValley.Characters
{
	// Token: 0x02000143 RID: 323
	public class Junimo : NPC
	{
		// Token: 0x06001236 RID: 4662 RVA: 0x00174703 File Offset: 0x00172903
		public Junimo()
		{
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0017472C File Offset: 0x0017292C
		public Junimo(Vector2 position, int whichArea, bool temporary = false) : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Junimo"), 0, 16, 16), position, 2, "Junimo", null)
		{
			this.whichArea = whichArea;
			try
			{
				this.friendly = ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[whichArea];
			}
			catch (Exception)
			{
				this.friendly = true;
			}
			this.temporaryJunimo = temporary;
			this.nextPosition = this.GetBoundingBox();
			this.breather = false;
			this.speed = 3;
			this.forceUpdateTimer = 9999;
			this.collidesWithOtherCharacters = true;
			this.farmerPassesThrough = true;
			this.scale = 0.75f;
			if (this.temporaryJunimo)
			{
				if (Game1.random.NextDouble() < 0.01)
				{
					switch (Game1.random.Next(8))
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
					if (Game1.random.NextDouble() < 0.01)
					{
						this.color = Color.White;
						return;
					}
				}
				else
				{
					switch (Game1.random.Next(8))
					{
					case 0:
						this.color = Color.LimeGreen;
						return;
					case 1:
						this.color = Color.Orange;
						return;
					case 2:
						this.color = Color.LightGreen;
						return;
					case 3:
						this.color = Color.Tan;
						return;
					case 4:
						this.color = Color.GreenYellow;
						return;
					case 5:
						this.color = Color.LawnGreen;
						return;
					case 6:
						this.color = Color.PaleGreen;
						return;
					case 7:
						this.color = Color.Turquoise;
						return;
					default:
						return;
					}
				}
			}
			else
			{
				switch (whichArea)
				{
				case -1:
				case 0:
					this.color = Color.LimeGreen;
					return;
				case 1:
					this.color = Color.Orange;
					return;
				case 2:
					this.color = Color.Turquoise;
					return;
				case 3:
					this.color = Color.Tan;
					return;
				case 4:
					this.color = Color.Gold;
					return;
				case 5:
					this.color = Color.BlanchedAlmond;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool canPassThroughActionTiles()
		{
			return false;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x001749F8 File Offset: 0x00172BF8
		public void fadeAway()
		{
			this.collidesWithOtherCharacters = false;
			this.alphaChange = (this.stayPut ? -0.005f : -0.015f);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00174A1B File Offset: 0x00172C1B
		public void setAlpha(float a)
		{
			this.alpha = a;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00174A24 File Offset: 0x00172C24
		public void fadeBack()
		{
			this.alpha = 0f;
			this.alphaChange = 0.02f;
			this.isInvisible = false;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00174A43 File Offset: 0x00172C43
		public void setMoving(int xSpeed, int ySpeed)
		{
			this.motion.X = (float)xSpeed;
			this.motion.Y = (float)ySpeed;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00174A5F File Offset: 0x00172C5F
		public void setMoving(Vector2 motion)
		{
			this.motion = motion;
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00174A68 File Offset: 0x00172C68
		public override void Halt()
		{
			base.Halt();
			this.motion = Vector2.Zero;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00174A7B File Offset: 0x00172C7B
		public void returnToJunimoHut(GameLocation location)
		{
			this.jump();
			this.collidesWithOtherCharacters = false;
			this.controller = new PathFindController(this, location, new Point(25, 10), 0, new PathFindController.endBehavior(this.junimoReachedHut));
			Game1.playSound("junimoMeep1");
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00174AB7 File Offset: 0x00172CB7
		public void stayStill()
		{
			this.stayPut = true;
			this.motion = Vector2.Zero;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00174ACB File Offset: 0x00172CCB
		public void allowToMoveAgain()
		{
			this.stayPut = false;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00174AD4 File Offset: 0x00172CD4
		public void returnToJunimoHutToFetchStar(GameLocation location)
		{
			this.friendly = true;
			if (((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areAllAreasComplete())
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.finalCutscene), 0.005f);
				Game1.freezeControls = true;
				this.collidesWithOtherCharacters = false;
				this.farmerPassesThrough = false;
				this.stayStill();
				this.faceDirection(0);
				GameLocation cc = Game1.getLocationFromName("CommunityCenter");
				if (!Game1.player.mailReceived.Contains("ccIsComplete"))
				{
					Game1.player.mailReceived.Add("ccIsComplete");
				}
				if (Game1.currentLocation.Equals(cc))
				{
					Game1.flashAlpha = 1f;
					(cc as CommunityCenter).addStarToPlaque();
					return;
				}
			}
			else
			{
				this.fadeBack();
				DelayedAction.textAboveHeadAfterDelay((Game1.random.NextDouble() < 0.5) ? Game1.content.LoadString("Strings\\Characters:JunimoTextAboveHead1", new object[0]) : Game1.content.LoadString("Strings\\Characters:JunimoTextAboveHead2", new object[0]), this, Game1.random.Next(3000, 6000));
				this.controller = new PathFindController(this, location, new Point(25, 10), 0, new PathFindController.endBehavior(this.junimoReachedHutToFetchStar));
				Game1.playSound("junimoMeep1");
				this.collidesWithOtherCharacters = false;
				this.farmerPassesThrough = false;
				this.holdingBundle = true;
			}
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00174C38 File Offset: 0x00172E38
		private void finalCutscene()
		{
			this.collidesWithOtherCharacters = false;
			this.farmerPassesThrough = false;
			Game1.player.position = new Vector2(29f, 11f) * (float)Game1.tileSize;
			Game1.player.completelyStopAnimatingOrDoingAction();
			Game1.player.faceDirection(3);
			Game1.UpdateViewPort(true, new Point(Game1.player.getStandingX(), Game1.player.getStandingY()));
			Game1.viewport.X = Game1.player.getStandingX() - Game1.viewport.Width / 2;
			Game1.viewport.Y = Game1.player.getStandingY() - Game1.viewport.Height / 2;
			Game1.viewportTarget = Vector2.Zero;
			Game1.viewportCenter = new Point(Game1.player.getStandingX(), Game1.player.getStandingY());
			Game1.moveViewportTo(new Vector2(32.5f, 6f) * (float)Game1.tileSize, 2f, 999999, null, null);
			Game1.globalFadeToClear(new Game1.afterFadeFunction(this.goodbyeDance), 0.005f);
			Game1.pauseTime = 1000f;
			Game1.freezeControls = true;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00174D68 File Offset: 0x00172F68
		public void bringBundleBackToHut(Color bundleColor, GameLocation location)
		{
			if (!this.holdingBundle)
			{
				this.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float)Game1.tileSize;
				int iter = 0;
				while (location.isCollidingPosition(this.GetBoundingBox(), Game1.viewport, this) && iter < 5)
				{
					this.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float)Game1.tileSize;
					iter++;
				}
				if (iter >= 5)
				{
					return;
				}
				if (Game1.random.NextDouble() < 0.25)
				{
					DelayedAction.textAboveHeadAfterDelay((Game1.random.NextDouble() < 0.5) ? Game1.content.LoadString("Strings\\Characters:JunimoThankYou1", new object[0]) : Game1.content.LoadString("Strings\\Characters:JunimoThankYou2", new object[0]), this, Game1.random.Next(3000, 6000));
				}
				this.fadeBack();
				this.bundleColor = bundleColor;
				this.controller = new PathFindController(this, location, new Point(25, 10), 0, new PathFindController.endBehavior(this.junimoReachedHutToReturnBundle));
				this.collidesWithOtherCharacters = false;
				this.farmerPassesThrough = false;
				this.holdingBundle = true;
				this.speed = 1;
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00174EA1 File Offset: 0x001730A1
		private void junimoReachedHutToReturnBundle(Character c, GameLocation l)
		{
			this.holdingBundle = false;
			this.collidesWithOtherCharacters = true;
			this.farmerPassesThrough = true;
			Game1.playSound("Ship");
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00174EC4 File Offset: 0x001730C4
		private void junimoReachedHutToFetchStar(Character c, GameLocation l)
		{
			this.holdingStar = true;
			this.holdingBundle = false;
			this.speed = 1;
			this.collidesWithOtherCharacters = false;
			this.farmerPassesThrough = false;
			this.controller = new PathFindController(this, l, new Point(32, 9), 2, new PathFindController.endBehavior(this.placeStar));
			Game1.playSound("dwop");
			this.farmerPassesThrough = false;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00174F28 File Offset: 0x00173128
		private void placeStar(Character c, GameLocation l)
		{
			this.collidesWithOtherCharacters = false;
			this.farmerPassesThrough = true;
			this.holdingStar = false;
			Game1.playSound("tinyWhip");
			this.friendly = true;
			this.speed = 3;
			l.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 109, 16, 19), 40f, 8, 10, this.position + new Vector2(0f, (float)(-(float)Game1.tileSize)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom * this.scale, 0f, 0f, 0f, false)
			{
				endFunction = new TemporaryAnimatedSprite.endBehavior(this.starDoneSpinning),
				endSound = "yoba",
				motion = new Vector2(0.22f, -2f),
				acceleration = new Vector2(0f, 0.01f),
				id = 777f
			});
			if (l is CommunityCenter && (l as CommunityCenter).areAllAreasComplete())
			{
				Game1.player.faceDirection(0);
				this.fadeAway();
				Game1.pauseThenDoFunction(2000, new Game1.afterFadeFunction(this.goodbyeDance));
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00175069 File Offset: 0x00173269
		public void sayGoodbye()
		{
			this.sayingGoodbye = true;
			this.farmerPassesThrough = true;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00175079 File Offset: 0x00173279
		private void goodbyeDance()
		{
			Game1.player.faceDirection(3);
			(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).junimoGoodbyeDance();
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0017509C File Offset: 0x0017329C
		private void starDoneSpinning(int extraInfo)
		{
			GameLocation cc = Game1.getLocationFromName("CommunityCenter");
			if (Game1.currentLocation.Equals(cc))
			{
				Game1.flashAlpha = 1f;
				(cc as CommunityCenter).addStarToPlaque();
			}
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x001750D8 File Offset: 0x001732D8
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			if (this.textAboveHeadTimer > 0 && this.textAboveHead != null)
			{
				Vector2 local = Game1.GlobalToLocal(new Vector2((float)base.getStandingX(), (float)base.getStandingY() - (float)Game1.tileSize * 2f + (float)this.yJumpOffset));
				if (this.textAboveHeadStyle == 0)
				{
					local += new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2));
				}
				SpriteText.drawStringWithScrollCenteredAt(b, this.textAboveHead, (int)local.X, (int)local.Y, "", this.textAboveHeadAlpha, this.textAboveHeadColor, 1, (float)(base.getTileY() * Game1.tileSize) / 10000f + 0.001f + (float)base.getTileX() / 10000f, !this.sayingGoodbye);
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x001751B4 File Offset: 0x001733B4
		public void junimoReachedHut(Character c, GameLocation l)
		{
			this.fadeAway();
			this.controller = null;
			this.motion.X = 0f;
			this.motion.Y = -1f;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x001751E4 File Offset: 0x001733E4
		public override void update(GameTime time, GameLocation location)
		{
			base.update(time, location);
			this.forceUpdateTimer = 99999;
			if (this.eventActor)
			{
				return;
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
			}
			Junimo.soundTimer--;
			this.farmerCloseCheckTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.sayingGoodbye)
			{
				this.flip = false;
				if (this.whichArea % 2 == 0)
				{
					this.sprite.Animate(time, 16, 8, 50f);
				}
				else
				{
					this.sprite.Animate(time, 28, 4, 80f);
				}
				if (!this.isInvisible && Game1.random.NextDouble() < 0.0099999997764825821 && this.yJumpOffset == 0)
				{
					this.jump();
					if (Game1.random.NextDouble() < 0.15 && Game1.player.getTileX() == 29 && Game1.player.getTileY() == 11)
					{
						base.showTextAboveHead((Game1.random.NextDouble() < 0.5) ? "Bai!" : "Gud-Bai!", -1, 2, 3000, 0);
					}
				}
				return;
			}
			if (this.temporaryJunimo)
			{
				this.sprite.Animate(time, 12, 4, 100f);
				if (Game1.random.NextDouble() < 0.001)
				{
					this.jumpWithoutSound(8f);
					Game1.playSound("junimoMeep1");
				}
				return;
			}
			if (!this.isInvisible && this.farmerCloseCheckTimer <= 0 && this.controller == null && this.alpha >= 1f && !this.stayPut)
			{
				this.farmerCloseCheckTimer = 100;
				Farmer f = Utility.isThereAFarmerWithinDistance(base.getTileLocation(), 5);
				if (f != null)
				{
					if (this.friendly && Vector2.Distance(this.position, f.position) > (float)(this.speed * 4))
					{
						if (this.motion.Equals(Vector2.Zero) && Junimo.soundTimer <= 0)
						{
							this.jump();
							Game1.playSound("junimoMeep1");
							Junimo.soundTimer = 400;
						}
						if (Game1.random.NextDouble() < 0.007)
						{
							this.jumpWithoutSound((float)Game1.random.Next(6, 9));
						}
						this.setMoving(Utility.getVelocityTowardPlayer(new Point((int)this.position.X, (int)this.position.Y), (float)this.speed, f));
					}
					else if (!this.friendly)
					{
						this.fadeAway();
						Vector2 v = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox(), f);
						v.Normalize();
						v.Y *= -1f;
						this.setMoving(v * (float)this.speed);
					}
					else if (this.alpha >= 1f)
					{
						this.motion = Vector2.Zero;
					}
				}
				else if (this.alpha >= 1f)
				{
					this.motion = Vector2.Zero;
				}
			}
			if (!this.isInvisible && this.controller == null)
			{
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
			}
			if (this.controller == null && this.motion.Equals(Vector2.Zero))
			{
				this.sprite.Animate(time, 8, 4, 100f);
				return;
			}
			if (this.holdingStar || this.holdingBundle)
			{
				this.sprite.Animate(time, 44, 4, 200f);
				return;
			}
			if (this.moveRight || (Math.Abs(this.motion.X) > Math.Abs(this.motion.Y) && this.motion.X > 0f))
			{
				this.flip = false;
				this.sprite.Animate(time, 16, 8, 50f);
				return;
			}
			if (this.moveLeft || (Math.Abs(this.motion.X) > Math.Abs(this.motion.Y) && this.motion.X < 0f))
			{
				this.sprite.Animate(time, 16, 8, 50f);
				this.flip = true;
				return;
			}
			if (this.moveUp || (Math.Abs(this.motion.Y) > Math.Abs(this.motion.X) && this.motion.Y < 0f))
			{
				this.sprite.Animate(time, 32, 8, 50f);
				return;
			}
			this.sprite.Animate(time, 0, 8, 50f);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0017583C File Offset: 0x00173A3C
		public override void draw(SpriteBatch b, float alpha = 1f)
		{
			if (!this.isInvisible)
			{
				b.Draw(base.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)this.sprite.spriteHeight * 3f / 4f * (float)Game1.pixelZoom / (float)Math.Pow((double)(this.sprite.spriteHeight / 16), 2.0) + (float)this.yJumpOffset - (float)(Game1.pixelZoom * 2)) + ((this.shakeTimer > 0) ? new Vector2((float)Game1.random.Next(-1, 2), (float)Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(base.Sprite.SourceRect), this.color * this.alpha, this.rotation, new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.sprite.spriteHeight * Game1.pixelZoom) * 3f / 4f) / (float)Game1.pixelZoom, Math.Max(0.2f, this.scale) * (float)Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, this.drawOnTop ? 0.991f : ((float)base.getStandingY() / 10000f)));
				if (!this.swimming && !this.hideShadow)
				{
					b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float)(Game1.tileSize * 3) / 4f - (float)Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), this.color * this.alpha, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), ((float)Game1.pixelZoom + (float)this.yJumpOffset / 40f) * this.scale, SpriteEffects.None, Math.Max(0f, (float)base.getStandingY() / 10000f) - 1E-06f);
				}
				if (this.holdingStar)
				{
					b.Draw(this.sprite.Texture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.pixelZoom * 2), (float)(-(float)Game1.tileSize) * this.scale + (float)Game1.pixelZoom + (float)this.yJumpOffset)), new Rectangle?(new Rectangle(0, 109, 16, 19)), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom * this.scale, SpriteEffects.None, this.position.Y / 10000f + 0.0001f);
					return;
				}
				if (this.holdingBundle)
				{
					b.Draw(this.sprite.Texture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float)(Game1.pixelZoom * 2), (float)(-(float)Game1.tileSize) * this.scale + (float)(Game1.pixelZoom * 5) + (float)this.yJumpOffset)), new Rectangle?(new Rectangle(0, 96, 16, 13)), this.bundleColor * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom * this.scale, SpriteEffects.None, this.position.Y / 10000f + 0.0001f);
				}
			}
		}

		// Token: 0x040012EE RID: 4846
		private float alpha = 1f;

		// Token: 0x040012EF RID: 4847
		private float alphaChange;

		// Token: 0x040012F0 RID: 4848
		private int farmerCloseCheckTimer = 100;

		// Token: 0x040012F1 RID: 4849
		public int whichArea;

		// Token: 0x040012F2 RID: 4850
		public bool friendly;

		// Token: 0x040012F3 RID: 4851
		public bool holdingStar;

		// Token: 0x040012F4 RID: 4852
		public bool holdingBundle;

		// Token: 0x040012F5 RID: 4853
		public bool temporaryJunimo;

		// Token: 0x040012F6 RID: 4854
		public bool stayPut;

		// Token: 0x040012F7 RID: 4855
		public new bool eventActor;

		// Token: 0x040012F8 RID: 4856
		private Vector2 motion = Vector2.Zero;

		// Token: 0x040012F9 RID: 4857
		private new Rectangle nextPosition;

		// Token: 0x040012FA RID: 4858
		private Color color;

		// Token: 0x040012FB RID: 4859
		private Color bundleColor;

		// Token: 0x040012FC RID: 4860
		private static int soundTimer;

		// Token: 0x040012FD RID: 4861
		private bool sayingGoodbye;
	}
}
