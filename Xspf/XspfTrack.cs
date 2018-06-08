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
        private readonly bool isStrict;

        internal readonly XElement xEle;

        private XElement GetElement(string name)
        {
            XElement result = xEle.Element(XName.Get(name, Xspf.NS));
            if (result != null)
                return result;

            if (!isStrict)
            {
                result = xEle.Element(XName.Get(name, Xspf.NS1));
                if (result != null)
                    return result;

                result = xEle.Element(XName.Get(name, string.Empty));
                if (result != null)
                    return result;
            }

            return null;
        }

        private void SetElementValue(string name, object value)
        {
            xEle.SetElementValue(XName.Get(name, Xspf.NS), value);
            xEle.SetElementValue(XName.Get(name, Xspf.NS1), null);
            xEle.SetElementValue(XName.Get(name, string.Empty), null);
        }

        /// <summary>
        /// Creates an empty instance of <see cref="XspfTrack"/>
        /// </summary>
        /// <param name="isStrict">Whether parsing is strict</param>
        public XspfTrack(bool isStrict)
        {
            this.isStrict = isStrict;
            xEle = new XElement("track");
        }

        /// <summary>
        /// Creates an instance of <see cref="XspfTrack"/> with its XML element
        /// </summary>
        /// <param name="xEle">The XML element of the track in the playlist</param>
        /// <param name="isStrict">Whether parsing is strict</param>
        public XspfTrack(XElement xEle, bool isStrict)
        {
            this.isStrict = isStrict;
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
    }
}