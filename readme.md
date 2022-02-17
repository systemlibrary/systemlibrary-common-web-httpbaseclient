# SystemLibrary Common Web HttpBaseClient

## Requirements
- &gt;= .NET 5
- System.Text.Json
- System.Web.Http
- SystemLibrary.Common.Net.Json &gt;= 2.0.0.8

## Latest Version
- Updated dependencies
- Updated Framework repository to .NET 5

## Description
SystemLibrary.Common.Web.HttpBaseClient for any .NET &gt;= 5 application - one way of calling external services/API's

Selling points:
* Cache's the underlying TCP Socket Connection for 5 minutes
  * No more socket exhaustion, nor need of 'iisreset' due to DNS changes or network bugs
* Configure a 'Retry Request' if a DNS change occurs
  * A new TCP Socket Connection will be used if the Cached connection fails due to DNS change
* Configure a 'Timeout' per request or per client, or a mix


## Docs			
Documentation with samples:
https://systemlibrary.github.io/systemlibrary-common-web-httpbaseclient/

## Nuget
https://www.nuget.org/packages/SystemLibrary.Common.Web.HttpBaseClient/

## Suggestions and feedback
support@systemlibrary.com

## Lisence
- It's free forever, copy paste as you'd like
