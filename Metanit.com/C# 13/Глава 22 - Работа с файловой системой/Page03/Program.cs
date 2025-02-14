#region Получение информации о файле

string path = @"C:/test.txt";
// string path = "/Users/eugene/Documents/content.txt";  // для MacOS/Linux

FileInfo fileInfo = new(path);

if (fileInfo.Exists)
{
    Console.WriteLine($"Имя файла: {fileInfo.Name}");
    Console.WriteLine($"Время создания: {fileInfo.CreationTime}");
    Console.WriteLine($"Размер: {fileInfo.Length}");
    Console.WriteLine($"Размер: {fileInfo.DirectoryName}");
}

#endregion

#region Удаление файла

//string path = @"C:\test.txt";

//FileInfo fileInf = new(path);

//if (fileInf.Exists)
//{
//    fileInf.Delete();

//    // альтернатива с помощью класса File
//    // File.Delete(path);
//}

#endregion

#region Перемещение файла

//string path = @"C:\test.txt";
//string newPath = @"C:\Test\test.txt";

//FileInfo fileInf = new(path);

//if (fileInf.Exists)
//{
//    fileInf.MoveTo(newPath);

//    // альтернатива с помощью класса File
//    // File.Move(path, newPath);
//}

#endregion

#region Перемещение файла с перезаписью

//string path = @"C:\OldDir\content.txt";
//string newPath = @"C:\NewDir\index.txt";

//FileInfo fileInf = new(path);

//if (fileInf.Exists)
//{
//    fileInf.MoveTo(newPath, true);

//    // альтернатива с помощью класса File
//    // File.Move(path, newPath, true);
//}

#endregion