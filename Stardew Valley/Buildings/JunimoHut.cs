using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace StardewValley.Buildings
{
	// Token: 0x02000146 RID: 326
	public class JunimoHut : Building
	{
		// Token: 0x06001278 RID: 4728 RVA: 0x0017787C File Offset: 0x00175A7C
		public JunimoHut(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
			this.sourceRect = this.getSourceRectForMenu();
			this.output = new Chest(true);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x001778EC File Offset: 0x00175AEC
		public JunimoHut()
		{
			this.sourceRect = this.getSourceRectForMenu();
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0017794C File Offset: 0x00175B4C
		public override Rectangle getRectForAnimalDoor()
		{
			return new Rectangle((1 + this.tileX) * Game1.tileSize, (this.tileY + 1) * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00177979 File Offset: 0x00175B79
		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(Utility.getSeasonNumber(Game1.currentSeason) * 48, 0, 48, 64);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00177992 File Offset: 0x00175B92
		public override void load()
		{
			base.load();
			this.sourceRect = this.getSourceRectForMenu();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x001779A6 File Offset: 0x00175BA6
		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			int arg_0F_0 = this.daysOfConstructionLeft;
			this.sourceRect = this.getSourceRectForMenu();
			this.myJunimos.Clear();
			this.wasLit = false;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x001779D6 File Offset: 0x00175BD6
		public void sendOutJunimos()
		{
			this.junimoSendOutTimer = 1000;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x001779E3 File Offset: 0x00175BE3
		public override void performActionOnConstruction(GameLocation location)
		{
			base.performActionOnConstruction(location);
			this.sendOutJunimos();
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x001779F4 File Offset: 0x00175BF4
		public override void performActionOnPlayerLocationEntry()
		{
			base.performActionOnPlayerLocationEntry();
			if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && !Game1.IsWinter)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 0.5f)
				{
					identifier = this.tileX + this.tileY * 777
				});
				AmbientLocationSounds.addSound(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)), 1);
				this.wasLit = true;
			}
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00177AC0 File Offset: 0x00175CC0
		public int getUnusedJunimoNumber()
		{
			for (int i = 0; i < 3; i++)
			{
				if (i >= this.myJunimos.Count<JunimoHarvester>())
				{
					return i;
				}
				bool found = false;
				using (List<JunimoHarvester>.Enumerator enumerator = this.myJunimos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.whichJunimoFromThisHut == i)
						{
							found = true;
							break;
						}
					}
				}
				if (!found)
				{
					return i;
				}
			}
			return 2;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00177B3C File Offset: 0x00175D3C
		public override void Update(GameTime time)
		{
			base.Update(time);
			if (this.junimoSendOutTimer > 0)
			{
				this.junimoSendOutTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.junimoSendOutTimer <= 0 && this.myJunimos.Count<JunimoHarvester>() < 3 && !Game1.IsWinter && !Game1.isRaining && this.areThereMatureCropsWithinRadius())
				{
					JunimoHarvester i = new JunimoHarvester(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 2)), this, this.getUnusedJunimoNumber());
					Game1.getFarm().characters.Add(i);
					this.myJunimos.Add(i);
					this.junimoSendOutTimer = 1000;
					if (Utility.isOnScreen(Utility.Vector2ToPoint(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1))), Game1.tileSize, Game1.getFarm()))
					{
						Game1.soundBank.PlayCue("junimoMeep1");
					}
				}
			}
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00177C5C File Offset: 0x00175E5C
		public bool areThereMatureCropsWithinRadius()
		{
			Farm f = Game1.getFarm();
			for (int x = this.tileX + 1 - 8; x < this.tileX + 2 + 8; x++)
			{
				for (int y = this.tileY - 8 + 1; y < this.tileY + 2 + 8; y++)
				{
					if (f.isCropAtTile(x, y) && (f.terrainFeatures[new Vector2((float)x, (float)y)] as HoeDirt).readyForHarvest())
					{
						this.lastKnownCropLocation = new Point(x, y);
						return true;
					}
				}
			}
			this.lastKnownCropLocation = Point.Zero;
			return false;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00177CF0 File Offset: 0x00175EF0
		public override void performTenMinuteAction(int timeElapsed)
		{
			base.performTenMinuteAction(timeElapsed);
			for (int i = this.myJunimos.Count - 1; i >= 0; i--)
			{
				if (!Game1.getFarm().characters.Contains(this.myJunimos[i]))
				{
					this.myJunimos.RemoveAt(i);
				}
				else
				{
					this.myJunimos[i].pokeToHarvest();
				}
			}
			if (this.myJunimos.Count<JunimoHarvester>() < 3 && Game1.timeOfDay < 1900)
			{
				this.junimoSendOutTimer = 1;
			}
			if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && !Game1.IsWinter && Utility.getLightSource(this.tileX + this.tileY * 777) == null && Game1.random.NextDouble() < 0.2)
			{
				Game1.currentLightSources.Add(new LightSource(4, new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 0.5f)
				{
					identifier = this.tileX + this.tileY * 777
				});
				AmbientLocationSounds.addSound(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)), 1);
				this.wasLit = true;
				return;
			}
			if (Game1.timeOfDay == 2400 && !Game1.IsWinter)
			{
				Utility.removeLightSource(this.tileX + this.tileY * 777);
				AmbientLocationSounds.removeSound(new Vector2((float)(this.tileX + 1), (float)(this.tileY + 1)));
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00177EB0 File Offset: 0x001760B0
		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (who.IsMainPlayer && tileLocation.X >= (float)this.tileX && tileLocation.X < (float)(this.tileX + this.tilesWide) && tileLocation.Y >= (float)this.tileY && tileLocation.Y < (float)(this.tileY + this.tilesHigh) && this.output != null)
			{
				Game1.activeClickableMenu = new ItemGrabMenu(this.output.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromInventory), null, new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromChest), false, true, true, true, true, 1, null, 1, this);
				return true;
			}
			return base.doAction(tileLocation, who);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00177F78 File Offset: 0x00176178
		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(0, 0, 48, 64)), this.color, 0f, new Vector2(0f, 0f), (float)Game1.pixelZoom, SpriteEffects.None, 0.89f);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00177FDC File Offset: 0x001761DC
		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.sourceRect), this.color * this.alpha, 0f, new Vector2(0f, (float)this.texture.Bounds.Height), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
			if (!this.output.isEmpty())
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize * 2 + Game1.pixelZoom * 3), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize - Game1.tileSize / 2))), new Rectangle?(this.bagRect), this.color * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize + 1) / 10000f);
			}
			if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && !Game1.IsWinter && this.wasLit)
			{
				b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize + Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize - Game1.tileSize))), new Rectangle?(this.lightInteriorRect), this.color * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize + 1) / 10000f);
			}
		}

		// Token: 0x04001314 RID: 4884
		public const int cropHarvestRadius = 8;

		// Token: 0x04001315 RID: 4885
		public Chest output;

		// Token: 0x04001316 RID: 4886
		public bool noHarvest;

		// Token: 0x04001317 RID: 4887
		public Rectangle sourceRect;

		// Token: 0x04001318 RID: 4888
		private int junimoSendOutTimer;

		// Token: 0x04001319 RID: 4889
		[XmlIgnore]
		public List<JunimoHarvester> myJunimos = new List<JunimoHarvester>();

		// Token: 0x0400131A RID: 4890
		[XmlIgnore]
		public Point lastKnownCropLocation = Point.Zero;

		// Token: 0x0400131B RID: 4891
		private bool wasLit;

		// Token: 0x0400131C RID: 4892
		private Rectangle lightInteriorRect = new Rectangle(195, 0, 18, 17);

		// Token: 0x0400131D RID: 4893
		private Rectangle bagRect = new Rectangle(208, 51, 15, 13);
	}
}
