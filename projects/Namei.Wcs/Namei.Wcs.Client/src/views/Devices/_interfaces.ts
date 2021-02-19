export interface Obj<T>{
  [ key: string ]: T;
}

export interface Door {
  id: string;
  isError: boolean;
  isForceOpened: boolean;
  isOpened: boolean;
  enteringTasksCount: number;
  requestingTasks: string[];
  type: string;
}

export interface Lifter {
  id: number;
  isWorking: boolean;
  isAlerting: boolean;
  floors: [{
    isDoorOpened: boolean;
    isExported: boolean;
    isImportAllowed: boolean;
    isScanned: boolean;
  }];
}
