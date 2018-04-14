using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Zhaobang.Xspf.Tests
{
    public class XspfTest
    {
        private const string TestString = "hello";

        [Fact]
        public void NewXspfTest()
        {
            var xspf = new Xspf(true);

            XspfCommonTest(xspf);
        }

        [Fact]
        public void LoadXspfTest()
        {
            var xspf = Xspf.Load("Data/Utf8WithBom.xspf", true);
            Assert.Equal(new Uri("file:///music/song_1.ogg"), xspf.TrackList[0].Location);

            XspfCommonTest(xspf);
        }

        [Fact]
        public void LoadXspfNotStrictTest()
        {
            var xspf = Xspf.Load("Data/NotStrict.xspf", false);
            Assert.Equal(new Uri("file:///music/song_1.ogg"), xspf.TrackList[0].Location);

            XspfCommonTest(xspf);
        }

        private void XspfCommonTest(Xspf xspf)
        {
            xspf.Annotation = TestString;
            Assert.Equal(TestString, xspf.Annotation);

            xspf.Annotation = null;
            Assert.Null(xspf.Annotation);

            xspf.Creator = TestString;
            Assert.Equal(TestString, xspf.Creator);

            xspf.Creator = null;
            Assert.Null(xspf.Creator);

            Assert.NotNull(xspf.TrackList);
        }
    }
}
