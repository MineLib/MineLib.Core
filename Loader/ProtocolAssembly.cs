using System.Collections.Generic;

namespace MineLib.Core.Loader
{
    public class ProtocolAssembly
    {
        public string FilePath { get; }

        public string FileName
        {
            get
            {
                var splits = new List<string>(FilePath.Split('\\'));

                return splits[splits.Count - 1];
            }
        }

        public ProtocolAssembly(string path) { FilePath = path; }

        public override string ToString() => FileName;
    }
}
