using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Characters;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x0200011F RID: 287
	public class BuildableGameLocation : GameLocation
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x00152083 File Offset: 0x00150283
		public BuildableGameLocation()
		{
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x001520B7 File Offset: 0x001502B7
		public BuildableGameLocation(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x001520F0 File Offset: 0x001502F0
		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.dayUpdate(dayOfMonth);
				}
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00152148 File Offset: 0x00150348
		public virtual void timeUpdate(int timeElapsed)
		{
			foreach (Building b in this.buildings)
			{
				if (b.indoors != null && b.indoors.GetType() == typeof(AnimalHouse))
				{
					foreach (KeyValuePair<long, FarmAnimal> kvp in ((AnimalHouse)b.indoors).animals)
					{
						kvp.Value.updatePerTenMinutes(Game1.timeOfDay, b.indoors);
					}
				}
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00152218 File Offset: 0x00150418
		public Building getBuildingAt(Vector2 tile)
		{
			for (int i = this.buildings.Count - 1; i >= 0; i--)
			{
				if (!this.buildings[i].isTilePassable(tile))
				{
					return this.buildings[i];
				}
			}
			return null;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00152260 File Offset: 0x00150460
		public bool destroyStructure(Vector2 tile)
		{
			for (int i = this.buildings.Count - 1; i >= 0; i--)
			{
				if (!this.buildings[i].isTilePassable(tile))
				{
					this.buildings[i].performActionOnDemolition(this);
					this.buildings.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x001522BA File Offset: 0x001504BA
		public bool destroyStructure(Building b)
		{
			b.performActionOnDemolition(this);
			return this.buildings.Remove(b);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x001522D0 File Offset: 0x001504D0
		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
		{
			if (!glider)
			{
				foreach (Building b in this.buildings)
				{
					if (b.intersects(position))
					{
						if (character != null && character.GetType() == typeof(FarmAnimal))
						{
							Microsoft.Xna.Framework.Rectangle door = b.getRectForAnimalDoor();
							door.Height += Game1.tileSize;
							if (door.Contains(position) && b.buildingType.Contains((character as FarmAnimal).buildingTypeILiveIn))
							{
								continue;
							}
						}
						else if (character != null && character is JunimoHarvester)
						{
							Microsoft.Xna.Framework.Rectangle door2 = b.getRectForAnimalDoor();
							door2.Height += Game1.tileSize;
							if (door2.Contains(position))
							{
								continue;
							}
						}
						return true;
					}
				}
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x001523D8 File Offset: 0x001505D8
		public override bool isActionableTile(int xTile, int yTile, Farmer who)
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isActionableTile(xTile, yTile, who))
					{
						return true;
					}
				}
			}
			return base.isActionableTile(xTile, yTile, who);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0015243C File Offset: 0x0015063C
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.doAction(new Vector2((float)tileLocation.X, (float)tileLocation.Y), who))
					{
						return true;
					}
				}
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x001524B4 File Offset: 0x001506B4
		public override bool isTileOccupied(Vector2 tileLocation, string characterToIngore = "")
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.isTilePassable(tileLocation))
					{
						return true;
					}
				}
			}
			return base.isTileOccupied(tileLocation, "");
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0015251C File Offset: 0x0015071C
		public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
		{
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.isTilePassable(tileLocation))
					{
						return true;
					}
				}
			}
			return base.isTileOccupiedForPlacement(tileLocation, toPlace);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00152580 File Offset: 0x00150780
		public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.updateWhenFarmNotCurrentLocation(time);
				}
			}
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x001525DC File Offset: 0x001507DC
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			if (this.wasUpdated && Game1.gameMode != 0)
			{
				return;
			}
			base.UpdateWhenCurrentLocation(time);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Update(time);
				}
			}
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00152644 File Offset: 0x00150844
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.draw(b);
				}
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0015269C File Offset: 0x0015089C
		public void tryToUpgrade(Building toUpgrade, BluePrint blueprint)
		{
			if (toUpgrade != null && blueprint.name != null && toUpgrade.buildingType.Equals(blueprint.nameOfBuildingToUpgrade))
			{
				if (toUpgrade.indoors.farmers.Count > 0)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\Locations:BuildableLocation_CantUpgrade_SomeoneInside", new object[0]), Color.Red, 3500f));
					return;
				}
				toUpgrade.indoors.map = Game1.content.Load<Map>("Maps\\" + blueprint.mapToWarpTo);
				toUpgrade.indoors.name = blueprint.mapToWarpTo;
				toUpgrade.indoors.isStructure = true;
				toUpgrade.nameOfIndoorsWithoutUnique = blueprint.mapToWarpTo;
				toUpgrade.buildingType = blueprint.name;
				toUpgrade.texture = blueprint.texture;
				if (toUpgrade.indoors.GetType() == typeof(AnimalHouse))
				{
					((AnimalHouse)toUpgrade.indoors).resetPositionsOfAllAnimals();
				}
				Game1.playSound("axe");
				blueprint.consumeResources();
				toUpgrade.performActionOnUpgrade(this);
				toUpgrade.color = Color.White;
				Game1.exitActiveMenu();
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastBuildingChange(2, new Vector2((float)toUpgrade.tileX, (float)toUpgrade.tileY), blueprint.name, Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
					return;
				}
				return;
			}
			else
			{
				if (toUpgrade != null)
				{
					Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\Locations:BuildableLocation_CantUpgrade_IncorrectBuildingType", new object[0]), Color.Red, 3500f));
					return;
				}
				return;
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x00152830 File Offset: 0x00150A30
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			using (List<Building>.Enumerator enumerator = this.buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.performActionOnPlayerLocationEntry();
				}
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00152888 File Offset: 0x00150A88
		public bool isBuildingConstructed(string name)
		{
			foreach (Building b in this.buildings)
			{
				if (b.buildingType.Equals(name) && b.daysOfConstructionLeft <= 0 && b.daysUntilUpgrade <= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x001528FC File Offset: 0x00150AFC
		public bool isThereABuildingUnderConstruction()
		{
			foreach (Building b in this.buildings)
			{
				if (b.daysOfConstructionLeft > 0 || b.daysUntilUpgrade > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00152964 File Offset: 0x00150B64
		public Building getBuildingUnderConstruction()
		{
			foreach (Building b in this.buildings)
			{
				if (b.daysOfConstructionLeft > 0 || b.daysUntilUpgrade > 0)
				{
					return b;
				}
			}
			return null;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x001529CC File Offset: 0x00150BCC
		public bool buildStructure(Building b, Vector2 tileLocation, bool serverMessage, Farmer who)
		{
			if (!serverMessage || !Game1.IsClient)
			{
				for (int y = 0; y < b.tilesHigh; y++)
				{
					for (int x = 0; x < b.tilesWide; x++)
					{
						Vector2 currentGlobalTilePosition = new Vector2(tileLocation.X + (float)x, tileLocation.Y + (float)y);
						if (!this.isBuildable(currentGlobalTilePosition))
						{
							return false;
						}
						for (int i = 0; i < this.farmers.Count; i++)
						{
							if (this.farmers[i].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
							{
								return false;
							}
						}
					}
				}
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastBuildingChange(0, tileLocation, b.buildingType, this.name, who.uniqueMultiplayerID);
					if (Game1.IsClient)
					{
						return false;
					}
				}
			}
			string finalCheckResult = b.isThereAnythingtoPreventConstruction(this);
			if (finalCheckResult != null)
			{
				Game1.addHUDMessage(new HUDMessage(finalCheckResult, Color.Red, 3500f));
				return false;
			}
			b.tileX = (int)tileLocation.X;
			b.tileY = (int)tileLocation.Y;
			if (b.indoors != null && b.indoors is AnimalHouse)
			{
				foreach (long a in (b.indoors as AnimalHouse).animalsThatLiveHere)
				{
					FarmAnimal animal = Utility.getAnimal(a);
					if (animal != null)
					{
						animal.homeLocation = tileLocation;
						animal.home = b;
					}
					else if (animal == null && (b.indoors as AnimalHouse).animals.ContainsKey(a))
					{
						animal = (b.indoors as AnimalHouse).animals[a];
						animal.homeLocation = tileLocation;
						animal.home = b;
					}
				}
			}
			if (b.indoors != null)
			{
				foreach (Warp expr_1F9 in b.indoors.warps)
				{
					expr_1F9.TargetX = b.humanDoor.X + b.tileX;
					expr_1F9.TargetY = b.humanDoor.Y + b.tileY + 1;
				}
			}
			this.buildings.Add(b);
			return true;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00152C48 File Offset: 0x00150E48
		public bool isBuildable(Vector2 tileLocation)
		{
			if ((!Game1.player.getTileLocation().Equals(tileLocation) || !Game1.player.currentLocation.Equals(this)) && !this.isTileOccupied(tileLocation, "") && base.isTilePassable(new Location((int)tileLocation.X, (int)tileLocation.Y), Game1.viewport) && base.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "NoFurniture", "Back") == null && !this.caveNoBuildRect.Contains(Utility.Vector2ToPoint(tileLocation)) && !this.shippingAreaNoBuildRect.Contains(Utility.Vector2ToPoint(tileLocation)))
			{
				if (Game1.currentLocation.doesTileHavePropertyNoNull((int)tileLocation.X, (int)tileLocation.Y, "Buildable", "Back").ToLower().Equals("t") || Game1.currentLocation.doesTileHavePropertyNoNull((int)tileLocation.X, (int)tileLocation.Y, "Buildable", "Back").ToLower().Equals("true"))
				{
					return true;
				}
				if (Game1.currentLocation.doesTileHaveProperty((int)tileLocation.X, (int)tileLocation.Y, "Diggable", "Back") != null && !Game1.currentLocation.doesTileHavePropertyNoNull((int)tileLocation.X, (int)tileLocation.Y, "Buildable", "Back").ToLower().Equals("f"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00152DC4 File Offset: 0x00150FC4
		public bool buildStructure(BluePrint structureForPlacement, Vector2 tileLocation, bool serverMessage, Farmer who, bool magicalConstruction = false)
		{
			if (!serverMessage || !Game1.IsClient)
			{
				for (int y = 0; y < structureForPlacement.tilesHeight; y++)
				{
					for (int x = 0; x < structureForPlacement.tilesWidth; x++)
					{
						Vector2 currentGlobalTilePosition = new Vector2(tileLocation.X + (float)x, tileLocation.Y + (float)y);
						if (!this.isBuildable(currentGlobalTilePosition))
						{
							return false;
						}
						for (int i = 0; i < this.farmers.Count; i++)
						{
							if (this.farmers[i].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
							{
								return false;
							}
						}
					}
				}
				if (Game1.IsMultiplayer)
				{
					MultiplayerUtility.broadcastBuildingChange(0, tileLocation, structureForPlacement.name, this.name, who.uniqueMultiplayerID);
					if (Game1.IsClient)
					{
						return false;
					}
				}
			}
			string name = structureForPlacement.name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			Building b;
			if (num <= 1972213674u)
			{
				if (num <= 846075854u)
				{
					if (num != 45101750u)
					{
						if (num != 846075854u)
						{
							goto IL_251;
						}
						if (!(name == "Big Barn"))
						{
							goto IL_251;
						}
						goto IL_233;
					}
					else
					{
						if (!(name == "Stable"))
						{
							goto IL_251;
						}
						b = new Stable(structureForPlacement, tileLocation);
						goto IL_259;
					}
				}
				else if (num != 1684694008u)
				{
					if (num != 1972213674u)
					{
						goto IL_251;
					}
					if (!(name == "Big Coop"))
					{
						goto IL_251;
					}
				}
				else if (!(name == "Coop"))
				{
					goto IL_251;
				}
			}
			else if (num <= 2601855023u)
			{
				if (num != 2575064728u)
				{
					if (num != 2601855023u)
					{
						goto IL_251;
					}
					if (!(name == "Deluxe Barn"))
					{
						goto IL_251;
					}
					goto IL_233;
				}
				else
				{
					if (!(name == "Junimo Hut"))
					{
						goto IL_251;
					}
					b = new JunimoHut(structureForPlacement, tileLocation);
					goto IL_259;
				}
			}
			else if (num != 3183088828u)
			{
				if (num != 3734277467u)
				{
					if (num != 3933183203u)
					{
						goto IL_251;
					}
					if (!(name == "Mill"))
					{
						goto IL_251;
					}
					b = new Mill(structureForPlacement, tileLocation);
					goto IL_259;
				}
				else if (!(name == "Deluxe Coop"))
				{
					goto IL_251;
				}
			}
			else
			{
				if (!(name == "Barn"))
				{
					goto IL_251;
				}
				goto IL_233;
			}
			b = new Coop(structureForPlacement, tileLocation);
			goto IL_259;
			IL_233:
			b = new Barn(structureForPlacement, tileLocation);
			goto IL_259;
			IL_251:
			b = new Building(structureForPlacement, tileLocation);
			IL_259:
			b.owner = who.uniqueMultiplayerID;
			string finalCheckResult = b.isThereAnythingtoPreventConstruction(this);
			if (finalCheckResult != null)
			{
				Game1.addHUDMessage(new HUDMessage(finalCheckResult, Color.Red, 3500f));
				return false;
			}
			this.buildings.Add(b);
			b.performActionOnConstruction(this);
			return true;
		}

		// Token: 0x040011CD RID: 4557
		public List<Building> buildings = new List<Building>();

		// Token: 0x040011CE RID: 4558
		private Microsoft.Xna.Framework.Rectangle caveNoBuildRect = new Microsoft.Xna.Framework.Rectangle(32, 8, 5, 3);

		// Token: 0x040011CF RID: 4559
		private Microsoft.Xna.Framework.Rectangle shippingAreaNoBuildRect = new Microsoft.Xna.Framework.Rectangle(69, 10, 4, 4);
	}
}
