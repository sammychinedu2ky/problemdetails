using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IProblemDetailsWriter, CustomWriter>();
//builder.Services.AddProblemDetails();


var app = builder.Build();


//app.UseDeveloperExceptionPage();


app.MapGet("/", async (HttpContext context) =>
{

    if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
    {

        await problemDetailsService.WriteAsync(new ProblemDetailsContext { HttpContext = context });
    }


    await context.Response.WriteAsync("Hello World");
});


app.Run();


public class CustomWriter : IProblemDetailsWriter
{
    // Indicates that only responses with StatusCode == 400
    // will be handled by this writer. All others will be
    // handled by different registered writers if available.
    public bool CanWrite(ProblemDetailsContext context)
        => context.HttpContext.Response.StatusCode == 400;

    public Task WriteAsync(ProblemDetailsContext context)
    {
        //Additional customizations

        // Write to the response
        context.HttpContext.Response.WriteAsJsonAsync(context.ProblemDetails);
        return Task.CompletedTask;
    }

    ValueTask IProblemDetailsWriter.WriteAsync(ProblemDetailsContext context)
    {
        throw new NotImplementedException();
    }
}



