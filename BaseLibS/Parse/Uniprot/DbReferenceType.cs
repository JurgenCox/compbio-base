using BaseLibS.Num;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BaseLibS.Parse.Uniprot
{
    [Serializable]
	public class DbReferenceType{
		public static DbReferenceType go = new DbReferenceType("go");
		public static DbReferenceType kegg = new DbReferenceType("kegg");
		public static DbReferenceType pfam = new DbReferenceType("pfam");
		public static DbReferenceType pdb = new DbReferenceType("pdb");
		public static DbReferenceType wormBase = new DbReferenceType("wormbase");
		public static DbReferenceType reactome = new DbReferenceType("reactome");
		public static DbReferenceType mgi = new DbReferenceType("mgi");
		public static DbReferenceType sgd = new DbReferenceType("sgd");
		public static DbReferenceType mim = new DbReferenceType("mim");
		public static DbReferenceType prosite = new DbReferenceType("prosite");
		public static DbReferenceType interPro = new DbReferenceType("interpro");
		public static DbReferenceType prints = new DbReferenceType("prints");
		public static DbReferenceType smart = new DbReferenceType("smart");
		public static DbReferenceType flyBase = new DbReferenceType("flybase");
		public static DbReferenceType wormPep = new DbReferenceType("wormpep");
		public static DbReferenceType ncbiTaxonomy = new DbReferenceType("ncbi taxonomy");
		public static DbReferenceType doi = new DbReferenceType("doi");
		public static DbReferenceType pubMed = new DbReferenceType("pubmed");
		public static DbReferenceType embl = new DbReferenceType("embl");
		public static DbReferenceType refSeq = new DbReferenceType("refseq");
		public static DbReferenceType geneId = new DbReferenceType("geneid");
		public static DbReferenceType protClustDb = new DbReferenceType("protclustdb");
		public static DbReferenceType medLine = new DbReferenceType("medline");
		public static DbReferenceType proteinModelPortal = new DbReferenceType("proteinmodelportal");
		public static DbReferenceType smr = new DbReferenceType("smr");
		public static DbReferenceType gene3D = new DbReferenceType("gene3d");
		public static DbReferenceType panther = new DbReferenceType("panther");
		public static DbReferenceType pirsf = new DbReferenceType("pirsf");
		public static DbReferenceType supfam = new DbReferenceType("supfam");
		public static DbReferenceType uniGene = new DbReferenceType("unigene");
		public static DbReferenceType dip = new DbReferenceType("dip");
		public static DbReferenceType intAct = new DbReferenceType("intact");
		public static DbReferenceType pride = new DbReferenceType("pride");
		public static DbReferenceType ensemblPlants = new DbReferenceType("ensemblplants");
		public static DbReferenceType gramene = new DbReferenceType("gramene");
		public static DbReferenceType eggNog = new DbReferenceType("eggnog");
		public static DbReferenceType oma = new DbReferenceType("oma");
		public static DbReferenceType phylomeDb = new DbReferenceType("phylomedb");
		public static DbReferenceType string1 = new DbReferenceType("string");
		public static DbReferenceType ctd = new DbReferenceType("ctd");
		public static DbReferenceType xenbase = new DbReferenceType("xenbase");
		public static DbReferenceType hogenom = new DbReferenceType("hogenom");
		public static DbReferenceType hovergen = new DbReferenceType("hovergen");
		public static DbReferenceType orthoDb = new DbReferenceType("orthodb");
		public static DbReferenceType ipi = new DbReferenceType("ipi");
		public static DbReferenceType geneTree = new DbReferenceType("genetree");
		public static DbReferenceType patric = new DbReferenceType("patric");
		public static DbReferenceType arrayExpress = new DbReferenceType("arrayexpress");
		public static DbReferenceType brenda = new DbReferenceType("brenda");
		public static DbReferenceType ec = new DbReferenceType("ec");
		public static DbReferenceType germOnline = new DbReferenceType("germonline");
		public static DbReferenceType ensemblMetazoa = new DbReferenceType("ensemblmetazoa");
		public static DbReferenceType bioCyc = new DbReferenceType("biocyc");
		public static DbReferenceType genomeReviews = new DbReferenceType("genomereviews");
		public static DbReferenceType pir = new DbReferenceType("pir");
		public static DbReferenceType ensembl = new DbReferenceType("ensembl");
		public static DbReferenceType bgee = new DbReferenceType("bgee");
		public static DbReferenceType ko = new DbReferenceType("ko");
		public static DbReferenceType inParanoid = new DbReferenceType("inparanoid");
		public static DbReferenceType nextBio = new DbReferenceType("nextbio");
		public static DbReferenceType hamap = new DbReferenceType("hamap");
		public static DbReferenceType tigrfams = new DbReferenceType("tigrfams");
		public static DbReferenceType ensemblBacteria = new DbReferenceType("ensemblbacteria");
		public static DbReferenceType dmdm = new DbReferenceType("dmdm");
		public static DbReferenceType geneCards = new DbReferenceType("genecards");
		public static DbReferenceType hgnc = new DbReferenceType("hgnc");
		public static DbReferenceType nextProt = new DbReferenceType("nextprot");
		public static DbReferenceType cleanEx = new DbReferenceType("cleanex");
		public static DbReferenceType genevestigator = new DbReferenceType("genevestigator");
		public static DbReferenceType mint = new DbReferenceType("mint");
		public static DbReferenceType uniProt = new DbReferenceType("uniprot");
		public static DbReferenceType peptideAtlas = new DbReferenceType("peptideatlas");
		public static DbReferenceType ensemblFungi = new DbReferenceType("ensemblfungi");
		public static DbReferenceType cygd = new DbReferenceType("cygd");
		public static DbReferenceType tair = new DbReferenceType("tair");
		public static DbReferenceType ucsc = new DbReferenceType("ucsc");
		public static DbReferenceType proMex = new DbReferenceType("promex");
		public static DbReferenceType proDom = new DbReferenceType("prodom");
		public static DbReferenceType pdbSum = new DbReferenceType("pdbsum");
		public static DbReferenceType phosphoSite = new DbReferenceType("phosphosite");
		public static DbReferenceType hssp = new DbReferenceType("hssp");
		public static DbReferenceType tigr = new DbReferenceType("tigr");
		public static DbReferenceType ensemblProtists = new DbReferenceType("ensemblprotists");
		public static DbReferenceType dictyBase = new DbReferenceType("dictybase");
		public static DbReferenceType hinvDb = new DbReferenceType("h-invdb");
		public static DbReferenceType pharmGkb = new DbReferenceType("pharmgkb");
		public static DbReferenceType rgd = new DbReferenceType("rgd");
		public static DbReferenceType zfin = new DbReferenceType("zfin");
		public static DbReferenceType merops = new DbReferenceType("merops");
		public static DbReferenceType cazy = new DbReferenceType("cazy");
		public static DbReferenceType dnasu = new DbReferenceType("dnasu");
		public static DbReferenceType hpa = new DbReferenceType("hpa");
		public static DbReferenceType pseudoCap = new DbReferenceType("pseudocap");
		public static DbReferenceType tcdb = new DbReferenceType("tcdb");
		public static DbReferenceType vectorBase = new DbReferenceType("vectorbase");
		public static DbReferenceType drugBank = new DbReferenceType("drugbank");
		public static DbReferenceType swiss2Dpage = new DbReferenceType("swiss-2dpage");
		public static DbReferenceType dosacCobs2Dpage = new DbReferenceType("dosac-cobs-2dpage");
		public static DbReferenceType ucd2Dpage = new DbReferenceType("ucd-2dpage");
		public static DbReferenceType legioList = new DbReferenceType("legiolist");
		public static DbReferenceType orphanet = new DbReferenceType("orphanet");
		public static DbReferenceType genoList = new DbReferenceType("genolist");
		public static DbReferenceType pomBase = new DbReferenceType("pombase");
		public static DbReferenceType phosSite = new DbReferenceType("phossite");
		public static DbReferenceType echoBase = new DbReferenceType("echobase");
		public static DbReferenceType ecoGene = new DbReferenceType("ecogene");
		public static DbReferenceType world2Dpage = new DbReferenceType("world-2dpage");
		public static DbReferenceType cornea2Dpage = new DbReferenceType("cornea-2dpage");
		public static DbReferenceType tubercuList = new DbReferenceType("tuberculist");
		public static DbReferenceType reproduction2Dpage = new DbReferenceType("reproduction-2dpage");
		public static DbReferenceType agricola = new DbReferenceType("agricola");
		public static DbReferenceType agd = new DbReferenceType("agd");
		public static DbReferenceType leproma = new DbReferenceType("leproma");
		public static DbReferenceType allergome = new DbReferenceType("allergome");
		public static DbReferenceType geneFarm = new DbReferenceType("genefarm");
		public static DbReferenceType peroxiBase = new DbReferenceType("peroxibase");
		public static DbReferenceType eco2Dbase = new DbReferenceType("eco2dbase");
		public static DbReferenceType cgd = new DbReferenceType("cgd");
		public static DbReferenceType glycoSuiteDb = new DbReferenceType("glycosuitedb");
		public static DbReferenceType siena2Dpage = new DbReferenceType("siena-2dpage");
		public static DbReferenceType pmapCutDb = new DbReferenceType("pmap-cutdb");
		public static DbReferenceType bindingDb = new DbReferenceType("bindingdb");
		public static DbReferenceType pathwayInteractionDb = new DbReferenceType("pathway_interaction_db");
		public static DbReferenceType twoDBaseEcoli = new DbReferenceType("2dbase-ecoli");
		public static DbReferenceType ogp = new DbReferenceType("ogp");
		public static DbReferenceType maizeGdb = new DbReferenceType("maizegdb");
		public static DbReferenceType conoServer = new DbReferenceType("conoserver");
		public static DbReferenceType disProt = new DbReferenceType("disprot");
		public static DbReferenceType aarhusGhent2Dpage = new DbReferenceType("aarhus/ghent-2dpage");
		public static DbReferenceType euPathDb = new DbReferenceType("eupathdb");
		public static DbReferenceType phci2Dpage = new DbReferenceType("phci-2dpage");
		public static DbReferenceType compluyeast2Dpage = new DbReferenceType("compluyeast-2dpage");
		public static DbReferenceType reBase = new DbReferenceType("rebase");
		public static DbReferenceType arachnoServer = new DbReferenceType("arachnoserver");
		public static DbReferenceType euHcvDb = new DbReferenceType("euhcvdb");
		public static DbReferenceType pmma2Dpage = new DbReferenceType("pmma-2dpage");
		public static DbReferenceType pptaseDb = new DbReferenceType("pptasedb");
		public static DbReferenceType ratHeart2Dpage = new DbReferenceType("rat-heart-2dpage");
		public static DbReferenceType anu2Dpage = new DbReferenceType("anu-2dpage");
		public static DbReferenceType ruleBase = new DbReferenceType("rulebase");
		public static DbReferenceType saas = new DbReferenceType("saas");
		public static DbReferenceType pirsr = new DbReferenceType("pirsr");
		public static DbReferenceType pirnr = new DbReferenceType("pirnr");
		public static DbReferenceType genpept = new DbReferenceType("genpept");
		public static DbReferenceType evolutionaryTrace = new DbReferenceType("evolutionarytrace");
		public static DbReferenceType uniPathway = new DbReferenceType("unipathway");
		public static DbReferenceType genomeRnai = new DbReferenceType("genomernai");
		public static DbReferenceType paxDb = new DbReferenceType("paxdb");
		public static DbReferenceType signaLink = new DbReferenceType("signalink");
		public static DbReferenceType hamapRule = new DbReferenceType("hamap-rule");
		public static DbReferenceType chiTars = new DbReferenceType("chitars");
		public static DbReferenceType chEmbl = new DbReferenceType("chembl");
		public static DbReferenceType sabioRk = new DbReferenceType("sabio-rk");
		public static DbReferenceType mycoClap = new DbReferenceType("mycoclap");
		public static DbReferenceType geneWiki = new DbReferenceType("genewiki");
		public static DbReferenceType uniCarbKb = new DbReferenceType("unicarbkb");
		public static DbReferenceType bioGrid = new DbReferenceType("biogrid");
		public static DbReferenceType treeFam = new DbReferenceType("treefam");
		public static DbReferenceType pro = new DbReferenceType("pro");
		public static DbReferenceType guideToPharmacology = new DbReferenceType("guidetopharmacology");
		public static DbReferenceType uniProtKb = new DbReferenceType("uniprotkb");
		public static DbReferenceType ccds = new DbReferenceType("ccds");
		public static DbReferenceType maxqb = new DbReferenceType("maxqb");
		public static DbReferenceType prositeProrule = new DbReferenceType("prosite-prorule");
		public static DbReferenceType geneReviews = new DbReferenceType("genereviews");
		public static DbReferenceType proteomes = new DbReferenceType("proteomes");
		public static DbReferenceType expressionAtlas = new DbReferenceType("expressionatlas");
		public static DbReferenceType bioMuta = new DbReferenceType("biomuta");
		public static DbReferenceType chEbi = new DbReferenceType("chebi");
		public static DbReferenceType depod = new DbReferenceType("depod");
		public static DbReferenceType moonProt = new DbReferenceType("moonprot");
		public static DbReferenceType SwissPalm = new DbReferenceType("swisspalm");
		public static DbReferenceType iPTMnet = new DbReferenceType("iptmnet");
		public static DbReferenceType PhosphoSitePlus = new DbReferenceType("phosphositeplus");
		public static DbReferenceType TopDownProteomics = new DbReferenceType("topdownproteomics");
		public static DbReferenceType Genevisible = new DbReferenceType("genevisible");
		public static DbReferenceType CORUM = new DbReferenceType("corum");
		public static DbReferenceType ELM = new DbReferenceType("elm");
		public static DbReferenceType EPD = new DbReferenceType("epd");
		public static DbReferenceType DisGeNET = new DbReferenceType("disgenet");
		public static DbReferenceType MalaCards = new DbReferenceType("malacards");
		public static DbReferenceType OpenTargets = new DbReferenceType("opentargets");
		public static DbReferenceType SIGNOR = new DbReferenceType("signor");
		public static DbReferenceType Araport = new DbReferenceType("araport");
		public static DbReferenceType CDD = new DbReferenceType("cdd");
		public static DbReferenceType SFLD = new DbReferenceType("sfld");
		public static DbReferenceType ESTHER = new DbReferenceType("esther");
		public static DbReferenceType SwissLipids = new DbReferenceType("swisslipids");
		public static DbReferenceType GeneDB = new DbReferenceType("genedb");
		public static DbReferenceType CollecTF = new DbReferenceType("collectf");
		public static DbReferenceType WBParaSite = new DbReferenceType("wbparasite");
		public static DbReferenceType IMGTGENEDB = new DbReferenceType("imgt_gene-db");
		public static DbReferenceType SAM = new DbReferenceType("sam");

        public static string[] allDbReferenceTypeStrings;
		public static DbReferenceType[] allDbReferenceTypes = CreateDbReferenceTypeList();

		private static DbReferenceType[] CreateDbReferenceTypeList(){
			List<DbReferenceType> ft = new List<DbReferenceType>{
				go,
				kegg,
				pfam,
				pdb,
				wormBase,
				reactome,
				mgi,
				sgd,
				mim,
				prosite,
				interPro,
				prints,
				smart,
				flyBase,
				wormPep,
				ncbiTaxonomy,
				doi,
				pubMed,
				embl,
				refSeq,
				geneId,
				protClustDb,
				medLine,
				proteinModelPortal,
				smr,
				gene3D,
				panther,
				pirsf,
				supfam,
				uniGene,
				dip,
				intAct,
				pride,
				ensemblPlants,
				gramene,
				eggNog,
				oma,
				phylomeDb,
				string1,
				ctd,
				xenbase,
				hogenom,
				hovergen,
				orthoDb,
				ipi,
				geneTree,
				patric,
				arrayExpress,
				brenda,
				ec,
				germOnline,
				ensemblMetazoa,
				bioCyc,
				genomeReviews,
				pir,
				ensembl,
				bgee,
				ko,
				inParanoid,
				nextBio,
				hamap,
				tigrfams,
				ensemblBacteria,
				dmdm,
				geneCards,
				hgnc,
				nextProt,
				cleanEx,
				genevestigator,
				mint,
				uniProt,
				peptideAtlas,
				ensemblFungi,
				cygd,
				tair,
				ucsc,
				proMex,
				proDom,
				pdbSum,
				phosphoSite,
				hssp,
				tigr,
				ensemblProtists,
				dictyBase,
				hinvDb,
				pharmGkb,
				rgd,
				zfin,
				merops,
				cazy,
				dnasu,
				hpa,
				pseudoCap,
				tcdb,
				vectorBase,
				drugBank,
				swiss2Dpage,
				dosacCobs2Dpage,
				ucd2Dpage,
				legioList,
				orphanet,
				genoList,
				pomBase,
				phosSite,
				echoBase,
				ecoGene,
				world2Dpage,
				cornea2Dpage,
				tubercuList,
				reproduction2Dpage,
				agricola,
				agd,
				leproma,
				allergome,
				geneFarm,
				peroxiBase,
				eco2Dbase,
				cgd,
				glycoSuiteDb,
				siena2Dpage,
				pmapCutDb,
				bindingDb,
				pathwayInteractionDb,
				twoDBaseEcoli,
				ogp,
				maizeGdb,
				conoServer,
				disProt,
				aarhusGhent2Dpage,
				euPathDb,
				phci2Dpage,
				compluyeast2Dpage,
				reBase,
				arachnoServer,
				euHcvDb,
				pmma2Dpage,
				pptaseDb,
				ratHeart2Dpage,
				anu2Dpage,
				ruleBase,
				saas,
				pirsr,
				pirnr,
				genpept,
				evolutionaryTrace,
				uniPathway,
				genomeRnai,
				paxDb,
				signaLink,
				hamapRule,
				chiTars,
				chEmbl,
				sabioRk,
				mycoClap,
				geneWiki,
				uniCarbKb,
				bioGrid,
				treeFam,
				pro,
				guideToPharmacology,
				uniProtKb,
				ccds,
				maxqb,
				prositeProrule,
				geneReviews,
				proteomes,
				expressionAtlas,
				bioMuta,
				chEbi,
				depod,
				moonProt,
                SwissPalm,
                iPTMnet,
                PhosphoSitePlus,
                TopDownProteomics,
                Genevisible,
                CORUM,
                ELM,
                EPD,
                DisGeNET,
                MalaCards,
                OpenTargets,
                SIGNOR,
                Araport,
                CDD,
                SFLD,
                ESTHER,
                SwissLipids,
                GeneDB,
                CollecTF,
                WBParaSite,
			    IMGTGENEDB,
                SAM
            };

			allDbReferenceTypeStrings = new string[ft.Count];
			for (int i = 0; i < allDbReferenceTypeStrings.Length; i++){
				allDbReferenceTypeStrings[i] = ft[i].UniprotName;
            }
			int[] o = ArrayUtils.Order(allDbReferenceTypeStrings);
			allDbReferenceTypeStrings = ArrayUtils.SubArray(allDbReferenceTypeStrings, o);
			DbReferenceType[] result = new DbReferenceType[ft.Count];
			for (int i = 0; i < result.Length; i++){
				result[i] = ft[o[i]];
				result[i].Index = i;
			}
			return result;
		}

		public static DbReferenceType GetDbReferenceType(string s){
			string q = s.ToLower();
		    
            int index = Array.BinarySearch(allDbReferenceTypeStrings, q);
            if (index < 0)
            {
                throw new Exception("Unknown DbReference type: " + s);
            }
            return allDbReferenceTypes[index];
		}

		public string UniprotName { get; set; }
		public int Index { get; set; }
		public DbReferenceType(string uniprotName) { UniprotName = uniprotName; }
	}
}