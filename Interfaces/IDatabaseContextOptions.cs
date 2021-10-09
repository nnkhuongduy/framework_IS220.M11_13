namespace _99phantram.Interfaces
{
  public interface IDatabaseContextOptions
  {
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string Username { get; }
    string Password { get; }
  }
}
