# Installation

# DEPRECATED/OBSOLETE
- All code from this repo is copy pasted into SystemLibrary.Common.Web version >= 2.0.1.1:
- https://github.com/systemlibrary/systemlibrary-common-web#latest-version


## Install nuget package

* Open your project/solution in Visual Studio
* Open Nuget Project Manager
* Search and install SystemLibrary.Common.Web.HttpBaseClient

## First time usage

- Classes and methods can be used out of the box by including the namespace they live in

- Sample:
```csharp  
	public string GetFrontPageOfSysLibAsString()
	{
		return new HttpBaseClient().Get<string>("https://www.systemlibrary.com/").Data;
	}
```

## Package Configurations
* Default (and modifiable) configurations in this package:

appSettings.json:
```json  
	{
		"systemLibraryCommonWebHttpBaseClient": {
			"retryRequestTimeoutSeconds": 12,
			"cacheDurationSeconds": 320
		}
	}
```  
