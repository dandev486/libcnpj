.PHONY: build
build:
	dotnet build

.PHONY: test
test:
	dotnet test

.PHONY: clean
clean:
	find . -name 'bin' -or -name 'obj' | xargs rm -rf

.PHONY: benchmark
benchmark:
	dotnet run --configuration Release --project src/Benchmark

.PHONY: license
license:
	find . -path './*/*' -not -path '*/.*' -not -path '*/doc*' -not -path '*/bin*' -not -path '*/obj*' -type f \
		| xargs grep --files-without-match 'SPDX-License-Identifier'
