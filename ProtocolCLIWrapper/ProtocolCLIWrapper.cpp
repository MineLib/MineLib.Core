#include "ProtocolCLIWrapper.h"

using namespace System;
using namespace MineLib::Network;

namespace ProtocolCLIWrapper {

IProtocol^ Protocol::Create(IMinecraftClient^ client, bool debugPackets) { return this;}

void Protocol::SendPacket(IPacket^ packet) {}

void Protocol::Connect() {}
void Protocol::Disconnect(){}


IAsyncResult^ Protocol::BeginSendPacketHandled(IPacket^ packet, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginSendPacket(IPacket^ packet, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
void Protocol::EndSendPacket(IAsyncResult^ asyncResult){}

IAsyncResult^ Protocol::BeginConnect(String^ ip, short port, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
void Protocol::EndConnect(IAsyncResult^ asyncResult){}

IAsyncResult^ Protocol::BeginDisconnect(AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
void Protocol::EndDisconnect(IAsyncResult^ asyncResult){}


IAsyncResult^ Protocol::BeginConnectToServer(AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginKeepAlive(int value, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginSendClientInfo(AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginRespawn(AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginPlayerMoved(PlaverMovedData data, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginSendMessage(String^ message, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginPlayerHeldItem(short slot, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}

Protocol::~Protocol() {}
}
