using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000037 RID: 55
	public class Polygon
	{
		// Token: 0x17000028 RID: 40
		public List<Polygon.Line> Lines
		{
			// Token: 0x060002C8 RID: 712 RVA: 0x00039665 File Offset: 0x00037865
			get
			{
				return this.lines;
			}
			// Token: 0x060002C9 RID: 713 RVA: 0x0003966D File Offset: 0x0003786D
			set
			{
				this.lines = value;
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00039676 File Offset: 0x00037876
		public void addPoint(Vector2 point)
		{
			if (this.lines.Count > 0)
			{
				this.lines.Add(new Polygon.Line(this.Lines[this.Lines.Count - 1].End, point));
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000396B4 File Offset: 0x000378B4
		public bool containsPoint(Vector2 point)
		{
			using (List<Polygon.Line>.Enumerator enumerator = this.Lines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.End.Equals(point))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00039714 File Offset: 0x00037914
		public static Polygon getGentlerBorderForLakes(Rectangle room, Random mineRandom)
		{
			return Polygon.getGentlerBorderForLakes(room, mineRandom, Rectangle.Empty);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00039722 File Offset: 0x00037922
		public static Polygon getEdgeBorder(Rectangle room, Random mineRandom)
		{
			return Polygon.getEdgeBorder(room, mineRandom, new List<Rectangle>(), (room.Width - 2) / 2, (room.Height - 2) / 2);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00039744 File Offset: 0x00037944
		public static Polygon getEdgeBorder(Rectangle room, Random mineRandom, List<Rectangle> smoothZone)
		{
			return Polygon.getEdgeBorder(room, mineRandom, smoothZone, (room.Width - 2) / 2, (room.Height - 2) / 2);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00039764 File Offset: 0x00037964
		public static Polygon getEdgeBorder(Rectangle room, Random mineRandom, List<Rectangle> smoothZone, int horizontalInwardLimit, int verticalInwardLimit)
		{
			if (smoothZone == null)
			{
				smoothZone = new List<Rectangle>();
			}
			int lakeAreaWidth = room.Width - 2;
			int lakeAreaHeight = room.Height - 2;
			int lakeXPosition = room.X + 1;
			int lakeYPosition = room.Y + 1;
			new Rectangle(lakeXPosition, lakeYPosition, lakeAreaWidth, lakeAreaHeight);
			Polygon lake = new Polygon();
			Vector2 lastPosition = new Vector2((float)mineRandom.Next(lakeXPosition + 5, lakeXPosition + 8), (float)mineRandom.Next(lakeYPosition + 5, lakeYPosition + 8));
			lake.Lines.Add(new Polygon.Line(lastPosition, new Vector2(lastPosition.X + 1f, lastPosition.Y)));
			lastPosition.X += 1f;
			int topWidth = lakeAreaWidth - 12;
			List<int> lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			int i = 0;
			while (i < topWidth)
			{
				int whichWayToGo = mineRandom.Next(3);
				if (lastDirection.Last<int>() != whichWayToGo && lastDirection[lastDirection.Count - 2] != lastDirection.Last<int>())
				{
					whichWayToGo = lastDirection.Last<int>();
				}
				if (whichWayToGo == 0 && lastPosition.Y > (float)lakeYPosition && !lastDirection.Contains(1))
				{
					lastPosition.Y -= 1f;
					lastDirection.Add(0);
				}
				else if (whichWayToGo == 1 && lastPosition.Y < (float)(lakeYPosition + verticalInwardLimit) && !lastDirection.Contains(0))
				{
					lastPosition.Y += 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.X += 1f;
					i++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			int rightWidth = lakeAreaHeight - 4 - (int)(lastPosition.Y - (float)room.Y);
			lastPosition.Y += 1f;
			lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			lake.addPoint(lastPosition);
			int j = 0;
			while (j < rightWidth)
			{
				int whichWayToGo2 = mineRandom.Next(3);
				if (lastDirection.Last<int>() != whichWayToGo2 && lastDirection[lastDirection.Count - 2] != lastDirection.Last<int>())
				{
					whichWayToGo2 = lastDirection.Last<int>();
				}
				if (j > 4 && whichWayToGo2 == 0 && lastPosition.X < (float)(lakeXPosition + lakeAreaWidth - 1) && !lastDirection.Contains(1) && !Utility.pointInRectangles(smoothZone, (int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X += 1f;
					lastDirection.Add(0);
				}
				else if (j > 4 && whichWayToGo2 == 1 && lastPosition.X > (float)(lakeXPosition + lakeAreaWidth - horizontalInwardLimit + 1) && !lastDirection.Contains(0) && !Utility.pointInRectangles(smoothZone, (int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X -= 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.Y += 1f;
					j++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			int bottomWidth = (int)lastPosition.X - (int)lake.Lines[0].Start.X + 1;
			lastPosition.X -= 1f;
			lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			lake.addPoint(lastPosition);
			int k = 0;
			while (k < bottomWidth)
			{
				int whichWayToGo3 = mineRandom.Next(3);
				if (lastDirection.Last<int>() != whichWayToGo3 && lastDirection[lastDirection.Count - 2] != lastDirection.Last<int>())
				{
					whichWayToGo3 = lastDirection.Last<int>();
				}
				if (k > 4 && whichWayToGo3 == 0 && lastPosition.Y > (float)(lakeYPosition + lakeAreaHeight - verticalInwardLimit) && !lastDirection.Contains(1) && !lake.containsPoint(new Vector2(lastPosition.X, lastPosition.Y - 1f)))
				{
					lastPosition.Y -= 1f;
					lastDirection.Add(0);
				}
				else if (k > 4 && whichWayToGo3 == 1 && lastPosition.Y < (float)(lakeYPosition + lakeAreaHeight) && !lastDirection.Contains(0))
				{
					lastPosition.Y += 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.X -= 1f;
					k++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			int leftWidth = (int)lastPosition.Y - (int)lake.Lines[0].Start.Y - 1;
			lastPosition.Y -= 1f;
			lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			lake.addPoint(lastPosition);
			int l = 0;
			while (l < leftWidth)
			{
				int whichWayToGo4 = mineRandom.Next(3);
				if (lastDirection.Last<int>() != whichWayToGo4 && lastDirection[lastDirection.Count - 2] != lastDirection.Last<int>())
				{
					whichWayToGo4 = lastDirection.Last<int>();
				}
				if (l > 4 && whichWayToGo4 == 0 && lastPosition.X < (float)((int)lake.Lines[0].Start.X) && !lastDirection.Contains(1) && !lake.containsPoint(new Vector2(lastPosition.X + 1f, lastPosition.Y)) && !lake.containsPoint(new Vector2(lastPosition.X, lastPosition.Y - 1f)) && !Utility.pointInRectangles(smoothZone, (int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X += 1f;
					lastDirection.Add(0);
				}
				else if (l > 4 && whichWayToGo4 == 1 && lastPosition.X > (float)(lakeXPosition + 1) && !lastDirection.Contains(0) && !Utility.pointInRectangles(smoothZone, (int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X -= 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.Y -= 1f;
					l++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			if (lastPosition.X < (float)((int)lake.Lines[0].Start.X))
			{
				int leftover = (int)lake.Lines[0].Start.X + 1 - (int)lastPosition.X - 1;
				for (int m = 0; m < leftover; m++)
				{
					lastPosition.X += 1f;
					lake.addPoint(lastPosition);
				}
			}
			return lake;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00039E34 File Offset: 0x00038034
		public static Polygon getGentlerBorderForLakes(Rectangle room, Random mineRandom, Rectangle smoothZone)
		{
			int lakeAreaWidth = room.Width - 2;
			int lakeAreaHeight = room.Height - 2;
			int lakeXPosition = room.X + 1;
			int lakeYPosition = room.Y + 1;
			new Rectangle(lakeXPosition, lakeYPosition, lakeAreaWidth, lakeAreaHeight);
			Polygon lake = new Polygon();
			Vector2 lastPosition = new Vector2((float)mineRandom.Next(lakeXPosition + 5, lakeXPosition + 8), (float)mineRandom.Next(lakeYPosition + 5, lakeYPosition + 8));
			lake.Lines.Add(new Polygon.Line(lastPosition, new Vector2(lastPosition.X + 1f, lastPosition.Y)));
			lastPosition.X += 1f;
			int topWidth = lakeAreaWidth - 12;
			List<int> lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			int i = 0;
			while (i < topWidth)
			{
				int whichWayToGo = mineRandom.Next(3);
				if (whichWayToGo == 0 && lastPosition.Y > (float)lakeYPosition && !lastDirection.Contains(1) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.Y -= 1f;
					lastDirection.Add(0);
				}
				else if (whichWayToGo == 1 && lastPosition.Y < (float)(lakeYPosition + lakeAreaHeight / 2) && !lastDirection.Contains(0) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.Y += 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.X += 1f;
					i++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			int rightWidth = lakeAreaHeight - 4 - (int)(lastPosition.Y - (float)room.Y);
			lastPosition.Y += 1f;
			lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			lake.addPoint(lastPosition);
			int j = 0;
			while (j < rightWidth)
			{
				int whichWayToGo2 = mineRandom.Next(3);
				if (whichWayToGo2 == 0 && lastPosition.X < (float)(lakeXPosition + lakeAreaWidth) && !lastDirection.Contains(1) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X += 1f;
					lastDirection.Add(0);
				}
				else if (whichWayToGo2 == 1 && lastPosition.X > (float)(lakeXPosition + lakeAreaWidth / 2 + 1) && !lastDirection.Contains(0) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X -= 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.Y += 1f;
					j++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			int bottomWidth = (int)lastPosition.X - (int)lake.Lines[0].Start.X + 1;
			lastPosition.X -= 1f;
			lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			lake.addPoint(lastPosition);
			int k = 0;
			while (k < bottomWidth)
			{
				int whichWayToGo3 = mineRandom.Next(3);
				if (whichWayToGo3 == 0 && lastPosition.Y > (float)(lakeYPosition + lakeAreaHeight / 2) && !lastDirection.Contains(1) && !lake.containsPoint(new Vector2(lastPosition.X, lastPosition.Y - 1f)) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.Y -= 1f;
					lastDirection.Add(0);
				}
				else if (whichWayToGo3 == 1 && lastPosition.Y < (float)(lakeYPosition + lakeAreaHeight) && !lastDirection.Contains(0) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.Y += 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.X -= 1f;
					k++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			int leftWidth = (int)lastPosition.Y - (int)lake.Lines[0].Start.Y - 1;
			lastPosition.Y -= 1f;
			lastDirection = new List<int>
			{
				2,
				2,
				2
			};
			lake.addPoint(lastPosition);
			int l = 0;
			while (l < leftWidth)
			{
				int whichWayToGo4 = mineRandom.Next(3);
				if (whichWayToGo4 == 0 && lastPosition.X < (float)((int)lake.Lines[0].Start.X) && !lastDirection.Contains(1) && !lake.containsPoint(new Vector2(lastPosition.X + 1f, lastPosition.Y)) && !lake.containsPoint(new Vector2(lastPosition.X, lastPosition.Y - 1f)) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X += 1f;
					lastDirection.Add(0);
				}
				else if (whichWayToGo4 == 1 && lastPosition.X > (float)(lakeXPosition + 1) && !lastDirection.Contains(0) && !smoothZone.Contains((int)lastPosition.X, (int)lastPosition.Y))
				{
					lastPosition.X -= 1f;
					lastDirection.Add(1);
				}
				else
				{
					lastPosition.Y -= 1f;
					l++;
					lastDirection.Add(2);
				}
				lastDirection.RemoveAt(0);
				lake.addPoint(lastPosition);
			}
			if (lastPosition.X < (float)((int)lake.Lines[0].Start.X))
			{
				int leftover = (int)lake.Lines[0].Start.X + 1 - (int)lastPosition.X - 1;
				for (int m = 0; m < leftover; m++)
				{
					lastPosition.X += 1f;
					lake.addPoint(lastPosition);
				}
			}
			return lake;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0003A488 File Offset: 0x00038688
		public static Polygon getRandomBorderRoom(Rectangle room, Random mineRandom)
		{
			int lakeAreaWidth = room.Width - 2;
			int lakeAreaHeight = room.Height - 2;
			int lakeXPosition = room.X + 1;
			int lakeYPosition = room.Y + 1;
			new Rectangle(lakeXPosition, lakeYPosition, lakeAreaWidth, lakeAreaHeight);
			Polygon lake = new Polygon();
			Vector2 lastPosition = new Vector2((float)mineRandom.Next(lakeXPosition + 5, lakeXPosition + 8), (float)mineRandom.Next(lakeYPosition + 5, lakeYPosition + 8));
			lake.Lines.Add(new Polygon.Line(lastPosition, new Vector2(lastPosition.X + 1f, lastPosition.Y)));
			lastPosition.X += 1f;
			int topWidth = room.Right - (int)lastPosition.X - lakeAreaWidth / 8;
			int lastDirection = 2;
			int i = 0;
			while (i < topWidth)
			{
				int whichWayToGo = mineRandom.Next(3);
				if ((whichWayToGo == 0 && lastPosition.Y > (float)room.Y && lastDirection != 1) || (lastDirection == 2 && lastPosition.Y >= (float)(lakeYPosition + lakeAreaHeight / 2)))
				{
					lastPosition.Y -= 1f;
					lastDirection = 0;
				}
				else if ((whichWayToGo == 1 && lastPosition.Y < (float)(lakeYPosition + lakeAreaHeight / 2) && lastDirection != 0) || (lastDirection == 2 && lastPosition.Y <= (float)room.Y))
				{
					lastPosition.Y += 1f;
					lastDirection = 1;
				}
				else
				{
					lastPosition.X += 1f;
					i++;
					lastDirection = 2;
				}
				lake.addPoint(lastPosition);
			}
			int rightWidth = lakeAreaHeight - 4 - (int)(lastPosition.Y - (float)room.Y);
			lastPosition.Y += 1f;
			lastDirection = 2;
			lake.addPoint(lastPosition);
			int j = 0;
			while (j < rightWidth)
			{
				int whichWayToGo2 = mineRandom.Next(3);
				if ((whichWayToGo2 == 0 && lastPosition.X < (float)room.Right && lastDirection != 1) || (lastDirection == 2 && lastPosition.X <= (float)(lakeXPosition + lakeAreaWidth / 2 + 1)))
				{
					lastPosition.X += 1f;
					lastDirection = 0;
				}
				else if ((whichWayToGo2 == 1 && lastPosition.X > (float)(lakeXPosition + lakeAreaWidth / 2 + 1) && lastDirection != 0) || (lastDirection == 2 && lastPosition.X >= (float)room.Right))
				{
					lastPosition.X -= 1f;
					lastDirection = 1;
				}
				else
				{
					lastPosition.Y += 1f;
					j++;
					lastDirection = 2;
				}
				lake.addPoint(lastPosition);
			}
			int bottomWidth = (int)lastPosition.X - (int)lake.Lines[0].Start.X + lakeAreaWidth / 4;
			lastPosition.X -= 1f;
			lastDirection = 2;
			lake.addPoint(lastPosition);
			int k = 0;
			while (k < bottomWidth)
			{
				int whichWayToGo3 = mineRandom.Next(3);
				if ((whichWayToGo3 == 0 && lastPosition.Y > (float)(lakeYPosition + lakeAreaHeight / 2) && lastDirection != 1 && !lake.containsPoint(new Vector2(lastPosition.X, lastPosition.Y - 1f))) || (lastDirection == 2 && lastPosition.Y >= (float)room.Bottom))
				{
					lastPosition.Y -= 1f;
					lastDirection = 0;
				}
				else if ((whichWayToGo3 == 1 && lastPosition.Y < (float)room.Bottom && lastDirection != 0) || (lastDirection == 2 && lastPosition.Y <= (float)(lakeYPosition + lakeAreaHeight / 2)))
				{
					lastPosition.Y += 1f;
					lastDirection = 1;
				}
				else
				{
					lastPosition.X -= 1f;
					k++;
					lastDirection = 2;
				}
				lake.addPoint(lastPosition);
			}
			int leftWidth = (int)lastPosition.Y - (int)lake.Lines[0].Start.Y - 1;
			lastPosition.Y -= 1f;
			lastDirection = 2;
			lake.addPoint(lastPosition);
			int l = 0;
			while (l < leftWidth)
			{
				int whichWayToGo4 = mineRandom.Next(3);
				if ((whichWayToGo4 == 0 && lastPosition.X < (float)room.Center.X && !lake.containsPoint(new Vector2(lastPosition.X + 1f, lastPosition.Y)) && !lake.containsPoint(new Vector2(lastPosition.X, lastPosition.Y - 1f))) || (lastDirection == 2 && lastPosition.X <= (float)room.X))
				{
					lastPosition.X += 1f;
					lastDirection = 0;
				}
				else if ((whichWayToGo4 == 1 && lastPosition.X > (float)room.X && lastDirection != 0) || (lastDirection == 2 && lastPosition.X >= (float)room.Center.X))
				{
					lastPosition.X -= 1f;
					lastDirection = 1;
				}
				else
				{
					lastPosition.Y -= 1f;
					l++;
					lastDirection = 2;
				}
				lake.addPoint(lastPosition);
			}
			if (lastPosition.X < (float)((int)lake.Lines[0].Start.X))
			{
				int leftover = (int)lake.Lines[0].Start.X + 1 - (int)lastPosition.X - 1;
				for (int m = 0; m < leftover; m++)
				{
					lastPosition.X += 1f;
					lake.addPoint(lastPosition);
				}
			}
			return lake;
		}

		// Token: 0x04000344 RID: 836
		public List<Polygon.Line> lines = new List<Polygon.Line>();

		// Token: 0x02000168 RID: 360
		public class Line
		{
			// Token: 0x0600135F RID: 4959 RVA: 0x00184B90 File Offset: 0x00182D90
			public Line(Vector2 Start, Vector2 End)
			{
				this.Start = Start;
				this.End = End;
			}

			// Token: 0x04001440 RID: 5184
			public Vector2 Start;

			// Token: 0x04001441 RID: 5185
			public Vector2 End;
		}
	}
}
