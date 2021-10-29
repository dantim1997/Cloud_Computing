using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Sas;
using DAL.Interface;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DAL.BlobService
{
    [ExcludeFromCodeCoverage]
    public class BlobStorage : IBlobStorage
    {
        private readonly ILogger<BlobStorage> _Logger;

        public BlobStorage(ILogger<BlobStorage> logger)
        {
            _Logger = logger;
        }

        // create the pdf
        public async Task<string> CreateFile(string file, string fileName)
        {
            var containerClient = await GetContainerClient();
            try
            {
                //select the file name you want to give the file
                var cblob = containerClient.GetBlockBlobReference(fileName);
                var bytes = Decode(file);
                using (var stream = new MemoryStream(bytes))
                {
                    //saves the file
                    await cblob.UploadFromStreamAsync(stream);
                }
            }
            catch (Exception e)
            {
                _Logger.LogInformation($"{DateTime.Now} Error with uploading to Blob-storage");
                throw;
            }

            _Logger.LogInformation($"{DateTime.Now} File uploaded to Blob-storage");

            return $"{fileName}";
        }

        // Get tge pdf download string
        public async Task<string> GetBlobFromServer(string fileName)
        {
            var containerClient = await GetContainerClient();
            var blob = containerClient.GetBlockBlobReference($"{fileName}");
            return blob.StorageUri.PrimaryUri.ToString();
        }

        // delete the pdf
        public async Task<bool> DeleteBlobFromServer(string fileName)
        {
            var containerClient = await GetContainerClient();
            var blob = containerClient.GetBlockBlobReference($"{fileName}");
            var deleted = await blob.DeleteIfExistsAsync();

            return deleted;
        }

        // init the container
        private async Task<CloudBlobContainer> GetContainerClient()
        {
            // Setup the connection to the storage account
            var connectionstring = Environment.GetEnvironmentVariable("BlobStorage");
            var storageAccount = CloudStorageAccount.Parse(connectionstring);
            var serviceClient = storageAccount.CreateCloudBlobClient();
            var container =
                serviceClient.GetContainerReference(
                    $"{Environment.GetEnvironmentVariable("FileContainer")}");
            await container.CreateIfNotExistsAsync();

            return container;
        }

        // from base64 to byte[]
        public static byte[] Decode(string input)
        {
            var output = input;

            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding

            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(input), "Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(output); // Standard base64 decoder

            return converted;
        }
    }
}