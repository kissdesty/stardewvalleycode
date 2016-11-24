using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;

namespace StardewValley
{
	// Token: 0x02000021 RID: 33
	public class CraftingRecipe
	{
		// Token: 0x06000173 RID: 371 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public CraftingRecipe(string name, bool isCookingRecipe)
		{
			this.isCookingRecipe = isCookingRecipe;
			this.name = name;
			string info = (isCookingRecipe && CraftingRecipe.cookingRecipes.ContainsKey(name)) ? CraftingRecipe.cookingRecipes[name] : (CraftingRecipe.craftingRecipes.ContainsKey(name) ? CraftingRecipe.craftingRecipes[name] : null);
			if (info == null)
			{
				this.name = "Torch";
				name = "Torch";
				info = CraftingRecipe.craftingRecipes[name];
			}
			string[] infoSplit = info.Split(new char[]
			{
				'/'
			});
			string[] ingredientsSplit = infoSplit[0].Split(new char[]
			{
				' '
			});
			for (int i = 0; i < ingredientsSplit.Length; i += 2)
			{
				this.recipeList.Add(Convert.ToInt32(ingredientsSplit[i]), Convert.ToInt32(ingredientsSplit[i + 1]));
			}
			string[] itemToProduceList = infoSplit[2].Split(new char[]
			{
				' '
			});
			for (int j = 0; j < itemToProduceList.Length; j += 2)
			{
				this.itemToProduce.Add(Convert.ToInt32(itemToProduceList[j]));
				this.numberProducedPerCraft = ((itemToProduceList.Length > 1) ? Convert.ToInt32(itemToProduceList[j + 1]) : 1);
			}
			this.bigCraftable = (!isCookingRecipe && Convert.ToBoolean(infoSplit[3]));
			try
			{
				bool isRing = this.itemToProduce[0] >= 516 && this.itemToProduce[0] <= 534;
				this.description = (this.bigCraftable ? Game1.bigCraftablesInformation[this.itemToProduce[0]].Split(new char[]
				{
					'/'
				})[4] : Game1.objectInformation[this.itemToProduce[0]].Split(new char[]
				{
					'/'
				})[isRing ? 1 : 4]);
			}
			catch (Exception)
			{
				this.description = "";
			}
			this.timesCrafted = (Game1.player.craftingRecipes.ContainsKey(name) ? Game1.player.craftingRecipes[name] : 0);
			if (name.Equals("Crab Pot") && Game1.player.professions.Contains(7))
			{
				this.recipeList = new Dictionary<int, int>();
				this.recipeList.Add(388, 25);
				this.recipeList.Add(334, 2);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00010838 File Offset: 0x0000EA38
		public int getIndexOfMenuView()
		{
			if (this.itemToProduce.Count <= 0)
			{
				return -1;
			}
			return this.itemToProduce[0];
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00010858 File Offset: 0x0000EA58
		public bool doesFarmerHaveIngredientsInInventory(List<Item> extraToCheck = null)
		{
			foreach (KeyValuePair<int, int> kvp in this.recipeList)
			{
				if (!Game1.player.hasItemInInventory(kvp.Key, kvp.Value, 5) && (extraToCheck == null || !Game1.player.hasItemInList(extraToCheck, kvp.Key, kvp.Value, 5)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000108E8 File Offset: 0x0000EAE8
		public void drawMenuView(SpriteBatch b, int x, int y, float layerDepth = 0.88f, bool shadow = true)
		{
			if (this.bigCraftable)
			{
				Utility.drawWithShadow(b, Game1.bigCraftableSpriteSheet, new Vector2((float)x, (float)y), Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.getIndexOfMenuView(), 16, 32), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, layerDepth, -1, -1, 0.35f);
				return;
			}
			Utility.drawWithShadow(b, Game1.objectSpriteSheet, new Vector2((float)x, (float)y), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIndexOfMenuView(), 16, 16), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, layerDepth, -1, -1, 0.35f);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0001098C File Offset: 0x0000EB8C
		public Item createItem()
		{
			int index = this.itemToProduce.ElementAt(Game1.random.Next(this.itemToProduce.Count));
			if (this.bigCraftable)
			{
				if (this.name.Equals("Chest"))
				{
					return new Chest(true);
				}
				return new Object(Vector2.Zero, index, false);
			}
			else
			{
				if (this.name.Equals("Torch"))
				{
					return new Torch(Vector2.Zero, this.numberProducedPerCraft);
				}
				if (index >= 516 && index <= 534)
				{
					return new Ring(index);
				}
				return new Object(Vector2.Zero, index, this.numberProducedPerCraft);
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00010A34 File Offset: 0x0000EC34
		public void consumeIngredients()
		{
			for (int i = this.recipeList.Count - 1; i >= 0; i--)
			{
				int originalToRemove = this.recipeList[this.recipeList.Keys.ElementAt(i)];
				bool foundInBackpack = false;
				for (int j = Game1.player.items.Count - 1; j >= 0; j--)
				{
					if (Game1.player.items[j] != null && Game1.player.items[j] is Object && !(Game1.player.items[j] as Object).bigCraftable && (((Object)Game1.player.items[j]).parentSheetIndex == this.recipeList.Keys.ElementAt(i) || ((Object)Game1.player.items[j]).category == this.recipeList.Keys.ElementAt(i)))
					{
						int toRemove = this.recipeList[this.recipeList.Keys.ElementAt(i)];
						Dictionary<int, int> dictionary = this.recipeList;
						int key = this.recipeList.Keys.ElementAt(i);
						dictionary[key] -= Game1.player.items[j].Stack;
						Game1.player.items[j].Stack -= toRemove;
						if (Game1.player.items[j].Stack <= 0)
						{
							Game1.player.items[j] = null;
						}
						if (this.recipeList[this.recipeList.Keys.ElementAt(i)] <= 0)
						{
							this.recipeList[this.recipeList.Keys.ElementAt(i)] = originalToRemove;
							foundInBackpack = true;
							break;
						}
					}
				}
				if (this.isCookingRecipe && !foundInBackpack)
				{
					FarmHouse f = Utility.getHomeOfFarmer(Game1.player);
					if (f != null)
					{
						for (int k = f.fridge.items.Count - 1; k >= 0; k--)
						{
							if (f.fridge.items[k] != null && f.fridge.items[k] is Object && (((Object)f.fridge.items[k]).parentSheetIndex == this.recipeList.Keys.ElementAt(i) || ((Object)f.fridge.items[k]).category == this.recipeList.Keys.ElementAt(i)))
							{
								int toRemove2 = this.recipeList[this.recipeList.Keys.ElementAt(i)];
								Dictionary<int, int> dictionary = this.recipeList;
								int key = this.recipeList.Keys.ElementAt(i);
								dictionary[key] -= f.fridge.items[k].Stack;
								f.fridge.items[k].Stack -= toRemove2;
								if (f.fridge.items[k].Stack <= 0)
								{
									f.fridge.items[k] = null;
								}
								if (this.recipeList[this.recipeList.Keys.ElementAt(i)] <= 0)
								{
									this.recipeList[this.recipeList.Keys.ElementAt(i)] = originalToRemove;
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00010E0C File Offset: 0x0000F00C
		public int getDescriptionHeight(int width)
		{
			return (int)(Game1.smallFont.MeasureString(Game1.parseText(this.description, Game1.smallFont, width)).Y + (float)(this.getNumberOfIngredients() * (Game1.tileSize / 2 + Game1.pixelZoom)) + (float)((int)Game1.smallFont.MeasureString("Ingredients").Y) + (float)(Game1.tileSize / 3));
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00010E70 File Offset: 0x0000F070
		public void drawRecipeDescription(SpriteBatch b, Vector2 position, int width)
		{
			b.Draw(Game1.staminaRect, new Rectangle((int)(position.X + 8f), (int)(position.Y + (float)(Game1.tileSize / 2) + Game1.smallFont.MeasureString("Ing").Y) - Game1.pixelZoom - 2, width - Game1.tileSize / 2, Game1.pixelZoom / 2), Game1.textColor * 0.35f);
			Utility.drawTextWithShadow(b, "Ingredients:", Game1.smallFont, position + new Vector2(8f, (float)(Game1.tileSize / 2 - Game1.pixelZoom)), Game1.textColor * 0.75f, 1f, -1f, -1, -1, 1f, 3);
			for (int i = 0; i < this.recipeList.Count; i++)
			{
				Color drawColor = Game1.player.hasItemInInventory(this.recipeList.Keys.ElementAt(i), this.recipeList.Values.ElementAt(i), 8) ? Game1.textColor : Color.Red;
				if (this.isCookingRecipe && Game1.player.hasItemInList(Utility.getHomeOfFarmer(Game1.player).fridge.items, this.recipeList.Keys.ElementAt(i), this.recipeList.Values.ElementAt(i), 8))
				{
					drawColor = Game1.textColor;
				}
				b.Draw(Game1.objectSpriteSheet, new Vector2(position.X, position.Y + (float)Game1.tileSize + (float)(i * Game1.tileSize / 2) + (float)(i * 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getSpriteIndexFromRawIndex(this.recipeList.Keys.ElementAt(i)), 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom / 2f, SpriteEffects.None, 0.86f);
				Game1.drawWithBorder(string.Concat(this.recipeList.Values.ElementAt(i)), Game1.textColor, Color.AntiqueWhite, new Vector2(position.X + (float)(Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat(this.recipeList.Values.ElementAt(i))).X, position.Y + (float)Game1.tileSize + (float)(i * Game1.tileSize / 2) + (float)(i * 4) + (float)(Game1.tileSize / 5)), 0f, 1f, 0.87f, true);
				Utility.drawTextWithShadow(b, this.getNameFromIndex(this.recipeList.Keys.ElementAt(i)), Game1.smallFont, new Vector2(position.X + (float)(Game1.tileSize / 2) + 8f, position.Y + (float)Game1.tileSize + (float)(i * Game1.tileSize / 2) + (float)(i * 4) + 4f), drawColor, 1f, -1f, -1, -1, 1f, 3);
			}
			b.Draw(Game1.staminaRect, new Rectangle((int)position.X + 8, (int)position.Y + Game1.tileSize + Game1.pixelZoom + this.recipeList.Count * (Game1.tileSize / 2 + 4), width - Game1.tileSize / 2, Game1.pixelZoom / 2), Game1.textColor * 0.35f);
			Utility.drawTextWithShadow(b, Game1.parseText(this.description, Game1.smallFont, width - 8), Game1.smallFont, position + new Vector2(0f, (float)(Game1.tileSize + Game1.pixelZoom * 3 + this.recipeList.Count * (Game1.tileSize / 2 + 4))), Game1.textColor * 0.75f, 1f, -1f, -1, -1, 1f, 3);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00011241 File Offset: 0x0000F441
		public int getNumberOfIngredients()
		{
			return this.recipeList.Count;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00011250 File Offset: 0x0000F450
		public int getSpriteIndexFromRawIndex(int index)
		{
			switch (index)
			{
			case -6:
				return 184;
			case -5:
				return 176;
			case -4:
				return 145;
			case -3:
				return 24;
			case -2:
				return 80;
			case -1:
				return 20;
			default:
				return index;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0001129C File Offset: 0x0000F49C
		public string getNameFromIndex(int index)
		{
			if (index < 0)
			{
				switch (index)
				{
				case -6:
					return "Milk (Any)";
				case -5:
					return "Egg (Any)";
				case -4:
					return "Fish (Any)";
				case -3:
					return "Vegetable (Any)";
				case -2:
					return "Gem (Any)";
				case -1:
					return "Greens (Any)";
				default:
					return "???";
				}
			}
			else
			{
				if (!Game1.objectInformation.ContainsKey(index))
				{
					return "Error Item";
				}
				return Game1.objectInformation[index].Split(new char[]
				{
					'/'
				})[0];
			}
		}

		// Token: 0x04000193 RID: 403
		public string name;

		// Token: 0x04000194 RID: 404
		private string description;

		// Token: 0x04000195 RID: 405
		public static Dictionary<string, string> craftingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CraftingRecipes");

		// Token: 0x04000196 RID: 406
		public static Dictionary<string, string> cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CookingRecipes");

		// Token: 0x04000197 RID: 407
		private Dictionary<int, int> recipeList = new Dictionary<int, int>();

		// Token: 0x04000198 RID: 408
		private List<int> itemToProduce = new List<int>();

		// Token: 0x04000199 RID: 409
		public bool bigCraftable;

		// Token: 0x0400019A RID: 410
		public bool isCookingRecipe;

		// Token: 0x0400019B RID: 411
		public int timesCrafted;

		// Token: 0x0400019C RID: 412
		public int numberProducedPerCraft;
	}
}
