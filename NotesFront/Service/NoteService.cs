using Notes.Data.DTOs.Note;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;

namespace Notes.Front.Service
{
  public interface INoteService
  {
    public Task<List<NoteDto>> GetAll();
    public Task<NoteDto?> GetById(int id);
    public Task<bool> Delete(int id);
    public Task<bool> Create(NoteCreateDto createNoteDto);

    public Task<bool> Update(int id, NoteUpdateDto updateNoteDto);
  }

  public class NoteService(HttpClient http) : INoteService
  {
    private readonly string baseURL = "/api/notes";

    public async Task<List<NoteDto>> GetAll()
    {
      var result = await http.GetFromJsonAsync<List<NoteDto>>($"{baseURL}");
      if (result is not null)
      {
        return result;
      }
      return [];
    }

    public async Task<NoteDto?> GetById(int id)
    {
      var result = await http.GetFromJsonAsync<NoteDto>($"{baseURL}/{id}");
      if (result is not null)
      {
        return result;
      }
      return null;
    }

    public async Task<bool> Delete(int id)
    {
      var result = await http.DeleteAsync($"{baseURL}/{id}");
      if (result.IsSuccessStatusCode)
      {
        return true;
      }
      return false;
    }

    public async Task<bool> Create(NoteCreateDto createNoteDto)
    {
      var result = await http.PostAsJsonAsync($"{baseURL}", createNoteDto);
      if (result.StatusCode == HttpStatusCode.Created)
      {
        return true;
      }
      return false;
    }

    public async Task<bool> Update(int id, NoteUpdateDto updateNoteDto)
    {
      var result = await http.PutAsJsonAsync($"{baseURL}/{id}", updateNoteDto);
      if (result.IsSuccessStatusCode)
      {
        return true;
      }
      return false;
    }
  }
}
