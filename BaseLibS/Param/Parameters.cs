using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BaseLibS.Param{
	[Serializable]
	public class Parameters : IXmlSerializable {
		private readonly List<ParameterGroup> paramGroups = new List<ParameterGroup>();

		public Parameters(IList<Parameter> param, string name){
			AddParameterGroup(param, name, false);
		}

		public Parameters(Parameter param) : this(new[]{param}){}

	    public Parameters(params Parameter[] param) : this(param, null) { }
	    public Parameters(string name, params Parameter[] param) : this(param, name) { }
	    public Parameters(IList<Parameter> param) : this(param, null){}
		public Parameters(){}

		public void Convert(Func<Parameter, Parameter> map){
			foreach (ParameterGroup t in paramGroups){
				t.Convert(map);
			}
		}

		public Parameters GetSubGroupAt(int index){
			return new Parameters(paramGroups[index].ParameterList);
		}

		public Parameter[] GetAllParameters(){
			List<Parameter> result = new List<Parameter>();
			foreach (ParameterGroup pg in paramGroups){
				result.AddRange(pg.ParameterList);
			}
			return result.ToArray();
		}

		public string[] GetAllGroupHeadings(){
			List<string> result = new List<string>();
			foreach (ParameterGroup pg in paramGroups){
				result.Add(pg.Name);
			}
			return result.ToArray();
		}

		public string[] Markup{
			get{
				List<string> result = new List<string>();
				foreach (ParameterGroup paramGroup in paramGroups){
					result.AddRange(paramGroup.Markup);
				}
				return result.ToArray();
			}
		}

		public bool IsModified{
			get{
				foreach (ParameterGroup parameterGroup in paramGroups){
					if (parameterGroup.IsModified){
						return true;
					}
				}
				return false;
			}
		}

		public void AddParameterGroup(IList<Parameter> param, string name, bool collapsed){
			paramGroups.Add(new ParameterGroup(param, name, collapsed));
		}

		public int Count{
			get{
				int c = 0;
				foreach (ParameterGroup parameterGroup in paramGroups){
					c += parameterGroup.Count;
				}
				return c;
			}
		}

		public float Height{
			get{
				float h = 0;
				foreach (ParameterGroup parameter in paramGroups){
					h += parameter.Height;
				}
				return h;
			}
		}

		public Parameter<T> GetParam<T>(string name){
			return (Parameter<T>) GetParam(name);
		}

		public ParameterWithSubParams<T> GetParamWithSubParams<T>(string name){
			return (ParameterWithSubParams<T>) GetParam(name);
		}

		public int GroupCount => paramGroups.Count;

		public Parameter GetParam(string name){
			foreach (ParameterGroup parameterGroup in paramGroups){
				Parameter p = parameterGroup.GetParam(name);
				if (p != null){
					return p;
				}
			}
			throw new Exception("Parameter does not exist: " + name);
		}

		public Parameter[] GetDropTargets(){
			List<Parameter> result = new List<Parameter>();
			foreach (ParameterGroup parameterGroup in paramGroups){
				foreach (Parameter p in parameterGroup.ParameterList){
					if (p.IsDropTarget){
						result.Add(p);
					}
				}
			}
			return result.ToArray();
		}

		public void SetSizes(int paramNameWidth, int totalWidth){
			foreach (ParameterGroup parameterGroup in paramGroups){
				foreach (Parameter p in parameterGroup.ParameterList){
					if (p is IParameterWithSubParams){
						IParameterWithSubParams q = (IParameterWithSubParams) p;
						q.ParamNameWidth = paramNameWidth;
						q.TotalWidth = totalWidth;
					}
				}
			}
		}

		public Parameter GetParamNoException(string name){
			foreach (ParameterGroup parameterGroup in paramGroups){
				Parameter p = parameterGroup.GetParam(name);
				if (p != null){
					return p;
				}
			}
			return null;
		}

		public void UpdateControlsFromValue(){
			foreach (ParameterGroup parameterGroup in paramGroups){
				parameterGroup.UpdateControlsFromValue();
			}
		}

		public ParameterGroup GetGroup(int i){
			return paramGroups[i];
		}

		public void SetValuesFromControl(){
			foreach (ParameterGroup parameterGroup in paramGroups){
				parameterGroup.SetParametersFromConrtol();
			}
		}

		public void Clear(){
			foreach (ParameterGroup parameterGroup in paramGroups){
				parameterGroup.Clear();
			}
		}

		public void ResetValues(){
			foreach (ParameterGroup parameterGroup in paramGroups){
				parameterGroup.ResetValues();
			}
		}

		public void ResetDefaults(){
			foreach (ParameterGroup parameterGroup in paramGroups){
				parameterGroup.ResetDefaults();
			}
		}

		public Parameter FindParameter(string paramName){
			return FindParameter(paramName, this);
		}

		private static Parameter FindParameter(string paramName, Parameters parameters){
			Parameter p = parameters.GetParamNoException(paramName);
			if (p != null){
				return p;
			}
			foreach (Parameter px in parameters.GetAllParameters()){
				if (px is IParameterWithSubParams){
					Parameters ps = ((IParameterWithSubParams) px).GetSubParameters();
					Parameter pq = FindParameter(paramName, ps);
					if (pq != null){
						return pq;
					}
				}
			}
			return null;
		}

	    public XmlSchema GetSchema() { return null; }

	    public void ReadXml(XmlReader reader)
	    {
	        XmlSerializer serializer = new XmlSerializer(typeof(ParameterGroup));
	        bool isEmpty = reader.IsEmptyElement;
	        reader.ReadStartElement();
	        if (!isEmpty)
	        {
	            while (reader.NodeType == XmlNodeType.Element)
	            {
	                paramGroups.Add((ParameterGroup) serializer.Deserialize(reader));
	            }
	            reader.ReadEndElement();
	        }
	    }

	    public void WriteXml(XmlWriter writer)
	    {
	        foreach (ParameterGroup paramGrp in paramGroups)
	        {
                XmlSerializer paramSerializer = new XmlSerializer(paramGrp.GetType());
	            paramSerializer.Serialize(writer, paramGrp);
	        }
	    }
	}
}