using System;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Helper.StaticInfo
{
    public class ViewInfo
    {
        public string Name { get; set; }
        public Guid View { get; set; }
        public int Num { get; set; }
        public bool IsActive { get; set; } = true;
        public ModelBaseClasses ModelClass { get; set; }
    }


    public static class Types
    {
        public static class ViewData
        {
            #region Настройки

            public static ViewInfo Settings = new ViewInfo { Name = "Настройки", View = new Guid("C475B3A2-B969-4B30-B08A-F669C8B6AFF9"), Num = 0, ModelClass = ModelBaseClasses.Settings};

            #endregion

            #region Главное меню

            public static ViewInfo MainMenu = new ViewInfo { Name = "Главное меню", View = new Guid("C7ECA40A-DACF-4524-B0F7-443860448FFA"), Num = 0, ModelClass = ModelBaseClasses.Main };

            #endregion

            #region Правое меню

            public static ViewInfo ConnectionDataBase = new ViewInfo { Name = "Подключение к БД", View = new Guid("2340A6C5-1468-4CF6-BF1D-5DDE403F8034"), Num = 1, ModelClass = ModelBaseClasses.RightMenu };

            public static ViewInfo DataBaseBrowsing = new ViewInfo { Name = "Локальные данные", View = new Guid("12011025-144B-4BE8-96C4-DAD33B8778FC"), Num = 2, ModelClass = ModelBaseClasses.RightMenu };

            public static ViewInfo Info = new ViewInfo { Name = "Просмотр доп. информации", View = new Guid("94E80C76-219B-4633-B35D-D8A39A5E80E0"), Num = 3, IsActive = false, ModelClass = ModelBaseClasses.RightMenu };

            #endregion

            #region Левое меню

            public static ViewInfo SurvivalAnalysisMethods = new ViewInfo { Name = "Методы анализа выживаемости", View = new Guid("9F25D681-33A1-46DD-91CB-E6B771D292F0"), Num = 0, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo Nonparametric = new ViewInfo { Name = "Непараметрические методы", View = new Guid("5E876693-A6A3-49A8-901C-4D2436766FB5"), Num = 1, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo Parametric = new ViewInfo { Name = "Параметрические методы", View = new Guid("2D40D7A3-817B-47E0-B7F0-AB8547586891"), Num = 2, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo SemiParametric = new ViewInfo { Name = "Полупараметрические методы", View = new Guid("02D7E135-792B-4475-9DFB-76C54C59DECC"), Num = 3, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo KaplanMeier = new ViewInfo { Name = "Метод Каплана-Мейера", View = new Guid("74E9731B-2C66-404C-9D75-11CFA7243EB9"), Num = 4, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo MortalityTable = new ViewInfo { Name = "Таблица смертности", View = new Guid("A7E9F714-7FB2-451C-AABD-86006C741B1E"), Num = 5, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo LiCarter = new ViewInfo { Name = "Метод Ли Картера", View = new Guid("C7A5DB71-240F-46F4-B193-6E840461EB6B"), Num = 6, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo Distribution = new ViewInfo { Name = "Распределения", View = new Guid("67528722-51C1-40AF-B6A5-124345CE267B"), Num = 7, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo KoksRisk = new ViewInfo { Name = "Модель пропорциональных рисков Кокса", View = new Guid("BF110F4E-BD5A-45D6-939D-61CAFA8F0896"), Num = 8, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo ExcludingMortality = new ViewInfo { Name = "Без учета причин смерти", View = new Guid("CF4D7660-96B0-4AC9-9287-E66C7BE3747A"), Num = 9, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo IncludingMortality = new ViewInfo { Name = "С учетом причин смерти", View = new Guid("65919B95-591D-4BE1-BBB4-E5CCB07A327D"), Num = 10, ModelClass = ModelBaseClasses.LeftMenu };

            public static ViewInfo Test = new ViewInfo { Name = "Методы", View = new Guid("842240AA-932A-471A-91D9-29F92C87CCDB"), Num = 11, ModelClass = ModelBaseClasses.LeftMenu };

            #endregion 

        } 
    }
}
