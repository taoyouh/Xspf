using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Zhaobang.Xspf
{
    /// <summary>
    /// Represents a list of tracks in a XSPF playlist
    /// </summary>
    public class XspfTrackList : IList<XspfTrack>
    {
        private readonly bool isStrict;

        internal readonly XElement xEle;

        private IEnumerable<XElement> Elements()
        {
            if (isStrict)
            {
                return xEle.Elements(XName.Get("track", Xspf.NS));
            }
            else
            {
                return xEle.Elements().Where(e =>
                    e.Name.LocalName == "track"
                    && (e.Name.NamespaceName == Xspf.NS
                        || e.Name.NamespaceName == Xspf.NS1
                        || e.Name.NamespaceName == string.Empty)
                    );
            }
        }

        /// <summary>
        /// Creates an empty instance of <see cref="XspfTrackList"/>
        /// </summary>
        /// <param name="isStrict">Whether XML parsing is strict</param>
        public XspfTrackList(bool isStrict)
        {
            this.isStrict = isStrict;
            xEle = new XElement("trackList");
        }

        /// <summary>
        /// Creates a new instance of <see cref="XspfTrackList"/> with its XML element.
        /// </summary>
        /// <param name="xEle">The XML element of the track list</param>
        /// <param name="isStrict">Whether XML parsing is strict</param>
        /// <exception cref="ArgumentNullException">
        /// The XML element is null.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The XML element is not named "trackList".
        /// </exception>
        public XspfTrackList(XElement xEle, bool isStrict)
        {
            this.isStrict = isStrict;
            this.xEle = xEle ?? throw new ArgumentNullException(nameof(XspfTrackList.xEle));
            if (this.xEle.Name.LocalName != "trackList")
                throw new InvalidDataException(string.Format(ErrorMessages.WrongElementName, "trackList", this.xEle.Name.LocalName));
            if (this.isStrict)
                if (this.xEle.Name.NamespaceName != Xspf.NS)
                    throw new InvalidDataException(
                        string.Format(ErrorMessages.WrongElementName, XName.Get("trackList", Xspf.NS), this.xEle.Name));
        }

        /// <summary>
        /// Gets the count of objects.
        /// </summary>
        public int Count
        {
            get
            {
                return Elements().Count();
            }
        }

        /// <summary>
        /// Gets whether the list is readonly. (Always returns false.)
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the value at given index.
        /// </summary>
        /// <param name="index">The index to find value at</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/>is less than 0 or equal to or greater than size of list.
        /// </exception>
        /// <returns>The item at <paramref name="index"/></returns>
        public XspfTrack this[int index]
        {
            get
            {
                try
                {
                    return new XspfTrack(Elements().ElementAt(index), isStrict);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            set
            {
                if (index == 0)
                {
                    try { Elements().FirstOrDefault().Remove(); }
                    catch (NullReferenceException)
                    { throw new ArgumentOutOfRangeException("index"); }
                    xEle.AddFirst(value.xEle);
                }
                else
                {
                    XElement prev = Elements().ElementAt(index - 1);
                    try { Elements().ElementAt(index).Remove(); }
                    catch (ArgumentOutOfRangeException) { throw new ArgumentOutOfRangeException("index"); }
                    prev.AddAfterSelf(value.xEle);
                }
            }
        }

        /// <summary>
        /// Gets the index of given item.
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <returns>The index in the list of <paramref name="item"/> or -1 if not found</returns>
        public int IndexOf(XspfTrack item)
        {
            int index = 0;
            foreach (XElement toSearch in Elements())
            {
                if (toSearch == item.xEle)
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Inserts an item at given index.
        /// </summary>
        /// <param name="index">The index at which the new item is to be inserted</param>
        /// <param name="item">The new item to insert</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0 or greater than the size of the list.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> is null.
        /// </exception>
        public void Insert(int index, XspfTrack item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (index == 0)
            {
                xEle.AddFirst(item.xEle);
            }
            else
            {
                try
                {
                    XElement prev = Elements().ElementAt(index - 1);
                    prev.AddAfterSelf(item.xEle);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        /// <summary>
        /// Removes the item at given index.
        /// </summary>
        /// <param name="index">The index at which the item is to be removed</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0 or equal to or greater than size of list.
        /// </exception>
        public void RemoveAt(int index)
        {
            try
            {
                Elements().ElementAt(index).Remove();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The item to add to the list</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> is null.
        /// </exception>
        public void Add(XspfTrack item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            xEle.Add(item.xEle);
        }

        /// <summary>
        /// Clear the list.
        /// </summary>
        public void Clear()
        {
            Elements().Remove();
        }

        /// <summary>
        /// Check whether the list contains the item.
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>true if the item is in the list, otherwise false</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> is null.
        /// </exception>
        public bool Contains(XspfTrack item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            return xEle.Elements().Contains(item.xEle);
        }

        /// <summary>
        /// Copy the list to an array.
        /// </summary>
        /// <param name="array">The target array</param>
        /// <param name="arrayIndex">The start index of the target array to copy to</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// There is not enough space in <paramref name="array"/>.
        /// The items that can fit into the <paramref name="array"/> are already copied when the exception throws.
        /// </exception>
        public void CopyTo(XspfTrack[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            try
            {
                foreach (XElement ele in Elements())
                {
                    array[arrayIndex] = new XspfTrack(ele, isStrict);
                    arrayIndex++;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException(ErrorMessages.ArrayDoesNotFit);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the list.
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if the item is removed, otherwise false</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> is null.
        /// </exception>
        public bool Remove(XspfTrack item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            XElement toDel = xEle.Elements().FirstOrDefault(x => x == item.xEle);
            if (toDel == null)
                return false;
            toDel.Remove();
            return true;
        }

        /// <summary>
        /// Gets an enumerator of the list.
        /// </summary>
        /// <returns>An enumerator of the list</returns>
        public IEnumerator<XspfTrack> GetEnumerator()
        {
            return Elements().Select(x => new XspfTrack(x, isStrict)).GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator of the list.
        /// </summary>
        /// <returns>An enumerator of the list</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
