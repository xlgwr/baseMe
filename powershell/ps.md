# 快速始动当前目录下Exe
```
$fileList = Get-ChildItem "." -recurse *.exe | %{$_.FullName}
Foreach($file in $fileList){ & $file; write-host $file; sleep 2; }

```
# 关闭当前目录下已打开的exe
```
$fileList = Get-ChildItem "." -recurse *.exe | %{$_.BaseName}
Foreach($file in $fileList){ write-host $file; Get-Process -Name $file | foreach-object{$_.Kill()} }
pause
```
