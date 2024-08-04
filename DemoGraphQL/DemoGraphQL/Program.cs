using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoGraphQL
{
    internal class Program
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            GithubGraphQL.GithubGraphQlDemo();

            Console.ReadLine();
        }


    }
}
