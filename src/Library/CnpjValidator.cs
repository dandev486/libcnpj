/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj;

public interface ICnpjValidator
{
    CnpjValidationResult Validate(string value);
    CnpjValidationResult Validate(ReadOnlySpan<char> value);
    bool IsValid(string value);
    bool IsValid(ReadOnlySpan<char> value);

}

public class CnpjValidator : ICnpjValidator
{
    private readonly int _length;
    private readonly CnpjCharacter[] _characters;

    public CnpjValidator(CnpjValidationFormat format, bool withSeparators = true)
    {
        Func<char, bool> isValue = format switch
        {
            CnpjValidationFormat.Alphanumeric => IsAlphanumeric,
            CnpjValidationFormat.AlphanumericRelaxed => IsAlphanumericRelaxed,
            CnpjValidationFormat.Numeric => IsNumeric,
            _ => throw new NotImplementedException(),
        };

        if (withSeparators)
        {
            _length = 18;
            _characters = [
                new(5, 6, isValue), new(4, 5, isValue), new(0, 0, IsDot),
                new(3, 4, isValue), new(2, 3, isValue), new(9, 2, isValue), new(0, 0, IsDot),
                new(8, 9, isValue), new(7, 8, isValue), new(6, 7, isValue), new(0, 0, IsSlash),
                new(5, 6, isValue), new(4, 5, isValue), new(3, 4, isValue), new(2, 3, isValue), new(0, 0, IsDash),
                new(0, 2, isValue), new(0, 0, isValue) ];
        }
        else
        {
            _length = 14;
            _characters = [
                new(5, 6, isValue), new(4, 5, isValue),
                new(3, 4, isValue), new(2, 3, isValue), new(9, 2, isValue),
                new(8, 9, isValue), new(7, 8, isValue), new(6, 7, isValue),
                new(5, 6, isValue), new(4, 5, isValue), new(3, 4, isValue), new(2, 3, isValue),
                new(0, 2, isValue), new(0, 0, isValue) ];
        }
    }

    public CnpjValidationResult Validate(string value) =>
        Validate(value.AsSpan());

    public CnpjValidationResult Validate(ReadOnlySpan<char> value)
    {
        var result = new CnpjValidationResult();
        IsValidImpl(value, error => result.Errors.Add(error));

        return result;
    }

    public bool IsValid(string value) =>
        IsValidImpl(value.AsSpan());

    public bool IsValid(ReadOnlySpan<char> value) =>
        IsValidImpl(value);

    private bool IsValidImpl(ReadOnlySpan<char> value, Action<string>? onErrorFn = null)
    {
        if (value.Length != _length)
        {
            onErrorFn?.Invoke($"'{value}' length is invalid!");
            return false;
        }

        int dv1 = 0;
        int dv2 = 0;
        for (int i = 0; i < _characters.Length; i++)
        {
            var c = value[i];
            if (!_characters[i].IsValid(c))
            {
                onErrorFn?.Invoke($"'{value}' character '{c}' at position '{i}' is invalid!");
                return false;
            }

            var v = c - Base(c);
            dv1 += v * _characters[i].W1;
            dv2 += v * _characters[i].W2;
        }

        dv1 = 11 - (dv1 % 11);
        if (dv1 > 9) dv1 = 0;
        var xdv1 = value[_length - 2];
        if (xdv1 - Base(xdv1) != dv1)
        {
            onErrorFn?.Invoke($"'{value}' first verifier digit '{xdv1}' is invalid!");
            return false;
        }

        dv2 = 11 - (dv2 % 11);
        if (dv2 > 9) dv2 = 0;
        var xdv2 = value[_length - 1];
        if (xdv2 - Base(xdv2) != dv2)
        {
            onErrorFn?.Invoke($"'{value}' second verifier digit '{xdv2}' is invalid!");
            return false;
        }

        return true;


        static int Base(char c) =>
            c >= 'a' && c <= 'z'
                ? 80  // '0' + 'a' - 'A', relaxed mode.
                : 48; // '0', strict mode.
    }

    private static bool IsAlphanumeric(char c) =>
        (c >= 'A' && c <= 'Z') ||
        (c >= '0' && c <= '9');

    private static bool IsAlphanumericRelaxed(char c) =>
        (c >= 'A' && c <= 'Z') ||
        (c >= 'a' && c <= 'z') ||
        (c >= '0' && c <= '9');

    private static bool IsNumeric(char c) =>
        c >= '0' && c <= '9';

    private static bool IsDash(char c) =>
        c == '-';

    private static bool IsDot(char c) =>
        c == '.';

    private static bool IsSlash(char c) =>
        c == '/';
}

internal record struct CnpjCharacter(int W1, int W2, Func<char, bool> IsValid);
