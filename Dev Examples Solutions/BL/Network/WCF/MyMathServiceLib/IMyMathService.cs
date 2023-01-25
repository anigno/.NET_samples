#region

using System;
using System.ServiceModel;

#endregion

namespace MyMathServiceLib
{
    [ServiceContract]
    public interface IMyMathService
    {
        #region Public Methods

        [OperationContract]
        double Add(double p_firstNnumber, double p_SecondNumber);

        [OperationContract]
        double Substract(double p_firstNnumber, double p_SecondNumber);

        #endregion
    }
}