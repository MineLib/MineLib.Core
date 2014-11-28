// MineLib.Network.CLI.Wrapper.h

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace MineLib::Network;
using namespace MineLib::Network::Data::Structs;

namespace ProtocolCLIWrapper {

public ref class Protocol : public MineLib::Network::IProtocol
{
private:
	String^ name;
	String^ version;

	ConnectionState state;
	bool connected;

	List<IPacket ^> ^packetsReceived;
	List<IPacket ^> ^packetsSended;
	List<IPacket ^> ^lastPackets;


public:
	virtual property String^ Name;
	virtual property String^ Version;

	virtual property ConnectionState State;
	virtual property bool Connected;

	virtual IProtocol^ Create(IMinecraftClient^ client, bool debugPackets);

	virtual void SendPacket(IPacket^ packet);

	virtual void Connect();
	virtual void Disconnect();


	virtual property List<IPacket ^>^ PacketsReceived;
	virtual property List<IPacket ^>^ PacketsSended;
	virtual property List<IPacket ^>^ LastPackets;
	virtual property IPacket^ LastPacket;
	virtual property bool SavePackets;
	

	virtual IAsyncResult^ BeginSendPacketHandled(IPacket^ packet, AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginSendPacket(IPacket^ packet, AsyncCallback^ asyncCallback, Object^ state);
    virtual void EndSendPacket(IAsyncResult^ asyncResult);

	virtual IAsyncResult^ BeginConnect(String^ ip, unsigned short port, AsyncCallback^ asyncCallback, Object^ state);
    virtual void EndConnect(IAsyncResult^ asyncResult);

    virtual IAsyncResult^ BeginDisconnect(AsyncCallback^ asyncCallback, Object^ state);
    virtual void EndDisconnect(IAsyncResult^ asyncResult);


	virtual IAsyncResult^ BeginConnectToServer(AsyncCallback^ asyncCallback, Object^ state);
	virtual IAsyncResult^ BeginKeepAlive(int value, AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginSendClientInfo(AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginRespawn(AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginPlayerMoved(PlaverMovedData data, AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginSendMessage(String^ message, AsyncCallback^ asyncCallback, Object^ state);
    virtual IAsyncResult^ BeginPlayerHeldItem(short slot, AsyncCallback^ asyncCallback, Object^ state);

	virtual bool Login(String^ login, String^ password);
	virtual bool Logout();

	virtual event EventHandler^ ChatMessage;

	~Protocol();
   };
}
