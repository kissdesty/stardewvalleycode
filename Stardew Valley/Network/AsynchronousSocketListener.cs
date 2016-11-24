using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace StardewValley.Network
{
	// Token: 0x020000A2 RID: 162
	public class AsynchronousSocketListener
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x000E4550 File Offset: 0x000E2750
		public static void StartListening()
		{
			new byte[1024];
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 24643);
			Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				listener.Bind(localEndPoint);
				listener.Listen(16);
				while (AsynchronousSocketListener.active)
				{
					AsynchronousSocketListener.allDone.Reset();
					Console.WriteLine("Waiting for a connection...");
					listener.BeginAccept(new AsyncCallback(AsynchronousSocketListener.AcceptCallback), listener);
					AsynchronousSocketListener.allDone.WaitOne();
				}
			}
			catch (Exception arg_72_0)
			{
				Console.WriteLine(arg_72_0.ToString());
			}
			Console.WriteLine("\nPress ENTER to continue...");
			Console.Read();
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000E45FC File Offset: 0x000E27FC
		public static void AcceptCallback(IAsyncResult ar)
		{
			AsynchronousSocketListener.allDone.Set();
			Socket handler = ((Socket)ar.AsyncState).EndAccept(ar);
			StateObject state = new StateObject();
			state.workSocket = handler;
			handler.BeginReceive(state.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.ReadCallback), state);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x000E4654 File Offset: 0x000E2854
		public static void ReadCallback(IAsyncResult ar)
		{
			string content = string.Empty;
			StateObject state = (StateObject)ar.AsyncState;
			Socket handler = state.workSocket;
			int bytesRead = handler.EndReceive(ar);
			if (bytesRead > 0)
			{
				state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
				content = state.sb.ToString();
				if (content.IndexOf("<EOF>") > -1)
				{
					Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
					AsynchronousSocketListener.Send(handler, content);
					return;
				}
				handler.BeginReceive(state.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.ReadCallback), state);
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x000E4700 File Offset: 0x000E2900
		private static void Send(Socket handler, string data)
		{
			string removed = data.Remove(data.IndexOf("<EOF>"));
			data = AsynchronousSocketListener.ToXml(Game1.getLocationFromName(removed.Split(new char[]
			{
				'_'
			})[0], Convert.ToBoolean(removed.Split(new char[]
			{
				'_'
			})[1])), typeof(GameLocation));
			byte[] byteData = Encoding.ASCII.GetBytes(data);
			byte[] intBytes = BitConverter.GetBytes(byteData.Length);
			handler.Send(intBytes);
			handler.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.SendCallback), handler);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x000E4798 File Offset: 0x000E2998
		public static string ToXml(object Obj, Type ObjType)
		{
			MemoryStream memStream = new MemoryStream();
			XmlWriter xmlWriter = XmlWriter.Create(memStream, new XmlWriterSettings
			{
				CloseOutput = true
			});
			xmlWriter.WriteStartDocument();
			SaveGame.locationSerializer.Serialize(xmlWriter, Obj);
			xmlWriter.WriteEndDocument();
			xmlWriter.Flush();
			xmlWriter.Close();
			memStream.Close();
			string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
			xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
			return xml.Substring(0, xml.LastIndexOf(Convert.ToChar(62)) + 1);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x000E4828 File Offset: 0x000E2A28
		private static void SendCallback(IAsyncResult ar)
		{
			try
			{
				Socket expr_0B = (Socket)ar.AsyncState;
				int bytesSent = expr_0B.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to client.", bytesSent);
				expr_0B.Shutdown(SocketShutdown.Both);
				expr_0B.Close();
			}
			catch (Exception arg_31_0)
			{
				Console.WriteLine(arg_31_0.ToString());
			}
		}

		// Token: 0x04000B53 RID: 2899
		public static ManualResetEvent allDone = new ManualResetEvent(false);

		// Token: 0x04000B54 RID: 2900
		public static bool active = true;
	}
}
