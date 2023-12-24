using SQLite4Unity3d;

public class Obj
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }


    public override string ToString()
    {
        return string.Format($"[Obj: Id={Id}, Login={Login}, Password={Password}]");
    }
}
