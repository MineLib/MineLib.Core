namespace ProtocolMinetest.Packets
{
    enum PacketsServer
    {
        TOCLIENT_INIT = 0x10,
        /*
            Server's reply to TOSERVER_INIT.
            Sent second after connected.
            [0] u16 TOSERVER_INIT
            [2] u8 deployed version
            [3] v3s16 player's position + v3f(0,BS/2,0) floatToInt'd
            [12] u64 map seed (new as of 2011-02-27)
            [20] f1000 recommended send interval (in seconds) (new as of 14)
            NOTE: The position in here is deprecated; position is
                  explicitly sent afterwards
        */

        TOCLIENT_BLOCKDATA = 0x20, //TODO: Multiple blocks
        TOCLIENT_ADDNODE = 0x21,
        /*
            u16 command
            v3s16 position
            serialized mapnode
            u8 keep_metadata // Added in protocol version 22
        */
        TOCLIENT_REMOVENODE = 0x22,

        TOCLIENT_INVENTORY = 0x27,
        /*
            [0] u16 command
            [2] serialized inventory
        */

        TOCLIENT_TIME_OF_DAY = 0x29,
        /*
            u16 command
            u16 time (0-23999)
            Added in a later version:
            f1000 time_speed
        */

        TOCLIENT_CHAT_MESSAGE = 0x30,
        /*
            u16 command
            u16 length
            wstring message
        */

        TOCLIENT_ACTIVE_OBJECT_REMOVE_ADD = 0x31,
        /*
            u16 command
            u16 count of removed objects
            for all removed objects {
                u16 id
            }
            u16 count of added objects
            for all added objects {
                u16 id
                u8 type
                u32 initialization data length
                string initialization data
            }
        */

        TOCLIENT_ACTIVE_OBJECT_MESSAGES = 0x32,
        /*
            u16 command
            for all objects
            {
                u16 id
                u16 message length
                string message
            }
        */

        TOCLIENT_HP = 0x33,
        /*
            u16 command
            u8 hp
        */

        TOCLIENT_MOVE_PLAYER = 0x34,
        /*
            u16 command
            v3f1000 player position
            f1000 player pitch
            f1000 player yaw
        */

        TOCLIENT_ACCESS_DENIED = 0x35,
        /*
            u16 command
            u16 reason_length
            wstring reason
        */

        TOCLIENT_DEATHSCREEN = 0x37,
        /*
            u16 command
            u8 bool set camera point target
            v3f1000 camera point target (to point the death cause or whatever)
        */

        TOCLIENT_MEDIA = 0x38,
        /*
            u16 command
            u16 total number of texture bunches
            u16 index of this bunch
            u32 number of files in this bunch
            for each file {
                u16 length of name
                string name
                u32 length of data
                data
            }
            u16 length of remote media server url (if applicable)
            string url
        */

        TOCLIENT_TOOLDEF = 0x39,
        /*
            u16 command
            u32 length of the next item
            serialized ToolDefManager
        */

        TOCLIENT_NODEDEF = 0x3a,
        /*
            u16 command
            u32 length of the next item
            serialized NodeDefManager
        */

        TOCLIENT_CRAFTITEMDEF = 0x3b,
        /*
            u16 command
            u32 length of the next item
            serialized CraftiItemDefManager
        */

        TOCLIENT_ANNOUNCE_MEDIA = 0x3c,

        /*
            u16 command
            u32 number of files
            for each texture {
                u16 length of name
                string name
                u16 length of sha1_digest
                string sha1_digest
            }
        */

        TOCLIENT_ITEMDEF = 0x3d,
        /*
            u16 command
            u32 length of next item
            serialized ItemDefManager
        */

        TOCLIENT_PLAY_SOUND = 0x3f,
        /*
            u16 command
            s32 sound_id
            u16 len
            u8[len] sound name
            s32 gain*1000
            u8 type (0=local, 1=positional, 2=object)
            s32[3] pos_nodes*10000
            u16 object_id
            u8 loop (bool)
        */

        TOCLIENT_STOP_SOUND = 0x40,
        /*
            u16 command
            s32 sound_id
        */

        TOCLIENT_PRIVILEGES = 0x41,
        /*
            u16 command
            u16 number of privileges
            for each privilege
                u16 len
                u8[len] privilege
        */

        TOCLIENT_INVENTORY_FORMSPEC = 0x42,
        /*
            u16 command
            u32 len
            u8[len] formspec
        */

        TOCLIENT_DETACHED_INVENTORY = 0x43,
        /*
            [0] u16 command
            u16 len
            u8[len] name
            [2] serialized inventory
        */

        TOCLIENT_SHOW_FORMSPEC = 0x44,
        /*
            [0] u16 command
            u32 len
            u8[len] formspec
            u16 len
            u8[len] formname
        */

        TOCLIENT_MOVEMENT = 0x45,
        /*
            u16 command
            f1000 movement_acceleration_default
            f1000 movement_acceleration_air
            f1000 movement_acceleration_fast
            f1000 movement_speed_walk
            f1000 movement_speed_crouch
            f1000 movement_speed_fast
            f1000 movement_speed_climb
            f1000 movement_speed_jump
            f1000 movement_liquid_fluidity
            f1000 movement_liquid_fluidity_smooth
            f1000 movement_liquid_sink
            f1000 movement_gravity
        */

        TOCLIENT_SPAWN_PARTICLE = 0x46,
        /*
            u16 command
            v3f1000 pos
            v3f1000 velocity
            v3f1000 acceleration
            f1000 expirationtime
            f1000 size
            u8 bool collisiondetection
            u8 bool vertical
            u32 len
            u8[len] texture
        */

        TOCLIENT_ADD_PARTICLESPAWNER = 0x47,
        /*
            u16 command
            u16 amount
            f1000 spawntime
            v3f1000 minpos
            v3f1000 maxpos
            v3f1000 minvel
            v3f1000 maxvel
            v3f1000 minacc
            v3f1000 maxacc
            f1000 minexptime
            f1000 maxexptime
            f1000 minsize
            f1000 maxsize
            u8 bool collisiondetection
            u8 bool vertical
            u32 len
            u8[len] texture
            u32 id
        */

        TOCLIENT_DELETE_PARTICLESPAWNER = 0x48,
        /*
            u16 command
            u32 id
        */

        TOCLIENT_HUDADD = 0x49,
        /*
            u16 command
            u32 id
            u8 type
            v2f1000 pos
            u32 len
            u8[len] name
            v2f1000 scale
            u32 len2
            u8[len2] text
            u32 number
            u32 item
            u32 dir
            v2f1000 align
            v2f1000 offset
            v3f1000 world_pos
            v2s32 size
        */

        TOCLIENT_HUDRM = 0x4a,
        /*
            u16 command
            u32 id
        */

        TOCLIENT_HUDCHANGE = 0x4b,
        /*
            u16 command
            u32 id
            u8 stat
            [v2f1000 data |
             u32 len
             u8[len] data |
             u32 data]
        */

        TOCLIENT_HUD_SET_FLAGS = 0x4c,
        /*
            u16 command
            u32 flags
            u32 mask
        */

        TOCLIENT_HUD_SET_PARAM = 0x4d,
        /*
            u16 command
            u16 param
            u16 len
            u8[len] value
        */

        TOCLIENT_BREATH = 0x4e,
        /*
            u16 command
            u16 breath
        */

        TOCLIENT_SET_SKY = 0x4f,
        /*
            u16 command
            u8[4] color (ARGB)
            u8 len
            u8[len] type
            u16 count
            foreach count:
                u8 len
                u8[len] param
        */

        TOCLIENT_OVERRIDE_DAY_NIGHT_RATIO = 0x50,
        /*
            u16 command
            u8 do_override (boolean)
            u16 day-night ratio 0...65535
        */

        TOCLIENT_LOCAL_PLAYER_ANIMATIONS = 0x51,
        /*
            u16 command
            v2s32 stand/idle
            v2s32 walk
            v2s32 dig
            v2s32 walk+dig
            f1000 frame_speed
        */

        TOCLIENT_EYE_OFFSET = 0x52,
        /*
            u16 command
            v3f1000 first
            v3f1000 third
        */
    };
}
