namespace Adaptive.ReactiveTrader.Client.Domain.Instrumentation
{
    public interface ITextFileWriter
    {
        void WriteFile(string filename, string content);
    }
}