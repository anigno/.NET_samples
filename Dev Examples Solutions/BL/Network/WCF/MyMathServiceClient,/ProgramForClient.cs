#region

using System;
using System.Threading;
using BlCommon;

#endregion

namespace MyMathServiceClient1
{
    internal class ProgramForClient
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            const double DBL_X = 2000.0;
            const double DBL_Y = 100.0;
            double dblResult;
            try
            {
                MyMathServiceClient mathClientTcp = new MyMathServiceClient("NetTcpBinding_IMyMathService");
                TestUtils.ConsoleLog(ConsoleColor.Green, "Using NetTcpBinding Binding");
                dblResult = mathClientTcp.Add(DBL_X, DBL_Y);
                TestUtils.ConsoleLog("Calling Add >> X : {0:F2} Y : {1:F2} Result : {2:F2}", DBL_X, DBL_Y, dblResult);
                dblResult = mathClientTcp.Substract(DBL_X, DBL_Y);
                TestUtils.ConsoleLog("Calling Sub >> X : {0:F2} Y : {1:F2} Result : {2:F2}", DBL_X, DBL_Y, dblResult);

                MyMathServiceClient mathClientHttp = new MyMathServiceClient("BasicHttpBinding_IMyMathService");
                TestUtils.ConsoleLog(ConsoleColor.Yellow, "Using BasicHttpBinding Binding");
                dblResult = mathClientHttp.Add(DBL_X, DBL_Y);
                TestUtils.ConsoleLog("Calling Add >> X : {0:F2} Y : {1:F2} Result : {2:F2}", DBL_X, DBL_Y, dblResult);
                dblResult = mathClientHttp.Substract(DBL_X, DBL_Y);
                TestUtils.ConsoleLog("Calling Sub >> X : {0:F2} Y : {1:F2} Result : {2:F2}", DBL_X, DBL_Y, dblResult);
            }
            catch (Exception eX)
            {
                TestUtils.ConsoleLog("There was an error while calling Service [" + eX.Message + "]");
            }
            TestUtils.WaitForEnter();
        }

        #endregion
    }
}