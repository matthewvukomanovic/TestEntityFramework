using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using TestFramework.Entities;

namespace TestFramework
{
    public static class Program
    {
        public const string ConnectionString = "Data Source=(local);Initial Catalog=Test;Integrated Security=True;Application Name=EntityFramework Reverse POCO Generator";
        public static int Main()
        {
            var context = new MyDbContext(ConnectionString);

            var testExpressions = new List<Expression<Func<TestTable, bool>>>();
            Expression<Func<TestTable, bool>> isActive = t => t.IsActive;
            testExpressions.Add(isActive);
            testExpressions.Add(GetIsActiveExpression<TestTable>());
            testExpressions.Add(GetIsActiveExpression<TestTable>().Expand());
            testExpressions.Add(GetIsActiveExpression2<TestTable>());
            testExpressions.Add(GetIsActiveExpression2<TestTable>().Expand());

            var anyBroke = false;

            foreach (var testExpression in testExpressions)
            {
                try
                {
                    Console.WriteLine("Running " + testExpression.ToString());
                    // This one works
                    var result = context.TestTable.AsExpandable().Where(t => testExpression.Invoke(t)).ToList();

                }
                catch (Exception exc)
                {
                    Console.WriteLine("ERROR: " + exc.Message);
                    //swallow
                    anyBroke = true;
                }
            }

            if (anyBroke)
            {
                Console.WriteLine("Waiting for key press...");
                Console.ReadKey();
                return 1;
            }

            return 0;
        }

        static Expression<Func<T, bool>> GetIsActiveExpression<T>() where T : class, TestFramework.Interfaces.IIsActive
        {
            return t => t.IsActive;
        }

        static Expression<Func<T, bool>> GetIsActiveExpression2<T>() where T : TestFramework.Interfaces.IIsActive
        {
            return t => t.IsActive;
        }

    }
}
