using SharedLibrary.Helper.Classes;
using System;
using static SharedLibrary.Helper.StaticInfo.Enums;

namespace Van.Helper.StaticInfo
{
    public static class Types
    {
        public static class ViewData
        {
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
