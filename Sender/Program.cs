using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startTime = DateTime.Now;

            Console.WriteLine("Enter target URL (http://172.17.0.1/api/messages/)");
            string targetUrl = Console.ReadLine() ?? "";
            Console.WriteLine($"Your target URL is \"{targetUrl}\"");

            // Open file 0 to read instruction
            Console.WriteLine("Reading 0.json from Data directory");
            var jsonText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Data", "0.json"));
            var metaDataModel = JsonConvert.DeserializeObject<MetaDataModel>(jsonText);

            if (metaDataModel is null) throw new Exception("Can not read 0.json");

            Console.WriteLine("Reading Data directory");
            foreach (var fileNumber in metaDataModel.orders)
            {
                Console.WriteLine(fileNumber);

                try
                {
                    var dataText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Data", $"{fileNumber}.json"));
                    var dataModel = JsonConvert.DeserializeObject<RequestModel>(dataText);

                    if (dataModel is null) throw new Exception($"Can not parse {fileNumber}.json");
                    await SendRequest(targetUrl, dataModel);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"File {fileNumber}.json not found");
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            Console.WriteLine(DateTime.Now - startTime);
        }

        static async Task SendRequest(string baseUrl, RequestModel model)
        {
            while (true)
            {
                IFlurlResponse result;

                switch (model.ActionEnum)
                {
                    case ActionEnum.POST:
                        try
                        {
                            result = await baseUrl.PostJsonAsync(model);
                            return;
                        }
                        catch (FlurlHttpException ex)
                        {
                            if (ex.StatusCode == 409)
                            {
                                return;
                            }

                            Console.WriteLine($"Error ({ex.StatusCode}) {model.Uuid} : {ex.GetResponseStringAsync()}");
                        }
                        break;
                    case ActionEnum.PUT:
                        try
                        {
                            var url = Url.Combine(baseUrl, model.Uuid);
                            result = await url.PutJsonAsync(model);
                        }
                        catch (FlurlHttpException ex)
                        {
                            Console.WriteLine($"Error ({ex.StatusCode}) {model.Uuid} : {ex.GetResponseStringAsync()}");
                        }
                        break;
                    case ActionEnum.DELETE:
                        try
                        {
                            var url = Url.Combine(baseUrl, model.Uuid);
                            result = await url.DeleteAsync();
                        }
                        catch (FlurlHttpException ex)
                        {
                            if (ex.StatusCode == 404)
                            {
                                return;
                            }

                            Console.WriteLine($"Error ({ex.StatusCode}) {model.Uuid} : {ex.GetResponseStringAsync()}");
                        }
                        break;
                }
            }
        }
    }
}