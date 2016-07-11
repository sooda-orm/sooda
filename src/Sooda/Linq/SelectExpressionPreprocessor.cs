using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sooda.Linq
{
    class SelectExpressionPreprocessor : ExpressionVisitor
    {
        [ThreadStatic]
        static Dictionary<Tuple<Type, string>, bool> _translations;

        private readonly SoodaQueryExecutor _executor;

        public SelectExpressionPreprocessor(SoodaQueryExecutor executor)
        {
            if (_translations == null)
                _translations = new Dictionary<Tuple<Type, string>, bool>();
            
            _executor = executor;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            MemberInfo current = node.Member;

            if (node.Expression == null)                // skip members in static objects
                return base.VisitMember(node);

            if (current.DeclaringType == null)
                return base.VisitMember(node);

            if (!typeof(SoodaObject).IsAssignableFrom(node.Expression.Type))   // skip members outside SoodaObject's
                return base.VisitMember(node);

            if (current.DeclaringType.Name.EndsWith("_Stub"))  // skip members declared in sooda stubs
                return base.VisitMember(node);

            // check for <member>Expression for processed member
            Tuple<Type, string> key = new Tuple<Type, string>(node.Expression.Type, current.Name);

            bool hasTranslation;
            bool foundInfo = _translations.TryGetValue(key, out hasTranslation);
            if (foundInfo && !hasTranslation) // we know that there are no expression
                return base.VisitMember(node);

            // check translation
            Expression expr = _executor.TranslateUnknownMember(node);
            if (!foundInfo)
                _translations.Add(key, expr != null);

            if (expr == null) // no translation
                return base.VisitMember(node);

            return base.Visit(expr);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            MethodInfo current = node.Method;

            if (current.DeclaringType == null || current.DeclaringType.FullName.StartsWith("System."))
                return base.VisitMethodCall(node);          // skip processing system methods (linq, etc)

            Type inType = node.Object != null ? node.Object.Type : node.Method.DeclaringType;

            string signature = string.Format("{0}({1})", current.Name, string.Join(", ", current.GetParameters().Select(p => p.ParameterType.Name)));
            
            Tuple<Type, string> key = new Tuple<Type, string>(inType, signature);

            bool hasTranslation;
            bool foundInfo = _translations.TryGetValue(key, out hasTranslation);
            if (foundInfo && !hasTranslation) // we know that there are no expression
                return base.VisitMethodCall(node);

            // check translation
            Expression expr = _executor.TranslateUnknownMethod(node);
            if (!foundInfo)
                _translations.Add(key, expr != null);

            if (expr == null) // no translation
                return base.VisitMethodCall(node);

            return base.Visit(expr);
        }
    }
}