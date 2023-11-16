using System;
using BaseLibS.Param;

namespace BaseLib.Param{
	public static class WinFormsParameterFactory{
		/// <summary>
		/// Convert <see cref="BaseLibS.Param"/> to <see cref="BaseLib.Param"/>
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static Parameter Convert(Parameter p){
			if (p.Type == ParamType.WinForms){
				return p;
			}
			if (p is RegexReplaceParam){
				RegexReplaceParam q = (RegexReplaceParam) p;
				RegexReplaceParamWf b = new RegexReplaceParamWf(q.Name, q.Value.Item1, q.Value.Item2, q.Previews){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is RegexMatchParam){
				RegexMatchParam q = (RegexMatchParam) p;
				RegexMatchParamWf b = new RegexMatchParamWf(q.Name, q.Value, q.Previews){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is BoolParam){
				BoolParam q = (BoolParam) p;
				BoolParamWf b = new BoolParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is EmptyParam){
				EmptyParam q = (EmptyParam) p;
				EmptyParamWf b = new EmptyParamWf(q.Name){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				return b;
			}
			if (p is BoolWithSubParams){
				BoolWithSubParams q = (BoolWithSubParams) p;
				q.SubParamsFalse?.Convert(Convert);
				q.SubParamsTrue?.Convert(Convert);
				BoolWithSubParamsWf b = new BoolWithSubParamsWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					SubParamsFalse = q.SubParamsFalse,
					SubParamsTrue = q.SubParamsTrue,
					Default = q.Default,
					ParamNameWidth = q.ParamNameWidth,
					TotalWidth = q.TotalWidth,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is DictionaryIntValueParam){
				DictionaryIntValueParam q = (DictionaryIntValueParam) p;
				DictionaryIntValueParamWf b = new DictionaryIntValueParamWf(q.Name, q.Value, q.Keys){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is DictionaryStringValueParam){
				DictionaryStringValueParam q = (DictionaryStringValueParam) p;
				DictionaryStringValueParamWf b = new DictionaryStringValueParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Default = q.Default,
					Url = q.Url,
					KeyName = q.KeyName,
					ValueName = q.ValueName
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is DoubleParam){
				DoubleParam q = (DoubleParam) p;
				DoubleParamWf b = new DoubleParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is CheckedFileParam){
				CheckedFileParam q = (CheckedFileParam) p;
				CheckedFileParamWf b = new CheckedFileParamWf(q.Name, q.Value, q.checkFileName){
					Help = q.Help,
					Visible = q.Visible,
					Default = q.Default,
					Filter = q.Filter,
					ProcessFileName = q.ProcessFileName,
					Save = q.Save,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is FileParam){
				FileParam q = (FileParam) p;
				FileParamWf b = new FileParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Default = q.Default,
					Filter = q.Filter,
					ProcessFileName = q.ProcessFileName,
					Save = q.Save,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is FolderParam){
				FolderParam q = (FolderParam) p;
				FolderParamWf b = new FolderParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is IntParam){
				IntParam q = (IntParam) p;
				IntParamWf b = new IntParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is LabelParam) {
				LabelParam q = (LabelParam)p;
				LabelParamWf b = new LabelParamWf(q.Name, q.Value) {
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()) {
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is SaveFileParam){
				SaveFileParam q = (SaveFileParam) p;
				SaveFileParamWf b = new SaveFileParamWf(q.Name, q.Value, q.FileName, q.Filter, q.WriteAction){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is SaveFolderParam){
				SaveFolderParam q = (SaveFolderParam) p;
				SaveFolderParamWf b = new SaveFolderParamWf(q.Name, q.Value, q.WriteAction){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiChoiceMultiBinParam){
				MultiChoiceMultiBinParam q = (MultiChoiceMultiBinParam) p;
				MultiChoiceMultiBinParamWf b = new MultiChoiceMultiBinParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Values = q.Values,
					Bins = q.Bins,
					Default = q.Default,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiChoiceParam){
				MultiChoiceParam q = (MultiChoiceParam) p;
				MultiChoiceParamWf b = new MultiChoiceParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Repeats = q.Repeats,
					Values = q.Values,
					Default = q.Default,
					DefaultSelections = q.DefaultSelections,
					DefaultSelectionNames = q.DefaultSelectionNames,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiFileParam){
				MultiFileParam q = (MultiFileParam) p;
				MultiFileParamWf b = new MultiFileParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Filter = q.Filter,
					Default = q.Default,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is FastaFilesParam){
				FastaFilesParam q = (FastaFilesParam) p;
				FastaFilesParamWf b = new FastaFilesParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Default = q.Default,
					HasVariationData = q.HasVariationData,
					HasModifications = q.HasModifications,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is IsobaricLabelsParam){
				IsobaricLabelsParam q = (IsobaricLabelsParam) p;
				IsobaricLabelsParamWf b = new IsobaricLabelsParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiStringParam) {
				MultiStringParam q = (MultiStringParam)p;
				MultiStringParamWf b = new MultiStringParamWf(q.Name, q.Value) {
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()) {
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is PerseusLoadMatrixParam) {
				PerseusLoadMatrixParam q = (PerseusLoadMatrixParam)p;
				PerseusLoadMatrixParamWf b = new PerseusLoadMatrixParamWf(q.Name) {
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url, Value = q.Value
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()) {
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiDoubleParam) {
				MultiDoubleParam q = (MultiDoubleParam)p;
				MultiDoubleParamWf b = new MultiDoubleParamWf(q.Name, q.Value) {
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()) {
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiShapeParam){
				MultiShapeParam q = (MultiShapeParam) p;
				MultiShapeParamWf b = new MultiShapeParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is SingleChoiceParam){
				SingleChoiceParam q = (SingleChoiceParam) p;
				SingleChoiceParamWf b = new SingleChoiceParamWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Values = q.Values,
					Default = q.Default,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is SingleChoiceWithSubParams){
				SingleChoiceWithSubParams q = (SingleChoiceWithSubParams) p;
				foreach (Parameters param in q.SubParams){
					param?.Convert(Convert);
				}
				SingleChoiceWithSubParamsWf b = new SingleChoiceWithSubParamsWf(q.Name, q.Value){
					Help = q.Help,
					Visible = q.Visible,
					Values = q.Values,
					Default = q.Default,
					SubParams = new Parameters[q.SubParams.Count],
					ParamNameWidth = q.ParamNameWidth,
					TotalWidth = q.TotalWidth,
					Url = q.Url
				};
				for (int i = 0; i < q.SubParams.Count; i++){
					b.SubParams[i] = q.SubParams[i];
				}
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is StringParam){
				StringParam q = (StringParam) p;
				StringParamWf b = new StringParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is ShapeParam){
				ShapeParam q = (ShapeParam) p;
				ShapeParamWf b = new ShapeParamWf(q.Name, q.Value){
					Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is Ms1LabelParam){
				Ms1LabelParam q = (Ms1LabelParam) p;
				Ms1LabelParamWf b = new Ms1LabelParamWf(q.Name, q.Value){
					Values = q.Values,
					Multiplicity = q.Multiplicity,
					Help = q.Help,
					Visible = q.Visible,
					Default = q.Default,
					Url = q.Url
				};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			throw new Exception("Could not convert parameter");
		}
	}
}