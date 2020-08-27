# UnityGCTest
I'll document any GC problems I find, as well as common GC perceptions of errors

【summary】
Test-Environment : Unity2018.4.22f1
Test-Count : 100 
--------------
【Coroutines】

Coroutines : 6.3KB 

【String】

New - String+ : 5.1KB

New - StringBuilder - Append : 3.8KB

New - StringBuilder - ToString : 0.4KB  

New - string.format : 2.7KB  

New - string.Concat : 2.9KB  

【array】

New - array : 2.3KB 

【foreach】

foreach : 48B 

【Get】

Get - obj.name : 3.8KB

Get - obj.tag : 4.1KB
