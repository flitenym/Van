using System;
using System.Collections.Generic;
using System.Linq;
using Van.DataBase.Models;
using Van.Methods.Helper;
using static Van.Helper.Methods;

namespace Van.Methods
{
    public class Standart : MethodAbstractClass
    {
        public override int ParametrCount { get; set; } = 2;

        public Standart(List<double> tValue, List<int> t, double r, List<MortalityTable> currentMortalityTables, List<int> delta = null)
            : base()
        {
            this.currentMortalityTables = currentMortalityTables;
            maxNumberOfSurvivors = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max();

            (this as MethodAbstractClass).ParamterCalculation(t, delta, r);
            (this as MethodAbstractClass).GetSurvivalFunctions(tValue);
        }

        public int? maxNumberOfSurvivors = default;

        public List<MortalityTable> currentMortalityTables = new List<MortalityTable>();

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        { }

        public override double SurvivalFunction(double tValue)
        {
            return (double)currentMortalityTables.FirstOrDefault(x=>GetTValue(x.AgeX) == tValue)?.NumberOfSurvivors /
                    (double)maxNumberOfSurvivors;
        }

        public override double GetDensity(double tValue)
        {
            return (double)currentMortalityTables.FirstOrDefault(x => GetTValue(x.AgeX) == tValue)?.NumberOfDead /
                    (double)maxNumberOfSurvivors;
        }
    }
}