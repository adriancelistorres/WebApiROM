namespace WebApiRestNetCore
{
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using System.Net.Http;
    using WebApiRestNetCore.Services.DataAccess;
    using Newtonsoft.Json;
    using WebApiRestNetCore.Models;

    namespace GoogleSheetsAPI
    {
        public class MyBackgroundService : BackgroundService
        {
            //private readonly DataProcessingService _dataProcessingService;


            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IServiceProvider _services;
            private readonly DataAcecss _dataAccess;

            public MyBackgroundService(IHttpClientFactory httpClientFactory, IServiceProvider services, DataAcecss dataAccess) //DataProcessingService dataProcessingService)
            {
                _httpClientFactory = httpClientFactory;
                _services = services;
                _dataAccess = dataAccess;

                //_dataProcessingService = dataProcessingService;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    using (var scope = _services.CreateScope())
                    {
                        try
                        {
                            // Obtén una instancia del cliente HttpClient desde el factory
                            var httpClient = _httpClientFactory.CreateClient();

                            // Realiza la llamada al endpoint GET de tu API
                            var response = await httpClient.GetAsync("https://localhost:44371/api/Items");

                            // Aquí puedes procesar la respuesta si es necesario
                            if (response.IsSuccessStatusCode)
                            {
                                // La respuesta fue exitosa
                                var jsonres = await response.Content.ReadAsStringAsync();
                                Console.WriteLine(jsonres);

                                //var itemsArray = JsonConvert.DeserializeObject<AperturaPDV[]>(jsonres);
                                //Console.WriteLine(itemsArray);

                                var items = JsonConvert.DeserializeObject<List<AperturaPDV>>(jsonres);
                                Console.WriteLine(items);
                                _dataAccess.InsertData(items);


                            }
                            else
                            {
                                Console.WriteLine("ERROR EN EL BACKGROUND");

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ocurrió un error: {ex.Message}");
                        }

                        // Espera 1 minutos antes de realizar la próxima llamada
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                    }
                }
            }
        }
    }

}
