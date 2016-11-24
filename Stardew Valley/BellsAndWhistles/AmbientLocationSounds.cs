using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace StardewValley.BellsAndWhistles
{
	// Token: 0x0200014C RID: 332
	public class AmbientLocationSounds
	{
		// Token: 0x060012D9 RID: 4825 RVA: 0x0017D8E4 File Offset: 0x0017BAE4
		public static void initialize()
		{
			if (Game1.soundBank != null)
			{
				AmbientLocationSounds.babblingBrook = Game1.soundBank.GetCue("babblingBrook");
				AmbientLocationSounds.cracklingFire = Game1.soundBank.GetCue("cracklingFire");
				AmbientLocationSounds.engine = Game1.soundBank.GetCue("heavyEngine");
				AmbientLocationSounds.cricket = Game1.soundBank.GetCue("cricketsAmbient");
				AmbientLocationSounds.babblingBrook.Play();
				AmbientLocationSounds.babblingBrook.Pause();
				AmbientLocationSounds.cracklingFire.Play();
				AmbientLocationSounds.cracklingFire.Pause();
				AmbientLocationSounds.engine.Play();
				AmbientLocationSounds.engine.Pause();
				AmbientLocationSounds.cricket.Play();
				AmbientLocationSounds.cricket.Pause();
			}
			AmbientLocationSounds.shortestDistanceForCue = new float[4];
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0017D9A8 File Offset: 0x0017BBA8
		public static void update(GameTime time)
		{
			if (AmbientLocationSounds.sounds.Count == 0)
			{
				return;
			}
			if (AmbientLocationSounds.volumeOverrideForLocChange < 1f)
			{
				AmbientLocationSounds.volumeOverrideForLocChange += (float)time.ElapsedGameTime.Milliseconds * 0.0003f;
			}
			AmbientLocationSounds.updateTimer -= time.ElapsedGameTime.Milliseconds;
			if (AmbientLocationSounds.updateTimer <= 0)
			{
				for (int i = 0; i < AmbientLocationSounds.shortestDistanceForCue.Length; i++)
				{
					AmbientLocationSounds.shortestDistanceForCue[i] = 9999999f;
				}
				Vector2 farmerPosition = Game1.player.getStandingPosition();
				for (int j = 0; j < AmbientLocationSounds.sounds.Count; j++)
				{
					float distance = Vector2.Distance(AmbientLocationSounds.sounds.ElementAt(j).Key, farmerPosition);
					if (AmbientLocationSounds.shortestDistanceForCue[AmbientLocationSounds.sounds.ElementAt(j).Value] > distance)
					{
						AmbientLocationSounds.shortestDistanceForCue[AmbientLocationSounds.sounds.ElementAt(j).Value] = distance;
					}
				}
				if (AmbientLocationSounds.volumeOverrideForLocChange >= 0f)
				{
					for (int k = 0; k < AmbientLocationSounds.shortestDistanceForCue.Length; k++)
					{
						if (AmbientLocationSounds.shortestDistanceForCue[k] <= (float)AmbientLocationSounds.farthestSoundDistance)
						{
							float volume = Math.Min(AmbientLocationSounds.volumeOverrideForLocChange, Math.Min(1f, 1f - AmbientLocationSounds.shortestDistanceForCue[k] / (float)AmbientLocationSounds.farthestSoundDistance));
							switch (k)
							{
							case 0:
								if (AmbientLocationSounds.babblingBrook != null)
								{
									AmbientLocationSounds.babblingBrook.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.babblingBrook.Resume();
								}
								break;
							case 1:
								if (AmbientLocationSounds.cracklingFire != null)
								{
									AmbientLocationSounds.cracklingFire.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.cracklingFire.Resume();
								}
								break;
							case 2:
								if (AmbientLocationSounds.engine != null)
								{
									AmbientLocationSounds.engine.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.engine.Resume();
								}
								break;
							case 3:
								if (AmbientLocationSounds.cricket != null)
								{
									AmbientLocationSounds.cricket.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientLocationSounds.cricket.Resume();
								}
								break;
							}
						}
						else
						{
							switch (k)
							{
							case 0:
								if (AmbientLocationSounds.babblingBrook != null)
								{
									AmbientLocationSounds.babblingBrook.Pause();
								}
								break;
							case 1:
								if (AmbientLocationSounds.cracklingFire != null)
								{
									AmbientLocationSounds.cracklingFire.Pause();
								}
								break;
							case 2:
								if (AmbientLocationSounds.engine != null)
								{
									AmbientLocationSounds.engine.Pause();
								}
								break;
							case 3:
								if (AmbientLocationSounds.cricket != null)
								{
									AmbientLocationSounds.cricket.Pause();
								}
								break;
							}
						}
					}
				}
				AmbientLocationSounds.updateTimer = 100;
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0017DCA6 File Offset: 0x0017BEA6
		public static void changeSpecificVariable(string variableName, float value, int whichSound)
		{
			if (whichSound == 2 && AmbientLocationSounds.engine != null)
			{
				AmbientLocationSounds.engine.SetVariable(variableName, value);
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0017DCBF File Offset: 0x0017BEBF
		public static void addSound(Vector2 tileLocation, int whichSound)
		{
			if (!AmbientLocationSounds.sounds.ContainsKey(tileLocation * (float)Game1.tileSize))
			{
				AmbientLocationSounds.sounds.Add(tileLocation * (float)Game1.tileSize, whichSound);
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0017DCF0 File Offset: 0x0017BEF0
		public static void removeSound(Vector2 tileLocation)
		{
			if (AmbientLocationSounds.sounds.ContainsKey(tileLocation * (float)Game1.tileSize))
			{
				switch (AmbientLocationSounds.sounds[tileLocation * (float)Game1.tileSize])
				{
				case 0:
					if (AmbientLocationSounds.babblingBrook != null)
					{
						AmbientLocationSounds.babblingBrook.Pause();
					}
					break;
				case 1:
					if (AmbientLocationSounds.cracklingFire != null)
					{
						AmbientLocationSounds.cracklingFire.Pause();
					}
					break;
				case 2:
					if (AmbientLocationSounds.engine != null)
					{
						AmbientLocationSounds.engine.Pause();
					}
					break;
				case 3:
					if (AmbientLocationSounds.cricket != null)
					{
						AmbientLocationSounds.cricket.Pause();
					}
					break;
				}
				AmbientLocationSounds.sounds.Remove(tileLocation * (float)Game1.tileSize);
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0017DDA8 File Offset: 0x0017BFA8
		public static void onLocationLeave()
		{
			AmbientLocationSounds.sounds.Clear();
			AmbientLocationSounds.volumeOverrideForLocChange = -0.5f;
			if (AmbientLocationSounds.babblingBrook != null)
			{
				AmbientLocationSounds.babblingBrook.Pause();
			}
			if (AmbientLocationSounds.cracklingFire != null)
			{
				AmbientLocationSounds.cracklingFire.Pause();
			}
			if (AmbientLocationSounds.engine != null)
			{
				AmbientLocationSounds.engine.SetVariable("Frequency", 100f);
				AmbientLocationSounds.engine.Pause();
			}
			if (AmbientLocationSounds.cricket != null)
			{
				AmbientLocationSounds.cricket.Pause();
			}
		}

		// Token: 0x04001343 RID: 4931
		public const int sound_babblingBrook = 0;

		// Token: 0x04001344 RID: 4932
		public const int sound_cracklingFire = 1;

		// Token: 0x04001345 RID: 4933
		public const int sound_engine = 2;

		// Token: 0x04001346 RID: 4934
		public const int sound_cricket = 3;

		// Token: 0x04001347 RID: 4935
		public const int numberOfSounds = 4;

		// Token: 0x04001348 RID: 4936
		public const float doNotPlay = 9999999f;

		// Token: 0x04001349 RID: 4937
		private static Dictionary<Vector2, int> sounds = new Dictionary<Vector2, int>();

		// Token: 0x0400134A RID: 4938
		private static int updateTimer = 100;

		// Token: 0x0400134B RID: 4939
		private static int farthestSoundDistance = Game1.tileSize * 16;

		// Token: 0x0400134C RID: 4940
		private static float[] shortestDistanceForCue;

		// Token: 0x0400134D RID: 4941
		private static Cue babblingBrook;

		// Token: 0x0400134E RID: 4942
		private static Cue cracklingFire;

		// Token: 0x0400134F RID: 4943
		private static Cue engine;

		// Token: 0x04001350 RID: 4944
		private static Cue cricket;

		// Token: 0x04001351 RID: 4945
		private static float volumeOverrideForLocChange;
	}
}
