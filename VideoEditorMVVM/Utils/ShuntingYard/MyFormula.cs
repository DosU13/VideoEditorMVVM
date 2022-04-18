using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditorMVVM.Utils.ShuntingYard
{
    public class MyFormula
    {
        List<Token> tokens;
        public MyFormula(string formulaStr)
        {
            tokens = StrToFormulaConverter.Convert(formulaStr);
        }
    }

    internal static class StrToFormulaConverter
    {
        internal static List<Token> Convert(string formulaStr)
        {
            List<Token> result = new List<Token>();
            int? strItStart = null;
            int? numItStart = null;
            bool isProperty = false;
            for (int i = 0; i < formulaStr.Length; i++)
            {
                char c = formulaStr[i];
                if (numItStart!=null && !char.IsDigit(c) && c != '.')
                {
                    result.Add(new NumberToken(double.Parse(
                        formulaStr.Substring(numItStart.Value, i - numItStart.Value + 1))));
                    numItStart = null;
                } 
                if (strItStart != null && isProperCharForName(c))
                {
                    string tokenStr = formulaStr.Substring(strItStart.Value, i - strItStart.Value + 1);
                    if (isProperty) {
                        result.Add(new PropertyToken(tokenStr));
                        isProperty = false;
                    }else if (c == '(')
                    {
                        result.Add(new FunctionToken(tokenStr));
                    }else if (c == '[')
                    {
                        result.Add(new ArrayToken(tokenStr));
                    }
                }


                if (Char.IsLetter(c))
                {
                    if (strItStart == null) strItStart = i;
                }
                else if (strItStart != null & Char.IsDigit(c)) //add here is proper character for value names
                {
                    if (numItStart == null) numItStart = i;
                }
                else if (numItStart != null & c != '.')
                {
                    result.Add(new NumberToken(double.Parse(
                        formulaStr.Substring(numItStart.Value, i - numItStart.Value + 1))));
                    numItStart = null;
                }
            }
            return result;
        }

        private static bool isProperCharForName(char c)
        {
            return Char.IsLetterOrDigit(c) || c == '_';
        }
    }
}
