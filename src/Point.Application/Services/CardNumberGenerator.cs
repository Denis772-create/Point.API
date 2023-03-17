namespace Point.Application.Services;

public class CardNumberGenerator : ICardNumberGenerator
{
    public string GenerateCardNumber(Guid userId)
    {
	    var baseNumber = Math.Abs(userId.GetHashCode()).ToString();
	    var randomNumber = new Random()
		    .Next(0, 9999).ToString("D4");

	    var checkSum = CalculateCheckSum(baseNumber + randomNumber);

	    return baseNumber + randomNumber + checkSum;
    }

	private static int CalculateCheckSum(string number)
    {
	    var digits = number.ToCharArray()
		    .Select(c => int.Parse(c.ToString())).ToArray();

	    var sum = 0;
	    for (var i = 0; i < digits.Length; i++)
	    {
		    if (i % 2 == 0)
		    {
			    digits[i] *= 2;
			    if (digits[i] > 9)
			    {
				    digits[i] -= 9;
			    }
		    }
		    sum += digits[i];
	    }

	    var checkSum = (10 - sum % 10) % 10;

	    return checkSum;
    }
}