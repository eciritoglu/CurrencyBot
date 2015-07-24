using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace CurrencyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Uri url = new Uri("http://www.tcmb.gov.tr/kurlar/today.xml");

            WebClient client = new WebClient()
            {
                Encoding = System.Text.Encoding.UTF8
                // Incase of character problems
            };

            //Adding Clients Informations
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-GB; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12");
            client.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");

            //Downloading contexts from url variable as a String
             String html = client.DownloadString(url);

            //We are using htmlagilitypack for parsing html
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            //loading html to documents for parsing operation
            document.LoadHtml(html);

            HtmlNodeCollection contents = document.DocumentNode.SelectNodes("//*[@id=\"kurlarContainer\"]/table[1]/tbody/tr[2]");
                                                                                
            for (int counter = 0; counter < contents.Count; counter++) {

                String datas = contents[counter].InnerText;
                Console.WriteLine(datas);

            }

             */
            //First buy Second sell
            String[,] moneys;  

            String PostUrl = "http://www.tcmb.gov.tr/kurlar/today.xml";
            WebResponse webResponse = WebRequest.Create(PostUrl).GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            String Result = sr.ReadToEnd().Trim();

            XmlDocument xdoc = new XmlDocument(); 
            xdoc.LoadXml(Result);
            
            XmlNodeList NodeList =
                xdoc.SelectNodes("Tarih_Date/Currency");

            moneys = new String[NodeList.Count, 3];
            for (int counter=0; counter<NodeList.Count-1;counter++)
            {
                moneys[counter,0] = NodeList[counter].SelectSingleNode("CurrencyName").InnerText;
                moneys[counter,1] = NodeList[counter].SelectSingleNode("ForexBuying").InnerText;
                moneys[counter,2] = NodeList[counter].SelectSingleNode("ForexSelling").InnerText;
                
                Console.WriteLine("Currency: "+moneys[counter, 0]);
                Console.WriteLine("Buying rates : " + moneys[counter, 1]);
                Console.WriteLine("Selling rates : " + moneys[counter, 2]+"\n");       

            }


        }
    }
}
