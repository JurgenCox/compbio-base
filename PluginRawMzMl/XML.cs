using System;
using System.Xml.Serialization;

namespace PluginRawMzMl
{
	/// <summary>
	/// This class aims to keep all the 'manual' parts of xml parsing in one place.
	///
	/// Since the .mzml file is not read top-to-bottom, but rather in parts using the index,
	/// XmlSerializer have to be created separately, and with additional options.
	///
	/// Often the XmlRootAttribute is not defined.
	/// </summary>
	public static class Xml
	{
		public static XmlSerializer IndexListOffsetSerializer;
		public static XmlSerializer IndexListSerializer;
		public static XmlSerializer SpectrumSerializer;
        public static XmlSerializer InstrumentConfigurationListSerializer;
		public static XmlSerializer IndexTypeSerializer;

        public const string IndexListOffsetElementName = "indexListOffset";
		public const string IndexTypeElementName = "indextypeOffset";
        public const string IndexListElementName = "indexList";
		public const string IndexedmzMLElementName = "indexedmzML";
		public const string InstrumentConfigurationListElementName = "instrumentConfigurationList";
		public const string SpectrumElementName = "spectrum";
		public const string RunElementName = "run";
		public const string DefaultInstrumentConfigurationRefAttributeName = "defaultInstrumentConfigurationRef";

		public const string MzMLNamespace = "http://psi.hupo.org/ms/mzml";

		static Xml()
		{
			IndexListOffsetSerializer = CreateMzMLSerializer(IndexListOffsetElementName, typeof(long?));
			IndexListSerializer = CreateMzMLSerializer(IndexListElementName, typeof(IndexListType));
			SpectrumSerializer = CreateMzMLSerializer(SpectrumElementName, typeof(SpectrumType));
			InstrumentConfigurationListSerializer = CreateMzMLSerializer(InstrumentConfigurationListElementName, typeof(InstrumentConfigurationListType));
			IndexTypeSerializer = CreateMzMLSerializer(IndexTypeElementName, typeof(IndexType));
		}

		private static XmlSerializer CreateMzMLSerializer(string elementName, Type type)
		{
			var indexListRootAttribute = new XmlRootAttribute
			{
				ElementName = elementName,
				Namespace = MzMLNamespace
			};
			return new XmlSerializer(type, indexListRootAttribute);
		}
	}
}