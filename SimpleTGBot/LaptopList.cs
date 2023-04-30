using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleTGBot
{
    public class LaptopList
    {
        public static List<Laptop> GetLaptopList(string path)
        {
            List<Laptop> list = new List<Laptop>();
            using (StreamReader sr = File.OpenText(path))
            {
                string line = sr.ReadLine();

                while (line != null)
                {
                    line = sr.ReadLine();

                    if (line != null)
                    {
                        var m = Regex.Match(line, @"^(?<=^)(.*?)(?=,),(?<=,)(.*?)(?=,),(?<=,)(.*?)(?=,),""(?<="")(.*?)(?="""""")"""""",(?<=,)(.*?)(?=,),(?<=,)(.*?)(?=,),(?<=,)(.*?)(?=GB,)GB,(?<=,)(.*?)(?=,)(?=,),(?<=,)(.*?)(?=,),(?<=,)(.*?)(?=,),(?<=,)(.*?)(?=,),(?<=,)(.*?)(?=kg,)kg,(?<=,)(.*?)(?=$)");
                        var laptop = new Laptop(brand: m.Groups[1].Value, model_name: m.Groups[2].Value, category: m.Groups[3].Value, screen_size: double.Parse(m.Groups[4].Value.Replace('.', ',')), screen: m.Groups[5].Value, cpu: m.Groups[6].Value, ram: m.Groups[7].Value, storage: m.Groups[8].Value, video_card: m.Groups[9].Value, operating_system: m.Groups[10].Value, operating_system_version: m.Groups[11].Value, weight: double.Parse(m.Groups[12].Value.Replace('.', ',')), price: double.Parse(m.Groups[13].Value.Replace('.', ',')));

                        if (line != null)
                        {
                            list.Add(laptop);
                        }

                    }




                }
            }
            return list;
        }


        public static List<Laptop> Top5SortList(List<Laptop> laptop, string field)
        {

            switch (field.ToLower())
            {
                case "названию":
                    return laptop.OrderBy(x => x.brand.ToLower()).ThenBy(x => x.model_name.ToLower()).Take(5).ToList();

                case "категории":
                    return laptop.OrderBy(x => x.category.ToLower()).Take(5).ToList();
                case "диагонали":
                    return laptop.OrderBy(x => x.category.ToLower()).Take(5).ToList();
                case "оперативной памяти":
                    return laptop.OrderBy(x => x.ram).Take(5).ToList();
                case "весу":
                    return laptop.OrderBy(x => x.weight).Take(5).ToList();
                case "цене":
                    return laptop.OrderBy(x => x.price).Take(5).ToList();

                default:
                    return laptop.OrderBy(x => x.brand.ToLower()).ThenBy(x => x.model_name.ToLower()).Take(5).ToList();

            }


        }
        public static HashSet<string> HashSetWithCategories(List<Laptop> laptopList, string sort)
        {
            HashSet<string> filteredLaptopList = new HashSet<string>();
            

            foreach (var item in laptopList)
            {


                switch (sort.ToLower())
                {
                    case "по бренду":
                        filteredLaptopList.Add(item.brand);
                        break;
                    case "по категории":
                        filteredLaptopList.Add(item.category);
                        break;
                    case "по размеру экрана":
                        filteredLaptopList.Add(item.screen_size.ToString());
                        break;
                    case "по размеру оперативной памяти":
                        filteredLaptopList.Add(item.ram.ToString());
                        break;
                    case "по памяти":
                        filteredLaptopList.Add(item.storage);
                        break;
                    case "по весу":
                        filteredLaptopList.Add(item.weight.ToString());
                        break;

                    case "по цене":
                        filteredLaptopList.Add(item.price.ToString());
                        break;


                    default:
                        break;
                }


            }
            switch (sort.ToLower())
            {
                case "по бренду":
                    return filteredLaptopList.OrderBy(x=>x).ToHashSet();
                   
                case "по категории":
                    return filteredLaptopList.OrderBy(x => x).ToHashSet();
                 
                case "по размеру экрана":
                    return filteredLaptopList.OrderBy(x => double.Parse(x)).ToHashSet();
                   
                case "по размеру оперативной памяти":
                    return filteredLaptopList.OrderBy(x => double.Parse(x)).ToHashSet();
                  
                case "по памяти":
                    return filteredLaptopList.OrderBy(x => x).ToHashSet();
                   
                case "по весу":
                     return filteredLaptopList.OrderBy(x => double.Parse(x)).ToHashSet();
                   

                case "по цене":
                   return filteredLaptopList.OrderBy(x => double.Parse(x)).ToHashSet();
                   

                  default:
                    return filteredLaptopList.OrderBy(x => x).ToHashSet();
                   
            }
            
        }
        public static List<Laptop> ListForSort(List<Laptop> laptopList, string sort,string message)
        {
            var list = new List<Laptop>();
            
            var newMessage = message.Replace(" ","").Replace(".",",");
            //var m = Regex.Match(newMessage, @"(\S*)(?=-)-(\S*)");
            var t = newMessage.Split('-');
            double max = double.Parse(t[1], System.Globalization.CultureInfo.InvariantCulture);
            double min = double.Parse(t[0], System.Globalization.CultureInfo.InvariantCulture);

            Console.WriteLine($"{max},{min}");
            Console.WriteLine(sort);
            switch (sort.ToLower())
                {
                    
                    case "по размеру экрана":
                    
                   
                    return laptopList.Where(x=> x.screen_size > min ).Where(x => x.screen_size< max).Take(10).ToList();
               
                    case "по размеру оперативной памяти":
                    return laptopList.Where(x => double.Parse(x.ram) > min && double.Parse(x.ram) < max).Take(10).ToList();
                   
                   
                    case "по весу":
                    return laptopList.Where(x => x.weight > min && x.weight < max).Take(10).ToList();
                   

                    case "по цене":
                    return laptopList.Where(x => x.price > min && x.price < max).Take(10).ToList();
         


                    default:
                    return laptopList.Where(x => x.screen_size > min && x.screen_size < max).Take(10).ToList();
               
                


            }

        }

    }
}