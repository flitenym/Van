using System.Windows;

namespace Van.Interfaces
{
    public interface ITheme
    {
        string Name { get; }
        string UriPath { get; }
        int Num { get; }
        ResourceDictionary ResourceDictionary { get; }
        void SelectTheme();
        void Deactivate();
        ResourceDictionary CreateView();
    }
}
