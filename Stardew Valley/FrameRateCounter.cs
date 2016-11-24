using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

// Token: 0x02000002 RID: 2
public class FrameRateCounter : DrawableGameComponent
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public FrameRateCounter(Game game) : base(game)
	{
		this.content = new ContentManager(game.Services);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002075 File Offset: 0x00000275
	protected override void LoadContent()
	{
		this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002088 File Offset: 0x00000288
	protected override void UnloadContent()
	{
		this.content.Unload();
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
	public override void Update(GameTime gameTime)
	{
		this.elapsedTime += gameTime.ElapsedGameTime;
		if (this.elapsedTime > TimeSpan.FromSeconds(1.0))
		{
			this.elapsedTime -= TimeSpan.FromSeconds(1.0);
			this.frameRate = this.frameCounter;
			this.frameCounter = 0;
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000210C File Offset: 0x0000030C
	public override void Draw(GameTime gameTime)
	{
		this.frameCounter++;
		string fps = string.Format("fps: {0}", this.frameRate);
		this.spriteBatch.Begin();
		this.spriteBatch.DrawString(Game1.dialogueFont, fps, new Vector2(33f, 33f), Color.Black);
		this.spriteBatch.DrawString(Game1.dialogueFont, fps, new Vector2(32f, 32f), Color.White);
		this.spriteBatch.End();
	}

	// Token: 0x04000001 RID: 1
	private ContentManager content;

	// Token: 0x04000002 RID: 2
	private SpriteBatch spriteBatch;

	// Token: 0x04000003 RID: 3
	private int frameRate;

	// Token: 0x04000004 RID: 4
	private int frameCounter;

	// Token: 0x04000005 RID: 5
	private TimeSpan elapsedTime = TimeSpan.Zero;
}
