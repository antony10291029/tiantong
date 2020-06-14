#!/usr/bin/env sh

set -e

rm -r /release || true

dotnet publish -o ./release -r win10-x64 --self-contained false
