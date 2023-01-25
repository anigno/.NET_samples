#region

using System;
using States.StateMachine;

#endregion

namespace States
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            initSm123();
            runSm();
        }

        #endregion

        #region Private Methods

        private void initSm123()
        {
            AState<int> stateR = new AState<int>("StateR", false) {StateEnterAction = p_i => { Console.WriteLine("StateR Enter"); }, StateLeaveAction = p_i => { Console.WriteLine("StateR Leave"); }};
            ;
            AState<int> stateA = new AState<int>("StateA", false) {StateEnterAction = p_i => { Console.WriteLine("StateA Enter"); }, StateLeaveAction = p_i => { Console.WriteLine("StateA Leave"); }};
            AState<int> stateB = new AState<int>("StateB", false) {StateEnterAction = p_i => { Console.WriteLine("StateB Enter"); }, StateLeaveAction = p_i => { Console.WriteLine("StateB Leave"); }};
            ;
            AState<int> stateC = new AState<int>("StateC", true) {StateEnterAction = p_i => { Console.WriteLine("StateC Enter"); }, StateLeaveAction = p_i => { Console.WriteLine("StateC Leave"); }};
            ;
            stateR.Triggers.Add(p_i =>
            {
                if (p_i == 1) return stateA;
                return stateR;
            });
            stateA.Triggers.Add(p_i =>
            {
                if (p_i == 1) return stateA;
                if (p_i == 2) return stateB;
                return stateR;
            });
            stateB.Triggers.Add(p_i =>
            {
                if (p_i == 3) return stateC;
                if (p_i == 1) return stateA;
                if (p_i == 2) return stateR;
                return stateR;
            });
            m_sm = new AStateMachine<int>(stateR);
            m_sm.StateChanged += m_sm_StateChanged;
            m_sm.StateFinit += m_sm_StateFinit;
        }

        private void m_sm_StateFinit(object sender, StateChangedEventArgs<int> e)
        {
            Console.WriteLine("*Finit* " + e);
        }

        private void runSm()
        {
            int[] ints = new int[] {2, 1, 1, 2, 1, 2, 4, 2, 1, 2, 3, 1, 2, 3};
            int a = 0;
            while (!m_sm.NextState(ints[a++]).IsFinit) ;
        }

        private void m_sm_StateChanged(object sender, StateChangedEventArgs<int> e)
        {
            Console.WriteLine(e);
        }

        private static void Main(string[] args)
        {
            new Program();
            Console.ReadKey();
        }

        #endregion

        #region Fields

        private AStateMachine<int> m_sm;

        #endregion
    }
}