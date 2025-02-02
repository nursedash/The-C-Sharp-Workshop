﻿using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace Chapter09.Service.Exercises.Exercise03
{
    public interface IFilesService
    {
        Task Delete(string name);
        Task Upload(string name, Stream content);
        Task<byte[]> Download(string filename);
        Uri GetDownloadLink(string filename);
    }

    public class FilesService : IFilesService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _defaultContainerClient;

        public FilesService()
        {
            var endpoint = "https://packtstorage2.blob.core.windows.net/";
            var account = "packtstorage2";
            var key = Environment.GetEnvironmentVariable("BlobStorageKey", EnvironmentVariableTarget.User);
            var storageEndpoint = new Uri(endpoint);
            var storageCredentials = new StorageSharedKeyCredential(account, key);
            _blobServiceClient = new BlobServiceClient(storageEndpoint, storageCredentials);
            _defaultContainerClient = CreateContainerIfNotExists("Exercise03").Result;
        }

        private async Task<BlobContainerClient> CreateContainerIfNotExists(string container)
        {
            var lowerCaseContainer = container.ToLower();
            var containerClient = _blobServiceClient.GetBlobContainerClient(lowerCaseContainer);
            if (!await containerClient.ExistsAsync())
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(lowerCaseContainer);
            }

            return containerClient;
        }

        public Task Delete(string name)
        {
            var blobClient = _defaultContainerClient.GetBlobClient(name);
            ValidateFileExists(blobClient);

            return blobClient.DeleteAsync();
        }

        public Task Upload(string name, Stream content)
        {
            var blobClient = _defaultContainerClient.GetBlobClient(name);
            return blobClient.UploadAsync(content);
        }

        public async Task<byte[]> Download(string filename)
        {
            var blobClient = _defaultContainerClient.GetBlobClient(filename);
            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);

            return stream.ToArray();
        }

        public Uri GetDownloadLink(string filename)
        {
            var blobClient = _defaultContainerClient.GetBlobClient(filename);
            ValidateFileExists(blobClient);
            var url = GetUri(blobClient);

            return url;
        }

        private Uri GetUri(BlobClient blobClient)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _defaultContainerClient.Name,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri;
        }

        private static void ValidateFileExists(BlobClient blobClient)
        {
            if (!blobClient.Exists())
            {
                throw new FileNotFoundException($"File {blobClient.Name} in default blob storage not found.");
            }
        }
    }
}
