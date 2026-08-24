/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj;

public interface ICnpjGenerator
{
    string Generate();
}

public class CnpjGenerator : ICnpjGenerator
{
    private static readonly Random _random = new(DateTime.UtcNow.Millisecond);
    private readonly int _length;
    private readonly CnpjGeneratorCharacter[] _characters;

    public CnpjGenerator(CnpjFormat format, bool withSeparators = true)
    {
        Func<char> value =  format switch
        {
            CnpjFormat.Alphanumeric => Alphanumeric,
            CnpjFormat.AlphanumericRelaxed => AlphanumericRelaxed,
            CnpjFormat.Numeric => Numeric,
            _ => throw new NotImplementedException(),
        };

        if (withSeparators)
        {
            _length = 18;
            _characters = [
                new(5, 6, value), new(4, 5, value), new(0, 0, CnpjCharacter.Dot),
                new(3, 4, value), new(2, 3, value), new(9, 2, value), new(0, 0, CnpjCharacter.Dot),
                new(8, 9, value), new(7, 8, value), new(6, 7, value), new(0, 0, CnpjCharacter.Slash),
                new(5, 6, value), new(4, 5, value), new(3, 4, value), new(2, 3, value), new(0, 0, CnpjCharacter.Dash),
                new(0, 2, value), new(0, 0, value) ];
        }
        else
        {
            _length = 14;
            _characters = [
                new(5, 6, value), new(4, 5, value),
                new(3, 4, value), new(2, 3, value), new(9, 2, value),
                new(8, 9, value), new(7, 8, value), new(6, 7, value),
                new(5, 6, value), new(4, 5, value), new(3, 4, value), new(2, 3, value),
                new(0, 2, value), new(0, 0, value) ];
        }
    }

    public string Generate()
    {
        Span<char> value = stackalloc char[_length];

        var sum1 = 0;
        var sum2 = 0;
        for (var i = 0; i < _characters.Length - 2; i++)
        {
            var c = _characters[i].Value();
            value[i] = c;

            var v = c - CnpjCharacter.Base(c);
            sum1 += v * _characters[i].WeightDv1;
            sum2 += v * _characters[i].WeightDv2;
        }

        var idv1 = _length - 2;
        var dv1 = CnpjCharacter.ToDv(sum1);
        value[idv1] = dv1;

        var vdv1 = dv1 - CnpjCharacter.Base(dv1);
        sum2 += vdv1 * _characters[idv1].WeightDv2;

        var idv2 = _length - 1;
        var dv2 = CnpjCharacter.ToDv(sum2);
        value[idv2] = dv2;

        return value.ToString();
    }

    private static char Alphanumeric() =>
        (_random.Next() % 2) switch
        {
            0 => (char)(_random.Next() % 26 + 'A'),
            1 => (char)(_random.Next() % 10 + '0'),
            _ => throw new NotImplementedException(),
        };

    private static char AlphanumericRelaxed() =>
        (_random.Next() % 3) switch
        {
            0 => (char)(_random.Next() % 26 + 'A'),
            1 => (char)(_random.Next() % 26 + 'a'),
            2 => (char)(_random.Next() % 10 + '0'),
            _ => throw new NotImplementedException(),
        };

    private static char Numeric() =>
        (char)(_random.Next() % 10 + '0');
}
