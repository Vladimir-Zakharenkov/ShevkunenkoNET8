<# 
Эта команда выполняет поиск во всех файлах с расширением .txt в текущем каталоге. 
На выходе отображаются строки в тех файлах, которые содержат указанную строку.
#>

Get-Alias | Out-File -FilePath D:\TEMP-SYSTEM\Alias.txt
Get-Command | Out-File -FilePath D:\TEMP-SYSTEM\Command.txt
Select-String -Path D:\TEMP-SYSTEM\*.txt -Pattern 'Get-'

<#
В этом примере Get-Alias и Get-Command используются с Out-File командлетом 
для создания двух текстовых файлов в текущем каталоге, Alias.txt и Command.txt.
Select-String использует параметр Path с подстановочным знаком asterisk(*) 
для поиска всех файлов в текущем каталоге с расширением .txtимени файла . 
Параметр Pattern указывает текст, соответствующий Get-. Select-String отображает 
выходные данные в консоли PowerShell. Имя файла и номер строки предшествуют 
каждой строке содержимого, содержащей совпадение для параметра Pattern .
#>