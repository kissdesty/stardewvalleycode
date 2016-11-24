using System;
using System.Collections.Generic;
using StardewValley.Locations;

namespace StardewValley
{
	// Token: 0x02000050 RID: 80
	public class Stats
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x000A5390 File Offset: 0x000A3590
		public void monsterKilled(string name)
		{
			if (this.specificMonstersKilled.ContainsKey(name))
			{
				int num;
				if (!AdventureGuild.willThisKillCompleteAMonsterSlayerQuest(name))
				{
					SerializableDictionary<string, int> expr_53 = this.specificMonstersKilled;
					num = expr_53[name];
					expr_53[name] = num + 1;
					return;
				}
				Game1.showGlobalMessage("Monster Slayer Goal Complete! See Gil for your reward.");
				SerializableDictionary<string, int> expr_28 = this.specificMonstersKilled;
				num = expr_28[name];
				expr_28[name] = num + 1;
				if (AdventureGuild.areAllMonsterSlayerQuestsComplete())
				{
					Game1.getSteamAchievement("Achievement_KeeperOfTheMysticRings");
					return;
				}
			}
			else
			{
				this.specificMonstersKilled.Add(name, 1);
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000A540F File Offset: 0x000A360F
		public int getMonstersKilled(string name)
		{
			if (this.specificMonstersKilled.ContainsKey(name))
			{
				return this.specificMonstersKilled[name];
			}
			return 0;
		}

		// Token: 0x1700007F RID: 127
		public uint CropsShipped
		{
			// Token: 0x06000703 RID: 1795 RVA: 0x000A542D File Offset: 0x000A362D
			get
			{
				return this.cropsShipped;
			}
			// Token: 0x06000704 RID: 1796 RVA: 0x000A5435 File Offset: 0x000A3635
			set
			{
				this.cropsShipped = value;
			}
		}

		// Token: 0x17000080 RID: 128
		public uint GeodesCracked
		{
			// Token: 0x06000705 RID: 1797 RVA: 0x000A543E File Offset: 0x000A363E
			get
			{
				return this.geodesCracked;
			}
			// Token: 0x06000706 RID: 1798 RVA: 0x000A5446 File Offset: 0x000A3646
			set
			{
				this.geodesCracked = value;
			}
		}

		// Token: 0x17000081 RID: 129
		public uint SlimesKilled
		{
			// Token: 0x06000707 RID: 1799 RVA: 0x000A544F File Offset: 0x000A364F
			get
			{
				return this.slimesKilled;
			}
			// Token: 0x06000708 RID: 1800 RVA: 0x000A5457 File Offset: 0x000A3657
			set
			{
				this.slimesKilled = value;
			}
		}

		// Token: 0x17000082 RID: 130
		public uint StarLevelCropsShipped
		{
			// Token: 0x06000709 RID: 1801 RVA: 0x000A5460 File Offset: 0x000A3660
			get
			{
				return this.starLevelCropsShipped;
			}
			// Token: 0x0600070A RID: 1802 RVA: 0x000A5468 File Offset: 0x000A3668
			set
			{
				this.starLevelCropsShipped = value;
				this.checkForStarCropsAchievements();
			}
		}

		// Token: 0x17000083 RID: 131
		public uint StoneGathered
		{
			// Token: 0x0600070B RID: 1803 RVA: 0x000A5477 File Offset: 0x000A3677
			get
			{
				return this.stoneGathered;
			}
			// Token: 0x0600070C RID: 1804 RVA: 0x000A547F File Offset: 0x000A367F
			set
			{
				this.stoneGathered = value;
				this.checkForStoneAchievements();
			}
		}

		// Token: 0x17000084 RID: 132
		public uint QuestsCompleted
		{
			// Token: 0x0600070D RID: 1805 RVA: 0x000A548E File Offset: 0x000A368E
			get
			{
				return this.questsCompleted;
			}
			// Token: 0x0600070E RID: 1806 RVA: 0x000A5496 File Offset: 0x000A3696
			set
			{
				this.questsCompleted = value;
				this.checkForQuestAchievements();
			}
		}

		// Token: 0x17000085 RID: 133
		public uint FishCaught
		{
			// Token: 0x0600070F RID: 1807 RVA: 0x000A54A5 File Offset: 0x000A36A5
			get
			{
				return this.fishCaught;
			}
			// Token: 0x06000710 RID: 1808 RVA: 0x000A54AD File Offset: 0x000A36AD
			set
			{
				this.fishCaught = value;
			}
		}

		// Token: 0x17000086 RID: 134
		public uint NotesFound
		{
			// Token: 0x06000711 RID: 1809 RVA: 0x000A54B6 File Offset: 0x000A36B6
			get
			{
				return this.notesFound;
			}
			// Token: 0x06000712 RID: 1810 RVA: 0x000A54BE File Offset: 0x000A36BE
			set
			{
				this.notesFound = value;
			}
		}

		// Token: 0x17000087 RID: 135
		public uint SticksChopped
		{
			// Token: 0x06000713 RID: 1811 RVA: 0x000A54C7 File Offset: 0x000A36C7
			get
			{
				return this.sticksChopped;
			}
			// Token: 0x06000714 RID: 1812 RVA: 0x000A54CF File Offset: 0x000A36CF
			set
			{
				this.sticksChopped = value;
				this.checkForWoodAchievements();
			}
		}

		// Token: 0x17000088 RID: 136
		public uint WeedsEliminated
		{
			// Token: 0x06000715 RID: 1813 RVA: 0x000A54DE File Offset: 0x000A36DE
			get
			{
				return this.weedsEliminated;
			}
			// Token: 0x06000716 RID: 1814 RVA: 0x000A54E6 File Offset: 0x000A36E6
			set
			{
				this.weedsEliminated = value;
			}
		}

		// Token: 0x17000089 RID: 137
		public uint DaysPlayed
		{
			// Token: 0x06000717 RID: 1815 RVA: 0x000A54EF File Offset: 0x000A36EF
			get
			{
				return this.daysPlayed;
			}
			// Token: 0x06000718 RID: 1816 RVA: 0x000A54F7 File Offset: 0x000A36F7
			set
			{
				this.daysPlayed = value;
			}
		}

		// Token: 0x1700008A RID: 138
		public uint BouldersCracked
		{
			// Token: 0x06000719 RID: 1817 RVA: 0x000A5500 File Offset: 0x000A3700
			get
			{
				return this.bouldersCracked;
			}
			// Token: 0x0600071A RID: 1818 RVA: 0x000A5508 File Offset: 0x000A3708
			set
			{
				this.bouldersCracked = value;
			}
		}

		// Token: 0x1700008B RID: 139
		public uint MysticStonesCrushed
		{
			// Token: 0x0600071B RID: 1819 RVA: 0x000A5511 File Offset: 0x000A3711
			get
			{
				return this.mysticStonesCrushed;
			}
			// Token: 0x0600071C RID: 1820 RVA: 0x000A5519 File Offset: 0x000A3719
			set
			{
				this.mysticStonesCrushed = value;
			}
		}

		// Token: 0x1700008C RID: 140
		public uint GoatCheeseMade
		{
			// Token: 0x0600071D RID: 1821 RVA: 0x000A5522 File Offset: 0x000A3722
			get
			{
				return this.goatCheeseMade;
			}
			// Token: 0x0600071E RID: 1822 RVA: 0x000A552A File Offset: 0x000A372A
			set
			{
				this.goatCheeseMade = value;
				this.checkForCheeseAchievements();
			}
		}

		// Token: 0x1700008D RID: 141
		public uint CheeseMade
		{
			// Token: 0x0600071F RID: 1823 RVA: 0x000A5539 File Offset: 0x000A3739
			get
			{
				return this.cheeseMade;
			}
			// Token: 0x06000720 RID: 1824 RVA: 0x000A5541 File Offset: 0x000A3741
			set
			{
				this.cheeseMade = value;
				this.checkForCheeseAchievements();
			}
		}

		// Token: 0x1700008E RID: 142
		public uint PiecesOfTrashRecycled
		{
			// Token: 0x06000721 RID: 1825 RVA: 0x000A5550 File Offset: 0x000A3750
			get
			{
				return this.piecesOfTrashRecycled;
			}
			// Token: 0x06000722 RID: 1826 RVA: 0x000A5558 File Offset: 0x000A3758
			set
			{
				this.piecesOfTrashRecycled = value;
			}
		}

		// Token: 0x1700008F RID: 143
		public uint PreservesMade
		{
			// Token: 0x06000723 RID: 1827 RVA: 0x000A5561 File Offset: 0x000A3761
			get
			{
				return this.preservesMade;
			}
			// Token: 0x06000724 RID: 1828 RVA: 0x000A5569 File Offset: 0x000A3769
			set
			{
				this.preservesMade = value;
			}
		}

		// Token: 0x17000090 RID: 144
		public uint BeveragesMade
		{
			// Token: 0x06000725 RID: 1829 RVA: 0x000A5572 File Offset: 0x000A3772
			get
			{
				return this.beveragesMade;
			}
			// Token: 0x06000726 RID: 1830 RVA: 0x000A557A File Offset: 0x000A377A
			set
			{
				this.beveragesMade = value;
			}
		}

		// Token: 0x17000091 RID: 145
		public uint BarsSmelted
		{
			// Token: 0x06000727 RID: 1831 RVA: 0x000A5583 File Offset: 0x000A3783
			get
			{
				return this.barsSmelted;
			}
			// Token: 0x06000728 RID: 1832 RVA: 0x000A558B File Offset: 0x000A378B
			set
			{
				this.barsSmelted = value;
			}
		}

		// Token: 0x17000092 RID: 146
		public uint IridiumFound
		{
			// Token: 0x06000729 RID: 1833 RVA: 0x000A5594 File Offset: 0x000A3794
			get
			{
				return this.iridiumFound;
			}
			// Token: 0x0600072A RID: 1834 RVA: 0x000A559C File Offset: 0x000A379C
			set
			{
				this.iridiumFound = value;
				this.checkForIridiumOreAchievements();
			}
		}

		// Token: 0x17000093 RID: 147
		public uint GoldFound
		{
			// Token: 0x0600072B RID: 1835 RVA: 0x000A55AB File Offset: 0x000A37AB
			get
			{
				return this.goldFound;
			}
			// Token: 0x0600072C RID: 1836 RVA: 0x000A55B3 File Offset: 0x000A37B3
			set
			{
				this.goldFound = value;
				this.checkForGoldOreAchievements();
			}
		}

		// Token: 0x17000094 RID: 148
		public uint CoinsFound
		{
			// Token: 0x0600072D RID: 1837 RVA: 0x000A55C2 File Offset: 0x000A37C2
			get
			{
				return this.coinsFound;
			}
			// Token: 0x0600072E RID: 1838 RVA: 0x000A55CA File Offset: 0x000A37CA
			set
			{
				this.coinsFound = value;
			}
		}

		// Token: 0x17000095 RID: 149
		public uint CoalFound
		{
			// Token: 0x0600072F RID: 1839 RVA: 0x000A55D3 File Offset: 0x000A37D3
			get
			{
				return this.coalFound;
			}
			// Token: 0x06000730 RID: 1840 RVA: 0x000A55DB File Offset: 0x000A37DB
			set
			{
				this.coalFound = value;
				this.checkForCoalOreAchievements();
			}
		}

		// Token: 0x17000096 RID: 150
		public uint IronFound
		{
			// Token: 0x06000731 RID: 1841 RVA: 0x000A55EA File Offset: 0x000A37EA
			get
			{
				return this.ironFound;
			}
			// Token: 0x06000732 RID: 1842 RVA: 0x000A55F2 File Offset: 0x000A37F2
			set
			{
				this.ironFound = value;
				this.checkForIronOreAchievements();
			}
		}

		// Token: 0x17000097 RID: 151
		public uint CopperFound
		{
			// Token: 0x06000733 RID: 1843 RVA: 0x000A5601 File Offset: 0x000A3801
			get
			{
				return this.copperFound;
			}
			// Token: 0x06000734 RID: 1844 RVA: 0x000A5609 File Offset: 0x000A3809
			set
			{
				this.copperFound = value;
				this.checkForCopperOreAchievements();
			}
		}

		// Token: 0x17000098 RID: 152
		public uint CaveCarrotsFound
		{
			// Token: 0x06000735 RID: 1845 RVA: 0x000A5618 File Offset: 0x000A3818
			get
			{
				return this.caveCarrotsFound;
			}
			// Token: 0x06000736 RID: 1846 RVA: 0x000A5620 File Offset: 0x000A3820
			set
			{
				this.caveCarrotsFound = value;
			}
		}

		// Token: 0x17000099 RID: 153
		public uint OtherPreciousGemsFound
		{
			// Token: 0x06000737 RID: 1847 RVA: 0x000A5629 File Offset: 0x000A3829
			get
			{
				return this.otherPreciousGemsFound;
			}
			// Token: 0x06000738 RID: 1848 RVA: 0x000A5631 File Offset: 0x000A3831
			set
			{
				this.otherPreciousGemsFound = value;
			}
		}

		// Token: 0x1700009A RID: 154
		public uint PrismaticShardsFound
		{
			// Token: 0x06000739 RID: 1849 RVA: 0x000A563A File Offset: 0x000A383A
			get
			{
				return this.prismaticShardsFound;
			}
			// Token: 0x0600073A RID: 1850 RVA: 0x000A5642 File Offset: 0x000A3842
			set
			{
				this.prismaticShardsFound = value;
			}
		}

		// Token: 0x1700009B RID: 155
		public uint DiamondsFound
		{
			// Token: 0x0600073B RID: 1851 RVA: 0x000A564B File Offset: 0x000A384B
			get
			{
				return this.diamondsFound;
			}
			// Token: 0x0600073C RID: 1852 RVA: 0x000A5653 File Offset: 0x000A3853
			set
			{
				this.diamondsFound = value;
			}
		}

		// Token: 0x1700009C RID: 156
		public uint MonstersKilled
		{
			// Token: 0x0600073D RID: 1853 RVA: 0x000A565C File Offset: 0x000A385C
			get
			{
				return this.monstersKilled;
			}
			// Token: 0x0600073E RID: 1854 RVA: 0x000A5664 File Offset: 0x000A3864
			set
			{
				this.monstersKilled = value;
			}
		}

		// Token: 0x1700009D RID: 157
		public uint StepsTaken
		{
			// Token: 0x0600073F RID: 1855 RVA: 0x000A566D File Offset: 0x000A386D
			get
			{
				return this.stepsTaken;
			}
			// Token: 0x06000740 RID: 1856 RVA: 0x000A5675 File Offset: 0x000A3875
			set
			{
				this.stepsTaken = value;
			}
		}

		// Token: 0x1700009E RID: 158
		public uint StumpsChopped
		{
			// Token: 0x06000741 RID: 1857 RVA: 0x000A567E File Offset: 0x000A387E
			get
			{
				return this.stumpsChopped;
			}
			// Token: 0x06000742 RID: 1858 RVA: 0x000A5686 File Offset: 0x000A3886
			set
			{
				this.stumpsChopped = value;
				this.checkForWoodAchievements();
			}
		}

		// Token: 0x1700009F RID: 159
		public uint TimesFished
		{
			// Token: 0x06000743 RID: 1859 RVA: 0x000A5695 File Offset: 0x000A3895
			get
			{
				return this.timesFished;
			}
			// Token: 0x06000744 RID: 1860 RVA: 0x000A569D File Offset: 0x000A389D
			set
			{
				this.timesFished = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		public uint AverageBedtime
		{
			// Token: 0x06000745 RID: 1861 RVA: 0x000A56A6 File Offset: 0x000A38A6
			get
			{
				return this.averageBedtime;
			}
			// Token: 0x06000746 RID: 1862 RVA: 0x000A56AE File Offset: 0x000A38AE
			set
			{
				this.averageBedtime = (this.averageBedtime * (this.daysPlayed - 1u) + value) / this.daysPlayed;
			}
		}

		// Token: 0x170000A1 RID: 161
		public uint TimesUnconscious
		{
			// Token: 0x06000747 RID: 1863 RVA: 0x000A56CE File Offset: 0x000A38CE
			get
			{
				return this.timesUnconscious;
			}
			// Token: 0x06000748 RID: 1864 RVA: 0x000A56D6 File Offset: 0x000A38D6
			set
			{
				this.timesUnconscious = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		public uint GiftsGiven
		{
			// Token: 0x06000749 RID: 1865 RVA: 0x000A56DF File Offset: 0x000A38DF
			get
			{
				return this.giftsGiven;
			}
			// Token: 0x0600074A RID: 1866 RVA: 0x000A56E7 File Offset: 0x000A38E7
			set
			{
				this.giftsGiven = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		public uint DirtHoed
		{
			// Token: 0x0600074B RID: 1867 RVA: 0x000A56F0 File Offset: 0x000A38F0
			get
			{
				return this.dirtHoed;
			}
			// Token: 0x0600074C RID: 1868 RVA: 0x000A56F8 File Offset: 0x000A38F8
			set
			{
				this.dirtHoed = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		public uint RocksCrushed
		{
			// Token: 0x0600074D RID: 1869 RVA: 0x000A5701 File Offset: 0x000A3901
			get
			{
				return this.rocksCrushed;
			}
			// Token: 0x0600074E RID: 1870 RVA: 0x000A5709 File Offset: 0x000A3909
			set
			{
				this.rocksCrushed = value;
				this.checkForStoneBreakAchievements();
			}
		}

		// Token: 0x170000A5 RID: 165
		public uint TrufflesFound
		{
			// Token: 0x0600074F RID: 1871 RVA: 0x000A5718 File Offset: 0x000A3918
			get
			{
				return this.trufflesFound;
			}
			// Token: 0x06000750 RID: 1872 RVA: 0x000A5720 File Offset: 0x000A3920
			set
			{
				this.trufflesFound = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		public uint SheepWoolProduced
		{
			// Token: 0x06000751 RID: 1873 RVA: 0x000A5729 File Offset: 0x000A3929
			get
			{
				return this.sheepWoolProduced;
			}
			// Token: 0x06000752 RID: 1874 RVA: 0x000A5731 File Offset: 0x000A3931
			set
			{
				this.sheepWoolProduced = value;
				this.checkForWoolAchievements();
			}
		}

		// Token: 0x170000A7 RID: 167
		public uint RabbitWoolProduced
		{
			// Token: 0x06000753 RID: 1875 RVA: 0x000A5740 File Offset: 0x000A3940
			get
			{
				return this.rabbitWoolProduced;
			}
			// Token: 0x06000754 RID: 1876 RVA: 0x000A5748 File Offset: 0x000A3948
			set
			{
				this.rabbitWoolProduced = value;
				this.checkForWoolAchievements();
			}
		}

		// Token: 0x170000A8 RID: 168
		public uint GoatMilkProduced
		{
			// Token: 0x06000755 RID: 1877 RVA: 0x000A5757 File Offset: 0x000A3957
			get
			{
				return this.goatMilkProduced;
			}
			// Token: 0x06000756 RID: 1878 RVA: 0x000A575F File Offset: 0x000A395F
			set
			{
				this.goatMilkProduced = value;
				this.checkForGoatMilkAchievements();
			}
		}

		// Token: 0x170000A9 RID: 169
		public uint CowMilkProduced
		{
			// Token: 0x06000757 RID: 1879 RVA: 0x000A576E File Offset: 0x000A396E
			get
			{
				return this.cowMilkProduced;
			}
			// Token: 0x06000758 RID: 1880 RVA: 0x000A5776 File Offset: 0x000A3976
			set
			{
				this.cowMilkProduced = value;
				this.checkForCowMilkAchievements();
			}
		}

		// Token: 0x170000AA RID: 170
		public uint DuckEggsLayed
		{
			// Token: 0x06000759 RID: 1881 RVA: 0x000A5785 File Offset: 0x000A3985
			get
			{
				return this.duckEggsLayed;
			}
			// Token: 0x0600075A RID: 1882 RVA: 0x000A578D File Offset: 0x000A398D
			set
			{
				this.duckEggsLayed = value;
				this.checkForDuckEggAchievements();
			}
		}

		// Token: 0x170000AB RID: 171
		public uint ItemsCrafted
		{
			// Token: 0x0600075B RID: 1883 RVA: 0x000A579C File Offset: 0x000A399C
			get
			{
				return this.itemsCrafted;
			}
			// Token: 0x0600075C RID: 1884 RVA: 0x000A57A4 File Offset: 0x000A39A4
			set
			{
				this.itemsCrafted = value;
				this.checkForCraftingAchievements();
			}
		}

		// Token: 0x170000AC RID: 172
		public uint ChickenEggsLayed
		{
			// Token: 0x0600075D RID: 1885 RVA: 0x000A57B3 File Offset: 0x000A39B3
			get
			{
				return this.chickenEggsLayed;
			}
			// Token: 0x0600075E RID: 1886 RVA: 0x000A57BB File Offset: 0x000A39BB
			set
			{
				this.chickenEggsLayed = value;
				this.checkForChickenEggAchievements();
			}
		}

		// Token: 0x170000AD RID: 173
		public uint ItemsCooked
		{
			// Token: 0x0600075F RID: 1887 RVA: 0x000A57CA File Offset: 0x000A39CA
			get
			{
				return this.itemsCooked;
			}
			// Token: 0x06000760 RID: 1888 RVA: 0x000A57D2 File Offset: 0x000A39D2
			set
			{
				this.itemsCooked = value;
			}
		}

		// Token: 0x170000AE RID: 174
		public uint ItemsShipped
		{
			// Token: 0x06000761 RID: 1889 RVA: 0x000A57DB File Offset: 0x000A39DB
			get
			{
				return this.itemsShipped;
			}
			// Token: 0x06000762 RID: 1890 RVA: 0x000A57E3 File Offset: 0x000A39E3
			set
			{
				this.itemsShipped = value;
			}
		}

		// Token: 0x170000AF RID: 175
		public uint SeedsSown
		{
			// Token: 0x06000763 RID: 1891 RVA: 0x000A57EC File Offset: 0x000A39EC
			get
			{
				return this.seedsSown;
			}
			// Token: 0x06000764 RID: 1892 RVA: 0x000A57F4 File Offset: 0x000A39F4
			set
			{
				this.seedsSown = value;
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x000A5800 File Offset: 0x000A3A00
		public void checkForWoodAchievements()
		{
			uint woodCollected = this.SticksChopped + this.StumpsChopped * 4u;
			if (woodCollected >= 5000u || woodCollected < 1500u)
			{
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000A5838 File Offset: 0x000A3A38
		public void checkForStoneAchievements()
		{
			uint stoneCollected = this.RocksCrushed + this.BouldersCracked * 4u;
			if (stoneCollected >= 5000u || stoneCollected < 1500u)
			{
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00002834 File Offset: 0x00000A34
		public void checkForStoneBreakAchievements()
		{
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000A5870 File Offset: 0x000A3A70
		public void checkForCookingAchievements()
		{
			Dictionary<string, string> recipes = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			int numberOfRecipesCooked = 0;
			int numberOfMealsMade = 0;
			foreach (KeyValuePair<string, string> v in recipes)
			{
				if (Game1.player.cookingRecipes.ContainsKey(v.Key))
				{
					int recipe = Convert.ToInt32(v.Value.Split(new char[]
					{
						'/'
					})[2].Split(new char[]
					{
						' '
					})[0]);
					if (Game1.player.recipesCooked.ContainsKey(recipe))
					{
						numberOfMealsMade += Game1.player.recipesCooked[recipe];
						numberOfRecipesCooked++;
					}
				}
			}
			this.itemsCooked = (uint)numberOfMealsMade;
			if (numberOfRecipesCooked == recipes.Count)
			{
				Game1.getAchievement(17);
			}
			if (numberOfRecipesCooked >= 25)
			{
				Game1.getAchievement(16);
			}
			if (numberOfRecipesCooked >= 10)
			{
				Game1.getAchievement(15);
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000A5974 File Offset: 0x000A3B74
		public void checkForCraftingAchievements()
		{
			Dictionary<string, string> recipes = Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
			int numberOfRecipesMade = 0;
			int numberOfItemsCrafted = 0;
			foreach (string s in recipes.Keys)
			{
				if (Game1.player.craftingRecipes.ContainsKey(s))
				{
					numberOfItemsCrafted += Game1.player.craftingRecipes[s];
					if (Game1.player.craftingRecipes[s] > 0)
					{
						numberOfRecipesMade++;
					}
				}
			}
			this.itemsCrafted = (uint)numberOfItemsCrafted;
			if (numberOfRecipesMade >= recipes.Count)
			{
				Game1.getAchievement(22);
			}
			if (numberOfRecipesMade >= 30)
			{
				Game1.getAchievement(21);
			}
			if (numberOfRecipesMade >= 15)
			{
				Game1.getAchievement(20);
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000A5A44 File Offset: 0x000A3C44
		public void checkForShippingAchievements()
		{
			if (this.farmerShipped(24, 15) && this.farmerShipped(188, 15) && this.farmerShipped(190, 15) && this.farmerShipped(192, 15) && this.farmerShipped(248, 15) && this.farmerShipped(250, 15) && this.farmerShipped(252, 15) && this.farmerShipped(254, 15) && this.farmerShipped(256, 15) && this.farmerShipped(258, 15) && this.farmerShipped(260, 15) && this.farmerShipped(262, 15) && this.farmerShipped(264, 15) && this.farmerShipped(266, 15) && this.farmerShipped(268, 15) && this.farmerShipped(270, 15) && this.farmerShipped(272, 15) && this.farmerShipped(274, 15) && this.farmerShipped(276, 15) && this.farmerShipped(278, 15) && this.farmerShipped(280, 15) && this.farmerShipped(282, 15) && this.farmerShipped(284, 15) && this.farmerShipped(300, 15) && this.farmerShipped(304, 15) && this.farmerShipped(398, 15) && this.farmerShipped(400, 15) && this.farmerShipped(433, 15))
			{
				Game1.getAchievement(31);
			}
			if (this.farmerShipped(24, 300) || this.farmerShipped(188, 300) || this.farmerShipped(190, 300) || this.farmerShipped(192, 300) || this.farmerShipped(248, 300) || this.farmerShipped(250, 300) || this.farmerShipped(252, 300) || this.farmerShipped(254, 300) || this.farmerShipped(256, 300) || this.farmerShipped(258, 300) || this.farmerShipped(260, 300) || this.farmerShipped(262, 300) || this.farmerShipped(264, 300) || this.farmerShipped(266, 300) || this.farmerShipped(268, 300) || this.farmerShipped(270, 300) || this.farmerShipped(272, 300) || this.farmerShipped(274, 300) || this.farmerShipped(276, 300) || this.farmerShipped(278, 300) || this.farmerShipped(280, 300) || this.farmerShipped(282, 300) || this.farmerShipped(284, 300) || this.farmerShipped(454, 300) || this.farmerShipped(300, 300) || this.farmerShipped(304, 300) || (this.farmerShipped(398, 300) | this.farmerShipped(433, 300)) || this.farmerShipped(400, 300) || this.farmerShipped(591, 300) || this.farmerShipped(593, 300) || this.farmerShipped(595, 300) || this.farmerShipped(597, 300))
			{
				Game1.getAchievement(32);
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000A5ED2 File Offset: 0x000A40D2
		public void checkForStarCropsAchievements()
		{
			if (this.StarLevelCropsShipped >= 100u)
			{
				Game1.getAchievement(77);
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000A5EE5 File Offset: 0x000A40E5
		private bool farmerShipped(int index, int number)
		{
			return Game1.player.basicShipped.ContainsKey(index) && Game1.player.basicShipped[index] >= number;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000A5F10 File Offset: 0x000A4110
		public void checkForFishingAchievements()
		{
			int numberOfFishCaught = 0;
			int differentKindsOfFishCaught = 0;
			int totalKindsOfFish = 59;
			foreach (KeyValuePair<int, string> v in Game1.objectInformation)
			{
				if (v.Value.Split(new char[]
				{
					'/'
				})[3].Contains("Fish") && (v.Key < 167 || v.Key >= 173) && Game1.player.fishCaught.ContainsKey(v.Key))
				{
					numberOfFishCaught += Game1.player.fishCaught[v.Key][0];
					differentKindsOfFishCaught++;
				}
			}
			this.fishCaught = (uint)numberOfFishCaught;
			if (numberOfFishCaught >= 100)
			{
				Game1.getAchievement(27);
			}
			if (differentKindsOfFishCaught == totalKindsOfFish)
			{
				Game1.getAchievement(26);
				if (!Game1.player.hasOrWillReceiveMail("CF_Fish"))
				{
					Game1.addMailForTomorrow("CF_Fish", false, false);
				}
			}
			if (differentKindsOfFishCaught >= 24)
			{
				Game1.getAchievement(25);
			}
			if (differentKindsOfFishCaught >= 10)
			{
				Game1.getAchievement(24);
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x000A6034 File Offset: 0x000A4234
		public void checkForChickenEggAchievements()
		{
			if (this.ChickenEggsLayed < 800u && this.ChickenEggsLayed < 350u)
			{
				uint arg_26_0 = this.ChickenEggsLayed;
			}
			uint arg_30_0 = this.chickenEggsLayed;
			uint arg_46_0 = this.chickenEggsLayed + this.duckEggsLayed * 2u;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x000A6088 File Offset: 0x000A4288
		public void checkForDuckEggAchievements()
		{
			if (this.DuckEggsLayed < 200u)
			{
				uint arg_16_0 = this.DuckEggsLayed;
			}
			uint arg_2C_0 = this.chickenEggsLayed + this.duckEggsLayed * 2u;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000A60B8 File Offset: 0x000A42B8
		public void checkForCowMilkAchievements()
		{
			if (this.CowMilkProduced < 500u)
			{
				uint arg_16_0 = this.CowMilkProduced;
			}
			uint arg_20_0 = this.cowMilkProduced;
			uint arg_36_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u;
			uint arg_55_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u + this.GoatMilkProduced * 2u;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000A611B File Offset: 0x000A431B
		public void checkForGoatMilkAchievements()
		{
			if (this.GoatMilkProduced < 300u)
			{
				uint arg_16_0 = this.GoatMilkProduced;
			}
			uint arg_35_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u + this.GoatMilkProduced * 2u;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000A6154 File Offset: 0x000A4354
		public void checkForWoolAchievements()
		{
			uint woolProduced = this.RabbitWoolProduced + this.SheepWoolProduced;
			if (woolProduced < 200u)
			{
			}
			uint arg_30_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u;
			uint arg_4F_0 = this.CowMilkProduced + this.SheepWoolProduced * 3u + this.GoatMilkProduced * 2u;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000A61B1 File Offset: 0x000A43B1
		public void checkForCheeseAchievements()
		{
			uint arg_10_0 = this.GoatCheeseMade + this.CheeseMade;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000A61C4 File Offset: 0x000A43C4
		public void checkForArchaeologyAchievements()
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000A61D1 File Offset: 0x000A43D1
		public void checkForCopperOreAchievements()
		{
			if (this.CopperFound < 2500u)
			{
				uint arg_19_0 = this.CopperFound;
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x000A61ED File Offset: 0x000A43ED
		public void checkForIronOreAchievements()
		{
			if (this.IronFound < 1000u)
			{
				uint arg_19_0 = this.IronFound;
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x000A6209 File Offset: 0x000A4409
		public void checkForCoalOreAchievements()
		{
			if (this.CoalFound < 750u)
			{
				uint arg_19_0 = this.CoalFound;
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000A6225 File Offset: 0x000A4425
		public void checkForGoldOreAchievements()
		{
			if (this.GoldFound < 500u)
			{
				uint arg_16_0 = this.GoldFound;
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000A623E File Offset: 0x000A443E
		public void checkForIridiumOreAchievements()
		{
			if (this.IridiumFound < 30u)
			{
				uint arg_12_0 = this.IridiumFound;
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000A6254 File Offset: 0x000A4454
		public void checkForMoneyAchievements()
		{
			if (Game1.player.totalMoneyEarned >= 10000000u)
			{
				Game1.getAchievement(4);
			}
			if (Game1.player.totalMoneyEarned >= 1000000u)
			{
				Game1.getAchievement(3);
			}
			if (Game1.player.totalMoneyEarned >= 250000u)
			{
				Game1.getAchievement(2);
			}
			if (Game1.player.totalMoneyEarned >= 50000u)
			{
				Game1.getAchievement(1);
			}
			if (Game1.player.totalMoneyEarned >= 15000u)
			{
				Game1.getAchievement(0);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000A62D4 File Offset: 0x000A44D4
		public void checkForBuildingUpgradeAchievements()
		{
			if (Game1.player.HouseUpgradeLevel == 2)
			{
				Game1.getAchievement(19);
			}
			if (Game1.player.HouseUpgradeLevel == 1)
			{
				Game1.getAchievement(18);
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000A62FE File Offset: 0x000A44FE
		public void checkForQuestAchievements()
		{
			if (this.QuestsCompleted >= 40u)
			{
				Game1.getAchievement(30);
				Game1.addMailForTomorrow("quest35", false, false);
			}
			if (this.QuestsCompleted >= 10u)
			{
				Game1.getAchievement(29);
				Game1.addMailForTomorrow("quest10", false, false);
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x000A633C File Offset: 0x000A453C
		public void checkForFriendshipAchievements()
		{
			int numberOf5Level = 0;
			int numberOf10Level = 0;
			foreach (int[] expr_22 in Game1.player.friendships.Values)
			{
				if (expr_22[0] >= 2500)
				{
					numberOf10Level++;
				}
				if (expr_22[0] >= 1250)
				{
					numberOf5Level++;
				}
			}
			if (numberOf5Level >= 20)
			{
				Game1.getAchievement(13);
			}
			if (numberOf5Level >= 10)
			{
				Game1.getAchievement(12);
			}
			if (numberOf5Level >= 4)
			{
				Game1.getAchievement(11);
			}
			if (numberOf5Level >= 1)
			{
				Game1.getAchievement(6);
			}
			if (numberOf10Level >= 8)
			{
				Game1.getAchievement(9);
			}
			if (numberOf10Level >= 1)
			{
				Game1.getAchievement(7);
			}
			Dictionary<string, string> cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
			foreach (string s in cookingRecipes.Keys)
			{
				string[] getConditions = cookingRecipes[s].Split(new char[]
				{
					'/'
				})[3].Split(new char[]
				{
					' '
				});
				if (getConditions[0].Equals("f") && Game1.player.friendships.ContainsKey(getConditions[1]) && Game1.player.friendships[getConditions[1]][0] >= Convert.ToInt32(getConditions[2]) * 250 && !Game1.player.cookingRecipes.ContainsKey(s))
				{
					Game1.addMailForTomorrow(getConditions[1] + "Cooking", false, false);
				}
			}
		}

		// Token: 0x040007F5 RID: 2037
		public uint seedsSown;

		// Token: 0x040007F6 RID: 2038
		public uint itemsShipped;

		// Token: 0x040007F7 RID: 2039
		public uint itemsCooked;

		// Token: 0x040007F8 RID: 2040
		public uint itemsCrafted;

		// Token: 0x040007F9 RID: 2041
		public uint chickenEggsLayed;

		// Token: 0x040007FA RID: 2042
		public uint duckEggsLayed;

		// Token: 0x040007FB RID: 2043
		public uint cowMilkProduced;

		// Token: 0x040007FC RID: 2044
		public uint goatMilkProduced;

		// Token: 0x040007FD RID: 2045
		public uint rabbitWoolProduced;

		// Token: 0x040007FE RID: 2046
		public uint sheepWoolProduced;

		// Token: 0x040007FF RID: 2047
		public uint cheeseMade;

		// Token: 0x04000800 RID: 2048
		public uint goatCheeseMade;

		// Token: 0x04000801 RID: 2049
		public uint trufflesFound;

		// Token: 0x04000802 RID: 2050
		public uint stoneGathered;

		// Token: 0x04000803 RID: 2051
		public uint rocksCrushed;

		// Token: 0x04000804 RID: 2052
		public uint dirtHoed;

		// Token: 0x04000805 RID: 2053
		public uint giftsGiven;

		// Token: 0x04000806 RID: 2054
		public uint timesUnconscious;

		// Token: 0x04000807 RID: 2055
		public uint averageBedtime;

		// Token: 0x04000808 RID: 2056
		public uint timesFished;

		// Token: 0x04000809 RID: 2057
		public uint fishCaught;

		// Token: 0x0400080A RID: 2058
		public uint bouldersCracked;

		// Token: 0x0400080B RID: 2059
		public uint stumpsChopped;

		// Token: 0x0400080C RID: 2060
		public uint stepsTaken;

		// Token: 0x0400080D RID: 2061
		public uint monstersKilled;

		// Token: 0x0400080E RID: 2062
		public uint diamondsFound;

		// Token: 0x0400080F RID: 2063
		public uint prismaticShardsFound;

		// Token: 0x04000810 RID: 2064
		public uint otherPreciousGemsFound;

		// Token: 0x04000811 RID: 2065
		public uint caveCarrotsFound;

		// Token: 0x04000812 RID: 2066
		public uint copperFound;

		// Token: 0x04000813 RID: 2067
		public uint ironFound;

		// Token: 0x04000814 RID: 2068
		public uint coalFound;

		// Token: 0x04000815 RID: 2069
		public uint coinsFound;

		// Token: 0x04000816 RID: 2070
		public uint goldFound;

		// Token: 0x04000817 RID: 2071
		public uint iridiumFound;

		// Token: 0x04000818 RID: 2072
		public uint barsSmelted;

		// Token: 0x04000819 RID: 2073
		public uint beveragesMade;

		// Token: 0x0400081A RID: 2074
		public uint preservesMade;

		// Token: 0x0400081B RID: 2075
		public uint piecesOfTrashRecycled;

		// Token: 0x0400081C RID: 2076
		public uint mysticStonesCrushed;

		// Token: 0x0400081D RID: 2077
		public uint daysPlayed;

		// Token: 0x0400081E RID: 2078
		public uint weedsEliminated;

		// Token: 0x0400081F RID: 2079
		public uint sticksChopped;

		// Token: 0x04000820 RID: 2080
		public uint notesFound;

		// Token: 0x04000821 RID: 2081
		public uint questsCompleted;

		// Token: 0x04000822 RID: 2082
		public uint starLevelCropsShipped;

		// Token: 0x04000823 RID: 2083
		public uint cropsShipped;

		// Token: 0x04000824 RID: 2084
		public uint slimesKilled;

		// Token: 0x04000825 RID: 2085
		public uint geodesCracked;

		// Token: 0x04000826 RID: 2086
		public SerializableDictionary<string, int> specificMonstersKilled = new SerializableDictionary<string, int>();
	}
}
