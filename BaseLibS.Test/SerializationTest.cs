using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using BaseLibS.Param;
using NUnit.Framework;

namespace BaseLibS.Test
{
    [TestFixture]
    public class SerializationTest
    {
        [Test]
        public void TestParamWithNullValue()
        {
            StringParam sparam = new StringParam("myname", null);
            StringParam sparam2 = (StringParam)sparam.ToXmlAndBack();
            Assert.AreEqual("", sparam2.Value); // "" default value for StringParam (contrived example)
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestBoolParam()
        {
            BoolParam sparam = new BoolParam("myname", false);
            BoolParam sparam2 = (BoolParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestBoolWithSubParams()
        {
            BoolWithSubParams param = new BoolWithSubParams("bws", true)
            {
                SubParamsFalse = new Parameters(new IntParam("false", 42)),
                SubParamsTrue = new Parameters(new BoolParam("true", false))
            };
            BoolWithSubParams param2 = (BoolWithSubParams) param.ToXmlAndBack();
            Assert.AreEqual(param.Name, param2.Name);
            Assert.AreEqual(param.Value, param2.Value);
            Assert.AreEqual(42, param2.SubParamsFalse.GetParam<int>("false").Value);
            Assert.AreEqual(false, param2.SubParamsTrue.GetParam<bool>("true").Value);

        }

        [Test]
        public void TestDictionaryIntValueParam()
        {
            DictionaryIntValueParam param = new DictionaryIntValueParam("dict", new Dictionary<string, int>() { {"a", 1}, {"b", 2} }, new []{"a", "b"});
            DictionaryIntValueParam param2 = (DictionaryIntValueParam) param.ToXmlAndBack();
            Assert.AreEqual(param.Name, param2.Name);
            Assert.IsTrue(param.Value.Keys.SequenceEqual(param2.Value.Keys));
            Assert.IsTrue(param.Value.Values.SequenceEqual(param2.Value.Values));
            Assert.IsTrue(param.Keys.SequenceEqual(param2.Keys));
        }

        [Test]
        public void TestDoubleParam()
        {
            DoubleParam sparam = new DoubleParam("myname", 42.0);
            DoubleParam sparam2 = (DoubleParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestFileParam()
        {
            FileParam sparam = new FileParam("myname", "myvalue");
            FileParam sparam2 = (FileParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestFolderParam()
        {
            FolderParam sparam = new FolderParam("myname", "myvalue");
            FolderParam sparam2 = (FolderParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestIntParam()
        {
            IntParam sparam = new IntParam("myname", 42);
            IntParam sparam2 = (IntParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestLabelParam()
        {
            LabelParam sparam = new LabelParam("myname", "my\nmultiline\n\n\nvalue");
            LabelParam sparam2 = (LabelParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }

        [Test]
        public void TestMs1LabelParam()
        {
            Ms1LabelParam sparam = new Ms1LabelParam("myname", new [] { new [] {1,2,3}, new [] {3,4,5} }) { Values = new [] {"a", "b"}, Multiplicity = 12};
            Ms1LabelParam sparam2 = (Ms1LabelParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Name, sparam2.Name);
            Assert.AreEqual(sparam.Multiplicity, sparam2.Multiplicity);
            Assert.IsTrue(sparam.Value.SelectMany(x => x).SequenceEqual(sparam2.Value.SelectMany(x => x)));
            Assert.IsTrue(sparam.Values.SequenceEqual(sparam2.Values));
        }

        [Test]
        public void TestMultiChoiceMultiBinParam()
        {
            MultiChoiceMultiBinParam sparam = new MultiChoiceMultiBinParam("myname", new [] { new [] {1,2,3}, new [] {3,4,5} }) { Values = new [] {"a", "b"}, Bins = new [] {"c", "d"}};
            MultiChoiceMultiBinParam sparam2 = (MultiChoiceMultiBinParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Name, sparam2.Name);
            Assert.IsTrue(sparam.Value.SelectMany(x => x).SequenceEqual(sparam2.Value.SelectMany(x => x)));
            Assert.IsTrue(sparam.Values.SequenceEqual(sparam2.Values));
            Assert.IsTrue(sparam.Bins.SequenceEqual(sparam2.Bins));
        }

        [Test]
        public void TestMultiChoiceParam()
        {
            MultiChoiceParam sparam = new MultiChoiceParam("myname", new [] {1,2,3}) { Values = new [] {"a", "b"} , Repeats = true};
            MultiChoiceParam sparam2 = (MultiChoiceParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Name, sparam2.Name);
            Assert.AreEqual(sparam.Repeats, sparam2.Repeats);
            Assert.IsTrue(sparam.Value.SequenceEqual(sparam2.Value));
            Assert.IsTrue(sparam.Values.SequenceEqual(sparam2.Values));
        }

        [Test]
        public void TestMultiFileParam()
        {
            MultiFileParam sparam = new MultiFileParam("myname", new [] {"file1", "file2"}) { Filter = "somefilter" };
            MultiFileParam sparam2 = (MultiFileParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Name, sparam2.Name);
            Assert.AreEqual(sparam.Filter, sparam2.Filter);
            Assert.IsTrue(sparam.Value.SequenceEqual(sparam2.Value));
        }

        [Test]
        public void TestMultiStringParam()
        {
            MultiStringParam sparam = new MultiStringParam("myname", new[] {"file1", "file2"});
            MultiStringParam sparam2 = (MultiStringParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Name, sparam2.Name);
            Assert.IsTrue(sparam.Value.SequenceEqual(sparam2.Value));
        }

        [Test]
        public void TestEmptyParameterGroup()
        {
            ParameterGroup sparam = new ParameterGroup(new Parameter[0], "myname", false);
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(sparam.GetType());
            serializer.Serialize(writer, sparam);
            StringReader writer2 = new StringReader(writer.ToString());
            ParameterGroup sparam2 = (ParameterGroup)serializer.Deserialize(writer2);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }
        [Test]
        public void TestParameterGroup()
        {
            ParameterGroup sparam = new ParameterGroup(new Parameter[] { new IntParam("int", 42), new StringParam("string", "42") }, "myname", false);
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(sparam.GetType());
            serializer.Serialize(writer, sparam);
            StringReader writer2 = new StringReader(writer.ToString());
            ParameterGroup sparam2 = (ParameterGroup)serializer.Deserialize(writer2);
            Assert.AreEqual(sparam.Name, sparam2.Name);
            Assert.AreEqual(42, ((IntParam)sparam[0]).Value);
            Assert.AreEqual("42", ((StringParam)sparam[1]).Value);
        }

        [Test]
        public void TestParameters()
        {
            Parameters parameters = new Parameters(new StringParam("myname", "myvalue"), new IntParam("some int", 42));
            Parameters parameters2 = parameters.ToXmlAndBack();
            Assert.AreEqual("myvalue", parameters2.GetParam<string>("myname").Value);
            Assert.AreEqual(42, parameters.GetParam<int>("some int").Value);
        }

        [Test]
        public void TestEmptyParameters()
        {
            Parameters parameters = new Parameters();
            Parameters parameters2 = parameters.ToXmlAndBack();
            Assert.IsNotNull(parameters2);
        }

        [Test]
        public void TestGroupedParameters()
        {
            Parameters parameters = new Parameters();
            parameters.AddParameterGroup(new Parameter[] {new StringParam("myname", "myvalue")}, "grp1", false);
            parameters.AddParameterGroup(new Parameter[] {new IntParam("some int", 42)}, "grp2", true);
            Parameters parameters2 = parameters.ToXmlAndBack();
            Assert.AreEqual("myvalue", parameters2.GetSubGroupAt(0).GetParam<string>("myname").Value);
            Assert.AreEqual(42, parameters.GetSubGroupAt(1).GetParam<int>("some int").Value);
            Assert.AreEqual("myvalue", parameters2.GetParam<string>("myname").Value);
        }

        [Test]
        public void TestRegexMatchParam()
        {
            RegexMatchParam parameter = new RegexMatchParam("myname", new Regex(".*"), new List<string>() {"a", "b" });
            RegexMatchParam parameter2 = (RegexMatchParam) parameter.ToXmlAndBack();
            Assert.AreEqual(parameter.Name, parameter2.Name);
            Assert.AreEqual(parameter.Value.ToString(), parameter2.Value.ToString());
            Assert.IsTrue(parameter.Previews.SequenceEqual(parameter2.Previews));
        }

        [Test]
        public void TestRegexReplaceParam()
        {
            RegexReplaceParam parameter = new RegexReplaceParam("myname", new Regex(".*"), "asdf", new List<string>() {"a", "b" });
            RegexReplaceParam parameter2 = (RegexReplaceParam) parameter.ToXmlAndBack();
            Assert.AreEqual(parameter.Name, parameter2.Name);
            Assert.AreEqual(parameter.Value.Item1.ToString(), parameter2.Value.Item1.ToString());
            Assert.AreEqual(parameter.Value.Item2, parameter2.Value.Item2);
            Assert.IsTrue(parameter.Previews.SequenceEqual(parameter2.Previews));
        }

        [Test]
        public void TestRegexReplaceParamNoPreview()
        {
            RegexReplaceParam parameter = new RegexReplaceParam("myname", new Regex(".*"), "asdf", new List<string>());
            RegexReplaceParam parameter2 = (RegexReplaceParam) parameter.ToXmlAndBack();
            Assert.AreEqual(parameter.Name, parameter2.Name);
            Assert.AreEqual(parameter.Value.Item1.ToString(), parameter2.Value.Item1.ToString());
            Assert.AreEqual(parameter.Value.Item2, parameter2.Value.Item2);
            Assert.IsTrue(parameter.Previews.SequenceEqual(parameter2.Previews));
        }


        [Test]
        public void TestSingleChoiceParam()
        {
            SingleChoiceParam param = new SingleChoiceParam("choice", 1) { Values = new List<string> {"c1", "b2"} };
            SingleChoiceParam param2 = (SingleChoiceParam) param.ToXmlAndBack();
            Assert.AreEqual(param.Name, param2.Name);
            Assert.AreEqual(param.Value, param2.Value);
            Assert.IsTrue(param.Values.SequenceEqual(param2.Values));
        }

        [Test]
        public void TestSingleChoiceWithSubParams()
        {
            SingleChoiceWithSubParams param = new SingleChoiceWithSubParams("choice", 1) { Values = new List<string> {"c1", "b2"}, SubParams = new []
            {
                new Parameters(new IntParam("sub1", 42)),
                new Parameters(new IntParam("sub2", 82)) 
            }};
            SingleChoiceWithSubParams param2 = (SingleChoiceWithSubParams) param.ToXmlAndBack();
            Assert.AreEqual(param.Name, param2.Name);
            Assert.AreEqual(param.Value, param2.Value);
            Assert.IsTrue(param.Values.SequenceEqual(param2.Values));
            Assert.AreEqual(42, param2.SubParams[0].GetParam<int>("sub1").Value );
            Assert.AreEqual(82, param2.SubParams[1].GetParam<int>("sub2").Value );
        }

        [Test]
        public void TestSingleChoiceWithSubParamsInParameters()
        {
            SingleChoiceWithSubParams param = new SingleChoiceWithSubParams("choice", 1) { Values = new List<string> {"c1", "b2"}, SubParams = new []
            {
                new Parameters(new Parameter[0]),
                new Parameters(new IntParam("sub2", 82)) 
            }};
            SingleChoiceWithSubParams param2 = (SingleChoiceWithSubParams) param.ToXmlAndBack();
            Assert.AreEqual(param.Name, param2.Name);
            Assert.AreEqual(param.Value, param2.Value);
            Assert.IsTrue(param.Values.SequenceEqual(param2.Values));
            Assert.IsNotNull(param2.SubParams[0]);
            Assert.AreEqual(82, param2.SubParams[1].GetParam<int>("sub2").Value );
        }

        [Test]
        public void TestStringParam()
        {
            StringParam sparam = new StringParam("myname", "myvalue");
            StringParam sparam2 = (StringParam) sparam.ToXmlAndBack();
            Assert.AreEqual(sparam.Value, sparam2.Value);
            Assert.AreEqual(sparam.Name, sparam2.Name);
        }
    }
    public static class ParameterExtensions
    {
        public static Parameters ToXmlAndBack(this Parameters p)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(p.GetType());
            serializer.Serialize(writer, p);
            StringReader writer2 = new StringReader(writer.ToString());
            return (Parameters) serializer.Deserialize(writer2);
        }
        public static Parameter ToXmlAndBack(this Parameter p)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(p.GetType());
            serializer.Serialize(writer, p);
            StringReader writer2 = new StringReader(writer.ToString());
            return (Parameter) serializer.Deserialize(writer2);
        }
        public static string ToXml(this Parameters p)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(p.GetType());
            serializer.Serialize(writer, p);
            return writer.ToString();
        }
        public static string ToXml(this Parameter p)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(p.GetType());
            serializer.Serialize(writer, p);
            return writer.ToString();
        }

    }
}
