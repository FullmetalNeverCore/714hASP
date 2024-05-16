using MikoshiASP.Controllers.Structures;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // Use Console logging
    loggingBuilder.AddDebug();   // Optionally add debug output
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string logo = @"

                                                                                                     
                  ,,                              ,,          ,,                                     
`7MMM.     ,MMF'  db  `7MM                      `7MM          db        db       .M""""""bgd `7MM""""""Mq. 
  MMMb    dPMM          MM                        MM                   ;MM:     ,MI    ""Y   MM   `MM.
  M YM   ,M MM  `7MM    MM  ,MP',pW""Wq.  ,pP""Ybd  MMpMMMb.  `7MM      ,V^MM.    `MMb.       MM   ,M9 
  M  Mb  M' MM    MM    MM ;Y  6W'   `Wb 8I   `""  MM    MM    MM     ,M  `MM      `YMMNq.   MMmmdM9  
  M  YM.P'  MM    MM    MM;Mm  8M     M8 `YMMMa.  MM    MM    MM     AbmmmqMA   .     `MM   MM       
  M  `YM'   MM    MM    MM `Mb.YA.   ,A9 L.   I8  MM    MM    MM    A'     VML  Mb     dM   MM       
.JML. `'  .JMML..JMML..JMML. YA.`Ybmd9'  M9mmmP'.JMML  JMML..JMML..AMA.   .AMMA.P""Ybmmd""  .JMML.     
                                                                                                     
                                                                                                    


";

Console.WriteLine(logo);


//single user singletones
builder.Services.AddSingleton<Model>();
builder.Services.AddSingleton<msgBuffer>();
builder.Services.AddSingleton<AKeyHandler>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

