using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace StardewValley
{
	// Token: 0x02000041 RID: 65
	public class Dialogue
	{
		// Token: 0x17000029 RID: 41
		public string CurrentEmotion
		{
			// Token: 0x0600030F RID: 783 RVA: 0x0003E037 File Offset: 0x0003C237
			get
			{
				return this.currentEmotion;
			}
			// Token: 0x06000310 RID: 784 RVA: 0x0003E03F File Offset: 0x0003C23F
			set
			{
				this.currentEmotion = value;
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0003E048 File Offset: 0x0003C248
		public Dialogue(string masterDialogue, NPC speaker)
		{
			this.speaker = speaker;
			this.parseDialogueString(masterDialogue);
			this.checkForSpecialDialogueAttributes();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0003E06F File Offset: 0x0003C26F
		public void setCurrentDialogue(string dialogue)
		{
			this.dialogues.Clear();
			this.currentDialogueIndex = 0;
			this.parseDialogueString(dialogue);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0003E08C File Offset: 0x0003C28C
		public void addMessageToFront(string dialogue)
		{
			this.currentDialogueIndex = 0;
			List<string> tmp = new List<string>();
			tmp.AddRange(this.dialogues);
			this.dialogues.Clear();
			this.parseDialogueString(dialogue);
			this.dialogues.AddRange(tmp);
			this.checkForSpecialDialogueAttributes();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0003E0D6 File Offset: 0x0003C2D6
		public static string getRandomVerb()
		{
			return Dialogue.verbs[Game1.random.Next(Dialogue.verbs.Length)];
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0003E0EF File Offset: 0x0003C2EF
		public static string getRandomAdjective()
		{
			return Dialogue.adjectives[Game1.random.Next(Dialogue.adjectives.Length)];
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0003E108 File Offset: 0x0003C308
		public static string getRandomNoun()
		{
			return Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)];
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0003E121 File Offset: 0x0003C321
		public static string getRandomPositional()
		{
			return Dialogue.positional[Game1.random.Next(Dialogue.positional.Length)];
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0003E13C File Offset: 0x0003C33C
		public int getPortraitIndex()
		{
			string a = this.currentEmotion;
			if (a == "$neutral")
			{
				return 0;
			}
			if (a == "$h")
			{
				return 1;
			}
			if (a == "$s")
			{
				return 2;
			}
			if (a == "$u")
			{
				return 3;
			}
			if (a == "$l")
			{
				return 4;
			}
			if (a == "$a")
			{
				return 5;
			}
			int i;
			if (int.TryParse(this.currentEmotion.Substring(1), out i))
			{
				return Convert.ToInt32(this.currentEmotion.Substring(1));
			}
			return 0;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0003E1D4 File Offset: 0x0003C3D4
		private void parseDialogueString(string masterString)
		{
			if (masterString == null)
			{
				masterString = "...";
			}
			this.temporaryDialogue = null;
			if (this.playerResponses != null)
			{
				this.playerResponses.Clear();
			}
			string[] masterDialogueSplit = masterString.Split(new char[]
			{
				'#'
			});
			for (int i = 0; i < masterDialogueSplit.Length; i++)
			{
				if (masterDialogueSplit[i].Length >= 2)
				{
					masterDialogueSplit[i] = this.checkForSpecialCharacters(masterDialogueSplit[i]);
					string stringIdentifier;
					try
					{
						stringIdentifier = masterDialogueSplit[i].Substring(0, 2);
					}
					catch (Exception)
					{
						stringIdentifier = "     ";
					}
					if (!stringIdentifier.Equals("$e"))
					{
						if (stringIdentifier.Equals("$b"))
						{
							if (this.dialogues.Count > 0)
							{
								List<string> list = this.dialogues;
								int index = this.dialogues.Count - 1;
								list[index] += "{";
							}
						}
						else if (stringIdentifier.Equals("$k"))
						{
							this.dialogueToBeKilled = true;
						}
						else if (stringIdentifier.Equals("$1") && masterDialogueSplit[i].Split(new char[]
						{
							' '
						}).Length > 1)
						{
							string messageID = masterDialogueSplit[i].Split(new char[]
							{
								' '
							})[1];
							if (!Game1.player.mailReceived.Contains(messageID))
							{
								masterDialogueSplit[i + 1] = this.checkForSpecialCharacters(masterDialogueSplit[i + 1]);
								this.dialogues.Add(messageID + "}" + masterDialogueSplit[i + 1]);
								i = 99999;
								return;
							}
							i += 3;
							masterDialogueSplit[i] = this.checkForSpecialCharacters(masterDialogueSplit[i]);
						}
						else if (stringIdentifier.Equals("$c") && masterDialogueSplit[i].Split(new char[]
						{
							' '
						}).Length > 1)
						{
							double chance = Convert.ToDouble(masterDialogueSplit[i].Split(new char[]
							{
								' '
							})[1]);
							if (Game1.random.NextDouble() > chance)
							{
								i++;
							}
							else
							{
								this.dialogues.Add(masterDialogueSplit[i + 1]);
								i += 2;
							}
						}
						else if (stringIdentifier.Equals("$q"))
						{
							if (this.dialogues.Count > 0)
							{
								List<string> list = this.dialogues;
								int index = this.dialogues.Count - 1;
								list[index] += "{";
							}
							string[] questionSplit = masterDialogueSplit[i].Split(new char[]
							{
								' '
							});
							string[] answerIDs = questionSplit[1].Split(new char[]
							{
								'/'
							});
							bool alreadySeenAnswer = false;
							for (int j = 0; j < answerIDs.Length; j++)
							{
								if (Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(answerIDs[j])))
								{
									alreadySeenAnswer = true;
									break;
								}
							}
							if (alreadySeenAnswer && Convert.ToInt32(answerIDs[0]) != -1)
							{
								if (!questionSplit[2].Equals("null"))
								{
									masterDialogueSplit = masterDialogueSplit.Take(i).ToArray<string>().Concat(this.speaker.Dialogue[questionSplit[2]].Split(new char[]
									{
										'#'
									})).ToArray<string>();
									i--;
								}
							}
							else
							{
								this.isLastDialogueInteractive = true;
							}
						}
						else if (stringIdentifier.Equals("$r"))
						{
							string[] responseSplit = masterDialogueSplit[i].Split(new char[]
							{
								' '
							});
							if (this.playerResponses == null)
							{
								this.playerResponses = new List<NPCDialogueResponse>();
							}
							this.isLastDialogueInteractive = true;
							this.playerResponses.Add(new NPCDialogueResponse(Convert.ToInt32(responseSplit[1]), Convert.ToInt32(responseSplit[2]), responseSplit[3], masterDialogueSplit[i + 1]));
							i++;
						}
						else if (stringIdentifier.Equals("$p"))
						{
							string[] prerequisiteSplit = masterDialogueSplit[i].Split(new char[]
							{
								' '
							});
							string[] prerequisiteDialogueSplit = masterDialogueSplit[i + 1].Split(new char[]
							{
								'|'
							});
							bool choseOne = false;
							for (int k = 1; k < prerequisiteSplit.Length; k++)
							{
								if (Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(prerequisiteSplit[1])))
								{
									choseOne = true;
									break;
								}
							}
							if (choseOne)
							{
								masterDialogueSplit = prerequisiteDialogueSplit[0].Split(new char[]
								{
									'#'
								});
								i = -1;
							}
							else
							{
								masterDialogueSplit[i + 1] = masterDialogueSplit[i + 1].Split(new char[]
								{
									'|'
								}).Last<string>();
							}
						}
						else if (stringIdentifier.Equals("$d"))
						{
							string[] arg_474_0 = masterDialogueSplit[i].Split(new char[]
							{
								' '
							});
							string prerequisiteDialogue = masterString.Substring(masterString.IndexOf('#') + 1);
							bool worldStateConfirmed = false;
							string a = arg_474_0[1].ToLower();
							if (!(a == "joja"))
							{
								if (!(a == "cc") && !(a == "communitycenter"))
								{
									if (a == "bus")
									{
										worldStateConfirmed = Game1.player.mailReceived.Contains("ccVault");
									}
								}
								else
								{
									worldStateConfirmed = Game1.isLocationAccessible("CommunityCenter");
								}
							}
							else
							{
								worldStateConfirmed = Game1.isLocationAccessible("JojaMart");
							}
							char toLookFor = prerequisiteDialogue.Contains('|') ? '|' : '#';
							if (worldStateConfirmed)
							{
								masterDialogueSplit = new string[]
								{
									prerequisiteDialogue.Split(new char[]
									{
										toLookFor
									})[0]
								};
							}
							else
							{
								masterDialogueSplit = new string[]
								{
									prerequisiteDialogue.Split(new char[]
									{
										toLookFor
									})[1]
								};
							}
							i--;
						}
						else if (stringIdentifier.Equals("$y"))
						{
							this.quickResponse = true;
							this.isLastDialogueInteractive = true;
							if (this.quickResponses == null)
							{
								this.quickResponses = new List<string>();
							}
							if (this.playerResponses == null)
							{
								this.playerResponses = new List<NPCDialogueResponse>();
							}
							string raw = masterDialogueSplit[i].Substring(masterDialogueSplit[i].IndexOf('\'') + 1);
							raw = raw.Substring(0, raw.Length - 1);
							string[] rawSplit = raw.Split(new char[]
							{
								'_'
							});
							this.dialogues.Add(rawSplit[0]);
							for (int l = 1; l < rawSplit.Length; l += 2)
							{
								this.playerResponses.Add(new NPCDialogueResponse(-1, -1, "quickResponse" + l, Game1.parseText(rawSplit[l])));
								this.quickResponses.Add(rawSplit[l + 1].Replace("*", "#$b#"));
							}
						}
						else if (masterDialogueSplit[i].Contains("^"))
						{
							if (Game1.player.IsMale)
							{
								this.dialogues.Add(masterDialogueSplit[i].Substring(0, masterDialogueSplit[i].IndexOf("^")));
							}
							else
							{
								this.dialogues.Add(masterDialogueSplit[i].Substring(masterDialogueSplit[i].IndexOf("^") + 1));
							}
						}
						else
						{
							this.dialogues.Add(masterDialogueSplit[i]);
						}
					}
				}
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0003E8A8 File Offset: 0x0003CAA8
		public string getCurrentDialogue()
		{
			if (this.currentDialogueIndex >= this.dialogues.Count || this.finishedLastDialogue)
			{
				return "";
			}
			this.showPortrait = true;
			if (this.speaker.name.Equals("Dwarf") && !Game1.player.canUnderstandDwarves)
			{
				return Dialogue.convertToDwarvish(this.dialogues[this.currentDialogueIndex]);
			}
			if (this.temporaryDialogue != null)
			{
				return this.temporaryDialogue;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("}"))
			{
				Game1.player.mailReceived.Add(this.dialogues[this.currentDialogueIndex].Split(new char[]
				{
					'}'
				})[0]);
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf("}") + 1);
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$k", "");
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains('['))
			{
				string items = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf('[') + 1, this.dialogues[this.currentDialogueIndex].IndexOf(']') - this.dialogues[this.currentDialogueIndex].IndexOf('[') - 1);
				string[] split = items.Split(new char[]
				{
					' '
				});
				Game1.player.addItemToInventoryBool(new Object(Vector2.Zero, Convert.ToInt32(split[Game1.random.Next(split.Length)]), null, false, true, false, false), true);
				Game1.player.showCarrying();
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("[" + items + "]", "");
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$k"))
			{
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$k", "");
				this.dialogues.RemoveRange(this.currentDialogueIndex + 1, this.dialogues.Count - 1 - this.currentDialogueIndex);
				this.finishedLastDialogue = true;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Length > 1 && this.dialogues[this.currentDialogueIndex][0] == '%')
			{
				this.showPortrait = false;
				return this.dialogues[this.currentDialogueIndex].Substring(1);
			}
			if (this.dialogues.Count <= 0)
			{
				return "Hello.";
			}
			return this.dialogues[this.currentDialogueIndex].Replace("%time", Game1.getTimeOfDayString(Game1.timeOfDay));
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0003EC3B File Offset: 0x0003CE3B
		public bool isItemGrabDialogue()
		{
			return this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains('[');
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0003EC65 File Offset: 0x0003CE65
		public bool isOnFinalDialogue()
		{
			return this.currentDialogueIndex == this.dialogues.Count - 1;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0003EC7C File Offset: 0x0003CE7C
		public bool isDialogueFinished()
		{
			return this.finishedLastDialogue;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0003EC84 File Offset: 0x0003CE84
		public string checkForSpecialCharacters(string str)
		{
			str = str.Replace("@", Game1.player.Name);
			str = str.Replace("%adj", Dialogue.adjectives[Game1.random.Next(Dialogue.adjectives.Length)].ToLower());
			if (str.Contains("%noun"))
			{
				str = str.Substring(0, str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)].ToLower()) + str.Substring(str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)].ToLower());
			}
			str = str.Replace("%place", Dialogue.places[Game1.random.Next(Dialogue.places.Length)]);
			str = str.Replace("%name", Dialogue.randomName());
			str = str.Replace("%firstnameletter", Game1.player.Name.Substring(0, Math.Max(0, Game1.player.Name.Length / 2)));
			str = str.Replace("%band", Game1.samBandName);
			str = str.Replace("%book", Game1.elliottBookName);
			str = str.Replace("%spouse", Game1.player.spouse);
			str = str.Replace("%farm", Game1.player.farmName);
			str = str.Replace("%favorite", Game1.player.favoriteThing);
			int kids = Game1.player.getNumberOfChildren();
			str = str.Replace("%kid1", (kids > 0) ? Game1.player.getChildren()[0].name : "baby");
			str = str.Replace("%kid2", (kids > 1) ? Game1.player.getChildren()[1].name : "the second baby");
			str = str.Replace("%pet", Game1.player.getPetName());
			if (str.Contains("%fork"))
			{
				str = str.Replace("%fork", "");
				if (Game1.currentLocation.currentEvent != null)
				{
					Game1.currentLocation.currentEvent.specialEventVariable1 = true;
				}
			}
			str = str.Replace("%rival", Utility.getOtherFarmerNames()[0].Split(new char[]
			{
				' '
			})[1]);
			return str;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0003EF24 File Offset: 0x0003D124
		public static string randomName()
		{
			string name = "";
			int nameLength = Game1.random.Next(3, 6);
			string[] startingConsonants = new string[]
			{
				"B",
				"Br",
				"J",
				"F",
				"S",
				"M",
				"C",
				"Ch",
				"L",
				"P",
				"K",
				"W",
				"G",
				"Z",
				"Tr",
				"T",
				"Gr",
				"Fr",
				"Pr",
				"N",
				"Sn",
				"R",
				"Sh",
				"St"
			};
			string[] consonants = new string[]
			{
				"ll",
				"tch",
				"l",
				"m",
				"n",
				"p",
				"r",
				"s",
				"t",
				"c",
				"rt",
				"ts"
			};
			string[] vowels = new string[]
			{
				"a",
				"e",
				"i",
				"o",
				"u"
			};
			string[] consonantEndings = new string[]
			{
				"ie",
				"o",
				"a",
				"ers",
				"ley"
			};
			Dictionary<string, string[]> endings = new Dictionary<string, string[]>();
			Dictionary<string, string[]> endingsForShortNames = new Dictionary<string, string[]>();
			endings.Add("a", new string[]
			{
				"nie",
				"bell",
				"bo",
				"boo",
				"bella",
				"s"
			});
			endings.Add("e", new string[]
			{
				"ll",
				"llo",
				"",
				"o"
			});
			endings.Add("i", new string[]
			{
				"ck",
				"e",
				"bo",
				"ba",
				"lo",
				"la",
				"to",
				"ta",
				"no",
				"na",
				"ni",
				"a",
				"o",
				"zor",
				"que",
				"ca",
				"co",
				"mi"
			});
			endings.Add("o", new string[]
			{
				"nie",
				"ze",
				"dy",
				"da",
				"o",
				"ver",
				"la",
				"lo",
				"s",
				"ny",
				"mo",
				"ra"
			});
			endings.Add("u", new string[]
			{
				"rt",
				"mo",
				"",
				"s"
			});
			endingsForShortNames.Add("a", new string[]
			{
				"nny",
				"sper",
				"trina",
				"bo",
				"-bell",
				"boo",
				"lbert",
				"sko",
				"sh",
				"ck",
				"ishe",
				"rk"
			});
			endingsForShortNames.Add("e", new string[]
			{
				"lla",
				"llo",
				"rnard",
				"cardo",
				"ffe",
				"ppo",
				"ppa",
				"tch",
				"x"
			});
			endingsForShortNames.Add("i", new string[]
			{
				"llard",
				"lly",
				"lbo",
				"cky",
				"card",
				"ne",
				"nnie",
				"lbert",
				"nono",
				"nano",
				"nana",
				"ana",
				"nsy",
				"msy",
				"skers",
				"rdo",
				"rda",
				"sh"
			});
			endingsForShortNames.Add("o", new string[]
			{
				"nie",
				"zzy",
				"do",
				"na",
				"la",
				"la",
				"ver",
				"ng",
				"ngus",
				"ny",
				"-mo",
				"llo",
				"ze",
				"ra",
				"ma",
				"cco",
				"z"
			});
			endingsForShortNames.Add("u", new string[]
			{
				"ssie",
				"bbie",
				"ffy",
				"bba",
				"rt",
				"s",
				"mby",
				"mbo",
				"mbus",
				"ngus",
				"cky"
			});
			name += startingConsonants[Game1.random.Next(startingConsonants.Length - 1)];
			for (int i = 1; i < nameLength - 1; i++)
			{
				if (i % 2 == 0)
				{
					name += consonants[Game1.random.Next(consonants.Length)];
				}
				else
				{
					name += vowels[Game1.random.Next(vowels.Length)];
				}
				if (name.Length >= nameLength)
				{
					break;
				}
			}
			if (Game1.random.NextDouble() < 0.5 && !vowels.Contains(name.ElementAt(name.Length - 1).ToString() ?? ""))
			{
				name += consonantEndings[Game1.random.Next(consonantEndings.Length)];
			}
			else if (vowels.Contains(name.ElementAt(name.Length - 1).ToString() ?? ""))
			{
				if (Game1.random.NextDouble() < 0.8)
				{
					if (name.Length <= 3)
					{
						name += endingsForShortNames[name.ElementAt(name.Length - 1).ToString() ?? ""].ElementAt(Game1.random.Next(endingsForShortNames[name.ElementAt(name.Length - 1).ToString() ?? ""].Length - 1));
					}
					else
					{
						name += endings[name.ElementAt(name.Length - 1).ToString() ?? ""].ElementAt(Game1.random.Next(endings[name.ElementAt(name.Length - 1).ToString() ?? ""].Length - 1));
					}
				}
			}
			else
			{
				name += vowels[Game1.random.Next(vowels.Length)];
			}
			for (int j = name.Length - 1; j > 2; j--)
			{
				if (vowels.Contains(name[j].ToString()) && vowels.Contains(name[j - 2].ToString()))
				{
					char c = name[j - 1];
					if (c != 'c')
					{
						if (c != 'l')
						{
							if (c == 'r')
							{
								name = name.Substring(0, j - 1) + "k" + name.Substring(j);
								j--;
							}
						}
						else
						{
							name = name.Substring(0, j - 1) + "n" + name.Substring(j);
							j--;
						}
					}
					else
					{
						name = name.Substring(0, j) + "k" + name.Substring(j);
						j--;
					}
				}
			}
			if (name.Length <= 3 && Game1.random.NextDouble() < 0.1)
			{
				name = ((Game1.random.NextDouble() < 0.5) ? (name + name) : (name + "-" + name));
			}
			if (name.Length <= 2 && name.Last<char>() == 'e')
			{
				name += ((Game1.random.NextDouble() < 0.3) ? "m" : ((Game1.random.NextDouble() < 0.5) ? "p" : "b"));
			}
			if (name.ToLower().Contains("sex") || name.ToLower().Contains("taboo") || name.ToLower().Contains("fuck") || name.ToLower().Contains("rape") || name.ToLower().Contains("cock") || name.ToLower().Contains("willy") || name.ToLower().Contains("cum") || name.ToLower().Contains("goock") || name.ToLower().Contains("trann") || name.ToLower().Contains("gook") || name.ToLower().Contains("bitch") || name.ToLower().Contains("shit") || name.ToLower().Contains("pusie") || name.ToLower().Contains("kike") || name.ToLower().Contains("nigg") || name.ToLower().Contains("puss"))
			{
				name = ((Game1.random.NextDouble() < 0.5) ? "Bobo" : "Wumbus");
			}
			return name;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0003FA2C File Offset: 0x0003DC2C
		public string exitCurrentDialogue()
		{
			if (this.temporaryDialogue != null)
			{
				return null;
			}
			bool arg_42_0 = this.isCurrentStringContinuedOnNextScreen;
			if (this.currentDialogueIndex < this.dialogues.Count - 1)
			{
				this.currentDialogueIndex++;
				this.checkForSpecialDialogueAttributes();
			}
			else
			{
				this.finishedLastDialogue = true;
			}
			if (arg_42_0)
			{
				return this.getCurrentDialogue();
			}
			return null;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0003FA88 File Offset: 0x0003DC88
		private void checkForSpecialDialogueAttributes()
		{
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("{"))
			{
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("{", "");
				this.isCurrentStringContinuedOnNextScreen = true;
			}
			else
			{
				this.isCurrentStringContinuedOnNextScreen = false;
			}
			this.checkEmotions();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0003FB08 File Offset: 0x0003DD08
		private void checkEmotions()
		{
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$h"))
			{
				this.currentEmotion = "$h";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$h", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$s"))
			{
				this.currentEmotion = "$s";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$s", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$u"))
			{
				this.currentEmotion = "$u";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$u", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$l"))
			{
				this.currentEmotion = "$l";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$l", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$a"))
			{
				this.currentEmotion = "$a";
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$a", "");
				return;
			}
			if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$"))
			{
				int digits = (this.dialogues[this.currentDialogueIndex].Length > this.dialogues[this.currentDialogueIndex].IndexOf("$") + 2 && char.IsDigit(this.dialogues[this.currentDialogueIndex][this.dialogues[this.currentDialogueIndex].IndexOf("$") + 2])) ? 2 : 1;
				string emote = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf("$"), digits + 1);
				this.currentEmotion = emote;
				this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace(emote, "");
				return;
			}
			this.currentEmotion = "$neutral";
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0003FE37 File Offset: 0x0003E037
		public List<NPCDialogueResponse> getNPCResponseOptions()
		{
			return this.playerResponses;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0003FE3F File Offset: 0x0003E03F
		public List<Response> getResponseOptions()
		{
			IEnumerable<NPCDialogueResponse> arg_25_0 = this.playerResponses;
			Func<NPCDialogueResponse, Response> arg_25_1;
			if ((arg_25_1 = Dialogue.<>c.<>9__80_0) == null)
			{
				arg_25_1 = (Dialogue.<>c.<>9__80_0 = new Func<NPCDialogueResponse, Response>(Dialogue.<>c.<>9.<getResponseOptions>b__80_0));
			}
			return new List<Response>(arg_25_0.Select(arg_25_1));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0003FE70 File Offset: 0x0003E070
		public bool isCurrentDialogueAQuestion()
		{
			return this.isLastDialogueInteractive && this.currentDialogueIndex == this.dialogues.Count - 1;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0003FE94 File Offset: 0x0003E094
		public bool chooseResponse(Response response)
		{
			int i = 0;
			while (i < this.playerResponses.Count)
			{
				if (this.playerResponses[i].responseKey != null && response.responseKey != null && this.playerResponses[i].responseKey.Equals(response.responseKey))
				{
					if (this.answerQuestionBehavior != null)
					{
						if (this.answerQuestionBehavior(i))
						{
							Game1.currentSpeaker = null;
						}
						this.isLastDialogueInteractive = false;
						this.finishedLastDialogue = true;
						this.answerQuestionBehavior = null;
						return true;
					}
					if (this.quickResponse)
					{
						this.isLastDialogueInteractive = false;
						this.finishedLastDialogue = true;
						this.isCurrentStringContinuedOnNextScreen = true;
						this.speaker.setNewDialogue(this.quickResponses[i], false, false);
						Game1.drawDialogue(this.speaker);
						this.speaker.faceTowardFarmerForPeriod(4000, 3, false, Game1.player);
						return true;
					}
					if (Game1.isFestival())
					{
						Game1.currentLocation.currentEvent.answerDialogueQuestion(this.speaker, this.playerResponses[i].responseKey);
						this.isLastDialogueInteractive = false;
						this.finishedLastDialogue = true;
						return false;
					}
					Game1.player.changeFriendship(this.playerResponses[i].friendshipChange, this.speaker);
					if (this.playerResponses[i].id != -1)
					{
						Game1.player.addSeenResponse(this.playerResponses[i].id);
					}
					this.isLastDialogueInteractive = false;
					this.finishedLastDialogue = false;
					this.parseDialogueString(this.speaker.Dialogue[this.playerResponses[i].responseKey]);
					this.isCurrentStringContinuedOnNextScreen = true;
					return false;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00040058 File Offset: 0x0003E258
		public static string convertToDwarvish(string str)
		{
			string translated = "";
			int i = 0;
			while (i < str.Length)
			{
				char c = str[i];
				if (c <= '?')
				{
					if (c <= '\'')
					{
						if (c != '\n')
						{
							switch (c)
							{
							case ' ':
							case '!':
							case '"':
								goto IL_2CB;
							default:
								if (c != '\'')
								{
									goto IL_2E3;
								}
								goto IL_2CB;
							}
						}
					}
					else if (c <= '5')
					{
						switch (c)
						{
						case ',':
						case '.':
							goto IL_2CB;
						case '-':
						case '/':
							goto IL_2E3;
						case '0':
							translated += "Q";
							break;
						case '1':
							translated += "M";
							break;
						default:
							if (c != '5')
							{
								goto IL_2E3;
							}
							translated += "X";
							break;
						}
					}
					else if (c != '9')
					{
						if (c != '?')
						{
							goto IL_2E3;
						}
						goto IL_2CB;
					}
					else
					{
						translated += "V";
					}
				}
				else if (c <= 'I')
				{
					if (c != 'A')
					{
						if (c != 'E')
						{
							if (c != 'I')
							{
								goto IL_2E3;
							}
							translated += "E";
						}
						else
						{
							translated += "U";
						}
					}
					else
					{
						translated += "O";
					}
				}
				else if (c <= 'u')
				{
					if (c != 'O')
					{
						switch (c)
						{
						case 'U':
							translated += "I";
							break;
						case 'V':
						case 'W':
						case 'X':
						case '[':
						case '\\':
						case ']':
						case '^':
						case '_':
						case '`':
						case 'b':
						case 'f':
						case 'j':
						case 'k':
						case 'l':
						case 'q':
						case 'r':
							goto IL_2E3;
						case 'Y':
							translated += "Ol";
							break;
						case 'Z':
							translated += "B";
							break;
						case 'a':
							translated += "o";
							break;
						case 'c':
							translated += "t";
							break;
						case 'd':
							translated += "p";
							break;
						case 'e':
							translated += "u";
							break;
						case 'g':
							translated += "l";
							break;
						case 'h':
						case 'm':
						case 's':
							goto IL_2CB;
						case 'i':
							translated += "e";
							break;
						case 'n':
						case 'p':
							break;
						case 'o':
							translated += "a";
							break;
						case 't':
							translated += "n";
							break;
						case 'u':
							translated += "i";
							break;
						default:
							goto IL_2E3;
						}
					}
					else
					{
						translated += "A";
					}
				}
				else if (c != 'y')
				{
					if (c != 'z')
					{
						goto IL_2E3;
					}
					translated += "b";
				}
				else
				{
					translated += "ol";
				}
				IL_30A:
				i++;
				continue;
				IL_2CB:
				translated += str[i].ToString();
				goto IL_30A;
				IL_2E3:
				if (char.IsLetterOrDigit(str[i]))
				{
					translated += (str[i] + '\u0002').ToString();
					goto IL_30A;
				}
				goto IL_30A;
			}
			return translated.Replace("nhu", "doo");
		}

		// Token: 0x0400037F RID: 895
		public const string dialogueHappy = "$h";

		// Token: 0x04000380 RID: 896
		public const string dialogueSad = "$s";

		// Token: 0x04000381 RID: 897
		public const string dialogueUnique = "$u";

		// Token: 0x04000382 RID: 898
		public const string dialogueNeutral = "$neutral";

		// Token: 0x04000383 RID: 899
		public const string dialogueLove = "$l";

		// Token: 0x04000384 RID: 900
		public const string dialogueAngry = "$a";

		// Token: 0x04000385 RID: 901
		public const string dialogueEnd = "$e";

		// Token: 0x04000386 RID: 902
		public const string dialogueBreak = "$b";

		// Token: 0x04000387 RID: 903
		public const string dialogueKill = "$k";

		// Token: 0x04000388 RID: 904
		public const string dialogueChance = "$c";

		// Token: 0x04000389 RID: 905
		public const string dialogueDependingOnWorldState = "$d";

		// Token: 0x0400038A RID: 906
		public const string dialogueQuickResponse = "$y";

		// Token: 0x0400038B RID: 907
		public const string dialoguePrerequisite = "$p";

		// Token: 0x0400038C RID: 908
		public const string dialogueSingle = "$1";

		// Token: 0x0400038D RID: 909
		public const string dialogueQuestion = "$q";

		// Token: 0x0400038E RID: 910
		public const string dialogueResponse = "$r";

		// Token: 0x0400038F RID: 911
		public const string breakSpecialCharacter = "{";

		// Token: 0x04000390 RID: 912
		public const string playerNameSpecialCharacter = "@";

		// Token: 0x04000391 RID: 913
		public const string genderDialogueSplitCharacter = "^";

		// Token: 0x04000392 RID: 914
		public const string quickResponseDelineator = "*";

		// Token: 0x04000393 RID: 915
		public const string randomAdjectiveSpecialCharacter = "%adj";

		// Token: 0x04000394 RID: 916
		public const string randomNounSpecialCharacter = "%noun";

		// Token: 0x04000395 RID: 917
		public const string randomPlaceSpecialCharacter = "%place";

		// Token: 0x04000396 RID: 918
		public const string spouseSpecialCharacter = "%spouse";

		// Token: 0x04000397 RID: 919
		public const string randomNameSpecialCharacter = "%name";

		// Token: 0x04000398 RID: 920
		public const string firstNameLettersSpecialCharacter = "%firstnameletter";

		// Token: 0x04000399 RID: 921
		public const string timeSpecialCharacter = "%time";

		// Token: 0x0400039A RID: 922
		public const string bandNameSpecialCharacter = "%band";

		// Token: 0x0400039B RID: 923
		public const string bookNameSpecialCharacter = "%book";

		// Token: 0x0400039C RID: 924
		public const string rivalSpecialCharacter = "%rival";

		// Token: 0x0400039D RID: 925
		public const string petSpecialCharacter = "%pet";

		// Token: 0x0400039E RID: 926
		public const string farmNameSpecialCharacter = "%farm";

		// Token: 0x0400039F RID: 927
		public const string favoriteThingSpecialCharacter = "%favorite";

		// Token: 0x040003A0 RID: 928
		public const string eventForkSpecialCharacter = "%fork";

		// Token: 0x040003A1 RID: 929
		public const string kid1specialCharacter = "%kid1";

		// Token: 0x040003A2 RID: 930
		public const string kid2SpecialCharacter = "%kid2";

		// Token: 0x040003A3 RID: 931
		public static string[] adjectives = new string[]
		{
			"Purple",
			"Gooey",
			"Chalky",
			"Green",
			"Plush",
			"Chunky",
			"Gigantic",
			"Greasy",
			"Gloomy",
			"Practical",
			"Lanky",
			"Dopey",
			"Crusty",
			"Fantastic",
			"Rubbery",
			"Silly",
			"Courageous",
			"Reasonable",
			"Lonely",
			"Bitter"
		};

		// Token: 0x040003A4 RID: 932
		public static string[] nouns = new string[]
		{
			"Dragon",
			"Buffet",
			"Biscuit",
			"Robot",
			"Planet",
			"Pepper",
			"Tomb",
			"Hyena",
			"Lip",
			"Quail",
			"Cheese",
			"Disaster",
			"Raincoat",
			"Shoe",
			"Castle",
			"Elf",
			"Pump",
			"Crisp",
			"Wig",
			"Mermaid",
			"Drumstick",
			"Puppet",
			"Submarine"
		};

		// Token: 0x040003A5 RID: 933
		public static string[] verbs = new string[]
		{
			"ran",
			"danced",
			"spoke",
			"galloped",
			"ate",
			"floated",
			"stood",
			"flowed",
			"smelled",
			"swam",
			"grilled",
			"cracked",
			"melted"
		};

		// Token: 0x040003A6 RID: 934
		public static string[] positional = new string[]
		{
			"atop",
			"near",
			"with",
			"alongside",
			"away from",
			"too close to",
			"dangerously close to",
			"far, far away from",
			"uncomfortably close to",
			"way above the",
			"miles below",
			"on a different planet from",
			"in a different century than"
		};

		// Token: 0x040003A7 RID: 935
		public static string[] places = new string[]
		{
			"Castle Village",
			"Basket Town",
			"Pine Mesa City",
			"Point Drake",
			"Minister Valley",
			"Grampleton",
			"Zuzu City",
			"a small island off the coast",
			"Fort Josa",
			"Chestervale",
			"Fern Islands",
			"Tanker Grove"
		};

		// Token: 0x040003A8 RID: 936
		public static string[] colors = new string[]
		{
			"/crimson",
			"/green",
			"/tan",
			"/purple",
			"/deep blue",
			"/neon pink",
			"/pale/yellow",
			"/chocolate/brown",
			"/sky/blue",
			"/bubblegum/pink",
			"/blood/red",
			"/bright/orange",
			"n/aquamarine",
			"/silvery",
			"/glimmering/gold",
			"/rainbow"
		};

		// Token: 0x040003A9 RID: 937
		private List<string> dialogues = new List<string>();

		// Token: 0x040003AA RID: 938
		private List<NPCDialogueResponse> playerResponses;

		// Token: 0x040003AB RID: 939
		private List<string> quickResponses;

		// Token: 0x040003AC RID: 940
		private bool isLastDialogueInteractive;

		// Token: 0x040003AD RID: 941
		private bool quickResponse;

		// Token: 0x040003AE RID: 942
		public bool isCurrentStringContinuedOnNextScreen;

		// Token: 0x040003AF RID: 943
		private bool dialogueToBeKilled;

		// Token: 0x040003B0 RID: 944
		private bool finishedLastDialogue;

		// Token: 0x040003B1 RID: 945
		public bool showPortrait;

		// Token: 0x040003B2 RID: 946
		public bool removeOnNextMove;

		// Token: 0x040003B3 RID: 947
		public int currentDialogueIndex;

		// Token: 0x040003B4 RID: 948
		private string currentEmotion;

		// Token: 0x040003B5 RID: 949
		public string temporaryDialogue;

		// Token: 0x040003B6 RID: 950
		public NPC speaker;

		// Token: 0x040003B7 RID: 951
		public Dialogue.onAnswerQuestion answerQuestionBehavior;

		// Token: 0x0200016B RID: 363
		// Token: 0x06001369 RID: 4969
		public delegate bool onAnswerQuestion(int whichResponse);

		// Token: 0x0200016C RID: 364
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600136E RID: 4974 RVA: 0x00184BB2 File Offset: 0x00182DB2
			internal Response <getResponseOptions>b__80_0(NPCDialogueResponse x)
			{
				return x;
			}

			// Token: 0x04001442 RID: 5186
			public static readonly Dialogue.<>c <>9 = new Dialogue.<>c();

			// Token: 0x04001443 RID: 5187
			public static Func<NPCDialogueResponse, Response> <>9__80_0;
		}
	}
}
