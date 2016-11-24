using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StardewValley.Quests
{
	// Token: 0x0200007D RID: 125
	public class LostItemQuest : Quest
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x000D4730 File Offset: 0x000D2930
		public LostItemQuest()
		{
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x000D4738 File Offset: 0x000D2938
		public LostItemQuest(string npcName, string locationOfItem, int itemIndex, int tileX, int tileY)
		{
			this.npcName = npcName;
			this.locationOfItem = locationOfItem;
			this.itemIndex = itemIndex;
			this.tileX = tileX;
			this.tileY = tileY;
			this.questType = 9;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000D4770 File Offset: 0x000D2970
		public override void adjustGameLocation(GameLocation location)
		{
			if (!this.itemFound && location.name.Equals(this.locationOfItem))
			{
				Vector2 position = new Vector2((float)this.tileX, (float)this.tileY);
				if (location.objects.ContainsKey(position))
				{
					location.objects.Remove(position);
				}
				Object o = new Object(position, this.itemIndex, 1);
				o.questItem = true;
				o.isSpawnedObject = true;
				location.objects.Add(position, o);
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x000D47F4 File Offset: 0x000D29F4
		public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
		{
			if (this.completed)
			{
				return false;
			}
			if (item != null && item is Object && (item as Object).parentSheetIndex == this.itemIndex && !this.itemFound)
			{
				this.itemFound = true;
				Game1.player.completelyStopAnimatingOrDoingAction();
				Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Quests:MessageFoundLostItem", new object[]
				{
					item.Name,
					this.npcName
				}));
				this.currentObjective = Game1.content.LoadString("Strings\\Quests:ObjectiveReturnToNPC", new object[]
				{
					this.npcName
				});
				Game1.playSound("jingle1");
			}
			else if (n != null && n.name.Equals(this.npcName) && n.isVillager() && this.itemFound && Game1.player.hasItemInInventory(this.itemIndex, 1, 0))
			{
				base.questComplete();
				if (Game1.temporaryContent == null)
				{
					Game1.temporaryContent = Game1.content.CreateTemporary();
				}
				Dictionary<int, string> questData = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
				string thankYou = (questData[this.id].Length > 9) ? questData[this.id].Split(new char[]
				{
					'/'
				})[9] : Game1.content.LoadString("Data\\ExtraDialogue:LostItemQuest_DefaultThankYou", new object[0]);
				n.setNewDialogue(thankYou, false, false);
				Game1.drawDialogue(n);
				Game1.player.changeFriendship(250, n);
				Game1.player.removeFirstOfThisItemFromInventory(this.itemIndex);
				return true;
			}
			return false;
		}

		// Token: 0x04000A5B RID: 2651
		public string npcName;

		// Token: 0x04000A5C RID: 2652
		public string locationOfItem;

		// Token: 0x04000A5D RID: 2653
		public int itemIndex;

		// Token: 0x04000A5E RID: 2654
		public int tileX;

		// Token: 0x04000A5F RID: 2655
		public int tileY;

		// Token: 0x04000A60 RID: 2656
		public bool itemFound;
	}
}
