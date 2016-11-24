using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using xTile.Dimensions;

namespace StardewValley.Characters
{
	// Token: 0x02000140 RID: 320
	public class Child : NPC
	{
		// Token: 0x06001208 RID: 4616 RVA: 0x0017103E File Offset: 0x0016F23E
		public Child()
		{
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00171048 File Offset: 0x0016F248
		public Child(string name, bool isMale, bool isDarkSkinned, Farmer parent)
		{
			this.age = 2;
			this.gender = (isMale ? 0 : 1);
			this.darkSkinned = isDarkSkinned;
			this.reloadSprite();
			this.name = name;
			base.DefaultMap = "FarmHouse";
			this.hideShadow = true;
			this.speed = 1;
			this.idOfParent = parent.uniqueMultiplayerID;
			this.breather = false;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x001710B0 File Offset: 0x0016F2B0
		public override void reloadSprite()
		{
			if (this.age >= 3)
			{
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Toddler" + ((this.gender == 0) ? "" : "_girl") + (this.darkSkinned ? "_dark" : "")), 0, 16, 32);
				this.hideShadow = false;
			}
			else
			{
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Baby" + (this.darkSkinned ? "_dark" : "")), 0, 22, (this.age == 1) ? 32 : 16);
				if (this.age == 1)
				{
					this.sprite.CurrentFrame = 4;
				}
				else if (this.age == 2)
				{
					this.sprite.CurrentFrame = 44;
				}
				this.hideShadow = true;
			}
			this.breather = false;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0017119C File Offset: 0x0016F39C
		public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (this.moveUp)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateUp(time, 0, "");
						this.facingDirection = 0;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveUp = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			else if (this.moveRight)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateRight(time, 0, "");
						this.facingDirection = 1;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveRight = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			else if (this.moveDown)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateDown(time, 0, "");
						this.facingDirection = 2;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveDown = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			else if (this.moveLeft)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					if (this.age == 3)
					{
						this.sprite.AnimateLeft(time, 0, "");
						this.facingDirection = 3;
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.moveLeft = false;
					this.sprite.CurrentFrame = ((this.sprite.currentAnimation != null) ? this.sprite.currentAnimation[0].frame : this.sprite.CurrentFrame);
					this.sprite.currentAnimation = null;
					if (Game1.IsMasterGame && this.age == 2 && Game1.timeOfDay < 1800)
					{
						this.setCrawlerInNewDirection();
					}
				}
			}
			if (this.blockedInterval >= 3000 && (float)this.blockedInterval <= 3750f && !Game1.eventUp)
			{
				base.doEmote((Game1.random.NextDouble() < 0.5) ? 8 : 40, true);
				this.blockedInterval = 3750;
				return;
			}
			if (this.blockedInterval >= 5000)
			{
				this.speed = 1;
				this.isCharging = true;
				this.blockedInterval = 0;
			}
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool canPassThroughActionTiles()
		{
			return false;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00171634 File Offset: 0x0016F834
		public override void dayUpdate(int dayOfMonth)
		{
			this.daysOld++;
			this.breather = false;
			if (this.daysOld == 13)
			{
				this.age = 1;
			}
			else if (this.daysOld == 27)
			{
				this.age = 2;
			}
			else if (this.daysOld == 55)
			{
				this.age = 3;
				this.hideShadow = false;
				this.speed = 4;
				this.reloadSprite();
			}
			if (this.age == 2)
			{
				this.hideShadow = true;
				this.speed = 1;
				Point p = (base.getHome() as FarmHouse).getRandomOpenPointInHouse(Game1.random, 0, 30);
				if (!p.Equals(Point.Zero))
				{
					base.setTilePosition(p);
				}
				else
				{
					this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
				}
				this.sprite.CurrentFrame = 32;
			}
			if (this.age == 3)
			{
				Point p2 = (base.getHome() as FarmHouse).getRandomOpenPointInHouse(Game1.random, 0, 30);
				if (!p2.Equals(Point.Zero))
				{
					base.setTilePosition(p2);
				}
				else
				{
					this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
				}
				this.sprite.spriteWidth = 16;
				this.sprite.spriteHeight = 32;
				this.sprite.CurrentFrame = 0;
				this.hideShadow = false;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x001717E4 File Offset: 0x0016F9E4
		public void toss(Farmer who)
		{
			who.forceTimePass = true;
			who.faceDirection(2);
			who.FarmerSprite.PauseForSingleAnimation = false;
			who.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
			{
				new FarmerSprite.AnimationFrame(57, 1500, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneTossing), true)
			});
			this.position = who.position + new Vector2((float)(-(float)Game1.pixelZoom * 4), (float)(-(float)Game1.tileSize * 3 / 2));
			this.yJumpVelocity = (float)Game1.random.Next(12, 19);
			this.yJumpOffset = -1;
			Game1.playSound("dwop");
			who.CanMove = false;
			who.freezePause = 1500;
			this.drawOnTop = true;
			this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(4, 100),
				new FarmerSprite.AnimationFrame(5, 100),
				new FarmerSprite.AnimationFrame(6, 100),
				new FarmerSprite.AnimationFrame(7, 100)
			});
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x001718F0 File Offset: 0x0016FAF0
		public void doneTossing(Farmer who)
		{
			who.forceTimePass = false;
			this.resetForPlayerEntry(who.currentLocation);
			who.CanMove = true;
			who.forceCanMove();
			who.faceDirection(0);
			this.drawOnTop = false;
			base.doEmote(20, true);
			if (!who.friendships.ContainsKey(this.name))
			{
				SerializableDictionary<string, int[]> arg_64_0 = who.friendships;
				string arg_64_1 = this.name;
				int[] expr_5C = new int[6];
				expr_5C[0] = 250;
				arg_64_0.Add(arg_64_1, expr_5C);
			}
			who.talkToFriend(this, 20);
			Game1.playSound("tinyWhip");
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0017197C File Offset: 0x0016FB7C
		public override Microsoft.Xna.Framework.Rectangle getMugShotSourceRect()
		{
			switch (this.age)
			{
			case 0:
				return new Microsoft.Xna.Framework.Rectangle(0, 0, 22, 16);
			case 1:
				return new Microsoft.Xna.Framework.Rectangle(0, 42, 22, 24);
			case 2:
				return new Microsoft.Xna.Framework.Rectangle(0, 112, 22, 16);
			case 3:
				return new Microsoft.Xna.Framework.Rectangle(0, 4, 16, 24);
			default:
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x001719E0 File Offset: 0x0016FBE0
		private void setCrawlerInNewDirection()
		{
			this.speed = 1;
			int state = Game1.random.Next(6);
			if (this.previousState >= 4 && Game1.random.NextDouble() < 0.6)
			{
				state = this.previousState;
			}
			if (state < 4)
			{
				while (state == this.previousState)
				{
					state = Game1.random.Next(6);
				}
			}
			else if (this.previousState >= 4)
			{
				state = this.previousState;
			}
			switch (state)
			{
			case 0:
				base.SetMovingOnlyUp();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(32, 160),
					new FarmerSprite.AnimationFrame(33, 160),
					new FarmerSprite.AnimationFrame(34, 160),
					new FarmerSprite.AnimationFrame(35, 160)
				});
				break;
			case 1:
				base.SetMovingOnlyRight();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(28, 160),
					new FarmerSprite.AnimationFrame(29, 160),
					new FarmerSprite.AnimationFrame(30, 160),
					new FarmerSprite.AnimationFrame(31, 160)
				});
				break;
			case 2:
				base.SetMovingOnlyDown();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(24, 160),
					new FarmerSprite.AnimationFrame(25, 160),
					new FarmerSprite.AnimationFrame(26, 160),
					new FarmerSprite.AnimationFrame(27, 160)
				});
				break;
			case 3:
				base.SetMovingOnlyLeft();
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(36, 160),
					new FarmerSprite.AnimationFrame(37, 160),
					new FarmerSprite.AnimationFrame(38, 160),
					new FarmerSprite.AnimationFrame(39, 160)
				});
				break;
			case 4:
				this.Halt();
				this.sprite.spriteHeight = 16;
				this.sprite.setCurrentAnimation(this.getRandomCrawlerAnimation(0));
				break;
			case 5:
				this.Halt();
				this.sprite.spriteHeight = 16;
				this.sprite.setCurrentAnimation(this.getRandomCrawlerAnimation(1));
				break;
			}
			this.previousState = state;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool hasSpecialCollisionRules()
		{
			return true;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00171C51 File Offset: 0x0016FE51
		public override bool isColliding(GameLocation l, Vector2 tile)
		{
			return !l.isTilePlaceable(tile, null);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00171C60 File Offset: 0x0016FE60
		public void tenMinuteUpdate()
		{
			if (!Game1.IsMasterGame || this.age != 2)
			{
				if (Game1.IsMasterGame && Game1.timeOfDay % 100 == 0 && this.age == 3 && Game1.timeOfDay < 1900)
				{
					base.IsWalkingInSquare = false;
					this.Halt();
					FarmHouse farmHouse = base.getHome() as FarmHouse;
					if (farmHouse.characters.Contains(this))
					{
						this.controller = new PathFindController(this, farmHouse, farmHouse.getRandomOpenPointInHouse(Game1.random, 0, 30), -1, new PathFindController.endBehavior(this.toddlerReachedDestination));
						if (this.controller.pathToEndPoint == null || !farmHouse.isTileOnMap(this.controller.pathToEndPoint.Last<Point>().X, this.controller.pathToEndPoint.Last<Point>().Y))
						{
							this.controller = null;
							return;
						}
					}
				}
				else if (Game1.IsMasterGame && this.age == 3 && Game1.timeOfDay == 1900)
				{
					base.IsWalkingInSquare = false;
					this.Halt();
					FarmHouse farmHouse2 = base.getHome() as FarmHouse;
					if (farmHouse2.characters.Contains(this))
					{
						this.controller = new PathFindController(this, farmHouse2, farmHouse2.getChildBed(this.gender), -1, new PathFindController.endBehavior(this.toddlerReachedDestination));
						if (this.controller.pathToEndPoint == null || !farmHouse2.isTileOnMap(this.controller.pathToEndPoint.Last<Point>().X, this.controller.pathToEndPoint.Last<Point>().Y))
						{
							this.controller = null;
						}
					}
				}
				return;
			}
			if (Game1.timeOfDay < 1800)
			{
				this.setCrawlerInNewDirection();
				return;
			}
			this.Halt();
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00171E20 File Offset: 0x00170020
		public void toddlerReachedDestination(Character c, GameLocation l)
		{
			if (Game1.random.NextDouble() < 0.8 && c.FacingDirection == 2)
			{
				List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
				animation.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(17, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(18, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(19, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(18, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(17, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(0, 1000, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(18, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(18, 300, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(0, 2000, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(17, 180, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(16, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(0, 800, 0, false, false, null, false, 0));
				this.sprite.setCurrentAnimation(animation);
				return;
			}
			if (Game1.random.NextDouble() < 0.8 && c.FacingDirection == 1)
			{
				List<FarmerSprite.AnimationFrame> animation2 = new List<FarmerSprite.AnimationFrame>();
				animation2.Add(new FarmerSprite.AnimationFrame(20, 120, 0, false, false, null, false, 0));
				animation2.Add(new FarmerSprite.AnimationFrame(21, 70, 0, false, false, null, false, 0));
				animation2.Add(new FarmerSprite.AnimationFrame(22, 70, 0, false, false, null, false, 0));
				animation2.Add(new FarmerSprite.AnimationFrame(23, 70, 0, false, false, null, false, 0));
				animation2.Add(new FarmerSprite.AnimationFrame(22, 999999, 0, false, false, null, false, 0));
				this.sprite.setCurrentAnimation(animation2);
				return;
			}
			if (Game1.random.NextDouble() < 0.8 && c.FacingDirection == 3)
			{
				List<FarmerSprite.AnimationFrame> animation3 = new List<FarmerSprite.AnimationFrame>();
				animation3.Add(new FarmerSprite.AnimationFrame(20, 120, 0, false, true, null, false, 0));
				animation3.Add(new FarmerSprite.AnimationFrame(21, 70, 0, false, true, null, false, 0));
				animation3.Add(new FarmerSprite.AnimationFrame(22, 70, 0, false, true, null, false, 0));
				animation3.Add(new FarmerSprite.AnimationFrame(23, 70, 0, false, true, null, false, 0));
				animation3.Add(new FarmerSprite.AnimationFrame(22, 999999, 0, false, true, null, false, 0));
				this.sprite.setCurrentAnimation(animation3);
				return;
			}
			if (c.FacingDirection == 0)
			{
				this.lastCrossroad = new Microsoft.Xna.Framework.Rectangle(base.getTileX() * Game1.tileSize, base.getTileY() * Game1.tileSize, Game1.tileSize, Game1.tileSize);
				this.squareMovementFacingPreference = -1;
				base.walkInSquare(4, 4, 2000);
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0017219D File Offset: 0x0017039D
		public bool isChildOf(Farmer who)
		{
			return who.uniqueMultiplayerID == this.idOfParent;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x001721B0 File Offset: 0x001703B0
		public override bool checkAction(Farmer who, GameLocation l)
		{
			if (!who.friendships.ContainsKey(this.name))
			{
				SerializableDictionary<string, int[]> arg_2D_0 = who.friendships;
				string arg_2D_1 = this.name;
				int[] expr_25 = new int[6];
				expr_25[0] = 250;
				arg_2D_0.Add(arg_2D_1, expr_25);
			}
			if (this.age >= 2 && !who.hasTalkedToFriendToday(this.name))
			{
				who.talkToFriend(this, 20);
				base.doEmote(20, true);
				if (this.age == 3)
				{
					base.faceTowardFarmerForPeriod(4000, 3, false, who);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00172234 File Offset: 0x00170434
		private List<FarmerSprite.AnimationFrame> getRandomCrawlerAnimation(int which = -1)
		{
			List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
			double d = Game1.random.NextDouble();
			if (which == 0 || d < 0.5)
			{
				animation.Add(new FarmerSprite.AnimationFrame(40, 500, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(43, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(40, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(43, 1900, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(42, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(40, 500, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(40, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(41, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(40, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(40, 1500, 0, false, false, null, false, 0));
			}
			else if (which == 1 || d >= 0.5)
			{
				animation.Add(new FarmerSprite.AnimationFrame(44, 1500, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(45, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(46, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(45, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(46, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(45, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(44, 200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(46, 200, 0, false, false, null, false, 0));
			}
			return animation;
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00172550 File Offset: 0x00170750
		private List<FarmerSprite.AnimationFrame> getRandomNewbornAnimation()
		{
			List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
			if (Game1.random.NextDouble() < 0.5)
			{
				animation.Add(new FarmerSprite.AnimationFrame(0, 400, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(1, 400, 0, false, false, null, false, 0));
			}
			else
			{
				animation.Add(new FarmerSprite.AnimationFrame(1, 3400, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(2, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(3, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(4, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(5, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(6, 4400, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(5, 3400, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(4, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(3, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(2, 100, 0, false, false, null, false, 0));
			}
			return animation;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00172680 File Offset: 0x00170880
		private List<FarmerSprite.AnimationFrame> getRandomBabyAnimation()
		{
			List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
			if (Game1.random.NextDouble() < 0.5)
			{
				animation.Add(new FarmerSprite.AnimationFrame(4, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(5, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(6, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(7, 120, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(4, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(5, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(6, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(7, 100, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(4, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(5, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(6, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(7, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(4, 2000, 0, false, false, null, false, 0));
				if (Game1.random.NextDouble() < 0.5)
				{
					animation.Add(new FarmerSprite.AnimationFrame(8, 1950, 0, false, false, null, false, 0));
					animation.Add(new FarmerSprite.AnimationFrame(9, 1200, 0, false, false, null, false, 0));
					animation.Add(new FarmerSprite.AnimationFrame(10, 180, 0, false, false, null, false, 0));
					animation.Add(new FarmerSprite.AnimationFrame(11, 1500, 0, false, false, null, false, 0));
					animation.Add(new FarmerSprite.AnimationFrame(8, 1500, 0, false, false, null, false, 0));
				}
			}
			else
			{
				animation.Add(new FarmerSprite.AnimationFrame(8, 250, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(9, 250, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(10, 250, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(11, 250, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(8, 1950, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(9, 1200, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(10, 180, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(11, 1500, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(8, 1500, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(9, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(10, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(11, 150, 0, false, false, null, false, 0));
				animation.Add(new FarmerSprite.AnimationFrame(8, 1500, 0, false, false, null, false, 0));
			}
			return animation;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00172986 File Offset: 0x00170B86
		public override void update(GameTime time, GameLocation location)
		{
			base.update(time, location);
			if (this.age >= 2)
			{
				this.MovePosition(time, Game1.viewport, location);
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x001729A8 File Offset: 0x00170BA8
		public void resetForPlayerEntry(GameLocation l)
		{
			if (this.age == 0)
			{
				this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
				if (Game1.timeOfDay >= 1800 && this.sprite != null)
				{
					this.sprite.StopAnimation();
					this.sprite.CurrentFrame = Game1.random.Next(7);
				}
				else if (this.sprite != null)
				{
					this.sprite.setCurrentAnimation(this.getRandomNewbornAnimation());
				}
			}
			else if (this.age == 1)
			{
				this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-3 * Game1.pixelZoom));
				if (Game1.timeOfDay >= 1800 && this.sprite != null)
				{
					this.sprite.StopAnimation();
					this.sprite.spriteHeight = 16;
					this.sprite.CurrentFrame = Game1.random.Next(7);
				}
				else if (this.sprite != null)
				{
					this.sprite.spriteHeight = 32;
					this.sprite.setCurrentAnimation(this.getRandomBabyAnimation());
				}
			}
			else if (this.age == 2)
			{
				if (this.sprite != null)
				{
					this.sprite.spriteHeight = 16;
				}
				if (Game1.timeOfDay >= 1800)
				{
					this.position = new Vector2(16f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2 + Game1.pixelZoom * 2));
					if (this.sprite != null)
					{
						this.sprite.StopAnimation();
						this.sprite.spriteHeight = 16;
						this.sprite.CurrentFrame = 7;
					}
				}
			}
			if (this.sprite != null)
			{
				this.sprite.loop = true;
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00172BC4 File Offset: 0x00170DC4
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (base.IsEmoting && !Game1.eventUp)
			{
				Vector2 emotePosition = base.getLocalPosition(Game1.viewport);
				emotePosition.Y -= (float)(Game1.tileSize / 2 + this.sprite.spriteHeight * Game1.pixelZoom - ((this.age == 1 || this.age == 3) ? Game1.tileSize : 0));
				emotePosition.X += (float)((this.age == 1) ? (2 * Game1.pixelZoom) : 0);
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(base.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)base.getStandingY() / 10000f);
			}
		}

		// Token: 0x040012DC RID: 4828
		public const int newborn = 0;

		// Token: 0x040012DD RID: 4829
		public const int baby = 1;

		// Token: 0x040012DE RID: 4830
		public const int crawler = 2;

		// Token: 0x040012DF RID: 4831
		public const int toddler = 3;

		// Token: 0x040012E0 RID: 4832
		public new int age;

		// Token: 0x040012E1 RID: 4833
		public int daysOld;

		// Token: 0x040012E2 RID: 4834
		public long idOfParent;

		// Token: 0x040012E3 RID: 4835
		public bool darkSkinned;

		// Token: 0x040012E4 RID: 4836
		private int previousState;
	}
}
