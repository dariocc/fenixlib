namespace FpgFromFolder
{
    public class ConversionEventArgs
    {
        public ConversionEventArgs(string path)
        {
            Path = path;
        }
        public string Path { get; }
    }
}
