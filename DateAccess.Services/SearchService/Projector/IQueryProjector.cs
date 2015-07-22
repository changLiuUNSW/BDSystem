using System;
using System.Linq.Expressions;

namespace DateAccess.Services.SearchService.Projector
{
    /// <summary>
    /// transform the search return type by projecting the search type from source to target
    /// </summary>
    public interface IQueryProjector
    {
        Expression<Func<T, TResult>> CreateExpression<T, TResult>() where T : class where TResult : class;
    }
}
