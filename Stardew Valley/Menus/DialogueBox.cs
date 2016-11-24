using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
	// Token: 0x020000EB RID: 235
	public class DialogueBox : IClickableMenu
	{
		// Token: 0x06000E29 RID: 3625 RVA: 0x001212D8 File Offset: 0x0011F4D8
		public DialogueBox(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
			this.activatedByGamePad = Game1.isAnyGamePadButtonBeingPressed();
			this.gamePadIntroTimer = 1000;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0012137C File Offset: 0x0011F57C
		public DialogueBox(string dialogue)
		{
			this.activatedByGamePad = Game1.isAnyGamePadButtonBeingPressed();
			if (this.activatedByGamePad)
			{
				Game1.mouseCursorTransparency = 0f;
				this.gamePadIntroTimer = 1000;
			}
			this.dialogues.AddRange(dialogue.Split(new char[]
			{
				'#'
			}));
			this.width = Math.Min(1200, SpriteText.getWidthOfString(dialogue) + Game1.tileSize);
			this.height = SpriteText.getHeightOfString(dialogue, this.width - Game1.pixelZoom * 5) + Game1.pixelZoom;
			this.x = (int)Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0).X;
			this.y = Game1.viewport.Height - this.height - Game1.tileSize;
			this.setUpIcons();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x001214B0 File Offset: 0x0011F6B0
		public DialogueBox(string dialogue, List<Response> responses)
		{
			this.activatedByGamePad = Game1.isAnyGamePadButtonBeingPressed();
			this.gamePadIntroTimer = 1000;
			this.dialogues.Add(dialogue);
			this.responses = responses;
			this.isQuestion = true;
			this.width = 1200;
			this.setUpQuestions();
			this.height = this.heightForQuestions;
			this.x = (int)Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0).X;
			this.y = Game1.viewport.Height - this.height - Game1.tileSize;
			this.setUpIcons();
			this.characterIndexInDialogue = dialogue.Length - 1;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x001215C0 File Offset: 0x0011F7C0
		public DialogueBox(Dialogue dialogue)
		{
			this.characterDialogue = dialogue;
			this.activatedByGamePad = Game1.isAnyGamePadButtonBeingPressed();
			if (this.activatedByGamePad)
			{
				Game1.mouseCursorTransparency = 0f;
				this.gamePadIntroTimer = 1000;
			}
			this.width = 1200;
			this.height = 6 * Game1.tileSize;
			this.x = (int)Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0).X;
			this.y = Game1.viewport.Height - this.height - Game1.tileSize;
			this.friendshipJewel = new Rectangle(this.x + this.width - Game1.tileSize, this.y + Game1.tileSize * 4, 11 * Game1.pixelZoom, 11 * Game1.pixelZoom);
			this.characterDialoguesBrokenUp.Push(dialogue.getCurrentDialogue());
			this.checkDialogue(dialogue);
			this.newPortaitShakeTimer = ((this.characterDialogue.getPortraitIndex() == 1) ? 250 : 0);
			this.setUpForGamePadMode();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0012172C File Offset: 0x0011F92C
		public DialogueBox(List<string> dialogues)
		{
			this.activatedByGamePad = Game1.isAnyGamePadButtonBeingPressed();
			if (this.activatedByGamePad)
			{
				Game1.mouseCursorTransparency = 0f;
				this.gamePadIntroTimer = 1000;
			}
			this.dialogues = dialogues;
			this.width = Math.Min(1200, SpriteText.getWidthOfString(dialogues[0]) + Game1.tileSize);
			this.height = SpriteText.getHeightOfString(dialogues[0], this.width - Game1.pixelZoom * 4);
			this.x = (int)Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0).X;
			this.y = Game1.viewport.Height - this.height - Game1.tileSize;
			this.setUpIcons();
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0000821B File Offset: 0x0000641B
		public override bool autoCenterMouseCursorForGamepad()
		{
			return false;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00121850 File Offset: 0x0011FA50
		private void playOpeningSound()
		{
			Game1.playSound("breathin");
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0012185C File Offset: 0x0011FA5C
		public override void setUpForGamePadMode()
		{
			if (Game1.options.gamepadControls && (this.activatedByGamePad || !Game1.lastCursorMotionWasMouse))
			{
				this.gamePadControlsImplemented = true;
				if (this.isQuestion)
				{
					int textHeight = 0;
					string s = this.getCurrentString();
					if (s != null && s.Length > 0)
					{
						textHeight = SpriteText.getHeightOfString(s, 999999);
					}
					Game1.setMousePosition(this.x + this.width - Game1.tileSize * 2, this.y + textHeight + Game1.tileSize);
					return;
				}
				Game1.mouseCursorTransparency = 0f;
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x001218E8 File Offset: 0x0011FAE8
		public void closeDialogue()
		{
			if (Game1.activeClickableMenu.Equals(this))
			{
				Game1.exitActiveMenu();
				Game1.dialogueUp = false;
				if (this.characterDialogue != null && this.characterDialogue.speaker != null && this.characterDialogue.speaker.CurrentDialogue.Count > 0 && this.dialogueFinished && this.characterDialogue.speaker.CurrentDialogue.Count > 0)
				{
					this.characterDialogue.speaker.CurrentDialogue.Pop();
				}
				if (Game1.messagePause)
				{
					Game1.pauseTime = 500f;
				}
				if (Game1.currentObjectDialogue.Count > 0)
				{
					Game1.currentObjectDialogue.Dequeue();
				}
				Game1.currentDialogueCharacterIndex = 0;
				if (Game1.currentObjectDialogue.Count > 0)
				{
					Game1.dialogueUp = true;
					Game1.questionChoices.Clear();
					Game1.dialogueTyping = true;
				}
				Game1.tvStation = -1;
				if (this.characterDialogue != null && this.characterDialogue.speaker != null && !this.characterDialogue.speaker.name.Equals("Gunther") && !Game1.eventUp && !this.characterDialogue.speaker.doingEndOfRouteAnimation)
				{
					this.characterDialogue.speaker.doneFacingPlayer(Game1.player);
				}
				Game1.currentSpeaker = null;
				if (!Game1.eventUp)
				{
					Game1.player.CanMove = true;
					Game1.player.movementDirections.Clear();
				}
				else if (Game1.currentLocation.currentEvent.CurrentCommand > 0 || Game1.currentLocation.currentEvent.specialEventVariable1)
				{
					if (!Game1.isFestival() || !Game1.currentLocation.currentEvent.canMoveAfterDialogue())
					{
						Event expr_1A3 = Game1.currentLocation.currentEvent;
						int currentCommand = expr_1A3.CurrentCommand;
						expr_1A3.CurrentCommand = currentCommand + 1;
					}
					else
					{
						Game1.player.CanMove = true;
					}
				}
				Game1.questionChoices.Clear();
			}
			if (Game1.afterDialogues != null)
			{
				Game1.afterFadeFunction arg_1DB_0 = Game1.afterDialogues;
				Game1.afterDialogues = null;
				arg_1DB_0();
			}
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00121AD5 File Offset: 0x0011FCD5
		public void finishTyping()
		{
			this.characterIndexInDialogue = this.getCurrentString().Length - 1;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00121AEA File Offset: 0x0011FCEA
		public void beginOutro()
		{
			this.transitioning = true;
			this.transitioningBigger = false;
			Game1.playSound("breathout");
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00121B04 File Offset: 0x0011FD04
		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
			this.receiveLeftClick(x, y, playSound);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00121B0F File Offset: 0x0011FD0F
		private void tryOutro()
		{
			if (Game1.activeClickableMenu != null && Game1.activeClickableMenu.Equals(this))
			{
				this.beginOutro();
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00121B2C File Offset: 0x0011FD2C
		public override void receiveKeyPress(Keys key)
		{
			if (Game1.options.doesInputListContain(Game1.options.actionButton, key))
			{
				this.receiveLeftClick(0, 0, true);
				return;
			}
			if (this.isQuestion && !Game1.eventUp && this.characterDialogue == null)
			{
				if (Game1.options.doesInputListContain(Game1.options.menuButton, key))
				{
					if (this.responses != null && this.responses.Count > 0 && Game1.currentLocation.answerDialogue(this.responses[this.responses.Count - 1]))
					{
						Game1.playSound("smallSelect");
					}
					this.selectedResponse = -1;
					this.tryOutro();
					return;
				}
				if (key == Keys.Y && this.responses != null && this.responses.Count > 0 && this.responses[0].responseKey.Equals("Yes") && Game1.currentLocation.answerDialogue(this.responses[0]))
				{
					Game1.playSound("smallSelect");
					this.selectedResponse = -1;
					this.tryOutro();
					return;
				}
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00121C4C File Offset: 0x0011FE4C
		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
			if (!this.transitioning)
			{
				if (this.characterIndexInDialogue < this.getCurrentString().Length - 1)
				{
					this.characterIndexInDialogue = this.getCurrentString().Length - 1;
					return;
				}
				if (this.safetyTimer > 0)
				{
					return;
				}
				if (this.isQuestion)
				{
					if (this.selectedResponse == -1)
					{
						return;
					}
					DialogueBox.questionFinishPauseTimer = (Game1.eventUp ? 600 : 200);
					this.transitioning = true;
					this.transitionX = -1;
					this.transitioningBigger = true;
					if (this.characterDialogue != null)
					{
						this.characterDialoguesBrokenUp.Pop();
						this.characterDialogue.chooseResponse(this.responses[this.selectedResponse]);
						this.characterDialoguesBrokenUp.Push("");
						Game1.playSound("smallSelect");
					}
					else
					{
						Game1.dialogueUp = false;
						if (Game1.eventUp)
						{
							Game1.playSound("smallSelect");
							Game1.currentLocation.currentEvent.answerDialogue(Game1.currentLocation.lastQuestionKey, this.selectedResponse);
							this.selectedResponse = -1;
							this.tryOutro();
							return;
						}
						if (Game1.currentLocation.answerDialogue(this.responses[this.selectedResponse]))
						{
							Game1.playSound("smallSelect");
						}
						this.selectedResponse = -1;
						this.tryOutro();
						return;
					}
				}
				else if (this.characterDialogue == null)
				{
					this.dialogues.RemoveAt(0);
					if (this.dialogues.Count == 0)
					{
						this.closeDialogue();
					}
					else
					{
						this.width = Math.Min(1200, SpriteText.getWidthOfString(this.dialogues[0]) + Game1.tileSize);
						this.height = SpriteText.getHeightOfString(this.dialogues[0], this.width - Game1.pixelZoom * 4);
						this.x = (int)Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0).X;
						this.y = Game1.viewport.Height - this.height - Game1.tileSize * 2;
						this.xPositionOnScreen = x;
						this.yPositionOnScreen = y;
						this.setUpIcons();
					}
				}
				this.characterIndexInDialogue = 0;
				if (this.characterDialogue != null)
				{
					int oldPortrait = this.characterDialogue.getPortraitIndex();
					if (this.characterDialoguesBrokenUp.Count == 0)
					{
						this.beginOutro();
						return;
					}
					this.characterDialoguesBrokenUp.Pop();
					if (this.characterDialoguesBrokenUp.Count == 0)
					{
						if (!this.characterDialogue.isCurrentStringContinuedOnNextScreen)
						{
							this.beginOutro();
						}
						this.characterDialogue.exitCurrentDialogue();
					}
					if (!this.characterDialogue.isDialogueFinished() && this.characterDialogue.getCurrentDialogue().Length > 0 && this.characterDialoguesBrokenUp.Count == 0)
					{
						this.characterDialoguesBrokenUp.Push(this.characterDialogue.getCurrentDialogue());
					}
					this.checkDialogue(this.characterDialogue);
					if (this.characterDialogue.getPortraitIndex() != oldPortrait)
					{
						this.newPortaitShakeTimer = ((this.characterDialogue.getPortraitIndex() == 1) ? 250 : 50);
					}
				}
				if (!this.transitioning)
				{
					Game1.playSound("smallSelect");
				}
				this.setUpIcons();
				this.safetyTimer = 750;
				if (this.getCurrentString() != null && this.getCurrentString().Length <= 20)
				{
					this.safetyTimer -= 200;
				}
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00121FA0 File Offset: 0x001201A0
		private void setUpIcons()
		{
			this.dialogueIcon = null;
			if (this.isQuestion)
			{
				this.setUpQuestionIcon();
			}
			else if (this.characterDialogue != null && (this.characterDialogue.isCurrentStringContinuedOnNextScreen || this.characterDialoguesBrokenUp.Count > 1))
			{
				this.setUpNextPageIcon();
			}
			else if (this.dialogues != null && this.dialogues.Count > 1)
			{
				this.setUpNextPageIcon();
			}
			else
			{
				this.setUpCloseDialogueIcon();
			}
			this.setUpForGamePadMode();
			if (this.getCurrentString() != null && this.getCurrentString().Length <= 20)
			{
				this.safetyTimer -= 200;
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00122044 File Offset: 0x00120244
		public override void performHoverAction(int mouseX, int mouseY)
		{
			this.hoverText = "";
			if (!this.transitioning && this.characterIndexInDialogue >= this.getCurrentString().Length - 1)
			{
				base.performHoverAction(mouseX, mouseY);
				if (this.isQuestion)
				{
					int oldResponse = this.selectedResponse;
					int responseY = this.y - (this.heightForQuestions - this.height) + SpriteText.getHeightOfString(this.getCurrentString(), this.width) + Game1.pixelZoom * 12;
					for (int i = 0; i < this.responses.Count; i++)
					{
						SpriteText.getHeightOfString(this.responses[i].responseText, this.width);
						if (mouseY >= responseY && mouseY < responseY + SpriteText.getHeightOfString(this.responses[i].responseText, this.width))
						{
							this.selectedResponse = i;
							break;
						}
						responseY += SpriteText.getHeightOfString(this.responses[i].responseText, this.width) + Game1.pixelZoom * 4;
					}
					if (this.selectedResponse != oldResponse)
					{
						Game1.playSound("Cowboy_gunshot");
					}
				}
			}
			if (!Game1.eventUp && !this.friendshipJewel.Equals(Rectangle.Empty) && this.friendshipJewel.Contains(mouseX, mouseY) && this.characterDialogue != null && this.characterDialogue.speaker != null && Game1.player.friendships.ContainsKey(this.characterDialogue.speaker.name))
			{
				this.hoverText = string.Concat(new object[]
				{
					Game1.player.getFriendshipHeartLevelForNPC(this.characterDialogue.speaker.name),
					"/",
					this.characterDialogue.speaker.name.Equals(Game1.player.spouse) ? "12" : "10",
					"<"
				});
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00122248 File Offset: 0x00120448
		private void setUpQuestionIcon()
		{
			Vector2 iconPosition = new Vector2((float)(this.x + this.width - 10 * Game1.pixelZoom), (float)(this.y + this.height - 11 * Game1.pixelZoom));
			this.dialogueIcon = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(330, 357, 7, 13), 100f, 6, 999999, iconPosition, false, false, 0.89f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
			{
				yPeriodic = true,
				yPeriodicLoopTime = 1500f,
				yPeriodicRange = (float)(Game1.tileSize / 8)
			};
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00122300 File Offset: 0x00120500
		private void setUpCloseDialogueIcon()
		{
			Vector2 iconPosition = new Vector2((float)(this.x + this.width - 10 * Game1.pixelZoom), (float)(this.y + this.height - 11 * Game1.pixelZoom));
			if (this.isPortraitBox())
			{
				iconPosition.X -= (float)(115 * Game1.pixelZoom + 8 * Game1.pixelZoom);
			}
			this.dialogueIcon = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(289, 342, 11, 12), 80f, 11, 999999, iconPosition, false, false, 0.89f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x001223C0 File Offset: 0x001205C0
		private void setUpNextPageIcon()
		{
			Vector2 iconPosition = new Vector2((float)(this.x + this.width - 10 * Game1.pixelZoom), (float)(this.y + this.height - 10 * Game1.pixelZoom));
			if (this.isPortraitBox())
			{
				iconPosition.X -= (float)(115 * Game1.pixelZoom + 8 * Game1.pixelZoom);
			}
			this.dialogueIcon = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(232, 346, 9, 9), 90f, 6, 999999, iconPosition, false, false, 0.89f, 0f, Color.White, (float)Game1.pixelZoom, 0f, 0f, 0f, true)
			{
				yPeriodic = true,
				yPeriodicLoopTime = 1500f,
				yPeriodicRange = (float)(Game1.tileSize / 8)
			};
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0012249C File Offset: 0x0012069C
		private void checkDialogue(Dialogue d)
		{
			this.isQuestion = false;
			string sub = "";
			if (this.characterDialoguesBrokenUp.Count == 1)
			{
				sub = SpriteText.getSubstringBeyondHeight(this.characterDialoguesBrokenUp.Peek(), this.width - 115 * Game1.pixelZoom - 5 * Game1.pixelZoom, this.height - Game1.pixelZoom * 4);
			}
			if (sub.Length > 0)
			{
				string full = this.characterDialoguesBrokenUp.Pop().Replace(Environment.NewLine, "");
				this.characterDialoguesBrokenUp.Push(sub.Trim());
				this.characterDialoguesBrokenUp.Push(full.Substring(0, full.Length - sub.Length + 1).Trim());
			}
			if (d.getCurrentDialogue().Length == 0)
			{
				this.dialogueFinished = true;
			}
			if (d.isCurrentStringContinuedOnNextScreen || this.characterDialoguesBrokenUp.Count > 1)
			{
				this.dialogueContinuedOnNextPage = true;
			}
			else if (d.getCurrentDialogue().Length == 0)
			{
				this.beginOutro();
			}
			if (d.isCurrentDialogueAQuestion())
			{
				this.responses = d.getResponseOptions();
				this.isQuestion = true;
				this.setUpQuestions();
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x001225BC File Offset: 0x001207BC
		private void setUpQuestions()
		{
			int tmpwidth = this.width - Game1.pixelZoom * 4;
			this.heightForQuestions = SpriteText.getHeightOfString(this.getCurrentString(), tmpwidth);
			foreach (Response r in this.responses)
			{
				this.heightForQuestions += SpriteText.getHeightOfString(r.responseText, tmpwidth) + Game1.pixelZoom * 4;
			}
			this.heightForQuestions += Game1.pixelZoom * 10;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00122660 File Offset: 0x00120860
		public bool isPortraitBox()
		{
			return this.characterDialogue != null && this.characterDialogue.speaker != null && this.characterDialogue.speaker.Portrait != null && this.characterDialogue.showPortrait && Game1.options.showPortraits;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x001226B0 File Offset: 0x001208B0
		public void drawBox(SpriteBatch b, int xPos, int yPos, int boxWidth, int boxHeight)
		{
			if (xPos > 0)
			{
				b.Draw(Game1.mouseCursors, new Rectangle(xPos, yPos, boxWidth, boxHeight), new Rectangle?(new Rectangle(306, 320, 16, 16)), Color.White);
				b.Draw(Game1.mouseCursors, new Rectangle(xPos, yPos - 5 * Game1.pixelZoom, boxWidth, 6 * Game1.pixelZoom), new Rectangle?(new Rectangle(275, 313, 1, 6)), Color.White);
				b.Draw(Game1.mouseCursors, new Rectangle(xPos + 3 * Game1.pixelZoom, yPos + boxHeight, boxWidth - 5 * Game1.pixelZoom, 8 * Game1.pixelZoom), new Rectangle?(new Rectangle(275, 328, 1, 8)), Color.White);
				b.Draw(Game1.mouseCursors, new Rectangle(xPos - 8 * Game1.pixelZoom, yPos + 6 * Game1.pixelZoom, 8 * Game1.pixelZoom, boxHeight - 7 * Game1.pixelZoom), new Rectangle?(new Rectangle(264, 325, 8, 1)), Color.White);
				b.Draw(Game1.mouseCursors, new Rectangle(xPos + boxWidth, yPos, 7 * Game1.pixelZoom, boxHeight), new Rectangle?(new Rectangle(293, 324, 7, 1)), Color.White);
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPos - 11 * Game1.pixelZoom), (float)(yPos - 7 * Game1.pixelZoom)), new Rectangle?(new Rectangle(261, 311, 14, 13)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPos + boxWidth - Game1.pixelZoom * 2), (float)(yPos - 7 * Game1.pixelZoom)), new Rectangle?(new Rectangle(291, 311, 12, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPos + boxWidth - Game1.pixelZoom * 2), (float)(yPos + boxHeight - 2 * Game1.pixelZoom)), new Rectangle?(new Rectangle(291, 326, 12, 12)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPos - 11 * Game1.pixelZoom), (float)(yPos + boxHeight - Game1.pixelZoom)), new Rectangle?(new Rectangle(261, 327, 14, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.87f);
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00122970 File Offset: 0x00120B70
		private bool shouldPortraitShake(Dialogue d)
		{
			int view = d.getPortraitIndex();
			return (d.speaker.name.Equals("Pam") && view == 3) || (d.speaker.name.Equals("Abigail") && view == 7) || (d.speaker.name.Equals("Haley") && view == 5) || (d.speaker.name.Equals("Maru") && view == 9) || this.newPortaitShakeTimer > 0;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00122A04 File Offset: 0x00120C04
		public void drawPortrait(SpriteBatch b)
		{
			if (this.width >= 107 * Game1.pixelZoom * 3 / 2)
			{
				int xPositionOfPortraitArea = this.x + this.width - 112 * Game1.pixelZoom + Game1.pixelZoom;
				int widthOfPortraitArea = this.x + this.width - xPositionOfPortraitArea;
				b.Draw(Game1.mouseCursors, new Rectangle(xPositionOfPortraitArea - 10 * Game1.pixelZoom, this.y, 9 * Game1.pixelZoom, this.height), new Rectangle?(new Rectangle(278, 324, 9, 1)), Color.White);
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPositionOfPortraitArea - 10 * Game1.pixelZoom), (float)(this.y - 5 * Game1.pixelZoom)), new Rectangle?(new Rectangle(278, 313, 10, 7)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPositionOfPortraitArea - 10 * Game1.pixelZoom), (float)(this.y + this.height)), new Rectangle?(new Rectangle(278, 328, 10, 8)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
				int portraitBoxX = xPositionOfPortraitArea + Game1.pixelZoom * 19;
				int portraitBoxY = this.y + this.height / 2 - 74 * Game1.pixelZoom / 2 - 18 * Game1.pixelZoom / 2;
				b.Draw(Game1.mouseCursors, new Vector2((float)(xPositionOfPortraitArea - 2 * Game1.pixelZoom), (float)this.y), new Rectangle?(new Rectangle(583, 411, 115, 97)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
				Rectangle portraitSource = Game1.getSourceRectForStandardTileSheet(this.characterDialogue.speaker.Portrait, this.characterDialogue.getPortraitIndex(), 64, 64);
				if (!this.characterDialogue.speaker.Portrait.Bounds.Contains(portraitSource))
				{
					portraitSource = new Rectangle(0, 0, 64, 64);
				}
				int xOffset = this.shouldPortraitShake(this.characterDialogue) ? Game1.random.Next(-1, 2) : 0;
				b.Draw(this.characterDialogue.speaker.Portrait, new Vector2((float)(portraitBoxX + 4 * Game1.pixelZoom + xOffset), (float)(portraitBoxY + 6 * Game1.pixelZoom)), new Rectangle?(portraitSource), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
				SpriteText.drawStringHorizontallyCenteredAt(b, this.characterDialogue.speaker.getName(), xPositionOfPortraitArea + widthOfPortraitArea / 2, portraitBoxY + 74 * Game1.pixelZoom + 4 * Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1);
				if (!Game1.eventUp && !this.friendshipJewel.Equals(Rectangle.Empty) && this.characterDialogue != null && this.characterDialogue.speaker != null && Game1.player.friendships.ContainsKey(this.characterDialogue.speaker.name))
				{
					b.Draw(Game1.mouseCursors, new Vector2((float)this.friendshipJewel.X, (float)this.friendshipJewel.Y), new Rectangle?((Game1.player.getFriendshipHeartLevelForNPC(this.characterDialogue.speaker.name) >= 10) ? new Rectangle(269, 494, 11, 11) : new Rectangle(Math.Max(140, 140 + (int)(Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1000.0 / 250.0) * 11), Math.Max(532, 532 + Game1.player.getFriendshipHeartLevelForNPC(this.characterDialogue.speaker.name) / 2 * 11), 11, 11)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
				}
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00122E34 File Offset: 0x00121034
		public string getCurrentString()
		{
			if (this.characterDialogue != null)
			{
				string s;
				if (this.characterDialoguesBrokenUp.Count > 0)
				{
					s = this.characterDialoguesBrokenUp.Peek().Trim().Replace(Environment.NewLine, "");
				}
				else
				{
					s = this.characterDialogue.getCurrentDialogue().Trim().Replace(Environment.NewLine, "");
				}
				if (!Game1.options.showPortraits)
				{
					s = this.characterDialogue.speaker.getName() + ": " + s;
				}
				return s;
			}
			if (this.dialogues.Count > 0)
			{
				return this.dialogues[0].Trim().Replace(Environment.NewLine, "");
			}
			return "";
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00122EF8 File Offset: 0x001210F8
		public override void update(GameTime time)
		{
			base.update(time);
			if (!Game1.lastCursorMotionWasMouse && !this.isQuestion)
			{
				Game1.mouseCursorTransparency = 0f;
			}
			else
			{
				Game1.mouseCursorTransparency = 1f;
			}
			if (this.gamePadIntroTimer > 0 && !this.isQuestion)
			{
				Game1.mouseCursorTransparency = 0f;
				this.gamePadIntroTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (this.safetyTimer > 0)
			{
				this.safetyTimer -= time.ElapsedGameTime.Milliseconds;
			}
			if (DialogueBox.questionFinishPauseTimer > 0)
			{
				DialogueBox.questionFinishPauseTimer -= time.ElapsedGameTime.Milliseconds;
				return;
			}
			if (this.transitioning)
			{
				if (this.transitionX == -1)
				{
					this.transitionX = this.x + this.width / 2;
					this.transitionY = this.y + this.height / 2;
					this.transitionWidth = 0;
					this.transitionHeight = 0;
				}
				if (this.transitioningBigger)
				{
					bool arg_267_0 = this.transitionWidth != 0;
					this.transitionX -= (int)((float)time.ElapsedGameTime.Milliseconds * 3f);
					this.transitionY -= (int)((float)time.ElapsedGameTime.Milliseconds * 3f * ((float)(this.isQuestion ? this.heightForQuestions : this.height) / (float)this.width));
					this.transitionX = Math.Max(this.x, this.transitionX);
					this.transitionY = Math.Max(this.isQuestion ? (this.y + this.height - this.heightForQuestions) : this.y, this.transitionY);
					this.transitionWidth += (int)((float)time.ElapsedGameTime.Milliseconds * 3f * 2f);
					this.transitionHeight += (int)((float)time.ElapsedGameTime.Milliseconds * 3f * ((float)(this.isQuestion ? this.heightForQuestions : this.height) / (float)this.width) * 2f);
					this.transitionWidth = Math.Min(this.width, this.transitionWidth);
					this.transitionHeight = Math.Min(this.isQuestion ? this.heightForQuestions : this.height, this.transitionHeight);
					if (!arg_267_0 && this.transitionWidth > 0)
					{
						this.playOpeningSound();
					}
					if (this.transitionX == this.x && this.transitionY == (this.isQuestion ? (this.y + this.height - this.heightForQuestions) : this.y))
					{
						this.transitioning = false;
						this.characterAdvanceTimer = 90;
						this.setUpIcons();
						this.transitionX = this.x;
						this.transitionY = this.y;
						this.transitionWidth = this.width;
						this.transitionHeight = this.height;
					}
				}
				else
				{
					this.transitionX += (int)((float)time.ElapsedGameTime.Milliseconds * 3f);
					this.transitionY += (int)((float)time.ElapsedGameTime.Milliseconds * 3f * ((float)this.height / (float)this.width));
					this.transitionX = Math.Min(this.x + this.width / 2, this.transitionX);
					this.transitionY = Math.Min(this.y + this.height / 2, this.transitionY);
					this.transitionWidth -= (int)((float)time.ElapsedGameTime.Milliseconds * 3f * 2f);
					this.transitionHeight -= (int)((float)time.ElapsedGameTime.Milliseconds * 3f * ((float)this.height / (float)this.width) * 2f);
					this.transitionWidth = Math.Max(0, this.transitionWidth);
					this.transitionHeight = Math.Max(0, this.transitionHeight);
					if (this.transitionWidth == 0 && this.transitionHeight == 0)
					{
						this.closeDialogue();
					}
				}
			}
			if (!this.transitioning && this.characterIndexInDialogue < this.getCurrentString().Length)
			{
				this.characterAdvanceTimer -= time.ElapsedGameTime.Milliseconds;
				if (this.characterAdvanceTimer <= 0)
				{
					this.characterAdvanceTimer = 30;
					int old = this.characterIndexInDialogue;
					this.characterIndexInDialogue = Math.Min(this.characterIndexInDialogue + 1, this.getCurrentString().Length);
					if (this.characterIndexInDialogue != old && this.characterIndexInDialogue == this.getCurrentString().Length)
					{
						Game1.playSound("dialogueCharacterClose");
					}
					if (this.characterIndexInDialogue > 1 && this.characterIndexInDialogue < this.getCurrentString().Length && Game1.options.dialogueTyping)
					{
						Game1.playSound("dialogueCharacter");
					}
				}
			}
			if (!this.transitioning && this.dialogueIcon != null)
			{
				this.dialogueIcon.update(time);
			}
			if (!this.transitioning && this.newPortaitShakeTimer > 0)
			{
				this.newPortaitShakeTimer -= time.ElapsedGameTime.Milliseconds;
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00123450 File Offset: 0x00121650
		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			this.width = 1200;
			this.height = 6 * Game1.tileSize;
			this.x = (int)Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0).X;
			this.y = Game1.viewport.Height - this.height - Game1.tileSize;
			this.friendshipJewel = new Rectangle(this.x + this.width - Game1.tileSize, this.y + Game1.tileSize * 4, 11 * Game1.pixelZoom, 11 * Game1.pixelZoom);
			this.setUpIcons();
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x001234F4 File Offset: 0x001216F4
		public override void draw(SpriteBatch b)
		{
			if (this.width < Game1.tileSize / 4 || this.height < Game1.tileSize / 4)
			{
				return;
			}
			if (this.transitioning)
			{
				this.drawBox(b, this.transitionX, this.transitionY, this.transitionWidth, this.transitionHeight);
				if ((!this.activatedByGamePad || Game1.lastCursorMotionWasMouse || this.isQuestion || Game1.isGamePadThumbstickInMotion()) && (Game1.getMouseX() != 0 || Game1.getMouseY() != 0))
				{
					base.drawMouse(b);
				}
				return;
			}
			if (this.isQuestion)
			{
				this.drawBox(b, this.x, this.y - (this.heightForQuestions - this.height), this.width, this.heightForQuestions);
				SpriteText.drawString(b, this.getCurrentString(), this.x + Game1.pixelZoom * 2, this.y + Game1.pixelZoom * 3 - (this.heightForQuestions - this.height), this.characterIndexInDialogue, this.width - Game1.pixelZoom * 4, 999999, 1f, 0.88f, false, -1, "", -1);
				if (this.characterIndexInDialogue >= this.getCurrentString().Length - 1)
				{
					int responseY = this.y - (this.heightForQuestions - this.height) + SpriteText.getHeightOfString(this.getCurrentString(), this.width - Game1.pixelZoom * 4) + Game1.pixelZoom * 12;
					for (int i = 0; i < this.responses.Count; i++)
					{
						if (i == this.selectedResponse)
						{
							IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), this.x + Game1.pixelZoom, responseY - Game1.pixelZoom * 2, this.width - Game1.pixelZoom * 2, SpriteText.getHeightOfString(this.responses[i].responseText, this.width - Game1.pixelZoom * 4) + Game1.pixelZoom * 4, Color.White, (float)Game1.pixelZoom, false);
						}
						SpriteText.drawString(b, this.responses[i].responseText, this.x + Game1.pixelZoom * 2, responseY, 999999, this.width, 999999, (this.selectedResponse == i) ? 1f : 0.6f, 0.88f, false, -1, "", -1);
						responseY += SpriteText.getHeightOfString(this.responses[i].responseText, this.width) + Game1.pixelZoom * 4;
					}
				}
			}
			else
			{
				this.drawBox(b, this.x, this.y, this.width, this.height);
				if (!this.isPortraitBox() && !this.isQuestion)
				{
					SpriteText.drawString(b, this.getCurrentString(), this.x + Game1.pixelZoom * 2, this.y + Game1.pixelZoom * 2, this.characterIndexInDialogue, this.width, 999999, 1f, 0.88f, false, -1, "", -1);
				}
			}
			if (this.isPortraitBox() && !this.isQuestion)
			{
				this.drawPortrait(b);
				if (!this.isQuestion)
				{
					SpriteText.drawString(b, this.getCurrentString(), this.x + Game1.pixelZoom * 2, this.y + Game1.pixelZoom * 2, this.characterIndexInDialogue, this.width - 115 * Game1.pixelZoom - 5 * Game1.pixelZoom, 999999, 1f, 0.88f, false, -1, "", -1);
				}
			}
			if (this.dialogueIcon != null && this.characterIndexInDialogue >= this.getCurrentString().Length - 1)
			{
				this.dialogueIcon.draw(b, true, 0, 0);
			}
			if ((!this.activatedByGamePad || Game1.lastCursorMotionWasMouse || this.isQuestion || Game1.isGamePadThumbstickInMotion()) && (Game1.getMouseX() != 0 || Game1.getMouseY() != 0))
			{
				base.drawMouse(b);
			}
			if (this.hoverText.Length > 0)
			{
				SpriteText.drawStringWithScrollBackground(b, this.hoverText, this.friendshipJewel.Center.X - SpriteText.getWidthOfString(this.hoverText) / 2, this.friendshipJewel.Y - Game1.tileSize, "", 1f, -1);
			}
		}

		// Token: 0x04000F1C RID: 3868
		private List<string> dialogues = new List<string>();

		// Token: 0x04000F1D RID: 3869
		private Dialogue characterDialogue;

		// Token: 0x04000F1E RID: 3870
		private Stack<string> characterDialoguesBrokenUp = new Stack<string>();

		// Token: 0x04000F1F RID: 3871
		private List<Response> responses = new List<Response>();

		// Token: 0x04000F20 RID: 3872
		public const int portraitBoxSize = 74;

		// Token: 0x04000F21 RID: 3873
		public const int nameTagWidth = 102;

		// Token: 0x04000F22 RID: 3874
		public const int nameTagHeight = 18;

		// Token: 0x04000F23 RID: 3875
		public const int portraitPlateWidth = 115;

		// Token: 0x04000F24 RID: 3876
		public const int nameTagSideMargin = 5;

		// Token: 0x04000F25 RID: 3877
		public const float transitionRate = 3f;

		// Token: 0x04000F26 RID: 3878
		public const int characterAdvanceDelay = 30;

		// Token: 0x04000F27 RID: 3879
		public const int safetyDelay = 750;

		// Token: 0x04000F28 RID: 3880
		public static int questionFinishPauseTimer;

		// Token: 0x04000F29 RID: 3881
		private Rectangle friendshipJewel = Rectangle.Empty;

		// Token: 0x04000F2A RID: 3882
		private bool activatedByGamePad;

		// Token: 0x04000F2B RID: 3883
		private int x;

		// Token: 0x04000F2C RID: 3884
		private int y;

		// Token: 0x04000F2D RID: 3885
		private int transitionX = -1;

		// Token: 0x04000F2E RID: 3886
		private int transitionY;

		// Token: 0x04000F2F RID: 3887
		private int transitionWidth;

		// Token: 0x04000F30 RID: 3888
		private int transitionHeight;

		// Token: 0x04000F31 RID: 3889
		private int characterAdvanceTimer;

		// Token: 0x04000F32 RID: 3890
		private int characterIndexInDialogue;

		// Token: 0x04000F33 RID: 3891
		private int safetyTimer = 750;

		// Token: 0x04000F34 RID: 3892
		private int heightForQuestions;

		// Token: 0x04000F35 RID: 3893
		private int selectedResponse = -1;

		// Token: 0x04000F36 RID: 3894
		private int newPortaitShakeTimer;

		// Token: 0x04000F37 RID: 3895
		private int gamePadIntroTimer;

		// Token: 0x04000F38 RID: 3896
		private bool transitioning = true;

		// Token: 0x04000F39 RID: 3897
		private bool transitioningBigger = true;

		// Token: 0x04000F3A RID: 3898
		private bool dialogueContinuedOnNextPage;

		// Token: 0x04000F3B RID: 3899
		private bool dialogueFinished;

		// Token: 0x04000F3C RID: 3900
		private bool isQuestion;

		// Token: 0x04000F3D RID: 3901
		private TemporaryAnimatedSprite dialogueIcon;

		// Token: 0x04000F3E RID: 3902
		private string hoverText = "";
	}
}
