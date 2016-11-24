using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
	// Token: 0x02000128 RID: 296
	public class BusStop : GameLocation
	{
		// Token: 0x060010D8 RID: 4312 RVA: 0x00158CC4 File Offset: 0x00156EC4
		public BusStop()
		{
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00158D24 File Offset: 0x00156F24
		public BusStop(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00158D84 File Offset: 0x00156F84
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
			{
				int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
				if (tileIndex <= 1057)
				{
					if (tileIndex != 958)
					{
						if (tileIndex != 1057)
						{
							goto IL_2B1;
						}
						if (!Game1.player.mailReceived.Contains("ccVault"))
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_DesertOutOfService", new object[0]));
							return true;
						}
						if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
						{
							Game1.player.getMount().checkAction(Game1.player, this);
							goto IL_2B1;
						}
						base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_BuyTicketToDesert", new object[0]), base.createYesNoResponses(), "Bus");
						goto IL_2B1;
					}
				}
				else if (tileIndex != 1080 && tileIndex != 1081)
				{
					goto IL_2B1;
				}
				if (Game1.player.getMount() != null)
				{
					return true;
				}
				if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
				{
					if (!Game1.player.isRidingHorse() || Game1.player.getMount() == null)
					{
						Response[] destinations;
						if (Game1.player.mailReceived.Contains("ccCraftsRoom"))
						{
							destinations = new Response[]
							{
								new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
								new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town", new object[0])),
								new Response("Quarry", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Quarry", new object[0])),
								new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
							};
						}
						else
						{
							destinations = new Response[]
							{
								new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines", new object[0])),
								new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town", new object[0])),
								new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel", new object[0]))
							};
						}
						base.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination", new object[0]), destinations, "Minecart");
						goto IL_2B1;
					}
					Game1.player.getMount().checkAction(Game1.player, this);
				}
				else
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder", new object[0]));
				}
				return true;
			}
			IL_2B1:
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0015904B File Offset: 0x0015724B
		private void pamReachedBusDoor(Character c, GameLocation l)
		{
			Game1.changeMusicTrack("none");
			c.position.X = -10000f;
			Game1.playSound("stoneStep");
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00159074 File Offset: 0x00157274
		private void playerReachedBusDoor(Character c, GameLocation l)
		{
			this.forceWarpTimer = 0;
			Game1.player.position.X = -10000f;
			this.busDriveOff();
			Game1.playSound("stoneStep");
			if (Game1.player.getMount() != null)
			{
				Game1.player.getMount().farmerPassesThrough = false;
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x001590C8 File Offset: 0x001572C8
		public override bool answerDialogue(Response answer)
		{
			string questionAndAnswer = this.lastQuestionKey.Split(new char[]
			{
				' '
			})[0] + "_" + answer.responseKey;
			if (questionAndAnswer == "Bus_Yes")
			{
				NPC pam = Game1.getCharacterFromName("Pam", false);
				if (Game1.player.Money >= (Game1.shippingTax ? 50 : 500) && this.characters.Contains(pam) && pam.getTileLocation().Equals(new Vector2(11f, 10f)))
				{
					Game1.player.Money -= (Game1.shippingTax ? 50 : 500);
					pam.ignoreMultiplayerUpdates = true;
					pam.faceTowardFarmerTimer = 0;
					pam.faceTowardFarmer = false;
					pam.faceAwayFromFarmer = false;
					pam.movementPause = 1;
					pam.controller = new PathFindController(pam, this, new Point(12, 9), 0, new PathFindController.endBehavior(this.pamReachedBusDoor));
					pam.forceUpdateTimer = 15000;
					Game1.freezeControls = true;
					Game1.viewportFreeze = true;
					this.forceWarpTimer = 8000;
					Game1.player.controller = new PathFindController(Game1.player, this, new Point(12, 9), 0, new PathFindController.endBehavior(this.playerReachedBusDoor));
					Game1.player.setRunning(true, false);
					if (Game1.player.getMount() != null)
					{
						Game1.player.getMount().farmerPassesThrough = true;
					}
				}
				else if (Game1.player.Money < (Game1.shippingTax ? 50 : 500))
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_NotEnoughMoneyForTicket", new object[0]));
				}
				else
				{
					Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_NoDriver", new object[0]));
				}
				return true;
			}
			return base.answerDialogue(answer);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x001592A8 File Offset: 0x001574A8
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
			{
				this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float)(6 * Game1.tileSize + Game1.pixelZoom * 2), (float)(3 * Game1.tileSize) - (float)Game1.tileSize * 3f / 4f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
				{
					totalNumberOfLoops = 999999,
					interval = 60f,
					flipped = true
				};
			}
			if (Game1.getFarm().grandpaScore == 0 && Game1.year >= 3 && Game1.player.eventsSeen.Contains(558292))
			{
				Game1.player.eventsSeen.Remove(558292);
			}
			if (Game1.player.getTileY() > 16 || Game1.eventUp || Game1.player.getTileX() == 0 || Game1.player.isRidingHorse() || !Game1.player.previousLocationName.Equals("Desert"))
			{
				this.drivingOff = false;
				this.drivingBack = false;
				this.busMotion = Vector2.Zero;
				this.busPosition = new Vector2(11f, 6f) * (float)Game1.tileSize;
				this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
				{
					interval = 999999f,
					animationLength = 6,
					holdLastFrame = true,
					layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
					scale = (float)Game1.pixelZoom
				};
				return;
			}
			this.busPosition = new Vector2(11f, 6f) * (float)Game1.tileSize;
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

		// Token: 0x060010DF RID: 4319 RVA: 0x00159567 File Offset: 0x00157767
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			this.minecartSteam = null;
			this.busDoor = null;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00159580 File Offset: 0x00157780
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
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00159680 File Offset: 0x00157880
		public void busDriveBack()
		{
			this.busPosition.X = (float)this.map.GetLayer("Back").DisplayWidth;
			this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom;
			this.drivingBack = true;
			this.drivingOff = false;
			Game1.playSound("busDriveOff");
			this.busMotion = new Vector2(-6f, 0f);
			Game1.freezeControls = true;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00159711 File Offset: 0x00157911
		private void busStartMovingOff(int extraInfo)
		{
			Game1.playSound("batFlap");
			this.drivingOff = true;
			Game1.playSound("busDriveOff");
			Game1.changeMusicTrack("none");
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00159738 File Offset: 0x00157938
		private void pamReturnedToSpot(Character c, GameLocation l)
		{
			c.ignoreMultiplayerUpdates = false;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00159744 File Offset: 0x00157944
		private void doorOpenAfterReturn(int extraInfo)
		{
			this.busDoor = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom, false, 0f, Color.White)
			{
				interval = 999999f,
				animationLength = 6,
				holdLastFrame = true,
				layerDepth = (this.busPosition.Y + (float)(3 * Game1.tileSize)) / 10000f + 1E-05f,
				scale = (float)Game1.pixelZoom
			};
			Game1.player.position = new Vector2(12f, 10f) * (float)Game1.tileSize;
			this.lastTouchActionLocation = Game1.player.getTileLocation();
			Game1.displayFarmer = true;
			Game1.player.forceCanMove();
			Game1.player.faceDirection(2);
			NPC pam = Game1.getCharacterFromName("Pam", false);
			if (pam != null)
			{
				if (this.characters.Contains(pam))
				{
					pam.speed = 2;
					pam.position = new Vector2(11f, 6f) * (float)Game1.tileSize;
					pam.controller = new PathFindController(pam, this, new Point(11, 10), 2, new PathFindController.endBehavior(this.pamReturnedToSpot));
					return;
				}
				pam.ignoreMultiplayerUpdates = false;
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x001598AD File Offset: 0x00157AAD
		private void busLeftToDesert()
		{
			Game1.viewport.Y = -100000;
			Game1.viewportFreeze = true;
			Game1.warpFarmer("Desert", 16, 24, true);
			Game1.freezeControls = false;
			Game1.globalFade = false;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x001598E0 File Offset: 0x00157AE0
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			if (this.forceWarpTimer > 0)
			{
				this.forceWarpTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.forceWarpTimer <= 0)
				{
					this.playerReachedBusDoor(Game1.player, this);
				}
			}
			if (this.minecartSteam != null)
			{
				this.minecartSteam.update(time);
			}
			if (this.drivingOff)
			{
				this.busMotion.X = this.busMotion.X - 0.075f;
				if (this.busPosition.X + (float)(8 * Game1.tileSize) < 0f)
				{
					this.drivingOff = false;
					Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.busLeftToDesert), 0.01f);
				}
			}
			if (this.drivingBack)
			{
				Game1.player.position = this.busPosition;
				if (this.busPosition.X - (float)(11 * Game1.tileSize) < (float)(Game1.tileSize * 4))
				{
					this.busMotion.X = Math.Min(-1f, this.busMotion.X * 0.98f);
				}
				if (Math.Abs(this.busPosition.X - (float)(11 * Game1.tileSize)) <= Math.Abs(this.busMotion.X * 1.5f))
				{
					this.busPosition.X = (float)(11 * Game1.tileSize);
					this.busMotion = Vector2.Zero;
					this.drivingBack = false;
					this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float)Game1.pixelZoom;
					this.busDoor.pingPong = true;
					this.busDoor.interval = 70f;
					this.busDoor.currentParentTileIndex = 5;
					this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.doorOpenAfterReturn);
					Game1.playSound("trashcanlid");
					for (int i = 0; i < this.characters.Count; i++)
					{
						if (this.characters[i] is Horse)
						{
							Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:BusStop_ReturnToHorse" + (Game1.random.Next(2) + 1), new object[]
							{
								this.characters[i].name
							}));
						}
					}
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

		// Token: 0x060010E7 RID: 4327 RVA: 0x00159B94 File Offset: 0x00157D94
		public override void draw(SpriteBatch spriteBatch)
		{
			base.draw(spriteBatch);
			if (this.minecartSteam != null)
			{
				this.minecartSteam.draw(spriteBatch, false, 0, 0);
			}
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

		// Token: 0x04001203 RID: 4611
		public const int busDefaultXTile = 11;

		// Token: 0x04001204 RID: 4612
		public const int busDefaultYTile = 6;

		// Token: 0x04001205 RID: 4613
		private TemporaryAnimatedSprite minecartSteam;

		// Token: 0x04001206 RID: 4614
		private TemporaryAnimatedSprite busDoor;

		// Token: 0x04001207 RID: 4615
		private Vector2 busPosition;

		// Token: 0x04001208 RID: 4616
		private Vector2 busMotion;

		// Token: 0x04001209 RID: 4617
		private bool drivingOff;

		// Token: 0x0400120A RID: 4618
		private bool drivingBack;

		// Token: 0x0400120B RID: 4619
		private int forceWarpTimer;

		// Token: 0x0400120C RID: 4620
		private Microsoft.Xna.Framework.Rectangle busSource = new Microsoft.Xna.Framework.Rectangle(288, 1247, 128, 64);

		// Token: 0x0400120D RID: 4621
		private Microsoft.Xna.Framework.Rectangle pamSource = new Microsoft.Xna.Framework.Rectangle(384, 1311, 15, 19);

		// Token: 0x0400120E RID: 4622
		private Vector2 pamOffset = new Vector2(0f, 29f);
	}
}
