# SecondFactorAuthenticator

Module that executes a second factor authentication on ADFS.
This module, at the moment, is configured to check user name and its client hostname and give access only from clients registered for the specificed user.

The association of user/client name is done on a database configured in the Resource file.
