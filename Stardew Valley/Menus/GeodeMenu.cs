using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
	// Token: 0x020000F1 RID: 241
	public class GeodeMenu : MenuWithInventory
	{
		// Token: 0x06000E70 RID: 3696 RVA: 0x00126D44 File Offset: 0x00124F44
		public GeodeMenu() : base(null, true, true, Game1.tileSize / 5, Game1.tileSize * 2 + Game1.pixelZoom)
		{
			this.inventory.highlightMethod = new InventoryMenu.highlightThisItem(this.highlightGeodes);
			this.geodeSpot = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 4, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 8, 560, 308), "");
			this.clint = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Clint"), 8, 32, 48);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00126DE7 File Offset: 0x00124FE7
		public override bool readyToClose()
		{
			return base.readyToClose() && this.geodeAnimationTimer <= 0 && this.heldItem == null;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00126E08 File Offset: 0x00125008
		public bool highlightGeodes(Item i)
		{
			if (this.heldItem != null)
			{
				return true;
			}
			int parentSheetIndex = i.parentSheetIndex;
			switch (parentSheetIndex)
			{
			case 535:
			case 536:
			case 537:
				break;
			default:
				if (parentSheetIndex != 749)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00126E4C File Offset: 0x0012504C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			base.receiveLeftClick(x, y, true);
			if (this.geodeSpot.containsPoint(x, y))
			{
				if (this.heldItem != null && this.heldItem.Name.Contains("Geode") && Game1.player.money >= 25 && this.geodeAnimationTimer <= 0)
				{
					if (Game1.player.freeSpotsInInventory() > 1 || (Game1.player.freeSpotsInInventory() == 1 && this.heldItem.Stack == 1))
					{
						this.geodeSpot.item = this.heldItem.getOne();
						Item expr_A7 = this.heldItem;
						int stack = expr_A7.Stack;
						expr_A7.Stack = stack - 1;
						if (this.heldItem.Stack <= 0)
						{
							this.heldItem = null;
						}
						this.geodeAnimationTimer = 2700;
						Game1.player.money -= 25;
						Game1.playSound("stoneStep");
						this.clint.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
						{
							new FarmerSprite.AnimationFrame(8, 300),
							new FarmerSprite.AnimationFrame(9, 200),
							new FarmerSprite.AnimationFrame(10, 80),
							new FarmerSprite.AnimationFrame(11, 200),
							new FarmerSprite.AnimationFrame(12, 100),
							new FarmerSprite.AnimationFrame(8, 300)
						});
						this.clint.loop = false;
						return;
					}
					this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_InventoryFull", new object[0]);
					this.wiggleWordsTimer = 500;
					this.alertTimer = 1500;
					return;
				}
				else if (Game1.player.money < 25)
				{
					this.wiggleWordsTimer = 500;
					Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
				}
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00127027 File Offset: 0x00125227
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			base.receiveRightClick(x, y, true);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00127034 File Offset: 0x00125234
		public override void performHoverAction(int x, int y)
		{
			if (this.alertTimer <= 0)
			{
				base.performHoverAction(x, y);
				if (this.descriptionText.Equals(""))
				{
					if (Game1.player.money < 25)
					{
						this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_Description_NotEnoughMoney", new object[0]);
						return;
					}
					this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_Description", new object[0]);
				}
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x001270A9 File Offset: 0x001252A9
		public override void emergencyShutDown()
		{
			base.emergencyShutDown();
			if (this.heldItem != null)
			{
				Game1.player.addItemToInventoryBool(this.heldItem, false);
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x001270CC File Offset: 0x001252CC
		public override void update(GameTime time)
		{
			base.update(time);
			if (this.alertTimer > 0)
			{
				this.alertTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.geodeAnimationTimer > 0)
			{
				Game1.changeMusicTrack("none");
				this.geodeAnimationTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.geodeAnimationTimer <= 0)
				{
					this.geodeDestructionAnimation = null;
					this.geodeSpot.item = null;
					Game1.player.addItemToInventoryBool(this.geodeTreasure, false);
					this.geodeTreasure = null;
					this.yPositionOfGem = 0;
					return;
				}
				int frame = this.clint.CurrentFrame;
				this.clint.animateOnce(time);
				if (this.clint.CurrentFrame == 11 && frame != 11)
				{
					Stats expr_D1 = Game1.stats;
					uint geodesCracked = expr_D1.GeodesCracked;
					expr_D1.GeodesCracked = geodesCracked + 1u;
					Game1.playSound("hammer");
					Game1.playSound("stoneCrack");
					int geodeDestructionYOffset = 448;
					if (this.geodeSpot.item != null)
					{
						int parentSheetIndex = (this.geodeSpot.item as Object).parentSheetIndex;
						if (parentSheetIndex != 536)
						{
							if (parentSheetIndex == 537)
							{
								geodeDestructionYOffset += Game1.tileSize * 2;
							}
						}
						else
						{
							geodeDestructionYOffset += Game1.tileSize;
						}
						this.geodeDestructionAnimation = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, geodeDestructionYOffset, Game1.tileSize, Game1.tileSize), 100f, 8, 0, new Vector2((float)(this.geodeSpot.bounds.X + 392 - Game1.tileSize / 2), (float)(this.geodeSpot.bounds.Y + 192 - Game1.tileSize / 2)), false, false);
						this.geodeTreasure = Utility.getTreasureFromGeode(this.geodeSpot.item);
						if (this.geodeTreasure.Type.Contains("Mineral"))
						{
							Game1.player.foundMineral(this.geodeTreasure.parentSheetIndex);
						}
						else if (this.geodeTreasure.Type.Contains("Arch") && !Game1.player.hasOrWillReceiveMail("artifactFound"))
						{
							this.geodeTreasure = new Object(390, 5, false, -1, 0);
						}
					}
				}
				if (this.geodeDestructionAnimation != null && this.geodeDestructionAnimation.currentParentTileIndex < 7)
				{
					this.geodeDestructionAnimation.update(time);
					if (this.geodeDestructionAnimation.currentParentTileIndex < 3)
					{
						this.yPositionOfGem--;
					}
					this.yPositionOfGem--;
					if (this.geodeDestructionAnimation.currentParentTileIndex == 7 && this.geodeTreasure.price > 75)
					{
						this.sparkle = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 640, Game1.tileSize, Game1.tileSize), 100f, 8, 0, new Vector2((float)(this.geodeSpot.bounds.X + 392 - Game1.tileSize / 2), (float)(this.geodeSpot.bounds.Y + 192 + this.yPositionOfGem - Game1.tileSize / 2)), false, false);
						Game1.playSound("discoverMineral");
					}
					else if (this.geodeDestructionAnimation.currentParentTileIndex == 7 && this.geodeTreasure.price <= 75)
					{
						Game1.playSound("newArtifact");
					}
				}
				if (this.sparkle != null && this.sparkle.update(time))
				{
					this.sparkle = null;
				}
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00127450 File Offset: 0x00125650
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.geodeSpot = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 4, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 8, 560, 308), "Anvil");
			int yPositionForInventory = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4 + Game1.tileSize * 2 + Game1.pixelZoom;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + Game1.tileSize / 5, yPositionForInventory, false, null, this.inventory.highlightMethod, -1, 3, 0, 0, true);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00127514 File Offset: 0x00125714
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
			base.draw(b, true, true);
			Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
			b.Draw(Game1.mouseCursors, new Vector2((float)this.geodeSpot.bounds.X, (float)this.geodeSpot.bounds.Y), new Rectangle?(new Rectangle(0, 512, 140, 77)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
			if (this.geodeSpot.item != null)
			{
				if (this.geodeDestructionAnimation == null)
				{
					this.geodeSpot.item.drawInMenu(b, new Vector2((float)(this.geodeSpot.bounds.X + 90 * Game1.pixelZoom), (float)(this.geodeSpot.bounds.Y + 40 * Game1.pixelZoom)), 1f);
				}
				else
				{
					this.geodeDestructionAnimation.draw(b, true, 0, 0);
				}
				if (this.geodeTreasure != null)
				{
					this.geodeTreasure.drawInMenu(b, new Vector2((float)(this.geodeSpot.bounds.X + 90 * Game1.pixelZoom), (float)(this.geodeSpot.bounds.Y + 40 * Game1.pixelZoom + this.yPositionOfGem)), 1f);
				}
				if (this.sparkle != null)
				{
					this.sparkle.draw(b, true, 0, 0);
				}
			}
			this.clint.draw(b, new Vector2((float)(this.geodeSpot.bounds.X + 96 * Game1.pixelZoom), (float)(this.geodeSpot.bounds.Y + 16 * Game1.pixelZoom)), 0.877f);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 8), (float)(Game1.getOldMouseY() + 8)), 1f);
			}
			if (!Game1.options.hardwareCursor)
			{
				base.drawMouse(b);
			}
		}

		// Token: 0x04000F81 RID: 3969
		public ClickableComponent geodeSpot;

		// Token: 0x04000F82 RID: 3970
		public AnimatedSprite clint;

		// Token: 0x04000F83 RID: 3971
		public TemporaryAnimatedSprite geodeDestructionAnimation;

		// Token: 0x04000F84 RID: 3972
		public TemporaryAnimatedSprite sparkle;

		// Token: 0x04000F85 RID: 3973
		public int geodeAnimationTimer;

		// Token: 0x04000F86 RID: 3974
		public int yPositionOfGem;

		// Token: 0x04000F87 RID: 3975
		public int alertTimer;

		// Token: 0x04000F88 RID: 3976
		public Object geodeTreasure;
	}
}
