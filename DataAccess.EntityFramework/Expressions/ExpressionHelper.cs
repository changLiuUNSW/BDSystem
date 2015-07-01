using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.EntityFramework.Expressions
{
    /// <summary>
    /// expression help classes
    /// </summary>
    internal static class ExpressionHelper
    {
        public class ExpressionParamRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            public ExpressionParamRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameter(Dictionary<ParameterExpression, ParameterExpression> map,
                Expression expression)
            {
                return new ExpressionParamRebinder(map).Visit(expression);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replace;
                if (_map.TryGetValue(node, out replace))
                {
                    node = replace;
                }

                return base.VisitParameter(node);
            }
        }
    }
}
