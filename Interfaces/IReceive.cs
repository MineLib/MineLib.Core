using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;

namespace MineLib.Core.Interfaces
{
    /// <summary>
    /// Interface for registering supported receive types.
    /// </summary>
    public interface IReceive { }

    public abstract class ReceiveArgs { }


    public struct OnChatMessage : IReceive
    {
        public string Message { get; private set; }

        public OnChatMessage(string message): this()
        {
            Message = message;
        }
    }

    #region Anvil

    public struct OnChunk : IReceive
    {
        public Chunk Chunk { get; private set; }

        public OnChunk(Chunk chunk) : this()
        {
            Chunk = chunk;
        }
    }

    public struct OnChunkList : IReceive
    {
        public ChunkList Chunks { get; private set; }

        public OnChunkList(ChunkList chunks) : this()
        {
            Chunks = chunks;
        }
    }

    public struct OnBlockChange : IReceive
    {
        public Position Location { get; private set; }
        public int Block { get; private set; }

        public OnBlockChange(Position location, int block) : this()
        {
            Location = location;
            Block = block;
        }
    }

    public struct OnMultiBlockChange : IReceive
    {
        public Coordinates2D ChunkLocation { get; private set; }
        public Record[] Records { get; private set; }

        public OnMultiBlockChange(Coordinates2D chunkLocation, Record[] records) : this()
        {
            ChunkLocation = chunkLocation;
            Records = records;
        }
    }

    public struct OnBlockAction : IReceive
    {
        public Position Location { get; private set; }
        public int Block { get; private set; }
        public object BlockAction { get; private set; }

        public OnBlockAction(Position location, int block, object blockAction) : this()
        {
            Location = location;
            Block = block;
            BlockAction = blockAction;
        }
    }

    public struct OnBlockBreakAction : IReceive
    {
        public int EntityID { get; private set; }
        public Position Location { get; private set; }
        public byte Stage { get; private set; }

        public OnBlockBreakAction(int entityID, Position location, byte stage) : this()
        {
            EntityID = entityID;
            Location = location;
            Stage = stage;
        }
    }

    #endregion

    public struct OnPlayerPosition : IReceive
    {
        public Vector3 Position { get; private set; }

        public OnPlayerPosition(Vector3 position): this()
        {
            Position = position;
        }
    }

    public struct OnPlayerLook : IReceive
    {
        public Vector3 Look { get; private set; }

        public OnPlayerLook(Vector3 look): this()
        {
            Look = look;
        }
    }

    public struct OnHeldItemChange : IReceive
    {
        public byte Slot { get; private set; }

        public OnHeldItemChange(byte slot): this()
        {
            Slot = slot;
        }
    }

    public struct OnSpawnPoint : IReceive
    {
        public Position Location { get; private set; }

        public OnSpawnPoint(Position location): this()
        {
            Location = location;
        }
    }

    public struct OnUpdateHealth : IReceive
    {
        public float Health { get; private set; }
        public int Food { get; set; }
        public float FoodSaturation { get; set; }

        public OnUpdateHealth(float health, int food, float foodSaturation): this()
        {
            Health = health;
            Food = food;
            FoodSaturation = foodSaturation;
        }
    }

    public struct OnRespawn : IReceive
    {
        public object GameInfo { get; private set; }

        public OnRespawn(object gameInfo): this()
        {
            GameInfo = gameInfo;
        }
    }

    public struct OnAction : IReceive
    {
        public int EntityID { get; private set; }
        public int Action { get; private set; }

        public OnAction(int entityID, int action): this()
        {
            EntityID = entityID;
            Action = action;
        }
    }
    
    public struct OnSetExperience : IReceive
    {
        public float ExperienceBar { get; private set; }
        public int Level { get; private set; }
        public int TotalExperience { get; private set; }

        public OnSetExperience(float experienceBar, int level, int totalExperience): this()
        {
            ExperienceBar = experienceBar;
            Level = level;
            TotalExperience = totalExperience;
        }
    }

    public struct OnTimeUpdate : IReceive
    {
        public long WorldAge { get; private set; }
        public long TimeOfDay { get; private set; }

        public OnTimeUpdate(long worldAge, long timeOfDay) : this()
        {
            WorldAge = worldAge;
            TimeOfDay = timeOfDay;
        }
    }  
}
