using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Objects;
using xTile;

namespace StardewValley.Locations
{
	// Token: 0x02000133 RID: 307
	public class SeedShop : GameLocation
	{
		// Token: 0x060011A2 RID: 4514 RVA: 0x0016938A File Offset: 0x0016758A
		public SeedShop()
		{
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x001693A8 File Offset: 0x001675A8
		public SeedShop(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x001693C8 File Offset: 0x001675C8
		public string getPurchasedItemDialogueForNPC(Object i, NPC n)
		{
			string response = "...";
			string whatToCallPlayer = (Game1.random.NextDouble() < (double)(Game1.player.getFriendshipLevelForNPC(n.name) / 1250)) ? Game1.player.name : Game1.content.LoadString("Strings\\Lexicon:GenericPlayerTerm", new object[0]);
			if (n.age != 0)
			{
				whatToCallPlayer = Game1.player.name;
			}
			string particle = Game1.getProperArticleForWord(i.name);
			if ((i.category == -4 || i.category == -75 || i.category == -79) && Game1.random.NextDouble() < 0.5)
			{
				particle = "a fresh";
			}
			int whichDialogue = Game1.random.Next(5);
			if (n.manners == 2)
			{
				whichDialogue = 2;
			}
			switch (whichDialogue)
			{
			case 0:
				if (Game1.random.NextDouble() < (double)i.quality * 0.5 + 0.2)
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_1_QualityHigh", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name,
						Lexicon.getRandomDeliciousAdjective(n)
					});
				}
				else
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_1_QualityLow", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name,
						Lexicon.getRandomNegativeFoodAdjective(n)
					});
				}
				break;
			case 1:
				if (i.quality == 0)
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityLow", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name
					});
				}
				else if (n.name.Equals("Jodi"))
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityHigh_Jodi", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name
					});
				}
				else
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityHigh", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name
					});
				}
				break;
			case 2:
				if (n.manners == 2)
				{
					if (i.quality != 2)
					{
						response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_QualityLow_Rude", new object[]
						{
							whatToCallPlayer,
							particle,
							i.name,
							i.salePrice() / 2,
							Lexicon.getRandomNegativeFoodAdjective(n),
							Lexicon.getRandomNegativeItemSlanderNoun()
						});
					}
					else
					{
						Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_QualityHigh_Rude", new object[]
						{
							whatToCallPlayer,
							particle,
							i.name,
							i.salePrice() / 2,
							Lexicon.getRandomSlightlyPositiveAdjectiveForEdibleNoun(n)
						});
					}
				}
				else
				{
					Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_NonRude", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name,
						i.salePrice() / 2
					});
				}
				break;
			case 3:
				response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_4", new object[]
				{
					whatToCallPlayer,
					particle,
					i.name
				});
				break;
			case 4:
				if (i.category == -75 || i.category == -79)
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_VegetableOrFruit", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name
					});
				}
				else if (i.category == -7)
				{
					string adjective = Lexicon.getRandomPositiveAdjectiveForEventOrPerson(n);
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_Cooking", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name,
						Game1.getProperArticleForWord(adjective),
						adjective
					});
				}
				else
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_Foraged", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name
					});
				}
				break;
			}
			if (n.age == 1 && Game1.random.NextDouble() < 0.6)
			{
				response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Teen", new object[]
				{
					whatToCallPlayer,
					particle,
					i.name
				});
			}
			string name = n.name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1708213605u)
			{
				if (num != 208794864u)
				{
					if (num != 786557384u)
					{
						if (num == 1708213605u)
						{
							if (name == "Alex")
							{
								response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Alex", new object[]
								{
									whatToCallPlayer,
									particle,
									i.name
								});
							}
						}
					}
					else if (name == "Caroline")
					{
						if (i.quality == 0)
						{
							response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Caroline_QualityLow", new object[]
							{
								whatToCallPlayer,
								particle,
								i.name
							});
						}
						else
						{
							response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Caroline_QualityHigh", new object[]
							{
								whatToCallPlayer,
								particle,
								i.name
							});
						}
					}
				}
				else if (name == "Pierre")
				{
					if (i.quality == 0)
					{
						response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Pierre_QualityLow", new object[]
						{
							whatToCallPlayer,
							particle,
							i.name
						});
					}
					else
					{
						response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Pierre_QualityHigh", new object[]
						{
							whatToCallPlayer,
							particle,
							i.name
						});
					}
				}
			}
			else if (num <= 2732913340u)
			{
				if (num != 2434294092u)
				{
					if (num == 2732913340u)
					{
						if (name == "Abigail")
						{
							if (i.quality == 0)
							{
								response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Abigail_QualityLow", new object[]
								{
									whatToCallPlayer,
									particle,
									i.name,
									Lexicon.getRandomNegativeItemSlanderNoun()
								});
							}
							else
							{
								response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Abigail_QualityHigh", new object[]
								{
									whatToCallPlayer,
									particle,
									i.name
								});
							}
						}
					}
				}
				else if (name == "Haley")
				{
					response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Haley", new object[]
					{
						whatToCallPlayer,
						particle,
						i.name
					});
				}
			}
			else if (num != 2826247323u)
			{
				if (num == 3066176300u)
				{
					if (name == "Elliott")
					{
						response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Elliott", new object[]
						{
							whatToCallPlayer,
							particle,
							i.name
						});
					}
				}
			}
			else if (name == "Leah")
			{
				response = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Leah", new object[]
				{
					whatToCallPlayer,
					particle,
					i.name
				});
			}
			return response;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00169AC0 File Offset: 0x00167CC0
		public override void DayUpdate(int dayOfMonth)
		{
			for (int i = this.itemsToStartSellingTomorrow.Count - 1; i >= 0; i--)
			{
				if (this.itemsFromPlayerToSell.Count < 11)
				{
					bool stacked = false;
					foreach (Item item in this.itemsFromPlayerToSell)
					{
						if (item.Name.Equals(this.itemsToStartSellingTomorrow[i].Name) && (item as Object).quality == (this.itemsToStartSellingTomorrow[i] as Object).quality)
						{
							Item expr_7F = item;
							int stack = expr_7F.Stack;
							expr_7F.Stack = stack + 1;
							stacked = true;
							break;
						}
					}
					if (!stacked)
					{
						this.itemsFromPlayerToSell.Add(this.itemsToStartSellingTomorrow[i]);
					}
					this.itemsToStartSellingTomorrow.RemoveAt(i);
				}
			}
			base.DayUpdate(dayOfMonth);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00169BC4 File Offset: 0x00167DC4
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (Game1.player.maxItems == 12)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(7 * Game1.tileSize + Game1.pixelZoom * 2), (float)(17 * Game1.tileSize))), new Rectangle?(new Rectangle(255, 1436, 12, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 19.25f * (float)Game1.tileSize / 10000f);
				return;
			}
			if (Game1.player.maxItems < 36)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)(7 * Game1.tileSize + Game1.pixelZoom * 2), (float)(17 * Game1.tileSize))), new Rectangle?(new Rectangle(267, 1436, 12, 14)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 19.25f * (float)Game1.tileSize / 10000f);
				return;
			}
			b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Rectangle(7 * Game1.tileSize + Game1.pixelZoom, 18 * Game1.tileSize + Game1.tileSize / 2, Game1.tileSize * 3 / 2 + Game1.pixelZoom * 4, Game1.tileSize / 4 + Game1.pixelZoom)), new Rectangle?(new Rectangle(258, 1449, 1, 1)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 19.25f * (float)Game1.tileSize / 10000f);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00169D60 File Offset: 0x00167F60
		public List<Item> shopStock()
		{
			List<Item> stock = new List<Item>();
			if (Game1.currentSeason.Equals("spring"))
			{
				stock.Add(new Object(Vector2.Zero, 472, 2147483647));
				stock.Add(new Object(Vector2.Zero, 473, 2147483647));
				stock.Add(new Object(Vector2.Zero, 474, 2147483647));
				stock.Add(new Object(Vector2.Zero, 475, 2147483647));
				stock.Add(new Object(Vector2.Zero, 427, 2147483647));
				stock.Add(new Object(Vector2.Zero, 477, 2147483647));
				stock.Add(new Object(Vector2.Zero, 429, 2147483647));
				if (Game1.year > 1)
				{
					stock.Add(new Object(Vector2.Zero, 476, 2147483647));
				}
			}
			if (Game1.currentSeason.Equals("summer"))
			{
				stock.Add(new Object(Vector2.Zero, 479, 2147483647));
				stock.Add(new Object(Vector2.Zero, 480, 2147483647));
				stock.Add(new Object(Vector2.Zero, 481, 2147483647));
				stock.Add(new Object(Vector2.Zero, 482, 2147483647));
				stock.Add(new Object(Vector2.Zero, 483, 2147483647));
				stock.Add(new Object(Vector2.Zero, 484, 2147483647));
				stock.Add(new Object(Vector2.Zero, 453, 2147483647));
				stock.Add(new Object(Vector2.Zero, 455, 2147483647));
				stock.Add(new Object(Vector2.Zero, 302, 2147483647));
				stock.Add(new Object(Vector2.Zero, 487, 2147483647));
				stock.Add(new Object(431, 2147483647, false, 100, 0));
				if (Game1.year > 1)
				{
					stock.Add(new Object(Vector2.Zero, 485, 2147483647));
				}
			}
			if (Game1.currentSeason.Equals("fall"))
			{
				stock.Add(new Object(Vector2.Zero, 490, 2147483647));
				stock.Add(new Object(Vector2.Zero, 487, 2147483647));
				stock.Add(new Object(Vector2.Zero, 488, 2147483647));
				stock.Add(new Object(Vector2.Zero, 491, 2147483647));
				stock.Add(new Object(Vector2.Zero, 492, 2147483647));
				stock.Add(new Object(Vector2.Zero, 493, 2147483647));
				stock.Add(new Object(Vector2.Zero, 483, 2147483647));
				stock.Add(new Object(431, 2147483647, false, 100, 0));
				stock.Add(new Object(Vector2.Zero, 425, 2147483647));
				stock.Add(new Object(Vector2.Zero, 299, 2147483647));
				stock.Add(new Object(Vector2.Zero, 301, 2147483647));
				if (Game1.year > 1)
				{
					stock.Add(new Object(Vector2.Zero, 489, 2147483647));
				}
			}
			stock.Add(new Object(Vector2.Zero, 297, 2147483647));
			stock.Add(new Object(Vector2.Zero, 245, 2147483647));
			stock.Add(new Object(Vector2.Zero, 246, 2147483647));
			stock.Add(new Object(Vector2.Zero, 423, 2147483647));
			stock.Add(new Object(Vector2.Zero, 247, 2147483647));
			stock.Add(new Object(Vector2.Zero, 419, 2147483647));
			if (Game1.stats.DaysPlayed >= 15u)
			{
				stock.Add(new Object(368, 2147483647, false, 50, 0));
				stock.Add(new Object(370, 2147483647, false, 50, 0));
				stock.Add(new Object(465, 2147483647, false, 50, 0));
			}
			if (Game1.year > 1)
			{
				stock.Add(new Object(369, 2147483647, false, 75, 0));
				stock.Add(new Object(371, 2147483647, false, 75, 0));
				stock.Add(new Object(466, 2147483647, false, 75, 0));
			}
			Random r = new Random((int)(Game1.stats.DaysPlayed + (uint)((int)Game1.uniqueIDForThisGame / 2)));
			int wp = r.Next(112);
			if (wp == 21)
			{
				wp = 36;
			}
			stock.Add(new Wallpaper(wp, false)
			{
				stack = 2147483647
			});
			stock.Add(new Wallpaper(r.Next(40), true)
			{
				stack = 2147483647
			});
			stock.Add(new Furniture(1308, Vector2.Zero)
			{
				stack = 2147483647
			});
			stock.Add(new Object(628, 2147483647, false, 1700, 0));
			stock.Add(new Object(629, 2147483647, false, 1000, 0));
			stock.Add(new Object(630, 2147483647, false, 2000, 0));
			stock.Add(new Object(631, 2147483647, false, 3000, 0));
			stock.Add(new Object(632, 2147483647, false, 3000, 0));
			stock.Add(new Object(633, 2147483647, false, 2000, 0));
			foreach (Item i in this.itemsFromPlayerToSell)
			{
				stock.Add(i);
			}
			if (Game1.player.hasAFriendWithHeartLevel(8, true))
			{
				stock.Add(new Object(Vector2.Zero, 458, 2147483647));
			}
			return stock;
		}

		// Token: 0x04001287 RID: 4743
		public const int maxItemsToSellFromPlayer = 11;

		// Token: 0x04001288 RID: 4744
		public List<Item> itemsFromPlayerToSell = new List<Item>();

		// Token: 0x04001289 RID: 4745
		public List<Item> itemsToStartSellingTomorrow = new List<Item>();
	}
}
