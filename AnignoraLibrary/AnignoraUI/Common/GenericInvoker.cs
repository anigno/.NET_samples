//#region

//using System;

//#endregion

//namespace AnignoraUI.Common
//{
//    public static class GenericInvoker
//    {


//        #region GenericInvoke

//        [Obsolete("Use CrossThreadActivities instead")]
//        public static void GenericInvoke<CONTROL>(CONTROL control, Action<CONTROL> actionDelegate) where CONTROL : System.Windows.Forms.Control
//        {
//            if (control.InvokeRequired)
//            {
//                control.Invoke(new Action<CONTROL, Action<CONTROL>>(GenericInvoke), new object[] { control, actionDelegate });
//            }
//            else
//            {
//                actionDelegate(control);
//            }
//        }

//        [Obsolete("Use CrossThreadActivities instead")]
//        public static void GenericInvokeTryCached<TControl>(TControl p_control, Action<TControl> actionDelegate) where TControl : System.Windows.Forms.Control
//        {
//            try
//            {
//                if (p_control.InvokeRequired)
//                {
//                    p_control.Invoke(new Action<TControl, Action<TControl>>(GenericInvoke), new object[] { p_control, actionDelegate });
//                }
//                else
//                {
//                    actionDelegate(p_control);
//                }
//            }
//            finally
//            {
//            }
//        }


//        //Usage example
//        //GenericInvoke(control, ctrl => ctrl.Text = text);

//        #endregion


//    }
//}