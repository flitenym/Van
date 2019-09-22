using System.Windows;
using static Van.Helper.Enums;

namespace Van.Interfaces
{
    public interface ITheme
    {
        string Title { get; }
        string Name { get; }
        string UriPath { get; }
        int Num { get; } 
        ThemeBaseClasses ThemeClass { get; }
        ResourceDictionary ResourceDictionary { get; }
        void SelectTheme();
        void Deactivate();
        ResourceDictionary CreateView();
    }
}
