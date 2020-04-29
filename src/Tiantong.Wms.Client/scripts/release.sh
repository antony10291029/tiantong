#!/usr/bin/env sh

set -e

yarn run build

qsctl rm qs://tiantong-wms/ -r

qsctl cp ./dist/ qs://tiantong-wms/ -r
