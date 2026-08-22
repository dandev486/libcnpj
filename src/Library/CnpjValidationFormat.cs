/*
LibCnpj, a simple library for validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj;

/// <summary>
/// CnpjValidationFormat.
/// </summary>
public enum CnpjValidationFormat
{
    /// <summary>
    /// CNPJ is composed of characters ranging from 0-9 and A-Z.
    /// </summary>
    Alphanumeric = 1,

    /// <summary>
    /// CNPJ is composed of characters ranging from 0-9, A-Z and a-z.
    ///
    /// <para>
    /// This is not the official validation rule and is provided as a convenience in order to avoid unnecessary string
    /// allocations with case conversions prior to knowing if a document is even valid (aka.: when using this mode, you
    /// should first validate and then only convert it later when valid).
    /// </para>
    /// </summary>
    AlphanumericRelaxed = 2,

    /// <summary>
    /// CNPJ is composed of characters ranging from 0-9.
    /// </summary>
    Numeric = 3,
}
