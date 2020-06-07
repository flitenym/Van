namespace Van.LocalDataBase.ModelsHelper
{
    public interface IMethod
    {
        double? Standart { get; set; }
        double? Weibull { get; set; }
        double? Relay { get; set; }
        double? Gompertz { get; set; }
        double? Exponential { get; set; }
        double? LogLogistic { get; set; }
        double? LogNormal { get; set; }
    }
}
