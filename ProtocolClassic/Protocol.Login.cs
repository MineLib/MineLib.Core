using System;
using MineLib.Network;

namespace ProtocolClassic
{
    public partial class Protocol : IProtocol
    {
        public bool UseLogin { get; private set; }
        public bool Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }
    }
}
