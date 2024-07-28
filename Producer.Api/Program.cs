using Confluent.Kafka;
using Producer.Api.Services.BookService;
using Producer.Api.Services.KafkaProducerService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();

builder.Services.AddSingleton(provider =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092",
        AllowAutoCreateTopics = true,
        Acks = Acks.All
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

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

await app.RunAsync();
