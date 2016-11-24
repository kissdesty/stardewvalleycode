using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StardewValley
{
	// Token: 0x0200004E RID: 78
	[XmlRoot("dictionary")]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable, INotifyCollectionChanged
	{
		// Token: 0x060006F2 RID: 1778 RVA: 0x00035CBD File Offset: 0x00033EBD
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000A4F3A File Offset: 0x000A313A
		public new void Add(TKey key, TValue value)
		{
			base.Add(key, value);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key, 0));
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000A4F57 File Offset: 0x000A3157
		public new bool Remove(TKey key)
		{
			bool arg_1A_0 = base.Remove(key);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key, 0));
			return arg_1A_0;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000A4F73 File Offset: 0x000A3173
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(null, e);
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000A4F8C File Offset: 0x000A318C
		public void ReadXml(XmlReader reader)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
			bool arg_2D_0 = reader.IsEmptyElement;
			reader.Read();
			if (arg_2D_0)
			{
				return;
			}
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				TKey key = (TKey)((object)keySerializer.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				TValue value = (TValue)((object)valueSerializer.Deserialize(reader));
				reader.ReadEndElement();
				this.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000A5038 File Offset: 0x000A3238
		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
			foreach (TKey key in base.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				TValue value = base[key];
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}

		// Token: 0x14000004 RID: 4
		// Token: 0x060006F8 RID: 1784 RVA: 0x000A50FC File Offset: 0x000A32FC
		// Token: 0x060006F9 RID: 1785 RVA: 0x000A5134 File Offset: 0x000A3334
		[method: CompilerGenerated]
		[CompilerGenerated]
		public event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
