using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace StardewValley
{
	// Token: 0x02000003 RID: 3
	public class LocalizedContentManager : ContentManager
	{
		// Token: 0x06000006 RID: 6 RVA: 0x0000219D File Offset: 0x0000039D
		public LocalizedContentManager(IServiceProvider serviceProvider, string rootDirectory, CultureInfo currentCulture, string languageCodeOverride) : base(serviceProvider, rootDirectory)
		{
			this.CurrentCulture = currentCulture;
			this.LanguageCodeOverride = languageCodeOverride;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021B6 File Offset: 0x000003B6
		public LocalizedContentManager(IServiceProvider serviceProvider, string rootDirectory) : this(serviceProvider, rootDirectory, Thread.CurrentThread.CurrentCulture, null)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021CC File Offset: 0x000003CC
		public override T Load<T>(string assetName)
		{
			string localizedAssetName = assetName + "." + this.languageCode();
			if (this.assetExists(localizedAssetName))
			{
				return base.Load<T>(localizedAssetName);
			}
			return base.Load<T>(assetName);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002203 File Offset: 0x00000403
		private string languageCode()
		{
			if (this.LanguageCodeOverride != null)
			{
				return this.LanguageCodeOverride;
			}
			return this.CurrentCulture.TwoLetterISOLanguageName;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000221F File Offset: 0x0000041F
		private bool assetExists(string assetName)
		{
			return File.Exists(Path.Combine(base.RootDirectory, assetName + ".xnb"));
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000223C File Offset: 0x0000043C
		public string LoadString(string path, params object[] substitutions)
		{
			string assetName;
			string key;
			this.parseStringPath(path, out assetName, out key);
			Dictionary<string, string> strings = this.Load<Dictionary<string, string>>(assetName);
			if (!strings.ContainsKey(key))
			{
				strings = base.Load<Dictionary<string, string>>(assetName);
			}
			return string.Format(strings[key], substitutions);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000227C File Offset: 0x0000047C
		private void parseStringPath(string path, out string assetName, out string key)
		{
			int i = path.IndexOf(':');
			if (i == -1)
			{
				throw new ContentLoadException("Unable to parse string path: " + path);
			}
			assetName = path.Substring(0, i);
			key = path.Substring(i + 1, path.Length - i - 1);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022C6 File Offset: 0x000004C6
		public LocalizedContentManager CreateTemporary()
		{
			return new LocalizedContentManager(base.ServiceProvider, base.RootDirectory, this.CurrentCulture, this.LanguageCodeOverride);
		}

		// Token: 0x04000006 RID: 6
		public CultureInfo CurrentCulture;

		// Token: 0x04000007 RID: 7
		public string LanguageCodeOverride;
	}
}
