# LibCnpj

LibCnpj, a simple library for validating Brazil's CNPJ. \
Copyright (C) 2026 Daniel Augusto \
SPDX-License-Identifier: LGPL-2.0-only or BSD-3-Clause (at your choice)

## Features

- **Flexible**, allows performing both `Alphanumeric` (newer rule, official) and `Relaxed Alphanumeric` (newer rule,
    non-official) document validations, where the latter is provided as a convenience in order to avoid unnecessary
    string allocations with case conversions prior to knowing if a document is even valid (aka.: when using this mode,
    you can first validate and then, only if valid, convert the input string to uppercase).
- **Retrocompatible**, allows performing `Numeric` document validations (older official rule).
- **Economy**, zero regex matching and zero string allocation during the validation process, just plain character
    comparisons and digit calculations based on their respective weights.

## Getting Started

Below a suggestion on how to use this library for a project that uses dependency injection.

1. **Configure and Register**, using your favourite validation rule (or most often, the proper business rule).

```csharp
// Example 1: When only alphanumeric validations are needed, registered once.
public static class MyServices
{
    private static void AddMyServices(IServiceCollection services)
    {
        services.AddSingleton<ICnpjValidator>(new CnpjValidator(CnpjValidationFormat.Alphanumeric));
    }
}

// Example 2: When both alphanumeric and numeric validations are needed, registered as different keyed services.
public static class MyServices
{
    private static void AddMyServices(IServiceCollection services)
    {
        services.AddKeyedSingleton<ICnpjValidator>(
            CnpjValidationFormat.Alphanumeric,
            new CnpjValidator(CnpjValidationFormat.Alphanumeric));

        services.AddKeyedSingleton<ICnpjValidator>(
            CnpjValidationFormat.Numeric,
            new CnpjValidator(CnpjValidationFormat.Numeric));
    }
}
```

2. **Use**, injecting it where appropriate.

```csharp
// Example 1: When only alphanumeric validations are needed, registered once.
public class MyClass(
    ICnpjValidator cnpjValidator)
{
    public void MyMethod(string document)
    {
        if (!cnpjValidator.IsValid(document))
        {
            // Wow! Very error, must handle.
        }

        // Wow! Much validated.
    }
}

// Example 2: When both alphanumeric and numeric validations are needed, registered as different keyed services.
public class MyClass(
    [FromKeyedServices(CnpjValidationFormat.Alphanumeric)] ICnpjValidator alphanumericCnpjValidator,
    [FromKeyedServices(CnpjValidationFormat.Numeric)] ICnpjValidator numericCnpjValidator))
{
    public void MyAlphanumericMethod(string document)
    {
        if (!alphanumericCnpjValidator.IsValid(document))
        {
            // Wow! Very error. Must handle.
        }

        // Wow! Much validated.
    }

    public void MyNumericMethod(string document)
    {
        if (!numericCnpjValidator.IsValid(document))
        {
            // Wow! Very error. Must handle.
        }

        // Wow! Much validated.
    }
}
```

## Q&A

Even though nobody asked (as of now), I strongly suggest that you don't even waste your time reading it.

**Q:** Why did you do it? \
**A:** I am working on another project and validating documents will be necessary, so why not.

**Q:** Why did you implement it the way you did? \
**A:** I [transliterated][1] the [reference implementation][2] from Java to C# and, on doing that, noticed that it doesn't
  allow validating documents with separators, which would result in an allocation for their removal for validation and
  then, noticed that it doesn't allow validating documents whose alphabetic characters are lowercase, which would result
  in an allocation for their conversion.

  I thought that it would be nice both if it was possible to ensure that the separators themselves are also valid and if
  one could avoid these allocations prior to even knowing that the document is valid since when working with systems
  that receive lots of information, this makes a difference. Then implemented in another way, measured both
  implementations and was satisfied enough with the results to publish it, maybe somehow it will be someday useful to
  someone, somewhere.

  So basically scratching a personal itch.

**Q:** Why did you compare character ranges directly instead of using functions from `System.Char`? \
**A:** While experimenting with the transliteration, I noticed that some functions that I would have to implement such
  as `IsDot` were considerably faster when invoked in comparison and decided to explore the [source code][2]. There are
  are additional calls involved reusing `IsBetween`, so I took a bet on skipping them, since in C# characters and
  strings are UTF-16, ASCII overlaps with it, and ASCII is all that is accepted in a CNPJ, checking by range seems
  reasonable instead of the more general purpose implementation provided there.

  So basically scratching another personal itch (found a bug? report it!).

[1]: https://github.com/dandev486/libcnpj/blob/master/doc/reference/Transliterated.md
[2]: https://www.gov.br/receitafederal/pt-br/centrais-de-conteudo/publicacoes/documentos-tecnicos/cnpj
[3]: https://github.com/dotnet/dotnet/blob/b0f34d51fccc69fd334253924abd8d6853fad7aa/src/runtime/src/libraries/System.Private.CoreLib/src/System/Char.cs#L301C52-L301C74
