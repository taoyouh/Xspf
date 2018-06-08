using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Zhaobang.Xspf
{
    /// <summary>
    /// Represents a track in a XSPF playlist
    /// </summary>
    public class XspfTrack
    {
        internal readonly XElement xEle;

        private XElement GetElement(string name)
        {
            return xEle.Element(XName.Get(name, Xspf.NS));
        }

        private void SetElementValue(string name, object value)
        {
            xEle.SetElementValue(XName.Get(name, Xspf.NS), value);
        }

        /// <summary>
        /// Creates an empty instance of <see cref="XspfTrack"/>
        /// </summary>
        public XspfTrack()
        {
            xEle = new XElement(XName.Get("track", Xspf.NS));
        }

        /// <summary>
        /// Creates an instance of <see cref="XspfTrack"/> with its XML element
        /// </summary>
        /// <param name="xEle">The XML element of the track in the playlist</param>
        public XspfTrack(XElement xEle) : this(xEle, true)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="XspfTrack"/> with its XML element
        /// </summary>
        /// <param name="xEle">The XML element of the track in the playlist</param>
        /// <param name="copy">Whether to copy the XML element</param>
        internal XspfTrack(XElement xEle, bool copy)
        {
            if (xEle == null)
                throw new ArgumentNullException(nameof(xEle));
            if (xEle.Name != XName.Get("track", Xspf.NS))
                throw new InvalidDataException(
                    string.Format(ErrorMessages.WrongElementName, XName.Get("track", Xspf.NS), this.xEle.Name));

            if (copy)
                this.xEle = new XElement(xEle);
            else
                this.xEle = xEle;
        }

        /// <summary>
        /// Gets or sets the first URI to be rendered. Null means it is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// The value of the first location element is not legal URI.
        /// </exception>
        public Uri Location
        {
            get
            {
                try { return new Uri(GetElement("location")?.Value); }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (UriFormatException e)
                {
                    throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "location"), e);
                }
            }
            set
            {
                SetElementValue("location", value);
            }
        }

        /// <summary>
        /// Gets or sets the first canonical ID for this resource. Null means it is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// The value of the first identifier element is not legal URI.
        /// </exception>
        public Uri Identifier
        {
            get
            {
                try { return new Uri(GetElement("identifier")?.Value); }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (UriFormatException e)
                {
                    throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "identifier"), e);
                }
            }
            set
            {
                SetElementValue("identifier", value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the track. Null means it is not present.
        /// </summary>
        public string Title
        {
            get { return GetElement("title")?.Value; }
            set { SetElementValue("title", value); }
        }

        /// <summary>
        /// Gets or sets the URI of image. Null means it is not present.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// The value the element is not legal URI.
        /// </exception>
        public Uri Image
        {
            get
            {
                try { return new Uri(GetElement("image")?.Value); }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (UriFormatException e)
                {
                    throw new InvalidDataException(string.Format(ErrorMessages.ElementNotLegalUri, "image"), e);
                }
            }
            set
            {
                SetElementValue("image", value);
            }
        }

        public static bool operator ==(XspfTrack a, XspfTrack b)
        {
            if ((object)a == null)
            {
                if ((object)b == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((object)b == null)
                {
                    return false;
                }
                else
                {
                    return a.xEle == b.xEle;
                }
            }
        }

        public static bool operator !=(XspfTrack a, XspfTrack b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is XspfTrack track)
            {
                return xEle == track.xEle;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return xEle.GetHashCode();
        }
    }
}