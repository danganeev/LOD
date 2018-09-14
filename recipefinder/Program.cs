using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace recipefinder
{
    public class RootObject
    {
        public string title { get; set; }
        public string version { get; set; }
        public string href { get; set; }
        public List<Result> results { get; set; }
    }

    public class Result
    {
        public string title { get; set; }
        public string href { get; set; }
        public string ingredients { get; set; }
        public string thumbnail { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Please include some ingredients!");
            }
            else
            {
                String ings = "";
                for (int i = 0; i < args.Length-1; i++)
                {
                    ings = ings + args[i] + ",";
                }
                ings = ings +args[args.Length - 1];

               String url = "http://www.recipepuppy.com/api/?i=" + ings + "&p=" + 1;

                System.Net.WebClient wc = new System.Net.WebClient();

                var json = wc.DownloadString(url);

                RootObject item = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);

                int MinNoIng = item.results[0].ingredients.Split(',').Length;
                string SimplestMealTitle = item.results[0].title;
                string SimplestMealIngredients = item.results[0].ingredients;
                string SimplestMealHref = item.results[0].href;

                int count = 2;
                int pagenumber = 2;


                while (count<11)
                {
                    
                    
                    try
                    {
                        url = "http://www.recipepuppy.com/api/?i=" + ings + "&p=" + pagenumber;

                        wc = new System.Net.WebClient();

                        json = wc.DownloadString(url);

                        item = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);

                        foreach (Result r in item.results)
                        {
                            if (r.ingredients.Split(',').Length < MinNoIng)
                            {
                                MinNoIng = r.ingredients.Split(',').Length;
                                SimplestMealTitle = r.title;
                                SimplestMealIngredients = r.ingredients;
                                SimplestMealHref = r.href;
                            }
                        }

                        count++;
                        pagenumber++;
                    }
                    catch (WebException wex)
                    {
                        pagenumber++;
                        continue;
                       
                    }
                }

                Console.WriteLine("Simplest recipe for requested ingredients is "+SimplestMealTitle);
                Console.WriteLine("It contains only these ingredients: '"+SimplestMealIngredients+"'");
                Console.WriteLine("Link: " + SimplestMealHref);

                Console.ReadKey();
            }
        }
    }
}
