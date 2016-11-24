using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x020000FD RID: 253
	public class MapPage : IClickableMenu
	{
		// Token: 0x06000F2D RID: 3885 RVA: 0x001384D8 File Offset: 0x001366D8
		public MapPage(int x, int y, int width, int height) : base(x, y, width, height, false)
		{
			this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + width + Game1.tileSize, this.yPositionOnScreen + height - IClickableMenu.borderWidth - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), null, null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
			this.map = Game1.content.Load<Texture2D>("LooseSprites\\map");
			Vector2 center = Utility.getTopLeftPositionForCenteringOnScreen(this.map.Bounds.Width * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0);
			this.mapX = (int)center.X;
			this.mapY = (int)center.Y;
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX, this.mapY, 292, 152), Game1.player.mailReceived.Contains("ccVault") ? "Calico Desert" : "???"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 324, this.mapY + 252, 188, 132), Game1.player.farmName + " Farm"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 360, this.mapY + 96, 188, 132), "Backwoods"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 516, this.mapY + 224, 76, 100), "Bus Stop"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 196, this.mapY + 352, 36, 76), "Wizard's Tower"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 420, this.mapY + 392, 76, 40), "Marnie's Ranch" + Environment.NewLine + "Open 9:00AM to 4:00PM most days"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 452, this.mapY + 436, 32, 24), "Leah's Cottage"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 612, this.mapY + 396, 36, 52), "1 Willow Lane" + Environment.NewLine + "Home of Jodi, Kent & Sam"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 652, this.mapY + 408, 40, 36), "2 Willow Lane" + Environment.NewLine + "Home of Emily & Haley"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 672, this.mapY + 340, 44, 60), "Town Square"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 680, this.mapY + 304, 16, 32), "Harvey's Clinic" + Environment.NewLine + "Open 9:00AM to 3:00PM"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 696, this.mapY + 296, 28, 40), string.Concat(new string[]
			{
				"Pierre's General Store",
				Environment.NewLine,
				"Home of Pierre, Caroline & Abigail ",
				Environment.NewLine,
				"Open 9:00AM to 6:00PM (Closed Wednesday)"
			})));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 852, this.mapY + 388, 80, 36), "Blacksmith" + Environment.NewLine + "Open 9:00AM to 4:00PM"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 716, this.mapY + 352, 28, 40), "Saloon" + Environment.NewLine + "Open 12:00PM To 12:00AM"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 768, this.mapY + 388, 44, 56), "Mayor's Manor"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 892, this.mapY + 416, 32, 28), "Stardew Valley Museum & Library" + Environment.NewLine + "Open 8:00AM to 6:00PM"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 824, this.mapY + 564, 28, 20), "Elliott's Cabin"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 696, this.mapY + 448, 24, 20), "Sewer"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 724, this.mapY + 424, 40, 32), "Graveyard"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 780, this.mapY + 360, 24, 20), "Trailer"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 748, this.mapY + 316, 36, 36), "1 River Road" + Environment.NewLine + "Home of George, Evelyn & Alex"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 732, this.mapY + 148, 48, 32), string.Concat(new string[]
			{
				"Carpenter's Shop",
				Environment.NewLine,
				"Home of Robin, Demetrius, Sebastian & Maru",
				Environment.NewLine,
				"Shop open 9:00AM to 5:00PM most days"
			})));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 784, this.mapY + 128, 12, 16), "Tent"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 880, this.mapY + 96, 16, 24), "Mines"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 900, this.mapY + 108, 32, 36), (Game1.stats.DaysPlayed >= 5u) ? ("Adventurer's Guild" + Environment.NewLine + "Open 2:00PM to 10:00PM") : "???"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 968, this.mapY + 116, 88, 76), Game1.player.mailReceived.Contains("ccCraftsRoom") ? "Quarry" : "???"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 872, this.mapY + 280, 52, 52), "JojaMart" + Environment.NewLine + "Open 9:00AM to 11:00PM"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 844, this.mapY + 608, 36, 40), "Fish Shop" + Environment.NewLine + "Open 9:00AM to 5:00PM"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 576, this.mapY + 60, 48, 36), Game1.isLocationAccessible("Railroad") ? ("Spa" + Environment.NewLine + "Open all day") : "???"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX, this.mapY + 272, 196, 176), Game1.player.mailReceived.Contains("beenToWoods") ? "Secret Woods" : "???"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 260, this.mapY + 572, 20, 20), "Ruined House"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 692, this.mapY + 204, 44, 36), "Community Center"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 380, this.mapY + 596, 24, 32), "Sewer Pipe"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 644, this.mapY + 64, 16, 8), Game1.isLocationAccessible("Railroad") ? "Railroad" : "???"));
			this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 728, this.mapY + 652, 28, 28), "Lonely Stone"));
			this.setUpPlayerMapPosition();
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00138ED4 File Offset: 0x001370D4
		public void setUpPlayerMapPosition()
		{
			this.playerMapPosition = new Vector2(-999f, -999f);
			string replacedName = Game1.player.currentLocation.Name;
			string name = Game1.player.currentLocation.Name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2026102357u)
			{
				if (num <= 807500499u)
				{
					if (num <= 278567071u)
					{
						if (num != 144182059u)
						{
							if (num != 263498407u)
							{
								if (num != 278567071u)
								{
									goto IL_538;
								}
								if (!(name == "HarveyRoom"))
								{
									goto IL_538;
								}
							}
							else
							{
								if (!(name == "BathHouse_Pool"))
								{
									goto IL_538;
								}
								goto IL_4D5;
							}
						}
						else
						{
							if (!(name == "WizardHouseBasement"))
							{
								goto IL_538;
							}
							goto IL_4CD;
						}
					}
					else if (num != 437214172u)
					{
						if (num != 746089795u)
						{
							if (num != 807500499u)
							{
								goto IL_538;
							}
							if (!(name == "Hospital"))
							{
								goto IL_538;
							}
						}
						else
						{
							if (!(name == "ScienceHouse"))
							{
								goto IL_538;
							}
							goto IL_4FC;
						}
					}
					else
					{
						if (!(name == "Desert"))
						{
							goto IL_538;
						}
						goto IL_45F;
					}
					replacedName = "Harvey's Clinic" + Environment.NewLine + "Open 9:00AM to 3:00PM";
					goto IL_538;
				}
				if (num <= 1428365440u)
				{
					if (num != 1167876998u)
					{
						if (num != 1253908523u)
						{
							if (num != 1428365440u)
							{
								goto IL_538;
							}
							if (!(name == "SeedShop"))
							{
								goto IL_538;
							}
							replacedName = string.Concat(new string[]
							{
								"Pierre's General Store",
								Environment.NewLine,
								"Home of Pierre, Caroline & Abigail ",
								Environment.NewLine,
								"Open 9:00AM to 6:00PM (Closed Wednesday)"
							});
							goto IL_538;
						}
						else
						{
							if (!(name == "JoshHouse"))
							{
								goto IL_538;
							}
							replacedName = "1 River Road" + Environment.NewLine + "Home of George, Evelyn & Alex";
							goto IL_538;
						}
					}
					else
					{
						if (!(name == "ManorHouse"))
						{
							goto IL_538;
						}
						replacedName = "Mayor's Manor";
						goto IL_538;
					}
				}
				else if (num <= 1840909614u)
				{
					if (num != 1807680626u)
					{
						if (num != 1840909614u)
						{
							goto IL_538;
						}
						if (!(name == "SandyHouse"))
						{
							goto IL_538;
						}
					}
					else if (!(name == "SandyShop"))
					{
						goto IL_538;
					}
				}
				else if (num != 1919215024u)
				{
					if (num != 2026102357u)
					{
						goto IL_538;
					}
					if (!(name == "UndergroundMine"))
					{
						goto IL_538;
					}
					goto IL_4F4;
				}
				else
				{
					if (!(name == "ElliottHouse"))
					{
						goto IL_538;
					}
					replacedName = "Elliott's Cabin";
					goto IL_538;
				}
			}
			else if (num <= 3006626703u)
			{
				if (num <= 2478616111u)
				{
					if (num != 2028543928u)
					{
						if (num != 2204429310u)
						{
							if (num != 2478616111u)
							{
								goto IL_538;
							}
							if (!(name == "BathHouse_Entry"))
							{
								goto IL_538;
							}
							goto IL_4D5;
						}
						else
						{
							if (!(name == "SebastianRoom"))
							{
								goto IL_538;
							}
							goto IL_4FC;
						}
					}
					else
					{
						if (!(name == "Backwoods"))
						{
							goto IL_538;
						}
						goto IL_538;
					}
				}
				else if (num <= 2708986271u)
				{
					if (num != 2706464810u)
					{
						if (num != 2708986271u)
						{
							goto IL_538;
						}
						if (!(name == "ArchaeologyHouse"))
						{
							goto IL_538;
						}
						replacedName = "Stardew Valley Museum & Library";
						goto IL_538;
					}
					else
					{
						if (!(name == "WizardHouse"))
						{
							goto IL_538;
						}
						goto IL_4CD;
					}
				}
				else if (num != 2844260897u)
				{
					if (num != 3006626703u)
					{
						goto IL_538;
					}
					if (!(name == "FishShop"))
					{
						goto IL_538;
					}
					replacedName = "Fish Shop";
					goto IL_538;
				}
				else
				{
					if (!(name == "Woods"))
					{
						goto IL_538;
					}
					replacedName = "Secret Woods";
					goto IL_538;
				}
			}
			else if (num <= 3653788295u)
			{
				if (num != 3095702198u)
				{
					if (num != 3647688262u)
					{
						if (num != 3653788295u)
						{
							goto IL_538;
						}
						if (!(name == "SkullCave"))
						{
							goto IL_538;
						}
					}
					else
					{
						if (!(name == "BathHouse_WomensLocker"))
						{
							goto IL_538;
						}
						goto IL_4D5;
					}
				}
				else
				{
					if (!(name == "AdventureGuild"))
					{
						goto IL_538;
					}
					replacedName = "Adventurer's Guild";
					goto IL_538;
				}
			}
			else if (num <= 3924195856u)
			{
				if (num != 3848897750u)
				{
					if (num != 3924195856u)
					{
						goto IL_538;
					}
					if (!(name == "BathHouse_MensLocker"))
					{
						goto IL_538;
					}
					goto IL_4D5;
				}
				else
				{
					if (!(name == "Mine"))
					{
						goto IL_538;
					}
					goto IL_4F4;
				}
			}
			else if (num != 3978811393u)
			{
				if (num != 3979572909u)
				{
					goto IL_538;
				}
				if (!(name == "Club"))
				{
					goto IL_538;
				}
			}
			else
			{
				if (!(name == "AnimalShop"))
				{
					goto IL_538;
				}
				replacedName = "Marnie's Ranch";
				goto IL_538;
			}
			IL_45F:
			replacedName = "Calico Desert";
			goto IL_538;
			IL_4CD:
			replacedName = "Wizard's Tower";
			goto IL_538;
			IL_4D5:
			replacedName = "Spa" + Environment.NewLine + "Open all day";
			goto IL_538;
			IL_4F4:
			replacedName = "Mines";
			goto IL_538;
			IL_4FC:
			replacedName = "Carpenter's Shop" + Environment.NewLine + "Home of Robin, Demetrius, Sebastian & Maru";
			IL_538:
			foreach (ClickableComponent c in this.points)
			{
				if (c.name.Equals(replacedName) || c.name.Replace(" ", "").Equals(replacedName) || (c.name.Contains(Environment.NewLine) && c.name.Substring(0, c.name.IndexOf(Environment.NewLine)).Equals(replacedName.Substring(0, replacedName.Contains(Environment.NewLine) ? replacedName.IndexOf(Environment.NewLine) : replacedName.Length))))
				{
					this.playerMapPosition = new Vector2((float)c.bounds.Center.X, (float)c.bounds.Center.Y);
					this.playerLocationName = (c.name.Contains(Environment.NewLine) ? c.name.Substring(0, c.name.IndexOf(Environment.NewLine)) : c.name);
					return;
				}
			}
			int x = Game1.player.getTileX();
			int y = Game1.player.getTileY();
			name = Game1.player.currentLocation.name;
			num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 2151182681u)
			{
				if (num <= 1256432216u)
				{
					if (num != 784782095u)
					{
						if (num != 1256432216u)
						{
							return;
						}
						if (!(name == "BackWoods"))
						{
							return;
						}
					}
					else
					{
						if (!(name == "FarmHouse"))
						{
							return;
						}
						goto IL_8F3;
					}
				}
				else if (num != 1667813495u)
				{
					if (num != 2151182681u)
					{
						return;
					}
					if (!(name == "Farm"))
					{
						return;
					}
					goto IL_8F3;
				}
				else if (!(name == "Tunnel"))
				{
					return;
				}
				this.playerMapPosition = new Vector2((float)(this.mapX + 109 * Game1.pixelZoom), (float)(this.mapY + 47 * Game1.pixelZoom));
				this.playerLocationName = "Backwoods";
				return;
				IL_8F3:
				this.playerMapPosition = new Vector2((float)(this.mapX + 96 * Game1.pixelZoom), (float)(this.mapY + 72 * Game1.pixelZoom));
				this.playerLocationName = Game1.player.farmName + " Farm";
				return;
			}
			if (num <= 2909376585u)
			{
				if (num != 2503779456u)
				{
					if (num != 2909376585u)
					{
						return;
					}
					if (!(name == "Saloon"))
					{
						return;
					}
					this.playerLocationName = "Stardrop Saloon";
					return;
				}
				else
				{
					if (!(name == "Forest"))
					{
						return;
					}
					if (y > 51)
					{
						this.playerLocationName = "Cindersap Forest";
						this.playerMapPosition = new Vector2((float)(this.mapX + 70 * Game1.pixelZoom), (float)(this.mapY + 135 * Game1.pixelZoom));
						return;
					}
					if (x < 58)
					{
						this.playerLocationName = "Cindersap Forest";
						this.playerMapPosition = new Vector2((float)(this.mapX + 63 * Game1.pixelZoom), (float)(this.mapY + 104 * Game1.pixelZoom));
						return;
					}
					this.playerLocationName = "W. Pelican Town";
					this.playerMapPosition = new Vector2((float)(this.mapX + 109 * Game1.pixelZoom), (float)(this.mapY + 107 * Game1.pixelZoom));
					return;
				}
			}
			else if (num != 3014964069u)
			{
				if (num != 3308967874u)
				{
					if (num != 3333348840u)
					{
						return;
					}
					if (!(name == "Beach"))
					{
						return;
					}
					this.playerLocationName = "Pelican Beach";
					this.playerMapPosition = new Vector2((float)(this.mapX + 202 * Game1.pixelZoom), (float)(this.mapY + 141 * Game1.pixelZoom));
					return;
				}
				else
				{
					if (!(name == "Mountain"))
					{
						return;
					}
					if (x < 38)
					{
						this.playerLocationName = "Mountains";
						this.playerMapPosition = new Vector2((float)(this.mapX + 185 * Game1.pixelZoom), (float)(this.mapY + 36 * Game1.pixelZoom));
						return;
					}
					if (x < 96)
					{
						this.playerLocationName = "Mountain Lake";
						this.playerMapPosition = new Vector2((float)(this.mapX + 220 * Game1.pixelZoom), (float)(this.mapY + 38 * Game1.pixelZoom));
						return;
					}
					this.playerLocationName = "Quarry";
					this.playerMapPosition = new Vector2((float)(this.mapX + 253 * Game1.pixelZoom), (float)(this.mapY + 40 * Game1.pixelZoom));
					return;
				}
			}
			else
			{
				if (!(name == "Town"))
				{
					return;
				}
				if (x > 84 && y < 68)
				{
					this.playerMapPosition = new Vector2((float)(this.mapX + 225 * Game1.pixelZoom), (float)(this.mapY + 81 * Game1.pixelZoom));
					this.playerLocationName = "Pelican Town";
					return;
				}
				if (x > 80 && y >= 68)
				{
					this.playerMapPosition = new Vector2((float)(this.mapX + 220 * Game1.pixelZoom), (float)(this.mapY + 108 * Game1.pixelZoom));
					this.playerLocationName = "Pelican Town";
					return;
				}
				if (y <= 42)
				{
					this.playerMapPosition = new Vector2((float)(this.mapX + 178 * Game1.pixelZoom), (float)(this.mapY + 64 * Game1.pixelZoom));
					this.playerLocationName = "Pelican Town";
					return;
				}
				if (y > 42 && y < 76)
				{
					this.playerMapPosition = new Vector2((float)(this.mapX + 175 * Game1.pixelZoom), (float)(this.mapY + 88 * Game1.pixelZoom));
					this.playerLocationName = "Pelican Town";
					return;
				}
				this.playerMapPosition = new Vector2((float)(this.mapX + 182 * Game1.pixelZoom), (float)(this.mapY + 109 * Game1.pixelZoom));
				this.playerLocationName = "Pelican Town";
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00139A2C File Offset: 0x00137C2C
		public override void receiveKeyPress(Keys key)
		{
			base.receiveKeyPress(key);
			if (Game1.options.doesInputListContain(Game1.options.mapButton, key))
			{
				base.exitThisMenu(true);
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00139A54 File Offset: 0x00137C54
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (this.okButton.containsPoint(x, y))
			{
				this.okButton.scale -= 0.25f;
				this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
				(Game1.activeClickableMenu as GameMenu).changeTab(0);
			}
			foreach (ClickableComponent c in this.points)
			{
				if (c.containsPoint(x, y))
				{
					string name = c.name;
					if (name == "Lonely Stone")
					{
						Game1.playSound("stoneCrack");
					}
				}
			}
			if (Game1.activeClickableMenu != null)
			{
				(Game1.activeClickableMenu as GameMenu).changeTab(0);
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00002834 File Offset: 0x00000A34
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00139B38 File Offset: 0x00137D38
		public override void performHoverAction(int x, int y)
		{
			this.descriptionText = "";
			this.hoverText = "";
			foreach (ClickableComponent c in this.points)
			{
				if (c.containsPoint(x, y))
				{
					this.hoverText = c.name;
					return;
				}
			}
			if (this.okButton.containsPoint(x, y))
			{
				this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
				return;
			}
			this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00139C24 File Offset: 0x00137E24
		public override void draw(SpriteBatch b)
		{
			Game1.drawDialogueBox(this.mapX - Game1.pixelZoom * 8, this.mapY - Game1.pixelZoom * 24, (this.map.Bounds.Width + 16) * Game1.pixelZoom, 212 * Game1.pixelZoom, false, true, null, false);
			b.Draw(this.map, new Vector2((float)this.mapX, (float)this.mapY), new Rectangle?(new Rectangle(0, 0, 300, 180)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.86f);
			switch (Game1.whichFarm)
			{
			case 1:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 180, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			case 2:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(131, 180, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			case 3:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 241, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			case 4:
				b.Draw(this.map, new Vector2((float)this.mapX, (float)(this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(131, 241, 131, 61)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
				break;
			}
			Game1.player.FarmerRenderer.drawMiniPortrat(b, this.playerMapPosition - new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 0.00011f, 4f, 2, Game1.player);
			if (this.playerLocationName != null)
			{
				SpriteText.drawStringWithScrollCenteredAt(b, this.playerLocationName, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height + Game1.tileSize / 2 + Game1.pixelZoom * 4, "", 1f, -1, 0, 0.88f, false);
			}
			this.okButton.draw(b);
			if (!this.hoverText.Equals(""))
			{
				IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
			}
		}

		// Token: 0x04001057 RID: 4183
		private string descriptionText = "";

		// Token: 0x04001058 RID: 4184
		private string hoverText = "";

		// Token: 0x04001059 RID: 4185
		private string playerLocationName;

		// Token: 0x0400105A RID: 4186
		private Texture2D map;

		// Token: 0x0400105B RID: 4187
		private int mapX;

		// Token: 0x0400105C RID: 4188
		private int mapY;

		// Token: 0x0400105D RID: 4189
		private Vector2 playerMapPosition;

		// Token: 0x0400105E RID: 4190
		private List<ClickableComponent> points = new List<ClickableComponent>();

		// Token: 0x0400105F RID: 4191
		private ClickableTextureComponent okButton;
	}
}
