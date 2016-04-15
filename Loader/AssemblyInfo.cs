namespace MineLib.Core.Loader
{
    public class AssemblyInfo
    {
        public string FileName
        {
            get
            {
                var splits = FilePath.Split('\\');

                return splits[splits.Length - 1];
            }
        }
        public string FilePath { get; }


        public AssemblyInfo(string path) { FilePath = path; }


        public override string ToString() => FileName;
    }
}
