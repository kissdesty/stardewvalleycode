using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using xTile;
using xTile.Dimensions;
using xTile.Layers;

namespace StardewValley.Minigames
{
	// Token: 0x020000D2 RID: 210
	public class TelescopeScene : IMinigame
	{
		// Token: 0x06000D56 RID: 3414 RVA: 0x0010D80C File Offset: 0x0010BA0C
		public TelescopeScene(NPC Maru)
		{
			this.temporaryContent = Game1.content.CreateTemporary();
			this.background = this.temporaryContent.Load<Texture2D>("LooseSprites\\nightSceneMaru");
			this.trees = this.temporaryContent.Load<Texture2D>("LooseSprites\\nightSceneMaruTrees");
			Map map = new Map();
			map.AddLayer(new Layer("Back", map, new Size(30, 1), new Size(Game1.tileSize)));
			this.walkSpace = new GameLocation(map, "walkSpace");
			Game1.currentLocation = this.walkSpace;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0000821B File Offset: 0x0000641B
		public bool tick(GameTime time)
		{
			return false;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00002834 File Offset: 0x00000A34
		public void leftClickHeld(int x, int y)
		{
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseLeftClick(int x, int y)
		{
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00002834 File Offset: 0x00000A34
		public void releaseRightClick(int x, int y)
		{
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyPress(Keys k)
		{
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0010D8A0 File Offset: 0x0010BAA0
		public void draw(SpriteBatch b)
		{
			b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			b.Draw(this.background, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.background.Bounds.Width / 2 * Game1.pixelZoom), (float)(-(float)(this.background.Bounds.Height * Game1.pixelZoom) + Game1.graphics.GraphicsDevice.Viewport.Height)), new Microsoft.Xna.Framework.Rectangle?(this.background.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.001f);
			b.Draw(this.trees, new Vector2((float)(Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.trees.Bounds.Width / 2 * Game1.pixelZoom), (float)(-(float)(this.trees.Bounds.Height * Game1.pixelZoom) + Game1.graphics.GraphicsDevice.Viewport.Height)), new Microsoft.Xna.Framework.Rectangle?(this.trees.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
			b.End();
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00002834 File Offset: 0x00000A34
		public void changeScreenSize()
		{
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0010DA04 File Offset: 0x0010BC04
		public void unload()
		{
			this.temporaryContent.Unload();
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000A8788 File Offset: 0x000A6988
		public void receiveEventPoke(int data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000DD0 RID: 3536
		public LocalizedContentManager temporaryContent;

		// Token: 0x04000DD1 RID: 3537
		public Texture2D background;

		// Token: 0x04000DD2 RID: 3538
		public Texture2D trees;

		// Token: 0x04000DD3 RID: 3539
		public float yOffset;

		// Token: 0x04000DD4 RID: 3540
		public GameLocation walkSpace;
	}
}
