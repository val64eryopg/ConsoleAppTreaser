using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAppTreaser
{
    internal class Task1
    {
       static Regex regex = new Regex(@"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");
        static Regex regex2 = new Regex(@"www.\w+.(com|net|ru|tv)");

        public static void Main(string[] args) {


            List<string> gettable = new List<string> {"Nomber   ip   as  country" };

            Console.WriteLine("Введите ip или адрес");
        
            string? input = Console.ReadLine();
            Ping ping = new Ping();
            String send = "";

            if (input != null)
            {
                MatchCollection matches = regex.Matches(input);
                if (matches.Count > 0)
                {
                    send = matches[0].ToString();
                } else 
                {
                    MatchCollection  matches2 = regex2.Matches(input);
                    if (matches2.Count > 0)
                    { 
                        send = matches2[0].ToString();  
                    }
                    else 
                    {
                        Console.WriteLine("Вывод  неверный");
                    }
                } 
            }

            if (send != "")
            {
                
                PingReply reply = ping.Send(send);

                IEnumerable<IPAddress> answeres = GetTraceRoute(reply.Address.ToString());
                Console.WriteLine("присвоение произошло подождите");
                List<WindolOfTable> answeresList = new List<WindolOfTable>();


                if (answeres != null)
                {
                    int Nomber = -1;
                    foreach (IPAddress ipAdress in answeres)
                    {                        
                        string str = getCountry("https://ipinfo.io/" + ipAdress + "/json");                       
                        string[] end = str.Split(' ');
                       

          
                        String[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if ((words != null)&&(words.Length >= 2)){                          
                            gettable.Add(Nomber+" "+ipAdress + " " + words[3].Trim(new Char[] { ' ', '"', ',' }) + " " + words[1].Trim(new Char[] { ' ', '"', ',' }));

                        }

                        
                        Nomber++ ;
                        
                    }
                    
                }
                else
                {
                    Console.WriteLine("Несуществующий адрес");
                }
                foreach (string a in gettable){ 
                    Console.WriteLine(a);
                }

            }
            else 
            {
                return;   
            }

           

        }

     


        static string getCountry(string url) {
            string country = "";
            string As = "";
            string input = getResponse(url);
            string[] liststr = input.Split("\n");

            if (liststr.Length != 0)
            {
                foreach (string i in liststr)
                {
                    Regex regex1 = new Regex(@"org");
                    Regex regex2 = new Regex(@"country");

                    if (regex2.Match(i).Success) { 
                        country = i;
                    }else if (regex1.Match(i).Success)
                            { 
                                
                               country = country + " " +i;
                            }
                }
            }
            else {
                Console.WriteLine("пусто");
            }
            //Console.WriteLine(input);
            //Console.WriteLine(liststr);
              
            
           return country;
        }

        static string getResponse(string uri)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = resStream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            }
            while (count > 0);
            return sb.ToString();
        }



 

       
      

        static IEnumerable<IPAddress> GetTraceRoute(string hostname)
        {
       
            const int timeout = 10000;
            const int maxTTL = 30;
            const int bufferSize = 32;

            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);

            using (var pinger = new Ping())
            {
                for (int ttl = 1; ttl <= maxTTL; ttl++)
                {
                    PingOptions options = new PingOptions(ttl, true);
                    PingReply reply = pinger.Send(hostname, timeout, buffer, options);

                    
                    if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TtlExpired)
                        yield return reply.Address;

                    
                    if (reply.Status != IPStatus.TtlExpired && reply.Status != IPStatus.TimedOut)
                        break;
                }
            }
        }
    }
}
