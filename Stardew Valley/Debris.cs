using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Quests;
using StardewValley.Tools;

namespace StardewValley
{
	// Token: 0x02000022 RID: 34
	public class Debris
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00011354 File Offset: 0x0000F554
		public Debris()
		{
		}

		// Token: 0x17000019 RID: 25
		public List<Chunk> Chunks
		{
			// Token: 0x06000180 RID: 384 RVA: 0x000113A1 File Offset: 0x0000F5A1
			get
			{
				return this.chunks;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000113AC File Offset: 0x0000F5AC
		public Debris(int objectIndex, Vector2 debrisOrigin, Vector2 playerPosition) : this(objectIndex, 1, debrisOrigin, playerPosition)
		{
			string type;
			if (objectIndex <= 0)
			{
				type = "Crafting";
			}
			else
			{
				type = Game1.objectInformation[objectIndex].Split(new char[]
				{
					'/'
				})[3].Split(new char[]
				{
					' '
				})[0];
			}
			if (type.Equals("Arch"))
			{
				this.debrisType = Debris.DebrisType.ARCHAEOLOGY;
			}
			else
			{
				this.debrisType = Debris.DebrisType.OBJECT;
			}
			if (objectIndex == 92)
			{
				this.debrisType = Debris.DebrisType.RESOURCE;
			}
			if (Game1.player.speed >= 5 && !Game1.IsMultiplayer)
			{
				for (int i = 0; i < this.chunks.Count; i++)
				{
					this.chunks[i].xVelocity *= (float)((Game1.player.FacingDirection == 1 || Game1.player.FacingDirection == 3) ? 1 : 1);
				}
			}
			this.chunks[0].debrisType = objectIndex;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000114A4 File Offset: 0x0000F6A4
		public Debris(int number, Vector2 debrisOrigin, Color messageColor, float scale, Character toHover) : this(-1, 1, debrisOrigin, Game1.player.Position)
		{
			this.chunkType = number;
			this.debrisType = Debris.DebrisType.NUMBERS;
			this.nonSpriteChunkColor = messageColor;
			this.chunks[0].scale = scale;
			this.toHover = toHover;
			this.chunks[0].xVelocity = (float)Game1.random.Next(-1, 2);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00011514 File Offset: 0x0000F714
		public Debris(Item item, Vector2 debrisOrigin) : this(-2, 1, debrisOrigin, new Vector2((float)Game1.player.GetBoundingBox().Center.X, (float)Game1.player.GetBoundingBox().Center.Y))
		{
			this.item = item;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00011567 File Offset: 0x0000F767
		public Debris(Item item, Vector2 debrisOrigin, Vector2 targetLocation) : this(-2, 1, debrisOrigin, targetLocation)
		{
			this.item = item;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0001157C File Offset: 0x0000F77C
		public Debris(string message, int numberOfChunks, Vector2 debrisOrigin, Color messageColor, float scale, float rotation) : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
		{
			this.debrisType = Debris.DebrisType.LETTERS;
			this.debrisMessage = message;
			this.nonSpriteChunkColor = messageColor;
			this.chunks[0].rotation = rotation;
			this.chunks[0].scale = scale;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000115D8 File Offset: 0x0000F7D8
		public Debris(Texture2D spriteSheet, int numberOfChunks, Vector2 debrisOrigin) : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
		{
			this.debrisType = Debris.DebrisType.SPRITECHUNKS;
			this.spriteChunkSheet = spriteSheet;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].xSpriteSheet = Game1.random.Next(0, Game1.tileSize - 8);
				this.chunks[i].ySpriteSheet = Game1.random.Next(0, Game1.tileSize * 3 / 2 - 8);
				this.chunks[i].scale = 1f;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0001167C File Offset: 0x0000F87C
		public Debris(Texture2D spriteSheet, Rectangle sourceRect, int numberOfChunks, Vector2 debrisOrigin) : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
		{
			this.debrisType = Debris.DebrisType.SPRITECHUNKS;
			this.spriteChunkSheet = spriteSheet;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].xSpriteSheet = Game1.random.Next(sourceRect.X, sourceRect.X + sourceRect.Width - 4);
				this.chunks[i].ySpriteSheet = Game1.random.Next(sourceRect.Y, sourceRect.Y + sourceRect.Width - 4);
				this.chunks[i].scale = 1f;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0001173C File Offset: 0x0000F93C
		public Debris(Texture2D spriteSheet, Rectangle sourceRect, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, int groundLevel, int sizeOfSourceRectSquares) : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
		{
			this.sizeOfSourceRectSquares = sizeOfSourceRectSquares;
			this.debrisType = Debris.DebrisType.SPRITECHUNKS;
			this.spriteChunkSheet = spriteSheet;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].xSpriteSheet = Game1.random.Next(2) * sizeOfSourceRectSquares + sourceRect.X;
				this.chunks[i].ySpriteSheet = Game1.random.Next(2) * sizeOfSourceRectSquares + sourceRect.Y;
				this.chunks[i].rotationVelocity = ((Game1.random.NextDouble() < 0.5) ? ((float)(3.1415926535897931 / (double)Game1.random.Next(-32, -16))) : ((float)(3.1415926535897931 / (double)Game1.random.Next(16, 32))));
				this.chunks[i].xVelocity *= 1.2f;
				this.chunks[i].yVelocity *= 1.2f;
				this.chunks[i].scale = (float)Game1.pixelZoom;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00011888 File Offset: 0x0000FA88
		public Debris(Texture2D spriteSheet, Rectangle sourceRect, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, int groundLevel) : this(-1, numberOfChunks, debrisOrigin, playerPosition)
		{
			this.debrisType = Debris.DebrisType.SPRITECHUNKS;
			this.spriteChunkSheet = spriteSheet;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].xSpriteSheet = Game1.random.Next(sourceRect.X, sourceRect.X + sourceRect.Width - 4);
				this.chunks[i].ySpriteSheet = Game1.random.Next(sourceRect.Y, sourceRect.Y + sourceRect.Width - 4);
				this.chunks[i].scale = 1f;
			}
			this.chunkFinalYLevel = groundLevel;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00011948 File Offset: 0x0000FB48
		public Debris(int type, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, int groundLevel, int color = -1) : this(-1, numberOfChunks, debrisOrigin, playerPosition)
		{
			this.debrisType = Debris.DebrisType.CHUNKS;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				this.chunks[i].debrisType = type;
			}
			this.chunkType = type;
			this.chunksColor = this.getColorForDebris((color == -1) ? type : color);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000119AC File Offset: 0x0000FBAC
		public Color getColorForDebris(int type)
		{
			if (type == 12)
			{
				return new Color(170, 106, 46);
			}
			switch (type)
			{
			case 100001:
			case 100006:
				return Color.LightGreen;
			case 100002:
				return Color.LightBlue;
			case 100003:
				return Color.Red;
			case 100004:
				return Color.Yellow;
			case 100005:
				return Color.Black;
			case 100007:
				return Color.DimGray;
			default:
				return Color.White;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00011A20 File Offset: 0x0000FC20
		public Debris(int debrisType, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition) : this(debrisType, numberOfChunks, debrisOrigin, playerPosition, 1f)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00011A34 File Offset: 0x0000FC34
		public Debris(int debrisType, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, float velocityMultiplyer)
		{
			switch (debrisType)
			{
			case 0:
				break;
			case 1:
			case 3:
			case 5:
			case 7:
			case 9:
			case 11:
			case 13:
				goto IL_13D;
			case 2:
				goto IL_D4;
			case 4:
				goto IL_104;
			case 6:
				goto IL_E4;
			case 8:
				this.debrisType = Debris.DebrisType.CHUNKS;
				goto IL_144;
			case 10:
				goto IL_F4;
			case 12:
				goto IL_114;
			case 14:
				goto IL_124;
			default:
				switch (debrisType)
				{
				case 378:
					break;
				case 379:
				case 381:
				case 383:
				case 385:
				case 387:
				case 389:
					goto IL_13D;
				case 380:
					goto IL_D4;
				case 382:
					goto IL_104;
				case 384:
					goto IL_E4;
				case 386:
					goto IL_F4;
				case 388:
					goto IL_114;
				case 390:
					goto IL_124;
				default:
					goto IL_13D;
				}
				break;
			}
			debrisType = 378;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_D4:
			debrisType = 380;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_E4:
			debrisType = 384;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_F4:
			debrisType = 386;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_104:
			debrisType = 382;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_114:
			debrisType = 388;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_124:
			debrisType = 390;
			this.debrisType = Debris.DebrisType.RESOURCE;
			goto IL_144;
			IL_13D:
			this.debrisType = Debris.DebrisType.OBJECT;
			IL_144:
			if (debrisType != -1)
			{
				playerPosition -= (playerPosition - debrisOrigin) * 2f;
			}
			this.chunkType = debrisType;
			this.floppingFish = (Game1.objectInformation.ContainsKey(debrisType) && Game1.objectInformation[debrisType].Split(new char[]
			{
				'/'
			})[3].Contains("-4"));
			this.isFishable = (Game1.objectInformation.ContainsKey(debrisType) && Game1.objectInformation[debrisType].Split(new char[]
			{
				'/'
			})[3].Contains("Fish"));
			int minYVelocity;
			int maxYVelocity;
			int minXVelocity;
			int maxXVelocity;
			if (playerPosition.Y >= debrisOrigin.Y - (float)(Game1.tileSize / 2) && playerPosition.Y <= debrisOrigin.Y + (float)(Game1.tileSize / 2))
			{
				this.chunkFinalYLevel = (int)debrisOrigin.Y - Game1.tileSize / 2;
				minYVelocity = 220;
				maxYVelocity = 250;
				if (playerPosition.X < debrisOrigin.X)
				{
					minXVelocity = 20;
					maxXVelocity = 140;
				}
				else
				{
					minXVelocity = -140;
					maxXVelocity = -20;
				}
			}
			else if (playerPosition.Y < debrisOrigin.Y - (float)(Game1.tileSize / 2))
			{
				this.chunkFinalYLevel = (int)debrisOrigin.Y + (int)((float)(Game1.tileSize / 2) * velocityMultiplyer);
				minYVelocity = 150;
				maxYVelocity = 200;
				minXVelocity = -50;
				maxXVelocity = 50;
			}
			else
			{
				this.movingFinalYLevel = true;
				this.chunkFinalYLevel = (int)debrisOrigin.Y - 1;
				this.chunkFinalYTarget = (int)debrisOrigin.Y - (int)((float)(Game1.tileSize * 3 / 2) * velocityMultiplyer);
				this.movingUp = true;
				minYVelocity = 350;
				maxYVelocity = 400;
				minXVelocity = -50;
				maxXVelocity = 50;
			}
			debrisOrigin.X -= (float)(Game1.tileSize / 2);
			debrisOrigin.Y -= (float)(Game1.tileSize / 2);
			minXVelocity = (int)((float)minXVelocity * velocityMultiplyer);
			maxXVelocity = (int)((float)maxXVelocity * velocityMultiplyer);
			minYVelocity = (int)((float)minYVelocity * velocityMultiplyer);
			maxYVelocity = (int)((float)maxYVelocity * velocityMultiplyer);
			this.uniqueID = Game1.recentMultiplayerRandom.Next(-2147483648, 2147483647);
			for (int i = 0; i < numberOfChunks; i++)
			{
				this.chunks.Add(new Chunk(debrisOrigin, (float)Game1.recentMultiplayerRandom.Next(minXVelocity, maxXVelocity) / 40f, (float)Game1.recentMultiplayerRandom.Next(minYVelocity, maxYVelocity) / 40f, Game1.recentMultiplayerRandom.Next(debrisType, debrisType + 2)));
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00011DF8 File Offset: 0x0000FFF8
		public bool updateChunks(GameTime time)
		{
			this.timeSinceDoneBouncing += (float)time.ElapsedGameTime.Milliseconds;
			if (this.timeSinceDoneBouncing >= (this.floppingFish ? 2500f : ((this.debrisType == Debris.DebrisType.SPRITECHUNKS || this.debrisType == Debris.DebrisType.NUMBERS) ? 1800f : 600f)))
			{
				if (this.debrisType == Debris.DebrisType.LETTERS || this.debrisType == Debris.DebrisType.NUMBERS || this.debrisType == Debris.DebrisType.SQUARES || this.debrisType == Debris.DebrisType.SPRITECHUNKS || (this.debrisType == Debris.DebrisType.CHUNKS && this.chunks[0].debrisType - this.chunks[0].debrisType % 2 != 8))
				{
					return true;
				}
				if (this.debrisType == Debris.DebrisType.ARCHAEOLOGY || this.debrisType == Debris.DebrisType.OBJECT || this.debrisType == Debris.DebrisType.RESOURCE || this.debrisType == Debris.DebrisType.CHUNKS)
				{
					this.chunksMoveTowardPlayer = true;
				}
				this.timeSinceDoneBouncing = 0f;
			}
			for (int i = this.chunks.Count - 1; i >= 0; i--)
			{
				if (this.chunks[i].alpha > 0.1f && (this.debrisType == Debris.DebrisType.SPRITECHUNKS || this.debrisType == Debris.DebrisType.NUMBERS) && this.timeSinceDoneBouncing > 600f)
				{
					this.chunks[i].alpha = (1800f - this.timeSinceDoneBouncing) / 1000f;
				}
				if (this.chunks[i].position.X < (float)(-(float)Game1.tileSize * 2) || this.chunks[i].position.Y < (float)(-(float)Game1.tileSize) || this.chunks[i].position.X >= (float)(Game1.currentLocation.map.DisplayWidth + Game1.tileSize) || this.chunks[i].position.Y >= (float)(Game1.currentLocation.map.DisplayHeight + Game1.tileSize))
				{
					this.chunks.RemoveAt(i);
				}
				else
				{
					bool canMoveTowardPlayer = Math.Abs(this.chunks[i].position.X + (float)(Game1.tileSize / 2) - (float)Game1.player.getStandingX()) <= (float)Game1.player.MagneticRadius && Math.Abs(this.chunks[i].position.Y + (float)(Game1.tileSize / 2) - (float)Game1.player.getStandingY()) <= (float)Game1.player.MagneticRadius;
					if (canMoveTowardPlayer)
					{
						switch (this.debrisType)
						{
						case Debris.DebrisType.ARCHAEOLOGY:
						case Debris.DebrisType.OBJECT:
							if (this.item != null)
							{
								canMoveTowardPlayer = Game1.player.couldInventoryAcceptThisItem(this.item);
								goto IL_338;
							}
							canMoveTowardPlayer = Game1.player.couldInventoryAcceptThisObject(this.chunks[i].debrisType, 1, this.itemQuality);
							if (this.chunks[i].debrisType == 102 && Game1.activeClickableMenu != null)
							{
								canMoveTowardPlayer = false;
								goto IL_338;
							}
							goto IL_338;
						case Debris.DebrisType.RESOURCE:
							canMoveTowardPlayer = Game1.player.couldInventoryAcceptThisObject(this.chunks[i].debrisType - this.chunks[i].debrisType % 2, 1, 0);
							goto IL_338;
						}
						canMoveTowardPlayer = true;
					}
					IL_338:
					if ((this.chunksMoveTowardPlayer || this.isFishable) & canMoveTowardPlayer)
					{
						if (this.chunks[i].position.X < Game1.player.position.X - 12f)
						{
							this.chunks[i].xVelocity = Math.Min(this.chunks[i].xVelocity + 0.8f, 8f);
						}
						else if (this.chunks[i].position.X > Game1.player.position.X + 12f)
						{
							this.chunks[i].xVelocity = Math.Max(this.chunks[i].xVelocity - 0.8f, -8f);
						}
						if (this.chunks[i].position.Y + (float)(Game1.tileSize / 2) < (float)(Game1.player.getStandingY() - 12))
						{
							this.chunks[i].yVelocity = Math.Max(this.chunks[i].yVelocity - 0.8f, -8f);
						}
						else if (this.chunks[i].position.Y + (float)(Game1.tileSize / 2) > (float)(Game1.player.getStandingY() + 12))
						{
							this.chunks[i].yVelocity = Math.Min(this.chunks[i].yVelocity + 0.8f, 8f);
						}
						Chunk expr_4EA_cp_0_cp_0 = this.chunks[i];
						expr_4EA_cp_0_cp_0.position.X = expr_4EA_cp_0_cp_0.position.X + this.chunks[i].xVelocity;
						Chunk expr_515_cp_0_cp_0 = this.chunks[i];
						expr_515_cp_0_cp_0.position.Y = expr_515_cp_0_cp_0.position.Y - this.chunks[i].yVelocity;
						if (Math.Abs(this.chunks[i].position.X + (float)(Game1.tileSize / 2) - (float)Game1.player.getStandingX()) <= 64f && Math.Abs(this.chunks[i].position.Y + (float)(Game1.tileSize / 2) - (float)Game1.player.getStandingY()) <= 64f)
						{
							int switcher = (this.debrisType == Debris.DebrisType.ARCHAEOLOGY || this.debrisType == Debris.DebrisType.OBJECT) ? this.chunks[i].debrisType : (this.chunks[i].debrisType - this.chunks[i].debrisType % 2);
							if (this.debrisType == Debris.DebrisType.ARCHAEOLOGY)
							{
								Game1.farmerFindsArtifact(this.chunks[i].debrisType);
							}
							else if (this.item != null)
							{
								if (!Game1.player.addItemToInventoryBool(this.item, false))
								{
									goto IL_1077;
								}
							}
							else if (this.debrisType != Debris.DebrisType.CHUNKS || switcher != 8)
							{
								if (switcher <= -10000)
								{
									if (!Game1.player.addItemToInventoryBool(new MeleeWeapon(switcher), false))
									{
										goto IL_1077;
									}
								}
								else if (switcher <= 0)
								{
									if (!Game1.player.addItemToInventoryBool(new Object(Vector2.Zero, -switcher, false), false))
									{
										goto IL_1077;
									}
								}
								else
								{
									Farmer arg_6C2_0 = Game1.player;
									Object arg_6C2_1;
									if (switcher != 93 && switcher != 94)
									{
										(arg_6C2_1 = new Object(Vector2.Zero, switcher, 1)).quality = this.itemQuality;
									}
									else
									{
										arg_6C2_1 = new Torch(Vector2.Zero, 1, switcher);
									}
									if (!arg_6C2_0.addItemToInventoryBool(arg_6C2_1, false))
									{
										goto IL_1077;
									}
								}
							}
							if (Game1.debrisSoundInterval <= 0f)
							{
								Game1.debrisSoundInterval = 10f;
								Game1.playSound("coin");
							}
							if (Game1.IsMultiplayer)
							{
								MultiplayerUtility.broadcastDebrisPickup(this.uniqueID, Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
							}
							this.chunks.RemoveAt(i);
						}
					}
					else
					{
						if (this.debrisType == Debris.DebrisType.NUMBERS && this.toHover != null)
						{
							this.relativeXPosition += this.chunks[i].xVelocity;
							this.chunks[i].position.X = this.toHover.position.X + (float)(Game1.tileSize / 2) + this.relativeXPosition;
							this.chunks[i].scale = Math.Min(2f, Math.Max(1f, 0.9f + Math.Abs(this.chunks[i].position.Y - (float)this.chunkFinalYLevel) / ((float)Game1.tileSize * 2f)));
							this.chunkFinalYLevel = this.toHover.getStandingY() + 8;
							if (this.timeSinceDoneBouncing > 250f)
							{
								this.chunks[i].alpha = Math.Max(0f, this.chunks[i].alpha - 0.033f);
							}
							if (!this.toHover.Equals(Game1.player) && !this.nonSpriteChunkColor.Equals(Color.Yellow) && !this.nonSpriteChunkColor.Equals(Color.Green))
							{
								this.nonSpriteChunkColor.R = (byte)Math.Max((double)Math.Min(255, 200 + this.chunkType), Math.Min((double)Math.Min(255, 220 + this.chunkType), 400.0 * Math.Sin((double)this.timeSinceDoneBouncing / 804.247719318987 + 0.26179938779914941)));
								this.nonSpriteChunkColor.G = (byte)Math.Max((double)(150 - this.chunkType), Math.Min((double)(255 - this.chunkType), (this.nonSpriteChunkColor.R > 220) ? (300.0 * Math.Sin((double)this.timeSinceDoneBouncing / 804.247719318987 + 0.26179938779914941)) : 0.0));
								this.nonSpriteChunkColor.B = (byte)Math.Max(0, Math.Min(255, (int)((this.nonSpriteChunkColor.G > 200) ? (this.nonSpriteChunkColor.G - 20) : 0)));
							}
						}
						Chunk expr_9B4_cp_0_cp_0 = this.chunks[i];
						expr_9B4_cp_0_cp_0.position.X = expr_9B4_cp_0_cp_0.position.X + this.chunks[i].xVelocity;
						Chunk expr_9DF_cp_0_cp_0 = this.chunks[i];
						expr_9DF_cp_0_cp_0.position.Y = expr_9DF_cp_0_cp_0.position.Y - this.chunks[i].yVelocity;
						if (this.movingFinalYLevel)
						{
							this.chunkFinalYLevel -= (int)Math.Ceiling((double)(this.chunks[i].yVelocity / 2f));
							if (this.chunkFinalYLevel <= this.chunkFinalYTarget)
							{
								this.chunkFinalYLevel = this.chunkFinalYTarget;
								this.movingFinalYLevel = false;
							}
						}
						if (this.debrisType == Debris.DebrisType.SQUARES && this.chunks[i].position.Y < (float)(this.chunkFinalYLevel - Game1.tileSize * 3 / 2) && Game1.random.NextDouble() < 0.1)
						{
							this.chunks[i].position.Y = (float)(this.chunkFinalYLevel - Game1.random.Next(1, Game1.tileSize / 3));
							this.chunks[i].yVelocity = (float)Game1.random.Next(30, 80) / 40f;
							this.chunks[i].position.X = (float)Game1.random.Next((int)(this.chunks[i].position.X - this.chunks[i].position.X % (float)Game1.tileSize + 1f), (int)(this.chunks[i].position.X - this.chunks[i].position.X % (float)Game1.tileSize + 64f));
						}
						if (this.debrisType != Debris.DebrisType.SQUARES)
						{
							this.chunks[i].yVelocity -= 0.4f;
						}
						bool destroyThisChunk = false;
						if (this.chunks[i].position.Y >= (float)this.chunkFinalYLevel && this.chunks[i].hasPassedRestingLineOnce && this.chunks[i].bounces <= (this.floppingFish ? 65 : 2))
						{
							if (this.debrisType != Debris.DebrisType.LETTERS && this.debrisType != Debris.DebrisType.NUMBERS && this.debrisType != Debris.DebrisType.SPRITECHUNKS && (this.debrisType != Debris.DebrisType.CHUNKS || this.chunks[i].debrisType - this.chunks[i].debrisType % 2 == 8))
							{
								Game1.playSound("shiny4");
							}
							this.chunks[i].bounces++;
							if (this.floppingFish)
							{
								this.chunks[i].yVelocity = Math.Abs(this.chunks[i].yVelocity) * ((this.movingUp && this.chunks[i].bounces < 2) ? 0.6f : 0.9f);
								this.chunks[i].xVelocity = (float)Game1.random.Next(-250, 250) / 100f;
							}
							else
							{
								this.chunks[i].yVelocity = Math.Abs(this.chunks[i].yVelocity * 2f / 3f);
								this.chunks[i].rotationVelocity = ((Game1.random.NextDouble() < 0.5) ? (this.chunks[i].rotationVelocity / 2f) : (-this.chunks[i].rotationVelocity * 2f / 3f));
								this.chunks[i].xVelocity -= this.chunks[i].xVelocity / 2f;
							}
							if (this.debrisType != Debris.DebrisType.LETTERS && this.debrisType != Debris.DebrisType.SPRITECHUNKS && this.debrisType != Debris.DebrisType.NUMBERS && Game1.currentLocation.doesTileHaveProperty((int)((this.chunks[i].position.X + (float)(Game1.tileSize / 2)) / (float)Game1.tileSize), (int)((this.chunks[i].position.Y + (float)(Game1.tileSize / 2)) / (float)Game1.tileSize), "Water", "Back") != null)
							{
								Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 300f, 2, 1, this.chunks[i].position, false, false));
								Game1.playSound("dropItemInWater");
								destroyThisChunk = true;
							}
						}
						if ((!this.chunks[i].hitWall && Game1.currentLocation.Map.GetLayer("Buildings").Tiles[(int)((this.chunks[i].position.X + (float)(Game1.tileSize / 2)) / (float)Game1.tileSize), (int)((this.chunks[i].position.Y + (float)(Game1.tileSize / 2)) / (float)Game1.tileSize)] != null) || Game1.currentLocation.Map.GetLayer("Back").Tiles[(int)((this.chunks[i].position.X + (float)(Game1.tileSize / 2)) / (float)Game1.tileSize), (int)((this.chunks[i].position.Y + (float)(Game1.tileSize / 2)) / (float)Game1.tileSize)] == null)
						{
							this.chunks[i].xVelocity = -this.chunks[i].xVelocity;
							this.chunks[i].hitWall = true;
						}
						if (this.chunks[i].position.Y < (float)this.chunkFinalYLevel)
						{
							this.chunks[i].hasPassedRestingLineOnce = true;
						}
						if (this.chunks[i].bounces > (this.floppingFish ? 65 : 2))
						{
							this.chunks[i].yVelocity = 0f;
							this.chunks[i].xVelocity = 0f;
							this.chunks[i].rotationVelocity = 0f;
						}
						this.chunks[i].rotation += this.chunks[i].rotationVelocity;
						if (destroyThisChunk)
						{
							this.chunks.RemoveAt(i);
						}
					}
				}
				IL_1077:;
			}
			return this.chunks.Count == 0;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00012E98 File Offset: 0x00011098
		public static string getNameOfDebrisTypeFromIntId(int id)
		{
			switch (id)
			{
			case 0:
			case 1:
				return "Copper";
			case 2:
			case 3:
				return "Iron";
			case 4:
			case 5:
				return "Coal";
			case 6:
			case 7:
				return "Gold";
			case 8:
			case 9:
				return "Coins";
			case 10:
			case 11:
				return "Iridium";
			case 12:
			case 13:
				return "Wood";
			case 14:
			case 15:
				return "Stone";
			case 28:
			case 29:
				return "Fuel";
			case 30:
			case 31:
				return "Quartz";
			}
			return "???";
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00012F70 File Offset: 0x00011170
		public static bool getDebris(int which, int howMuch)
		{
			switch (which)
			{
			case 0:
				Game1.player.CopperPieces += howMuch;
				Game1.addHUDMessage(new HUDMessage("Copper", howMuch, true, Color.Sienna, null));
				if (howMuch > 0)
				{
					Game1.stats.CopperFound += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			case 1:
			case 3:
			case 5:
			case 7:
			case 9:
			case 11:
				break;
			case 2:
				Game1.player.IronPieces += howMuch;
				Game1.addHUDMessage(new HUDMessage("Iron", howMuch, true, Color.LightSlateGray, null));
				if (howMuch > 0)
				{
					Game1.stats.IronFound += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			case 4:
				Game1.player.CoalPieces += howMuch;
				Game1.addHUDMessage(new HUDMessage("Coal", howMuch, true, Color.DimGray, null));
				if (howMuch > 0)
				{
					Game1.stats.CoalFound += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			case 6:
				Game1.player.GoldPieces += howMuch;
				Game1.addHUDMessage(new HUDMessage("Gold", howMuch, true, Color.Gold, null));
				if (howMuch > 0)
				{
					Game1.stats.GoldFound += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			case 8:
			{
				int i = Game1.random.Next(10, 50) * howMuch;
				i -= i % 5;
				Game1.player.Money += i;
				if (howMuch > 0)
				{
					Game1.stats.CoinsFound += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			}
			case 10:
				Game1.player.IridiumPieces += howMuch;
				Game1.addHUDMessage(new HUDMessage("Iridium", howMuch, true, Color.Purple, null));
				if (howMuch > 0)
				{
					Game1.stats.IridiumFound += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			case 12:
				Game1.player.WoodPieces += howMuch;
				Game1.addHUDMessage(new HUDMessage("Wood", howMuch, true, Color.Tan, null));
				if (howMuch > 0)
				{
					Game1.stats.SticksChopped += (uint)howMuch;
					goto IL_256;
				}
				goto IL_256;
			default:
				if (which == 28)
				{
					Game1.player.fuelLantern(howMuch * 2);
					Game1.addHUDMessage(new HUDMessage("Fuel", howMuch * 2, true, Color.Goldenrod, null));
					goto IL_256;
				}
				break;
			}
			return false;
			IL_256:
			if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && !Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("ResourceCollectionQuest"))
			{
				((ResourceCollectionQuest)Game1.questOfTheDay).checkIfComplete(null, which, howMuch, null, null);
			}
			return true;
		}

		// Token: 0x0400019D RID: 413
		public const int copperDebris = 0;

		// Token: 0x0400019E RID: 414
		public const int ironDebris = 2;

		// Token: 0x0400019F RID: 415
		public const int coalDebris = 4;

		// Token: 0x040001A0 RID: 416
		public const int goldDebris = 6;

		// Token: 0x040001A1 RID: 417
		public const int coinsDebris = 8;

		// Token: 0x040001A2 RID: 418
		public const int iridiumDebris = 10;

		// Token: 0x040001A3 RID: 419
		public const int woodDebris = 12;

		// Token: 0x040001A4 RID: 420
		public const int stoneDebris = 14;

		// Token: 0x040001A5 RID: 421
		public const int fuelDebris = 28;

		// Token: 0x040001A6 RID: 422
		public const int quartzDebris = 30;

		// Token: 0x040001A7 RID: 423
		public const int bigStoneDebris = 32;

		// Token: 0x040001A8 RID: 424
		public const int bigWoodDebris = 34;

		// Token: 0x040001A9 RID: 425
		public const int timesToBounce = 2;

		// Token: 0x040001AA RID: 426
		public const int minMoneyPerCoin = 10;

		// Token: 0x040001AB RID: 427
		public const int maxMoneyPerCoin = 40;

		// Token: 0x040001AC RID: 428
		public const float gravity = 0.4f;

		// Token: 0x040001AD RID: 429
		public const float timeToWaitBeforeRemoval = 600f;

		// Token: 0x040001AE RID: 430
		public const int marginForChunkPickup = 64;

		// Token: 0x040001AF RID: 431
		public const int white = 10000;

		// Token: 0x040001B0 RID: 432
		public const int green = 100001;

		// Token: 0x040001B1 RID: 433
		public const int blue = 100002;

		// Token: 0x040001B2 RID: 434
		public const int red = 100003;

		// Token: 0x040001B3 RID: 435
		public const int yellow = 100004;

		// Token: 0x040001B4 RID: 436
		public const int black = 100005;

		// Token: 0x040001B5 RID: 437
		public const int charcoal = 100007;

		// Token: 0x040001B6 RID: 438
		public const int gray = 100006;

		// Token: 0x040001B7 RID: 439
		private List<Chunk> chunks = new List<Chunk>();

		// Token: 0x040001B8 RID: 440
		public int chunkType;

		// Token: 0x040001B9 RID: 441
		public int sizeOfSourceRectSquares = 8;

		// Token: 0x040001BA RID: 442
		public int itemQuality;

		// Token: 0x040001BB RID: 443
		public int chunkFinalYLevel;

		// Token: 0x040001BC RID: 444
		public int chunkFinalYTarget;

		// Token: 0x040001BD RID: 445
		public float timeSinceDoneBouncing;

		// Token: 0x040001BE RID: 446
		public float scale = 1f;

		// Token: 0x040001BF RID: 447
		private bool chunksMoveTowardPlayer;

		// Token: 0x040001C0 RID: 448
		private bool movingUp;

		// Token: 0x040001C1 RID: 449
		public bool itemDebris;

		// Token: 0x040001C2 RID: 450
		public bool floppingFish;

		// Token: 0x040001C3 RID: 451
		public bool isFishable;

		// Token: 0x040001C4 RID: 452
		public bool movingFinalYLevel;

		// Token: 0x040001C5 RID: 453
		public bool visible = true;

		// Token: 0x040001C6 RID: 454
		public Debris.DebrisType debrisType;

		// Token: 0x040001C7 RID: 455
		public string debrisMessage = "";

		// Token: 0x040001C8 RID: 456
		public Color nonSpriteChunkColor = Color.White;

		// Token: 0x040001C9 RID: 457
		public Color chunksColor;

		// Token: 0x040001CA RID: 458
		[XmlIgnore]
		public Texture2D spriteChunkSheet;

		// Token: 0x040001CB RID: 459
		public Item item;

		// Token: 0x040001CC RID: 460
		public int uniqueID;

		// Token: 0x040001CD RID: 461
		public Character toHover;

		// Token: 0x040001CE RID: 462
		private float relativeXPosition;

		// Token: 0x02000166 RID: 358
		public enum DebrisType
		{
			// Token: 0x04001438 RID: 5176
			CHUNKS,
			// Token: 0x04001439 RID: 5177
			LETTERS,
			// Token: 0x0400143A RID: 5178
			SQUARES,
			// Token: 0x0400143B RID: 5179
			ARCHAEOLOGY,
			// Token: 0x0400143C RID: 5180
			OBJECT,
			// Token: 0x0400143D RID: 5181
			SPRITECHUNKS,
			// Token: 0x0400143E RID: 5182
			RESOURCE,
			// Token: 0x0400143F RID: 5183
			NUMBERS
		}
	}
}
