using NotificationService.Interface;
using NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);
var emailSettings =builder.Configuration.GetSection("EmailNotification");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IEmailService> (provider =>
    new EmailService(emailSettings["Username"], emailSettings["Password"], emailSettings["server"], int.Parse(emailSettings["port"]), emailSettings["Name"]));

//builder.Services.AddSingleton<ISmsService, SmsService>();
//builder.Services.AddSingleton<IWhatsAppService, WhatsAppService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
