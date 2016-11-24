using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace StardewValley.Buildings
{
	// Token: 0x0200014B RID: 331
	public class Barn : Building
	{
		// Token: 0x060012C9 RID: 4809 RVA: 0x00178B10 File Offset: 0x00176D10
		public Barn(BluePrint b, Vector2 tileLocation) : base(b, tileLocation)
		{
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00178B1A File Offset: 0x00176D1A
		public Barn()
		{
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0017CDEC File Offset: 0x0017AFEC
		protected override GameLocation getIndoors()
		{
			if (this.indoors != null)
			{
				this.nameOfIndoorsWithoutUnique = this.indoors.name;
			}
			string nameOfIndoorsWithoutUnique = this.nameOfIndoorsWithoutUnique;
			if (!(nameOfIndoorsWithoutUnique == "Big Barn"))
			{
				if (nameOfIndoorsWithoutUnique == "Deluxe Barn")
				{
					this.nameOfIndoorsWithoutUnique = "Barn3";
				}
			}
			else
			{
				this.nameOfIndoorsWithoutUnique = "Barn2";
			}
			GameLocation indoors = new AnimalHouse(Game1.content.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
			indoors.IsFarm = true;
			indoors.isStructure = true;
			nameOfIndoorsWithoutUnique = this.nameOfIndoorsWithoutUnique;
			if (!(nameOfIndoorsWithoutUnique == "Big Barn"))
			{
				if (nameOfIndoorsWithoutUnique == "Deluxe Barn")
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
				this.yPositionOfAnimalDoor = Barn.openAnimalDoorPosition;
			}
			return indoors;
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0017CF44 File Offset: 0x0017B144
		public override Rectangle getRectForAnimalDoor()
		{
			return new Rectangle((this.animalDoor.X + this.tileX) * Game1.tileSize, (this.tileY + this.animalDoor.Y) * Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0017CF94 File Offset: 0x0017B194
		public override bool doAction(Vector2 tileLocation, Farmer who)
		{
			if (this.daysOfConstructionLeft <= 0 && (tileLocation.X == (float)(this.tileX + this.animalDoor.X) || tileLocation.X == (float)(this.tileX + this.animalDoor.X + 1)) && tileLocation.Y == (float)(this.tileY + this.animalDoor.Y))
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
				this.animalDoorMotion = (this.animalDoorOpen ? -3 : 2);
				return true;
			}
			return base.doAction(tileLocation, who);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0017C5E6 File Offset: 0x0017A7E6
		public override void updateWhenFarmNotCurrentLocation(GameTime time)
		{
			base.updateWhenFarmNotCurrentLocation(time);
			((AnimalHouse)this.indoors).updateWhenNotCurrentLocation(this, time);
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0017C523 File Offset: 0x0017A723
		public override Rectangle getSourceRectForMenu()
		{
			return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height - 16);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0017D04C File Offset: 0x0017B24C
		public override void performActionOnUpgrade(GameLocation location)
		{
			(this.indoors as AnimalHouse).animalLimit += 4;
			if ((this.indoors as AnimalHouse).animalLimit == 8)
			{
				Object o = new Object(new Vector2(1f, 3f), 104, false);
				o.fragility = 2;
				this.indoors.objects.Add(new Vector2(1f, 3f), o);
			}
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0017D0C4 File Offset: 0x0017B2C4
		public override void performActionOnConstruction(GameLocation location)
		{
			base.performActionOnConstruction(location);
			Object o = new Object(new Vector2(6f, 3f), 99, false);
			o.fragility = 2;
			this.indoors.objects.Add(new Vector2(6f, 3f), o);
			this.daysOfConstructionLeft = 3;
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0017D120 File Offset: 0x0017B320
		public override void dayUpdate(int dayOfMonth)
		{
			base.dayUpdate(dayOfMonth);
			if (this.daysOfConstructionLeft <= 0)
			{
				this.currentOccupants = ((AnimalHouse)this.indoors).animals.Count;
				if ((this.indoors as AnimalHouse).animalLimit == 16)
				{
					int arg_6C_0 = (this.indoors as AnimalHouse).animals.Count;
					int numExistingHay = this.indoors.numberOfObjectsWithName("Hay");
					int piecesHay = Math.Min(arg_6C_0 - numExistingHay, (Game1.getLocationFromName("Farm") as Farm).piecesOfHay);
					(Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= piecesHay;
					int i = 0;
					while (i < 16 && piecesHay > 0)
					{
						Vector2 tile = new Vector2((float)(8 + i), 3f);
						if (!this.indoors.objects.ContainsKey(tile))
						{
							this.indoors.objects.Add(tile, new Object(178, 1, false, -1, 0));
						}
						piecesHay--;
						i++;
					}
				}
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0017D228 File Offset: 0x0017B428
		public override void Update(GameTime time)
		{
			base.Update(time);
			if (this.animalDoorMotion != 0)
			{
				if (this.animalDoorOpen && this.yPositionOfAnimalDoor <= Barn.openAnimalDoorPosition)
				{
					this.animalDoorMotion = 0;
					this.yPositionOfAnimalDoor = Barn.openAnimalDoorPosition;
				}
				else if (!this.animalDoorOpen && this.yPositionOfAnimalDoor >= 0)
				{
					this.animalDoorMotion = 0;
					this.yPositionOfAnimalDoor = 0;
				}
				this.yPositionOfAnimalDoor += this.animalDoorMotion;
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0017D2A0 File Offset: 0x0017B4A0
		public override void upgrade()
		{
			if (this.buildingType.Equals("Big Barn"))
			{
				this.animalDoor.X = this.animalDoor.X + 1;
				this.indoors.moveObject(15, 3, 18, 13);
				this.indoors.moveObject(16, 3, 19, 13);
				this.indoors.moveObject(1, 4, 20, 3);
				for (int i = 4; i < 13; i++)
				{
					this.indoors.moveObject(16, i, 20, i);
				}
				return;
			}
			this.indoors.moveObject(20, 3, 1, 4);
			for (int j = 6; j < 12; j++)
			{
				this.indoors.moveObject(20, j, 23, j);
			}
			this.indoors.moveObject(20, 4, 20, 13);
			this.indoors.moveObject(20, 5, 21, 13);
			this.indoors.moveObject(20, 12, 22, 13);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0017D390 File Offset: 0x0017B590
		public override Vector2 getUpgradeSignLocation()
		{
			return new Vector2((float)this.tileX, (float)(this.tileY + 1)) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize * 3), (float)Game1.pixelZoom);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0017D3CC File Offset: 0x0017B5CC
		public override void drawInMenu(SpriteBatch b, int x, int y)
		{
			this.drawShadow(b, x, y);
			b.Draw(this.texture, new Vector2((float)x, (float)y) + new Vector2((float)this.animalDoor.X, (float)(this.animalDoor.Y + 3)) * (float)Game1.tileSize, new Rectangle?(new Rectangle(64, 112, 32, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.888f);
			b.Draw(this.texture, new Vector2((float)x, (float)y) + new Vector2((float)this.animalDoor.X, (float)this.animalDoor.Y + 2.25f) * (float)Game1.tileSize, new Rectangle?(new Rectangle(0, 112, 32, 16)), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f - 1E-07f);
			b.Draw(this.texture, new Vector2((float)x, (float)y), new Rectangle?(new Rectangle(0, 0, 112, 112)), this.color, 0f, new Vector2(0f, 0f), 4f, SpriteEffects.None, 0.89f);
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0017D52C File Offset: 0x0017B72C
		public override void draw(SpriteBatch b)
		{
			if (this.daysOfConstructionLeft > 0)
			{
				base.drawInConstruction(b);
				return;
			}
			this.drawShadow(b, -1, -1);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX + this.animalDoor.X), (float)(this.tileY + this.animalDoor.Y - 1)) * (float)Game1.tileSize), new Rectangle?(new Rectangle(32, 112, 32, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX + this.animalDoor.X), (float)(this.tileY + this.animalDoor.Y)) * (float)Game1.tileSize), new Rectangle?(new Rectangle(64, 112, 32, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((this.tileX + this.animalDoor.X) * Game1.tileSize), (float)((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor - Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(0, 112, 32, 12)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 0.0001f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)((this.tileX + this.animalDoor.X) * Game1.tileSize), (float)((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor))), new Rectangle?(new Rectangle(0, 112, 32, 16)), Color.White * this.alpha, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 0.0001f);
			b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(this.tileX * Game1.tileSize), (float)(this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(new Rectangle(0, 0, 112, 112)), this.color * this.alpha, 0f, new Vector2(0f, 112f), 4f, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f - 1E-05f);
			if (this.daysUntilUpgrade > 0)
			{
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.getUpgradeSignLocation()), new Rectangle?(new Rectangle(367, 309, 16, 15)), Color.White * this.alpha, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f + 0.0001f);
			}
		}

		// Token: 0x0400133F RID: 4927
		public static int openAnimalDoorPosition = -Game1.tileSize - 24 + Game1.pixelZoom * 3;

		// Token: 0x04001340 RID: 4928
		private const int closedAnimalDoorPosition = 0;

		// Token: 0x04001341 RID: 4929
		private int yPositionOfAnimalDoor;

		// Token: 0x04001342 RID: 4930
		private int animalDoorMotion;
	}
}
