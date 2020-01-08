using System;

namespace Van.Helper.StaticInfo
{
    public static class Types
    {
        public static class ViewData
        {
            #region Настройки

            public static string SettingsName = "Настройки";
            public static readonly Guid SettingsView = new Guid("C475B3A2-B969-4B30-B08A-F669C8B6AFF9"); 
            public static int SettingsNum = 0;

            #endregion

            #region Правое меню

            public static string MainMenuName = "Главное меню";
            public static readonly Guid MainMenuView = new Guid("C7ECA40A-DACF-4524-B0F7-443860448FFA");
            public static int MainMenuNum = 0;

            public static string ConnectionDataBaseName = "Подключение к БД";
            public static readonly Guid ConnectionDataBaseView = new Guid("2340A6C5-1468-4CF6-BF1D-5DDE403F8034");
            public static int ConnectionDataBaseNum = 1;

            public static string DataBaseBrowsingName = "Локальные данные";
            public static readonly Guid DataBaseBrowsingView = new Guid("12011025-144B-4BE8-96C4-DAD33B8778FC");
            public static int DataBaseBrowsingNum = 2;
            #endregion

            #region Левое меню

            public static string SurvivalAnalysisMethodsName = "Методы анализа выживаемости";
            public static readonly Guid SurvivalAnalysisMethodsView = new Guid("9F25D681-33A1-46DD-91CB-E6B771D292F0");
            public static int SurvivalAnalysisMethodsNum = 0;

            public static string NonparametricName = "Непараметрические методы";
            public static readonly Guid NonparametricView = new Guid("5E876693-A6A3-49A8-901C-4D2436766FB5");
            public static int NonparametricNum = 1;

            public static string ParametricName = "Параметрические методы";
            public static readonly Guid ParametricView = new Guid("2D40D7A3-817B-47E0-B7F0-AB8547586891");
            public static int ParametricNum = 2;

            public static string SemiParametricName = "Полупараметрические методы";
            public static readonly Guid SemiParametricView = new Guid("02D7E135-792B-4475-9DFB-76C54C59DECC");
            public static int SemiParametricNum = 3;

            public static string KaplanMeierName = "Метод Каплана-Мейера";
            public static readonly Guid KaplanMeierView = new Guid("74E9731B-2C66-404C-9D75-11CFA7243EB9");
            public static int KaplanMeierNum = 4;

            public static string MortalityTableName = "Таблица смертности";
            public static readonly Guid MortalityTableView = new Guid("A7E9F714-7FB2-451C-AABD-86006C741B1E");
            public static int MortalityTableNum = 5;

            public static string LiCarterName = "Метод Ли Картера";
            public static readonly Guid LiCarterView = new Guid("C7A5DB71-240F-46F4-B193-6E840461EB6B");
            public static int LiCarterNum = 6;

            public static string DistributionName = "Распределения";
            public static readonly Guid DistributionView = new Guid("67528722-51C1-40AF-B6A5-124345CE267B");
            public static int DistributionNum = 7;

            public static string KoksRiskName = "Модель пропорциональных рисков Кокса";
            public static readonly Guid KoksRiskView = new Guid("BF110F4E-BD5A-45D6-939D-61CAFA8F0896");
            public static int KoksRiskNum = 8;

            public static string ExcludingMortalityName = "Без учета причин смерти";
            public static readonly Guid ExcludingMortalityView = new Guid("CF4D7660-96B0-4AC9-9287-E66C7BE3747A");
            public static int ExcludingMortalityNum = 9;

            public static string IncludingMortalityName = "С учетом причин смерти";
            public static readonly Guid IncludingMortalityView = new Guid("65919B95-591D-4BE1-BBB4-E5CCB07A327D");
            public static int IncludingMortalityNum = 10;

            public static string TestName = "Методы";
            public static readonly Guid TestView = new Guid("842240AA-932A-471A-91D9-29F92C87CCDB");
            public static int TestNum = 11;

            #endregion 

        } 
    }
}
