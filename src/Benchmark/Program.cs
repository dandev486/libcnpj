/*
LibCnpj, a simple library for generating and validating Brazil's CNPJ.
Copyright (C) 2026 Daniel Augusto
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)
*/
using BenchmarkDotNet.Running;
using LibCnpj.Benchmark;

BenchmarkRunner.Run<CnpjGeneratorBenchmark>();
BenchmarkRunner.Run<CnpjValidatorBenchmark>();
