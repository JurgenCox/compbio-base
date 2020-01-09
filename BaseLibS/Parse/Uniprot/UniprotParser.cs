using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using BaseLibS.Util;

namespace BaseLibS.Parse.Uniprot{
	public delegate void HandleUniprotEntry(UniprotEntry entry);

	public class UniprotParser{
		private readonly string swissprotFileName;
		private readonly string tremblFileName;
		//private int entryCount;
		private UniprotEntry entry;
		private StringBuilder sequence;
		private StringBuilder keyword;
		private StringBuilder accession;
		private StringBuilder original;
		private StringBuilder variation;
		private List<string> accessions;
		private StringBuilder proteinFullName;
		private StringBuilder proteinShortName;
		private StringBuilder proteinEcNumber;
		private List<string> proteinFullNames;
		private List<string> proteinShortNames;
		private List<string> proteinEcNumbers;
		private bool inProtein;
		private bool inProteinRecommendedName;
		private StringBuilder uname;
		private List<string> unames;
		private StringBuilder gname;
		private StringBuilder oname;
		private List<Tuple<string, string>> gnames;
		private List<string> onames;
		private bool inGene;
		private string featureType;
		private string featureDescription;
		private string featureStatus;
		private string featureId;
		private bool inFeature;
		//private bool inFeatureLocation;
		private string featureBegin;
		private string featureEnd;
		private bool inOrganism;
		private bool inOrganismHost;
		private int level;
		private readonly Dictionary<FeatureType, long> featureCounts = new Dictionary<FeatureType, long>();
		private string gnameType;
		private bool inDbRef;
		private string dbReferenceType;
		private string dbReferenceId;
        private int numIsoforms;
        private Dictionary<string, List<string>> isoformToEnst;
        private StringBuilder molecule;
        private readonly bool resolveIsoforms;

		public UniprotParser(string swissprotFileName, string tremblFileName, bool includeTrembl, HandleUniprotEntry handle, bool resolveIsos){
		    resolveIsoforms = resolveIsos;
			if (swissprotFileName != null){
				this.swissprotFileName = swissprotFileName;
			}
			if (tremblFileName != null){
				this.tremblFileName = tremblFileName;
			}
			Parse(this.swissprotFileName, handle, false);
			if (includeTrembl){
				Parse(this.tremblFileName, handle, true);
			}
		}

		private void Parse(string filename, HandleUniprotEntry handle, bool isTrembl){
			Stream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			Stream stream = filename.ToLower().EndsWith(".gz")
				? new GZipStream(fileStream, CompressionMode.Decompress) : fileStream;
			XmlTextReader reader = new XmlTextReader(new StreamReader(stream));
			while (reader.Read()){
				switch (reader.NodeType){
					case XmlNodeType.Element:
						string name = reader.Name;
						Dictionary<string, string> attributes = new Dictionary<string, string>();
						if (reader.HasAttributes){
							for (int i = 0; i < reader.AttributeCount; i++){
								reader.MoveToAttribute(i);
								attributes.Add(reader.Name, reader.Value);
							}
						}
						StartElement(name, attributes);
						level++;
						break;
					case XmlNodeType.EndElement:
						level--;
						EndElement(reader.Name, handle, isTrembl);
						break;
					case XmlNodeType.Text:
						Characters(reader.Value, 0, reader.Value.Length);
						break;
				}
			}
		}

		private void StartElement(IEquatable<string> qName, IDictionary<string, string> attrs){
			if (inFeature){
				if (qName.Equals("location")){
					//inFeatureLocation = true;
				} else if (qName.Equals("position")){
					string position = attrs["position"];
					featureBegin = position;
					featureEnd = position;
				} else if (qName.Equals("begin")){
					string position = attrs.ContainsKey("position") ? attrs["position"] : attrs["status"];
					featureBegin = position;
				} else if (qName.Equals("end")){
					string position = attrs.ContainsKey("position") ? attrs["position"] : attrs["status"];
					featureEnd = position;
				} else if (qName.Equals("original")){
					original = new StringBuilder();
				} else if (qName.Equals("variation")){
					variation = new StringBuilder();
				}else{
					throw new Exception("Unknown qname: " + qName);
				}
			}
			if (inOrganism) {
				if (qName.Equals("name")) {
					string type = attrs["type"];
					if (type.Equals("scientific")) {
						oname = new StringBuilder();
					}
				} else if (qName.Equals("dbReference")) {
					string type = attrs["type"];
					if (type.Equals("NCBI Taxonomy")) {
						string id = attrs["id"];
						entry.AddTaxonomyId(id);
					}
				}
			}
			if (inOrganismHost) {
				if (qName.Equals("dbReference")) {
					string type = attrs["type"];
					if (type.Equals("NCBI Taxonomy")) {
						string id = attrs["id"];
						entry.AddHostTaxonomyId(id);
					}
				}
			}
			if (qName.Equals("entry")) {
				entry = new UniprotEntry();
				accessions = new List<string>();
				proteinFullNames = new List<string>();
				proteinShortNames = new List<string>();
				proteinEcNumbers = new List<string>();
				gnames = new List<Tuple<string, string>>();
				onames = new List<string>();
				unames = new List<string>();
				level = 0;
                numIsoforms = 0;
                isoformToEnst = new Dictionary<string, List<string>>();
			} else if (qName.Equals("dbReference")){
				inDbRef = true;
			    dbReferenceType = attrs["type"];
			    dbReferenceId = attrs["id"];
				entry.AddDbEntry(dbReferenceType, dbReferenceId);
            } else if (qName.Equals("molecule") && dbReferenceType.Equals("Ensembl")){
                molecule = new StringBuilder();
            } else if (qName.Equals("property")){
				if (inDbRef){
					entry.AddDbEntryProperty(dbReferenceType, dbReferenceId, attrs["type"], attrs["value"]);
				}
			} else if (qName.Equals("feature")){
				inFeature = true;
				featureType = attrs.ContainsKey("type") ? attrs["type"] : "";
				featureDescription = attrs.ContainsKey("description") ? attrs["description"] : "";
				featureStatus = attrs.ContainsKey("status") ? attrs["status"] : "";
				featureId = attrs.ContainsKey("id") ? attrs["id"] : "";
				entry.AddFeature(featureType, featureDescription, featureStatus, featureId);
			} else if (qName.Equals("sequence")) {
				sequence = new StringBuilder();
			} else if (qName.Equals("keyword")) {
				keyword = new StringBuilder();
			} else if (qName.Equals("accession")) {
				accession = new StringBuilder();
			} else if (qName.Equals("protein")){
				inProtein = true;
			} else if (qName.Equals("recommendedName") && inProtein){
				inProteinRecommendedName = true;
			} else if (qName.Equals("organism")) {
				inOrganism = true;
			} else if (qName.Equals("organismHost")) {
				inOrganismHost = true;
			} else if (qName.Equals("gene")) {
				inGene = true;
			} else if (qName.Equals("fullName") && inProteinRecommendedName){
				proteinFullName = new StringBuilder();
			} else if (qName.Equals("shortName") && inProteinRecommendedName){
				proteinShortName = new StringBuilder();
			} else if (qName.Equals("ecNumber") && inProteinRecommendedName){
				proteinEcNumber = new StringBuilder();
			} else if (qName.Equals("name") && inGene){
				gname = new StringBuilder();
				gnameType = attrs["type"];
			} else if (qName.Equals("name") && level == 1){
				uname = new StringBuilder();
            } else if (qName.Equals("isoform")){
                if (inDbRef)
                    ++numIsoforms;
            }
		}

		private void EndElement(IEquatable<string> qName, HandleUniprotEntry handle, bool isTrembl){
			if (qName.Equals("sequence")){
				entry.Sequence = StringUtils.RemoveWhitespace(sequence.ToString());
				sequence = null;
			} else if (qName.Equals("keyword")) {
				entry.AddKeyword(StringUtils.RemoveWhitespace(keyword.ToString()));
				keyword = null;
            } else if (qName.Equals("molecule") && dbReferenceType.Equals("Ensembl")){
                string mol = molecule.ToString().Trim();
                entry.AddDbEntryProperty(dbReferenceType, dbReferenceId, "isoform ID", mol);
                if (!isoformToEnst.ContainsKey(mol))
                    isoformToEnst.Add(mol, new List<string>());
                isoformToEnst[mol].Add(dbReferenceId);
                molecule = null;
            } else if (qName.Equals("entry")){
				entry.Accessions = accessions.ToArray();
				entry.ProteinFullNames = proteinFullNames.ToArray();
				entry.ProteinShortNames = proteinShortNames.ToArray();
				entry.ProteinEcNumbers = proteinEcNumbers.ToArray();
				entry.GeneNamesAndTypes = gnames.ToArray();
				entry.OrganismNames = onames.ToArray();
				entry.UniprotNames = unames.ToArray();
				entry.IsTrembl = isTrembl;
                if (resolveIsoforms){
                    if (numIsoforms > 1 && isoformToEnst.Count > 1){
                        List<UniprotEntry> isoEntries = entry.ResolveIsoforms(isoformToEnst);
                        foreach (UniprotEntry e in isoEntries){
                            handle(e);
                        }
                    } else
                        handle(entry);
                } else 
				    handle(entry);
			} else if (qName.Equals("dbReference")){
				inDbRef = false;
			} else if (qName.Equals("accession")){
				accessions.Add(StringUtils.RemoveWhitespace(accession.ToString()));
				accession = null;
			} else if (qName.Equals("location")){
				if (inFeature){
					//inFeatureLocation = false;
					entry.AddFeatureLocation(featureBegin, featureEnd);
				}
			} else if (qName.Equals("variation")){
				if (inFeature){
					entry.AddFeatureVariation(StringUtils.RemoveWhitespace(variation.ToString()));
					variation = null;
				}
			} else if (qName.Equals("original")){
				if (inFeature){
					entry.AddFeatureOriginal(StringUtils.RemoveWhitespace(original.ToString()));
					original = null;
				}
			} else if (qName.Equals("feature")){
				inFeature = false;
				foreach (FeatureType type in entry.GetAllFeatureTypes()){
					int c = entry.GetFeatureCount(type);
					if (!featureCounts.ContainsKey(type)){
						featureCounts.Add(type, 0);
					}
					featureCounts[type] += c;
				}
			} else if (qName.Equals("fullName") && inProteinRecommendedName){
				proteinFullNames.Add(proteinFullName.ToString().Trim());
				proteinFullName = null;
			} else if (qName.Equals("shortName") && inProteinRecommendedName){
				proteinShortNames.Add(proteinShortName.ToString().Trim());
				proteinShortName = null;
			} else if (qName.Equals("ecNumber") && inProteinRecommendedName){
				proteinEcNumbers.Add(proteinEcNumber.ToString().Trim());
				proteinEcNumber = null;
			} else if (qName.Equals("name") && inGene){
				gnames.Add(new Tuple<string, string>(gname.ToString().Trim(), gnameType.Trim()));
				gname = null;
				gnameType = null;
			} else if (qName.Equals("name") && inOrganism){
				string on = oname?.ToString().Trim();
				if (@on?.Length > 0){
					onames.Add(@on);
					oname = null;
				}
			} else if (qName.Equals("name") && level == 1){
				unames.Add(uname.ToString().Trim());
				uname = null;
			} else if (qName.Equals("protein")){
				inProtein = false;
			} else if (qName.Equals("recommendedName") && inProtein){
				inProteinRecommendedName = false;
			} else if (qName.Equals("gene")){
				inGene = false;
			} else if (qName.Equals("organism")) {
				inOrganism = false;
			} else if (qName.Equals("organismHost")) {
				inOrganismHost = false;
			}
		}

		private void Characters(string buf, int offset, int len){
			sequence?.Append(buf, offset, len);
			keyword?.Append(buf, offset, len);
			accession?.Append(buf, offset, len);
			proteinFullName?.Append(buf, offset, len);
			proteinShortName?.Append(buf, offset, len);
			proteinEcNumber?.Append(buf, offset, len);
			gname?.Append(buf, offset, len);
			oname?.Append(buf, offset, len);
			uname?.Append(buf, offset, len);
			original?.Append(buf, offset, len);
			variation?.Append(buf, offset, len);
			molecule?.Append(buf, offset, len);
		}
	}
}