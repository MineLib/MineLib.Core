namespace ProtocolMinetest.Packets
{
    enum PacketsClient
    {
        TOSERVER_INIT = 0x10,
        /*
            Sent first after connected.
            [0] u16 TOSERVER_INIT
            [2] u8 SER_FMT_VER_HIGHEST_READ
            [3] u8[20] player_name
            [23] u8[28] password (new in some version)
            [51] u16 minimum supported network protocol version (added sometime)
            [53] u16 maximum supported network protocol version (added later than the previous one)
        */

        TOSERVER_INIT2 = 0x11,
        /*
            Sent as an ACK for TOCLIENT_INIT.
            After this, the server can send data.
            [0] u16 TOSERVER_INIT2
        */

        TOSERVER_PLAYERPOS = 0x23,
        /*
            [0] u16 command
            [2] v3s32 position*100
            [2+12] v3s32 speed*100
            [2+12+12] s32 pitch*100
            [2+12+12+4] s32 yaw*100
            [2+12+12+4+4] u32 keyPressed
        */

        TOSERVER_GOTBLOCKS = 0x24,
        /*
            [0] u16 command
            [2] u8 count
            [3] v3s16 pos_0
            [3+6] v3s16 pos_1
            ...
        */

        TOSERVER_DELETEDBLOCKS = 0x25,
        /*
            [0] u16 command
            [2] u8 count
            [3] v3s16 pos_0
            [3+6] v3s16 pos_1
            ...
        */

        TOSERVER_INVENTORY_ACTION = 0x31,
        /*
            See InventoryAction in inventory.h
        */

        TOSERVER_CHAT_MESSAGE = 0x32,
        /*
            u16 command
            u16 length
            wstring message
        */

        TOSERVER_DAMAGE = 0x35,
        /*
            u16 command
            u8 amount
        */

        TOSERVER_PASSWORD = 0x36,
        /*
            Sent to change password.
            [0] u16 TOSERVER_PASSWORD
            [2] u8[28] old password
            [30] u8[28] new password
        */

        TOSERVER_PLAYERITEM = 0x37,
        /*
            Sent to change selected item.
            [0] u16 TOSERVER_PLAYERITEM
            [2] u16 item
        */

        TOSERVER_RESPAWN = 0x38,
        /*
            u16 TOSERVER_RESPAWN
        */

        TOSERVER_INTERACT = 0x39,
        /*
            [0] u16 command
            [2] u8 action
            [3] u16 item
            [5] u32 length of the next item
            [9] serialized PointedThing
            actions:
            0: start digging (from undersurface) or use
            1: stop digging (all parameters ignored)
            2: digging completed
            3: place block or item (to abovesurface)
            4: use item
            (Obsoletes TOSERVER_GROUND_ACTION and TOSERVER_CLICK_ACTIVEOBJECT.)
        */

        TOSERVER_REMOVED_SOUNDS = 0x3a,
        /*
            u16 command
            u16 len
            s32[len] sound_id
        */

        TOSERVER_NODEMETA_FIELDS = 0x3b,
        /*
            u16 command
            v3s16 p
            u16 len
            u8[len] forield:
                u16 len
                u8[len] field name
                u32 len
                u8[len] field value
        */

        TOSERVER_REQUEST_MEDIA = 0x40,
        /*
            u16 command
            u16 number of files requested
            for each file {
                u16 length of name
                string name
            }m name (reserved for future use)
            u16 number of fields
            for each field:
                u16 len
                u8[len] field name
                u32 len
                u8[len] field value
        */

        TOSERVER_INVENTORY_FIELDS = 0x3c,
        /*
            u16 command
            u16 len
            u8[len] form name (reserved for future use)
            u16 number of fields
            for each f
         */

        TOSERVER_RECEIVED_MEDIA = 0x41,
        /*
            u16 command
        */

        TOSERVER_BREATH = 0x42,
        /*
            u16 command
            u16 breath
        */

        TOSERVER_CLIENT_READY = 0x43,
        /*
            u8 major
            u8 minor
            u8 patch
            u8 reserved
            u16 len
            u8[len] full_version_string
        */
    };

}
