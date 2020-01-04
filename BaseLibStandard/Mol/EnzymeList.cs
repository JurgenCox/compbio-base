namespace BaseLibS.Mol {
	[System.SerializableAttribute, System.Diagnostics.DebuggerStepThroughAttribute,
	 System.ComponentModel.DesignerCategoryAttribute("code"),
	 System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true),
	 System.Xml.Serialization.XmlRoot("enzymes", IsNullable = false)]
	public class EnzymeList {
		[System.Xml.Serialization.XmlElementAttribute("enzyme", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public Enzyme[] Enzymes { get; set; }
	}
}
