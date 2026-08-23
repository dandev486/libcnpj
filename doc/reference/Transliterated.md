# CnpjValidator (Transliterated)

A simple transliteration of the [reference implementation][1] from Java to C#, while keeping it as close to the original
implementation as possible and that was subject to the same testing as the released one.

```csharp
using System.Text.RegularExpressions;

namespace LibCnpj;

public class CnpjValidator
{
    private readonly int _lengthWithoutDV = 12;
    private readonly Regex _separators = new("[./-]");
    private readonly Regex _format = new("[A-Z\\d]{12}");
    private readonly Regex _formatWithDV = new("[A-Z\\d]{12}[\\d]{2}");
    private readonly Regex _zeroed = new("^[0]+$");
    private readonly int _baseValue = '0';
    private readonly int[] _weights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    public bool IsValid(string value)
    {
        if (value != null)
        {
            value = RemoveSeparators(value);
            if (IsValidWithDV(value))
            {
                string informedDV = value[_lengthWithoutDV..];
                string calculatedDV = CalculateDV(value[.._lengthWithoutDV]);
                return calculatedDV == informedDV;
            }
        }

        return false;
    }

    private string CalculateDV(string baseValue)
    {
        if (baseValue != null)
        {
            baseValue = RemoveSeparators(baseValue);
            if (IsValidWithoutDV(baseValue))
            {
                string dv1 = string.Format("{0}", CalculateDigit(baseValue));
                string dv2 = string.Format("{0}", CalculateDigit(string.Concat(baseValue, dv1)));
                return string.Concat(dv1, dv2);
            }
        }

        throw new ArgumentException(string.Format("Cnpj {0} não é válido para o cálculo do DV", baseValue));
    }

    private int CalculateDigit(string value)
    {
        int sum = 0;

        for (int i = value.Length - 1; i >= 0; i--)
        {
            int characterValue = value.ElementAt(i) - _baseValue;
            sum += characterValue * _weights[_weights.Length - value.Length + i];
        }

        return sum % 11 < 2 ? 0 : 11 - (sum % 11);
    }

    private string RemoveSeparators(string value) =>
        _separators.Replace(value.Trim(), "");

    private bool IsValidWithoutDV(string value) =>
        _format.IsMatch(value) && !_zeroed.IsMatch(value);

    private bool IsValidWithDV(string value) =>
        _formatWithDV.IsMatch(value) && !_zeroed.IsMatch(value);
}
```

[1]: https://www.gov.br/receitafederal/pt-br/centrais-de-conteudo/publicacoes/documentos-tecnicos/cnpj
