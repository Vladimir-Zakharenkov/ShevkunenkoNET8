#region Example01 ������ ������������ � ������� C#

using Chapter06_07;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

builder.Configuration.AddJsonFile("person.json");

var tom = new Person();

app.Configuration.Bind(tom);    // ��������� ������������ � �������� tom

app.Run(async (context) => await context.Response.WriteAsync($"{tom.Name} - {tom.Age}"));

app.Run();

#endregion
