﻿using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Плотность гамма распределения (остаточная)", CanInsert = false)]
    public class ResidualDensity : ModelClass, IMortalityTable, IMethod, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [ColumnData(ShowInTable = false, IsNullable = false)]
        public int MortalityTableID { get; set; }

        [Description("Возраст X")]
        public string MortalityTableAgeX { get; set; }

        [Description("Табличный")]
        public double? Standart { get; set; }

        [Description("Вейбулла")]
        public double? Weibull { get; set; }

        [Description("Рэлея")]
        public double? Relay { get; set; }

        [Description("Гомпертца")]
        public double? Gompertz { get; set; }

        [Description("Экспоненциальное")]
        public double? Exponential { get; set; }

        [Description("Логлогистическое")]
        public double? LogLogistic { get; set; }

        [Description("Логнормальное")]
        public double? LogNormal { get; set; }

        public object Clone()
        {
            return new ResidualDensity
            {
                ID = this.ID,
                MortalityTableID = this.MortalityTableID,
                MortalityTableAgeX = this.MortalityTableAgeX,
                Standart = this.Standart,
                Weibull = this.Weibull,
                Relay = this.Relay,
                Gompertz = this.Gompertz,
                Exponential = this.Exponential,
                LogLogistic = this.LogLogistic,
                LogNormal = this.LogNormal
            };
        }
    }
}
