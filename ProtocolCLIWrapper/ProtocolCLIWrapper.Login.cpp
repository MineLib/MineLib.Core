#include "stdafx.h"
#include "ProtocolCLIWrapper.h"

using namespace System;
using namespace MineLib::Network;

namespace ProtocolCLIWrapper {

bool Protocol::Login(String^ login, String^ password) {
	return false;
}
bool Protocol::Logout() {
	return false;
}
void Protocol::SendPacket(IPacket^ packet) {
	return;
}
void Protocol::Connect() {
	return;
}
void Protocol::Disconnect(){
	return;
}

}
