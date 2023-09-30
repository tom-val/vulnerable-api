using System.Text;
using SinKien.IBAN4Net;

namespace VulnerableAPI;

public static class IbanGenerator
{
    public static string GenerateIban()
    {
        var iban = new IbanBuilder()
            .CountryCode(CountryCode.GetCountryCode( "LT" ))
            .BankCode("654")
            .AccountNumber(GenerateRandomAccountNumber())
            .Build();

        return iban.ToString();
    }

    private static string GenerateRandomAccountNumber()
    {
        var number = new StringBuilder();
        var random = new Random();
        number.Append(random.Next(100, 999));
        return number.ToString();
    }

}
