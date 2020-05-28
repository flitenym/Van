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
            double? GetDivide(double? firstValue, double? secondValue)
            {
                return (firstValue ?? 0) / (secondValue == 0 || secondValue == null ? 1 : secondValue);
            }

            for (int i = 0; i < resultListData.Count(); i++)
            {
                resultListData[i].Standart = GetDivide(firstListData[i].Standart, secondListData[i].Standart);
                resultListData[i].Weibull = GetDivide(firstListData[i].Weibull, secondListData[i].Weibull);
                resultListData[i].Exponential = GetDivide(firstListData[i].Exponential, secondListData[i].Exponential);
                resultListData[i].Gompertz = GetDivide(firstListData[i].Gompertz, secondListData[i].Gompertz);
                resultListData[i].Relay = GetDivide(firstListData[i].Relay, secondListData[i].Relay);
                resultListData[i].LogLogistic = GetDivide(firstListData[i].LogLogistic, secondListData[i].LogLogistic);
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

                var logLogistic = new LineSeries
                {
                    Title = "Логлогистическое",
                    Values = new ChartValues<double>(firstListData.Select(x => Math.Round(x.LogLogistic ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                seriescCollection.Add(logLogistic);
            }));

            return seriescCollection;
        }
    }
}