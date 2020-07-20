#!/usr/bin/env sh

set -e

cd ../Tiantong.Iot.Client

yarn build

cd ../Tiantong.Iot.Winform

rm -r ./release || true

dotnet publish -o ./release -r win10-x64 --self-contained false
