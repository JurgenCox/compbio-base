using System;
using BaseLibS.Param;

namespace BaseLib.Param{
    /// <summary>
    /// Conversion to server parameters e.g. for Xml serialization
    /// </summary>
	public static class ServerParameterFactory{
        /// <summary>
        /// Convert <see cref="BaseLib.Param"/> to <see cref="BaseLibS.Param"/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Parameter Convert(Parameter p){
			if (p.Type == ParamType.Server){
				return p;
			}
			if (p is RegexReplaceParamWf){
				RegexReplaceParamWf q = (RegexReplaceParamWf) p;
				RegexReplaceParam b = new RegexReplaceParam(q.Name, q.Value.Item1, q.Value.Item2, q.Previews){
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
			if (p is RegexMatchParamWf){
				RegexMatchParamWf q = (RegexMatchParamWf) p;
				RegexMatchParam b = new RegexMatchParam(q.Name, q.Value, q.Previews){
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
			if (p is BoolParamWf){
				BoolParamWf q = (BoolParamWf) p;
				BoolParam b = new BoolParam(q.Name, q.Value){
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
			if (p is BoolWithSubParamsWf){
				BoolWithSubParamsWf q = (BoolWithSubParamsWf) p;
				q.SubParamsFalse?.Convert(Convert);
				q.SubParamsTrue?.Convert(Convert);
				BoolWithSubParams b = new BoolWithSubParams(q.Name, q.Value){
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
			if (p is DictionaryIntValueParamWf){
				DictionaryIntValueParamWf q = (DictionaryIntValueParamWf) p;
				DictionaryIntValueParam b = new DictionaryIntValueParam(q.Name, q.Value, q.Keys){
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
			if (p is DoubleParamWf){
				DoubleParamWf q = (DoubleParamWf) p;
				DoubleParam b = new DoubleParam(q.Name, q.Value){
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
			if (p is FileParamWf){
				FileParamWf q = (FileParamWf) p;
				FileParam b = new FileParam(q.Name, q.Value){
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
			if (p is FolderParamWf){
				FolderParamWf q = (FolderParamWf) p;
				FolderParam b = new FolderParam(q.Name, q.Value){
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
			if (p is IntParamWf){
				IntParamWf q = (IntParamWf) p;
				IntParam b = new IntParam(q.Name, q.Value){
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
			if (p is LabelParamWf){
				LabelParamWf q = (LabelParamWf) p;
				LabelParam b = new LabelParam(q.Name, q.Value){
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
			if (p is MultiChoiceMultiBinParamWf){
				MultiChoiceMultiBinParamWf q = (MultiChoiceMultiBinParamWf) p;
				MultiChoiceMultiBinParam b = new MultiChoiceMultiBinParam(q.Name, q.Value){
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
			if (p is MultiChoiceParamWf){
				MultiChoiceParamWf q = (MultiChoiceParamWf) p;
				MultiChoiceParam b = new MultiChoiceParam(q.Name, q.Value){
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
	        if (p is MultiFileParamWf) {
		        MultiFileParamWf q = (MultiFileParamWf)p;
		        MultiFileParam b = new MultiFileParam(q.Name, q.Value) {
			        Help = q.Help,
			        Visible = q.Visible,
			        Filter = q.Filter,
			        Default = q.Default,
			        Url = q.Url
		        };
		        foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()) {
			        b.ValueChanged += act;
		        }
		        return b;
	        }
	        if (p is FastaFilesParamWf) {
		        FastaFilesParamWf q = (FastaFilesParamWf)p;
		        FastaFilesParam b = new FastaFilesParam(q.Name, q.Value) {
			        Help = q.Help,
			        Visible = q.Visible,
			        Default = q.Default,
			        Url = q.Url
		        };
		        foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()) {
			        b.ValueChanged += act;
		        }
		        return b;
	        }
			if (p is MultiStringParamWf){
				MultiStringParamWf q = (MultiStringParamWf) p;
				MultiStringParam b = new MultiStringParam(q.Name, q.Value){
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
			if (p is SingleChoiceParamWf){
				SingleChoiceParamWf q = (SingleChoiceParamWf) p;
				SingleChoiceParam b = new SingleChoiceParam(q.Name, q.Value){
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
			if (p is SingleChoiceWithSubParamsWf){
				SingleChoiceWithSubParamsWf q = (SingleChoiceWithSubParamsWf) p;
				foreach (Parameters param in q.SubParams){
					param?.Convert(Convert);
				}
				SingleChoiceWithSubParams b = new SingleChoiceWithSubParams(q.Name, q.Value){
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
			if (p is StringParamWf){
				StringParamWf q = (StringParamWf) p;
				StringParam b = new StringParam(q.Name, q.Value){
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
			if (p is Ms1LabelParamWf){
				Ms1LabelParamWf q = (Ms1LabelParamWf) p;
				Ms1LabelParam b = new Ms1LabelParam(q.Name, q.Value){
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
			throw new Exception("Could not convert ParamWfeter");
		}
	}
}