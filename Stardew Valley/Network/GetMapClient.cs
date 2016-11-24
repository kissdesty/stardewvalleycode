using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;

namespace StardewValley.Network
{
	// Token: 0x0200009C RID: 156
	public class GetMapClient
	{
		// Token: 0x06000B2A RID: 2858 RVA: 0x000E33AC File Offset: 0x000E15AC
		public static void receiveMapFromServer(GameLocation map, bool isStructure)
		{
			long currentTime = DateTime.Now.Ticks / 10000L;
			byte[] bytes = new byte[4];
			IPEndPoint remoteEP = new IPEndPoint(Game1.client.serverAddress, 24643);
			Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			sender.Connect(remoteEP);
			Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
			byte[] msg = Encoding.ASCII.GetBytes((isStructure ? map.uniqueName : map.name) + "_" + isStructure.ToString() + "_<EOF>");
			sender.Send(msg);
			sender.Receive(bytes);
			bytes = new byte[BitConverter.ToInt32(bytes, 0)];
			sender.Receive(bytes);
			GameLocation receivedMap = (GameLocation)GetMapClient.FromXml(Encoding.ASCII.GetString(bytes), typeof(GameLocation));
			map.terrainFeatures = receivedMap.terrainFeatures;
			using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator = map.terrainFeatures.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.loadSprite();
				}
			}
			map.objects = receivedMap.objects;
			using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator2 = map.objects.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.reloadSprite();
				}
			}
			map.characters = receivedMap.characters;
			if (receivedMap is Farm && map is Farm)
			{
				(map as Farm).buildings = (receivedMap as Farm).buildings;
				using (List<Building>.Enumerator enumerator3 = (map as Farm).buildings.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						enumerator3.Current.load();
					}
				}
				(map as Farm).animals = (receivedMap as Farm).animals;
				foreach (KeyValuePair<long, FarmAnimal> kvp in (map as Farm).animals)
				{
					kvp.Value.reload();
				}
			}
			using (List<NPC>.Enumerator enumerator5 = map.characters.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					enumerator5.Current.reloadSprite();
				}
			}
			Game1.player.remotePosition = Game1.player.position;
			Console.WriteLine("Time: " + (DateTime.Now.Ticks / 10000L - currentTime));
			sender.Close();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x000E369C File Offset: 0x000E189C
		public static object FromXml(string Xml, Type ObjType)
		{
			StringReader stringReader = new StringReader(Xml);
			XmlTextReader xmlReader = new XmlTextReader(stringReader);
			object arg_25_0 = SaveGame.locationSerializer.Deserialize(xmlReader);
			xmlReader.Close();
			stringReader.Close();
			return arg_25_0;
		}
	}
}
