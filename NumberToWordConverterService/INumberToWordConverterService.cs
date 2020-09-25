using System.ServiceModel;

namespace NumberToWordConverterService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INumberToWordConverterService" in both code and config file together.
    [ServiceContract]
    public interface INumberToWordConverterService
    {
        [OperationContract]
        string getNumber(string numAsString);
    }
}
