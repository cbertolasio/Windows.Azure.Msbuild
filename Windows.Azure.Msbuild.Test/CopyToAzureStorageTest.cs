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
        public void Task_Creates_TargetDirectory(string containerName)
        {
            task.ContainerName = containerName;
            storageFactory.Stub(it => it.Create(new Uri(uri), accountName, accountKey)).Return(blobClient);
            blobClient.Expect(it => it.GetContainerReference(task.ContainerName)).Return(blobContainer);
            blobContainer.Expect(it => it.CreateIfNotExists()).Return(true);

            task.Execute();

            blobClient.VerifyAllExpectations();
            blobContainer.VerifyAllExpectations();
        }

        [SetUp]
        public void Setup()
        {
            kernel = new RhinoMocksMockingKernel();

            storageFactory = kernel.Get<ICloudBlobClientWrapper>();
            blobClient = kernel.Get<IBlobClient>();
            logger = kernel.Get<ITaskLogger>();
            blobContainer = kernel.Get<IBlobContainer>();

            task = kernel.Get<CopyToAzureStorage>();

            accountName = "accountName";
            accountKey = "accountKey";

            uri = "http://testuri.com";
            task.Endpoint = uri;
            task.StorageAccountName = accountName;
            task.StorageAccountKey = accountKey;

        }


        private string accountKey;
        private string accountName;
        private IBlobClient blobClient;
        private IBlobContainer blobContainer;
        private ITaskLogger logger;
        private CopyToAzureStorage task;
        private ICloudBlobClientWrapper storageFactory;
        private string uri;
        private RhinoMocksMockingKernel kernel;
    }
}
