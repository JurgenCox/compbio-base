using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using BaseLibS.Mol;
using BaseLibS.Ms.Data.Protein;
using BaseLibS.Ms.Search;
using BaseLibS.Parse.Misc;



namespace RunSomething
{
    internal class DeepMassPreprocess
    {
        private static void Main()
        {
            string[] paths = { "d:\\Predicted_libraries\\arabidopsis_thaliana\\UP000006548_3702.fasta", "d:\\Predicted_libraries\\arabidopsis_thaliana\\UP000006548_3702_additional.fasta" };
            string outPeptideFile = "d:\\Predicted_libraries\\arabidopsis_thaliana\\peptides.out.txt";
            string peptideFile = "d:\\Predicted_libraries\\arabidopsis_thaliana\\peptides.txt"; 
            IDictionary<string, List<string>> peptideDict = new Dictionary<string, List<string>>();
            foreach (string path in paths)
            {
                FastaParser fp = new FastaParser(path, (header, sequence) => {
                    Protein p = new Protein(sequence, header, "", "", false, false, "", "", false, false);
                    string[] peptides = DigestionUtil.DigestToArray(sequence, header, EnzymeMode.Specific, 7, 30, 1,
                        Tables.ToEnzymes(new[] { "Trypsin/P" }));
                    foreach (string peptide in peptides)
                    {
                        if (peptide.Contains("U")) {continue;}
                        if (!peptideDict.ContainsKey(peptide)) { peptideDict.Add(peptide, new List<string> { p.Accession.Split(" ")[0] }); }
                        else
                        {
                            peptideDict[peptide].Add(p.Accession.Split(" ")[0]);
                        }
                    }
                    return true;
                });
                fp.Parse();
            }

            /*StreamWriter writer = new StreamWriter(outPeptideFile);*/
            using (StreamWriter writer = File.CreateText(outPeptideFile))
            {
                writer.WriteLine("ModifiedSequence,Charge,Fragmentation,MassAnalyzer");
                foreach (string peptide in peptideDict.Keys)
                {
                    writer.WriteLine(string.Join(",",
                        new string[] {
                            peptide, "2", "HCD", "FTMS"
                        }));
                    writer.WriteLine(string.Join(",",
                        new string[] {
                            peptide, "3", "HCD", "FTMS"
                        }));
                }
            }

            using (StreamWriter writer = File.CreateText(peptideFile))
            {
                writer.WriteLine($"sequence\tproteins");
                foreach (KeyValuePair<string, List<string>> pair in peptideDict)
                {
                    writer.WriteLine($"{pair.Key}\t{string.Join(";", pair.Value)}");
                }
            }
            /*StreamWriter writer = new StreamWriter(peptideFile);*/
        }
    }
}

