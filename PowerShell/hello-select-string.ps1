'Hello', 'HELLO' | Select-String -Pattern 'HELLO' -CaseSensitive -SimpleMatch
<#
Текстовые строки Hello и HELLO отправляются вниз по конвейеру в командлет.
Select-String Select-String использует параметр Pattern для указания HELLO. 
Параметр CaseSensitive указывает, что регистр должен соответствовать только шаблону в верхнем регистре. 
SimpleMatch является необязательным параметром и указывает, что строка в шаблоне 
не интерпретируется как регулярное выражение. 
Select-String отображает HELLO в консоли PowerShell.
#>