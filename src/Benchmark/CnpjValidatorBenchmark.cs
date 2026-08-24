/*
LibCnpj, a simple library for validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace LibCnpj.Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Method)]
public class CnpjValidatorBenchmark
{
    private readonly CnpjValidator _alphanumericWithSeparator = new(CnpjValidationFormat.Alphanumeric, true);
    private readonly CnpjValidator _alphanumericRelaxedWithSeparator = new(CnpjValidationFormat.AlphanumericRelaxed, true);
    private readonly CnpjValidator _numericWithSeparator = new(CnpjValidationFormat.Numeric, true);
    private readonly CnpjValidator _alphanumericWithoutSeparator = new(CnpjValidationFormat.Alphanumeric, false);
    private readonly CnpjValidator _alphanumericRelaxedWithoutSeparator = new(CnpjValidationFormat.AlphanumericRelaxed, false);
    private readonly CnpjValidator _numericWithoutSeparator = new(CnpjValidationFormat.Numeric, false);
    private readonly TransliteratedCnpjValidator _transliteratedValidator = new();

    [Benchmark]
    [Arguments("28.5HW.5PL/0001-12")]
    [Arguments("40.416.464/0001-06")]
    public void AlphanumericWithSeparator(string value) =>
        _alphanumericWithSeparator.IsValid(value);

    [Benchmark]
    [Arguments("285HW5PL000112")]
    [Arguments("40416464000106")]
    public void AlphanumericWithoutSeparator(string value) =>
        _alphanumericWithoutSeparator.IsValid(value);

    [Benchmark]
    [Arguments("28.5HW.5PL/0001-12")]
    [Arguments("28.5hw.5pl/0001-12")]
    [Arguments("40.416.464/0001-06")]
    public void AlphanumericRelaxedWithSeparator(string value) =>
        _alphanumericRelaxedWithSeparator.IsValid(value);

    [Benchmark]
    [Arguments("285HW5PL000112")]
    [Arguments("285hw5pl000112")]
    [Arguments("40416464000106")]
    public void AlphanumericRelaxedWithoutSeparator(string value) =>
        _alphanumericRelaxedWithoutSeparator.IsValid(value);

    [Benchmark]
    [Arguments("40.416.464/0001-06")]
    public void NumericWithSeparator(string value) =>
        _numericWithSeparator.IsValid(value);

    [Benchmark]
    [Arguments("40416464000106")]
    public void NumericWithoutSeparator(string value) =>
        _numericWithoutSeparator.IsValid(value);

    [Benchmark]
    [Arguments("28.5HW.5PL/0001-12")]
    [Arguments("40.416.464/0001-06")]
    public void TransliteratedAlphanumericWithSeparator(string value) =>
        _transliteratedValidator.IsValid(value);

    [Benchmark]
    [Arguments("285HW5PL000112")]
    [Arguments("40416464000106")]
    public void TransliteratedAlphanumericWithoutSeparator(string value) =>
        _transliteratedValidator.IsValid(value);

    [Benchmark]
    [Arguments("40.416.464/0001-06")]
    public void TransliteratedNumericWithSeparator(string value) =>
        _transliteratedValidator.IsValid(value);

    [Benchmark]
    [Arguments("40416464000106")]
    public void TransliteratedNumericWithoutSeparator(string value) =>
        _transliteratedValidator.IsValid(value);
}
