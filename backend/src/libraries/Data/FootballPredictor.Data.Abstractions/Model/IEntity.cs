namespace FootballPredictor.Data.Abstractions.Model
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
