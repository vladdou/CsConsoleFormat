﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Alba.CsConsoleFormat.Framework.Reflection;
using Alba.CsConsoleFormat.Framework.Text;

namespace Alba.CsConsoleFormat.Markup
{
    using ConverterDelegate = Func<Object, Object, CultureInfo, Object>;
    using ConverterDelegatesCache = Dictionary<MethodInfo, Func<Object, Object, CultureInfo, Object>>;

    public class CallConverterExpression : GetExpressionBase
    {
        private static readonly ThreadLocal<ConverterDelegatesCache> _converterFunctions =
            new ThreadLocal<ConverterDelegatesCache>(() => new ConverterDelegatesCache());

        protected override object GetValueFromSource (object source)
        {
            if (source == null)
                throw new InvalidOperationException("Source cannot be null.");
            if (Path.IsNullOrEmpty())
                return ConvertValue(source);
            return TraversePathToMethod(source);
        }

        protected override object TryGetCachedMethod (MethodInfo method)
        {
            ConverterDelegate func;
            return _converterFunctions.Value.TryGetValue(method, out func) ? func : null;
        }

        protected override object ConvertValue (object value)
        {
            var func3 = value as ConverterDelegate;
            if (func3 != null)
                return func3;
            var func2 = value as Func<object, object, object>;
            if (func2 != null)
                return (ConverterDelegate)((v, p, c) => func2(v, p));
            var func1 = value as Func<object, object>;
            if (func1 != null)
                return (ConverterDelegate)((v, p, c) => func1(v));
            throw new InvalidOperationException("Cannot cast value to converter delegate.");
        }

        protected override object ConvertMethod (MethodInfo method, object target)
        {
            ConverterDelegate func;
            if (!_converterFunctions.Value.TryGetValue(method, out func)) {
                ParameterInfo[] parameters = method.GetParameters();
                Type targetType = target.GetType();
                if (method.IsStatic) {
                    CultureInfo culture = EffectiveCulture;
                    if (parameters.Length == 3) {
                        var call = DynamicCaller.CallStatic<Func<Type, object, object, CultureInfo, object>>(method.Name);
                        func = (v, p, c) => call(targetType, v, p, c ?? culture);
                    }
                    else if (parameters.Length == 2) {
                        var call = DynamicCaller.CallStatic<Func<Type, object, object, object>>(method.Name);
                        func = (v, p, c) => call(targetType, v, p);
                    }
                    else if (parameters.Length == 1) {
                        var call = DynamicCaller.CallStatic<Func<Type, object, object>>(method.Name);
                        func = (v, p, c) => call(targetType, v);
                    }
                }
                else {
                    CultureInfo culture = EffectiveCulture;
                    if (parameters.Length == 3) {
                        var call = DynamicCaller.Call<Func<Object, object, object, CultureInfo, object>>(method.Name);
                        func = (v, p, c) => call(target, v, p, c ?? culture);
                    }
                    else if (parameters.Length == 2) {
                        var call = DynamicCaller.Call<Func<Object, object, object, object>>(method.Name);
                        func = (v, p, c) => call(target, v, p);
                    }
                    else if (parameters.Length == 1) {
                        var call = DynamicCaller.Call<Func<Object, object, object>>(method.Name);
                        func = (v, p, c) => call(target, v);
                    }
                }
                _converterFunctions.Value.Add(method, func);
            }
            return func;
        }
    }
}