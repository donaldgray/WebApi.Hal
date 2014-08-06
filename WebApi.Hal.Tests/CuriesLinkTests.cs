using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.Hal.Tests
{
    public class CuriesLinkTests
    {
        [Fact]
        public void Ctor_ThrowsException_IfNameNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var link = new CuriesLink(null, "/test");
            });
        }

        [Fact]
        public void Ctor_ThrowsException_IfNameEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var link = new CuriesLink("", "/test");
            });
        }

        [Fact]
        public void Ctor_ThrowsException_IfHrefNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var link = new CuriesLink("name", null);
            });
        }

        [Fact]
        public void Ctor_ThrowsException_IfHrefEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var link = new CuriesLink("name", "");
            });
        }

        [Fact]
        public void Ctor_ThrowsException_IfHrefNotValidCurie()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var link = new CuriesLink("name", "/rel");
            });
        }

        [Fact]
        public void Ctor_SetsProperties_IfHrefValidCurie()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var link = new CuriesLink(name, href);

            Assert.Equal(name, link.Name);
            Assert.Equal(href, link.Href);
        }

        [Fact]
        public void GetLink_ReturnsLinkWithHrefAndNameSet()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var curieLink = new CuriesLink(name, href);

            var link = curieLink.GetLink();

            Assert.Equal(name, link.Name);
            Assert.Equal(href, link.Href);
        }

        [Fact]
        public void GetLink_ReturnsLinkWithRelSet()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var curieLink = new CuriesLink(name, href);

            var link = curieLink.GetLink();

            Assert.Equal("curie", link.Rel);
        }

        [Fact]
        public void CreateLinkRelation_ThrowsException_IfNameNull()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var curieLink = new CuriesLink(name, href);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var linkRelation = curieLink.CreateLinkRelation(null);
            });
        }

        [Fact]
        public void CreateLinkRelation_ThrowsException_IfNameEmpty()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var curieLink = new CuriesLink(name, href);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var linkRelation = curieLink.CreateLinkRelation("");
            });
        }

        [Fact]
        public void CreateLinkRelation_ThrowsException_IfNameHasColon()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var curieLink = new CuriesLink(name, href);

            Assert.Throws<ArgumentException>(() =>
            {
                var linkRelation = curieLink.CreateLinkRelation("some:thing");
            });
        }

        [Fact]
        public void CreateLinkRelation_ReturnsCorrectly()
        {
            var href = "/rel/{rel}";
            var name = "name";
            var curieLink = new CuriesLink(name, href);

            var linkRelation = curieLink.CreateLinkRelation("theRel");

            Assert.Equal("name:theRel", linkRelation);
        }
    }
}
