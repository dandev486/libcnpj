/*
LibCnpj, a simple library for validating Brazil's CNPJ.
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
    public CnpjGenerator(CnpjValidationFormat format, bool withSeparators = true)
    {
    }

    public string Generate()
    {
        throw new NotImplementedException();
    }
}
