<#
В этом примере выполняется поиск определенной текстовой строки в каталоге и всех его подкаталогах.
#>
Get-ChildItem -Path C:\Windows\System32\*.txt -Recurse |
    Select-String -Pattern 'Microsoft' -CaseSensitive
<#
Get-ChildItem использует параметр Path для указания C:\Windows\System32*.txt. 
Параметр Recurse включает в себя поддиректории. 
Объекты отправляются по конвейеру в Select-String.
Select-String использует параметр Pattern и указывает строку Microsoft. 
Параметр CaseSensitive используется для точного сопоставления регистра строки. 
Select-String отображает выходные данные в консоли PowerShell.
#>