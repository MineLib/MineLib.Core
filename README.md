**Mono Latest:** | **Windows .NET 4.5:**
------------ | -------------
[![Build Status](https://travis-ci.org/MineLib/MineLib.Core.svg)](https://travis-ci.org/MineLib/MineLib.Core) | [![Build status](https://ci.appveyor.com/api/projects/status/myur4dwflth7oybm?svg=true)](https://ci.appveyor.com/project/Aragas/minelib-core)

MineLib.Network.Modular
===============

Library for handling network connection with any voxel-based server.

My implementation of How-it-should-be. Now with modules.

Not all server and clients packets for Modern 1.8 are fully supported yet (Reading or sending could be a bit wrong). Few have some problems. Compression and encryption is fully supported. 

Interface based. All methods and event (for Minecraft Modern, Classic, etc.) are same. Still have some problems to unite dat stuff, but this is possible.

Supported:
* Minecraft Modern
* Minecraft Classic (without Online mode yet)

TODO:
* Minecraft PocketEdition
* Minetest (lol)

You can also create your own protocol implementation on C# or C++/CLI. Yea, would be cool.

Right now you cant do anything with it, because i haven't done yet this so called "united methods & event stuff", so, use MineLib.Network. Yea, life is pain.

Documentation will come soon (nope).
See https://github.com/Aragas/MineLib.ClientWrapper for "How use dat shiet".

Used repos:
* https://github.com/umby24/libMC.NET
* https://github.com/Conji/the-syhno-project
* https://github.com/pollyzoid/mclib
* https://github.com/pdelvo/Pdelvo.Minecraft
* https://github.com/SirCmpwn/Craft.Net
* https://github.com/GlowstoneMC/Glowstone
