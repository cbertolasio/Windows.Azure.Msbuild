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
    public class CleanupAzureStorageContainersTest
    {
        [TestCase("http://test.com", "accountName", "accountKey")]
        [TestCase("http://test123test.com", "accountName2", "accountKe3")]
        public void Execute_CreatesCloudStorageClient(string uri, string accountName, string accountKey)
        {
            storageFactory.ClearBehavior();

            storageFactory.Expect(it => it.Create(new Uri(uri), accountName, accountKey, task.StorageClientTimeoutInMinutes, task.ParallelOptionsThreadCount)).Return(blobClient);
                        
            task.Endpoint = uri;
            task.StorageAccountName = accountName;
            task.StorageAccountKey = accountKey;
            task.Execute();

            storageFactory.VerifyAllExpectations();
        }

        [TestCase()]
        public void Task_Deletes_TargetContainer()
        {
            storageFactory.ClearBehavior();

            storageFactory.Expect(it => it.Create(new Uri(uri), accountName, accountKey, task.StorageClientTimeoutInMinutes, task.ParallelOptionsThreadCount)).Return(blobClient);

            task.Execute();

            blobClient.VerifyAllExpectations();
            Assert.AreEqual(2, blobContainers.OfType<MockIAzureBlobContainer>().Where(c => !c.DeleteCalled).Count());
            Assert.AreEqual(18, blobContainers.OfType<MockIAzureBlobContainer>().Where(c => c.DeleteCalled).Count());
        }

        public class MockIAzureBlobContainer : IAzureBlobContainer
        {
            public bool DeleteCalled { get; set; }

            public DateTimeOffset? LastModified { get; set; }

            public IAzureBlob GetBlobReference(string fileName)
            {
                throw new NotImplementedException();
            }

            public IAzureBlob GetBlockBlobReference(string fileName)
            {
                throw new NotImplementedException();
            }

            public bool CreateIfNotExists()
            {
                throw new NotImplementedException();
            }

            public void Delete()
            {
                DeleteCalled = true;
            }
        }
        [SetUp]
        public void Setup()
        {
            kernel = new RhinoMocksMockingKernel();
            var tempBlobContainers = new List<IAzureBlobContainer>();
            for (int i = 0; i < 20; i++)
            {
                var blobContainer = new MockIAzureBlobContainer()
                {
                    LastModified = DateTimeOffset.Now.Date.AddHours(i),
                };

                tempBlobContainers.Add(blobContainer);
            }

            blobContainers = tempBlobContainers;

            logger = kernel.Get<ITaskLogger>();
            storageFactory = kernel.Get<IAzureBlobClientFactory>();
            blobClient = kernel.Get<IAzureBlobClient>();
            blob = kernel.Get<IAzureBlob>();

            blob.Stub(it => it.DeleteIfExists()).Return(true).Repeat.AtLeastOnce();
            storageFactory.Stub(it => it.Create(Arg<Uri>.Is.NotNull, Arg<string>.Is.NotNull, Arg<string>.Is.NotNull, Arg<int>.Is.Anything, Arg<int>.Is.Anything)).Return(blobClient);
            blobClient.Stub(it => it.GetAllContainerReferences()).Return(tempBlobContainers);

            task = kernel.Get<CleanupAzureStorageContainers>();

            accountName = "accountName";
            accountKey = "accountKey";

            uri = "http://testuri.com";
            task.Endpoint = uri;
            task.StorageAccountName = accountName;
            task.StorageAccountKey = accountKey;
            task.RemainingContainers = 2;
        }

        private string accountKey;
        private string accountName;
        private IAzureBlob blob;
        private IAzureBlobClient blobClient;
        private IEnumerable<IAzureBlobContainer> blobContainers;
        private ITaskLogger logger;
        private CleanupAzureStorageContainers task;
        private IAzureBlobClientFactory storageFactory;
        private string uri;
        private RhinoMocksMockingKernel kernel;
    }
}
