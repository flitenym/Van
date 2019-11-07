using System;
using System.Windows.Controls;
using static Van.Helper.Enums;

namespace Van.AbstractClasses
{
    public abstract class ModuleBase
    {
        private UserControl view;

        protected abstract UserControl CreateViewAndViewModel();

        public abstract string Name { get; }

        public abstract int Num { get; }

        public abstract bool IsActive { get; }

        public abstract Guid ID { get; }

        public abstract Guid? ParentID { get; }

        public abstract ModelBaseClasses modelClass { get; }

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

        public void Deactivate()
        {
            if (view != null)
            {
                var d = view as IDisposable;
                if (d != null) d.Dispose(); 
                view.DataContext = null;
                view = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}
