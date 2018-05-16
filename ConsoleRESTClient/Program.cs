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
            string response = "";

            do
            {
                Console.WriteLine("");
                Console.WriteLine("1 - Add, 2 - Update, 3 - Delete, 4 - Select 5 - Exit(yes) \n");
                Console.Write("Digite a opção: ");

                var option = Console.ReadLine();
                Console.WriteLine("");
                switch (option)
                {
                    case "1":
                        PostAsync().Wait();
                        break;
                    case "2":
                        UpdateAsync().Wait();
                        break;
                    case "3":
                        DeleteAsync().Wait();
                        break;
                    case "4":
                        GetAsync().Wait();
                        break;
                    case "yes":
                        Logout();
                        break;
                    default:
                        Main(null);
                        break;
                }
                //Console.ReadLine();
            } while (1 > 0);
        }

        public static void Logout()
        {
            Console.WriteLine("Até logo... pressione qualquer tecla para sair...");
            Console.ReadKey();
            System.Environment.Exit(0);
        }


        static async Task PostAsync()
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
                
                Console.WriteLine("MÉTODO POST");
                Console.WriteLine("");

                Console.WriteLine("ADICIONAR EXPERIÊNCIA");
                Console.WriteLine("");

                Console.Write("Company Name: ");
                var companyName = Console.ReadLine();

                Experiences newExperience = new Experiences();
                newExperience.companyname = companyName;

                response = await client.PostAsJsonAsync("experiences", newExperience);

                if (response.IsSuccessStatusCode)
                {
                    Uri experienceUrl = response.Headers.Location;
                    //Console.WriteLine(experienceUrl);
                    Console.WriteLine("");
                    Console.WriteLine(companyName + " adicionada com sucesso");
                }
            }
        }

        static async Task UpdateAsync()
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

                Console.WriteLine("MÉTODO UPDATE");
                Console.WriteLine("");
                Console.Write("Informe o ID para fazer a alteração: ");
                var companyCheckId = Console.ReadLine();

                Console.WriteLine("Carregando...");
                Console.WriteLine("");

                response = await client.GetAsync("experiences/" + companyCheckId);

                if (response.IsSuccessStatusCode)
                {
                    Experiences showExperiencebyId = await response.Content.ReadAsAsync<Experiences>();
                    Console.WriteLine("Nome da Compania: " + showExperiencebyId.companyname);
                }
                
                Console.Write("Informe o novo nome: ");
                var companyUpdateName = Console.ReadLine();

                Experiences newExperience = new Experiences();

                newExperience.companyname = companyUpdateName;
                response = await client.PutAsJsonAsync("experiences/" + companyCheckId, newExperience);

                if (response.IsSuccessStatusCode)
                {
                    Uri experienceUrl = response.Headers.Location;
                    //    //Console.WriteLine(experienceUrl);
                    Console.WriteLine("");
                    Console.WriteLine(companyUpdateName+" atualizado(a) com sucesso");
                }
            }

        }

        static async Task DeleteAsync()
        {

        }

        static async Task GetAsync()
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
                        Console.WriteLine("Company id: " + experiences[i].id);
                        Console.WriteLine("Company Name: " + experiences[i].companyname);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Usuário carregado com sucesso!");
            }
        }
    }
}