#!/usr/bin/env sh

set -e

dotnet publish -o ./release -r win10-x64 --self-contained false
