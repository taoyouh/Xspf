using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Zhaobang.Xspf.Tests
{
    public class XspfTrackListTest
    {
        [Fact]
        public void TestXspfTrackList()
        {
            const string Title = "This is a sample title";
            var xspf = new Xspf();
            var track1 = new XspfTrack()
            {
                Title = Title
            };
            var track2 = new XspfTrack()
            {
                Title = Title
            };

            Assert.NotEqual(track1, track2);

            xspf.TrackList.Add(track1);
            xspf.TrackList.Add(track2);
            Assert.Contains(track1, xspf.TrackList);
            Assert.Contains(track2, xspf.TrackList);

            Assert.Equal(track1, xspf.TrackList[0]);
            Assert.True(track1 == xspf.TrackList[0]);
            Assert.Equal(track2, xspf.TrackList[1]);
            Assert.True(track2 == xspf.TrackList[1]);

            xspf.TrackList.Remove(track1);
            Assert.DoesNotContain(track1, xspf.TrackList);
            Assert.Equal(track2, xspf.TrackList[0]);
        }
    }
}
