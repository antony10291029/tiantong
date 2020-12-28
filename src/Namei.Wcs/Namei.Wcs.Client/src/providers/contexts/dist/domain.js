"use strict";
exports.__esModule = true;
var _axios_1 = require("./_axios");
var url = 'http://' + window.location.hostname + ":5100";
var domain = _axios_1["default"].create(url);
exports["default"] = domain;
