using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types

namespace ConsoleApplication1
{
    class Program
    {

        public static void Exercise_0()
        {
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            Console.WriteLine("Created successfully the container if it was not there yet");
        }

        public static String Exercise_1()
        {
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            Console.WriteLine("Created successfully the container if it was not there yet");

            // Retrieve reference to a blob named "myblob".
            String timestamp = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            String blockName = "larf_" + timestamp + ".txt" ;
            Console.WriteLine("We will generate the following file " + blockName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blockName);

            using (var fileStream = System.IO.File.OpenRead(@"C:\test_files\Awesome_local_file.txt"))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            return blockName;
        }

        public static void Exercise_2()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                }
            }
        }

        public static void Exercise_3()
        {
            String blockName = Exercise_1();
            String blockOutputName = "o" + blockName;
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blockName);
            CloudBlockBlob targetBlob = container.GetBlockBlobReference(blockOutputName);
            targetBlob.StartCopy(blockBlob);

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"C:\test_files\AwesomeOutput.txt"))
            {
                targetBlob.DownloadToStream(fileStream);
            }
        }

        public static void Exercise_4()
        {
            String blockName = Exercise_1();
            String blockOutputName = "o" + blockName;
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blockName);
            CloudBlockBlob targetBlob = container.GetBlockBlobReference(blockOutputName);
            targetBlob.StartCopy(blockBlob);

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"C:\test_files\AwesomeOutput.txt"))
            {
                targetBlob.DownloadToStream(fileStream);
            }
            targetBlob.Delete();
            blockBlob.Delete();
        }


        public static void Exercise_5()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1} will be deleted ", blob.Properties.Length, blob.Uri);
                    blob.Delete();

                }
            }
            container.Delete();
        }



        static void Main(string[] args)
        {
            Console.WriteLine("Starting exercise.");
            Exercise_0();
            Console.WriteLine("Ending exercise.");
            Console.ReadKey();
      
        }
    }
}
