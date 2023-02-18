using SharpConfig;

namespace rLibrary.Interfaces
{
    public interface IDataBaseProvider
    {
        IDatabaseService currentDataBaseService { get; }
        void LoadConfiguration(Configuration rLibraryConfig);
    }
}
