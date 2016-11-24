using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.TerrainFeatures
{
	// Token: 0x02000078 RID: 120
	[XmlInclude(typeof(Grass)), XmlInclude(typeof(Tree)), XmlInclude(typeof(Quartz)), XmlInclude(typeof(Stalagmite)), XmlInclude(typeof(HoeDirt)), XmlInclude(typeof(Flooring)), XmlInclude(typeof(CosmeticPlant)), XmlInclude(typeof(ResourceClump)), XmlInclude(typeof(GiantCrop)), XmlInclude(typeof(FruitTree)), XmlInclude(typeof(Bush))]
	public abstract class TerrainFeature
	{
		// Token: 0x060009E1 RID: 2529 RVA: 0x0000282C File Offset: 0x00000A2C
		public TerrainFeature()
		{
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x000D20EC File Offset: 0x000D02EC
		public virtual Rectangle getBoundingBox(Vector2 tileLocation)
		{
			return new Rectangle((int)tileLocation.X * Game1.tileSize, (int)tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void loadSprite()
		{
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool isPassable(Character c = null)
		{
			return false;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
		{
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool performUseAction(Vector2 tileLocation)
		{
			return false;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
		{
			return false;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool tickUpdate(GameTime time, Vector2 tileLocation)
		{
			return false;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void dayUpdate(GameLocation environment, Vector2 tileLocation)
		{
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool seasonUpdate(bool onLoad)
		{
			return false;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool isActionable()
		{
			return false;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void performPlayerEntryAction(Vector2 tileLocation)
		{
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
		{
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0000821B File Offset: 0x0000641B
		public virtual bool forceDraw()
		{
			return false;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
		{
		}
	}
}
