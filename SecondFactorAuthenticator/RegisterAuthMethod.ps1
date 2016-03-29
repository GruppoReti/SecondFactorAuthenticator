# Params
$location       = "C:\SecondFactorAuthenticator"
$dllName        = "SecondFactorAuthenticator"
$dllVersion     = "1.0.0.1"
$publicKeyToken = "48936ade7df38d9f"

# Register/Unregister DLL in GAC
Set-location $location
[System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
$publish = New-Object System.EnterpriseServices.Internal.Publish
$publish.GacInstall($location + "\" + $dllName + ".dll")
$publish.GacInstall($location + "\it-it\" + $dllName + ".resources.dll")
#$publish.GacRemove($location + "\" + $dllName + ".dll")
#$publish.GacRemove($location + "\it-it\" + $dllName + ".resources.dll")

# Register/Unregister AuthN mechanism in ADFS
$typeName = "SecondFactorAuthenticator.AuthenticationAdapter, SecondFactorAuthenticator, Version=" + $dllVersion + ", Culture=neutral, PublicKeyToken=" + $publicKeyToken
Register-AdfsAuthenticationProvider -TypeName $typeName -Name "SecondFactorAuthenticator" -Verbose
#Unregister-AdfsAuthenticationProvider -Name "SecondFactorAuthenticator" -Verbose

# Restart ADFS
#net stop adfssrv
#net start adfssrv