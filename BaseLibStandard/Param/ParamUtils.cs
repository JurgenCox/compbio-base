namespace BaseLibS.Param{
	public static class ParamUtils{
		/// <summary>
		/// Convert client side parameters back to <see cref="BaseLibS.Param"/>
		/// used for xml serialization
		/// </summary>
		/// <param name="p"></param>
		/// <returns>Server side version of parameter.</returns>
		public static Parameter ConvertBack(Parameter p){
			if (p.Type == ParamType.Server){
				return p;
			}
			if (p is RegexReplaceParam){
				RegexReplaceParam q = (RegexReplaceParam) p;
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
			if (p is RegexMatchParam){
				RegexMatchParam q = (RegexMatchParam) p;
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
			if (p is BoolParam){
				BoolParam q = (BoolParam) p;
				BoolParam b = new BoolParam(q.Name, q.Value){Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is BoolWithSubParams){
				BoolWithSubParams q = (BoolWithSubParams) p;
				q.SubParamsFalse?.Convert(ConvertBack);
				q.SubParamsTrue?.Convert(ConvertBack);
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
			if (p is DictionaryIntValueParam){
				DictionaryIntValueParam q = (DictionaryIntValueParam) p;
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
			if (p is DoubleParam){
				DoubleParam q = (DoubleParam) p;
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
			if (p is FileParam){
				FileParam q = (FileParam) p;
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
			if (p is FolderParam){
				FolderParam q = (FolderParam) p;
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
			if (p is IntParam){
				IntParam q = (IntParam) p;
				IntParam b = new IntParam(q.Name, q.Value){Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is LabelParam){
				LabelParam q = (LabelParam) p;
				LabelParam b = new LabelParam(q.Name, q.Value){Help = q.Help, Visible = q.Visible, Default = q.Default, Url = q.Url};
				foreach (ValueChangedHandler act in q.GetPropertyChangedHandlers()){
					b.ValueChanged += act;
				}
				return b;
			}
			if (p is MultiChoiceMultiBinParam){
				MultiChoiceMultiBinParam q = (MultiChoiceMultiBinParam) p;
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
			if (p is MultiChoiceParam){
				MultiChoiceParam q = (MultiChoiceParam) p;
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
			if (p is MultiFileParam){
				MultiFileParam q = (MultiFileParam) p;
				MultiFileParam b = new MultiFileParam(q.Name, q.Value){
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
			if (p is MultiStringParam){
				MultiStringParam q = (MultiStringParam) p;
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
			if (p is SingleChoiceParam){
				SingleChoiceParam q = (SingleChoiceParam) p;
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
			if (p is SingleChoiceWithSubParams){
				SingleChoiceWithSubParams q = (SingleChoiceWithSubParams) p;
				foreach (Parameters param in q.SubParams){
					param?.Convert(ConvertBack);
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
			if (p is StringParam){
				StringParam q = (StringParam) p;
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
			if (p is Ms1LabelParam){
				Ms1LabelParam q = (Ms1LabelParam) p;
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
			return p; // and hope for the best
		}
	}
}