using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Van.Helper.StaticInfo;
using Van.LocalDataBase.ModelsHelper;

namespace Van.Methods.Helper
{
    public static class CreateGraphics
    {
        public static Func<double, string> GetYFormatter() => value => Math.Round(value, 3).ToString();

        /// <summary>
        /// Получения разделения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="firstListData">Числитель</param>
        /// <param name="secondListData">Знаменатель</param>
        /// <returns></returns>
        public static List<T> GetDivideList<T, Y>(List<T> firstListData, List<Y> secondListData, List<T> resultListData) where T : IMethod where Y : IMethod
        {
            for (int i = 0; i < resultListData.Count(); i++)
            {
                resultListData[i].Standart = 0; //(firstListData[i].Standart ?? 1) / (secondListData[i].Standart == 0 || secondListData[i].Standart == null ? 1 : secondListData[i].Standart);
                resultListData[i].Weibull = (firstListData[i].Weibull ?? 1) / (secondListData[i].Weibull == 0 || secondListData[i].Weibull == null ? 1 : secondListData[i].Weibull);
                resultListData[i].Exponential = (firstListData[i].Exponential ?? 1) / (secondListData[i].Exponential == 0 || secondListData[i].Exponential == null ? 1 : secondListData[i].Exponential);
                resultListData[i].Gompertz = (firstListData[i].Gompertz ?? 1) / (secondListData[i].Gompertz == 0 || secondListData[i].Gompertz == null ? 1 : secondListData[i].Gompertz);
                resultListData[i].Relay = (firstListData[i].Relay ?? 1) / (secondListData[i].Relay == 0 || secondListData[i].Relay == null ? 1 : secondListData[i].Relay);
            }

            return resultListData;
        }

        /// <summary>
        /// Отрисовка графика
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seriescCollection"></param>
        /// <param name="firstListData"></param>
        /// <returns></returns>
        public static async Task<SeriesCollection> GetSeriesCollection<T>(SeriesCollection seriescCollection, List<T> firstListData) where T : IMethod
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                seriescCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(firstListData.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                seriescCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(firstListData.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                seriescCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(firstListData.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                seriescCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(firstListData.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                seriescCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(firstListData.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                seriescCollection.Add(relay);
            }));

            return seriescCollection;
        }
    }
}