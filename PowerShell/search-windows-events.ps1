<#
 В этом примере выполняется поиск строки в журнале событий Windows. Переменная $_ представляет текущий объект в конвейере.
#>
$Events = Get-WinEvent -LogName Application -MaxEvents 50
$Events | Select-String -InputObject {$_.Message} -Pattern 'Failed'
<#
Командлет Get-WinEvent использует параметр LogName для указания журнала приложения. 
Параметр MaxEvents получает 50 самых последних событий из журнала. 
Содержимое журнала хранится в переменной с именем $Events.
Переменная $Events отправляется по конвейеру командлету Select-String. 
Select-String использует параметр InputObject . 
Переменная $_ представляет текущий объект и Message является свойством события. 
Параметр Pattern указывает строку failed и выполняет поиск совпадений в $_.Message. 
Select-String отображает выходные данные в консоли PowerShell.
#>