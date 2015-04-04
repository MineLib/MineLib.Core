#include "stdafx.h"
#include "ProtocolCLIWrapper.h"

using namespace System;
using namespace MineLib::Network;

namespace ProtocolCLIWrapper {

IProtocol^ Protocol::Initialize(IMinecraftClient^ client, bool debugPackets) { return this;}

IAsyncResult^ Protocol::BeginSendPacketHandled(IPacket^ packet, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
IAsyncResult^ Protocol::BeginSendPacket(IPacket^ packet, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
void Protocol::EndSendPacket(IAsyncResult^ asyncResult){}

IAsyncResult^ Protocol::BeginConnect(String^ ip, unsigned short port, AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
void Protocol::EndConnect(IAsyncResult^ asyncResult){}

IAsyncResult^ Protocol::BeginDisconnect(AsyncCallback^ asyncCallback, Object^ state){ return gcnew ProtocolAsyncResult();}
void Protocol::EndDisconnect(IAsyncResult^ asyncResult){}

void Protocol::RegisterAsyncSending(Type^ asyncSendingType, Func<IAsyncSendingParameters^, IAsyncResult^>^ method) {}
IAsyncResult^ Protocol::DoAsyncSending(Type^ asyncSendingType, IAsyncSendingParameters^ parameters){ return gcnew ProtocolAsyncResult();}

Protocol::~Protocol() {}
}
