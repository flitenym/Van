﻿using System;
using System.Windows;
using static Van.Core.Helper.Enums;

namespace Van.Core.AbstractClasses
{
    abstract class ThemeBase
    {
        private ResourceDictionary resource;

        public abstract string Title { get; }
        public abstract string Name { get; }
        public abstract int Num { get; }
        public abstract ThemeBaseClasses ThemeClass { get; }
        public abstract string UriPath { get; }

        public ResourceDictionary ResourceDictionary
        {
            get
            {
                if (resource == null)
                {
                    resource = CreateView();
                }
                return resource;
            }
        }

        public void Deactivate()
        {
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.MergedDictionaries.Remove(ResourceDictionary);
        }

        public void SelectTheme()
        {
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);
        }

        public ResourceDictionary CreateView()
        {
            return Application.LoadComponent(new Uri(UriPath, UriKind.Relative)) as ResourceDictionary;
        }
    }
}