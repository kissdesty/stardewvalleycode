using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Projectiles;

namespace StardewValley.Tools
{
	// Token: 0x02000065 RID: 101
	public class Slingshot : Tool
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x000C6EFC File Offset: 0x000C50FC
		public Slingshot()
		{
			this.initialParentTileIndex = 32;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.currentParentTileIndex;
			Dictionary<int, string> weaponData = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
			this.name = weaponData[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			})[0];
			this.description = weaponData[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			})[1];
			this.numAttachmentSlots = 1;
			this.attachments = new Object[1];
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool doesShowTileLocationMarker()
		{
			return false;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000C6F9C File Offset: 0x000C519C
		public Slingshot(int which = 32)
		{
			this.initialParentTileIndex = which;
			this.currentParentTileIndex = this.initialParentTileIndex;
			this.indexOfMenuItemView = this.currentParentTileIndex;
			Dictionary<int, string> weaponData = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
			this.name = weaponData[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			})[0];
			this.description = weaponData[this.initialParentTileIndex].Split(new char[]
			{
				'/'
			})[1];
			this.numAttachmentSlots = 1;
			this.attachments = new Object[1];
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x000C7039 File Offset: 0x000C5239
		public bool didStartWithGamePad()
		{
			return this.startedWithGamePad;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000C7044 File Offset: 0x000C5244
		public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
		{
			this.indexOfMenuItemView = this.initialParentTileIndex;
			who.usingSlingshot = false;
			who.canReleaseTool = true;
			who.usingTool = false;
			who.canMove = true;
			if (this.attachments[0] != null)
			{
				Object ammunition = (Object)this.attachments[0].getOne();
				Object expr_54 = this.attachments[0];
				int num = expr_54.Stack;
				expr_54.Stack = num - 1;
				if (this.attachments[0].Stack <= 0)
				{
					this.attachments[0] = null;
				}
				int mouseX = Game1.getOldMouseX() + Game1.viewport.X;
				int mouseY = Game1.getOldMouseY() + Game1.viewport.Y;
				if (this.startedWithGamePad)
				{
					Point expr_107 = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					mouseX = expr_107.X;
					mouseY = expr_107.Y;
				}
				int arg_195_0 = Math.Min(20, (int)Vector2.Distance(new Vector2((float)who.getStandingX(), (float)(who.getStandingY() - Game1.tileSize)), new Vector2((float)mouseX, (float)mouseY)) / 20);
				Vector2 v = Utility.getVelocityTowardPoint(new Point(who.getStandingX(), who.getStandingY() + Game1.tileSize), new Vector2((float)mouseX, (float)(mouseY + Game1.tileSize)), (float)(15 + Game1.random.Next(4, 6)) * (1f + who.weaponSpeedModifier));
				if (arg_195_0 > 4 && !this.canPlaySound)
				{
					int damage = 1;
					BasicProjectile.onCollisionBehavior collisionBehavior = null;
					string collisionSound = "hammer";
					float damageMod = 1f;
					if (this.initialParentTileIndex == 33)
					{
						damageMod = 2f;
					}
					else if (this.initialParentTileIndex == 34)
					{
						damageMod = 4f;
					}
					num = ammunition.ParentSheetIndex;
					switch (num)
					{
					case 378:
					{
						damage = 10;
						Object expr_270 = ammunition;
						num = expr_270.ParentSheetIndex;
						expr_270.ParentSheetIndex = num + 1;
						break;
					}
					case 379:
					case 381:
					case 383:
					case 385:
					case 387:
					case 389:
						break;
					case 380:
					{
						damage = 20;
						Object expr_288 = ammunition;
						num = expr_288.ParentSheetIndex;
						expr_288.ParentSheetIndex = num + 1;
						break;
					}
					case 382:
					{
						damage = 15;
						Object expr_2B8 = ammunition;
						num = expr_2B8.ParentSheetIndex;
						expr_2B8.ParentSheetIndex = num + 1;
						break;
					}
					case 384:
					{
						damage = 30;
						Object expr_2A0 = ammunition;
						num = expr_2A0.ParentSheetIndex;
						expr_2A0.ParentSheetIndex = num + 1;
						break;
					}
					case 386:
					{
						damage = 50;
						Object expr_2D0 = ammunition;
						num = expr_2D0.ParentSheetIndex;
						expr_2D0.ParentSheetIndex = num + 1;
						break;
					}
					case 388:
					{
						damage = 2;
						Object expr_23B = ammunition;
						num = expr_23B.ParentSheetIndex;
						expr_23B.ParentSheetIndex = num + 1;
						break;
					}
					case 390:
					{
						damage = 5;
						Object expr_255 = ammunition;
						num = expr_255.ParentSheetIndex;
						expr_255.ParentSheetIndex = num + 1;
						break;
					}
					default:
						if (num == 441)
						{
							damage = 20;
							collisionBehavior = new BasicProjectile.onCollisionBehavior(BasicProjectile.explodeOnImpact);
							collisionSound = "explosion";
						}
						break;
					}
					num = ammunition.category;
					if (num == -5)
					{
						collisionSound = "slimedead";
					}
					location.projectiles.Add(new BasicProjectile((int)(damageMod * (float)(damage + Game1.random.Next(-(damage / 2), damage + 2)) * (1f + who.attackIncreaseModifier)), ammunition.ParentSheetIndex, 0, 0, (float)(3.1415926535897931 / (double)(64f + (float)Game1.random.Next(-63, 64))), -v.X, -v.Y, new Vector2((float)(who.getStandingX() - 16), (float)(who.getStandingY() - Game1.tileSize - 8)), collisionSound, "", false, true, who, true, collisionBehavior)
					{
						ignoreLocationCollision = (Game1.currentLocation.currentEvent != null)
					});
				}
			}
			else
			{
				Game1.showRedMessage("Out of ammo");
			}
			this.canPlaySound = true;
			who.Halt();
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000C7430 File Offset: 0x000C5630
		public override bool canThisBeAttached(Object o)
		{
			return o == null || (!o.bigCraftable && ((o.parentSheetIndex >= 378 && o.parentSheetIndex <= 390) || o.category == -5 || o.category == -79 || o.category == -75 || o.parentSheetIndex == 441));
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x000C7490 File Offset: 0x000C5690
		public override Object attach(Object o)
		{
			Object arg_1B_0 = this.attachments[0];
			this.attachments[0] = o;
			Game1.playSound("button1");
			return arg_1B_0;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x000C74B0 File Offset: 0x000C56B0
		public override string getHoverBoxText(Item hoveredItem)
		{
			if (hoveredItem != null && hoveredItem is Object && this.canThisBeAttached(hoveredItem as Object))
			{
				return string.Concat(new string[]
				{
					"Right-Click to arm ",
					this.name,
					Environment.NewLine,
					"with ",
					hoveredItem.Name
				});
			}
			if (hoveredItem == null && this.attachments != null && this.attachments[0] != null)
			{
				return "Right-Click to retrieve " + this.attachments[0].name;
			}
			return null;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x000C753A File Offset: 0x000C573A
		public override bool onRelease(GameLocation location, int x, int y, Farmer who)
		{
			this.DoFunction(location, x, y, 1, who);
			return true;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000C754C File Offset: 0x000C574C
		public override void beginUsing(GameLocation location, int x, int y, Farmer who)
		{
			who.usingSlingshot = true;
			who.canReleaseTool = false;
			this.mouseDragAmount = 0;
			int offset = (who.FacingDirection == 3 || who.FacingDirection == 1) ? 1 : ((who.FacingDirection == 0) ? 2 : 0);
			who.FarmerSprite.setCurrentFrame(42 + offset);
			double mouseX = (double)(Game1.getOldMouseX() + Game1.viewport.X - who.getStandingX());
			double mouseY = (double)(Game1.getOldMouseY() + Game1.viewport.Y - who.getStandingY());
			if (Math.Abs(mouseX) > Math.Abs(mouseY))
			{
				mouseX /= Math.Abs(mouseX);
				mouseY = 0.5;
			}
			else
			{
				mouseY /= Math.Abs(mouseY);
				mouseX = 0.0;
			}
			mouseX *= 16.0;
			mouseY *= 16.0;
			if (this.didStartWithGamePad())
			{
				Mouse.SetPosition(who.getStandingX() - Game1.viewport.X + (int)mouseX, who.getStandingY() - Game1.viewport.Y + (int)mouseY);
			}
			Game1.oldMouseState = Mouse.GetState();
			Game1.lastMousePositionBeforeFade = Game1.getMousePosition();
			this.lastClickX = Game1.getOldMouseX() + Game1.viewport.X;
			this.lastClickY = Game1.getOldMouseY() + Game1.viewport.Y;
			this.startedWithGamePad = false;
			if (Game1.options.gamepadControls && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X))
			{
				this.startedWithGamePad = true;
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000C76D0 File Offset: 0x000C58D0
		public override void tickUpdate(GameTime time, Farmer who)
		{
			if (who.usingSlingshot)
			{
				Point mousePos = Game1.getMousePosition();
				if (this.startedWithGamePad)
				{
					mousePos = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					mousePos.X -= Game1.viewport.X;
					mousePos.Y -= Game1.viewport.Y;
				}
				int mouseX = mousePos.X + Game1.viewport.X;
				int mouseY = mousePos.Y + Game1.viewport.Y;
				Game1.debugOutput = string.Concat(new object[]
				{
					"playerPos: ",
					Game1.player.getStandingPosition().ToString(),
					", mousePos: ",
					mouseX,
					", ",
					mouseY
				});
				this.mouseDragAmount++;
				who.faceGeneralDirection(new Vector2((float)mouseX, (float)mouseY), 0);
				who.faceDirection((who.FacingDirection + 2) % 4);
				int offset = (who.FacingDirection == 3 || who.FacingDirection == 1) ? 1 : ((who.FacingDirection == 0) ? 2 : 0);
				who.FarmerSprite.setCurrentFrame(42 + offset);
				if (this.canPlaySound && (Math.Abs(mouseX - this.lastClickX) > 8 || Math.Abs(mouseY - this.lastClickY) > 8) && this.mouseDragAmount > 4)
				{
					Game1.playSound("slingshot");
					this.canPlaySound = false;
				}
				this.lastClickX = mouseX;
				this.lastClickY = mouseY;
				Game1.mouseCursor = -1;
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x000C78B4 File Offset: 0x000C5AB4
		public override void drawAttachments(SpriteBatch b, int x, int y)
		{
			if (this.attachments[0] == null)
			{
				b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 43, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
				return;
			}
			b.Draw(Game1.menuTexture, new Vector2((float)x, (float)y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
			this.attachments[0].drawInMenu(b, new Vector2((float)x, (float)y), 1f);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000C796C File Offset: 0x000C5B6C
		public override void draw(SpriteBatch b)
		{
			if (Game1.player.usingSlingshot)
			{
				int mouseX = Game1.getOldMouseX() + Game1.viewport.X;
				int mouseY = Game1.getOldMouseY() + Game1.viewport.Y;
				if (this.startedWithGamePad)
				{
					Point expr_98 = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float)Game1.tileSize * 4f);
					mouseX = expr_98.X;
					mouseY = expr_98.Y;
				}
				Vector2 v = Utility.getVelocityTowardPoint(new Point(Game1.player.getStandingX(), Game1.player.getStandingY() + Game1.tileSize / 2), new Vector2((float)mouseX, (float)mouseY), 256f);
				if (Math.Abs(v.X) < 1f)
				{
					int arg_F5_0 = this.mouseDragAmount;
				}
				double distanceBetweenRadiusAndSquare = Math.Sqrt((double)(v.X * v.X + v.Y * v.Y)) - 181.0;
				double xPercent = (double)(v.X / 256f);
				double yPercent = (double)(v.Y / 256f);
				int x = (int)((double)v.X - distanceBetweenRadiusAndSquare * xPercent);
				int y = (int)((double)v.Y - distanceBetweenRadiusAndSquare * yPercent);
				b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float)(Game1.player.getStandingX() - x), (float)(Game1.player.getStandingY() - Game1.tileSize - 8 - y))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 43, -1, -1)), Color.White, 0f, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), 1f, SpriteEffects.None, 0.999999f);
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x000C7B54 File Offset: 0x000C5D54
		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
		{
			if (this.indexOfMenuItemView == 0 || this.indexOfMenuItemView == 21 || this.indexOfMenuItemView == 47 || this.currentParentTileIndex == 47)
			{
				string name = this.name;
				if (!(name == "Slingshot"))
				{
					if (!(name == "Master Slingshot"))
					{
						if (name == "Galaxy Slingshot")
						{
							this.currentParentTileIndex = 34;
						}
					}
					else
					{
						this.currentParentTileIndex = 33;
					}
				}
				else
				{
					this.currentParentTileIndex = 32;
				}
				this.indexOfMenuItemView = this.currentParentTileIndex;
			}
			spriteBatch.Draw(Tool.weaponsTexture, location + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, this.indexOfMenuItemView, 16, 16)), Color.White * transparency, 0f, new Vector2(8f, 8f), scaleSize * (float)Game1.pixelZoom, SpriteEffects.None, layerDepth);
			if (drawStackNumber && this.attachments != null && this.attachments[0] != null)
			{
				if (!Game1.options.useCrisperNumberFont)
				{
					float scale = 0.5f + scaleSize;
					Game1.drawWithBorder(string.Concat(this.attachments[0].Stack), Color.Black, Color.White, location + new Vector2((float)Game1.tileSize - Game1.tinyFont.MeasureString(string.Concat(this.attachments[0].Stack)).X * scale, (float)Game1.tileSize - Game1.tinyFont.MeasureString(string.Concat(this.attachments[0].Stack)).Y * 3f / 4f * scale), 0f, scale, 1f, true);
					return;
				}
				Utility.drawTinyDigits(this.attachments[0].Stack, spriteBatch, location + new Vector2((float)(Game1.tileSize - Utility.getWidthOfTinyDigitString(this.attachments[0].Stack, 3f * scaleSize)) + 3f * scaleSize, (float)Game1.tileSize - 18f * scaleSize + 2f), 3f * scaleSize, 1f, Color.White);
			}
		}

		// Token: 0x04000926 RID: 2342
		public const int basicDamage = 5;

		// Token: 0x04000927 RID: 2343
		public const int basicSlingshot = 32;

		// Token: 0x04000928 RID: 2344
		public const int masterSlingshot = 33;

		// Token: 0x04000929 RID: 2345
		public const int galaxySlingshot = 34;

		// Token: 0x0400092A RID: 2346
		public const int drawBackSoundThreshold = 8;

		// Token: 0x0400092B RID: 2347
		[XmlIgnore]
		public int recentClickX;

		// Token: 0x0400092C RID: 2348
		[XmlIgnore]
		public int recentClickY;

		// Token: 0x0400092D RID: 2349
		[XmlIgnore]
		public int lastClickX;

		// Token: 0x0400092E RID: 2350
		[XmlIgnore]
		public int lastClickY;

		// Token: 0x0400092F RID: 2351
		[XmlIgnore]
		public int mouseDragAmount;

		// Token: 0x04000930 RID: 2352
		private bool canPlaySound;

		// Token: 0x04000931 RID: 2353
		private bool startedWithGamePad;
	}
}
