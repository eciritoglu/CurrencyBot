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
            #region second alternative
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
            #endregion
           
            String Text = getDataFromWebPage("http://www.tcmb.gov.tr/kurlar/today.xml");
            XmlDocument xdoc = createXML(Text);
            String[,]currencyRates=getCurrenyRates(xdoc);
            printCurrency(currencyRates);


        }

        private static String[,] getCurrenyRates(XmlDocument xdoc) {
            String[,] currencyInformation;
            XmlNodeList NodeList =
                xdoc.SelectNodes("Tarih_Date/Currency");
            // ParentNode 
            // 0 date dd.mm.yyyy
            // 1 date mm/dd.yyyy
            // 2 bulletin no

            currencyInformation = new String[NodeList.Count, 5];
            for (int counter = 0; counter < NodeList.Count - 1; counter++)
            {
                currencyInformation[counter, 0] = NodeList[counter].SelectSingleNode("Unit").InnerText;
                currencyInformation[counter, 1] = NodeList[counter].SelectSingleNode("CurrencyName").InnerText;
                currencyInformation[counter, 2] = NodeList[counter].SelectSingleNode("ForexBuying").InnerText;
                currencyInformation[counter, 3] = NodeList[counter].SelectSingleNode("ForexSelling").InnerText;
                currencyInformation[counter, 4] = "Date : "+NodeList[counter].ParentNode.Attributes["Tarih"].Value + " Bulletin Number: " +
                                                                        NodeList[counter].ParentNode.Attributes["Bulten_No"].Value;               
            }
            return currencyInformation;
                   
        }

        private static void printCurrency(String[,] currencyInformation) {
            Console.WriteLine("==============================================================");
            Console.WriteLine("== Latest Update "+currencyInformation[0,4]+"==");
            Console.WriteLine("==============================================================\n");


            for (int counter = 0; counter < currencyInformation.GetLength(0)-1; counter++) {
                Console.WriteLine(" == " + currencyInformation[counter, 0] + "  "+ currencyInformation[counter, 1] + " == ");
                Console.WriteLine("Buying rates : " + currencyInformation[counter, 2]+" TL ");
                Console.WriteLine("Selling rates : " + currencyInformation[counter, 3] + " TL " + "\n");
            }
        }
       

        private static XmlDocument createXML(String text) {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(text);
            return xdoc;            
        }

        private static string getDataFromWebPage(String url){
            String PostUrl = url;
            WebResponse webResponse = WebRequest.Create(PostUrl).GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            String Result = sr.ReadToEnd().Trim();
            return Result;
        }

       
    }
}
