using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Toolsbox.ShuntingYard;
using VideoEditorMVVM.Utils.ShuntingYard;

namespace ShuntingYardTest
{
    class Program
    {
        public static void MainRun()
        {
            {
                MyShuntingYard SY = new MyShuntingYard();
                String s = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
                Debug.WriteLine("input: {0}", s); Debug.WriteLine("");
                List<String> ss = s.Split(' ').ToList();
                SY.DebugRPNSteps += new ShuntingYardBase<double, string>.DebugRPNDelegate(SY_DebugRPNSteps);
                SY.DebugResSteps += new ShuntingYardBase<double, string>.DebugResDelegate(SY_DebugResSteps);
                Double res = SY.Execute(ss, null);

                bool ok = res == 3.0001220703125;
                Debug.WriteLine("input: {0} = {1} {2}", s, res, (ok ? "Ok" : "Error"));
            }
        }

        static void SY_DebugRPNSteps(List<object> inter, List<char> opr)
        {
            Debug.Write("RPN: ");
            Debug.Write("inters: ");
            foreach (object o in inter)
                Debug.Write(o.ToString()+" ");
            Debug.Write("ops: ");
            foreach (char o in opr)
                Debug.Write(o.ToString()+" ");
            Debug.WriteLine("");
        }

        static void SY_DebugResSteps(List<object> res, List<double> var)
        {
            Debug.Write("Res: ");
            Debug.Write("res: ");
            foreach (object o in res)
                Debug.Write(o.ToString()+" ");
            Debug.Write(" = var ");
            foreach (double o in var)
                Debug.Write(o.ToString()+" ");
            Debug.WriteLine("");
        }
        
    }
}