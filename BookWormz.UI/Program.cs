using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            //APITest.GetDataWithAuthentication();

            ProgramUI program = new ProgramUI();
            program.Start();
        }
    }
}
