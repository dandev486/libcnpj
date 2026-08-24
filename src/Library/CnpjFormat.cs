/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj;

/// <summary>
/// CnpjFormat.
/// </summary>
public enum CnpjFormat
{
    /// <summary>
    /// CNPJ is composed of characters ranging from 0-9 and A-Z.
    /// <para>
    /// This is the newer official validation rule.
    /// </para>
    /// </summary>
    Alphanumeric = 1,

    /// <summary>
    /// CNPJ is composed of characters ranging from 0-9, A-Z and a-z.
    /// <para>
    /// This is the newer non-official validation rule.
    /// </para>
    /// </summary>
    AlphanumericRelaxed = 2,

    /// <summary>
    /// CNPJ is composed of characters ranging from 0-9.
    /// <para>
    /// This is the older official validation rule.
    /// </para>
    /// </summary>
    Numeric = 3,
}
