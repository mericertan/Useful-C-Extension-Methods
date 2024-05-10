using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Useful_CSharp_Extension_Methods.Models;

namespace Useful_CSharp_Extension_Methods
{
    public static class ExpressionExtensions
    {

        public static IQueryable<T> Filter<T>(this IQueryable<T> source, Dictionary<string, object> filters)
        {
            var param = Expression.Parameter(typeof(T), "e");

            Expression body = null;
            foreach (var filter in filters)
            {
                var propExpr = Expression.PropertyOrField(param, filter.Key);
                //var value = filter.Value as object;
                if (filter.Value != null)
                {


                    var valueList = JsonConvert.DeserializeObject<List<FilterObjectValue<object>>>(filter.Value.ToString());

                    foreach (var valueItem in valueList)
                    {
                        var value = valueItem.Value;
                        //var t = value.GetType();
                        if (value != null && value.GetType() != propExpr.Type)
                        {
                            var convertType = propExpr.Type;
                            if (convertType.IsGenericType && convertType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                convertType = convertType.GetGenericArguments()[0];

                            value = convertType == value.GetType()
                                ? value
                                : Convert.ChangeType(value, convertType);

                        }

                        if (value != null)
                        {

                            if (valueItem.Condition == "eq" || string.IsNullOrEmpty(valueItem.Condition))
                            {
                                if (value.GetType() == typeof(DateTime))
                                {
                                    var currDate = (DateTime)value;
                                    var nextDate = currDate.AddDays(1);
                                    var ex1 = Expression.GreaterThanOrEqual(propExpr, Expression.Constant(currDate, propExpr.Type));
                                    var ex2 = Expression.LessThan(propExpr, Expression.Constant(nextDate, propExpr.Type));
                                    var betweenExpr = Expression.And(ex1, ex2);
                                    body = body == null ? betweenExpr : Expression.AndAlso(body, betweenExpr);

                                }
                                else
                                {
                                    var valueExpr = Expression.Constant(value, propExpr.Type);
                                    var equalExpr = Expression.Equal(propExpr, valueExpr);
                                    body = body == null ? equalExpr : Expression.AndAlso(body, equalExpr);
                                }
                            }
                            else if (valueItem.Condition == "gt")
                            {
                                if (value.GetType() == typeof(DateTime))
                                {
                                    value = (DateTime)value;
                                }

                                var valueExpr = Expression.Constant(value, propExpr.Type);
                                var condExpr = Expression.GreaterThanOrEqual(propExpr, valueExpr);
                                body = body == null ? condExpr : Expression.AndAlso(body, condExpr);

                            }
                            else if (valueItem.Condition == "lt")
                            {
                                var condExpr = Expression.LessThanOrEqual(propExpr, Expression.Constant(value, propExpr.Type));
                                if (value.GetType() == typeof(DateTime))
                                {
                                    value = ((DateTime)value).AddDays(1);
                                    condExpr = Expression.LessThan(propExpr, Expression.Constant(value, propExpr.Type));

                                }

                                body = body == null ? condExpr : Expression.AndAlso(body, condExpr);
                            }


                        }
                    }


                }
            }

            if (body == null)
                return source;

            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return source.Where(lambda);
        }

        private static Expression GreaterThanOrEqualExtended(Expression e1, Expression e2)
        {
            if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
                e2 = Expression.Convert(e2, e1.Type);
            else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
                e1 = Expression.Convert(e1, e2.Type);
            return Expression.GreaterThanOrEqual(e1, e2);
        }
        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        private static object GetValue<T>(this MemberExpression member)
        {
            var param = Expression.Parameter(typeof(T), "e");

            var objectMember = Expression.Convert(member, typeof(T));

            var getterLambda = Expression.Lambda<Func<T>>(objectMember, param);

            var getter = getterLambda.Compile();

            return getter();
        }
    }
}
