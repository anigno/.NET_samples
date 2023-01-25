using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnignoraDataTypes.HtmlReporting;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester
{
    [TestFixture]
    public class TestHtmlReporting
    {
        [Test]
        public void TestCreating()
        {
            HtmlReporter reporter=new HtmlReporter();
            
            reporter.HtmlTag.AddTag(new TagHead());
            Tag body = reporter.HtmlTag.AddTag(new TagBody());
            body.AddAttribute(new TagAttribute("bgColor", "white")); body.AddAttribute(new TagAttribute("Color", "yellow")); body.InnerText = "Test Body"; body.AddTag(new TagBr());
            Tag table = body.AddTag(new TagTable());
            table.AddAttribute(new TagAttribute("border", "1"));
            table.AddAttribute(new TagAttribute("bgcolor", "gray"));
            Tag tr = table.AddTag(new TagTr());
            tr.AddTag(new TagTd() { InnerText = "columnl" });
            tr.AddTag(new TagTd() { InnerText = "column2" });
            tr.AddTag(new TagTd() { InnerText = "column3" });
            table.AddTag(new TagTr());
            File.WriteAllText("file.html", reporter.Report);
            Console.WriteLine(reporter.Report);
            const string EXPECTED_RESULT = "<HTML><Head></Head><Body bgColor=\"white\"  Color=\"yellow\" >Test Body<BR><Table "+
                                          "border=\"1\"  bgcolor=\"gray\" ><TR><TD>columnl</TD><TD>column2</TD><TD>column3</TD></TR><TR></TR>"+
                                          "</Table></Body></HTML>";
            string actual = reporter.Report.Replace(Environment.NewLine, "");
            Assert.AreEqual(EXPECTED_RESULT,actual);
        }
    }
}
