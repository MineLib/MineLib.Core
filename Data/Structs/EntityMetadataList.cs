using System;
using System.Collections.Generic;
using System.Text;

using Aragas.Core.Data;

using MineLib.Core.Data.EntityMetadata;

namespace MineLib.Core.Data.Structs
{
    /// <summary>
    /// Used to send metadata with entities
    /// </summary>
    public class EntityMetadataList : IEquatable<EntityMetadataList>
    {
        internal readonly Dictionary<byte, EntityMetadataEntry> _entries;

        public EntityMetadataList(int length = 0)
        {
            _entries = new Dictionary<byte, EntityMetadataEntry>(length);
        }

        public int Count => _entries.Count;

        public EntityMetadataEntry this[byte index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }

        internal delegate EntityMetadataEntry CreateEntryInstance();

        internal static readonly CreateEntryInstance[] EntryTypes =
        {
            () => new EntityMetadataByte(),           // 0
            () => new EntityMetadataShort(),          // 1
            () => new EntityMetadataInt(),            // 2
            () => new EntityMetadataFloat(),          // 3
            () => new EntityMetadataString(),         // 4
            () => new EntityMetadataSlot(),           // 5
            () => new EntityMetadataVector(),         // 6
            () => new EntityMetadataRotation()        // 7
        };

        /// <summary>
        /// Converts this EntityMetadata to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = null;

            foreach (var entry in _entries.Values)
            {
                if (sb != null)
                    sb.Append(", ");
                else
                    sb = new StringBuilder();

                sb.Append(entry);
            }

            if (sb != null)
                return sb.ToString();

            return string.Empty;
        }

        public bool Equals(EntityMetadataList other)
        {
            if (!Count.Equals(other.Count))
                return false;

            for (byte i = 0; i < Count; i++)
                if (!this[i].Equals(other[i])) return false;
            
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((EntityMetadataList) obj);
        }

        public override int GetHashCode()
        {
            return _entries.GetHashCode() ^ Count.GetHashCode();
        }
    }
}
