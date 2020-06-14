#!/usr/bin/env sh

set -e

yarn run build

cd ./dist

set +e

coscmd -b ierp-client-1258274033 -r ap-shanghai delete -rf /

set -e

coscmd -b ierp-client-1258274033 -r ap-shanghai upload -r ./ /
