enum PlcStateType {
  uint16 = "uint16",
  int32 = "int32",
  bool = "bool",
  string = "string",
}

enum PlcStateCommand {
  get = "get",
  set = "set",
}

export {
  PlcStateType,
  PlcStateCommand,
};
