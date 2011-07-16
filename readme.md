## Overview 

This repo contains a custom MSBuild Task that will allow you to copy files of your choosing to from 
your build server's artifact directory into a Windows Azure Cloud Storage Container.

## What's Next?

Ok, so here is what you need to do...

* Download the code from this repo
* Compile it
* In your MSBuild script, make a reference to the 'CopyToAzureStorage' task (i.e. the task in this repo)

## What properties do I need to set?
* Endpoint - this is the URI to your azure cloud storage, in the sample file it is set to the devstorage URI
* StorageAccountName - this is the name of your azure storage account (in the sample it's set to devstoreaccount1)
* StorageAccountKey - this is the key that you get when you provision an Azure Storage Acccount (int the sample its set to the default development storage account key)
* ContainerName - this is the name of the Azure Storage Container that you want to send files to when the task is executed
* SourceFiles - this is the list of files that you want to copy into your cloud container
* DestinationFiles - this is a list that allows you to specify what the target files will be

### Note #

These values below are the default local / development storage values that are common to all out of the box Windows Azure SDK installations.

* Endpoint: \http://127.0.0.:10000/devstoreaccount1
* StorageAccountName: devstoreaccount1
* StorageAccountKey: Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==

When shipping files to the REAL Azure Storage account, the Endpoint, StorageAccountName, and StorageAccountKey should be taken from the Windows Azure Portal

* Endpoint - https://{YourStorageAccountName}.blob.core.windows.net/
* StorageAccountName - {YourStorageAccountName} - taken from the Azure Portal
* StorageAccountKey - {Your Access Key} - taken from the Azure Portal

## Noteworthy Files
* [CopyToAzureStorage.cs] (https://github.com/cbertolasio/Windows.Azure.Msbuild/blob/master/Windows.Azure.Msbuild/CopyToAzureStorage.cs) -> is the code for the custom task
* [CopyToAzureStorageTest.cs] (https://github.com/cbertolasio/Windows.Azure.Msbuild/blob/master/Windows.Azure.Msbuild.Test/CopyToAzureStorageTest.cs) -> contains the unit test for the msbuild tasks
* [AzureTools] (https://github.com/cbertolasio/Windows.Azure.Msbuild/tree/master/Windows.Azure.Msbuild/AzureTools) -> contains the abstractions for the Managed Azure Storage API
* [Build.msbuild] (https://github.com/cbertolasio/Windows.Azure.Msbuild/blob/master/Build/build.msbuild) -> is a sample msbuild file that can be run in your environment to test the custom task