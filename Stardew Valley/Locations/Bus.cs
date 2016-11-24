using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
	// Token: 0x0200011E RID: 286
	public class Bus : GameLocation
	{
		// Token: 0x06001050 RID: 4176 RVA: 0x00151B90 File Offset: 0x0014FD90
		public Bus()
		{
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00151B98 File Offset: 0x0014FD98
		public Bus(Map m, string name) : base(m, name)
		{
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00151BA4 File Offset: 0x0014FDA4
		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
			if (tile != null && Game1.year == 1 && Game1.dayOfMonth == 0 && who.IsMainPlayer)
			{
				int tileIndex = tile.TileIndex;
				if (tileIndex <= 238)
				{
					if (tileIndex <= 225)
					{
						if (tileIndex != 221)
						{
							if (tileIndex == 225)
							{
								if (!this.talkedToHaley)
								{
									Game1.drawDialogue(Game1.getCharacterFromName("Haley", false), Game1.parseText("Oooo... My feet hurt from all that shopping.$s#$b#Hey. Could you ask the driver how long it'll be until we get to Stardew?"));
								}
								else
								{
									Game1.drawDialogue(Game1.getCharacterFromName("Haley", false), Game1.parseText("...well?$u"));
								}
								this.talkedToHaley = true;
							}
						}
						else
						{
							Game1.drawObjectDialogue("Occupied");
						}
					}
					else
					{
						switch (tileIndex)
						{
						case 229:
							if (!this.talkedToMan)
							{
								Game1.drawObjectDialogue(Game1.parseText("Moustached Man: Don't worry, friend. I've got my eyes peeled for the valley."));
							}
							break;
						case 230:
						case 231:
							break;
						case 232:
							Game1.drawObjectDialogue(Game1.parseText("It doesn't look too happy in there..."));
							break;
						case 233:
							if (!this.talkedToOldLady)
							{
								Game1.drawObjectDialogue(Game1.parseText("Old Lady: You're liable to hurt someone, waltzing around on a moving bus!"));
							}
							else
							{
								Game1.drawObjectDialogue(Game1.parseText("Old Lady: Ah well, if my knees weren't so stiff I'd probably do the same. Hehee."));
							}
							this.talkedToOldLady = true;
							break;
						default:
							if (tileIndex != 236)
							{
								if (tileIndex == 238)
								{
									if (this.talkedToHaley)
									{
										this.busEvent();
									}
									else
									{
										Game1.drawObjectDialogue(Game1.parseText("Driver: Hey, you'd better take a seat."));
									}
								}
							}
							else
							{
								switch (this.timesBagAttempt)
								{
								case 0:
									Game1.drawObjectDialogue(Game1.parseText("Man: Get your claws off my bag."));
									goto IL_36A;
								case 1:
									Game1.drawObjectDialogue(Game1.parseText("Man: ...Did you hear me, kid? Scram!"));
									goto IL_36A;
								case 2:
									Game1.drawObjectDialogue(Game1.parseText("Man: What are you, some kind of bum?"));
									goto IL_36A;
								case 3:
									Game1.drawObjectDialogue(Game1.parseText("Man: ...*Sigh*... Kids these days..."));
									goto IL_36A;
								case 4:
									Game1.drawObjectDialogue(Game1.parseText("Man: For the love of Yoba, get your filthy paws off my designer bag!"));
									goto IL_36A;
								case 5:
									Game1.drawObjectDialogue(Game1.parseText("Man: That bag costs more than you'll make in ten years!"));
									goto IL_36A;
								case 10:
									Game1.multipleDialogues(new string[]
									{
										Game1.parseText("Man: Arghh!! Take this and leave me alone, you hooligan!"),
										Game1.parseText("Received 1g.")
									});
									Game1.player.money++;
									goto IL_36A;
								}
								Game1.drawObjectDialogue(Game1.parseText("Man: ..."));
								IL_36A:
								this.timesBagAttempt++;
							}
							break;
						}
					}
				}
				else if (tileIndex <= 270)
				{
					if (tileIndex != 265)
					{
						if (tileIndex != 266)
						{
							if (tileIndex == 270)
							{
								if (!this.talkedToWoman)
								{
									Game1.multipleDialogues(new string[]
									{
										Game1.parseText("Lady: We just got back from the Zuzu City fun park... didn't we Simon?"),
										Game1.parseText("Simon: *babble*... *slurp*")
									});
								}
								else
								{
									Game1.drawObjectDialogue(Game1.parseText("Lady: He's shy around strangers."));
								}
								this.talkedToWoman = true;
							}
						}
						else if (Game1.player.isMale)
						{
							Game1.drawObjectDialogue(Game1.parseText("Kid: Dude, don't touch my board."));
						}
					}
					else
					{
						Game1.drawObjectDialogue((Game1.player.isMale || this.talkedToKid) ? "...No response." : "Kid: 'Sup?");
						this.talkedToKid = true;
					}
				}
				else if (tileIndex != 274)
				{
					if (tileIndex != 278)
					{
						if (tileIndex == 459)
						{
							Game1.drawObjectDialogue(Game1.parseText("The bus hasn't arrived yet..."));
						}
					}
					else
					{
						Game1.drawObjectDialogue("Girl: Hi! <");
					}
				}
				else if (!this.foundChange)
				{
					Game1.player.money += 20;
					Game1.drawObjectDialogue(Game1.parseText("Found 20g under the seat!"));
				}
				else
				{
					this.foundChange = true;
				}
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00151FB0 File Offset: 0x001501B0
		public void busEvent()
		{
			this.characters.Add(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Dobson"), 0, Game1.tileSize, Game1.tileSize * 2), new Vector2((float)(-1000 * Game1.tileSize), (float)(3 * Game1.tileSize - Game1.tileSize)), "Bus", 0, "Dobson", false, null, Game1.content.Load<Texture2D>("Portraits\\Dobson")));
			this.currentEvent = new Event("none/10 4/farmer 18 5 0 Dobson -100 -100 2/message \"Driver: Huh? We're almost there. It should only be a few more minutes.\"/pause 500/faceDirection farmer 3/pause 1000/playSound doorClose/warp Dobson 1 4/pause 500/speak Dobson \"Sir? Are you still there?$u#$b#Uh-huh... sorry to put you on hold, sir. I had some business to attend to.$h\"/move Dobson 3 0 1/speak Dobson \"That's right, sir... I'm on the bus right now, heading to Stardew Valley.$h\"/move Dobson 5 0 1/pause 500/faceDirection Dobson 0/speak Dobson \"I have to say, so far I've been quite unimpressed with the locals.$h\"/pause 500/faceDirection Dobson 3/speak Dobson \"They seem like a docile and unfashionable sort of people.$s#$b#The children look sickly, and all the young men are delinquents from what I've seen so far...$h\"/pause 800/move Dobson 2 0 0/pause 800/showFrame Dobson 16/pause 800/speak Dobson \"Although, they are displaying our corporate logo, which you'll be pleased to hear, sir.$h\"/showFrame Dobson 8/pause 400/move Dobson 5 0 1/pause 1000/showFrame Dobson 17/pause 1000/showFrame Dobson 4/pause 1000/faceDirection Dobson 3/speak Dobson \"*snort* ... Yes, sir. It definitely won't be a problem to get these country rubes to sign the paperwork...$u#$b#We'll have that new Joja Hypermarket up and running in no time. Then maybe we can discuss that new-$u\"/message \"Moustached Man: Everyone, look!!\"/pause 800/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 400/changeMapTile Buildings 9 1 185/changeMapTile Buildings 9 2 207/pause 400/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 300/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 400/changeMapTile Buildings 9 1 185/changeMapTile Buildings 9 2 207/pause 400/changeMapTile Buildings 9 1 119/changeMapTile Buildings 9 2 141/pause 300/message \"Moustached Man: There it is! Stardew Valley!\"/pause 400/faceDirection farmer 0/pause 500/faceDirection Dobson 0/pause 1000/end busIntro", -1);
			Game1.eventUp = true;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00152040 File Offset: 0x00150240
		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);
			this.map.Update((long)time.ElapsedGameTime.Milliseconds);
			if (this.currentEvent != null)
			{
				this.currentEvent.checkForNextCommand(this, time);
			}
		}

		// Token: 0x040011C6 RID: 4550
		private bool talkedToKid;

		// Token: 0x040011C7 RID: 4551
		private bool talkedToWoman;

		// Token: 0x040011C8 RID: 4552
		private bool talkedToMan;

		// Token: 0x040011C9 RID: 4553
		private bool foundChange;

		// Token: 0x040011CA RID: 4554
		private bool talkedToOldLady;

		// Token: 0x040011CB RID: 4555
		private bool talkedToHaley;

		// Token: 0x040011CC RID: 4556
		private int timesBagAttempt;
	}
}
