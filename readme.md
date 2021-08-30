# SystemLibrary Common Web BaseHttpClient

## Requirements
- .NET 4.8
- System.Text.Json
- SystemLibrary.Common.Net.Json
- SystemLibrary.Common.Net

## Latest Version
- Added comments to each method with some sample code
- Updated /docs
- Updated dependencies to latest patches

## Description
- One base class to call external API's/services
- Contains a cache for the HttpClient which greatly reduces amount of TCP connections created
- Contains a built in timeout handler which can be configured per request type (GET, POST, PUT, ...) or once per Client
- Configure a Retry if a request fails one time due to either cancellation/DNS change/...
- See the docs for a real world sample

## Install
- Install through Nuget Package Manager in Visual Studio
- https://www.nuget.org/packages/SystemLibrary.Common.Web.HttpBaseClient/

## Docs
https://systemlibrary.github.io/systemlibrary-common-web-httpbaseclient/

## Lisence
- It's free forever, copy paste as you'd like...