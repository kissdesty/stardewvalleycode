using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using xTile;

namespace StardewValley.Locations
{
	// Token: 0x02000121 RID: 289
	public class Club : GameLocation
	{
		// Token: 0x06001074 RID: 4212 RVA: 0x00151B90 File Offset: 0x0014FD90
		public Club()
		{
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00151B98 File Offset: 0x0014FD98
		public Club(Map map, string name) : base(map, name)
		{
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x001535A4 File Offset: 0x001517A4
		public override void resetForPlayerEntry()
		{
			base.resetForPlayerEntry();
			this.lightGlows.Clear();
			if (!Game1.player.hasClubCard)
			{
				Game1.currentLocation = Game1.getLocationFromName("SandyHouse");
				Game1.changeMusicTrack("none");
				Game1.currentLocation.resetForPlayerEntry();
				NPC i = Game1.currentLocation.getCharacterFromName("Bouncer");
				if (i != null)
				{
					Vector2 placementTile = new Vector2(17f, 4f);
					i.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:Club_Bouncer_TextAboveHead" + (Game1.random.Next(2) + 1), new object[0]), -1, 2, 3000, 0);
					int idNum = Game1.random.Next();
					Game1.playSound("thudStep");
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(288, 100f, 1, 24, placementTile * (float)Game1.tileSize, true, false, Game1.currentLocation, Game1.player)
					{
						shakeIntensity = 0.5f,
						shakeIntensityChange = 0.002f,
						extraInfoForEndBehavior = idNum,
						endFunction = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID)
					});
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(4 * Game1.tileSize + 7) / 10000f, 0f, Color.Yellow, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						id = (float)idNum
					});
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, true, (float)(4 * Game1.tileSize + 7) / 10000f, 0f, Color.Orange, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = 100,
						id = (float)idNum
					});
					Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, placementTile * (float)Game1.tileSize + new Vector2(5f, 0f) * (float)Game1.pixelZoom, true, false, (float)(4 * Game1.tileSize + 7) / 10000f, 0f, Color.White, (float)Game1.pixelZoom * 0.75f, 0f, 0f, 0f, false)
					{
						delayBeforeAnimationStart = 200,
						id = (float)idNum
					});
					if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
					{
						Game1.fuseSound = Game1.soundBank.GetCue("fuse");
						Game1.fuseSound.Play();
					}
				}
				Game1.player.position = new Vector2(17f, 4f) * (float)Game1.tileSize;
			}
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00153909 File Offset: 0x00151B09
		public override void checkForMusic(GameTime time)
		{
			if (Game1.random.NextDouble() < 0.002)
			{
				Game1.playSound("boop");
			}
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0015392A File Offset: 0x00151B2A
		public override void cleanupBeforePlayerExit()
		{
			Game1.changeMusicTrack("none");
			base.cleanupBeforePlayerExit();
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0015393C File Offset: 0x00151B3C
		public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
		{
			base.drawAboveAlwaysFrontLayer(b);
			SpriteText.drawStringWithScrollBackground(b, "  " + Game1.player.clubCoins, Game1.tileSize, Game1.tileSize / 4, "", 1f, -1);
			Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float)(Game1.tileSize + Game1.pixelZoom), (float)(Game1.tileSize / 4 + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
		}

		// Token: 0x040011D9 RID: 4569
		public static int timesPlayedCalicoJack;

		// Token: 0x040011DA RID: 4570
		public static int timesPlayedSlots;
	}
}
