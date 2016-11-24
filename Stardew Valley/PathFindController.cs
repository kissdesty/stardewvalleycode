using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.Locations;

namespace StardewValley
{
	// Token: 0x0200003A RID: 58
	public class PathFindController
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x0003B408 File Offset: 0x00039608
		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, null, 10000, endPoint)
		{
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0003B434 File Offset: 0x00039634
		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, PathFindController.endBehavior endBehaviorFunction) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, null, 10000, endPoint)
		{
			this.endPoint = endPoint;
			this.endBehaviorFunction = endBehaviorFunction;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0003B470 File Offset: 0x00039670
		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, PathFindController.endBehavior endBehaviorFunction, int limit) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, null, limit, endPoint)
		{
			this.endPoint = endPoint;
			this.endBehaviorFunction = endBehaviorFunction;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0003B4A8 File Offset: 0x000396A8
		public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, bool eraseOldPathController) : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, eraseOldPathController, null, 10000, endPoint)
		{
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0003B4D4 File Offset: 0x000396D4
		public static bool isAtEndPoint(PathNode currentNode, Point endPoint, GameLocation location, Character c)
		{
			return currentNode.x == endPoint.X && currentNode.y == endPoint.Y;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0003B4F4 File Offset: 0x000396F4
		public PathFindController(Stack<Point> pathToEndPoint, GameLocation location, Character c, Point endPoint)
		{
			this.pathToEndPoint = pathToEndPoint;
			this.location = location;
			this.character = c;
			this.endPoint = endPoint;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0003B519 File Offset: 0x00039719
		public PathFindController(Stack<Point> pathToEndPoint, Character c, GameLocation l)
		{
			this.pathToEndPoint = pathToEndPoint;
			this.character = c;
			this.location = l;
			this.NPCSchedule = true;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0003B540 File Offset: 0x00039740
		public PathFindController(Character c, GameLocation location, PathFindController.isAtEnd endFunction, int finalFacingDirection, bool eraseOldPathController, PathFindController.endBehavior endBehaviorFunction, int limit, Point endPoint)
		{
			this.limit = limit;
			this.character = c;
			if (c is NPC && (c as NPC).CurrentDialogue.Count > 0 && (c as NPC).CurrentDialogue.Peek().removeOnNextMove)
			{
				(c as NPC).CurrentDialogue.Pop();
			}
			this.location = location;
			this.endFunction = ((endFunction == null) ? new PathFindController.isAtEnd(PathFindController.isAtEndPoint) : endFunction);
			this.endBehaviorFunction = endBehaviorFunction;
			if (endPoint == Point.Zero)
			{
				endPoint = new Point((int)c.getTileLocation().X, (int)c.getTileLocation().Y);
			}
			this.finalFacingDirection = finalFacingDirection;
			if (!(this.character is NPC) && !Game1.currentLocation.Name.Equals(location.Name) && endFunction == new PathFindController.isAtEnd(PathFindController.isAtEndPoint) && endPoint.X > 0 && endPoint.Y > 0)
			{
				this.character.position = new Vector2((float)(endPoint.X * Game1.tileSize), (float)(endPoint.Y * Game1.tileSize - Game1.tileSize / 2));
				return;
			}
			this.pathToEndPoint = PathFindController.findPath(new Point((int)c.getTileLocation().X, (int)c.getTileLocation().Y), endPoint, endFunction, location, this.character, limit);
			if (this.pathToEndPoint == null)
			{
				FarmHouse arg_177_0 = location as FarmHouse;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0003B6C8 File Offset: 0x000398C8
		public bool update(GameTime time)
		{
			if (this.pathToEndPoint == null || this.pathToEndPoint.Count == 0)
			{
				return true;
			}
			if (!this.NPCSchedule && !Game1.currentLocation.Name.Equals(this.location.Name) && this.endPoint.X > 0 && this.endPoint.Y > 0)
			{
				this.character.position = new Vector2((float)(this.endPoint.X * Game1.tileSize), (float)(this.endPoint.Y * Game1.tileSize - Game1.tileSize / 2));
				return true;
			}
			if (Game1.activeClickableMenu == null)
			{
				this.timerSinceLastCheckPoint += time.ElapsedGameTime.Milliseconds;
				Vector2 position = this.character.position;
				this.moveCharacter(time);
				if (this.character.position.Equals(position))
				{
					this.pausedTimer += time.ElapsedGameTime.Milliseconds;
				}
				else
				{
					this.pausedTimer = 0;
				}
				if (!this.NPCSchedule && this.pausedTimer > 5000)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0003B7F0 File Offset: 0x000399F0
		public static Stack<Point> findPath(Point startPoint, Point endPoint, PathFindController.isAtEnd endPointFunction, GameLocation location, Character character, int limit)
		{
			sbyte[,] directions = new sbyte[,]
			{
				{
					-1,
					0
				},
				{
					1,
					0
				},
				{
					0,
					1
				},
				{
					0,
					-1
				}
			};
			PriorityQueue openList = new PriorityQueue();
			Dictionary<PathNode, PathNode> closedList = new Dictionary<PathNode, PathNode>();
			int iterations = 0;
			openList.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
			while (!openList.IsEmpty())
			{
				PathNode currentNode = openList.Dequeue();
				if (endPointFunction(currentNode, endPoint, location, character))
				{
					return PathFindController.reconstructPath(currentNode, closedList);
				}
				if (!closedList.ContainsKey(currentNode))
				{
					closedList.Add(currentNode, currentNode.parent);
				}
				for (int i = 0; i < 4; i++)
				{
					PathNode neighbor = new PathNode(currentNode.x + (int)directions[i, 0], currentNode.y + (int)directions[i, 1], currentNode);
					neighbor.g = currentNode.g + 1;
					if (!closedList.ContainsKey(neighbor) && ((neighbor.x == endPoint.X && neighbor.y == endPoint.Y) || (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < location.map.Layers[0].LayerWidth && neighbor.y < location.map.Layers[0].LayerHeight)) && !location.isCollidingPosition(new Rectangle(neighbor.x * Game1.tileSize + 1, neighbor.y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2), Game1.viewport, false, 0, false, character, true, false, false))
					{
						int f = (int)neighbor.g + (Math.Abs(endPoint.X - neighbor.x) + Math.Abs(endPoint.Y - neighbor.y));
						if (!openList.Contains(neighbor, f))
						{
							openList.Enqueue(neighbor, f);
						}
					}
				}
				iterations++;
				if (iterations >= limit)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0003BA18 File Offset: 0x00039C18
		public static Stack<Point> reconstructPath(PathNode finalNode, Dictionary<PathNode, PathNode> closedList)
		{
			Stack<Point> path = new Stack<Point>();
			path.Push(new Point(finalNode.x, finalNode.y));
			for (PathNode parent = finalNode.parent; parent != null; parent = closedList[parent])
			{
				path.Push(new Point(parent.x, parent.y));
			}
			return path;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0003BA70 File Offset: 0x00039C70
		private byte[,] createMapGrid(GameLocation location, Point endPoint)
		{
			byte[,] mapGrid = new byte[location.map.Layers[0].LayerWidth, location.map.Layers[0].LayerHeight];
			for (int x = 0; x < location.map.Layers[0].LayerWidth; x++)
			{
				for (int y = 0; y < location.map.Layers[0].LayerHeight; y++)
				{
					if (!location.isCollidingPosition(new Rectangle(x * Game1.tileSize + 1, y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2), Game1.viewport, false, 0, false, this.character, true, false, false))
					{
						mapGrid[x, y] = (byte)(Math.Abs(endPoint.X - x) + Math.Abs(endPoint.Y - y));
					}
					else
					{
						mapGrid[x, y] = 255;
					}
				}
			}
			return mapGrid;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0003BB6C File Offset: 0x00039D6C
		private void moveCharacter(GameTime time)
		{
			Rectangle targetTile = new Rectangle(this.pathToEndPoint.Peek().X * Game1.tileSize, this.pathToEndPoint.Peek().Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			targetTile.Inflate(-2, 0);
			Rectangle bbox = this.character.GetBoundingBox();
			if ((targetTile.Contains(bbox) || (bbox.Width > targetTile.Width && targetTile.Contains(bbox.Center))) && targetTile.Bottom - bbox.Bottom >= 2)
			{
				this.timerSinceLastCheckPoint = 0;
				this.pathToEndPoint.Pop();
				this.character.stopWithoutChangingFrame();
				if (this.pathToEndPoint.Count == 0)
				{
					this.character.Halt();
					if (this.finalFacingDirection != -1)
					{
						this.character.faceDirection(this.finalFacingDirection);
					}
					if (this.NPCSchedule)
					{
						(this.character as NPC).DirectionsToNewLocation = null;
						(this.character as NPC).endOfRouteMessage = (this.character as NPC).nextEndOfRouteMessage;
					}
					if (this.endBehaviorFunction != null)
					{
						this.endBehaviorFunction(this.character, this.location);
						return;
					}
				}
			}
			else
			{
				if (this.character is Farmer)
				{
					(this.character as Farmer).movementDirections.Clear();
				}
				else
				{
					foreach (NPC c in this.location.characters)
					{
						if (!c.Equals(this.character) && c.GetBoundingBox().Intersects(bbox) && c.isMoving() && string.Compare(c.name, this.character.name) < 0)
						{
							this.character.Halt();
							return;
						}
					}
				}
				if (bbox.Left < targetTile.Left && bbox.Right < targetTile.Right)
				{
					this.character.SetMovingRight(true);
				}
				else if (bbox.Right > targetTile.Right && bbox.Left > targetTile.Left)
				{
					this.character.SetMovingLeft(true);
				}
				else if (bbox.Top <= targetTile.Top)
				{
					this.character.SetMovingDown(true);
				}
				else if (bbox.Bottom >= targetTile.Bottom - 2)
				{
					this.character.SetMovingUp(true);
				}
				this.character.MovePosition(time, Game1.viewport, this.location);
				if (this.NPCSchedule)
				{
					Warp w = this.location.isCollidingWithWarpOrDoor(this.character.nextPosition(this.character.getDirection()));
					if (w != null)
					{
						if (this.character is NPC && (this.character as NPC).isMarried() && (this.character as NPC).followSchedule)
						{
							NPC i = this.character as NPC;
							if (this.location is FarmHouse)
							{
								w = new Warp(w.X, w.Y, "BusStop", 0, 23, false);
							}
							if (this.location is BusStop && w.X <= 0)
							{
								w = new Warp(w.X, w.Y, i.getHome().name, (i.getHome() as FarmHouse).getEntryLocation().X, (i.getHome() as FarmHouse).getEntryLocation().Y, false);
							}
							if (i.temporaryController != null && i.controller != null)
							{
								i.controller.location = Game1.getLocationFromName(w.TargetName);
							}
						}
						Game1.warpCharacter(this.character as NPC, w.TargetName, new Vector2((float)w.TargetX, (float)w.TargetY), false, this.location.isOutdoors);
						this.location.characters.Remove(this.character as NPC);
						if (this.location.Equals(Game1.currentLocation) && Utility.isOnScreen(new Vector2((float)(w.X * Game1.tileSize), (float)(w.Y * Game1.tileSize)), Game1.tileSize * 6) && this.location.doors.ContainsKey(new Point(w.X, w.Y)))
						{
							Game1.playSound("doorClose");
						}
						this.location = Game1.getLocationFromName(w.TargetName);
						if (this.location.Equals(Game1.currentLocation) && Utility.isOnScreen(new Vector2((float)(w.TargetX * Game1.tileSize), (float)(w.TargetY * Game1.tileSize)), Game1.tileSize * 6) && this.location.doors.ContainsKey(new Point(w.TargetX, w.TargetY - 1)))
						{
							Game1.playSound("doorClose");
						}
						if (this.pathToEndPoint.Count > 0)
						{
							this.pathToEndPoint.Pop();
						}
						while (this.pathToEndPoint.Count > 0 && (Math.Abs(this.pathToEndPoint.Peek().X - this.character.getTileX()) > 1 || Math.Abs(this.pathToEndPoint.Peek().Y - this.character.getTileY()) > 1))
						{
							this.pathToEndPoint.Pop();
						}
					}
				}
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0003C128 File Offset: 0x0003A328
		public static Stack<Point> findPathForNPCSchedules(Point startPoint, Point endPoint, GameLocation location, int limit)
		{
			sbyte[,] directions = new sbyte[,]
			{
				{
					-1,
					0
				},
				{
					1,
					0
				},
				{
					0,
					1
				},
				{
					0,
					-1
				}
			};
			PriorityQueue openList = new PriorityQueue();
			Dictionary<PathNode, PathNode> closedList = new Dictionary<PathNode, PathNode>();
			int iterations = 0;
			openList.Enqueue(new PathNode(startPoint.X, startPoint.Y, 0, null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
			PathNode previousNode = (PathNode)openList.Peek();
			while (!openList.IsEmpty())
			{
				PathNode currentNode = openList.Dequeue();
				if (currentNode.x == endPoint.X && currentNode.y == endPoint.Y)
				{
					return PathFindController.reconstructPath(currentNode, closedList);
				}
				if (currentNode.x == 79)
				{
					int arg_B2_0 = currentNode.y;
				}
				if (!closedList.ContainsKey(currentNode))
				{
					closedList.Add(currentNode, currentNode.parent);
				}
				for (int i = 0; i < 4; i++)
				{
					PathNode neighbor = new PathNode(currentNode.x + (int)directions[i, 0], currentNode.y + (int)directions[i, 1], currentNode);
					neighbor.g = currentNode.g + 1;
					if (!closedList.ContainsKey(neighbor) && ((neighbor.x == endPoint.X && neighbor.y == endPoint.Y) || (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < location.map.Layers[0].LayerWidth && neighbor.y < location.map.Layers[0].LayerHeight && !PathFindController.isPositionImpassableForNPCSchedule(location, neighbor.x, neighbor.y))))
					{
						int f = (int)neighbor.g + PathFindController.getPreferenceValueForTerrainType(location, neighbor.x, neighbor.y) + (Math.Abs(endPoint.X - neighbor.x) + Math.Abs(endPoint.Y - neighbor.y) + (((neighbor.x == currentNode.x && neighbor.x == previousNode.x) || (neighbor.y == currentNode.y && neighbor.y == previousNode.y)) ? -2 : 0));
						if (!openList.Contains(neighbor, f))
						{
							openList.Enqueue(neighbor, f);
						}
					}
				}
				previousNode = currentNode;
				iterations++;
				if (iterations >= limit)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0003C3B4 File Offset: 0x0003A5B4
		private static bool isPositionImpassableForNPCSchedule(GameLocation l, int x, int y)
		{
			return (l.getTileIndexAt(x, y, "Buildings") != -1 && (l.doesTileHaveProperty(x, y, "Action", "Buildings") == null || (!l.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Door") && !l.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Passable")))) || l.isTerrainFeatureAt(x, y);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0003C430 File Offset: 0x0003A630
		private static int getPreferenceValueForTerrainType(GameLocation l, int x, int y)
		{
			string type = l.doesTileHaveProperty(x, y, "Type", "Back");
			if (type != null)
			{
				string a = type.ToLower();
				if (a == "stone")
				{
					return -7;
				}
				if (a == "wood")
				{
					return -4;
				}
				if (a == "dirt")
				{
					return -2;
				}
				if (a == "grass")
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x0400034B RID: 843
		public const byte impassable = 255;

		// Token: 0x0400034C RID: 844
		public const int timeToWaitBeforeCancelling = 5000;

		// Token: 0x0400034D RID: 845
		private Character character;

		// Token: 0x0400034E RID: 846
		public GameLocation location;

		// Token: 0x0400034F RID: 847
		public Stack<Point> pathToEndPoint;

		// Token: 0x04000350 RID: 848
		public Point endPoint;

		// Token: 0x04000351 RID: 849
		public int finalFacingDirection;

		// Token: 0x04000352 RID: 850
		public int pausedTimer;

		// Token: 0x04000353 RID: 851
		public int limit;

		// Token: 0x04000354 RID: 852
		private PathFindController.isAtEnd endFunction;

		// Token: 0x04000355 RID: 853
		public PathFindController.endBehavior endBehaviorFunction;

		// Token: 0x04000356 RID: 854
		public bool NPCSchedule;

		// Token: 0x04000357 RID: 855
		public int timerSinceLastCheckPoint;

		// Token: 0x02000169 RID: 361
		// Token: 0x06001361 RID: 4961
		public delegate bool isAtEnd(PathNode currentNode, Point endPoint, GameLocation location, Character c);

		// Token: 0x0200016A RID: 362
		// Token: 0x06001365 RID: 4965
		public delegate void endBehavior(Character c, GameLocation location);
	}
}
