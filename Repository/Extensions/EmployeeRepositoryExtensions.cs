using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    internal static class EmployeeRepositoryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="minAge"></param>
        /// <param name="maxAge"></param>
        /// <returns></returns>
        public async static  Task<IQueryable<Employee>> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        {
            return employees.Where(item =>  item.Age >= minAge && item.Age <= maxAge);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async static Task<IQueryable<Employee>> SearchByName(this IQueryable<Employee> employees, String pattern)
        {
            if(String.IsNullOrWhiteSpace(pattern))
            {
                return employees;
            }
            else
            {
                var searchTerm = pattern.ToLower();
                return employees.Where(item =>  item.Name.ToLower().Contains(searchTerm)); //! comment on test les nulls ?
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="orderByQueryString"></param>
        /// <returns></returns>
        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.GetOrderBy<Employee>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);
            return employees.OrderBy(orderQuery);
        }
    }
}
