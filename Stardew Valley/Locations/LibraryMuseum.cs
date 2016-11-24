using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x02000131 RID: 305
	public class LibraryMuseum : GameLocation
	{
		// Token: 0x06001187 RID: 4487 RVA: 0x00167A10 File Offset: 0x00165C10
		public LibraryMuseum()
		{
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00167A24 File Offset: 0x00165C24
		public LibraryMuseum(Map map, string name) : base(map, name)
		{
			this.museumPieces = new SerializableDictionary<Vector2, int>();
			for (int x = 0; x < map.Layers[0].LayerWidth; x++)
			{
				for (int y = 0; y < map.Layers[0].LayerHeight; y++)
				{
					if (base.doesTileHaveProperty(x, y, "Action", "Buildings") != null && base.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Notes"))
					{
						this.lostBooksLocations.Add(Convert.ToInt32(base.doesTileHaveProperty(x, y, "Action", "Buildings").Split(new char[]
						{
							' '
						})[1]), new Vector2((float)x, (float)y));
					}
				}
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00167B00 File Offset: 0x00165D00
		public bool museumAlreadyHasArtifact(int index)
		{
			foreach (KeyValuePair<Vector2, int> v in this.museumPieces)
			{
				if (v.Value == index)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00167B60 File Offset: 0x00165D60
		public bool isItemSuitableForDonation(Item i)
		{
			if (i is Object && (i as Object).type != null && ((i as Object).type.Equals("Arch") || (i as Object).type.Equals("Minerals")))
			{
				int index = (i as Object).parentSheetIndex;
				bool museumHasItem = false;
				foreach (KeyValuePair<Vector2, int> v in this.museumPieces)
				{
					if (v.Value == index)
					{
						museumHasItem = true;
						break;
					}
				}
				if (!museumHasItem)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00167C14 File Offset: 0x00165E14
		public bool doesFarmerHaveAnythingToDonate(Farmer who)
		{
			for (int i = 0; i < who.maxItems; i++)
			{
				if (i < who.items.Count && who.items[i] is Object && (who.items[i] as Object).type != null && ((who.items[i] as Object).type.Equals("Arch") || (who.items[i] as Object).type.Equals("Minerals")))
				{
					int index = (who.items[i] as Object).parentSheetIndex;
					bool museumHasItem = false;
					foreach (KeyValuePair<Vector2, int> v in this.museumPieces)
					{
						if (v.Value == index)
						{
							museumHasItem = true;
							break;
						}
					}
					if (!museumHasItem)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00167D2C File Offset: 0x00165F2C
		private bool museumContainsTheseItems(int[] items, HashSet<int> museumItems)
		{
			for (int i = 0; i < items.Length; i++)
			{
				if (!museumItems.Contains(items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00167D58 File Offset: 0x00165F58
		private int numberOfMuseumItemsOfType(string type)
		{
			int num = 0;
			foreach (KeyValuePair<Vector2, int> v in this.museumPieces)
			{
				if (Game1.objectInformation[v.Value].Split(new char[]
				{
					'/'
				})[3].Contains(type))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00167DD8 File Offset: 0x00165FD8
		public override void resetForPlayerEntry()
		{
			if (!Game1.player.eventsSeen.Contains(0) && this.doesFarmerHaveAnythingToDonate(Game1.player) && !Game1.player.mailReceived.Contains("somethingToDonate"))
			{
				Game1.player.mailReceived.Add("somethingToDonate");
			}
			base.resetForPlayerEntry();
			if (!Game1.isRaining)
			{
				Game1.changeMusicTrack("libraryTheme");
			}
			int booksFound = Game1.player.archaeologyFound.ContainsKey(102) ? Game1.player.archaeologyFound[102][0] : 0;
			for (int i = 0; i < this.lostBooksLocations.Count; i++)
			{
				if (this.lostBooksLocations.ElementAt(i).Key <= booksFound && !Game1.player.mailReceived.Contains("lb_" + this.lostBooksLocations.ElementAt(i).Key))
				{
					this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 447, 15, 15), new Vector2(this.lostBooksLocations.ElementAt(i).Value.X * (float)Game1.tileSize, this.lostBooksLocations.ElementAt(i).Value.Y * (float)Game1.tileSize - (float)(Game1.tileSize * 3 / 2) - 16f), false, 0f, Color.White)
					{
						interval = 99999f,
						animationLength = 1,
						totalNumberOfLoops = 9999,
						yPeriodic = true,
						yPeriodicLoopTime = 4000f,
						yPeriodicRange = (float)(Game1.tileSize / 4),
						layerDepth = 1f,
						scale = (float)Game1.pixelZoom,
						id = (float)this.lostBooksLocations.ElementAt(i).Key
					});
				}
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00167FD4 File Offset: 0x001661D4
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (!Game1.isRaining)
			{
				Game1.changeMusicTrack("none");
			}
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00167FF0 File Offset: 0x001661F0
		public List<Item> getRewardsForPlayer(Farmer who)
		{
			List<Item> rewards = new List<Item>();
			HashSet<int> museumItems = new HashSet<int>(this.museumPieces.Values);
			int archItems = this.numberOfMuseumItemsOfType("Arch");
			int mineralItems = this.numberOfMuseumItemsOfType("Minerals");
			int total = archItems + mineralItems;
			if (!who.canUnderstandDwarves && museumItems.Contains(96) && museumItems.Contains(97) && museumItems.Contains(98) && museumItems.Contains(99))
			{
				rewards.Add(new Object(326, 1, false, -1, 0));
			}
			if (!who.specialBigCraftables.Contains(1305) && museumItems.Contains(113) && archItems > 4)
			{
				rewards.Add(new Furniture(1305, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1304) && archItems >= 15)
			{
				rewards.Add(new Furniture(1304, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(139) && archItems >= 20)
			{
				rewards.Add(new Object(Vector2.Zero, 139, false));
			}
			if (!who.specialBigCraftables.Contains(1545) && this.museumContainsTheseItems(new int[]
			{
				108,
				122
			}, museumItems) && archItems > 10)
			{
				rewards.Add(new Furniture(1545, Vector2.Zero));
			}
			if (!who.specialItems.Contains(464) && museumItems.Contains(119) && archItems > 2)
			{
				rewards.Add(new Object(464, 1, false, -1, 0));
			}
			if (!who.specialItems.Contains(463) && museumItems.Contains(123) && archItems > 2)
			{
				rewards.Add(new Object(463, 1, false, -1, 0));
			}
			if (!who.specialItems.Contains(499) && museumItems.Contains(114))
			{
				rewards.Add(new Object(499, 1, false, -1, 0));
				rewards.Add(new Object(499, 1, true, -1, 0));
			}
			if (!who.specialBigCraftables.Contains(1301) && this.museumContainsTheseItems(new int[]
			{
				579,
				581,
				582
			}, museumItems))
			{
				rewards.Add(new Furniture(1301, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1302) && this.museumContainsTheseItems(new int[]
			{
				583,
				584
			}, museumItems))
			{
				rewards.Add(new Furniture(1302, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1303) && this.museumContainsTheseItems(new int[]
			{
				580,
				585
			}, museumItems))
			{
				rewards.Add(new Furniture(1303, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1298) && mineralItems > 10)
			{
				rewards.Add(new Furniture(1298, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(1299) && mineralItems > 30)
			{
				rewards.Add(new Furniture(1299, Vector2.Zero));
			}
			if (!who.specialBigCraftables.Contains(94) && mineralItems > 20)
			{
				rewards.Add(new Object(Vector2.Zero, 94, false));
			}
			if (!who.specialBigCraftables.Contains(21) && mineralItems >= 50)
			{
				rewards.Add(new Object(Vector2.Zero, 21, false));
			}
			if (!who.specialBigCraftables.Contains(131) && mineralItems > 40)
			{
				rewards.Add(new Furniture(131, Vector2.Zero));
			}
			using (List<Item>.Enumerator enumerator = rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.specialItem = true;
				}
			}
			if (!who.mailReceived.Contains("museum5") && total >= 5)
			{
				rewards.Add(new Object(474, 9, false, -1, 0));
			}
			if (!who.mailReceived.Contains("museum10") && total >= 10)
			{
				rewards.Add(new Object(479, 9, false, -1, 0));
			}
			if (!who.mailReceived.Contains("museum15") && total >= 15)
			{
				rewards.Add(new Object(486, 1, false, -1, 0));
			}
			if (!who.mailReceived.Contains("museum20") && total >= 20)
			{
				rewards.Add(new Furniture(1541, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museum25") && total >= 25)
			{
				rewards.Add(new Furniture(1554, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museum30") && total >= 30)
			{
				rewards.Add(new Furniture(1669, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museum40") && total >= 40)
			{
				rewards.Add(new Object(Vector2.Zero, 140, false));
			}
			if (!who.mailReceived.Contains("museum50") && total >= 50)
			{
				rewards.Add(new Furniture(1671, Vector2.Zero));
			}
			if (!who.mailReceived.Contains("museumComplete") && total >= 95)
			{
				rewards.Add(new Object(434, 1, false, -1, 0));
			}
			if (total >= 60)
			{
				if (!Game1.player.eventsSeen.Contains(295672))
				{
					Game1.player.eventsSeen.Add(295672);
				}
				else if (!Game1.player.hasRustyKey)
				{
					Game1.player.eventsSeen.Remove(66);
				}
			}
			return rewards;
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x001685B8 File Offset: 0x001667B8
		public void collectedReward(Item item, Farmer who)
		{
			if (item != null && item is Object)
			{
				(item as Object).specialItem = true;
				int parentSheetIndex = (item as Object).ParentSheetIndex;
				if (parentSheetIndex <= 479)
				{
					if (parentSheetIndex <= 434)
					{
						if (parentSheetIndex == 140)
						{
							who.mailReceived.Add("museum40");
							return;
						}
						if (parentSheetIndex != 434)
						{
							return;
						}
						who.mailReceived.Add("museumComplete");
						return;
					}
					else
					{
						if (parentSheetIndex == 474)
						{
							who.mailReceived.Add("museum5");
							return;
						}
						if (parentSheetIndex != 479)
						{
							return;
						}
						who.mailReceived.Add("museum10");
						return;
					}
				}
				else if (parentSheetIndex <= 1541)
				{
					if (parentSheetIndex == 486)
					{
						who.mailReceived.Add("museum15");
						return;
					}
					if (parentSheetIndex != 1541)
					{
						return;
					}
					who.mailReceived.Add("museum20");
					return;
				}
				else
				{
					if (parentSheetIndex == 1554)
					{
						who.mailReceived.Add("museum25");
						return;
					}
					if (parentSheetIndex == 1669)
					{
						who.mailReceived.Add("museum30");
						return;
					}
					if (parentSheetIndex != 1671)
					{
						return;
					}
					who.mailReceived.Add("museum50");
				}
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x001686F0 File Offset: 0x001668F0
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			foreach (KeyValuePair<Vector2, int> v in this.museumPieces)
			{
				if (v.Key.X == (float)tileLocation.X && (v.Key.Y == (float)tileLocation.Y || v.Key.Y == (float)(tileLocation.Y - 1)))
				{
					Game1.drawObjectDialogue(Game1.parseText(string.Concat(new string[]
					{
						" - ",
						Game1.objectInformation[v.Value].Split(new char[]
						{
							'/'
						})[0],
						" - ",
						Environment.NewLine,
						Game1.objectInformation[v.Value].Split(new char[]
						{
							'/'
						})[4]
					})));
					return true;
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00168810 File Offset: 0x00166A10
		public bool isTileSuitableForMuseumPiece(int x, int y)
		{
			Vector2 p = new Vector2((float)x, (float)y);
			if (!this.museumPieces.ContainsKey(p))
			{
				int indexOfBuildingsLayer = base.getTileIndexAt(new Point(x, y), "Buildings");
				if (indexOfBuildingsLayer == 1073 || indexOfBuildingsLayer == 1074 || indexOfBuildingsLayer == 1072 || indexOfBuildingsLayer == 1237 || indexOfBuildingsLayer == 1238)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00168874 File Offset: 0x00166A74
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			foreach (TemporaryAnimatedSprite t in this.temporarySprites)
			{
				if (t.layerDepth >= 1f)
				{
					t.draw(b, false, 0, 0);
				}
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x001688D8 File Offset: 0x00166AD8
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			foreach (KeyValuePair<Vector2, int> v in this.museumPieces)
			{
				b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, v.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize - 12))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (v.Key.Y * (float)Game1.tileSize - 2f) / 10000f);
				b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, v.Key * (float)Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, v.Value, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, v.Key.Y * (float)Game1.tileSize / 10000f);
			}
		}

		// Token: 0x0400127B RID: 4731
		public const int dwarvenGuide = 0;

		// Token: 0x0400127C RID: 4732
		public const int totalArtifacts = 95;

		// Token: 0x0400127D RID: 4733
		public const int totalNotes = 21;

		// Token: 0x0400127E RID: 4734
		public SerializableDictionary<Vector2, int> museumPieces;

		// Token: 0x0400127F RID: 4735
		private Dictionary<int, Vector2> lostBooksLocations = new Dictionary<int, Vector2>();
	}
}
