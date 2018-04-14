using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Zhaobang.Xspf.Tests
{
    public class XspfTrackTest
    {
        private static readonly Uri TestUri = new Uri("http://www.taoyouh.cn");

        [Fact]
        public void NewXspfTrackTest()
        {
            XspfTrack track = new XspfTrack(true);
            
            track.Location = TestUri;
            Assert.Equal(TestUri, track.Location);
        }
    }
}
