using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleRESTClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                //Go get the data
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                //adding json
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //global HttpResponseMessage
                HttpResponseMessage response;

                Console.WriteLine("MÉTODO GET");
                Console.WriteLine("");
                Console.WriteLine("Carregando...");
                Console.WriteLine("");

                response = await client.GetAsync("user/123");

                if (response.IsSuccessStatusCode)
                {
                    Person person = await response.Content.ReadAsAsync<Person>();
                    Console.WriteLine("DADOS GERAIS");
                    Console.WriteLine("");
                    Console.WriteLine("Name: " + person.name);
                    Console.WriteLine("Street: " + person.street);
                    Console.WriteLine("City: " + person.city);
                    Console.WriteLine("Zip: " + person.zipcode);
                }

                Console.WriteLine("");
                Console.WriteLine("EXPERIÊNCIAS");
                Console.WriteLine("");

                response = await client.GetAsync("experiences");

                if (response.IsSuccessStatusCode)
                {
                    List<Experiences> experiences = await response.Content.ReadAsAsync<List<Experiences>>();
                    for (int i = 0; i < experiences.Count; i++)
                    {
                        Console.WriteLine("Name: " + experiences[i].companyid);
                        Console.WriteLine("Street: " + experiences[i].companyname);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Usuário carregado com sucesso!");
                Console.WriteLine("");

                Console.WriteLine("MÉTODO POST");
                Console.WriteLine("");

                Console.WriteLine("ADICIONAR EXPERIÊNCIA");
                Console.WriteLine("");

                Console.Write("Company Id: ");
                var companyId = Console.ReadLine();
                Console.Write("Company Name: ");
                var companyName = Console.ReadLine();

                Experiences newExperience = new Experiences();
                newExperience.companyid = int.Parse(companyId);
                newExperience.companyname = companyName;

                response = await client.PostAsJsonAsync("experiences", newExperience);

                Console.WriteLine(response);

                if (response.IsSuccessStatusCode)
                {
                    Uri experienceUrl = response.Headers.Location;
                    Console.WriteLine(experienceUrl);
                    Console.WriteLine("Deu certo");
                }

                Console.ReadKey();
            }
        }
    }
}
