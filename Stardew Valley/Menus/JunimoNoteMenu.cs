using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;

namespace StardewValley.Menus
{
	// Token: 0x020000F7 RID: 247
	public class JunimoNoteMenu : IClickableMenu
	{
		// Token: 0x06000EDB RID: 3803 RVA: 0x0012FC4C File Offset: 0x0012DE4C
		public JunimoNoteMenu(bool fromGameMenu, int area = 1, bool fromThisMenu = false) : base(Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2, Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			CommunityCenter cc = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
			if (fromGameMenu && !fromThisMenu)
			{
				for (int i = 0; i < cc.areasComplete.Length; i++)
				{
					if (cc.shouldNoteAppearInArea(i) && !cc.areasComplete[i])
					{
						area = i;
						this.whichArea = area;
						break;
					}
				}
			}
			this.setUpMenu(area, cc.bundles);
			Game1.player.forceCanMove();
			this.areaNextButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize * 2, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				visible = false
			};
			this.areaBackButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
			{
				visible = false
			};
			for (int j = 0; j < 6; j++)
			{
				if (cc.shouldNoteAppearInArea((area + j) % 6))
				{
					this.areaNextButton.visible = true;
				}
			}
			for (int k = 0; k < 6; k++)
			{
				int a = area - k;
				if (a == -1)
				{
					a = 5;
				}
				if (cc.shouldNoteAppearInArea(a))
				{
					this.areaBackButton.visible = true;
				}
			}
			this.fromGameMenu = fromGameMenu;
			using (List<Bundle>.Enumerator enumerator = this.bundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.depositsAllowed = false;
				}
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0012FE90 File Offset: 0x0012E090
		public JunimoNoteMenu(int whichArea, Dictionary<int, bool[]> bundlesComplete) : base(Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2, Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
		{
			this.setUpMenu(whichArea, bundlesComplete);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0012FF24 File Offset: 0x0012E124
		public void setUpMenu(int whichArea, Dictionary<int, bool[]> bundlesComplete)
		{
			if (Game1.temporaryContent == null)
			{
				Game1.temporaryContent = Game1.content.CreateTemporary();
			}
			this.noteTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\JunimoNote");
			if (!Game1.player.hasOrWillReceiveMail("seenJunimoNote"))
			{
				Game1.player.removeQuest(26);
				Game1.player.mailReceived.Add("seenJunimoNote");
			}
			if (!Game1.player.hasOrWillReceiveMail("wizardJunimoNote"))
			{
				Game1.addMailForTomorrow("wizardJunimoNote", false, false);
			}
			this.scrambledText = !Game1.player.hasOrWillReceiveMail("canReadJunimoText");
			JunimoNoteMenu.tempSprites.Clear();
			this.whichArea = whichArea;
			this.inventory = new InventoryMenu(this.xPositionOnScreen + 32 * Game1.pixelZoom, this.yPositionOnScreen + 35 * Game1.pixelZoom, true, null, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), 36, 6, Game1.pixelZoom * 2, 2 * Game1.pixelZoom, false)
			{
				capacity = 36
			};
			Dictionary<string, string> bundlesInfo = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
			string areaName = CommunityCenter.getAreaNameFromNumber(whichArea);
			int bundlesAdded = 0;
			foreach (string i in bundlesInfo.Keys)
			{
				if (i.Contains(areaName))
				{
					int bundleIndex = Convert.ToInt32(i.Split(new char[]
					{
						'/'
					})[1]);
					this.bundles.Add(new Bundle(bundleIndex, bundlesInfo[i], bundlesComplete[bundleIndex], this.getBundleLocationFromNumber(bundlesAdded), this.noteTexture, this));
					bundlesAdded++;
				}
			}
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
			this.checkForRewards();
			JunimoNoteMenu.canClick = true;
			Game1.playSound("shwip");
			bool isOneIncomplete = false;
			foreach (Bundle b in this.bundles)
			{
				if (!b.complete && !b.Equals(this.currentPageBundle))
				{
					isOneIncomplete = true;
					break;
				}
			}
			if (!isOneIncomplete)
			{
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[whichArea] = true;
				this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(whichArea);
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x001301F0 File Offset: 0x0012E3F0
		public override bool readyToClose()
		{
			return this.heldItem == null;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x001301FC File Offset: 0x0012E3FC
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!JunimoNoteMenu.canClick)
			{
				return;
			}
			base.receiveLeftClick(x, y, playSound);
			if (this.scrambledText)
			{
				return;
			}
			if (this.specificBundlePage)
			{
				this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
				if (this.backButton.containsPoint(x, y) && this.heldItem == null)
				{
					this.takeDownBundleSpecificPage(this.currentPageBundle);
					Game1.playSound("shwip");
				}
				if (this.heldItem != null)
				{
					if (Game1.oldKBState.IsKeyDown(Keys.LeftShift))
					{
						for (int i = 0; i < this.ingredientSlots.Count; i++)
						{
							if (this.ingredientSlots[i].item == null)
							{
								this.heldItem = this.currentPageBundle.tryToDepositThisItem(this.heldItem, this.ingredientSlots[i], this.noteTexture);
								this.checkIfBundleIsComplete();
								return;
							}
						}
					}
					for (int j = 0; j < this.ingredientSlots.Count; j++)
					{
						if (this.ingredientSlots[j].containsPoint(x, y))
						{
							this.heldItem = this.currentPageBundle.tryToDepositThisItem(this.heldItem, this.ingredientSlots[j], this.noteTexture);
							this.checkIfBundleIsComplete();
						}
					}
				}
				if (this.purchaseButton != null && this.purchaseButton.containsPoint(x, y))
				{
					int moneyRequired = this.currentPageBundle.ingredients.Last<BundleIngredientDescription>().stack;
					if (Game1.player.Money >= moneyRequired)
					{
						Game1.player.Money -= moneyRequired;
						Game1.playSound("select");
						this.currentPageBundle.completionAnimation(this, true, 0);
						if (this.purchaseButton != null)
						{
							this.purchaseButton.scale = this.purchaseButton.baseScale * 0.75f;
						}
						((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[this.currentPageBundle.bundleIndex] = true;
						(Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[this.currentPageBundle.bundleIndex][0] = true;
						this.checkForRewards();
						bool isOneIncomplete = false;
						foreach (Bundle b in this.bundles)
						{
							if (!b.complete && !b.Equals(this.currentPageBundle))
							{
								isOneIncomplete = true;
								break;
							}
						}
						if (!isOneIncomplete)
						{
							((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[this.whichArea] = true;
							this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
							((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(this.whichArea);
						}
						else
						{
							Junimo k = ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).getJunimoForArea(this.whichArea);
							if (k != null)
							{
								k.bringBundleBackToHut(Bundle.getColorFromColorIndex(this.currentPageBundle.bundleColor), Game1.getLocationFromName("CommunityCenter"));
							}
						}
					}
					else
					{
						Game1.dayTimeMoneyBox.moneyShakeTimer = 600;
					}
				}
			}
			else
			{
				foreach (Bundle b2 in this.bundles)
				{
					if (b2.canBeClicked() && b2.containsPoint(x, y))
					{
						this.setUpBundleSpecificPage(b2);
						Game1.playSound("shwip");
						return;
					}
				}
				if (this.presentButton != null && this.presentButton.containsPoint(x, y))
				{
					this.openRewardsMenu();
				}
				if (this.fromGameMenu)
				{
					CommunityCenter cc = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
					if (this.areaNextButton.containsPoint(x, y))
					{
						for (int l = 1; l < 7; l++)
						{
							if (cc.shouldNoteAppearInArea((this.whichArea + l) % 6))
							{
								Game1.activeClickableMenu = new JunimoNoteMenu(true, (this.whichArea + l) % 6, true);
								return;
							}
						}
					}
					else if (this.areaBackButton.containsPoint(x, y))
					{
						int area = this.whichArea;
						for (int m = 1; m < 7; m++)
						{
							area--;
							if (area == -1)
							{
								area = 5;
							}
							if (cc.shouldNoteAppearInArea(area))
							{
								Game1.activeClickableMenu = new JunimoNoteMenu(true, area, true);
								return;
							}
						}
					}
				}
			}
			if (this.heldItem != null && !this.isWithinBounds(x, y) && this.heldItem.canBeTrashed())
			{
				Game1.playSound("throwDownITem");
				Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, null);
				this.heldItem = null;
			}
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x001306C0 File Offset: 0x0012E8C0
		public override void receiveGamePadButton(Buttons b)
		{
			base.receiveGamePadButton(b);
			if (this.fromGameMenu)
			{
				CommunityCenter cc = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
				if (b == Buttons.RightTrigger)
				{
					for (int i = 1; i < 7; i++)
					{
						if (cc.shouldNoteAppearInArea((this.whichArea + i) % 6))
						{
							Game1.activeClickableMenu = new JunimoNoteMenu(true, (this.whichArea + i) % 6, true);
							return;
						}
					}
					return;
				}
				if (b == Buttons.LeftTrigger)
				{
					int area = this.whichArea;
					for (int j = 1; j < 7; j++)
					{
						area--;
						if (area == -1)
						{
							area = 5;
						}
						if (cc.shouldNoteAppearInArea(area))
						{
							Game1.activeClickableMenu = new JunimoNoteMenu(true, area, true);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0013076C File Offset: 0x0012E96C
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (key.Equals(Keys.Delete) && this.heldItem != null && this.heldItem.canBeTrashed())
			{
				if (this.heldItem is Object && Game1.player.specialItems.Contains((this.heldItem as Object).parentSheetIndex))
				{
					Game1.player.specialItems.Remove((this.heldItem as Object).parentSheetIndex);
				}
				this.heldItem = null;
				Game1.playSound("trashcan");
			}
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0013080A File Offset: 0x0012EA0A
		private void reOpenThisMenu()
		{
			Game1.activeClickableMenu = new JunimoNoteMenu(this.whichArea, ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00130830 File Offset: 0x0012EA30
		private void updateIngredientSlots()
		{
			int slotNumber = 0;
			for (int i = 0; i < this.currentPageBundle.ingredients.Count; i++)
			{
				if (this.currentPageBundle.ingredients[i].completed)
				{
					this.ingredientSlots[slotNumber].item = new Object(this.currentPageBundle.ingredients[i].index, this.currentPageBundle.ingredients[i].stack, false, -1, this.currentPageBundle.ingredients[i].quality);
					this.currentPageBundle.ingredientDepositAnimation(this.ingredientSlots[slotNumber], this.noteTexture, true);
					slotNumber++;
				}
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x001308F4 File Offset: 0x0012EAF4
		private void openRewardsMenu()
		{
			Game1.playSound("smallSelect");
			List<Item> rewards = new List<Item>();
			Dictionary<string, string> bundlesInfo = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
			foreach (string i in bundlesInfo.Keys)
			{
				if (i.Contains(CommunityCenter.getAreaNameFromNumber(this.whichArea)))
				{
					int bundleIndex = Convert.ToInt32(i.Split(new char[]
					{
						'/'
					})[1]);
					if (((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[bundleIndex])
					{
						Item j = Utility.getItemFromStandardTextDescription(bundlesInfo[i].Split(new char[]
						{
							'/'
						})[1], Game1.player, ' ');
						j.specialVariable = bundleIndex;
						rewards.Add(j);
					}
				}
			}
			Game1.activeClickableMenu = new ItemGrabMenu(rewards, false, true, null, null, null, new ItemGrabMenu.behaviorOnItemSelect(this.rewardGrabbed), false, false, true, true, false, 0, null, -1, null);
			Game1.activeClickableMenu.exitFunction = ((this.exitFunction != null) ? this.exitFunction : new IClickableMenu.onExit(this.reOpenThisMenu));
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00130A34 File Offset: 0x0012EC34
		private void rewardGrabbed(Item item, Farmer who)
		{
			((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[item.specialVariable] = false;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00130A58 File Offset: 0x0012EC58
		private void checkIfBundleIsComplete()
		{
			if (!this.specificBundlePage || this.currentPageBundle == null)
			{
				return;
			}
			int numberOfFilledSlots = 0;
			using (List<ClickableTextureComponent>.Enumerator enumerator = this.ingredientSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item != null)
					{
						numberOfFilledSlots++;
					}
				}
			}
			if (numberOfFilledSlots >= this.currentPageBundle.numberOfIngredientSlots)
			{
				if (this.heldItem != null)
				{
					Game1.player.addItemToInventory(this.heldItem);
					this.heldItem = null;
				}
				for (int i = 0; i < ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles[this.currentPageBundle.bundleIndex].Length; i++)
				{
					((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundles[this.currentPageBundle.bundleIndex][i] = true;
				}
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).checkForNewJunimoNotes();
				JunimoNoteMenu.screenSwipe = new ScreenSwipe(0, -1f, -1);
				this.currentPageBundle.completionAnimation(this, true, 400);
				JunimoNoteMenu.canClick = false;
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[this.currentPageBundle.bundleIndex] = true;
				bool isOneIncomplete = false;
				foreach (Bundle b in this.bundles)
				{
					if (!b.complete && !b.Equals(this.currentPageBundle))
					{
						isOneIncomplete = true;
						break;
					}
				}
				if (!isOneIncomplete)
				{
					((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areasComplete[this.whichArea] = true;
					this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
					((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(this.whichArea);
				}
				else
				{
					Junimo j = ((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).getJunimoForArea(this.whichArea);
					if (j != null)
					{
						j.bringBundleBackToHut(Bundle.getColorFromColorIndex(this.currentPageBundle.bundleColor), Game1.getLocationFromName("CommunityCenter"));
					}
				}
				this.checkForRewards();
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.sendMessageToEveryone(6, string.Concat(this.currentPageBundle.bundleIndex), Game1.player.uniqueMultiplayerID);
				}
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00130CC8 File Offset: 0x0012EEC8
		private void restoreAreaOnExit()
		{
			if (!this.fromGameMenu)
			{
				((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).restoreAreaCutscene(this.whichArea);
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00130CEC File Offset: 0x0012EEEC
		public void checkForRewards()
		{
			foreach (string i in Game1.content.Load<Dictionary<string, string>>("Data\\Bundles").Keys)
			{
				if (i.Contains(CommunityCenter.getAreaNameFromNumber(this.whichArea)))
				{
					int bundleIndex = Convert.ToInt32(i.Split(new char[]
					{
						'/'
					})[1]);
					if (((CommunityCenter)Game1.getLocationFromName("CommunityCenter")).bundleRewards[bundleIndex])
					{
						this.presentButton = new ClickableAnimatedComponent(new Rectangle(this.xPositionOnScreen + 148 * Game1.pixelZoom, this.yPositionOnScreen + 128 * Game1.pixelZoom, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom), "", "Rewards", new TemporaryAnimatedSprite(this.noteTexture, new Rectangle(548, 262, 18, 20), 70f, 4, 99999, new Vector2((float)(-(float)Game1.tileSize), (float)(-(float)Game1.tileSize)), false, false, 0.5f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true));
						break;
					}
				}
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00130E5C File Offset: 0x0012F05C
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			if (!JunimoNoteMenu.canClick)
			{
				return;
			}
			if (this.specificBundlePage)
			{
				this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
			}
			if (!this.specificBundlePage && this.readyToClose())
			{
				base.exitThisMenu(true);
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00130EAC File Offset: 0x0012F0AC
		public override void update(GameTime time)
		{
			using (List<Bundle>.Enumerator enumerator = this.bundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.update(time);
				}
			}
			for (int i = JunimoNoteMenu.tempSprites.Count - 1; i >= 0; i--)
			{
				if (JunimoNoteMenu.tempSprites[i].update(time))
				{
					JunimoNoteMenu.tempSprites.RemoveAt(i);
				}
			}
			if (this.presentButton != null)
			{
				this.presentButton.update(time);
			}
			if (JunimoNoteMenu.screenSwipe != null)
			{
				JunimoNoteMenu.canClick = false;
				if (JunimoNoteMenu.screenSwipe.update(time))
				{
					JunimoNoteMenu.screenSwipe = null;
					JunimoNoteMenu.canClick = true;
				}
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00130F6C File Offset: 0x0012F16C
		public override void performHoverAction(int x, int y)
		{
			base.performHoverAction(x, y);
			if (this.scrambledText)
			{
				return;
			}
			JunimoNoteMenu.hoverText = "";
			if (this.specificBundlePage)
			{
				this.backButton.tryHover(x, y, 0.1f);
				this.hoveredItem = this.inventory.hover(x, y, this.heldItem);
				foreach (ClickableTextureComponent c in this.ingredientList)
				{
					if (c.bounds.Contains(x, y))
					{
						JunimoNoteMenu.hoverText = c.hoverText;
						break;
					}
				}
				if (this.heldItem != null)
				{
					foreach (ClickableTextureComponent c2 in this.ingredientSlots)
					{
						if (c2.bounds.Contains(x, y) && this.currentPageBundle.canAcceptThisItem(this.heldItem, c2))
						{
							c2.sourceRect.X = 530;
							c2.sourceRect.Y = 262;
						}
						else
						{
							c2.sourceRect.X = 512;
							c2.sourceRect.Y = 244;
						}
					}
				}
				if (this.purchaseButton != null)
				{
					this.purchaseButton.tryHover(x, y, 0.1f);
					return;
				}
			}
			else
			{
				if (this.presentButton != null)
				{
					JunimoNoteMenu.hoverText = this.presentButton.tryHover(x, y);
				}
				using (List<Bundle>.Enumerator enumerator2 = this.bundles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.tryHoverAction(x, y);
					}
				}
				if (this.fromGameMenu)
				{
					Game1.getLocationFromName("CommunityCenter");
					this.areaNextButton.tryHover(x, y, 0.1f);
					this.areaBackButton.tryHover(x, y, 0.1f);
				}
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00131180 File Offset: 0x0012F380
		public override void draw(SpriteBatch b)
		{
			b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
			if (!this.specificBundlePage)
			{
				b.Draw(this.noteTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(new Rectangle(0, 0, 320, 180)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
				SpriteText.drawStringHorizontallyCenteredAt(b, CommunityCenter.getAreaDisplayNameFromNumber(this.whichArea), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + Game1.pixelZoom * 3, 999999, -1, 99999, 0.88f, 0.88f, this.scrambledText, -1);
				if (this.scrambledText)
				{
					SpriteText.drawString(b, "We    the  Junimo are    happy to    aid  you.  In     return we     ask for gifts   of      the valley.    If you   are one     with    forest   magic    then you    will   see    the true    nature   of  this scroll.", this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen + Game1.tileSize * 3 / 2, 999999, this.width - Game1.tileSize * 3, 99999, 0.88f, 0.88f, true, -1, "", -1);
					base.draw(b);
					if (JunimoNoteMenu.canClick)
					{
						base.drawMouse(b);
					}
					return;
				}
				using (List<Bundle>.Enumerator enumerator = this.bundles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b);
					}
				}
				if (this.presentButton != null)
				{
					this.presentButton.draw(b);
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = JunimoNoteMenu.tempSprites.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b, true, 0, 0);
					}
				}
				if (this.fromGameMenu)
				{
					if (this.areaNextButton.visible)
					{
						this.areaNextButton.draw(b);
					}
					if (this.areaBackButton.visible)
					{
						this.areaBackButton.draw(b);
					}
				}
			}
			else
			{
				b.Draw(this.noteTexture, new Vector2((float)this.xPositionOnScreen, (float)this.yPositionOnScreen), new Rectangle?(new Rectangle(320, 0, 320, 180)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
				if (this.currentPageBundle != null)
				{
					b.Draw(this.noteTexture, new Vector2((float)(this.xPositionOnScreen + 218 * Game1.pixelZoom), (float)(this.yPositionOnScreen + 22 * Game1.pixelZoom)), new Rectangle?(new Rectangle(this.currentPageBundle.bundleIndex * 16 * 2 % this.noteTexture.Width, 180 + 32 * (this.currentPageBundle.bundleIndex * 16 * 2 / this.noteTexture.Width), 32, 32)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.15f);
					float textX = Game1.dialogueFont.MeasureString((!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					})).X;
					b.Draw(this.noteTexture, new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom - (int)textX / 2 - Game1.pixelZoom * 4), (float)(this.yPositionOnScreen + 57 * Game1.pixelZoom)), new Rectangle?(new Rectangle(517, 266, 4, 17)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
					b.Draw(this.noteTexture, new Rectangle(this.xPositionOnScreen + 234 * Game1.pixelZoom - (int)textX / 2, this.yPositionOnScreen + 57 * Game1.pixelZoom, (int)textX, 17 * Game1.pixelZoom), new Rectangle?(new Rectangle(520, 266, 1, 17)), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
					b.Draw(this.noteTexture, new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom + (int)textX / 2), (float)(this.yPositionOnScreen + 57 * Game1.pixelZoom)), new Rectangle?(new Rectangle(524, 266, 4, 17)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
					b.DrawString(Game1.dialogueFont, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText")) ? "???" : Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", new object[]
					{
						this.currentPageBundle.label
					}), new Vector2((float)(this.xPositionOnScreen + 234 * Game1.pixelZoom) - textX / 2f, (float)this.yPositionOnScreen + 59.5f * (float)Game1.pixelZoom), Game1.textColor);
				}
				this.backButton.draw(b);
				if (this.purchaseButton != null)
				{
					this.purchaseButton.draw(b);
					Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
				}
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator2 = JunimoNoteMenu.tempSprites.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.draw(b, true, 0, 0);
					}
				}
				foreach (ClickableTextureComponent c in this.ingredientSlots)
				{
					if (c.item == null)
					{
						c.draw(b, this.fromGameMenu ? (Color.LightGray * 0.5f) : Color.White, 0.89f);
					}
					c.drawItem(b, Game1.pixelZoom, Game1.pixelZoom);
				}
				foreach (ClickableTextureComponent c2 in this.ingredientList)
				{
					b.Draw(Game1.shadowTexture, new Vector2((float)(c2.bounds.Center.X - Game1.shadowTexture.Bounds.Width * Game1.pixelZoom / 2 - Game1.pixelZoom), (float)(c2.bounds.Center.Y + Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.1f);
					c2.drawItem(b, 0, 0);
				}
				this.inventory.draw(b);
			}
			SpriteText.drawStringWithScrollCenteredAt(b, this.getRewardNameForArea(this.whichArea), this.xPositionOnScreen + this.width / 2, Math.Min(this.yPositionOnScreen + this.height + Game1.pixelZoom * 5, Game1.viewport.Height - Game1.tileSize - Game1.pixelZoom * 2), "", 1f, -1, 0, 0.88f, false);
			base.draw(b);
			if (JunimoNoteMenu.canClick)
			{
				base.drawMouse(b);
			}
			if (this.heldItem != null)
			{
				this.heldItem.drawInMenu(b, new Vector2((float)(Game1.getOldMouseX() + 16), (float)(Game1.getOldMouseY() + 16)), 1f);
			}
			if (this.inventory.descriptionText.Length > 0)
			{
				if (this.hoveredItem != null)
				{
					IClickableMenu.drawToolTip(b, this.hoveredItem.getDescription(), this.hoveredItem.Name, this.hoveredItem, false, -1, 0, -1, -1, null, -1);
				}
			}
			else
			{
				IClickableMenu.drawHoverText(b, (!Game1.player.hasOrWillReceiveMail("canReadJunimoText") && JunimoNoteMenu.hoverText.Length > 0) ? "???" : JunimoNoteMenu.hoverText, Game1.dialogueFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
			if (JunimoNoteMenu.screenSwipe != null)
			{
				JunimoNoteMenu.screenSwipe.draw(b);
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x001319E0 File Offset: 0x0012FBE0
		public string getRewardNameForArea(int whichArea)
		{
			switch (whichArea)
			{
			case 0:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardPantry", new object[0]);
			case 1:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardCrafts", new object[0]);
			case 2:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardFishTank", new object[0]);
			case 3:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardBoiler", new object[0]);
			case 4:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardVault", new object[0]);
			case 5:
				return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardBulletin", new object[0]);
			default:
				return "???";
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00131A9C File Offset: 0x0012FC9C
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
			this.xPositionOnScreen = Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2;
			this.yPositionOnScreen = Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2;
			this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
			if (this.fromGameMenu)
			{
				this.areaNextButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize * 2, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false)
				{
					visible = false
				};
				this.areaBackButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false)
				{
					visible = false
				};
			}
			this.inventory = new InventoryMenu(this.xPositionOnScreen + 32 * Game1.pixelZoom, this.yPositionOnScreen + 35 * Game1.pixelZoom, true, null, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), Game1.player.maxItems, 6, Game1.pixelZoom * 2, 2 * Game1.pixelZoom, false);
			for (int i = 0; i < this.bundles.Count; i++)
			{
				Point p = this.getBundleLocationFromNumber(i);
				this.bundles[i].bounds.X = p.X;
				this.bundles[i].bounds.Y = p.Y;
				this.bundles[i].sprite.position = new Vector2((float)p.X, (float)p.Y);
			}
			if (this.specificBundlePage)
			{
				int numberOfIngredientSlots = this.currentPageBundle.numberOfIngredientSlots;
				List<Rectangle> ingredientSlotRectangles = new List<Rectangle>();
				this.addRectangleRowsToList(ingredientSlotRectangles, numberOfIngredientSlots, 233 * Game1.pixelZoom, 135 * Game1.pixelZoom);
				this.ingredientSlots.Clear();
				for (int j = 0; j < ingredientSlotRectangles.Count; j++)
				{
					this.ingredientSlots.Add(new ClickableTextureComponent(ingredientSlotRectangles[j], this.noteTexture, new Rectangle(512, 244, 18, 18), (float)Game1.pixelZoom, false));
				}
				List<Rectangle> ingredientListRectangles = new List<Rectangle>();
				this.ingredientList.Clear();
				this.addRectangleRowsToList(ingredientListRectangles, this.currentPageBundle.ingredients.Count, 233 * Game1.pixelZoom, 91 * Game1.pixelZoom);
				for (int k = 0; k < ingredientListRectangles.Count; k++)
				{
					if (Game1.objectInformation.ContainsKey(this.currentPageBundle.ingredients[k].index))
					{
						this.ingredientList.Add(new ClickableTextureComponent("", ingredientListRectangles[k], "", Game1.objectInformation[this.currentPageBundle.ingredients[k].index].Split(new char[]
						{
							'/'
						})[0], Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.currentPageBundle.ingredients[k].index, 16, 16), (float)Game1.pixelZoom, false)
						{
							item = new Object(this.currentPageBundle.ingredients[k].index, this.currentPageBundle.ingredients[k].stack, false, -1, this.currentPageBundle.ingredients[k].quality)
						});
					}
				}
				this.updateIngredientSlots();
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00131EDC File Offset: 0x001300DC
		private void setUpBundleSpecificPage(Bundle b)
		{
			JunimoNoteMenu.tempSprites.Clear();
			this.currentPageBundle = b;
			this.specificBundlePage = true;
			if (this.whichArea == 4)
			{
				if (!this.fromGameMenu)
				{
					this.purchaseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 200 * Game1.pixelZoom, this.yPositionOnScreen + 126 * Game1.pixelZoom, 65 * Game1.pixelZoom, 18 * Game1.pixelZoom), this.noteTexture, new Rectangle(517, 286, 65, 20), (float)Game1.pixelZoom, false);
					return;
				}
			}
			else
			{
				int numberOfIngredientSlots = b.numberOfIngredientSlots;
				List<Rectangle> ingredientSlotRectangles = new List<Rectangle>();
				this.addRectangleRowsToList(ingredientSlotRectangles, numberOfIngredientSlots, 233 * Game1.pixelZoom, 135 * Game1.pixelZoom);
				for (int i = 0; i < ingredientSlotRectangles.Count; i++)
				{
					this.ingredientSlots.Add(new ClickableTextureComponent(ingredientSlotRectangles[i], this.noteTexture, new Rectangle(512, 244, 18, 18), (float)Game1.pixelZoom, false));
				}
				List<Rectangle> ingredientListRectangles = new List<Rectangle>();
				this.addRectangleRowsToList(ingredientListRectangles, b.ingredients.Count, 233 * Game1.pixelZoom, 91 * Game1.pixelZoom);
				for (int j = 0; j < ingredientListRectangles.Count; j++)
				{
					if (Game1.objectInformation.ContainsKey(b.ingredients[j].index))
					{
						this.ingredientList.Add(new ClickableTextureComponent("", ingredientListRectangles[j], "", Game1.objectInformation[b.ingredients[j].index].Split(new char[]
						{
							'/'
						})[0], Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, b.ingredients[j].index, 16, 16), (float)Game1.pixelZoom, false)
						{
							item = new Object(b.ingredients[j].index, b.ingredients[j].stack, false, -1, b.ingredients[j].quality)
						});
					}
				}
				this.updateIngredientSlots();
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00132118 File Offset: 0x00130318
		private void addRectangleRowsToList(List<Rectangle> toAddTo, int numberOfItems, int centerX, int centerY)
		{
			switch (numberOfItems)
			{
			case 1:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 1, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 2:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 2, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 3:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 4:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 5:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 2, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 6:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 7:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 8:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 9:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 10:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 11:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			case 12:
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				toAddTo.AddRange(this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
				return;
			default:
				return;
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00132620 File Offset: 0x00130820
		private List<Rectangle> createRowOfBoxesCenteredAt(int xStart, int yStart, int numBoxes, int boxWidth, int boxHeight, int horizontalGap)
		{
			List<Rectangle> rectangles = new List<Rectangle>();
			int actualXStart = xStart - numBoxes * (boxWidth + horizontalGap) / 2;
			int actualYStart = yStart - boxHeight / 2;
			for (int i = 0; i < numBoxes; i++)
			{
				rectangles.Add(new Rectangle(actualXStart + i * (boxWidth + horizontalGap), actualYStart, boxWidth, boxHeight));
			}
			return rectangles;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0013266C File Offset: 0x0013086C
		public void takeDownBundleSpecificPage(Bundle b = null)
		{
			if (!this.specificBundlePage)
			{
				return;
			}
			if (b == null)
			{
				b = this.currentPageBundle;
			}
			this.specificBundlePage = false;
			this.ingredientSlots.Clear();
			this.ingredientList.Clear();
			JunimoNoteMenu.tempSprites.Clear();
			this.purchaseButton = null;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x001326BC File Offset: 0x001308BC
		private Point getBundleLocationFromNumber(int whichBundle)
		{
			Point location = new Point(this.xPositionOnScreen, this.yPositionOnScreen);
			switch (whichBundle)
			{
			case 0:
				location.X += 148 * Game1.pixelZoom;
				location.Y += 34 * Game1.pixelZoom;
				break;
			case 1:
				location.X += 98 * Game1.pixelZoom;
				location.Y += 96 * Game1.pixelZoom;
				break;
			case 2:
				location.X += 196 * Game1.pixelZoom;
				location.Y += 97 * Game1.pixelZoom;
				break;
			case 3:
				location.X += 76 * Game1.pixelZoom;
				location.Y += 63 * Game1.pixelZoom;
				break;
			case 4:
				location.X += 223 * Game1.pixelZoom;
				location.Y += 63 * Game1.pixelZoom;
				break;
			case 5:
				location.X += 147 * Game1.pixelZoom;
				location.Y += 69 * Game1.pixelZoom;
				break;
			case 6:
				location.X += 147 * Game1.pixelZoom;
				location.Y += 95 * Game1.pixelZoom;
				break;
			case 7:
				location.X += 110 * Game1.pixelZoom;
				location.Y += 41 * Game1.pixelZoom;
				break;
			case 8:
				location.X += 194 * Game1.pixelZoom;
				location.Y += 41 * Game1.pixelZoom;
				break;
			}
			return location;
		}

		// Token: 0x04000FED RID: 4077
		public Texture2D noteTexture;

		// Token: 0x04000FEE RID: 4078
		private bool specificBundlePage;

		// Token: 0x04000FEF RID: 4079
		public const int baseWidth = 320;

		// Token: 0x04000FF0 RID: 4080
		public const int baseHeight = 180;

		// Token: 0x04000FF1 RID: 4081
		private InventoryMenu inventory;

		// Token: 0x04000FF2 RID: 4082
		private Item heldItem;

		// Token: 0x04000FF3 RID: 4083
		private Item hoveredItem;

		// Token: 0x04000FF4 RID: 4084
		public static bool canClick = true;

		// Token: 0x04000FF5 RID: 4085
		private int whichArea;

		// Token: 0x04000FF6 RID: 4086
		public static ScreenSwipe screenSwipe;

		// Token: 0x04000FF7 RID: 4087
		public static string hoverText = "";

		// Token: 0x04000FF8 RID: 4088
		private List<Bundle> bundles = new List<Bundle>();

		// Token: 0x04000FF9 RID: 4089
		public static List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();

		// Token: 0x04000FFA RID: 4090
		public List<ClickableTextureComponent> ingredientSlots = new List<ClickableTextureComponent>();

		// Token: 0x04000FFB RID: 4091
		public List<ClickableTextureComponent> ingredientList = new List<ClickableTextureComponent>();

		// Token: 0x04000FFC RID: 4092
		public List<ClickableTextureComponent> otherClickableComponents = new List<ClickableTextureComponent>();

		// Token: 0x04000FFD RID: 4093
		public bool fromGameMenu;

		// Token: 0x04000FFE RID: 4094
		public bool scrambledText;

		// Token: 0x04000FFF RID: 4095
		private ClickableTextureComponent backButton;

		// Token: 0x04001000 RID: 4096
		private ClickableTextureComponent purchaseButton;

		// Token: 0x04001001 RID: 4097
		private ClickableTextureComponent areaNextButton;

		// Token: 0x04001002 RID: 4098
		private ClickableTextureComponent areaBackButton;

		// Token: 0x04001003 RID: 4099
		private ClickableAnimatedComponent presentButton;

		// Token: 0x04001004 RID: 4100
		private Bundle currentPageBundle;
	}
}
