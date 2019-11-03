using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ConsoleAppGeneratePDFFile
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {

           
        }

        public static void CallBootstrapper()
        {
            var container = new UnityContainer();
            container.RegisterType<IAuthor, Author>();
        }
    }
}
