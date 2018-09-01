using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interpreter
{
    public static class StdLib
    {
        #region General
        public static string type(object Value)
        {
            if (Value == null)
            {
                return "null";
            }
            else if (Value is string)
            {
                return "string";
            }
            else if(Value is double)
            {
                return "number";
            }
            else if (Value is bool)
            {
                return "boolean";
            }
            else if (Value is Dictionary<int, object>)
            {
                return "array";
            }
            else if (Value is FuncInfo)
            {
                return "function";
            }
            else
            {
                return "unknown";
            }
        }

        public static double tonumber(string Value)
        {
            return double.Parse(Value, CultureInfo.InvariantCulture);
        }

        public static void sleep(double Time)
        {
            Thread.Sleep((int)Time);
        }
        #endregion

        #region Strings and arrays
        public static Dictionary<int, object> copy(Dictionary<int, object> Arr)
        {
            Dictionary<int, object> Result = new Dictionary<int, object>();
            foreach (KeyValuePair<int, object> KVP in Arr)
            {
                Result.Add(KVP.Key, KVP.Value is Dictionary<int, object> ? copy((Dictionary <int, object>)KVP.Value) : KVP.Value);
            }
            return Result;
        }

        public static string lower(string Value)
        {
            return Value.ToLower();
        }

        public static string upper(string Value)
        {
            return Value.ToUpper();
        }

        public static double length(object Value)
        {
            if (Value is string)
            {
                return ((string)Value).Length;
            }
            else if (Value is Dictionary<int, object>)
            {
                return ((Dictionary<int, object>)Value).Count;
            }
            return 0f;
        }

        public static string substring(string Value, double Start, double Length)
        {
            return Value.Substring((int)Start, (int)Length);
        }
        #endregion

        #region Math
        private static Random Rand = new Random();
        public static double cos(double Value)
        {
            return Math.Cos(Value);
        }

        public static double abs(double Value)
        {
            return Math.Abs(Value);
        }

        public static double asin(double Value)
        {
            return Math.Asin(Value);
        }

        public static double acos(double Value)
        {
            return Math.Acos(Value);
        }

        public static double atan(double Value)
        {
            return Math.Atan(Value);
        }

        public static double ceil(double Value)
        {
            return Math.Ceiling(Value);
        }

        public static double floor(double Value)
        {
            return Math.Floor(Value);
        }

        public static double log(double Value)
        {
            return Math.Log10(Value);
        }

        public static double max(double Value1, double Value2)
        {
            return Math.Max(Value1, Value2);
        }

        public static double min(double Value1, double Value2)
        {
            return Math.Min(Value1, Value2);
        }

        public static double pi()
        {
            return Math.PI;
        }

        public static double power(double Value1, double Value2)
        {
            return Math.Pow(Value1, Value2);
        }

        public static double randdouble()
        {
            return Rand.NextDouble();
        }

        public static double randint(double Value1, double Value2)
        {
            return Rand.Next((int)Value1, (int)Value2);
        }

        public static double sin(double Value)
        {
            return Math.Sin(Value);
        }

        public static double sinh(double Value)
        {
            return Math.Sinh(Value);
        }

        public static double cosh(double Value)
        {
            return Math.Cosh(Value);
        }

        public static double sqrt(double Value)
        {
            return Math.Sqrt(Value);
        }

        public static double tan(double Value)
        {
            return Math.Tan(Value);
        }

        public static double tanh(double Value)
        {
            return Math.Tanh(Value);
        }
        #endregion
    }
}
