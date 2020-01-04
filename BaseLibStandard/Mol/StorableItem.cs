using System;
using System.Xml.Serialization;
using BaseLibS.Api;

namespace BaseLibS.Mol{
	public class StorableItem : INamedItem{
		[XmlIgnore]
		public ushort Index { get; set; }
		/// <summary>
		/// Unique title of the item
		/// </summary>
		[XmlAttribute("title")]
		public string Name { get; set; }
		/// <summary>
		/// Description or full name of the item
		/// </summary>
		[XmlAttribute("description")]
		public string Description { get; set; }
		/// <summary>
		/// Date of creation
		/// </summary>
		[XmlAttribute("create_date")]
		public DateTime CreationDate { get; set; }
		/// <summary>
		/// Date of last modification.
		/// </summary>
		[XmlAttribute("last_modified_date")]
		public DateTime ModifiedDate { get; set; }
		/// <summary>
		/// Name of the user who last modified the entry.
		/// </summary>
		[XmlAttribute("user")]
		public string User { get; set; }
	}
}