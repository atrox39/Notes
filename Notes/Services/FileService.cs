using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Notes.Data.DTOs.Note;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;


namespace Notes.Services
{
    public class FileService
    {
        private readonly string _StorageAccount;
        private readonly string _key;
        private readonly BlobContainerClient _containerInstance;
        public readonly IConfiguration configuration;
        //public readonly 

        public FileService(IConfiguration configuration)
        {
            this.configuration = configuration;
            _key = configuration["blob:KeyBlobStorage"] ?? "Default";
            _StorageAccount = configuration["blob:AccountStorage"] ?? "Default";
            var credential = new StorageSharedKeyCredential(_StorageAccount,_key);
            string blobUri = $"https://{_StorageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri),credential);
            _containerInstance = blobServiceClient.GetBlobContainerClient("stuff-files");

        }


        public async Task<BlobResponseDto> UploadFile(IFormFile imageFile)
        {
            //BlobResponseDto response = new BlobResponseDto();
            BlobClient client = _containerInstance.GetBlobClient("stuff-files");


            try{
                await using (Stream? data = imageFile.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }


                //response.Status = $"FiIe {imageFile.FileName} Uptoaded Successfully";
                //response.Error = false;
                //response.Blob.Uri = client.Uri.AbsoluteUri;
                //response.Blob.Name = client.Name;
                return new BlobResponseDto()
                {
                    Status = $"FiIe {imageFile.FileName} Uploaded Successfully",
                    Error = false,
                    Blob = new() { Uri=client.Uri.AbsoluteUri,Name=client.Name}                   
                };

            }catch (Exception ex)
            {
                return new BlobResponseDto()
                {
                    Status = $"FiIe {imageFile.FileName} Error {ex.Message}",
                    Error = true,
                    Blob = new() { Uri = client.Uri.AbsoluteUri, Name = client.Name }
                };
            }
            
        }






    }    

}
