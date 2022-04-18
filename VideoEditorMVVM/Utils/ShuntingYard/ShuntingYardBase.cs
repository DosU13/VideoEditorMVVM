/*
 * Copyright 2012 Søren Gullach
 * 
 * This code is written by Søren Gullach
 * mail code@toolsbox.dk
 * Web www.toolsbox.dk
 * 
 * Ver. 1.1
 * */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Toolsbox.ShuntingYard
{
    // See http://en.wikipedia.org/wiki/Shunting-yard_algorithm 

    /// <summary>
    /// Base class for a Shunting Yard algormittm
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    public abstract class ShuntingYardBase<TResult, TInput>
    {
        /// <summary>
        /// Struct to make a table of operators, precedence and associativity
        /// </summary>
        public struct PrecedensAssociativity
        {
            public PrecedensAssociativity(int p, Asso a)
            {
                Prec = p;
                Associativity = a;
            }
            public int Prec;
            public enum Asso { Left, Right };
            public Asso Associativity;
        }

        public delegate void DebugRPNDelegate(List<object> inter, List<char> opr);
        public event DebugRPNDelegate DebugRPNSteps;
        public delegate void DebugResDelegate(List<object> res, List<TResult> var);
        public event DebugResDelegate DebugResSteps;

        /// <summary>
        /// Execute the input list
        /// </summary>
        /// <param name="InputList">List of ident and opr</param>
        /// <param name="InputObj">alternative object to evaluate on</param>
        /// <returns></returns>
        public TResult Execute(List<TInput> InputList, object TagObj)
        {
            Stack<object> inter = new Stack<object>(); // output stack
            Stack<char> opr = new Stack<char>();    // operator stack

            foreach (TInput s in InputList)
            {
                if (IsNoise(s))
                    continue;

                char? o = TypecastOperator(s);
                if (IsOperator(o))
                {
                    while (opr.Count > 0)
                    {
                        char ot = opr.Peek();
                        // if ot is operator && o < ot
                        if (IsOperator(ot) && (
                            (Association((char)o) == PrecedensAssociativity.Asso.Left && Precedence((char)o, ot) <= 0) ||
                            (Association((char)o) == PrecedensAssociativity.Asso.Right && Precedence((char)o, ot) < 0))
                            )
                            inter.Push(opr.Pop()); // stack to output
                        else
                            break;
                    }
                    opr.Push((char)o);
                }
                else if (s.ToString() == "(")
                {
                    opr.Push('(');
                }
                else if (s.ToString() == ")")
                {
                    bool pe = false;
                    while (opr.Count > 0)
                    {   // opr to out until (
                        char sc = opr.Peek();
                        if (sc == '(')
                        {
                            pe = true;
                            break;
                        }
                        else
                            inter.Push(opr.Pop());
                    }
                    if (!pe) throw new Exception("No Left (");
                    opr.Pop(); // pop off (
                }
                else if (IsIdentifier(s))
                {
                    inter.Push(s);
                }
                else
                {
                    if (!IsNoise(s))
                        throw new Exception("Unknowen token");
                }
                if (DebugRPNSteps != null)
                    DebugRPNSteps(inter.Reverse().ToList(), opr.ToList());
            }

            // put opr to out
            while (opr.Count > 0)
                inter.Push(opr.Pop());
            if (DebugRPNSteps != null)
                DebugRPNSteps(inter.Reverse().ToList(), opr.ToList());

            Queue<object> res = new Queue<object>(inter.Reverse());
            Stack<TResult> var = new Stack<TResult>(); // vars stack
            if (DebugResSteps != null)
                DebugResSteps(res.ToList(), var.ToList());
            // execute output stack
            while (res.Count > 0)
            {
                object o = res.Dequeue();
                if (o.GetType() == typeof(TInput))
                {
                    var.Push(TypecastIdentifier((TInput)o, TagObj));
                }
                if (o.GetType() == typeof(char))
                {
                    TResult r = var.Pop(); TResult l = var.Pop();
                    var.Push(Evaluate(l, (char)o, r));
                }
                if (DebugResSteps != null)
                    DebugResSteps(res.ToList(), var.ToList());
            }
            return var.Peek(); // return result
        }

        /// <summary>
        /// Is input acceptable noise
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract bool IsNoise(TInput input);

        /// <summary>
        /// Calcualate the result of result1 and result2
        /// </summary>
        /// <param name="result1"></param>
        /// <param name="opr"></param>
        /// <param name="result2"></param>
        /// <returns></returns>
        public abstract TResult Evaluate(TResult result1, char opr, TResult result2);

        /// <summary>
        /// Typecast input to Result type
        /// </summary>
        /// <param name="InputObj">Alt. object to evaluate on</param>
        /// <param name="input">Identifier to typecast</param>
        /// <returns></returns>
        public abstract TResult TypecastIdentifier(TInput input, object TagObj);

        /// <summary>
        /// Is input a identifier
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract bool IsIdentifier(TInput input);

        /// <summary>
        /// Calc. precedence
        /// </summary>
        /// <param name="opr1"></param>
        /// <param name="opr2"></param>
        /// <returns></returns>
        public abstract int Precedence(char opr1, char opr2);

        /// <summary>
        /// Is operator left/Right assocative
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        public abstract PrecedensAssociativity.Asso Association(char opr);

        /// <summary>
        /// Is input a operato
        /// </summary>
        /// <param name="Opr"></param>
        /// <returns></returns>
        public abstract bool IsOperator(char? Opr);

        /// <summary>
        /// Typecast input to a operator
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        public abstract char? TypecastOperator(TInput opr);
    }

    /// <summary>
    /// Implementation of simpel math class
    /// </summary>
}