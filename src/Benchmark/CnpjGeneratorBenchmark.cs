/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace LibCnpj.Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
public class CnpjGeneratorBenchmark
{
    private readonly CnpjGenerator _alphanumericWithSeparator = new(CnpjFormat.Alphanumeric, true);
    private readonly CnpjGenerator _alphanumericRelaxedWithSeparator = new(CnpjFormat.AlphanumericRelaxed, true);
    private readonly CnpjGenerator _numericWithSeparator = new(CnpjFormat.Numeric, true);
    private readonly CnpjGenerator _alphanumericWithoutSeparator = new(CnpjFormat.Alphanumeric, false);
    private readonly CnpjGenerator _alphanumericRelaxedWithoutSeparator = new(CnpjFormat.AlphanumericRelaxed, false);
    private readonly CnpjGenerator _numericWithoutSeparator = new(CnpjFormat.Numeric, false);

    [Benchmark]
    public string AlphanumericWithSeparator() =>
        _alphanumericWithSeparator.Generate();

    [Benchmark]
    public string AlphanumericWithoutSeparator() =>
        _alphanumericWithoutSeparator.Generate();

    [Benchmark]
    public string AlphanumericRelaxedWithSeparator() =>
        _alphanumericRelaxedWithSeparator.Generate();

    [Benchmark]
    public string AlphanumericRelaxedWithoutSeparator() =>
        _alphanumericRelaxedWithoutSeparator.Generate();

    [Benchmark]
    public string NumericWithSeparator() =>
        _numericWithSeparator.Generate();

    [Benchmark]
    public string NumericWithoutSeparator() =>
        _numericWithoutSeparator.Generate();
}
