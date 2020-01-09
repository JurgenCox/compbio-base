namespace BaseLibS.Mol{
	[System.SerializableAttribute, System.Diagnostics.DebuggerStepThroughAttribute,
	System.ComponentModel.DesignerCategoryAttribute("code"),
	System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true),
	System.Xml.Serialization.XmlRoot("databases", IsNullable = false)]
	public class SequenceDatabaseList{
		[System.Xml.Serialization.XmlElementAttribute("database", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public SequenceDatabase[] SequenceDatabases { get; set; }
	}
}