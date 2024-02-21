namespace Notes.Data.Models
{
  public class NoteFile
  {
    public int Id { get; set; }
    public string PathFile { get; set; } = null!;
    public DateTime Date { get; set; }
  }
}
