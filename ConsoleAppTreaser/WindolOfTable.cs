using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTreaser
{
    internal class WindolOfTable 
    {
        int Nomber;
        string IpAdreess;
        string As;
        string Country;

        public WindolOfTable(int Nomber,string IpAdrees, string As,string Country) { 
            this.Nomber = Nomber;
            this.IpAdreess = IpAdrees;
            this.As = As;   
            this.Country = Country;
        }

        public void Print() { 
            Console.WriteLine(Nomber.ToString(),IpAdreess,As,Country);
        }
    }
}
