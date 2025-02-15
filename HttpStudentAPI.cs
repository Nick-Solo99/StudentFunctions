using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using StudentFunctions.Models.School;

namespace StudentFunctions;

public class HttpStudentAPI
{
    private readonly ILogger<HttpStudentAPI> _logger;
    private readonly SchoolContext _context;

    public HttpStudentAPI(ILogger<HttpStudentAPI> logger, SchoolContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Function("Welcome")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }

    [Function("GetStudents")]
    public HttpResponseData GetStudents([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP GET function processed a request in GetStudents().");

        var students = _context.Students.ToArray();
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        
        response.WriteStringAsync(JsonConvert.SerializeObject(students));
        
        return response;
    }

    [Function("GetStudentById")]
    public HttpResponseData GetStudentById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "students/{id}")] HttpRequestData req, int id)
    {
        _logger.LogInformation("C# HTTP GET function processed a request.");
        var student = _context.Students.FindAsync(id).Result;
        if (student == null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteStringAsync("Not Found");
            return response;
        }
        var response2 = req.CreateResponse(HttpStatusCode.OK);
        response2.Headers.Add("Content-Type", "application/json");
        response2.WriteStringAsync(JsonConvert.SerializeObject(student));
        return response2;
    }

    [Function("UpdateStudent")]
    public HttpResponseData UpdateStudent([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "students/{id}")] HttpRequestData req, int id)
    {
        _logger.LogInformation("C# HTTP PUT function processed a request.");
        var student = _context.Students.FindAsync(id).Result;
        if (student == null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteStringAsync("Not Found");
            return response;
        }
        var student2 = JsonConvert.DeserializeObject<Student>(req.ReadAsStringAsync().Result);
        student.FirstName = student2.FirstName;
        student.LastName = student2.LastName;
        student.School = student2.School;
        _context.SaveChanges();
        var response2 = req.CreateResponse(HttpStatusCode.OK);
        response2.Headers.Add("Content-Type", "application/json");
        response2.WriteStringAsync(JsonConvert.SerializeObject(student2));
        return response2;
    }

    [Function("DeleteStudent")]
    public HttpResponseData DeleteStudent([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "students/{id}")] HttpRequestData req,
        int id)
    {
        _logger.LogInformation("C# HTTP DELETE function processed a request.");
        var student = _context.Students.FindAsync(id).Result;
        if (student == null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteStringAsync("Not Found");
            return response;
        }
        _context.Students.Remove(student);
        _context.SaveChanges();
        var response2 = req.CreateResponse(HttpStatusCode.OK);
        response2.Headers.Add("Content-Type", "application/json");
        response2.WriteStringAsync(JsonConvert.SerializeObject(student));
        return response2;
    }
    
    

}