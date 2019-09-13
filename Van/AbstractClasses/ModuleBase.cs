using System;
using System.Windows.Controls;
using Van.Interfaces;

namespace Van.AbstractClasses
{
    abstract class ModuleBase : IModule
    {
        private UserControl view;

        protected abstract UserControl CreateViewAndViewModel();

        public abstract string Name { get; }

        public abstract int Num { get; }

        public UserControl UserInterface
        {
            get
            {
                if (view == null)
                {
                    view = CreateViewAndViewModel();
                }
                return view;
            }
        }

        public abstract int IdType { get; }

        public void Deactivate()
        {
            if (view != null)
            {
                var d = view.DataContext as IDisposable;
                if (d != null) d.Dispose();
                view = null;
            }
        }

    }
}
