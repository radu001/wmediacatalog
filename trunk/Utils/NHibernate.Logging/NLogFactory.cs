using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate.Logging
{
    public class NLogFactory : ILoggerFactory
    {
        private static readonly System.Type LogManagerType = System.Type.GetType("NLog.LogManager, NLog");

        private static Func<string, object> CreateLoggerInstanceFunc;

        static NLogFactory()
        {
            CreateLoggerInstanceFunc = CreateLoggerInstance();
        }

        #region ILoggerFactory Members

        public IInternalLogger LoggerFor(System.Type type)
        {
            return new NLogLogger(CreateLoggerInstanceFunc(type.Name));
        }

        public IInternalLogger LoggerFor(string keyName)
        {
            return new NLogLogger(CreateLoggerInstanceFunc(keyName));
        }

        #endregion

        private static Func<string, object> CreateLoggerInstance()
        {
            var method = LogManagerType.GetMethod("GetLogger", new[] { typeof(string) });
            ParameterExpression nameParam = Expression.Parameter(typeof(string));
            MethodCallExpression methodCall = Expression.Call(null, method, new Expression[] { nameParam });

            return Expression.Lambda<Func<string, object>>(methodCall, new[] { nameParam }).Compile();
        }
    }

    public class NLogLogger : IInternalLogger
    {
        private static readonly System.Type LoggerType = System.Type.GetType("NLog.Logger, NLog");

        private static readonly Func<object, bool> DebugPropertyGetter;
        private static readonly Func<object, bool> ErrorPropertyGetter;
        private static readonly Func<object, bool> FatalPropertyGetter;
        private static readonly Func<object, bool> InfoPropertyGetter;
        private static readonly Func<object, bool> WarnPropertyGetter;

        private static readonly Action<object, string> DebugAction;
        private static readonly Action<object, string> ErrorAction;
        private static readonly Action<object, string> WarnAction;
        private static readonly Action<object, string> InfoAction;
        private static readonly Action<object, string> FatalAction;

        private static readonly Action<object, string, Exception> DebugExceptionAction;
        private static readonly Action<object, string, Exception> ErrorExceptionAction;
        private static readonly Action<object, string, Exception> WarnExceptionAction;
        private static readonly Action<object, string, Exception> InfoExceptionAction;
        private static readonly Action<object, string, Exception> FatalExceptionAction;

        private object log;

        static NLogLogger()
        {
            DebugPropertyGetter = CreatePropertyGetter("IsDebugEnabled");
            ErrorPropertyGetter = CreatePropertyGetter("IsErrorEnabled");
            FatalPropertyGetter = CreatePropertyGetter("IsFatalEnabled");
            InfoPropertyGetter = CreatePropertyGetter("IsInfoEnabled");
            WarnPropertyGetter = CreatePropertyGetter("IsWarnEnabled");

            DebugAction = CreateSimpleAction("Debug");
            ErrorAction = CreateSimpleAction("Error");
            WarnAction = CreateSimpleAction("Warn");
            InfoAction = CreateSimpleAction("Info");
            FatalAction = CreateSimpleAction("Fatal");

            DebugExceptionAction = CreateExceptionAction("Debug");
            ErrorExceptionAction = CreateExceptionAction("Error");
            WarnExceptionAction = CreateExceptionAction("Warn");
            InfoExceptionAction = CreateExceptionAction("Info");
            FatalExceptionAction = CreateExceptionAction("Fatal");
        }

        public NLogLogger(object log)
        {
            this.log = log;
        }

        #region IInternalLogger Members

        #region Properties

        public bool IsDebugEnabled
        {
            get
            {
                return DebugPropertyGetter(log);
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return ErrorPropertyGetter(log);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return FatalPropertyGetter(log);
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return InfoPropertyGetter(log);
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return WarnPropertyGetter(log);
            }
        }

        #endregion

        #region IInternalLogger Methods

        public void Debug(object message, Exception exception)
        {
            if (message == null || exception == null)
                return;

            DebugExceptionAction(log, message.ToString(), exception);
        }

        public void Debug(object message)
        {
            if (message == null)
                return;

            DebugAction(log, message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            Debug(String.Format(format, args));
        }

        public void Error(object message, Exception exception)
        {
            if (message == null || exception == null)
                return;

            ErrorExceptionAction(log, message.ToString(), exception);
        }

        public void Error(object message)
        {
            if (message == null)
                return;

            ErrorAction(log, message.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Error(String.Format(format, args));
        }

        public void Fatal(object message, Exception exception)
        {
            if (message == null || exception == null)
                return;

            FatalExceptionAction(log, message.ToString(), exception);
        }

        public void Fatal(object message)
        {
            if (message == null)
                return;

            FatalAction(log, message.ToString());
        }

        public void Info(object message, Exception exception)
        {
            if (message == null || exception == null)
                return;

            InfoExceptionAction(log, message.ToString(), exception);
        }

        public void Info(object message)
        {
            if (message == null)
                return;

            InfoAction(log, message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            Info(String.Format(format, args));
        }

        public void Warn(object message, Exception exception)
        {
            if (message == null || exception == null)
                return;

            WarnExceptionAction(log, message.ToString(), exception);
        }

        public void Warn(object message)
        {
            if (message == null)
                return;

            WarnAction(log, message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            Warn(String.Format(format, args));
        }

        #endregion

        #endregion

        #region Private methods

        private static Func<object, bool> CreatePropertyGetter(string propertyName)
        {
            ParameterExpression paramExpr = Expression.Parameter(typeof(object), "pv");
            Expression convertedExpr = Expression.Convert(paramExpr, LoggerType);
            Expression property = Expression.Property(convertedExpr, propertyName);

            return Expression.Lambda<Func<object, bool>>(property, new[] { paramExpr }).Compile();
        }

        private static Action<object, string> CreateSimpleAction(string methodName)
        {
            MethodInfo methodInfo = GetMethodInfo(methodName, new[] { typeof(string) });
            ParameterExpression instanceParam = Expression.Parameter(typeof(object), "i");
            var converterInstanceParam = Expression.Convert(instanceParam, LoggerType);
            ParameterExpression messageParam = Expression.Parameter(typeof(string), "m");

            MethodCallExpression methodCall = Expression.Call(converterInstanceParam, methodInfo, new Expression[] { messageParam });

            return (Action<object, string>)Expression.Lambda(methodCall, new[] { instanceParam, messageParam }).Compile();
        }

        private static Action<object, string, Exception> CreateExceptionAction(string methodName)
        {
            MethodInfo methodInfo = GetMethodInfo(methodName, new[] { typeof(string), typeof(Exception) });

            ParameterExpression messageParam = Expression.Parameter(typeof(string), "m");
            ParameterExpression instanceParam = Expression.Parameter(typeof(object), "i");
            ParameterExpression exceptionParam = Expression.Parameter(typeof(Exception), "e");
            var convertedParam = Expression.Convert(instanceParam, LoggerType);

            MethodCallExpression methodCall = Expression.Call(convertedParam, methodInfo, new Expression[] { messageParam, exceptionParam });

            return (Action<object, string, Exception>)Expression.Lambda(methodCall, new[] { instanceParam, messageParam, exceptionParam }).Compile();
        }

        private static MethodInfo GetMethodInfo(string methodName, System.Type[] parameters)
        {
            return LoggerType.GetMethod(methodName, parameters);
        }

        #endregion
    }
}
