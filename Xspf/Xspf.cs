using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Zhaobang.Xspf
{
    /// <summary>
    /// Represents a XSPF playlist
    /// </summary>
    public class Xspf
    {
        internal const string NS = "http://xspf.org/ns/0/";
        internal const string NS1 = "http://xspf.org/ns/0";

        private readonly bool isStrict;

        private readonly XDocument xDoc;

        /// <summary>
        /// Gets the playlist element in XML document.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// The element is not found.
        /// </exception>
        private XElement Playlist
        {
            get
            {
                XElement result = xDoc.Element(XName.Get("playlist", NS));
                if (result != null)
                    return result;

                result = xDoc.Element(XName.Get("playlist", NS1));
                if (result != null)
                    return result;

                throw new InvalidDataException(string.Format(ErrorMessages.ElementMissing, "playlist", "XML root"));
            }
        }

        /// <summary>
        /// Gets element with specified name in <see cref="Playlist"/>.
        /// </summary>
        /// <param name="name">The local name of the elmeent</param>
        /// <remarks>
        /// If <see cref="isStrict"/> is set to true, the exact full name is required.
        /// Otherwise, alternate namespace or empty namespace is accepted if exact full name is not found.
        /// </remarks>
        /// <returns>The found element or null if not found</returns>
        /// <exception cref="InvalidDataException">
        /// <see cref="Playlist"/> is not found in XML.
        /// </exception>
        private XElement GetElementInPlaylist(string name)
        {
            XElement result = Playlist.Element(XName.Get(name, NS));
            if (result != null)
                return result;

            if (!isStrict)
            {
                result = Playlist.Element(XName.Get(name, NS1));
                if (result != null)
                    return result;

                result = Playlist.Element(XName.Get(name, string.Empty));
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Sets element with specified name in <see cref="Playlist"/>
        /// </summary>
        /// <param name="name">The local name of the elmeent</param>
        /// <param name="value">The value of the element. Set to null to remove element.</param>
        /// <remarks>
        /// The standard namespace <see cref="NS"/> will be used and elements with alternate namespace <see cref="NS1"/>
        /// or empty namespace will be removed.
        /// </remarks>
        /// <exception cref="InvalidDataException">
        /// <see cref="Playlist"/> is not found in XML.
        /// </exception>
        private void SetElementValueInPlaylist(string name, object value)
        {
            Playlist.SetElementValue(XName.Get(name, NS), value);
            Playlist.SetElementValue(XName.Get(name, NS1), null);
            Playlist.SetElementValue(XName.Get(name, string.Empty), null);
        }

        /// <summary>
        /// Creates an empty XSPF playlist
        /// </summary>
        /// <param name="isStrict">Whether parsing is strict</param>
        public Xspf(bool isStrict)
        {
            this.isStrict = isStrict;
            xDoc = new XDocument();
            var playList = new XElement(XName.Get("playlist", NS));
            playList.Add(new XElement(XName.Get("trackList", NS)));
            playList.Add(new XAttribute("version", 1));
            xDoc.Add(playList);
        }

        /// <summary>
        /// Loads a XSPF playlist from an instance of <see cref="XDocument"/>
        /// </summary>
        /// <remarks>
        /// The created instance reference to <paramref name="xDoc"/> rather than copying it.
        /// </remarks>
        /// <param name="xDoc">The XML document of XSPF file</param>
        /// <param name="isStrict">Whether parsing is strict</param>
        public Xspf(XDocument xDoc, bool isStrict)
        {
            this.isStrict = isStrict;
            this.xDoc = xDoc;
        }

        /// <summary>
        /// Loads a XSPF playlist from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load from</param>
        /// <param name="isStrict">Whether XML parsing is strict</param>
        /// <returns>The <see cref="Xspf"/> instance loaded from the stream</returns>
        public static Xspf Load(Stream stream, bool isStrict)
        {
            return new Xspf(XDocument.Load(stream), isStrict);
        }

        /// <summary>
        /// Loads a XSPF playlist from a URI
        /// </summary>
        /// <param name="uri">The URI to load from</param>
        /// <param name="isStrict">Whether XML parsing is strict</param>
        /// <returns>The <see cref="Xspf"/> instance loaded from the stream</returns>
        public static Xspf Load(string uri, bool isStrict)
        {
            return new Xspf(XDocument.Load(uri), isStrict);
        }

        /// <summary>
        /// Save the playlist to a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save to</param>
        public void Save(Stream stream)
        { xDoc.Save(stream); }

        /// <summary>
        /// Save the playlist to a <see cref="System.Xml.XmlWriter"/>
        /// </summary>
        /// <param name="xmlWriter">The <see cref="System.Xml.XmlWriter"/> to save to</param>
        public void Save(System.Xml.XmlWriter xmlWriter)
        { xDoc.Save(xmlWriter); }

        /// <summary>
        /// Save the playlist to a <see cref="TextWriter"/>
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to save to</param>
        public void Save(TextWriter textWriter)
        { xDoc.Save(textWriter); }

        /// <summary>
        /// Save the playlist to a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save to</param>
        /// <param name="options">The <see cref="SaveOptions"/> instance containing options of XML writing</param>
        public void Save(Stream stream, SaveOptions options)
        { xDoc.Save(stream, options); }

        /// <summary>
        /// Save the playlist to a <see cref="TextWriter"/>
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to save to</param>
        /// <param name="options">The <see cref="SaveOptions"/> instance containing options of XML writing</param>
        public void Save(TextWriter textWriter, SaveOptions options)
        { xDoc.Save(textWriter, options); }

        /// <summary>
        /// A human-readable title for the playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found.
        /// </exception>
        public string Title
        {
            get
            {
                return GetElementInPlaylist("title")?.Value;
            }
            set
            {
                SetElementValueInPlaylist("title", value);
            }
        }

        /// <summary>
        /// Human-readable name of the entity that authored the playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found.
        /// </exception>
        public string Creator
        {
            get
            {
                return GetElementInPlaylist("creator")?.Value;
            }
            set
            {
                SetElementValueInPlaylist("creator", value);
            }
        }

        /// <summary>
        /// A human-readable comment on the playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found.
        /// </exception>
        public string Annotation
        {
            get
            {
                return GetElementInPlaylist("annotation")?.Value;
            }
            set
            {
                SetElementValueInPlaylist("annotation", value);
            }
        }

        /// <summary>
        /// URI of a web page to find out more about the playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or value of "info" element is not legal URI.
        /// </exception>
        public Uri Info
        {
            get
            {
                try { return new Uri(GetElementInPlaylist("info")?.Value); }
                catch (ArgumentNullException)
                { return null; }
                catch (InvalidDataException)
                { throw; }
                catch (UriFormatException e)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "info"), e); }
            }
            set
            {
                SetElementValueInPlaylist("info", value?.ToString());
            }
        }

        /// <summary>
        /// Source URI for this playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or value of "location" element is not legal URI.
        /// </exception>
        public Uri Location
        {
            get
            {
                try { return new Uri(GetElementInPlaylist("location")?.Value); }
                catch (ArgumentNullException)
                { return null; }
                catch (InvalidDataException)
                { throw; }
                catch (UriFormatException e)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "location"), e); }
            }
            set
            {
                SetElementValueInPlaylist("location", value?.ToString());
            }
        }

        /// <summary>
        /// Canonical ID for this playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or value of "identifier" element is not legal URI.
        /// </exception>
        public Uri Identifier
        {
            get
            {
                try { return new Uri(GetElementInPlaylist("identifier")?.Value); }
                catch (ArgumentNullException)
                { return null; }
                catch (InvalidDataException)
                { throw; }
                catch (UriFormatException e)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "identifier"), e); }
            }
            set
            {
                SetElementValueInPlaylist("identifier", value?.ToString());
            }
        }

        /// <summary>
        /// URI for an image to display in absence of a 
        /// //playback/trackList/image element. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or value of "image" element is not legal URI.
        /// </exception>
        public Uri Image
        {
            get
            {
                try { return new Uri(GetElementInPlaylist("image")?.Value); }
                catch (ArgumentNullException)
                { return null; }
                catch (InvalidDataException)
                { throw; }
                catch (UriFormatException e)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "image"), e); }
            }
            set
            {
                SetElementValueInPlaylist("image", value?.ToString());
            }
        }

        /// <summary>
        /// Creation date (time?) of the playlist. Null means the element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or value of "date" element is not legal date.
        /// </exception>
        public DateTime? Date
        {
            get
            {
                try { return DateTime.Parse(GetElementInPlaylist("date")?.Value); }
                catch (ArgumentNullException)
                { return null; }
                catch (InvalidDataException)
                { throw; }
                catch (FormatException e)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalDate, "date"), e); }
            }
            set
            {
                SetElementValueInPlaylist("date", value?.ToString("O"));
            }
        }

        /// <summary>
        /// URI of a resource that describes the license under which this
        /// playlist was released. Null means element is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or value of "license" element is not legal URI.
        /// </exception>
        public Uri License
        {
            get
            {
                try { return new Uri(GetElementInPlaylist("license")?.Value); }
                catch (ArgumentNullException)
                { return null; }
                catch (InvalidDataException)
                { throw; }
                catch (UriFormatException e)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "license"), e); }
            }
            set
            {
                SetElementValueInPlaylist("license", value?.ToString());
            }
        }

        /// <summary>
        /// Ordered list of XspfTrack elements to be rendered.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// Element "playlist" is required but not found,
        /// or element "trackList" can't be found.
        /// </exception>
        public XspfTrackList TrackList
        {
            get
            {
                try
                {
                    return new XspfTrackList(GetElementInPlaylist("trackList"), isStrict);
                }
                catch (ArgumentNullException)
                { throw new InvalidDataException(string.Format(ErrorMessages.ElementMissing, "trackList", "playlist")); }
            }
            set
            {
                GetElementInPlaylist("trackList")?.Remove();
                Playlist.Add(value.xEle);
            }
        }
    }
}
