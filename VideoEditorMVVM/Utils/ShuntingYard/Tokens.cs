using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditorMVVM.Utils.ShuntingYard
{
    public abstract class Token
    {
        object Value { get;}

        public Token(object value)
        {
            Value = value;
        }
    }

    public class OperationToken : Token
    {
        public OperationToken(char value) : base(value)
        {
        }
    }

    public class FunctionToken : Token
    {
        public FunctionToken(string value) : base(value)
        {
        }
    }

    public class ArrayToken : Token
    {
        public ArrayToken(string value) : base(value)
        {
        }
    }

    public class PropertyToken : Token
    {
        public PropertyToken(string value) : base(value)
        {
        }
    }

    public class NumberToken : Token
    {
        public NumberToken(double value) : base(value)
        {
        }
    }

    public class ValueToken : Token
    {
        public ValueToken(string value) : base(value)
        {
        }
    }
}
