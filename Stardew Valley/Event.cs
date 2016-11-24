using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Objects;
using StardewValley.Tools;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
	// Token: 0x02000023 RID: 35
	public class Event
	{
		// Token: 0x1700001A RID: 26
		public int CurrentCommand
		{
			// Token: 0x06000191 RID: 401 RVA: 0x00013223 File Offset: 0x00011423
			get
			{
				return this.currentCommand;
			}
			// Token: 0x06000192 RID: 402 RVA: 0x0001322B File Offset: 0x0001142B
			set
			{
				this.currentCommand = value;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00013234 File Offset: 0x00011434
		public Event(string eventString, int eventID = -1)
		{
			this.id = eventID;
			this.eventCommands = eventString.Split(new char[]
			{
				'/'
			});
			this.actorPositionsAfterMove = new Dictionary<string, Vector3>();
			this.previousAmbientLight = Game1.ambientLight;
			this.wasBloomDay = Game1.bloomDay;
			this.wasBloomVisible = (Game1.bloom != null && Game1.bloom.Visible);
			if (this.wasBloomDay)
			{
				this.previousBloomSettings = Game1.bloom.Settings;
			}
			if (Game1.player.getMount() != null)
			{
				this.playerWasMounted = true;
				Game1.player.getMount().dismount();
			}
			Game1.player.canOnlyWalk = true;
			Game1.player.showNotCarrying();
			this.drawTool = false;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00013358 File Offset: 0x00011558
		public Event()
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000133D0 File Offset: 0x000115D0
		public bool tryToLoadFestival(string festival)
		{
			Game1.player.festivalScore = 0;
			using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.festivalScore = 0;
				}
			}
			this.temporaryContent = Game1.content.CreateTemporary();
			try
			{
				this.festivalData = this.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + festival);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			string locationName = this.festivalData["conditions"].Split(new char[]
			{
				'/'
			})[0];
			int startTime = Convert.ToInt32(this.festivalData["conditions"].Split(new char[]
			{
				'/'
			})[1].Split(new char[]
			{
				' '
			})[0]);
			int endTime = Convert.ToInt32(this.festivalData["conditions"].Split(new char[]
			{
				'/'
			})[1].Split(new char[]
			{
				' '
			})[1]);
			if (!locationName.Equals(Game1.currentLocation.Name) || Game1.timeOfDay < startTime || Game1.timeOfDay >= endTime || Game1.currentLocation.getFarmers().Count + 1 < Game1.numberOfPlayers())
			{
				return false;
			}
			this.eventCommands = this.festivalData["set-up"].Split(new char[]
			{
				'/'
			});
			this.actorPositionsAfterMove = new Dictionary<string, Vector3>();
			this.previousAmbientLight = Game1.ambientLight;
			bool arg_18F_0 = this.wasBloomDay;
			this.isFestival = true;
			return true;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00013594 File Offset: 0x00011794
		public void endBehaviors(string[] split, GameLocation location)
		{
			Game1.pixelZoom = this.oldPixelZoom;
			if (Game1.currentSong != null && !Game1.currentSong.Name.Contains(Game1.currentSeason) && !this.eventCommands[0].Equals("continue"))
			{
				Game1.changeMusicTrack("none");
			}
			if (split != null && split.Length > 1)
			{
				string text = split[1];
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1997527009u)
				{
					if (num <= 1353598700u)
					{
						if (num != 878656485u)
						{
							if (num != 1266391031u)
							{
								if (num == 1353598700u)
								{
									if (text == "bed")
									{
										Game1.player.position = Game1.player.mostRecentBed + new Vector2(0f, (float)Game1.tileSize);
									}
								}
							}
							else if (text == "wedding")
							{
								if (Game1.player.isMale)
								{
									Game1.player.changeShirt(this.oldShirt);
									Game1.player.changePants(this.oldPants);
									Game1.getCharacterFromName("Lewis", false).CurrentDialogue.Push(new Dialogue("That was a beautiful ceremony. Congratulations!$h", Game1.getCharacterFromName("Lewis", false)));
								}
								Game1.warpFarmer("Farm", Utility.getHomeOfFarmer(Game1.player).getPorchStandingSpot().X - 1, Utility.getHomeOfFarmer(Game1.player).getPorchStandingSpot().Y, 2);
							}
						}
						else if (text == "busIntro")
						{
							Game1.currentMinigame = new Intro(4);
						}
					}
					else if (num != 1358361813u)
					{
						if (num != 1619733218u)
						{
							if (num == 1997527009u)
							{
								if (text == "warpOut")
								{
									int whichWarp = 0;
									if (location is BathHousePool && Game1.player.isMale)
									{
										whichWarp = 1;
									}
									Game1.warpFarmer(location.warps[whichWarp].TargetName, location.warps[whichWarp].TargetX, location.warps[whichWarp].TargetY, true);
									Game1.eventOver = true;
									this.CurrentCommand += 2;
									Game1.screenGlowHold = false;
								}
							}
						}
						else if (text == "invisibleWarpOut")
						{
							Game1.getCharacterFromName(split[2], false).isInvisible = true;
							Game1.warpFarmer(location.warps[0].TargetName, location.warps[0].TargetX, location.warps[0].TargetY, true);
							Game1.fadeScreenToBlack();
							Game1.eventOver = true;
							this.CurrentCommand += 2;
							Game1.screenGlowHold = false;
						}
					}
					else if (text == "credits")
					{
						Game1.debrisWeather.Clear();
						Game1.isDebrisWeather = false;
						Game1.changeMusicTrack("wedding");
						Game1.gameMode = 10;
						this.CurrentCommand += 2;
					}
				}
				else if (num <= 2519057040u)
				{
					if (num != 2158831383u)
					{
						if (num != 2471448074u)
						{
							if (num == 2519057040u)
							{
								if (text == "invisible")
								{
									Game1.getCharacterFromName(split[2], false).isInvisible = true;
								}
							}
						}
						else if (text == "position")
						{
							Game1.player.positionBeforeEvent = new Vector2((float)Convert.ToInt32(split[2]), (float)Convert.ToInt32(split[3]));
						}
					}
					else if (text == "dialogueWarpOut")
					{
						int whichWarp = 0;
						if (location is BathHousePool && Game1.player.isMale)
						{
							whichWarp = 1;
						}
						Game1.warpFarmer(location.warps[whichWarp].TargetName, location.warps[whichWarp].TargetX, location.warps[whichWarp].TargetY, true);
						NPC i = Game1.getCharacterFromName(split[2], false);
						int firstQuoteIndex = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
						int lastQuoteIndex = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
						i.CurrentDialogue.Clear();
						i.CurrentDialogue.Push(new Dialogue(this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex, lastQuoteIndex), i));
						Game1.eventOver = true;
						this.CurrentCommand += 2;
						Game1.screenGlowHold = false;
					}
				}
				else if (num <= 2988976489u)
				{
					if (num != 2923112787u)
					{
						if (num == 2988976489u)
						{
							if (text == "newDay")
							{
								if (Game1.player.isRidingHorse())
								{
									Game1.player.getMount().dismount();
								}
								Game1.player.faceDirection(2);
								Game1.warpFarmer(Utility.getHomeOfFarmer(Game1.player), (int)Game1.player.mostRecentBed.X / Game1.tileSize, (int)Game1.player.mostRecentBed.Y / Game1.tileSize, 2, false);
								Game1.newDay = true;
								Game1.player.currentLocation.lastTouchActionLocation = new Vector2((float)((int)Game1.player.mostRecentBed.X / Game1.tileSize), (float)((int)Game1.player.mostRecentBed.Y / Game1.tileSize));
								Game1.player.completelyStopAnimatingOrDoingAction();
								if (Game1.player.bathingClothes)
								{
									Game1.player.changeOutOfSwimSuit();
								}
								Game1.player.swimming = false;
								Game1.player.CanMove = false;
								Game1.changeMusicTrack("none");
							}
						}
					}
					else if (text == "dialogue")
					{
						NPC i = Game1.getCharacterFromName(split[2], false);
						int firstQuoteIndex = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
						int lastQuoteIndex = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
						if (i != null)
						{
							i.CurrentDialogue.Clear();
							i.CurrentDialogue.Push(new Dialogue(this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex, lastQuoteIndex), i));
						}
					}
				}
				else if (num != 3396363028u)
				{
					if (num == 4256882065u)
					{
						if (text == "Maru1")
						{
							Game1.getCharacterFromName("Demetrius", false).setNewDialogue("Maybe I was being a little presumptuous earlier... sorry.$5", false, false);
							Game1.getCharacterFromName("Maru", false).setNewDialogue("Thanks for helping out in the lab.$h", false, false);
							Game1.warpFarmer(location.warps[0].TargetName, location.warps[0].TargetX, location.warps[0].TargetY, true);
							Game1.fadeScreenToBlack();
							Game1.eventOver = true;
							this.CurrentCommand += 2;
						}
					}
				}
				else if (text == "beginGame")
				{
					Game1.gameMode = 3;
					if (Game1.IsServer)
					{
						Game1.initializeMultiplayerServer();
					}
					if (Game1.IsClient)
					{
						Game1.initializeMultiplayerClient();
					}
					Game1.warpFarmer("FarmHouse", 9, 9, false);
					Game1.NewDay(1000f);
				}
			}
			this.exitEvent();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00013D20 File Offset: 0x00011F20
		public void exitEvent()
		{
			if (this.id != -1 && !Game1.player.eventsSeen.Contains(this.id))
			{
				Game1.player.eventsSeen.Add(this.id);
			}
			Game1.player.canOnlyWalk = false;
			Game1.nonWarpFade = true;
			if (!Game1.fadeIn || Game1.fadeToBlackAlpha >= 1f)
			{
				Game1.fadeScreenToBlack();
			}
			Game1.eventOver = true;
			Game1.fadeToBlack = true;
			this.CurrentCommand += 2;
			Game1.screenGlowHold = false;
			if (this.isFestival)
			{
				Game1.timeOfDay = 2200;
				int timePass = 780;
				if (this.FestivalName != null && (this.FestivalName.Contains("Moonlight") || this.FestivalName.Equals("Spirit's Eve")))
				{
					Game1.timeOfDay = 2400;
					timePass = 240;
				}
				Game1.warpFarmer(Game1.getFarm(), 64 - Utility.getFarmerNumberFromFarmer(Game1.player), 15, 2, false);
				Game1.player.toolOverrideFunction = null;
				this.isFestival = false;
				if (Game1.player.getSpouse() != null)
				{
					Game1.warpCharacter(Game1.player.getSpouse(), "FarmHouse", Utility.getHomeOfFarmer(Game1.player).getSpouseBedSpot(), false, true);
				}
				Game1.currentLocation.currentEvent = null;
				foreach (GameLocation i in Game1.locations)
				{
					i.currentEvent = null;
					using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator2 = i.objects.Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.minutesElapsed(timePass, i);
						}
					}
				}
				Game1.player.freezePause = 1500;
				return;
			}
			if (this.playerWasMounted && Game1.currentLocation.isOutdoors)
			{
				Horse horse = Utility.findHorse();
				if (horse != null)
				{
					Game1.warpCharacter(horse, Game1.currentLocation.name, new Vector2((float)Game1.xLocationAfterWarp, (float)Game1.yLocationAfterWarp), false, true);
				}
			}
			Game1.player.forceCanMove();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00013F54 File Offset: 0x00012154
		public void incrementCommandAfterFade()
		{
			int num = this.CurrentCommand;
			this.CurrentCommand = num + 1;
			Game1.globalFade = false;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00013F78 File Offset: 0x00012178
		public void cleanup()
		{
			Game1.ambientLight = this.previousAmbientLight;
			if (Game1.bloom != null)
			{
				Game1.bloom.Settings = this.previousBloomSettings;
				Game1.bloom.Visible = this.wasBloomVisible;
				Game1.bloom.reload();
			}
			foreach (NPC i in this.npcsWithUniquePortraits)
			{
				i.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + i.name);
				i.uniquePortraitActive = false;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00014028 File Offset: 0x00012228
		public void checkForNextCommand(GameLocation location, GameTime time)
		{
			if (this.skipped)
			{
				return;
			}
			foreach (NPC i in this.actors)
			{
				i.update(time, Game1.currentLocation);
				if (i.Sprite.currentAnimation != null)
				{
					i.Sprite.animateOnce(time);
				}
			}
			if (this.aboveMapSprites != null)
			{
				for (int j = this.aboveMapSprites.Count - 1; j >= 0; j--)
				{
					if (this.aboveMapSprites[j].update(time))
					{
						this.aboveMapSprites.RemoveAt(j);
					}
				}
			}
			if (!this.playerControlSequence)
			{
				Game1.player.setRunning(false, false);
			}
			if (this.npcControllers != null)
			{
				for (int k = this.npcControllers.Count - 1; k >= 0; k--)
				{
					if (this.npcControllers[k].update(time, location, this.npcControllers))
					{
						this.npcControllers.RemoveAt(k);
					}
				}
			}
			if (this.isFestival)
			{
				this.festivalUpdate(time);
			}
			string[] split = this.eventCommands[Math.Min(this.eventCommands.Length - 1, this.CurrentCommand)].Split(new char[]
			{
				' '
			});
			if (this.temporaryLocation != null && !Game1.currentLocation.Equals(this.temporaryLocation))
			{
				this.temporaryLocation.updateEvenIfFarmerIsntHere(time, true);
			}
			if (this.CurrentCommand == 0 && !this.forked && !this.eventSwitched)
			{
				Game1.player.speed = 2;
				Game1.player.running = false;
				Game1.eventOver = false;
				if ((!this.eventCommands[0].Equals("none") || !Game1.isRaining) && !this.eventCommands[0].Equals("continue") && !this.eventCommands[0].Contains("pause"))
				{
					Game1.changeMusicTrack(this.eventCommands[0]);
				}
				if (location is Farm)
				{
					Point p = Farm.getFrontDoorPositionForFarmer(Game1.player);
					Game1.viewport.X = (Game1.currentLocation.IsOutdoors ? Math.Max(0, Math.Min(p.X - Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.currentLocation.Map.DisplayWidth - Game1.graphics.GraphicsDevice.Viewport.Width)) : (p.X - Game1.graphics.GraphicsDevice.Viewport.Width / 2));
					Game1.viewport.Y = (Game1.currentLocation.IsOutdoors ? Math.Max(0, Math.Min(p.Y - Game1.graphics.GraphicsDevice.Viewport.Height / 2, Game1.currentLocation.Map.DisplayHeight - Game1.graphics.GraphicsDevice.Viewport.Height)) : (p.Y - Game1.graphics.GraphicsDevice.Viewport.Height / 2));
				}
				else if (!this.eventCommands[1].Equals("follow"))
				{
					try
					{
						string[] arg_35E_0 = this.eventCommands[1].Split(new char[]
						{
							' '
						});
						Game1.viewportFreeze = true;
						int centerX = Convert.ToInt32(arg_35E_0[0]) * Game1.tileSize + Game1.tileSize / 2;
						int centerY = Convert.ToInt32(arg_35E_0[1]) * Game1.tileSize + Game1.tileSize / 2;
						if (arg_35E_0[0][0] == '-')
						{
							Game1.viewport.X = centerX;
							Game1.viewport.Y = centerY;
						}
						else
						{
							Game1.viewport.X = (Game1.currentLocation.IsOutdoors ? Math.Max(0, Math.Min(centerX - Game1.viewport.Width / 2, Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width)) : (centerX - Game1.viewport.Width / 2));
							Game1.viewport.Y = (Game1.currentLocation.IsOutdoors ? Math.Max(0, Math.Min(centerY - Game1.viewport.Height / 2, Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height)) : (centerY - Game1.viewport.Height / 2));
						}
						if (centerX > 0 && Game1.graphics.GraphicsDevice.Viewport.Width > Game1.currentLocation.Map.DisplayWidth)
						{
							Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
						}
						if (centerY > 0 && Game1.graphics.GraphicsDevice.Viewport.Height > Game1.currentLocation.Map.DisplayHeight)
						{
							Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
						}
					}
					catch (Exception)
					{
						this.forked = true;
						return;
					}
				}
				this.setUpCharacters(this.eventCommands[2], location);
				this.populateWalkLocationsList();
				this.CurrentCommand = 3;
				using (List<NPC>.Enumerator enumerator = this.actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NPC arg_555_0 = enumerator.Current;
					}
					goto IL_43C1;
				}
			}
			if (!Game1.fadeToBlack || this.actorPositionsAfterMove.Count > 0 || this.CurrentCommand > 3 || this.forked)
			{
				if (this.eventCommands.Length <= this.CurrentCommand)
				{
					return;
				}
				Vector3 arg_5B2_0 = this.viewportTarget;
				if (!this.viewportTarget.Equals(Vector3.Zero))
				{
					int playerSpeed = Game1.player.speed;
					Game1.player.speed = (int)this.viewportTarget.X;
					Game1.viewport.X = Game1.viewport.X + (int)this.viewportTarget.X;
					if (this.viewportTarget.X != 0f)
					{
						Game1.updateRainDropPositionForPlayerMovement((this.viewportTarget.X < 0f) ? 3 : 1, true, Math.Abs(this.viewportTarget.X + (float)((Game1.player.isMoving() && Game1.player.facingDirection == 3) ? (-(float)Game1.player.speed) : ((Game1.player.isMoving() && Game1.player.facingDirection == 1) ? Game1.player.speed : 0))));
					}
					Game1.viewport.Y = Game1.viewport.Y + (int)this.viewportTarget.Y;
					Game1.player.speed = (int)this.viewportTarget.Y;
					if (this.viewportTarget.Y != 0f)
					{
						Game1.updateRainDropPositionForPlayerMovement((this.viewportTarget.Y < 0f) ? 0 : 2, true, Math.Abs(this.viewportTarget.Y - (float)((Game1.player.isMoving() && Game1.player.facingDirection == 0) ? (-(float)Game1.player.speed) : ((Game1.player.isMoving() && Game1.player.facingDirection == 2) ? Game1.player.speed : 0))));
					}
					Game1.player.speed = playerSpeed;
					this.viewportTarget.Z = this.viewportTarget.Z - (float)time.ElapsedGameTime.Milliseconds;
					if (this.viewportTarget.Z <= 0f)
					{
						this.viewportTarget = Vector3.Zero;
					}
				}
				if (this.actorPositionsAfterMove.Count > 0)
				{
					string[] array = this.actorPositionsAfterMove.Keys.ToArray<string>();
					for (int num = 0; num < array.Length; num++)
					{
						string s = array[num];
						Microsoft.Xna.Framework.Rectangle targetTile = new Microsoft.Xna.Framework.Rectangle((int)this.actorPositionsAfterMove[s].X * Game1.tileSize, (int)this.actorPositionsAfterMove[s].Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
						targetTile.Inflate(-Game1.pixelZoom, 0);
						if (this.getActorByName(s) != null && this.getActorByName(s).GetBoundingBox().Width > Game1.tileSize)
						{
							targetTile.Width = this.getActorByName(s).GetBoundingBox().Width + Game1.pixelZoom;
							targetTile.Height = this.getActorByName(s).GetBoundingBox().Height + Game1.pixelZoom;
						}
						if (s.Contains("farmer"))
						{
							Farmer f = Utility.getFarmerFromFarmerNumberString(s);
							if (f != null && targetTile.Contains(f.GetBoundingBox()) && (((float)(f.GetBoundingBox().Y - targetTile.Top) <= (float)(Game1.tileSize / 4) + f.getMovementSpeed() && f.FacingDirection != 2) || ((float)(targetTile.Bottom - f.GetBoundingBox().Bottom) <= (float)(Game1.tileSize / 4) + f.getMovementSpeed() && f.FacingDirection == 2)))
							{
								f.showNotCarrying();
								f.Halt();
								f.faceDirection((int)this.actorPositionsAfterMove[s].Z);
								f.FarmerSprite.StopAnimation();
								f.Halt();
								this.actorPositionsAfterMove.Remove(s);
							}
							else if (f != null)
							{
								f.canOnlyWalk = false;
								f.setRunning(false, true);
								f.canOnlyWalk = true;
								f.lastPosition = Game1.player.position;
								f.MovePosition(time, Game1.viewport, location);
							}
						}
						else
						{
							foreach (NPC l in this.actors)
							{
								Microsoft.Xna.Framework.Rectangle r = l.GetBoundingBox();
								if (l.name.Equals(s) && targetTile.Contains(r) && l.GetBoundingBox().Y - targetTile.Top <= Game1.tileSize / 4)
								{
									l.Halt();
									l.faceDirection((int)this.actorPositionsAfterMove[s].Z);
									this.actorPositionsAfterMove.Remove(s);
									break;
								}
								if (l.name.Equals(s))
								{
									l.MovePosition(time, Game1.viewport, null);
									break;
								}
							}
						}
					}
					if (this.actorPositionsAfterMove.Count == 0)
					{
						if (this.continueAfterMove)
						{
							this.continueAfterMove = false;
						}
						else
						{
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
					}
					if (!this.continueAfterMove)
					{
						return;
					}
				}
				if (split[0].Equals("move"))
				{
					int m = 1;
					while (m < split.Length && split.Length - m >= 3)
					{
						if (split[m].Contains("farmer") && !this.actorPositionsAfterMove.ContainsKey(split[m]))
						{
							Farmer f2 = Utility.getFarmerFromFarmerNumberString(split[m]);
							if (f2 != null)
							{
								f2.canOnlyWalk = false;
								f2.setRunning(false, true);
								f2.canOnlyWalk = true;
								f2.convertEventMotionCommandToMovement(new Vector2((float)Convert.ToInt32(split[m + 1]), (float)Convert.ToInt32(split[m + 2])));
								this.actorPositionsAfterMove.Add(split[m], this.getPositionAfterMove(Game1.player, Convert.ToInt32(split[m + 1]), Convert.ToInt32(split[m + 2]), Convert.ToInt32(split[m + 3])));
							}
						}
						else
						{
							NPC n = this.getActorByName(split[m]);
							string name = split[m].Equals("rival") ? Utility.getOtherFarmerNames()[0] : split[m];
							if (!this.actorPositionsAfterMove.ContainsKey(name))
							{
								n.convertEventMotionCommandToMovement(new Vector2((float)Convert.ToInt32(split[m + 1]), (float)Convert.ToInt32(split[m + 2])));
								this.actorPositionsAfterMove.Add(name, this.getPositionAfterMove(n, Convert.ToInt32(split[m + 1]), Convert.ToInt32(split[m + 2]), Convert.ToInt32(split[m + 3])));
							}
						}
						m += 4;
					}
					if (split.Last<string>().Equals("true"))
					{
						this.continueAfterMove = true;
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
					}
					else if (split.Last<string>().Equals("false"))
					{
						this.continueAfterMove = false;
						if (split.Length == 2 && this.actorPositionsAfterMove.Count == 0)
						{
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
					}
				}
				else if (split[0].Equals("speak"))
				{
					if (this.skipped)
					{
						return;
					}
					if (!Game1.dialogueUp)
					{
						this.timeAccumulator += (float)time.ElapsedGameTime.Milliseconds;
						if (this.timeAccumulator < 500f)
						{
							return;
						}
						this.timeAccumulator = 0f;
						NPC n2 = Game1.getCharacterFromName(split[1].Equals("rival") ? Utility.getOtherFarmerNames()[0] : split[1], false);
						if (n2 == null)
						{
							n2 = this.getActorByName(split[1]);
						}
						if (n2 == null)
						{
							Game1.eventFinished();
							return;
						}
						int firstQuoteIndex = this.eventCommands[this.currentCommand].IndexOf('"');
						if (firstQuoteIndex > 0)
						{
							int lastQuoteIndex = this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex + 1).IndexOf('"');
							Game1.player.checkForQuestComplete(n2, -1, -1, null, null, 5, -1);
							if (Game1.NPCGiftTastes.ContainsKey(split[1]) && !Game1.player.friendships.ContainsKey(split[1]))
							{
								Game1.player.friendships.Add(split[1], new int[6]);
							}
							if (lastQuoteIndex > 0)
							{
								n2.CurrentDialogue.Push(new Dialogue(this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex + 1, lastQuoteIndex), n2));
							}
							else
							{
								n2.CurrentDialogue.Push(new Dialogue("...", n2));
							}
						}
						else
						{
							n2.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString(split[2], new object[0]), n2));
						}
						Game1.drawDialogue(n2);
					}
				}
				else if (split[0].Equals("minedeath"))
				{
					if (!Game1.dialogueUp)
					{
						Random r2 = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + Game1.timeOfDay);
						int moneyToLose = r2.Next(Game1.player.Money / 20, Game1.player.Money / 4);
						moneyToLose = Math.Min(moneyToLose, 5000);
						moneyToLose -= (int)((double)Game1.player.LuckLevel * 0.01 * (double)moneyToLose);
						moneyToLose -= moneyToLose % 100;
						int numberOfItemsLost = 0;
						double itemLossRate = 0.25 - (double)Game1.player.LuckLevel * 0.05 - Game1.dailyLuck;
						for (int i2 = Game1.player.Items.Count - 1; i2 >= 0; i2--)
						{
							if (Game1.player.Items[i2] != null && (!(Game1.player.Items[i2] is Tool) || (Game1.player.Items[i2] is MeleeWeapon && (Game1.player.Items[i2] as MeleeWeapon).initialParentTileIndex != 47 && (Game1.player.Items[i2] as MeleeWeapon).initialParentTileIndex != 4)) && Game1.player.Items[i2].canBeTrashed() && !(Game1.player.Items[i2] is Ring) && r2.NextDouble() < itemLossRate)
							{
								numberOfItemsLost++;
								Game1.player.Items[i2] = null;
							}
						}
						Game1.player.Stamina = Math.Min(Game1.player.Stamina, 2f);
						int mineLevelsToLose = (int)((double)(10 - Game1.player.LuckLevel / 3) - Game1.dailyLuck * 20.0);
						Game1.player.deepestMineLevel = Math.Max(1, Game1.mine.lowestLevelReached - mineLevelsToLose);
						if (Game1.mine != null)
						{
							Game1.mine.lowestLevelReached = Math.Max(1, Game1.mine.lowestLevelReached - mineLevelsToLose);
						}
						Game1.player.Money = Math.Max(0, Game1.player.money - moneyToLose);
						Game1.drawObjectDialogue(((mineLevelsToLose > 0) ? ("I must have hit my head pretty hard... I've forgotten everything about the last " + mineLevelsToLose + " levels of the mine. ") : "Ow...") + ((moneyToLose <= 0) ? "" : ("I also seem to have lost " + moneyToLose + "gp")) + ((numberOfItemsLost > 0) ? ((moneyToLose <= 0) ? ("It seems I've lost " + ((numberOfItemsLost == 1) ? "an item from my backpack." : (numberOfItemsLost + " items from my backpack."))) : (", and " + ((numberOfItemsLost == 1) ? "an item from my backpack." : (numberOfItemsLost + " items from my backpack.")))) : ((moneyToLose <= 0) ? "" : ".")));
					}
				}
				else if (split[0].Equals("hospitaldeath"))
				{
					if (!Game1.dialogueUp)
					{
						Random r3 = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + Game1.timeOfDay);
						int numberOfItemsLost2 = 0;
						double itemLossRate2 = 0.25 - (double)Game1.player.LuckLevel * 0.05 - Game1.dailyLuck;
						for (int i3 = Game1.player.Items.Count - 1; i3 >= 0; i3--)
						{
							if (Game1.player.Items[i3] != null && (!(Game1.player.Items[i3] is Tool) || (Game1.player.Items[i3] is MeleeWeapon && (Game1.player.Items[i3] as MeleeWeapon).initialParentTileIndex != 47 && (Game1.player.Items[i3] as MeleeWeapon).initialParentTileIndex != 4)) && Game1.player.Items[i3].canBeTrashed() && !(Game1.player.Items[i3] is Ring) && r3.NextDouble() < itemLossRate2)
							{
								numberOfItemsLost2++;
								Game1.player.Items[i3] = null;
							}
						}
						Game1.player.Stamina = Math.Min(Game1.player.Stamina, 2f);
						int moneyToLose2 = Math.Min(1000, Game1.player.money);
						Game1.player.Money -= moneyToLose2;
						Game1.drawObjectDialogue(((moneyToLose2 > 0) ? ("Dr. Harvey charged me " + moneyToLose2 + "g for the hospital visit. ") : "I have no money, but Harvey was obligated to save my life free of charge. ") + ((numberOfItemsLost2 > 0) ? ("It also appears that I've lost " + ((numberOfItemsLost2 == 1) ? "an item from my backpack." : (numberOfItemsLost2 + " items from my backpack."))) : ""));
					}
				}
				else if (split[0].Equals("end"))
				{
					this.endBehaviors(split, location);
				}
				else if (split[0].Equals("skippable"))
				{
					this.skippable = true;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("emote"))
				{
					bool nextCommandImmediate = split.Length > 3;
					if (split[1].Contains("farmer"))
					{
						if (Utility.getFarmerFromFarmerNumberString(split[1]) != null)
						{
							Game1.player.doEmote(Convert.ToInt32(split[2]), !nextCommandImmediate);
						}
					}
					else
					{
						NPC n3 = this.getActorByName(split[1]);
						if (!n3.isEmoting)
						{
							n3.doEmote(Convert.ToInt32(split[2]), !nextCommandImmediate);
						}
					}
					if (nextCommandImmediate)
					{
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
						this.checkForNextCommand(location, time);
					}
				}
				else if (split[0].Equals("stopMusic"))
				{
					Game1.changeMusicTrack("none");
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("playSound"))
				{
					Game1.playSound(split[1]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("pause"))
				{
					if (Game1.pauseTime <= 0f)
					{
						Game1.pauseTime = (float)Convert.ToInt32(split[1]);
					}
				}
				else if (split[0].Equals("resetVariable"))
				{
					this.specialEventVariable1 = false;
					this.currentCommand++;
				}
				else if (split[0].Equals("faceDirection"))
				{
					if (split[1].Contains("farmer"))
					{
						Farmer f3 = Utility.getFarmerFromFarmerNumberString(split[1]);
						if (f3 != null)
						{
							f3.FarmerSprite.StopAnimation();
							f3.completelyStopAnimatingOrDoingAction();
							f3.faceDirection(Convert.ToInt32(split[2]));
							f3.FarmerSprite.StopAnimation();
						}
					}
					else if (split[1].Contains("spouse"))
					{
						if (Game1.player.spouse != null && Game1.player.spouse.Length > 0 && this.getActorByName(Game1.player.spouse.Replace("engaged", "")) != null)
						{
							this.getActorByName(Game1.player.spouse.Replace("engaged", "")).faceDirection(Convert.ToInt32(split[2]));
						}
					}
					else
					{
						this.getActorByName(split[1]).faceDirection(Convert.ToInt32(split[2]));
					}
					if (split.Length == 3 && Game1.pauseTime <= 0f)
					{
						Game1.pauseTime = 500f;
					}
					else if (split.Length > 3)
					{
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
						this.checkForNextCommand(location, time);
					}
				}
				else if (split[0].Equals("warp"))
				{
					if (split[1].Contains("farmer"))
					{
						Farmer f4 = Utility.getFarmerFromFarmerNumberString(split[1]);
						if (f4 != null)
						{
							f4.position.X = (float)(Convert.ToInt32(split[2]) * Game1.tileSize);
							f4.position.Y = (float)(Convert.ToInt32(split[3]) * Game1.tileSize);
							if (Game1.IsClient)
							{
								f4.remotePosition = new Vector2(f4.position.X, f4.position.Y);
							}
						}
					}
					else if (split[1].Contains("spouse"))
					{
						if (Game1.player.spouse != null && Game1.player.spouse.Length > 0 && this.getActorByName(Game1.player.spouse.Replace("engaged", "")) != null)
						{
							for (int i4 = this.npcControllers.Count - 1; i4 >= 0; i4--)
							{
								if (this.npcControllers[i4].puppet.name.Equals(Game1.player.spouse.Replace("engaged", "")))
								{
									this.npcControllers.RemoveAt(i4);
								}
							}
							this.getActorByName(Game1.player.spouse.Replace("engaged", "")).position = new Vector2((float)(Convert.ToInt32(split[2]) * Game1.tileSize), (float)(Convert.ToInt32(split[3]) * Game1.tileSize));
						}
					}
					else
					{
						NPC n4 = this.getActorByName(split[1]);
						if (n4 != null)
						{
							n4.position.X = (float)(Convert.ToInt32(split[2]) * Game1.tileSize + Game1.pixelZoom);
							n4.position.Y = (float)(Convert.ToInt32(split[3]) * Game1.tileSize);
						}
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					if (split.Length > 4)
					{
						this.checkForNextCommand(location, time);
					}
				}
				else if (split[0].Equals("speed"))
				{
					if (split[1].Equals("farmer"))
					{
						this.farmerAddedSpeed = Convert.ToInt32(split[2]);
					}
					else
					{
						this.getActorByName(split[1]).speed = Convert.ToInt32(split[2]);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("stopAdvancedMoves"))
				{
					this.npcControllers.Clear();
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("doAction"))
				{
					location.checkAction(new Location(Convert.ToInt32(split[1]), Convert.ToInt32(split[2])), Game1.viewport, Game1.player);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("removeTile"))
				{
					location.removeTile(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), split[3]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("textAboveHead"))
				{
					NPC n5 = this.getActorByName(split[1]);
					if (n5 != null)
					{
						int firstQuoteIndex2 = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
						int lastQuoteIndex2 = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
						n5.showTextAboveHead(this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex2, lastQuoteIndex2), -1, 2, 3000, 0);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("showFrame"))
				{
					if (split.Length > 2 && !split[2].Equals("flip") && !split[1].Contains("farmer"))
					{
						NPC n6 = this.getActorByName(split[1]);
						if (n6 != null)
						{
							n6.sprite.CurrentFrame = Convert.ToInt32(split[2]);
							if (split[1].Equals("spouse") && n6.gender == 0 && n6.sprite.CurrentFrame >= 36 && n6.sprite.CurrentFrame <= 38)
							{
								n6.sprite.CurrentFrame += 12;
							}
						}
					}
					else
					{
						Farmer f5 = Utility.getFarmerFromFarmerNumberString(split[1]);
						if (split.Length == 2)
						{
							f5 = Game1.player;
						}
						if (f5 != null)
						{
							if (split.Length > 2)
							{
								split[1] = split[2];
							}
							List<FarmerSprite.AnimationFrame> animationFrames = new List<FarmerSprite.AnimationFrame>();
							animationFrames.Add(new FarmerSprite.AnimationFrame(Convert.ToInt32(split[1]), 100, false, split.Length > 2, null, false));
							f5.FarmerSprite.setCurrentAnimation(animationFrames.ToArray());
							f5.FarmerSprite.loopThisAnimation = true;
							f5.FarmerSprite.PauseForSingleAnimation = true;
							f5.sprite.CurrentFrame = Convert.ToInt32(split[1]);
						}
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("farmerAnimation"))
				{
					Game1.player.FarmerSprite.setCurrentSingleAnimation(Convert.ToInt32(split[1]));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("animate"))
				{
					int interval = Convert.ToInt32(split[4]);
					bool flip = split[2].Equals("true");
					bool loop = split[3].Equals("true");
					List<FarmerSprite.AnimationFrame> animationFrames2 = new List<FarmerSprite.AnimationFrame>();
					for (int i5 = 5; i5 < split.Length; i5++)
					{
						animationFrames2.Add(new FarmerSprite.AnimationFrame(Convert.ToInt32(split[i5]), interval, false, flip, null, false));
					}
					if (split[1].Contains("farmer"))
					{
						Farmer f6 = Utility.getFarmerFromFarmerNumberString(split[1]);
						if (f6 != null)
						{
							f6.FarmerSprite.setCurrentAnimation(animationFrames2.ToArray());
							f6.FarmerSprite.loopThisAnimation = loop;
							f6.FarmerSprite.PauseForSingleAnimation = true;
						}
					}
					else
					{
						NPC n7 = this.getActorByName(split[1].Replace('_', ' '));
						if (n7 != null)
						{
							n7.Sprite.setCurrentAnimation(animationFrames2);
							n7.Sprite.loop = loop;
						}
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("stopAnimation"))
				{
					int num;
					if (split[1].Contains("farmer"))
					{
						Farmer f7 = Utility.getFarmerFromFarmerNumberString(split[1]);
						if (f7 != null)
						{
							f7.completelyStopAnimatingOrDoingAction();
							f7.Halt();
							f7.FarmerSprite.currentAnimation = null;
							num = f7.facingDirection;
							switch (num)
							{
							case 0:
								f7.FarmerSprite.setCurrentSingleFrame(12, 32000, false, false);
								break;
							case 1:
								f7.FarmerSprite.setCurrentSingleFrame(6, 32000, false, false);
								break;
							case 2:
								f7.FarmerSprite.setCurrentSingleFrame(0, 32000, false, false);
								break;
							case 3:
								f7.FarmerSprite.setCurrentSingleFrame(6, 32000, false, true);
								break;
							}
						}
					}
					else
					{
						NPC n8 = this.getActorByName(split[1]);
						if (n8 != null)
						{
							n8.Sprite.StopAnimation();
							if (split.Length > 2)
							{
								n8.Sprite.CurrentFrame = Convert.ToInt32(split[2]);
							}
						}
					}
					num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("showRivalFrame"))
				{
					this.getActorByName("rival").sprite.CurrentFrame = Convert.ToInt32(split[1]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("weddingSprite"))
				{
					this.getActorByName("WeddingOutfits").sprite.CurrentFrame = Convert.ToInt32(split[1]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("changeLocation"))
				{
					Event e = Game1.currentLocation.currentEvent;
					Game1.currentLocation.currentEvent = null;
					Game1.currentLocation.cleanupBeforePlayerExit();
					Game1.currentLocation = Game1.getLocationFromName(split[1]);
					Game1.currentLocation.currentEvent = e;
					Game1.player.currentLocation = Game1.currentLocation;
					Game1.currentLocation.resetForPlayerEntry();
					Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
					this.temporaryLocation = null;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("halt"))
				{
					using (List<NPC>.Enumerator enumerator = this.actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.Halt();
						}
					}
					Game1.player.Halt();
					using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator2 = Game1.otherFarmers.Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.Halt();
						}
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.continueAfterMove = false;
					this.actorPositionsAfterMove.Clear();
				}
				else if (split[0].Equals("message"))
				{
					if (!Game1.dialogueUp && Game1.activeClickableMenu == null)
					{
						int firstQuoteIndex3 = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
						int lastQuoteIndex3 = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
						if (lastQuoteIndex3 > 0)
						{
							Game1.drawDialogueNoTyping(Game1.parseText(this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex3, lastQuoteIndex3)));
						}
						else
						{
							Game1.drawDialogueNoTyping("...");
						}
					}
				}
				else if (split[0].Equals("addCookingRecipe"))
				{
					Game1.player.cookingRecipes.Add(this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf(' ') + 1), 0);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("itemAboveHead"))
				{
					if (split.Length > 1 && split[1].Equals("pan"))
					{
						Game1.player.holdUpItemThenMessage(new Pan(), true);
					}
					else if (split.Length > 1 && split[1].Equals("hero"))
					{
						Game1.player.holdUpItemThenMessage(new Object(Vector2.Zero, 116, false), true);
					}
					else if (split.Length > 1 && split[1].Equals("sculpture"))
					{
						Game1.player.holdUpItemThenMessage(new Furniture(1306, Vector2.Zero), true);
					}
					else if (split.Length > 1 && split[1].Equals("joja"))
					{
						Game1.player.holdUpItemThenMessage(new Object(Vector2.Zero, 117, false), true);
					}
					else if (split.Length > 1 && split[1].Equals("slimeEgg"))
					{
						Game1.player.holdUpItemThenMessage(new Object(680, 1, false, -1, 0), true);
					}
					else if (split.Length > 1 && split[1].Equals("rod"))
					{
						Game1.player.holdUpItemThenMessage(new FishingRod(), true);
					}
					else if (split.Length > 1 && split[1].Equals("sword"))
					{
						Game1.player.holdUpItemThenMessage(new MeleeWeapon(0), true);
					}
					else if (split.Length > 1 && split[1].Equals("ore"))
					{
						Game1.player.holdUpItemThenMessage(new Object(378, 1, false, -1, 0), false);
					}
					else
					{
						Game1.player.holdUpItemThenMessage(null, false);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("addCraftingRecipe"))
				{
					if (!Game1.player.craftingRecipes.ContainsKey(this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf(' ') + 1)))
					{
						Game1.player.craftingRecipes.Add(this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf(' ') + 1), 0);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("mail"))
				{
					if (!Game1.player.hasOrWillReceiveMail(split[1]))
					{
						Game1.addMailForTomorrow(split[1], false, false);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("shake"))
				{
					this.getActorByName(split[1]).shake(Convert.ToInt32(split[2]));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("temporarySprite"))
				{
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Convert.ToInt32(split[3]), new Vector2((float)(Convert.ToInt32(split[1]) * Game1.tileSize), (float)(Convert.ToInt32(split[2]) * Game1.tileSize)), Color.White, Convert.ToInt32(split[4]), split.Length > 6 && split[6] == "true", (split.Length > 5) ? ((float)Convert.ToInt32(split[5])) : 300f, 0, Game1.tileSize, (split.Length > 7) ? ((float)Convert.ToDouble(split[7])) : -1f, -1, 0));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("removeTemporarySprites"))
				{
					location.TemporarySprites.Clear();
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("specificTemporarySprite"))
				{
					this.addSpecificTemporarySprite(split[1], location, split);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("playMusic"))
				{
					if (split[1].Equals("samBand"))
					{
						if (Game1.player.DialogueQuestionsAnswered.Contains(78))
						{
							Game1.changeMusicTrack("shimmeringbastion");
						}
						else if (Game1.player.DialogueQuestionsAnswered.Contains(79))
						{
							Game1.changeMusicTrack("honkytonky");
						}
						else if (Game1.player.DialogueQuestionsAnswered.Contains(76))
						{
							Game1.changeMusicTrack("poppy");
						}
						else if (Game1.player.DialogueQuestionsAnswered.Contains(77))
						{
							Game1.changeMusicTrack("heavy");
						}
					}
					else if (Game1.options.musicVolumeLevel > 0f)
					{
						StringBuilder b = new StringBuilder(split[1]);
						for (int i6 = 2; i6 < split.Length; i6++)
						{
							b.Append(" " + split[i6]);
						}
						Game1.changeMusicTrack(b.ToString());
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("nameSelect"))
				{
					if (!Game1.nameSelectUp)
					{
						Game1.showNameSelectScreen(split[1]);
					}
				}
				else if (split[0].Equals("characterSelect"))
				{
					if (Game1.gameMode != 5)
					{
						Game1.gameMode = 5;
						Game1.menuChoice = 0;
					}
				}
				else if (split[0].Equals("addObject"))
				{
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Convert.ToInt32(split[3]), 9999f, 1, 9999, new Vector2((float)Convert.ToInt32(split[1]), (float)Convert.ToInt32(split[2])) * (float)Game1.tileSize, false, false)
					{
						layerDepth = (float)(Convert.ToInt32(split[2]) * Game1.tileSize) / 10000f
					});
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("addBigProp"))
				{
					this.props.Add(new Object(new Vector2((float)Convert.ToInt32(split[1]), (float)Convert.ToInt32(split[2])), Convert.ToInt32(split[3]), false));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("addProp") || split[0].Equals("addFloorProp"))
				{
					int tileX = Convert.ToInt32(split[2]);
					int tileY = Convert.ToInt32(split[3]);
					int index = Convert.ToInt32(split[1]);
					int drawWidth = (split.Length > 4) ? Convert.ToInt32(split[4]) : 1;
					int drawHeight = (split.Length > 5) ? Convert.ToInt32(split[5]) : 1;
					int boundingHeight = (split.Length > 6) ? Convert.ToInt32(split[6]) : drawHeight;
					bool solid = !split[0].Contains("Floor");
					if (this.festivalTexture == null)
					{
						if (this.temporaryContent == null)
						{
							this.temporaryContent = Game1.content.CreateTemporary();
						}
						this.festivalTexture = this.temporaryContent.Load<Texture2D>("LooseSprites\\Festivals");
					}
					this.festivalProps.Add(new Prop(this.festivalTexture, index, drawWidth, boundingHeight, drawHeight, tileX, tileY, solid));
					if (split.Length > 7)
					{
						int tilesHorizontal = Convert.ToInt32(split[7]);
						for (int x = tileX + tilesHorizontal; x != tileX; x -= Math.Sign(tilesHorizontal))
						{
							this.festivalProps.Add(new Prop(this.festivalTexture, index, drawWidth, boundingHeight, drawHeight, x, tileY, solid));
						}
					}
					if (split.Length > 8)
					{
						int tilesVertical = Convert.ToInt32(split[8]);
						for (int y = tileY + tilesVertical; y != tileY; y -= Math.Sign(tilesVertical))
						{
							this.festivalProps.Add(new Prop(this.festivalTexture, index, drawWidth, boundingHeight, drawHeight, tileX, y, solid));
						}
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("addToTable"))
				{
					if (location is FarmHouse)
					{
						(location as FarmHouse).furniture[0].heldObject = new Object(Vector2.Zero, Convert.ToInt32(split[3]), 1);
					}
					else
					{
						location.objects[new Vector2((float)Convert.ToInt32(split[1]), (float)Convert.ToInt32(split[2]))].heldObject = new Object(Vector2.Zero, Convert.ToInt32(split[3]), 1);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("removeObject"))
				{
					Vector2 position = new Vector2((float)Convert.ToInt32(split[1]), (float)Convert.ToInt32(split[2]));
					for (int i7 = this.props.Count - 1; i7 >= 0; i7--)
					{
						if (this.props[i7].TileLocation.Equals(position))
						{
							this.props.RemoveAt(i7);
							break;
						}
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
				}
				else if (split[0].Equals("glow"))
				{
					bool hold = false;
					if (split.Length > 4 && split[4].Equals("true"))
					{
						hold = true;
					}
					Game1.screenGlowOnce(new Color(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3])), hold, 0.005f, 0.3f);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("stopGlowing"))
				{
					Game1.screenGlowUp = false;
					Game1.screenGlowHold = false;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("addQuest"))
				{
					Game1.player.addQuest(Convert.ToInt32(split[1]));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("removeQuest"))
				{
					Game1.player.removeQuest(Convert.ToInt32(split[1]));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("awardFestivalPrize"))
				{
					if (Game1.player.festivalScore == -9999)
					{
						string text = this.festivalData["name"];
						if (!(text == "Egg Festival"))
						{
							if (text == "Festival of Ice")
							{
								if (!Game1.player.mailReceived.Contains("Ice Festival"))
								{
									if (Game1.activeClickableMenu == null)
									{
										Game1.activeClickableMenu = new ItemGrabMenu(new List<Item>
										{
											new Hat(17),
											new Object(687, 1, false, -1, 0),
											new Object(691, 1, false, -1, 0),
											new Object(703, 1, false, -1, 0)
										});
									}
									Game1.player.mailReceived.Add("Ice Festival");
									int num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
									return;
								}
								Game1.player.money += 2000;
								Game1.playSound("money");
								Game1.drawObjectDialogue("Received 2000g ");
								this.CurrentCommand += 2;
							}
						}
						else
						{
							if (!Game1.player.mailReceived.Contains("Egg Festival"))
							{
								if (Game1.activeClickableMenu == null)
								{
									Game1.player.addItemByMenuIfNecessary(new Hat(4), null);
								}
								Game1.player.mailReceived.Add("Egg Festival");
								int num = this.CurrentCommand;
								this.CurrentCommand = num + 1;
								if (Game1.activeClickableMenu == null)
								{
									num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
								}
								return;
							}
							Game1.player.money += 1000;
							Game1.playSound("money");
							this.CurrentCommand += 2;
							Game1.drawObjectDialogue("Received 1000g ");
						}
					}
					else if (split.Length > 1)
					{
						string text = split[1].ToLower();
						uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num2 <= 890111454u)
						{
							if (num2 != 456875097u)
							{
								if (num2 != 659772054u)
								{
									if (num2 == 890111454u)
									{
										if (text == "rod")
										{
											Game1.player.addItemByMenuIfNecessary(new FishingRod(), null);
											int num;
											if (Game1.activeClickableMenu == null)
											{
												num = this.CurrentCommand;
												this.CurrentCommand = num + 1;
											}
											num = this.CurrentCommand;
											this.CurrentCommand = num + 1;
											return;
										}
									}
								}
								else if (text == "sword")
								{
									Game1.player.addItemByMenuIfNecessary(new MeleeWeapon(0), null);
									int num;
									if (Game1.activeClickableMenu == null)
									{
										num = this.CurrentCommand;
										this.CurrentCommand = num + 1;
									}
									num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
									return;
								}
							}
							else if (text == "hero")
							{
								Game1.getSteamAchievement("Achievement_LocalLegend");
								Game1.player.addItemByMenuIfNecessary(new Object(Vector2.Zero, 116, false), null);
								int num;
								if (Game1.activeClickableMenu == null)
								{
									num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
								}
								num = this.CurrentCommand;
								this.CurrentCommand = num + 1;
								return;
							}
						}
						else if (num2 <= 1716282848u)
						{
							if (num2 != 1331031788u)
							{
								if (num2 == 1716282848u)
								{
									if (text == "slimeegg")
									{
										Game1.player.addItemByMenuIfNecessary(new Object(680, 1, false, -1, 0), null);
										int num;
										if (Game1.activeClickableMenu == null)
										{
											num = this.CurrentCommand;
											this.CurrentCommand = num + 1;
										}
										num = this.CurrentCommand;
										this.CurrentCommand = num + 1;
										return;
									}
								}
							}
							else if (text == "pan")
							{
								Game1.player.addItemByMenuIfNecessary(new Pan(), null);
								int num;
								if (Game1.activeClickableMenu == null)
								{
									num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
								}
								num = this.CurrentCommand;
								this.CurrentCommand = num + 1;
								return;
							}
						}
						else if (num2 != 3793392911u)
						{
							if (num2 == 3820005428u)
							{
								if (text == "sculpture")
								{
									Game1.player.addItemByMenuIfNecessary(new Furniture(1306, Vector2.Zero), null);
									int num;
									if (Game1.activeClickableMenu == null)
									{
										num = this.CurrentCommand;
										this.CurrentCommand = num + 1;
									}
									num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
									return;
								}
							}
						}
						else if (text == "joja")
						{
							Game1.getSteamAchievement("Achievement_Joja");
							Game1.player.addItemByMenuIfNecessary(new Object(Vector2.Zero, 117, false), null);
							int num;
							if (Game1.activeClickableMenu == null)
							{
								num = this.CurrentCommand;
								this.CurrentCommand = num + 1;
							}
							num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
							return;
						}
					}
					else
					{
						this.CurrentCommand += 2;
					}
				}
				else if (split[0].Equals("pixelZoom"))
				{
					Game1.pixelZoom = Convert.ToInt32(split[1]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("attachCharacterToTempSprite"))
				{
					TemporaryAnimatedSprite t = location.temporarySprites.Last<TemporaryAnimatedSprite>();
					if (t != null)
					{
						t.attachedCharacter = this.getActorByName(split[1]);
					}
				}
				else if (split[0].Equals("fork"))
				{
					if (split.Length > 2)
					{
						int i8;
						if (Game1.player.mailReceived.Contains(split[1]) || (int.TryParse(split[1], out i8) && Game1.player.dialogueQuestionsAnswered.Contains(i8)))
						{
							string[] newCommands = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + Game1.currentLocation.Name)[split[2]].Split(new char[]
							{
								'/'
							});
							this.eventCommands = newCommands;
							this.CurrentCommand = 0;
							this.forked = !this.forked;
						}
						else
						{
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
					}
					else if (this.specialEventVariable1)
					{
						string[] newCommands2 = this.isFestival ? this.festivalData[split[1]].Split(new char[]
						{
							'/'
						}) : Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + Game1.currentLocation.Name)[split[1]].Split(new char[]
						{
							'/'
						});
						this.eventCommands = newCommands2;
						this.CurrentCommand = 0;
						this.forked = !this.forked;
					}
					else
					{
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
					}
				}
				else if (split[0].Equals("switchEvent"))
				{
					string[] newCommands3;
					if (this.isFestival)
					{
						newCommands3 = this.festivalData[split[1]].Split(new char[]
						{
							'/'
						});
					}
					else
					{
						newCommands3 = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + Game1.currentLocation.Name)[split[1]].Split(new char[]
						{
							'/'
						});
					}
					this.eventCommands = newCommands3;
					this.CurrentCommand = 0;
					this.eventSwitched = true;
				}
				else if (split[0].Equals("globalFade"))
				{
					if (!Game1.globalFade)
					{
						if (split.Length > 2)
						{
							Game1.globalFadeToBlack(null, (split.Length > 1) ? ((float)Convert.ToDouble(split[1])) : 0.007f);
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
						else
						{
							Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.incrementCommandAfterFade), (split.Length > 1) ? ((float)Convert.ToDouble(split[1])) : 0.007f);
						}
					}
				}
				else if (split[0].Equals("globalFadeToClear"))
				{
					if (!Game1.globalFade)
					{
						if (split.Length > 2)
						{
							Game1.globalFadeToClear(null, (split.Length > 1) ? ((float)Convert.ToDouble(split[1])) : 0.007f);
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
						else
						{
							Game1.globalFadeToClear(new Game1.afterFadeFunction(this.incrementCommandAfterFade), (split.Length > 1) ? ((float)Convert.ToDouble(split[1])) : 0.007f);
						}
					}
				}
				else if (split[0].Equals("cutscene"))
				{
					if (Game1.currentMinigame == null)
					{
						string text = split[1];
						uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
						if (num2 <= 679977874u)
						{
							if (num2 <= 306545799u)
							{
								if (num2 != 48516534u)
								{
									if (num2 != 113222854u)
									{
										if (num2 == 306545799u)
										{
											if (text == "linusMoneyGone")
											{
												foreach (TemporaryAnimatedSprite expr_35A9 in location.temporarySprites)
												{
													expr_35A9.alphaFade = 0.01f;
													expr_35A9.motion = new Vector2(0f, -1f);
												}
												int num = this.CurrentCommand;
												this.CurrentCommand = num + 1;
												return;
											}
										}
									}
									else if (text == "clearTempSprites")
									{
										location.temporarySprites.Clear();
										int num = this.CurrentCommand;
										this.CurrentCommand = num + 1;
									}
								}
								else if (text == "AbigailGame")
								{
									Game1.currentMinigame = new AbigailGame(true);
								}
							}
							else if (num2 <= 477618658u)
							{
								if (num2 != 323687113u)
								{
									if (num2 == 477618658u)
									{
										if (text == "governorTaste")
										{
											this.governorTaste();
											this.currentCommand++;
											return;
										}
									}
								}
								else if (text == "boardGame")
								{
									Game1.currentMinigame = new FantasyBoardGame();
									int num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
								}
							}
							else if (num2 != 658671424u)
							{
								if (num2 == 679977874u)
								{
									if (text == "balloonChangeMap")
									{
										location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1183, 84, 160), 10000f, 1, 99999, new Vector2(22f, 36f) * (float)Game1.tileSize + new Vector2(-23f, 0f) * (float)Game1.pixelZoom, false, false, 2E-05f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
										{
											motion = new Vector2(0f, -2f),
											yStopCoordinate = 9 * Game1.tileSize,
											reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.balloonInSky),
											attachedCharacter = Game1.player,
											id = 1f
										});
										location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(84, 1205, 38, 26), 10000f, 1, 99999, new Vector2(22f, 36f) * (float)Game1.tileSize + new Vector2(0f, 134f) * (float)Game1.pixelZoom, false, false, (float)(41 * Game1.tileSize) / 10000f + 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
										{
											motion = new Vector2(0f, -2f),
											id = 2f,
											attachedCharacter = this.getActorByName("Harvey")
										});
										int num = this.CurrentCommand;
										this.CurrentCommand = num + 1;
									}
								}
							}
							else if (text == "eggHuntWinner")
							{
								this.eggHuntWinner();
								int num = this.CurrentCommand;
								this.CurrentCommand = num + 1;
								return;
							}
						}
						else if (num2 <= 3320997440u)
						{
							if (num2 != 2271400013u)
							{
								if (num2 != 3007187074u)
								{
									if (num2 == 3320997440u)
									{
										if (text == "haleyCows")
										{
											Game1.currentMinigame = new HaleyCowPictures();
										}
									}
								}
								else if (text == "bandFork")
								{
									int whichBand = 76;
									if (Game1.player.dialogueQuestionsAnswered.Contains(77))
									{
										whichBand = 77;
									}
									else if (Game1.player.dialogueQuestionsAnswered.Contains(78))
									{
										whichBand = 78;
									}
									else if (Game1.player.dialogueQuestionsAnswered.Contains(79))
									{
										whichBand = 79;
									}
									this.answerDialogue("bandFork", whichBand);
									int num = this.CurrentCommand;
									this.CurrentCommand = num + 1;
									return;
								}
							}
							else if (text == "iceFishingWinner")
							{
								this.iceFishingWinner();
								this.currentCommand++;
								return;
							}
						}
						else if (num2 <= 3457429727u)
						{
							if (num2 != 3435855957u)
							{
								if (num2 == 3457429727u)
								{
									if (text == "robot")
									{
										Game1.currentMinigame = new RobotBlastoff();
									}
								}
							}
							else if (text == "plane")
							{
								Game1.currentMinigame = new PlaneFlyBy();
							}
						}
						else if (num2 != 3774548842u)
						{
							if (num2 == 4262882538u)
							{
								if (text == "addSecretSantaItem")
								{
									Item o = Utility.getGiftFromNPC(this.mySecretSanta);
									Game1.player.addItemByMenuIfNecessaryElseHoldUp(o, null);
									this.currentCommand++;
									return;
								}
							}
						}
						else if (text == "balloonDepart")
						{
							TemporaryAnimatedSprite expr_3659 = location.getTemporarySpriteByID(1);
							expr_3659.attachedCharacter = Game1.player;
							expr_3659.motion = new Vector2(0f, -2f);
							TemporaryAnimatedSprite expr_367F = location.getTemporarySpriteByID(2);
							expr_367F.attachedCharacter = this.getActorByName("Harvey");
							expr_367F.motion = new Vector2(0f, -2f);
							location.getTemporarySpriteByID(3).scaleChange = -0.01f;
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
							return;
						}
						Game1.globalFadeToClear(null, 0.02f);
					}
				}
				else if (split[0].Equals("grabObject"))
				{
					Game1.player.grabObject(new Object(Vector2.Zero, Convert.ToInt32(split[1]), null, false, true, false, false));
					this.showActiveObject = true;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("addTool"))
				{
					if (split[1].Equals("Sword"))
					{
						if (!Game1.player.addItemToInventoryBool(new Sword("Battered Sword", 67, "A rusty old sword."), false))
						{
							Game1.player.addItemToInventoryBool(new Sword("Battered Sword", 67, "A rusty old sword."), false);
							Game1.drawObjectDialogue(Game1.parseText("You got the Battered Sword! It was sent home to your toolbox."));
						}
						else
						{
							for (int i9 = 0; i9 < Game1.player.Items.Count; i9++)
							{
								if (Game1.player.Items[i9] != null && Game1.player.Items[i9] is Tool && Game1.player.Items[i9].Name.Contains("Sword"))
								{
									Game1.player.CurrentToolIndex = i9;
									Game1.switchToolAnimation();
									break;
								}
							}
						}
					}
					else if (split[1].Equals("Wand") && !Game1.player.addItemToInventoryBool(new Wand(), false))
					{
						Game1.player.addItemToInventoryBool(new Wand(), false);
						Game1.drawObjectDialogue(Game1.parseText("You got the 'Wand'! It seems like it could be a powerful item, but it's missing something... some kind of energy. It was sent home to your toolbox."));
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("waitForKey"))
				{
					string whichKey = split[1];
					KeyboardState currentState = Keyboard.GetState();
					bool keyWasPressed = false;
					if (!Game1.player.UsingTool && !Game1.pickingTool)
					{
						Keys[] pressedKeys = currentState.GetPressedKeys();
						int num = 0;
						while (num < pressedKeys.Length)
						{
							Keys k2 = pressedKeys[num];
							if (Enum.GetName(k2.GetType(), k2).Equals(whichKey.ToUpper()))
							{
								keyWasPressed = true;
								if (k2 == Keys.C)
								{
									Game1.pressUseToolButton();
									Game1.releaseUseToolButton();
									break;
								}
								if (k2 == Keys.S)
								{
									Game1.pressAddItemToInventoryButton();
									this.showActiveObject = false;
									Game1.player.showNotCarrying();
									break;
								}
								if (k2 == Keys.Z)
								{
									Game1.pressSwitchToolButton();
									break;
								}
								break;
							}
							else
							{
								num++;
							}
						}
					}
					int firstQuoteIndex4 = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
					int lastQuoteIndex4 = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
					this.messageToScreen = this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex4, lastQuoteIndex4);
					if (keyWasPressed)
					{
						this.messageToScreen = null;
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
					}
				}
				else if (split[0].Equals("cave"))
				{
					if (Game1.activeClickableMenu == null)
					{
						Response[] responses = new Response[]
						{
							new Response("Mushrooms", "Mushrooms"),
							new Response("Bats", "Bats")
						};
						Game1.currentLocation.createQuestionDialogue("Which one would you prefer? ", responses, "cave");
						Game1.dialogueTyping = false;
					}
				}
				else if (split[0].Equals("updateMinigame"))
				{
					if (Game1.currentMinigame != null)
					{
						Game1.currentMinigame.receiveEventPoke(Convert.ToInt32(split[1]));
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("startJittering"))
				{
					Game1.player.jitterStrength = 1f;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("stopJittering"))
				{
					Game1.player.stopJittering();
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("addLantern"))
				{
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Convert.ToInt32(split[1]), 999999f, 1, 0, new Vector2((float)Convert.ToInt32(split[2]), (float)Convert.ToInt32(split[3])) * (float)Game1.tileSize, false, false)
					{
						light = true,
						lightRadius = (float)Convert.ToInt32(split[4])
					});
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("rustyKey"))
				{
					Game1.player.hasRustyKey = true;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("swimming"))
				{
					if (split[1].Equals("farmer"))
					{
						Game1.player.bathingClothes = true;
						Game1.player.swimming = true;
					}
					else
					{
						this.getActorByName(split[1]).swimming = true;
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("stopSwimming"))
				{
					if (split[1].Equals("farmer"))
					{
						Game1.player.bathingClothes = (location is BathHousePool);
						Game1.player.swimming = false;
					}
					else
					{
						this.getActorByName(split[1]).swimming = false;
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("tutorialMenu"))
				{
					if (Game1.activeClickableMenu == null)
					{
						Game1.activeClickableMenu = new TutorialMenu();
					}
				}
				else if (split[0].Equals("animalNaming"))
				{
					if (Game1.activeClickableMenu == null)
					{
						Game1.activeClickableMenu = new NamingMenu(new NamingMenu.doneNamingBehavior((Game1.currentLocation as AnimalHouse).addNewHatchedAnimal), "Choose a name", null);
					}
				}
				else if (split[0].Equals("splitSpeak"))
				{
					if (!Game1.dialogueUp)
					{
						this.timeAccumulator += (float)time.ElapsedGameTime.Milliseconds;
						if (this.timeAccumulator < 500f)
						{
							return;
						}
						this.timeAccumulator = 0f;
						int firstQuoteIndex5 = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
						int lastQuoteIndex5 = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
						string[] speakSplit = this.eventCommands[this.CurrentCommand].Substring(firstQuoteIndex5, lastQuoteIndex5).Split(new char[]
						{
							'~'
						});
						NPC n9 = Game1.getCharacterFromName(split[1].Equals("rival") ? Utility.getOtherFarmerNames()[0] : split[1], false);
						if (n9 == null)
						{
							n9 = this.getActorByName(split[1]);
						}
						if (n9 == null || this.previousAnswerChoice < 0 || this.previousAnswerChoice >= speakSplit.Length)
						{
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
							return;
						}
						n9.CurrentDialogue.Push(new Dialogue(speakSplit[this.previousAnswerChoice], n9));
						Game1.drawDialogue(n9);
					}
				}
				else if (split[0].Equals("catQuestion"))
				{
					if (!Game1.isQuestion && Game1.activeClickableMenu == null)
					{
						Game1.currentLocation.createQuestionDialogue("Will you adopt this " + (Game1.player.catPerson ? "cat" : "dog") + "?", Game1.currentLocation.createYesNoResponses(), "pet");
					}
				}
				else if (split[0].Equals("taxvote"))
				{
					if (!Game1.isQuestion)
					{
						Response[] responses2 = new Response[]
						{
							new Response("For", "For"),
							new Response("Against", "Against")
						};
						Game1.currentLocation.createQuestionDialogue("Do you vote for or against the 3% shipping tax?", responses2, "taxvote");
						Game1.dialogueTyping = false;
						Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
					}
				}
				else if (split[0].Equals("ambientLight"))
				{
					Game1.ambientLight = new Color(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("bloom"))
				{
					Game1.bloom.Settings = new BloomSettings("eventBloom", (float)Convert.ToDouble(split[1]) / 10f, (float)Convert.ToDouble(split[2]) / 10f, (float)Convert.ToDouble(split[3]) / 10f, (float)Convert.ToDouble(split[4]) / 10f, (float)Convert.ToDouble(split[5]) / 10f, (float)Convert.ToDouble(split[6]) / 10f, split.Length > 7);
					Game1.bloom.reload();
					Game1.bloomDay = true;
					Game1.bloom.Visible = true;
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("elliottbooktalk"))
				{
					if (!Game1.dialogueUp)
					{
						string speech;
						if (Game1.player.dialogueQuestionsAnswered.Contains(958699))
						{
							speech = "I finished writing my book, and it ended up being a mystery novel! Remember when you told me you were into mystery?$h#$b#Behold, '%book', a thrilling tale of deception and intrigue!$u#$b#I -almost- feel like I should split the profits with you, considering how much help you gave me!$h";
						}
						else if (Game1.player.dialogueQuestionsAnswered.Contains(958700))
						{
							speech = "I finished writing my book, and it ended up being a romance novel! ...You told me once that you enjoyed the genre, and, well, I've also been able to draw some inpsiration from real life experiences lately. $l#$b#Here it is, '%book'. It's about a train stewardess who falls in love with a traveling architect... but he's secretly engaged to the ticket collector's daughter!#$b#I *almost* feel like I should split the profits with you, considering how much help you gave me!$h";
						}
						else if (Game1.player.dialogueQuestionsAnswered.Contains(9586701))
						{
							speech = "I finished writing my book, and it's a sci-fi novel! Remember when you told me you were a fan of sci-fi?$h#$b#Behold, '%book'. It's about a boy who forms a symbiotic partnership with a semi-telepathic lifeform. Together they unravel the dark mysteries of an interstellar cabal.$u#$b#I -almost- feel like I should split the profits with you, considering how much help you gave me!$h";
						}
						else
						{
							speech = "I finished writing my book! $h#$b#Well, here it is... '%book'. It's about a government land surveyor who visits a tiny, isolated mining community... and discovers some disturbing secrets.$u#$b#I -almost- feel like I should split the profits with you, considering how much help you gave me!$h";
						}
						NPC n10 = Game1.getCharacterFromName("Elliott", false);
						n10.CurrentDialogue.Push(new Dialogue(speech, n10));
						Game1.drawDialogue(n10);
					}
				}
				else if (split[0].Equals("removeItem"))
				{
					Game1.player.removeFirstOfThisItemFromInventory(Convert.ToInt32(split[1]));
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("friendship"))
				{
					Game1.player.friendships[split[1]][0] += Convert.ToInt32(split[2]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
			}
			IL_43C1:
			if (split[0].Equals("setRunning"))
			{
				Game1.player.setRunning(true, false);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			if (split[0].Equals("extendSourceRect"))
			{
				if (split[2].Equals("reset"))
				{
					this.getActorByName(split[1]).reloadSprite();
					this.getActorByName(split[1]).sprite.spriteWidth = 16;
					this.getActorByName(split[1]).sprite.spriteHeight = 32;
				}
				else
				{
					this.getActorByName(split[1]).extendSourceRect(Convert.ToInt32(split[2]), Convert.ToInt32(split[3]), split.Length <= 4);
				}
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			if (split[0].Equals("waitForOtherPlayers"))
			{
				if (Game1.IsMultiplayer)
				{
					if (Game1.IsServer)
					{
						Game1.player.readyConfirmation = true;
					}
					this.readyConfirmationTimer -= time.ElapsedGameTime.Milliseconds;
					if (this.readyConfirmationTimer <= 0 && Game1.IsClient)
					{
						MultiplayerUtility.sendReadyConfirmation(Game1.player.uniqueMultiplayerID);
						this.sentReadyConfirmation = true;
						this.readyConfirmationTimer = 2000;
					}
					if (this.allPlayersReady)
					{
						this.readyConfirmationTimer = -1;
						this.allPlayersReady = false;
						this.sentReadyConfirmation = false;
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
					}
				}
				else
				{
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
			}
			if (split[0].Equals("advancedMove"))
			{
				this.setUpAdvancedMove(split, null);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			if (split[0].Equals("stopRunning"))
			{
				Game1.player.setRunning(false, false);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			if (split[0].Equals("eyes"))
			{
				Game1.player.currentEyes = Convert.ToInt32(split[1]);
				Game1.player.blinkTimer = Convert.ToInt32(split[2]);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			if (split[0].Equals("addMailReceived"))
			{
				Game1.player.mailReceived.Add(split[1]);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			if (split[0].Equals("fade"))
			{
				Game1.fadeToBlack = true;
				Game1.fadeIn = true;
				if (Game1.fadeToBlackAlpha >= 0.97f)
				{
					if (split.Length == 1)
					{
						Game1.fadeIn = false;
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
			}
			else if (split[0].Equals("changeMapTile"))
			{
				string whichLayer = split[1];
				int tileX2 = Convert.ToInt32(split[2]);
				int tileY2 = Convert.ToInt32(split[3]);
				int newTileIndex = Convert.ToInt32(split[4]);
				location.map.GetLayer(whichLayer).Tiles[tileX2, tileY2].TileIndex = newTileIndex;
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			else if (split[0].Equals("changeSprite"))
			{
				this.getActorByName(split[1]).Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + split[1] + "_" + split[2]);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			else
			{
				if (split[0].Equals("proceedPosition"))
				{
					this.continueAfterMove = true;
					try
					{
						if (!this.getCharacterByName(split[1]).isMoving() || (this.npcControllers != null && this.npcControllers.Count == 0))
						{
							this.getCharacterByName(split[1]).Halt();
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
						goto IL_483E;
					}
					catch (Exception)
					{
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
						goto IL_483E;
					}
				}
				if (split[0].Equals("changePortrait"))
				{
					NPC n11 = Game1.getCharacterFromName(split[1], false);
					n11.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + split[1] + "_" + split[2]);
					n11.uniquePortraitActive = true;
					this.npcsWithUniquePortraits.Add(n11);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
				else if (split[0].Equals("changeYSourceRectOffset"))
				{
					NPC n12 = this.getActorByName(split[1]);
					if (n12 != null)
					{
						n12.ySourceRectOffset = Convert.ToInt32(split[2]);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
				}
			}
			IL_483E:
			if (split[0].Equals("addTemporaryActor"))
			{
				if (this.temporaryContent == null)
				{
					this.temporaryContent = Game1.content.CreateTemporary();
				}
				string textureLocation = "Characters\\";
				if (split.Length > 8 && split[8].Equals("Animal"))
				{
					textureLocation = "Animals\\";
				}
				if (split.Length > 8 && split[8].Equals("Monster"))
				{
					textureLocation = "Characters\\Monsters\\";
				}
				NPC n13 = new NPC(new AnimatedSprite(this.temporaryContent.Load<Texture2D>(textureLocation + split[1].Replace('_', ' ')), 0, Convert.ToInt32(split[2]), Convert.ToInt32(split[3])), new Vector2((float)Convert.ToInt32(split[4]), (float)Convert.ToInt32(split[5])) * (float)Game1.tileSize, Convert.ToInt32(split[6]), split[1].Replace('_', ' '), this.temporaryContent);
				if (split.Length > 7)
				{
					n13.breather = Convert.ToBoolean(split[7]);
				}
				if (this.isFestival)
				{
					try
					{
						n13.CurrentDialogue.Push(new Dialogue(this.festivalData[n13.name], n13));
					}
					catch (Exception)
					{
					}
				}
				if (textureLocation.Contains("Animals") && split.Length > 9)
				{
					n13.name = split[9];
				}
				this.actors.Add(n13);
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
				return;
			}
			if (split[0].Equals("changeToTemporaryMap"))
			{
				if (this.temporaryContent == null)
				{
					this.temporaryContent = Game1.content.CreateTemporary();
				}
				this.temporaryLocation = new GameLocation(this.temporaryContent.Load<Map>("Maps\\" + split[1]), "Temp");
				this.temporaryLocation.map.LoadTileSheets(Game1.mapDisplayDevice);
				Event e2 = Game1.currentLocation.currentEvent;
				Game1.currentLocation.cleanupBeforePlayerExit();
				Game1.currentLightSources.Clear();
				Game1.currentLocation = this.temporaryLocation;
				Game1.currentLocation.resetForPlayerEntry();
				Game1.currentLocation.currentEvent = e2;
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
				Game1.player.currentLocation = Game1.currentLocation;
				if (this.isFestival)
				{
					foreach (Farmer f8 in Game1.otherFarmers.Values)
					{
						f8.currentLocation = Game1.currentLocation;
						Game1.currentLocation.farmers.Add(f8);
					}
				}
				if (split.Length < 3)
				{
					Game1.panScreen(0, 0);
					return;
				}
			}
			else if (split[0].Equals("positionOffset"))
			{
				if (split[1].Contains("farmer"))
				{
					Farmer f9 = Utility.getFarmerFromFarmerNumberString(split[1]);
					if (f9 != null)
					{
						Farmer expr_4B14_cp_0_cp_0 = f9;
						expr_4B14_cp_0_cp_0.position.X = expr_4B14_cp_0_cp_0.position.X + (float)Convert.ToInt32(split[2]);
						Farmer expr_4B2D_cp_0_cp_0 = f9;
						expr_4B2D_cp_0_cp_0.position.Y = expr_4B2D_cp_0_cp_0.position.Y + (float)Convert.ToInt32(split[3]);
					}
				}
				else
				{
					NPC n14 = this.getActorByName(split[1]);
					if (n14 != null)
					{
						NPC expr_4B57_cp_0_cp_0 = n14;
						expr_4B57_cp_0_cp_0.position.X = expr_4B57_cp_0_cp_0.position.X + (float)Convert.ToInt32(split[2]);
						NPC expr_4B70_cp_0_cp_0 = n14;
						expr_4B70_cp_0_cp_0.position.Y = expr_4B70_cp_0_cp_0.position.Y + (float)Convert.ToInt32(split[3]);
					}
				}
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
				if (split.Length > 4)
				{
					this.checkForNextCommand(location, time);
					return;
				}
			}
			else if (split[0].Equals("question"))
			{
				if (!Game1.isQuestion && Game1.activeClickableMenu == null)
				{
					string[] questionAndAnswersSplit = this.eventCommands[Math.Min(this.eventCommands.Length - 1, this.CurrentCommand)].Split(new char[]
					{
						'"'
					})[1].Split(new char[]
					{
						'#'
					});
					string question = questionAndAnswersSplit[0];
					Response[] answers = new Response[questionAndAnswersSplit.Length - 1];
					for (int i10 = 1; i10 < questionAndAnswersSplit.Length; i10++)
					{
						Response[] arg_4C3C_0 = answers;
						int arg_4C3C_1 = i10 - 1;
						int num = i10 - 1;
						arg_4C3C_0[arg_4C3C_1] = new Response(num.ToString(), questionAndAnswersSplit[i10]);
					}
					Game1.currentLocation.createQuestionDialogue(question, answers, split[1]);
					return;
				}
			}
			else
			{
				if (split[0].Equals("jump"))
				{
					float jumpV = (split.Length > 2) ? ((float)Convert.ToDouble(split[2])) : 8f;
					if (split[1].Equals("farmer"))
					{
						Game1.player.jump(jumpV);
					}
					else
					{
						this.getActorByName(split[1]).jump(jumpV);
					}
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					this.checkForNextCommand(location, time);
					return;
				}
				if (split[0].Equals("farmerEat"))
				{
					Game1.playerEatObject(new Object(Convert.ToInt32(split[1]), 1, false, -1, 0), true);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					return;
				}
				if (split[0].Equals("screenFlash"))
				{
					Game1.flashAlpha = (float)Convert.ToDouble(split[1]);
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					return;
				}
				if (split[0].Equals("grandpaCandles"))
				{
					int candles = Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore());
					Game1.getFarm().grandpaScore = candles;
					for (int i11 = 0; i11 < candles; i11++)
					{
						DelayedAction.playSoundAfterDelay("fireball", 100 * i11);
					}
					Game1.getFarm().addGrandpaCandles();
					int num = this.CurrentCommand;
					this.CurrentCommand = num + 1;
					return;
				}
				if (split[0].Equals("grandpaEvaluation2"))
				{
					switch (Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore()))
					{
					case 1:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"The farm hasn't changed much all that much since we last spoke... but that's okay.#$b#If you're enjoying your new life, that's all that matters to me...#$b#I must return to the other world, now... but feel free to call on me again when you're ready.\"";
						break;
					case 2:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"You've done a good job improving the place since we last spoke.#$b#It seems you've tried your best... that's all I can ask for. I'm proud of you.#$b#I must return to the other world, now... but feel free to call on me again when you're ready.\"";
						break;
					case 3:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"You've done well, my boy.^You've done well, my dear.#$b#%farm Farm has never looked better. It's an honor to the family name.#$b#Grandpa is pleased.#$b#I must return to the other world, now... but feel free to call on me again when you're ready.\"";
						break;
					case 4:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"I'm so proud of you, my boy!^I'm so proud of you, my dear!#$b#You've really turned the place around since we last spoke!#$b#You're a better farmer than I ever was, and you've brought great honor to the family name.#$b#I can feel it now... My spirit is finally put to rest. Bless you.\"";
						break;
					}
					Game1.player.eventsSeen.Remove(2146991);
					return;
				}
				if (split[0].Equals("grandpaEvaluation"))
				{
					switch (Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore()))
					{
					case 1:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"The farm hasn't changed much in these past few years... but that's okay.#$b#If you're enjoying your new life, that's all that matters to me...\"";
						return;
					case 2:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"You've done a good job with the place.#$b#It seems you've tried your best... that's all I can ask for. I'm proud of you.\"";
						return;
					case 3:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"You've done well, my boy.^You've done well, my dear.#$b#%farm Farm has never looked better. It's an honor to the family name.#$b#Grandpa is pleased.\"";
						return;
					case 4:
						this.eventCommands[this.currentCommand] = "speak Grandpa \"I'm so proud of you, my boy!^I'm so proud of you, my dear!#$b#You're a better farmer than I ever was, and you've brought great honor to the family name.#$b#I can feel it now... My spirit is finally put to rest. Bless you.\"";
						return;
					default:
						return;
					}
				}
				else
				{
					if (split[0].Equals("loadActors"))
					{
						if (this.temporaryLocation != null && this.temporaryLocation.map.GetLayer(split[1]) != null)
						{
							this.actors.Clear();
							if (this.npcControllers != null)
							{
								this.npcControllers.Clear();
							}
							Dictionary<string, string> NPCData = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
							for (int x2 = 0; x2 < this.temporaryLocation.map.GetLayer(split[1]).LayerWidth; x2++)
							{
								for (int y2 = 0; y2 < this.temporaryLocation.map.GetLayer(split[1]).LayerHeight; y2++)
								{
									if (this.temporaryLocation.map.GetLayer(split[1]).Tiles[x2, y2] != null)
									{
										int actorIndex = this.temporaryLocation.map.GetLayer(split[1]).Tiles[x2, y2].TileIndex / 4;
										int actorFacingDirection = this.temporaryLocation.map.GetLayer(split[1]).Tiles[x2, y2].TileIndex % 4;
										string actorName = NPCData.ElementAt(actorIndex).Key;
										if (actorName != null && Game1.getCharacterFromName(actorName, false) != null)
										{
											this.addActor(actorName, x2, y2, actorFacingDirection, this.temporaryLocation);
										}
									}
								}
							}
						}
						int num = this.CurrentCommand;
						this.CurrentCommand = num + 1;
						return;
					}
					if (split[0].Equals("playerControl"))
					{
						if (!this.playerControlSequence)
						{
							this.setUpPlayerControlSequence(split[1]);
							return;
						}
					}
					else
					{
						if (split[0].Equals("removeSprite"))
						{
							Vector2 tile = new Vector2((float)Convert.ToInt32(split[1]), (float)Convert.ToInt32(split[2])) * (float)Game1.tileSize;
							for (int i12 = Game1.currentLocation.temporarySprites.Count - 1; i12 >= 0; i12--)
							{
								if (Game1.currentLocation.temporarySprites[i12].position.Equals(tile))
								{
									Game1.currentLocation.temporarySprites.RemoveAt(i12);
								}
							}
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
							return;
						}
						if (split[0].Equals("viewport"))
						{
							if (split[1].Equals("move"))
							{
								this.viewportTarget = new Vector3((float)Convert.ToInt32(split[2]), (float)Convert.ToInt32(split[3]), (float)Convert.ToInt32(split[4]));
							}
							else
							{
								if (this.aboveMapSprites != null && Convert.ToInt32(split[1]) < 0)
								{
									this.aboveMapSprites.Clear();
									this.aboveMapSprites = null;
								}
								Game1.viewportFreeze = true;
								Game1.viewport.X = Convert.ToInt32(split[1]) * Game1.tileSize + Game1.tileSize / 2 - Game1.viewport.Width / 2;
								Game1.viewport.Y = Convert.ToInt32(split[2]) * Game1.tileSize + Game1.tileSize / 2 - Game1.viewport.Height / 2;
								if (Game1.viewport.X > 0 && Game1.viewport.Width > Game1.currentLocation.Map.DisplayWidth)
								{
									Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
								}
								if (Game1.viewport.Y > 0 && Game1.viewport.Height > Game1.currentLocation.Map.DisplayHeight)
								{
									Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
								}
								if (split.Length > 3 && split[3].Equals("true"))
								{
									Game1.fadeScreenToBlack();
									Game1.fadeToBlackAlpha = 1f;
									Game1.nonWarpFade = true;
								}
								else if (split.Length > 3 && split[3].Equals("clamp"))
								{
									if (Game1.viewport.X + Game1.viewport.Width > Game1.currentLocation.Map.DisplayWidth)
									{
										Game1.viewport.X = Game1.currentLocation.Map.DisplayWidth - Game1.graphics.GraphicsDevice.Viewport.Width;
									}
									if (Game1.viewport.Y + Game1.viewport.Height > Game1.currentLocation.Map.DisplayHeight)
									{
										Game1.viewport.Y = Game1.currentLocation.Map.DisplayHeight - Game1.graphics.GraphicsDevice.Viewport.Height;
									}
									if (Game1.viewport.X < 0)
									{
										Game1.viewport.X = 0;
									}
									if (Game1.viewport.Y < 0)
									{
										Game1.viewport.Y = 0;
									}
									if (split.Length > 4 && split[4].Equals("true"))
									{
										Game1.fadeScreenToBlack();
										Game1.fadeToBlackAlpha = 1f;
										Game1.nonWarpFade = true;
									}
								}
								if (split.Length > 4 && split[4].Equals("unfreeze"))
								{
									Game1.viewportFreeze = false;
								}
								if (Game1.gameMode == 2)
								{
									Game1.viewport.X = Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width;
								}
							}
							int num = this.CurrentCommand;
							this.CurrentCommand = num + 1;
						}
					}
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00019538 File Offset: 0x00017738
		public bool isTileWalkedOn(int x, int y)
		{
			return this.characterWalkLocations.Contains(new Vector2((float)x, (float)y));
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00019550 File Offset: 0x00017750
		private void populateWalkLocationsList()
		{
			Vector2 pos = Game1.player.getTileLocation();
			this.characterWalkLocations.Add(pos);
			for (int i = 2; i < this.eventCommands.Length; i++)
			{
				string[] split = this.eventCommands[i].Split(new char[]
				{
					' '
				});
				string a = split[0];
				if (a == "move" && split[1].Equals("farmer"))
				{
					for (int x = 0; x < Math.Abs(Convert.ToInt32(split[2])); x++)
					{
						pos.X += (float)Math.Sign(Convert.ToInt32(split[2]));
						this.characterWalkLocations.Add(pos);
					}
					for (int y = 0; y < Math.Abs(Convert.ToInt32(split[3])); y++)
					{
						pos.Y += (float)Math.Sign(Convert.ToInt32(split[3]));
						this.characterWalkLocations.Add(pos);
					}
				}
			}
			foreach (NPC j in this.actors)
			{
				pos = j.getTileLocation();
				this.characterWalkLocations.Add(pos);
				for (int k = 2; k < this.eventCommands.Length; k++)
				{
					string[] split2 = this.eventCommands[k].Split(new char[]
					{
						' '
					});
					string a = split2[0];
					if (a == "move" && split2[1].Equals(j.name))
					{
						for (int x2 = 0; x2 < Math.Abs(Convert.ToInt32(split2[2])); x2++)
						{
							pos.X += (float)Math.Sign(Convert.ToInt32(split2[2]));
							this.characterWalkLocations.Add(pos);
						}
						for (int y2 = 0; y2 < Math.Abs(Convert.ToInt32(split2[3])); y2++)
						{
							pos.Y += (float)Math.Sign(Convert.ToInt32(split2[3]));
							this.characterWalkLocations.Add(pos);
						}
					}
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000197A0 File Offset: 0x000179A0
		public NPC getActorByName(string name)
		{
			if (name.Equals("rival"))
			{
				name = Utility.getOtherFarmerNames()[0];
			}
			if (name.Equals("spouse"))
			{
				name = Game1.player.spouse.Replace("engaged", "");
			}
			foreach (NPC i in this.actors)
			{
				if (i.name.Equals(name))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00019844 File Offset: 0x00017A44
		private void addActor(string name, int x, int y, int facingDirection, GameLocation location)
		{
			Texture2D portrait = null;
			try
			{
				portrait = Game1.content.Load<Texture2D>("Portraits\\" + (name.Equals("WeddingOutfits") ? Game1.player.spouse : name));
			}
			catch (Exception)
			{
			}
			int height = (name.Contains("Dwarf") || name.Equals("Krobus")) ? (Game1.tileSize * 3 / 2) : (Game1.tileSize * 2);
			NPC i = new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + name), 0, Game1.tileSize / 4, height / 4), new Vector2((float)(x * Game1.tileSize), (float)(y * Game1.tileSize)), location.Name, facingDirection, name.Contains("Rival") ? Utility.getOtherFarmerNames()[0] : name, null, portrait, true);
			i.eventActor = true;
			if (this.isFestival)
			{
				try
				{
					i.setNewDialogue(this.festivalData[i.name], false, false);
				}
				catch (Exception)
				{
				}
			}
			this.actors.Add(i);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00019970 File Offset: 0x00017B70
		public Character getCharacterByName(string name)
		{
			if (name.Equals("rival"))
			{
				name = Utility.getOtherFarmerNames()[0];
			}
			if (name.Contains("farmer"))
			{
				return Utility.getFarmerFromFarmerNumberString(name);
			}
			foreach (NPC i in this.actors)
			{
				if (i.name.Equals(name))
				{
					return i;
				}
			}
			return null;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00019A00 File Offset: 0x00017C00
		public Vector3 getPositionAfterMove(Character c, int xMove, int yMove, int facingDirection)
		{
			Vector2 tileLocation = c.getTileLocation();
			return new Vector3(tileLocation.X + (float)xMove, tileLocation.Y + (float)yMove, (float)facingDirection);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00019A30 File Offset: 0x00017C30
		private void setUpCharacters(string description, GameLocation location)
		{
			Game1.player.Halt();
			Game1.player.positionBeforeEvent = Game1.player.getTileLocation();
			Game1.player.orientationBeforeEvent = Game1.player.facingDirection;
			string[] split = description.Split(new char[]
			{
				' '
			});
			int i = 0;
			while (i < split.Length)
			{
				if (split[i + 1].Equals("-1") && !split[i].Equals("farmer"))
				{
					using (List<NPC>.Enumerator enumerator = location.getCharacters().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NPC j = enumerator.Current;
							if (j.name.Equals(split[i]))
							{
								this.actors.Add(j);
							}
						}
						goto IL_445;
					}
					goto IL_B9;
				}
				goto IL_B9;
				IL_445:
				i += 4;
				continue;
				IL_B9:
				if (!split[i].Equals("farmer"))
				{
					string name = split[i];
					if (split[i].Equals("spouse"))
					{
						name = Game1.player.spouse.Replace("engaged", "");
					}
					if (split[i].Equals("rival"))
					{
						name = (Game1.player.isMale ? "maleRival" : "femaleRival");
					}
					if (split[i].Equals("cat"))
					{
						this.actors.Add(new Cat(Convert.ToInt32(split[i + 1]), Convert.ToInt32(split[i + 2])));
						this.actors.Last<NPC>().name = "Cat";
						NPC expr_180_cp_0_cp_0 = this.actors.Last<NPC>();
						expr_180_cp_0_cp_0.position.X = expr_180_cp_0_cp_0.position.X - (float)(Game1.tileSize / 2);
						goto IL_445;
					}
					if (split[i].Equals("dog"))
					{
						this.actors.Add(new Dog(Convert.ToInt32(split[i + 1]), Convert.ToInt32(split[i + 2])));
						this.actors.Last<NPC>().name = "Dog";
						NPC expr_1EE_cp_0_cp_0 = this.actors.Last<NPC>();
						expr_1EE_cp_0_cp_0.position.X = expr_1EE_cp_0_cp_0.position.X - (float)(Game1.tileSize * 2 / 3);
						goto IL_445;
					}
					if (split[i].Equals("Junimo"))
					{
						this.actors.Add(new Junimo(new Vector2((float)(Convert.ToInt32(split[i + 1]) * Game1.tileSize), (float)(Convert.ToInt32(split[i + 2]) * Game1.tileSize - Game1.tileSize / 2)), -1, false)
						{
							name = "Junimo",
							eventActor = true
						});
						goto IL_445;
					}
					int xPos = Convert.ToInt32(split[i + 1]);
					int yPos = Convert.ToInt32(split[i + 2]);
					int facingDir = Convert.ToInt32(split[i + 3]);
					if (location is Farm)
					{
						xPos = Farm.getFrontDoorPositionForFarmer(Game1.player).X;
						yPos = Farm.getFrontDoorPositionForFarmer(Game1.player).Y + 2;
						facingDir = 0;
					}
					this.addActor(name, xPos, yPos, facingDir, location);
					goto IL_445;
				}
				else
				{
					if (split[i + 1].Equals("-1"))
					{
						goto IL_445;
					}
					Game1.player.position.X = (float)(Convert.ToInt32(split[i + 1]) * Game1.tileSize);
					Game1.player.position.Y = (float)(Convert.ToInt32(split[i + 2]) * Game1.tileSize + Game1.tileSize / 4);
					Game1.player.faceDirection(Convert.ToInt32(split[i + 3]));
					if (location is Farm)
					{
						Game1.player.position.X = (float)(Farm.getFrontDoorPositionForFarmer(Game1.player).X * Game1.tileSize);
						Game1.player.position.Y = (float)((Farm.getFrontDoorPositionForFarmer(Game1.player).Y + 1) * Game1.tileSize);
						Game1.player.faceDirection(2);
					}
					Game1.player.FarmerSprite.StopAnimation();
					if (this.isFestival)
					{
						foreach (Farmer expr_3D2 in Game1.otherFarmers.Values)
						{
							expr_3D2.position.X = (float)(Convert.ToInt32(split[i + 1]) * Game1.tileSize);
							expr_3D2.position.Y = (float)(Convert.ToInt32(split[i + 2]) * Game1.tileSize + Game1.tileSize / 4);
							expr_3D2.faceDirection(Convert.ToInt32(split[i + 3]));
							expr_3D2.FarmerSprite.StopAnimation();
						}
						goto IL_445;
					}
					goto IL_445;
				}
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00019EAC File Offset: 0x000180AC
		private void beakerSmashEndFunction(int extraInfo)
		{
			Game1.playSound("breakingGlass");
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2(9f, 16f) * (float)Game1.tileSize, Color.LightBlue, 10, false, 100f, 0, -1, -1f, -1, 0));
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(400, 3008, Game1.tileSize, Game1.tileSize), 99999f, 2, 0, new Vector2(9f, 16f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.LightBlue, 1f, 0f, 0f, 0f, false)
			{
				delayBeforeAnimationStart = 700
			});
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2(9f, 16f) * (float)Game1.tileSize, Color.White * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
			{
				motion = new Vector2(0f, -1f)
			});
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00019FF4 File Offset: 0x000181F4
		private void eggSmashEndFunction(int extraInfo)
		{
			Game1.playSound("slimedead");
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2(9f, 16f) * (float)Game1.tileSize, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0));
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(177, 99999f, 9999, 0, new Vector2(6f, 5f) * (float)Game1.tileSize, false, false)
			{
				layerDepth = 1E-06f
			});
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0001A09C File Offset: 0x0001829C
		private void balloonInSky(int extraInfo)
		{
			TemporaryAnimatedSprite t = Game1.currentLocation.getTemporarySpriteByID(2);
			if (t != null)
			{
				t.motion = Vector2.Zero;
			}
			t = Game1.currentLocation.getTemporarySpriteByID(1);
			if (t != null)
			{
				t.motion = Vector2.Zero;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0001A0E0 File Offset: 0x000182E0
		private void marcelloBalloonLand(int extraInfo)
		{
			Game1.playSound("thudStep");
			Game1.playSound("dirtyHit");
			TemporaryAnimatedSprite t = Game1.currentLocation.getTemporarySpriteByID(2);
			if (t != null)
			{
				t.motion = Vector2.Zero;
			}
			t = Game1.currentLocation.getTemporarySpriteByID(3);
			if (t != null)
			{
				t.scaleChange = 0f;
			}
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 2944, 64, 64), 120f, 8, 1, new Vector2(25f, 39f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 2), (float)(Game1.pixelZoom * 8)), false, true, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false));
			Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 2944, 64, 64), 120f, 8, 1, new Vector2(27f, 39f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.pixelZoom * 12)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
			{
				delayBeforeAnimationStart = 300
			});
			int num = this.CurrentCommand;
			this.CurrentCommand = num + 1;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0001A264 File Offset: 0x00018464
		private void samPreOllie(int extraInfo)
		{
			this.getActorByName("Sam").sprite.CurrentFrame = 27;
			Game1.player.faceDirection(0);
			TemporaryAnimatedSprite expr_2D = Game1.currentLocation.getTemporarySpriteByID(1);
			expr_2D.xStopCoordinate = 22 * Game1.tileSize;
			expr_2D.reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.samOllie);
			expr_2D.motion = new Vector2(2f, 0f);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0001A2D4 File Offset: 0x000184D4
		private void samOllie(int extraInfo)
		{
			Game1.playSound("crafting");
			this.getActorByName("Sam").sprite.CurrentFrame = 26;
			TemporaryAnimatedSprite expr_2C = Game1.currentLocation.getTemporarySpriteByID(1);
			expr_2C.currentNumberOfLoops = 0;
			expr_2C.totalNumberOfLoops = 1;
			expr_2C.motion.Y = -9f;
			expr_2C.motion.X = 2f;
			expr_2C.acceleration = new Vector2(0f, 0.4f);
			expr_2C.animationLength = 1;
			expr_2C.interval = 530f;
			expr_2C.timer = 0f;
			expr_2C.endFunction = new TemporaryAnimatedSprite.endBehavior(this.samGrind);
			expr_2C.destroyable = false;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0001A388 File Offset: 0x00018588
		private void samGrind(int extraInfo)
		{
			Game1.playSound("hammer");
			this.getActorByName("Sam").sprite.CurrentFrame = 28;
			TemporaryAnimatedSprite expr_2C = Game1.currentLocation.getTemporarySpriteByID(1);
			expr_2C.currentNumberOfLoops = 0;
			expr_2C.totalNumberOfLoops = 9999;
			expr_2C.motion.Y = 0f;
			expr_2C.motion.X = 2f;
			expr_2C.acceleration = new Vector2(0f, 0f);
			expr_2C.animationLength = 1;
			expr_2C.interval = 99999f;
			expr_2C.timer = 0f;
			expr_2C.xStopCoordinate = 26 * Game1.tileSize;
			expr_2C.yStopCoordinate = -1;
			expr_2C.reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.samDropOff);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0001A44C File Offset: 0x0001864C
		private void samDropOff(int extraInfo)
		{
			NPC expr_0B = this.getActorByName("Sam");
			expr_0B.sprite.CurrentFrame = 31;
			TemporaryAnimatedSprite expr_23 = Game1.currentLocation.getTemporarySpriteByID(1);
			expr_23.currentNumberOfLoops = 9999;
			expr_23.totalNumberOfLoops = 0;
			expr_23.motion.Y = 0f;
			expr_23.motion.X = 2f;
			expr_23.acceleration = new Vector2(0f, 0.4f);
			expr_23.animationLength = 1;
			expr_23.interval = 99999f;
			expr_23.yStopCoordinate = 90 * Game1.tileSize;
			expr_23.reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.samGround);
			expr_23.endFunction = null;
			expr_0B.Sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
			{
				new FarmerSprite.AnimationFrame(29, 100),
				new FarmerSprite.AnimationFrame(30, 100),
				new FarmerSprite.AnimationFrame(31, 100),
				new FarmerSprite.AnimationFrame(32, 100)
			});
			expr_0B.Sprite.loop = false;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0001A554 File Offset: 0x00018754
		private void samGround(int extraInfo)
		{
			TemporaryAnimatedSprite arg_15_0 = Game1.currentLocation.getTemporarySpriteByID(1);
			Game1.playSound("thudStep");
			arg_15_0.attachedCharacter = null;
			arg_15_0.reachedStopCoordinate = null;
			arg_15_0.totalNumberOfLoops = -1;
			arg_15_0.interval = 0f;
			arg_15_0.destroyable = true;
			int num = this.CurrentCommand;
			this.CurrentCommand = num + 1;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0001A5AC File Offset: 0x000187AC
		private void catchFootball(int extraInfo)
		{
			TemporaryAnimatedSprite arg_15_0 = Game1.currentLocation.getTemporarySpriteByID(1);
			Game1.playSound("fishSlap");
			arg_15_0.motion = new Vector2(2f, -8f);
			arg_15_0.rotationChange = 0.1308997f;
			arg_15_0.reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.footballLand);
			arg_15_0.yStopCoordinate = 17 * Game1.tileSize;
			Game1.player.jump();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0001A618 File Offset: 0x00018818
		private void footballLand(int extraInfo)
		{
			TemporaryAnimatedSprite arg_15_0 = Game1.currentLocation.getTemporarySpriteByID(1);
			Game1.playSound("sandyStep");
			arg_15_0.motion = new Vector2(0f, 0f);
			arg_15_0.rotationChange = 0f;
			arg_15_0.reachedStopCoordinate = null;
			arg_15_0.animationLength = 1;
			arg_15_0.interval = 999999f;
			int num = this.CurrentCommand;
			this.CurrentCommand = num + 1;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0001A684 File Offset: 0x00018884
		private void parrotSplat(int extraInfo)
		{
			Game1.soundBank.PlayCue("drumkit0");
			DelayedAction.playSoundAfterDelay("drumkit5", 100);
			Game1.soundBank.PlayCue("slimeHit");
			using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.aboveMapSprites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.alpha = 0f;
				}
			}
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float)(24 * Game1.tileSize - Game1.tileSize / 2), (float)(87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float)Game1.pixelZoom, 0f, 1.57079637f, 0.0490873866f, false)
			{
				motion = new Vector2(2f, -2f),
				acceleration = new Vector2(0f, 0.1f)
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float)(24 * Game1.tileSize - Game1.tileSize / 2), (float)(87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float)Game1.pixelZoom, 0f, 0.7853982f, 0.0490873866f, false)
			{
				motion = new Vector2(-2f, -1f),
				acceleration = new Vector2(0f, 0.1f)
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float)(24 * Game1.tileSize - Game1.tileSize / 2), (float)(87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float)Game1.pixelZoom, 0f, 3.14159274f, 0.0490873866f, false)
			{
				motion = new Vector2(1f, 1f),
				acceleration = new Vector2(0f, 0.1f)
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float)(24 * Game1.tileSize - Game1.tileSize / 2), (float)(87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0.0490873866f, false)
			{
				motion = new Vector2(-2f, -2f),
				acceleration = new Vector2(0f, 0.1f)
			});
			Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(148, 165, 25, 23), 99999f, 1, 99999, new Vector2((float)(24 * Game1.tileSize - Game1.tileSize / 2), (float)(87 * Game1.tileSize)), false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
			{
				id = 666f
			});
			int num = this.CurrentCommand;
			this.CurrentCommand = num + 1;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0001AA34 File Offset: 0x00018C34
		private void addSpecificTemporarySprite(string key, GameLocation location, string[] split)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
			if (num <= 2358341695u)
			{
				if (num <= 1042337593u)
				{
					if (num <= 390240131u)
					{
						if (num <= 246031843u)
						{
							if (num <= 84876044u)
							{
								if (num != 37764568u)
								{
									if (num != 84876044u)
									{
										return;
									}
									if (!(key == "pennyFieldTrip"))
									{
										return;
									}
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1813, 86, 54), 999999f, 1, 0, new Vector2(68f, 44f) * (float)Game1.tileSize, false, false, 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
									return;
								}
								else
								{
									if (!(key == "JoshMom"))
									{
										return;
									}
									TemporaryAnimatedSprite parent = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(416, 1931, 58, 65), 750f, 2, 99999, new Vector2((float)(Game1.viewport.Width / 2), (float)Game1.viewport.Height), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										alpha = 0.6f,
										local = true,
										xPeriodic = true,
										xPeriodicLoopTime = 2000f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										motion = new Vector2(0f, -1.25f),
										initialPosition = new Vector2((float)(Game1.viewport.Width / 2), (float)Game1.viewport.Height)
									};
									location.temporarySprites.Add(parent);
									for (int i = 0; i < 19; i++)
									{
										location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(516, 1916, 7, 10), 99999f, 1, 99999, new Vector2((float)Game1.tileSize, (float)(Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
										{
											alphaFade = 0.01f,
											local = true,
											motion = new Vector2(-1f, -1f),
											parentSprite = parent,
											delayBeforeAnimationStart = (i + 1) * 1000
										});
									}
									return;
								}
							}
							else if (num != 172887865u)
							{
								if (num != 199811880u)
								{
									if (num != 246031843u)
									{
										return;
									}
									if (!(key == "heart"))
									{
										return;
									}
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2((float)Convert.ToInt32(split[2]), (float)Convert.ToInt32(split[3])) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), (float)(-(float)Game1.pixelZoom * 4)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										motion = new Vector2(0f, -0.5f),
										alphaFade = 0.01f
									});
									return;
								}
								else
								{
									if (!(key == "shaneCliffProps"))
									{
										return;
									}
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(549, 1891, 19, 12), 99999f, 1, 99999, new Vector2(104f, 96f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										id = 999f
									});
									return;
								}
							}
							else
							{
								if (!(key == "dropEgg"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(176, 800f, 1, 0, new Vector2(6f, 4f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 2)), false, false)
								{
									rotationChange = 0.1308997f,
									motion = new Vector2(0f, -7f),
									acceleration = new Vector2(0f, 0.3f),
									endFunction = new TemporaryAnimatedSprite.endBehavior(this.eggSmashEndFunction),
									layerDepth = 1f
								});
								return;
							}
						}
						else if (num <= 299483063u)
						{
							if (num != 262838603u)
							{
								if (num != 264360623u)
								{
									if (num != 299483063u)
									{
										return;
									}
									if (!(key == "wed"))
									{
										return;
									}
									this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
									Game1.flashAlpha = 1f;
									for (int j = 0; j < 150; j++)
									{
										Vector2 position = new Vector2((float)Game1.random.Next(Game1.viewport.Width - Game1.tileSize * 2), (float)Game1.random.Next(Game1.viewport.Height));
										int scale = Game1.random.Next(2, 5);
										this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(424, 1266, 8, 8), 60f + (float)Game1.random.Next(-10, 10), 7, 999999, position, false, false, 0.99f, 0f, Color.White, (float)scale, 0f, 0f, 0f, false)
										{
											local = true,
											motion = new Vector2(0.1625f, -0.25f) * (float)scale
										});
									}
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(558, 1425, 20, 26), 400f, 3, 99999, new Vector2(26f, 64f) * (float)Game1.tileSize, false, false, (float)(65 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										pingPong = true
									});
									Game1.changeMusicTrack("wedding");
									Game1.musicPlayerVolume = 0f;
									return;
								}
								else
								{
									if (!(key == "haleyRoomDark"))
									{
										return;
									}
									Game1.currentLightSources.Clear();
									Game1.ambientLight = new Color(200, 200, 100);
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(743, 999999f, 1, 0, new Vector2(4f, 1f) * (float)Game1.tileSize, false, false)
									{
										light = true,
										lightcolor = new Color(0, 255, 255),
										lightRadius = 2f
									});
									return;
								}
							}
							else
							{
								if (!(key == "EmilyBoomBox"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(586, 1871, 24, 14), 99999f, 1, 99999, new Vector2(15f, 4f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									id = 999f
								});
								return;
							}
						}
						else if (num != 321043561u)
						{
							if (num != 354301824u)
							{
								if (num != 390240131u)
								{
									return;
								}
								if (!(key == "shaneCliffs"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(533, 1864, 19, 27), 99999f, 1, 99999, new Vector2(83f, 98f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									id = 999f
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(552, 1862, 31, 21), 99999f, 1, 99999, new Vector2(83f, 98f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), false, false, 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(549, 1891, 19, 12), 99999f, 1, 99999, new Vector2(84f, 99f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									id = 999f
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(549, 1891, 19, 12), 99999f, 1, 99999, new Vector2(82f, 98f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									id = 999f
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(542, 1893, 4, 6), 99999f, 1, 99999, new Vector2(83f, 99f) * (float)Game1.tileSize + new Vector2(-8f, 4f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								return;
							}
							else
							{
								if (!(key == "sebastianGarage"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1843, 48, 42), 9999f, 1, 999, new Vector2(17f, 23f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.pixelZoom * 2)), false, false, (float)(23 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								this.getActorByName("Sebastian").hideShadow = true;
								return;
							}
						}
						else
						{
							if (!(key == "sebastianOnBike"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 1600, 64, 128), 80f, 8, 9999, new Vector2(19f, 27f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.pixelZoom * 4)), false, true, (float)(28 * Game1.tileSize) / 10000f, 0f, Color.White, 1f, 0f, 0f, 0f, false));
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(405, 1854, 47, 33), 9999f, 1, 999, new Vector2(17f, 27f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.pixelZoom * 2)), false, false, (float)(28 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							return;
						}
					}
					else if (num <= 750296570u)
					{
						if (num <= 428895204u)
						{
							if (num != 416176087u)
							{
								if (num != 428895204u)
								{
									return;
								}
								if (!(key == "balloonBirds"))
								{
									return;
								}
								int positionOffset = 0;
								if (split != null && split.Length > 2)
								{
									positionOffset = Convert.ToInt32(split[2]);
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float)(positionOffset + 12)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1500
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float)(positionOffset + 13)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1250
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float)(positionOffset + 14)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1100
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(45f, (float)(positionOffset + 15)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1000
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float)(positionOffset + 16)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1080
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float)(positionOffset + 17)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1300
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float)(positionOffset + 18)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-3f, 0f),
									delayBeforeAnimationStart = 1450
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float)(positionOffset + 15)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-4f, 0f),
									delayBeforeAnimationStart = 5450
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float)(positionOffset + 10)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f),
									delayBeforeAnimationStart = 500
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float)(positionOffset + 11)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f),
									delayBeforeAnimationStart = 250
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float)(positionOffset + 12)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f),
									delayBeforeAnimationStart = 100
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(45f, (float)(positionOffset + 13)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f)
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float)(positionOffset + 14)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f),
									delayBeforeAnimationStart = 80
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float)(positionOffset + 15)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f),
									delayBeforeAnimationStart = 300
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float)(positionOffset + 16)) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
								{
									motion = new Vector2(-2f, 0f),
									delayBeforeAnimationStart = 450
								});
								return;
							}
							else
							{
								if (!(key == "abbyOuija"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 960, Game1.tileSize * 2, Game1.tileSize * 2), 60f, 4, 0, new Vector2(6f, 9f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false));
								return;
							}
						}
						else if (num != 477416675u)
						{
							if (num != 655907427u)
							{
								if (num != 750296570u)
								{
									return;
								}
								if (!(key == "dickBag"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(528, 1435, 16, 16), 99999f, 1, 99999, new Vector2(48f, 7f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								return;
							}
							else
							{
								if (!(key == "jojaCeremony"))
								{
									return;
								}
								this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
								for (int k = 0; k < 16; k++)
								{
									Vector2 position2 = new Vector2((float)Game1.random.Next(Game1.viewport.Width - Game1.tileSize * 2), (float)(Game1.viewport.Height + k * Game1.tileSize));
									this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(534, 1413, 11, 16), 99999f, 1, 99999, position2, false, false, 0.99f, 0f, Color.DeepSkyBlue, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										local = true,
										motion = new Vector2(0.25f, -1.5f),
										acceleration = new Vector2(0f, -0.001f)
									});
									this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(545, 1413, 11, 34), 99999f, 1, 99999, position2 + new Vector2(0f, 0f), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										local = true,
										motion = new Vector2(0.25f, -1.5f),
										acceleration = new Vector2(0f, -0.001f)
									});
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1363, 114, 58), 99999f, 1, 99999, new Vector2(50f, 20f) * (float)Game1.tileSize, false, false, (float)(23 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 200f, 3, 99999, new Vector2(48f, 20f) * (float)Game1.tileSize, false, false, (float)(23 * Game1.tileSize) / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									pingPong = true
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 200f, 3, 99999, new Vector2(49f, 20f) * (float)Game1.tileSize, false, false, (float)(23 * Game1.tileSize) / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									pingPong = true
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 210f, 3, 99999, new Vector2(62f, 20f) * (float)Game1.tileSize, false, false, (float)(23 * Game1.tileSize) / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									pingPong = true
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 190f, 3, 99999, new Vector2(60f, 20f) * (float)Game1.tileSize, false, false, (float)(23 * Game1.tileSize) / 10000f + 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									pingPong = true
								});
								return;
							}
						}
						else
						{
							if (!(key == "EmilyBoomBoxStop"))
							{
								return;
							}
							location.getTemporarySpriteByID(999).pulse = false;
							location.getTemporarySpriteByID(999).scale = (float)Game1.pixelZoom;
							return;
						}
					}
					else if (num <= 820334071u)
					{
						if (num != 762742231u)
						{
							if (num != 789298023u)
							{
								if (num != 820334071u)
								{
									return;
								}
								if (!(key == "leahTree"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(744, 999999f, 1, 0, new Vector2(42f, 8f) * (float)Game1.tileSize, false, false));
								return;
							}
							else
							{
								if (!(key == "leahLaptop"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(130, 1849, 19, 19), 9999f, 1, 999, new Vector2(12f, 10f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 4 + Game1.pixelZoom * 2)), false, false, (float)(29 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								return;
							}
						}
						else
						{
							if (!(key == "curtainClose"))
							{
								return;
							}
							location.getTemporarySpriteByID(999).sourceRect.X = 644;
							Game1.playSound("shwip");
							return;
						}
					}
					else if (num != 888455659u)
					{
						if (num != 1006429515u)
						{
							if (num != 1042337593u)
							{
								return;
							}
							if (!(key == "iceFishingCatch"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 500f, 3, 99999, new Vector2(68f, 30f) * (float)Game1.tileSize, false, false, (float)(31 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 510f, 3, 99999, new Vector2(74f, 30f) * (float)Game1.tileSize, false, false, (float)(31 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 490f, 3, 99999, new Vector2(67f, 36f) * (float)Game1.tileSize, false, false, (float)(37 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 500f, 3, 99999, new Vector2(76f, 35f) * (float)Game1.tileSize, false, false, (float)(36 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							return;
						}
						else
						{
							if (!(key == "sebastianRide"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(405, 1843, 14, 9), 40f, 4, 999, new Vector2(19f, 8f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.pixelZoom * 7)), false, false, (float)(28 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								motion = new Vector2(-2f, 0f)
							});
							return;
						}
					}
					else
					{
						if (!(key == "WizardPromise"))
						{
							return;
						}
						Utility.addSprinklesToLocation(location, 16, 15, 9, 9, 2000, 50, Color.White, null, false);
						return;
					}
				}
				else if (num <= 1984971571u)
				{
					if (num <= 1382680148u)
					{
						if (num <= 1139299912u)
						{
							if (num != 1057059047u)
							{
								if (num != 1139299912u)
								{
									return;
								}
								if (!(key == "moonlightJellies"))
								{
									return;
								}
								if (this.npcControllers != null)
								{
									this.npcControllers.Clear();
								}
								this.underwaterSprites = new List<TemporaryAnimatedSprite>();
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(26f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									delayBeforeAnimationStart = 10000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(29f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(31f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 41 * Game1.tileSize,
									delayBeforeAnimationStart = 12000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(20f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 27 * Game1.tileSize,
									delayBeforeAnimationStart = 14000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(17f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 29 * Game1.tileSize,
									delayBeforeAnimationStart = 19500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(16f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 32 * Game1.tileSize,
									delayBeforeAnimationStart = 20300,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(17f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 39 * Game1.tileSize,
									delayBeforeAnimationStart = 21500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(16f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 44 * Game1.tileSize,
									delayBeforeAnimationStart = 22400,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(12f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 42 * Game1.tileSize,
									delayBeforeAnimationStart = 23200,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(9f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 43 * Game1.tileSize,
									delayBeforeAnimationStart = 24000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(18f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 30 * Game1.tileSize,
									delayBeforeAnimationStart = 24600,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(33f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									delayBeforeAnimationStart = 25600,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(36f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 39 * Game1.tileSize,
									delayBeforeAnimationStart = 26900,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(21f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 34 * Game1.tileSize,
									delayBeforeAnimationStart = 28000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(20f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 35 * Game1.tileSize,
									delayBeforeAnimationStart = 28500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(22f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 36 * Game1.tileSize,
									delayBeforeAnimationStart = 28500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(33f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 43 * Game1.tileSize,
									delayBeforeAnimationStart = 29000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(36f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 43 * Game1.tileSize,
									delayBeforeAnimationStart = 30000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 32, 16, 16), 250f, 3, 9999, new Vector2(28f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-0.5f, -0.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 4000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 2f,
									xStopCoordinate = 19 * Game1.tileSize,
									yStopCoordinate = 38 * Game1.tileSize,
									delayBeforeAnimationStart = 32000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(40f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									delayBeforeAnimationStart = 10000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(42f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 43 * Game1.tileSize,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(43f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 41 * Game1.tileSize,
									delayBeforeAnimationStart = 12000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(45f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 39 * Game1.tileSize,
									delayBeforeAnimationStart = 14000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(46f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 29 * Game1.tileSize,
									delayBeforeAnimationStart = 19500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(48f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 35 * Game1.tileSize,
									delayBeforeAnimationStart = 20300,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(49f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									delayBeforeAnimationStart = 21500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(50f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 30 * Game1.tileSize,
									delayBeforeAnimationStart = 22400,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(51f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 33 * Game1.tileSize,
									delayBeforeAnimationStart = 23200,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(52f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 38 * Game1.tileSize,
									delayBeforeAnimationStart = 24000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(53f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 35 * Game1.tileSize,
									delayBeforeAnimationStart = 24600,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(54f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 30 * Game1.tileSize,
									delayBeforeAnimationStart = 25600,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(55f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									delayBeforeAnimationStart = 26900,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(4f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 30 * Game1.tileSize,
									delayBeforeAnimationStart = 24000,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(5f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 40 * Game1.tileSize,
									delayBeforeAnimationStart = 24600,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(3f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 34 * Game1.tileSize,
									delayBeforeAnimationStart = 25600,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(6f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 37 * Game1.tileSize,
									delayBeforeAnimationStart = 26900,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(8f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1f),
									xPeriodic = true,
									xPeriodicLoopTime = 3000f,
									xPeriodicRange = (float)(Game1.tileSize / 4),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 42 * Game1.tileSize,
									delayBeforeAnimationStart = 26900,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(50f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 42 * Game1.tileSize,
									delayBeforeAnimationStart = 28500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(51f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 43 * Game1.tileSize,
									delayBeforeAnimationStart = 28500,
									pingPong = true
								});
								this.underwaterSprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(52f, 49f) * (float)Game1.tileSize, false, false, 0.1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, -1.5f),
									xPeriodic = true,
									xPeriodicLoopTime = 2500f,
									xPeriodicRange = (float)(Game1.tileSize / 6),
									light = true,
									lightcolor = Color.Black,
									lightRadius = 1f,
									yStopCoordinate = 44 * Game1.tileSize,
									delayBeforeAnimationStart = 29000,
									pingPong = true
								});
								return;
							}
							else
							{
								if (!(key == "waterShane"))
								{
									return;
								}
								this.drawTool = true;
								this.tmpItem = Game1.player.CurrentItem;
								Game1.player.items[Game1.player.CurrentToolIndex] = new WateringCan();
								Game1.player.CurrentTool.Update(1, 0);
								Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[]
								{
									new FarmerSprite.AnimationFrame(58, 0, false, false, null, false),
									new FarmerSprite.AnimationFrame(58, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
									new FarmerSprite.AnimationFrame(59, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
									new FarmerSprite.AnimationFrame(45, 500, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
								});
								return;
							}
						}
						else if (num != 1215102451u)
						{
							if (num != 1266391031u)
							{
								if (num != 1382680148u)
								{
									return;
								}
								if (!(key == "EmilySleeping"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(574, 1892, 11, 11), 1000f, 2, 99999, new Vector2(20f, 3f) * (float)Game1.tileSize + new Vector2((float)(2 * Game1.pixelZoom), (float)(Game1.pixelZoom * 8)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									id = 999f
								});
								return;
							}
							else
							{
								if (!(key == "wedding"))
								{
									return;
								}
								if (Game1.player.isMale)
								{
									this.oldShirt = Game1.player.shirt;
									Game1.player.changeShirt(10);
									this.oldPants = Game1.player.pantsColor;
									Game1.player.changePants(new Color(49, 49, 49));
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(540, 1196, 98, 54), 99999f, 1, 99999, new Vector2(25f, 60f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(540, 1250, 98, 25), 99999f, 1, 99999, new Vector2(25f, 60f) * (float)Game1.tileSize + new Vector2(0f, 54f) * (float)Game1.pixelZoom + new Vector2(0f, (float)(-(float)Game1.tileSize)), false, false, 0f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(24f, 62f) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(32f, 62f) * (float)Game1.tileSize, false, false, 0f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(24f, 69f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(32f, 69f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								return;
							}
						}
						else
						{
							if (!(key == "waterShaneDone"))
							{
								return;
							}
							Game1.player.completelyStopAnimatingOrDoingAction();
							Game1.player.items[Game1.player.CurrentToolIndex] = this.tmpItem;
							this.drawTool = false;
							location.removeTemporarySpritesWithID(999);
							return;
						}
					}
					else if (num <= 1834783535u)
					{
						if (num != 1782528198u)
						{
							if (num != 1797522365u)
							{
								if (num != 1834783535u)
								{
									return;
								}
								if (!(key == "jasGift"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1231, 16, 16), 100f, 6, 1, new Vector2(22f, 16f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									id = 999f,
									paused = true,
									holdLastFrame = true
								});
								return;
							}
							else
							{
								if (!(key == "marcelloLand"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1183, 84, 160), 10000f, 1, 99999, new Vector2(25f, 19f) * (float)Game1.tileSize + new Vector2(-23f, 0f) * (float)Game1.pixelZoom, false, false, 2E-05f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, 2f),
									yStopCoordinate = 41 * Game1.tileSize - 160 * Game1.pixelZoom,
									reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.marcelloBalloonLand),
									attachedCharacter = this.getActorByName("Marcello"),
									id = 1f
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(84, 1205, 38, 26), 10000f, 1, 99999, new Vector2(25f, 19f) * (float)Game1.tileSize + new Vector2(0f, 134f) * (float)Game1.pixelZoom, false, false, (float)(41 * Game1.tileSize) / 10000f + 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(0f, 2f),
									id = 2f
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(24, 1343, 36, 19), 7000f, 1, 99999, new Vector2(25f, 40f) * (float)Game1.tileSize, false, false, 1E-05f, 0f, Color.White, 0f, 0f, 0f, 0f, false)
								{
									scaleChange = 0.01f,
									id = 3f
								});
								return;
							}
						}
						else
						{
							if (!(key == "maruTelescope"))
							{
								return;
							}
							for (int l = 0; l < 9; l++)
							{
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(256, 1680, 16, 16), 80f, 5, 0, new Vector2((float)Game1.random.Next(1, 28), (float)Game1.random.Next(1, 20)) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									delayBeforeAnimationStart = 8000 + l * Game1.random.Next(2000),
									motion = new Vector2(4f, 4f)
								});
							}
							return;
						}
					}
					else if (num != 1927366026u)
					{
						if (num != 1976122661u)
						{
							if (num != 1984971571u)
							{
								return;
							}
							if (!(key == "secretGift"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1231, 16, 16), new Vector2(30f, 70f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 3)), false, 0f, Color.White)
							{
								animationLength = 1,
								interval = 999999f,
								id = 666f,
								scale = 4f
							});
							return;
						}
						else
						{
							if (!(key == "curtainOpen"))
							{
								return;
							}
							location.getTemporarySpriteByID(999).sourceRect.X = 672;
							Game1.playSound("shwip");
							return;
						}
					}
					else
					{
						if (!(key == "elliottBoat"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(461, 1843, 32, 51), 1000f, 2, 9999, new Vector2(15f, 26f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.pixelZoom * 7), 0f), false, false, (float)(26 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
						return;
					}
				}
				else
				{
					if (num > 2236820530u)
					{
						if (num <= 2303195829u)
						{
							if (num != 2254371385u)
							{
								if (num != 2279977045u)
								{
									if (num != 2303195829u)
									{
										return;
									}
									if (!(key == "dickGlitter"))
									{
										return;
									}
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false));
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
									{
										delayBeforeAnimationStart = 200
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
									{
										delayBeforeAnimationStart = 300
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
									{
										delayBeforeAnimationStart = 100
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(Game1.tileSize / 4)), false, false, 1f, 0f, Color.White, (float)(Game1.pixelZoom / 2), 0f, 0f, 0f, false)
									{
										delayBeforeAnimationStart = 400
									});
									return;
								}
								else
								{
									if (!(key == "abbyAtLake"))
									{
										return;
									}
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(735, 999999f, 1, 0, new Vector2(48f, 30f) * (float)Game1.tileSize, false, false)
									{
										light = true,
										lightRadius = 2f
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2000f,
										yPeriodicLoopTime = 1600f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										yPeriodicRange = (float)(Game1.tileSize / 3)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 1000f,
										yPeriodicLoopTime = 1600f,
										xPeriodicRange = (float)(Game1.tileSize / 4),
										yPeriodicRange = (float)(Game1.tileSize / 3)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2400f,
										yPeriodicLoopTime = 2800f,
										xPeriodicRange = (float)(Game1.tileSize / 3),
										yPeriodicRange = (float)(Game1.tileSize / 2)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2000f,
										yPeriodicLoopTime = 2400f,
										xPeriodicRange = (float)(Game1.tileSize / 4),
										yPeriodicRange = (float)(Game1.tileSize / 4)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2000f,
										yPeriodicLoopTime = 2600f,
										xPeriodicRange = (float)(Game1.tileSize / 3),
										yPeriodicRange = (float)(Game1.tileSize * 3 / 4)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2000f,
										yPeriodicLoopTime = 2600f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										yPeriodicRange = (float)(Game1.tileSize / 3)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 4000f,
										yPeriodicLoopTime = 5000f,
										xPeriodicRange = (float)(Game1.tileSize * 2 / 3),
										yPeriodicRange = (float)(Game1.tileSize / 2)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 4000f,
										yPeriodicLoopTime = 5500f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										yPeriodicRange = (float)(Game1.tileSize / 2)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2400f,
										yPeriodicLoopTime = 3600f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										yPeriodicRange = (float)(Game1.tileSize / 3)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2500f,
										yPeriodicLoopTime = 3600f,
										xPeriodicRange = (float)(Game1.tileSize * 2 / 3),
										yPeriodicRange = (float)(Game1.tileSize * 4 / 5)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 4500f,
										yPeriodicLoopTime = 3000f,
										xPeriodicRange = (float)(Game1.tileSize / 3),
										yPeriodicRange = (float)(Game1.tileSize / 2)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 5000f,
										yPeriodicLoopTime = 4500f,
										xPeriodicRange = (float)Game1.tileSize,
										yPeriodicRange = (float)(Game1.tileSize * 3 / 4)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2000f,
										yPeriodicLoopTime = 3000f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										yPeriodicRange = (float)(Game1.tileSize / 3)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), 0f), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 2900f,
										yPeriodicLoopTime = 3200f,
										xPeriodicRange = (float)(Game1.tileSize / 3),
										yPeriodicRange = (float)(Game1.tileSize / 2)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 4200f,
										yPeriodicLoopTime = 3300f,
										xPeriodicRange = (float)(Game1.tileSize / 4),
										yPeriodicRange = (float)(Game1.tileSize / 2)
									});
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.tileSize / 2)), false, false, 1f, 0f, Color.White, 1f, 0f, 0f, 0f, false)
									{
										lightcolor = Color.Orange,
										light = true,
										lightRadius = 0.2f,
										xPeriodic = true,
										yPeriodic = true,
										xPeriodicLoopTime = 5100f,
										yPeriodicLoopTime = 4000f,
										xPeriodicRange = (float)(Game1.tileSize / 2),
										yPeriodicRange = (float)(Game1.tileSize / 4)
									});
								}
							}
							else
							{
								if (!(key == "junimoCage"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(325, 1977, 18, 19), 60f, 3, 999999, new Vector2(10f, 17f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.pixelZoom)), false, false, 0f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									light = true,
									lightRadius = 1f,
									lightcolor = Color.Black,
									id = 1f,
									shakeIntensity = 0f
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float)Game1.tileSize + new Vector2(0f, (float)(-(float)Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									light = true,
									lightRadius = 0.5f,
									lightcolor = Color.Black,
									id = 1f,
									xPeriodic = true,
									xPeriodicLoopTime = 2000f,
									xPeriodicRange = (float)(6 * Game1.pixelZoom),
									yPeriodic = true,
									yPeriodicLoopTime = 2000f,
									yPeriodicRange = (float)(6 * Game1.pixelZoom)
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float)Game1.tileSize + new Vector2((float)(18 * Game1.pixelZoom), (float)(-(float)Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									light = true,
									lightRadius = 0.5f,
									lightcolor = Color.Black,
									id = 1f,
									xPeriodic = true,
									xPeriodicLoopTime = 2000f,
									xPeriodicRange = (float)(-6 * Game1.pixelZoom),
									yPeriodic = true,
									yPeriodicLoopTime = 2000f,
									yPeriodicRange = (float)(6 * Game1.pixelZoom),
									delayBeforeAnimationStart = 250
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float)Game1.tileSize + new Vector2(0f, (float)(13 * Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									light = true,
									lightRadius = 0.5f,
									lightcolor = Color.Black,
									id = 1f,
									xPeriodic = true,
									xPeriodicLoopTime = 2000f,
									xPeriodicRange = (float)(-6 * Game1.pixelZoom),
									yPeriodic = true,
									yPeriodicLoopTime = 2000f,
									yPeriodicRange = (float)(6 * Game1.pixelZoom),
									delayBeforeAnimationStart = 450
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float)Game1.tileSize + new Vector2((float)(18 * Game1.pixelZoom), (float)(13 * Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									light = true,
									lightRadius = 0.5f,
									lightcolor = Color.Black,
									id = 1f,
									xPeriodic = true,
									xPeriodicLoopTime = 2000f,
									xPeriodicRange = (float)(6 * Game1.pixelZoom),
									yPeriodic = true,
									yPeriodicLoopTime = 2000f,
									yPeriodicRange = (float)(6 * Game1.pixelZoom),
									delayBeforeAnimationStart = 650
								});
								return;
							}
						}
						else if (num != 2306978800u)
						{
							if (num != 2354569370u)
							{
								if (num != 2358341695u)
								{
									return;
								}
								if (!(key == "secretGiftOpen"))
								{
									return;
								}
								TemporaryAnimatedSprite t = location.getTemporarySpriteByID(666);
								if (t != null)
								{
									t.animationLength = 6;
									t.interval = 100f;
									t.totalNumberOfLoops = 1;
									t.timer = 0f;
									t.holdLastFrame = true;
									return;
								}
							}
							else
							{
								if (!(key == "beachStuff"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1887, 47, 29), 9999f, 1, 999, new Vector2(44f, 21f) * (float)Game1.tileSize, false, false, 1E-05f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								return;
							}
						}
						else
						{
							if (!(key == "shakeTent"))
							{
								return;
							}
							location.getTemporarySpriteByID(999).shakeIntensity = 1f;
							return;
						}
						return;
					}
					if (num <= 2103090580u)
					{
						if (num != 1992359646u)
						{
							if (num != 2082305658u)
							{
								if (num != 2103090580u)
								{
									return;
								}
								if (!(key == "grandpaNight"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1453, 639, 176), 9999f, 1, 999999, new Vector2(0f, 1f) * (float)Game1.tileSize, false, false, 0.9f, 0f, Color.Cyan, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
								{
									alpha = 0.01f,
									alphaFade = -0.002f,
									local = true
								});
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1453, 639, 176), 9999f, 1, 999999, new Vector2(0f, (float)(176 * Game1.pixelZoom + Game1.tileSize)), false, true, 0.9f, 0f, Color.Blue, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
								{
									alpha = 0.01f,
									alphaFade = -0.002f,
									local = true
								});
								return;
							}
							else
							{
								if (!(key == "EmilySongBackLights"))
								{
									return;
								}
								this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
								for (int lightcolumns = 0; lightcolumns < 5; lightcolumns++)
								{
									for (int yPos = 0; yPos < Game1.graphics.GraphicsDevice.Viewport.Height + 12 * Game1.pixelZoom; yPos += 12 * Game1.pixelZoom)
									{
										this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(681, 1890, 18, 12), 42241f, 1, 1, new Vector2((float)((lightcolumns + 1) * Game1.graphics.GraphicsDevice.Viewport.Width / 5 - Game1.graphics.GraphicsDevice.Viewport.Width / 7), (float)(-6 * Game1.pixelZoom + yPos)), false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
										{
											xPeriodic = true,
											xPeriodicLoopTime = 1760f,
											xPeriodicRange = (float)(Game1.tileSize * 2 + yPos / 12 * Game1.pixelZoom),
											delayBeforeAnimationStart = lightcolumns * 100 + yPos / 4,
											local = true
										});
									}
								}
								for (int numFlyers = 0; numFlyers < 27; numFlyers++)
								{
									int flyerNumber = 0;
									int yPos2 = Game1.random.Next(Game1.tileSize, Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize);
									int loopTime = Game1.random.Next(800, 2000);
									int loopRange = Game1.random.Next(Game1.tileSize / 2, Game1.tileSize);
									bool pulse = Game1.random.NextDouble() < 0.25;
									int speed = Game1.random.Next(-6, -3);
									for (int tails = 0; tails < 8; tails++)
									{
										this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(616 + flyerNumber * 10, 1891, 10, 10), 42241f, 1, 1, new Vector2((float)Game1.graphics.GraphicsDevice.Viewport.Width, (float)yPos2), false, false, 0.01f, 0f, Color.White * (1f - (float)tails * 0.11f), (float)Game1.pixelZoom, 0f, 0f, 0f, false)
										{
											yPeriodic = true,
											motion = new Vector2((float)speed, 0f),
											yPeriodicLoopTime = (float)loopTime,
											pulse = pulse,
											pulseTime = 440f,
											pulseAmount = 1.5f,
											yPeriodicRange = (float)loopRange,
											delayBeforeAnimationStart = 14000 + numFlyers * 900 + tails * 100,
											local = true
										});
									}
								}
								for (int numRainbows = 0; numRainbows < 15; numRainbows++)
								{
									int it = 0;
									int yPos3 = Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2);
									for (int xPos = Game1.graphics.GraphicsDevice.Viewport.Height; xPos >= -Game1.tileSize; xPos -= Game1.tileSize * 3 / 4)
									{
										this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(597, 1888, 16, 16), 99999f, 1, 99999, new Vector2((float)yPos3, (float)xPos), false, false, 1f, 0.02f, Color.White, (float)Game1.pixelZoom, 0f, -1.57079637f, 0f, false)
										{
											delayBeforeAnimationStart = 27500 + numRainbows * 880 + it * 25,
											local = true
										});
										it++;
									}
								}
								for (int numRainbows2 = 0; numRainbows2 < 120; numRainbows2++)
								{
									this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(626 + numRainbows2 / 28 * 10, 1891, 10, 10), 2000f, 1, 1, new Vector2((float)Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Width), (float)Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Height)), false, false, 0.01f, 0f, Color.White, 0.1f, 0f, 0f, 0f, false)
									{
										motion = new Vector2(0f, -2f),
										alphaFade = 0.002f,
										scaleChange = 0.5f,
										scaleChangeChange = -0.0085f,
										delayBeforeAnimationStart = 27500 + numRainbows2 * 110,
										local = true
									});
								}
								return;
							}
						}
						else
						{
							if (!(key == "shaneThrowCan"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(542, 1893, 4, 6), 99999f, 1, 99999, new Vector2(103f, 95f) * (float)Game1.tileSize + new Vector2(0f, 4f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								motion = new Vector2(0f, -4f),
								acceleration = new Vector2(0f, 0.25f),
								rotationChange = 0.0245436933f
							});
							Game1.playSound("shwip");
							return;
						}
					}
					else if (num != 2140977878u)
					{
						if (num != 2197978832u)
						{
							if (num != 2236820530u)
							{
								return;
							}
							if (!(key == "shaneHospital"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(533, 1864, 19, 10), 99999f, 1, 99999, new Vector2(20f, 3f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(3 * Game1.pixelZoom)), false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 999f
							});
							return;
						}
						else
						{
							if (!(key == "EmilyCamping"))
							{
								return;
							}
							this.showGroundObjects = false;
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(644, 1578, 59, 53), 999999f, 1, 99999, new Vector2(26f, 9f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.pixelZoom * 4), 0f), false, false, (float)(9 * Game1.tileSize + 53 * Game1.pixelZoom) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 999f
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(675, 1299, 29, 24), 999999f, 1, 99999, new Vector2(27f, 14f) * (float)Game1.tileSize, false, false, 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 99f
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), new Vector2(27f, 14f) * (float)Game1.tileSize + new Vector2(8f, 4f) * (float)Game1.pixelZoom, false, 0f, Color.White)
							{
								interval = 50f,
								totalNumberOfLoops = 99999,
								animationLength = 4,
								light = true,
								lightID = 666,
								id = 666f,
								lightRadius = 2f,
								scale = (float)Game1.pixelZoom,
								layerDepth = 0.01f
							});
							Game1.currentLightSources.Add(new LightSource(4, new Vector2(27f, 14f) * (float)Game1.tileSize, 2f));
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(585, 1846, 26, 22), 999999f, 1, 99999, new Vector2(25f, 12f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 2), 0f), false, false, 0.001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 96f
							});
							AmbientLocationSounds.addSound(new Vector2(27f, 14f), 1);
							return;
						}
					}
					else
					{
						if (!(key == "ClothingTherapy"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(644, 1405, 28, 46), 999999f, 1, 99999, new Vector2(5f, 6f) * (float)Game1.tileSize + new Vector2((float)(-8 * Game1.pixelZoom), (float)(-36 * Game1.pixelZoom)), false, false, (float)(6 * Game1.tileSize + 10 * Game1.pixelZoom) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							id = 999f
						});
						return;
					}
				}
			}
			else if (num <= 3216534692u)
			{
				if (num <= 2807316816u)
				{
					if (num <= 2478151365u)
					{
						if (num <= 2369812317u)
						{
							if (num != 2366212848u)
							{
								if (num != 2369812317u)
								{
									return;
								}
								if (!(key == "leahPicnic"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(96, 1808, 32, 48), 9999f, 1, 999, new Vector2(75f, 37f) * (float)Game1.tileSize, false, false, (float)(39 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								if (this.temporaryContent == null)
								{
									this.temporaryContent = Game1.content.CreateTemporary();
								}
								NPC m = new NPC(new AnimatedSprite(this.temporaryContent.Load<Texture2D>("Characters\\" + (Game1.player.isMale ? "LeahExMale" : "LeahExFemale")), 0, 16, 32), new Vector2(-100f, -100f) * (float)Game1.tileSize, 2, "LeahEx", null);
								this.actors.Add(m);
								return;
							}
							else
							{
								if (!(key == "stopShakeTent"))
								{
									return;
								}
								location.getTemporarySpriteByID(999).shakeIntensity = 0f;
								return;
							}
						}
						else if (num != 2411807913u)
						{
							if (num != 2463657030u)
							{
								if (num != 2478151365u)
								{
									return;
								}
								if (!(key == "samSkate1"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0), 9999f, 1, 999, new Vector2(12f, 90f) * (float)Game1.tileSize, false, false, 1E-05f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(4f, 0f),
									acceleration = new Vector2(-0.008f, 0f),
									xStopCoordinate = 21 * Game1.tileSize,
									reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.samPreOllie),
									attachedCharacter = this.getActorByName("Sam"),
									id = 1f
								});
								return;
							}
							else
							{
								if (!(key == "wizardWarp2"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(387, 1965, 16, 31), 9999f, 1, 999999, new Vector2(54f, 34f) * (float)Game1.tileSize + new Vector2(0f, (float)Game1.pixelZoom), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-1f, 2f),
									acceleration = new Vector2(-0.1f, 0.2f),
									scaleChange = 0.03f,
									alphaFade = 0.001f
								});
								return;
							}
						}
						else
						{
							if (!(key == "parrots1"))
							{
								return;
							}
							this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
							this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float)Game1.graphics.GraphicsDevice.Viewport.Width, (float)(Game1.tileSize * 4)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								motion = new Vector2(-3f, 0f),
								yPeriodic = true,
								yPeriodicLoopTime = 2000f,
								yPeriodicRange = (float)(Game1.tileSize / 2),
								delayBeforeAnimationStart = 0,
								local = true
							});
							this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float)Game1.graphics.GraphicsDevice.Viewport.Width, (float)(Game1.tileSize * 3)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								motion = new Vector2(-3f, 0f),
								yPeriodic = true,
								yPeriodicLoopTime = 2000f,
								yPeriodicRange = (float)(Game1.tileSize / 2),
								delayBeforeAnimationStart = 600,
								local = true
							});
							this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float)Game1.graphics.GraphicsDevice.Viewport.Width, (float)(Game1.tileSize * 5)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								motion = new Vector2(-3f, 0f),
								yPeriodic = true,
								yPeriodicLoopTime = 2000f,
								yPeriodicRange = (float)(Game1.tileSize / 2),
								delayBeforeAnimationStart = 1200,
								local = true
							});
							return;
						}
					}
					else if (num <= 2748349572u)
					{
						if (num != 2610559111u)
						{
							if (num != 2646482856u)
							{
								if (num != 2748349572u)
								{
									return;
								}
								if (!(key == "abbyManyBats"))
								{
									return;
								}
								for (int n = 0; n < 100; n++)
								{
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2(23f, 9f) * (float)Game1.tileSize, false, false, 1f, 0.003f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										xPeriodic = true,
										xPeriodicLoopTime = (float)Game1.random.Next(1500, 2500),
										xPeriodicRange = (float)Game1.random.Next(Game1.tileSize, Game1.tileSize * 3),
										motion = new Vector2((float)Game1.random.Next(-2, 3), (float)Game1.random.Next(-Game1.pixelZoom * 2, -Game1.pixelZoom)),
										delayBeforeAnimationStart = n * 30,
										startSound = ((n % 10 == 0 || Game1.random.NextDouble() < 0.1) ? "batScreech" : null)
									});
								}
								for (int i2 = 0; i2 < 100; i2++)
								{
									location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2(23f, 9f) * (float)Game1.tileSize, false, false, 1f, 0.003f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										motion = new Vector2((float)Game1.random.Next(-4, 5), (float)Game1.random.Next(-Game1.pixelZoom * 2, -Game1.pixelZoom)),
										delayBeforeAnimationStart = 10 + i2 * 30
									});
								}
								return;
							}
							else
							{
								if (!(key == "joshFootball"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(405, 1916, 14, 8), 40f, 6, 9999, new Vector2(25f, 16f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									rotation = -0.7853982f,
									rotationChange = 0.0157079641f,
									motion = new Vector2(6f, -4f),
									acceleration = new Vector2(0f, 0.2f),
									xStopCoordinate = 29 * Game1.tileSize,
									reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.catchFootball),
									layerDepth = 1f,
									id = 1f
								});
								return;
							}
						}
						else
						{
							if (!(key == "wizardSewerMagic"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), 50f, 4, 20, new Vector2(15f, 13f) * (float)Game1.tileSize + new Vector2((float)(Game1.pixelZoom * 2), 0f), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								light = true,
								lightRadius = 1f,
								lightcolor = Color.Black,
								alphaFade = 0.005f
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), 50f, 4, 20, new Vector2(17f, 13f) * (float)Game1.tileSize + new Vector2((float)(Game1.pixelZoom * 2), 0f), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								light = true,
								lightRadius = 1f,
								lightcolor = Color.Black,
								alphaFade = 0.005f
							});
							return;
						}
					}
					else if (num != 2749403796u)
					{
						if (num != 2805730427u)
						{
							if (num != 2807316816u)
							{
								return;
							}
							if (!(key == "EmilySign"))
							{
								return;
							}
							this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
							for (int numRainbows3 = 0; numRainbows3 < 10; numRainbows3++)
							{
								int iter = 0;
								int yPos4 = Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 2);
								for (int xPos2 = Game1.graphics.GraphicsDevice.Viewport.Width; xPos2 >= -Game1.tileSize; xPos2 -= Game1.tileSize * 3 / 4)
								{
									this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(597, 1888, 16, 16), 99999f, 1, 99999, new Vector2((float)xPos2, (float)yPos4), false, false, 1f, 0.02f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
									{
										delayBeforeAnimationStart = numRainbows3 * 600 + iter * 25,
										startSound = ((iter == 0) ? "dwoop" : null),
										local = true
									});
									iter++;
								}
							}
							return;
						}
						else
						{
							if (!(key == "grandpaSpirit"))
							{
								return;
							}
							TemporaryAnimatedSprite p = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(555, 1956, 18, 35), 9999f, 1, 99999, new Vector2(-1000f, -1010f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								yStopCoordinate = -1002 * Game1.tileSize,
								xPeriodic = true,
								xPeriodicLoopTime = 3000f,
								xPeriodicRange = (float)(Game1.tileSize / 4),
								motion = new Vector2(0f, 1f),
								overrideLocationDestroy = true
							};
							location.temporarySprites.Add(p);
							for (int i3 = 0; i3 < 19; i3++)
							{
								location.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
								{
									parentSprite = p,
									delayBeforeAnimationStart = (i3 + 1) * 500,
									overrideLocationDestroy = true,
									scale = 1f,
									alpha = 1f
								});
							}
							return;
						}
					}
					else
					{
						if (!(key == "skateboardFly"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1875, 16, 6), 9999f, 1, 999, new Vector2(26f, 90f) * (float)Game1.tileSize, false, false, 1E-05f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							rotationChange = 0.1308997f,
							motion = new Vector2(-8f, -10f),
							acceleration = new Vector2(0.02f, 0.3f),
							yStopCoordinate = 91 * Game1.tileSize,
							xStopCoordinate = 16 * Game1.tileSize,
							layerDepth = 1f
						});
						return;
					}
				}
				else if (num <= 3017460216u)
				{
					if (num <= 2881735469u)
					{
						if (num != 2833175679u)
						{
							if (num != 2862938824u)
							{
								if (num != 2881735469u)
								{
									return;
								}
								if (!(key == "maruTrapdoor"))
								{
									return;
								}
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1632, 16, 32), 150f, 4, 0, new Vector2(1f, 5f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(688, 1632, 16, 32), 99999f, 1, 0, new Vector2(1f, 5f) * (float)Game1.tileSize, false, false, 0.99f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									delayBeforeAnimationStart = 500
								});
								return;
							}
							else
							{
								if (!(key == "witchFlyby"))
								{
									return;
								}
								Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1886, 35, 29), 9999f, 1, 999999, new Vector2((float)Game1.graphics.GraphicsDevice.Viewport.Width, (float)(Game1.tileSize * 3)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
								{
									motion = new Vector2(-4f, 0f),
									acceleration = new Vector2(-0.025f, 0f),
									yPeriodic = true,
									yPeriodicLoopTime = 2000f,
									yPeriodicRange = (float)Game1.tileSize,
									local = true
								});
								return;
							}
						}
						else
						{
							if (!(key == "farmerForestVision"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(393, 1973, 1, 1), 9999f, 1, 999999, new Vector2(0f, 0f) * (float)Game1.tileSize, false, false, 0.9f, 0f, Color.LimeGreen * 0.85f, (float)(Game1.viewport.Width * 2), 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.002f,
								id = 1f
							});
							Game1.player.mailReceived.Add("canReadJunimoText");
							int x = -Game1.tileSize;
							int y = -Game1.tileSize;
							int index = 0;
							int yIndex = 0;
							while (y < Game1.viewport.Height + Game1.tileSize * 2)
							{
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(367 + ((index % 2 == 0) ? 8 : 0), 1969, 8, 8), 9999f, 1, 999999, new Vector2((float)x, (float)y), false, false, 0.99f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
								{
									alpha = 0f,
									alphaFade = -0.0015f,
									xPeriodic = true,
									xPeriodicLoopTime = 4000f,
									xPeriodicRange = (float)Game1.tileSize,
									yPeriodic = true,
									yPeriodicLoopTime = 5000f,
									yPeriodicRange = (float)(Game1.tileSize * 3 / 2),
									rotationChange = (float)Game1.random.Next(-1, 2) * 3.14159274f / 256f,
									id = 1f,
									delayBeforeAnimationStart = 20 * index
								});
								x += Game1.tileSize * 2;
								if (x > Game1.viewport.Width + Game1.tileSize)
								{
									yIndex++;
									x = ((yIndex % 2 == 0) ? (-Game1.tileSize) : Game1.tileSize);
									y += Game1.tileSize * 2;
								}
								index++;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width / 2 - 25 * Game1.pixelZoom), (float)(Game1.viewport.Height / 2 - 60 * Game1.pixelZoom)), false, false, 1f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 6000,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width / 4 - 25 * Game1.pixelZoom), (float)(Game1.viewport.Height / 4 - 30 * Game1.pixelZoom)), false, false, 0.99f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 9000,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width * 3 / 4), (float)(Game1.viewport.Height / 3 - 30 * Game1.pixelZoom)), false, false, 0.98f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 12000,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width / 3 - 15 * Game1.pixelZoom), (float)(Game1.viewport.Height * 3 / 4 - 30 * Game1.pixelZoom)), false, false, 0.97f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 15000,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width * 2 / 3), (float)(Game1.viewport.Height * 2 / 3 - 30 * Game1.pixelZoom)), false, false, 0.96f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 18000,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width / 8), (float)(Game1.viewport.Height / 5 - 30 * Game1.pixelZoom)), false, false, 0.95f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 19500,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float)(Game1.viewport.Width * 2 / 3), (float)(Game1.viewport.Height / 5 - 30 * Game1.pixelZoom)), false, false, 0.94f, 0f, Color.White, (float)(Game1.pixelZoom * 3) / 4f, 0f, 0f, 0f, true)
							{
								alpha = 0f,
								alphaFade = -0.001f,
								id = 1f,
								delayBeforeAnimationStart = 21000,
								scaleChange = 0.004f,
								xPeriodic = true,
								xPeriodicLoopTime = 4000f,
								xPeriodicRange = (float)Game1.tileSize,
								yPeriodic = true,
								yPeriodicLoopTime = 5000f,
								yPeriodicRange = (float)(Game1.tileSize / 2)
							});
							return;
						}
					}
					else if (num != 2956180802u)
					{
						if (num != 3003121694u)
						{
							if (num != 3017460216u)
							{
								return;
							}
							if (!(key == "shanePassedOut"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(533, 1864, 19, 27), 99999f, 1, 99999, new Vector2(25f, 7f) * (float)Game1.tileSize, false, false, 0.01f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 999f
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(552, 1862, 31, 21), 99999f, 1, 99999, new Vector2(25f, 7f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), false, false, 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							return;
						}
						else
						{
							if (!(key == "morrisFlying"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(105, 1318, 13, 31), 9999f, 1, 99999, new Vector2(32f, 13f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								motion = new Vector2(4f, -8f),
								rotationChange = 0.196349546f,
								shakeIntensity = 1f
							});
							return;
						}
					}
					else
					{
						if (!(key == "junimoCageGone"))
						{
							return;
						}
						location.removeTemporarySpritesWithID(1);
						return;
					}
				}
				else if (num <= 3091229030u)
				{
					if (num != 3023463258u)
					{
						if (num != 3065465346u)
						{
							if (num != 3091229030u)
							{
								return;
							}
							if (!(key == "candleBoat"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(240, 112, 16, 32), 1000f, 2, 99999, new Vector2(22f, 36f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 1f,
								light = true,
								lightRadius = 2f,
								lightcolor = Color.Black
							});
							return;
						}
						else
						{
							if (!(key == "abbyvideoscreen"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(167, 1714, 19, 14), 100f, 3, 9999, new Vector2(2f, 3f) * (float)Game1.tileSize + new Vector2(7f, 12f) * (float)Game1.pixelZoom, false, false, 0.0002f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							return;
						}
					}
					else
					{
						if (!(key == "arcaneBook"))
						{
							return;
						}
						for (int i4 = 0; i4 < 16; i4++)
						{
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float)(2 * Game1.tileSize), (float)(12 * Game1.tileSize + 6 * Game1.pixelZoom)) + new Vector2((float)Game1.random.Next(Game1.tileSize / 2), (float)(Game1.random.Next(Game1.tileSize / 2) - i4 * Game1.pixelZoom)), false, 0f, Color.White)
							{
								interval = 50f,
								totalNumberOfLoops = 99999,
								animationLength = 7,
								layerDepth = 1f,
								scale = (float)Game1.pixelZoom,
								alphaFade = 0.008f,
								motion = new Vector2(0f, -0.5f)
							});
						}
						this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
						this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(325, 1977, 18, 18), new Vector2((float)(2 * Game1.tileSize + 8 * Game1.pixelZoom), (float)(12 * Game1.tileSize + 8 * Game1.pixelZoom)), false, 0f, Color.White)
						{
							interval = 25f,
							totalNumberOfLoops = 99999,
							animationLength = 3,
							layerDepth = 1f,
							scale = 1f,
							scaleChange = 1f,
							scaleChangeChange = -0.05f,
							alpha = 0.65f,
							alphaFade = 0.005f,
							motion = new Vector2(-8f, -8f),
							acceleration = new Vector2(0.4f, 0.4f)
						});
						for (int i5 = 0; i5 < 16; i5++)
						{
							this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(2f, 12f) * (float)Game1.tileSize + new Vector2((float)Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), 0f), false, 0.002f, Color.Gray)
							{
								alpha = 0.75f,
								motion = new Vector2(1f, -1f) + new Vector2((float)(Game1.random.Next(100) - 50) / 100f, (float)(Game1.random.Next(100) - 50) / 100f),
								interval = 99999f,
								layerDepth = (float)(6 * Game1.tileSize) / 10000f + (float)Game1.random.Next(100) / 10000f,
								scale = (float)(Game1.pixelZoom * 3) / 4f,
								scaleChange = 0.01f,
								rotationChange = (float)Game1.random.Next(-5, 6) * 3.14159274f / 256f,
								delayBeforeAnimationStart = i5 * 25
							});
						}
						location.setMapTileIndex(2, 12, 2143, "Front", 1);
						return;
					}
				}
				else if (num != 3098278158u)
				{
					if (num != 3203398193u)
					{
						if (num != 3216534692u)
						{
							return;
						}
						if (!(key == "abbyOneBat"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2(23f, 9f) * (float)Game1.tileSize, false, false, 1f, 0.003f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							xPeriodic = true,
							xPeriodicLoopTime = 2000f,
							xPeriodicRange = (float)(Game1.tileSize * 2),
							motion = new Vector2(0f, (float)(-(float)Game1.pixelZoom * 2))
						});
						return;
					}
					else
					{
						if (!(key == "pennyMess"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(739, 999999f, 1, 0, new Vector2(10f, 5f) * (float)Game1.tileSize, false, false));
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(740, 999999f, 1, 0, new Vector2(15f, 5f) * (float)Game1.tileSize, false, false));
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(741, 999999f, 1, 0, new Vector2(16f, 6f) * (float)Game1.tileSize, false, false));
						return;
					}
				}
				else
				{
					if (!(key == "leahShow"))
					{
						return;
					}
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 688, 16, 32), 9999f, 1, 999, new Vector2(29f, 59f) * (float)Game1.tileSize - new Vector2(0f, (float)(Game1.tileSize / 4)), false, false, (float)(59 * Game1.tileSize) / 10000f - 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(112, 656, 16, 64), 9999f, 1, 999, new Vector2(29f, 56f) * (float)Game1.tileSize, false, false, (float)(59 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 688, 16, 32), 9999f, 1, 999, new Vector2(33f, 59f) * (float)Game1.tileSize - new Vector2(0f, (float)(Game1.tileSize / 4)), false, false, (float)(59 * Game1.tileSize) / 10000f - 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(128, 688, 16, 32), 9999f, 1, 999, new Vector2(33f, 58f) * (float)Game1.tileSize, false, false, (float)(59 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(160, 656, 32, 64), 9999f, 1, 999, new Vector2(29f, 60f) * (float)Game1.tileSize, false, false, (float)(63 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 688, 16, 32), 9999f, 1, 999, new Vector2(34f, 63f) * (float)Game1.tileSize, false, false, (float)(63 * Game1.tileSize) / 10000f - 0.0001f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(113, 592, 16, 64), 100f, 4, 99999, new Vector2(34f, 60f) * (float)Game1.tileSize, false, false, (float)(63 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
					if (this.temporaryContent == null)
					{
						this.temporaryContent = Game1.content.CreateTemporary();
					}
					NPC m = new NPC(new AnimatedSprite(this.temporaryContent.Load<Texture2D>("Characters\\" + (Game1.player.isMale ? "LeahExMale" : "LeahExFemale")), 0, 16, 32), new Vector2(46f, 57f) * (float)Game1.tileSize, 2, "LeahEx", null);
					this.actors.Add(m);
					return;
				}
			}
			else if (num <= 3744618201u)
			{
				if (num <= 3447055920u)
				{
					if (num <= 3339287078u)
					{
						if (num != 3339171029u)
						{
							if (num != 3339287078u)
							{
								return;
							}
							if (!(key == "parrotSlide"))
							{
								return;
							}
							location.getTemporarySpriteByID(666).yStopCoordinate = 88 * Game1.tileSize;
							location.getTemporarySpriteByID(666).motion.X = 0f;
							location.getTemporarySpriteByID(666).motion.Y = 1f;
							return;
						}
						else
						{
							if (!(key == "joshSteak"))
							{
								return;
							}
							location.temporarySprites.Clear();
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1936, 12, 20), 80f, 4, 99999, new Vector2(53f, 67f) * (float)Game1.tileSize + new Vector2(3f, 3f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 1f
							});
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(497, 1918, 11, 11), 999f, 1, 9999, new Vector2(50f, 68f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 2), (float)(-(float)Game1.tileSize / 8)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							return;
						}
					}
					else if (num != 3385570989u)
					{
						if (num != 3441767033u)
						{
							if (num != 3447055920u)
							{
								return;
							}
							if (!(key == "abbyGraveyard"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(736, 999999f, 1, 0, new Vector2(48f, 86f) * (float)Game1.tileSize, false, false));
							return;
						}
						else
						{
							if (!(key == "joshDog"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1916, 12, 20), 500f, 6, 9999, new Vector2(53f, 67f) * (float)Game1.tileSize + new Vector2(3f, 3f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 1f
							});
							return;
						}
					}
					else
					{
						if (!(key == "joshDinner"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(649, 9999f, 1, 9999, new Vector2(6f, 4f) * (float)Game1.tileSize + new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.tileSize / 2)), false, false)
						{
							layerDepth = (float)(4 * Game1.tileSize) / 10000f
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(664, 9999f, 1, 9999, new Vector2(8f, 4f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.pixelZoom * 2), (float)(Game1.tileSize / 2)), false, false)
						{
							layerDepth = (float)(4 * Game1.tileSize) / 10000f
						});
						return;
					}
				}
				else if (num <= 3513988944u)
				{
					if (num != 3448286637u)
					{
						if (num != 3457429727u)
						{
							if (num != 3513988944u)
							{
								return;
							}
							if (!(key == "junimoCageGone2"))
							{
								return;
							}
							location.removeTemporarySpritesWithID(1);
							Game1.viewportFreeze = true;
							Game1.viewport.X = -1000;
							Game1.viewport.Y = -1000;
							return;
						}
						else
						{
							if (!(key == "robot"))
							{
								return;
							}
							TemporaryAnimatedSprite parent2 = new TemporaryAnimatedSprite(this.getActorByName("robot").Sprite.Texture, new Microsoft.Xna.Framework.Rectangle(35, 42, 35, 42), 50f, 1, 9999, new Vector2(13f, 27f) * (float)Game1.tileSize - new Vector2(0f, (float)(Game1.tileSize / 2)), false, false, 0.98f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								acceleration = new Vector2(0f, -0.01f),
								accelerationChange = new Vector2(0f, -0.0001f)
							};
							location.temporarySprites.Add(parent2);
							for (int i6 = 0; i6 < 420; i6++)
							{
								location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(Game1.random.Next(4) * Game1.tileSize, 320, Game1.tileSize, Game1.tileSize), new Vector2((float)Game1.random.Next(Game1.tileSize * 3 / 2), (float)(Game1.tileSize * 2 + Game1.pixelZoom * 2)), false, 0.01f, Color.White * 0.75f)
								{
									layerDepth = 1f,
									delayBeforeAnimationStart = i6 * 10,
									animationLength = 1,
									currentNumberOfLoops = 0,
									interval = 9999f,
									motion = new Vector2((float)(Game1.random.Next(-100, 100) / (i6 + 20)), 0.25f + (float)i6 / 100f),
									parentSprite = parent2
								});
							}
							return;
						}
					}
					else
					{
						if (!(key == "ccCelebration"))
						{
							return;
						}
						this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
						for (int i7 = 0; i7 < 32; i7++)
						{
							Vector2 position3 = new Vector2((float)Game1.random.Next(Game1.viewport.Width - Game1.tileSize * 2), (float)(Game1.viewport.Height + i7 * Game1.tileSize));
							this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(534, 1413, 11, 16), 99999f, 1, 99999, position3, false, false, 1f, 0f, Utility.getRandomRainbowColor(null), (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								local = true,
								motion = new Vector2(0.25f, -1.5f),
								acceleration = new Vector2(0f, -0.001f)
							});
							this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(545, 1413, 11, 34), 99999f, 1, 99999, position3 + new Vector2(0f, 0f), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								local = true,
								motion = new Vector2(0.25f, -1.5f),
								acceleration = new Vector2(0f, -0.001f)
							});
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(558, 1425, 20, 26), 400f, 3, 99999, new Vector2(53f, 21f) * (float)Game1.tileSize, false, false, 0.5f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							pingPong = true
						});
						return;
					}
				}
				else if (num != 3580969289u)
				{
					if (num != 3732153936u)
					{
						if (num != 3744618201u)
						{
							return;
						}
						if (!(key == "linusLights"))
						{
							return;
						}
						Game1.currentLightSources.Add(new LightSource(2, new Vector2(55f, 62f) * (float)Game1.tileSize, 2f));
						Game1.currentLightSources.Add(new LightSource(2, new Vector2(60f, 62f) * (float)Game1.tileSize, 2f));
						Game1.currentLightSources.Add(new LightSource(2, new Vector2(57f, 60f) * (float)Game1.tileSize, 3f));
						Game1.currentLightSources.Add(new LightSource(2, new Vector2(57f, 60f) * (float)Game1.tileSize, 2f));
						Game1.currentLightSources.Add(new LightSource(2, new Vector2(47f, 70f) * (float)Game1.tileSize, 2f));
						Game1.currentLightSources.Add(new LightSource(2, new Vector2(52f, 63f) * (float)Game1.tileSize, 2f));
						return;
					}
					else
					{
						if (!(key == "wizardWarp"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(387, 1965, 16, 31), 9999f, 1, 999999, new Vector2(8f, 16f) * (float)Game1.tileSize + new Vector2(0f, (float)Game1.pixelZoom), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							motion = new Vector2(2f, -2f),
							acceleration = new Vector2(0.1f, 0f),
							scaleChange = -0.02f,
							alphaFade = 0.001f
						});
						return;
					}
				}
				else
				{
					if (!(key == "EmilyBoomBoxStart"))
					{
						return;
					}
					location.getTemporarySpriteByID(999).pulse = true;
					location.getTemporarySpriteByID(999).pulseTime = 420f;
					return;
				}
			}
			else if (num <= 3992366603u)
			{
				if (num <= 3872194338u)
				{
					if (num != 3760313400u)
					{
						if (num != 3775217944u)
						{
							if (num != 3872194338u)
							{
								return;
							}
							if (!(key == "alexDiningDog"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1936, 12, 20), 80f, 4, 99999, new Vector2(7f, 2f) * (float)Game1.tileSize + new Vector2(2f, -8f) * (float)Game1.pixelZoom, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
							{
								id = 1f
							});
							return;
						}
						else
						{
							if (!(key == "maruElectrocution"))
							{
								return;
							}
							location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1664, 16, 32), 40f, 1, 20, new Vector2(7f, 5f) * (float)Game1.tileSize - new Vector2((float)(-(float)Game1.tileSize / 8 + Game1.pixelZoom), (float)(Game1.tileSize / 8)), true, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
							return;
						}
					}
					else
					{
						if (!(key == "linusMoney"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1002f, -1000f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 10,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1003f, -1002f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 100,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-999f, -1000f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 200,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1004f, -1001f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 300,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1001f, -998f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 400,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-998f, -999f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 500,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-998f, -1002f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 600,
							overrideLocationDestroy = true
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-997f, -1001f) * (float)Game1.tileSize, false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							startSound = "money",
							delayBeforeAnimationStart = 700,
							overrideLocationDestroy = true
						});
						return;
					}
				}
				else if (num != 3920807573u)
				{
					if (num != 3925840365u)
					{
						if (num != 3992366603u)
						{
							return;
						}
						if (!(key == "parrotSplat"))
						{
							return;
						}
						this.aboveMapSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float)(Game1.viewport.X + Game1.graphics.GraphicsDevice.Viewport.Width), (float)(Game1.viewport.Y + Game1.tileSize)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
						{
							id = 999f,
							motion = new Vector2(-2f, 4f),
							acceleration = new Vector2(-0.1f, 0f),
							delayBeforeAnimationStart = 0,
							yStopCoordinate = 87 * Game1.tileSize,
							xStopCoordinate = 24 * Game1.tileSize - Game1.tileSize / 2,
							reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.parrotSplat)
						});
						return;
					}
					else
					{
						if (!(key == "pennyCook"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float)Game1.tileSize, false, 0f, Color.White)
						{
							layerDepth = 1f,
							animationLength = 6,
							interval = 75f,
							motion = new Vector2(0f, -0.5f)
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), 0f), false, 0f, Color.White)
						{
							layerDepth = 0.1f,
							animationLength = 6,
							interval = 75f,
							motion = new Vector2(0f, -0.5f),
							delayBeforeAnimationStart = 500
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float)Game1.tileSize + new Vector2((float)(-(float)Game1.tileSize / 4), 0f), false, 0f, Color.White)
						{
							layerDepth = 1f,
							animationLength = 6,
							interval = 75f,
							motion = new Vector2(0f, -0.5f),
							delayBeforeAnimationStart = 750
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float)Game1.tileSize, false, 0f, Color.White)
						{
							layerDepth = 0.1f,
							animationLength = 6,
							interval = 75f,
							motion = new Vector2(0f, -0.5f),
							delayBeforeAnimationStart = 1000
						});
						return;
					}
				}
				else
				{
					if (!(key == "linusCampfire"))
					{
						return;
					}
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), 50f, 4, 99999, new Vector2(29f, 9f) * (float)Game1.tileSize + new Vector2((float)(Game1.pixelZoom * 2), 0f), false, false, (float)(9 * Game1.tileSize) / 10000f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						light = true,
						lightRadius = 3f,
						lightcolor = Color.Black
					});
					return;
				}
			}
			else if (num <= 4119912933u)
			{
				if (num != 4027113491u)
				{
					if (num != 4028797203u)
					{
						if (num != 4119912933u)
						{
							return;
						}
						if (!(key == "umbrella"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1843, 27, 23), 80f, 3, 9999, new Vector2(12f, 39f) * (float)Game1.tileSize + new Vector2((float)(-5 * Game1.pixelZoom), (float)(-(float)Game1.tileSize * 3 / 2 - Game1.pixelZoom * 2)), false, false, 1f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false));
						return;
					}
					else
					{
						if (!(key == "abbyOuijaCandles"))
						{
							return;
						}
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(737, 999999f, 1, 0, new Vector2(5f, 9f) * (float)Game1.tileSize, false, false)
						{
							light = true,
							lightRadius = 1f
						});
						location.TemporarySprites.Add(new TemporaryAnimatedSprite(737, 999999f, 1, 0, new Vector2(7f, 8f) * (float)Game1.tileSize, false, false)
						{
							light = true,
							lightRadius = 1f
						});
						return;
					}
				}
				else
				{
					if (!(key == "candleBoatMove"))
					{
						return;
					}
					location.getTemporarySpriteByID(1).motion = new Vector2(0f, 2f);
					return;
				}
			}
			else if (num != 4188226542u)
			{
				if (num != 4199220531u)
				{
					if (num != 4215720008u)
					{
						return;
					}
					if (!(key == "maruBeaker"))
					{
						return;
					}
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(738, 1380f, 1, 0, new Vector2(9f, 14f) * (float)Game1.tileSize + new Vector2(0f, (float)(Game1.tileSize / 2)), false, false)
					{
						rotationChange = 0.1308997f,
						motion = new Vector2(0f, -7f),
						acceleration = new Vector2(0f, 0.2f),
						endFunction = new TemporaryAnimatedSprite.endBehavior(this.beakerSmashEndFunction),
						layerDepth = 1f
					});
					return;
				}
				else
				{
					if (!(key == "jasGiftOpen"))
					{
						return;
					}
					location.getTemporarySpriteByID(999).paused = false;
					location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(537, 1850, 11, 10), 1500f, 1, 1, new Vector2(23f, 16f) * (float)Game1.tileSize + new Vector2((float)(Game1.tileSize / 4), (float)(-(float)Game1.tileSize * 3) / 4f), false, false, 0.99f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, false)
					{
						motion = new Vector2(0f, -0.25f),
						delayBeforeAnimationStart = 500,
						yStopCoordinate = 14 * Game1.tileSize + Game1.tileSize / 2
					});
					location.temporarySprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize - Game1.tileSize / 2, 16 * Game1.tileSize - Game1.tileSize / 2, Game1.tileSize * 2, Game1.tileSize), 5, Color.White, 300, 0, ""));
					return;
				}
			}
			else
			{
				if (!(key == "parrotGone"))
				{
					return;
				}
				location.removeTemporarySpritesWithID(666);
				return;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00025AE8 File Offset: 0x00023CE8
		public void receiveMouseClick(int x, int y)
		{
			if (!this.skipped && this.skippable && x <= 24 * Game1.pixelZoom && y > Game1.viewport.Height - Game1.tileSize)
			{
				this.skipped = true;
				this.skipEvent();
				Game1.freezeControls = false;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00025B38 File Offset: 0x00023D38
		public void skipEvent()
		{
			Game1.playSound("drumkit6");
			this.actorPositionsAfterMove.Clear();
			foreach (NPC expr_2A in this.actors)
			{
				expr_2A.Halt();
				expr_2A.CurrentDialogue.Clear();
			}
			Game1.player.Halt();
			Game1.exitActiveMenu();
			Game1.dialogueUp = false;
			Game1.dialogueTyping = false;
			int num = this.id;
			if (num <= 100162)
			{
				if (num == 112)
				{
					this.endBehaviors(new string[]
					{
						"end"
					}, Game1.currentLocation);
					Game1.player.mailReceived.Add("canReadJunimoText");
					return;
				}
				if (num == 60367)
				{
					this.endBehaviors(new string[]
					{
						"end",
						"beginGame"
					}, Game1.currentLocation);
					return;
				}
				if (num == 100162)
				{
					if (Game1.player.hasItemWithNameThatContains("Rusty Sword") == null)
					{
						Game1.player.addItemByMenuIfNecessary(new MeleeWeapon(0), null);
					}
					Game1.player.position = new Vector2(-9999f, -99999f);
					this.endBehaviors(new string[]
					{
						"end"
					}, Game1.currentLocation);
					return;
				}
			}
			else
			{
				if (num == 558292)
				{
					Game1.player.eventsSeen.Remove(2146991);
					this.endBehaviors(new string[]
					{
						"end",
						"bed"
					}, Game1.currentLocation);
					return;
				}
				if (num == 739330)
				{
					if (Game1.player.hasItemWithNameThatContains("Bamboo Pole") == null)
					{
						Game1.player.addItemByMenuIfNecessary(new FishingRod(), null);
					}
					this.endBehaviors(new string[]
					{
						"end",
						"position",
						"43",
						"36"
					}, Game1.currentLocation);
					return;
				}
				if (num == 992553)
				{
					if (!Game1.player.craftingRecipes.ContainsKey("Furnace"))
					{
						Game1.player.craftingRecipes.Add("Furnace", 0);
					}
					if (!Game1.player.hasQuest(11))
					{
						Game1.player.addQuest(11);
					}
					this.endBehaviors(new string[]
					{
						"end"
					}, Game1.currentLocation);
					return;
				}
			}
			this.endBehaviors(new string[]
			{
				"end"
			}, Game1.currentLocation);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyPress(Keys k)
		{
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00002834 File Offset: 0x00000A34
		public void receiveKeyRelease(Keys k)
		{
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00025DC0 File Offset: 0x00023FC0
		public void receiveActionPress(int xTile, int yTile)
		{
			if (xTile == this.playerControlTargetTile.X && yTile == this.playerControlTargetTile.Y)
			{
				string a = this.playerControlSequenceID;
				if (a == "haleyBeach")
				{
					this.props.Clear();
					Game1.playSound("coin");
					this.playerControlTargetTile = new Point(35, 11);
					this.playerControlSequenceID = "haleyBeach2";
					return;
				}
				if (!(a == "haleyBeach2"))
				{
					return;
				}
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00025E4C File Offset: 0x0002404C
		public void startSecretSantaEvent()
		{
			this.playerControlSequence = false;
			this.playerControlSequenceID = null;
			this.eventCommands = this.festivalData["secretSanta"].Split(new char[]
			{
				'/'
			});
			this.setUpSecretSantaCommands();
			this.currentCommand = 0;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00025E9C File Offset: 0x0002409C
		public void festivalUpdate(GameTime time)
		{
			if (this.festivalTimer > 0)
			{
				this.oldTime = this.festivalTimer;
				this.festivalTimer -= time.ElapsedGameTime.Milliseconds;
				string a = this.playerControlSequenceID;
				if (a == "iceFishing")
				{
					if (!Game1.player.usingTool)
					{
						Game1.player.forceCanMove();
					}
					if (this.oldTime % 500 < this.festivalTimer % 500)
					{
						NPC temp = this.getActorByName("Pam");
						temp.sprite.sourceRect.Offset(temp.sprite.SourceRect.Width, 0);
						if (temp.sprite.sourceRect.X >= temp.sprite.Texture.Width)
						{
							temp.sprite.sourceRect.Offset(-temp.sprite.Texture.Width, 0);
						}
						temp = this.getActorByName("Elliott");
						temp.sprite.sourceRect.Offset(temp.sprite.SourceRect.Width, 0);
						if (temp.sprite.sourceRect.X >= temp.sprite.Texture.Width)
						{
							temp.sprite.sourceRect.Offset(-temp.sprite.Texture.Width, 0);
						}
						temp = this.getActorByName("Willy");
						temp.sprite.sourceRect.Offset(temp.sprite.SourceRect.Width, 0);
						if (temp.sprite.sourceRect.X >= temp.sprite.Texture.Width)
						{
							temp.sprite.sourceRect.Offset(-temp.sprite.Texture.Width, 0);
						}
					}
					if (this.oldTime % 29900 < this.festivalTimer % 29900)
					{
						this.getActorByName("Willy").shake(500);
						Game1.playSound("dwop");
						this.temporaryLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16), this.getActorByName("Willy").position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, 0.015f, Color.White)
						{
							layerDepth = 1f,
							scale = (float)Game1.pixelZoom,
							interval = 9999f,
							motion = new Vector2(0f, -1f)
						});
					}
					if (this.oldTime % 45900 < this.festivalTimer % 45900)
					{
						this.getActorByName("Pam").shake(500);
						Game1.playSound("dwop");
						this.temporaryLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16), this.getActorByName("Pam").position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, 0.015f, Color.White)
						{
							layerDepth = 1f,
							scale = (float)Game1.pixelZoom,
							interval = 9999f,
							motion = new Vector2(0f, -1f)
						});
					}
					if (this.oldTime % 59900 < this.festivalTimer % 59900)
					{
						this.getActorByName("Elliott").shake(500);
						Game1.playSound("dwop");
						this.temporaryLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16), this.getActorByName("Elliott").position + new Vector2(0f, (float)(-(float)Game1.tileSize * 3 / 2)), false, 0.015f, Color.White)
						{
							layerDepth = 1f,
							scale = (float)Game1.pixelZoom,
							interval = 9999f,
							motion = new Vector2(0f, -1f)
						});
					}
				}
				if (this.festivalTimer <= 0)
				{
					a = this.playerControlSequenceID;
					if (!(a == "eggHunt"))
					{
						if (a == "iceFishing")
						{
							this.playerControlSequence = false;
							this.playerControlSequenceID = null;
							this.eventCommands = this.festivalData["afterIceFishing"].Split(new char[]
							{
								'/'
							});
							this.currentCommand = 0;
							if (Game1.activeClickableMenu != null)
							{
								Game1.activeClickableMenu.emergencyShutDown();
							}
							Game1.activeClickableMenu = null;
							if (Game1.player.UsingTool && Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
							{
								(Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, false);
							}
							Game1.screenOverlayTempSprites.Clear();
							if (this.tempItemStash != null)
							{
								Game1.player.addItemToInventory(this.tempItemStash, 0);
								this.tempItemStash = null;
							}
							Game1.player.forceCanMove();
						}
					}
					else
					{
						this.playerControlSequence = false;
						this.playerControlSequenceID = null;
						this.eventCommands = this.festivalData["afterEggHunt"].Split(new char[]
						{
							'/'
						});
						this.currentCommand = 0;
					}
				}
			}
			if (this.underwaterSprites != null)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.underwaterSprites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.update(time);
					}
				}
			}
			if (this.startSecretSantaAfterDialogue && !Game1.dialogueUp)
			{
				Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.startSecretSantaEvent), 0.01f);
				this.startSecretSantaAfterDialogue = false;
			}
			Game1.player.festivalScore = Math.Min(Game1.player.festivalScore, 9999);
			if (this.waitingForMenuClose && Game1.activeClickableMenu == null)
			{
				string a = this.festivalData["name"];
				if (a == "Stardew Valley Fair")
				{
					MultiplayerUtility.sendMessageToEveryone(2, "null", 0L);
					if (Game1.IsServer)
					{
						this.playerUsingGrangeDisplay = null;
					}
				}
				this.waitingForMenuClose = false;
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00026514 File Offset: 0x00024714
		private void setUpSecretSantaCommands()
		{
			int secretSantaX = 0;
			int secretSantaY = 0;
			try
			{
				secretSantaX = this.getActorByName(this.mySecretSanta.name).getTileX();
				secretSantaY = this.getActorByName(this.mySecretSanta.name).getTileY();
			}
			catch (Exception)
			{
				this.mySecretSanta = this.getActorByName("Lewis");
				secretSantaX = this.getActorByName(this.mySecretSanta.name).getTileX();
				secretSantaY = this.getActorByName(this.mySecretSanta.name).getTileY();
			}
			string dialogue = "";
			string dialogue2 = "";
			switch (this.mySecretSanta.age)
			{
			case 0:
			case 1:
				switch (this.mySecretSanta.manners)
				{
				case 0:
				case 1:
					dialogue = "Hey, @. I'm your secret gift-giver this year.#$b#Here, open it.$h";
					dialogue2 = "It's not much, but I hope you like it.$h";
					break;
				case 2:
					dialogue = "Hi. So, I'm your secret gift-giver this year.#$b#Well? Open it!";
					dialogue2 = (this.mySecretSanta.name.Equals("George") ? "You don't like it, huh? Bah! Kids these days... no appreciation for anything. That was expensive, and I'm not made of money, you know!" : "It's nothing fancy, but It's the best I could afford.");
					break;
				}
				break;
			case 2:
				dialogue = "Um, excuse me.#$b#I have a gift for you. I found it last summer when I was playing at the beach. I hope you like it.$h";
				dialogue2 = "I'm glad you moved here, farmer @!$h";
				break;
			}
			for (int i = 0; i < this.eventCommands.Length; i++)
			{
				this.eventCommands[i] = this.eventCommands[i].Replace("secretSanta", this.mySecretSanta.name);
				this.eventCommands[i] = this.eventCommands[i].Replace("warpX", string.Concat(secretSantaX));
				this.eventCommands[i] = this.eventCommands[i].Replace("warpY", string.Concat(secretSantaY));
				this.eventCommands[i] = this.eventCommands[i].Replace("dialogue1", dialogue);
				this.eventCommands[i] = this.eventCommands[i].Replace("dialogue2", dialogue2);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0002670C File Offset: 0x0002490C
		public void draw(SpriteBatch b)
		{
			foreach (NPC i in this.actors)
			{
				i.name.Equals("Marcello");
				if (i.ySourceRectOffset == 0)
				{
					i.draw(b);
				}
				else
				{
					i.draw(b, i.ySourceRectOffset, 1f);
				}
			}
			using (List<Object>.Enumerator enumerator2 = this.props.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.drawAsProp(b);
				}
			}
			using (List<Prop>.Enumerator enumerator3 = this.festivalProps.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.Current.draw(b);
				}
			}
			if (this.isFestival)
			{
				using (List<Farmer>.Enumerator enumerator4 = Game1.currentLocation.farmers.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						enumerator4.Current.draw(b);
					}
				}
				if (Game1.IsMultiplayer && Game1.ChatBox != null)
				{
					Game1.ChatBox.draw(b);
				}
				string a = this.festivalData["name"];
				if (a == "Stardew Valley Fair" && this.grangeDisplay != null)
				{
					Vector2 start = Game1.GlobalToLocal(Game1.viewport, new Vector2(37f, 56f) * (float)Game1.tileSize);
					start.X += (float)Game1.pixelZoom;
					int xCutoff = (int)start.X + 3 * (Game1.tileSize - Game1.pixelZoom * 2);
					start.Y += (float)(Game1.tileSize / 8);
					for (int j = 0; j < this.grangeDisplay.Count; j++)
					{
						if (this.grangeDisplay[j] != null)
						{
							start.Y += (float)(Game1.tileSize * 2 / 3);
							start.X += (float)Game1.pixelZoom;
							b.Draw(Game1.shadowTexture, start, new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.0001f);
							start.Y -= (float)(Game1.tileSize * 2 / 3);
							start.X -= (float)Game1.pixelZoom;
							this.grangeDisplay[j].drawInMenu(b, start, 1f, 1f, (float)j / 1000f + 0.001f, false);
						}
						start.X += (float)(Game1.tileSize - Game1.pixelZoom);
						if (start.X >= (float)xCutoff)
						{
							start.X = (float)(xCutoff - (Game1.tileSize - Game1.pixelZoom * 2) * 3);
							start.Y += (float)Game1.tileSize;
						}
					}
				}
			}
			if (this.drawTool)
			{
				Game1.drawTool(Game1.player);
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00026A4C File Offset: 0x00024C4C
		public void drawUnderWater(SpriteBatch b)
		{
			if (this.underwaterSprites != null)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.underwaterSprites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, false, 0, 0);
					}
				}
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00026AA8 File Offset: 0x00024CA8
		public void drawAfterMap(SpriteBatch b)
		{
			if (this.aboveMapSprites != null)
			{
				using (List<TemporaryAnimatedSprite>.Enumerator enumerator = this.aboveMapSprites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.draw(b, false, 0, 0);
					}
				}
			}
			if (this.playerControlSequenceID != null)
			{
				string a = this.playerControlSequenceID;
				if (!(a == "eggHunt"))
				{
					if (!(a == "fair"))
					{
						if (a == "iceFishing")
						{
							b.End();
							b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
							b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + ((Game1.player.festivalScore > 999) ? (Game1.tileSize / 4) : 0), Game1.tileSize), Color.Black * 0.75f);
							b.Draw(this.festivalTexture, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
							Game1.drawWithBorder(string.Concat(Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2 + 16 * Game1.pixelZoom), (float)(Game1.tileSize / 3 + Game1.pixelZoom * 2)), 0f, 1f, 1f, false);
							Game1.drawWithBorder(Utility.getMinutesSecondsStringFromMilliseconds(this.festivalTimer), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 3 + Game1.tileSize + Game1.pixelZoom * 2)), 0f, 1f, 1f, false);
							b.End();
							b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
						}
					}
					else
					{
						b.End();
						b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
						b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + ((Game1.player.festivalScore > 999) ? (Game1.tileSize / 4) : 0), Game1.tileSize), Color.Black * 0.75f);
						b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.tileSize / 2), (float)(Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(338, 400, 8, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 1f);
						Game1.drawWithBorder(string.Concat(Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float)(Game1.tileSize / 2 + 10 * Game1.pixelZoom), (float)(Game1.tileSize / 3 + Game1.pixelZoom * 2)), 0f, 1f, 1f, false);
						if (Game1.activeClickableMenu == null)
						{
							Game1.dayTimeMoneyBox.drawMoneyBox(b, Game1.dayTimeMoneyBox.xPositionOnScreen, Game1.pixelZoom);
						}
						b.End();
						b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
					}
				}
				else
				{
					b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize * 3 + Game1.tileSize / 2, Game1.tileSize * 2 + Game1.tileSize / 2), Color.Black * 0.5f);
					Game1.drawWithBorder("Time: " + this.festivalTimer / 1000, Color.Black, Color.Yellow, new Vector2((float)Game1.tileSize, (float)Game1.tileSize), 0f, 1f, 1f, false);
					Game1.drawWithBorder("Eggs: " + Game1.player.festivalScore, Color.Black, Color.Pink, new Vector2((float)Game1.tileSize, (float)(Game1.tileSize * 2)), 0f, 1f, 1f, false);
				}
			}
			using (List<NPC>.Enumerator enumerator2 = this.actors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.drawAboveAlwaysFrontLayer(b);
				}
			}
			if (this.skippable)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)(Game1.pixelZoom * 2), (float)(Game1.viewport.Height - Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(205, 406, 22, 15)), Color.White * ((Game1.getOldMouseX() <= 24 * Game1.pixelZoom && Game1.getOldMouseY() > Game1.viewport.Height - Game1.tileSize) ? 1f : 0.5f), 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.92f);
			}
			if (!this.isFestival && !Game1.options.hardwareCursor)
			{
				b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getOldMouseX(), (float)Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0002704C File Offset: 0x0002524C
		public void setUpPlayerControlSequence(string id)
		{
			this.playerControlSequenceID = id;
			this.playerControlSequence = true;
			Game1.player.CanMove = true;
			using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.CanMove = true;
				}
			}
			Game1.viewportFreeze = false;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
			if (num <= 1073062698u)
			{
				if (num <= 750634491u)
				{
					if (num != 5462067u)
					{
						if (num != 750634491u)
						{
							return;
						}
						if (!(id == "christmas"))
						{
							return;
						}
						Random r = new Random((int)(Game1.uniqueIDForThisGame / 2uL) - Game1.year);
						this.secretSantaRecipient = Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player));
						while (this.mySecretSanta == null || this.mySecretSanta.Equals(this.secretSantaRecipient))
						{
							this.mySecretSanta = Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player) + 10);
						}
						Game1.debugOutput = "Secret Santa Recipient: " + this.secretSantaRecipient.name + "  My Secret Santa: " + this.mySecretSanta.name;
						return;
					}
					else
					{
						if (!(id == "fair"))
						{
							return;
						}
						this.festivalTexture = this.temporaryContent.Load<Texture2D>("Maps\\Festivals");
						this.festivalHost = this.getActorByName("Lewis");
						this.hostMessage = "$q -1 null#Oh... are you already finished setting up your grange display?#$r -1 0 yes#Yes.#$r -1 0 no#Not yet.";
						return;
					}
				}
				else if (num != 863075767u)
				{
					if (num != 875582698u)
					{
						if (num != 1073062698u)
						{
							return;
						}
						if (!(id == "flowerFestival"))
						{
							return;
						}
						this.festivalHost = this.getActorByName("Lewis");
						this.hostMessage = "$q -1 null#Well, should we start the dance now?#$r -1 0 yes#Yes, let's start.#$r -1 0 no#Not yet.";
						return;
					}
					else
					{
						if (!(id == "eggFestival"))
						{
							return;
						}
						this.festivalHost = this.getActorByName("Lewis");
						this.hostMessage = "$q -1 null#Do you think everyone's ready for the egg hunt yet?#$r -1 0 yes#Yes, let's start.#$r -1 0 no#Not yet.";
						return;
					}
				}
				else
				{
					if (!(id == "eggHunt"))
					{
						return;
					}
					this.festivalTexture = this.temporaryContent.Load<Texture2D>("Maps\\Festivals");
					for (int x = 0; x < Game1.currentLocation.map.GetLayer("Paths").LayerWidth; x++)
					{
						for (int y = 0; y < Game1.currentLocation.map.GetLayer("Paths").LayerHeight; y++)
						{
							if (Game1.currentLocation.map.GetLayer("Paths").Tiles[x, y] != null)
							{
								this.festivalProps.Add(new Prop(this.festivalTexture, Game1.currentLocation.map.GetLayer("Paths").Tiles[x, y].TileIndex, 1, 1, 1, x, y, true));
							}
						}
					}
					this.festivalTimer = 52000;
					this.currentCommand++;
					return;
				}
			}
			else if (num <= 2614493766u)
			{
				if (num != 2052688871u)
				{
					if (num != 2177915280u)
					{
						if (num != 2614493766u)
						{
							return;
						}
						if (!(id == "iceFishing"))
						{
							return;
						}
						this.festivalTexture = this.temporaryContent.Load<Texture2D>("Maps\\Festivals");
						this.festivalTimer = 120000;
						foreach (Farmer expr_4B6 in this.temporaryLocation.getFarmers())
						{
							expr_4B6.festivalScore = 0;
							expr_4B6.CurrentToolIndex = 0;
							this.tempItemStash = Game1.player.addItemToInventory(new FishingRod(), 0);
							(expr_4B6.CurrentTool as FishingRod).attachments[0] = new Object(690, 99, false, -1, 0);
							(expr_4B6.CurrentTool as FishingRod).attachments[1] = new Object(687, 1, false, -1, 0);
							expr_4B6.CurrentToolIndex = 0;
						}
						return;
					}
					else
					{
						if (!(id == "iceFestival"))
						{
							return;
						}
						this.festivalHost = this.getActorByName("Lewis");
						this.hostMessage = "$q -1 null#Are you ready to participate in the ice fishing competition?#$r -1 0 yes#Yes.#$r -1 0 no#Not yet.";
						return;
					}
				}
				else
				{
					if (!(id == "haleyBeach"))
					{
						return;
					}
					this.playerControlTargetTile = new Point(53, 8);
					this.props.Add(new Object(new Vector2(53f, 8f), 742, 1)
					{
						flipped = false
					});
					return;
				}
			}
			else if (num != 3356754971u)
			{
				if (num != 3623672272u)
				{
					if (num != 3776204284u)
					{
						return;
					}
					if (!(id == "luau"))
					{
						return;
					}
					this.festivalHost = this.getActorByName("Lewis");
					this.hostMessage = "$q -1 null#Should we move forward with the Luau? The governer seems a little hungry.#$r -1 0 yes#Yes, let's start.#$r -1 0 no#Not yet.";
					return;
				}
				else
				{
					if (!(id == "halloween"))
					{
						return;
					}
					this.temporaryLocation.objects.Add(new Vector2(33f, 13f), new Chest(0, new List<Item>
					{
						new Object(373, 1, false, -1, 0)
					}, new Vector2(33f, 13f), false));
					return;
				}
			}
			else
			{
				if (!(id == "jellies"))
				{
					return;
				}
				this.festivalTexture = this.temporaryContent.Load<Texture2D>("Maps\\Festivals");
				this.festivalHost = this.getActorByName("Lewis");
				this.hostMessage = "$q -1 null#What do you think... should I launch the boat now?#$r -1 0 yes#Yes.#$r -1 0 no#Not yet.";
				return;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000275B0 File Offset: 0x000257B0
		public bool canMoveAfterDialogue()
		{
			if (this.playerControlSequenceID != null && this.playerControlSequenceID.Equals("eggHunt"))
			{
				Game1.player.canMove = true;
				int num = this.CurrentCommand;
				this.CurrentCommand = num + 1;
			}
			return this.playerControlSequence;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000275F8 File Offset: 0x000257F8
		public void forceFestivalContinue()
		{
			if (this.FestivalName.Equals("Stardew Valley Fair"))
			{
				this.initiateGrangeJudging();
				return;
			}
			Game1.dialogueUp = false;
			if (Game1.activeClickableMenu != null)
			{
				Game1.activeClickableMenu.emergencyShutDown();
			}
			Game1.exitActiveMenu();
			string[] newCommands = this.festivalData["mainEvent"].Split(new char[]
			{
				'/'
			});
			this.eventCommands = newCommands;
			this.CurrentCommand = 0;
			this.eventSwitched = true;
			this.playerControlSequence = false;
			this.setUpFestivalMainEvent();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00027680 File Offset: 0x00025880
		public void setUpFestivalMainEvent()
		{
			if (this.festivalData["name"].Equals("Flower Dance"))
			{
				List<string> females = new List<string>();
				List<string> males = new List<string>();
				List<string> leftoverFemales = new List<string>
				{
					"Abigail",
					"Penny",
					"Leah",
					"Maru",
					"Haley",
					"Emily"
				};
				List<string> leftoverMales = new List<string>
				{
					"Sebastian",
					"Sam",
					"Elliott",
					"Harvey",
					"Alex",
					"Shane"
				};
				for (int i = 0; i < Game1.numberOfPlayers(); i++)
				{
					Farmer f = Utility.getFarmerFromFarmerNumber(i + 1);
					if (f.dancePartner != null)
					{
						if (f.dancePartner.gender == 1)
						{
							females.Add(f.dancePartner.name);
							leftoverFemales.Remove(f.dancePartner.name);
							males.Add("farmer" + (i + 1));
						}
						else
						{
							males.Add(f.dancePartner.name);
							leftoverMales.Remove(f.dancePartner.name);
							females.Add("farmer" + (i + 1));
						}
					}
				}
				while (females.Count < 6)
				{
					string female = leftoverFemales.Last<string>();
					if (leftoverMales.Contains(Utility.getLoveInterest(female)))
					{
						females.Add(female);
						males.Add(Utility.getLoveInterest(female));
					}
					leftoverFemales.Remove(female);
				}
				string rawFestivalData = this.festivalData["mainEvent"];
				for (int j = 1; j <= 6; j++)
				{
					rawFestivalData = rawFestivalData.Replace("Girl" + j, females[j - 1]);
					rawFestivalData = rawFestivalData.Replace("Guy" + j, males[j - 1]);
				}
				Regex arg_270_0 = new Regex("showFrame (?<farmerName>farmer\\d) 44");
				Regex showFrameGirl = new Regex("showFrame (?<farmerName>farmer\\d) 40");
				Regex animation1Guy = new Regex("animate (?<farmerName>farmer\\d) false true 600 44 45");
				Regex animation1Girl = new Regex("animate (?<farmerName>farmer\\d) false true 600 43 41 43 42");
				Regex animation2Guy = new Regex("animate (?<farmerName>farmer\\d) false true 300 46 47");
				Regex animation2Girl = new Regex("animate (?<farmerName>farmer\\d) false true 600 46 47");
				rawFestivalData = arg_270_0.Replace(rawFestivalData, "showFrame $1 12/faceDirection $1 0");
				rawFestivalData = showFrameGirl.Replace(rawFestivalData, "showFrame $1 0/faceDirection $1 2");
				rawFestivalData = animation1Guy.Replace(rawFestivalData, "animate $1 false true 600 12 13 12 14");
				rawFestivalData = animation1Girl.Replace(rawFestivalData, "animate $1 false true 596 4 0");
				rawFestivalData = animation2Guy.Replace(rawFestivalData, "animate $1 false true 150 12 13 12 14");
				rawFestivalData = animation2Girl.Replace(rawFestivalData, "animate $1 false true 600 0 3");
				string[] newCommands = rawFestivalData.Split(new char[]
				{
					'/'
				});
				this.eventCommands = newCommands;
			}
		}

		// Token: 0x1700001B RID: 27
		public string FestivalName
		{
			// Token: 0x060001BE RID: 446 RVA: 0x00027970 File Offset: 0x00025B70
			get
			{
				if (this.festivalData == null)
				{
					return "";
				}
				return this.festivalData["name"];
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00027990 File Offset: 0x00025B90
		private void lewisDoneJudgingGrange()
		{
			int pointsEarned = 14;
			Dictionary<int, bool> categoriesRepresented = new Dictionary<int, bool>();
			int nullsCount = 0;
			bool purpleShorts = false;
			if (this.grangeDisplay != null)
			{
				foreach (Item i in this.grangeDisplay)
				{
					if (i != null && i is Object)
					{
						if ((i as Object).parentSheetIndex == 789)
						{
							purpleShorts = true;
						}
						pointsEarned += (i as Object).quality + 1;
						int expr_78 = (i as Object).sellToStorePrice();
						if (expr_78 >= 20)
						{
							pointsEarned++;
						}
						if (expr_78 >= 90)
						{
							pointsEarned++;
						}
						if (expr_78 >= 200)
						{
							pointsEarned++;
						}
						if (expr_78 >= 300 && (i as Object).quality < 2)
						{
							pointsEarned++;
						}
						if (expr_78 >= 400 && (i as Object).quality < 1)
						{
							pointsEarned++;
						}
						int category = (i as Object).Category;
						if (category <= -27)
						{
							switch (category)
							{
							case -81:
							case -80:
								break;
							case -79:
								categoriesRepresented[-79] = true;
								continue;
							case -78:
							case -77:
							case -76:
								continue;
							case -75:
								categoriesRepresented[-75] = true;
								continue;
							default:
								if (category != -27)
								{
									continue;
								}
								break;
							}
							categoriesRepresented[-81] = true;
						}
						else if (category != -26)
						{
							if (category != -18)
							{
								switch (category)
								{
								case -14:
								case -6:
								case -5:
									break;
								case -13:
								case -11:
								case -10:
								case -9:
								case -8:
								case -3:
									continue;
								case -12:
								case -2:
									categoriesRepresented[-12] = true;
									continue;
								case -7:
									categoriesRepresented[-7] = true;
									continue;
								case -4:
									categoriesRepresented[-4] = true;
									continue;
								default:
									continue;
								}
							}
							categoriesRepresented[-5] = true;
						}
						else
						{
							categoriesRepresented[-26] = true;
						}
					}
					else if (i == null)
					{
						nullsCount++;
					}
				}
			}
			pointsEarned += Math.Min(30, categoriesRepresented.Count * 5);
			int displayFilledPoints = 9 - 2 * nullsCount;
			if (this.grangeDisplay == null)
			{
				displayFilledPoints = 0;
			}
			pointsEarned += displayFilledPoints;
			this.grangeScore = pointsEarned;
			if (purpleShorts)
			{
				this.grangeScore = -666;
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendMessageToEveryone(4, string.Concat(this.grangeScore), Game1.player.uniqueMultiplayerID);
				Game1.ChatBox.receiveChatMessage("Your grange display has been judged. Return to Mayor Lewis for the result!", -1L);
				Game1.player.Halt();
			}
			else if (Game1.activeClickableMenu == null)
			{
				Game1.drawObjectDialogue(Game1.parseText("Your grange display has been judged. Return to Mayor Lewis for the result!"));
				Game1.player.Halt();
			}
			this.interpretGrangeResults();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00027C40 File Offset: 0x00025E40
		public void interpretGrangeResults()
		{
			List<Character> winners = new List<Character>();
			winners.Add(this.getActorByName("Pierre"));
			winners.Add(this.getActorByName("Marnie"));
			winners.Add(this.getActorByName("Willy"));
			if (this.grangeScore >= 90)
			{
				winners.Insert(0, Game1.player);
			}
			else if (this.grangeScore >= 75)
			{
				winners.Insert(1, Game1.player);
			}
			else if (this.grangeScore >= 60)
			{
				winners.Insert(2, Game1.player);
			}
			else
			{
				winners.Add(Game1.player);
			}
			if (winners[0] is NPC && winners[0].name.Equals("Pierre"))
			{
				this.getActorByName("Pierre").setNewDialogue("It feels great to be on top! Hah!$h#$e#Nice effort, though.", false, false);
			}
			else
			{
				this.getActorByName("Pierre").setNewDialogue("I can't believe I lost...$s", false, false);
			}
			this.getActorByName("Marnie").setNewDialogue("Well, I didn't win... but it was still satisfying to share all my hard work!", false, false);
			this.getActorByName("Willy").setNewDialogue("Wow, I got a low score. I guess Mayor Lewis isn't much of a fish man. Oh well...$s", false, false);
			if (this.grangeScore == -666)
			{
				NPC marnie = this.getActorByName("Marnie");
				if (marnie != null)
				{
					marnie.setNewDialogue("That was some strange-looking purple lettuce in your grange display!$h#$b#I swear, It looked just like Mayor Lewis' special und... $3#$b#Oh!... heh... nevermind!$4", false, false);
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00027D88 File Offset: 0x00025F88
		private void initiateGrangeJudging()
		{
			this.hostMessage = null;
			this.setUpAdvancedMove("advancedMove Lewis False 2 0 0 7 8 0 4 3000 3 0 4 3000 3 0 4 3000 3 0 4 3000 -14 0 2 1000".Split(new char[]
			{
				' '
			}), new NPCController.endBehavior(this.lewisDoneJudgingGrange));
			this.getActorByName("Lewis").CurrentDialogue.Clear();
			this.setUpAdvancedMove("advancedMove Marnie False 0 1 4 1000".Split(new char[]
			{
				' '
			}), null);
			this.getActorByName("Marnie").setNewDialogue("Well, here goes nothing...", false, false);
			this.getActorByName("Pierre").setNewDialogue("Hey, good luck.", false, false);
			this.getActorByName("Willy").setNewDialogue("I don't have high hopes of winning... the fish look nice but they don't exactly smell great...$s", false, false);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00027E3C File Offset: 0x0002603C
		public void answerDialogueQuestion(NPC who, string answerKey)
		{
			if (this.isFestival)
			{
				if (!(answerKey == "yes"))
				{
					if (!(answerKey == "no"))
					{
						if (!(answerKey == "danceAsk"))
						{
							return;
						}
						if (Game1.player.spouse != null && who.name.Equals(Game1.player.spouse))
						{
							Game1.player.dancePartner = who;
							who.hasPartnerForDance = true;
							string spouse = Game1.player.spouse;
							uint num = <PrivateImplementationDetails>.ComputeStringHash(spouse);
							if (num <= 1866496948u)
							{
								if (num <= 1067922812u)
								{
									if (num != 161540545u)
									{
										if (num != 587846041u)
										{
											if (num == 1067922812u)
											{
												if (spouse == "Sam")
												{
													who.setNewDialogue("Aw, I gotta put on that dorky suit again? I thought now that we're married... Well, alright.$s", false, false);
													goto IL_398;
												}
											}
										}
										else if (spouse == "Penny")
										{
											who.setNewDialogue("I accept!$h#$b#*whisper* Thanks for going through with the formalities, dear.", false, false);
											goto IL_398;
										}
									}
									else if (spouse == "Sebastian")
									{
										who.setNewDialogue("Ugh... do we have to? Alright.", false, false);
										goto IL_398;
									}
								}
								else if (num != 1281010426u)
								{
									if (num != 1708213605u)
									{
										if (num == 1866496948u)
										{
											if (spouse == "Shane")
											{
												who.setNewDialogue(Game1.content.LoadString("Strings\\Events:SpouseFlowerDanceAccept_Shane", new object[0]), false, false);
												goto IL_398;
											}
										}
									}
									else if (spouse == "Alex")
									{
										who.setNewDialogue("Okay, this should be fun... I hope I can remember the moves this year! It's been a while...$h", false, false);
										goto IL_398;
									}
								}
								else if (spouse == "Maru")
								{
									who.setNewDialogue("Of course... *whisper* Then can we go home and snuggle?$l", false, false);
									goto IL_398;
								}
							}
							else if (num <= 2571828641u)
							{
								if (num != 2010304804u)
								{
									if (num != 2434294092u)
									{
										if (num == 2571828641u)
										{
											if (spouse == "Emily")
											{
												who.setNewDialogue(Game1.content.LoadString("Strings\\Events:SpouseFlowerDanceAccept_Emily", new object[0]), false, false);
												goto IL_398;
											}
										}
									}
									else if (spouse == "Haley")
									{
										who.setNewDialogue("*sigh*... My days of being Flower Queen are over... so it's a bittersweet dance for me.$s#$b#But, yes...", false, false);
										goto IL_398;
									}
								}
								else if (spouse == "Harvey")
								{
									who.setNewDialogue("I've been looking forward to it all afternoon! You look radiant in the fresh spring air, my love.$l", false, false);
									goto IL_398;
								}
							}
							else if (num != 2732913340u)
							{
								if (num != 2826247323u)
								{
									if (num == 3066176300u)
									{
										if (spouse == "Elliott")
										{
											who.setNewDialogue("Yes... could I refuse that soft, kind face? The touch of spring-time's sweet embrace?", false, false);
											goto IL_398;
										}
									}
								}
								else if (spouse == "Leah")
								{
									who.setNewDialogue("Finally! I was starting to get worried you wanted to dance with someone else!$h", false, false);
									goto IL_398;
								}
							}
							else if (spouse == "Abigail")
							{
								who.setNewDialogue("B... But... I wanted to dance with...$s#$b#Just kidding! Of course I'm dancing with you. I love you.$h", false, false);
								goto IL_398;
							}
							who.setNewDialogue("Of course I'll dance with you!$h", false, false);
							IL_398:
							using (List<NPC>.Enumerator enumerator = this.actors.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									NPC i = enumerator.Current;
									if (i.CurrentDialogue != null && i.CurrentDialogue.Count > 0 && i.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
									{
										i.CurrentDialogue.Clear();
									}
								}
								goto IL_57D;
							}
						}
						if (!who.hasPartnerForDance && Game1.player.getFriendshipLevelForNPC(who.name) >= 1000)
						{
							string accept = "";
							int gender = who.gender;
							if (gender != 0)
							{
								if (gender == 1)
								{
									accept = "You want to be my partner for the flower dance?#$b#Okay! I'd love to.$h";
								}
							}
							else
							{
								accept = "You want to be my partner for the flower dance?#$b#Okay. I look forward to it.$h";
							}
							if (Game1.IsMultiplayer)
							{
								MultiplayerUtility.sendMessageToEveryone(0, who.name, Game1.player.uniqueMultiplayerID);
							}
							try
							{
								Game1.player.changeFriendship(250, Game1.getCharacterFromName(who.name, false));
							}
							catch (Exception)
							{
							}
							if (!Game1.IsMultiplayer || Game1.IsServer)
							{
								Game1.player.dancePartner = who;
								who.hasPartnerForDance = true;
							}
							who.setNewDialogue(accept, false, false);
							using (List<NPC>.Enumerator enumerator = this.actors.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									NPC j = enumerator.Current;
									if (j.CurrentDialogue != null && j.CurrentDialogue.Count > 0 && j.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
									{
										j.CurrentDialogue.Clear();
									}
								}
								goto IL_57D;
							}
						}
						if (who.hasPartnerForDance)
						{
							who.setNewDialogue("I'm sorry... I already have a partner.", false, false);
						}
						else
						{
							try
							{
								who.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + who.name)["danceRejection"], false, false);
							}
							catch (Exception)
							{
								return;
							}
						}
						IL_57D:
						Game1.drawDialogue(who);
						who.immediateSpeak = true;
						who.facePlayer(Game1.player);
						who.Halt();
					}
				}
				else if (this.FestivalName.Equals("Stardew Valley Fair"))
				{
					this.initiateGrangeJudging();
					if (Game1.IsServer)
					{
						MultiplayerUtility.sendServerToClientsMessage("festivalEvent");
						return;
					}
				}
				else
				{
					string[] newCommands = this.festivalData["mainEvent"].Split(new char[]
					{
						'/'
					});
					this.eventCommands = newCommands;
					this.CurrentCommand = 0;
					this.eventSwitched = true;
					this.playerControlSequence = false;
					this.setUpFestivalMainEvent();
					if (Game1.IsServer)
					{
						MultiplayerUtility.sendServerToClientsMessage("festivalEvent");
						return;
					}
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00028418 File Offset: 0x00026618
		public void addItemToGrangeDisplay(Item i, int position, bool force)
		{
			if (this.grangeDisplay == null)
			{
				this.grangeDisplay = new List<Item>();
				for (int j = 0; j < 9; j++)
				{
					this.grangeDisplay.Add(null);
				}
			}
			if (position < 0 || position >= this.grangeDisplay.Count || (this.grangeDisplay[position] != null && !force))
			{
				return;
			}
			this.grangeDisplay[position] = i;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00028484 File Offset: 0x00026684
		public void setGrangeDisplayUser(Farmer who)
		{
			this.playerUsingGrangeDisplay = who;
			if (who != null && who.IsMainPlayer)
			{
				who.Halt();
				who.movementDirections.Clear();
				Game1.activeClickableMenu = new StorageContainer(this.grangeDisplay, 9, 3, new StorageContainer.behaviorOnItemChange(this.onGrangeChange), new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects));
				this.waitingForMenuClose = true;
			}
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendMessageToEveryone(2, (who != null) ? who.name : "null null", (who != null) ? who.uniqueMultiplayerID : 0L);
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00028510 File Offset: 0x00026710
		private bool onGrangeChange(Item i, int position, Item old, StorageContainer container, bool onRemoval)
		{
			if (!onRemoval)
			{
				if (i.Stack > 1 || (i.Stack == 1 && old != null && old.Stack == 1 && i.canStackWith(old)))
				{
					if (old != null && i != null && old.canStackWith(i))
					{
						container.ItemsToGrabMenu.actualInventory[position].Stack = 1;
						container.heldItem = old;
						return false;
					}
					if (old != null)
					{
						Utility.addItemToInventory(old, position, container.ItemsToGrabMenu.actualInventory, null);
						container.heldItem = i;
						return false;
					}
					int allButOne = i.Stack - 1;
					Item reject = i.getOne();
					reject.Stack = allButOne;
					container.heldItem = reject;
					i.Stack = 1;
				}
			}
			else if (old != null && old.Stack > 1 && !old.Equals(i))
			{
				return false;
			}
			if (Game1.IsMultiplayer)
			{
				if (onRemoval && old == null)
				{
					MultiplayerUtility.sendMessageToEveryone(3, position + " null null", Game1.player.uniqueMultiplayerID);
				}
				else
				{
					MultiplayerUtility.sendMessageToEveryone(3, string.Concat(new object[]
					{
						position,
						" ",
						(i as Object).ParentSheetIndex,
						" ",
						(i as Object).quality
					}), Game1.player.uniqueMultiplayerID);
				}
			}
			else
			{
				this.addItemToGrangeDisplay((onRemoval && (old == null || old.Equals(i))) ? null : i, position, true);
			}
			return true;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00028695 File Offset: 0x00026895
		public bool canPlayerUseTool()
		{
			if (this.FestivalName.Equals("Festival of Ice") && this.festivalTimer > 0 && !Game1.player.usingTool)
			{
				this.previousFacingDirection = Game1.player.FacingDirection;
				return true;
			}
			return false;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000286D4 File Offset: 0x000268D4
		public bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (this.isFestival)
			{
				if (this.temporaryLocation != null && this.temporaryLocation.objects.ContainsKey(new Vector2((float)tileLocation.X, (float)tileLocation.Y)))
				{
					this.temporaryLocation.objects[new Vector2((float)tileLocation.X, (float)tileLocation.Y)].checkForAction(who, false);
				}
				int tileIndex = Game1.currentLocation.getTileIndexAt(tileLocation.X, tileLocation.Y, "Buildings");
				string tileAction = Game1.currentLocation.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
				bool success = true;
				if (tileIndex <= 176)
				{
					if (tileIndex <= 88)
					{
						if (tileIndex == 87 || tileIndex == 88)
						{
							Response[] responses = new Response[]
							{
								new Response("Buy", "Buy"),
								new Response("Leave", "Leave")
							};
							if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
							{
								Game1.currentLocation.createQuestionDialogue("Selling Star Tokens for just 50g a piece!", responses, "StarTokenShop");
							}
							goto IL_66D;
						}
					}
					else if (tileIndex == 175 || tileIndex == 176)
					{
						if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
						{
							Game1.playerEatObject(new Object(241, 1, false, -1, 0), true);
						}
						goto IL_66D;
					}
				}
				else if (tileIndex <= 309)
				{
					if (tileIndex == 308 || tileIndex == 309)
					{
						Response[] colors = new Response[]
						{
							new Response("Orange", "Orange"),
							new Response("Green", "Green"),
							new Response("I", "I don't want to play")
						};
						if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
						{
							Game1.currentLocation.createQuestionDialogue(Game1.parseText("Step right up! Pick a color and place your bet for a chance to double your wager!"), colors, "wheelBet");
						}
						goto IL_66D;
					}
				}
				else
				{
					switch (tileIndex)
					{
					case 349:
					case 350:
					case 351:
						if (!this.FestivalName.Equals("Stardew Valley Fair"))
						{
							goto IL_66D;
						}
						if (this.grangeDisplay == null)
						{
							this.grangeDisplay = new List<Item>();
							for (int i = 0; i < 9; i++)
							{
								this.grangeDisplay.Add(null);
							}
						}
						if (this.playerUsingGrangeDisplay == null)
						{
							if (!Game1.IsMultiplayer)
							{
								Game1.activeClickableMenu = new StorageContainer(this.grangeDisplay.ToList<Item>(), 9, 3, new StorageContainer.behaviorOnItemChange(this.onGrangeChange), new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects));
								this.waitingForMenuClose = true;
								goto IL_66D;
							}
							if (Game1.IsServer)
							{
								this.setGrangeDisplayUser(who);
							}
							goto IL_66D;
						}
						else
						{
							if (who.IsMainPlayer)
							{
								Game1.drawObjectDialogue(Game1.parseText("Someone else is using that right now."));
							}
							goto IL_66D;
						}
						break;
					default:
						switch (tileIndex)
						{
						case 501:
						case 502:
						{
							Response[] responses2 = new Response[]
							{
								new Response("Play", "Play (50g)"),
								new Response("Leave", "Leave")
							};
							if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
							{
								Game1.currentLocation.createQuestionDialogue("Play the slingshot game and win star tokens!", responses2, "slingshotGame");
							}
							goto IL_66D;
						}
						case 503:
						case 504:
						{
							Response[] responses3 = new Response[]
							{
								new Response("Play", "Play (50g)"),
								new Response("Leave", "Leave")
							};
							if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
							{
								Game1.currentLocation.createQuestionDialogue("Try your hand at some fishing? You could win big.", responses3, "fishingGame");
							}
							goto IL_66D;
						}
						case 505:
						case 506:
							if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
							{
								if (who.Money >= 100 && !who.mailReceived.Contains("fortuneTeller" + Game1.year))
								{
									Response[] responses4 = new Response[]
									{
										new Response("Read", "Read my fortune (100g)"),
										new Response("No", "No Thanks")
									};
									Game1.currentLocation.createQuestionDialogue(Game1.parseText("Ah, yes... my crystal ball is swirling with visions of your future, young one."), responses4, "fortuneTeller");
								}
								else if (who.mailReceived.Contains("fortuneTeller" + Game1.year))
								{
									Game1.drawObjectDialogue(Game1.parseText("I've already read your fortune... go away."));
								}
								else
								{
									Game1.drawObjectDialogue(Game1.parseText("You don't have enough money to pay my fee... What a shame."));
								}
								who.Halt();
							}
							goto IL_66D;
						case 507:
						case 508:
						case 509:
							break;
						case 510:
						case 511:
							if (who.IsMainPlayer && this.FestivalName.Equals("Stardew Valley Fair"))
							{
								if (this.festivalShops == null)
								{
									this.festivalShops = new Dictionary<string, Dictionary<Item, int[]>>();
								}
								if (!this.festivalShops.ContainsKey("starTokenShop"))
								{
									Dictionary<Item, int[]> stock = new Dictionary<Item, int[]>();
									stock.Add(new Furniture(1307, Vector2.Zero), new int[]
									{
										100,
										1
									});
									stock.Add(new Hat(19), new int[]
									{
										500,
										1
									});
									stock.Add(new Object(Vector2.Zero, 110, false), new int[]
									{
										800,
										1
									});
									if (!Game1.player.mailReceived.Contains("CF_Fair"))
									{
										stock.Add(new Object(434, 1, false, -1, 0), new int[]
										{
											2000,
											1
										});
									}
									this.festivalShops.Add("starTokenShop", stock);
								}
								Game1.currentLocation.createQuestionDialogue("Trade in your star tokens for prizes?", Game1.currentLocation.createYesNoResponses(), "starTokenShop");
							}
							goto IL_66D;
						default:
							if (tileIndex == 540)
							{
								if (!who.IsMainPlayer || !this.FestivalName.Equals("Stardew Valley Fair"))
								{
									goto IL_66D;
								}
								if (who.getTileX() == 29)
								{
									Game1.activeClickableMenu = new StrengthGame();
									goto IL_66D;
								}
								Game1.drawObjectDialogue(Game1.parseText("Please stand at the red arrow."));
								goto IL_66D;
							}
							break;
						}
						break;
					}
				}
				success = false;
				IL_66D:
				if (success)
				{
					return true;
				}
				if (tileAction != null)
				{
					try
					{
						string[] split = tileAction.Split(new char[]
						{
							' '
						});
						string a = split[0];
						if (!(a == "Shop"))
						{
							if (!(a == "Message") && !(a == "Dialogue"))
							{
								if (a == "LuauSoup")
								{
									if (!this.specialEventVariable2)
									{
										Game1.activeClickableMenu = new ItemGrabMenu(null, true, false, new InventoryMenu.highlightThisItem(Utility.highlightEdibleNonCookingItems), new ItemGrabMenu.behaviorOnItemSelect(this.clickToAddItemToLuauSoup), "Select an ingredient to add.", null, false, false, true, true, false, 0, null, -1, null);
									}
								}
							}
							else
							{
								Game1.drawObjectDialogue(Game1.currentLocation.actionParamsToString(split).Replace("#", " "));
							}
						}
						else
						{
							if (!who.IsMainPlayer)
							{
								bool result = false;
								return result;
							}
							if (this.festivalShops == null)
							{
								this.festivalShops = new Dictionary<string, Dictionary<Item, int[]>>();
							}
							Dictionary<Item, int[]> stockList;
							if (!this.festivalShops.ContainsKey(split[1]))
							{
								string[] inventoryList = this.festivalData[split[1]].Split(new char[]
								{
									' '
								});
								stockList = new Dictionary<Item, int[]>();
								int infiniteStock = 2147483647;
								int j = 0;
								while (j < inventoryList.Length)
								{
									string type = inventoryList[j];
									int index = Convert.ToInt32(inventoryList[j + 1]);
									int price = Convert.ToInt32(inventoryList[j + 2]);
									int stock2 = Convert.ToInt32(inventoryList[j + 3]);
									Item item = null;
									uint num = <PrivateImplementationDetails>.ComputeStringHash(type);
									if (num <= 2707948032u)
									{
										if (num <= 1430892386u)
										{
											if (num <= 568155902u)
											{
												if (num != 551378283u)
												{
													if (num != 568155902u)
													{
														goto IL_A84;
													}
													if (!(type == "BO"))
													{
														goto IL_A84;
													}
												}
												else
												{
													if (!(type == "BL"))
													{
														goto IL_A84;
													}
													goto IL_A5B;
												}
											}
											else if (num != 930363637u)
											{
												if (num != 1430892386u)
												{
													goto IL_A84;
												}
												if (!(type == "Hat"))
												{
													goto IL_A84;
												}
												goto IL_A6A;
											}
											else
											{
												if (!(type == "Boot"))
												{
													goto IL_A84;
												}
												goto IL_A45;
											}
										}
										else if (num <= 2089749334u)
										{
											if (num != 2005354379u)
											{
												if (num != 2089749334u)
												{
													goto IL_A84;
												}
												if (!(type == "BigObject"))
												{
													goto IL_A84;
												}
											}
											else
											{
												if (!(type == "Ring"))
												{
													goto IL_A84;
												}
												goto IL_A3A;
											}
										}
										else if (num != 2675227691u)
										{
											if (num != 2707948032u)
											{
												goto IL_A84;
											}
											if (!(type == "Blueprint"))
											{
												goto IL_A84;
											}
											goto IL_A5B;
										}
										else
										{
											if (!(type == "BBL"))
											{
												goto IL_A84;
											}
											goto IL_A75;
										}
										item = new Object(Vector2.Zero, index, false);
										goto IL_A84;
										IL_A5B:
										item = new Object(index, 1, true, -1, 0);
									}
									else
									{
										if (num <= 3389784126u)
										{
											if (num <= 3212111499u)
											{
												if (num != 3082879841u)
												{
													if (num != 3212111499u)
													{
														goto IL_A84;
													}
													if (!(type == "BBl"))
													{
														goto IL_A84;
													}
													goto IL_A75;
												}
												else
												{
													if (!(type == "Weapon"))
													{
														goto IL_A84;
													}
													goto IL_A50;
												}
											}
											else if (num != 3339451269u)
											{
												if (num != 3389784126u)
												{
													goto IL_A84;
												}
												if (!(type == "O"))
												{
													goto IL_A84;
												}
											}
											else
											{
												if (!(type == "B"))
												{
													goto IL_A84;
												}
												goto IL_A45;
											}
										}
										else if (num <= 3524005078u)
										{
											if (num != 3440116983u)
											{
												if (num != 3524005078u)
												{
													goto IL_A84;
												}
												if (!(type == "W"))
												{
													goto IL_A84;
												}
												goto IL_A50;
											}
											else
											{
												if (!(type == "H"))
												{
													goto IL_A84;
												}
												goto IL_A6A;
											}
										}
										else if (num != 3579274004u)
										{
											if (num != 3607893173u)
											{
												if (num != 3851314394u)
												{
													goto IL_A84;
												}
												if (!(type == "Object"))
												{
													goto IL_A84;
												}
											}
											else
											{
												if (!(type == "R"))
												{
													goto IL_A84;
												}
												goto IL_A3A;
											}
										}
										else
										{
											if (!(type == "BigBlueprint"))
											{
												goto IL_A84;
											}
											goto IL_A75;
										}
										item = new Object(index, 1, false, -1, 0);
										goto IL_A84;
										IL_A50:
										item = new MeleeWeapon(index);
									}
									IL_A84:
									if ((!(item is Object) || !(item as Object).isRecipe || !who.knowsRecipe(item.Name)) && item != null)
									{
										stockList.Add(item, new int[]
										{
											price,
											(stock2 <= 0) ? infiniteStock : stock2
										});
									}
									j += 4;
									continue;
									IL_A3A:
									item = new Ring(index);
									goto IL_A84;
									IL_A45:
									item = new Boots(index);
									goto IL_A84;
									IL_A6A:
									item = new Hat(index);
									goto IL_A84;
									IL_A75:
									item = new Object(Vector2.Zero, index, true);
									goto IL_A84;
								}
								this.festivalShops.Add(split[1], stockList);
							}
							else
							{
								stockList = this.festivalShops[split[1]];
							}
							if (stockList != null && stockList.Count > 0)
							{
								Game1.activeClickableMenu = new ShopMenu(stockList, 0, null);
							}
							else
							{
								Game1.drawObjectDialogue("Out of Stock.");
							}
						}
						return false;
					}
					catch (Exception)
					{
						return false;
					}
				}
				if (!this.isFestival)
				{
					return false;
				}
				if (who.IsMainPlayer)
				{
					foreach (NPC k in this.actors)
					{
						if (k.getTileX() == tileLocation.X && k.getTileY() == tileLocation.Y && (k.CurrentDialogue.Count >= 1 || (k.CurrentDialogue.Count > 0 && !k.CurrentDialogue.Peek().isOnFinalDialogue()) || (k.Equals(this.festivalHost) || (k.datable && this.festivalData["name"].Equals("Flower Dance"))) || (this.secretSantaRecipient != null && k.name.Equals(this.secretSantaRecipient.name))))
						{
							if ((this.grangeScore > -100 || this.grangeScore == -666) && k.Equals(this.festivalHost))
							{
								string message;
								if (this.grangeScore >= 90)
								{
									Game1.playSound("reward");
									message = "Congratulations! You won 1st place with a rating of " + this.grangeScore + "!#$b#Your prize is 1000 star tokens! Spend them wisely.$h#$b#Oh, and don't forget to clean out your grange display box.";
									Game1.player.festivalScore += 1000;
								}
								else if (this.grangeScore >= 75)
								{
									Game1.playSound("reward");
									message = "Hey, not bad! You won 2nd place with a rating of " + this.grangeScore + ".#$b#Your prize is 500 star tokens! Spend them wisely.$h#$b#Oh, and don't forget to clean out your grange display box.";
									Game1.player.festivalScore += 500;
								}
								else if (this.grangeScore >= 60)
								{
									Game1.playSound("newArtifact");
									message = "Hi there, @. It looks like you won 3rd place with a rating of " + this.grangeScore + ".#$b#Your prize is 200 star tokens! Spend them wisely.$h#$b#Oh, and don't forget to clean out your grange display box.";
									Game1.player.festivalScore += 250;
								}
								else if (this.grangeScore == -666)
								{
									Game1.playSound("secret1");
									message = "You!!! Was that some kind of sick prank?! Those are very private!$4#$b#Here, take 750 star tokens and don't tell a soul.$3#$b#Now go clean up your box and bring me my... item... tomorrow.$3";
									Game1.player.festivalScore += 750;
								}
								else
								{
									Game1.playSound("newArtifact");
									message = "Hi there, @. You got 4th place with a score of " + this.grangeScore + ".#$b#Your reward for participating is 50 star tokens. Hey, maybe you'll do better next year.$h#$b#Oh, and don't forget to clean out your grange display box.";
									Game1.player.festivalScore += 50;
								}
								this.grangeScore = -100;
								k.setNewDialogue(message, false, false);
							}
							else if ((Game1.serverHost == null || Game1.player.Equals(Game1.serverHost)) && k.Equals(this.festivalHost) && (k.CurrentDialogue.Count == 0 || k.CurrentDialogue.Peek().isOnFinalDialogue()) && this.hostMessage != null)
							{
								k.setNewDialogue(this.hostMessage, false, false);
							}
							bool result;
							if (this.festivalData != null && this.festivalData["name"].Equals("Flower Dance") && (k.datable || (who.spouse != null && k.name.Equals(who.spouse))))
							{
								if (who.dancePartner == null)
								{
									if (k.CurrentDialogue.Count > 0 && k.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
									{
										k.CurrentDialogue.Clear();
									}
									if (k.CurrentDialogue.Count == 0)
									{
										k.CurrentDialogue.Push(new Dialogue("...", k));
										if (k.name.Equals(who.spouse))
										{
											k.setNewDialogue("$q -1 null#...Yes, dear?#$r -1 0 danceAsk#(Ask " + k.name + " to be your dance partner)#$r -1 0 null#Nevermind...", true, false);
										}
										else
										{
											k.setNewDialogue("$q -1 null#...Yes?#$r -1 0 danceAsk#(Ask " + k.name + " to be your dance partner)#$r -1 0 null#Nevermind...", true, false);
										}
									}
									else if (k.CurrentDialogue.Peek().isOnFinalDialogue())
									{
										Dialogue d = k.CurrentDialogue.Peek();
										Game1.drawDialogue(k);
										k.faceTowardFarmerForPeriod(3000, 2, false, who);
										who.Halt();
										k.CurrentDialogue = new Stack<Dialogue>();
										k.CurrentDialogue.Push(new Dialogue("...", k));
										k.CurrentDialogue.Push(d);
										result = true;
										return result;
									}
								}
								else if (k.CurrentDialogue.Count > 0 && k.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
								{
									k.CurrentDialogue.Clear();
								}
							}
							if (this.secretSantaRecipient != null && k.name.Equals(this.secretSantaRecipient.name))
							{
								Game1.currentLocation.createQuestionDialogue(Game1.parseText(string.Concat(new string[]
								{
									"Give ",
									this.secretSantaRecipient.name,
									" ",
									(this.secretSantaRecipient.gender == 0) ? "his" : "her",
									" secret gift?"
								})), Game1.currentLocation.createYesNoResponses(), "secretSanta");
								who.Halt();
								result = true;
								return result;
							}
							if (k.CurrentDialogue.Count == 0)
							{
								result = true;
								return result;
							}
							if (who.spouse != null && k.name.Equals(who.spouse) && !this.FestivalName.Contains("Flower") && this.festivalData.ContainsKey(k.name + "_spouse"))
							{
								k.CurrentDialogue.Clear();
								k.CurrentDialogue.Push(new Dialogue(this.festivalData[k.name + "_spouse"], k));
							}
							Game1.drawDialogue(k);
							k.faceTowardFarmerForPeriod(3000, 2, false, who);
							who.Halt();
							result = true;
							return result;
						}
					}
				}
				if (this.festivalData != null && this.festivalData["name"].Equals("Egg Festival"))
				{
					Microsoft.Xna.Framework.Rectangle tile = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
					for (int l = this.festivalProps.Count - 1; l >= 0; l--)
					{
						if (this.festivalProps[l].isColliding(tile))
						{
							who.festivalScore++;
							this.festivalProps.RemoveAt(l);
							if (who.IsMainPlayer)
							{
								Game1.playSound("coin");
							}
							return false;
						}
					}
					return false;
				}
				return false;
			}
			return false;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00029980 File Offset: 0x00027B80
		public void checkForSpecialCharacterIconAtThisTile(Vector2 tileLocation)
		{
			if (this.isFestival && this.festivalHost != null && this.festivalHost.getTileLocation().Equals(tileLocation))
			{
				Game1.mouseCursor = 4;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000299B9 File Offset: 0x00027BB9
		public void forceEndFestival(Farmer who)
		{
			Game1.currentMinigame = null;
			Game1.exitActiveMenu();
			this.endBehaviors(null, Game1.currentLocation);
			if (Game1.IsServer)
			{
				MultiplayerUtility.sendServerToClientsMessage("endFest");
			}
			Game1.changeMusicTrack("none");
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000299F0 File Offset: 0x00027BF0
		public bool checkForCollision(Microsoft.Xna.Framework.Rectangle position, Farmer who)
		{
			foreach (NPC i in this.actors)
			{
				if (i.GetBoundingBox().Intersects(position) && !Game1.player.temporarilyInvincible && Game1.player.temporaryImpassableTile.Equals(Microsoft.Xna.Framework.Rectangle.Empty) && !i.isInvisible)
				{
					bool result = true;
					return result;
				}
			}
			if (position.X < 0 || position.Y < 0 || position.X >= Game1.currentLocation.map.Layers[0].DisplayWidth || position.Y >= Game1.currentLocation.map.Layers[0].DisplayHeight)
			{
				if (who.IsMainPlayer && this.isFestival && (Game1.IsServer || !Game1.IsMultiplayer) && Game1.activeClickableMenu == null)
				{
					who.Halt();
					who.position = who.lastPosition;
					Game1.activeClickableMenu = new ConfirmationDialog("Leave the " + this.FestivalName + "? Once you do, the festival will end.", new ConfirmationDialog.behavior(this.forceEndFestival), null);
				}
				return true;
			}
			foreach (Object expr_142 in this.props)
			{
				if (expr_142.getBoundingBox(expr_142.tileLocation).Intersects(position))
				{
					bool result = true;
					return result;
				}
			}
			if (this.temporaryLocation != null)
			{
				foreach (Object expr_1A0 in this.temporaryLocation.objects.Values)
				{
					if (expr_1A0.getBoundingBox(expr_1A0.tileLocation).Intersects(position))
					{
						bool result = true;
						return result;
					}
				}
			}
			using (List<Prop>.Enumerator enumerator4 = this.festivalProps.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					if (enumerator4.Current.isColliding(position))
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00029C44 File Offset: 0x00027E44
		public void answerDialogue(string questionKey, int answerChoice)
		{
			this.previousAnswerChoice = answerChoice;
			if (questionKey.Contains("fork"))
			{
				int forkAnswer = Convert.ToInt32(questionKey.Replace("fork", ""));
				if (answerChoice == forkAnswer)
				{
					this.specialEventVariable1 = !this.specialEventVariable1;
					return;
				}
			}
			else
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(questionKey);
				if (num <= 1836559258u)
				{
					if (num <= 390240131u)
					{
						if (num != 119764934u)
						{
							if (num != 269688027u)
							{
								if (num != 390240131u)
								{
									return;
								}
								if (!(questionKey == "shaneCliffs"))
								{
									return;
								}
								switch (answerChoice)
								{
								case 0:
									this.eventCommands[this.currentCommand + 2] = "speak Shane \"Maybe for you, but not for me! You're not going to understand...  Just... go away.$7#$b#...Ugh...$7#$b#Wait...$7\"";
									return;
								case 1:
									this.eventCommands[this.currentCommand + 2] = "speak Shane \"...You're right. Jas... Ugh, God... I'm a horrible, *hic*... selfish person.$7#$b#Now I feel even worse...$7\"";
									return;
								case 2:
									this.eventCommands[this.currentCommand + 2] = "speak Shane \"How is that s... supposed to make me feel better? I don't even *hic* believe in Yoba...$7#$b#Just... go away.$7#$b#...Ugh...$7#$b#Wait...$7\"";
									return;
								case 3:
									this.eventCommands[this.currentCommand + 2] = "speak Shane \"...$7#$b#Thanks... I appreciate that... I really do.$7\"";
									return;
								default:
									return;
								}
							}
							else
							{
								if (!(questionKey == "wheelBet"))
								{
									return;
								}
								this.specialEventVariable2 = (answerChoice == 1);
								if (answerChoice != 2)
								{
									Game1.activeClickableMenu = new NumberSelectionMenu("How many star tokens would you like to wager?", new NumberSelectionMenu.behaviorOnNumberSelect(this.betStarTokens), -1, 1, Game1.player.festivalScore, 1);
									return;
								}
							}
						}
						else
						{
							if (!(questionKey == "shaneLoan"))
							{
								return;
							}
							if (answerChoice != 0)
							{
								return;
							}
							this.specialEventVariable1 = true;
							this.eventCommands[this.currentCommand + 1] = "fork giveShaneLoan";
							Game1.player.money -= 3000;
							return;
						}
					}
					else if (num <= 504494762u)
					{
						if (num != 472382138u)
						{
							if (num != 504494762u)
							{
								return;
							}
							if (!(questionKey == "starTokenShop"))
							{
								return;
							}
							if (answerChoice == 0)
							{
								if (this.festivalShops["starTokenShop"].Count == 0)
								{
									Game1.drawObjectDialogue(Game1.parseText("You've already bought everything in the shop!"));
									return;
								}
								Game1.activeClickableMenu = new ShopMenu(this.festivalShops["starTokenShop"], 1, null);
								return;
							}
						}
						else
						{
							if (!(questionKey == "fortuneTeller"))
							{
								return;
							}
							if (answerChoice == 0)
							{
								Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.readFortune), 0.02f);
								Game1.player.Money -= 100;
								Game1.player.mailReceived.Add("fortuneTeller" + Game1.year);
								return;
							}
						}
					}
					else if (num != 1766558334u)
					{
						if (num != 1836559258u)
						{
							return;
						}
						if (!(questionKey == "secretSanta"))
						{
							return;
						}
						if (answerChoice == 0)
						{
							Game1.activeClickableMenu = new ItemGrabMenu(null, true, false, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), new ItemGrabMenu.behaviorOnItemSelect(this.chooseSecretSantaGift), "Select a gift for " + this.secretSantaRecipient.name + ".", null, false, false, true, true, false, 0, null, -1, null);
							return;
						}
					}
					else
					{
						if (!(questionKey == "pet"))
						{
							return;
						}
						if (answerChoice == 0)
						{
							Game1.activeClickableMenu = new NamingMenu(new NamingMenu.doneNamingBehavior(this.namePet), "Choose a name", Game1.player.isMale ? (Game1.player.catPerson ? "Dudley" : "Yogi") : (Game1.player.catPerson ? "Miso" : "Snooch"));
							return;
						}
						Game1.player.mailReceived.Add("rejectedPet");
						this.eventCommands = new string[2];
						this.eventCommands[1] = "end";
						this.eventCommands[0] = "speak Marnie \"No? Okay... I'll find a different home for the poor thing.$s\"";
						this.currentCommand = 0;
						this.eventSwitched = true;
						this.specialEventVariable1 = true;
					}
				}
				else if (num <= 2337399242u)
				{
					if (num != 2205664227u)
					{
						if (num != 2249818047u)
						{
							if (num != 2337399242u)
							{
								return;
							}
							if (!(questionKey == "StarTokenShop"))
							{
								return;
							}
							if (answerChoice == 0)
							{
								Game1.activeClickableMenu = new NumberSelectionMenu("How many star tokens would you like to buy?", new NumberSelectionMenu.behaviorOnNumberSelect(this.buyStarTokens), 50, 0, 99, 0);
								return;
							}
						}
						else
						{
							if (!(questionKey == "chooseCharacter"))
							{
								return;
							}
							switch (answerChoice)
							{
							case 0:
								this.specialEventVariable1 = true;
								this.eventCommands[this.currentCommand + 1] = "fork warrior";
								return;
							case 1:
								this.specialEventVariable1 = true;
								this.eventCommands[this.currentCommand + 1] = "fork healer";
								return;
							case 2:
								break;
							default:
								return;
							}
						}
					}
					else
					{
						if (!(questionKey == "haleyDarkRoom"))
						{
							return;
						}
						switch (answerChoice)
						{
						case 0:
							this.specialEventVariable1 = true;
							this.eventCommands[this.currentCommand + 1] = "fork decorate";
							return;
						case 1:
							this.specialEventVariable1 = true;
							this.eventCommands[this.currentCommand + 1] = "fork leave";
							return;
						case 2:
							break;
						default:
							return;
						}
					}
				}
				else if (num <= 2900380439u)
				{
					if (num != 2536635992u)
					{
						if (num != 2900380439u)
						{
							return;
						}
						if (!(questionKey == "fishingGame"))
						{
							return;
						}
						if (answerChoice == 0 && Game1.player.Money >= 50)
						{
							Game1.globalFadeToBlack(new Game1.afterFadeFunction(FishingGame.startMe), 0.01f);
							Game1.player.Money -= 50;
							return;
						}
						if (answerChoice == 0 && Game1.player.Money < 50)
						{
							Game1.drawObjectDialogue("You don't have enough money.");
							return;
						}
					}
					else
					{
						if (!(questionKey == "cave"))
						{
							return;
						}
						if (answerChoice == 0)
						{
							Game1.player.caveChoice = 2;
							(Game1.getLocationFromName("FarmCave") as FarmCave).setUpMushroomHouse();
							return;
						}
						Game1.player.caveChoice = 1;
						return;
					}
				}
				else if (num != 3007187074u)
				{
					if (num != 3548149252u)
					{
						return;
					}
					if (!(questionKey == "slingshotGame"))
					{
						return;
					}
					if (answerChoice == 0 && Game1.player.Money >= 50)
					{
						Game1.globalFadeToBlack(new Game1.afterFadeFunction(TargetGame.startMe), 0.01f);
						Game1.player.Money -= 50;
						return;
					}
					if (answerChoice == 0 && Game1.player.Money < 50)
					{
						Game1.drawObjectDialogue("You don't have enough money.");
						return;
					}
				}
				else
				{
					if (!(questionKey == "bandFork"))
					{
						return;
					}
					switch (answerChoice)
					{
					case 76:
						this.specialEventVariable1 = true;
						this.eventCommands[this.currentCommand + 1] = "fork poppy";
						return;
					case 77:
						this.specialEventVariable1 = true;
						this.eventCommands[this.currentCommand + 1] = "fork heavy";
						return;
					case 78:
						this.specialEventVariable1 = true;
						this.eventCommands[this.currentCommand + 1] = "fork techno";
						return;
					case 79:
						this.specialEventVariable1 = true;
						this.eventCommands[this.currentCommand + 1] = "fork honkytonk";
						return;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0002A2F0 File Offset: 0x000284F0
		private void namePet(string name)
		{
			Pet p;
			if (Game1.player.catPerson)
			{
				p = new Cat(68, 13);
			}
			else
			{
				p = new Dog(68, 13);
			}
			p.warpToFarmHouse(Game1.player);
			p.name = name;
			Game1.exitActiveMenu();
			int num = this.CurrentCommand;
			this.CurrentCommand = num + 1;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0002A348 File Offset: 0x00028548
		public void chooseSecretSantaGift(Item i, Farmer who)
		{
			if (i == null)
			{
				return;
			}
			if (i is Object)
			{
				Game1.exitActiveMenu();
				NPC recipient = this.getActorByName(this.secretSantaRecipient.name);
				recipient.faceTowardFarmerForPeriod(15000, 5, false, who);
				recipient.receiveGift(i as Object, who, false, 5f, false);
				recipient.CurrentDialogue.Clear();
				recipient.CurrentDialogue.Push(new Dialogue(string.Concat(new string[]
				{
					"Oh! So it's you?$h#$b#Ah... ",
					Game1.getProperArticleForWord(i.Name),
					" ",
					i.Name,
					"! Thanks."
				}), recipient));
				Game1.drawDialogue(recipient);
				this.secretSantaRecipient = null;
				this.startSecretSantaAfterDialogue = true;
				who.Halt();
				who.completelyStopAnimatingOrDoingAction();
				who.faceGeneralDirection(recipient.position, 0);
				return;
			}
			Game1.drawObjectDialogue("Not a valid gift.");
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0002A42C File Offset: 0x0002862C
		public void perfectFishing()
		{
			if (this.isFestival && Game1.currentMinigame != null && this.festivalData["name"].Equals("Stardew Valley Fair"))
			{
				(Game1.currentMinigame as FishingGame).perfections++;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0002A47C File Offset: 0x0002867C
		public void caughtFish(int whichFish, int size, Farmer who)
		{
			if (this.isFestival)
			{
				if (whichFish != -1 && Game1.currentMinigame != null && this.festivalData["name"].Equals("Stardew Valley Fair"))
				{
					(Game1.currentMinigame as FishingGame).score += ((size > 0) ? (size + 5) : 1);
					if (size > 0)
					{
						(Game1.currentMinigame as FishingGame).fishCaught++;
					}
					Game1.player.FarmerSprite.pauseForSingleAnimation = false;
					Game1.player.FarmerSprite.StopAnimation();
					return;
				}
				if (whichFish != -1 && this.FestivalName.Equals("Festival of Ice"))
				{
					if (size > 0 && who.getTileX() < 79 && who.getTileY() < 43)
					{
						who.festivalScore++;
						Game1.playSound("newArtifact");
					}
					who.forceCanMove();
					if (this.previousFacingDirection != -1)
					{
						who.faceDirection(this.previousFacingDirection);
					}
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0002A578 File Offset: 0x00028778
		public void readFortune()
		{
			Game1.globalFade = true;
			Game1.fadeToBlackAlpha = 1f;
			NPC topRomance = Utility.getTopRomanticInterest(Game1.player);
			NPC topFriend = Utility.getTopNonRomanticInterest(Game1.player);
			int topSkill = Utility.getHighestSkill(Game1.player);
			string[] fortune = new string[5];
			if (topFriend != null && Game1.player.getFriendshipLevelForNPC(topFriend.name) > 100)
			{
				if (Utility.getNumberOfFriendsWithinThisRange(Game1.player, Game1.player.getFriendshipLevelForNPC(topFriend.name) - 100, Game1.player.getFriendshipLevelForNPC(topFriend.name), false) > 3 && Game1.random.NextDouble() < 0.5)
				{
					fortune[0] = "Ahh... I see you in the saloon, surrounded by friends. It doesn't seem like you have any favorites... you're popular with everyone!";
				}
				else
				{
					switch (Game1.random.Next(4))
					{
					case 0:
						fortune[0] = "Ahh... yes. I see you at the beach. There's some kind of event taking place. You walk up to " + topFriend.name + " and say something funny... Hmmm... It seems like you two are good friends.";
						break;
					case 1:
						fortune[0] = string.Concat(new string[]
						{
							"Ahhh... yes. It's ",
							topFriend.name,
							"'s birthday. ",
							(topFriend.gender == 0) ? "He" : "She",
							" thought everyone forgot, but then you show up with a nice gift. What a good friend."
						});
						break;
					case 2:
						fortune[0] = "Hmm... I see you laying on a cot... It looks like a hospital. " + topFriend.name + " is there to keep you company while you recover. What a nice friend.";
						break;
					case 3:
						fortune[0] = string.Concat(new string[]
						{
							"Ahh... Indeed. I see you in a room, having a conversation with a ",
							(topFriend.gender == 0) ? "man." : "lady.",
							"..  Oh! It's ",
							topFriend.name,
							". You seem to be close friends."
						});
						break;
					}
				}
			}
			else
			{
				fortune[0] = "Hmm... I see you sitting in a plush chair by the fire... you're surrounded by luxury, yet filled with lonely desperation.";
			}
			if (topRomance != null && Game1.player.getFriendshipLevelForNPC(topRomance.name) > 250)
			{
				if (Utility.getNumberOfFriendsWithinThisRange(Game1.player, Game1.player.getFriendshipLevelForNPC(topRomance.name) - 100, Game1.player.getFriendshipLevelForNPC(topRomance.name), true) > 2)
				{
					fortune[1] = "...Oh? It seems you'll be leaving more than a few heartbroken... Are you playing games with those who put their trust in you? Hmm... ";
				}
				else
				{
					switch (Game1.random.Next(4))
					{
					case 0:
						fortune[1] = "Now I see you and " + topRomance.name + " in a field of grass. You seem happy.";
						break;
					case 1:
						fortune[1] = "Now I see you and " + topRomance.name + " in a dimly lit room. You look serious... but not unhappy. Something important must be happening.";
						break;
					case 2:
						fortune[1] = string.Concat(new string[]
						{
							"Ooo... It's dark, and I see you and a certain young ",
							(topRomance.gender == 0) ? "man. He" : "lady. She",
							" looks ",
							(topRomance.socialAnxiety == 1) ? "a little bashful, but happy to be with you." : "quite hopeful, and eager to spend time with you.",
							" Hmmm... now what's ",
							(topRomance.gender == 0) ? "this young man's" : "this young lady's",
							" name? ... I believe it starts with ",
							Game1.getProperArticleForWord(topRomance.name),
							" '",
							topRomance.name[0].ToString(),
							"'."
						});
						break;
					case 3:
						fortune[1] = "Interesting... I see you and " + topRomance.name + " working together on a farm. You seem very pleased about something.";
						break;
					}
				}
			}
			else
			{
				fortune[1] = "Now I see you, middle-aged, walking through town at dusk. You pause at a window to see a family having dinner. You hang your head and hurry off into the darkness.";
			}
			switch (topSkill)
			{
			case 0:
				fortune[2] = "The crystal ball has moved on... Now I see you harvesting a plump, ripe melon. You're on a wonderful looking farm, bursting with life!";
				break;
			case 1:
				fortune[2] = "The crystal ball has moved on... Now I see you relaxing on the riverbanks, holding a fishing pole. Oh! Looks like something big is on the line!";
				break;
			case 2:
				fortune[2] = "The crystal ball has moved on... It's you... in the forest. You spot some rare and delicious mushrooms hidden beneath a clump of grass... what a find!";
				break;
			case 3:
				fortune[2] = "The crystal ball has moved on... I see you somewhere dark... but there you are, inspecting a marvelous gemstone! It's glittering in the light of a small lantern.";
				break;
			case 4:
				fortune[2] = "The crystal ball has moved on... AH! You're in combat! There's something dreadful bearing down on you from the dark, but you seem more than ready to face it.";
				break;
			case 5:
				fortune[2] = "The crystal ball has moved on... I see you in a golden room... grinning about something. Oh, I see... You're playing some kind of game and you just can't lose!";
				break;
			}
			fortune[3] = "Ah... the crystal ball has gone dim. That's all I can do for you, young one.";
			fortune[4] = "Now, just keep in mind that the future isn't set in stone! Whatever I've told you today can still be changed, if you set your heart on it. Farewell.";
			Game1.multipleDialogues(fortune);
			Game1.afterDialogues = new Game1.afterFadeFunction(this.fadeClearAndviewportUnfreeze);
			Game1.viewportFreeze = true;
			Game1.viewport.X = -9999;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0002A94D File Offset: 0x00028B4D
		public void fadeClearAndviewportUnfreeze()
		{
			Game1.fadeClear();
			Game1.viewportFreeze = false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0002A95A File Offset: 0x00028B5A
		public void betStarTokens(int value, int price, Farmer who)
		{
			if (value <= who.festivalScore)
			{
				Game1.playSound("smallSelect");
				Game1.activeClickableMenu = new WheelSpinGame(value);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0002A97A File Offset: 0x00028B7A
		public void buyStarTokens(int value, int price, Farmer who)
		{
			if (value > 0 && value * price <= who.Money)
			{
				who.Money -= price * value;
				who.festivalScore += value;
				Game1.playSound("purchase");
				Game1.exitActiveMenu();
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0002A9B8 File Offset: 0x00028BB8
		public void clickToAddItemToLuauSoup(Item i, Farmer who)
		{
			if (!Game1.IsMultiplayer || Game1.IsServer)
			{
				this.addItemToLuauSoup(i, who);
			}
			if (Game1.IsMultiplayer)
			{
				MultiplayerUtility.sendMessageToEveryone(1, (i as Object).parentSheetIndex + " " + (i as Object).quality, who.uniqueMultiplayerID);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0002AA18 File Offset: 0x00028C18
		public void setUpAdvancedMove(string[] split, NPCController.endBehavior endBehavior = null)
		{
			if (this.npcControllers == null)
			{
				this.npcControllers = new List<NPCController>();
			}
			List<Vector2> path = new List<Vector2>();
			for (int i = 3; i < split.Length; i += 2)
			{
				path.Add(new Vector2((float)Convert.ToInt32(split[i]), (float)Convert.ToInt32(split[i + 1])));
			}
			NPC j = this.getActorByName(split[1].Replace('_', ' '));
			if (j == null)
			{
				return;
			}
			this.npcControllers.Add(new NPCController(j, path, Convert.ToBoolean(split[2]), endBehavior));
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0002AAA0 File Offset: 0x00028CA0
		public void addItemToLuauSoup(Item i, Farmer who)
		{
			if (i == null)
			{
				return;
			}
			if (this.luauIngredients == null)
			{
				this.luauIngredients = new List<Item>();
			}
			this.luauIngredients.Add(i);
			if (who.IsMainPlayer)
			{
				this.specialEventVariable2 = true;
				if (i != null && i.Stack > 1)
				{
					int stack = i.Stack;
					i.Stack = stack - 1;
					who.addItemToInventory(i);
				}
				Game1.exitActiveMenu();
				Game1.playSound("dropItemInWater");
				if (i != null)
				{
					Game1.drawObjectDialogue(string.Concat(new string[]
					{
						"You added ",
						Game1.getProperArticleForWord(i.Name),
						" ",
						i.Name,
						" to the soup."
					}));
					return;
				}
			}
			else
			{
				Game1.ChatBox.receiveChatMessage(string.Concat(new string[]
				{
					who.Name,
					" added ",
					Game1.getProperArticleForWord(i.Name),
					" ",
					i.Name,
					" to the soup."
				}), -1L);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0002ABAC File Offset: 0x00028DAC
		private void governorTaste()
		{
			int likeLevel = 5;
			if (this.luauIngredients != null)
			{
				using (List<Item>.Enumerator enumerator = this.luauIngredients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Object o = enumerator.Current as Object;
						int itemLevel = 5;
						if ((o.quality >= 2 && o.price >= 160) || (o.quality == 1 && o.price >= 300 && o.edibility > 10))
						{
							itemLevel = 4;
							Utility.improveFriendshipWithEveryoneInRegion(Game1.player, 120, 2);
						}
						else if (o.edibility >= 20 || o.price >= 100 || (o.price >= 70 && o.quality >= 1))
						{
							itemLevel = 3;
							Utility.improveFriendshipWithEveryoneInRegion(Game1.player, 60, 2);
						}
						else if ((o.price > 20 && o.edibility >= 10) || (o.price >= 40 && o.edibility >= 5))
						{
							itemLevel = 2;
						}
						else if (o.edibility >= 0)
						{
							itemLevel = 1;
							Utility.improveFriendshipWithEveryoneInRegion(Game1.player, -50, 2);
						}
						if (o.edibility > -300 && o.edibility < 0)
						{
							itemLevel = 0;
							Utility.improveFriendshipWithEveryoneInRegion(Game1.player, -100, 2);
						}
						if (itemLevel < likeLevel)
						{
							likeLevel = itemLevel;
						}
					}
				}
				if (this.luauIngredients.Count < Game1.numberOfPlayers())
				{
					likeLevel = 5;
				}
			}
			this.eventCommands[this.CurrentCommand + 1] = "switchEvent governorReaction" + likeLevel;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0002AD40 File Offset: 0x00028F40
		private void eggHuntWinner()
		{
			int numberOfEggsToWin = 12;
			switch (Game1.numberOfPlayers())
			{
			case 1:
				numberOfEggsToWin = 9;
				break;
			case 2:
				numberOfEggsToWin = 6;
				break;
			case 3:
				numberOfEggsToWin = 5;
				break;
			case 4:
				numberOfEggsToWin = 4;
				break;
			}
			List<Farmer> winners = new List<Farmer>();
			Farmer arg_3F_0 = Game1.player;
			int mostEggsScore = Game1.player.festivalScore;
			for (int i = 1; i <= Game1.numberOfPlayers(); i++)
			{
				Farmer temp = Utility.getFarmerFromFarmerNumber(i);
				if (temp != null && temp.festivalScore > mostEggsScore)
				{
					mostEggsScore = temp.festivalScore;
				}
			}
			for (int j = 1; j <= Game1.numberOfPlayers(); j++)
			{
				Farmer temp2 = Utility.getFarmerFromFarmerNumber(j);
				if (temp2 != null && temp2.festivalScore == mostEggsScore)
				{
					winners.Add(temp2);
					temp2.festivalScore = -9999;
				}
			}
			string winnerDialogue = "Abigail!";
			if (mostEggsScore >= numberOfEggsToWin)
			{
				if (winners.Count == 1)
				{
					winnerDialogue = winners[0].name + "!";
				}
				else
				{
					winnerDialogue = "It's a tie between";
					for (int k = 0; k < winners.Count; k++)
					{
						if (k == winners.Count - 1)
						{
							winnerDialogue += " and";
						}
						winnerDialogue = winnerDialogue + " " + winners[k].name;
						if (k < winners.Count - 1)
						{
							winnerDialogue += ",";
						}
					}
					winnerDialogue += "!";
				}
				this.specialEventVariable1 = false;
			}
			else
			{
				this.specialEventVariable1 = true;
			}
			this.getActorByName("Lewis").CurrentDialogue.Push(new Dialogue(winnerDialogue, this.getActorByName("Lewis")));
			Game1.drawDialogue(this.getActorByName("Lewis"));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0002AEF0 File Offset: 0x000290F0
		private void iceFishingWinner()
		{
			int numberOfFishToWin = 5;
			List<Farmer> winners = new List<Farmer>();
			Farmer arg_0D_0 = Game1.player;
			int mostFishScore = Game1.player.festivalScore;
			for (int i = 1; i <= Game1.numberOfPlayers(); i++)
			{
				Farmer temp = Utility.getFarmerFromFarmerNumber(i);
				if (temp != null && temp.festivalScore > mostFishScore)
				{
					mostFishScore = temp.festivalScore;
				}
			}
			for (int j = 1; j <= Game1.numberOfPlayers(); j++)
			{
				Farmer temp2 = Utility.getFarmerFromFarmerNumber(j);
				if (temp2 != null && temp2.festivalScore == mostFishScore)
				{
					winners.Add(temp2);
				}
			}
			string winnerDialogue = "Willy, with 5 big, fresh fish!";
			if (mostFishScore >= numberOfFishToWin)
			{
				if (winners.Count == 1)
				{
					winnerDialogue = string.Concat(new object[]
					{
						winners[0].name,
						", with ",
						winners[0].festivalScore,
						" big, slimy fish!"
					});
					winners[0].festivalScore = -9999;
				}
				else
				{
					winnerDialogue = "It's a tie between";
					for (int k = 0; k < winners.Count; k++)
					{
						if (k == winners.Count - 1)
						{
							winnerDialogue += " and";
						}
						winnerDialogue = winnerDialogue + " " + winners[k].name;
						if (k < winners.Count - 1)
						{
							winnerDialogue += ",";
						}
					}
					winnerDialogue += "!";
				}
				this.specialEventVariable1 = false;
			}
			else
			{
				this.specialEventVariable1 = true;
			}
			this.getActorByName("Lewis").CurrentDialogue.Push(new Dialogue(winnerDialogue, this.getActorByName("Lewis")));
			Game1.drawDialogue(this.getActorByName("Lewis"));
		}

		// Token: 0x040001CF RID: 463
		private const float timeBetweenSpeech = 500f;

		// Token: 0x040001D0 RID: 464
		private const float viewportMoveSpeed = 3f;

		// Token: 0x040001D1 RID: 465
		public string[] eventCommands;

		// Token: 0x040001D2 RID: 466
		public int currentCommand;

		// Token: 0x040001D3 RID: 467
		public int oldPixelZoom = Game1.pixelZoom;

		// Token: 0x040001D4 RID: 468
		public int readyConfirmationTimer;

		// Token: 0x040001D5 RID: 469
		public int farmerAddedSpeed;

		// Token: 0x040001D6 RID: 470
		public List<NPC> actors = new List<NPC>();

		// Token: 0x040001D7 RID: 471
		public List<Object> props = new List<Object>();

		// Token: 0x040001D8 RID: 472
		public List<Prop> festivalProps = new List<Prop>();

		// Token: 0x040001D9 RID: 473
		public string messageToScreen;

		// Token: 0x040001DA RID: 474
		public string playerControlSequenceID;

		// Token: 0x040001DB RID: 475
		public bool showActiveObject;

		// Token: 0x040001DC RID: 476
		public bool continueAfterMove;

		// Token: 0x040001DD RID: 477
		public bool specialEventVariable1;

		// Token: 0x040001DE RID: 478
		public bool forked;

		// Token: 0x040001DF RID: 479
		public bool wasBloomDay;

		// Token: 0x040001E0 RID: 480
		public bool wasBloomVisible;

		// Token: 0x040001E1 RID: 481
		public bool playerControlSequence;

		// Token: 0x040001E2 RID: 482
		public bool eventSwitched;

		// Token: 0x040001E3 RID: 483
		public bool isFestival;

		// Token: 0x040001E4 RID: 484
		public bool sentReadyConfirmation;

		// Token: 0x040001E5 RID: 485
		public bool allPlayersReady;

		// Token: 0x040001E6 RID: 486
		public bool playerWasMounted;

		// Token: 0x040001E7 RID: 487
		public bool showGroundObjects = true;

		// Token: 0x040001E8 RID: 488
		private Dictionary<string, Vector3> actorPositionsAfterMove;

		// Token: 0x040001E9 RID: 489
		private float timeAccumulator;

		// Token: 0x040001EA RID: 490
		private float viewportXAccumulator;

		// Token: 0x040001EB RID: 491
		private float viewportYAccumulator;

		// Token: 0x040001EC RID: 492
		private Vector3 viewportTarget;

		// Token: 0x040001ED RID: 493
		private Color previousAmbientLight;

		// Token: 0x040001EE RID: 494
		private BloomSettings previousBloomSettings;

		// Token: 0x040001EF RID: 495
		public List<NPC> npcsWithUniquePortraits = new List<NPC>();

		// Token: 0x040001F0 RID: 496
		private LocalizedContentManager temporaryContent;

		// Token: 0x040001F1 RID: 497
		private GameLocation temporaryLocation;

		// Token: 0x040001F2 RID: 498
		public Point playerControlTargetTile;

		// Token: 0x040001F3 RID: 499
		public Texture2D festivalTexture;

		// Token: 0x040001F4 RID: 500
		public List<NPCController> npcControllers;

		// Token: 0x040001F5 RID: 501
		public NPC secretSantaRecipient;

		// Token: 0x040001F6 RID: 502
		public NPC mySecretSanta;

		// Token: 0x040001F7 RID: 503
		private bool skippable;

		// Token: 0x040001F8 RID: 504
		private int id;

		// Token: 0x040001F9 RID: 505
		public List<Vector2> characterWalkLocations = new List<Vector2>();

		// Token: 0x040001FA RID: 506
		private Dictionary<string, string> festivalData;

		// Token: 0x040001FB RID: 507
		private int oldShirt;

		// Token: 0x040001FC RID: 508
		private Color oldPants;

		// Token: 0x040001FD RID: 509
		private Item tmpItem;

		// Token: 0x040001FE RID: 510
		private bool drawTool;

		// Token: 0x040001FF RID: 511
		public bool skipped;

		// Token: 0x04000200 RID: 512
		private bool waitingForMenuClose;

		// Token: 0x04000201 RID: 513
		private int oldTime;

		// Token: 0x04000202 RID: 514
		public List<TemporaryAnimatedSprite> underwaterSprites;

		// Token: 0x04000203 RID: 515
		public List<TemporaryAnimatedSprite> aboveMapSprites;

		// Token: 0x04000204 RID: 516
		private NPC festivalHost;

		// Token: 0x04000205 RID: 517
		private string hostMessage;

		// Token: 0x04000206 RID: 518
		public int festivalTimer;

		// Token: 0x04000207 RID: 519
		private Item tempItemStash;

		// Token: 0x04000208 RID: 520
		public int grangeScore = -1000;

		// Token: 0x04000209 RID: 521
		public Farmer playerUsingGrangeDisplay;

		// Token: 0x0400020A RID: 522
		private int previousFacingDirection = -1;

		// Token: 0x0400020B RID: 523
		public Dictionary<string, Dictionary<Item, int[]>> festivalShops;

		// Token: 0x0400020C RID: 524
		private int previousAnswerChoice = -1;

		// Token: 0x0400020D RID: 525
		private bool startSecretSantaAfterDialogue;

		// Token: 0x0400020E RID: 526
		public List<Item> grangeDisplay;

		// Token: 0x0400020F RID: 527
		public bool specialEventVariable2;

		// Token: 0x04000210 RID: 528
		public List<Item> luauIngredients;
	}
}
