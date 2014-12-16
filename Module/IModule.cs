using System;

namespace MineLib.Network.Module
{
    public interface IModule
    {
        String Name { get; }
        String Version { get; }
    }
}
