using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Rhino.Mocks;
using FluentAssertions.Assertions;
using NUnit.Framework;
using Ninject.MockingKernel.RhinoMock;
using Windows.Azure.Msbuild.AzureTools;
using Microsoft.Build.Framework;
using System.Collections;
using System.IO;
namespace Windows.Azure.Msbuild.Test
{
    [TestFixture]
    public class CopyToAzureStorageTest
    {
        [TestCase("http://test.com", "accountName", "accountKey")]
        [TestCase("http://test123test.com", "accountName2", "accountKe3")]
        public void Execute_CreatesCloudStorageClient(string uri, string accountName, string accountKey)
        {

            storageFactory.Expect(it => it.Create(new Uri(uri), accountName, accountKey)).Return(blobClient);
            blobClient.Stub(it => it.GetContainerReference(task.ContainerName)).Return(blobContainer);
            blobContainer.Stub(it => it.CreateIfNotExists()).Return(true);
                        
            task.Endpoint = uri;
            task.StorageAccountName = accountName;
            task.StorageAccountKey = accountKey;
            task.Execute();

            storageFactory.VerifyAllExpectations();
        }

        [TestCase("container1")]
        [TestCase("container2")]
        public void Task_Creates_TargetContainer(string containerName)
        {
            task.ContainerName = containerName;
            storageFactory.Stub(it => it.Create(new Uri(uri), accountName, accountKey)).Return(blobClient);
            blobClient.Expect(it => it.GetContainerReference(task.ContainerName)).Return(blobContainer);
            blobContainer.Expect(it => it.CreateIfNotExists()).Return(true);

            task.Execute();

            blobClient.VerifyAllExpectations();
            blobContainer.VerifyAllExpectations();
        }

        [TestCaseSource("GetBlobReferenceDataSource")]
        public void Task_Gets_BlobReference(ITaskItem[] sourceFiles, ITaskItem[] destinationFiles)
        {
            
            storageFactory.Stub(it => it.Create(Arg<Uri>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull)).Return(blobClient);
            blobClient.Stub(it => it.GetContainerReference(Arg<string>.Is.NotNull)).Return(blobContainer);
            blobContainer.Expect(it => it.GetBlobReference(Arg<string>.Is.NotNull)).Repeat.AtLeastOnce().Return(blob);    
                
            task.SourceFiles = sourceFiles;
            task.DestinationFiles = destinationFiles;

            // act
            task.Execute();

            // assert
            blobContainer.AssertWasCalled(it => it.GetBlobReference(Arg<string>.Is.NotNull), opt => opt.Repeat.Times(task.SourceFiles.Count()));
        }

        [TestCaseSource("GetBlobReferenceDataSource")]
        public void Task_Deletes_ExistingBlobs(ITaskItem[] sourceFiles, ITaskItem[] destinationFiles)
        {
            task.SourceFiles = sourceFiles;
            task.DestinationFiles = destinationFiles;

            blob.Expect(it => it.DeleteIfExists()).Return(true).Repeat.Times(task.DestinationFiles.Count());
            storageFactory.Stub(it => it.Create(Arg<Uri>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull)).Return(blobClient);
            blobClient.Stub(it => it.GetContainerReference(Arg<string>.Is.NotNull)).Return(blobContainer);
            blobContainer.Stub(it => it.GetBlobReference(Arg<string>.Is.NotNull)).Repeat.AtLeastOnce().Return(blob);    

            task.Execute();

            blob.AssertWasCalled(it => it.DeleteIfExists(), opt => opt.Repeat.Times(task.DestinationFiles.Count()));
        }

        [TestCaseSource("GetBlobReferenceDataSource")]
        public void Task_ReadsFile_FromStream(ITaskItem[] sourceFiles, ITaskItem[] destinationFiles)
        {
            task.SourceFiles = sourceFiles;
            task.DestinationFiles = destinationFiles;

            fileManager.BackToRecord();
            fileManager.Replay();
            fileManager.Expect(it => it.GetFile(Arg<string>.Is.NotNull)).Return(stream);
            //blob.Stub(it => it.DeleteIfExists()).Return(true).Repeat.AtLeastOnce();
            //storageFactory.Stub(it => it.Create(Arg<Uri>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull)).Return(blobClient);
            //blobClient.Stub(it => it.GetContainerReference(Arg<string>.Is.NotNull)).Return(blobContainer);
            //blobContainer.Stub(it => it.GetBlobReference(Arg<string>.Is.NotNull)).Repeat.AtLeastOnce().Return(blob);

            task.Execute();

            fileManager.VerifyAllExpectations();
        }


        [TestCaseSource("GetBlobReferenceDataSource")]
        public void Task_Uploads_Files(ITaskItem[] sourceFiles, ITaskItem[] destinationFiles)
        {
            task.SourceFiles = sourceFiles;
            task.DestinationFiles = destinationFiles;

            //blob.Stub(it => it.DeleteIfExists()).Return(true).Repeat.AtLeastOnce();
            //storageFactory.Stub(it => it.Create(Arg<Uri>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull)).Return(blobClient);
            //blobClient.Stub(it => it.GetContainerReference(Arg<string>.Is.NotNull)).Return(blobContainer);
            //blobContainer.Stub(it => it.GetBlobReference(Arg<string>.Is.NotNull)).Repeat.AtLeastOnce().Return(blob);
            //fileManager.Stub(it => it.GetFile(Arg<string>.Is.NotNull)).Return(stream);

            task.Execute();

            blob.AssertWasCalled(it => it.UploadFromStream(Arg<Stream>.Is.NotNull), opt => opt.Repeat.Times(task.DestinationFiles.Count()));
        }



        private static IEnumerable GetBlobReferenceDataSource()
        {
            var kernel = new RhinoMocksMockingKernel();
            var sourceItem1 = kernel.Get<ITaskItem>();
            sourceItem1.Stub(it => it.ItemSpec).Return("file1.txt");
            var sourceItem2 = kernel.Get<ITaskItem>();
            sourceItem2.Stub(it => it.ItemSpec).Return("file2.txt");
            var sourceFiles = new ITaskItem[] { sourceItem1, sourceItem2 };


            var destItem1 = kernel.Get<ITaskItem>();
            destItem1.Stub(it => it.ItemSpec).Return("prd\file1.txt");
            var destItem2 = kernel.Get<ITaskItem>();
            destItem2.Stub(it => it.ItemSpec).Return("prd\file2.txt");
            var destinationFiles = new ITaskItem[] { destItem1, destItem2 };

            yield return new TestCaseData(sourceFiles, destinationFiles);
        }

        [SetUp]
        public void Setup()
        {
            kernel = new RhinoMocksMockingKernel();
            blobContainer = kernel.Get<IBlobContainer>();
            logger = kernel.Get<ITaskLogger>();
            storageFactory = kernel.Get<ICloudBlobClientWrapper>();
            blobClient = kernel.Get<IBlobClient>();
            blob = kernel.Get<IBlob>();
            fileManager = kernel.Get<IFileManager>();

            stream = new MemoryStream(Encoding.ASCII.GetBytes("this is a test stream \r\n this is a test stream"));

            blob.Stub(it => it.DeleteIfExists()).Return(true).Repeat.AtLeastOnce();
            storageFactory.Stub(it => it.Create(Arg<Uri>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull)).Return(blobClient);
            blobClient.Stub(it => it.GetContainerReference(Arg<string>.Is.NotNull)).Return(blobContainer);
            blobContainer.Stub(it => it.GetBlobReference(Arg<string>.Is.NotNull)).Repeat.AtLeastOnce().Return(blob);
            fileManager.Stub(it => it.GetFile(Arg<string>.Is.NotNull)).Return(stream);

            task = kernel.Get<CopyToAzureStorage>();

            accountName = "accountName";
            accountKey = "accountKey";

            uri = "http://testuri.com";
            task.Endpoint = uri;
            task.StorageAccountName = accountName;
            task.StorageAccountKey = accountKey;
            task.ContainerName = "someContainer";
            task.SourceFiles = new ITaskItem[] { };
            task.DestinationFiles = new ITaskItem[] { };
        }


        private string accountKey;
        private string accountName;
        private IBlob blob;
        private IBlobClient blobClient;
        private IBlobContainer blobContainer;
        private IFileManager fileManager;
        private ITaskLogger logger;
        private MemoryStream stream;
        private CopyToAzureStorage task;
        private ICloudBlobClientWrapper storageFactory;
        private string uri;
        private RhinoMocksMockingKernel kernel;
    }
}
