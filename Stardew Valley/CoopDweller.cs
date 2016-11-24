using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
	// Token: 0x02000020 RID: 32
	public class CoopDweller : Character
	{
		// Token: 0x0600016B RID: 363 RVA: 0x0000F9E9 File Offset: 0x0000DBE9
		public CoopDweller()
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
		public CoopDweller(string type, string name) : base(null, new Vector2((float)(Game1.tileSize * Game1.random.Next(2, 9)), (float)(Game1.tileSize * Game1.random.Next(5, 9))), 2, name)
		{
			this.type = type;
			if (type == "WhiteChicken")
			{
				this.daysToLay = 1;
				this.ageWhenMature = 1;
				this.defaultProduceIndex = 176;
				this.sound = "cluck";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyWhiteChicken"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (type == "BrownChicken")
			{
				this.daysToLay = 1;
				this.ageWhenMature = 1;
				this.defaultProduceIndex = 180;
				this.sound = "cluck";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyBrownChicken"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (type == "Duck")
			{
				this.daysToLay = 2;
				this.ageWhenMature = 1;
				this.defaultProduceIndex = 442;
				this.sound = "cluck";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyBrownChicken"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (type == "Rabbit")
			{
				this.daysToLay = 4;
				this.ageWhenMature = 3;
				this.defaultProduceIndex = 440;
				this.sound = "rabbit";
				this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyRabbit"), 0, Game1.tileSize, Game1.tileSize);
				return;
			}
			if (!(type == "Dinosaur"))
			{
				return;
			}
			this.daysToLay = 7;
			this.ageWhenMature = 0;
			this.defaultProduceIndex = 107;
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\Dinosaur"), 0, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
		public void reload()
		{
			string textureName = this.type;
			if (this.age < this.ageWhenMature)
			{
				textureName = "Baby" + this.type;
			}
			this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + textureName), 0, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000FC58 File Offset: 0x0000DE58
		public void pet()
		{
			if (Game1.timeOfDay >= 1900)
			{
				Game1.drawObjectDialogue(this.name + " is sleeping.");
				return;
			}
			this.Halt();
			this.sprite.StopAnimation();
			this.uniqueFrameAccumulator = -1;
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
			case 3:
				this.sprite.currentFrame = 4;
				break;
			}
			if (!this.wasPet)
			{
				this.wasPet = true;
				this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 10);
				base.doEmote(20, true);
				Game1.playSound(this.sound);
				return;
			}
			if (this.daysSinceLastFed == 0)
			{
				Game1.drawObjectDialogue(this.name + " is looking happy today!");
				return;
			}
			Game1.drawObjectDialogue(this.name + " seems to be in a bad mood... ");
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000FD6C File Offset: 0x0000DF6C
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
					this.SetMovingLeft(true);
					break;
				}
				this.pushAccumulator = 0;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
		public void setRandomPosition()
		{
			GameLocation coop = Game1.getLocationFromName("Coop");
			string[] expr_26 = coop.getMapProperty("ProduceArea").Split(new char[]
			{
				' '
			});
			int produceX = Convert.ToInt32(expr_26[0]);
			int produceY = Convert.ToInt32(expr_26[1]);
			int produceWidth = Convert.ToInt32(expr_26[2]);
			int produceHeight = Convert.ToInt32(expr_26[3]);
			this.position = new Vector2((float)Game1.random.Next(produceX, produceX + produceWidth), (float)Game1.random.Next(produceY, produceY + produceHeight));
			int tries = 0;
			while (coop.Objects.ContainsKey(this.position))
			{
				this.position = new Vector2((float)Game1.random.Next(produceX, produceX + produceWidth), (float)Game1.random.Next(produceY, produceY + produceHeight));
				tries++;
				if (tries > 2)
				{
					break;
				}
			}
			this.position.X = this.position.X * (float)Game1.tileSize;
			this.position.Y = this.position.Y * (float)Game1.tileSize;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public int dayUpdate()
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
			}
			this.wasPet = false;
			int whichProduce;
			if (!this.wasFed || this.age < this.ageWhenMature || this.daysSinceLastFed > 0 || this.daysSinceLastLay < this.daysToLay)
			{
				whichProduce = -1;
			}
			else
			{
				whichProduce = this.defaultProduceIndex;
				if (this.type.Equals("Duck") && Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.01)
				{
					whichProduce = 444;
				}
				else if (this.type.Equals("Rabbit") && Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 5000.0 + Game1.dailyLuck + (double)Game1.player.LuckLevel * 0.02)
				{
					whichProduce = 446;
				}
				this.daysSinceLastLay = 0;
				if (whichProduce <= 180)
				{
					if (whichProduce != 176)
					{
						if (whichProduce == 180)
						{
							Stats expr_1AD = Game1.stats;
							uint num = expr_1AD.ChickenEggsLayed;
							expr_1AD.ChickenEggsLayed = num + 1u;
						}
					}
					else
					{
						Stats expr_197 = Game1.stats;
						uint num = expr_197.ChickenEggsLayed;
						expr_197.ChickenEggsLayed = num + 1u;
					}
				}
				else if (whichProduce != 440)
				{
					if (whichProduce == 442)
					{
						Stats expr_1C3 = Game1.stats;
						uint num = expr_1C3.DuckEggsLayed;
						expr_1C3.DuckEggsLayed = num + 1u;
					}
				}
				else
				{
					Stats expr_1D9 = Game1.stats;
					uint num = expr_1D9.RabbitWoolProduced;
					expr_1D9.RabbitWoolProduced = num + 1u;
				}
				if (Game1.random.NextDouble() < (double)this.friendshipTowardFarmer / 1200.0)
				{
					if (whichProduce != 176)
					{
						if (whichProduce == 180)
						{
							whichProduce += 2;
						}
					}
					else
					{
						whichProduce -= 2;
					}
				}
			}
			if (!this.wasFed)
			{
				this.daysSinceLastFed++;
			}
			else
			{
				this.daysSinceLastFed = Math.Max(0, this.daysSinceLastFed - 1);
			}
			this.wasFed = false;
			return whichProduce;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00010154 File Offset: 0x0000E354
		public new void update(GameTime time, GameLocation location)
		{
			if (this.isEmoting)
			{
				base.updateEmote(time);
			}
			if (Game1.timeOfDay >= 1900)
			{
				this.sprite.currentFrame = 16;
				this.sprite.UpdateSourceRect();
				if (!this.isEmoting && Game1.random.NextDouble() < 0.002)
				{
					base.doEmote(24, true);
					return;
				}
			}
			else
			{
				if (Game1.random.NextDouble() < 0.002 && this.age >= this.ageWhenMature && this.sound != null)
				{
					Game1.playSound(this.sound);
				}
				if (Game1.random.NextDouble() < 0.007 && this.uniqueFrameAccumulator == -1)
				{
					int newDirection = Game1.random.Next(5);
					if (newDirection != (this.facingDirection + 2) % 4)
					{
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
				if (this.isMoving() && Game1.random.NextDouble() < 0.014 && this.uniqueFrameAccumulator == -1)
				{
					this.Halt();
					this.sprite.StopAnimation();
					if (Game1.random.NextDouble() < 0.75)
					{
						this.uniqueFrameAccumulator = 0;
						switch (this.facingDirection)
						{
						case 0:
							this.sprite.currentFrame = 20;
							break;
						case 1:
							this.sprite.currentFrame = 18;
							break;
						case 2:
							this.sprite.currentFrame = 16;
							break;
						case 3:
							this.sprite.currentFrame = 22;
							break;
						}
					}
				}
				if (this.uniqueFrameAccumulator != -1)
				{
					this.uniqueFrameAccumulator += time.ElapsedGameTime.Milliseconds;
					if (this.uniqueFrameAccumulator > 500)
					{
						this.sprite.CurrentFrame = this.sprite.CurrentFrame + 1 - this.sprite.CurrentFrame % 2 * 2;
						this.uniqueFrameAccumulator = 0;
						if (Game1.random.NextDouble() < 0.4)
						{
							this.uniqueFrameAccumulator = -1;
							return;
						}
					}
				}
				else
				{
					if (this.moveUp)
					{
						if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, false))
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
						return;
					}
					if (this.moveRight)
					{
						if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false))
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
						return;
					}
					if (this.moveDown)
					{
						if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, false))
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
						return;
					}
					if (this.moveLeft)
					{
						if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false))
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
				}
			}
		}

		// Token: 0x0400017E RID: 382
		public const double chancePerUpdateToChangeDirection = 0.007;

		// Token: 0x0400017F RID: 383
		public new const double chanceForSound = 0.002;

		// Token: 0x04000180 RID: 384
		public const int uniqueDownFrame = 16;

		// Token: 0x04000181 RID: 385
		public const int uniqueRightFrame = 18;

		// Token: 0x04000182 RID: 386
		public const int uniqueUpFrame = 20;

		// Token: 0x04000183 RID: 387
		public const int uniqueLeftFrame = 22;

		// Token: 0x04000184 RID: 388
		public const int pushAccumulatorTimeTillPush = 40;

		// Token: 0x04000185 RID: 389
		public const int timePerUniqueFrame = 500;

		// Token: 0x04000186 RID: 390
		public int daysToLay;

		// Token: 0x04000187 RID: 391
		public int daysSinceLastLay;

		// Token: 0x04000188 RID: 392
		public int defaultProduceIndex;

		// Token: 0x04000189 RID: 393
		public int friendshipTowardFarmer;

		// Token: 0x0400018A RID: 394
		public int daysSinceLastFed;

		// Token: 0x0400018B RID: 395
		public int pushAccumulator;

		// Token: 0x0400018C RID: 396
		public int uniqueFrameAccumulator = -1;

		// Token: 0x0400018D RID: 397
		public int age;

		// Token: 0x0400018E RID: 398
		public int ageWhenMature;

		// Token: 0x0400018F RID: 399
		public bool wasFed;

		// Token: 0x04000190 RID: 400
		public bool wasPet;

		// Token: 0x04000191 RID: 401
		public string sound;

		// Token: 0x04000192 RID: 402
		public string type;
	}
}
