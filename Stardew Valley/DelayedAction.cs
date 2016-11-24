using System;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000004 RID: 4
	public class DelayedAction
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000022E5 File Offset: 0x000004E5
		public DelayedAction(int timeUntilAction)
		{
			this.timeUntilAction = timeUntilAction;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022F4 File Offset: 0x000004F4
		public DelayedAction(int timeUntilAction, DelayedAction.delayedBehavior behavior)
		{
			this.timeUntilAction = timeUntilAction;
			this.behavior = behavior;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000230C File Offset: 0x0000050C
		public bool update(GameTime time)
		{
			if (!this.waitUntilMenusGone || Game1.activeClickableMenu == null)
			{
				this.timeUntilAction -= time.ElapsedGameTime.Milliseconds;
				if (this.timeUntilAction <= 0)
				{
					this.behavior();
				}
			}
			return this.timeUntilAction <= 0;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002364 File Offset: 0x00000564
		public static void warpAfterDelay(string nameToWarpTo, Point pointToWarp, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.warp);
			action.stringData = nameToWarpTo;
			action.pointData = pointToWarp;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023A4 File Offset: 0x000005A4
		public static void addTemporarySpriteAfterDelay(TemporaryAnimatedSprite t, GameLocation l, int timer, bool waitUntilMenusGone = false)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.addTempSprite);
			action.temporarySpriteData = t;
			action.location = l;
			action.waitUntilMenusGone = waitUntilMenusGone;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023EC File Offset: 0x000005EC
		public static void playSoundAfterDelay(string soundName, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.playSound);
			action.stringData = soundName;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002424 File Offset: 0x00000624
		public static void removeTemporarySpriteAfterDelay(GameLocation location, float idOfTempSprite, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.removeTemporarySprite);
			action.location = location;
			action.floatData = idOfTempSprite;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002464 File Offset: 0x00000664
		public static void playMusicAfterDelay(string musicName, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.changeMusicTrack);
			action.stringData = musicName;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000249C File Offset: 0x0000069C
		public static void textAboveHeadAfterDelay(string text, NPC who, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.showTextAboveHead);
			action.stringData = text;
			action.character = who;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024DC File Offset: 0x000006DC
		public static void stopFarmerGlowing(int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.stopGlowing);
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002510 File Offset: 0x00000710
		public static void showDialogueAfterDelay(string dialogue, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.showDialogue);
			action.stringData = dialogue;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002548 File Offset: 0x00000748
		public static void screenFlashAfterDelay(float intensity, int timer, string sound = "")
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.screenFlash);
			action.stringData = sound;
			action.floatData = intensity;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002588 File Offset: 0x00000788
		public static void removeTileAfterDelay(int x, int y, int timer, GameLocation l, string whichLayer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.removeBuildingsTile);
			action.pointData = new Point(x, y);
			action.location = l;
			action.stringData = whichLayer;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025D8 File Offset: 0x000007D8
		public static void fadeAfterDelay(Game1.afterFadeFunction behaviorAfterFade, int timer)
		{
			DelayedAction action = new DelayedAction(timer);
			action.behavior = new DelayedAction.delayedBehavior(action.doGlobalFade);
			action.afterFadeBehavior = behaviorAfterFade;
			Game1.delayedActions.Add(action);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002610 File Offset: 0x00000810
		public void doGlobalFade()
		{
			Game1.globalFadeToBlack(this.afterFadeBehavior, 0.02f);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002622 File Offset: 0x00000822
		public void showTextAboveHead()
		{
			if (this.character != null && this.stringData != null)
			{
				this.character.showTextAboveHead(this.stringData, -1, 2, 3000, 0);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000264D File Offset: 0x0000084D
		public void addTempSprite()
		{
			if (this.location != null && this.temporarySpriteData != null)
			{
				this.location.TemporarySprites.Add(this.temporarySpriteData);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002675 File Offset: 0x00000875
		public void stopGlowing()
		{
			Game1.player.stopGlowing();
			Game1.player.stopJittering();
			Game1.screenGlowHold = false;
			if (Game1.isFestival() && Game1.currentSeason.Equals("fall"))
			{
				Game1.changeMusicTrack("fallFest");
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026B3 File Offset: 0x000008B3
		public void showDialogue()
		{
			Game1.drawObjectDialogue(this.stringData);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026C0 File Offset: 0x000008C0
		public void warp()
		{
			if (this.stringData != null)
			{
				Point arg_0E_0 = this.pointData;
				Game1.warpFarmer(this.stringData, this.pointData.X, this.pointData.Y, false);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026F3 File Offset: 0x000008F3
		public void removeBuildingsTile()
		{
			Point arg_06_0 = this.pointData;
			if (this.location != null && this.stringData != null)
			{
				this.location.removeTile(this.pointData.X, this.pointData.Y, this.stringData);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002733 File Offset: 0x00000933
		public void removeTemporarySprite()
		{
			if (this.location != null)
			{
				this.location.removeTemporarySpritesWithID(this.floatData);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000274E File Offset: 0x0000094E
		public void playSound()
		{
			if (this.stringData != null)
			{
				Game1.playSound(this.stringData);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002763 File Offset: 0x00000963
		public void changeMusicTrack()
		{
			if (this.stringData != null)
			{
				Game1.changeMusicTrack(this.stringData);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002778 File Offset: 0x00000978
		public void screenFlash()
		{
			if (this.stringData != null && this.stringData.Length > 0)
			{
				Game1.playSound(this.stringData);
			}
			Game1.flashAlpha = this.floatData;
		}

		// Token: 0x04000008 RID: 8
		public int timeUntilAction;

		// Token: 0x04000009 RID: 9
		public float floatData;

		// Token: 0x0400000A RID: 10
		public string stringData;

		// Token: 0x0400000B RID: 11
		public Point pointData;

		// Token: 0x0400000C RID: 12
		public NPC character;

		// Token: 0x0400000D RID: 13
		public GameLocation location;

		// Token: 0x0400000E RID: 14
		public DelayedAction.delayedBehavior behavior;

		// Token: 0x0400000F RID: 15
		public Game1.afterFadeFunction afterFadeBehavior;

		// Token: 0x04000010 RID: 16
		public bool waitUntilMenusGone;

		// Token: 0x04000011 RID: 17
		public TemporaryAnimatedSprite temporarySpriteData;

		// Token: 0x02000162 RID: 354
		// Token: 0x06001350 RID: 4944
		public delegate void delayedBehavior();
	}
}
