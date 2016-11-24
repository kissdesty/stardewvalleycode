using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000055 RID: 85
	public class Utility
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x000A93E8 File Offset: 0x000A75E8
		public static char getRandomSlotCharacter()
		{
			return Utility.getRandomSlotCharacter('o');
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000A93F4 File Offset: 0x000A75F4
		public static List<Vector2> removeDuplicates(List<Vector2> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = list.Count - 1; j >= 0; j--)
				{
					if (j != i && list[i].Equals(list[j]))
					{
						list.RemoveAt(j);
					}
				}
			}
			return list;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000A9449 File Offset: 0x000A7649
		public static Point Vector2ToPoint(Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000A945E File Offset: 0x000A765E
		public static Vector2 PointToVector2(Point p)
		{
			return new Vector2((float)p.X, (float)p.Y);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000A9474 File Offset: 0x000A7674
		public static int getStartTimeOfFestival()
		{
			if (Game1.weatherIcon == 1)
			{
				if (Game1.temporaryContent == null)
				{
					Game1.temporaryContent = Game1.content.CreateTemporary();
				}
				return Convert.ToInt32(Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + Game1.dayOfMonth)["conditions"].Split(new char[]
				{
					'/'
				})[1].Split(new char[]
				{
					' '
				})[0]);
			}
			return -1;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000A94FC File Offset: 0x000A76FC
		public static bool isFestivalDay(int day, string season)
		{
			string s = season + day;
			if (Game1.temporaryContent == null)
			{
				Game1.temporaryContent = Game1.content.CreateTemporary();
			}
			return Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\FestivalDates").ContainsKey(s);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000A9544 File Offset: 0x000A7744
		public static bool isObjectOffLimitsForSale(int index)
		{
			if (index <= 439)
			{
				if (index <= 341)
				{
					switch (index)
					{
					case 158:
					case 159:
					case 160:
					case 161:
					case 162:
					case 163:
						break;
					default:
						if (index != 326 && index != 341)
						{
							return false;
						}
						break;
					}
				}
				else if (index != 413 && index != 437 && index != 439)
				{
					return false;
				}
			}
			else if (index <= 645)
			{
				if (index != 454 && index != 460 && index != 645)
				{
					return false;
				}
			}
			else
			{
				switch (index)
				{
				case 680:
				case 681:
				case 682:
				case 688:
				case 689:
				case 690:
					break;
				case 683:
				case 684:
				case 685:
				case 686:
				case 687:
					return false;
				default:
					if (index != 774 && index != 775)
					{
						return false;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000A9627 File Offset: 0x000A7827
		public static Microsoft.Xna.Framework.Rectangle xTileToMicrosoftRectangle(xTile.Dimensions.Rectangle rect)
		{
			return new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000A964C File Offset: 0x000A784C
		public static Dictionary<Item, int[]> getTravelingMerchantStock()
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			Random r = new Random((int)(Game1.uniqueIDForThisGame + (ulong)Game1.stats.DaysPlayed));
			for (int i = 0; i < 10; i++)
			{
				int index = r.Next(2, 790);
				string[] split;
				while (true)
				{
					index++;
					index %= 790;
					if (Game1.objectInformation.ContainsKey(index) && !Utility.isObjectOffLimitsForSale(index))
					{
						split = Game1.objectInformation[index].Split(new char[]
						{
							'/'
						});
						if (split[3].Contains('-') && Convert.ToInt32(split[1]) > 0 && !split[3].Contains("-13") && !split[3].Equals("Quest") && !split[0].Equals("Weeds") && !split[3].Contains("Minerals") && !split[3].Contains("Arch"))
						{
							break;
						}
					}
				}
				stock.Add(new Object(index, 1, false, -1, 0), new int[]
				{
					Math.Max(r.Next(1, 11) * 100, Convert.ToInt32(split[1]) * r.Next(3, 6)),
					(r.NextDouble() < 0.1) ? 5 : 1
				});
			}
			stock.Add(Utility.getRandomFurniture(r, null, 0, 1613), new int[]
			{
				r.Next(1, 11) * 250,
				1
			});
			if (Utility.getSeasonNumber(Game1.currentSeason) < 2)
			{
				stock.Add(new Object(347, 1, false, -1, 0), new int[]
				{
					1000,
					(r.NextDouble() < 0.1) ? 5 : 1
				});
			}
			else if (r.NextDouble() < 0.4)
			{
				stock.Add(new Object(Vector2.Zero, 136, false), new int[]
				{
					4000,
					1
				});
			}
			if (r.NextDouble() < 0.25)
			{
				stock.Add(new Object(433, 1, false, -1, 0), new int[]
				{
					2500,
					1
				});
			}
			return stock;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000A9884 File Offset: 0x000A7A84
		public static Dictionary<Item, int[]> getDwarfShopStock()
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			stock.Add(new Object(773, 1, false, -1, 0), new int[]
			{
				2000,
				2147483647
			});
			stock.Add(new Object(772, 1, false, -1, 0), new int[]
			{
				3000,
				2147483647
			});
			stock.Add(new Object(286, 1, false, -1, 0), new int[]
			{
				300,
				2147483647
			});
			stock.Add(new Object(287, 1, false, -1, 0), new int[]
			{
				600,
				2147483647
			});
			stock.Add(new Object(288, 1, false, -1, 0), new int[]
			{
				1000,
				2147483647
			});
			stock.Add(new Object(243, 1, false, -1, 0), new int[]
			{
				1000,
				2147483647
			});
			stock.Add(new Object(Vector2.Zero, 138, false), new int[]
			{
				2500,
				2147483647
			});
			if (!Game1.player.craftingRecipes.ContainsKey("Weathered Floor"))
			{
				stock.Add(new Object(331, 1, true, -1, 0), new int[]
				{
					500,
					1
				});
			}
			return stock;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000A99FC File Offset: 0x000A7BFC
		public static Dictionary<Item, int[]> getHospitalStock()
		{
			return new Dictionary<Item, int[]>
			{
				{
					new Object(349, 1, false, -1, 0),
					new int[]
					{
						1000,
						2147483647
					}
				},
				{
					new Object(351, 1, false, -1, 0),
					new int[]
					{
						1000,
						2147483647
					}
				}
			};
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000A9A64 File Offset: 0x000A7C64
		public static bool hasFarmerShippedAllItems()
		{
			int farmerShipped = 0;
			int total = 0;
			foreach (KeyValuePair<int, string> kvp in Game1.objectInformation)
			{
				string typeString = kvp.Value.Split(new char[]
				{
					'/'
				})[3];
				if (!typeString.Contains("Arch") && !typeString.Contains("Fish") && !typeString.Contains("Mineral") && !typeString.Substring(typeString.Length - 3).Equals("-2") && !typeString.Contains("Cooking") && !typeString.Substring(typeString.Length - 3).Equals("-7") && Object.isPotentialBasicShippedCategory(kvp.Key, typeString.Substring(typeString.Length - 3)))
				{
					total++;
					if (Game1.player.basicShipped.ContainsKey(kvp.Key))
					{
						farmerShipped++;
					}
				}
			}
			return total == farmerShipped;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000A9B90 File Offset: 0x000A7D90
		public static Dictionary<Item, int[]> getQiShopStock()
		{
			return new Dictionary<Item, int[]>
			{
				{
					new Furniture(1552, Vector2.Zero),
					new int[]
					{
						5000,
						2147483647
					}
				},
				{
					new Furniture(1545, Vector2.Zero),
					new int[]
					{
						4000,
						2147483647
					}
				},
				{
					new Furniture(1563, Vector2.Zero),
					new int[]
					{
						4000,
						2147483647
					}
				},
				{
					new Furniture(1561, Vector2.Zero),
					new int[]
					{
						3000,
						2147483647
					}
				},
				{
					new Hat(2),
					new int[]
					{
						8000,
						2147483647
					}
				},
				{
					new Object(Vector2.Zero, 126, false),
					new int[]
					{
						10000,
						2147483647
					}
				},
				{
					new Object(298, 1, false, -1, 0),
					new int[]
					{
						100,
						2147483647
					}
				},
				{
					new Object(703, 1, false, -1, 0),
					new int[]
					{
						1000,
						2147483647
					}
				},
				{
					new Object(688, 1, false, -1, 0),
					new int[]
					{
						500,
						2147483647
					}
				}
			};
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000A9D18 File Offset: 0x000A7F18
		public static Dictionary<Item, int[]> getJojaStock()
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			stock.Add(new Object(Vector2.Zero, 167, 2147483647), new int[]
			{
				75,
				2147483647
			});
			stock.Add(new Wallpaper(21, false)
			{
				stack = 2147483647
			}, new int[]
			{
				20,
				2147483647
			});
			stock.Add(new Furniture(1609, Vector2.Zero)
			{
				stack = 2147483647
			}, new int[]
			{
				500,
				2147483647
			});
			float priceMod = Game1.player.hasOrWillReceiveMail("JojaMember") ? 2f : 2.5f;
			if (Game1.currentSeason.Equals("spring"))
			{
				stock.Add(new Object(Vector2.Zero, 472, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[472].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 473, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[473].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 474, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[474].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 475, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[475].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 427, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[427].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 429, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[429].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 477, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[477].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
			}
			if (Game1.currentSeason.Equals("summer"))
			{
				stock.Add(new Object(Vector2.Zero, 480, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[480].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 482, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[482].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 483, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[483].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 484, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[484].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 479, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[479].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 302, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[302].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 453, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[453].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 455, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[455].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(431, 2147483647, false, 100, 0), new int[]
				{
					(int)(50f * priceMod),
					2147483647
				});
			}
			if (Game1.currentSeason.Equals("fall"))
			{
				stock.Add(new Object(Vector2.Zero, 487, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[487].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 488, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[488].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 483, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[483].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 490, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[490].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 299, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[299].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 301, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[301].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 492, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[492].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 491, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[491].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 493, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[493].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
				stock.Add(new Object(431, 2147483647, false, 100, 0), new int[]
				{
					(int)(50f * priceMod),
					2147483647
				});
				stock.Add(new Object(Vector2.Zero, 425, 2147483647), new int[]
				{
					(int)((float)Convert.ToInt32(Game1.objectInformation[425].Split(new char[]
					{
						'/'
					})[1]) * priceMod),
					2147483647
				});
			}
			stock.Add(new Object(Vector2.Zero, 297, 2147483647), new int[]
			{
				(int)((float)Convert.ToInt32(Game1.objectInformation[297].Split(new char[]
				{
					'/'
				})[1]) * priceMod),
				2147483647
			});
			stock.Add(new Object(Vector2.Zero, 245, 2147483647), new int[]
			{
				(int)((float)Convert.ToInt32(Game1.objectInformation[245].Split(new char[]
				{
					'/'
				})[1]) * priceMod),
				2147483647
			});
			stock.Add(new Object(Vector2.Zero, 246, 2147483647), new int[]
			{
				(int)((float)Convert.ToInt32(Game1.objectInformation[246].Split(new char[]
				{
					'/'
				})[1]) * priceMod),
				2147483647
			});
			stock.Add(new Object(Vector2.Zero, 423, 2147483647), new int[]
			{
				(int)((float)Convert.ToInt32(Game1.objectInformation[423].Split(new char[]
				{
					'/'
				})[1]) * priceMod),
				2147483647
			});
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2) + 1u));
			int whichWallpaper = r.Next(112);
			if (whichWallpaper == 21)
			{
				whichWallpaper = 22;
			}
			stock.Add(new Wallpaper(whichWallpaper, false)
			{
				stack = 2147483647
			}, new int[]
			{
				250,
				2147483647
			});
			stock.Add(new Wallpaper(r.Next(40), true)
			{
				stack = 2147483647
			}, new int[]
			{
				250,
				2147483647
			});
			return stock;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000AA8AC File Offset: 0x000A8AAC
		public static Dictionary<Item, int[]> getHatStock()
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			foreach (KeyValuePair<int, string> v in Game1.content.Load<Dictionary<int, string>>("Data\\Achievements"))
			{
				if (Game1.player.achievements.Contains(v.Key))
				{
					stock.Add(new Hat(Convert.ToInt32(v.Value.Split(new char[]
					{
						'^'
					})[4])), new int[]
					{
						1000,
						2147483647
					});
				}
			}
			return stock;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000AA960 File Offset: 0x000A8B60
		public static NPC getTodaysBirthdayNPC(string season, int day)
		{
			foreach (NPC i in Utility.getAllCharacters())
			{
				if (i.isBirthday(season, day))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000AA9BC File Offset: 0x000A8BBC
		public static bool highlightEdibleItems(Item i)
		{
			return i is Object && (i as Object).edibility != -300;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000AA9E0 File Offset: 0x000A8BE0
		public static int getRandomSingleTileFurniture(Random r)
		{
			switch (r.Next(3))
			{
			case 0:
				return r.Next(10) * 3;
			case 1:
				return r.Next(1376, 1391);
			case 2:
				return r.Next(7) * 2 + 1391;
			default:
				return 0;
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000AAA38 File Offset: 0x000A8C38
		public static void improveFriendshipWithEveryoneInRegion(Farmer who, int amount, int region)
		{
			using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (NPC i in enumerator.Current.characters)
					{
						if (i.homeRegion == region && who.friendships.ContainsKey(i.name))
						{
							who.changeFriendship(amount, i);
						}
					}
				}
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000AAAE0 File Offset: 0x000A8CE0
		public static Item getGiftFromNPC(NPC who)
		{
			Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + Game1.year + Game1.dayOfMonth + Utility.getSeasonNumber(Game1.currentSeason) + who.getTileX());
			List<Item> possibleObjects = new List<Item>();
			string name = who.name;
			if (!(name == "Clint"))
			{
				if (!(name == "Marnie"))
				{
					if (!(name == "Robin"))
					{
						if (!(name == "Willy"))
						{
							if (!(name == "Evelyn"))
							{
								int age = who.age;
								if (age == 2)
								{
									possibleObjects.Add(new Object(330, 1, false, -1, 0));
									possibleObjects.Add(new Object(103, 1, false, -1, 0));
									possibleObjects.Add(new Object(394, 1, false, -1, 0));
									possibleObjects.Add(new Object(r.Next(535, 538), 1, false, -1, 0));
								}
								else
								{
									possibleObjects.Add(new Object(608, 1, false, -1, 0));
									possibleObjects.Add(new Object(651, 1, false, -1, 0));
									possibleObjects.Add(new Object(611, 1, false, -1, 0));
									possibleObjects.Add(new Ring(517));
									possibleObjects.Add(new Object(466, 10, false, -1, 0));
									possibleObjects.Add(new Object(422, 1, false, -1, 0));
									possibleObjects.Add(new Object(392, 1, false, -1, 0));
									possibleObjects.Add(new Object(348, 1, false, -1, 0));
									possibleObjects.Add(new Object(346, 1, false, -1, 0));
									possibleObjects.Add(new Object(341, 1, false, -1, 0));
									possibleObjects.Add(new Object(221, 1, false, -1, 0));
									possibleObjects.Add(new Object(64, 1, false, -1, 0));
									possibleObjects.Add(new Object(60, 1, false, -1, 0));
									possibleObjects.Add(new Object(70, 1, false, -1, 0));
								}
							}
							else
							{
								possibleObjects.Add(new Object(223, 1, false, -1, 0));
							}
						}
						else
						{
							possibleObjects.Add(new Object(690, 25, false, -1, 0));
							possibleObjects.Add(new Object(687, 1, false, -1, 0));
							possibleObjects.Add(new Object(703, 1, false, -1, 0));
						}
					}
					else
					{
						possibleObjects.Add(new Object(388, 99, false, -1, 0));
						possibleObjects.Add(new Object(390, 50, false, -1, 0));
						possibleObjects.Add(new Object(709, 25, false, -1, 0));
					}
				}
				else
				{
					possibleObjects.Add(new Object(176, 12, false, -1, 0));
				}
			}
			else
			{
				possibleObjects.Add(new Object(337, 1, false, -1, 0));
				possibleObjects.Add(new Object(336, 5, false, -1, 0));
				possibleObjects.Add(new Object(r.Next(535, 538), 5, false, -1, 0));
			}
			return possibleObjects[r.Next(possibleObjects.Count)];
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000AAE04 File Offset: 0x000A9004
		public static NPC getTopRomanticInterest(Farmer who)
		{
			NPC topSpot = null;
			int highestFriendPoints = -1;
			foreach (NPC i in Utility.getAllCharacters())
			{
				if (who.friendships.ContainsKey(i.name) && i.datable && who.getFriendshipLevelForNPC(i.name) > highestFriendPoints)
				{
					topSpot = i;
					highestFriendPoints = who.getFriendshipLevelForNPC(i.name);
				}
			}
			return topSpot;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000AAE90 File Offset: 0x000A9090
		public static Color getRandomRainbowColor(Random r = null)
		{
			switch ((r == null) ? Game1.random.Next(8) : r.Next(8))
			{
			case 0:
				return Color.Red;
			case 1:
				return Color.Orange;
			case 2:
				return Color.Yellow;
			case 3:
				return Color.Lime;
			case 4:
				return Color.Cyan;
			case 5:
				return new Color(0, 100, 255);
			case 6:
				return new Color(152, 96, 255);
			case 7:
				return new Color(255, 100, 255);
			default:
				return Color.White;
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000AAF34 File Offset: 0x000A9134
		public static NPC getTopNonRomanticInterest(Farmer who)
		{
			NPC topSpot = null;
			int highestFriendPoints = -1;
			foreach (NPC i in Utility.getAllCharacters())
			{
				if (who.friendships.ContainsKey(i.name) && !i.datable && who.getFriendshipLevelForNPC(i.name) > highestFriendPoints)
				{
					topSpot = i;
					highestFriendPoints = who.getFriendshipLevelForNPC(i.name);
				}
			}
			return topSpot;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x000AAFC0 File Offset: 0x000A91C0
		public static int getHighestSkill(Farmer who)
		{
			int topSkillExperience = 0;
			int topSkill = 0;
			for (int i = 0; i < who.experiencePoints.Length; i++)
			{
				if (who.experiencePoints[i] > topSkillExperience)
				{
					topSkill = i;
				}
			}
			return topSkill;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000AAFF4 File Offset: 0x000A91F4
		public static int getNumberOfFriendsWithinThisRange(Farmer who, int minFriendshipPoints, int maxFriendshipPoints, bool romanceOnly = false)
		{
			int number = 0;
			foreach (NPC i in Utility.getAllCharacters())
			{
				if (who.friendships.ContainsKey(i.name) && who.getFriendshipLevelForNPC(i.name) >= minFriendshipPoints && who.getFriendshipLevelForNPC(i.name) <= maxFriendshipPoints && (!romanceOnly || i.datable))
				{
					number++;
				}
			}
			return number;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x000AB084 File Offset: 0x000A9284
		public static bool highlightEdibleNonCookingItems(Item i)
		{
			return i is Object && (i as Object).edibility != -300 && (i as Object).category != -7;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000AB0B4 File Offset: 0x000A92B4
		public static bool highlightSmallObjects(Item i)
		{
			return i is Object && !(i as Object).bigCraftable;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x000AB0CE File Offset: 0x000A92CE
		public static bool highlightShippableObjects(Item i)
		{
			return i is Object && (i as Object).canBeShipped();
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000AB0E8 File Offset: 0x000A92E8
		public static Farmer getFarmerFromFarmerNumberString(string s)
		{
			if (s.Equals("farmer"))
			{
				return Game1.player;
			}
			return Utility.getFarmerFromFarmerNumber(Convert.ToInt32(s[s.Length - 1].ToString() ?? ""));
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000AB134 File Offset: 0x000A9334
		public static int getFarmerNumberFromFarmer(Farmer who)
		{
			for (int i = 1; i <= 4; i++)
			{
				if (Utility.getFarmerFromFarmerNumber(i).Equals(who))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000AB160 File Offset: 0x000A9360
		public static Farmer getFarmerFromFarmerNumber(int number)
		{
			if (!Game1.IsMultiplayer)
			{
				if (number == 1)
				{
					return Game1.player;
				}
				return null;
			}
			else
			{
				if (number == 1 && Game1.serverHost != null)
				{
					return Game1.serverHost;
				}
				if (number <= Game1.numberOfPlayers())
				{
					long[] ids = new long[Game1.numberOfPlayers() - 1];
					int idIndex = 0;
					for (int i = 0; i < Game1.otherFarmers.Count; i++)
					{
						if (Game1.otherFarmers.Values.ElementAt(i).uniqueMultiplayerID != Game1.serverHost.uniqueMultiplayerID)
						{
							ids[idIndex] = Game1.otherFarmers.Values.ElementAt(i).uniqueMultiplayerID;
							idIndex++;
						}
					}
					return Game1.otherFarmers[ids[number - 2]];
				}
				return null;
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000AB20C File Offset: 0x000A940C
		public static string getLoveInterest(string who)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(who);
			if (num <= 1866496948u)
			{
				if (num <= 1067922812u)
				{
					if (num != 161540545u)
					{
						if (num != 587846041u)
						{
							if (num == 1067922812u)
							{
								if (who == "Sam")
								{
									return "Penny";
								}
							}
						}
						else if (who == "Penny")
						{
							return "Sam";
						}
					}
					else if (who == "Sebastian")
					{
						return "Abigail";
					}
				}
				else if (num != 1281010426u)
				{
					if (num != 1708213605u)
					{
						if (num == 1866496948u)
						{
							if (who == "Shane")
							{
								return "Emily";
							}
						}
					}
					else if (who == "Alex")
					{
						return "Haley";
					}
				}
				else if (who == "Maru")
				{
					return "Harvey";
				}
			}
			else if (num <= 2571828641u)
			{
				if (num != 2010304804u)
				{
					if (num != 2434294092u)
					{
						if (num == 2571828641u)
						{
							if (who == "Emily")
							{
								return "Shane";
							}
						}
					}
					else if (who == "Haley")
					{
						return "Alex";
					}
				}
				else if (who == "Harvey")
				{
					return "Maru";
				}
			}
			else if (num != 2732913340u)
			{
				if (num != 2826247323u)
				{
					if (num == 3066176300u)
					{
						if (who == "Elliott")
						{
							return "Leah";
						}
					}
				}
				else if (who == "Leah")
				{
					return "Elliott";
				}
			}
			else if (who == "Abigail")
			{
				return "Sebastian";
			}
			return "";
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000AB3F8 File Offset: 0x000A95F8
		public static Dictionary<Item, int[]> getFishShopStock(Farmer who)
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			stock.Add(new Object(219, 1, false, -1, 0), new int[]
			{
				250,
				2147483647
			});
			if (Game1.player.fishingLevel >= 2)
			{
				stock.Add(new Object(685, 1, false, -1, 0), new int[]
				{
					5,
					2147483647
				});
			}
			if (Game1.player.fishingLevel >= 3)
			{
				stock.Add(new Object(710, 1, false, -1, 0), new int[]
				{
					1500,
					2147483647
				});
			}
			if (Game1.player.fishingLevel >= 6)
			{
				stock.Add(new Object(686, 1, false, -1, 0), new int[]
				{
					500,
					2147483647
				});
				stock.Add(new Object(694, 1, false, -1, 0), new int[]
				{
					500,
					2147483647
				});
				stock.Add(new Object(692, 1, false, -1, 0), new int[]
				{
					200,
					2147483647
				});
			}
			if (Game1.player.fishingLevel >= 7)
			{
				stock.Add(new Object(693, 1, false, -1, 0), new int[]
				{
					750,
					2147483647
				});
				stock.Add(new Object(695, 1, false, -1, 0), new int[]
				{
					750,
					2147483647
				});
			}
			if (Game1.player.fishingLevel >= 8)
			{
				stock.Add(new Object(691, 1, false, -1, 0), new int[]
				{
					1000,
					2147483647
				});
				stock.Add(new Object(687, 1, false, -1, 0), new int[]
				{
					1000,
					2147483647
				});
			}
			if (Game1.player.fishingLevel >= 9)
			{
				stock.Add(new Object(703, 1, false, -1, 0), new int[]
				{
					1000,
					2147483647
				});
			}
			stock.Add(new FishingRod(0), new int[]
			{
				500,
				2147483647
			});
			if (Game1.player.fishingLevel >= 2)
			{
				stock.Add(new FishingRod(2), new int[]
				{
					1800,
					2147483647
				});
			}
			if (Game1.player.fishingLevel >= 6)
			{
				stock.Add(new FishingRod(3), new int[]
				{
					7500,
					2147483647
				});
			}
			return stock;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000AB6A8 File Offset: 0x000A98A8
		public static void Shuffle<T>(Random rng, T[] array)
		{
			int i = array.Length;
			while (i > 1)
			{
				int j = rng.Next(i--);
				T temp = array[i];
				array[i] = array[j];
				array[j] = temp;
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000AB6EC File Offset: 0x000A98EC
		public static int getSeasonNumber(string whichSeason)
		{
			string a = whichSeason.ToLower();
			if (a == "spring")
			{
				return 0;
			}
			if (a == "summer")
			{
				return 1;
			}
			if (a == "autumn" || a == "fall")
			{
				return 2;
			}
			if (!(a == "winter"))
			{
				return -1;
			}
			return 3;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000AB74C File Offset: 0x000A994C
		public static char getRandomSlotCharacter(char current)
		{
			char which = 'o';
			while (which == 'o' || which == current)
			{
				switch (Game1.random.Next(8))
				{
				case 0:
					which = '=';
					break;
				case 1:
					which = '\\';
					break;
				case 2:
					which = ']';
					break;
				case 3:
					which = '[';
					break;
				case 4:
					which = '<';
					break;
				case 5:
					which = '*';
					break;
				case 6:
					which = '$';
					break;
				case 7:
					which = '}';
					break;
				}
			}
			return which;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000AB7C4 File Offset: 0x000A99C4
		public static List<Vector2> getPositionsInClusterAroundThisTile(Vector2 startTile, int number)
		{
			Queue<Vector2> openList = new Queue<Vector2>();
			List<Vector2> tiles = new List<Vector2>();
			openList.Enqueue(startTile);
			while (tiles.Count < number)
			{
				Vector2 currentTile = openList.Dequeue();
				tiles.Add(currentTile);
				if (!tiles.Contains(new Vector2(currentTile.X + 1f, currentTile.Y)))
				{
					openList.Enqueue(new Vector2(currentTile.X + 1f, currentTile.Y));
				}
				if (!tiles.Contains(new Vector2(currentTile.X - 1f, currentTile.Y)))
				{
					openList.Enqueue(new Vector2(currentTile.X - 1f, currentTile.Y));
				}
				if (!tiles.Contains(new Vector2(currentTile.X, currentTile.Y + 1f)))
				{
					openList.Enqueue(new Vector2(currentTile.X, currentTile.Y + 1f));
				}
				if (!tiles.Contains(new Vector2(currentTile.X, currentTile.Y - 1f)))
				{
					openList.Enqueue(new Vector2(currentTile.X, currentTile.Y - 1f));
				}
			}
			return tiles;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x000AB8F8 File Offset: 0x000A9AF8
		public static bool doesPointHaveLineOfSightInMine(Vector2 start, Vector2 end, int visionDistance)
		{
			if (Vector2.Distance(start, end) > (float)visionDistance)
			{
				return false;
			}
			foreach (Point p in Utility.GetPointsOnLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y))
			{
				if (Game1.mine.getTileIndexAt(p, "Buildings") != -1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000AB980 File Offset: 0x000A9B80
		public static void addSprinklesToLocation(GameLocation l, int sourceXTile, int sourceYTile, int tilesWide, int tilesHigh, int totalSprinkleDuration, int millisecondsBetweenSprinkles, Color sprinkleColor, string sound = null, bool motionTowardCenter = false)
		{
			Microsoft.Xna.Framework.Rectangle area = new Microsoft.Xna.Framework.Rectangle(sourceXTile - tilesWide / 2, sourceYTile - tilesHigh / 2, tilesWide, tilesHigh);
			Random r = new Random();
			int numSprinkles = totalSprinkleDuration / millisecondsBetweenSprinkles;
			for (int i = 0; i < numSprinkles; i++)
			{
				Vector2 currentSprinklePosition = Utility.getRandomPositionInThisRectangle(area, r) * (float)Game1.tileSize;
				l.temporarySprites.Add(new TemporaryAnimatedSprite(r.Next(10, 12), currentSprinklePosition, sprinkleColor, 8, false, 50f, 0, -1, -1f, -1, 0)
				{
					layerDepth = 1f,
					delayBeforeAnimationStart = millisecondsBetweenSprinkles * i,
					interval = 100f,
					startSound = sound,
					motion = (motionTowardCenter ? Utility.getVelocityTowardPoint(currentSprinklePosition, new Vector2((float)sourceXTile, (float)sourceYTile) * (float)Game1.tileSize, Vector2.Distance(new Vector2((float)sourceXTile, (float)sourceYTile) * (float)Game1.tileSize, currentSprinklePosition) / 64f) : Vector2.Zero),
					xStopCoordinate = sourceXTile,
					yStopCoordinate = sourceYTile
				});
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000ABA88 File Offset: 0x000A9C88
		public static void addStarsAndSpirals(GameLocation l, int sourceXTile, int sourceYTile, int tilesWide, int tilesHigh, int totalSprinkleDuration, int millisecondsBetweenSprinkles, Color sprinkleColor, string sound = null, bool motionTowardCenter = false)
		{
			Microsoft.Xna.Framework.Rectangle area = new Microsoft.Xna.Framework.Rectangle(sourceXTile - tilesWide / 2, sourceYTile - tilesHigh / 2, tilesWide, tilesHigh);
			Random r = new Random();
			int numSprinkles = totalSprinkleDuration / millisecondsBetweenSprinkles;
			for (int i = 0; i < numSprinkles; i++)
			{
				Vector2 currentSprinklePosition = Utility.getRandomPositionInThisRectangle(area, r) * (float)Game1.tileSize;
				l.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, (r.NextDouble() < 0.5) ? new Microsoft.Xna.Framework.Rectangle(359, 1437, 14, 14) : new Microsoft.Xna.Framework.Rectangle(377, 1438, 9, 9), currentSprinklePosition, false, 0.01f, sprinkleColor)
				{
					xPeriodic = true,
					xPeriodicLoopTime = (float)r.Next(2000, 3000),
					xPeriodicRange = (float)r.Next(-Game1.tileSize, Game1.tileSize),
					motion = new Vector2(0f, -2f),
					rotationChange = 3.14159274f / (float)r.Next(4, 64),
					delayBeforeAnimationStart = millisecondsBetweenSprinkles * i,
					layerDepth = 1f,
					scaleChange = 0.04f,
					scaleChangeChange = -0.0008f,
					scale = 4f
				});
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000ABBCE File Offset: 0x000A9DCE
		public static Vector2 clampToTile(Vector2 nonTileLocation)
		{
			nonTileLocation.X -= nonTileLocation.X % (float)Game1.tileSize;
			nonTileLocation.Y -= nonTileLocation.Y % (float)Game1.tileSize;
			return nonTileLocation;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000ABC01 File Offset: 0x000A9E01
		public static float distance(float x1, float x2, float y1, float y2)
		{
			return (float)Math.Sqrt((double)((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000ABC1C File Offset: 0x000A9E1C
		public static void facePlayerEndBehavior(Character c, GameLocation location)
		{
			c.faceGeneralDirection(new Vector2((float)Game1.player.GetBoundingBox().Center.X, (float)Game1.player.GetBoundingBox().Center.Y), 0);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000ABC68 File Offset: 0x000A9E68
		public static bool couldSeePlayerInPeripheralVision(Character c)
		{
			switch (c.facingDirection)
			{
			case 0:
				if (Game1.player.GetBoundingBox().Center.Y < c.GetBoundingBox().Center.Y + Game1.tileSize / 2)
				{
					return true;
				}
				break;
			case 1:
				if (Game1.player.GetBoundingBox().Center.X > c.GetBoundingBox().Center.X - Game1.tileSize / 2)
				{
					return true;
				}
				break;
			case 2:
				if (Game1.player.GetBoundingBox().Center.Y > c.GetBoundingBox().Center.Y - Game1.tileSize / 2)
				{
					return true;
				}
				break;
			case 3:
				if (Game1.player.GetBoundingBox().Center.X < c.GetBoundingBox().Center.X + Game1.tileSize / 2)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000ABD74 File Offset: 0x000A9F74
		public static List<Microsoft.Xna.Framework.Rectangle> divideThisRectangleIntoQuarters(Microsoft.Xna.Framework.Rectangle rect)
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
			{
				new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2),
				new Microsoft.Xna.Framework.Rectangle(rect.X + rect.Width / 2, rect.Y, rect.Width / 2, rect.Height / 2),
				new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2),
				new Microsoft.Xna.Framework.Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2)
			};
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000ABE48 File Offset: 0x000AA048
		public static Item getUncommonItemForThisMineLevel(int level, Point location)
		{
			Dictionary<int, string> arg_3F_0 = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
			List<int> possibleWeapons = new List<int>();
			int closest = -1;
			int closestLevel = -1;
			int guassianC = 12;
			Random r = new Random(location.X * 1000 + location.Y + (int)Game1.uniqueIDForThisGame + level);
			foreach (KeyValuePair<int, string> kvp in arg_3F_0)
			{
				if (Game1.mine.mineLevel >= Convert.ToInt32(kvp.Value.Split(new char[]
				{
					'/'
				})[10]) && Convert.ToInt32(kvp.Value.Split(new char[]
				{
					'/'
				})[9]) != -1)
				{
					int baseLevel = Convert.ToInt32(kvp.Value.Split(new char[]
					{
						'/'
					})[9]);
					if (closest == -1 || closestLevel > Math.Abs(Game1.mine.mineLevel - baseLevel))
					{
						closest = kvp.Key;
						closestLevel = Convert.ToInt32(kvp.Value.Split(new char[]
						{
							'/'
						})[9]);
					}
					double gaussian = Math.Pow(2.7182818284590451, -Math.Pow((double)(Game1.mine.mineLevel - baseLevel), 2.0) / (double)(2 * (guassianC * guassianC)));
					if (r.NextDouble() < gaussian)
					{
						possibleWeapons.Add(kvp.Key);
					}
				}
			}
			possibleWeapons.Add(closest);
			return new MeleeWeapon(possibleWeapons.ElementAt(r.Next(possibleWeapons.Count)));
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000AC004 File Offset: 0x000AA204
		public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
		{
			return Utility.GetPointsOnLine(x0, y0, x1, y1, false);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000AC010 File Offset: 0x000AA210
		public static List<Vector2> getBorderOfThisRectangle(Microsoft.Xna.Framework.Rectangle r)
		{
			List<Vector2> border = new List<Vector2>();
			for (int i = r.X; i < r.Right; i++)
			{
				border.Add(new Vector2((float)i, (float)r.Y));
			}
			for (int j = r.Y + 1; j < r.Bottom; j++)
			{
				border.Add(new Vector2((float)(r.Right - 1), (float)j));
			}
			for (int k = r.Right - 2; k >= r.X; k--)
			{
				border.Add(new Vector2((float)k, (float)(r.Bottom - 1)));
			}
			for (int l = r.Bottom - 2; l >= r.Y + 1; l--)
			{
				border.Add(new Vector2((float)r.X, (float)l));
			}
			return border;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000AC0E4 File Offset: 0x000AA2E4
		public static Point getTranslatedPoint(Point p, int direction, int movementAmount)
		{
			switch (direction)
			{
			case 0:
				return new Point(p.X, p.Y - movementAmount);
			case 1:
				return new Point(p.X + movementAmount, p.Y);
			case 2:
				return new Point(p.X, p.Y + movementAmount);
			case 3:
				return new Point(p.X - movementAmount, p.Y);
			default:
				return p;
			}
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000AC15C File Offset: 0x000AA35C
		public static Vector2 getTranslatedVector2(Vector2 p, int direction, float movementAmount)
		{
			switch (direction)
			{
			case 0:
				return new Vector2(p.X, p.Y - movementAmount);
			case 1:
				return new Vector2(p.X + movementAmount, p.Y);
			case 2:
				return new Vector2(p.X, p.Y + movementAmount);
			case 3:
				return new Vector2(p.X - movementAmount, p.Y);
			default:
				return p;
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000AC1D2 File Offset: 0x000AA3D2
		public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1, bool ignoreSwap)
		{
			bool flag = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (flag)
			{
				int t = x0;
				x0 = y0;
				y0 = t;
				t = x1;
				x1 = y1;
				y1 = t;
			}
			if (!ignoreSwap && x0 > x1)
			{
				int t2 = x0;
				x0 = x1;
				x1 = t2;
				t2 = y0;
				y0 = y1;
				y1 = t2;
			}
			int num = x1 - x0;
			int num2 = Math.Abs(y1 - y0);
			int num3 = num / 2;
			int num4 = (y0 < y1) ? 1 : -1;
			int num5 = y0;
			int num6;
			for (int i = x0; i <= x1; i = num6 + 1)
			{
				yield return new Point(flag ? num5 : i, flag ? i : num5);
				num3 -= num2;
				if (num3 < 0)
				{
					num5 += num4;
					num3 += num;
				}
				num6 = i;
			}
			yield break;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000AC200 File Offset: 0x000AA400
		public static Vector2 getRandomAdjacentOpenTile(Vector2 tile)
		{
			List<Vector2> i = Utility.getAdjacentTileLocations(tile);
			int iter = 0;
			int which = Game1.random.Next(i.Count);
			Vector2 v = i[which];
			while (iter < 4 && (Game1.currentLocation.isTileOccupiedForPlacement(v, null) || !Game1.currentLocation.isTilePassable(new Location((int)v.X, (int)v.Y), Game1.viewport)))
			{
				which = (which + 1) % i.Count;
				v = i[which];
				iter++;
			}
			if (iter >= 4)
			{
				return Vector2.Zero;
			}
			return v;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000AC28C File Offset: 0x000AA48C
		public static int getObjectIndexFromSlotCharacter(char character)
		{
			if (character <= '<')
			{
				if (character == '$')
				{
					return 398;
				}
				if (character == '*')
				{
					return 176;
				}
				if (character == '<')
				{
					return 400;
				}
			}
			else
			{
				if (character == '=')
				{
					return 72;
				}
				switch (character)
				{
				case '[':
					return 276;
				case '\\':
					return 336;
				case ']':
					return 221;
				default:
					if (character == '}')
					{
						return 184;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000AC300 File Offset: 0x000AA500
		private static string farmerAccomplishments()
		{
			string accomplishments = (Game1.player.isMale ? " he's " : " she's ") + "worked hard on " + (Game1.player.isMale ? "his" : "her") + " farm, ";
			if (Game1.player.hasRustyKey)
			{
				accomplishments += " made significant contributions to the field of archaeology, ";
			}
			if (Game1.player.achievements.Contains(71))
			{
				accomplishments += "has been a big help around town, ";
			}
			if (Game1.player.achievements.Contains(45))
			{
				accomplishments += "has come to be loved by many residents, ";
			}
			if (accomplishments.Length > 115)
			{
				accomplishments += "#$b#";
			}
			if (Game1.player.achievements.Contains(63))
			{
				accomplishments += "has become a local fishing legend, ";
			}
			if (Game1.player.timesReachedMineBottom > 0)
			{
				accomplishments += "has explored the shadowy depths of the local mine, ";
			}
			return string.Concat(new object[]
			{
				accomplishments,
				" and has earned over ",
				Game1.player.totalMoneyEarned - Game1.player.totalMoneyEarned % 1000u,
				"g in revenue."
			});
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000AC438 File Offset: 0x000AA638
		public static string getCreditsString()
		{
			return string.Concat(new string[]
			{
				string.Concat(new string[]
				{
					"Credits:",
					Environment.NewLine,
					" ",
					Environment.NewLine,
					Environment.NewLine,
					Environment.NewLine,
					"Programming,",
					Environment.NewLine,
					"Art, Music,",
					Environment.NewLine,
					"Sound Effects, Writing:",
					Environment.NewLine,
					Environment.NewLine,
					"-Eric Barone",
					Environment.NewLine,
					" ",
					Environment.NewLine,
					Environment.NewLine,
					Environment.NewLine,
					"Special Thanks:",
					Environment.NewLine,
					Environment.NewLine
				}),
				"-Amber Hageman",
				Environment.NewLine,
				"-Shane Waletzko",
				Environment.NewLine,
				"-Fiddy, Nuns, Kappy &",
				Environment.NewLine,
				"everyone from HarvestCraft",
				Environment.NewLine,
				"-Trader Joe's Organic Hummus",
				Environment.NewLine,
				Environment.NewLine,
				Environment.NewLine,
				Environment.NewLine,
				"Thanks for playing! :)"
			});
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000AC594 File Offset: 0x000AA794
		public static string getStardewHeroCelebrationEventString(int finalFarmerScore)
		{
			string eventString;
			if (finalFarmerScore >= Game1.percentageToWinStardewHero)
			{
				eventString = string.Concat(new object[]
				{
					"title_day/-100 -100/farmer 18 20 1 rival 27 20 2",
					Utility.getCelebrationPositionsForDatables(Game1.player.spouse),
					(Game1.player.spouse != null && !Game1.player.spouse.Contains("engaged")) ? (Game1.player.spouse + " 17 21 1 ") : "",
					"Lewis 22 19 2 Marnie 21 22 0 Caroline 24 22 0 Pierre 25 22 0 Gus 26 22 0 Clint 26 23 0 Emily 25 23 0 Shane 27 23 0 ",
					(Game1.player.friendships.ContainsKey("Sandy") && Game1.player.friendships["Sandy"][0] > 0) ? "Sandy 24 23 0 " : "",
					"George 21 23 0 Evelyn 20 23 0 Pam 19 23 0 Jodi 27 24 0 ",
					(Game1.getCharacterFromName("Kent", false) != null) ? "Kent 26 24 0 " : "",
					"Linus 24 24 0 Robin 21 24 0 Demetrius 20 24 0",
					(Game1.player.timesReachedMineBottom > 0) ? " Dwarf 19 24 0" : "",
					"/addObject 18 19 ",
					Game1.random.Next(313, 320),
					"/addObject 19 19 ",
					Game1.random.Next(313, 320),
					"/addObject 20 19 ",
					Game1.random.Next(313, 320),
					"/addObject 25 19 ",
					Game1.random.Next(313, 320),
					"/addObject 26 19 ",
					Game1.random.Next(313, 320),
					"/addObject 27 19 ",
					Game1.random.Next(313, 320),
					"/addObject 23 19 468/viewport 22 20 true/pause 4000/speak Lewis \"Welcome, everyone, to the 16th Centennial Stardew Hero Celebration!$h#$b#We've been a long time without a Stardew Hero, and I think we can all agree it's time to bestow this prestigious award on someone new!\"/pause 400/faceDirection Lewis 3/pause 500/faceDirection Lewis 1/pause 600/faceDirection Lewis 2/speak Lewis \"...The title of Stardew Hero is awarded only to those who've displayed extraordinary commitment to this valley and the people who live here.\"/pause 200/showRivalFrame 16/pause 600/speak Lewis \"There are many here who've done a lot for this community, but unfortunately there can only be one Stardew Hero.#$b#...And standing before you today are the two top contenders. @ and %rival.\"/pause 700/move Lewis 0 1 3/stopMusic/move Lewis -2 0 3/playMusic musicboxsong/faceDirection farmer 1/showRivalFrame 12/speak Lewis \"@ moved here 2 years ago, and in that time ",
					Utility.farmerAccomplishments(),
					"#$b#...Those kinds of accomplishments could breathe life into any community!\"/pause 800/move Lewis 5 0 1/showRivalFrame 12/playMusic rival/pause 500/speak Lewis \"%rival also moved here 2 years ago. In that time ",
					Game1.player.isMale ? "he's" : "she's",
					" created one of the most productive farms I've ever seen!$h#$b#...And funneled a large sum into my personal bank acc... er, I mean campaign fund!\"/pause 500/speak rival \"Thank you, Mayor, sir!$h\"/move rival 0 1 2/showRivalFrame 17/pause 500/speak rival \"I would like to to say how much of a *blessing* it's been to meet all of you wonderful people! I'm so happy to know how important I am in making this community great.$h#$b#Unfortunately, @ was so jealous ",
					(Game1.random.NextDouble() < 0.5) ? "of me" : "of my incredible farm",
					" that ",
					Game1.player.isMale ? "he" : "she",
					" did everything possible to drag me down!$u#$b#But, against all odds, I've single-handedly transformed this valley into a new paradise, and touched your hearts in a way you'll never forget. *sniff*\"/pause 600/emote farmer 40/showRivalFrame 16/pause 900/move rival 0 -1 2/showRivalFrame 16/move Lewis -3 0 2/stopMusic/pause 500/speak Lewis \"...Without further ado, ladies and gentleman...\"/stopMusic/move Lewis 0 -1 2/pause 600/faceDirection Lewis 1/pause 600/faceDirection Lewis 3/pause 600/faceDirection Lewis 2/speak Lewis \"The next Stardew Hero will be...\"/pause 300/move rival -2 0 2/showRivalFrame 16/pause 1500/speak Lewis \"...@.$h\"/pause 500/showRivalFrame 18/pause 400/playMusic happy/emote farmer 16/move farmer 5 0 2/move Lewis 0 1 1/speak Lewis \"Congratulations, @! You earned it with a whopping ",
					finalFarmerScore,
					" points!$h\"/speak Emily \"Way to go, @!$h\"/speak Gus \"Good job, buddy!\"/speak Pierre \"I told you my seeds would get the job done!$h\"/showRivalFrame 12/pause 500/speak rival \"This is the stupidest town I've ever set my foot in!$s\"/speed rival 4/move rival 6 0 0/faceDirection farmer 1 true/speed rival 4/move rival 0 -10 1/warp rival -100 -100/move farmer 0 1 2/emote farmer 20/fade/viewport -1000 -1000/message \"",
					Utility.getOtherFarmerNames()[0],
					" was never heard from again...\"/end credits"
				});
			}
			else
			{
				eventString = string.Concat(new object[]
				{
					"title_day/-100 -100/farmer 18 20 1 rival 27 20 2",
					Utility.getCelebrationPositionsForDatables(Game1.player.spouse),
					(Game1.player.spouse != null && !Game1.player.spouse.Contains("engaged")) ? (Game1.player.spouse + " 17 21 1 ") : "",
					"Lewis 22 19 2 Marnie 21 22 0 Caroline 24 22 0 Pierre 25 22 0 Gus 26 22 0 Clint 26 23 0 Emily 25 23 0 Shane 27 23 0 ",
					(Game1.player.friendships.ContainsKey("Sandy") && Game1.player.friendships["Sandy"][0] > 0) ? "Sandy 24 23 0 " : "",
					"George 21 23 0 Evelyn 20 23 0 Pam 19 23 0 Jodi 27 24 0 ",
					(Game1.getCharacterFromName("Kent", false) != null) ? "Kent 26 24 0 " : "",
					"Linus 24 24 0 Robin 21 24 0 Demetrius 20 24 0",
					(Game1.player.timesReachedMineBottom > 0) ? " Dwarf 19 24 0" : "",
					"/addObject 18 19 ",
					Game1.random.Next(313, 320),
					"/addObject 19 19 ",
					Game1.random.Next(313, 320),
					"/addObject 20 19 ",
					Game1.random.Next(313, 320),
					"/addObject 25 19 ",
					Game1.random.Next(313, 320),
					"/addObject 26 19 ",
					Game1.random.Next(313, 320),
					"/addObject 27 19 ",
					Game1.random.Next(313, 320),
					"/addObject 23 19 468/viewport 22 20 true/pause 4000/speak Lewis \"Welcome, everyone, to the 16th Centennial Stardew Hero Celebration!$h#$b#We've been a long time without a Stardew Hero, and I think we can all agree it's time to bestow this prestigious award on someone new!\"/pause 400/faceDirection Lewis 3/pause 500/faceDirection Lewis 1/pause 600/faceDirection Lewis 2/speak Lewis \"...The title of Stardew Hero is awarded only to those who've displayed extraordinary commitment to this valley and the people who live here.\"/pause 200/showRivalFrame 16/pause 600/speak Lewis \"There are many here who've done a lot for this community, but unfortunately there can only be one Stardew Hero.#$b#...And standing before you today are the two top contenders. @ and %rival.\"/pause 700/move Lewis 0 1 3/stopMusic/move Lewis -2 0 3/playMusic musicboxsong/faceDirection farmer 1/showRivalFrame 12/speak Lewis \"@ moved here 2 years ago, and in that time ",
					Utility.farmerAccomplishments(),
					"#$b#...Those kinds of accomplishments could breathe life into any community!\"/pause 800/move Lewis 5 0 1/showRivalFrame 12/playMusic rival/pause 500/speak Lewis \"%rival also moved here 2 years ago. In that time ",
					Game1.player.isMale ? "he's" : "she's",
					" created one of the most productive farms I've ever seen!$h#$b#...And funneled a large sum into my personal bank acc... er, I mean campaign fund!\"/pause 500/speak rival \"Thank you, Mayor, sir!$h\"/move rival 0 1 2/showRivalFrame 17/pause 500/speak rival \"I would like to to say how much of a *blessing* it's been to meet all of you wonderful people! I'm so happy to know how important I am in making this community great.$h#$b#Unfortunately, @ was so jealous ",
					(Game1.random.NextDouble() < 0.5) ? "of me" : "of my incredible farm",
					" that ",
					Game1.player.isMale ? "he" : "she",
					" did everything possible to drag me down!$u#$b#But, against all odds, I've single-handedly transformed this valley into a new paradise, and touched your hearts in a way you'll never forget. *sniff*\"/pause 600/emote farmer 40/showRivalFrame 16/pause 900/move rival 0 -1 2/showRivalFrame 16/move Lewis -3 0 2/stopMusic/pause 500/speak Lewis \"...Without further ado, ladies and gentleman...\"/stopMusic/move Lewis 0 -1 2/pause 600/faceDirection Lewis 1/pause 600/faceDirection Lewis 3/pause 600/faceDirection Lewis 2/speak Lewis \"The next Stardew Hero will be...\"/pause 300/move rival -2 0 2/showRivalFrame 16/pause 1500/speak Lewis \"...%rival.$h\"/pause 200/showFrame 32/move rival -2 0 2/showRivalFrame 19/pause 400/playSound death/emote farmer 28/speak Lewis \"Well, congratulations %rival... It looks like you outscored @ by ",
					Game1.percentageToWinStardewHero - finalFarmerScore,
					" points. $h\"/speak rival \"Of course I did. I never considered @ a threat even for a millisecond.$u\"/pause 600/faceDirection Lewis 3/speak Lewis \"...Sorry @. You did well, but you didn't accomplish enough to win.$s\"/speak Emily \"Don't be sad, @... We still love you.$h\"/fade/viewport -1000 -1000/message \"Final score: ",
					finalFarmerScore,
					Environment.NewLine,
					"Better luck next time!\"/end credits"
				});
			}
			return eventString;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000ACAF0 File Offset: 0x000AACF0
		public static void perpareDayForStardewCelebration(int finalFarmerScore)
		{
			bool farmerWon = finalFarmerScore >= Game1.percentageToWinStardewHero;
			using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (NPC i in enumerator.Current.characters)
					{
						string dialogue = "";
						if (farmerWon)
						{
							switch (Game1.random.Next(6))
							{
							case 0:
								dialogue = "Congratulations, @! I knew you'd win.$h";
								break;
							case 1:
								dialogue = "We all knew you were going to win, @. %rival was kind of a jerk.";
								break;
							case 2:
								dialogue = "Stardew Hero... I'm impressed!$h";
								break;
							case 3:
								dialogue = "It's such a great honor you received, @! But you earned it.";
								break;
							case 4:
								dialogue = "I'm so glad %rival didn't win.";
								break;
							case 5:
								dialogue = "You did it, @!";
								break;
							}
							if (i.name.Equals("Sebastian") || i.name.Equals("Abigail"))
							{
								dialogue = "Good job, " + (Game1.player.isMale ? "dude.$h" : "@.$h");
							}
							else if (i.name.Equals("George"))
							{
								dialogue = "Hmmph. They'd never give that award to an old timer like me.";
							}
						}
						else
						{
							switch (Game1.random.Next(4))
							{
							case 0:
								dialogue = "Sorry, @...";
								break;
							case 1:
								dialogue = "Maybe in 'another life' you'll win...";
								break;
							case 2:
								dialogue = "%rival won? I'm still shocked...";
								break;
							case 3:
								dialogue = "Ugh... how in the world did %rival win? That's awful.";
								break;
							}
							if (i.name.Equals("George"))
							{
								dialogue = "Hmmph. They'd never give that award to an old timer like me.";
							}
						}
						i.CurrentDialogue.Push(new Dialogue(dialogue, i));
					}
				}
			}
			if (farmerWon)
			{
				Game1.player.stardewHero = true;
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x000ACCFC File Offset: 0x000AAEFC
		public static string getCelebrationPositionsForDatables(string personToExclude)
		{
			if (personToExclude == null)
			{
				personToExclude = "";
			}
			string positions = " ";
			if (!personToExclude.Equals("Sam"))
			{
				positions += "Sam 25 65 0 ";
			}
			if (!personToExclude.Equals("Sebastian"))
			{
				positions += "Sebastian 24 65 0 ";
			}
			if (!personToExclude.Equals("Alex"))
			{
				positions += "Alex 25 69 0 ";
			}
			if (!personToExclude.Equals("Harvey"))
			{
				positions += "Harvey 23 67 0 ";
			}
			if (!personToExclude.Equals("Elliott"))
			{
				positions += "Elliott 32 65 0 ";
			}
			if (!personToExclude.Equals("Haley"))
			{
				positions += "Haley 26 69 0 ";
			}
			if (!personToExclude.Equals("Penny"))
			{
				positions += "Penny 23 66 0 ";
			}
			if (!personToExclude.Equals("Maru"))
			{
				positions += "Maru 24 68 0 ";
			}
			if (!personToExclude.Equals("Leah"))
			{
				positions += "Leah 33 65 0 ";
			}
			if (!personToExclude.Equals("Abigail"))
			{
				positions += "Abigail 23 65 0 ";
			}
			return positions;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x000ACE14 File Offset: 0x000AB014
		public static void fixAllAnimals()
		{
			Farm f = Game1.getFarm();
			foreach (Building b in f.buildings)
			{
				if (b.indoors != null && b.indoors is AnimalHouse)
				{
					using (List<long>.Enumerator enumerator2 = (b.indoors as AnimalHouse).animalsThatLiveHere.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FarmAnimal a = Utility.getAnimal(enumerator2.Current);
							if (a != null)
							{
								a.home = b;
								a.homeLocation = new Vector2((float)b.tileX, (float)b.tileY);
								a.setRandomPosition(a.home.indoors);
								if (!(a.home.indoors as AnimalHouse).animals.ContainsKey(a.myID))
								{
									(a.home.indoors as AnimalHouse).animals.Add(a.myID, a);
								}
							}
						}
					}
				}
			}
			List<FarmAnimal> buggedAnimals = new List<FarmAnimal>();
			foreach (FarmAnimal a2 in f.getAllFarmAnimals())
			{
				if (a2.home == null)
				{
					buggedAnimals.Add(a2);
				}
			}
			foreach (FarmAnimal a3 in buggedAnimals)
			{
				foreach (Building b2 in f.buildings)
				{
					if (b2.indoors != null && b2.indoors is AnimalHouse)
					{
						for (int i = (b2.indoors as AnimalHouse).animals.Count - 1; i >= 0; i--)
						{
							if ((b2.indoors as AnimalHouse).animals.ElementAt(i).Value.Equals(a3))
							{
								(b2.indoors as AnimalHouse).animals.Remove((b2.indoors as AnimalHouse).animals.ElementAt(i).Key);
							}
						}
					}
				}
				for (int j = f.animals.Count - 1; j >= 0; j--)
				{
					if (f.animals.ElementAt(j).Value.Equals(a3))
					{
						f.animals.Remove(f.animals.ElementAt(j).Key);
					}
				}
			}
			foreach (Building b3 in f.buildings)
			{
				if (b3.indoors != null && b3.indoors is AnimalHouse)
				{
					for (int k = (b3.indoors as AnimalHouse).animalsThatLiveHere.Count - 1; k >= 0; k--)
					{
						if (Utility.getAnimal((b3.indoors as AnimalHouse).animalsThatLiveHere[k]).home != b3)
						{
							(b3.indoors as AnimalHouse).animalsThatLiveHere.RemoveAt(k);
						}
					}
				}
			}
			foreach (FarmAnimal a4 in buggedAnimals)
			{
				foreach (Building b4 in f.buildings)
				{
					if (b4.buildingType.Contains(a4.buildingTypeILiveIn) && b4.indoors != null && b4.indoors is AnimalHouse && !(b4.indoors as AnimalHouse).isFull())
					{
						a4.home = b4;
						a4.homeLocation = new Vector2((float)b4.tileX, (float)b4.tileY);
						a4.setRandomPosition(a4.home.indoors);
						(a4.home.indoors as AnimalHouse).animals.Add(a4.myID, a4);
						(a4.home.indoors as AnimalHouse).animalsThatLiveHere.Add(a4.myID);
						break;
					}
				}
			}
			List<FarmAnimal> leftovers = new List<FarmAnimal>();
			foreach (FarmAnimal a5 in buggedAnimals)
			{
				if (a5.home == null)
				{
					leftovers.Add(a5);
				}
			}
			foreach (FarmAnimal a6 in leftovers)
			{
				a6.position = Utility.recursiveFindOpenTileForCharacter(a6, f, new Vector2(40f, 40f), 200) * (float)Game1.tileSize;
				if (!f.animals.ContainsKey(a6.myID))
				{
					f.animals.Add(a6.myID, a6);
				}
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000AD49C File Offset: 0x000AB69C
		public static string getWeddingEvent()
		{
			return string.Concat(new object[]
			{
				"sweet/-1000 -100/farmer 27 63 2 spouse 28 63 2",
				Utility.getCelebrationPositionsForDatables(Game1.player.spouse),
				"Lewis 27 64 2 Marnie 26 65 0 Caroline 29 65 0 Pierre 30 65 0 Gus 31 65 0 Clint 31 66 0 ",
				Game1.player.spouse.Contains("Emily") ? "" : "Emily 30 66 0 ",
				Game1.player.spouse.Contains("Shane") ? "" : "Shane 32 66 0 ",
				(Game1.player.friendships.ContainsKey("Sandy") && Game1.player.friendships["Sandy"][0] > 0) ? "Sandy 29 66 0 " : "",
				"George 26 66 0 Evelyn 25 66 0 Pam 24 66 0 Jodi 32 67 0 ",
				(Game1.getCharacterFromName("Kent", false) != null) ? "Kent 31 67 0 " : "",
				"Linus 29 67 0 Robin 25 67 0 Demetrius 26 67 0 Vincent 26 68 3 Jas 25 68 1",
				(Game1.player.timesReachedMineBottom > 0) ? " Dwarf 30 67 0" : "",
				"/showFrame spouse 36/specificTemporarySprite wedding/viewport 27 64 true/pause 4000/showFrame 133/pause 2000/speak Lewis \"When @ first arrived in Pelican Town, no one knew if ",
				Game1.player.IsMale ? "he'd" : "she'd",
				" fit in with our community...#$b#But from this day forward, @ is going to be as much a part of this town as any of us!$h#$b#It is my great honor on this day ",
				Game1.dayOfMonth,
				" of ",
				Game1.currentSeason,
				", to unite @ and %spouse in the bonds of marriage.\"/faceDirection farmer 1/showFrame spouse 37/pause 500/faceDirection Lewis 0/pause 2000/speak Lewis \"Well, let's get right to it!\"/move Lewis 0 1 0/playMusic none/pause 1000/showFrame Lewis 20/speak Lewis \"@... %spouse... #$b# As the mayor of pelican town, and regional bearer of the matrimonial seal, I now prounounce you ",
				Game1.player.IsMale ? "husband" : "wife",
				" and ",
				Utility.isMale(Game1.player.spouse) ? (Game1.player.IsMale ? "..., well, husband" : "husband") : ((!Game1.player.IsMale) ? "..., well, wife" : "wife"),
				"!$h\"/pause 500/speak Lewis \"You may kiss.\"/pause 1000/showFrame 101/showFrame spouse 38/specificTemporarySprite heart 27 63/playSound dwop/pause 2000/specificTemporarySprite wed/warp Marnie -2000 -2000/faceDirection farmer 2/showFrame spouse 36/faceDirection Pam 1 true/faceDirection Evelyn 3 true/faceDirection Pierre 3 true/faceDirection Caroline 1 true/animate Robin false true 500 20 21 20 22/animate Demetrius false true 500 24 25 24 26/move Lewis 0 3 3 true/move Caroline 0 -1 3 false/pause 4000/faceDirection farmer 1/showFrame spouse 37/globalFade/viewport -1000 -1000/pause 1000/message \"Life is going to be different from now on...\"/pause 500/message \"...But the future looks bright!\"/pause 4000/end wedding"
			});
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000AD674 File Offset: 0x000AB874
		public static void drawTinyDigits(int toDraw, SpriteBatch b, Vector2 position, float scale, float layerDepth, Color c)
		{
			int xPosition = 0;
			int currentValue = toDraw;
			int numDigits = 0;
			do
			{
				numDigits++;
			}
			while ((toDraw /= 10) >= 1);
			int digitStrip = (int)Math.Pow(10.0, (double)(numDigits - 1));
			bool significant = false;
			for (int i = 0; i < numDigits; i++)
			{
				int currentDigit = currentValue / digitStrip % 10;
				if (currentDigit > 0 || i == numDigits - 1)
				{
					significant = true;
				}
				if (significant)
				{
					b.Draw(Game1.mouseCursors, position + new Vector2((float)xPosition, 0f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(368 + currentDigit * 5, 56, 5, 7)), c, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
				}
				xPosition += (int)(5f * scale) - 1;
				digitStrip /= 10;
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000AD730 File Offset: 0x000AB930
		public static int getWidthOfTinyDigitString(int toDraw, float scale)
		{
			int numDigits = 0;
			do
			{
				numDigits++;
			}
			while ((toDraw /= 10) >= 1);
			return (int)((float)(numDigits * 5) * scale);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x000AD754 File Offset: 0x000AB954
		public static bool isMale(string who)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(who);
			if (num <= 2434294092u)
			{
				if (num != 587846041u)
				{
					if (num != 1281010426u)
					{
						if (num != 2434294092u)
						{
							return true;
						}
						if (!(who == "Haley"))
						{
							return true;
						}
					}
					else if (!(who == "Maru"))
					{
						return true;
					}
				}
				else if (!(who == "Penny"))
				{
					return true;
				}
			}
			else if (num <= 2732913340u)
			{
				if (num != 2571828641u)
				{
					if (num != 2732913340u)
					{
						return true;
					}
					if (!(who == "Abigail"))
					{
						return true;
					}
				}
				else if (!(who == "Emily"))
				{
					return true;
				}
			}
			else if (num != 2826247323u)
			{
				if (num != 4194582670u)
				{
					return true;
				}
				if (!(who == "Sandy"))
				{
					return true;
				}
			}
			else if (!(who == "Leah"))
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000AD824 File Offset: 0x000ABA24
		public static bool doesItemWithThisIndexExistAnywhere(int index, bool bigCraftable = false)
		{
			for (int i = Game1.player.items.Count - 1; i >= 0; i--)
			{
				if (Game1.player.items[i] != null && Game1.player.items[i] is Object && Game1.player.items[i].parentSheetIndex == index && (Game1.player.items[i] as Object).bigCraftable == bigCraftable)
				{
					return true;
				}
			}
			foreach (GameLocation j in Game1.locations)
			{
				foreach (KeyValuePair<Vector2, Object> v in j.objects)
				{
					if (v.Value != null)
					{
						if (v.Value.parentSheetIndex == index && v.Value.bigCraftable == bigCraftable)
						{
							bool result = true;
							return result;
						}
						if (v.Value is Chest)
						{
							foreach (Item k in (v.Value as Chest).items)
							{
								if (k != null && k is Object && k.parentSheetIndex == index && (k as Object).bigCraftable == bigCraftable)
								{
									bool result = true;
									return result;
								}
							}
						}
						if (v.Value.heldObject != null && v.Value.heldObject.parentSheetIndex == index && v.Value.heldObject.bigCraftable == bigCraftable)
						{
							bool result = true;
							return result;
						}
					}
				}
				foreach (Debris d in j.debris)
				{
					if (d.item != null && d.item is Object && d.item.parentSheetIndex == index && (d.item as Object).bigCraftable == bigCraftable)
					{
						bool result = true;
						return result;
					}
				}
				if (j is Farm)
				{
					using (List<Building>.Enumerator enumerator5 = (j as Farm).buildings.GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							Building b = enumerator5.Current;
							if (b.indoors != null)
							{
								foreach (KeyValuePair<Vector2, Object> v2 in b.indoors.objects)
								{
									if (v2.Value != null)
									{
										if (v2.Value.parentSheetIndex == index && v2.Value.bigCraftable == bigCraftable)
										{
											bool result = true;
											return result;
										}
										if (v2.Value is Chest)
										{
											foreach (Item l in (v2.Value as Chest).items)
											{
												if (l != null && l is Object && l.parentSheetIndex == index && (l as Object).bigCraftable == bigCraftable)
												{
													bool result = true;
													return result;
												}
											}
										}
									}
								}
								foreach (Debris d2 in b.indoors.debris)
								{
									if (d2.item != null && d2.item is Object && d2.item.parentSheetIndex == index && (d2.item as Object).bigCraftable == bigCraftable)
									{
										bool result = true;
										return result;
									}
								}
								if (!(b.indoors is DecoratableLocation))
								{
									continue;
								}
								using (List<Furniture>.Enumerator enumerator6 = (b.indoors as DecoratableLocation).furniture.GetEnumerator())
								{
									while (enumerator6.MoveNext())
									{
										Furniture f = enumerator6.Current;
										if (f.heldObject != null && f.heldObject.parentSheetIndex == index && f.bigCraftable == bigCraftable)
										{
											bool result = true;
											return result;
										}
									}
									continue;
								}
							}
							if (b is Mill)
							{
								using (List<Item>.Enumerator enumerator3 = (b as Mill).output.items.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										Item m = enumerator3.Current;
										if (m != null && m is Object && m.parentSheetIndex == index && (m as Object).bigCraftable == bigCraftable)
										{
											bool result = true;
											return result;
										}
									}
									continue;
								}
							}
							if (b is JunimoHut)
							{
								foreach (Item n in (b as JunimoHut).output.items)
								{
									if (n != null && n is Object && n.parentSheetIndex == index && (n as Object).bigCraftable == bigCraftable)
									{
										bool result = true;
										return result;
									}
								}
							}
						}
						goto IL_5AB;
					}
					goto IL_53A;
				}
				goto IL_53A;
				IL_5AB:
				if (j is DecoratableLocation)
				{
					foreach (Furniture f2 in (j as DecoratableLocation).furniture)
					{
						if (f2.heldObject != null && f2.heldObject.parentSheetIndex == index && f2.bigCraftable == bigCraftable)
						{
							bool result = true;
							return result;
						}
					}
					continue;
				}
				continue;
				IL_53A:
				if (j is FarmHouse)
				{
					foreach (Item i2 in (j as FarmHouse).fridge.items)
					{
						if (i2 != null && i2 is Object && i2.parentSheetIndex == index && (i2 as Object).bigCraftable == bigCraftable)
						{
							bool result = true;
							return result;
						}
					}
					goto IL_5AB;
				}
				goto IL_5AB;
			}
			return false;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000ADFA0 File Offset: 0x000AC1A0
		public static int getSwordUpgradeLevel()
		{
			foreach (Item t in Game1.player.items)
			{
				if (t != null && t.GetType() == typeof(Sword))
				{
					return ((Tool)t).upgradeLevel;
				}
			}
			Tool[] toolBox = Game1.player.toolBox;
			for (int i = 0; i < toolBox.Length; i++)
			{
				Tool t2 = toolBox[i];
				if (t2 != null && t2.name.Contains("Sword"))
				{
					return t2.upgradeLevel;
				}
			}
			return 0;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x000AE060 File Offset: 0x000AC260
		public static bool tryToAddObjectToHome(Object o)
		{
			GameLocation home = Game1.getLocationFromName("FarmHouse");
			for (int x = home.map.GetLayer("Back").LayerWidth - 1; x >= 0; x--)
			{
				for (int y = home.map.GetLayer("Back").LayerHeight - 1; y >= 0; y--)
				{
					if (home.map.GetLayer("Back").Tiles[x, y] != null && home.dropObject(o, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize)), Game1.viewport, false, null))
					{
						if (o.ParentSheetIndex == 468)
						{
							Object table = new Object(new Vector2((float)x, (float)y), 308, null, true, true, false, false);
							table.heldObject = o;
							home.objects[new Vector2((float)x, (float)y)] = table;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x000AE150 File Offset: 0x000AC350
		public static List<Color> getOverallsColors()
		{
			return new List<Color>
			{
				new Color(33, 36, 105),
				new Color(40, 68, 21),
				new Color(68, 44, 21),
				new Color(39, 10, 71),
				new Color(23, 64, 66),
				new Color(62, 23, 66),
				new Color(53, 11, 30),
				new Color(23, 21, 24),
				new Color(50, 50, 50),
				new Color(57, 54, 34),
				new Color(7, 84, 55),
				new Color(29, 32, 0),
				new Color(30, 1, 1),
				new Color(0, 30, 1),
				new Color(31, 0, 30)
			};
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000AE25C File Offset: 0x000AC45C
		public static List<Color> getShirtColors()
		{
			return new List<Color>
			{
				new Color(97, 96, 97),
				new Color(21, 22, 21),
				new Color(40, 90, 41),
				new Color(90, 41, 40),
				new Color(41, 40, 90),
				new Color(49, 0, 0),
				new Color(0, 51, 0),
				new Color(1, 0, 52),
				new Color(80, 1, 80),
				new Color(0, 84, 84),
				new Color(80, 84, 0)
			};
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000AE320 File Offset: 0x000AC520
		public static List<Color> getSkinColors()
		{
			return new List<Color>
			{
				new Color(96, 57, 19),
				new Color(170, 120, 18),
				new Color(150, 100, 18),
				new Color(85, 62, 10),
				new Color(60, 48, 11)
			};
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x000AE390 File Offset: 0x000AC590
		public static List<Color> getHairColors()
		{
			return new List<Color>
			{
				new Color(35, 21, 11),
				new Color(1, 1, 1),
				new Color(30, 20, 3),
				new Color(52, 26, 14),
				new Color(100, 80, 10),
				new Color(27, 38, 39),
				new Color(120, 90, 0),
				new Color(90, 29, 37),
				new Color(18, 15, 51),
				new Color(49, 17, 41),
				new Color(19, 50, 16),
				new Color(13, 46, 73)
			};
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000AE46C File Offset: 0x000AC66C
		public static List<Color> getEyeColors()
		{
			return new List<Color>
			{
				new Color(82, 49, 16),
				new Color(3, 3, 3),
				new Color(58, 64, 90),
				new Color(0, 0, 61),
				new Color(3, 75, 176),
				new Color(60, 11, 11),
				new Color(70, 11, 95),
				new Color(0, 79, 53),
				new Color(50, 26, 10),
				new Color(41, 52, 46),
				new Color(39, 0, 94),
				new Color(71, 57, 36)
			};
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000AE548 File Offset: 0x000AC748
		public static void changeFarmerSkinColor(Color baseColor)
		{
			int pixelX = 209;
			int pixelIndex = 3214 * Game1.player.Sprite.Texture.Bounds.Width + pixelX;
			if (baseColor.R < 60)
			{
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex, (int)baseColor.R, (int)baseColor.G, (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)(baseColor.R + 45), (int)(baseColor.G + 15), (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 50), (int)(baseColor.G + 20), (int)(baseColor.B + 5));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 60), (int)(baseColor.G + 30), (int)(baseColor.B + 10));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 90), (int)(baseColor.G + 50), (int)(baseColor.B + 20));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 120), (int)(baseColor.G + 70), (int)(baseColor.B + 30));
				return;
			}
			if (baseColor.R <= 85 || baseColor.B == 18)
			{
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex, (int)((baseColor.B == 18) ? (baseColor.R - baseColor.R / 2) : baseColor.R), (int)((baseColor.B == 18) ? (baseColor.G - baseColor.G / 2) : baseColor.G), (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)(baseColor.R + 65), (int)(baseColor.G + 20), (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 75), (int)(baseColor.G + 30), (int)(baseColor.B + 20));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 90), (int)(baseColor.G + 40), (int)(baseColor.B + 40));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 110), (int)(baseColor.G + 60), (int)(baseColor.B + 60));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 130), (int)(baseColor.G + 80), (int)(baseColor.B + 70));
				if (Game1.player.eyeColor == 0)
				{
					Game1.player.eyeColor = 1;
					Utility.changeFarmerEyeColor(Utility.getEyeColors()[Game1.player.eyeColor]);
					return;
				}
			}
			else
			{
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex, (int)baseColor.R, (int)baseColor.G, (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)(baseColor.R + 112), (int)(baseColor.G + 69), (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 129), (int)(baseColor.G + 115), (int)(baseColor.B + 86));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 149), (int)(baseColor.G + 129), (int)(baseColor.B + 93));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 150), (int)(baseColor.G + 145), (int)(baseColor.B + 118));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 157), (int)(baseColor.G + 169), (int)(baseColor.B + 160));
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000AEAD8 File Offset: 0x000ACCD8
		public static void changeFarmerHairColor(Color baseColor)
		{
			int pixelX = 201;
			int pixelIndex = 3206 * Game1.player.Sprite.Texture.Bounds.Width + pixelX;
			if (baseColor.B == 3)
			{
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)baseColor.R, (int)baseColor.G, (int)(baseColor.B + 1));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 10), (int)(baseColor.G + 5), (int)(baseColor.B + 2));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 20), (int)(baseColor.G + 10), (int)(baseColor.B + 3));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 30), (int)(baseColor.G + 20), (int)(baseColor.B + 4));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 40), (int)(baseColor.G + 30), (int)(baseColor.B + 5));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 24, (int)(baseColor.R + 50), (int)(baseColor.G + 40), (int)(baseColor.B + 6));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 28, (int)(baseColor.R + 60), (int)(baseColor.G + 50), (int)(baseColor.B + 8));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 32, (int)(baseColor.R + 80), (int)(baseColor.G + 70), (int)(baseColor.B + 20));
				return;
			}
			if (baseColor.B == 1)
			{
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)baseColor.R, (int)baseColor.G, (int)baseColor.B);
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 5), (int)(baseColor.G + 5), (int)(baseColor.B + 5));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 10), (int)(baseColor.G + 10), (int)(baseColor.B + 10));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 15), (int)(baseColor.G + 15), (int)(baseColor.B + 15));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 20), (int)(baseColor.G + 20), (int)(baseColor.B + 20));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 24, (int)(baseColor.R + 25), (int)(baseColor.G + 25), (int)(baseColor.B + 25));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 28, (int)(baseColor.R + 35), (int)(baseColor.G + 35), (int)(baseColor.B + 35));
				Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 32, (int)(baseColor.R + 50), (int)(baseColor.G + 50), (int)(baseColor.B + 50));
				return;
			}
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)(baseColor.R + 10), (int)(baseColor.G + 10), (int)(baseColor.B + 10));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 20), (int)(baseColor.G + 20), (int)(baseColor.B + 20));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 30), (int)(baseColor.G + 30), (int)(baseColor.B + 30));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 40), (int)(baseColor.G + 40), (int)(baseColor.B + 40));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 50), (int)(baseColor.G + 50), (int)(baseColor.B + 50));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 24, (int)(baseColor.R + 60), (int)(baseColor.G + 60), (int)(baseColor.B + 60));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 28, (int)(baseColor.R + 80), (int)(baseColor.G + 80), (int)(baseColor.B + 80));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 32, (int)(baseColor.R + 120), (int)(baseColor.G + 120), (int)(baseColor.B + 120));
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000AF184 File Offset: 0x000AD384
		public static void changeFarmerShirtColor(Color baseColor)
		{
			int pixelX = 213;
			int pixelIndex = 3221 * Game1.player.Sprite.Texture.Bounds.Width + pixelX;
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex, (int)baseColor.R, (int)baseColor.G, (int)baseColor.B);
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)(baseColor.R + 50), (int)(baseColor.G + 50), (int)(baseColor.B + 50));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 100), (int)(baseColor.G + 100), (int)(baseColor.B + 100));
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000AF27C File Offset: 0x000AD47C
		public static void changeFarmerEyeColor(Color baseColor)
		{
			int pixelX = 217;
			int pixelIndex = 3230 * Game1.player.Sprite.Texture.Bounds.Width + pixelX;
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex, (int)baseColor.R, (int)baseColor.G, (int)baseColor.B);
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex - 4, (int)(baseColor.R + 50), (int)(baseColor.G + 50), (int)(baseColor.B + 50));
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000AF330 File Offset: 0x000AD530
		public static string InvokeSimpleReturnTypeMethod(object toBeCalled, string methodName, object[] parameters)
		{
			Type calledType = toBeCalled.GetType();
			string s = "";
			try
			{
				s = (((string)calledType.InvokeMember(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, toBeCalled, parameters)) ?? "");
			}
			catch (Exception e)
			{
				s = Game1.parseText("Didn't work - " + e.Message);
			}
			return s;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000AF394 File Offset: 0x000AD594
		public static List<int> possibleCropsAtThisTime(string season, bool firstWeek)
		{
			List<int> firstWeekCrops = null;
			List<int> secondWeekCrops = null;
			if (season.Equals("spring"))
			{
				firstWeekCrops = new List<int>
				{
					24,
					192
				};
				if (Game1.year > 1)
				{
					firstWeekCrops.Add(250);
				}
				if (Game1.player.eventsSeen.Contains(61))
				{
					firstWeekCrops.Add(248);
				}
				secondWeekCrops = new List<int>
				{
					190,
					188
				};
				if (Game1.player.eventsSeen.Contains(61))
				{
					secondWeekCrops.Add(252);
				}
				secondWeekCrops.AddRange(firstWeekCrops);
			}
			else if (season.Equals("summer"))
			{
				firstWeekCrops = new List<int>
				{
					264,
					262,
					260
				};
				secondWeekCrops = new List<int>
				{
					254,
					256
				};
				if (Game1.year > 1)
				{
					firstWeekCrops.Add(266);
				}
				if (Game1.player.eventsSeen.Contains(61))
				{
					secondWeekCrops.AddRange(new int[]
					{
						258,
						268
					});
				}
				secondWeekCrops.AddRange(firstWeekCrops);
			}
			else if (season.Equals("fall"))
			{
				firstWeekCrops = new List<int>
				{
					272,
					278
				};
				secondWeekCrops = new List<int>
				{
					270,
					276,
					280
				};
				if (Game1.year > 1)
				{
					secondWeekCrops.Add(274);
				}
				if (Game1.player.eventsSeen.Contains(61))
				{
					firstWeekCrops.Add(284);
					secondWeekCrops.Add(282);
				}
				secondWeekCrops.AddRange(firstWeekCrops);
			}
			if (firstWeek)
			{
				return firstWeekCrops;
			}
			return secondWeekCrops;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000AF584 File Offset: 0x000AD784
		public static int[] cropsOfTheWeek()
		{
			Random r = new Random((int)Game1.uniqueIDForThisGame + (int)(Game1.stats.DaysPlayed / 29u));
			int[] cropsOfTheWeek = new int[4];
			List<int> firstWeekCrops = Utility.possibleCropsAtThisTime(Game1.currentSeason, true);
			List<int> secondWeekCrops = Utility.possibleCropsAtThisTime(Game1.currentSeason, false);
			if (firstWeekCrops != null)
			{
				cropsOfTheWeek[0] = firstWeekCrops.ElementAt(r.Next(firstWeekCrops.Count));
				for (int i = 1; i < 4; i++)
				{
					cropsOfTheWeek[i] = secondWeekCrops.ElementAt(r.Next(secondWeekCrops.Count));
					while (cropsOfTheWeek[i] == cropsOfTheWeek[i - 1])
					{
						cropsOfTheWeek[i] = secondWeekCrops.ElementAt(r.Next(secondWeekCrops.Count));
					}
				}
			}
			return cropsOfTheWeek;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000AF630 File Offset: 0x000AD830
		public static int getRandomItemFromSeason(string season, int randomSeedAddition, bool forQuest)
		{
			Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + randomSeedAddition);
			List<int> possibleItems = new List<int>
			{
				68,
				66,
				78,
				80,
				86,
				152,
				167,
				153,
				420
			};
			if (Game1.mine != null)
			{
				if (Game1.mine.mineLevel > 40 || Game1.player.timesReachedMineBottom >= 1)
				{
					possibleItems.AddRange(new int[]
					{
						62,
						70,
						72,
						84,
						422
					});
				}
				if (Game1.mine.mineLevel > 80 || Game1.player.timesReachedMineBottom >= 1)
				{
					possibleItems.AddRange(new int[]
					{
						64,
						60,
						82
					});
				}
			}
			if (Game1.player.eventsSeen.Contains(61))
			{
				possibleItems.AddRange(new int[]
				{
					88,
					90,
					164,
					165
				});
			}
			if (Game1.player.craftingRecipes.Keys.Contains("Furnace"))
			{
				possibleItems.AddRange(new int[]
				{
					334,
					335,
					336,
					338
				});
			}
			if (Game1.player.craftingRecipes.Keys.Contains("Quartz Globe"))
			{
				possibleItems.Add(339);
			}
			if (season.Equals("spring"))
			{
				possibleItems.AddRange(new int[]
				{
					16,
					18,
					20,
					22,
					129,
					131,
					132,
					136,
					137,
					142,
					143,
					145,
					147,
					148,
					152,
					167
				});
			}
			else if (season.Equals("summer"))
			{
				possibleItems.AddRange(new int[]
				{
					128,
					130,
					131,
					132,
					136,
					138,
					142,
					144,
					145,
					146,
					149,
					150,
					155,
					396,
					398,
					400,
					402
				});
			}
			else if (season.Equals("fall"))
			{
				possibleItems.AddRange(new int[]
				{
					404,
					406,
					408,
					410,
					129,
					131,
					132,
					136,
					137,
					139,
					140,
					142,
					143,
					148,
					150,
					154,
					155
				});
			}
			else if (season.Equals("winter"))
			{
				possibleItems.AddRange(new int[]
				{
					412,
					414,
					416,
					418,
					130,
					131,
					132,
					136,
					140,
					141,
					143,
					144,
					146,
					147,
					150,
					151,
					154
				});
			}
			if (forQuest)
			{
				if (Game1.player.coopUpgradeLevel >= 1)
				{
					if (Game1.player.hasCoopDweller("WhiteChicken") || Game1.player.hasCoopDweller("BrownChicken") || Game1.player.hasCoopDweller("Duck"))
					{
						possibleItems.Add(-5);
					}
					if (Game1.player.hasCoopDweller("Rabbit"))
					{
						possibleItems.Add(440);
					}
				}
				if (Game1.player.BarnUpgradeLevel >= 1)
				{
					if (Game1.player.hasBarnDweller("WhiteBlackCow") || Game1.player.hasBarnDweller("Goat"))
					{
						possibleItems.Add(-6);
					}
					if (Game1.player.hasBarnDweller("Sheep") && !possibleItems.Contains(440))
					{
						possibleItems.Add(440);
					}
					if (Game1.player.hasBarnDweller("Pig"))
					{
						possibleItems.Add(430);
					}
				}
				foreach (string s in Game1.player.cookingRecipes.Keys)
				{
					if (r.NextDouble() >= 0.4)
					{
						List<int> cropsAvailableNow = Utility.possibleCropsAtThisTime(Game1.currentSeason, Game1.dayOfMonth <= 7);
						Dictionary<string, string> cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CookingRecipes");
						if (cookingRecipes.ContainsKey(s))
						{
							string[] ingredientsSplit = cookingRecipes[s].Split(new char[]
							{
								'/'
							})[0].Split(new char[]
							{
								' '
							});
							bool ingredientsAvailable = true;
							for (int i = 0; i < ingredientsSplit.Length; i++)
							{
								if (!possibleItems.Contains(Convert.ToInt32(ingredientsSplit[i])) && !Utility.isCategoryIngredientAvailable(Convert.ToInt32(ingredientsSplit[i])) && (cropsAvailableNow == null || !cropsAvailableNow.Contains(Convert.ToInt32(ingredientsSplit[i]))))
								{
									ingredientsAvailable = false;
									break;
								}
							}
							if (ingredientsAvailable)
							{
								possibleItems.Add(Convert.ToInt32(cookingRecipes[s].Split(new char[]
								{
									'/'
								})[2]));
							}
						}
					}
				}
			}
			return possibleItems.ElementAt(r.Next(possibleItems.Count));
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000AFA80 File Offset: 0x000ADC80
		private static bool isCategoryIngredientAvailable(int category)
		{
			return category < 0 && (category != -5 || Game1.player.hasCoopDweller("WhiteChicken") || Game1.player.hasCoopDweller("BrownChicken") || Game1.player.hasCoopDweller("Duck")) && (category != -6 || Game1.player.hasBarnDweller("WhiteBlackCow") || Game1.player.hasBarnDweller("Goat"));
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000AFAF8 File Offset: 0x000ADCF8
		public static int weatherDebrisOffsetForSeason(string season)
		{
			if (season == "spring")
			{
				return 16;
			}
			if (season == "summer")
			{
				return 24;
			}
			if (season == "fall")
			{
				return 18;
			}
			if (!(season == "winter"))
			{
				return 0;
			}
			return 20;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000AFB48 File Offset: 0x000ADD48
		public static Color getSkyColorForSeason(string season)
		{
			if (season == "spring")
			{
				return new Color(92, 170, 255);
			}
			if (season == "summer")
			{
				return new Color(24, 163, 255);
			}
			if (season == "fall")
			{
				return new Color(255, 184, 151);
			}
			if (!(season == "winter"))
			{
				return new Color(92, 170, 255);
			}
			return new Color(165, 207, 255);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000AFBEC File Offset: 0x000ADDEC
		public static void farmerHeardSong(string trackName)
		{
			string adjustedName = "coin";
			if (trackName.Equals("springsongs"))
			{
				if (Game1.player.songsHeard.Contains("Bouncy"))
				{
					return;
				}
				adjustedName = "Pink Petals";
			}
			else if (trackName.Equals("summersongs"))
			{
				if (Game1.player.songsHeard.Contains("Orange"))
				{
					return;
				}
				adjustedName = "Hometone";
			}
			else if (trackName.Equals("fallsongs"))
			{
				if (Game1.player.songsHeard.Contains("Majestic"))
				{
					return;
				}
				adjustedName = "Ghost Synth";
			}
			else if (trackName.Equals("wintersongs"))
			{
				if (Game1.player.songsHeard.Contains("Ancient"))
				{
					return;
				}
				adjustedName = "Ancient";
			}
			else if (trackName.Equals("EarthMine"))
			{
				if (Game1.player.songsHeard.Contains("Cavern"))
				{
					return;
				}
				adjustedName = "Secret Gnomes";
			}
			else if (trackName.Equals("FrostMine"))
			{
				if (Game1.player.songsHeard.Contains("Cloth"))
				{
					return;
				}
				adjustedName = "XOR";
			}
			else if (trackName.Equals("LavaMine"))
			{
				if (Game1.player.songsHeard.Contains("Of Dwarves"))
				{
					return;
				}
				adjustedName = "Of Dwarves";
			}
			else if (!trackName.Equals("none") && !trackName.Equals("rain"))
			{
				adjustedName = trackName;
			}
			if (!Game1.player.songsHeard.Contains(adjustedName))
			{
				Game1.player.songsHeard.Add(adjustedName);
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x000AFDD0 File Offset: 0x000ADFD0
		public static int percentGameComplete()
		{
			int percentage = 0;
			if (Game1.player.spouse != null && !Game1.player.spouse.Contains("engaged"))
			{
				percentage++;
			}
			if (Utility.playerHasGalaxySword())
			{
				percentage++;
			}
			percentage += Utility.itemsShippedPercent();
			percentage += Utility.upgradePercent();
			percentage += Utility.friendshipPercent();
			percentage += Utility.achievementsPercent();
			percentage += Utility.artifactsPercent();
			percentage += Utility.fishPercent();
			percentage += Utility.cosmicFruitPercent();
			percentage += Utility.cookingPercent();
			percentage += Utility.craftingPercent();
			percentage += Utility.minePercentage();
			return percentage + Game1.player.Level;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000AFE70 File Offset: 0x000AE070
		public static List<string> getOtherFarmerNames()
		{
			List<string> otherFarmerNames = new List<string>();
			Random r = new Random((int)Game1.uniqueIDForThisGame);
			Random dayRandom = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			string[] maleNames = new string[]
			{
				"Ron",
				"Desmond",
				"Gary",
				"Bart",
				"Willy",
				"Tex",
				"Chris",
				"Lenny",
				"Patrick",
				"Marty",
				"Jared",
				"Kyle",
				"Mitch",
				"Dale",
				"Leland",
				"Hunt",
				"Curtis",
				"Leone",
				"Andy",
				"Steve",
				"Frank",
				"Zach",
				"Bert",
				"Lucas",
				"Logan",
				"Stu",
				"Mike",
				"Jake",
				"Nick",
				"Ben",
				"Daniel",
				"Bubs",
				"Jack"
			};
			string[] femaleNames = new string[]
			{
				"Susan",
				"Danielle",
				"Rosie",
				"Joanie",
				"Emma",
				"Kate",
				"Pauline",
				"Bev",
				"Melissa",
				"Penny",
				"Nancy",
				"Betty",
				"Minnie",
				"Rebecca",
				"Holly",
				"Ashley",
				"Jasmine",
				"Nina",
				"Carly",
				"Jessica",
				"Samantha",
				"Amanda",
				"Brittany",
				"Liz",
				"Taylor",
				"Megan",
				"Hannah",
				"Lauren",
				"Stephanie"
			};
			string[] maletitles = new string[]
			{
				"Farmer",
				"Prospector",
				"Fisherman",
				"Woodsman",
				"Lumberjack",
				"Explorer",
				"Swordsman",
				"Rancher",
				"Cowboy",
				"Slick",
				"'King'",
				"Professor",
				"Seafarer",
				"Sailor",
				"Hotshot",
				"Hunter",
				"Warlock"
			};
			string[] femaletitles = new string[]
			{
				"Farmer",
				"Prospector",
				"Seafarer",
				"Herbalist",
				"Explorer",
				"Swordmaiden",
				"Rancher",
				"Cowgirl",
				"Sweet",
				"Cheerleader",
				"Sorceress",
				"Floralist"
			};
			string[] maleGarbagetitles = new string[]
			{
				"Geezer",
				"'Daddy'",
				"Big",
				"Lil'",
				"Plumber",
				"Great-Grandpa",
				"Bubba",
				"Doughboy",
				"Bag Boy",
				"Courtesy Clerk",
				"Banker",
				"Grocer",
				"Golf Pro",
				"Pirate",
				"Burglar",
				"Hamburger",
				"Cool Guy",
				"Simple",
				"Good Guy",
				"'Garbage'",
				"Math Whiz",
				"'Lucky'",
				"Middle Aged",
				"Software Developer",
				"Baker",
				"Business Major",
				"Pony Master",
				"Ol'"
			};
			string[] femaleGarbagetitles = new string[]
			{
				"Granny",
				"Old Mother",
				"Tiny",
				"Simple",
				"Scrapbook",
				"Log Lady",
				"Miss",
				"Clever",
				"Gossiping",
				"Prom Queen",
				"Diva",
				"Sweet Lil'",
				"Blushing",
				"Bashful",
				"Cat Lady",
				"Astronomer",
				"Housewife",
				"Gardener",
				"Computer Whiz",
				"Lunch Lady",
				"Bumpkin"
			};
			string[] malegarbageNicknames = new string[]
			{
				"'The Meatloaf'",
				"'The Boy Wonder'",
				"'The Wiz'",
				"'Super Legs'",
				"'The Nose'",
				"'The Duck'",
				"'Spoonface'",
				"'The Brain'",
				"'The Shark'"
			};
			string[] maleRivalTitles = new string[]
			{
				"Farmer",
				"Rancher",
				"Cowboy",
				"Farmboy"
			};
			string[] femaleRivalTitles = new string[]
			{
				"Farmer",
				"Rancher",
				"Cowgirl",
				"Farmgirl"
			};
			string name;
			if (Game1.player.isMale)
			{
				name = maleNames[r.Next(maleNames.Length)];
				for (int i = 0; i < 2; i++)
				{
					while (otherFarmerNames.Contains(name) || Game1.player.name.Equals(name))
					{
						if (i == 0)
						{
							name = maleNames[r.Next(maleNames.Length)];
						}
						else
						{
							name = maleNames[dayRandom.Next(maleNames.Length)];
						}
					}
					if (i == 0)
					{
						name = maleRivalTitles[r.Next(maleRivalTitles.Length)] + " " + name;
					}
					else
					{
						name = maletitles[dayRandom.Next(maletitles.Length)] + " " + name;
					}
					otherFarmerNames.Add(name);
				}
			}
			else
			{
				name = femaleNames[r.Next(femaleNames.Length)];
				for (int j = 0; j < 2; j++)
				{
					while (otherFarmerNames.Contains(name) || Game1.player.name.Equals(name))
					{
						if (j == 0)
						{
							name = femaleNames[r.Next(femaleNames.Length)];
						}
						else
						{
							name = femaleNames[dayRandom.Next(femaleNames.Length)];
						}
					}
					if (j == 0)
					{
						name = femaleRivalTitles[r.Next(femaleRivalTitles.Length)] + " " + name;
					}
					else
					{
						name = femaletitles[dayRandom.Next(femaletitles.Length)] + " " + name;
					}
					otherFarmerNames.Add(name);
				}
			}
			if (dayRandom.NextDouble() < 0.5)
			{
				name = maleNames[dayRandom.Next(maleNames.Length)];
				while (Game1.player.name.Equals(name))
				{
					name = maleNames[dayRandom.Next(maleNames.Length)];
				}
				if (dayRandom.NextDouble() < 0.5)
				{
					name = maleGarbagetitles[dayRandom.Next(maleGarbagetitles.Length)] + " " + name;
				}
				else
				{
					name = name + " " + malegarbageNicknames[dayRandom.Next(malegarbageNicknames.Length)];
				}
			}
			else
			{
				name = femaleNames[dayRandom.Next(femaleNames.Length)];
				while (Game1.player.name.Equals(name))
				{
					name = femaleNames[dayRandom.Next(femaleNames.Length)];
				}
				name = femaleGarbagetitles[dayRandom.Next(femaleGarbagetitles.Length)] + " " + name;
			}
			otherFarmerNames.Add(name);
			return otherFarmerNames;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000B0678 File Offset: 0x000AE878
		public static string getStardewHeroStandingsString()
		{
			string standings = "";
			Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed);
			List<string> otherFarmers = Utility.getOtherFarmerNames();
			int[] otherFarmerScores = new int[otherFarmers.Count];
			otherFarmerScores[0] = (int)(Game1.stats.DaysPlayed / 208f * (float)Game1.percentageToWinStardewHero);
			otherFarmerScores[1] = (int)((float)otherFarmerScores[0] * 0.75f + (float)r.Next(-5, 5));
			otherFarmerScores[2] = Math.Max(0, otherFarmerScores[1] / 2 + r.Next(-10, 0));
			if (Game1.stats.DaysPlayed < 30u)
			{
				otherFarmerScores[0] += 3;
			}
			else if (Game1.stats.DaysPlayed < 60u)
			{
				otherFarmerScores[0] += 7;
			}
			int farmerScore = Utility.percentGameComplete();
			bool farmerPlaced = false;
			for (int i = 0; i < 3; i++)
			{
				if (farmerScore > otherFarmerScores[i] && !farmerPlaced)
				{
					farmerPlaced = true;
					standings = string.Concat(new object[]
					{
						standings,
						Game1.player.getTitle(),
						" ",
						Game1.player.Name,
						" ....... ",
						farmerScore,
						" pts.",
						Environment.NewLine
					});
				}
				standings = string.Concat(new object[]
				{
					standings,
					otherFarmers[i],
					" ....... ",
					otherFarmerScores[i],
					" pts.",
					Environment.NewLine
				});
			}
			if (!farmerPlaced)
			{
				standings = string.Concat(new object[]
				{
					standings,
					Game1.player.getTitle(),
					" ",
					Game1.player.Name,
					" ....... ",
					farmerScore,
					" pts."
				});
			}
			return standings;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000B084B File Offset: 0x000AEA4B
		private static int cosmicFruitPercent()
		{
			return Math.Max(0, (Game1.player.MaxStamina - 120) / 20);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000B0863 File Offset: 0x000AEA63
		private static int minePercentage()
		{
			if (Game1.player.timesReachedMineBottom > 0)
			{
				return 4;
			}
			if (Game1.mine != null && Game1.mine.mineLevel >= 80)
			{
				return 2;
			}
			if (Game1.mine != null && Game1.mine.mineLevel >= 40)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x000B08A4 File Offset: 0x000AEAA4
		private static int cookingPercent()
		{
			int recipesCooked = 0;
			foreach (string s in Game1.player.cookingRecipes.Keys)
			{
				if (Game1.player.cookingRecipes[s] > 0)
				{
					recipesCooked++;
				}
			}
			return (int)((float)(recipesCooked / Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes").Count) * 3f);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000B0930 File Offset: 0x000AEB30
		private static int craftingPercent()
		{
			int recipesCrafted = 0;
			foreach (string s in Game1.player.craftingRecipes.Keys)
			{
				if (Game1.player.craftingRecipes[s] > 0)
				{
					recipesCrafted++;
				}
			}
			return (int)((float)(recipesCrafted / Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes").Count) * 3f);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x000B09BC File Offset: 0x000AEBBC
		private static int achievementsPercent()
		{
			return (int)((float)(Game1.player.achievements.Count / Game1.content.Load<Dictionary<int, string>>("Data\\achievements").Count) * 15f);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000B09EA File Offset: 0x000AEBEA
		private static int itemsShippedPercent()
		{
			return (int)((float)Game1.player.basicShipped.Count / 92f * 5f);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x000B0A09 File Offset: 0x000AEC09
		private static int artifactsPercent()
		{
			return (int)((float)Game1.player.archaeologyFound.Count / 32f * 3f);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x000B0A28 File Offset: 0x000AEC28
		private static int fishPercent()
		{
			return (int)((float)Game1.player.fishCaught.Count / 42f * 3f);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000B0A48 File Offset: 0x000AEC48
		private static int upgradePercent()
		{
			int upgradePercent = 0;
			foreach (Item t in Game1.player.items)
			{
				if (t != null && t.GetType() == typeof(Tool) && (t.Name.Contains("Hoe") || t.Name.Contains("Axe") || t.Name.Contains("Pickaxe") || t.Name.Contains("Can")) && ((Tool)t).upgradeLevel == 4)
				{
					upgradePercent++;
				}
			}
			Tool[] toolBox = Game1.player.toolBox;
			for (int i = 0; i < toolBox.Length; i++)
			{
				Tool t2 = toolBox[i];
				if (t2 != null && (t2.name.Contains("Hoe") || t2.name.Contains("Axe") || t2.name.Contains("Pickaxe") || t2.name.Contains("Can")) && t2.upgradeLevel == 4)
				{
					upgradePercent++;
				}
			}
			upgradePercent += Game1.player.HouseUpgradeLevel;
			upgradePercent += Game1.player.CoopUpgradeLevel;
			upgradePercent += Game1.player.BarnUpgradeLevel;
			if (Game1.player.hasGreenhouse)
			{
				upgradePercent++;
			}
			return upgradePercent;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000B0BCC File Offset: 0x000AEDCC
		private static int friendshipPercent()
		{
			int friendshipPoints = 0;
			foreach (string s in Game1.player.friendships.Keys)
			{
				friendshipPoints += Game1.player.friendships[s][0];
			}
			return Math.Min(10, (int)((float)friendshipPoints / 70000f * 10f));
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000B0C50 File Offset: 0x000AEE50
		private static bool playerHasGalaxySword()
		{
			foreach (Item t in Game1.player.Items)
			{
				if (t != null && t.GetType() == typeof(Sword) && t.Name.Contains("Galaxy"))
				{
					return true;
				}
			}
			Tool[] toolBox = Game1.player.toolBox;
			for (int i = 0; i < toolBox.Length; i++)
			{
				Tool t2 = toolBox[i];
				if (t2 != null && t2.name.Contains("Galaxy"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x000B0D10 File Offset: 0x000AEF10
		public static Quest getQuestOfTheDay()
		{
			if (Game1.stats.DaysPlayed <= 1u)
			{
				return null;
			}
			double d = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed).NextDouble();
			Quest quest;
			if (d < 0.08)
			{
				quest = new ResourceCollectionQuest();
			}
			else if (d < 0.18 && Game1.mine != null && Game1.stats.DaysPlayed > 5u)
			{
				quest = new SlayMonsterQuest();
			}
			else if (d < 0.53)
			{
				quest = null;
			}
			else if (d < 0.6)
			{
				quest = new FishingQuest();
			}
			else
			{
				quest = new ItemDeliveryQuest();
			}
			return quest;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000B0DB1 File Offset: 0x000AEFB1
		public static Color getOppositeColor(Color color)
		{
			return new Color((int)(255 - color.R), (int)(255 - color.G), (int)(255 - color.B));
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000B0DE0 File Offset: 0x000AEFE0
		public static void drawLightningBolt(Vector2 strikePosition, GameLocation l)
		{
			Microsoft.Xna.Framework.Rectangle lightningSourceRect = new Microsoft.Xna.Framework.Rectangle(644, 1078, 37, 57);
			Vector2 drawPosition = strikePosition + new Vector2((float)(-(float)lightningSourceRect.Width * Game1.pixelZoom / 2), (float)(-(float)lightningSourceRect.Height * Game1.pixelZoom));
			while (drawPosition.Y > (float)(-(float)lightningSourceRect.Height * Game1.pixelZoom))
			{
				l.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, lightningSourceRect, 9999f, 1, 999, drawPosition, false, Game1.random.NextDouble() < 0.5, (strikePosition.Y + (float)(Game1.tileSize / 2)) / 10000f + 0.001f, 0.025f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					light = true,
					lightRadius = 2f,
					delayBeforeAnimationStart = 200,
					lightcolor = Color.Black
				});
				drawPosition.Y -= (float)(lightningSourceRect.Height * Game1.pixelZoom);
			}
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000B0F00 File Offset: 0x000AF100
		public static string getDateString(int offset = 0)
		{
			int currentDay = Game1.dayOfMonth;
			int currentSeason = Utility.getSeasonNumber(Game1.currentSeason);
			int currentYear = Game1.year;
			currentDay += offset;
			if (currentDay <= 0)
			{
				currentDay += 28;
				currentSeason--;
				if (currentSeason < 0)
				{
					currentSeason = 3;
					currentYear--;
				}
			}
			else if (currentDay > 28)
			{
				currentDay -= 28;
				currentSeason++;
				if (currentSeason > 3)
				{
					currentSeason = 0;
					currentYear++;
				}
			}
			if (currentYear == 0)
			{
				return "First Night";
			}
			return string.Concat(new object[]
			{
				currentDay,
				Utility.getNumberEnding(currentDay),
				" of ",
				Utility.getSeasonNameFromNumber(currentSeason),
				", Year ",
				currentYear
			});
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000B0FA1 File Offset: 0x000AF1A1
		public static string getYesterdaysDate()
		{
			return Utility.getDateString(-1);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000B0FA9 File Offset: 0x000AF1A9
		public static string getSeasonNameFromNumber(int number)
		{
			switch (number)
			{
			case 0:
				return "Spring";
			case 1:
				return "Summer";
			case 2:
				return "Fall";
			case 3:
				return "Winter";
			default:
				return "";
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000B0FE0 File Offset: 0x000AF1E0
		public static string getNumberEnding(int number)
		{
			if (number % 100 > 10 && number % 100 < 20)
			{
				return "th";
			}
			switch (number % 10)
			{
			case 0:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				return "th";
			case 1:
				return "st";
			case 2:
				return "nd";
			case 3:
				return "rd";
			default:
				return "";
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000B1058 File Offset: 0x000AF258
		public static void killAllStaticLoopingSoundCues()
		{
			if (Game1.soundBank != null)
			{
				if (Fly.buzz != null)
				{
					Fly.buzz.Stop(AudioStopOptions.Immediate);
				}
				if (Railroad.trainLoop != null)
				{
					Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
				}
				if (BobberBar.reelSound != null)
				{
					BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
				}
				if (BobberBar.unReelSound != null)
				{
					BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
				}
				if (FishingRod.reelSound != null)
				{
					FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
				}
				if (Game1.fuseSound != null)
				{
					Game1.fuseSound.Stop(AudioStopOptions.Immediate);
				}
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000B10D8 File Offset: 0x000AF2D8
		public static void consolidateStacks(List<Item> objects)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				if (objects[i] != null && objects[i] is Object)
				{
					Object o = objects[i] as Object;
					for (int j = i + 1; j < objects.Count; j++)
					{
						if (objects[j] != null && o.canStackWith(objects[j]))
						{
							o.Stack = objects[j].addToStack(o.Stack);
							if (o.Stack <= 0)
							{
								break;
							}
						}
					}
				}
			}
			for (int k = objects.Count - 1; k >= 0; k--)
			{
				if (objects[k] != null && objects[k].Stack <= 0)
				{
					objects.RemoveAt(k);
				}
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000B1198 File Offset: 0x000AF398
		public static void performLightningUpdate()
		{
			Random random = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + Game1.timeOfDay);
			if (random.NextDouble() < 0.125 + Game1.dailyLuck + (double)((float)Game1.player.luckLevel / 100f))
			{
				if (Game1.currentLocation.IsOutdoors && !(Game1.currentLocation is Desert) && !Game1.newDay)
				{
					Game1.flashAlpha = (float)(0.5 + random.NextDouble());
					Game1.playSound("thunder");
				}
				GameLocation farm = Game1.getLocationFromName("Farm");
				List<Vector2> lightningRods = new List<Vector2>();
				foreach (KeyValuePair<Vector2, Object> v in farm.objects)
				{
					if (v.Value.bigCraftable && v.Value.ParentSheetIndex == 9)
					{
						lightningRods.Add(v.Key);
					}
				}
				if (lightningRods.Count > 0)
				{
					for (int i = 0; i < 2; i++)
					{
						Vector2 v2 = lightningRods.ElementAt(random.Next(lightningRods.Count));
						if (farm.objects[v2].heldObject == null)
						{
							farm.objects[v2].heldObject = new Object(787, 1, false, -1, 0);
							farm.objects[v2].minutesUntilReady = 3000 - Game1.timeOfDay;
							farm.objects[v2].shakeTimer = 1000;
							if (Game1.currentLocation is Farm)
							{
								Utility.drawLightningBolt(v2 * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), farm);
							}
							return;
						}
					}
				}
				if (random.NextDouble() >= 0.25 - Game1.dailyLuck - (double)((float)Game1.player.luckLevel / 100f))
				{
					return;
				}
				try
				{
					KeyValuePair<Vector2, TerrainFeature> c = farm.terrainFeatures.ElementAt(random.Next(farm.terrainFeatures.Count));
					if (!(c.Value is FruitTree) && c.Value.performToolAction(null, 50, c.Key, farm))
					{
						farm.terrainFeatures.Remove(c.Key);
						if (Game1.currentLocation.name.Equals("Farm"))
						{
							farm.temporarySprites.Add(new TemporaryAnimatedSprite(362, 75f, 6, 1, c.Key, false, false));
							Utility.drawLightningBolt(c.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 2)), farm);
						}
					}
					else if (c.Value is FruitTree)
					{
						(c.Value as FruitTree).struckByLightningCountdown = 4;
						(c.Value as FruitTree).shake(c.Key, true);
						Utility.drawLightningBolt(c.Key * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize * 2)), farm);
					}
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			if (random.NextDouble() < 0.1 && Game1.currentLocation.IsOutdoors && !(Game1.currentLocation is Desert) && !Game1.newDay)
			{
				Game1.flashAlpha = (float)(0.5 + random.NextDouble());
				if (random.NextDouble() < 0.5)
				{
					DelayedAction.screenFlashAfterDelay((float)(0.3 + random.NextDouble()), random.Next(500, 1000), "");
				}
				DelayedAction.playSoundAfterDelay("thunder_small", random.Next(500, 1500));
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000B15BC File Offset: 0x000AF7BC
		public static void overnightLightning()
		{
			int numberOfLoops = (2400 - Game1.timeOfDay) / 100;
			for (int i = 0; i < numberOfLoops; i++)
			{
				Utility.performLightningUpdate();
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000B15EC File Offset: 0x000AF7EC
		public static List<Vector2> getAdjacentTileLocations(Vector2 tileLocation)
		{
			return new List<Vector2>
			{
				new Vector2(-1f, 0f) + tileLocation,
				new Vector2(1f, 0f) + tileLocation,
				new Vector2(0f, 1f) + tileLocation,
				new Vector2(0f, -1f) + tileLocation
			};
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x000B166C File Offset: 0x000AF86C
		public static List<Point> getAdjacentTilePoints(float xTile, float yTile)
		{
			List<Point> arg_0B_0 = new List<Point>();
			int x = (int)xTile;
			int y = (int)yTile;
			arg_0B_0.Add(new Point(-1 + x, y));
			arg_0B_0.Add(new Point(1 + x, y));
			arg_0B_0.Add(new Point(x, 1 + y));
			arg_0B_0.Add(new Point(x, -1 + y));
			return arg_0B_0;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x000B16C0 File Offset: 0x000AF8C0
		public static Vector2[] getAdjacentTileLocationsArray(Vector2 tileLocation)
		{
			return new Vector2[]
			{
				new Vector2(-1f, 0f) + tileLocation,
				new Vector2(1f, 0f) + tileLocation,
				new Vector2(0f, 1f) + tileLocation,
				new Vector2(0f, -1f) + tileLocation
			};
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000B1744 File Offset: 0x000AF944
		public static Vector2[] getDiagonalTileLocationsArray(Vector2 tileLocation)
		{
			return new Vector2[]
			{
				new Vector2(-1f, -1f) + tileLocation,
				new Vector2(1f, 1f) + tileLocation,
				new Vector2(-1f, 1f) + tileLocation,
				new Vector2(1f, -1f) + tileLocation
			};
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000B17C8 File Offset: 0x000AF9C8
		public static Vector2[] getSurroundingTileLocationsArray(Vector2 tileLocation)
		{
			return new Vector2[]
			{
				new Vector2(-1f, 0f) + tileLocation,
				new Vector2(1f, 0f) + tileLocation,
				new Vector2(0f, 1f) + tileLocation,
				new Vector2(0f, -1f) + tileLocation,
				new Vector2(-1f, -1f) + tileLocation,
				new Vector2(1f, -1f) + tileLocation,
				new Vector2(1f, 1f) + tileLocation,
				new Vector2(-1f, 1f) + tileLocation
			};
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x000B18BC File Offset: 0x000AFABC
		public static Crop findCloseFlower(Vector2 startTileLocation)
		{
			Queue<Vector2> openList = new Queue<Vector2>();
			HashSet<Vector2> closedList = new HashSet<Vector2>();
			Farm f = Game1.getLocationFromName("Farm") as Farm;
			openList.Enqueue(startTileLocation);
			int attempts = 0;
			while (attempts <= 150 && openList.Count > 0)
			{
				Vector2 currentTile = openList.Dequeue();
				if (f.terrainFeatures.ContainsKey(currentTile) && f.terrainFeatures[currentTile] is HoeDirt && (f.terrainFeatures[currentTile] as HoeDirt).crop != null && (f.terrainFeatures[currentTile] as HoeDirt).crop.programColored && (f.terrainFeatures[currentTile] as HoeDirt).crop.currentPhase >= (f.terrainFeatures[currentTile] as HoeDirt).crop.phaseDays.Count - 1 && !(f.terrainFeatures[currentTile] as HoeDirt).crop.dead)
				{
					return (f.terrainFeatures[currentTile] as HoeDirt).crop;
				}
				foreach (Vector2 v in Utility.getAdjacentTileLocations(currentTile))
				{
					if (!closedList.Contains(v))
					{
						openList.Enqueue(v);
					}
				}
				closedList.Add(currentTile);
				attempts++;
			}
			return null;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x000B1A44 File Offset: 0x000AFC44
		public static Point findCloseMatureCrop(Vector2 startTileLocation)
		{
			Queue<Vector2> openList = new Queue<Vector2>();
			HashSet<Vector2> closedList = new HashSet<Vector2>();
			Farm f = Game1.getLocationFromName("Farm") as Farm;
			openList.Enqueue(startTileLocation);
			int attempts = 0;
			while (attempts <= 40 && openList.Count<Vector2>() > 0)
			{
				Vector2 currentTile = openList.Dequeue();
				if (f.terrainFeatures.ContainsKey(currentTile) && f.terrainFeatures[currentTile] is HoeDirt && (f.terrainFeatures[currentTile] as HoeDirt).crop != null && (f.terrainFeatures[currentTile] as HoeDirt).readyForHarvest())
				{
					return Utility.Vector2ToPoint(currentTile);
				}
				foreach (Vector2 v in Utility.getAdjacentTileLocations(currentTile))
				{
					if (!closedList.Contains(v))
					{
						openList.Enqueue(v);
					}
				}
				closedList.Add(currentTile);
				attempts++;
			}
			return Point.Zero;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000B1B54 File Offset: 0x000AFD54
		public static void recursiveFenceBuild(Vector2 position, int direction, GameLocation location, Random r)
		{
			if (r.NextDouble() < 0.04)
			{
				return;
			}
			if (location.objects.ContainsKey(position) || !location.isTileLocationOpen(new Location((int)position.X * Game1.tileSize, (int)position.Y * Game1.tileSize)))
			{
				return;
			}
			location.objects.Add(position, new Fence(position, 1, false));
			int directionToBuild = direction;
			if (r.NextDouble() < 0.16)
			{
				directionToBuild = r.Next(4);
			}
			if (directionToBuild == (direction + 2) % 4)
			{
				directionToBuild = (directionToBuild + 1) % 4;
			}
			switch (direction)
			{
			case 0:
				Utility.recursiveFenceBuild(position + new Vector2(0f, -1f), directionToBuild, location, r);
				return;
			case 1:
				Utility.recursiveFenceBuild(position + new Vector2(1f, 0f), directionToBuild, location, r);
				return;
			case 2:
				Utility.recursiveFenceBuild(position + new Vector2(0f, 1f), directionToBuild, location, r);
				return;
			case 3:
				Utility.recursiveFenceBuild(position + new Vector2(-1f, 0f), directionToBuild, location, r);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x000B1C74 File Offset: 0x000AFE74
		public static bool addAnimalToFarm(FarmAnimal animal)
		{
			if (animal == null || animal.sprite == null)
			{
				return false;
			}
			foreach (Building b in ((Farm)Game1.getLocationFromName("Farm")).buildings)
			{
				if (b.buildingType.Contains(animal.buildingTypeILiveIn))
				{
					((AnimalHouse)b.indoors).animals.Add(animal.myID, animal);
					animal.setRandomPosition(b.indoors);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x000B1D20 File Offset: 0x000AFF20
		public static Vector2 getAwayFromPlayerTrajectory(Microsoft.Xna.Framework.Rectangle monsterBox)
		{
			return Utility.getAwayFromPlayerTrajectory(monsterBox, Game1.player);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000B1D30 File Offset: 0x000AFF30
		public static Item getItemFromStandardTextDescription(string description, Farmer who, char delimiter = ' ')
		{
			string[] expr_10 = description.Split(new char[]
			{
				delimiter
			});
			string type = expr_10[0];
			int index = Convert.ToInt32(expr_10[1]);
			int stock = Convert.ToInt32(expr_10[2]);
			Item item = null;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(type);
			if (num <= 3082879841u)
			{
				if (num <= 1430892386u)
				{
					if (num <= 568155902u)
					{
						if (num != 551378283u)
						{
							if (num != 568155902u)
							{
								goto IL_35D;
							}
							if (!(type == "BO"))
							{
								goto IL_35D;
							}
						}
						else
						{
							if (!(type == "BL"))
							{
								goto IL_35D;
							}
							goto IL_33A;
						}
					}
					else if (num != 930363637u)
					{
						if (num != 1430892386u)
						{
							goto IL_35D;
						}
						if (!(type == "Hat"))
						{
							goto IL_35D;
						}
						goto IL_347;
					}
					else
					{
						if (!(type == "Boot"))
						{
							goto IL_35D;
						}
						goto IL_328;
					}
				}
				else if (num <= 2089749334u)
				{
					if (num != 2005354379u)
					{
						if (num != 2089749334u)
						{
							goto IL_35D;
						}
						if (!(type == "BigObject"))
						{
							goto IL_35D;
						}
					}
					else
					{
						if (!(type == "Ring"))
						{
							goto IL_35D;
						}
						goto IL_31F;
					}
				}
				else if (num != 2675227691u)
				{
					if (num != 2707948032u)
					{
						if (num != 3082879841u)
						{
							goto IL_35D;
						}
						if (!(type == "Weapon"))
						{
							goto IL_35D;
						}
						goto IL_331;
					}
					else
					{
						if (!(type == "Blueprint"))
						{
							goto IL_35D;
						}
						goto IL_33A;
					}
				}
				else
				{
					if (!(type == "BBL"))
					{
						goto IL_35D;
					}
					goto IL_350;
				}
				item = new Object(Vector2.Zero, index, false);
				goto IL_35D;
				IL_33A:
				item = new Object(index, 1, true, -1, 0);
				goto IL_35D;
			}
			if (num <= 3440116983u)
			{
				if (num <= 3272340793u)
				{
					if (num != 3212111499u)
					{
						if (num != 3272340793u)
						{
							goto IL_35D;
						}
						if (!(type == "F"))
						{
							goto IL_35D;
						}
					}
					else
					{
						if (!(type == "BBl"))
						{
							goto IL_35D;
						}
						goto IL_350;
					}
				}
				else if (num != 3339451269u)
				{
					if (num != 3389784126u)
					{
						if (num != 3440116983u)
						{
							goto IL_35D;
						}
						if (!(type == "H"))
						{
							goto IL_35D;
						}
						goto IL_347;
					}
					else
					{
						if (!(type == "O"))
						{
							goto IL_35D;
						}
						goto IL_303;
					}
				}
				else
				{
					if (!(type == "B"))
					{
						goto IL_35D;
					}
					goto IL_328;
				}
			}
			else if (num <= 3579274004u)
			{
				if (num != 3524005078u)
				{
					if (num != 3579274004u)
					{
						goto IL_35D;
					}
					if (!(type == "BigBlueprint"))
					{
						goto IL_35D;
					}
					goto IL_350;
				}
				else
				{
					if (!(type == "W"))
					{
						goto IL_35D;
					}
					goto IL_331;
				}
			}
			else if (num != 3607893173u)
			{
				if (num != 3851314394u)
				{
					if (num != 4225588997u)
					{
						goto IL_35D;
					}
					if (!(type == "Furniture"))
					{
						goto IL_35D;
					}
				}
				else
				{
					if (!(type == "Object"))
					{
						goto IL_35D;
					}
					goto IL_303;
				}
			}
			else
			{
				if (!(type == "R"))
				{
					goto IL_35D;
				}
				goto IL_31F;
			}
			item = new Furniture(index, Vector2.Zero);
			goto IL_35D;
			IL_303:
			item = new Object(index, 1, false, -1, 0);
			goto IL_35D;
			IL_31F:
			item = new Ring(index);
			goto IL_35D;
			IL_328:
			item = new Boots(index);
			goto IL_35D;
			IL_331:
			item = new MeleeWeapon(index);
			goto IL_35D;
			IL_347:
			item = new Hat(index);
			goto IL_35D;
			IL_350:
			item = new Object(Vector2.Zero, index, true);
			IL_35D:
			if (!type.Equals("BO") && !type.Equals("BigObject"))
			{
				item.Stack = stock;
			}
			if (item is Object && (item as Object).isRecipe && who.knowsRecipe(item.Name))
			{
				return null;
			}
			return item;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000B20E1 File Offset: 0x000B02E1
		public static List<TemporaryAnimatedSprite> sparkleWithinArea(Microsoft.Xna.Framework.Rectangle bounds, int numberOfSparkles, Color sparkleColor, int delayBetweenSparkles = 100, int delayBeforeStarting = 0, string sparkleSound = "")
		{
			return Utility.getTemporarySpritesWithinArea(new int[]
			{
				10,
				11
			}, bounds, numberOfSparkles, sparkleColor, delayBetweenSparkles, delayBeforeStarting, sparkleSound);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000B2100 File Offset: 0x000B0300
		public static List<TemporaryAnimatedSprite> getTemporarySpritesWithinArea(int[] temporarySpriteRowNumbers, Microsoft.Xna.Framework.Rectangle bounds, int numberOfsprites, Color color, int delayBetweenSprites = 100, int delayBeforeStarting = 0, string sound = "")
		{
			List<TemporaryAnimatedSprite> sparkles = new List<TemporaryAnimatedSprite>();
			for (int i = 0; i < numberOfsprites; i++)
			{
				sparkles.Add(new TemporaryAnimatedSprite(temporarySpriteRowNumbers[Game1.random.Next(temporarySpriteRowNumbers.Length)], new Vector2((float)Game1.random.Next(bounds.X, bounds.Right), (float)Game1.random.Next(bounds.Y, bounds.Bottom)), color, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					delayBeforeAnimationStart = delayBeforeStarting + delayBetweenSprites * i,
					startSound = ((sound.Length > 0) ? sound : null)
				});
			}
			return sparkles;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000B21A8 File Offset: 0x000B03A8
		public static Vector2 getAwayFromPlayerTrajectory(Microsoft.Xna.Framework.Rectangle monsterBox, Farmer who)
		{
			if (who == null)
			{
				who = Game1.player;
			}
			float arg_4E_0 = (float)(-(float)(who.GetBoundingBox().Center.X - monsterBox.Center.X));
			float ySlope = (float)(who.GetBoundingBox().Center.Y - monsterBox.Center.Y);
			float total = Math.Abs(arg_4E_0) + Math.Abs(ySlope);
			if (total < 1f)
			{
				total = 5f;
			}
			float arg_97_0 = arg_4E_0 / total * (float)(50 + Game1.random.Next(-20, 20));
			ySlope = ySlope / total * (float)(50 + Game1.random.Next(-20, 20));
			return new Vector2(arg_97_0, ySlope);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x000B2254 File Offset: 0x000B0454
		public static string getSongTitleFromCueName(string cueName)
		{
			string text = cueName.ToLower();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2381310912u)
			{
				if (num <= 1014963253u)
				{
					if (num <= 513174751u)
					{
						if (num <= 311403980u)
						{
							if (num <= 31721691u)
							{
								if (num != 14925162u)
								{
									if (num != 31721691u)
									{
										return cueName;
									}
									if (!(text == "poppy"))
									{
										return cueName;
									}
									return "Sam's Band (poppy)";
								}
								else
								{
									if (!(text == "cowboy_boss"))
									{
										return cueName;
									}
									return "Journey Of The Praire King (Boss)";
								}
							}
							else if (num != 214487563u)
							{
								if (num != 295673152u)
								{
									if (num != 311403980u)
									{
										return cueName;
									}
									if (!(text == "spirits_eve"))
									{
										return cueName;
									}
									return "Spirit's Eve Festival";
								}
								else
								{
									if (!(text == "heavy"))
									{
										return cueName;
									}
									return "Sam's Band (heavy)";
								}
							}
							else
							{
								if (!(text == "cloth"))
								{
									return cueName;
								}
								return "Mines (Cloth)";
							}
						}
						else if (num <= 369403945u)
						{
							if (num != 322927272u)
							{
								if (num != 369403945u)
								{
									return cueName;
								}
								if (!(text == "flowerdance"))
								{
									return cueName;
								}
								return "Flower Dance";
							}
							else
							{
								if (!(text == "moonlightjellies"))
								{
									return cueName;
								}
								return "Dance Of The Moonlight Jellies";
							}
						}
						else if (num != 437551800u)
						{
							if (num != 461776425u)
							{
								if (num != 513174751u)
								{
									return cueName;
								}
								if (!(text == "summer1"))
								{
									return cueName;
								}
								return "Summer (Nature's Crescendo)";
							}
							else
							{
								if (!(text == "marnieshop"))
								{
									return cueName;
								}
								return "Country Shop";
							}
						}
						else
						{
							if (!(text == "emilydance"))
							{
								return cueName;
							}
							return "Emily's Dance";
						}
					}
					else if (num <= 613046764u)
					{
						if (num <= 545110351u)
						{
							if (num != 529952370u)
							{
								if (num != 545110351u)
								{
									return cueName;
								}
								if (!(text == "cowboy_overworld"))
								{
									return cueName;
								}
								return "Journey Of The Praire King (Overworld)";
							}
							else
							{
								if (!(text == "summer2"))
								{
									return cueName;
								}
								return "Summer (The Sun Can Bend An Orange Sky)";
							}
						}
						else if (num != 546729989u)
						{
							if (num != 575982768u)
							{
								if (num != 613046764u)
								{
									return cueName;
								}
								if (!(text == "musicboxsong"))
								{
									return cueName;
								}
								return "Music Box Song";
							}
							else if (!(text == "title_day"))
							{
								return cueName;
							}
						}
						else
						{
							if (!(text == "summer3"))
							{
								return cueName;
							}
							return "Summer (Tropicala)";
						}
					}
					else if (num <= 906885789u)
					{
						if (num != 766748620u)
						{
							if (num != 906885789u)
							{
								return cueName;
							}
							if (!(text == "near the planet core"))
							{
								return cueName;
							}
							return "Mines (A Visitor To The Unknown)";
						}
						else
						{
							if (!(text == "of dwarves"))
							{
								return cueName;
							}
							return "Mines (The Lava Dwellers)";
						}
					}
					else if (num != 971922320u)
					{
						if (num != 1003090502u)
						{
							if (num != 1014963253u)
							{
								return cueName;
							}
							if (!(text == "sampractice"))
							{
								return cueName;
							}
							return "Band Practice";
						}
						else
						{
							if (!(text == "abigailflute"))
							{
								return cueName;
							}
							return "A Stillness In The Rain Solo";
						}
					}
					else
					{
						if (!(text == "shimmeringbastion"))
						{
							return cueName;
						}
						return "Sam's Band (Electronic)";
					}
				}
				else if (num <= 1422361652u)
				{
					if (num <= 1203340822u)
					{
						if (num <= 1150398120u)
						{
							if (num != 1104502631u)
							{
								if (num != 1150398120u)
								{
									return cueName;
								}
								if (!(text == "distantbanjo"))
								{
									return cueName;
								}
								return "Distant Banjo";
							}
							else
							{
								if (!(text == "gusviolin"))
								{
									return cueName;
								}
								return "Violin Solo";
							}
						}
						else if (num != 1176080900u)
						{
							if (num != 1186563203u)
							{
								if (num != 1203340822u)
								{
									return cueName;
								}
								if (!(text == "fall2"))
								{
									return cueName;
								}
								return "Fall (Ghost Synth)";
							}
							else
							{
								if (!(text == "fall1"))
								{
									return cueName;
								}
								return "Fall (The Smell Of Mushroom)";
							}
						}
						else
						{
							if (!(text == "jojaOfficeSoundscape"))
							{
								return cueName;
							}
							return "Joja Office Ambience";
						}
					}
					else if (num <= 1220118441u)
					{
						if (num != 1216379696u)
						{
							if (num != 1220118441u)
							{
								return cueName;
							}
							if (!(text == "fall3"))
							{
								return cueName;
							}
							return "Fall (Raven's Descent)";
						}
						else
						{
							if (!(text == "overcast"))
							{
								return cueName;
							}
							return "Mines (Magical Shoes)";
						}
					}
					else if (num != 1266391031u)
					{
						if (num != 1403190614u)
						{
							if (num != 1422361652u)
							{
								return cueName;
							}
							if (!(text == "title_night"))
							{
								return cueName;
							}
							return "Load Game Theme";
						}
						else
						{
							if (!(text == "woodstheme"))
							{
								return cueName;
							}
							return "In The Deep Woods";
						}
					}
					else
					{
						if (!(text == "wedding"))
						{
							return cueName;
						}
						return "Wedding Theme";
					}
				}
				else if (num <= 1637564208u)
				{
					if (num <= 1505686774u)
					{
						if (num != 1434728751u)
						{
							if (num != 1505686774u)
							{
								return cueName;
							}
							if (!(text == "playful"))
							{
								return cueName;
							}
							return "Playful";
						}
						else
						{
							if (!(text == "sweet"))
							{
								return cueName;
							}
							return "Buttercup Melody";
						}
					}
					else if (num != 1611928003u)
					{
						if (num != 1626719312u)
						{
							if (num != 1637564208u)
							{
								return cueName;
							}
							if (!(text == "kindadumbautumn"))
							{
								return cueName;
							}
							return "Grapefruit Sky";
						}
						else
						{
							if (!(text == "cavern"))
							{
								return cueName;
							}
							return "Mines (A Flicker In The Deep)";
						}
					}
					else
					{
						if (!(text == "buglevelloop"))
						{
							return cueName;
						}
						return "Mutant Bug Ambience";
					}
				}
				else if (num <= 1898587220u)
				{
					if (num != 1833789906u)
					{
						if (num != 1855523881u)
						{
							if (num != 1898587220u)
							{
								return cueName;
							}
							if (!(text == "sadpiano"))
							{
								return cueName;
							}
							return "A Dark Corner Of The Past";
						}
						else
						{
							if (!(text == "50s"))
							{
								return cueName;
							}
							return "Pleasant Memory";
						}
					}
					else
					{
						if (!(text == "secret gnomes"))
						{
							return cueName;
						}
						return "Mines (Star Lumpy)";
					}
				}
				else if (num != 2305400673u)
				{
					if (num != 2317861964u)
					{
						if (num != 2381310912u)
						{
							return cueName;
						}
						if (!(text == "wavy"))
						{
							return cueName;
						}
						return "Calico Desert";
					}
					else
					{
						if (!(text == "starshoot"))
						{
							return cueName;
						}
						return "Starshoot";
					}
				}
				else
				{
					if (!(text == "shanetheme"))
					{
						return cueName;
					}
					return "Shane's Theme";
				}
			}
			else if (num <= 3366607222u)
			{
				if (num <= 2966460075u)
				{
					if (num <= 2720137060u)
					{
						if (num <= 2549047255u)
						{
							if (num != 2425292108u)
							{
								if (num != 2549047255u)
								{
									return cueName;
								}
								if (!(text == "aerobics"))
								{
									return cueName;
								}
								return "Aerobics Class";
							}
							else
							{
								if (!(text == "cloudcountry"))
								{
									return cueName;
								}
								return "Cloud Country";
							}
						}
						else if (num != 2554705608u)
						{
							if (num != 2601366617u)
							{
								if (num != 2720137060u)
								{
									return cueName;
								}
								if (!(text == "christmastheme"))
								{
									return cueName;
								}
								return "Winter Festival";
							}
							else
							{
								if (!(text == "tribal"))
								{
									return cueName;
								}
								return "Mines (Danger!)";
							}
						}
						else
						{
							if (!(text == "saloon1"))
							{
								return cueName;
							}
							return "The Stardrop Saloon";
						}
					}
					else if (num <= 2844201942u)
					{
						if (num != 2819207984u)
						{
							if (num != 2844201942u)
							{
								return cueName;
							}
							if (!(text == "emilytheme"))
							{
								return cueName;
							}
							return "Song Of Feathers";
						}
						else
						{
							if (!(text == "jaunty"))
							{
								return cueName;
							}
							return "Jaunty";
						}
					}
					else if (num != 2876653781u)
					{
						if (num != 2961267221u)
						{
							if (num != 2966460075u)
							{
								return cueName;
							}
							if (!(text == "crystal bells"))
							{
								return cueName;
							}
							return "Mines (Crystal Bells)";
						}
						else
						{
							if (!(text == "librarytheme"))
							{
								return cueName;
							}
							return "Library And Museum";
						}
					}
					else
					{
						if (!(text == "echos"))
						{
							return cueName;
						}
						return "Echos";
					}
				}
				else if (num <= 3121092129u)
				{
					if (num <= 2998131962u)
					{
						if (num != 2981354343u)
						{
							if (num != 2998131962u)
							{
								return cueName;
							}
							if (!(text == "spring2"))
							{
								return cueName;
							}
							return "Spring (The Valley Comes Alive)";
						}
						else
						{
							if (!(text == "spring1"))
							{
								return cueName;
							}
							return "Spring (It's A Big World Outside)";
						}
					}
					else if (num != 3014909581u)
					{
						if (num != 3067782397u)
						{
							if (num != 3121092129u)
							{
								return cueName;
							}
							if (!(text == "elliottpiano"))
							{
								return cueName;
							}
							return "Piano Solo";
						}
						else if (!(text == "maintheme"))
						{
							return cueName;
						}
					}
					else
					{
						if (!(text == "spring3"))
						{
							return cueName;
						}
						return "Spring (Wild Horseradish Jam)";
					}
				}
				else if (num <= 3293919832u)
				{
					if (num != 3123466408u)
					{
						if (num != 3224386323u)
						{
							if (num != 3293919832u)
							{
								return cueName;
							}
							if (!(text == "ragtime"))
							{
								return cueName;
							}
							return "Pickle Jar Rag";
						}
						else
						{
							if (!(text == "ticktock"))
							{
								return cueName;
							}
							return "Festival Game";
						}
					}
					else
					{
						if (!(text == "springtown"))
						{
							return cueName;
						}
						return "Pelican Town";
					}
				}
				else if (num != 3304440099u)
				{
					if (num != 3328725896u)
					{
						if (num != 3366607222u)
						{
							return cueName;
						}
						if (!(text == "settlingin"))
						{
							return cueName;
						}
						return "Settling In";
					}
					else
					{
						if (!(text == "desolate"))
						{
							return cueName;
						}
						return "A Sad Story";
					}
				}
				else
				{
					if (!(text == "wizardsong"))
					{
						return cueName;
					}
					return "A Glimpse Of The Other World";
				}
			}
			else if (num <= 3570958826u)
			{
				if (num <= 3460154593u)
				{
					if (num <= 3406989666u)
					{
						if (num != 3373546083u)
						{
							if (num != 3406989666u)
							{
								return cueName;
							}
							if (!(text == "breezy"))
							{
								return cueName;
							}
							return "Land Of Green And Gold";
						}
						else
						{
							if (!(text == "tinymusicbox"))
							{
								return cueName;
							}
							return "Music Box Song";
						}
					}
					else if (num != 3429620606u)
					{
						if (num != 3457015272u)
						{
							if (num != 3460154593u)
							{
								return cueName;
							}
							if (!(text == "grandpas_theme"))
							{
								return cueName;
							}
							return "Grandpa's Theme";
						}
						else
						{
							if (!(text == "emilydream"))
							{
								return cueName;
							}
							return "Dreamscape";
						}
					}
					else
					{
						if (!(text == "xor"))
						{
							return cueName;
						}
						return "Mines (Marimba Of Frozen Bone)";
					}
				}
				else if (num <= 3490171344u)
				{
					if (num != 3464053098u)
					{
						if (num != 3490171344u)
						{
							return cueName;
						}
						if (!(text == "abigailfluteduet"))
						{
							return cueName;
						}
						return "A Stillness In The Rain Duet";
					}
					else
					{
						if (!(text == "cowboy_outlawsong"))
						{
							return cueName;
						}
						return "Journey Of The Praire King (Outlaw)";
					}
				}
				else if (num != 3492248772u)
				{
					if (num != 3554181207u)
					{
						if (num != 3570958826u)
						{
							return cueName;
						}
						if (!(text == "event1"))
						{
							return cueName;
						}
						return "Fun Festival";
					}
					else
					{
						if (!(text == "event2"))
						{
							return cueName;
						}
						return "Luau Festival";
					}
				}
				else
				{
					if (!(text == "christmasTheme"))
					{
						return cueName;
					}
					return "Winter Festival";
				}
			}
			else if (num <= 3975651554u)
			{
				if (num <= 3858316600u)
				{
					if (num != 3601997915u)
					{
						if (num != 3858316600u)
						{
							return cueName;
						}
						if (!(text == "cowboy_singing"))
						{
							return cueName;
						}
						return "Journey Of The Praire King (Ending)";
					}
					else
					{
						if (!(text == "icicles"))
						{
							return cueName;
						}
						return "Pleasant Memory";
					}
				}
				else if (num != 3880729775u)
				{
					if (num != 3942493534u)
					{
						if (num != 3975651554u)
						{
							return cueName;
						}
						if (!(text == "cowboy_undead"))
						{
							return cueName;
						}
						return "Journey Of The Praire King (Undead)";
					}
					else
					{
						if (!(text == "marlonstheme"))
						{
							return cueName;
						}
						return "Marlon's Theme";
					}
				}
				else
				{
					if (!(text == "honkytonky"))
					{
						return cueName;
					}
					return "Sam's Band (Bluegrass)";
				}
			}
			else if (num <= 4049627284u)
			{
				if (num != 4026246663u)
				{
					if (num != 4043024282u)
					{
						if (num != 4049627284u)
						{
							return cueName;
						}
						if (!(text == "spacemusic"))
						{
							return cueName;
						}
						return "Starwatcher";
					}
					else
					{
						if (!(text == "winter2"))
						{
							return cueName;
						}
						return "Winter (The Wind Can Be Still)";
					}
				}
				else
				{
					if (!(text == "winter1"))
					{
						return cueName;
					}
					return "Winter (Nocturne Of Ice)";
				}
			}
			else if (num != 4059801901u)
			{
				if (num != 4175787570u)
				{
					if (num != 4289135460u)
					{
						return cueName;
					}
					if (!(text == "junimostarsong"))
					{
						return cueName;
					}
					return "A Golden Star Is Born";
				}
				else
				{
					if (!(text == "fallfest"))
					{
						return cueName;
					}
					return "Stardew Valley Fair Theme";
				}
			}
			else
			{
				if (!(text == "winter3"))
				{
					return cueName;
				}
				return "Winter (Ancient)";
			}
			return "Stardew Valley Overture";
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000B3069 File Offset: 0x000B1269
		public static bool isOffScreenEndFunction(PathNode currentNode, Point endPoint, GameLocation location, Character c)
		{
			return !Utility.isOnScreen(new Vector2((float)(currentNode.x * Game1.tileSize), (float)(currentNode.y * Game1.tileSize)), Game1.tileSize / 2);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000B309C File Offset: 0x000B129C
		public static Vector2 getAwayFromPositionTrajectory(Microsoft.Xna.Framework.Rectangle monsterBox, Vector2 position)
		{
			float arg_2A_0 = -(position.X - (float)monsterBox.Center.X);
			float ySlope = position.Y - (float)monsterBox.Center.Y;
			float total = Math.Abs(arg_2A_0) + Math.Abs(ySlope);
			if (total < 1f)
			{
				total = 5f;
			}
			float arg_59_0 = arg_2A_0 / total * 20f;
			ySlope = ySlope / total * 20f;
			return new Vector2(arg_59_0, ySlope);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000B3108 File Offset: 0x000B1308
		public static bool tileWithinRadiusOfPlayer(int xTile, int yTile, int tileRadius, Farmer f)
		{
			Point point = new Point(xTile, yTile);
			Vector2 playerTile = f.getTileLocation();
			return Math.Abs((float)point.X - playerTile.X) <= (float)tileRadius && Math.Abs((float)point.Y - playerTile.Y) <= (float)tileRadius;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000B3158 File Offset: 0x000B1358
		public static bool withinRadiusOfPlayer(int x, int y, int tileRadius, Farmer f)
		{
			Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
			Vector2 playerTile = f.getTileLocation();
			return Math.Abs((float)point.X - playerTile.X) <= (float)tileRadius && Math.Abs((float)point.Y - playerTile.Y) <= (float)tileRadius;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000B31B4 File Offset: 0x000B13B4
		public static bool isThereAnObjectHereWhichAcceptsThisItem(GameLocation location, Item item, int x, int y)
		{
			if (item is Tool)
			{
				return false;
			}
			Vector2 tileLocation = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			if (location.Objects.ContainsKey(tileLocation) && location.objects[tileLocation].heldObject == null)
			{
				location.objects[tileLocation].performObjectDropInAction((Object)item, true, Game1.player);
				bool arg_89_0 = location.objects[tileLocation].heldObject != null;
				location.objects[tileLocation].heldObject = null;
				return arg_89_0;
			}
			return false;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000B324C File Offset: 0x000B144C
		public static bool buyWallpaper()
		{
			if (Game1.player.Money >= Game1.wallpaperPrice)
			{
				Game1.updateWallpaperInFarmHouse(Game1.currentWallpaper);
				Game1.farmerWallpaper = Game1.currentWallpaper;
				Game1.player.Money -= Game1.wallpaperPrice;
				Game1.addHUDMessage(new HUDMessage("Wallpaper applied", Color.Green, 3500f));
				return true;
			}
			return false;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000B32B4 File Offset: 0x000B14B4
		public static FarmAnimal getAnimal(long id)
		{
			if (Game1.getFarm().animals.ContainsKey(id))
			{
				return Game1.getFarm().animals[id];
			}
			foreach (Building b in Game1.getFarm().buildings)
			{
				if (b.indoors is AnimalHouse && (b.indoors as AnimalHouse).animals.ContainsKey(id))
				{
					return (b.indoors as AnimalHouse).animals[id];
				}
			}
			return null;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000B3368 File Offset: 0x000B1568
		public static bool buyFloor()
		{
			if (Game1.player.Money >= Game1.floorPrice)
			{
				Game1.FarmerFloor = Game1.currentFloor;
				Game1.updateFloorInFarmHouse(Game1.currentFloor);
				Game1.player.Money -= Game1.floorPrice;
				Game1.addHUDMessage(new HUDMessage("Flooring applied", Color.Green, 3500f));
				return true;
			}
			return false;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000B33D0 File Offset: 0x000B15D0
		public static int numSilos()
		{
			int num = 0;
			foreach (Building b in (Game1.getLocationFromName("Farm") as Farm).buildings)
			{
				if (b.buildingType.Equals("Silo") && b.daysOfConstructionLeft <= 0)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000B344C File Offset: 0x000B164C
		public static List<Item> getCarpenterStock()
		{
			List<Item> stock = new List<Item>();
			stock.Add(new Object(Vector2.Zero, 388, 2147483647));
			stock.Add(new Object(Vector2.Zero, 390, 2147483647));
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			stock.Add(new Furniture(1614, Vector2.Zero));
			stock.Add(new Furniture(1616, Vector2.Zero));
			switch (Game1.dayOfMonth % 7)
			{
			case 0:
				stock.Add(Utility.getRandomFurniture(r, stock, 1296, 1391));
				stock.Add(Utility.getRandomFurniture(r, stock, 416, 537));
				break;
			case 1:
				stock.Add(new Furniture(0, Vector2.Zero));
				stock.Add(new Furniture(192, Vector2.Zero));
				stock.Add(new Furniture(704, Vector2.Zero));
				stock.Add(new Furniture(1120, Vector2.Zero));
				stock.Add(new Furniture(1216, Vector2.Zero));
				stock.Add(new Furniture(1391, Vector2.Zero));
				break;
			case 2:
				stock.Add(new Furniture(3, Vector2.Zero));
				stock.Add(new Furniture(197, Vector2.Zero));
				stock.Add(new Furniture(709, Vector2.Zero));
				stock.Add(new Furniture(1122, Vector2.Zero));
				stock.Add(new Furniture(1218, Vector2.Zero));
				stock.Add(new Furniture(1393, Vector2.Zero));
				break;
			case 3:
				stock.Add(new Furniture(6, Vector2.Zero));
				stock.Add(new Furniture(202, Vector2.Zero));
				stock.Add(new Furniture(714, Vector2.Zero));
				stock.Add(new Furniture(1124, Vector2.Zero));
				stock.Add(new Furniture(1220, Vector2.Zero));
				stock.Add(new Furniture(1395, Vector2.Zero));
				break;
			case 4:
				stock.Add(Utility.getRandomFurniture(r, stock, 1296, 1391));
				stock.Add(Utility.getRandomFurniture(r, stock, 1296, 1391));
				break;
			case 5:
				stock.Add(Utility.getRandomFurniture(r, stock, 1443, 1450));
				stock.Add(Utility.getRandomFurniture(r, stock, 288, 313));
				break;
			case 6:
				stock.Add(Utility.getRandomFurniture(r, stock, 1565, 1607));
				stock.Add(Utility.getRandomFurniture(r, stock, 12, 129));
				break;
			}
			stock.Add(Utility.getRandomFurniture(r, stock, 0, 1462));
			stock.Add(Utility.getRandomFurniture(r, stock, 0, 1462));
			while (r.NextDouble() < 0.25)
			{
				stock.Add(Utility.getRandomFurniture(r, stock, 1673, 1815));
			}
			stock.Add(new Furniture(1402, Vector2.Zero));
			stock.Add(new TV(1466, Vector2.Zero));
			stock.Add(new TV(1680, Vector2.Zero));
			if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
			{
				stock.Add(new TV(1468, Vector2.Zero));
			}
			if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
			{
				stock.Add(new Furniture(1226, Vector2.Zero));
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Wooden Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 143, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Stone Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 144, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Barrel Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 150, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Stump Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 147, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Gold Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 145, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Carved Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 148, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Skull Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 149, true)
				{
					isRecipe = true
				});
			}
			else if (!Game1.player.craftingRecipes.ContainsKey("Marble Brazier"))
			{
				stock.Add(new Torch(Vector2.Zero, 151, true)
				{
					isRecipe = true
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Wood Lamp-post"))
			{
				stock.Add(new Object(Vector2.Zero, 152, true)
				{
					isRecipe = true
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Iron Lamp-post"))
			{
				stock.Add(new Object(Vector2.Zero, 153, true)
				{
					isRecipe = true
				});
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Wood Floor"))
			{
				stock.Add(new Object(328, 1, true, 50, 0));
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Stone Floor"))
			{
				stock.Add(new Object(329, 1, true, 50, 0));
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Stepping Stone Path"))
			{
				stock.Add(new Object(415, 1, true, 50, 0));
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Straw Floor"))
			{
				stock.Add(new Object(401, 1, true, 100, 0));
			}
			if (!Game1.player.craftingRecipes.ContainsKey("Crystal Path"))
			{
				stock.Add(new Object(409, 1, true, 100, 0));
			}
			return stock;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000B3B10 File Offset: 0x000B1D10
		private static bool isFurnitureOffLimitsForSale(int index)
		{
			if (index <= 1468)
			{
				if (index <= 1308)
				{
					if (index != 131 && index != 1226)
					{
						switch (index)
						{
						case 1298:
						case 1299:
						case 1300:
						case 1301:
						case 1302:
						case 1303:
						case 1304:
						case 1305:
						case 1306:
						case 1307:
						case 1308:
							break;
						default:
							return false;
						}
					}
				}
				else if (index != 1402 && index != 1466 && index != 1468)
				{
					return false;
				}
			}
			else if (index <= 1554)
			{
				if (index != 1541 && index != 1545 && index != 1554)
				{
					return false;
				}
			}
			else if (index <= 1671)
			{
				if (index != 1669 && index != 1671)
				{
					return false;
				}
			}
			else if (index != 1680 && index != 1733)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000B3BE8 File Offset: 0x000B1DE8
		private static Furniture getRandomFurniture(Random r, List<Item> stock, int lowerIndexBound = 0, int upperIndexBound = 1462)
		{
			int index = -1;
			Dictionary<int, string> furnitureData = Game1.content.Load<Dictionary<int, string>>("Data\\Furniture");
			do
			{
				index = r.Next(lowerIndexBound, upperIndexBound);
				if (stock != null)
				{
					foreach (Item i in stock)
					{
						if (i is Furniture && i.parentSheetIndex == index)
						{
							index = -1;
						}
					}
				}
			}
			while (Utility.isFurnitureOffLimitsForSale(index) || !furnitureData.ContainsKey(index));
			return new Furniture(index, Vector2.Zero)
			{
				stack = 2147483647
			};
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000B3C88 File Offset: 0x000B1E88
		public static List<Item> getSaloonStock()
		{
			List<Item> stock = new List<Item>();
			stock.Add(new Object(Vector2.Zero, 346, 2147483647));
			stock.Add(new Object(Vector2.Zero, 196, 2147483647));
			stock.Add(new Object(Vector2.Zero, 216, 2147483647));
			stock.Add(new Object(Vector2.Zero, 224, 2147483647));
			stock.Add(new Object(Vector2.Zero, 206, 2147483647));
			stock.Add(new Object(Vector2.Zero, 395, 2147483647));
			if (!Game1.player.cookingRecipes.ContainsKey("Hashbrowns"))
			{
				stock.Add(new Object(210, 1, true, 25, 0));
			}
			if (!Game1.player.cookingRecipes.ContainsKey("Omelet"))
			{
				stock.Add(new Object(195, 1, true, 50, 0));
			}
			if (!Game1.player.cookingRecipes.ContainsKey("Pancakes"))
			{
				stock.Add(new Object(211, 1, true, 50, 0));
			}
			if (!Game1.player.cookingRecipes.ContainsKey("Bread"))
			{
				stock.Add(new Object(216, 1, true, 50, 0));
			}
			if (!Game1.player.cookingRecipes.ContainsKey("Tortilla"))
			{
				stock.Add(new Object(229, 1, true, 50, 0));
			}
			if (!Game1.player.cookingRecipes.ContainsKey("Pizza"))
			{
				stock.Add(new Object(206, 1, true, 75, 0));
			}
			if (!Game1.player.cookingRecipes.ContainsKey("Maki Roll"))
			{
				stock.Add(new Object(228, 1, true, 150, 0));
			}
			if (Game1.dishOfTheDay.stack > 0)
			{
				stock.Add(Game1.dishOfTheDay);
			}
			return stock;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000B3E80 File Offset: 0x000B2080
		public static bool removeLightSource(int identifier)
		{
			bool removed = false;
			for (int i = Game1.currentLightSources.Count - 1; i >= 0; i--)
			{
				if (Game1.currentLightSources.ElementAt(i).identifier == identifier)
				{
					Game1.currentLightSources.Remove(Game1.currentLightSources.ElementAt(i));
					removed = true;
				}
			}
			return removed;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000B3ED4 File Offset: 0x000B20D4
		public static Horse findHorse()
		{
			using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (NPC i in enumerator.Current.characters)
					{
						if (i is Horse)
						{
							return i as Horse;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000B3F6C File Offset: 0x000B216C
		public static void addSmokePuff(GameLocation l, Vector2 v, int delay = 0)
		{
			l.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), v, false, 0.002f, Color.Gray)
			{
				alpha = 0.75f,
				motion = new Vector2(0f, -0.5f),
				acceleration = new Vector2(0.002f, 0f),
				interval = 99999f,
				layerDepth = 1f,
				scale = (float)(Game1.pixelZoom / 2),
				scaleChange = 0.02f,
				rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f,
				delayBeforeAnimationStart = delay
			});
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000B4038 File Offset: 0x000B2238
		public static LightSource getLightSource(int identifier)
		{
			foreach (LightSource i in Game1.currentLightSources)
			{
				if (i.identifier == identifier)
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000B4094 File Offset: 0x000B2294
		public static Dictionary<Item, int[]> getAllWallpapersAndFloorsForFree()
		{
			Dictionary<Item, int[]> decors = new Dictionary<Item, int[]>();
			for (int i = 0; i < 112; i++)
			{
				decors.Add(new Wallpaper(i, false)
				{
					stack = 2147483647
				}, new int[]
				{
					0,
					2147483647
				});
			}
			for (int j = 0; j < 40; j++)
			{
				decors.Add(new Wallpaper(j, true)
				{
					stack = 2147483647
				}, new int[]
				{
					0,
					2147483647
				});
			}
			return decors;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000B4110 File Offset: 0x000B2310
		public static Dictionary<Item, int[]> getAllFurnituresForFree()
		{
			Dictionary<Item, int[]> decors = new Dictionary<Item, int[]>();
			foreach (KeyValuePair<int, string> v in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture"))
			{
				if (!Utility.isFurnitureOffLimitsForSale(v.Key))
				{
					decors.Add(new Furniture(v.Key, Vector2.Zero), new int[]
					{
						0,
						2147483647
					});
				}
			}
			decors.Add(new Furniture(1402, Vector2.Zero), new int[]
			{
				0,
				2147483647
			});
			decors.Add(new TV(1680, Vector2.Zero), new int[]
			{
				0,
				2147483647
			});
			decors.Add(new TV(1466, Vector2.Zero), new int[]
			{
				0,
				2147483647
			});
			decors.Add(new TV(1468, Vector2.Zero), new int[]
			{
				0,
				2147483647
			});
			return decors;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000B422C File Offset: 0x000B242C
		public static FarmEvent pickFarmEvent()
		{
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			if (Game1.weddingToday)
			{
				return null;
			}
			if (Game1.stats.DaysPlayed == 31u)
			{
				return new SoundInTheNightEvent(4);
			}
			if (Game1.player.mailForTomorrow.Contains("jojaPantry%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaPantry"))
			{
				return new WorldChangeEvent(0);
			}
			if (Game1.player.mailForTomorrow.Contains("ccPantry%&NL&%") || Game1.player.mailForTomorrow.Contains("ccPantry"))
			{
				return new WorldChangeEvent(1);
			}
			if (Game1.player.mailForTomorrow.Contains("jojaVault%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaVault"))
			{
				return new WorldChangeEvent(6);
			}
			if (Game1.player.mailForTomorrow.Contains("ccVault%&NL&%") || Game1.player.mailForTomorrow.Contains("ccVault"))
			{
				return new WorldChangeEvent(7);
			}
			if (Game1.player.mailForTomorrow.Contains("jojaBoilerRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaBoilerRoom"))
			{
				return new WorldChangeEvent(2);
			}
			if (Game1.player.mailForTomorrow.Contains("ccBoilerRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("ccBoilerRoom"))
			{
				return new WorldChangeEvent(3);
			}
			if (Game1.player.mailForTomorrow.Contains("jojaCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaCraftsRoom"))
			{
				return new WorldChangeEvent(4);
			}
			if (Game1.player.mailForTomorrow.Contains("ccCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("ccCraftsRoom"))
			{
				return new WorldChangeEvent(5);
			}
			if (Game1.player.mailForTomorrow.Contains("jojaFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaFishTank"))
			{
				return new WorldChangeEvent(8);
			}
			if (Game1.player.mailForTomorrow.Contains("ccFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("ccFishTank"))
			{
				return new WorldChangeEvent(9);
			}
			if (Game1.player.isMarried() && Game1.player.spouse != null && Game1.getCharacterFromName(Game1.player.spouse, false).daysUntilBirthing == 0)
			{
				return new BirthingEvent();
			}
			if (Game1.player.isMarried() && Game1.player.spouse != null && Game1.getCharacterFromName(Game1.player.spouse, false).canGetPregnant() && r.NextDouble() < 0.05)
			{
				return new QuestionEvent(1);
			}
			if (r.NextDouble() < 0.01 && !Game1.currentSeason.Equals("winter"))
			{
				return new FairyEvent();
			}
			if (r.NextDouble() < 0.01)
			{
				return new WitchEvent();
			}
			if (r.NextDouble() < 0.01)
			{
				return new SoundInTheNightEvent(1);
			}
			if (r.NextDouble() < 0.01 && Game1.year > 1)
			{
				return new SoundInTheNightEvent(0);
			}
			if (r.NextDouble() < 0.01)
			{
				return new SoundInTheNightEvent(3);
			}
			if (r.NextDouble() < 0.5)
			{
				return new QuestionEvent(2);
			}
			return new SoundInTheNightEvent(2);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x000B4598 File Offset: 0x000B2798
		public static string capitalizeFirstLetter(string s)
		{
			if (s == null || s.Length < 1)
			{
				return "";
			}
			return s[0].ToString().ToUpper() + ((s.Length > 1) ? s.Substring(1) : "");
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000B45E8 File Offset: 0x000B27E8
		public static Dictionary<Item, int[]> getBlacksmithStock()
		{
			return new Dictionary<Item, int[]>
			{
				{
					new Object(Vector2.Zero, 378, 2147483647),
					new int[]
					{
						75,
						2147483647
					}
				},
				{
					new Object(Vector2.Zero, 380, 2147483647),
					new int[]
					{
						150,
						2147483647
					}
				},
				{
					new Object(Vector2.Zero, 382, 2147483647),
					new int[]
					{
						150,
						2147483647
					}
				},
				{
					new Object(Vector2.Zero, 384, 2147483647),
					new int[]
					{
						400,
						2147483647
					}
				}
			};
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000B46BC File Offset: 0x000B28BC
		public static bool alreadyHasLightSourceWithThisID(int identifier)
		{
			using (HashSet<LightSource>.Enumerator enumerator = Game1.currentLightSources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.identifier == identifier)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x000B4718 File Offset: 0x000B2918
		public static void repositionLightSource(int identifier, Vector2 position)
		{
			foreach (LightSource i in Game1.currentLightSources)
			{
				if (i.identifier == identifier)
				{
					i.position = position;
				}
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x000B4774 File Offset: 0x000B2974
		public static Dictionary<Item, int[]> getAnimalShopStock()
		{
			Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
			stock.Add(new Object(178, 1, false, -1, 0), new int[]
			{
				50,
				2147483647
			});
			stock.Add(new Object(Vector2.Zero, 104, false)
			{
				price = 2000,
				Stack = 1
			}, new int[]
			{
				2000,
				2147483647
			});
			if (Game1.player.hasItemWithNameThatContains("Milk Pail") == null)
			{
				stock.Add(new MilkPail(), new int[]
				{
					1000,
					1
				});
			}
			if (Game1.player.hasItemWithNameThatContains("Shears") == null)
			{
				stock.Add(new Shears(), new int[]
				{
					1000,
					1
				});
			}
			return stock;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x000B4848 File Offset: 0x000B2A48
		public static bool areThereAnyOtherAnimalsWithThisName(string name)
		{
			Farm f = Game1.getLocationFromName("Farm") as Farm;
			foreach (Building b in f.buildings)
			{
				if (b.indoors is AnimalHouse)
				{
					using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator2 = (b.indoors as AnimalHouse).animals.Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.name.Equals(name))
							{
								bool result = true;
								return result;
							}
						}
					}
				}
			}
			using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator2 = f.animals.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.name.Equals(name))
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x000B4964 File Offset: 0x000B2B64
		public static string getNumberWithCommas(int number)
		{
			StringBuilder s = new StringBuilder(string.Concat(number));
			for (int i = s.Length - 4; i >= 0; i -= 3)
			{
				s.Insert(i + 1, ",");
			}
			return s.ToString();
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000B49AC File Offset: 0x000B2BAC
		public static List<Object> getPurchaseAnimalStock()
		{
			List<Object> arg_61_0 = new List<Object>();
			Object o = new Object(100, 1, false, 400, 0)
			{
				name = "Chicken",
				type = ((Game1.getFarm().isBuildingConstructed("Coop") || Game1.getFarm().isBuildingConstructed("Deluxe Coop") || Game1.getFarm().isBuildingConstructed("Big Coop")) ? null : "Requires construction of a Coop")
			};
			arg_61_0.Add(o);
			o = new Object(100, 1, false, 750, 0)
			{
				name = "Dairy Cow",
				type = ((Game1.getFarm().isBuildingConstructed("Barn") || Game1.getFarm().isBuildingConstructed("Deluxe Barn") || Game1.getFarm().isBuildingConstructed("Big Barn")) ? null : "Requires construction of a Barn")
			};
			arg_61_0.Add(o);
			o = new Object(100, 1, false, 2000, 0)
			{
				name = "Goat",
				type = ((Game1.getFarm().isBuildingConstructed("Big Barn") || Game1.getFarm().isBuildingConstructed("Deluxe Barn")) ? null : "Requires construction of a Big Barn")
			};
			arg_61_0.Add(o);
			o = new Object(100, 1, false, 2000, 0)
			{
				name = "Duck",
				type = ((Game1.getFarm().isBuildingConstructed("Big Coop") || Game1.getFarm().isBuildingConstructed("Deluxe Coop")) ? null : "Requires construction of a Big Coop")
			};
			arg_61_0.Add(o);
			o = new Object(100, 1, false, 4000, 0)
			{
				name = "Sheep",
				type = (Game1.getFarm().isBuildingConstructed("Deluxe Barn") ? null : "Requires construction of a Deluxe Barn")
			};
			arg_61_0.Add(o);
			o = new Object(100, 1, false, 4000, 0)
			{
				name = "Rabbit",
				type = (Game1.getFarm().isBuildingConstructed("Deluxe Coop") ? null : "Requires construction of a Deluxe Coop")
			};
			arg_61_0.Add(o);
			o = new Object(100, 1, false, 8000, 0)
			{
				name = "Pig",
				type = (Game1.getFarm().isBuildingConstructed("Deluxe Barn") ? null : "Requires construction of a Deluxe Barn")
			};
			arg_61_0.Add(o);
			return arg_61_0;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000B4BEC File Offset: 0x000B2DEC
		public static List<Item> getShopStock(bool Pierres)
		{
			List<Item> stock = new List<Item>();
			if (Pierres)
			{
				if (Game1.currentSeason.Equals("spring"))
				{
					stock.Add(new Object(Vector2.Zero, 472, 2147483647));
					stock.Add(new Object(Vector2.Zero, 473, 2147483647));
					stock.Add(new Object(Vector2.Zero, 474, 2147483647));
					stock.Add(new Object(Vector2.Zero, 475, 2147483647));
					stock.Add(new Object(Vector2.Zero, 427, 2147483647));
					stock.Add(new Object(Vector2.Zero, 429, 2147483647));
					stock.Add(new Object(Vector2.Zero, 477, 2147483647));
					stock.Add(new Object(628, 2147483647, false, 1700, 0));
					stock.Add(new Object(629, 2147483647, false, 1000, 0));
					if (Game1.year > 1)
					{
						stock.Add(new Object(Vector2.Zero, 476, 2147483647));
					}
				}
				if (Game1.currentSeason.Equals("summer"))
				{
					stock.Add(new Object(Vector2.Zero, 480, 2147483647));
					stock.Add(new Object(Vector2.Zero, 482, 2147483647));
					stock.Add(new Object(Vector2.Zero, 483, 2147483647));
					stock.Add(new Object(Vector2.Zero, 484, 2147483647));
					stock.Add(new Object(Vector2.Zero, 479, 2147483647));
					stock.Add(new Object(Vector2.Zero, 302, 2147483647));
					stock.Add(new Object(Vector2.Zero, 453, 2147483647));
					stock.Add(new Object(Vector2.Zero, 455, 2147483647));
					stock.Add(new Object(630, 2147483647, false, 2000, 0));
					stock.Add(new Object(631, 2147483647, false, 3000, 0));
					if (Game1.year > 1)
					{
						stock.Add(new Object(Vector2.Zero, 485, 2147483647));
					}
				}
				if (Game1.currentSeason.Equals("fall"))
				{
					stock.Add(new Object(Vector2.Zero, 487, 2147483647));
					stock.Add(new Object(Vector2.Zero, 488, 2147483647));
					stock.Add(new Object(Vector2.Zero, 490, 2147483647));
					stock.Add(new Object(Vector2.Zero, 299, 2147483647));
					stock.Add(new Object(Vector2.Zero, 301, 2147483647));
					stock.Add(new Object(Vector2.Zero, 492, 2147483647));
					stock.Add(new Object(Vector2.Zero, 491, 2147483647));
					stock.Add(new Object(Vector2.Zero, 493, 2147483647));
					stock.Add(new Object(431, 2147483647, false, 100, 0));
					stock.Add(new Object(Vector2.Zero, 425, 2147483647));
					stock.Add(new Object(632, 2147483647, false, 3000, 0));
					stock.Add(new Object(633, 2147483647, false, 2000, 0));
					if (Game1.year > 1)
					{
						stock.Add(new Object(Vector2.Zero, 489, 2147483647));
					}
				}
				stock.Add(new Object(Vector2.Zero, 297, 2147483647));
				stock.Add(new Object(Vector2.Zero, 245, 2147483647));
				stock.Add(new Object(Vector2.Zero, 246, 2147483647));
				stock.Add(new Object(Vector2.Zero, 423, 2147483647));
				Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
				stock.Add(new Wallpaper(r.Next(112), false)
				{
					stack = 2147483647
				});
				stock.Add(new Wallpaper(r.Next(40), true)
				{
					stack = 2147483647
				});
				if (Game1.player.achievements.Contains(38))
				{
					stock.Add(new Object(Vector2.Zero, 458, 2147483647));
				}
			}
			else
			{
				if (Game1.currentSeason.Equals("spring"))
				{
					stock.Add(new Object(Vector2.Zero, 478, 2147483647));
				}
				if (Game1.currentSeason.Equals("summer"))
				{
					stock.Add(new Object(Vector2.Zero, 486, 2147483647));
					stock.Add(new Object(Vector2.Zero, 481, 2147483647));
				}
				if (Game1.currentSeason.Equals("fall"))
				{
					stock.Add(new Object(Vector2.Zero, 493, 2147483647));
					stock.Add(new Object(Vector2.Zero, 494, 2147483647));
				}
				stock.Add(new Object(Vector2.Zero, 88, 2147483647));
				stock.Add(new Object(Vector2.Zero, 90, 2147483647));
			}
			return stock;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000B51B0 File Offset: 0x000B33B0
		public static List<Vector2> getCornersOfThisRectangle(Microsoft.Xna.Framework.Rectangle r)
		{
			return new List<Vector2>
			{
				new Vector2((float)r.X, (float)r.Y),
				new Vector2((float)(r.Right - 1), (float)r.Y),
				new Vector2((float)(r.Right - 1), (float)(r.Bottom - 1)),
				new Vector2((float)r.X, (float)(r.Bottom - 1))
			};
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000B5234 File Offset: 0x000B3434
		public static Farmer getNearestFarmerInCurrentLocation(Vector2 tileLocation)
		{
			float lowestDistance = 999999f;
			List<Farmer> farmers = Game1.currentLocation.getFarmers();
			Farmer nearest = null;
			for (int i = 0; i < farmers.Count; i++)
			{
				float tmp = lowestDistance;
				lowestDistance = Math.Min(lowestDistance, Math.Abs(farmers[i].getTileLocation().X - tileLocation.X) + Math.Abs(farmers[i].getTileLocation().Y - tileLocation.Y));
				if (lowestDistance < tmp)
				{
					nearest = farmers[i];
				}
			}
			return nearest;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000B52B8 File Offset: 0x000B34B8
		private static int priceForToolUpgradeLevel(int level)
		{
			switch (level)
			{
			case 1:
				return 2000;
			case 2:
				return 5000;
			case 3:
				return 10000;
			case 4:
				return 25000;
			default:
				return 2000;
			}
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000B52F1 File Offset: 0x000B34F1
		private static int indexOfExtraMaterialForToolUpgrade(int level)
		{
			switch (level)
			{
			case 1:
				return 334;
			case 2:
				return 335;
			case 3:
				return 336;
			case 4:
				return 337;
			default:
				return 334;
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000B532C File Offset: 0x000B352C
		public static Dictionary<Item, int[]> getBlacksmithUpgradeStock(Farmer who)
		{
			Dictionary<Item, int[]> toolStock = new Dictionary<Item, int[]>();
			Tool axe = who.getToolFromName("Axe");
			Tool wateringCan = who.getToolFromName("Watering Can");
			Tool pickAxe = who.getToolFromName("Pickaxe");
			Tool hoe = who.getToolFromName("Hoe");
			if (axe != null && axe.upgradeLevel < 4)
			{
				Tool shopAxe = new Axe();
				shopAxe.UpgradeLevel = axe.upgradeLevel + 1;
				toolStock.Add(shopAxe, new int[]
				{
					Utility.priceForToolUpgradeLevel(shopAxe.UpgradeLevel),
					1,
					Utility.indexOfExtraMaterialForToolUpgrade(shopAxe.upgradeLevel)
				});
			}
			if (wateringCan != null && wateringCan.upgradeLevel < 4)
			{
				Tool shopAxe2 = new WateringCan();
				shopAxe2.UpgradeLevel = wateringCan.upgradeLevel + 1;
				toolStock.Add(shopAxe2, new int[]
				{
					Utility.priceForToolUpgradeLevel(shopAxe2.UpgradeLevel),
					1,
					Utility.indexOfExtraMaterialForToolUpgrade(shopAxe2.upgradeLevel)
				});
			}
			if (pickAxe != null && pickAxe.upgradeLevel < 4)
			{
				Tool shopAxe3 = new Pickaxe();
				shopAxe3.UpgradeLevel = pickAxe.upgradeLevel + 1;
				toolStock.Add(shopAxe3, new int[]
				{
					Utility.priceForToolUpgradeLevel(shopAxe3.UpgradeLevel),
					1,
					Utility.indexOfExtraMaterialForToolUpgrade(shopAxe3.upgradeLevel)
				});
			}
			if (hoe != null && hoe.upgradeLevel < 4)
			{
				Tool shopAxe4 = new Hoe();
				shopAxe4.UpgradeLevel = hoe.upgradeLevel + 1;
				toolStock.Add(shopAxe4, new int[]
				{
					Utility.priceForToolUpgradeLevel(shopAxe4.UpgradeLevel),
					1,
					Utility.indexOfExtraMaterialForToolUpgrade(shopAxe4.upgradeLevel)
				});
			}
			return toolStock;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000B54BC File Offset: 0x000B36BC
		public static Vector2 GetCurvePoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float cx = 3f * (p1.X - p0.X);
			float cy = 3f * (p1.Y - p0.Y);
			float bx = 3f * (p2.X - p1.X) - cx;
			float by = 3f * (p2.Y - p1.Y) - cy;
			float arg_88_0 = p3.X - p0.X - cx - bx;
			float ay = p3.Y - p0.Y - cy - by;
			float Cube = t * t * t;
			float Square = t * t;
			float arg_B2_0 = arg_88_0 * Cube + bx * Square + cx * t + p0.X;
			float resY = ay * Cube + by * Square + cy * t + p0.Y;
			return new Vector2(arg_B2_0, resY);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000B5580 File Offset: 0x000B3780
		public static GameLocation getGameLocationOfCharacter(NPC n)
		{
			foreach (GameLocation i in Game1.locations)
			{
				if (i.characters.Contains(n))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000B55E0 File Offset: 0x000B37E0
		public static int[] parseStringToIntArray(string s, char delimiter = ' ')
		{
			string[] split = s.Split(new char[]
			{
				delimiter
			});
			int[] result = new int[split.Length];
			for (int i = 0; i < split.Length; i++)
			{
				result[i] = Convert.ToInt32(split[i]);
			}
			return result;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000B5624 File Offset: 0x000B3824
		public static void drawLineWithScreenCoordinates(int x1, int y1, int x2, int y2, SpriteBatch b, Color color1, float layerDepth = 1f)
		{
			Vector2 arg_15_0 = new Vector2((float)x2, (float)y2);
			Vector2 start = new Vector2((float)x1, (float)y1);
			Vector2 edge = arg_15_0 - start;
			float angle = (float)Math.Atan2((double)edge.Y, (double)edge.X);
			b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color1, angle, new Vector2(0f, 0f), SpriteEffects.None, layerDepth);
			b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int)start.X, (int)start.Y + 1, (int)edge.Length(), 1), null, color1, angle, new Vector2(0f, 0f), SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000B56F0 File Offset: 0x000B38F0
		public static string getRandomNonLoopingSong()
		{
			switch (Game1.random.Next(7))
			{
			case 0:
				return "springsongs";
			case 1:
				return "summersongs";
			case 2:
				return "fallsongs";
			case 3:
				return "wintersongs";
			case 4:
				return "EarthMine";
			case 5:
				return "FrostMine";
			case 6:
				return "LavaMine";
			default:
				return "fallsongs";
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x000B575C File Offset: 0x000B395C
		public static Farmer isThereAFarmerWithinDistance(Vector2 tileLocation, int tilesAway)
		{
			foreach (Farmer f in Game1.currentLocation.getFarmers())
			{
				if (Math.Abs(tileLocation.X - f.getTileLocation().X) <= (float)tilesAway && Math.Abs(tileLocation.Y - f.getTileLocation().Y) <= (float)tilesAway)
				{
					return f;
				}
			}
			return null;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000B57EC File Offset: 0x000B39EC
		public static Character isThereAFarmerOrCharacterWithinDistance(Vector2 tileLocation, int tilesAway, GameLocation environment)
		{
			foreach (Character c in environment.characters)
			{
				if (Vector2.Distance(c.getTileLocation(), tileLocation) <= (float)tilesAway)
				{
					return c;
				}
			}
			return Utility.isThereAFarmerWithinDistance(tileLocation, tilesAway);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000B5858 File Offset: 0x000B3A58
		public static Color getRedToGreenLerpColor(float power)
		{
			return new Color((int)((power <= 0.5f) ? 255f : ((1f - power) * 2f * 255f)), (int)Math.Min(255f, power * 2f * 255f), 0);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x000B58A6 File Offset: 0x000B3AA6
		public static FarmHouse getHomeOfFarmer(Farmer who)
		{
			if (!Game1.IsMultiplayer)
			{
				return Game1.getLocationFromName("FarmHouse") as FarmHouse;
			}
			return null;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000B58C0 File Offset: 0x000B3AC0
		public static Vector2 getRandomPositionOnScreen()
		{
			return new Vector2((float)Game1.random.Next(Game1.viewport.Width), (float)Game1.random.Next(Game1.viewport.Height));
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000B58F1 File Offset: 0x000B3AF1
		public static Microsoft.Xna.Framework.Rectangle getRectangleCenteredAt(Vector2 v, int size)
		{
			return new Microsoft.Xna.Framework.Rectangle((int)v.X - size / 2, (int)v.Y - size / 2, size, size);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000B5910 File Offset: 0x000B3B10
		public static bool checkForCharacterInteractionAtTile(Vector2 tileLocation, Farmer who)
		{
			if (Game1.currentLocation.isCharacterAtTile(tileLocation) != null && !Game1.currentLocation.isCharacterAtTile(tileLocation).IsMonster)
			{
				if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift() && who.friendships.ContainsKey(Game1.currentLocation.isCharacterAtTile(tileLocation).name) && who.friendships[Game1.currentLocation.isCharacterAtTile(tileLocation).name][3] != 1 && !Game1.eventUp)
				{
					Game1.mouseCursor = 3;
				}
				else if (Game1.currentLocation.isCharacterAtTile(tileLocation).CurrentDialogue != null && (Game1.currentLocation.isCharacterAtTile(tileLocation).CurrentDialogue.Count > 0 || Game1.currentLocation.isCharacterAtTile(tileLocation).hasTemporaryMessageAvailable()) && !Game1.currentLocation.isCharacterAtTile(tileLocation).isOnSilentTemporaryMessage())
				{
					Game1.mouseCursor = 4;
				}
				Game1.currentLocation.checkForSpecialCharacterIconAtThisTile(tileLocation);
				if (Game1.mouseCursor == 3 || Game1.mouseCursor == 4)
				{
					if (Utility.tileWithinRadiusOfPlayer((int)tileLocation.X, (int)tileLocation.Y, 1, who))
					{
						Game1.mouseCursorTransparency = 1f;
					}
					else
					{
						Game1.mouseCursorTransparency = 0.5f;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000B5A44 File Offset: 0x000B3C44
		public static bool canGrabSomethingFromHere(int x, int y, Farmer who)
		{
			Vector2 tileLocation = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			if (Game1.currentLocation.isObjectAt(x, y))
			{
				Game1.currentLocation.getObjectAt(x, y).hoverAction();
			}
			if (Utility.checkForCharacterInteractionAtTile(tileLocation, who))
			{
				return false;
			}
			if (Utility.checkForCharacterInteractionAtTile(tileLocation + new Vector2(0f, 1f), who))
			{
				return false;
			}
			if (who.IsMainPlayer)
			{
				if (Game1.currentLocation.Objects.ContainsKey(tileLocation))
				{
					if (Game1.currentLocation.Objects[tileLocation].readyForHarvest || (Game1.currentLocation.Objects[tileLocation].Name.Contains("Table") && Game1.currentLocation.Objects[tileLocation].heldObject != null) || Game1.currentLocation.Objects[tileLocation].isSpawnedObject)
					{
						Game1.mouseCursor = 6;
						if (!Utility.withinRadiusOfPlayer(x, y, 1, who))
						{
							Game1.mouseCursorTransparency = 0.5f;
							return false;
						}
						return true;
					}
				}
				else if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) && Game1.currentLocation.terrainFeatures[tileLocation].GetType() == typeof(HoeDirt) && ((HoeDirt)Game1.currentLocation.terrainFeatures[tileLocation]).readyForHarvest())
				{
					Game1.mouseCursor = 6;
					if (!Utility.withinRadiusOfPlayer(x, y, 1, who))
					{
						Game1.mouseCursorTransparency = 0.5f;
						return false;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000B5BD0 File Offset: 0x000B3DD0
		public static Microsoft.Xna.Framework.Rectangle getSourceRectWithinRectangularRegion(int regionX, int regionY, int regionWidth, int sourceIndex, int sourceWidth, int sourceHeight)
		{
			int sourceRectWidthsOfRegion = regionWidth / sourceWidth;
			return new Microsoft.Xna.Framework.Rectangle(regionX + sourceIndex % sourceRectWidthsOfRegion * sourceWidth, regionY + sourceIndex / sourceRectWidthsOfRegion * sourceHeight, sourceWidth, sourceHeight);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000B5BFC File Offset: 0x000B3DFC
		public static void drawWithShadow(SpriteBatch b, Texture2D texture, Vector2 position, Microsoft.Xna.Framework.Rectangle sourceRect, Color color, float rotation, Vector2 origin, float scale = -1f, bool flipped = false, float layerDepth = -1f, int horizontalShadowOffset = -1, int verticalShadowOffset = -1, float shadowIntensity = 0.35f)
		{
			if (scale == -1f)
			{
				scale = (float)Game1.pixelZoom;
			}
			if (layerDepth == -1f)
			{
				layerDepth = position.Y / 10000f;
			}
			if (horizontalShadowOffset == -1)
			{
				horizontalShadowOffset = -Game1.pixelZoom;
			}
			if (verticalShadowOffset == -1)
			{
				verticalShadowOffset = Game1.pixelZoom;
			}
			b.Draw(texture, position + new Vector2((float)horizontalShadowOffset, (float)verticalShadowOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.Black * shadowIntensity, rotation, origin, scale, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth - 0.0001f);
			b.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(sourceRect), color, rotation, origin, scale, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000B5CAC File Offset: 0x000B3EAC
		public static void drawTextWithShadow(SpriteBatch b, string text, SpriteFont font, Vector2 position, Color color, float scale = 1f, float layerDepth = -1f, int horizontalShadowOffset = -1, int verticalShadowOffset = -1, float shadowIntensity = 1f, int numShadows = 3)
		{
			if (layerDepth == -1f)
			{
				layerDepth = position.Y / 10000f;
			}
			if (horizontalShadowOffset == -1)
			{
				horizontalShadowOffset = (font.Equals(Game1.smallFont) ? (-Game1.pixelZoom + 2) : (-Game1.pixelZoom + 1));
			}
			if (verticalShadowOffset == -1)
			{
				verticalShadowOffset = (font.Equals(Game1.smallFont) ? (Game1.pixelZoom - 2) : (Game1.pixelZoom - 1));
			}
			if (text == null)
			{
				text = "";
			}
			b.DrawString(font, text, position + new Vector2((float)horizontalShadowOffset, (float)verticalShadowOffset), new Color(221, 148, 84) * shadowIntensity, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth - 0.0001f);
			if (numShadows == 2)
			{
				b.DrawString(font, text, position + new Vector2((float)horizontalShadowOffset, 0f), new Color(221, 148, 84) * shadowIntensity, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth - 0.0002f);
			}
			if (numShadows == 3)
			{
				b.DrawString(font, text, position + new Vector2(0f, (float)verticalShadowOffset), new Color(221, 148, 84) * shadowIntensity, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth - 0.0003f);
			}
			b.DrawString(font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000B5E20 File Offset: 0x000B4020
		public static void drawBoldText(SpriteBatch b, string text, SpriteFont font, Vector2 position, Color color, float scale = 1f, float layerDepth = -1f, int boldnessOffset = 1)
		{
			if (layerDepth == -1f)
			{
				layerDepth = position.Y / 10000f;
			}
			b.DrawString(font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
			b.DrawString(font, text, position + new Vector2((float)boldnessOffset, 0f), color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
			b.DrawString(font, text, position + new Vector2((float)boldnessOffset, (float)boldnessOffset), color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
			b.DrawString(font, text, position + new Vector2(0f, (float)boldnessOffset), color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000B5EE0 File Offset: 0x000B40E0
		public static bool playerCanPlaceItemHere(GameLocation location, Item item, int x, int y, Farmer f)
		{
			if (item == null || item is Tool || Game1.eventUp || f.bathingClothes)
			{
				return false;
			}
			if (Utility.withinRadiusOfPlayer(x, y, 1, f) || (Utility.withinRadiusOfPlayer(x, y, 2, f) && Game1.isAnyGamePadButtonBeingPressed() && Game1.mouseCursorTransparency == 0f) || ((item is Furniture || item is Wallpaper) && location is DecoratableLocation))
			{
				Vector2 tileLocation = new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
				if (item.canBePlacedHere(location, tileLocation))
				{
					if (!((Object)item).isPassable())
					{
						for (int i = 0; i < location.farmers.Count; i++)
						{
							if (location.farmers[i].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
							{
								return false;
							}
						}
					}
					if ((location.isTilePlaceable(tileLocation, item) && item.isPlaceable() && (((Object)item).isPassable() || !new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X * (float)Game1.tileSize), (int)(tileLocation.Y * (float)Game1.tileSize), Game1.tileSize, Game1.tileSize).Intersects(Game1.player.GetBoundingBox()))) || (((Object)item).category == -74 && location.terrainFeatures.ContainsKey(tileLocation) && location.terrainFeatures[tileLocation].GetType() == typeof(HoeDirt) && ((HoeDirt)location.terrainFeatures[tileLocation]).canPlantThisSeedHere((item as Object).ParentSheetIndex, (int)tileLocation.X, (int)tileLocation.Y, false)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000B60B8 File Offset: 0x000B42B8
		public static int getDirectionFromChange(Vector2 current, Vector2 previous, bool yBias = false)
		{
			if (!yBias && current.X > previous.X)
			{
				return 1;
			}
			if (!yBias && current.X < previous.X)
			{
				return 3;
			}
			if (current.Y > previous.Y)
			{
				return 2;
			}
			if (current.Y < previous.Y)
			{
				return 0;
			}
			if (current.X > previous.X)
			{
				return 1;
			}
			if (current.X < previous.X)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000B612C File Offset: 0x000B432C
		public static bool doesRectangleIntersectTile(Microsoft.Xna.Framework.Rectangle r, int tileX, int tileY)
		{
			Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize, Game1.tileSize, Game1.tileSize);
			return r.Intersects(tileRect);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000B6160 File Offset: 0x000B4360
		public static List<NPC> getAllCharacters()
		{
			List<NPC> characters = new List<NPC>();
			foreach (GameLocation i in Game1.locations)
			{
				characters.AddRange(i.characters);
			}
			Farm f = Game1.getFarm();
			if (f != null)
			{
				foreach (Building b in f.buildings)
				{
					if (b.indoors != null)
					{
						using (List<NPC>.Enumerator enumerator3 = b.indoors.characters.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								enumerator3.Current.currentLocation = b.indoors;
							}
						}
						characters.AddRange(b.indoors.characters);
					}
				}
			}
			return characters;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000B6270 File Offset: 0x000B4470
		public static Item removeItemFromInventory(int whichItemIndex, List<Item> items)
		{
			if (whichItemIndex >= 0 && whichItemIndex < items.Count && items[whichItemIndex] != null)
			{
				Item tmp = items[whichItemIndex];
				if (whichItemIndex == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && tmp != null)
				{
					tmp.actionWhenStopBeingHeld(Game1.player);
				}
				items[whichItemIndex] = null;
				return tmp;
			}
			return null;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000B62D3 File Offset: 0x000B44D3
		public static NPC getRandomTownNPC()
		{
			return Utility.getRandomTownNPC(Game1.random, 0);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x000B62E0 File Offset: 0x000B44E0
		public static NPC getRandomTownNPC(Random r, int offset)
		{
			Dictionary<string, string> giftTastes = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
			int index = r.Next(giftTastes.Count);
			NPC i = Game1.getCharacterFromName(giftTastes.ElementAt(index).Key, false);
			while (giftTastes.ElementAt(index).Key.Equals("Wizard") || giftTastes.ElementAt(index).Key.Equals("Krobus") || giftTastes.ElementAt(index).Key.Equals("Sandy") || giftTastes.ElementAt(index).Key.Equals("Dwarf") || giftTastes.ElementAt(index).Key.Equals("Marlon") || i == null)
			{
				index = r.Next(giftTastes.Count);
				i = Game1.getCharacterFromName(giftTastes.ElementAt(index).Key, false);
			}
			return i;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x000B63DC File Offset: 0x000B45DC
		public static bool foundAllStardrops()
		{
			return Game1.player.hasOrWillReceiveMail("CF_Fair") && Game1.player.hasOrWillReceiveMail("CF_Fish") && Game1.player.hasOrWillReceiveMail("CF_Mines") && Game1.player.hasOrWillReceiveMail("CF_Sewer") && Game1.player.hasOrWillReceiveMail("museumComplete") && Game1.player.hasOrWillReceiveMail("CF_Spouse") && Game1.player.hasOrWillReceiveMail("CF_Statue");
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000B6460 File Offset: 0x000B4660
		public static int getGrandpaScore()
		{
			int points = 0;
			if (Game1.player.totalMoneyEarned >= 50000u)
			{
				points++;
			}
			if (Game1.player.totalMoneyEarned >= 100000u)
			{
				points++;
			}
			if (Game1.player.totalMoneyEarned >= 200000u)
			{
				points++;
			}
			if (Game1.player.totalMoneyEarned >= 300000u)
			{
				points++;
			}
			if (Game1.player.totalMoneyEarned >= 500000u)
			{
				points++;
			}
			if (Game1.player.totalMoneyEarned >= 1000000u)
			{
				points += 2;
			}
			if (Game1.player.achievements.Contains(5))
			{
				points++;
			}
			if (Game1.player.hasSkullKey)
			{
				points++;
			}
			if (Game1.isLocationAccessible("CommunityCenter") || Game1.player.hasCompletedCommunityCenter())
			{
				points++;
			}
			if (Game1.isLocationAccessible("CommunityCenter"))
			{
				points += 2;
			}
			if (Game1.player.isMarried() && Utility.getHomeOfFarmer(Game1.player).upgradeLevel >= 2)
			{
				points++;
			}
			if (Game1.player.hasRustyKey)
			{
				points++;
			}
			if (Game1.player.achievements.Contains(26))
			{
				points++;
			}
			if (Game1.player.achievements.Contains(34))
			{
				points++;
			}
			if (Utility.getNumberOfFriendsWithinThisRange(Game1.player, 1975, 999999, false) >= 5)
			{
				points++;
			}
			if (Utility.getNumberOfFriendsWithinThisRange(Game1.player, 1975, 999999, false) >= 10)
			{
				points++;
			}
			if (Game1.player.Level >= 15)
			{
				points++;
			}
			if (Game1.player.Level >= 25)
			{
				points++;
			}
			if (Game1.player.getPetName() != null && Game1.getCharacterFromName(Game1.player.getPetName(), false) != null && Game1.getCharacterFromName(Game1.player.getPetName(), false) is Pet && (Game1.getCharacterFromName(Game1.player.getPetName(), false) as Pet).friendshipTowardFarmer >= 999)
			{
				points++;
			}
			return points;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000B6657 File Offset: 0x000B4857
		public static int getGrandpaCandlesFromScore(int score)
		{
			if (score >= 12)
			{
				return 4;
			}
			if (score >= 8)
			{
				return 3;
			}
			if (score >= 4)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000B6670 File Offset: 0x000B4870
		public static bool canItemBeAddedToThisInventoryList(Item i, List<Item> list, int listMaxSpace = -1)
		{
			if (listMaxSpace != -1 && list.Count < listMaxSpace)
			{
				return true;
			}
			int stack = i.Stack;
			foreach (Item it in list)
			{
				if (it == null)
				{
					bool result = true;
					return result;
				}
				if (it.canStackWith(i) && it.getRemainingStackSpace() > 0)
				{
					stack -= it.getRemainingStackSpace();
					if (stack <= 0)
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000B66FC File Offset: 0x000B48FC
		public static Item addItemToThisInventoryList(Item i, List<Item> list, int listMaxSpace = -1)
		{
			if (i.Stack == 0)
			{
				i.Stack = 1;
			}
			foreach (Item it in list)
			{
				if (it != null && it.canStackWith(i) && it.getRemainingStackSpace() > 0)
				{
					i.Stack = it.addToStack(i.Stack);
					if (i.Stack <= 0)
					{
						return null;
					}
				}
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (list[j] == null)
				{
					if (i.Stack <= i.maximumStackSize())
					{
						list[j] = i;
						return null;
					}
					list[j] = i.getOne();
					list[j].Stack = i.maximumStackSize();
					i.Stack -= i.maximumStackSize();
				}
			}
			while (listMaxSpace != -1 && list.Count < listMaxSpace)
			{
				if (i.Stack <= i.maximumStackSize())
				{
					list.Add(i);
					return null;
				}
				Item tmp = i.getOne();
				tmp.Stack = i.maximumStackSize();
				i.Stack -= i.maximumStackSize();
				list.Add(tmp);
			}
			return i;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000B684C File Offset: 0x000B4A4C
		public static Item addItemToInventory(Item item, int position, List<Item> items, ItemGrabMenu.behaviorOnItemSelect onAddFunction = null)
		{
			if (items.Equals(Game1.player.items) && item is Object && (item as Object).specialItem)
			{
				if ((item as Object).bigCraftable)
				{
					if (!Game1.player.specialBigCraftables.Contains((item as Object).isRecipe ? (-(item as Object).parentSheetIndex) : (item as Object).parentSheetIndex))
					{
						Game1.player.specialBigCraftables.Add((item as Object).isRecipe ? (-(item as Object).parentSheetIndex) : (item as Object).parentSheetIndex);
					}
				}
				else if (!Game1.player.specialItems.Contains((item as Object).isRecipe ? (-(item as Object).parentSheetIndex) : (item as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Add((item as Object).isRecipe ? (-(item as Object).parentSheetIndex) : (item as Object).parentSheetIndex);
				}
			}
			if (position < 0 || position >= items.Count)
			{
				return item;
			}
			if (items[position] == null)
			{
				items[position] = item;
				if (onAddFunction != null)
				{
					onAddFunction(item, null);
				}
				return null;
			}
			if (items[position].maximumStackSize() == -1 || !items[position].Name.Equals(item.Name) || (item is Object && items[position] is Object && ((item as Object).quality != (items[position] as Object).quality || (item as Object).parentSheetIndex != (items[position] as Object).parentSheetIndex)) || !item.canStackWith(items[position]))
			{
				Item tmp = items[position];
				if (position == Game1.player.CurrentToolIndex && items.Equals(Game1.player.items) && tmp != null)
				{
					tmp.actionWhenStopBeingHeld(Game1.player);
					item.actionWhenBeingHeld(Game1.player);
				}
				items[position] = item;
				if (onAddFunction != null)
				{
					onAddFunction(item, null);
				}
				return tmp;
			}
			int stackLeft = items[position].addToStack(item.getStack());
			if (stackLeft <= 0)
			{
				return null;
			}
			item.Stack = stackLeft;
			if (onAddFunction != null)
			{
				onAddFunction(item, null);
			}
			return item;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000B6AB8 File Offset: 0x000B4CB8
		public static void spawnObjectAround(Vector2 tileLocation, Object o, GameLocation l)
		{
			if (o == null || l == null || tileLocation.Equals(Vector2.Zero))
			{
				return;
			}
			int attempts = 0;
			Queue<Vector2> openList = new Queue<Vector2>();
			HashSet<Vector2> closedList = new HashSet<Vector2>();
			openList.Enqueue(tileLocation);
			Vector2 current = Vector2.Zero;
			while (attempts < 100)
			{
				current = openList.Dequeue();
				if (!l.isTileOccupiedForPlacement(current, null) && !l.isOpenWater((int)current.X, (int)current.Y))
				{
					break;
				}
				closedList.Add(current);
				foreach (Vector2 v in Utility.getAdjacentTileLocations(current))
				{
					if (!closedList.Contains(v))
					{
						openList.Enqueue(v);
					}
				}
				attempts++;
			}
			o.isSpawnedObject = true;
			o.canBeGrabbed = true;
			o.tileLocation = current;
			if (!current.Equals(Vector2.Zero))
			{
				l.objects.Add(current, o);
				if (l.Equals(Game1.currentLocation))
				{
					Game1.playSound("coin");
					l.temporarySprites.Add(new TemporaryAnimatedSprite(5, current * (float)Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
				}
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000B6BFC File Offset: 0x000B4DFC
		public static Object getTreasureFromGeode(Item geode)
		{
			try
			{
				Random r = new Random((int)(Game1.stats.GeodesCracked + (uint)((int)Game1.uniqueIDForThisGame / 2)));
				int whichGeode = (geode as Object).parentSheetIndex;
				if (r.NextDouble() < 0.5)
				{
					int amount = r.Next(3) * 2 + 1;
					if (r.NextDouble() < 0.1)
					{
						amount = 10;
					}
					if (r.NextDouble() < 0.01)
					{
						amount = 20;
					}
					if (r.NextDouble() < 0.5)
					{
						switch (r.Next(4))
						{
						case 0:
						case 1:
						{
							Object result = new Object(390, amount, false, -1, 0);
							return result;
						}
						case 2:
						{
							Object result = new Object(330, 1, false, -1, 0);
							return result;
						}
						case 3:
						{
							Object result = new Object((whichGeode == 535) ? 86 : ((whichGeode == 536) ? 84 : 82), 1, false, -1, 0);
							return result;
						}
						}
					}
					else if (whichGeode == 535)
					{
						switch (r.Next(3))
						{
						case 0:
						{
							Object result = new Object(378, amount, false, -1, 0);
							return result;
						}
						case 1:
						{
							Object result = new Object((Game1.player.deepestMineLevel > 25) ? 380 : 378, amount, false, -1, 0);
							return result;
						}
						case 2:
						{
							Object result = new Object(382, amount, false, -1, 0);
							return result;
						}
						}
					}
					else if (whichGeode == 536)
					{
						switch (r.Next(4))
						{
						case 0:
						{
							Object result = new Object(378, amount, false, -1, 0);
							return result;
						}
						case 1:
						{
							Object result = new Object(380, amount, false, -1, 0);
							return result;
						}
						case 2:
						{
							Object result = new Object(382, amount, false, -1, 0);
							return result;
						}
						case 3:
						{
							Object result = new Object((Game1.player.deepestMineLevel > 75) ? 384 : 380, amount, false, -1, 0);
							return result;
						}
						}
					}
					else
					{
						switch (r.Next(5))
						{
						case 0:
						{
							Object result = new Object(378, amount, false, -1, 0);
							return result;
						}
						case 1:
						{
							Object result = new Object(380, amount, false, -1, 0);
							return result;
						}
						case 2:
						{
							Object result = new Object(382, amount, false, -1, 0);
							return result;
						}
						case 3:
						{
							Object result = new Object(384, amount, false, -1, 0);
							return result;
						}
						case 4:
						{
							Object result = new Object(386, amount / 2 + 1, false, -1, 0);
							return result;
						}
						}
					}
				}
				else
				{
					string[] treasures = Game1.objectInformation[whichGeode].Split(new char[]
					{
						'/'
					})[5].Split(new char[]
					{
						' '
					});
					int index = Convert.ToInt32(treasures[r.Next(treasures.Length)]);
					Object result;
					if (whichGeode == 749 && r.NextDouble() < 0.008 && Game1.stats.GeodesCracked > 15u)
					{
						result = new Object(74, 1, false, -1, 0);
						return result;
					}
					result = new Object(index, 1, false, -1, 0);
					return result;
				}
			}
			catch (Exception)
			{
			}
			return new Object(Vector2.Zero, 390, 1);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000B6F60 File Offset: 0x000B5160
		public static Vector2 snapToInt(Vector2 v)
		{
			v.X = (float)((int)v.X);
			v.Y = (float)((int)v.Y);
			return v;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000B6F84 File Offset: 0x000B5184
		public static void tryToPlaceItem(GameLocation location, Item item, int x, int y)
		{
			if (item is Tool)
			{
				return;
			}
			new Vector2((float)(x / Game1.tileSize), (float)(y / Game1.tileSize));
			if (Utility.playerCanPlaceItemHere(location, item, x, y, Game1.player))
			{
				if (((Object)item).placementAction(location, x, y, Game1.player))
				{
					if (Game1.IsClient)
					{
						Game1.client.sendMessage(4, new object[]
						{
							(short)x,
							(short)y,
							0,
							((Object)item).bigCraftable ? 1 : 0,
							((Object)item).ParentSheetIndex
						});
					}
					Game1.player.reduceActiveItemByOne();
					return;
				}
			}
			else
			{
				Utility.withinRadiusOfPlayer(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 3, Game1.player);
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000B7074 File Offset: 0x000B5274
		public static int showLanternBar()
		{
			foreach (Item t in Game1.player.Items)
			{
				if (t != null && t.GetType() == typeof(Lantern) && ((Lantern)t).on)
				{
					return ((Lantern)t).fuelLeft;
				}
			}
			return -1;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000B70FC File Offset: 0x000B52FC
		public static void plantCrops(GameLocation farm, int seedType, int x, int y, int width, int height, int daysOld)
		{
			for (int i = x; i < x + width; i++)
			{
				for (int j = y; j < y + height; j++)
				{
					Vector2 v = new Vector2((float)i, (float)j);
					farm.makeHoeDirt(v);
					if (farm.terrainFeatures.ContainsKey(v) && farm.terrainFeatures[v].GetType() == typeof(HoeDirt))
					{
						((HoeDirt)farm.terrainFeatures[v]).crop = new Crop(seedType, x, y);
					}
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000B7188 File Offset: 0x000B5388
		public static List<Vector2> getDirectionsTileVectors()
		{
			return new List<Vector2>
			{
				new Vector2(0f, -1f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(-1f, 0f)
			};
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000B71F0 File Offset: 0x000B53F0
		public static bool pointInRectangles(List<Microsoft.Xna.Framework.Rectangle> rectangles, int x, int y)
		{
			foreach (Microsoft.Xna.Framework.Rectangle r in rectangles)
			{
				if (r.Contains(x, y))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000B724C File Offset: 0x000B544C
		public static Keys mapGamePadButtonToKey(Buttons b)
		{
			if (b == Buttons.A)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.actionButton);
			}
			if (b == Buttons.X)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.useToolButton);
			}
			if (b == Buttons.B)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.menuButton);
			}
			if (b == Buttons.Back)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.journalButton);
			}
			if (b == Buttons.Start)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.menuButton);
			}
			if (b == Buttons.Y)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.menuButton);
			}
			if (b == Buttons.RightTrigger)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.toolSwapButton);
			}
			if (b == Buttons.LeftTrigger)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.toolSwapButton);
			}
			if (b == Buttons.RightShoulder)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.actionButton);
			}
			if (b == Buttons.LeftShoulder)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.actionButton);
			}
			if (b == Buttons.DPadUp)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveUpButton);
			}
			if (b == Buttons.DPadRight)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveRightButton);
			}
			if (b == Buttons.DPadDown)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveDownButton);
			}
			if (b == Buttons.DPadLeft)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveLeftButton);
			}
			if (b == Buttons.LeftThumbstickUp)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveUpButton);
			}
			if (b == Buttons.LeftThumbstickRight)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveRightButton);
			}
			if (b == Buttons.LeftThumbstickDown)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveDownButton);
			}
			if (b == Buttons.LeftThumbstickLeft)
			{
				return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveLeftButton);
			}
			return Keys.None;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000B7450 File Offset: 0x000B5650
		public static List<Buttons> getPressedButtons(GamePadState padState, GamePadState oldPadState)
		{
			List<Buttons> pressed = new List<Buttons>();
			if (padState.IsButtonDown(Buttons.A) && !oldPadState.IsButtonDown(Buttons.A))
			{
				pressed.Add(Buttons.A);
			}
			if (padState.IsButtonDown(Buttons.B) && !oldPadState.IsButtonDown(Buttons.B))
			{
				pressed.Add(Buttons.B);
			}
			if (padState.IsButtonDown(Buttons.X) && !oldPadState.IsButtonDown(Buttons.X))
			{
				pressed.Add(Buttons.X);
			}
			if (padState.IsButtonDown(Buttons.Y) && !oldPadState.IsButtonDown(Buttons.Y))
			{
				pressed.Add(Buttons.Y);
			}
			if (padState.IsButtonDown(Buttons.Start) && !oldPadState.IsButtonDown(Buttons.Start))
			{
				pressed.Add(Buttons.Start);
			}
			if (padState.IsButtonDown(Buttons.Back) && !oldPadState.IsButtonDown(Buttons.Back))
			{
				pressed.Add(Buttons.Back);
			}
			if (padState.IsButtonDown(Buttons.RightTrigger) && !oldPadState.IsButtonDown(Buttons.RightTrigger))
			{
				pressed.Add(Buttons.RightTrigger);
			}
			if (padState.IsButtonDown(Buttons.LeftTrigger) && !oldPadState.IsButtonDown(Buttons.LeftTrigger))
			{
				pressed.Add(Buttons.LeftTrigger);
			}
			if (padState.IsButtonDown(Buttons.RightShoulder) && !oldPadState.IsButtonDown(Buttons.RightShoulder))
			{
				pressed.Add(Buttons.RightShoulder);
			}
			if (padState.IsButtonDown(Buttons.LeftShoulder) && !oldPadState.IsButtonDown(Buttons.LeftShoulder))
			{
				pressed.Add(Buttons.LeftShoulder);
			}
			if (padState.IsButtonDown(Buttons.DPadUp) && !oldPadState.IsButtonDown(Buttons.DPadUp))
			{
				pressed.Add(Buttons.DPadUp);
			}
			if (padState.IsButtonDown(Buttons.DPadRight) && !oldPadState.IsButtonDown(Buttons.DPadRight))
			{
				pressed.Add(Buttons.DPadRight);
			}
			if (padState.IsButtonDown(Buttons.DPadDown) && !oldPadState.IsButtonDown(Buttons.DPadDown))
			{
				pressed.Add(Buttons.DPadDown);
			}
			if (padState.IsButtonDown(Buttons.DPadLeft) && !oldPadState.IsButtonDown(Buttons.DPadLeft))
			{
				pressed.Add(Buttons.DPadLeft);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickUp) && !oldPadState.IsButtonDown(Buttons.LeftThumbstickUp))
			{
				pressed.Add(Buttons.LeftThumbstickUp);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickRight) && !oldPadState.IsButtonDown(Buttons.LeftThumbstickRight))
			{
				pressed.Add(Buttons.LeftThumbstickRight);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickDown) && !oldPadState.IsButtonDown(Buttons.LeftThumbstickDown))
			{
				pressed.Add(Buttons.LeftThumbstickDown);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickLeft) && !oldPadState.IsButtonDown(Buttons.LeftThumbstickLeft))
			{
				pressed.Add(Buttons.LeftThumbstickLeft);
			}
			return pressed;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000B76E0 File Offset: 0x000B58E0
		public static List<Buttons> getHeldButtons(GamePadState padState)
		{
			List<Buttons> pressed = new List<Buttons>();
			if (padState.IsButtonDown(Buttons.A))
			{
				pressed.Add(Buttons.A);
			}
			if (padState.IsButtonDown(Buttons.B))
			{
				pressed.Add(Buttons.B);
			}
			if (padState.IsButtonDown(Buttons.X))
			{
				pressed.Add(Buttons.X);
			}
			if (padState.IsButtonDown(Buttons.Y))
			{
				pressed.Add(Buttons.Y);
			}
			if (padState.IsButtonDown(Buttons.Start))
			{
				pressed.Add(Buttons.Start);
			}
			if (padState.IsButtonDown(Buttons.Back))
			{
				pressed.Add(Buttons.Back);
			}
			if (padState.IsButtonDown(Buttons.RightTrigger))
			{
				pressed.Add(Buttons.RightTrigger);
			}
			if (padState.IsButtonDown(Buttons.LeftTrigger))
			{
				pressed.Add(Buttons.LeftTrigger);
			}
			if (padState.IsButtonDown(Buttons.RightShoulder))
			{
				pressed.Add(Buttons.RightShoulder);
			}
			if (padState.IsButtonDown(Buttons.LeftShoulder))
			{
				pressed.Add(Buttons.LeftShoulder);
			}
			if (padState.IsButtonDown(Buttons.DPadUp))
			{
				pressed.Add(Buttons.DPadUp);
			}
			if (padState.IsButtonDown(Buttons.DPadRight))
			{
				pressed.Add(Buttons.DPadRight);
			}
			if (padState.IsButtonDown(Buttons.DPadDown))
			{
				pressed.Add(Buttons.DPadDown);
			}
			if (padState.IsButtonDown(Buttons.DPadLeft))
			{
				pressed.Add(Buttons.DPadLeft);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickUp))
			{
				pressed.Add(Buttons.LeftThumbstickUp);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickRight))
			{
				pressed.Add(Buttons.LeftThumbstickRight);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickDown))
			{
				pressed.Add(Buttons.LeftThumbstickDown);
			}
			if (padState.IsButtonDown(Buttons.LeftThumbstickLeft))
			{
				pressed.Add(Buttons.LeftThumbstickLeft);
			}
			return pressed;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000B788C File Offset: 0x000B5A8C
		public static bool toggleMuteMusic()
		{
			if (Game1.soundBank != null)
			{
				if (Game1.options.musicVolumeLevel != 0f)
				{
					Game1.options.musicVolumeLevel = 0f;
					Game1.musicCategory.SetVolume(0f);
					Game1.options.ambientVolumeLevel = 0f;
					Game1.ambientCategory.SetVolume(0f);
					Game1.ambientPlayerVolume = 0f;
					Game1.musicPlayerVolume = 0f;
					return true;
				}
				Game1.options.musicVolumeLevel = 0.75f;
				Game1.musicCategory.SetVolume(0.75f);
				Game1.musicPlayerVolume = 0.75f;
				Game1.options.ambientVolumeLevel = 0.75f;
				Game1.ambientCategory.SetVolume(0.75f);
				Game1.ambientPlayerVolume = 0.75f;
			}
			return false;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x000B7957 File Offset: 0x000B5B57
		public static Vector2 getVelocityTowardPlayer(Point startingPoint, float speed, Farmer f)
		{
			return Utility.getVelocityTowardPoint(startingPoint, new Vector2((float)f.GetBoundingBox().X, (float)f.GetBoundingBox().Y), speed);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x000B7980 File Offset: 0x000B5B80
		public static string getHoursMinutesStringFromMilliseconds(uint milliseconds)
		{
			return string.Concat(new object[]
			{
				milliseconds / 3600000u,
				":",
				(milliseconds % 3600000u / 60000u < 10u) ? "0" : "",
				milliseconds % 3600000u / 60000u
			});
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x000B79E4 File Offset: 0x000B5BE4
		public static string getMinutesSecondsStringFromMilliseconds(int milliseconds)
		{
			return string.Concat(new object[]
			{
				milliseconds / 60000,
				":",
				(milliseconds % 60000 / 1000 < 10) ? "0" : "",
				milliseconds % 60000 / 1000
			});
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000B7A48 File Offset: 0x000B5C48
		public static Vector2 getVelocityTowardPoint(Vector2 startingPoint, Vector2 endingPoint, float speed)
		{
			double arg_1D_0 = (double)(endingPoint.X - startingPoint.X);
			double yDif = (double)(endingPoint.Y - startingPoint.Y);
			double total = Math.Sqrt(Math.Pow(arg_1D_0, 2.0) + Math.Pow(yDif, 2.0));
			float arg_4A_0 = (float)(arg_1D_0 / total);
			yDif /= total;
			return new Vector2((float)((double)arg_4A_0 * (double)speed), (float)(yDif * (double)speed));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000B7AAC File Offset: 0x000B5CAC
		public static Vector2 getVelocityTowardPoint(Point startingPoint, Vector2 endingPoint, float speed)
		{
			double arg_1F_0 = (double)(endingPoint.X - (float)startingPoint.X);
			double yDif = (double)(endingPoint.Y - (float)startingPoint.Y);
			double total = Math.Sqrt(Math.Pow(arg_1F_0, 2.0) + Math.Pow(yDif, 2.0));
			float arg_4C_0 = (float)(arg_1F_0 / total);
			yDif /= total;
			return new Vector2((float)((double)arg_4C_0 * (double)speed), (float)(yDif * (double)speed));
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x000B7B11 File Offset: 0x000B5D11
		public static Vector2 getRandomPositionInThisRectangle(Microsoft.Xna.Framework.Rectangle r, Random random)
		{
			return new Vector2((float)random.Next(r.X, r.X + r.Width), (float)random.Next(r.Y, r.Y + r.Height));
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000B7B4C File Offset: 0x000B5D4C
		public static Vector2 getTopLeftPositionForCenteringOnScreen(int width, int height, int xOffset = 0, int yOffset = 0)
		{
			return new Vector2((float)(Game1.viewport.Width / 2 - width / 2 + xOffset), (float)(Game1.viewport.Height / 2 - height / 2 + yOffset));
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000B7B7C File Offset: 0x000B5D7C
		public static void recursiveFindPositionForCharacter(NPC c, GameLocation l, Vector2 tileLocation, int maxIterations)
		{
			List<Vector2> directions = Utility.getDirectionsTileVectors();
			int iterations = 0;
			Queue<Vector2> positionsToCheck = new Queue<Vector2>();
			positionsToCheck.Enqueue(tileLocation);
			List<Vector2> closedList = new List<Vector2>();
			while (iterations < maxIterations && positionsToCheck.Count > 0)
			{
				Vector2 currentPoint = positionsToCheck.Dequeue();
				closedList.Add(currentPoint);
				c.position = new Vector2(currentPoint.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(c.GetBoundingBox().Width / 2), currentPoint.Y * (float)Game1.tileSize - (float)c.GetBoundingBox().Height);
				if (!l.isCollidingPosition(c.GetBoundingBox(), Game1.viewport, false, 0, false, c, true, false, false))
				{
					if (!l.characters.Contains(c))
					{
						l.characters.Add(c);
					}
					return;
				}
				foreach (Vector2 v in directions)
				{
					if (!closedList.Contains(currentPoint + v))
					{
						positionsToCheck.Enqueue(currentPoint + v);
					}
				}
				iterations++;
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000B7CAC File Offset: 0x000B5EAC
		public static Vector2 recursiveFindOpenTileForCharacter(Character c, GameLocation l, Vector2 tileLocation, int maxIterations)
		{
			List<Vector2> directions = Utility.getDirectionsTileVectors();
			int iterations = 0;
			Queue<Vector2> positionsToCheck = new Queue<Vector2>();
			positionsToCheck.Enqueue(tileLocation);
			List<Vector2> closedList = new List<Vector2>();
			Vector2 originalPosition = c.position;
			while (iterations < maxIterations && positionsToCheck.Count > 0)
			{
				Vector2 currentPoint = positionsToCheck.Dequeue();
				closedList.Add(currentPoint);
				c.position = new Vector2(currentPoint.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(c.GetBoundingBox().Width / 2), currentPoint.Y * (float)Game1.tileSize + (float)Game1.pixelZoom);
				if (!l.isCollidingPosition(c.GetBoundingBox(), Game1.viewport, c is Farmer, 0, false, c, true, false, false))
				{
					c.position = originalPosition;
					return currentPoint;
				}
				foreach (Vector2 v in directions)
				{
					if (!closedList.Contains(currentPoint + v))
					{
						positionsToCheck.Enqueue(currentPoint + v);
					}
				}
				iterations++;
			}
			c.position = originalPosition;
			return Vector2.Zero;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000B7DE4 File Offset: 0x000B5FE4
		public static List<Vector2> recursiveFindOpenTiles(GameLocation l, Vector2 tileLocation, int maxOpenTilesToFind = 24, int maxIterations = 50)
		{
			List<Vector2> directions = Utility.getDirectionsTileVectors();
			int iterations = 0;
			Queue<Vector2> positionsToCheck = new Queue<Vector2>();
			positionsToCheck.Enqueue(tileLocation);
			List<Vector2> closedList = new List<Vector2>();
			List<Vector2> successList = new List<Vector2>();
			while (iterations < maxIterations && positionsToCheck.Count > 0 && successList.Count < maxOpenTilesToFind)
			{
				Vector2 currentPoint = positionsToCheck.Dequeue();
				closedList.Add(currentPoint);
				if (l.isTileLocationTotallyClearAndPlaceable(currentPoint))
				{
					successList.Add(currentPoint);
				}
				foreach (Vector2 v in directions)
				{
					if (!closedList.Contains(currentPoint + v))
					{
						positionsToCheck.Enqueue(currentPoint + v);
					}
				}
				iterations++;
			}
			return successList;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000B7EB4 File Offset: 0x000B60B4
		public static void spreadAnimalsAround(Building b, Farm environment)
		{
			try
			{
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000B7ED8 File Offset: 0x000B60D8
		public static void spreadAnimalsAround(Building b, Farm environment, List<FarmAnimal> animalsList)
		{
			if (b.indoors != null && b.indoors.GetType() == typeof(AnimalHouse))
			{
				List<Vector2> directions = Utility.getDirectionsTileVectors();
				Queue<FarmAnimal> animals = new Queue<FarmAnimal>(animalsList);
				int iterations = 0;
				Queue<Vector2> positionsToCheck = new Queue<Vector2>();
				positionsToCheck.Enqueue(new Vector2((float)(b.tileX + b.animalDoor.X), (float)(b.tileY + b.animalDoor.Y + 1)));
				while (animals.Count > 0 && iterations < 40 && positionsToCheck.Count > 0)
				{
					Vector2 currentPoint = positionsToCheck.Dequeue();
					animals.Peek().position = new Vector2(currentPoint.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(animals.Peek().GetBoundingBox().Width / 2), currentPoint.Y * (float)Game1.tileSize - (float)(Game1.tileSize / 2) - (float)(animals.Peek().GetBoundingBox().Height / 2));
					if (!environment.isCollidingPosition(animals.Peek().GetBoundingBox(), Game1.viewport, false, 0, false, animals.Peek(), true, false, false))
					{
						FarmAnimal a = animals.Dequeue();
						environment.animals.Add(a.myID, a);
					}
					if (animals.Count > 0)
					{
						foreach (Vector2 v in directions)
						{
							animals.Peek().position = new Vector2((currentPoint.X + v.X) * (float)Game1.tileSize + (float)(Game1.tileSize / 2) - (float)(animals.Peek().GetBoundingBox().Width / 2), (currentPoint.Y + v.Y) * (float)Game1.tileSize - (float)(Game1.tileSize / 2) - (float)(animals.Peek().GetBoundingBox().Height / 2));
							if (!environment.isCollidingPosition(animals.Peek().GetBoundingBox(), Game1.viewport, false, 0, false, animals.Peek(), true, false, false))
							{
								positionsToCheck.Enqueue(currentPoint + v);
							}
						}
					}
					iterations++;
				}
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x000B8120 File Offset: 0x000B6320
		public static bool[] horizontalOrVerticalCollisionDirections(Microsoft.Xna.Framework.Rectangle boundingBox, bool projectile = false)
		{
			return Utility.horizontalOrVerticalCollisionDirections(boundingBox, null, projectile);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000B812C File Offset: 0x000B632C
		public static Point findTile(GameLocation location, int tileIndex, string layer)
		{
			for (int y = 0; y < location.map.GetLayer(layer).LayerHeight; y++)
			{
				for (int x = 0; x < location.map.GetLayer(layer).LayerWidth; x++)
				{
					if (location.getTileIndexAt(x, y, layer) == tileIndex)
					{
						return new Point(x, y);
					}
				}
			}
			return new Point(-1, -1);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000B818C File Offset: 0x000B638C
		public static bool[] horizontalOrVerticalCollisionDirections(Microsoft.Xna.Framework.Rectangle boundingBox, Character c, bool projectile = false)
		{
			bool[] directions = new bool[2];
			Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle(boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height);
			rect.Width = 1;
			rect.X = boundingBox.Center.X;
			if (c != null)
			{
				if (Game1.currentLocation.isCollidingPosition(rect, Game1.viewport, false, -1, projectile, c, false, projectile, false))
				{
					directions[1] = true;
				}
			}
			else if (Game1.currentLocation.isCollidingPosition(rect, Game1.viewport, false, -1, projectile, c, false, projectile, false))
			{
				directions[1] = true;
			}
			rect.Width = boundingBox.Width;
			rect.X = boundingBox.X;
			rect.Height = 1;
			rect.Y = boundingBox.Center.Y;
			if (c != null)
			{
				if (Game1.currentLocation.isCollidingPosition(rect, Game1.viewport, false, -1, projectile, c, false, projectile, false))
				{
					directions[0] = true;
				}
			}
			else if (Game1.currentLocation.isCollidingPosition(rect, Game1.viewport, false, -1, projectile, c, false, projectile, false))
			{
				directions[0] = true;
			}
			return directions;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000B8290 File Offset: 0x000B6490
		public static Color getBlendedColor(Color c1, Color c2)
		{
			return new Color((int)((Game1.random.NextDouble() < 0.5) ? Math.Max(c1.R, c2.R) : ((c1.R + c2.R) / 2)), (int)((Game1.random.NextDouble() < 0.5) ? Math.Max(c1.G, c2.G) : ((c1.G + c2.G) / 2)), (int)((Game1.random.NextDouble() < 0.5) ? Math.Max(c1.B, c2.B) : ((c1.B + c2.B) / 2)));
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x000B8354 File Offset: 0x000B6554
		public static Character checkForCharacterWithinArea(Type kindOfCharacter, Vector2 positionToAvoid, GameLocation location, Microsoft.Xna.Framework.Rectangle area)
		{
			foreach (NPC i in location.characters)
			{
				if (i.GetType().Equals(kindOfCharacter) && i.GetBoundingBox().Intersects(area) && !i.position.Equals(positionToAvoid))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x000B83D4 File Offset: 0x000B65D4
		public static int getNumberOfCharactersInRadius(GameLocation l, Point position, int tileRadius)
		{
			Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle(position.X - tileRadius * Game1.tileSize, position.Y - tileRadius * Game1.tileSize, (tileRadius * 2 + 1) * Game1.tileSize, (tileRadius * 2 + 1) * Game1.tileSize);
			int count = 0;
			foreach (NPC i in l.characters)
			{
				if (rect.Contains(Utility.Vector2ToPoint(i.position)))
				{
					count++;
				}
			}
			return count;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000B8474 File Offset: 0x000B6674
		public static List<Vector2> getListOfTileLocationsForBordersOfNonTileRectangle(Microsoft.Xna.Framework.Rectangle rectangle)
		{
			return new List<Vector2>
			{
				new Vector2((float)(rectangle.Left / Game1.tileSize), (float)(rectangle.Top / Game1.tileSize)),
				new Vector2((float)(rectangle.Right / Game1.tileSize), (float)(rectangle.Top / Game1.tileSize)),
				new Vector2((float)(rectangle.Left / Game1.tileSize), (float)(rectangle.Bottom / Game1.tileSize)),
				new Vector2((float)(rectangle.Right / Game1.tileSize), (float)(rectangle.Bottom / Game1.tileSize)),
				new Vector2((float)(rectangle.Left / Game1.tileSize), (float)(rectangle.Center.Y / Game1.tileSize)),
				new Vector2((float)(rectangle.Right / Game1.tileSize), (float)(rectangle.Center.Y / Game1.tileSize)),
				new Vector2((float)(rectangle.Center.X / Game1.tileSize), (float)(rectangle.Bottom / Game1.tileSize)),
				new Vector2((float)(rectangle.Center.X / Game1.tileSize), (float)(rectangle.Top / Game1.tileSize)),
				new Vector2((float)(rectangle.Center.X / Game1.tileSize), (float)(rectangle.Center.Y / Game1.tileSize))
			};
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x000B8604 File Offset: 0x000B6804
		public static void makeTemporarySpriteJuicier(TemporaryAnimatedSprite t, GameLocation l, int numAddOns = 4, int xRange = 64, int yRange = 64)
		{
			t.position.Y = t.position.Y - (float)(Game1.pixelZoom * 2);
			l.temporarySprites.Add(t);
			for (int i = 0; i < numAddOns; i++)
			{
				TemporaryAnimatedSprite clone = t.getClone();
				clone.delayBeforeAnimationStart = i * 100;
				clone.position += new Vector2((float)Game1.random.Next(-xRange / 2, xRange / 2 + 1), (float)Game1.random.Next(-yRange / 2, yRange / 2 + 1));
				l.temporarySprites.Add(clone);
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x000B86A0 File Offset: 0x000B68A0
		public static void recursiveObjectPlacement(Object o, int tileX, int tileY, double growthRate, double decay, GameLocation location, string terrainToExclude = "", int objectIndexAddRange = 0, double failChance = 0.0, int objectIndeAddRangeMultiplier = 1)
		{
			if (location.isTileLocationOpen(new Location(tileX * Game1.tileSize, tileY * Game1.tileSize)) && !location.isTileOccupied(new Vector2((float)tileX, (float)tileY), "") && location.getTileIndexAt(tileX, tileY, "Back") != -1 && (terrainToExclude.Equals("") || (location.doesTileHaveProperty(tileX, tileY, "Type", "Back") != null && !location.doesTileHaveProperty(tileX, tileY, "Type", "Back").Equals(terrainToExclude))))
			{
				Vector2 objectPos = new Vector2((float)tileX, (float)tileY);
				if (Game1.random.NextDouble() > failChance * 2.0)
				{
					location.objects.Add(objectPos, new Object(objectPos, o.parentSheetIndex + Game1.random.Next(objectIndexAddRange + 1) * objectIndeAddRangeMultiplier, o.name, o.canBeSetDown, o.canBeGrabbed, o.isHoedirt, o.isSpawnedObject)
					{
						fragility = o.fragility,
						minutesUntilReady = o.minutesUntilReady
					});
				}
				growthRate -= decay;
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveObjectPlacement(o, tileX + 1, tileY, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveObjectPlacement(o, tileX - 1, tileY, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveObjectPlacement(o, tileX, tileY + 1, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveObjectPlacement(o, tileX, tileY - 1, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
				}
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x000B885C File Offset: 0x000B6A5C
		public static void recursiveFarmGrassPlacement(int tileX, int tileY, double growthRate, double decay, GameLocation farm)
		{
			if (farm.isTileLocationOpen(new Location(tileX * Game1.tileSize, tileY * Game1.tileSize)) && !farm.isTileOccupied(new Vector2((float)tileX, (float)tileY), "") && farm.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null)
			{
				Vector2 objectPos = new Vector2((float)tileX, (float)tileY);
				if (Game1.random.NextDouble() < 0.05)
				{
					farm.objects.Add(new Vector2((float)tileX, (float)tileY), new Object(new Vector2((float)tileX, (float)tileY), (Game1.random.NextDouble() < 0.5) ? 674 : 675, 1));
				}
				else
				{
					farm.terrainFeatures.Add(objectPos, new Grass(1, 4 - (int)((1.0 - growthRate) * 4.0)));
				}
				growthRate -= decay;
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveFarmGrassPlacement(tileX + 1, tileY, growthRate, decay, farm);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveFarmGrassPlacement(tileX - 1, tileY, growthRate, decay, farm);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveFarmGrassPlacement(tileX, tileY + 1, growthRate, decay, farm);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveFarmGrassPlacement(tileX, tileY - 1, growthRate, decay, farm);
				}
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x000B89BC File Offset: 0x000B6BBC
		public static void recursiveTreePlacement(int tileX, int tileY, double growthRate, int growthStage, double skipChance, GameLocation l, Microsoft.Xna.Framework.Rectangle clearPatch, bool sparse)
		{
			if (clearPatch.Contains(tileX, tileY))
			{
				return;
			}
			Vector2 location = new Vector2((float)tileX, (float)tileY);
			if (l.doesTileHaveProperty((int)location.X, (int)location.Y, "Diggable", "Back") != null && l.doesTileHaveProperty((int)location.X, (int)location.Y, "NoSpawn", "Back") == null && l.isTileLocationOpen(new Location((int)location.X * Game1.tileSize, (int)location.Y * Game1.tileSize)) && !l.isTileOccupied(location, ""))
			{
				if (sparse)
				{
					if (l.isTileOccupied(new Vector2((float)tileX, (float)(tileY + -1)), ""))
					{
						return;
					}
					if (l.isTileOccupied(new Vector2((float)tileX, (float)(tileY + 1)), ""))
					{
						return;
					}
					if (l.isTileOccupied(new Vector2((float)(tileX + 1), (float)tileY), ""))
					{
						return;
					}
					if (l.isTileOccupied(new Vector2((float)(tileX + -1), (float)tileY), ""))
					{
						return;
					}
					if (l.isTileOccupied(new Vector2((float)(tileX + 1), (float)(tileY + 1)), ""))
					{
						return;
					}
				}
				if (Game1.random.NextDouble() > skipChance)
				{
					if (sparse && location.X < 70f && (location.X < 48f || location.Y > 26f) && Game1.random.NextDouble() < 0.07)
					{
						(l as Farm).resourceClumps.Add(new ResourceClump((Game1.random.NextDouble() < 0.5) ? 672 : ((Game1.random.NextDouble() < 0.5) ? 600 : 602), 2, 2, location));
					}
					else
					{
						l.terrainFeatures.Add(location, new Tree(Game1.random.Next(1, 4), (growthStage < 5) ? Game1.random.Next(5) : 5));
					}
					growthRate -= 0.05;
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveTreePlacement(tileX + Game1.random.Next(1, 3), tileY, growthRate, growthStage, skipChance, l, clearPatch, sparse);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveTreePlacement(tileX - Game1.random.Next(1, 3), tileY, growthRate, growthStage, skipChance, l, clearPatch, sparse);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveTreePlacement(tileX, tileY + Game1.random.Next(1, 3), growthRate, growthStage, skipChance, l, clearPatch, sparse);
				}
				if (Game1.random.NextDouble() < growthRate)
				{
					Utility.recursiveTreePlacement(tileX, tileY - Game1.random.Next(1, 3), growthRate, growthStage, skipChance, l, clearPatch, sparse);
				}
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000B8C88 File Offset: 0x000B6E88
		public static void recursiveRemoveTerrainFeatures(int tileX, int tileY, double growthRate, double decay, GameLocation l)
		{
			Vector2 location = new Vector2((float)tileX, (float)tileY);
			l.terrainFeatures.Remove(location);
			growthRate -= decay;
			if (Game1.random.NextDouble() < growthRate)
			{
				Utility.recursiveRemoveTerrainFeatures(tileX + 1, tileY, growthRate, decay, l);
			}
			if (Game1.random.NextDouble() < growthRate)
			{
				Utility.recursiveRemoveTerrainFeatures(tileX - 1, tileY, growthRate, decay, l);
			}
			if (Game1.random.NextDouble() < growthRate)
			{
				Utility.recursiveRemoveTerrainFeatures(tileX, tileY + 1, growthRate, decay, l);
			}
			if (Game1.random.NextDouble() < growthRate)
			{
				Utility.recursiveRemoveTerrainFeatures(tileX, tileY - 1, growthRate, decay, l);
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000B8D1B File Offset: 0x000B6F1B
		public static IEnumerator<int> generateNewFarm(bool skipFarmGeneration)
		{
			return Utility.generateNewFarm(skipFarmGeneration, true);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000B8D24 File Offset: 0x000B6F24
		public static IEnumerator<int> generateNewFarm(bool skipFarmGeneration, bool loadForNewGame)
		{
			Game1.fadeToBlack = false;
			Game1.fadeToBlackAlpha = 1f;
			Game1.debrisWeather.Clear();
			Game1.viewport.X = -9999;
			Game1.changeMusicTrack("none");
			if (loadForNewGame)
			{
				Game1.loadForNewGame(false);
			}
			if (Game1.IsClient)
			{
				long num = DateTime.Now.Ticks / 10000L;
				while (!Game1.client.isConnected && DateTime.Now.Ticks / 10000L < num + 4000L)
				{
					Thread.Sleep(1);
					Game1.client.receiveMessages();
					yield return 50;
				}
				Game1.loadingMessage = "Requesting Fresh Map";
				yield return 75;
				Game1.receiveNewLocationInfoFromServer(Game1.getLocationFromName("FarmHouse"));
				Game1.loadingMessage = "Received Fresh Map!";
				yield return 99;
			}
			IL_6AB:
			Game1.currentLocation = Game1.getLocationFromName("Farmhouse");
			Game1.currentLocation.currentEvent = new Event("none/-600 -600/farmer 4 8 2/warp farmer 4 8/end beginGame", -1);
			Game1.gameMode = 2;
			yield return 100;
			yield break;
			GameLocation gameLocation = Game1.getLocationFromName("Farm");
			Random random = new Random((int)Game1.uniqueIDForThisGame);
			random.Next(2, 4);
			int num2 = random.Next(150, 250);
			int num3 = random.Next(70, 100);
			int num4 = 30;
			random.Next(Dialogue.adjectives.Length);
			random.Next(Dialogue.nouns.Length);
			float num5 = (float)(num4 + num2 + num3);
			Microsoft.Xna.Framework.Rectangle clearPatch = new Microsoft.Xna.Framework.Rectangle(52, 16, 14, 6);
			Game1.loadingMessage = "Forming Spring Buds...";
			yield return 3;
			int num6 = 0;
			goto IL_1F5;
			IL_186:
			yield return 4 + num6 / 3;
			Utility.recursiveTreePlacement(Game1.random.Next(6, 75), Game1.random.Next(4, 62), 0.4, 5, 0.7, gameLocation, clearPatch, true);
			int num7 = num6;
			num6 = num7 + 1;
			IL_1F5:
			if (num6 < num2 / 10)
			{
				goto IL_186;
			}
			int num8 = 0;
			goto IL_2A5;
			IL_212:
			yield return 4 + num3 / 12 / 3 + num8 / 6;
			Utility.recursiveTreePlacement(Game1.random.Next(6, 75), Game1.random.Next(4, 65), 0.1, (Game1.random.NextDouble() < 0.5) ? 2 : 3, 0.9, gameLocation, clearPatch, true);
			num7 = num8;
			num8 = num7 + 1;
			IL_2A5:
			if (num8 < num3 / 6)
			{
				goto IL_212;
			}
			yield return 48;
			Utility.recursiveTreePlacement(Game1.random.Next(8, 75), Game1.random.Next(8, 65), 0.6, 0, 0.8, gameLocation, clearPatch, true);
			Game1.loadingMessage = "Dehibernating Grass...";
			int num9 = 0;
			goto IL_3F6;
			IL_325:
			Vector2 location = new Vector2((float)random.Next(2, gameLocation.Map.GetLayer("Back").LayerWidth), (float)random.Next(2, gameLocation.Map.GetLayer("Back").LayerHeight));
			Utility.recursiveFarmGrassPlacement((int)location.X, (int)location.Y, 0.89999997615814209, 0.1, gameLocation);
			yield return 2 + (int)((float)(num2 + num3 + num9) / num5 / 2f * 100f);
			num7 = num9;
			num9 = num7 + 1;
			IL_3F6:
			if (num9 < num4)
			{
				goto IL_325;
			}
			gameLocation.spawnWeedsAndStones(500, false, true);
			gameLocation.spawnWeedsAndStones(1000, true, true);
			Game1.loadingMessage = "Spreading Grass...";
			yield return 60;
			yield return 70;
			Game1.loadingMessage = "Sunrising...";
			yield return 80;
			Utility.recursiveRemoveTerrainFeatures(56, 16, 0.8, 0.06, gameLocation);
			Utility.recursiveRemoveTerrainFeatures(71, 16, 0.8, 0.06, gameLocation);
			yield return 90;
			GameLocation expr_4ED = Game1.getLocationFromName("FarmHouse");
			expr_4ED.Objects.Add(new Vector2(1f, 5f), new Object(Vector2.Zero, Game1.random.Next(8), false));
			expr_4ED.Objects.Add(new Vector2(4f, 4f), new Object(Vector2.Zero, 27, false));
			expr_4ED.Objects.Add(new Vector2(5f, 4f), new Object(Vector2.Zero, 22, false));
			expr_4ED.Objects.Add(new Vector2(6f, 4f), new Object(Vector2.Zero, 23, false));
			if (Game1.currentSong != null || Game1.soundBank == null)
			{
				goto IL_5B3;
			}
			Game1.currentSong = Game1.soundBank.GetCue("spring_day_ambient");
			IL_5B3:
			Game1.dayOfMonth = 0;
			gameLocation = null;
			random = null;
			clearPatch = default(Microsoft.Xna.Framework.Rectangle);
			goto IL_6AB;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x000B8D34 File Offset: 0x000B6F34
		public static void lightSourceOptimization(Vector2 tileLocation)
		{
			List<Vector2> closedList = new List<Vector2>();
			Queue<Vector2> openList = new Queue<Vector2>();
			openList.Enqueue(tileLocation);
			while (openList.Count != 0)
			{
				Vector2 currentTorch = openList.Dequeue();
				closedList.Add(currentTorch);
				Vector2[] neighbors = Utility.getAdjacentTileLocationsArray(currentTorch);
				bool surrounded = true;
				for (int i = 0; i < neighbors.Length; i++)
				{
					if (Utility.alreadyHasLightSourceWithThisID((int)(neighbors[i].X * 2000f + neighbors[i].Y)))
					{
						surrounded = false;
						break;
					}
				}
				if (surrounded)
				{
					Utility.removeLightSource((int)(currentTorch.X * 2000f + currentTorch.Y));
				}
				else if (!currentTorch.Equals(tileLocation) && !Utility.alreadyHasLightSourceWithThisID((int)(currentTorch.X * 2000f + currentTorch.Y)))
				{
					Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float)Game1.tileSize + (float)(Game1.tileSize / 2), tileLocation.Y * (float)Game1.tileSize), (Game1.currentLocation.GetType() == typeof(MineShaft)) ? 1.5f : 1.25f, new Color(0, 131, 255) * 0.9f, (int)(currentTorch.X * 2000f + currentTorch.Y)));
				}
				for (int j = 0; j < neighbors.Length; j++)
				{
					if (Game1.currentLocation.Objects.ContainsKey(neighbors[j]) && Game1.currentLocation.Objects[neighbors[j]] is Torch && !closedList.Contains(neighbors[j]))
					{
						openList.Enqueue(neighbors[j]);
					}
				}
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x000B8F00 File Offset: 0x000B7100
		public static bool isOnScreen(Vector2 positionNonTile, int acceptableDistanceFromScreen)
		{
			return positionNonTile.X > (float)(Game1.viewport.X - acceptableDistanceFromScreen) && positionNonTile.X < (float)(Game1.viewport.X + Game1.viewport.Width + acceptableDistanceFromScreen) && positionNonTile.Y > (float)(Game1.viewport.Y - acceptableDistanceFromScreen) && positionNonTile.Y < (float)(Game1.viewport.Y + Game1.viewport.Height + acceptableDistanceFromScreen);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000B8F7C File Offset: 0x000B717C
		public static bool isOnScreen(Point positionTile, int acceptableDistanceFromScreenNonTile, GameLocation location = null)
		{
			return (location == null || location.Equals(Game1.currentLocation)) && (positionTile.X * Game1.tileSize > Game1.viewport.X - acceptableDistanceFromScreenNonTile && positionTile.X * Game1.tileSize < Game1.viewport.X + Game1.viewport.Width + acceptableDistanceFromScreenNonTile && positionTile.Y * Game1.tileSize > Game1.viewport.Y - acceptableDistanceFromScreenNonTile) && positionTile.Y * Game1.tileSize < Game1.viewport.Y + Game1.viewport.Height + acceptableDistanceFromScreenNonTile;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x000B901C File Offset: 0x000B721C
		public static string getExcerptText(Random r)
		{
			string result = "The pages are too faded to read";
			int whichLine = r.Next(138);
			int linesSeen = 0;
			using (StreamReader file = File.OpenText("Content\\Data\\excerpts.txt"))
			{
				while (!file.EndOfStream)
				{
					string line = file.ReadLine();
					if (line.Length > 0 && linesSeen == whichLine)
					{
						result = line;
						break;
					}
					linesSeen++;
				}
			}
			return "..." + result + "...";
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00002834 File Offset: 0x00000A34
		public static void createPotteryTreasure(int tileX, int tileY)
		{
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000B90A0 File Offset: 0x000B72A0
		public static void changeFarmerOverallsColor(Color baseColor)
		{
			int pixelX = 205;
			int pixelIndex = 3198 * Game1.player.Sprite.Texture.Bounds.Width + pixelX;
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex, (int)(baseColor.R - 40), (int)(baseColor.G - 40), (int)(baseColor.B - 40));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 4, (int)baseColor.R, (int)baseColor.G, (int)baseColor.B);
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 8, (int)(baseColor.R + 15), (int)(baseColor.G + 15), (int)(baseColor.B + 15));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 12, (int)(baseColor.R + 20), (int)(baseColor.G + 20), (int)(baseColor.B + 20));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 16, (int)(baseColor.R + 30), (int)(baseColor.G + 30), (int)(baseColor.B + 30));
			Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, pixelIndex + 20, (int)(baseColor.R + 70), (int)(baseColor.G + 70), (int)(baseColor.B + 70));
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000B9268 File Offset: 0x000B7468
		public static void clearObjectsInArea(Microsoft.Xna.Framework.Rectangle r, GameLocation l)
		{
			for (int x = r.Left; x < r.Right; x += Game1.tileSize)
			{
				for (int y = r.Top; y < r.Bottom; y += Game1.tileSize)
				{
					l.removeEverythingFromThisTile(x / Game1.tileSize, y / Game1.tileSize);
				}
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000B92C4 File Offset: 0x000B74C4
		public static void recolorDialogueAndMenu(string theme)
		{
			Color color = Color.White;
			Color color2 = Color.White;
			Color color3 = Color.White;
			Color color4 = Color.White;
			Color color5 = Color.White;
			Color color6 = Color.White;
			Color color7 = Color.White;
			Color color8 = Color.White;
			Color color9 = Color.White;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(theme);
			if (num <= 2465108186u)
			{
				if (num <= 1139731784u)
				{
					if (num != 260554019u)
					{
						if (num != 338638484u)
						{
							if (num == 1139731784u)
							{
								if (theme == "Bombs Away")
								{
									color = new Color(50, 20, 0);
									color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
									color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
									color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
									color5 = Color.Tan;
									color6 = new Color((int)(color4.R + 30), (int)(color4.G + 30), (int)(color4.B + 30));
									color7 = new Color(192, 167, 143);
									color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
									color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
								}
							}
						}
						else if (theme == "Ghosts N' Goblins")
						{
							color = new Color(55, 0, 0);
							color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
							color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
							color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
							color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
							color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
							color7 = new Color(196, 197, 230);
							color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
							color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
						}
					}
					else if (theme == "Polynomial")
					{
						color = new Color(60, 60, 60);
						color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
						color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
						color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
						color6 = new Color(254, 254, 254);
						color5 = new Color((int)(color4.R + 30), (int)(color4.G + 30), (int)(color4.B + 30));
						color7 = new Color(225, 225, 225);
						color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
						color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
					}
				}
				else if (num != 1519897354u)
				{
					if (num != 2098783532u)
					{
						if (num == 2465108186u)
						{
							if (theme == "Sports")
							{
								color = new Color(110, 45, 0);
								color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
								color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
								color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
								color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
								color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
								color7 = new Color(255, 214, 168);
								color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
								color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
							}
						}
					}
					else if (theme == "Wasteland")
					{
						color = new Color(14, 12, 10);
						color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
						color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
						color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
						color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
						color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
						color7 = new Color(185, 178, 165);
						color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
						color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
					}
				}
				else if (theme == "Earthy")
				{
					color = new Color(44, 35, 0);
					color2 = new Color(115, 147, 102);
					color3 = new Color(91, 65, 0);
					color4 = new Color(122, 83, 0);
					color5 = new Color(179, 181, 125);
					color6 = new Color(144, 96, 0);
					color7 = new Color(234, 227, 190);
					color8 = new Color(255, 255, 227);
					color9 = new Color(193, 187, 156);
				}
			}
			else if (num <= 2707891832u)
			{
				if (num != 2594742282u)
				{
					if (num != 2602723858u)
					{
						if (num == 2707891832u)
						{
							if (theme == "Skyscape")
							{
								color = new Color(15, 31, 57);
								color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
								color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
								color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
								color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
								color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
								color7 = new Color(206, 237, 254);
								color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
								color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
							}
						}
					}
					else if (theme == "Sweeties")
					{
						color = new Color(120, 60, 60);
						color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
						color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
						color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
						color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
						color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
						color7 = new Color(255, 213, 227);
						color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
						color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
					}
				}
				else if (theme == "Duchess")
				{
					color = new Color(69, 45, 0);
					color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 30));
					color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 20));
					color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 20));
					color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 10));
					color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 10));
					color7 = new Color(227, 221, 174);
					color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
					color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
				}
			}
			else if (num != 2869765680u)
			{
				if (num != 3050970914u)
				{
					if (num == 3610215645u)
					{
						if (theme == "Basic")
						{
							color = new Color(47, 46, 36);
							color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
							color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
							color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
							color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
							color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
							color7 = new Color(220, 215, 194);
							color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
							color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
						}
					}
				}
				else if (theme == "Outer Space")
				{
					color = new Color(20, 20, 20);
					color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
					color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
					color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
					color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
					color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
					color7 = new Color(194, 189, 202);
					color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
					color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
				}
			}
			else if (theme == "Biomes")
			{
				color = new Color(17, 36, 0);
				color2 = new Color((int)(color.R + 60), (int)(color.G + 60), (int)(color.B + 60));
				color3 = new Color((int)(color2.R + 30), (int)(color2.G + 30), (int)(color2.B + 30));
				color4 = new Color((int)(color3.R + 30), (int)(color3.G + 30), (int)(color3.B + 30));
				color5 = new Color((int)(color4.R + 15), (int)(color4.G + 15), (int)(color4.B + 15));
				color6 = new Color((int)(color5.R + 15), (int)(color5.G + 15), (int)(color5.B + 15));
				color7 = new Color(192, 255, 183);
				color8 = new Color(Math.Min(255, (int)(color7.R + 30)), Math.Min(255, (int)(color7.G + 30)), Math.Min(255, (int)(color7.B + 30)));
				color9 = new Color((int)(color7.R - 30), (int)(color7.G - 30), (int)(color7.B - 30));
			}
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15633, (int)color.R, (int)color.G, (int)color.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15645, (int)color6.R, (int)color6.G, (int)color6.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15649, (int)color4.R, (int)color4.G, (int)color4.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15641, (int)color4.R, (int)color4.G, (int)color4.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15637, (int)color3.R, (int)color3.G, (int)color3.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15666, (int)color7.R, (int)color7.G, (int)color7.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 40577, (int)color8.R, (int)color8.G, (int)color8.B);
			Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 40637, (int)color9.R, (int)color9.G, (int)color9.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1760, (int)color.R, (int)color.G, (int)color.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1764, (int)color3.R, (int)color3.G, (int)color3.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1768, (int)color4.R, (int)color4.G, (int)color4.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1841, (int)color6.R, (int)color6.G, (int)color6.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1792, (int)color7.R, (int)color7.G, (int)color7.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1834, (int)color8.R, (int)color8.G, (int)color8.B);
			Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1773, (int)color9.R, (int)color9.G, (int)color9.B);
		}

		// Token: 0x040008A7 RID: 2215
		public static List<VertexPositionColor[]> straightLineVertex = new List<VertexPositionColor[]>
		{
			new VertexPositionColor[2],
			new VertexPositionColor[2],
			new VertexPositionColor[2],
			new VertexPositionColor[2]
		};
	}
}
