#region Example01 �������� ������������ ����� IOptions<TOptions>

//using Chapter06_08;
//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder();
//builder.Configuration.AddJsonFile("person.json");

//// ������������� ������ Person �� ���������� �� ������������
//builder.Services.Configure<Person>(builder.Configuration);

//var app = builder.Build();

//app.Map("/", (IOptions<Person> options) =>
//{
//    Person person = options.Value;  // �������� ���������� ����� Options ������ Person

//    return person;
//});

//app.Run();

#endregion

#region �������� ������������ ����� IOptions<TOptions>

//using Chapter06_08;

//var builder = WebApplication.CreateBuilder();

//builder.Configuration.AddJsonFile("person.json");
//builder.Services.Configure<Person>(builder.Configuration);

//var app = builder.Build();

//app.UseMiddleware<PersonMiddleware>();

//app.Run();

#endregion

#region Example03 ��������������� �������� ������������

//using Chapter06_08;

//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder();

//builder.Configuration.AddJsonFile("person.json");
//builder.Services.Configure<Person>(builder.Configuration);

//builder.Services.Configure<Person>(opt =>
//{
//    opt.Age = 22;
//});

//var app = builder.Build();

//app.Map("/", (IOptions<Person> options) =>
//{
//    Person person = options.Value;  // �������� ���������� ����� Options ������ Person

//    return person;
//});

//app.Run();

#endregion

#region Example04 �������� ��������� ������ ������������

using Chapter06_08;

using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder();

builder.Configuration.AddJsonFile("person.json");
builder.Services.Configure<Person>(builder.Configuration);
builder.Services.Configure<Company>(builder.Configuration.GetSection("company"));

var app = builder.Build();

app.Map("/", (IOptions<Company> options) => options.Value);

app.Run();

#endregion