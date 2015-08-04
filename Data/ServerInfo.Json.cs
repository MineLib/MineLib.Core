using System.Collections.Generic;

using Newtonsoft.Json;

namespace MineLib.Core.Data
{
    public struct Version
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("protocol")]
        public int Protocol;
    }

    public struct Sample
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("id")]
        public string ID;
    }

    public struct Players
    {
        [JsonProperty("max")]
        public int Max;

        [JsonProperty("online")]
        public int Online;

        [JsonProperty("sample")]
        public List<Sample> Sample;
    }

    public struct ModList
    {
        [JsonProperty("modid")]
        public string ModID;

        [JsonProperty("version")]
        public string Version;
    }

    public struct ModInfo
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("modList")]
        public List<ModList> ModList;
    }


    public struct ServerInfo
    {
        [JsonProperty("version")]
        public Version Version;

        [JsonProperty("players")]
        public Players Players;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("favicon")]
        public byte[] Favicon;

        [JsonProperty("modinfo")]
        public ModInfo? ModInfo;
    }

    public struct ResponseData
    {
        public ServerInfo Info;
        public long Ping;
    }


    public struct Address
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("port")]
        public ushort Port { get; set; }
    }

    public sealed class Server
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonIgnore]
        public ResponseData ServerResponse { get; set; }
    }
}
