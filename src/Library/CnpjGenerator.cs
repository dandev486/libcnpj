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
    // TODO: [CONSIDER] IF INJECTED AS A SINGLETON IT WILL INDEFINETELY BE PSEUDO-RANDOM
    private readonly Random _random = new(DateTime.UtcNow.Millisecond);
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
                new(5, 6, value), new(4, 5, value), new(0, 0, Dot),
                new(3, 4, value), new(2, 3, value), new(9, 2, value), new(0, 0, Dot),
                new(8, 9, value), new(7, 8, value), new(6, 7, value), new(0, 0, Slash),
                new(5, 6, value), new(4, 5, value), new(3, 4, value), new(2, 3, value), new(0, 0, Dash),
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
        // TODO: [CONSIDER] CODE REUSE BETWEEN GENERATOR AND VALIDATOR, BUT MAKE IT WORK FIRST
        // TODO: [RESEARCH] IS THERE ANY SPECIAL RULE FOR THE '/0001' PORTION BESIDES THAT IT ALSO ACCEPTS LETTERS?
        Span<char> value = stackalloc char[_length];

        int dv1 = 0;
        int dv2 = 0;
        int i = 0;
        int v = 0;
        char c;

        for (i = 0; i < _characters.Length - 2; i++)
        {
            c = _characters[i].Value();
            value[i] = c;

            v = c - Base(c);
            dv1 += v * _characters[i].W1;
            dv2 += v * _characters[i].W2;
        }

        dv1 = 11 - (dv1 % 11);
        if (dv1 > 9) dv1 = 0;

        c = (char)(dv1 + '0');
        value[i] = c;

        v = c - Base(c);
        dv2 += v * _characters[i].W2;

        dv2 = 11 - (dv2 % 11);
        if (dv2 > 9) dv2 = 0;
        value[++i] = (char)(dv2 + '0');

        return value.ToString();


        static int Base(char c) =>
            c >= 'a' && c <= 'z'
                ? 80  // '0' + 'a' - 'A', relaxed mode.
                : 48; // '0', strict mode.
    }

    // TODO: [CONSIDER] SINGLE RANDOM CALL, BUT MAKE IT WORK FIRST
    private char Alphanumeric() =>
        (_random.Next() % 2) switch
        {
            0 => (char)(_random.Next() % 26 + 'A'),
            1 => (char)(_random.Next() % 10 + '0'),
            _ => throw new NotImplementedException(),
        };

    // TODO: [CONSIDER] SINGLE RANDOM CALL, BUT MAKE IT WORK FIRST
    private char AlphanumericRelaxed() =>
        (_random.Next() % 3) switch
        {
            0 => (char)(_random.Next() % 26 + 'A'),
            1 => (char)(_random.Next() % 26 + 'a'),
            2 => (char)(_random.Next() % 10 + '0'),
            _ => throw new NotImplementedException(),
        };

    private char Numeric() =>
        (char)(_random.Next() % 10 + '0');

    private char Dash() => '-';

    private char Dot() => '.';

    private char Slash() => '/';
}

internal record struct CnpjGeneratorCharacter(int W1, int W2, Func<char> Value);
