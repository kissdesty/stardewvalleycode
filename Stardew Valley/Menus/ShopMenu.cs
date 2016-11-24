using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;

namespace StardewValley.Menus
{
	// Token: 0x02000110 RID: 272
	public class ShopMenu : IClickableMenu
	{
		// Token: 0x06000FB6 RID: 4022 RVA: 0x001446CC File Offset: 0x001428CC
		public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null) : this(itemPriceAndStock.Keys.ToList<Item>(), currency, who)
		{
			this.itemPriceAndStock = itemPriceAndStock;
			if (this.potraitPersonDialogue == null)
			{
				this.setUpShopOwner(who);
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x001446F8 File Offset: 0x001428F8
		public ShopMenu(List<Item> itemsForSale, int currency = 0, string who = null) : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 1000 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
		{
			this.currency = currency;
			if (Game1.viewport.Width < 1500)
			{
				this.xPositionOnScreen = Game1.tileSize / 2;
			}
			Game1.player.forceCanMove();
			Game1.playSound("dwop");
			this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, null, new InventoryMenu.highlightThisItem(this.highlightItemToSell), -1, 3, 0, 0, true)
			{
				showGrayedOutSlots = true
			};
			this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
			this.currency = currency;
			int arg_17E_0 = this.xPositionOnScreen;
			int arg_184_0 = IClickableMenu.borderWidth;
			int arg_18A_0 = IClickableMenu.spaceToClearSideBorder;
			int arg_191_0 = this.yPositionOnScreen;
			int arg_197_0 = IClickableMenu.borderWidth;
			int arg_19D_0 = IClickableMenu.spaceToClearTopBorder;
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
			for (int i = 0; i < 4; i++)
			{
				this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat(i)));
			}
			foreach (Item j in itemsForSale)
			{
				if (j is Object && (j as Object).isRecipe)
				{
					if (Game1.player.knowsRecipe(j.Name))
					{
						continue;
					}
					j.Stack = 1;
				}
				this.forSale.Add(j);
				this.itemPriceAndStock.Add(j, new int[]
				{
					j.salePrice(),
					j.Stack
				});
			}
			if (this.itemPriceAndStock.Count >= 2)
			{
				this.setUpShopOwner(who);
			}
			string name = Game1.currentLocation.name;
			if (!(name == "SeedShop"))
			{
				if (!(name == "Blacksmith"))
				{
					if (!(name == "ScienceHouse"))
					{
						if (!(name == "AnimalShop"))
						{
							if (!(name == "FishShop"))
							{
								if (name == "AdventureGuild")
								{
									this.categoriesToSellHere.AddRange(new int[]
									{
										-28,
										-98,
										-97,
										-96
									});
								}
							}
							else
							{
								this.categoriesToSellHere.AddRange(new int[]
								{
									-4,
									-23,
									-21,
									-22
								});
							}
						}
						else
						{
							this.categoriesToSellHere.AddRange(new int[]
							{
								-18,
								-6,
								-5,
								-14
							});
						}
					}
					else
					{
						this.categoriesToSellHere.AddRange(new int[]
						{
							-16
						});
					}
				}
				else
				{
					this.categoriesToSellHere.AddRange(new int[]
					{
						-12,
						-2,
						-15
					});
				}
			}
			else
			{
				this.categoriesToSellHere.AddRange(new int[]
				{
					-81,
					-75,
					-79,
					-80,
					-74,
					-17,
					-18,
					-6,
					-26,
					-5,
					-14,
					-19,
					-7,
					-25
				});
			}
			Game1.currentLocation.Name.Equals("SeedShop");
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00144CB4 File Offset: 0x00142EB4
		public void setUpShopOwner(string who)
		{
			if (who != null)
			{
				Random r = new Random((int)(Game1.uniqueIDForThisGame + (ulong)Game1.stats.DaysPlayed));
				string ppDialogue = "Have a look at my wares.";
				uint num = <PrivateImplementationDetails>.ComputeStringHash(who);
				if (num <= 1771728057u)
				{
					if (num <= 1305917497u)
					{
						if (num != 208794864u)
						{
							if (num != 1089105211u)
							{
								if (num == 1305917497u)
								{
									if (who == "Krobus")
									{
										this.portraitPerson = Game1.getCharacterFromName("Krobus", false);
										ppDialogue = "Rare Goods";
									}
								}
							}
							else if (who == "Dwarf")
							{
								this.portraitPerson = Game1.getCharacterFromName("Dwarf", false);
								ppDialogue = "Buy something?";
							}
						}
						else if (who == "Pierre")
						{
							this.portraitPerson = Game1.getCharacterFromName("Pierre", false);
							switch (Game1.dayOfMonth % 7)
							{
							case 0:
								ppDialogue = "Got anything you want to sell?";
								break;
							case 1:
								ppDialogue = "Need some supplies?";
								break;
							case 2:
								ppDialogue = "Don't forget to check out my daily wallpaper and flooring selection!";
								break;
							case 3:
								ppDialogue = "What can I get for you?";
								break;
							case 4:
								ppDialogue = "I carry only the finest goods.";
								break;
							case 5:
								ppDialogue = "I've got quality goods for sale.";
								break;
							case 6:
								ppDialogue = "Looking to buy something?";
								break;
							}
							ppDialogue = "Welcome to Pierre's! " + ppDialogue;
							if (Game1.dayOfMonth == 28)
							{
								ppDialogue = "The season's almost over. I'll be changing stock tomorrow.";
							}
						}
					}
					else if (num != 1409564722u)
					{
						if (num != 1639180769u)
						{
							if (num == 1771728057u)
							{
								if (who == "ClintUpgrade")
								{
									this.portraitPerson = Game1.getCharacterFromName("Clint", false);
									ppDialogue = "I can upgrade your tools with more power. You'll have to leave them with me for a few days, though.";
								}
							}
						}
						else if (who == "HatMouse")
						{
							ppDialogue = "Hiyo, poke. Did you bring coins? Gud. Me sell hats.";
						}
					}
					else if (who == "Traveler")
					{
						switch (r.Next(5))
						{
						case 0:
							ppDialogue = "I've got a little bit of everything. Take a look!";
							break;
						case 1:
							ppDialogue = "I smuggled these goods out of the Gotoro Empire. Why do you think they're so expensive?";
							break;
						case 2:
							ppDialogue = "I'll have new items every week, so make sure to come back!";
							break;
						case 3:
							ppDialogue = "Let me see... Oh! I've got just what you need: " + Lexicon.appendArticle(this.itemPriceAndStock.ElementAt(r.Next(this.itemPriceAndStock.Count)).Key.Name) + "!";
							break;
						case 4:
							ppDialogue = "Beautiful country you have here. One of my favorite stops. The pig likes it, too.";
							break;
						}
					}
				}
				else if (num <= 2750361957u)
				{
					if (num != 2379602843u)
					{
						if (num != 2711797968u)
						{
							if (num == 2750361957u)
							{
								if (who == "Marnie")
								{
									this.portraitPerson = Game1.getCharacterFromName("Marnie", false);
									ppDialogue = "Animal supplies for sale!";
									if (r.NextDouble() < 0.0001)
									{
										ppDialogue = "*sigh*... When the door opened I thought it might be Lewis.";
									}
								}
							}
						}
						else if (who == "Marlon")
						{
							this.portraitPerson = Game1.getCharacterFromName("Marlon", false);
							switch (r.Next(4))
							{
							case 0:
								ppDialogue = "The caves can be dangerous. Make sure you're prepared.";
								break;
							case 1:
								ppDialogue = "In the market for a new sword?";
								break;
							case 2:
								ppDialogue = "Welcome to the adventurer's guild.";
								break;
							case 3:
								ppDialogue = "Slay any monsters? I'll buy the loot.";
								break;
							}
							if (r.NextDouble() < 0.001)
							{
								ppDialogue = "The caves can be dangerous. How do you think I lost this eye?";
							}
						}
					}
					else if (who == "Robin")
					{
						this.portraitPerson = Game1.getCharacterFromName("Robin", false);
						switch (Game1.random.Next(5))
						{
						case 0:
							ppDialogue = "Need some construction supplies? Or are you looking to re-decorate?";
							break;
						case 1:
							ppDialogue = "I have a rotating selection of hand-made furniture.";
							break;
						case 2:
							ppDialogue = "I've got some great furniture for sale.";
							break;
						case 3:
							ppDialogue = "Got any spare construction material to sell?";
							break;
						case 4:
							ppDialogue = string.Concat(new string[]
							{
								"I've got ",
								Lexicon.appendArticle(this.itemPriceAndStock.ElementAt(Game1.random.Next(2, this.itemPriceAndStock.Count)).Key.Name),
								" that would look just ",
								Lexicon.getRandomPositiveAdjectiveForEventOrPerson(null),
								" in your house."
							});
							break;
						}
					}
				}
				else if (num <= 3818424508u)
				{
					if (num != 3015695534u)
					{
						if (num == 3818424508u)
						{
							if (who == "Willy")
							{
								this.portraitPerson = Game1.getCharacterFromName("Willy", false);
								ppDialogue = "Need fishing supplies? You've come to the right place.";
								if (Game1.random.NextDouble() < 0.05)
								{
									ppDialogue = "Sorry about the smell.";
								}
							}
						}
					}
					else if (who == "Gus")
					{
						this.portraitPerson = Game1.getCharacterFromName("Gus", false);
						switch (Game1.random.Next(4))
						{
						case 0:
							ppDialogue = "What'll you have?";
							break;
						case 1:
							ppDialogue = "Can you smell that? It's the " + this.itemPriceAndStock.ElementAt(r.Next(this.itemPriceAndStock.Count)).Key.Name;
							break;
						case 2:
							ppDialogue = "Hungry? Thirsty? I've got just the thing.";
							break;
						case 3:
							ppDialogue = "Welcome to the Stardrop Saloon! What can I get ya?";
							break;
						}
					}
				}
				else if (num != 3845337251u)
				{
					if (num == 4194582670u)
					{
						if (who == "Sandy")
						{
							this.portraitPerson = Game1.getCharacterFromName("Sandy", false);
							ppDialogue = "You won't find these goods anywhere else!";
							if (r.NextDouble() < 0.0001)
							{
								ppDialogue = "I've got just what you need.";
							}
						}
					}
				}
				else if (who == "Clint")
				{
					this.portraitPerson = Game1.getCharacterFromName("Clint", false);
					switch (Game1.random.Next(3))
					{
					case 0:
						ppDialogue = "Too lazy to mine your own ore? No problem.";
						break;
					case 1:
						ppDialogue = "I've got lumps of raw metal for sale. Knock yourself out.";
						break;
					case 2:
						ppDialogue = "Looking to sell any metals or minerals?";
						break;
					}
				}
				this.potraitPersonDialogue = Game1.parseText(ppDialogue, Game1.dialogueFont, Game1.tileSize * 5 - Game1.pixelZoom * 4);
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00145327 File Offset: 0x00143527
		public bool highlightItemToSell(Item i)
		{
			return this.categoriesToSellHere.Contains(i.category);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0014533F File Offset: 0x0014353F
		public static int getPlayerCurrencyAmount(Farmer who, int currencyType)
		{
			switch (currencyType)
			{
			case 0:
				return who.Money;
			case 1:
				return who.festivalScore;
			case 2:
				return who.clubCoins;
			default:
				return 0;
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0014536C File Offset: 0x0014356C
		public override void leftClickHeld(int x, int y)
		{
			base.leftClickHeld(x, y);
			if (this.scrolling)
			{
				int arg_E8_0 = this.scrollBar.bounds.Y;
				this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
				float percentage = (float)(y - this.scrollBarRunner.Y) / (float)this.scrollBarRunner.Height;
				this.currentItemIndex = Math.Min(this.forSale.Count - 4, Math.Max(0, (int)((float)this.forSale.Count * percentage)));
				this.setScrollBarToCurrentIndex();
				if (arg_E8_0 != this.scrollBar.bounds.Y)
				{
					Game1.playSound("shiny4");
				}
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0014546D File Offset: 0x0014366D
		public override void releaseLeftClick(int x, int y)
		{
			base.releaseLeftClick(x, y);
			this.scrolling = false;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00145480 File Offset: 0x00143680
		private void setScrollBarToCurrentIndex()
		{
			if (this.forSale.Count > 0)
			{
				this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.forSale.Count - 4 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
				if (this.currentItemIndex == this.forSale.Count - 4)
				{
					this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
				}
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0014553C File Offset: 0x0014373C
		public override void receiveScrollWheelAction(int direction)
		{
			base.receiveScrollWheelAction(direction);
			if (direction > 0 && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shiny4");
				return;
			}
			if (direction < 0 && this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
			{
				this.downArrowPressed();
				Game1.playSound("shiny4");
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0014559D File Offset: 0x0014379D
		private void downArrowPressed()
		{
			this.downArrow.scale = this.downArrow.baseScale;
			this.currentItemIndex++;
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x001455C9 File Offset: 0x001437C9
		private void upArrowPressed()
		{
			this.upArrow.scale = this.upArrow.baseScale;
			this.currentItemIndex--;
			this.setScrollBarToCurrentIndex();
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x001455F8 File Offset: 0x001437F8
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, true);
			if (Game1.activeClickableMenu == null)
			{
				return;
			}
			Vector2 snappedPosition = this.inventory.snapToClickableComponent(x, y);
			if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
			{
				this.downArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
			{
				this.upArrowPressed();
				Game1.playSound("shwip");
			}
			else if (this.scrollBar.containsPoint(x, y))
			{
				this.scrolling = true;
			}
			else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen && y < this.yPositionOnScreen + this.height)
			{
				this.scrolling = true;
				this.leftClickHeld(x, y);
				this.releaseLeftClick(x, y);
			}
			this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
			if (this.heldItem == null)
			{
				Item toSell = this.inventory.leftClick(x, y, null, false);
				if (toSell != null)
				{
					ShopMenu.chargePlayer(Game1.player, this.currency, -((int)((toSell is Object) ? ((float)(toSell as Object).sellToStorePrice() * this.sellPercentage) : ((float)(toSell.salePrice() / 2) * this.sellPercentage)) * toSell.Stack));
					int coins = toSell.Stack / 8 + 2;
					for (int i = 0; i < coins; i++)
					{
						this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, snappedPosition + new Vector2(32f, 32f), false, false)
						{
							alphaFade = 0.025f,
							motion = new Vector2((float)Game1.random.Next(-3, 4), -4f),
							acceleration = new Vector2(0f, 0.5f),
							delayBeforeAnimationStart = i * 25,
							scale = (float)Game1.pixelZoom * 0.5f
						});
						this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, snappedPosition + new Vector2(32f, 32f), false, false)
						{
							scale = (float)Game1.pixelZoom,
							alphaFade = 0.025f,
							delayBeforeAnimationStart = i * 50,
							motion = Utility.getVelocityTowardPoint(new Point((int)snappedPosition.X + 32, (int)snappedPosition.Y + 32), new Vector2((float)(this.xPositionOnScreen - Game1.pixelZoom * 9), (float)(this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4)), 8f),
							acceleration = Utility.getVelocityTowardPoint(new Point((int)snappedPosition.X + 32, (int)snappedPosition.Y + 32), new Vector2((float)(this.xPositionOnScreen - Game1.pixelZoom * 9), (float)(this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4)), 0.5f)
						});
					}
					if (toSell is Object && (toSell as Object).edibility != -300)
					{
						for (int j = 0; j < toSell.Stack; j++)
						{
							if (Game1.random.NextDouble() < 0.039999999105930328)
							{
								(Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(toSell.getOne());
							}
						}
					}
					Game1.playSound("sell");
					Game1.playSound("purchase");
					if (this.inventory.getItemAt(x, y) == null)
					{
						this.animations.Add(new TemporaryAnimatedSprite(5, snappedPosition + new Vector2(32f, 32f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
						{
							motion = new Vector2(0f, -0.5f)
						});
					}
				}
			}
			else
			{
				this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
			}
			for (int k = 0; k < this.forSaleButtons.Count; k++)
			{
				if (this.currentItemIndex + k < this.forSale.Count && this.forSaleButtons[k].containsPoint(x, y))
				{
					int index = this.currentItemIndex + k;
					if (this.forSale[index] != null)
					{
						int toBuy = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) / Math.Max(1, this.itemPriceAndStock[this.forSale[index]][0])), Math.Max(1, this.itemPriceAndStock[this.forSale[index]][1])) : 1;
						toBuy = Math.Min(toBuy, this.forSale[index].maximumStackSize());
						if (toBuy == -1)
						{
							toBuy = 1;
						}
						if (toBuy > 0 && this.tryToPurchaseItem(this.forSale[index], this.heldItem, toBuy, x, y, index))
						{
							this.itemPriceAndStock.Remove(this.forSale[index]);
							this.forSale.RemoveAt(index);
						}
						else if (toBuy <= 0)
						{
							Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
							Game1.playSound("cancel");
						}
					}
					this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
					return;
				}
			}
			if (this.readyToClose() && (x < this.xPositionOnScreen - Game1.tileSize || y < this.yPositionOnScreen - Game1.tileSize || x > this.xPositionOnScreen + this.width + Game1.tileSize * 2 || y > this.yPositionOnScreen + this.height + Game1.tileSize))
			{
				base.exitThisMenu(true);
			}
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00145C73 File Offset: 0x00143E73
		public override bool readyToClose()
		{
			return this.heldItem == null && this.animations.Count == 0;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00145C8D File Offset: 0x00143E8D
		public override void emergencyShutDown()
		{
			base.emergencyShutDown();
			if (this.heldItem != null)
			{
				Game1.player.addItemToInventoryBool(this.heldItem, false);
				Game1.playSound("coin");
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00145CBC File Offset: 0x00143EBC
		public static void chargePlayer(Farmer who, int currencyType, int amount)
		{
			switch (currencyType)
			{
			case 0:
				who.Money -= amount;
				return;
			case 1:
				who.festivalScore -= amount;
				return;
			case 2:
				who.clubCoins -= amount;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00145D08 File Offset: 0x00143F08
		private bool tryToPurchaseItem(Item item, Item heldItem, int numberToBuy, int x, int y, int indexInForSaleList)
		{
			if (heldItem == null)
			{
				int price = this.itemPriceAndStock[item][0] * numberToBuy;
				int extraTradeItem = -1;
				if (this.itemPriceAndStock[item].Length > 2)
				{
					extraTradeItem = this.itemPriceAndStock[item][2];
				}
				if (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= price && (extraTradeItem == -1 || Game1.player.hasItemInInventory(extraTradeItem, 5, 0)))
				{
					this.heldItem = item.getOne();
					this.heldItem.Stack = numberToBuy;
					if (!Game1.player.couldInventoryAcceptThisItem(this.heldItem))
					{
						Game1.playSound("smallSelect");
						this.heldItem = null;
						return false;
					}
					if (this.itemPriceAndStock[item][1] != 2147483647)
					{
						this.itemPriceAndStock[item][1] -= numberToBuy;
						this.forSale[indexInForSaleList].Stack -= numberToBuy;
					}
					ShopMenu.chargePlayer(Game1.player, this.currency, price);
					if (extraTradeItem != -1)
					{
						Game1.player.removeItemsFromInventory(extraTradeItem, 5);
					}
					if (item.actionWhenPurchased())
					{
						if (this.heldItem is Object && (this.heldItem as Object).isRecipe)
						{
							string recipeName = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
							try
							{
								if ((this.heldItem as Object).category == -7)
								{
									Game1.player.cookingRecipes.Add(recipeName, 0);
								}
								else
								{
									Game1.player.craftingRecipes.Add(recipeName, 0);
								}
								Game1.playSound("newRecipe");
							}
							catch (Exception)
							{
							}
							heldItem = null;
							this.heldItem = null;
						}
					}
					else if (Game1.mouseClickPolling > 300)
					{
						Game1.playSound("purchaseRepeat");
					}
					else
					{
						Game1.playSound("purchaseClick");
					}
				}
				else
				{
					Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
					Game1.playSound("cancel");
				}
			}
			else if (heldItem.Name.Equals(item.Name))
			{
				numberToBuy = Math.Min(numberToBuy, heldItem.maximumStackSize() - heldItem.Stack);
				if (numberToBuy > 0)
				{
					int price2 = this.itemPriceAndStock[item][0] * numberToBuy;
					int extraTradeItem2 = -1;
					if (this.itemPriceAndStock[item].Length > 2)
					{
						extraTradeItem2 = this.itemPriceAndStock[item][2];
					}
					if (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= price2)
					{
						this.heldItem.Stack += numberToBuy;
						if (this.itemPriceAndStock[item][1] != 2147483647)
						{
							this.itemPriceAndStock[item][1] -= numberToBuy;
						}
						ShopMenu.chargePlayer(Game1.player, this.currency, price2);
						if (Game1.mouseClickPolling > 300)
						{
							Game1.playSound("purchaseRepeat");
						}
						else
						{
							Game1.playSound("purchaseClick");
						}
						if (extraTradeItem2 != -1)
						{
							Game1.player.removeItemsFromInventory(extraTradeItem2, 5);
						}
						if (item.actionWhenPurchased())
						{
							this.heldItem = null;
						}
					}
					else
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
						Game1.playSound("cancel");
					}
				}
			}
			if (this.itemPriceAndStock[item][1] <= 0)
			{
				this.hoveredItem = null;
				return true;
			}
			return false;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00146070 File Offset: 0x00144270
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			Vector2 snappedPosition = this.inventory.snapToClickableComponent(x, y);
			if (this.heldItem == null)
			{
				Item toSell = this.inventory.rightClick(x, y, null, false);
				if (toSell != null)
				{
					ShopMenu.chargePlayer(Game1.player, this.currency, -((int)((toSell is Object) ? ((float)(toSell as Object).sellToStorePrice() * this.sellPercentage) : ((float)(toSell.salePrice() / 2) * this.sellPercentage)) * toSell.Stack));
					toSell = null;
					if (Game1.mouseClickPolling > 300)
					{
						Game1.playSound("purchaseRepeat");
					}
					else
					{
						Game1.playSound("purchaseClick");
					}
					this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * Game1.tileSize, 256, Game1.tileSize, Game1.tileSize), 9999f, 1, 999, snappedPosition + new Vector2(32f, 32f), false, false)
					{
						alphaFade = 0.025f,
						motion = Utility.getVelocityTowardPoint(new Point((int)snappedPosition.X + 32, (int)snappedPosition.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 12f),
						acceleration = Utility.getVelocityTowardPoint(new Point((int)snappedPosition.X + 32, (int)snappedPosition.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 0.5f)
					});
					if (toSell is Object && (toSell as Object).edibility != -300 && Game1.random.NextDouble() < 0.039999999105930328)
					{
						(Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(toSell.getOne());
					}
					if (this.inventory.getItemAt(x, y) == null)
					{
						Game1.playSound("sell");
						this.animations.Add(new TemporaryAnimatedSprite(5, snappedPosition + new Vector2(32f, 32f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
						{
							motion = new Vector2(0f, -0.5f)
						});
					}
				}
			}
			else
			{
				this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
			}
			for (int i = 0; i < this.forSaleButtons.Count; i++)
			{
				if (this.currentItemIndex + i < this.forSale.Count && this.forSaleButtons[i].containsPoint(x, y))
				{
					int index = this.currentItemIndex + i;
					if (this.forSale[index] != null)
					{
						int toBuy = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) / this.itemPriceAndStock[this.forSale[index]][0]), this.itemPriceAndStock[this.forSale[index]][1]) : 1;
						if (toBuy > 0 && this.tryToPurchaseItem(this.forSale[index], this.heldItem, toBuy, x, y, index))
						{
							this.itemPriceAndStock.Remove(this.forSale[index]);
							this.forSale.RemoveAt(index);
						}
					}
					return;
				}
			}
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x001463F4 File Offset: 0x001445F4
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			this.descriptionText = "";
			this.hoverText = "";
			this.hoveredItem = null;
			this.hoverPrice = -1;
			this.boldTitleText = "";
			this.upArrow.tryHover(x, y, 0.1f);
			this.downArrow.tryHover(x, y, 0.1f);
			this.scrollBar.tryHover(x, y, 0.1f);
			if (this.scrolling)
			{
				return;
			}
			for (int i = 0; i < this.forSaleButtons.Count; i++)
			{
				if (this.currentItemIndex + i < this.forSale.Count && this.forSaleButtons[i].containsPoint(x, y))
				{
					Item item = this.forSale[this.currentItemIndex + i];
					this.hoverText = item.getDescription();
					this.boldTitleText = item.Name;
					this.hoverPrice = ((this.itemPriceAndStock != null && this.itemPriceAndStock.ContainsKey(item)) ? this.itemPriceAndStock[item][0] : item.salePrice());
					this.hoveredItem = item;
					this.forSaleButtons[i].scale = Math.Min(this.forSaleButtons[i].scale + 0.03f, 1.1f);
				}
				else
				{
					this.forSaleButtons[i].scale = Math.Max(1f, this.forSaleButtons[i].scale - 0.03f);
				}
			}
			if (this.heldItem == null)
			{
				foreach (ClickableComponent c in this.inventory.inventory)
				{
					if (c.containsPoint(x, y))
					{
						Item j = this.inventory.getItemFromClickableComponent(c);
						if (j != null && this.highlightItemToSell(j))
						{
							this.hoverText = j.Name + " x " + j.Stack;
							this.hoverPrice = (int)((j is Object) ? ((float)(j as Object).sellToStorePrice() * this.sellPercentage) : ((float)(j.salePrice() / 2) * this.sellPercentage)) * j.Stack;
						}
					}
				}
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0014666C File Offset: 0x0014486C
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.poof != null && this.poof.update(time))
			{
				this.poof = null;
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00146694 File Offset: 0x00144894
		public void drawCurrency(SpriteBatch b)
		{
			int num = this.currency;
			if (num != 0)
			{
				return;
			}
			Game1.dayTimeMoneyBox.drawMoneyBox(b, this.xPositionOnScreen - Game1.pixelZoom * 9, this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 3);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x001466EC File Offset: 0x001448EC
		private int getHoveredItemExtraItemIndex()
		{
			if (this.itemPriceAndStock != null && this.hoveredItem != null && this.itemPriceAndStock.ContainsKey(this.hoveredItem) && this.itemPriceAndStock[this.hoveredItem].Length > 2)
			{
				return this.itemPriceAndStock[this.hoveredItem][2];
			}
			return -1;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00146747 File Offset: 0x00144947
		private int getHoveredItemExtraItemAmount()
		{
			return 5;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0014674C File Offset: 0x0014494C
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.xPositionOnScreen = Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2;
			this.width = 1000 + IClickableMenu.borderWidth * 2;
			this.height = 600 + IClickableMenu.borderWidth * 2;
			base.initializeUpperRightCloseButton();
			if (Game1.viewport.Width < 1500)
			{
				this.xPositionOnScreen = Game1.tileSize / 2;
			}
			Game1.player.forceCanMove();
			this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, null, new InventoryMenu.highlightThisItem(this.highlightItemToSell), -1, 3, 0, 0, true)
			{
				showGrayedOutSlots = true
			};
			this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
			int arg_113_0 = this.xPositionOnScreen;
			int arg_119_0 = IClickableMenu.borderWidth;
			int arg_11F_0 = IClickableMenu.spaceToClearSideBorder;
			int arg_126_0 = this.yPositionOnScreen;
			int arg_12C_0 = IClickableMenu.borderWidth;
			int arg_132_0 = IClickableMenu.spaceToClearTopBorder;
			this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float)Game1.pixelZoom, false);
			this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float)Game1.pixelZoom, false);
			this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float)Game1.pixelZoom, false);
			this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
			this.forSaleButtons.Clear();
			for (int i = 0; i < 4; i++)
			{
				this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + i * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat(i)));
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00146AD4 File Offset: 0x00144CD4
		public override void draw(SpriteBatch b)
		{
			if (!Game1.options.showMenuBackground)
			{
				b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
			}
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen + this.width - this.inventory.width - Game1.tileSize / 2 - Game1.pixelZoom * 6, this.yPositionOnScreen + this.height - Game1.tileSize * 4 + Game1.pixelZoom * 10, this.inventory.width + Game1.pixelZoom * 14, this.height - Game1.tileSize * 7 + Game1.pixelZoom * 5, Color.White, (float)Game1.pixelZoom, true);
			IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - Game1.tileSize * 4 + Game1.tileSize / 2 + Game1.pixelZoom, Color.White, (float)Game1.pixelZoom, true);
			this.drawCurrency(b);
			for (int i = 0; i < this.forSaleButtons.Count; i++)
			{
				if (this.currentItemIndex + i < this.forSale.Count)
				{
					IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.forSaleButtons[i].bounds.X, this.forSaleButtons[i].bounds.Y, this.forSaleButtons[i].bounds.Width, this.forSaleButtons[i].bounds.Height, (this.forSaleButtons[i].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && !this.scrolling) ? Color.Wheat : Color.White, (float)Game1.pixelZoom, false);
					b.Draw(Game1.mouseCursors, new Vector2((float)(this.forSaleButtons[i].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 3), (float)(this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 6 - Game1.pixelZoom)), new Rectangle?(new Rectangle(296, 363, 18, 18)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
					this.forSale[this.currentItemIndex + i].drawInMenu(b, new Vector2((float)(this.forSaleButtons[i].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 2), (float)(this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 6)), 1f);
					SpriteText.drawString(b, this.forSale[this.currentItemIndex + i].Name, this.forSaleButtons[i].bounds.X + Game1.tileSize * 3 / 2 + Game1.pixelZoom * 2, this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
					SpriteText.drawString(b, this.itemPriceAndStock[this.forSale[this.currentItemIndex + i]][0] + " ", this.forSaleButtons[i].bounds.Right - SpriteText.getWidthOfString(this.itemPriceAndStock[this.forSale[this.currentItemIndex + i]][0] + " ") - Game1.pixelZoom * 8, this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= this.itemPriceAndStock[this.forSale[this.currentItemIndex + i]][0]) ? 1f : 0.5f, 0.88f, false, -1, "", -1);
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.forSaleButtons[i].bounds.Right - Game1.pixelZoom * 13), (float)(this.forSaleButtons[i].bounds.Y + Game1.pixelZoom * 10 - Game1.pixelZoom)), new Rectangle(193 + this.currency * 9, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
				}
			}
			if (this.forSale.Count == 0)
			{
				SpriteText.drawString(b, "Out of stock", this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString("Out of stock.") / 2, this.yPositionOnScreen + this.height / 2 - Game1.tileSize * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
			}
			this.inventory.draw(b);
			for (int j = this.animations.Count - 1; j >= 0; j--)
			{
				if (this.animations[j].update(Game1.currentGameTime))
				{
					this.animations.RemoveAt(j);
				}
				else
				{
					this.animations[j].draw(b, true, 0, 0);
				}
			}
			if (this.poof != null)
			{
				this.poof.draw(b, false, 0, 0);
			}
			this.upArrow.draw(b);
			this.downArrow.draw(b);
			if (this.forSale.Count > 4)
			{
				IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float)Game1.pixelZoom, true);
				this.scrollBar.draw(b);
			}
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawToolTip(b, this.hoverText, this.boldTitleText, this.hoveredItem, this.heldItem != null, -1, this.currency, this.getHoveredItemExtraItemIndex(), this.getHoveredItemExtraItemAmount(), null, this.hoverPrice);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
			}
			base.draw(b);
			if (Game1.viewport.Width > 1800 && Game1.options.showMerchantPortraits)
			{
				if (this.portraitPerson != null)
				{
					Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(this.xPositionOnScreen - 80 * Game1.pixelZoom), (float)this.yPositionOnScreen), new Rectangle(603, 414, 74, 74), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 0.91f, -1, -1, 0.35f);
					if (this.portraitPerson.Portrait != null)
					{
						b.Draw(this.portraitPerson.Portrait, new Vector2((float)(this.xPositionOnScreen - 80 * Game1.pixelZoom + Game1.pixelZoom * 5), (float)(this.yPositionOnScreen + Game1.pixelZoom * 5)), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.92f);
					}
				}
				if (this.potraitPersonDialogue != null)
				{
					IClickableMenu.drawHoverText(b, this.potraitPersonDialogue, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, this.xPositionOnScreen - (int)Game1.dialogueFont.MeasureString(this.potraitPersonDialogue).X - Game1.tileSize, this.yPositionOnScreen + ((this.portraitPerson != null) ? (78 * Game1.pixelZoom) : 0), 1f, null);
				}
			}
			base.drawMouse(b);
		}

		// Token: 0x04001112 RID: 4370
		public const int howManyRecipesFitOnPage = 28;

		// Token: 0x04001113 RID: 4371
		public const int infiniteStock = 2147483647;

		// Token: 0x04001114 RID: 4372
		public const int salePriceIndex = 0;

		// Token: 0x04001115 RID: 4373
		public const int stockIndex = 1;

		// Token: 0x04001116 RID: 4374
		public const int extraTradeItemIndex = 2;

		// Token: 0x04001117 RID: 4375
		public const int itemsPerPage = 4;

		// Token: 0x04001118 RID: 4376
		public const int numberRequiredForExtraItemTrade = 5;

		// Token: 0x04001119 RID: 4377
		private string descriptionText = "";

		// Token: 0x0400111A RID: 4378
		private string hoverText = "";

		// Token: 0x0400111B RID: 4379
		private string boldTitleText = "";

		// Token: 0x0400111C RID: 4380
		private InventoryMenu inventory;

		// Token: 0x0400111D RID: 4381
		private Item heldItem;

		// Token: 0x0400111E RID: 4382
		private Item hoveredItem;

		// Token: 0x0400111F RID: 4383
		private Texture2D wallpapers;

		// Token: 0x04001120 RID: 4384
		private Texture2D floors;

		// Token: 0x04001121 RID: 4385
		private int lastWallpaperFloorPrice;

		// Token: 0x04001122 RID: 4386
		private TemporaryAnimatedSprite poof;

		// Token: 0x04001123 RID: 4387
		private Rectangle scrollBarRunner;

		// Token: 0x04001124 RID: 4388
		private List<Item> forSale = new List<Item>();

		// Token: 0x04001125 RID: 4389
		private List<ClickableComponent> forSaleButtons = new List<ClickableComponent>();

		// Token: 0x04001126 RID: 4390
		private List<int> categoriesToSellHere = new List<int>();

		// Token: 0x04001127 RID: 4391
		private Dictionary<Item, int[]> itemPriceAndStock = new Dictionary<Item, int[]>();

		// Token: 0x04001128 RID: 4392
		private float sellPercentage = 1f;

		// Token: 0x04001129 RID: 4393
		private List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();

		// Token: 0x0400112A RID: 4394
		private int hoverPrice = -1;

		// Token: 0x0400112B RID: 4395
		private int currency;

		// Token: 0x0400112C RID: 4396
		private int currentItemIndex;

		// Token: 0x0400112D RID: 4397
		private ClickableTextureComponent upArrow;

		// Token: 0x0400112E RID: 4398
		private ClickableTextureComponent downArrow;

		// Token: 0x0400112F RID: 4399
		private ClickableTextureComponent scrollBar;

		// Token: 0x04001130 RID: 4400
		public NPC portraitPerson;

		// Token: 0x04001131 RID: 4401
		public string potraitPersonDialogue;

		// Token: 0x04001132 RID: 4402
		private bool scrolling;
	}
}
