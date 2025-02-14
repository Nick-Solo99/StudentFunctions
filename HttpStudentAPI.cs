using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentFunctions;

public class HttpStudentAPI
{
    private readonly ILogger<HttpStudentAPI> _logger;

    public HttpStudentAPI(ILogger<HttpStudentAPI> logger)
    {
        _logger = logger;
    }

    [Function("Welcome")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }

}