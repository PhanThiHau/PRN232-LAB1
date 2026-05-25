using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace PRN232.LMS.Services.Helpers
{
    public static class QueryHelper
    {
        public static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string? sortString)
        {
            if (string.IsNullOrWhiteSpace(sortString))
            {
                return query;
            }

            var sortFields = sortString.Split(',', StringSplitOptions.RemoveEmptyEntries);
            bool isFirstSort = true;

            foreach (var field in sortFields)
            {
                var trimmedField = field.Trim();
                bool descending = trimmedField.StartsWith('-');
                var propertyName = descending ? trimmedField.Substring(1) : trimmedField;

                var property = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                {
                    continue;
                }

                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, property);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);

                string methodName;
                if (isFirstSort)
                {
                    methodName = descending ? "OrderByDescending" : "OrderBy";
                    isFirstSort = false;
                }
                else
                {
                    methodName = descending ? "ThenByDescending" : "ThenBy";
                }

                var resultExpression = Expression.Call(
                    typeof(Queryable),
                    methodName,
                    new[] { typeof(T), property.PropertyType },
                    query.Expression,
                    Expression.Quote(orderExpression));

                query = query.Provider.CreateQuery<T>(resultExpression);
            }

            return query;
        }

        public static IQueryable<T> ApplyExpansion<T>(IQueryable<T> query, string? expandString) where T : class
        {
            if (string.IsNullOrWhiteSpace(expandString))
            {
                return query;
            }

            var expandFields = expandString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var field in expandFields)
            {
                var trimmedField = field.Trim();
                var property = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => p.Name.Equals(trimmedField, StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {
                    query = query.Include(property.Name);
                }
            }

            return query;
        }

        public static List<object> ApplyFieldSelection<T>(List<T> items, string? fieldsString)
        {
            if (string.IsNullOrWhiteSpace(fieldsString))
            {
                return items.Cast<object>().ToList();
            }

            var requestedFields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.Trim())
                .ToList();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => requestedFields.Any(rf => rf.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (properties.Count == 0)
            {
                return items.Cast<object>().ToList();
            }

            var result = new List<object>();
            foreach (var item in items)
            {
                var dict = new Dictionary<string, object?>();
                foreach (var prop in properties)
                {
                    var camelCaseName = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);
                    dict[camelCaseName] = prop.GetValue(item);
                }
                result.Add(dict);
            }

            return result;
        }
    }
}
