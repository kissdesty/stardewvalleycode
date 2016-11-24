using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000009 RID: 9
	public class BarnDweller : Character
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00004771 File Offset: 0x00002971
		public BarnDweller()
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000477C File Offset: 0x0000297C
		public BarnDweller(string type, string name) : base(new AnimatedSprite(null, 0, Game1.tileSize, Game1.tileSize), new Vector2((float)(Game1.tileSize * Game1.random.Next(6, 16)), (float)(Game1.tileSize * Game1.random.Next(6, 13))), 2, name)
		{
			this.type = type;
			if (type == "WhiteBlackCow")
			{
				this.defaultProduceIndex = 184;
				this.sound = "cow";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyWhiteBlackCow"), 0);
				this.ageWhenMature = 1;
				this.daysToLay = 1;
				return;
			}
			if (type == "Pig")
			{
				this.defaultProduceIndex = 430;
				this.sound = "pig";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyPig"), 0);
				this.ageWhenMature = 1;
				this.daysToLay = 1;
				return;
			}
			if (type == "Goat")
			{
				this.defaultProduceIndex = 436;
				this.sound = "goat";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyGoat"), 0);
				this.ageWhenMature = 1;
				this.daysToLay = 2;
				return;
			}
			if (!(type == "Sheep"))
			{
				return;
			}
			this.defaultProduceIndex = 440;
			this.sound = "goat";
			this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabySheep"), 0);
			this.ageWhenMature = 1;
			this.daysToLay = 3;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004910 File Offset: 0x00002B10
		public BarnDweller(string type, int tileX, int tileY) : base(new AnimatedSprite(null, 0, Game1.tileSize, Game1.tileSize), new Vector2((float)(Game1.tileSize * tileX), (float)(Game1.tileSize * tileY)), 2, "Missingno")
		{
			if (type == "Cow")
			{
				this.sound = "cow";
				this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\Cow"), 0);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004984 File Offset: 0x00002B84
		public void reload()
		{
			string textureName = this.type;
			if (this.age < this.ageWhenMature)
			{
				textureName = "Baby" + this.type;
			}
			this.sprite = new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\" + textureName), 0);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000049D8 File Offset: 0x00002BD8
		public void farmerPushing()
		{
			this.pushAccumulator++;
			if (this.pushAccumulator > 40)
			{
				switch (Game1.player.facingDirection)
				{
				case 0:
					this.Halt();
					this.SetMovingUp(true);
					break;
				case 1:
					this.Halt();
					this.SetMovingRight(true);
					break;
				case 2:
					this.Halt();
					this.SetMovingDown(true);
					break;
				case 3:
					this.Halt();
					if (!Game1.currentLocation.isCollidingPosition(new Rectangle((int)this.position.X - Game1.tileSize, (int)this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, Game1.tileSize * 2 - Game1.tileSize / 4, Game1.tileSize / 2), Game1.viewport, this))
					{
						this.SetMovingLeft(true);
						if (this.facingDirection != 3)
						{
							this.position.X = this.position.X - (float)Game1.tileSize;
						}
					}
					else
					{
						this.SetMovingUp(true);
						this.faceDirection(0);
					}
					break;
				}
				this.faceDirection(Game1.player.facingDirection);
				this.sprite.UpdateSourceRect();
				this.pushAccumulator = 0;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004B18 File Offset: 0x00002D18
		public override Rectangle GetBoundingBox()
		{
			int width = (this.facingDirection == 3 || this.facingDirection == 1) ? (Game1.tileSize * 2 - Game1.tileSize / 4) : (Game1.tileSize * 3 / 4);
			return new Rectangle((int)this.position.X + Game1.tileSize / 8, (int)this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, width, Game1.tileSize / 2);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004B94 File Offset: 0x00002D94
		public void dayUpdate()
		{
			this.age++;
			this.daysSinceLastLay++;
			if (this.age == this.ageWhenMature)
			{
				this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\" + this.type);
			}
			if (!this.wasPet)
			{
				this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - (10 - this.friendshipTowardFarmer / 200));
				if (this.friendshipTowardFarmer <= 990)
				{
					this.daysToLay = 3;
				}
			}
			if (this.wasFed)
			{
				int arg_A8_0 = 0;
				int num = this.daysSinceLastFed;
				this.daysSinceLastFed = num - 1;
				this.daysSinceLastFed = Math.Max(arg_A8_0, num);
			}
			else
			{
				this.daysSinceLastFed++;
				this.hasProduce = false;
			}
			if (this.daysSinceLastFed <= 0 && this.daysSinceLastLay >= this.daysToLay && this.age >= this.ageWhenMature)
			{
				this.hasProduce = true;
				if (this.type.Equals("Pig") && Game1.random.NextDouble() < 0.75 - (double)Game1.player.LuckLevel * 0.015 - Game1.dailyLuck - (double)this.friendshipTowardFarmer / 10000.0)
				{
					this.hasProduce = false;
				}
				else if (this.type.Equals("Sheep"))
				{
					this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheep");
				}
			}
			this.wasFed = false;
			this.wasPet = false;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004D38 File Offset: 0x00002F38
		public Object getProduce()
		{
			if (this.hasProduce)
			{
				this.hasProduce = false;
				this.daysSinceLastLay = 0;
				if (this.type.Equals("Sheep"))
				{
					this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\ShearedSheep");
				}
				int num = this.defaultProduceIndex;
				if (num <= 430)
				{
					if (num != 184)
					{
						if (num == 430)
						{
							Stats expr_93 = Game1.stats;
							uint num2 = expr_93.TrufflesFound;
							expr_93.TrufflesFound = num2 + 1u;
						}
					}
					else
					{
						Stats expr_7D = Game1.stats;
						uint num2 = expr_7D.CowMilkProduced;
						expr_7D.CowMilkProduced = num2 + 1u;
					}
				}
				else if (num != 436)
				{
					if (num == 440)
					{
						Stats expr_BF = Game1.stats;
						uint num2 = expr_BF.SheepWoolProduced;
						expr_BF.SheepWoolProduced = num2 + 1u;
					}
				}
				else
				{
					Stats expr_A9 = Game1.stats;
					uint num2 = expr_A9.GoatMilkProduced;
					expr_A9.GoatMilkProduced = num2 + 1u;
				}
				int whichProduce = this.defaultProduceIndex;
				if (Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 10.0 && (whichProduce == 184 || whichProduce == 436))
				{
					whichProduce += 2;
				}
				return new Object(Vector2.Zero, whichProduce, null, false, false, false, false);
			}
			return null;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004E60 File Offset: 0x00003060
		public void pet()
		{
			if (Game1.timeOfDay >= 1900)
			{
				Game1.drawObjectDialogue(this.name + " is sleeping.");
				return;
			}
			if (!this.hasProduce)
			{
				this.Halt();
				this.sprite.StopAnimation();
				switch (Game1.player.FacingDirection)
				{
				case 0:
					this.sprite.currentFrame = 0;
					break;
				case 1:
					this.sprite.currentFrame = 12;
					break;
				case 2:
					this.sprite.currentFrame = 8;
					break;
				}
				this.sprite.UpdateSourceRect();
				if (!this.wasPet)
				{
					this.wasPet = true;
					this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 10);
					base.doEmote(20, true);
					return;
				}
				if (this.daysSinceLastFed == 0)
				{
					Game1.drawObjectDialogue(this.name + " is looking happy today!");
					return;
				}
				Game1.drawObjectDialogue(this.name + " seems to be in a bad mood...");
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004F64 File Offset: 0x00003164
		public void setRandomPosition()
		{
			string[] expr_24 = Game1.getLocationFromName("Barn").getMapProperty("ProduceArea").Split(new char[]
			{
				' '
			});
			int produceX = Convert.ToInt32(expr_24[0]);
			int produceY = Convert.ToInt32(expr_24[1]);
			int produceWidth = Convert.ToInt32(expr_24[2]);
			int produceHeight = Convert.ToInt32(expr_24[3]);
			this.position = new Vector2((float)(Game1.random.Next(produceX, produceX + produceWidth) * Game1.tileSize), (float)(Game1.random.Next(produceY, produceY + produceHeight) * Game1.tileSize));
			int tries = 0;
			while (this.doesIntersectAnotherBarnDweller())
			{
				this.faceDirection(Game1.random.Next(4));
				this.position = new Vector2((float)(Game1.random.Next(produceX, produceX + produceWidth) * Game1.tileSize), (float)(Game1.random.Next(produceY, produceY + produceHeight) * Game1.tileSize));
				tries++;
				if (tries > 5)
				{
					break;
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000504C File Offset: 0x0000324C
		public bool doesIntersectAnotherBarnDweller()
		{
			for (int i = 0; i < Game1.player.barnDwellers.Count; i++)
			{
				if (!Game1.player.barnDwellers[i].name.Equals(this.name) && this.GetBoundingBox().Intersects(Game1.player.barnDwellers[i].GetBoundingBox()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000050C0 File Offset: 0x000032C0
		public new void update(GameTime time, GameLocation location)
		{
			if (this.isEmoting)
			{
				base.updateEmote(time);
			}
			if (Game1.timeOfDay >= 1900)
			{
				this.facingDirection = 2;
				this.sprite.SourceRect = new Rectangle(Game1.tileSize * 4, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
				if (!this.isEmoting && Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
					return;
				}
			}
			else
			{
				if (Game1.random.NextDouble() < 0.002 && this.age >= this.ageWhenMature && !Game1.eventUp && (!Game1.currentLocation.name.Equals("Forest") || Game1.random.NextDouble() < 0.001))
				{
					Game1.playSound(this.sound);
				}
				if (Game1.random.NextDouble() < 0.007)
				{
					int newDirection = Game1.random.Next(5);
					if (newDirection != (this.facingDirection + 2) % 4)
					{
						if (newDirection < 4)
						{
							int oldDirection = this.facingDirection;
							this.faceDirection(newDirection);
							if (location.isCollidingPosition(this.nextPosition(newDirection), Game1.viewport, this))
							{
								this.faceDirection(oldDirection);
								return;
							}
						}
						switch (newDirection)
						{
						case 0:
							this.SetMovingUp(true);
							break;
						case 1:
							this.SetMovingRight(true);
							break;
						case 2:
							this.SetMovingDown(true);
							break;
						case 3:
							this.SetMovingLeft(true);
							break;
						default:
							this.Halt();
							this.sprite.StopAnimation();
							break;
						}
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
					}
				}
				if (this.isMoving() && Game1.random.NextDouble() < 0.014)
				{
					this.Halt();
					this.sprite.StopAnimation();
				}
				if (this.moveUp)
				{
					if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, this))
					{
						this.position.Y = this.position.Y - (float)this.speed;
						this.sprite.AnimateUp(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingDown(true);
						}
					}
					this.facingDirection = 0;
				}
				else if (this.moveRight)
				{
					if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, this))
					{
						this.position.X = this.position.X + (float)this.speed;
						this.sprite.AnimateRight(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingLeft(true);
						}
					}
					this.facingDirection = 1;
				}
				else if (this.moveDown)
				{
					if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, this))
					{
						this.position.Y = this.position.Y + (float)this.speed;
						this.sprite.AnimateDown(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingUp(true);
						}
					}
					this.facingDirection = 2;
				}
				else if (this.moveLeft)
				{
					if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, this))
					{
						this.position.X = this.position.X - (float)this.speed;
						this.sprite.AnimateLeft(time, 0, "");
					}
					else
					{
						this.Halt();
						this.sprite.StopAnimation();
						if (Game1.random.NextDouble() < 0.6)
						{
							this.SetMovingRight(true);
						}
					}
					this.facingDirection = 3;
				}
				this.sprite.UpdateSourceRect();
			}
		}

		// Token: 0x04000039 RID: 57
		public const double chancePerUpdateToChangeDirection = 0.007;

		// Token: 0x0400003A RID: 58
		public new const double chanceForSound = 0.002;

		// Token: 0x0400003B RID: 59
		public const int pushAccumulatorTimeTillPush = 40;

		// Token: 0x0400003C RID: 60
		public int daysToLay;

		// Token: 0x0400003D RID: 61
		public int daysSinceLastLay;

		// Token: 0x0400003E RID: 62
		public int defaultProduceIndex;

		// Token: 0x0400003F RID: 63
		public int friendshipTowardFarmer;

		// Token: 0x04000040 RID: 64
		public int daysSinceLastFed;

		// Token: 0x04000041 RID: 65
		public int pushAccumulator;

		// Token: 0x04000042 RID: 66
		public int age;

		// Token: 0x04000043 RID: 67
		public int ageWhenMature;

		// Token: 0x04000044 RID: 68
		public bool hasProduce;

		// Token: 0x04000045 RID: 69
		public bool wasPet;

		// Token: 0x04000046 RID: 70
		public bool wasFed;

		// Token: 0x04000047 RID: 71
		public string sound;

		// Token: 0x04000048 RID: 72
		public string type;
	}
}
