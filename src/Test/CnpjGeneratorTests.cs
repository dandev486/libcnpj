/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
namespace LibCnpj.Test;

public class CnpjGeneratorTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Given_generated_alphanumeric_cnpj_should_be_valid(bool withSeparators)
    {
        var validator = new CnpjValidator(CnpjFormat.Alphanumeric, withSeparators);
        var generator = new CnpjGenerator(CnpjFormat.Alphanumeric, withSeparators);

        for (int i = 0; i < 1000000; i++)
        {
            var value = generator.Generate();
            var valid = validator.IsValid(value);
            Assert.True(valid);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Given_generated_alphanumeric_relaxed_cnpj_should_be_valid(bool withSeparators)
    {
        var validator = new CnpjValidator(CnpjFormat.AlphanumericRelaxed, withSeparators);
        var generator = new CnpjGenerator(CnpjFormat.AlphanumericRelaxed, withSeparators);

        for (int i = 0; i < 1000000; i++)
        {
            var value = generator.Generate();
            var valid = validator.IsValid(value);
            Assert.True(valid);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Given_generated_numeric_cnpj_should_be_valid(bool withSeparators)
    {
        var validator = new CnpjValidator(CnpjFormat.Numeric, withSeparators);
        var generator = new CnpjGenerator(CnpjFormat.Numeric, withSeparators);

        for (int i = 0; i < 1000000; i++)
        {
            var value = generator.Generate();
            var valid = validator.IsValid(value);
            Assert.True(valid);
        }
    }
}
