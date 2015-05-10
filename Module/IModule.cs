using System;

namespace MineLib.Core.Module
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
    }
}
