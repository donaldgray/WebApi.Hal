using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Hal.JsonConverters;
using Xunit;

namespace WebApi.Hal.Tests.JsonConverters
{
    public class LinksConverterTests
    {
        [Fact]
        public void WriteJson_OutputsCurieSingular_IfOnlyOneCurieLink()
        {
            dynamic result = GetResults(1);

            Assert.NotNull(result.curie);
        }

        [Fact]
        public void WriteJson_OutputsCuriesPlural_IfOnlyMultipleCuriesLink()
        {
            dynamic result = GetResults(3);

            Assert.NotNull(result.curies);
        }

        private JObject GetResults(int numberOfCuries)
        {
            var converter = new LinksConverter();

            var result = new StringBuilder();

            var links = GetLinks(numberOfCuries);

            using (TextWriter tw = new StringWriter(result))
            using (JsonWriter writer = new JsonTextWriter(tw))
            {
                converter.WriteJson(writer, links, null);
            }

            return JObject.Parse(result.ToString());
        }

        private IEnumerable<Link> GetLinks(int numberOfCuries)
        {
            var links = new List<Link>();
            for (int x = 0; x < numberOfCuries; x++)
            {
                var curieLink = new CuriesLink("curie" + x, "/rels/" + x + "/{rel}");
                links.Add(curieLink.GetLink());
            }

            return links;
        }
    }
}