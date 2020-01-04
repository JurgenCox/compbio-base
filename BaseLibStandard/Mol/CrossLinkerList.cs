namespace BaseLibS.Mol{
	[System.SerializableAttribute, System.Diagnostics.DebuggerStepThroughAttribute,
	 System.ComponentModel.DesignerCategoryAttribute("code"),
	 System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true),
	 System.Xml.Serialization.XmlRoot("crosslinks", IsNullable = false)]
	public class CrossLinkerList{
		private CrossLinker[] crosslinks;
		[System.Xml.Serialization.XmlElementAttribute("crosslink", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public CrossLinker[] Crosslinks { get => crosslinks;
			set => crosslinks = value;
		}
	}
}