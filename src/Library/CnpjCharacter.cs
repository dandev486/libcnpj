/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj;

internal static class CnpjCharacter
{
    internal static int Base(char c) =>
        c >= 'a' && c <= 'z'
            ? 80  // '0' + 'a' - 'A', relaxed mode.
            : 48; // '0', strict mode.

    internal static char ToDv(int value)
    {
        var dv = 11 - (value % 11);
        return dv <= 9
            ? (char)(dv + '0')
            : '0';
    }

    internal static char Dash() => '-';

    internal static bool IsDash(char c) =>
        c == Dash();

    internal static char Dot() => '.';

    internal static bool IsDot(char c) =>
        c == Dot();

    internal static char Slash() => '/';

    internal static bool IsSlash(char c) =>
        c == Slash();
}

internal record struct CnpjGeneratorCharacter(int WeightDv1, int WeightDv2, Func<char> Value);

internal record struct CnpjValidatorCharacter(int WeightDv1, int WeightDv2, Func<char, bool> IsValid);
