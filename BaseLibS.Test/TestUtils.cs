using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BaseLibS.Test
{
    public static class TestUtils
    {
        public static float[,] ReadMatrix(string path)
        {
            float[,] vals;
            using (GZipStream stream = new GZipStream(new FileStream(path, FileMode.Open), CompressionMode.Decompress))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string colnames = reader.ReadLine();
                    string[] coltypes = reader.ReadLine().Replace("#!{Type}", "").Split('\t').TakeWhile(s => s.Equals("E")).ToArray();
                    int m = coltypes.Length;
                    string line;
                    List<float[]> lines = new List<float[]>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("#")) { continue; }
                        lines.Add(line.Split('\t').Take(m).Select(float.Parse).ToArray());
                    }
                    int n = lines.Count;
                    vals = new float[n,m];
                    for (int row = 0; row < n; row++)
                    {
                        float[] rowVals = lines[row];
                        for (int col = 0; col < m; col++)
                        {
                            vals[row, col] = rowVals[col];
                        }
                    }
                }
                
            }
            return vals;
        }
    }
}