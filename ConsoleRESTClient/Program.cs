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

                // ################# START GET SINGLE USER ################# 

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

                // ################# END GET SINGLE USER ################# 

                // ################# START GET ALL EXPERIENCES ################# 

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
                Console.WriteLine("");

                // ################# END GET ALL EXPERIENCES ################# 

                // ################# START POST METHOD ################# 

                Console.WriteLine("MÉTODO POST");
                Console.WriteLine("");

                Console.WriteLine("ADICIONAR EXPERIÊNCIA");
                Console.WriteLine("");

                Console.Write("Company Id: ");
                var companyId = Console.ReadLine();
                Console.Write("Company Name: ");
                var companyName = Console.ReadLine();

                Experiences newExperience = new Experiences();
                newExperience.id = int.Parse(companyId);
                newExperience.companyname = companyName;

                response = await client.PostAsJsonAsync("experiences", newExperience);

                Console.WriteLine(response);

                if (response.IsSuccessStatusCode)
                {
                    Uri experienceUrl = response.Headers.Location;
                    //Console.WriteLine(experienceUrl);
                    Console.WriteLine("Deu certo");
                }

                // ################# END POST METHOD ################# 

                // ################# START UPDATE METHOD ################# 

                Console.WriteLine("");
                Console.WriteLine("MÉTODO UPDATE");
                Console.WriteLine("");
                Console.WriteLine("Informe o ID para fazer a alteração");
                Console.Write("Company Id: ");
                var companyCheckId = Console.ReadLine();

                response = await client.GetAsync("experiences/"+companyCheckId);

                if (response.IsSuccessStatusCode)
                {
                    Experiences showExperiencebyId = await response.Content.ReadAsAsync<Experiences>();
                    Console.WriteLine("Nome da Compania: " + showExperiencebyId.companyname);
                }

                Console.Write("Informe o novo nome: ");
                var companyUpdateName = Console.ReadLine();

                newExperience.companyname = companyUpdateName;
                response = await client.PutAsJsonAsync("experiences/" + companyCheckId, newExperience);

                if (response.IsSuccessStatusCode)
                {
                    Uri experienceUrl = response.Headers.Location;
                    //    //Console.WriteLine(experienceUrl);
                    Console.WriteLine("Experiência atualizada com sucesso");
                }

                // ################# END UPDATE METHOD ################# 

                Console.ReadKey();
            }
        }
    }
}