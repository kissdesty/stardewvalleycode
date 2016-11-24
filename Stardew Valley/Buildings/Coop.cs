using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace StardewValley.Buildings
{
	// Token: 0x0200014A RID: 330
	public class Coop : Building
	{
		// Token: 0x060012BA RID: 4794 RVA: 0x00178B10 File Offset: 0x00176D10
		public Coop(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00178B1A File Offset: 0x00176D1A
		public Coop()
		{
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0017C244 File Offset: 0x0017A444
		protected override GameLocation getIndoors()
		{
			if (this.indoors != null)
			{
				this.nameOfIndoorsWithoutUnique = this.indoors.name;
			}
			string nameOfIndoorsWithoutUnique = this.nameOfIndoorsWithoutUnique;
			if (!(nameOfIndoorsWithoutUnique == "Big Coop"))
			{
				if (nameOfIndoorsWithoutUnique == "Deluxe Coop")
				{
					this.nameOfIndoorsWithoutUnique = "Coop3";
				}
			}
			else
			{
				this.nameOfIndoorsWithoutUnique = "Coop2";
			}
			GameLocation indoors = new AnimalHouse(Game1.content.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
			indoors.IsFarm = true;
			indoors.isStructure = true;
			nameOfIndoorsWithoutUnique = this.nameOfIndoorsWithoutUnique;
			if (!(nameOfIndoorsWithoutUnique == "Big Coop"))
			{
				if (nameOfIndoorsWithoutUnique == "Deluxe Coop")
				{
					(indoors as AnimalHouse).animalLimit = 12;
				}
			}
			else
			{
				(indoors as AnimalHouse).animalLimit = 8;
			}
			foreach (Warp expr_DB in indoors.warps)
			{
				expr_DB.TargetX = this.humanDoor.X + this.tileX;
				expr_DB.TargetY = this.humanDoor.Y + this.tileY + 1;
			}
			if (this.animalDoorOpen)
			{
				this.yPositionOfAnimalDoor = Coop.openAnimalDoorPosition;
			}
			if ((indoors as AnimalHouse).incubatingEgg.Y > 0)
			{
				indoors.map.GetLayer("Front").Tiles[1, 2].TileIndex += ((Game1.player.ActiveObject.ParentSheetIndex == 180 || Game1.player.ActiveObject.ParentSheetIndex == 182) ? 2 : 1);
			}
			return indoors;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0017C408 File Offset: 0x0017A608
		public override void performActionOnConstruction(GameLocation location)
		{
			base.performActionOnConstruction(location);
			Object o = new Object(new Vector2(3f, 3f), 99, false);
			o.fragility = 2;
			this.indoors.objects.Add(new Vector2(3f, 3f), o);
			this.daysOfConstructionLeft = 3;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0017C464 File Offset: 0x0017A664
		public override void performActionOnUpgrade(GameLocation location)
		{
			(this.indoors as AnimalHouse).animalLimit += 4;
			if ((this.indoors as AnimalHouse).animalLimit == 8)
			{
				Object o = new Object(new Vector2(2f, 3f), 104, false);
				o.fragility = 2;
				this.indoors.objects.Add(new Vector2(2f, 3f), o);
				this.indoors.moveObject(1, 3, 14, 7);
				return;
			}
			this.indoors.moveObject(14, 7, 21, 7);
			this.indoors.moveObject(14, 8, 21, 8);
			this.indoors.moveObject(14, 4, 20, 4);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0017C523 File Offset: 0x0017A723
		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height - 16);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0017C550 File Offset: 0x0017A750
		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (this.daysOfConstructionLeft <= 0 && tileLocation.X == (float)(this.tileX + this.animalDoor.X) && tileLocation.Y == (float)(this.tileY + this.animalDoor.Y))
			{
				if (!this.animalDoorOpen)
				{
					Game1.playSound("doorCreak");
				}
				else
				{
					Game1.playSound("doorCreakReverse");
				}
				this.animalDoorOpen = !this.animalDoorOpen;
				this.animalDoorMotion = (this.animalDoorOpen ? -2 : 2);
				return true;
			}
			return base.doAction(tileLocation, who);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0017C5E6 File Offset: 0x0017A7E6
		public override void updateWhenFarmNotCurrentLocation(GameTime time)
		{
			base.updateWhenFarmNotCurrentLocation(time);
			((AnimalHouse)this.indoors).updateWhenNotCurrentLocation(this, time);
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0017C604 File Offset: 0x0017A804
		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			if (this.daysOfConstructionLeft <= 0)
			{
				if ((this.indoors as AnimalHouse).incubatingEgg.Y > 0)
				{
					AnimalHouse expr_43_cp_0_cp_0 = this.indoors as AnimalHouse;
					expr_43_cp_0_cp_0.incubatingEgg.X = expr_43_cp_0_cp_0.incubatingEgg.X - 1;
					if ((this.indoors as AnimalHouse).incubatingEgg.X <= 0)
					{
						long id = MultiplayerUtility.getNewID();
						FarmAnimal layed = new FarmAnimal(((this.indoors as AnimalHouse).incubatingEgg.Y == 442) ? "Duck" : (((this.indoors as AnimalHouse).incubatingEgg.Y == 180 || (this.indoors as AnimalHouse).incubatingEgg.Y == 182) ? "BrownChicken" : (((this.indoors as AnimalHouse).incubatingEgg.Y == 107) ? "Dinosaur" : "Chicken")), id, this.owner);
						(this.indoors as AnimalHouse).incubatingEgg.X = 0;
						(this.indoors as AnimalHouse).incubatingEgg.Y = -1;
						this.indoors.map.GetLayer("Front").Tiles[1, 2].TileIndex = 45;
						((AnimalHouse)this.indoors).animals.Add(id, layed);
					}
				}
				if ((this.indoors as AnimalHouse).animalLimit == 16)
				{
					int arg_1A6_0 = (this.indoors as AnimalHouse).animals.Count;
					int numExistingHay = this.indoors.numberOfObjectsWithName("Hay");
					int piecesHay = Math.Min(arg_1A6_0 - numExistingHay, (Game1.getLocationFromName("Farm") as Farm).piecesOfHay);
					(Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= piecesHay;
					int i = 0;
					while (i < 16 && piecesHay > 0)
					{
						Vector2 tile = new Vector2((float)(6 + i), 3f);
						if (!this.indoors.objects.ContainsKey(tile))
						{
							this.indoors.objects.Add(tile, new Object(178, 1, false, -1, 0));
						}
						piecesHay--;
						i++;
					}
				}
			}
			this.currentOccupants = ((AnimalHouse)this.indoors).animals.Count;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0017C868 File Offset: 0x0017AA68
		public override void Update(GameTime time)
		{
			base.Update(time);
			if (this.animalDoorMotion != 0)
			{
				if (this.animalDoorOpen && this.yPositionOfAnimalDoor <= Coop.openAnimalDoorPosition)
				{
					this.animalDoorMotion = 0;
					this.yPositionOfAnimalDoor = Coop.openAnimalDoorPosition;
				}
				else if (!this.animalDoorOpen && this.yPositionOfAnimalDoor >= 0)
				{
					this.animalDoorMotion = 0;
					this.yPositionOfAnimalDoor = 0;
				}
				this.yPositionOfAnimalDoor += this.animalDoorMotion;
			}
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0017C8E0 File Offset: 0x0017AAE0
		public override void upgrade()
		{
			base.upgrade();
			if (this.buildingType.Equals("Big Coop"))
			{
				this.indoors.moveObject(2, 3, 14, 8);
				this.indoors.moveObject(1, 3, 14, 7);
				this.indoors.moveObject(10, 4, 14, 4);
				this.indoors.objects.Add(new Vector2(2f, 3f), new Object(new Vector2(2f, 3f), 101, false));
				if (!Game1.player.hasOrWillReceiveMail("incubator"))
				{
					Game1.mailbox.Enqueue("incubator");
				}
			}
			if ((this.indoors as AnimalHouse).animalLimit != 8)
			{
				this.indoors.moveObject(14, 7, 21, 7);
				this.indoors.moveObject(14, 8, 21, 8);
				this.indoors.moveObject(14, 4, 20, 4);
			}
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0017C9DC File Offset: 0x0017ABDC
		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y) + new Vector2((float)this.animalDoor.X, (float)(this.animalDoor.Y + 4)) * (float)Game1.tileSize, new Rectangle?(new Rectangle(16, 112, 16, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
			b.Draw(this.texture, new Vector2((float)x, (float)y) + new Vector2((float)this.animalDoor.X, (float)this.animalDoor.Y + 3.5f) * (float)Game1.tileSize, new Rectangle?(new Rectangle(0, 112, 16, 15)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 1E-07f);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(0, 0, 96, 112)), this.color, 0f, new Vector2(0f, 0f), 4f, SpriteEffects.None, 0.89f);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0017CB3A File Offset: 0x0017AD3A
		public override Vector2 getUpgradeSignLocation()
		{
			return new Vector2((float)this.tileX, (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 2), (float)Game1.pixelZoom);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0017CB74 File Offset: 0x0017AD74
		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX + this.animalDoor.X), (float)(this.tileY + this.animalDoor.Y)) * (float)Game1.tileSize), new Rectangle?(new Rectangle(16, 112, 16, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((this.tileX + this.animalDoor.X) * Game1.tileSize), (float)((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor))), new Rectangle?(new Rectangle(0, 112, 16, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 1E-07f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(new Rectangle(0, 0, 96, 112)), this.color * this.alpha, 0f, new Vector2(0f, 112f), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
			if (this.daysUntilUpgrade > 0)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.getUpgradeSignLocation()), new Rectangle?(new Rectangle(367, 309, 16, 15)), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
			}
		}

		// Token: 0x0400133B RID: 4923
		public static int openAnimalDoorPosition = -Game1.tileSize + Game1.pixelZoom * 3;

		// Token: 0x0400133C RID: 4924
		private const int closedAnimalDoorPosition = 0;

		// Token: 0x0400133D RID: 4925
		private int yPositionOfAnimalDoor;

		// Token: 0x0400133E RID: 4926
		private int animalDoorMotion;
	}
}
