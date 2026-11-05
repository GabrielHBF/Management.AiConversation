using System.Net.Http.Json;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7159")
};

while (true)
{
    Console.Write("Você: ");

    var message = Console.ReadLine();

    if (message == "sair")
        break;

    var response = await httpClient.PostAsJsonAsync(
        "/api/v1/chat/jarvis",
        new { Message = message });

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine("Erro ao enviar mensagem.");
        continue;
    }

    var result = await response.Content.ReadAsStringAsync();

    Console.WriteLine($"Jarvis: {result}");
}