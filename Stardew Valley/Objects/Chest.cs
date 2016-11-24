using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Tools;

namespace StardewValley.Objects
{
	// Token: 0x0200008E RID: 142
	[XmlInclude(typeof(MeleeWeapon))]
	public class Chest : Object
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x000DB294 File Offset: 0x000D9494
		public Chest()
		{
			this.name = "Chest";
			this.type = "interactive";
			this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000DB334 File Offset: 0x000D9534
		public Chest(bool playerChest) : base(Vector2.Zero, 130, false)
		{
			this.Name = "Chest";
			this.type = "Crafting";
			if (playerChest)
			{
				this.playerChest = playerChest;
				this.currentLidFrame = 131;
				this.bigCraftable = true;
				this.canBeSetDown = true;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000DB3CC File Offset: 0x000D95CC
		public Chest(Vector2 location)
		{
			this.tileLocation = location;
			this.name = "Chest";
			this.type = "interactive";
			this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x000DB474 File Offset: 0x000D9674
		public Chest(string type, Vector2 location)
		{
			this.tileLocation = location;
			if (!(type == "OreChest"))
			{
				if (!(type == "dungeon"))
				{
					if (type == "Grand")
					{
						this.tint = new Color(150, 150, 255);
						this.coins = (int)location.Y % 8 + 6;
					}
				}
				else
				{
					switch ((int)location.X % 5)
					{
					case 1:
						this.coins = (int)location.Y % 3 + 2;
						break;
					case 2:
						this.items.Add(new Object(this.tileLocation, 382, (int)location.Y % 3 + 1));
						break;
					case 3:
						this.items.Add(new Object(this.tileLocation, (Game1.mine.getMineArea(-1) == 0) ? 378 : ((Game1.mine.getMineArea(-1) == 40) ? 380 : 384), (int)location.Y % 3 + 1));
						break;
					case 4:
						this.chestType = "Monster";
						break;
					}
				}
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					this.items.Add(new Object(this.tileLocation, (Game1.random.NextDouble() < 0.5) ? 384 : 382, 1));
				}
			}
			this.name = "Chest";
			this.type = "interactive";
			this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x000DB688 File Offset: 0x000D9888
		public Chest(int coins, List<Item> items, Vector2 location, bool giftBox = false)
		{
			this.name = "Chest";
			this.type = "interactive";
			this.giftbox = giftBox;
			if (items != null)
			{
				this.items = items;
			}
			this.coins = coins;
			this.tileLocation = location;
			this.boundingBox = new Rectangle((int)this.tileLocation.X * Game1.tileSize, (int)this.tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool performObjectDropInAction(Object dropIn, bool probe, Farmer who)
		{
			return false;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x000DB748 File Offset: 0x000D9948
		public override bool performToolAction(Tool t)
		{
			if (this.playerChest)
			{
				this.clearNulls();
				if (this.items.Count == 0)
				{
					return base.performToolAction(t);
				}
				if (t != null && t.isHeavyHitter() && !(t is MeleeWeapon))
				{
					Game1.playSound("hammer");
					this.shakeTimer = 100;
				}
			}
			else if (t != null && t is Pickaxe && this.currentLidFrame == 503 && this.frameCounter == -1 && this.items.Count == 0)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x000DB7D0 File Offset: 0x000D99D0
		public void addContents(int coins, Item item)
		{
			this.coins += coins;
			this.items.Add(item);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x000DB7EC File Offset: 0x000D99EC
		public void dumpContents()
		{
			Random r = new Random((int)this.tileLocation.X + (int)this.tileLocation.Y + (int)Game1.uniqueIDForThisGame + ((Game1.mine != null && Game1.currentLocation is MineShaft) ? Game1.mine.mineLevel : 0));
			if (this.coins <= 0 && this.items.Count <= 0)
			{
				if (this.tileLocation.X % 7f == 0f)
				{
					this.chestType = "Monster";
				}
				else
				{
					this.addContents(r.Next(4, Math.Max(8, Game1.mine.mineLevel / 10 - 5)), Utility.getUncommonItemForThisMineLevel(Game1.mine.mineLevel, new Point((int)this.tileLocation.X, (int)this.tileLocation.Y)));
				}
			}
			if (this.items.Count > 0 && !this.chestType.Equals("Monster") && this.items.Count >= 1 && this.opener.IsMainPlayer)
			{
				if (Game1.currentLocation is FarmHouse)
				{
					Game1.player.addQuest(6);
					Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(128, 208, 16, 16), 200f, 2, 30, new Vector2((float)(Game1.dayTimeMoneyBox.questButton.bounds.Left - Game1.tileSize / 4), (float)(Game1.dayTimeMoneyBox.questButton.bounds.Bottom + Game1.pixelZoom * 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true));
				}
				if (this.items[0] is Object && !(this.items[0] as Object).bigCraftable && this.items[0].parentSheetIndex == 434)
				{
					if (!Game1.player.mailReceived.Contains("CF_Mines"))
					{
						Game1.playerEatObject(this.items[0] as Object, true);
						Game1.player.mailReceived.Add("CF_Mines");
					}
					this.items.Clear();
				}
				else
				{
					this.opener.addItemByMenuIfNecessaryElseHoldUp(this.items[0], new ItemGrabMenu.behaviorOnItemSelect(this.itemTakenCallback));
				}
				if (this.opener.currentLocation is MineShaft)
				{
					(this.opener.currentLocation as MineShaft).updateMineLevelData(1, -1);
				}
			}
			if (this.chestType.Equals("Monster"))
			{
				Monster monster = Game1.mine.getMonsterForThisLevel(Game1.mine.mineLevel, (int)this.tileLocation.X, (int)this.tileLocation.Y);
				Vector2 v = Utility.getVelocityTowardPlayer(new Point((int)this.tileLocation.X, (int)this.tileLocation.Y), 8f, this.opener);
				monster.xVelocity = v.X;
				monster.yVelocity = v.Y;
				Game1.currentLocation.characters.Add(monster);
				Game1.playSound("explosion");
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2(this.tileLocation.X * (float)Game1.tileSize, this.tileLocation.Y * (float)Game1.tileSize), false, Game1.random.NextDouble() < 0.5));
				Game1.currentLocation.objects.Remove(this.tileLocation);
				Game1.addHUDMessage(new HUDMessage("Monster in a box!", Color.Red, 3500f));
			}
			else
			{
				this.opener.gainExperience(5, 25 + ((Game1.mine != null && Game1.currentLocation is MineShaft) ? Game1.mine.mineLevel : 0));
			}
			if (this.giftbox)
			{
				Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(0, 348, 16, 19), 80f, 11, 1, this.tileLocation * (float)Game1.tileSize, false, false, this.tileLocation.Y / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
				{
					destroyable = false,
					holdLastFrame = true
				});
				Game1.currentLocation.removeObject(this.tileLocation, false);
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000DBCAE File Offset: 0x000D9EAE
		public void itemTakenCallback(Item item, Farmer who)
		{
			if (item != null && this.items.Count > 0 && item.Equals(this.items[0]))
			{
				this.items.RemoveAt(0);
			}
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x000DBCE4 File Offset: 0x000D9EE4
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			if (this.giftbox)
			{
				this.opener = who;
				Game1.player.Halt();
				Game1.player.freezePause = 1000;
				Game1.playSound("Ship");
				this.dumpContents();
			}
			else if (this.playerChest && this.frameCounter == -1)
			{
				this.opener = who;
				this.frameCounter = 5;
				Game1.playSound(this.fridge ? "doorCreak" : "openChest");
				Game1.player.Halt();
				Game1.player.freezePause = 1000;
			}
			else if (!this.playerChest)
			{
				if (this.currentLidFrame == 501 && this.frameCounter <= -1)
				{
					this.opener = who;
					this.frameCounter = 5;
					Game1.playSound("openChest");
				}
				else if (this.currentLidFrame == 503 && this.items.Count > 0)
				{
					who.addItemByMenuIfNecessaryElseHoldUp(this.items[0], new ItemGrabMenu.behaviorOnItemSelect(this.itemTakenCallback));
					if (this.items.Count > 0 && this.items[0] != null && this.items[0] is Object)
					{
						int arg_15A_0 = (this.items[0] as Object).ParentSheetIndex;
					}
				}
			}
			if (this.items.Count == 0 && this.coins == 0 && !this.playerChest)
			{
				who.currentLocation.removeObject(this.tileLocation, false);
				Game1.playSound("woodWhack");
			}
			return true;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x000DBE88 File Offset: 0x000DA088
		public void grabItemFromChest(Item item, Farmer who)
		{
			if (who.couldInventoryAcceptThisItem(item))
			{
				this.items.Remove(item);
				this.clearNulls();
				Game1.activeClickableMenu = new ItemGrabMenu(this.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromChest), false, true, true, true, true, 1, this, -1, null);
			}
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000DBEF4 File Offset: 0x000DA0F4
		public Item addItem(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] != null && this.items[i].canStackWith(item))
				{
					item.Stack = this.items[i].addToStack(item.Stack);
					if (item.Stack <= 0)
					{
						return null;
					}
				}
			}
			if (this.items.Count < 36)
			{
				this.items.Add(item);
				return null;
			}
			return item;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000DBF80 File Offset: 0x000DA180
		public void grabItemFromInventory(Item item, Farmer who)
		{
			if (item.Stack == 0)
			{
				item.Stack = 1;
			}
			Item tmp = this.addItem(item);
			if (tmp == null)
			{
				who.removeItemFromInventory(item);
			}
			else
			{
				who.addItemToInventory(tmp);
			}
			this.clearNulls();
			Game1.activeClickableMenu = new ItemGrabMenu(this.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromChest), false, true, true, true, true, 1, this, -1, null);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000DC000 File Offset: 0x000DA200
		public bool isEmpty()
		{
			for (int i = this.items.Count<Item>() - 1; i >= 0; i--)
			{
				if (this.items[i] != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000DC038 File Offset: 0x000DA238
		public void clearNulls()
		{
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				if (this.items[i] == null)
				{
					this.items.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000DC078 File Offset: 0x000DA278
		public override void updateWhenCurrentLocation(GameTime time)
		{
			if (this.shakeTimer > 0)
			{
				this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.shakeTimer <= 0)
				{
					this.health = 10;
				}
			}
			if (this.playerChest)
			{
				if (this.frameCounter > -1 && this.currentLidFrame < 136)
				{
					this.frameCounter--;
					if (this.frameCounter <= 0)
					{
						if (this.opener != null)
						{
							if (this.currentLidFrame == 135)
							{
								Game1.activeClickableMenu = new ItemGrabMenu(this.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.grabItemFromChest), false, true, true, true, true, 1, this.fridge ? null : this, -1, null);
								this.frameCounter = -1;
								return;
							}
							this.frameCounter = 5;
							this.currentLidFrame++;
							return;
						}
						else
						{
							this.frameCounter = 5;
							this.currentLidFrame--;
							if (this.currentLidFrame == 131)
							{
								this.frameCounter = -1;
								Game1.playSound("woodyStep");
								return;
							}
						}
					}
				}
				else if (this.frameCounter == -1 && this.currentLidFrame > 131 && Game1.activeClickableMenu == null && this.opener != null)
				{
					this.opener = null;
					this.currentLidFrame = 135;
					this.frameCounter = 2;
					Game1.playSound("doorCreakReverse");
					return;
				}
			}
			else if (this.frameCounter > -1 && this.currentLidFrame < 504)
			{
				this.frameCounter--;
				if (this.frameCounter <= 0)
				{
					if (this.currentLidFrame == 503)
					{
						this.dumpContents();
						this.frameCounter = -1;
						return;
					}
					this.frameCounter = 10;
					this.currentLidFrame++;
					if (this.currentLidFrame == 503)
					{
						this.frameCounter += 5;
					}
				}
			}
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000DC27E File Offset: 0x000DA47E
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x000DC290 File Offset: 0x000DA490
		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
		{
			if (!this.playerChest)
			{
				if (this.giftbox)
				{
					spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 4), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 5f, SpriteEffects.None, 1E-07f);
					if (this.items.Count > 0 || this.coins > 0)
					{
						spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)(y * Game1.tileSize))), new Rectangle?(new Rectangle(0, 348, 16, 19)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
						return;
					}
				}
				else
				{
					spriteBatch.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2((float)(Game1.tileSize / 2 - Game1.tileSize / 4), (float)(Game1.tileSize * 5 / 6)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, new Vector2((float)Game1.shadowTexture.Bounds.Center.X, (float)Game1.shadowTexture.Bounds.Center.Y), 5f, SpriteEffects.None, 1E-07f);
					spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 500, 16, 16)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
					Vector2 lidPosition = new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize));
					switch (this.currentLidFrame)
					{
					case 501:
						lidPosition.Y -= (float)(Game1.tileSize / 2);
						break;
					case 502:
						lidPosition.Y -= 40f;
						break;
					case 503:
						lidPosition.Y -= 60f;
						break;
					}
					spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, lidPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.currentLidFrame, 16, 16)), this.tint, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
				}
				return;
			}
			if (this.playerChoiceColor.Equals(Color.Black))
			{
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, 130, 16, 32)), this.tint * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame, 16, 32)), this.tint * alpha * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
				return;
			}
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y - 1) * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, 168, 16, 32)), this.playerChoiceColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 4) / 10000f);
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize + Game1.pixelZoom * 5))), new Rectangle?(new Rectangle(0, 725, 16, 11)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 6) / 10000f);
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y - 1) * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame + 46, 16, 32)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 6) / 10000f);
			spriteBatch.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y - 1) * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame + 38, 16, 32)), this.playerChoiceColor * alpha * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)(y * Game1.tileSize + 5) / 10000f);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x000DC8F8 File Offset: 0x000DAAF8
		public void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f, bool local = false)
		{
			if (this.playerChest)
			{
				if (this.playerChoiceColor.Equals(Color.Black))
				{
					spriteBatch.Draw(Game1.bigCraftableSpriteSheet, local ? new Vector2((float)x, (float)(y - Game1.tileSize)) : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, 130, 16, 32)), this.tint * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, local ? 0.89f : ((float)(y * Game1.tileSize + 4) / 10000f));
					spriteBatch.Draw(Game1.bigCraftableSpriteSheet, local ? new Vector2((float)x, (float)(y - Game1.tileSize)) : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)), (float)((y - 1) * Game1.tileSize))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame, 16, 32)), this.tint * alpha * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, local ? 0.9f : ((float)(y * Game1.tileSize + 5) / 10000f));
					return;
				}
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, local ? new Vector2((float)x, (float)(y - Game1.tileSize)) : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y - 1) * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, 168, 16, 32)), this.playerChoiceColor * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, local ? 0.9f : ((float)(y * Game1.tileSize + 4) / 10000f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, local ? new Vector2((float)x, (float)(y - Game1.tileSize)) : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y - 1) * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame + 38, 16, 32)), this.playerChoiceColor * alpha * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, local ? 0.9f : ((float)(y * Game1.tileSize + 5) / 10000f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, local ? new Vector2((float)x, (float)(y + Game1.pixelZoom * 5)) : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize + Game1.pixelZoom * 5))), new Rectangle?(new Rectangle(0, 725, 16, 11)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, local ? 0.91f : ((float)(y * Game1.tileSize + 6) / 10000f));
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet, local ? new Vector2((float)x, (float)(y - Game1.tileSize)) : Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(x * Game1.tileSize), (float)((y - 1) * Game1.tileSize + ((this.shakeTimer > 0) ? Game1.random.Next(-1, 2) : 0)))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.currentLidFrame + 46, 16, 32)), Color.White * alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, local ? 0.91f : ((float)(y * Game1.tileSize + 6) / 10000f));
			}
		}

		// Token: 0x04000AD9 RID: 2777
		public const int capacity = 36;

		// Token: 0x04000ADA RID: 2778
		public int currentLidFrame = 501;

		// Token: 0x04000ADB RID: 2779
		public int frameCounter = -1;

		// Token: 0x04000ADC RID: 2780
		public int coins;

		// Token: 0x04000ADD RID: 2781
		public List<Item> items = new List<Item>();

		// Token: 0x04000ADE RID: 2782
		private Farmer opener;

		// Token: 0x04000ADF RID: 2783
		public string chestType = "";

		// Token: 0x04000AE0 RID: 2784
		public Color tint = Color.White;

		// Token: 0x04000AE1 RID: 2785
		public Color playerChoiceColor = Color.Black;

		// Token: 0x04000AE2 RID: 2786
		public bool playerChest;

		// Token: 0x04000AE3 RID: 2787
		public bool fridge;

		// Token: 0x04000AE4 RID: 2788
		public bool giftbox;
	}
}
