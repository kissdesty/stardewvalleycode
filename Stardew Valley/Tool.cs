using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;

namespace StardewValley
{
	// Token: 0x02000054 RID: 84
	[XmlInclude(typeof(MagnifyingGlass)), XmlInclude(typeof(Shears)), XmlInclude(typeof(MilkPail)), XmlInclude(typeof(Axe)), XmlInclude(typeof(Wand)), XmlInclude(typeof(Hoe)), XmlInclude(typeof(FishingRod)), XmlInclude(typeof(MeleeWeapon)), XmlInclude(typeof(Pan)), XmlInclude(typeof(Pickaxe)), XmlInclude(typeof(WateringCan)), XmlInclude(typeof(Slingshot))]
	public class Tool : Item
	{
		// Token: 0x170000B2 RID: 178
		public override string Name
		{
			// Token: 0x0600079C RID: 1948 RVA: 0x000A8128 File Offset: 0x000A6328
			get
			{
				switch (this.upgradeLevel)
				{
				case 1:
					return "Copper " + this.name;
				case 2:
					return "Steel " + this.name;
				case 3:
					return "Gold " + this.name;
				case 4:
					return "Iridium " + this.name;
				default:
					return this.name;
				}
			}
			// Token: 0x0600079D RID: 1949 RVA: 0x000A81A0 File Offset: 0x000A63A0
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		public override int Stack
		{
			// Token: 0x0600079E RID: 1950 RVA: 0x000A81A9 File Offset: 0x000A63A9
			get
			{
				if (this.stackable)
				{
					return ((Stackable)this).NumberInStack;
				}
				return 1;
			}
			// Token: 0x0600079F RID: 1951 RVA: 0x000A81C0 File Offset: 0x000A63C0
			set
			{
				if (this.stackable)
				{
					((Stackable)this).Stack = Math.Min(Math.Max(0, value), this.maximumStackSize());
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		public string Description
		{
			// Token: 0x060007A0 RID: 1952 RVA: 0x000A81E7 File Offset: 0x000A63E7
			get
			{
				return this.description;
			}
		}

		// Token: 0x170000B5 RID: 181
		public int CurrentParentTileIndex
		{
			// Token: 0x060007A1 RID: 1953 RVA: 0x000A81EF File Offset: 0x000A63EF
			get
			{
				return this.currentParentTileIndex;
			}
			// Token: 0x060007A2 RID: 1954 RVA: 0x000A81F7 File Offset: 0x000A63F7
			set
			{
				this.currentParentTileIndex = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		public virtual int UpgradeLevel
		{
			// Token: 0x060007A3 RID: 1955 RVA: 0x000A8200 File Offset: 0x000A6400
			get
			{
				return this.upgradeLevel;
			}
			// Token: 0x060007A4 RID: 1956 RVA: 0x000A8208 File Offset: 0x000A6408
			set
			{
				this.upgradeLevel = value;
				this.setNewTileIndexForUpgradeLevel();
			}
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000A8217 File Offset: 0x000A6417
		public Tool()
		{
			this.category = -99;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000A8228 File Offset: 0x000A6428
		public Tool(string name, int upgradeLevel, int initialParentTileIndex, int indexOfMenuItemView, string description, bool stackable, int numAttachmentSlots = 0)
		{
			this.name = name;
			this.description = description;
			this.initialParentTileIndex = initialParentTileIndex;
			this.indexOfMenuItemView = indexOfMenuItemView;
			this.stackable = stackable;
			this.currentParentTileIndex = initialParentTileIndex;
			this.numAttachmentSlots = numAttachmentSlots;
			if (numAttachmentSlots > 0)
			{
				this.attachments = new Object[numAttachmentSlots];
			}
			this.category = -99;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000A828C File Offset: 0x000A648C
		public override string getCategoryName()
		{
			if (this is MeleeWeapon && (this as MeleeWeapon).indexOfMenuItemView != 47)
			{
				return string.Concat(new object[]
				{
					"Level ",
					(this as MeleeWeapon).getItemLevel(),
					" ",
					((this as MeleeWeapon).type == 1) ? "Dagger" : (((this as MeleeWeapon).type == 2) ? "Club" : "Sword")
				});
			}
			return "Tool";
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000A8316 File Offset: 0x000A6516
		public override Color getCategoryColor()
		{
			return Color.DarkSlateGray;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000A8320 File Offset: 0x000A6520
		public virtual void draw(SpriteBatch b)
		{
			if (Game1.player.toolPower > 0 && Game1.player.canReleaseTool)
			{
				foreach (Vector2 v in this.tilesAffected(Game1.player.GetToolLocation(false) / (float)Game1.tileSize, Game1.player.toolPower, Game1.player))
				{
					b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float)((int)v.X * Game1.tileSize), (float)((int)v.Y * Game1.tileSize))), new Rectangle?(new Rectangle(194, 388, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.01f);
				}
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void tickUpdate(GameTime time, Farmer who)
		{
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x000A8414 File Offset: 0x000A6614
		public void Update(int direction, int farmerMotionFrame)
		{
			this.Update(direction, farmerMotionFrame, Game1.player);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000A8423 File Offset: 0x000A6623
		public bool isHeavyHitter()
		{
			return this is MeleeWeapon || this is Hoe || this is Axe || this is Pickaxe;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000A8448 File Offset: 0x000A6648
		public void Update(int direction, int farmerMotionFrame, Farmer who)
		{
			int offset = 0;
			if (this is WateringCan)
			{
				switch (direction)
				{
				case 0:
					offset = 4;
					break;
				case 1:
					offset = 2;
					break;
				case 2:
					offset = 0;
					break;
				case 3:
					offset = 2;
					break;
				}
			}
			else if (this is FishingRod)
			{
				switch (direction)
				{
				case 0:
					offset = 3;
					break;
				case 1:
					offset = 0;
					break;
				case 3:
					offset = 0;
					break;
				}
			}
			else
			{
				switch (direction)
				{
				case 0:
					offset = 3;
					break;
				case 1:
					offset = 2;
					break;
				case 3:
					offset = 2;
					break;
				}
			}
			if (!this.Name.Equals("Watering Can"))
			{
				if (farmerMotionFrame < 1)
				{
					this.currentParentTileIndex = this.initialParentTileIndex;
				}
				else if (who.FacingDirection == 0 || (who.FacingDirection == 2 && farmerMotionFrame >= 2))
				{
					this.currentParentTileIndex = this.initialParentTileIndex + 1;
				}
			}
			else if (farmerMotionFrame < 5 || direction == 0)
			{
				this.currentParentTileIndex = this.initialParentTileIndex;
			}
			else
			{
				this.currentParentTileIndex = this.initialParentTileIndex + 1;
			}
			this.currentParentTileIndex += offset;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000A854F File Offset: 0x000A674F
		public override int attachmentSlots()
		{
			return this.numAttachmentSlots;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000A8557 File Offset: 0x000A6757
		public Farmer getLastFarmerToUse()
		{
			return this.lastUser;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void leftClick(Farmer who)
		{
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000A8560 File Offset: 0x000A6760
		public virtual void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			this.lastUser = who;
			short seed = (short)Game1.random.Next(-32768, 32768);
			if (Game1.IsClient && who.Equals(Game1.player))
			{
				Game1.recentMultiplayerRandom = new Random((int)seed);
				ToolDescription t = ToolFactory.getIndexFromTool(this);
				Game1.client.sendMessage(7, new object[]
				{
					t.index,
					t.upgradeLevel,
					(short)x,
					(short)y,
					location.name,
					(byte)who.FacingDirection,
					seed
				});
			}
			else if (Game1.IsServer && who.Equals(Game1.player))
			{
				Game1.recentMultiplayerRandom = new Random((int)seed);
				MultiplayerUtility.broadcastToolAction(this, x, y, location.name, (byte)who.FacingDirection, seed, who.uniqueMultiplayerID);
			}
			if (this.isHeavyHitter() && !(this is MeleeWeapon))
			{
				Rumble.rumble(0.1f + (float)(Game1.random.NextDouble() / 4.0), (float)(100 + Game1.random.Next(50)));
				location.damageMonster(new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), this.upgradeLevel + 1, (this.upgradeLevel + 1) * 3, false, who);
			}
			if (this is MeleeWeapon && (!who.UsingTool || Game1.mouseClickPolling >= 50 || (this as MeleeWeapon).type == 1 || (this as MeleeWeapon).initialParentTileIndex == 47 || MeleeWeapon.timedHitTimer > 0 || who.FarmerSprite.indexInCurrentAnimation != 5 || who.FarmerSprite.timer >= who.FarmerSprite.interval / 4f))
			{
				if ((this as MeleeWeapon).type == 2 && (this as MeleeWeapon).isOnSpecial)
				{
					(this as MeleeWeapon).doClubFunction(who);
					return;
				}
				if (who.FarmerSprite.indexInCurrentAnimation > 0)
				{
					MeleeWeapon.timedHitTimer = 500;
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000A8788 File Offset: 0x000A6988
		public virtual void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			return false;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool canBeDropped()
		{
			return false;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x000A8790 File Offset: 0x000A6990
		public virtual bool canThisBeAttached(Object o)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Length; i++)
				{
					if (this.attachments[i] == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000A87C8 File Offset: 0x000A69C8
		public virtual Object attach(Object o)
		{
			for (int i = 0; i < this.attachments.Length; i++)
			{
				if (this.attachments[i] == null)
				{
					this.attachments[i] = o;
					Game1.playSound("button1");
					return null;
				}
			}
			return o;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000A8808 File Offset: 0x000A6A08
		public void colorTool(int level)
		{
			int initialLocation = 0;
			int startPixel = 0;
			string a = this.name.Split(new char[]
			{
				' '
			}).Last<string>();
			if (!(a == "Hoe"))
			{
				if (!(a == "Pickaxe"))
				{
					if (!(a == "Axe"))
					{
						if (a == "Can")
						{
							initialLocation = 168713;
							startPixel = 163840;
						}
					}
					else
					{
						initialLocation = 134681;
						startPixel = 131072;
					}
				}
				else
				{
					initialLocation = 100749;
					startPixel = 98304;
				}
			}
			else
			{
				initialLocation = 69129;
				startPixel = 65536;
			}
			int red = 0;
			int green = 0;
			int blue = 0;
			switch (level)
			{
			case 1:
				red = 198;
				green = 108;
				blue = 43;
				break;
			case 2:
				red = 197;
				green = 226;
				blue = 222;
				break;
			case 3:
				red = 248;
				green = 255;
				blue = 73;
				break;
			case 4:
				red = 144;
				green = 135;
				blue = 181;
				break;
			}
			if (startPixel > 0 && level > 0)
			{
				if (this.name.Contains("Can"))
				{
					Game1.toolSpriteSheet = ColorChanger.swapColor(Game1.toolSpriteSheet, initialLocation + 36, red * 5 / 4, green * 5 / 4, blue * 5 / 4, startPixel, startPixel + 32768);
				}
				Game1.toolSpriteSheet = ColorChanger.swapColor(Game1.toolSpriteSheet, initialLocation + 8, red, green, blue, startPixel, startPixel + 32768);
				Game1.toolSpriteSheet = ColorChanger.swapColor(Game1.toolSpriteSheet, initialLocation + 4, red * 3 / 4, green * 3 / 4, blue * 3 / 4, startPixel, startPixel + 32768);
				Game1.toolSpriteSheet = ColorChanger.swapColor(Game1.toolSpriteSheet, initialLocation, red * 3 / 8, green * 3 / 8, blue * 3 / 8, startPixel, startPixel + 32768);
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000A89CC File Offset: 0x000A6BCC
		public override bool actionWhenPurchased()
		{
			if (this is Axe || this is Pickaxe || this is Hoe || this is WateringCan)
			{
				Tool t = Game1.player.getToolFromName(this.name);
				Tool expr_32 = t;
				int num = expr_32.UpgradeLevel;
				expr_32.UpgradeLevel = num + 1;
				Game1.player.toolBeingUpgraded = t;
				Game1.player.daysLeftForToolUpgrade = 2;
				Game1.playSound("parry");
				Game1.player.removeItemFromInventory(t);
				Game1.exitActiveMenu();
				Game1.drawDialogue(Game1.getCharacterFromName("Clint", false), "Thanks. I'll get started on this as soon as I can. It should be ready in a couple days.");
				return true;
			}
			return base.actionWhenPurchased();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000A8A68 File Offset: 0x000A6C68
		protected List<Vector2> tilesAffected(Vector2 tileLocation, int power, Farmer who)
		{
			power++;
			List<Vector2> tileLocations = new List<Vector2>();
			tileLocations.Add(tileLocation);
			if (who.facingDirection == 0)
			{
				if (power >= 2)
				{
					tileLocations.Add(tileLocation + new Vector2(0f, -1f));
					tileLocations.Add(tileLocation + new Vector2(0f, -2f));
				}
				if (power >= 3)
				{
					tileLocations.Add(tileLocation + new Vector2(0f, -3f));
					tileLocations.Add(tileLocation + new Vector2(0f, -4f));
				}
				if (power >= 4)
				{
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.Add(tileLocation + new Vector2(1f, -2f));
					tileLocations.Add(tileLocation + new Vector2(1f, -1f));
					tileLocations.Add(tileLocation + new Vector2(1f, 0f));
					tileLocations.Add(tileLocation + new Vector2(-1f, -2f));
					tileLocations.Add(tileLocation + new Vector2(-1f, -1f));
					tileLocations.Add(tileLocation + new Vector2(-1f, 0f));
				}
				if (power >= 5)
				{
					for (int i = tileLocations.Count - 1; i >= 0; i--)
					{
						tileLocations.Add(tileLocations[i] + new Vector2(0f, -3f));
					}
				}
			}
			else if (who.facingDirection == 1)
			{
				if (power >= 2)
				{
					tileLocations.Add(tileLocation + new Vector2(1f, 0f));
					tileLocations.Add(tileLocation + new Vector2(2f, 0f));
				}
				if (power >= 3)
				{
					tileLocations.Add(tileLocation + new Vector2(3f, 0f));
					tileLocations.Add(tileLocation + new Vector2(4f, 0f));
				}
				if (power >= 4)
				{
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.Add(tileLocation + new Vector2(0f, -1f));
					tileLocations.Add(tileLocation + new Vector2(1f, -1f));
					tileLocations.Add(tileLocation + new Vector2(2f, -1f));
					tileLocations.Add(tileLocation + new Vector2(0f, 1f));
					tileLocations.Add(tileLocation + new Vector2(1f, 1f));
					tileLocations.Add(tileLocation + new Vector2(2f, 1f));
				}
				if (power >= 5)
				{
					for (int j = tileLocations.Count - 1; j >= 0; j--)
					{
						tileLocations.Add(tileLocations[j] + new Vector2(3f, 0f));
					}
				}
			}
			else if (who.facingDirection == 2)
			{
				if (power >= 2)
				{
					tileLocations.Add(tileLocation + new Vector2(0f, 1f));
					tileLocations.Add(tileLocation + new Vector2(0f, 2f));
				}
				if (power >= 3)
				{
					tileLocations.Add(tileLocation + new Vector2(0f, 3f));
					tileLocations.Add(tileLocation + new Vector2(0f, 4f));
				}
				if (power >= 4)
				{
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.Add(tileLocation + new Vector2(1f, 2f));
					tileLocations.Add(tileLocation + new Vector2(1f, 1f));
					tileLocations.Add(tileLocation + new Vector2(1f, 0f));
					tileLocations.Add(tileLocation + new Vector2(-1f, 2f));
					tileLocations.Add(tileLocation + new Vector2(-1f, 1f));
					tileLocations.Add(tileLocation + new Vector2(-1f, 0f));
				}
				if (power >= 5)
				{
					for (int k = tileLocations.Count - 1; k >= 0; k--)
					{
						tileLocations.Add(tileLocations[k] + new Vector2(0f, 3f));
					}
				}
			}
			else if (who.facingDirection == 3)
			{
				if (power >= 2)
				{
					tileLocations.Add(tileLocation + new Vector2(-1f, 0f));
					tileLocations.Add(tileLocation + new Vector2(-2f, 0f));
				}
				if (power >= 3)
				{
					tileLocations.Add(tileLocation + new Vector2(-3f, 0f));
					tileLocations.Add(tileLocation + new Vector2(-4f, 0f));
				}
				if (power >= 4)
				{
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.RemoveAt(tileLocations.Count - 1);
					tileLocations.Add(tileLocation + new Vector2(0f, -1f));
					tileLocations.Add(tileLocation + new Vector2(-1f, -1f));
					tileLocations.Add(tileLocation + new Vector2(-2f, -1f));
					tileLocations.Add(tileLocation + new Vector2(0f, 1f));
					tileLocations.Add(tileLocation + new Vector2(-1f, 1f));
					tileLocations.Add(tileLocation + new Vector2(-2f, 1f));
				}
				if (power >= 5)
				{
					for (int l = tileLocations.Count - 1; l >= 0; l--)
					{
						tileLocations.Add(tileLocations[l] + new Vector2(-3f, 0f));
					}
				}
			}
			return tileLocations;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000846E File Offset: 0x0000666E
		public virtual bool doesShowTileLocationMarker()
		{
			return true;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000A9098 File Offset: 0x000A7298
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			spriteBatch.Draw(Game1.toolSpriteSheet, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Rectangle?(Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, this.indexOfMenuItemView)), Color.White * transparency, 0f, new Vector2((float)(Game1.tileSize / 4 / 2), (float)(Game1.tileSize / 4 / 2)), (float)Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);
			if (this.stackable)
			{
				Game1.drawWithBorder(string.Concat(((Stackable)this).NumberInStack), Color.Black, Color.White, location + new Vector2((float)Game1.tileSize - Game1.dialogueFont.MeasureString(string.Concat(((Stackable)this).NumberInStack)).X, (float)Game1.tileSize - Game1.dialogueFont.MeasureString(string.Concat(((Stackable)this).NumberInStack)).Y * 3f / 4f), 0f, 0.5f, 1f);
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool isPlaceable()
		{
			return false;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000A91CE File Offset: 0x000A73CE
		public override int maximumStackSize()
		{
			if (this.stackable)
			{
				return 99;
			}
			return -1;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000A91DC File Offset: 0x000A73DC
		public virtual void setNewTileIndexForUpgradeLevel()
		{
			if (this is MeleeWeapon || this is MagnifyingGlass || this is MilkPail || this is Shears || this is Pan || this is Slingshot || this is Wand)
			{
				return;
			}
			int toolTypeOffset = 21;
			if (this is FishingRod)
			{
				this.initialParentTileIndex = 8 + this.upgradeLevel;
				this.currentParentTileIndex = this.initialParentTileIndex;
				this.indexOfMenuItemView = this.initialParentTileIndex;
				return;
			}
			if (this is Axe)
			{
				toolTypeOffset = 189;
			}
			else if (this is Hoe)
			{
				toolTypeOffset = 21;
			}
			else if (this is Pickaxe)
			{
				toolTypeOffset = 105;
			}
			else if (this is WateringCan)
			{
				toolTypeOffset = 273;
			}
			toolTypeOffset += this.upgradeLevel * 7;
			if (this.upgradeLevel > 2)
			{
				toolTypeOffset += 21;
			}
			this.initialParentTileIndex = toolTypeOffset;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.initialParentTileIndex + ((this is WateringCan) ? 2 : 5) + 21;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000A92D4 File Offset: 0x000A74D4
		public override Item getOne()
		{
			if (this.stackable)
			{
				return new Seeds(((Seeds)this).SeedType, 1);
			}
			return this;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000A81A9 File Offset: 0x000A63A9
		public override int getStack()
		{
			if (this.stackable)
			{
				return ((Stackable)this).NumberInStack;
			}
			return 1;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000A92F4 File Offset: 0x000A74F4
		public override int addToStack(int amount)
		{
			if (!this.stackable)
			{
				return amount;
			}
			((Stackable)this).NumberInStack += amount;
			if (((Stackable)this).NumberInStack > 99)
			{
				int arg_45_0 = ((Stackable)this).NumberInStack - 99;
				((Stackable)this).NumberInStack = 99;
				return arg_45_0;
			}
			return 0;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000A934A File Offset: 0x000A754A
		public override string getDescription()
		{
			return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
		}

		// Token: 0x04000884 RID: 2180
		public const int standardStaminaReduction = 2;

		// Token: 0x04000885 RID: 2181
		public const int nonUpgradeable = -1;

		// Token: 0x04000886 RID: 2182
		public const int stone = 0;

		// Token: 0x04000887 RID: 2183
		public const int copper = 1;

		// Token: 0x04000888 RID: 2184
		public const int steel = 2;

		// Token: 0x04000889 RID: 2185
		public const int gold = 3;

		// Token: 0x0400088A RID: 2186
		public const int iridium = 4;

		// Token: 0x0400088B RID: 2187
		public const int parsnipSpriteIndex = 0;

		// Token: 0x0400088C RID: 2188
		public const int hoeSpriteIndex = 21;

		// Token: 0x0400088D RID: 2189
		public const int hammerSpriteIndex = 105;

		// Token: 0x0400088E RID: 2190
		public const int axeSpriteIndex = 189;

		// Token: 0x0400088F RID: 2191
		public const int wateringCanSpriteIndex = 273;

		// Token: 0x04000890 RID: 2192
		public const int fishingRodSpriteIndex = 8;

		// Token: 0x04000891 RID: 2193
		public const int batteredSwordSpriteIndex = 67;

		// Token: 0x04000892 RID: 2194
		public const int axeMenuIndex = 215;

		// Token: 0x04000893 RID: 2195
		public const int hoeMenuIndex = 47;

		// Token: 0x04000894 RID: 2196
		public const int pickAxeMenuIndex = 131;

		// Token: 0x04000895 RID: 2197
		public const int wateringCanMenuIndex = 296;

		// Token: 0x04000896 RID: 2198
		public const int startOfNegativeWeaponIndex = -10000;

		// Token: 0x04000897 RID: 2199
		public static Texture2D weaponsTexture = Game1.content.Load<Texture2D>("TileSheets\\weapons");

		// Token: 0x04000898 RID: 2200
		public string name;

		// Token: 0x04000899 RID: 2201
		public string description;

		// Token: 0x0400089A RID: 2202
		public int initialParentTileIndex;

		// Token: 0x0400089B RID: 2203
		public int currentParentTileIndex;

		// Token: 0x0400089C RID: 2204
		public int indexOfMenuItemView;

		// Token: 0x0400089D RID: 2205
		public bool stackable;

		// Token: 0x0400089E RID: 2206
		public bool instantUse;

		// Token: 0x0400089F RID: 2207
		public static Color copperColor = new Color(198, 108, 43);

		// Token: 0x040008A0 RID: 2208
		public static Color steelColor = new Color(197, 226, 222);

		// Token: 0x040008A1 RID: 2209
		public static Color goldColor = new Color(248, 255, 73);

		// Token: 0x040008A2 RID: 2210
		public static Color iridiumColor = new Color(144, 135, 181);

		// Token: 0x040008A3 RID: 2211
		public int upgradeLevel;

		// Token: 0x040008A4 RID: 2212
		public int numAttachmentSlots;

		// Token: 0x040008A5 RID: 2213
		protected Farmer lastUser;

		// Token: 0x040008A6 RID: 2214
		public Object[] attachments;
	}
}
