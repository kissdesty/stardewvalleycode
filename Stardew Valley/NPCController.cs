using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000025 RID: 37
	public class NPCController
	{
		// Token: 0x1700001C RID: 28
		private int CurrentPathX
		{
			// Token: 0x060001DD RID: 477 RVA: 0x0002B21D File Offset: 0x0002941D
			get
			{
				if (this.pathIndex >= this.path.Count)
				{
					return 0;
				}
				return (int)this.path[this.pathIndex].X;
			}
		}

		// Token: 0x1700001D RID: 29
		private int CurrentPathY
		{
			// Token: 0x060001DE RID: 478 RVA: 0x0002B24B File Offset: 0x0002944B
			get
			{
				if (this.pathIndex >= this.path.Count)
				{
					return 0;
				}
				return (int)this.path[this.pathIndex].Y;
			}
		}

		// Token: 0x1700001E RID: 30
		private bool MovingHorizontally
		{
			// Token: 0x060001DF RID: 479 RVA: 0x0002B279 File Offset: 0x00029479
			get
			{
				return this.CurrentPathX != 0;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0002B284 File Offset: 0x00029484
		public NPCController(NPC n, List<Vector2> path, bool loop, NPCController.endBehavior endBehavior = null)
		{
			if (n == null)
			{
				return;
			}
			this.speed = n.speed;
			this.loop = loop;
			this.puppet = n;
			this.path = path;
			this.setMoving(true);
			this.behaviorAtEnd = endBehavior;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0002B2D4 File Offset: 0x000294D4
		private bool setMoving(bool newTarget)
		{
			if (this.puppet == null || this.pathIndex >= this.path.Count)
			{
				return false;
			}
			int facingDirection = 2;
			if (this.CurrentPathX > 0)
			{
				facingDirection = 1;
			}
			else if (this.CurrentPathX < 0)
			{
				facingDirection = 3;
			}
			else if (this.CurrentPathY < 0)
			{
				facingDirection = 0;
			}
			else if (this.CurrentPathY > 0)
			{
				facingDirection = 2;
			}
			this.puppet.Halt();
			this.puppet.faceDirection(facingDirection);
			if (this.CurrentPathX != 0 && this.CurrentPathY != 0)
			{
				this.pauseTime = this.CurrentPathY;
				facingDirection = this.CurrentPathX % 4;
				this.puppet.faceDirection(facingDirection);
				return true;
			}
			this.puppet.setMovingInFacingDirection();
			if (newTarget)
			{
				this.target = new Vector2(this.puppet.position.X + (float)(this.CurrentPathX * Game1.tileSize), this.puppet.Position.Y + (float)(this.CurrentPathY * Game1.tileSize));
			}
			return true;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0002B3D8 File Offset: 0x000295D8
		public bool update(GameTime time, GameLocation location, List<NPCController> allControllers)
		{
			this.puppet.speed = this.speed;
			bool reachedMeYet = false;
			foreach (NPCController i in allControllers)
			{
				if (i.puppet != null)
				{
					if (i.puppet.Equals(this.puppet))
					{
						reachedMeYet = true;
					}
					if (i.puppet.facingDirection == this.puppet.facingDirection && !i.puppet.Equals(this.puppet) && i.puppet.GetBoundingBox().Intersects(this.puppet.nextPosition(this.puppet.facingDirection)))
					{
						if (reachedMeYet)
						{
							break;
						}
						return false;
					}
				}
			}
			this.puppet.MovePosition(time, Game1.viewport, location);
			if (this.pauseTime < 0 && !this.puppet.isMoving())
			{
				this.setMoving(false);
			}
			if (this.pauseTime < 0 && Math.Abs(Vector2.Distance(this.puppet.position, this.target)) <= (float)this.puppet.Speed)
			{
				this.pathIndex++;
				if (!this.setMoving(true))
				{
					if (!this.loop)
					{
						if (this.behaviorAtEnd != null)
						{
							this.behaviorAtEnd();
						}
						return true;
					}
					this.pathIndex = 0;
					this.setMoving(true);
				}
			}
			else if (this.pauseTime >= 0)
			{
				this.pauseTime -= time.ElapsedGameTime.Milliseconds;
				if (this.pauseTime < 0)
				{
					this.pathIndex++;
					if (!this.setMoving(true))
					{
						if (!this.loop)
						{
							if (this.behaviorAtEnd != null)
							{
								this.behaviorAtEnd();
							}
							return true;
						}
						this.pathIndex = 0;
						this.setMoving(true);
					}
				}
			}
			return false;
		}

		// Token: 0x04000216 RID: 534
		public NPC puppet;

		// Token: 0x04000217 RID: 535
		private bool loop;

		// Token: 0x04000218 RID: 536
		private List<Vector2> path;

		// Token: 0x04000219 RID: 537
		private Vector2 target;

		// Token: 0x0400021A RID: 538
		private int pathIndex;

		// Token: 0x0400021B RID: 539
		private int pauseTime = -1;

		// Token: 0x0400021C RID: 540
		private int speed;

		// Token: 0x0400021D RID: 541
		private NPCController.endBehavior behaviorAtEnd;

		// Token: 0x02000167 RID: 359
		// Token: 0x0600135C RID: 4956
		public delegate void endBehavior();
	}
}
