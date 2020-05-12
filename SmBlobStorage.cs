using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using SmWeb.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SmWeb.Services
{
    public class SmBlobStorage : ISmBlobStorage
    {
        private readonly IOptions<BlobStorageAcctOptions> _optsAccessor;

        private CloudBlobContainer _imagesContainer = null;
        private CloudStorageAccount _storageAcct = null;
        private CloudBlobClient _blobClient = null;

        public SmBlobStorage(IOptions<BlobStorageAcctOptions> optionsAccessor)
        {
            _optsAccessor = optionsAccessor;

            StorageCredentials storageCredentials = new StorageCredentials(_optsAccessor.Value.StorageAccountName,
                                                                           _optsAccessor.Value.StorageAccountKey);
            _storageAcct = new CloudStorageAccount(storageCredentials, useHttps: true);

            _blobClient = _storageAcct.CreateCloudBlobClient();
            _imagesContainer = _blobClient.GetContainerReference(_optsAccessor.Value.FullImagesName);
        }

        public async Task<bool> UploadBlob(string filename, Stream stream = null)
        {
            if (stream == null)
                throw new NullReferenceException("SmBlobStorage.UploadBlob : null byte stream");

            try
            {
                //// Set the permissions so the blobs are public. 
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await _imagesContainer.SetPermissionsAsync(permissions);

                // Get a reference to the blob address, then upload the file to the blob.
                CloudBlockBlob cloudBlockBlob = _imagesContainer.GetBlockBlobReference(filename);
                cloudBlockBlob.Properties.ContentType = "image/png";

                await cloudBlockBlob.UploadFromStreamAsync(stream);

                return true;
            }
            catch (StorageException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Download File From Blob
        /// </summary>
        /// <param name="fileName">For example: image.PNG</param>
        /// <param name="containerName">container name of blob</param>
        /// <param name="localFilePath">For example: @"C:\Test\BlobTest.PNG"</param>
        public async Task DownloadFileFromBlob(string fileName, 
                                               string localFilePath)
        {
            CloudBlob blob = _imagesContainer.GetBlobReference(fileName);
            using (var fileStream = System.IO.File.OpenWrite(localFilePath))
            {
                await blob.DownloadToStreamAsync(fileStream);
            }
        }

        public async Task DeleteBlob(string filename)
        {
            //_blobClient
            CloudBlockBlob cbb = _imagesContainer.GetBlockBlobReference(filename);
            await cbb.DeleteIfExistsAsync();
        }
    }
}
