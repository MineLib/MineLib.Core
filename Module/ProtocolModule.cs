using System.Collections.Generic;

namespace MineLib.Core.Module
{
    public class ProtocolModule
    {
        public string FilePath { get; private set; }

        public string FileName
        {
            get
            {
                var splits = new List<string>(FilePath.Split('\\'));

                return splits[splits.Count - 1];
            }
        }

        public ProtocolModule(string path)
        {
            FilePath = path;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
