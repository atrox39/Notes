using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Runtime.CompilerServices;


namespace Notes.Services
{
    public class FileService
    {
        private readonly string _StorageAccount;
        private readonly string _key= new IConfiguration.GetValue<string>("");
        private readonly BlobContainerClient _fileContainer;
        private readonly IConfiguration configuration;

        public FileService() {

            _key = configuration.GetValue<string>("blob:keyBlobStorage");
            _StorageAccount = configuration.GetValue<string>("blob:StorageAccount");           

        }

       
        public static CredentialsAzure AccountSettings (IConfiguration configuration)
        {
            CredentialsAzure settings = new CredentialsAzure(AccountSto);
            return settings;
            
        }
    }

    

}
