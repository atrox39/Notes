using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Notes.Data.DTOs.Note;
using Notes.Data.Models;
using Notes.Services;
using System.ComponentModel;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Notes.Repository
{

    public interface IFileRepository
    {
        public Task<NoteFile> Create(IFormFile imageFile);
    }
    public class FileRepository(NotesContext db):IFileRepository
    {
        //BlobServiceCliente
        private readonly IConfiguration config;       

        //public async Task<BlobDto> 
        public async Task<NoteFile> Create (IFormFile imageFile)
        {
            try
            {
                //FileService
                var azureBlob = new FileService(config);
                var response = await azureBlob.UploadFile(imageFile);
                string pathFile = response.Blob.Uri??"";
                

                if(response?.Error ?? false)
                {
                    var result = await db.NoteFileModel.AddAsync(new () { Date = new DateTime(), PathFile = pathFile});
                    await db.SaveChangesAsync();
                    return result.Entity;
                    //return null;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


    }
}
