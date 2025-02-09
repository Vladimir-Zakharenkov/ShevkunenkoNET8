#region Example01 - Запись файла

//string path = "note1.txt";
//string text = "Hello World\nHello METANIT.COM";

//// полная перезапись файла 
//using (StreamWriter writer = new(path, false))
//{
//    await writer.WriteLineAsync(text);
//}

//// добавление в файл
//using (StreamWriter writer = new(path, true))
//{
//    await writer.WriteLineAsync("Addition");
//    await writer.WriteAsync("4,5");
//}

#endregion

#region Example02 - считывание файла

//string path = "note1.txt";

//// асинхронное чтение
//using StreamReader reader = new(path);

//string text = await reader.ReadToEndAsync();

//Console.WriteLine(text);

#endregion

#region Example03 - считывание файла построчно

string path = "F:\\ShevkunenkoNET8\\ShevkunenkoSite\\wwwroot\\texts\\serial-tyajelyi-pesok-interview.txt";

// асинхронное чтение
using StreamReader reader = new(path);

string? line;

while ((line = await reader.ReadLineAsync()) != null)
{
    Console.WriteLine(line);
}

#endregion