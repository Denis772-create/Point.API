namespace Point.Domain.Entities.CardAggregate;

public class QrCode : ValueObject
{
	public QrCode(string code)
	{
		Code = code;
	}

	public string Code { get; set; }

	protected override IEnumerable<object> GetAtomicValues()
	{
		yield return Code;
	}
}