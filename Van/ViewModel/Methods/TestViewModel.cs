﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Van.Helper;
using System;
using Van.Methods;
using System.Threading.Tasks;
using LiveCharts;
using Van.DataBase.Models;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Media;
using LiveCharts.Wpf;
using System.Windows;
using Dapper;
using System.Threading;
using Van.LocalDataBase;
using Van.Helper.HelperClasses;
using System.Collections.ObjectModel;
using Van.Helper.StaticInfo;
using Van.Commands;
using Van.AbstractClasses;

namespace Van.ViewModel.Methods
{
    class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public TestViewModel()
        {
            Task.Factory.StartNew(async () =>
                await LoadTables()
            );
        }

        public List<double> ageValues = new List<double>();

        public List<double> standartValues = new List<double>();

        public List<int> t = new List<int>();

        public List<int> delta = new List<int>();

        double r => delta.Where(x => x == 1).Count();

        public Random random = new Random();

        public bool HaveNullProbability = true;

        public double epsilon = 0.01;

        public List<MortalityTable> currentMortalityTables = new List<MortalityTable>();

        public List<SurvivalFunction> currentSurvivalFunctions = new List<SurvivalFunction>();

        public List<Density> currentDensitys = new List<Density>();

        public List<ResidualSurvivalFunction> currentResidualSurvivalFunctions = new List<ResidualSurvivalFunction>();

        public List<ResidualDensity> currentResidualDensitys = new List<ResidualDensity>();

        public List<LifeTimes> currentLifeTimes = new List<LifeTimes>();

        /// <summary>
        /// Акаики
        /// </summary>
        public QualityAssessmentOfModels Acaici = new QualityAssessmentOfModels();

        /// <summary>
        /// Расстояние от табличного первый метод
        /// </summary>
        public QualityAssessmentOfModels DistanceFirstMethod = new QualityAssessmentOfModels();

        /// <summary>
        /// Расстояние от табличного второй метод
        /// </summary>
        public QualityAssessmentOfModels DistanceSecondMethod = new QualityAssessmentOfModels();

        public async Task LoadTables()
        {  
            await SelectMortalityAsync();
            await SelectSurvivalFunctionAsync();
            await SelectDensity();
            await SelectLifeTimesFunction();
            await SelectQualityAssessmentOfModelsAsync();

            await SelectResidualSurvivalFunctionAsync();
            await SelectResidualDensity();

            await RefreshChartsDivides();
            await RefreshChartsResidualDivides();

            
        }

        #region Диапазон

        private ObservableCollection<RangeData> rangeDataList = new ObservableCollection<RangeData>();

        public ObservableCollection<RangeData> RangeDataList
        {
            get { return rangeDataList; }
            set
            {
                rangeDataList = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(RangeDataList)));
            }
        }

        private RangeData firstAgeX = new RangeData();

        public RangeData FirstAgeX
        {
            get { return firstAgeX; }
            set
            {
                firstAgeX = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstAgeX)));
            }
        }

        private RangeData secondAgeX = new RangeData();

        public RangeData SecondAgeX
        {
            get { return secondAgeX; }
            set
            {
                secondAgeX = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SecondAgeX)));
            }
        }

        #endregion

        #region Таблица оценка качесва моделей

        private DataTable qualityAssessmentOfModelsTableData;

        public DataTable QualityAssessmentOfModelsTableData
        {
            get { return qualityAssessmentOfModelsTableData; }
            set
            {
                if (value == null) return;
                qualityAssessmentOfModelsTableData?.Clear();
                qualityAssessmentOfModelsTableData?.Columns.Clear();
                qualityAssessmentOfModelsTableData?.Rows.Clear();

                qualityAssessmentOfModelsTableData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(QualityAssessmentOfModelsTableData)));
            }
        }

        private async Task SelectQualityAssessmentOfModelsAsync()
        {
            QualityAssessmentOfModelsTableData = await SQLExecutor.SelectExecutorAsync(typeof(QualityAssessmentOfModels), nameof(QualityAssessmentOfModels));
            QualityAssessmentOfModelsTableData.AcceptChanges(); 
        }

        #endregion

        #region Таблица смертности

        private DataTable mortalityTableData;

        public DataTable MortalityTableData
        {
            get { return mortalityTableData; }
            set
            {
                if (value == null) return;
                mortalityTableData?.Clear();
                mortalityTableData?.Columns.Clear();
                mortalityTableData?.Rows.Clear();

                mortalityTableData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MortalityTableData)));
            }
        }

        private async Task SelectMortalityAsync(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                MortalityTableData = await SQLExecutor.SelectExecutorAsync(typeof(MortalityTable), nameof(MortalityTable));
                MortalityTableData.AcceptChanges();
            }

            currentMortalityTables = await SQLExecutor.SelectExecutorAsync<MortalityTable>(nameof(MortalityTable));

            NValue = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max().Value;
            if (currentMortalityTables.Where(x => x.Probability == null).Any())
                HaveNullProbability = true;
            else HaveNullProbability = false;


            ageValues = new List<double>();

            var temp = new ObservableCollection<RangeData>(); 

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                ageValues.Add(getTValue(currentMortalityTables[i]?.AgeX));

                temp.Add(new RangeData() { ID = (currentMortalityTables[i]?.ID).Value, AgeX = (int)getTValue(currentMortalityTables[i]?.AgeX) });
            }

            RangeDataList = temp;

            FirstAgeX = RangeDataList.First();
            SecondAgeX = RangeDataList.Last();
        }

        #endregion

        #region Таблица s(t)

        private DataTable survivalFunctionTable;

        public DataTable SurvivalFunctionTable
        {
            get { return survivalFunctionTable; }
            set
            {
                if (value == null) return;
                survivalFunctionTable?.Clear();
                survivalFunctionTable?.Columns.Clear();
                survivalFunctionTable?.Rows.Clear();

                survivalFunctionTable = value; 

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionTable)));
            }
        }

        private async Task SelectSurvivalFunctionAsync(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                SurvivalFunctionTable = await SQLExecutor.SelectExecutorAsync(typeof(SurvivalFunction), nameof(SurvivalFunction));
                SurvivalFunctionTable.AcceptChanges();
            }

            currentSurvivalFunctions = await SQLExecutor.SelectExecutorAsync<SurvivalFunction>(nameof(SurvivalFunction));

            if (!currentSurvivalFunctions.Where(x => x.Standart == null).Any())
            {
                await RefreshChartsSurvivalFunctions();
            }

            standartValues.Clear();
            standartValues = currentSurvivalFunctions.Select(x => x.Standart ?? 0).ToList();
        }

        #endregion

        #region Таблица f(t)

        private DataTable densityTable;

        public DataTable DensityTable
        {
            get { return densityTable; }
            set
            {
                if (value == null) return;
                densityTable?.Clear();
                densityTable?.Columns.Clear();
                densityTable?.Rows.Clear();

                densityTable = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DensityTable)));
            }
        }

        private async Task SelectDensity(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                DensityTable = await SQLExecutor.SelectExecutorAsync(typeof(Density), nameof(Density));
                DensityTable.AcceptChanges();
            }

            currentDensitys = await SQLExecutor.SelectExecutorAsync<Density>(nameof(Density));

            if (!currentDensitys.Where(x => x.Standart == null).Any())
            {
                await RefreshChartsDensitys();
            }
        }

        #endregion

        #region Таблица s(t) для остаточного времени жизни

        private DataTable residualSurvivalFunctionTable;

        public DataTable ResidualSurvivalFunctionTable
        {
            get { return residualSurvivalFunctionTable; }
            set
            {
                if (value == null) return;
                residualSurvivalFunctionTable?.Clear();
                residualSurvivalFunctionTable?.Columns.Clear();
                residualSurvivalFunctionTable?.Rows.Clear();

                residualSurvivalFunctionTable = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualSurvivalFunctionTable)));
            }
        }

        private async Task SelectResidualSurvivalFunctionAsync(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                ResidualSurvivalFunctionTable = await SQLExecutor.SelectExecutorAsync(typeof(ResidualSurvivalFunction), nameof(ResidualSurvivalFunction));
                ResidualSurvivalFunctionTable.AcceptChanges();
            }

            currentResidualSurvivalFunctions = await SQLExecutor.SelectExecutorAsync<ResidualSurvivalFunction>(nameof(ResidualSurvivalFunction));

            if (!currentResidualSurvivalFunctions.Where(x => x.Standart == null).Any())
            {
                await RefreshChartsResidualSurvivalFunctions();
            }
        }

        #endregion

        #region Таблица f(t) для остаточного времени жизни

        private DataTable residualDensityTable;

        public DataTable ResidualDensityTable
        {
            get { return residualDensityTable; }
            set
            {
                if (value == null) return;
                residualDensityTable?.Clear();
                residualDensityTable?.Columns.Clear();
                residualDensityTable?.Rows.Clear();

                residualDensityTable = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualDensityTable)));
            }
        }

        private async Task SelectResidualDensity(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                ResidualDensityTable = await SQLExecutor.SelectExecutorAsync(typeof(ResidualDensity), nameof(ResidualDensity));
                ResidualDensityTable.AcceptChanges();
            }

            currentResidualDensitys = await SQLExecutor.SelectExecutorAsync<ResidualDensity>(nameof(ResidualDensity));

            if (!currentResidualDensitys.Where(x => x.Standart == null).Any())
            {
                await RefreshChartsResidualDensitys();
            }
        }

        #endregion

        #region Таблица t

        private DataTable lifeTimesTable;

        public DataTable LifeTimesTable
        {
            get { return lifeTimesTable; }
            set
            {
                if (value == null) return;
                lifeTimesTable?.Clear();
                lifeTimesTable?.Columns.Clear();
                lifeTimesTable?.Rows.Clear();
                lifeTimesTable = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LifeTimesTable)));
            }
        }

        private async Task SelectLifeTimesFunction(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                LifeTimesTable = await SQLExecutor.SelectExecutorAsync(typeof(LifeTimes), nameof(LifeTimes));
                LifeTimesTable.AcceptChanges();
            }

            currentLifeTimes = await SQLExecutor.SelectExecutorAsync<LifeTimes>(nameof(LifeTimes));

            t = currentLifeTimes.Select(x => x.LifeTime.Value).ToList();
            delta = currentLifeTimes.Select(x => x.Censor.Value).ToList();
        }

        #endregion

        #region n значение

        private int nValue;

        public int NValue
        {
            get { return nValue; }
            set
            {
                nValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NValue)));
            }
        }

        #endregion

        #region Комманда для вычисления T

        private AsyncCommand calculateTCommand;

        public AsyncCommand CalculateTCommand => calculateTCommand ?? (calculateTCommand = new AsyncCommand(x => CalculateT(), y => CanCalculateT()));

        private bool CanCalculateT()
        {
            return !HaveNullProbability;
        }

        public async Task CalculateT()
        {
            t = new List<int>(); 

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await GenerateT();
            
            await RewriteLifeTimesTable();
            
            await SelectLifeTimesFunction();

            await HelperMethods.Message($"t вычислено. Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();
        }

        public async Task GenerateT()
        {
            await Task.Run(
                () =>
                {
                    var minimalProb = currentMortalityTables.Select(x => x.Probability).Min().Value;
                    var maxsurv = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max().Value;
                    minimalProb /= 2;

                    for (int i = 0; i < NValue; i++)
                    {
                        int randomNumber = random.Next(0, maxsurv + 1);
                        double z = (double)randomNumber / maxsurv;
                        z -= minimalProb;

                        double sumProbubility = 0;

                        for (int j = 0; j < currentMortalityTables.Count(); j++)
                        {
                            if (z < currentMortalityTables[j].Probability)
                            {
                                t.Add((int)getTValue(currentMortalityTables[j].AgeX));
                                break;
                            }
                            else if (j != 0 && z >= sumProbubility && z < sumProbubility + currentMortalityTables[j].Probability.Value)
                            {
                                t.Add((int)getTValue(currentMortalityTables[j].AgeX));
                                break;
                            }
                            sumProbubility += currentMortalityTables[j].Probability.Value;
                        }
                    }
                });
        }

        public async Task RewriteLifeTimesTable()
        {
            currentLifeTimes = new List<LifeTimes>();
            delta = new List<int>();

            for (int i = 0; i < t.Count(); i++)
            {
                currentLifeTimes.Add(new LifeTimes() { LifeTime = t[i], Censor = 1 });
                delta.Add(1);
            }
            await Task.Run(async () =>
            {
                await DeleteLifeTimes();
            });

            await Task.Run(async () =>
            {
                await InsertLifeTimes();
            });
        }

        public async Task DeleteLifeTimes() {
            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                await cn.OpenAsync();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"DELETE FROM LifeTimes";
                        await cmd.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
                cn.Close();
            }
        }

        public async Task InsertLifeTimes()
        {
            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                await cn.OpenAsync();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO LifeTimes(LifeTime, Censor) VALUES(@LifeTime, @Censor);";
                        for (int i = 0; i < currentLifeTimes.Count(); i++)
                        {
                            cmd.Parameters.AddWithValue("@LifeTime", currentLifeTimes[i].LifeTime);
                            cmd.Parameters.AddWithValue("@Censor", currentLifeTimes[i].Censor);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    transaction.Commit();
                }
                cn.Close();
            }
        }

        /// <summary>
        /// В AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
        /// </summary>
        public double getTValue(string ageX) {
            // в AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
            if (double.TryParse(string.Join("", ageX.Where(c => char.IsDigit(c))), out double value))
            { 
                return value;
            }
            return 0;
        }

        #endregion 

        #region Комманда для вычисления числа умерших d(x)    

        private AsyncCommand calculateDataCommand;

        public AsyncCommand CalculateDataCommand => calculateDataCommand ?? (calculateDataCommand = new AsyncCommand(x => CalculateMortalityAsync()));

        public async Task CalculateMortalityAsync()
        {
            //обновим таблицу через БД
            await SelectMortalityAsync(false);

            if (currentMortalityTables.Count() < 2)
            {
                await SelectMortalityAsync();
                await HelperMethods.Message("Слишком мало данных");
                return;
            }

            await CalculateNumberOfDead();

            await CalculateProbability();

            //Обновим в БД данные исходя из текущего списка
            await UpdateMortality(); 
            //обновим таблицу через БД
            await SelectMortalityAsync();

            await HelperMethods.Message("Вычисление прошло успешно");
        }

        private async Task CalculateNumberOfDead() {
            await Task.Run(
                () =>
                {
                    int mortalityTableCount = currentMortalityTables.Count() - 1;

                    for (int i = 0; i < mortalityTableCount; i++)
                    {
                        currentMortalityTables[i].NumberOfDead = currentMortalityTables[i].NumberOfSurvivors - currentMortalityTables[i + 1].NumberOfSurvivors;
                    }
                    currentMortalityTables[mortalityTableCount].NumberOfDead = currentMortalityTables[mortalityTableCount].NumberOfSurvivors;
                });
        }

        private async Task CalculateProbability()
        {
            await Task.Run(
                () =>
                {
                    for (int i = 0; i < currentMortalityTables.Count(); i++)
                    {
                        currentMortalityTables[i].Probability = (double?)currentMortalityTables[i].NumberOfDead / (double)currentMortalityTables.First().NumberOfSurvivors;
                    }
                });
        }

        #endregion

        #region Комманда для обновления таблицы (используется после вычислений)

        private AsyncCommand updateMortalityCommand;

        public AsyncCommand UpdateMortalityCommand => updateMortalityCommand ?? (updateMortalityCommand = new AsyncCommand(x => UpdateMortalityTable()));

        public async Task UpdateMortalityTable()
        {
            //обновим таблицу через БД
            await SelectMortalityAsync();

            await HelperMethods.Message("Таблица обновлена");
        }

        private async Task UpdateMortality()
        {
            for (int i = 0; i < currentMortalityTables.Count(); i++)
            {
                await SQLExecutor.UpdateExecutorAsync(currentMortalityTables[i], currentMortalityTables[i], currentMortalityTables[i].ID);
            }
        }

        #endregion

        #region Комманда для методов

        private AsyncCommand calculateMethodsCommand;

        public AsyncCommand CalculateMethodsCommand => calculateMethodsCommand ?? (calculateMethodsCommand = new AsyncCommand(x => CalculateMethodsAsync(), y => CanCalculateMethods()));

        private bool CanCalculateMethods()
        {
            return currentSurvivalFunctions.Any() && currentSurvivalFunctions.Count == currentMortalityTables.Count;
        }

        private async Task UpdateSTAsync()
        {
            for (int i = 0; i < currentSurvivalFunctions.Count(); i++) 
            {
                await SQLExecutor.UpdateExecutorAsync(currentSurvivalFunctions[i], currentSurvivalFunctions[i], currentSurvivalFunctions[i].ID);
            }
        }

        private async Task UpdateD()
        {
            for (int i = 0; i < currentDensitys.Count(); i++)
            {
                await SQLExecutor.UpdateExecutorAsync(currentDensitys[i], currentDensitys[i], currentDensitys[i].ID);
            }
        }
        
        public async Task CalculateMethodsAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
             
            Acaici = new QualityAssessmentOfModels() { Quality = "Акаики"};
             
            DistanceFirstMethod = new QualityAssessmentOfModels() { Quality = "Первая метрика" };
            DistanceSecondMethod = new QualityAssessmentOfModels() { Quality = "Вторая метрика" };

            CalculateSTStandart();


            var taskWeibull = new Task(CalculateSTWeibull);
            var taskRelay = new Task(CalculateSTRelay);
            var taskGompertz = new Task(CalculateSTGompertz);
            var taskExponential = new Task(CalculateSTExponential);

            taskWeibull.Start();
            taskRelay.Start();
            taskGompertz.Start();
            taskExponential.Start();

            await Task.WhenAll(
                taskWeibull,
                taskRelay,
                taskGompertz,
                taskExponential);

            await UpdateAndSelectST();
            await UpdateAndSelectD();

            await RefreshChartsDivides();

            //--------Обновление оценки качества
            await QualityUpdateAsync();


            await HelperMethods.Message($"Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();
        }

        public async Task UpdateAndSelectST() {
            await UpdateSTAsync();
            await SelectSurvivalFunctionAsync();
        }

        public async Task UpdateAndSelectD()
        {
            await UpdateD();
            await SelectDensity();
        }

        public void CalculateSTStandart() {
            var maxNumberOfSurvivors = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max();

            if (maxNumberOfSurvivors == null) return;

            maxNumberOfSurvivors = maxNumberOfSurvivors.Value;

            for (int i = 0; i < currentMortalityTables.Count; i++) {
                currentSurvivalFunctions[i].Standart = Math.Round(
                    (double)currentMortalityTables[i]?.NumberOfSurvivors / 
                    (double)maxNumberOfSurvivors
                    , SettingsDictionary.round);

                currentDensitys[i].Standart = Math.Round(
                    (double)currentMortalityTables[i]?.NumberOfDead /
                    (double)maxNumberOfSurvivors
                    , SettingsDictionary.round);
            }

            standartValues.Clear();
            standartValues = currentSurvivalFunctions.Select(x => x.Standart ?? 0).ToList();
        }

        public void CalculateSTWeibull()
        {
            Weibull weibull = new Weibull(standartValues, ageValues, t, delta, SettingsDictionary.round, r, (double)int.MaxValue, epsilon, FirstAgeX, SecondAgeX);

            Acaici.Weibull = weibull.Quality;
            DistanceFirstMethod.Weibull = weibull.DistanceFirstMethod;
            DistanceSecondMethod.Weibull = weibull.DistanceSecondMethod;


            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Weibull = weibull.SurvivalFunctions[i];
                currentDensitys[i].Weibull = weibull.Densitys[i];
            }
        }

        public void CalculateSTRelay()
        {
            Relay relay = new Relay(standartValues, ageValues, t, delta, SettingsDictionary.round, r, FirstAgeX, SecondAgeX);

            Acaici.Relay = relay.Quality;
            DistanceFirstMethod.Relay = relay.DistanceFirstMethod;
            DistanceSecondMethod.Relay = relay.DistanceSecondMethod;

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Relay = relay.SurvivalFunctions[i];
                currentDensitys[i].Relay = relay.Densitys[i];
            }
        }

        public void CalculateSTGompertz()
        {
            Gompertz gompertz = new Gompertz(standartValues, ageValues, t, delta, SettingsDictionary.round, r, (double)int.MaxValue, epsilon, FirstAgeX, SecondAgeX);

            Acaici.Gompertz = gompertz.Quality;
            DistanceFirstMethod.Gompertz = gompertz.DistanceFirstMethod;
            DistanceSecondMethod.Gompertz = gompertz.DistanceSecondMethod;

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Gompertz = gompertz.SurvivalFunctions[i];
                currentDensitys[i].Gompertz = gompertz.Densitys[i];
            }
        }

        public void CalculateSTExponential()
        {
            Exponential exponential = new Exponential(standartValues, ageValues, t, delta, SettingsDictionary.round, r, FirstAgeX, SecondAgeX);

            Acaici.Exponential = exponential.Quality;
            DistanceFirstMethod.Exponential = exponential.DistanceFirstMethod;
            DistanceSecondMethod.Exponential = exponential.DistanceSecondMethod;

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Exponential = exponential.SurvivalFunctions[i];
                currentDensitys[i].Exponential = exponential.Densitys[i];
            }
        }

        public async Task QualityUpdateAsync()
        {
            //Удалим значение из таблицы
            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                slc.Execute($"DELETE from {nameof(QualityAssessmentOfModels)}");
            }

            await SQLExecutor.InsertExecutorAsync(Acaici, Acaici);
            await SQLExecutor.InsertExecutorAsync(DistanceFirstMethod, DistanceFirstMethod);
            await SQLExecutor.InsertExecutorAsync(DistanceFirstMethod, DistanceSecondMethod);

            await SelectQualityAssessmentOfModelsAsync();
        }

        #endregion

        #region Комманда для вычисления остаточных значений

        private AsyncCommand calculateResidualCommand;

        public AsyncCommand CalculateResidualCommand => calculateResidualCommand ?? (calculateResidualCommand = new AsyncCommand(x => CalculateResidual(), y => CanCalculateResidual()));

        public async Task CalculateResidual()
        {
            var firstSF = currentSurvivalFunctions.Where(x => (int)getTValue(x.MortalityTableAgeX) >= FirstAgeX.AgeX && (int)getTValue(x.MortalityTableAgeX) <= SecondAgeX.AgeX).ToList();
            var survival = currentSurvivalFunctions.First(x => (int)getTValue(x.MortalityTableAgeX) == FirstAgeX.AgeX);

            currentResidualSurvivalFunctions.Clear();

            for (int i = 0; i < firstSF.Count(); i++)
            {
                currentResidualSurvivalFunctions.Add(new ResidualSurvivalFunction()
                {
                    MortalityTableID = firstSF[i].MortalityTableID,
                    MortalityTableAgeX = firstSF[i].MortalityTableAgeX,
                    Standart = firstSF[i].Standart / survival.Standart,
                    Weibull = firstSF[i].Weibull / survival.Weibull,
                    Relay = firstSF[i].Relay / survival.Relay,
                    Gompertz = firstSF[i].Gompertz / survival.Gompertz,
                    Exponential = firstSF[i].Exponential / survival.Exponential
                });
            }

            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                await cn.OpenAsync();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"DELETE FROM ResidualSurvivalFunction";
                        await cmd.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
                cn.Close();
            }

            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                await cn.OpenAsync();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $@"INSERT INTO {nameof(ResidualSurvivalFunction)}(MortalityTableID, MortalityTableAgeX, Standart, Weibull, Relay, Gompertz, Exponential) VALUES (@MortalityTableID, @MortalityTableAgeX, @Standart, @Weibull, @Relay, @Gompertz, @Exponential);  select last_insert_rowid()";
                        for (int i = 0; i < currentResidualSurvivalFunctions.Count(); i++)
                        {
                            cmd.Parameters.AddWithValue("@MortalityTableID", currentResidualSurvivalFunctions[i].MortalityTableID); 
                            cmd.Parameters.AddWithValue("@MortalityTableAgeX", currentResidualSurvivalFunctions[i].MortalityTableAgeX);
                            cmd.Parameters.AddWithValue("@Standart", currentResidualSurvivalFunctions[i].Standart);
                            cmd.Parameters.AddWithValue("@Weibull", currentResidualSurvivalFunctions[i].Weibull);
                            cmd.Parameters.AddWithValue("@Relay", currentResidualSurvivalFunctions[i].Relay);
                            cmd.Parameters.AddWithValue("@Gompertz", currentResidualSurvivalFunctions[i].Gompertz);
                            cmd.Parameters.AddWithValue("@Exponential", currentResidualSurvivalFunctions[i].Exponential);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    transaction.Commit();
                }
                cn.Close();
            }

            await SelectResidualSurvivalFunctionAsync();


            var firstD = currentDensitys.Where(x => (int)getTValue(x.MortalityTableAgeX) >= FirstAgeX.AgeX && (int)getTValue(x.MortalityTableAgeX) <= SecondAgeX.AgeX).ToList();
            var density = currentDensitys.First(x => (int)getTValue(x.MortalityTableAgeX) == FirstAgeX.AgeX);

            currentResidualDensitys.Clear();

            for (int i = 0; i < firstD.Count(); i++)
            {
                currentResidualDensitys.Add(new ResidualDensity()
                {
                    MortalityTableID = firstD[i].MortalityTableID,
                    MortalityTableAgeX = firstD[i].MortalityTableAgeX,
                    Standart = firstD[i].Standart / density.Standart,
                    Weibull = firstD[i].Weibull / density.Weibull,
                    Relay = firstD[i].Relay / density.Relay,
                    Gompertz = firstD[i].Gompertz / density.Gompertz,
                    Exponential = firstD[i].Exponential / density.Exponential
                });
            }

            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                await cn.OpenAsync();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"DELETE FROM ResidualDensity";
                        await cmd.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
                cn.Close();
            }

            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                await cn.OpenAsync();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $@"INSERT INTO {nameof(ResidualDensity)}(MortalityTableID, MortalityTableAgeX, Standart, Weibull, Relay, Gompertz, Exponential) VALUES (@MortalityTableID, @MortalityTableAgeX, @Standart, @Weibull, @Relay, @Gompertz, @Exponential);  select last_insert_rowid()";
                        for (int i = 0; i < currentResidualDensitys.Count(); i++)
                        {
                            cmd.Parameters.AddWithValue("@MortalityTableID", currentResidualDensitys[i].MortalityTableID);
                            cmd.Parameters.AddWithValue("@MortalityTableAgeX", currentResidualDensitys[i].MortalityTableAgeX);
                            cmd.Parameters.AddWithValue("@Standart", currentResidualDensitys[i].Standart);
                            cmd.Parameters.AddWithValue("@Weibull", currentResidualDensitys[i].Weibull);
                            cmd.Parameters.AddWithValue("@Relay", currentResidualDensitys[i].Relay);
                            cmd.Parameters.AddWithValue("@Gompertz", currentResidualDensitys[i].Gompertz);
                            cmd.Parameters.AddWithValue("@Exponential", currentResidualDensitys[i].Exponential);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    transaction.Commit();
                }
                cn.Close();
            }

            await SelectResidualDensity();


            await RefreshChartsResidualDivides();
        }

        private bool CanCalculateResidual()
        {
            return currentDensitys.Any(x => x.Standart != null) && currentSurvivalFunctions.Any(x=>x.Standart != null) && FirstAgeX != null && RangeDataList != null && FirstAgeX != RangeDataList?.First();
        }

        #endregion

        #region SurvivalFunctions

        public async Task RefreshChartsSurvivalFunctions() {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SurvivalFunctionsCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                }; 
                SurvivalFunctionsCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(relay);


                SurvivalFunctionsYFormatter = value => Math.Round(value, 3).ToString();
            }));
        }
         
        private SeriesCollection survivalFunctionsCollection;

        public SeriesCollection SurvivalFunctionsCollection
        {
            get { return survivalFunctionsCollection; }
            set
            {
                survivalFunctionsCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionsCollection)));
            }
        }

        private Func<double, string> survivalFunctionsYFormatter;

        public Func<double, string> SurvivalFunctionsYFormatter
        {
            get { return survivalFunctionsYFormatter; }
            set
            {
                survivalFunctionsYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionsYFormatter)));
            }
        }

        #endregion

        #region Density

        public async Task RefreshChartsDensitys()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DensitysCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(relay);


                DensitysYFormatter = value => Math.Round(value, 3).ToString();
            }));
        }

        private SeriesCollection densitysCollection;

        public SeriesCollection DensitysCollection
        {
            get { return densitysCollection; }
            set
            {
                densitysCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DensitysCollection)));
            }
        }

        private Func<double, string> densitysYFormatter;

        public Func<double, string> DensitysYFormatter
        {
            get { return densitysYFormatter; }
            set
            {
                densitysYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DensitysYFormatter)));
            }
        }

        #endregion

        #region ResidualSurvivalFunctions

        public async Task RefreshChartsResidualSurvivalFunctions()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ResidualSurvivalFunctionsCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(currentResidualSurvivalFunctions.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualSurvivalFunctionsCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentResidualSurvivalFunctions.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualSurvivalFunctionsCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentResidualSurvivalFunctions.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualSurvivalFunctionsCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentResidualSurvivalFunctions.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualSurvivalFunctionsCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentResidualSurvivalFunctions.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualSurvivalFunctionsCollection.Add(relay);


                ResidualSurvivalFunctionsYFormatter = value => Math.Round(value, 3).ToString();
            }));
        }

        private SeriesCollection residualSurvivalFunctionsCollection;

        public SeriesCollection ResidualSurvivalFunctionsCollection
        {
            get { return residualSurvivalFunctionsCollection; }
            set
            {
                residualSurvivalFunctionsCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualSurvivalFunctionsCollection)));
            }
        }

        private Func<double, string> residualSurvivalFunctionsYFormatter;

        public Func<double, string> ResidualSurvivalFunctionsYFormatter
        {
            get { return residualSurvivalFunctionsYFormatter; }
            set
            {
                residualSurvivalFunctionsYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualSurvivalFunctionsYFormatter)));
            }
        }

        #endregion

        #region ResidualDensity

        public async Task RefreshChartsResidualDensitys()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ResidualDensitysCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(currentResidualDensitys.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDensitysCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentResidualDensitys.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDensitysCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentResidualDensitys.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDensitysCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentResidualDensitys.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDensitysCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentResidualDensitys.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDensitysCollection.Add(relay);


                ResidualDensitysYFormatter = value => Math.Round(value, 3).ToString();
            }));
        }

        private SeriesCollection residualDensitysCollection;

        public SeriesCollection ResidualDensitysCollection
        {
            get { return residualDensitysCollection; }
            set
            {
                residualDensitysCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualDensitysCollection)));
            }
        }

        private Func<double, string> residualDensitysYFormatter;

        public Func<double, string> ResidualDensitysYFormatter
        {
            get { return residualDensitysYFormatter; }
            set
            {
                residualDensitysYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualDensitysYFormatter)));
            }
        }

        #endregion

        #region Density/SurvivalFunctions

        public async Task RefreshChartsDivides()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!currentDensitys.Where(x=>x.Weibull != null).Any() || !currentSurvivalFunctions.Where(x => x.Weibull != null).Any()) return;

                List<Density> divides = new List<Density>();

                for (int i = 0; i < currentDensitys.Count(); i++)
                {
                    divides.Add(new Density()
                    {
                        Standart = currentDensitys[i].Standart / (currentSurvivalFunctions[i].Standart == 0 ? 1 : currentSurvivalFunctions[i].Standart),
                        Weibull = currentDensitys[i].Weibull / (currentSurvivalFunctions[i].Weibull == 0 ? 1 : currentSurvivalFunctions[i].Weibull),
                        Exponential = currentDensitys[i].Exponential / (currentSurvivalFunctions[i].Exponential == 0 ? 1 : currentSurvivalFunctions[i].Exponential),
                        Gompertz = currentDensitys[i].Gompertz / (currentSurvivalFunctions[i].Gompertz == 0 ? 1 : currentSurvivalFunctions[i].Gompertz),
                        Relay = currentDensitys[i].Relay / (currentSurvivalFunctions[i].Relay == 0 ? 1 : currentSurvivalFunctions[i].Relay)
                    });
                }

                DividesCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(relay);


                DividesYFormatter = value => Math.Round(value, 3).ToString();
            }));
        }

        private SeriesCollection dividesCollection;

        public SeriesCollection DividesCollection
        {
            get { return dividesCollection; }
            set
            {
                dividesCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DividesCollection)));
            }
        }

        private Func<double, string> dividesYFormatter;

        public Func<double, string> DividesYFormatter
        {
            get { return dividesYFormatter; }
            set
            {
                dividesYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DividesYFormatter)));
            }
        }

        #endregion

        #region ResidualDensity/ResidualSurvivalFunctions

        public async Task RefreshChartsResidualDivides()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!currentResidualDensitys.Where(x => x.Weibull != null).Any() || !currentResidualSurvivalFunctions.Where(x => x.Weibull != null).Any()) return;

                List<Density> residualDivides = new List<Density>();

                for (int i = 0; i < currentResidualDensitys.Count(); i++)
                {
                    residualDivides.Add(new Density()
                    {
                        Standart = currentResidualDensitys[i].Standart / (currentResidualSurvivalFunctions[i].Standart == 0 ? 1 : currentResidualSurvivalFunctions[i].Standart),
                        Weibull = currentResidualDensitys[i].Weibull / (currentResidualSurvivalFunctions[i].Weibull == 0 ? 1 : currentResidualSurvivalFunctions[i].Weibull),
                        Exponential = currentResidualDensitys[i].Exponential / (currentResidualSurvivalFunctions[i].Exponential == 0 ? 1 : currentResidualSurvivalFunctions[i].Exponential),
                        Gompertz = currentResidualDensitys[i].Gompertz / (currentResidualSurvivalFunctions[i].Gompertz == 0 ? 1 : currentResidualSurvivalFunctions[i].Gompertz),
                        Relay = currentResidualDensitys[i].Relay / (currentResidualSurvivalFunctions[i].Relay == 0 ? 1 : currentResidualSurvivalFunctions[i].Relay)
                    });
                }

                ResidualDividesCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(residualDivides.Select(x => Math.Round(x.Standart ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDividesCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(residualDivides.Select(x => Math.Round(x.Weibull ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDividesCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(residualDivides.Select(x => Math.Round(x.Exponential ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDividesCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(residualDivides.Select(x => Math.Round(x.Gompertz ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDividesCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(residualDivides.Select(x => Math.Round(x.Relay ?? 0, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                ResidualDividesCollection.Add(relay);


                ResidualDividesYFormatter = value => Math.Round(value, 3).ToString();
            }));
        }

        private SeriesCollection residualDividesCollection;

        public SeriesCollection ResidualDividesCollection
        {
            get { return residualDividesCollection; }
            set
            {

                residualDividesCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualDividesCollection)));
            }
        }

        private Func<double, string> residualDividesYFormatter;

        public Func<double, string> ResidualDividesYFormatter
        {
            get { return residualDividesYFormatter; }
            set
            {
                residualDividesYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResidualDividesYFormatter)));
            }
        }

        #endregion

    }
}
