using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x0200012D RID: 301
	public class Desert : GameLocation
	{
		// Token: 0x0600114E RID: 4430 RVA: 0x0016347C File Offset: 0x0016167C
		public Desert()
		{
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x001634DC File Offset: 0x001616DC
		public Desert(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0016353C File Offset: 0x0016173C
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int arg_3D_0 = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				return base.checkAction(tileLocation, viewport, who);
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0015904B File Offset: 0x0015724B
		private void pamReachedBusDoor(Character c, GameLocation l)
		{
			Game1.changeMusicTrack("none");
			c.position.X = -10000f;
			Game1.playSound("stoneStep");
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0016359A File Offset: 0x0016179A
		private void playerReachedBusDoor(Character c, GameLocation l)
		{
			Game1.player.position.X = -10000f;
			this.busDriveOff();
			Game1.playSound("stoneStep");
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x001635C0 File Offset: 0x001617C0
		public override bool answerDialogue(Response answer)
		{
			string questionAndAnswer = this.lastQuestionKey.Split(new char[]
			{
				' '
			})[0] + "_" + answer.responseKey;
			if (questionAndAnswer == "DesertBus_Yes")
			{
				this.playerReachedBusDoor(Game1.player, this);
				return true;
			}
			return base.answerDialogue(answer);
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00163618 File Offset: 0x00161818
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			Game1.ambientLight = Color.White;
			if (Game1.player.getTileY() > 40 || Game1.player.getTileY() < 10)
			{
				this.drivingOff = false;
				this.drivingBack = false;
				this.busMotion = Vector2.Zero;
				this.busPosition = new Vector2(17f, 24f) * (float)Game1.tileSize;
				this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
				{
					interval = 999999f,
					animationLength = 6,
					holdLastFrame = true,
					layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
					scale = (float)Game1.pixelZoom
				};
				Game1.changeMusicTrack("wavy");
				return;
			}
			if (Game1.isRaining)
			{
				Game1.changeMusicTrack("none");
			}
			this.busPosition = new Vector2(17f, 24f) * (float)Game1.tileSize;
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 1,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			Game1.displayFarmer = false;
			this.busDriveBack();
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00163811 File Offset: 0x00161A11
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.busDoor = null;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00163820 File Offset: 0x00161A20
		public void busDriveOff()
		{
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 6,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			this.busDoor.timer = 0f;
			this.busDoor.interval = 70f;
			this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.busStartMovingOff);
			Game1.playSound("trashcanlid");
			this.drivingBack = false;
			this.busDoor.paused = false;
			Game1.changeMusicTrack("none");
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0016392C File Offset: 0x00161B2C
		public void busDriveBack()
		{
			this.busPosition.X = (float)this.map.GetLayer("Back").DisplayWidth;
			this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom;
			this.drivingBack = true;
			this.drivingOff = false;
			Game1.playSound("busDriveOff");
			this.busMotion = new Vector2(-6f, 0f);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x001639B7 File Offset: 0x00161BB7
		private void busStartMovingOff(int extraInfo)
		{
			Game1.playSound("batFlap");
			this.drivingOff = true;
			Game1.playSound("busDriveOff");
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x001639D4 File Offset: 0x00161BD4
		public override void performTouchAction(string fullActionString, Vector2 playerStandingPosition)
		{
			string a = fullActionString.Split(new char[]
			{
				' '
			})[0];
			if (a == "DesertBus")
			{
				Response[] returnOptions = new Response[]
				{
					new Response("Yes", Game1.content.LoadString("Strings\\Locations:Desert_Return_Yes", new object[0])),
					new Response("Not", Game1.content.LoadString("Strings\\Locations:Desert_Return_No", new object[0]))
				};
				base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Desert_Return_Question", new object[0]), returnOptions, "DesertBus");
				return;
			}
			base.performTouchAction(fullActionString, playerStandingPosition);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00163A78 File Offset: 0x00161C78
		private void doorOpenAfterReturn(int extraInfo)
		{
			Game1.playSound("batFlap");
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 6,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			Game1.player.position = new Vector2(18f, 27f) * (float)Game1.tileSize;
			this.lastTouchActionLocation = Game1.player.getTileLocation();
			Game1.displayFarmer = true;
			Game1.player.forceCanMove();
			Game1.player.faceDirection(2);
			Game1.changeMusicTrack("wavy");
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00163B86 File Offset: 0x00161D86
		private void busLeftToValley()
		{
			Game1.viewport.Y = -100000;
			Game1.viewportFreeze = true;
			Game1.warpFarmer("BusStop", 12, 10, true);
			Game1.freezeControls = false;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00163BB4 File Offset: 0x00161DB4
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.drivingOff)
			{
				this.busMotion.X = this.busMotion.X - 0.075f;
				if (this.busPosition.X + (float)(8 * Game1.tileSize) < 0f)
				{
					this.drivingOff = false;
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.busLeftToValley), 0.01f);
				}
			}
			if (this.drivingBack)
			{
				Game1.player.position = this.busDoor.position;
				Game1.player.freezePause = 100;
				if (this.busPosition.X - (float)(17 * Game1.tileSize) < (float)(Game1.tileSize * 4))
				{
					this.busMotion.X = Math.Min(-1f, this.busMotion.X * 0.98f);
				}
				if (Math.Abs(this.busPosition.X - (float)(17 * Game1.tileSize)) <= Math.Abs(this.busMotion.X * 1.5f))
				{
					this.busPosition.X = (float)(17 * Game1.tileSize);
					this.busMotion = Vector2.Zero;
					this.drivingBack = false;
					this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom;
					this.busDoor.pingPong = true;
					this.busDoor.interval = 70f;
					this.busDoor.currentParentTileIndex = 5;
					this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.doorOpenAfterReturn);
					Game1.playSound("trashcanlid");
				}
			}
			if (!this.busMotion.Equals(Vector2.Zero))
			{
				this.busPosition += this.busMotion;
				this.busDoor.Position += this.busMotion;
			}
			if (this.busDoor != null)
			{
				this.busDoor.update(time);
			}
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00163DBC File Offset: 0x00161FBC
		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition), new Microsoft.Xna.Framework.Rectangle?(this.busSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f);
			if (this.busDoor != null)
			{
				this.busDoor.draw(spriteBatch, false, 0, 0);
			}
			if (this.drivingOff || this.drivingBack)
			{
				spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition + this.pamOffset * (float)Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(this.pamSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (this.busPosition.Y + (float)(3 * Game1.tileSize) + (float)Game1.pixelZoom) / 10000f);
			}
		}

		// Token: 0x0400124D RID: 4685
		public const int busDefaultXTile = 17;

		// Token: 0x0400124E RID: 4686
		public const int busDefaultYTile = 24;

		// Token: 0x0400124F RID: 4687
		private TemporaryAnimatedSprite busDoor;

		// Token: 0x04001250 RID: 4688
		private Vector2 busPosition;

		// Token: 0x04001251 RID: 4689
		private Vector2 busMotion;

		// Token: 0x04001252 RID: 4690
		private bool drivingOff;

		// Token: 0x04001253 RID: 4691
		private bool drivingBack;

		// Token: 0x04001254 RID: 4692
		private Microsoft.Xna.Framework.Rectangle busSource = new Microsoft.Xna.Framework.Rectangle(288, 1247, 128, 64);

		// Token: 0x04001255 RID: 4693
		private Microsoft.Xna.Framework.Rectangle pamSource = new Microsoft.Xna.Framework.Rectangle(384, 1311, 15, 19);

		// Token: 0x04001256 RID: 4694
		private Vector2 pamOffset = new Vector2(0f, 29f);
	}
}
