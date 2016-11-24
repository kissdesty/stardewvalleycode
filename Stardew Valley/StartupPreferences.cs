using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace StardewValley
{
	// Token: 0x0200004F RID: 79
	public class StartupPreferences
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x000A5171 File Offset: 0x000A3371
		public void StartUpPreferences()
		{
			this.ensureFolderStructureExists();
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000A517C File Offset: 0x000A337C
		private void ensureFolderStructureExists()
		{
			FileInfo info = new FileInfo(Path.Combine(new string[]
			{
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley", "placeholder")
			}));
			if (!info.Directory.Exists)
			{
				info.Directory.Create();
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000A51D0 File Offset: 0x000A33D0
		public void savePreferences()
		{
			string fullFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), StartupPreferences.filename);
			this.ensureFolderStructureExists();
			if (File.Exists(fullFilePath))
			{
				File.Delete(fullFilePath);
			}
			Stream stream = null;
			try
			{
				stream = File.Create(fullFilePath);
			}
			catch (IOException arg_44_0)
			{
				if (stream != null)
				{
					stream.Close();
				}
				Game1.debugOutput = Game1.parseText(arg_44_0.Message);
				return;
			}
			XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings
			{
				CloseOutput = true
			});
			writer.WriteStartDocument();
			StartupPreferences.serializer.Serialize(writer, this);
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000A527C File Offset: 0x000A347C
		public void loadPreferences()
		{
			string fullFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), StartupPreferences.filename);
			if (!File.Exists(fullFilePath))
			{
				Game1.debugOutput = "File does not exist (-_-)";
				return;
			}
			Stream stream = null;
			try
			{
				stream = File.Open(fullFilePath, FileMode.Open);
			}
			catch (IOException arg_3B_0)
			{
				Game1.debugOutput = Game1.parseText(arg_3B_0.Message);
				if (stream != null)
				{
					stream.Close();
				}
				return;
			}
			StartupPreferences p = (StartupPreferences)StartupPreferences.serializer.Deserialize(stream);
			stream.Close();
			stream.Dispose();
			this.startWindowed = p.startWindowed;
			this.startMuted = p.startMuted;
			this.timesPlayed = p.timesPlayed + 1;
			this.levelTenCombat = p.levelTenCombat;
			this.levelTenFishing = p.levelTenFishing;
			this.levelTenForaging = p.levelTenForaging;
			this.levelTenMining = p.levelTenMining;
			this.skipWindowPreparation = p.skipWindowPreparation;
		}

		// Token: 0x040007EB RID: 2027
		public static string filename = "startup_preferences";

		// Token: 0x040007EC RID: 2028
		public static XmlSerializer serializer = new XmlSerializer(typeof(StartupPreferences));

		// Token: 0x040007ED RID: 2029
		public bool startWindowed;

		// Token: 0x040007EE RID: 2030
		public bool startMuted;

		// Token: 0x040007EF RID: 2031
		public bool levelTenFishing;

		// Token: 0x040007F0 RID: 2032
		public bool levelTenMining;

		// Token: 0x040007F1 RID: 2033
		public bool levelTenForaging;

		// Token: 0x040007F2 RID: 2034
		public bool levelTenCombat;

		// Token: 0x040007F3 RID: 2035
		public bool skipWindowPreparation;

		// Token: 0x040007F4 RID: 2036
		public int timesPlayed;
	}
}
