# SecondFactorAuthenticator

Module that executes a second factor authentication on ADFS.
This module, at the moment, is configured to check user name and its client hostname and give access only from clients registered for the specificed user.

The association of user/client name is done on a database configured in the Resource file.

###Usage Sample:
- Compile the solution wihin Visual Studio.
  The compilation will produce a set of DLLs that need to be installed on AD FS server. These DLLs are:
  . ``SecondFactorAuthenticator.dll``, the main DLL for the authentication module
  . ``it-it\SecondFactorAuthenticator.resources.dll``, a DLL for each language for which a resource file is provided (currently this module only supports English and Italian)
  . ``Newtonsoft.Json.dll``, the DLL used to manage JSON files (used as a database backend for user/host association)
  
- Register DLL on the AD FS server.
  Once compiled all the files needs to be copied on the AD FS server and needs to be registered in the GAC.
  The script ``RegisterAuthMethod.ps1`` contains an example of these operations:
 
  ```powershell
  # Params
  $location       = "C:\SecondFactorAuthenticator"
  $dllName        = "SecondFactorAuthenticator"
  $dllVersion     = "1.0.0.1"
  $publicKeyToken = "48936ade7df38d9f"

  # Register/Unregister DLL in GAC
  Set-location $location
  [System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
  $publish = New-Object System.EnterpriseServices.Internal.Publish
  $publish.GacInstall($location + "\Newtonsoft.Json.dll")
  $publish.GacInstall($location + "\" + $dllName + ".dll")
  $publish.GacInstall($location + "\it-it\" + $dllName + ".resources.dll")
  ```
   
- Enable second factor module in AD FS.
  The module hve to be enabled in AD FS. The script ``RegisterAuthMethod.ps1`` contains an example of these operations:

  ```powershell
  # Params
  $location       = "C:\SecondFactorAuthenticator"
  $dllName        = "SecondFactorAuthenticator"
  $dllVersion     = "1.0.0.1"
  $publicKeyToken = "48936ade7df38d9f"

  $typeName = $dllName + ".AuthenticationAdapter, " + $dllName + ", Version=" + $dllVersion + ", Culture=neutral, PublicKeyToken=" + $publicKeyToken
  Register-AdfsAuthenticationProvider -TypeName $typeName -Name $dllName -Verbose
  ```

-  Configure authentication.
   The second factor authentication mechanism must now be configured into the AD FS server.
   On the server, in the ``AD FS Manager`` application:
   . Under ``Authentication Policies`` select ``Edit Global Multi-factor Authentication...`` and enable the added module as a MFA.
   . For the desired relying party enable the MFA module for the desired devices and locations.
   
- Restart the AD FS server.

  ```bat
  net stop adfssrv
  net start adfssrv
  ```
  
- Check database.
  The database backend provided in the default configuration uses a JSON file to describe user/host association.
  If you want to test the module, you have to update the file ``c:\SecondFactorAuthenticator\database.json``.
  
  The format of the file is as follows:
  
  ```json
  {
    "username": [
      "host1.fqdn.com", "host2.fqdn.com"
    ]
  }
  ```