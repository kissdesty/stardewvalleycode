using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000010 RID: 16
	public class Character
	{
		// Token: 0x17000007 RID: 7
		public Vector2 Position
		{
			// Token: 0x06000095 RID: 149 RVA: 0x000081B6 File Offset: 0x000063B6
			get
			{
				return this.position;
			}
			// Token: 0x06000096 RID: 150 RVA: 0x000081BE File Offset: 0x000063BE
			set
			{
				this.position = value;
			}
		}

		// Token: 0x17000008 RID: 8
		public int Speed
		{
			// Token: 0x06000097 RID: 151 RVA: 0x000081C7 File Offset: 0x000063C7
			get
			{
				return this.speed;
			}
			// Token: 0x06000098 RID: 152 RVA: 0x000081CF File Offset: 0x000063CF
			set
			{
				this.speed = value;
			}
		}

		// Token: 0x17000009 RID: 9
		public int FacingDirection
		{
			// Token: 0x06000099 RID: 153 RVA: 0x000081D8 File Offset: 0x000063D8
			get
			{
				return this.facingDirection;
			}
		}

		// Token: 0x1700000A RID: 10
		[XmlIgnore]
		public AnimatedSprite Sprite
		{
			// Token: 0x0600009A RID: 154 RVA: 0x000081E0 File Offset: 0x000063E0
			get
			{
				return this.sprite;
			}
			// Token: 0x0600009B RID: 155 RVA: 0x000081E8 File Offset: 0x000063E8
			set
			{
				this.sprite = value;
			}
		}

		// Token: 0x1700000B RID: 11
		public bool IsEmoting
		{
			// Token: 0x0600009C RID: 156 RVA: 0x000081F1 File Offset: 0x000063F1
			get
			{
				return this.isEmoting;
			}
			// Token: 0x0600009D RID: 157 RVA: 0x000081F9 File Offset: 0x000063F9
			set
			{
				this.isEmoting = value;
			}
		}

		// Token: 0x1700000C RID: 12
		public int CurrentEmote
		{
			// Token: 0x0600009E RID: 158 RVA: 0x00008202 File Offset: 0x00006402
			get
			{
				return this.currentEmote;
			}
			// Token: 0x0600009F RID: 159 RVA: 0x0000820A File Offset: 0x0000640A
			set
			{
				this.currentEmote = value;
			}
		}

		// Token: 0x1700000D RID: 13
		public int CurrentEmoteIndex
		{
			// Token: 0x060000A0 RID: 160 RVA: 0x00008213 File Offset: 0x00006413
			get
			{
				return this.currentEmoteFrame;
			}
		}

		// Token: 0x1700000E RID: 14
		public virtual bool IsMonster
		{
			// Token: 0x060000A1 RID: 161 RVA: 0x0000821B File Offset: 0x0000641B
			get
			{
				return false;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000821E File Offset: 0x0000641E
		public Character()
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000825C File Offset: 0x0000645C
		public Character(AnimatedSprite sprite, Vector2 position, int speed, string name)
		{
			this.sprite = sprite;
			this.position = position;
			this.speed = speed;
			this.name = name;
			if (sprite != null)
			{
				this.originalSourceRect = sprite.SourceRect;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000082D1 File Offset: 0x000064D1
		public virtual void SetMovingUp(bool b)
		{
			this.moveUp = b;
			if (!b)
			{
				this.Halt();
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000082E3 File Offset: 0x000064E3
		public virtual void SetMovingRight(bool b)
		{
			this.moveRight = b;
			if (!b)
			{
				this.Halt();
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000082F5 File Offset: 0x000064F5
		public virtual void SetMovingDown(bool b)
		{
			this.moveDown = b;
			if (!b)
			{
				this.Halt();
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00008307 File Offset: 0x00006507
		public virtual void SetMovingLeft(bool b)
		{
			this.moveLeft = b;
			if (!b)
			{
				this.Halt();
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000831C File Offset: 0x0000651C
		public void setMovingInFacingDirection()
		{
			switch (this.facingDirection)
			{
			case 0:
				this.SetMovingUp(true);
				return;
			case 1:
				this.SetMovingRight(true);
				return;
			case 2:
				this.SetMovingDown(true);
				return;
			case 3:
				this.SetMovingLeft(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00008366 File Offset: 0x00006566
		public int getFacingDirection()
		{
			if (this.sprite.CurrentFrame < 4)
			{
				return 2;
			}
			if (this.sprite.CurrentFrame < 8)
			{
				return 1;
			}
			if (this.sprite.CurrentFrame < 12)
			{
				return 0;
			}
			return 3;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000839A File Offset: 0x0000659A
		public void setTrajectory(int xVelocity, int yVelocity)
		{
			this.xVelocity = (float)xVelocity;
			this.yVelocity = (float)yVelocity;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000083AC File Offset: 0x000065AC
		public void setTrajectory(Vector2 trajectory)
		{
			this.xVelocity = trajectory.X;
			this.yVelocity = trajectory.Y;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000083C6 File Offset: 0x000065C6
		public virtual void Halt()
		{
			this.moveUp = false;
			this.moveDown = false;
			this.moveRight = false;
			this.moveLeft = false;
			this.sprite.StopAnimation();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000083F0 File Offset: 0x000065F0
		public void extendSourceRect(int horizontal, int vertical, bool ignoreSourceRectUpdates = true)
		{
			this.sprite.sourceRect.Inflate(Math.Abs(horizontal) / 2, Math.Abs(vertical) / 2);
			this.sprite.sourceRect.Offset(horizontal / 2, vertical / 2);
			Microsoft.Xna.Framework.Rectangle arg_3C_0 = this.originalSourceRect;
			if (this.sprite.SourceRect.Equals(this.originalSourceRect))
			{
				this.sprite.ignoreSourceRectUpdates = false;
				return;
			}
			this.sprite.ignoreSourceRectUpdates = ignoreSourceRectUpdates;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool collideWith(Object o)
		{
			return true;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008471 File Offset: 0x00006671
		public virtual void faceDirection(int direction)
		{
			if (direction != -3)
			{
				this.facingDirection = direction;
				if (this.sprite != null)
				{
					this.sprite.faceDirection(direction);
				}
				this.faceTowardFarmer = false;
				return;
			}
			this.faceTowardFarmer = true;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000084A2 File Offset: 0x000066A2
		public int getDirection()
		{
			if (this.moveUp)
			{
				return 0;
			}
			if (this.moveRight)
			{
				return 1;
			}
			if (this.moveDown)
			{
				return 2;
			}
			if (this.moveLeft)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000084D0 File Offset: 0x000066D0
		public void tryToMoveInDirection(int direction, bool isFarmer, int damagesFarmer, bool glider)
		{
			if (Game1.currentLocation.isCollidingPosition(this.nextPosition(direction), Game1.viewport, isFarmer, damagesFarmer, glider, this))
			{
				return;
			}
			switch (direction)
			{
			case 0:
				this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
				return;
			case 1:
				this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
				return;
			case 2:
				this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
				return;
			case 3:
				this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
				return;
			default:
				return;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000858C File Offset: 0x0000678C
		public virtual bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return this.controller == null && !this.IsMonster;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000085A4 File Offset: 0x000067A4
		public virtual void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
		{
			if (base.GetType() == typeof(FarmAnimal))
			{
				this.willDestroyObjectsUnderfoot = false;
			}
			if (this.xVelocity != 0f || this.yVelocity != 0f)
			{
				Microsoft.Xna.Framework.Rectangle nextPosition = this.GetBoundingBox();
				nextPosition.X += (int)this.xVelocity;
				nextPosition.Y -= (int)this.yVelocity;
				if (currentLocation == null || !currentLocation.isCollidingPosition(nextPosition, viewport, false, 0, false, this))
				{
					this.position.X = this.position.X + this.xVelocity;
					this.position.Y = this.position.Y - this.yVelocity;
				}
				this.xVelocity = (float)((int)(this.xVelocity - this.xVelocity / 2f));
				this.yVelocity = (float)((int)(this.yVelocity - this.yVelocity / 2f));
			}
			else if (this.moveUp)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateUp(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(0);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize - 1));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(0), true))
					{
						this.doEmote(12, true);
						this.position.Y = this.position.Y - (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (this.moveRight)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateRight(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(1);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize + 1), (float)(this.getStandingY() / Game1.tileSize));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(1), true))
					{
						this.doEmote(12, true);
						this.position.X = this.position.X + (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (this.moveDown)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateDown(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(2);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize + 1));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(2), true))
					{
						this.doEmote(12, true);
						this.position.Y = this.position.Y + (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			else if (this.moveLeft)
			{
				if (currentLocation == null || !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, 0, false, this) || this.isCharging)
				{
					this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					if (!this.ignoreMovementAnimation)
					{
						this.sprite.AnimateLeft(time, (this.speed - 2 + this.addedSpeed) * -25, Utility.isOnScreen(this.getTileLocationPoint(), 1, currentLocation) ? "Cowboy_Footstep" : "");
						this.faceDirection(3);
					}
				}
				else if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
				{
					this.Halt();
				}
				else if (this.willDestroyObjectsUnderfoot)
				{
					new Vector2((float)(this.getStandingX() / Game1.tileSize - 1), (float)(this.getStandingY() / Game1.tileSize));
					if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(3), true))
					{
						this.doEmote(12, true);
						this.position.X = this.position.X - (float)(this.speed + this.addedSpeed);
					}
					else
					{
						this.blockedInterval += time.ElapsedGameTime.Milliseconds;
					}
				}
			}
			if (this.blockedInterval >= 3000 && (float)this.blockedInterval <= 3750f && !Game1.eventUp)
			{
				this.doEmote((Game1.random.NextDouble() < 0.5) ? 8 : 40, true);
				this.blockedInterval = 3750;
				return;
			}
			if (this.blockedInterval >= 5000)
			{
				this.speed = 4;
				this.isCharging = true;
				this.blockedInterval = 0;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool canPassThroughActionTiles()
		{
			return false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008BF8 File Offset: 0x00006DF8
		public virtual Microsoft.Xna.Framework.Rectangle nextPosition(int direction)
		{
			Microsoft.Xna.Framework.Rectangle nextPosition = this.GetBoundingBox();
			switch (direction)
			{
			case 0:
				nextPosition.Y -= this.speed + this.addedSpeed;
				break;
			case 1:
				nextPosition.X += this.speed + this.addedSpeed;
				break;
			case 2:
				nextPosition.Y += this.speed + this.addedSpeed;
				break;
			case 3:
				nextPosition.X -= this.speed + this.addedSpeed;
				break;
			}
			return nextPosition;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00008C8C File Offset: 0x00006E8C
		public Location nextPositionPoint()
		{
			Location nextPositionTile = default(Location);
			switch (this.getDirection())
			{
			case 0:
				nextPositionTile = new Location(this.getStandingX(), this.getStandingY() - Game1.tileSize);
				break;
			case 1:
				nextPositionTile = new Location(this.getStandingX() + Game1.tileSize, this.getStandingY());
				break;
			case 2:
				nextPositionTile = new Location(this.getStandingX(), this.getStandingY() + Game1.tileSize);
				break;
			case 3:
				nextPositionTile = new Location(this.getStandingX() - Game1.tileSize, this.getStandingY());
				break;
			}
			return nextPositionTile;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00008D2B File Offset: 0x00006F2B
		public int getHorizontalMovement()
		{
			if (this.moveRight)
			{
				return this.speed + this.addedSpeed;
			}
			if (!this.moveLeft)
			{
				return 0;
			}
			return -this.speed - this.addedSpeed;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008D5B File Offset: 0x00006F5B
		public int getVerticalMovement()
		{
			if (this.moveDown)
			{
				return this.speed + this.addedSpeed;
			}
			if (!this.moveUp)
			{
				return 0;
			}
			return -this.speed - this.addedSpeed;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008D8B File Offset: 0x00006F8B
		public Vector2 nextPositionVector2()
		{
			return new Vector2((float)(this.getStandingX() + this.getHorizontalMovement()), (float)(this.getStandingY() + this.getVerticalMovement()));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008DB0 File Offset: 0x00006FB0
		public Location nextPositionTile()
		{
			Location nextPositionTile = this.nextPositionPoint();
			nextPositionTile.X /= Game1.tileSize;
			nextPositionTile.Y /= Game1.tileSize;
			return nextPositionTile;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public virtual void doEmote(int whichEmote, bool playSound, bool nextEventCommand = true)
		{
			if (!this.isEmoting && (!Game1.eventUp || this is Farmer || (Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.actors.Contains(this))))
			{
				this.isEmoting = true;
				this.currentEmote = whichEmote;
				this.currentEmoteFrame = 0;
				this.emoteInterval = 0f;
				this.nextEventcommandAfterEmote = nextEventCommand;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00008E56 File Offset: 0x00007056
		public void doEmote(int whichEmote, bool nextEventCommand = true)
		{
			this.doEmote(whichEmote, true, nextEventCommand);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00008E64 File Offset: 0x00007064
		public void updateEmote(GameTime time)
		{
			if (this.isEmoting)
			{
				this.emoteInterval += (float)time.ElapsedGameTime.Milliseconds;
				if (this.emoteFading && this.emoteInterval > 20f)
				{
					this.emoteInterval = 0f;
					this.currentEmoteFrame--;
					if (this.currentEmoteFrame < 0)
					{
						this.emoteFading = false;
						this.isEmoting = false;
						if (this.nextEventcommandAfterEmote && Game1.currentLocation.currentEvent != null && (Game1.currentLocation.currentEvent.actors.Contains(this) || this.name.Equals(Game1.player.Name)))
						{
							Event expr_CA = Game1.currentLocation.currentEvent;
							int currentCommand = expr_CA.CurrentCommand;
							expr_CA.CurrentCommand = currentCommand + 1;
							return;
						}
					}
				}
				else if (!this.emoteFading && this.emoteInterval > 20f && this.currentEmoteFrame <= 3)
				{
					this.emoteInterval = 0f;
					this.currentEmoteFrame++;
					if (this.currentEmoteFrame == 4)
					{
						this.currentEmoteFrame = this.currentEmote;
						return;
					}
				}
				else if (!this.emoteFading && this.emoteInterval > 250f)
				{
					this.emoteInterval = 0f;
					this.currentEmoteFrame++;
					if (this.currentEmoteFrame >= this.currentEmote + 4)
					{
						this.emoteFading = true;
						this.currentEmoteFrame = 3;
					}
				}
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00008FE4 File Offset: 0x000071E4
		public Vector2 GetGrabTile()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (this.facingDirection)
			{
			case 0:
				return new Vector2((float)((boundingBox.X + boundingBox.Width / 2) / Game1.tileSize), (float)((boundingBox.Y - 5) / Game1.tileSize));
			case 1:
				return new Vector2((float)((boundingBox.X + boundingBox.Width + 5) / Game1.tileSize), (float)((boundingBox.Y + boundingBox.Height / 2) / Game1.tileSize));
			case 2:
				return new Vector2((float)((boundingBox.X + boundingBox.Width / 2) / Game1.tileSize), (float)((boundingBox.Y + boundingBox.Height + 5) / Game1.tileSize));
			case 3:
				return new Vector2((float)((boundingBox.X - 5) / Game1.tileSize), (float)((boundingBox.Y + boundingBox.Height / 2) / Game1.tileSize));
			default:
				return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000090E8 File Offset: 0x000072E8
		public Vector2 GetDropLocation()
		{
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			switch (this.facingDirection)
			{
			case 0:
				return new Vector2((float)(boundingBox.X + Game1.tileSize / 4), (float)(boundingBox.Y - Game1.tileSize));
			case 1:
				return new Vector2((float)(boundingBox.X + boundingBox.Width + Game1.tileSize), (float)(boundingBox.Y + Game1.tileSize / 4));
			case 2:
				return new Vector2((float)(boundingBox.X + Game1.tileSize / 4), (float)(boundingBox.Y + boundingBox.Height + Game1.tileSize));
			case 3:
				return new Vector2((float)(boundingBox.X - Game1.tileSize), (float)(boundingBox.Y + Game1.tileSize / 4));
			default:
				return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000091C8 File Offset: 0x000073C8
		public List<Vector2> getAdjacentTiles()
		{
			List<Vector2> arg_1C_0 = new List<Vector2>();
			Vector2 tmp = this.getTileLocation();
			tmp.X += 1f;
			arg_1C_0.Add(tmp);
			tmp = this.getTileLocation();
			tmp.X -= 1f;
			arg_1C_0.Add(tmp);
			tmp = this.getTileLocation();
			tmp.Y -= 1f;
			arg_1C_0.Add(tmp);
			tmp = this.getTileLocation();
			tmp.Y += 1f;
			arg_1C_0.Add(tmp);
			return arg_1C_0;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00009254 File Offset: 0x00007454
		public virtual Vector2 GetToolLocation(bool ignoreClick = false)
		{
			if (Game1.mouseCursorTransparency == 0f || Game1.isAnyGamePadButtonBeingHeld())
			{
				ignoreClick = true;
			}
			if ((Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is WateringCan)) && (int)(this.lastClick.X / (float)Game1.tileSize) == Game1.player.getTileX() && (int)(this.lastClick.Y / (float)Game1.tileSize) == Game1.player.getTileY())
			{
				Microsoft.Xna.Framework.Rectangle bb = this.GetBoundingBox();
				switch (this.facingDirection)
				{
				case 0:
					return new Vector2((float)(bb.X + bb.Width / 2), (float)(bb.Y - Game1.tileSize));
				case 1:
					return new Vector2((float)(bb.X + bb.Width + Game1.tileSize), (float)(bb.Y + bb.Height / 2));
				case 2:
					return new Vector2((float)(bb.X + bb.Width / 2), (float)(bb.Y + bb.Height + Game1.tileSize));
				case 3:
					return new Vector2((float)(bb.X - Game1.tileSize), (float)(bb.Y + bb.Height / 2));
				}
			}
			if (!ignoreClick && !this.lastClick.Equals(Vector2.Zero) && this.name.Equals(Game1.player.name) && ((int)(this.lastClick.X / (float)Game1.tileSize) != Game1.player.getTileX() || (int)(this.lastClick.Y / (float)Game1.tileSize) != Game1.player.getTileY() || (Game1.player.CurrentTool != null && Game1.player.CurrentTool is WateringCan)) && Utility.distance(this.lastClick.X, (float)Game1.player.getStandingX(), this.lastClick.Y, (float)Game1.player.getStandingY()) <= (float)(Game1.tileSize * 2))
			{
				return this.lastClick;
			}
			Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
			if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Fishing Rod"))
			{
				switch (this.facingDirection)
				{
				case 0:
					return new Vector2((float)(boundingBox.X - Game1.tileSize / 4), (float)(boundingBox.Y - Game1.tileSize * 8 / 5));
				case 1:
					return new Vector2((float)(boundingBox.X + boundingBox.Width + Game1.tileSize), (float)boundingBox.Y);
				case 2:
					return new Vector2((float)(boundingBox.X - Game1.tileSize / 4), (float)(boundingBox.Y + boundingBox.Height + Game1.tileSize));
				case 3:
					return new Vector2((float)(boundingBox.X - Game1.tileSize * 7 / 4), (float)boundingBox.Y);
				}
			}
			else
			{
				switch (this.facingDirection)
				{
				case 0:
					return new Vector2((float)(boundingBox.X + boundingBox.Width / 2), (float)(boundingBox.Y - Game1.tileSize * 3 / 4));
				case 1:
					return new Vector2((float)(boundingBox.X + boundingBox.Width + Game1.tileSize * 3 / 4), (float)(boundingBox.Y + boundingBox.Height / 2));
				case 2:
					return new Vector2((float)(boundingBox.X + boundingBox.Width / 2), (float)(boundingBox.Y + boundingBox.Height + Game1.tileSize * 3 / 4));
				case 3:
					return new Vector2((float)(boundingBox.X - Game1.tileSize * 3 / 4), (float)(boundingBox.Y + boundingBox.Height / 2));
				}
			}
			return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00009634 File Offset: 0x00007834
		public void faceGeneralDirection(Vector2 target, int yBias = 0)
		{
			int playerX = this.getTileX();
			int playerY = this.getTileY();
			int xDif = (int)(target.X / (float)Game1.tileSize) - playerX;
			int yDif = (int)(target.Y / (float)Game1.tileSize) - playerY;
			if (xDif > Math.Abs(yDif) + yBias)
			{
				this.faceDirection(1);
				return;
			}
			if (Math.Abs(xDif) > Math.Abs(yDif) + yBias)
			{
				this.faceDirection(3);
				return;
			}
			if (yDif > 0)
			{
				this.faceDirection(2);
				return;
			}
			this.faceDirection(0);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000096AF File Offset: 0x000078AF
		public Vector2 getStandingPosition()
		{
			return new Vector2((float)this.getStandingX(), (float)this.getStandingY());
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000096C4 File Offset: 0x000078C4
		public virtual void draw(SpriteBatch b)
		{
			this.draw(b, 1f);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000096D4 File Offset: 0x000078D4
		public virtual void draw(SpriteBatch b, float alpha = 1f)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position), (float)this.GetBoundingBox().Center.Y / 10000f);
			if (this.IsEmoting)
			{
				Vector2 emotePosition = this.getLocalPosition(Game1.viewport);
				emotePosition.Y -= (float)(Game1.tileSize * 3 / 2);
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)this.getStandingY() / 10000f);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000097CC File Offset: 0x000079CC
		public virtual void draw(SpriteBatch b, int ySourceRectOffset, float alpha = 1f)
		{
			this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position) + new Vector2((float)(this.sprite.spriteWidth * Game1.pixelZoom / 2), (float)(this.GetBoundingBox().Height / 2)), (float)this.GetBoundingBox().Center.Y / 10000f, 0, ySourceRectOffset, Color.White, false, (float)Game1.pixelZoom, 0f, true);
			if (this.IsEmoting)
			{
				Vector2 emotePosition = this.getLocalPosition(Game1.viewport);
				emotePosition.Y -= (float)(Game1.tileSize * 3 / 2);
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White * alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)this.getStandingY() / 10000f);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00009904 File Offset: 0x00007B04
		public virtual Microsoft.Xna.Framework.Rectangle GetBoundingBox()
		{
			if (this.sprite == null)
			{
				return Microsoft.Xna.Framework.Rectangle.Empty;
			}
			return new Microsoft.Xna.Framework.Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y + Game1.tileSize / 4, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize / 2);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009968 File Offset: 0x00007B68
		public void stopWithoutChangingFrame()
		{
			this.moveDown = false;
			this.moveLeft = false;
			this.moveRight = false;
			this.moveUp = false;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void collisionWithFarmerBehavior()
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009988 File Offset: 0x00007B88
		public int getStandingX()
		{
			return this.GetBoundingBox().Center.X;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000099A8 File Offset: 0x00007BA8
		public int getStandingY()
		{
			return this.GetBoundingBox().Center.Y;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000099C8 File Offset: 0x00007BC8
		public Vector2 getLocalPosition(xTile.Dimensions.Rectangle viewport)
		{
			return new Vector2(this.position.X - (float)viewport.X, this.position.Y - (float)viewport.Y + (float)this.yJumpOffset) + this.drawOffset;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009A15 File Offset: 0x00007C15
		public virtual bool isMoving()
		{
			return this.moveUp || this.moveDown || this.moveRight || this.moveLeft;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009A37 File Offset: 0x00007C37
		public Point getTileLocationPoint()
		{
			return new Point(this.getStandingX() / Game1.tileSize, this.getStandingY() / Game1.tileSize);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009A58 File Offset: 0x00007C58
		public Point getLeftMostTileX()
		{
			return new Point(this.GetBoundingBox().X / Game1.tileSize, this.GetBoundingBox().Center.Y / Game1.tileSize);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009A94 File Offset: 0x00007C94
		public Point getRightMostTileX()
		{
			return new Point((this.GetBoundingBox().Right - 1) / Game1.tileSize, this.GetBoundingBox().Center.Y / Game1.tileSize);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009AD5 File Offset: 0x00007CD5
		public int getTileX()
		{
			return this.getStandingX() / Game1.tileSize;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009AE3 File Offset: 0x00007CE3
		public int getTileY()
		{
			return this.getStandingY() / Game1.tileSize;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009AF1 File Offset: 0x00007CF1
		public Vector2 getTileLocation()
		{
			return new Vector2((float)(this.getStandingX() / Game1.tileSize), (float)(this.getStandingY() / Game1.tileSize));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00009B12 File Offset: 0x00007D12
		public void startGlowing(Color glowingColor, bool border, float glowRate)
		{
			if (!this.glowingColor.Equals(glowingColor))
			{
				this.isGlowing = true;
				this.coloredBorder = border;
				this.glowingColor = glowingColor;
				this.glowUp = true;
				this.glowRate = glowRate;
				this.glowingTransparency = 0f;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00009B50 File Offset: 0x00007D50
		public void stopGlowing()
		{
			this.isGlowing = false;
			this.glowingColor = Color.White;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00009B64 File Offset: 0x00007D64
		public virtual void jumpWithoutSound(float velocity = 8f)
		{
			this.yJumpVelocity = velocity;
			this.yJumpOffset = -1;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00009B74 File Offset: 0x00007D74
		public virtual void jump()
		{
			this.yJumpVelocity = 8f;
			this.yJumpOffset = -1;
			Game1.playSound("dwop");
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00009B92 File Offset: 0x00007D92
		public virtual void jump(float jumpVelocity)
		{
			this.yJumpVelocity = jumpVelocity;
			this.yJumpOffset = -1;
			Game1.playSound("dwop");
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00009BAC File Offset: 0x00007DAC
		public void faceTowardFarmerForPeriod(int milliseconds, int radius, bool faceAway, Farmer who)
		{
			if ((this.sprite != null && this.sprite.currentAnimation == null) || this.isMoving())
			{
				if (this.isMoving())
				{
					milliseconds /= 2;
				}
				this.Halt();
				if (this.facingDirectionBeforeSpeakingToPlayer == -1)
				{
					this.facingDirectionBeforeSpeakingToPlayer = this.facingDirection;
				}
				this.faceTowardFarmerTimer = milliseconds;
				this.faceTowardFarmerRadius = radius;
				this.movementPause = milliseconds;
				this.faceAwayFromFarmer = faceAway;
				this.whoToFace = who;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00009C22 File Offset: 0x00007E22
		public virtual void update(GameTime time, GameLocation location)
		{
			this.update(time, location, 0L, true);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void performBehavior(byte which)
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00009C30 File Offset: 0x00007E30
		public virtual void update(GameTime time, GameLocation location, long id, bool move)
		{
			if (this.yJumpOffset != 0)
			{
				this.yJumpVelocity -= 0.5f;
				this.yJumpOffset -= (int)this.yJumpVelocity;
				if (this.yJumpOffset >= 0)
				{
					this.yJumpOffset = 0;
					this.yJumpVelocity = 0f;
					if (!this.IsMonster && (location == null || location.Equals(Game1.currentLocation)))
					{
						FarmerSprite.checkForFootstep(this);
					}
				}
			}
			if (this.faceTowardFarmerTimer > 0)
			{
				this.faceTowardFarmerTimer -= time.ElapsedGameTime.Milliseconds;
				if (!this.faceTowardFarmer && this.faceTowardFarmerTimer > 0 && Utility.tileWithinRadiusOfPlayer((int)this.getTileLocation().X, (int)this.getTileLocation().Y, this.faceTowardFarmerRadius, this.whoToFace))
				{
					this.faceTowardFarmer = true;
				}
				else if (!Utility.tileWithinRadiusOfPlayer((int)this.getTileLocation().X, (int)this.getTileLocation().Y, this.faceTowardFarmerRadius, this.whoToFace) || this.faceTowardFarmerTimer <= 0)
				{
					this.faceDirection(this.facingDirectionBeforeSpeakingToPlayer);
					if (this.faceTowardFarmerTimer <= 0)
					{
						this.facingDirectionBeforeSpeakingToPlayer = -1;
						this.faceTowardFarmer = false;
						this.faceAwayFromFarmer = false;
						this.faceTowardFarmerTimer = 0;
					}
				}
			}
			if (this.forceUpdateTimer > 0)
			{
				this.forceUpdateTimer -= time.ElapsedGameTime.Milliseconds;
			}
			this.updateGlow();
			this.updateEmote(time);
			if (!Game1.IsMultiplayer || Game1.IsServer || this.ignoreMultiplayerUpdates)
			{
				if (this.faceTowardFarmer && this.whoToFace != null)
				{
					this.faceGeneralDirection(this.whoToFace.getStandingPosition(), 0);
					if (this.faceAwayFromFarmer)
					{
						this.faceDirection((this.facingDirection + 2) % 4);
					}
				}
				if ((this.controller == null & move) && !this.freezeMotion)
				{
					this.updateMovement(location, time);
				}
				if (this.controller != null && !this.freezeMotion && this.controller.update(time))
				{
					this.controller = null;
				}
				if (Game1.IsServer && !Game1.isFestival() && Game1.random.NextDouble() < 0.2)
				{
					MultiplayerUtility.broadcastNPCMove((int)this.position.X, (int)this.position.Y, id, location);
					return;
				}
			}
			else if (!Game1.eventUp)
			{
				this.lerpPosition(this.positionToLerpTo);
				if (this.distanceFromLastServerPosition() >= 8f)
				{
					this.animateInFacingDirection(time);
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool hasSpecialCollisionRules()
		{
			return false;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool isColliding(GameLocation l, Vector2 tile)
		{
			return false;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00009EA6 File Offset: 0x000080A6
		public float distanceFromLastServerPosition()
		{
			return Vector2.DistanceSquared(this.position, this.positionToLerpTo);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00009EBC File Offset: 0x000080BC
		public virtual void animateInFacingDirection(GameTime time)
		{
			switch (this.FacingDirection)
			{
			case 0:
				this.Sprite.AnimateUp(time, 0, "");
				return;
			case 1:
				this.Sprite.AnimateRight(time, 0, "");
				return;
			case 2:
				this.Sprite.AnimateDown(time, 0, "");
				return;
			case 3:
				this.Sprite.AnimateLeft(time, 0, "");
				return;
			default:
				return;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00009F34 File Offset: 0x00008134
		public virtual void updatePositionFromServer(Vector2 newPosition)
		{
			this.positionToLerpTo = newPosition;
			int xDif = (int)(newPosition.X - this.position.X);
			int yDif = (int)(newPosition.Y - this.position.Y);
			if (xDif > Math.Abs(yDif))
			{
				this.faceDirection(1);
				return;
			}
			if (Math.Abs(xDif) > Math.Abs(yDif))
			{
				this.faceDirection(3);
				return;
			}
			if (yDif > 0)
			{
				this.faceDirection(2);
				return;
			}
			if (yDif < 0)
			{
				this.faceDirection(0);
				return;
			}
			if (this.sprite.CurrentFrame < 16)
			{
				this.Halt();
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00009FC4 File Offset: 0x000081C4
		public virtual void lerpPosition(Vector2 target)
		{
			if (target.Equals(Vector2.Zero))
			{
				return;
			}
			int difference = (int)(target.X - this.position.X);
			if (Math.Abs(difference) > Game1.tileSize * 4)
			{
				this.position.X = target.X;
			}
			else
			{
				this.position.X = this.position.X + (float)(difference * this.Speed) * 0.04f;
			}
			difference = (int)(target.Y - this.position.Y);
			if (Math.Abs(difference) > Game1.tileSize * 4)
			{
				this.position.Y = target.Y;
				return;
			}
			this.position.Y = this.position.Y + (float)(difference * this.Speed) * 0.04f;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void updateMovement(GameLocation location, GameTime time)
		{
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000A088 File Offset: 0x00008288
		public void updateGlow()
		{
			if (this.isGlowing)
			{
				if (this.glowUp)
				{
					this.glowingTransparency += this.glowRate;
					if (this.glowingTransparency >= 1f)
					{
						this.glowingTransparency = 1f;
						this.glowUp = false;
						return;
					}
				}
				else
				{
					this.glowingTransparency -= this.glowRate;
					if (this.glowingTransparency <= 0f)
					{
						this.glowingTransparency = 0f;
						this.glowUp = true;
					}
				}
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000A10C File Offset: 0x0000830C
		public void convertEventMotionCommandToMovement(Vector2 command)
		{
			if (command.X < 0f)
			{
				this.SetMovingLeft(true);
				return;
			}
			if (command.X > 0f)
			{
				this.SetMovingRight(true);
				return;
			}
			if (command.Y < 0f)
			{
				this.SetMovingUp(true);
				return;
			}
			if (command.Y > 0f)
			{
				this.SetMovingDown(true);
			}
		}

		// Token: 0x040000B8 RID: 184
		public const float emoteBeginInterval = 20f;

		// Token: 0x040000B9 RID: 185
		public const float emoteNormalInterval = 250f;

		// Token: 0x040000BA RID: 186
		public const int emptyCanEmote = 4;

		// Token: 0x040000BB RID: 187
		public const int questionMarkEmote = 8;

		// Token: 0x040000BC RID: 188
		public const int angryEmote = 12;

		// Token: 0x040000BD RID: 189
		public const int exclamationEmote = 16;

		// Token: 0x040000BE RID: 190
		public const int heartEmote = 20;

		// Token: 0x040000BF RID: 191
		public const int sleepEmote = 24;

		// Token: 0x040000C0 RID: 192
		public const int sadEmote = 28;

		// Token: 0x040000C1 RID: 193
		public const int happyEmote = 32;

		// Token: 0x040000C2 RID: 194
		public const int xEmote = 36;

		// Token: 0x040000C3 RID: 195
		public const int pauseEmote = 40;

		// Token: 0x040000C4 RID: 196
		public const int videoGameEmote = 52;

		// Token: 0x040000C5 RID: 197
		public const int musicNoteEmote = 56;

		// Token: 0x040000C6 RID: 198
		public const int blushEmote = 60;

		// Token: 0x040000C7 RID: 199
		public const int blockedIntervalBeforeEmote = 3000;

		// Token: 0x040000C8 RID: 200
		public const int blockedIntervalBeforeSprint = 5000;

		// Token: 0x040000C9 RID: 201
		public const double chanceForSound = 0.001;

		// Token: 0x040000CA RID: 202
		[XmlIgnore]
		public AnimatedSprite sprite;

		// Token: 0x040000CB RID: 203
		[XmlIgnore]
		public Vector2 position;

		// Token: 0x040000CC RID: 204
		[XmlIgnore]
		public int speed;

		// Token: 0x040000CD RID: 205
		[XmlIgnore]
		public int addedSpeed;

		// Token: 0x040000CE RID: 206
		[XmlIgnore]
		public int facingDirection = 2;

		// Token: 0x040000CF RID: 207
		[XmlIgnore]
		public int blockedInterval;

		// Token: 0x040000D0 RID: 208
		[XmlIgnore]
		public int faceTowardFarmerRadius;

		// Token: 0x040000D1 RID: 209
		[XmlIgnore]
		public int faceTowardFarmerTimer;

		// Token: 0x040000D2 RID: 210
		[XmlIgnore]
		public int forceUpdateTimer;

		// Token: 0x040000D3 RID: 211
		[XmlIgnore]
		public int movementPause;

		// Token: 0x040000D4 RID: 212
		public string name;

		// Token: 0x040000D5 RID: 213
		protected bool moveUp;

		// Token: 0x040000D6 RID: 214
		protected bool moveRight;

		// Token: 0x040000D7 RID: 215
		protected bool moveDown;

		// Token: 0x040000D8 RID: 216
		protected bool moveLeft;

		// Token: 0x040000D9 RID: 217
		protected bool freezeMotion;

		// Token: 0x040000DA RID: 218
		public bool isEmoting;

		// Token: 0x040000DB RID: 219
		public bool isCharging;

		// Token: 0x040000DC RID: 220
		public bool willDestroyObjectsUnderfoot = true;

		// Token: 0x040000DD RID: 221
		public bool isGlowing;

		// Token: 0x040000DE RID: 222
		public bool coloredBorder;

		// Token: 0x040000DF RID: 223
		public bool flip;

		// Token: 0x040000E0 RID: 224
		public bool drawOnTop;

		// Token: 0x040000E1 RID: 225
		public bool faceTowardFarmer;

		// Token: 0x040000E2 RID: 226
		public bool faceAwayFromFarmer;

		// Token: 0x040000E3 RID: 227
		public bool ignoreMovementAnimation;

		// Token: 0x040000E4 RID: 228
		protected int currentEmote;

		// Token: 0x040000E5 RID: 229
		protected int currentEmoteFrame;

		// Token: 0x040000E6 RID: 230
		protected int facingDirectionBeforeSpeakingToPlayer = -1;

		// Token: 0x040000E7 RID: 231
		[XmlIgnore]
		public float emoteInterval;

		// Token: 0x040000E8 RID: 232
		[XmlIgnore]
		public float xVelocity;

		// Token: 0x040000E9 RID: 233
		[XmlIgnore]
		public float yVelocity;

		// Token: 0x040000EA RID: 234
		[XmlIgnore]
		public Vector2 lastClick = Vector2.Zero;

		// Token: 0x040000EB RID: 235
		[XmlIgnore]
		public Vector2 positionToLerpTo;

		// Token: 0x040000EC RID: 236
		public float scale = 1f;

		// Token: 0x040000ED RID: 237
		public float timeBeforeAIMovementAgain;

		// Token: 0x040000EE RID: 238
		public float glowingTransparency;

		// Token: 0x040000EF RID: 239
		public float glowRate;

		// Token: 0x040000F0 RID: 240
		private bool glowUp;

		// Token: 0x040000F1 RID: 241
		[XmlIgnore]
		public bool nextEventcommandAfterEmote;

		// Token: 0x040000F2 RID: 242
		[XmlIgnore]
		public bool swimming;

		// Token: 0x040000F3 RID: 243
		[XmlIgnore]
		public bool collidesWithOtherCharacters;

		// Token: 0x040000F4 RID: 244
		[XmlIgnore]
		public bool farmerPassesThrough;

		// Token: 0x040000F5 RID: 245
		[XmlIgnore]
		public bool ignoreMultiplayerUpdates;

		// Token: 0x040000F6 RID: 246
		[XmlIgnore]
		public bool eventActor;

		// Token: 0x040000F7 RID: 247
		protected bool ignoreMovementAnimations;

		// Token: 0x040000F8 RID: 248
		[XmlIgnore]
		public int yJumpOffset;

		// Token: 0x040000F9 RID: 249
		[XmlIgnore]
		public int ySourceRectOffset;

		// Token: 0x040000FA RID: 250
		[XmlIgnore]
		public float yJumpVelocity;

		// Token: 0x040000FB RID: 251
		private Farmer whoToFace;

		// Token: 0x040000FC RID: 252
		[XmlIgnore]
		public Color glowingColor;

		// Token: 0x040000FD RID: 253
		[XmlIgnore]
		public PathFindController controller;

		// Token: 0x040000FE RID: 254
		private bool emoteFading;

		// Token: 0x040000FF RID: 255
		private Microsoft.Xna.Framework.Rectangle originalSourceRect;

		// Token: 0x04000100 RID: 256
		[XmlIgnore]
		public Vector2 drawOffset = Vector2.Zero;
	}
}
