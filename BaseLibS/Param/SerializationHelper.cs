using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace BaseLibS.Param
{
    public static class SerializationHelper
    {
        /// <summary>
        /// Read a list of strings &lt;List&gt; &lt;Item&gt; Value &lt;/Item&gt; &lt;/List&gt;.
        /// Element names are ignored
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="list"></param>
        /// <returns></returns>
	    public static List<string> ReadInto(this XmlReader reader, List<string> list)
	    {
	        bool isEmpty = reader.IsEmptyElement;
	        reader.ReadStartElement();
	        if (!isEmpty)
	        {
	            while (reader.NodeType == XmlNodeType.Element)
	            {
	                list.Add(reader.ReadElementContentAsString());
	            }
	            reader.ReadEndElement();
	        }
	        return list;
;	    }
        /// <summary>
        /// Read a list of integers &lt;List&gt; &lt;Item&gt; Value &lt;/Item&gt; &lt;/List&gt;.
        /// Element names are ignored
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="list"></param>
        /// <returns></returns>
	    public static List<int> ReadInto(this XmlReader reader, List<int> list)
	    {
	        bool isEmpty = reader.IsEmptyElement;
	        reader.ReadStartElement();
	        if (!isEmpty)
	        {
	            while (reader.NodeType == XmlNodeType.Element)
	            {
	                list.Add(reader.ReadElementContentAsInt());
	            }
	            reader.ReadEndElement();
	        }
	        return list;
;	    }

        /// <summary>
        /// Read a list of objects &lt;List&gt; &lt;Item&gt; Value &lt;/Item&gt; &lt;/List&gt;.
        /// Element names are ignored
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="list"></param>
        /// <returns></returns>
	    public static List<T> ReadInto<T>(this XmlReader reader, List<T> list)
	    {
	        bool isEmpty = reader.IsEmptyElement;
	        reader.ReadStartElement();
	        if (!isEmpty)
	        {
	            while (reader.NodeType == XmlNodeType.Element)
	            {
	                list.Add((T) reader.ReadElementContentAs(typeof(T), null));
	            }
	            reader.ReadEndElement();
	        }
	        return list;
	    }

        /// <summary>
        /// Read a neseted list of objects &lt;List&gt; &lt;Item&gt; Value with sub-elements &lt;/Item&gt; &lt;/List&gt;.
        /// Element names are ignored
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="list"></param>
        /// <returns></returns>
	    public static List<T> ReadIntoNested<T>(this XmlReader reader, List<T> list)
	    {
	        bool isEmpty = reader.IsEmptyElement;
	        reader.ReadStartElement();
	        if (!isEmpty)
	        {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
	            while (reader.NodeType == XmlNodeType.Element)
	            {
	                list.Add((T) serializer.Deserialize(reader));
	            }
	            reader.ReadEndElement();
	        }
	        return list;
	    }

        /// <summary>
        /// Read jagged array <code>int[][]</code>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<List<int>> ReadJagged2DArrayInto(this XmlReader reader, List<List<int>> list)
        {
	        bool isEmpty = reader.IsEmptyElement;
	        reader.ReadStartElement();
	        if (!isEmpty)
	        {
	            while (reader.NodeType == XmlNodeType.Element)
	            {
	                list.Add(ReadInto(reader, new List<int>()));
	            }
	            reader.ReadEndElement();
	        }
	        return list;
        }

        /// <summary>
        /// Write values with root tag <code>&lt;Root&gt; &lt;Item/&gt; &lt;Item/&gt; &lt;/Root&gt;</code>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="values"></param>
        /// <param name="childTag"></param>
        public static void WriteValues<T>(this XmlWriter writer, string rootTag, IList<T> values, string childTag = "Item")
        {
            writer.WriteStartElement(rootTag);
            writer.WriteValues(values, childTag);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Write values without root tag <code>&lt;Item/&gt; &lt;Item/&gt;</code>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="values"></param>
        /// <param name="childTag"></param>
        public static void WriteValues<T>(this XmlWriter writer, IList<T> values, string childTag = "Item")
        {
	        foreach (T value in values)
	        {
                writer.WriteStartElement(childTag);
                writer.WriteValue(value);
                writer.WriteEndElement();
	        }
        }

    }
}