using System;
using System.Collections.Generic;

namespace MineLib.Core.Data.Anvil
{
    public abstract class World : IDisposable
    {
        List<Chunk> Chunks { get; set; }
        TimeSpan TimeOfDay { get; set; }

        public abstract void Dispose();
    }
}