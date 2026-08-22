/*
LibCnpj, a simple library for validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj;

public record class CnpjValidationResult(List<string> Errors)
{
    public bool IsValid => Errors.Count != 0;

    public CnpjValidationResult() : this([])
    {
    }
};
