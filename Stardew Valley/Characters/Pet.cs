using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;

namespace StardewValley.Characters
{
	// Token: 0x02000145 RID: 325
	public class Pet : NPC
	{
		// Token: 0x170000E0 RID: 224
		public int CurrentBehavior
		{
			// Token: 0x06001266 RID: 4710 RVA: 0x00177107 File Offset: 0x00175307
			get
			{
				return this.currentBehavior;
			}
			// Token: 0x06001267 RID: 4711 RVA: 0x0017710F File Offset: 0x0017530F
			set
			{
				this.currentBehavior = value;
				this.initiateCurrentBehavior();
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00177120 File Offset: 0x00175320
		public override void behaviorOnFarmerLocationEntry(GameLocation location, Farmer who)
		{
			if ((location is Farm || (location is FarmHouse && this.currentBehavior != 1)) && Game1.timeOfDay >= 2000)
			{
				if (this.currentBehavior != 1 || this.currentLocation is Farm)
				{
					this.warpToFarmHouse(who);
					return;
				}
			}
			else if (Game1.timeOfDay < 2000 && Game1.random.NextDouble() < 0.5)
			{
				this.CurrentBehavior = 1;
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00177198 File Offset: 0x00175398
		public override void reloadSprite()
		{
			base.DefaultPosition = new Vector2(54f, 8f) * (float)Game1.tileSize;
			this.hideShadow = true;
			this.breather = false;
			this.setAtFarmPosition();
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x001771D0 File Offset: 0x001753D0
		public void warpToFarmHouse(Farmer who)
		{
			FarmHouse farmHouse = Utility.getHomeOfFarmer(who);
			Vector2 sleepTile = Vector2.Zero;
			int tries = 0;
			sleepTile = new Vector2((float)Game1.random.Next(2, farmHouse.map.Layers[0].LayerWidth - 3), (float)Game1.random.Next(3, farmHouse.map.Layers[0].LayerHeight - 5));
			while (tries < 50 && (!farmHouse.isTileLocationTotallyClearAndPlaceable(sleepTile) || !farmHouse.isTileLocationTotallyClearAndPlaceable(sleepTile + new Vector2(1f, 0f)) || farmHouse.isTileOnWall((int)sleepTile.X, (int)sleepTile.Y)))
			{
				sleepTile = new Vector2((float)Game1.random.Next(2, farmHouse.map.Layers[0].LayerWidth - 3), (float)Game1.random.Next(3, farmHouse.map.Layers[0].LayerHeight - 4));
				tries++;
			}
			if (tries < 50)
			{
				Game1.warpCharacter(this, "FarmHouse", sleepTile, false, false);
				Game1.getFarm().characters.Remove(this);
				this.currentBehavior = 1;
				this.initiateCurrentBehavior();
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00177308 File Offset: 0x00175508
		public override void dayUpdate(int dayOfMonth)
		{
			base.DefaultPosition = new Vector2(54f, 8f) * (float)Game1.tileSize;
			this.sprite.loop = false;
			this.breather = false;
			if (Game1.isRaining)
			{
				this.CurrentBehavior = 2;
				if (this.currentLocation is Farm)
				{
					this.warpToFarmHouse(Game1.player);
				}
			}
			else if (this.currentLocation is FarmHouse)
			{
				this.setAtFarmPosition();
			}
			if (this.currentLocation is Farm)
			{
				if (this.currentLocation.getTileIndexAt(54, 7, "Buildings") == 1939)
				{
					this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 6);
				}
				this.currentLocation.setMapTileIndex(54, 7, 1938, "Buildings", 0);
				base.setTilePosition(54, 8);
				this.position.X = this.position.X - (float)Game1.tileSize;
			}
			this.Halt();
			this.CurrentBehavior = 1;
			this.wasPetToday = false;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00177410 File Offset: 0x00175610
		public void setAtFarmPosition()
		{
			bool isOnFarm = this.currentLocation is Farm;
			if (!Game1.isRaining)
			{
				this.faceDirection(2);
				this.currentLocation.characters.Remove(this);
				Game1.warpCharacter(this, "Farm", new Vector2(54f, 8f), false, false);
				this.position.X = this.position.X - (float)Game1.tileSize;
				return;
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0000846E File Offset: 0x0000666E
		public override bool shouldCollideWithBuildingLayer(GameLocation location)
		{
			return true;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool canPassThroughActionTiles()
		{
			return false;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00177480 File Offset: 0x00175680
		public override bool checkAction(Farmer who, GameLocation l)
		{
			if (!this.wasPetToday)
			{
				this.wasPetToday = true;
				this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 12);
				if (this.friendshipTowardFarmer >= 1000 && who != null && !who.mailReceived.Contains("petLoveMessage"))
				{
					Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Characters:PetLovesYou", new object[]
					{
						this.name
					}));
					who.mailReceived.Add("petLoveMessage");
				}
				base.doEmote(20, true);
				this.playContentSound();
				return true;
			}
			return false;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00002834 File Offset: 0x00000A34
		public virtual void playContentSound()
		{
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00177520 File Offset: 0x00175720
		public void hold(Farmer who)
		{
			this.flip = this.sprite.currentAnimation.Last<FarmerSprite.AnimationFrame>().flip;
			this.sprite.CurrentFrame = this.sprite.currentAnimation.Last<FarmerSprite.AnimationFrame>().frame;
			this.sprite.currentAnimation = null;
			this.sprite.loop = false;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00177580 File Offset: 0x00175780
		public override void behaviorOnFarmerPushing()
		{
			if (this is Dog && (this as Dog).currentBehavior == 51)
			{
				return;
			}
			this.pushingTimer += 2;
			if (this.pushingTimer > 100)
			{
				Vector2 trajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
				base.setTrajectory((int)trajectory.X / 2, (int)trajectory.Y / 2);
				this.pushingTimer = 0;
				this.Halt();
				base.facePlayer(Game1.player);
				this.facingDirection += 2;
				this.facingDirection %= 4;
				this.faceDirection(this.facingDirection);
				this.CurrentBehavior = 0;
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00177628 File Offset: 0x00175828
		public override void update(GameTime time, GameLocation location, long id, bool move)
		{
			base.update(time, location, id, move);
			this.pushingTimer = Math.Max(0, this.pushingTimer - 1);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0017764C File Offset: 0x0017584C
		public virtual void initiateCurrentBehavior()
		{
			this.flip = false;
			bool localFlip = false;
			switch (this.currentBehavior)
			{
			case 0:
				this.Halt();
				this.faceDirection(Game1.random.Next(4));
				base.setMovingInFacingDirection();
				return;
			case 1:
				this.sprite.loop = true;
				localFlip = (Game1.random.NextDouble() < 0.5);
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(28, 1000, false, localFlip, null, false),
					new FarmerSprite.AnimationFrame(29, 1000, false, localFlip, null, false)
				});
				return;
			case 2:
				this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
				{
					new FarmerSprite.AnimationFrame(16, 100, false, localFlip, null, false),
					new FarmerSprite.AnimationFrame(17, 100, false, localFlip, null, false),
					new FarmerSprite.AnimationFrame(18, 100, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hold), false)
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00177750 File Offset: 0x00175950
		public override Rectangle GetBoundingBox()
		{
			return new Rectangle((int)this.position.X + Game1.tileSize / 4, (int)this.position.Y + Game1.tileSize / 4, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize / 2);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x001777A8 File Offset: 0x001759A8
		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (base.IsEmoting)
			{
				Vector2 emotePosition = base.getLocalPosition(Game1.viewport);
				emotePosition.X += (float)(Game1.tileSize / 2);
				emotePosition.Y -= (float)(Game1.tileSize * 3 / 2 + ((this is Dog) ? (Game1.tileSize / 4) : 0));
				b.Draw(Game1.emoteSpriteSheet, emotePosition, new Rectangle?(new Rectangle(base.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, base.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, (float)base.getStandingY() / 10000f + 0.0001f);
			}
		}

		// Token: 0x0400130A RID: 4874
		public const int bedTime = 2000;

		// Token: 0x0400130B RID: 4875
		public const int maxFriendship = 1000;

		// Token: 0x0400130C RID: 4876
		public const int behavior_walking = 0;

		// Token: 0x0400130D RID: 4877
		public const int behavior_Sleep = 1;

		// Token: 0x0400130E RID: 4878
		public const int behavior_Sit_Down = 2;

		// Token: 0x0400130F RID: 4879
		public const int frame_basicSit = 18;

		// Token: 0x04001310 RID: 4880
		private int currentBehavior;

		// Token: 0x04001311 RID: 4881
		private bool wasPetToday;

		// Token: 0x04001312 RID: 4882
		public int friendshipTowardFarmer;

		// Token: 0x04001313 RID: 4883
		private int pushingTimer;
	}
}
