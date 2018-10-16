using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using PluginRawMzMl;

namespace PluginRawMzML.Test
{
	[TestFixture]
    public class MzMLRawFileTest
    {
		[Test]
	    public void TestRawFile()
		{
			using (var raw = new MzMLRawFile())
			{
				var path = @"D:\Downloads\022817_V4_Plasma_8ug_F1_001-4025-02-3-V3-Plasma006.mzML";
				if (!File.Exists(path))
				{
					Assert.Inconclusive("Cannot run unit test without local file");
				}
				raw.Init(path);
				var x = raw.FirstScanNumber;
				var y = raw.LastScanNumber;
				var z = raw.MaxIntensity;
				var posLayer = raw.GetPosLayer();
				for (int i = 0; i < raw.LastScanNumber; i++)
				{
					var spec = posLayer.GetMs1Spectrum(i, false);
					var ms2spec = posLayer.GetMs2Spectrum(i, false);
					var ints = spec.Intensities;
				}
			}
		}
    }
}
